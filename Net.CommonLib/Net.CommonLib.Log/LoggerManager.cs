/*******************************************************************
 * * 文件名： LoggerManager.cs
 * * 文件作用： 日志管理类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-04-22    xwj       新增
 * *******************************************************************/

using System.Collections;

namespace Net.CommonLib.Log
{
    /// <summary>
    /// 日志管理类
    /// </summary>
    public class LoggerManager
    {
        private static Hashtable logTable = new Hashtable();

        /// <summary>
        /// 获取Log接口
        /// </summary>
        /// <example>ILog log = LoggerManager.GetLogger("DBLog");</example>
        /// <param name="logName">log名</param>
        /// <returns>log接口</returns>
        public static ILog GetLogger(string logName)
        {
            if (logTable.Contains(logName))
            {
                return (ILog)logTable[logName];
            }
            else
            {
                Logger log = new Logger(logName);
                log.SetConfig();
                logTable.Add(logName, log);
                return log;
            }
        }

        /// <summary>
        /// 获取Log接口
        /// </summary>
        /// <remarks>当isDefaultSetting设成false时，必须调用(log as Suntek.Common.Log.Logger).SetConfig();设置配置</remarks>
        /// <param name="logName">log名.</param>
        /// <param name="isDefaultSetting">是否设成 <c>true</c> [使用默认设置].</param>
        /// <returns>log接口</returns>
        public static ILog GetLogger(string logName, bool isDefaultSetting)
        {
            if (logTable.Contains(logName))
            {
                return (ILog)logTable[logName];
            }
            else
            {
                Logger log = new Logger(logName);
                if (isDefaultSetting)
                {
                    log.SetConfig();
                }
                logTable.Add(logName, log);
                return log;
            }
        }
    }

    /// <summary>
    /// log输出类型
    /// </summary>
    public enum LogType : short
    {
        /// <summary>
        ///
        /// </summary>
        None = 0x00,

        /// <summary>
        /// log文件
        /// </summary>
        File = 0x01,         //log文件

        /// <summary>
        /// 控制台输出
        /// </summary>
        Console = 0x02,         //控制台输出

        /// <summary>
        /// 数据库输出
        /// </summary>
        DataBase = 0x04,         //数据库输出

        /// <summary>
        /// Debug输出
        /// </summary>
        Debug = 0x08,         //Debug输出
    }

    /// <summary>
    /// 文件生成方式
    /// </summary>
    public enum RollingType
    {
        /// <summary>
        /// 唯一
        /// </summary>
        Once = 0,

        /// <summary>
        /// 按大小
        /// </summary>
        Size = 1,

        /// <summary>
        /// 按时间
        /// </summary>
        Date = 2,

        /// <summary>
        /// 混合
        /// </summary>
        Composite = 3,
    }
}