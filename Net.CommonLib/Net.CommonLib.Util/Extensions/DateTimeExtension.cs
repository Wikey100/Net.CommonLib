/*******************************************************************
 * * 文件名： DateTimeExtension.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using System;

namespace Net.CommonLib.Util.Extensions
{
    public static class DateTimeExtension
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }

        public static int ComputeAge(this DateTime target)
        {
            return DateTime.Now.Year - target.Year;
        }
    }
}