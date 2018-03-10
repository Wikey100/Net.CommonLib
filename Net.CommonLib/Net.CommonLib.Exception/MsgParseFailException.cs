/*******************************************************************
 * * 文件名： MsgParseFailException.cs
 * * 文件作用： 自定义消息转换失败异常类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2018-03-10    xwj       新增
 * *******************************************************************/

using System;
using System.Runtime.Serialization;

namespace Net.CommonLib.Exception
{
    [Serializable]
    public class MsgParseFailException : ApplicationException
    {
        public MsgParseFailException()
        {
        }

        public MsgParseFailException(string message)
            : base(message)
        {
        }

        public MsgParseFailException(string message, ApplicationException exception)
            : base(message, exception)
        {
        }

        public MsgParseFailException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}