/*******************************************************************
 * * 文件名： PrintException.cs
 * * 文件作用：打印异常自定义
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2016-04-01    xwj       新增
 * *******************************************************************/

using System;

namespace Net.CommonLib.Print
{
    public class PrintException : Exception
    {
        public PrintException(string msg, Exception exp) : base(msg, exp)
        {
        }

        public override string StackTrace
        {
            get
            {
                return this.InnerException.StackTrace;
            }
        }

        public override string Message
        {
            get
            {
                return InnerException.Message;
            }
        }
    }
}