/*******************************************************************
 * * 文件名： InvalidCRCException.cs
 * * 文件作用： 自定义CRC异常类
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
    public class InvalidCRCException:ApplicationException
    {
        public InvalidCRCException()
        {

        }

        public InvalidCRCException(string message)
            :base(message)
        {

        }

        public InvalidCRCException(string message,ApplicationException exception)
            :base(message,exception)
        {

        }

        public InvalidCRCException(SerializationInfo info,StreamingContext context)
            :base(info,context)
        {

        }
    }
}
