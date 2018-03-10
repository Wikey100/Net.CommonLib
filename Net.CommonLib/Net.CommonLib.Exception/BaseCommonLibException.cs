/*******************************************************************
 * * 文件名： BaseCommonLibException.cs
 * * 文件作用： 
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2018-03-10    xwj       新增
 * *******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Net.CommonLib.Exception
{
    [Serializable]
    public class BaseCommonLibException:ApplicationException
    {
        /// <summary>
        /// 添加一个默认的构造函数
        /// </summary>
        public BaseCommonLibException()
        {

        }

        /// <summary>
        /// 添加一个包含Message的构造函数
        /// </summary>
        /// <param name="message"></param>
        public BaseCommonLibException(string message)
            :base(message)
        {

        }

        /// <summary>
        /// 添加一个包含Message及内部异常类型参数的构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BaseCommonLibException(string message,ApplicationException innerException)
            :base(message,innerException)
        {

        }

        /// <summary>
        /// 添加一个序列化信息相关的参数的构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public BaseCommonLibException(SerializationInfo info,StreamingContext context)
            :base(info,context)
        {

        }
    }
}
