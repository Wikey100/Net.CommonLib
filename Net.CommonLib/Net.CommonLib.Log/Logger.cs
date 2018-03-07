/*******************************************************************
 * * 文件名： Logger.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-04-22    xwj       新增
 * *******************************************************************/

using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Net.CommonLib.Log
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Logger : ILog
    {
        private string strLayoutPattern = "%date 线程ID:[%thread] [%-5level]: %message%newline";

        /// <summary>
        /// 设置输出格式。默认："%date 线程ID:[%thread] [%-5level]: %message%newline"
        /// </summary>
        /// <value>输出格式.</value>
        public string StrLayoutPattern
        {
            get { return strLayoutPattern; }
            set
            {
                strLayoutPattern = value;
                SetConfig();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public string DBConnectionString = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public string DBInsertSql = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public DBAppenderParameter[] DBParameter;

        private log4net.ILog log;
        private ILoggerRepository rep;
        private string filePath;
        private string logName;
        private string directory;
        private RollingFileAppender fileApp;
        private LogType logType = LogType.File;
        private RollingType rollingMode = RollingType.Date;
        private long maxFileSize = 10 * 1024 * 1024;
        private int maxSizeRollBackups = 10;

        /// <summary>
        ///
        /// </summary>
        public int MaxSizeRollBackups
        {
            get { return maxSizeRollBackups; }
            set { maxSizeRollBackups = value; }
        }

        // private string datePattern = ".yyyyMMdd'.log'";

        private string datePattern = @"\\\\yyyy-MM-dd\\\\yyyyMMdd'.log'";

        /// <summary>
        /// Gets or sets 日志输出方式.
        /// </summary>
        /// <value>The type of the log.</value>
        public LogType LogType
        {
            get { return logType; }
            set
            {
                logType = value;
            }
        }

        /// <summary>
        /// 设置或获取按日期生成文件时的文件名格式,默认为".yyyyMMdd'.log'"
        /// </summary>
        /// <remarks>设置该属性会导致生成多个文件：1.原文件名文件;2.文件名修改后文件</remarks>
        /// <value>文件名格式.</value>
        public string DatePattern
        {
            get { return datePattern; }
            set
            {
                datePattern = value;
            }
        }

        /// <summary>
        /// 设置或获取
        /// </summary>
        /// <value>The file rolling mode.</value>
        public RollingType FileRollingMode
        {
            get { return rollingMode; }
            set
            {
                rollingMode = value;
            }
        }

        /// <summary>
        /// 日志目录，如果不指定，默认为"..\config\"
        /// </summary>
        public string Directory
        {
            get { return directory; }
            set
            {
                directory = value;
                if (Path.IsPathRooted(directory))
                {
                    filePath = Path.Combine(directory, logName + ".log");
                }
                else
                {
                    filePath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)), directory);
                    filePath = Path.Combine(filePath, logName + ".log");
                }
                fileApp.File = filePath;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public long MaxFileSize
        {
            get { return maxFileSize; }
            set { maxFileSize = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int MaxBackIndex { get; set; }

        public string LogRoot { get; set; }

        /// <summary>
        /// 初始化 <see cref="Logger"/> class的实例.
        /// </summary>
        /// <param name="logName">Name of the log.</param>
        public Logger(string logName)
        {
            ////设置显示内容
            //PatternLayout layout = new PatternLayout(strLayoutPattern);

            this.logName = logName;
            LogRoot = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName)), "Log");
            filePath = Path.Combine(LogRoot, logName + ".log");

            ////创建Log文件输出方式
            fileApp = new RollingFileAppender();

            fileApp.Layout = new PatternLayout(strLayoutPattern);
            fileApp.File = filePath;
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        public void SetConfig()
        {
            //不是第一次设置
            if (rep != null)
            {
                rep.ResetConfiguration();
            }
            else//初次创建
            {
                //创建Repository
                try
                {
                    rep = LogManager.GetRepository(logName);
                    //log = LogManager.GetLogger(rep.Name, logName);
                }
                catch
                {
                    rep = LogManager.CreateRepository(logName);
                    log4net.Config.BasicConfigurator.Configure(rep, fileApp);
                    log = LogManager.GetLogger(logName, logName);
                }
            }

            //重设文件输出
            RollingFileAppender fileAppender = new RollingFileAppender();
            fileAppender.Layout = new PatternLayout(strLayoutPattern);

            fileAppender.RollingStyle = (RollingFileAppender.RollingMode)rollingMode;
            if (rollingMode == RollingType.Date)
            {
                fileAppender.StaticLogFileName = false;
                fileAppender.DatePattern = @"\\\\yyyy-MM-dd\\\\yyyyMMdd'.log'";
                fileAppender.File = filePath;

                if (fileAppender.File != null && fileAppender.File.EndsWith(".log"))
                    fileAppender.File = fileAppender.File.Remove(fileAppender.File.IndexOf(".log"));
            }
            else
            {
                fileAppender.File = filePath;
            }
            fileAppender.LockingModel = new RollingFileAppender.MinimalLock();
            fileAppender.AppendToFile = true;
            fileAppender.Encoding = Encoding.GetEncoding("GB2312");
            //fileAppender.CountDirection = 1;
            fileAppender.MaxFileSize = maxFileSize;
            fileAppender.MaxSizeRollBackups = this.MaxSizeRollBackups;

            fileAppender.ActivateOptions();
            fileApp = fileAppender;

            if ((logType & LogType.File) == LogType.File)
            {
                //创建Log文件输出方式
                log4net.Config.BasicConfigurator.Configure(rep, fileApp);
            }
            //控制台输出
            if ((logType & LogType.Console) == LogType.Console)
            {
                PatternLayout layout = new PatternLayout(strLayoutPattern);
                ConsoleAppender appender = new ConsoleAppender();
                appender.Layout = layout;
                log4net.Config.BasicConfigurator.Configure(rep, appender);
            }
            //数据库输出
            if ((logType & LogType.DataBase) == LogType.DataBase)
            {
                AdoNetAppender appender = new AdoNetAppender();
                appender.ConnectionString = DBConnectionString;
                appender.CommandText = DBInsertSql;
                if (DBParameter != null)
                {
                    for (int i = 0; i < DBParameter.Length; i++)
                    {
                        appender.AddParameter(DBParameter[i]);
                    }
                }
                log4net.Config.BasicConfigurator.Configure(rep, appender);
            }
            //Debug输出
            if ((logType & LogType.Debug) == LogType.Debug)
            {
                DebugAppender appender = new DebugAppender();
                appender.Layout = new PatternLayout(strLayoutPattern);
                log4net.Config.BasicConfigurator.Configure(rep, appender);
            }

            log = LogManager.GetLogger(rep.Name, logName);
        }

        #region 接口实现

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg)
        {
            log.Error(msg);
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public void Error(string msg, Exception e)
        {
            log.Error(msg, e);
        }

        /// <summary>
        /// 记录消息信息
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            log.Info(msg);
        }

        /// <summary>
        /// Infoes the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="e">The e.</param>
        public void Info(string msg, Exception e)
        {
            log.Info(msg, e);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(string msg)
        {
            log.Warn(msg);
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public void Warn(string msg, Exception e)
        {
            log.Warn(msg, e);
        }

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg"></param>
        public void Fatal(string msg)
        {
            log.Fatal(msg);
        }

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e"></param>
        public void Fatal(string msg, Exception e)
        {
            log.Fatal(msg, e);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg">信息</param>
        public void Debug(string msg)
        {
            log.Debug(msg);
        }

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        public void Debug(string msg, Exception e)
        {
            log.Debug(msg, e);
        }

        #endregion 接口实现
    }

    /// <summary>
    ///
    /// </summary>
    public class DBAppenderParameter : AdoNetAppenderParameter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="paramterName"></param>
        /// <param name="dbType"></param>
        /// <param name="layout"></param>
        /// <param name="size"></param>
        public DBAppenderParameter(string paramterName, DbType dbType, string layout, int size)
        {
            this.DbType = dbType;
            this.ParameterName = paramterName;
            RawPropertyLayout rawlayout = new RawPropertyLayout();
            rawlayout.Key = layout;
            this.Layout = rawlayout;
            this.Size = size;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="dbType"></param>
        /// <param name="layout"></param>
        public DBAppenderParameter(string parameterName, DbType dbType, string layout)
        {
            this.DbType = dbType;
            this.ParameterName = parameterName;

            RawPropertyLayout rawLayout = new RawPropertyLayout();
            rawLayout.Key = layout;
            this.Layout = rawLayout;
        }
    }
}