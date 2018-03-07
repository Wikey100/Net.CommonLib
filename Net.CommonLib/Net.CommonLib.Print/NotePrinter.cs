/*******************************************************************
 * * 文件名： NotePrinter.cs
 * * 文件作用： 票据打印机打印类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2016-04-01    xwj       新增
 * *******************************************************************/

using System;
using System.IO.Ports;
using System.Text;

namespace Net.CommonLib.Print
{
    public class NotePrinter
    {
        private SerialPort serial = new SerialPort();
        private const string Cmd_Init = "1b40";
        private const string Cmd_NewLine = "0a";
        private const string Cmd_DefaultLineInterval = "1b32";
        private const string Cmd_SetLineInterval = "1b33";
        private const string Cmd_SlidePaper = "1b4a";
        private const string Cmd_SetCharType = "1b21";
        private const string Cmd_SetDoubleWidth = "1c0e";
        private const string Cmd_ResetDoubleWidth = "1c14";
        private const string Cmd_SetDoubleHeigth = "1c21";
        private const string Cmd_Bitmap = "1b2a";

        public NotePrinter(string comName, int baudRate)
        {
            serial.PortName = comName;
            serial.BaudRate = baudRate;
        }

        public void Print(string msg)
        {
            Write(msg);
            SendCmd(Cmd_NewLine);
        }

        private bool doubleWidth;

        /// <summary>
        /// 设置或获取是否是倍宽字符
        /// </summary>
        /// <value><c>true</c> if [double width]; otherwise, <c>false</c>.</value>
        public bool DoubleWidth
        {
            get { return doubleWidth; }
            set
            {
                doubleWidth = value;
                if (value)
                {
                    SendCmd(Cmd_SetDoubleWidth);
                }
                else
                {
                    SendCmd(Cmd_ResetDoubleWidth);
                }
            }
        }

        private bool doubleHeigth;

        /// <summary>
        /// 设置或获取是否是倍高字符
        /// </summary>
        /// <value><c>true</c> if [double heigth]; otherwise, <c>false</c>.</value>
        public bool DoubleHeigth
        {
            get { return doubleHeigth; }
            set
            {
                doubleHeigth = value;
                if (value)
                {
                    if (doubleWidth)
                        SendCmd(Cmd_SetCharType, 0x30);
                    else
                        SendCmd(Cmd_SetCharType, 0x10);
                }
                else
                {
                    if (doubleWidth)
                    {
                        SendCmd(Cmd_SetCharType, 0x20);
                    }
                    else
                    {
                        SendCmd(Cmd_SetCharType, 0);
                    }
                }
            }
        }

        /// <summary>
        /// 打印并走纸
        /// </summary>
        /// <param name="length">走纸长度，单位：1/203英寸</param>
        public void SlidePaper(byte length)
        {
            SendCmd(Cmd_SlidePaper, length);
        }

        public void SendCmd(string cmd, byte arg)
        {
            SendCmd(cmd, new byte[] { arg });
        }

        public void SendCmd(string cmd, byte[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                cmd += args[i].ToString("X2");
            }
            SendCmd(cmd);
        }

        private void SendCmd(string cmd)
        {
            byte[] buf = new byte[cmd.Length / 2];
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = byte.Parse(cmd.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }
            Write(buf);
        }

        /// <summary>
        /// Writes the specified buf.
        /// </summary>
        /// <param name="buf">The buf.</param>
        private void Write(byte[] buf)
        {
            try
            {
                if (!serial.IsOpen)
                {
                    serial.Open();
                }
                serial.Write(buf, 0, buf.Length);
            }
            catch (Exception e)
            {
                PrintException exp = new PrintException("打印输出失败", e);
                throw exp;
            }
            finally
            {
                serial.Close();
            }
        }

        /// <summary>
        /// 输出字符串.
        /// </summary>
        /// <param name="str">The STR.</param>
        private void Write(string str)
        {
            byte[] buf = Encoding.GetEncoding("GB2312").GetBytes(str);
            Write(buf);
        }
    }
}