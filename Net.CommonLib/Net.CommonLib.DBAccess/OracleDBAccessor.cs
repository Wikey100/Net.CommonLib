/*******************************************************************
 * * 文件名： OracleDBAccessor.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

namespace Net.CommonLib.DBAccess
{
    internal class OracleDBAccessor
    {
        private OracleDBConnector connector;

        public OracleDBConnector Connector
        {
            get { return connector; }
            set { connector = value; }
        }

        public OracleDBAccessor()
        {
        }
    }
}