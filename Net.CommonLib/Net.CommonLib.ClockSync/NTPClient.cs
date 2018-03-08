using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace Net.CommonLib.ClockSync
{
    public enum _LeapIndicator
    {
        NoWarning,
        LastMinute61,
        LastMinute59,
        Alarm
    }

    public enum _Mode
    {
        SymmetricActive,
        SymmetricPassive,
        Client,
        Server,
        Broadcast,
        Unknown
    }

    public enum _Stratum
    {
        Unspecified,
        PrimaryReference,
        SecondaryReference,
        Reserved
    }

    internal class UdpState
    {
        // Fields
        public IPEndPoint e;
        public UdpClient u;
    }

    /// <summary>
    /// NTP协议 client处理类
    /// </summary>
    public class NTPClient
    {
        // Fields
        private static byte[] cbReceiveBytes = null;
        public static bool messageReceived = false;
        private byte[] NTPData = new byte[0x30];
        private const byte NTPDataLength = 0x30;
        private const byte offOriginateTimestamp = 0x18;
        private const byte offReceiveTimestamp = 0x20;
        private const byte offReferenceID = 12;
        private const byte offReferenceTimestamp = 0x10;
        private const byte offTransmitTimestamp = 40;
        public DateTime ReceptionTimestamp;
        private string timeServerHost;
        private IList<byte> serverHostBytes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host"></param>
        public NTPClient(string host)
        {
            messageReceived = false;
            cbReceiveBytes = null;
            this.timeServerHost = host;
            this.serverHostBytes = new List<byte>();
            foreach (var c in this.timeServerHost.Split('.'))
            {
                this.serverHostBytes.Add(Convert.ToByte(c));
            }
        }

        /// <summary>
        /// 计算出时间
        /// </summary>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        private DateTime ComputeDate(ulong milliseconds)
        {
            TimeSpan span = TimeSpan.FromMilliseconds((double)milliseconds);
            DateTime time = new DateTime(0x76c, 1, 1);
            return (time + span);
        }

        /// <summary>
        /// 连接服务器端
        /// </summary>
        /// <param name="bUpdateSystemTime"></param>
        /// <param name="diffHours"></param>
        /// <returns></returns>
        public bool Connect(bool bUpdateSystemTime)
        {
            bool success = false;
            UdpClient timeSocket = null;

            try
            {
                //Log.Instance.LogNtpInfo(string.Format("开始与{0}时钟同步...", this.timeServerHost));

                timeSocket = new UdpClient();
                IPEndPoint ephost = new IPEndPoint(IPAddress.Parse(timeServerHost), 0x7b);
                timeSocket.Connect(ephost);

                //Log.Instance.LogNtpInfo("时钟源连接已建立.");

                this.Initialize();
                timeSocket.Send(this.NTPData, this.NTPData.Length);

                //Log.Instance.LogNtpInfo("数据已发送到时钟源.");

                this.NTPData = this.ReceiveMessage(timeSocket, ephost);

                if ((this.NTPData != null) && this.IsResponseValid())
                {
                    //Log.Instance.LogNtpInfo("接收到时钟数据.");
                    this.ReceptionTimestamp = DateTime.Now;
                    success = true;
                }

                if (success && (bUpdateSystemTime & (this.NTPData != null)))
                {
                    success = this.SetTime();
                }
                else
                {
                    //Log.Instance.LogNtpError("未接收到时钟源数据!");
                }
            }
            catch (InvalidOperationException ioe)
            {
                //Log.Instance.LogNtpError("时钟同步失败.", ioe);
            }
            catch (Exception ex)
            {
                //Log.Instance.LogNtpError("时钟同步失败.", ex);
            }
            finally
            {
                if (timeSocket != null)
                {
                    timeSocket.Close();
                }
            }

            return success;
        }

        /// <summary>
        /// 获得毫秒
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private ulong GetMilliSeconds(byte offset)
        {
            int i;
            ulong intpart = 0L;
            ulong fractpart = 0L;
            for (i = 0; i <= 3; i++)
            {
                intpart = (((ulong)0x100L) * intpart) + this.NTPData[offset + i];
            }
            for (i = 4; i <= 7; i++)
            {
                fractpart = (((ulong)0x100L) * fractpart) + this.NTPData[offset + i];
            }
            return ((intpart * ((ulong)0x3e8L)) + ((fractpart * ((ulong)0x3e8L)) / ((ulong)0x100000000L)));
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            if (this.NTPData == null)
            {
                // 接收不到数据后NTPData会被置为null，需要重新初始化
                this.NTPData = new byte[0x30];
            }
            this.NTPData[0] = 0x1b;
            for (int i = 1; i < 0x30; i++)
            {
                this.NTPData[i] = 0;
            }
            this.TransmitTimestamp = DateTime.Now;
        }

        /// <summary>
        /// 判断返回是否合法
        /// </summary>
        /// <returns></returns>
        public bool IsResponseValid()
        {
            if ((this.NTPData.Length < 0x30) || (this.Mode != _Mode.Server))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 接收消息处理函数
        /// </summary>
        /// <param name="ar"></param>
        public static void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient u = ((UdpState)ar.AsyncState).u;
            IPEndPoint e = ((UdpState)ar.AsyncState).e;
            try
            {
                cbReceiveBytes = u.EndReceive(ar, ref e);
                if (cbReceiveBytes != null)
                {
                    messageReceived = true;
                }
            }
            catch
            {
                cbReceiveBytes = null;
                messageReceived = false;
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="u"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private byte[] ReceiveMessage(UdpClient u, IPEndPoint e)
        {
            UdpState s = new UdpState();
            s.e = e;
            s.u = u;
            u.BeginReceive(new AsyncCallback(NTPClient.ReceiveCallback), s);
            int tickCount = Environment.TickCount;
            while (!messageReceived && ((Environment.TickCount - tickCount) < 0x7d0))
            {
                Thread.Sleep(100);
            }
            if (messageReceived)
            {
                return cbReceiveBytes;
            }
            return null;
        }

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="date"></param>
        private void SetDate(byte offset, DateTime date)
        {
            int i;
            ulong intpart = 0L;
            ulong fractpart = 0L;
            DateTime StartOfCentury = new DateTime(0x76c, 1, 1, 0, 0, 0);
            TimeSpan timespan = (TimeSpan)(date - StartOfCentury);
            ulong milliseconds = (ulong)timespan.TotalMilliseconds;
            intpart = milliseconds / ((ulong)0x3e8L);
            fractpart = ((milliseconds % ((ulong)0x3e8L)) * ((ulong)0x100000000L)) / ((ulong)0x3e8L);
            ulong temp = intpart;
            for (i = 3; i >= 0; i--)
            {
                this.NTPData[offset + i] = (byte)(temp % ((ulong)0x100L));
                temp /= (ulong)0x100L;
            }
            temp = fractpart;
            for (i = 7; i >= 4; i--)
            {
                this.NTPData[offset + i] = (byte)(temp % ((ulong)0x100L));
                temp /= (ulong)0x100L;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);
        private bool SetTime()
        {
            SYSTEMTIME st;
            DateTime trts = this.TransmitTimestamp;

            st.year = (short)trts.Year;
            st.month = (short)trts.Month;
            st.dayOfWeek = (short)trts.DayOfWeek;
            st.day = (short)trts.Day;
            st.hour = (short)trts.Hour;
            st.minute = (short)trts.Minute;
            st.second = (short)trts.Second;
            st.milliseconds = (short)trts.Millisecond;

            TimeSpan diff = trts.Subtract(this.ReceptionTimestamp);
            if (SetLocalTime(ref st))
            {
                //Log.Instance.LogNtpInfo(
                //    string.Format("时钟同步成功,时钟误差{0}秒.", Math.Round(diff.TotalSeconds, 5)));
                return true;
            }
            throw new InvalidOperationException(
                string.Format("设置本地时间出错，Win32错误码:{0}", Marshal.GetLastWin32Error().ToString()));
        }

        public override string ToString()
        {
            string str = "Leap Indicator: ";
            switch (this.LeapIndicator)
            {
                case _LeapIndicator.NoWarning:
                    str = str + "No warning";
                    break;

                case _LeapIndicator.LastMinute61:
                    str = str + "Last minute has 61 seconds";
                    break;

                case _LeapIndicator.LastMinute59:
                    str = str + "Last minute has 59 seconds";
                    break;

                case _LeapIndicator.Alarm:
                    str = str + "Alarm Condition (clock not synchronized)";
                    break;
            }
            str = (str + "\r\nVersion number: " + this.VersionNumber.ToString() + "\r\n") + "Mode: ";
            switch (this.Mode)
            {
                case _Mode.SymmetricActive:
                    str = str + "Symmetric Active";
                    break;

                case _Mode.SymmetricPassive:
                    str = str + "Symmetric Pasive";
                    break;

                case _Mode.Client:
                    str = str + "Client";
                    break;

                case _Mode.Server:
                    str = str + "Server";
                    break;

                case _Mode.Broadcast:
                    str = str + "Broadcast";
                    break;

                case _Mode.Unknown:
                    str = str + "Unknown";
                    break;
            }
            str = str + "\r\nStratum: ";
            switch (this.Stratum)
            {
                case _Stratum.Unspecified:
                case _Stratum.Reserved:
                    str = str + "Unspecified";
                    break;

                case _Stratum.PrimaryReference:
                    str = str + "Primary Reference";
                    break;

                case _Stratum.SecondaryReference:
                    str = str + "Secondary Reference";
                    break;
            }
            return ((((((((str + "\r\nLocal time: " + this.TransmitTimestamp.ToString()) + "\r\nPrecision: " + this.Precision.ToString() + " ms") + "\r\nPoll Interval: " + this.PollInterval.ToString() + " s") + "\r\nReference ID: " + this.ReferenceID.ToString()) + "\r\nRoot Dispersion: " + this.RootDispersion.ToString() + " ms") + "\r\nRound Trip Delay: " + this.RoundTripDelay.ToString() + " ms") + "\r\nLocal Clock Offset: " + this.LocalClockOffset.ToString() + " ms") + "\r\n");
        }


        public _LeapIndicator LeapIndicator
        {
            get
            {
                switch (((byte)(this.NTPData[0] >> 6)))
                {
                    case 0:
                        return _LeapIndicator.NoWarning;

                    case 1:
                        return _LeapIndicator.LastMinute61;

                    case 2:
                        return _LeapIndicator.LastMinute59;
                }
                return _LeapIndicator.Alarm;
            }
        }

        public int LocalClockOffset
        {
            get
            {
                TimeSpan span = (TimeSpan)((this.ReceiveTimestamp - this.OriginateTimestamp) - (this.ReceptionTimestamp - this.TransmitTimestamp));
                return (int)(span.TotalMilliseconds / 2.0);
            }
        }

        public _Mode Mode
        {
            get
            {
                switch (((byte)(this.NTPData[0] & 7)))
                {
                    case 1:
                        return _Mode.SymmetricActive;

                    case 2:
                        return _Mode.SymmetricPassive;

                    case 3:
                        return _Mode.Client;

                    case 4:
                        return _Mode.Server;

                    case 5:
                        return _Mode.Broadcast;
                }
                return _Mode.Unknown;
            }
        }

        public DateTime OriginateTimestamp
        {
            get
            {
                return this.ComputeDate(this.GetMilliSeconds(0x18));
            }
        }

        public uint PollInterval
        {
            get
            {
                return (uint)Math.Round(Math.Pow(2.0, (double)this.NTPData[2]));
            }
        }

        public double Precision
        {
            get
            {
                return (1000.0 * Math.Pow(2.0, (double)this.NTPData[3]));
            }
        }

        public DateTime ReceiveTimestamp
        {
            get
            {
                DateTime time = this.ComputeDate(this.GetMilliSeconds(0x20));
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return (time + offspan);
            }
        }

        public string ReferenceID
        {
            get
            {
                string val = "";
                switch (this.Stratum)
                {
                    case _Stratum.Unspecified:
                    case _Stratum.PrimaryReference:
                        return ((val + ((char)this.NTPData[12]) + ((char)this.NTPData[13])) + ((char)this.NTPData[14]) + ((char)this.NTPData[15]));

                    case _Stratum.SecondaryReference:
                        switch (this.VersionNumber)
                        {
                            case 3:
                                {
                                    string Address = this.NTPData[12].ToString() + "." + this.NTPData[13].ToString() + "." + this.NTPData[14].ToString() + "." + this.NTPData[15].ToString();
                                    try
                                    {
                                        return (Dns.GetHostEntry(Address).HostName + " (" + Address + ")");
                                    }
                                    catch (Exception)
                                    {
                                        return "N/A";
                                    }
                                }
                            case 4:
                                {
                                    DateTime time = this.ComputeDate(this.GetMilliSeconds(12));
                                    TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                                    DateTime datetime = time + offspan;
                                    return datetime.ToString();
                                }
                        }
                        break;

                    default:
                        return val;
                }
                return "N/A";
            }
        }

        public DateTime ReferenceTimestamp
        {
            get
            {
                DateTime time = this.ComputeDate(this.GetMilliSeconds(0x10));
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return (time + offspan);
            }
        }

        public double RootDelay
        {
            get
            {
                int temp = 0;
                temp = (0x100 * ((0x100 * ((0x100 * this.NTPData[4]) + this.NTPData[5])) + this.NTPData[6])) + this.NTPData[7];
                return (1000.0 * (((double)temp) / 65536.0));
            }
        }

        public double RootDispersion
        {
            get
            {
                int temp = 0;
                temp = (0x100 * ((0x100 * ((0x100 * this.NTPData[8]) + this.NTPData[9])) + this.NTPData[10])) + this.NTPData[11];
                return (1000.0 * (((double)temp) / 65536.0));
            }
        }

        public int RoundTripDelay
        {
            get
            {
                TimeSpan span = (TimeSpan)((this.ReceiveTimestamp - this.OriginateTimestamp) + (this.ReceptionTimestamp - this.TransmitTimestamp));
                return (int)span.TotalMilliseconds;
            }
        }



        public _Stratum Stratum
        {
            get
            {
                byte val = this.NTPData[1];
                switch (val)
                {
                    case 0:
                        return _Stratum.Unspecified;

                    case 1:
                        return _Stratum.PrimaryReference;
                }
                if (val <= 15)
                {
                    return _Stratum.SecondaryReference;
                }
                return _Stratum.Reserved;
            }
        }

        public DateTime TransmitTimestamp
        {
            get
            {
                DateTime time = this.ComputeDate(this.GetMilliSeconds(40));
                TimeSpan offspan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
                return (time + offspan);
            }
            set
            {
                this.SetDate(40, value);
            }
        }

        public byte VersionNumber
        {
            get
            {
                return (byte)((this.NTPData[0] & 0x38) >> 3);
            }
        }

        // Nested Types
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }
    }
}
