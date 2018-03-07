/*******************************************************************
 * * 文件名： BCDCoder.cs
 * * 文件作用： BCD编码类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-04-22    xwj       新增
 * *******************************************************************/

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Net.CommonLib.Util
{
    public class BCDCoder
    {
        /// <summary>
        /// 将时间转换为7字节的BCD数组
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static byte[] GetBytes(DateTime time)
        {
            byte[] buf = new byte[7];
            //转换日期
            int timeValue = time.Year;
            timeValue *= 100;
            timeValue += time.Month;
            timeValue *= 100;
            timeValue += time.Day;
            GetBytes(timeValue, 4).CopyTo(buf, 0);

            //转换时间
            timeValue = time.Hour;
            timeValue *= 100;
            timeValue += time.Minute;
            timeValue *= 100;
            timeValue += time.Second;
            GetBytes(timeValue, 3).CopyTo(buf, 4);

            return buf;
        }

        /// <summary>
        /// int型转换BCD码
        /// </summary>
        /// <param name="value">输入int值</param>
        /// <param name="byteNum">BCD码字节长度</param>
        /// <returns></returns>
        public static byte[] GetBytes(int value, int byteNum)
        {
            byte[] buf = new byte[byteNum];
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] |= (byte)(value % 10);
                value /= 10;
                buf[i] |= (byte)((value % 10) << 4);
                value /= 10;
            }
            return buf;
        }

        /// <summary>
        /// 将数字形式的字符串转换为BCD
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string str)
        {
            if (str == null || str == string.Empty)
            {
                throw new ArgumentException("string is null");
            }
            if (str.Length % 2 == 1)
            {
                str = "0" + str;
            }
            byte[] buf = new byte[str.Length / 2];

            for (int i = 0; i < buf.Length; i++)
            {
                int num = int.Parse(str.Substring(i * 2, 2));

                buf[i] = BCDCoder.GetBytes(num, 1)[0];
            }
            return buf;
        }

        public static DateTime GetDateTime(byte[] buf)
        {
            return GetDateTime(BCDCoder.GetString(buf));
        }

        // wzl 2010-11-29
        // 如果不是合法日期格式，就返回日期的最小值
        public static DateTime GetDateTime(string str)
        {
            DateTime datetime;

            if (Regex.IsMatch(str, "^[0-9]{14}$"))
            {
                str = str.Insert(12, ":");
                str = str.Insert(10, ":");
                str = str.Insert(8, " ");
                str = str.Insert(6, "/");
                str = str.Insert(4, "/");
            }
            else if (Regex.IsMatch(str, "^[0-9]{8}$"))
            {
                str = str.Insert(6, "/");
                str = str.Insert(4, "/");
            }

            if (DateTime.TryParse(str, out datetime))
            {
                return datetime;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static int GetInt32(byte[] buf)
        {
            return int.Parse(GetString(buf));
        }

        public static int GetInt32(byte b)
        {
            return int.Parse(GetString(b));
        }

        /// <summary>
        /// BCD形式的byte数组转换成字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static string GetString(byte[] byteArray)
        {
            return BCDCoder.GetString(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// BCD形式的byte数组转换成字符串
        /// </summary>
        /// <param name="byteArray">byte数组</param>
        /// <param name="offset">偏移量</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetString(byte[] byteArray, int offset, int len)
        {
            StringBuilder str = new StringBuilder();
            for (int i = offset; i < offset + len; i++)
            {
                str.Append(byteArray[i].ToString("X2"));
            }
            return str.ToString();
        }

        public static byte GetByte(int i)
        {
            return GetBytes(i, 1)[0];
        }

        public static string GetString(byte b)
        {
            return b.ToString("X2");
        }
    }
}