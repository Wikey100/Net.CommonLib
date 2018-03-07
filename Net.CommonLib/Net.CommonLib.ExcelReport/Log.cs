/*******************************************************************
 * * 文件名：
 * * 文件作用：
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.IO;
using System.Text;

namespace Net.CommonLib.ExcelReport
{
    public class Log
    {
        public static Log Instance = new Log();

        private string logRootPath = string.Empty;
        private string logFilePath = string.Empty;

        private static readonly object WriteLock = new object();

        public Log()
        {
            InitLogRootPath(AppDomain.CurrentDomain.BaseDirectory);
        }

        public void InitLogRootPath(string path)
        {
            try
            {
                logRootPath = path;
                logFilePath = Path.Combine(path, "Report.log");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception)
            {
                logRootPath = string.Empty;
                logFilePath = string.Empty;
            }
        }

        private void AppendLog(string strLogMsg)
        {
            lock (WriteLock)
            {
                try
                {
                    if (File.Exists(logFilePath))
                    {
                        long len = new FileInfo(logFilePath).Length;
                        if (len > 1024 * 1024)
                        {
                            File.WriteAllText(logFilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">  日志文件大于1M,重置\r\n", Encoding.Default);
                        }
                    }

                    if (!Directory.Exists(logRootPath))
                    {
                        Directory.CreateDirectory(logRootPath);
                    }

                    using (StreamWriter writer = new StreamWriter(logFilePath, true, Encoding.Default))
                    {
                        writer.WriteLine(strLogMsg);
                        writer.Flush();
                    }
                }
                catch { }
            }
        }

        public void Reset()
        {
            //File.WriteAllText(logFilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">  Start..........\r\n", Encoding.Default);
            Info("Start..........");
        }

        public void Info(string str)
        {
            if (logFilePath == string.Empty)
            {
                return;
            }
            string logStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ">  " + str;
            AppendLog(logStr);
        }
    }
}