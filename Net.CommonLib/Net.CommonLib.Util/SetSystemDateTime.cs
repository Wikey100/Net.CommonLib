using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Net.CommonLib.Util
{
    public class SetSystemDateTime
    {
        [DllImport("Kernel32.dll")]
        private static extern Boolean SetSystemTime([In, Out] SystemTime st);

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="newdatetime">新时间</param>
        /// <returns></returns>
        public static bool SetSysTime(DateTime newdatetime)
        {
            SystemTime st = new SystemTime
            {
                year = Convert.ToUInt16(newdatetime.Year),
                month = Convert.ToUInt16(newdatetime.Month),
                day = Convert.ToUInt16(newdatetime.Day),
                dayofweek = Convert.ToUInt16(newdatetime.DayOfWeek),
                hour = Convert.ToUInt16(newdatetime.Hour -
                                        TimeZone.CurrentTimeZone.GetUtcOffset(new DateTime(2001, 09, 01)).Hours),
                minute = Convert.ToUInt16(newdatetime.Minute),
                second = Convert.ToUInt16(newdatetime.Second),
                milliseconds = Convert.ToUInt16(newdatetime.Millisecond)
            };
            return SetSystemTime(st);
        }
    }

    /// <summary>
    ///系统时间类
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class SystemTime
    {
        public ushort year;
        public ushort month;
        public ushort dayofweek;
        public ushort day;
        public ushort hour;
        public ushort minute;
        public ushort second;
        public ushort milliseconds;
    }
}
