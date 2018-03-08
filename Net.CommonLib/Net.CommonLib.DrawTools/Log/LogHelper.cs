/**********************************************************
** 文件名： LogHelper.cs
** 文件作用:车站设备编辑工具日志类
**
**---------------------------------------------------------
**修改历史记录：
**修改时间      修改人    修改内容概要
**2016-02-02    xwj       增加
**
**********************************************************/

using System;

namespace DrawTools.Log
{
    public class LogHelper
    {
        /// <summary>
        /// 一般日志信息
        /// </summary>
        /// <param name="msg"></param>
        public static void DeviceDeviceLogInfo(string msg)
        {
            //Log.Instance.DeviceConfigInfo(msg);
        }

        /// <summary>
        /// 错误日志信息
        /// </summary>
        /// <param name="msg"></param>
        public static void DeviceConfigLogError(string msg)
        {
            //Log.Instance.DeviceConfigError(msg);
        }

        /// <summary>
        /// 错误日志信息，包含错误异常信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public static void DeviceConfigLogError(string msg, Exception ex)
        {
            //Log.Instance.DeviceConfigError(msg, ex);
        }
    }
}