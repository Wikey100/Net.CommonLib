using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.CommonLib.Exception;
using System.Text;
using System.IO;

namespace Net.CommonLib.UnitTest
{
    [TestClass]
    public class ExceptionUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                CheckMsgParseFailExceptionFun();
            }
            catch(MsgParseFailException msgEx)
            {
                Assert.IsTrue(true);
            }
            catch (System.Exception ex)
            {

            }
        }

        private void CheckMsgParseFailExceptionFun()
        {
            string msg = "1122";
            if (msg.Length>0)
            {
                throw new MsgParseFailException("消息类型转换失败");
            }
        }
    }
}
