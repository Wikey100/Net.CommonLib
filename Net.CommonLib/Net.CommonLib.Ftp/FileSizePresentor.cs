/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.Ftp
{
    public class FileSizePresentor
    {
        public static double Kilo = 1024;
        public static double Mill = 1024 * 1024;
        public static double Giga = 1024 * 1024 * 1024;

        //public static long Tega = 1024 * 1024 * 1024;
        public static string GetPersent(long size)
        {
            if (size < Kilo)
            {
                return size + "B";
            }
            else if (size < Mill)
            {
                return (size / Kilo).ToString("0.00") + "KB";
            }
            else if (size < Giga)
            {
                return (size / Mill).ToString("0.00") + "MB";
            }
            else
            {
                return (size / Giga).ToString("0.00") + "GB";
            }
        }
    }
}