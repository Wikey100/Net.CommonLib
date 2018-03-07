using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net.CommonLib.DBAccess;

namespace Net.CommonLib.UnitTest
{
    [TestClass]
    public class DBAccessUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Assert.IsNotNull()
        }

        [TestMethod]
        public void TestAdd()
        {
            var dt = new OracleDBConnector().GetPagedQuerySql("", 1, 20);
            Assert.IsNotNull(dt);
        }
    }
}
