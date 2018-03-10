/*******************************************************************
 * * 文件名： ImportFileFailException.cs
 * * 文件作用： 自定义导入文件失败异常类
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
using System.Runtime;
using System.Runtime.Serialization;

namespace Net.CommonLib.Exception
{
    [Serializable]
    public class ImportFileFailException:ApplicationException
    {
        /// <summary>
        /// 添加一个默认的构造函数
        /// </summary>
        public ImportFileFailException()
        {

        }

        /// <summary>
        /// 添加一个包含Message的构造函数
        /// </summary>
        /// <param name="message"></param>
        public ImportFileFailException (string message)
            :base(message)
        {

        }

        /// <summary>
        /// 添加一个包含Message和内部类型参数错误信息的构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public ImportFileFailException(string message,ApplicationException exception)
            :base(message,exception)
        {

        }

        /// <summary>
        /// 添加一个序列化信息相关的参数的构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ImportFileFailException(SerializationInfo info,StreamingContext context)
            :base(info,context)
        {

        }

    }
}
