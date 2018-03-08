/*******************************************************************
 * * 文件名： StringExtension.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2014-04-27    xwj       新增
 * *******************************************************************/

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Net.CommonLib.Util.Extensions
{
    public static partial class StringExtension
    {
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        public static string FillEmptyNull(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return "";
            }
            return target;
        }

        public static bool IsNullOrEmpty(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return true;
            }

            return false;
        }

        public static string FormatWith(this string target, params object[] args)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return string.Format(target, args);
        }

        public static string Hash(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }

        public static bool IsValidDateTime(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            DateTime dt;

            if (DateTime.TryParse(target, out dt))
            {
                return dt.IsValid();
            }

            return false;
        }

        public static bool IsValidInt(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            int value;

            if (int.TryParse(target, out value))
            {
                return true;
            }

            return false;
        }

        public static bool IsValidDecimal(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            decimal value;

            if (decimal.TryParse(target, out value))
            {
                return true;
            }

            return false;
        }

        public static DateTime ParseDateTime(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return DateTime.Parse(target);
        }

        public static int ParseInt(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return int.Parse(target);
        }

        public static int TryParseInt(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            int value;

            int.TryParse(target, out value);

            return value;
        }

        public static long ParseLong(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return long.Parse(target);
        }

        public static decimal ParseDecimal(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return decimal.Parse(target);
        }

        public static bool ParseBool(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            return bool.Parse(target);
        }

        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(target))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        public static string[] SplitString(this string target, char delimiter)
        {
            if (String.IsNullOrEmpty(target))
            {
                return new string[0];
            }

            var split = from piece in target.Split(delimiter)
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;

            return split.ToArray();
        }

        /// <summary>
        /// 将字符串填充到指定长度(后面补空格的方式)
        /// </summary>
        /// <param name="target"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static string FillSpace(this string target, int totalLength)
        {
            Check.Argument.IsNotEmpty(target, "target");

            StringBuilder builder = new StringBuilder(target);

            int count = totalLength - target.Length;

            if (count > 0)
            {
                for (int i = 1; i <= count; i++)
                {
                    builder.Append("\0");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 从路径中获取文件名
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string GetFileName(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            string[] s = target.Split('\\');

            return s[s.Length - 1];
        }

        public static string GetStationIDByFullDeviceID(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            if (target.Length == 10)
            {
                return target.Substring(0, 4);
            }
            return string.Empty;
        }

        public static string GetDeviceTypeByFullDeviceID(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            if (target.Length == 10)
            {
                return target.Substring(4, 2);
            }
            return string.Empty;
        }

        public static string GetDeviceIDByFullDeviceID(this string target)
        {
            Check.Argument.IsNotEmpty(target, "target");

            if (target.Length == 10)
            {
                return target.Substring(6, 4);
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回字符串中的日期时间
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool TryGetDateTime(this string str, out DateTime result)
        {
            Check.Argument.IsNotEmpty(str, "str");
            result = DateTime.Now;
            //日期时间正则表达式
            string parttern = @"(?<year>\d{4})[\s|-]?(?<month>\d{1,2})[\s|-]?(?<day>\d{1,2})[\s|-]?((?<hour>\d{1,2})[\s|:]?(?<min>\d{1,2})[\s|:]?(?<sec>\d{1,2}))?";

            Match match = Regex.Match(str, parttern);

            if (!match.Success)
            {
                return false;
            }

            //从字符串中或取出年月日时分秒
            string year = match.Groups["year"].Value;
            string month = match.Groups["month"].Value;
            string day = match.Groups["day"].Value;
            string hour = match.Groups["hour"].Value;
            string min = match.Groups["min"].Value;
            string sec = match.Groups["sec"].Value;

            //过滤
            year = string.IsNullOrWhiteSpace(year) ? DateTime.MinValue.Year.ToString() : year;
            month = string.IsNullOrWhiteSpace(month) ? "01" : month;
            day = string.IsNullOrWhiteSpace(day) ? "01" : day;

            hour = string.IsNullOrWhiteSpace(hour) ? "00" : hour;
            min = string.IsNullOrWhiteSpace(min) ? "00" : min;
            sec = string.IsNullOrWhiteSpace(sec) ? "00" : sec;

            string dtStr = string.Format("{0}-{1}-{2} {3}:{4}:{5}", year, month, day, hour, min, sec);

            //释放
            match = null;

            //转换格式并返回
            if (DateTime.TryParse(dtStr, out result))
            {
                return true;
            }
            return false;
        }
    }
}