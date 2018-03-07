/*******************************************************************
 * * 文件名： ILog.cs
 * * 文件作用： 日志接口
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-04-22    xwj       新增
 * *******************************************************************/

using System;

namespace Net.CommonLib.Log
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        void Error(string msg);

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e"></param>
        void Error(string msg, Exception e);

        /// <summary>
        /// 记录消息信息
        /// </summary>
        /// <param name="msg">信息</param>
        void Info(string msg);

        /// <summary>
        /// 记录消息信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e"></param>
        void Info(string msg, Exception e);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg">信息</param>
        void Warn(string msg);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e">The e.</param>
        void Warn(string msg, Exception e);

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        void Fatal(string msg);

        /// <summary>
        /// 记录重大错误信息
        /// </summary>
        /// <param name="msg">信息</param>
        /// <param name="e"></param>
        void Fatal(string msg, Exception e);

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg">信息</param>
        void Debug(string msg);

        /// <summary>
        /// 记录Debug信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="e"></param>
        void Debug(string msg, Exception e);
    }
}