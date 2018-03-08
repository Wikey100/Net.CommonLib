/*******************************************************************
 * * 文件名： ConnectorFactory.cs
 * * 文件作用：连接器工厂类
 * *-------------------------------------------------------------------
 * * 修改历史记录：
 * * 修改时间      修改人    修改内容概要
 * * 2013-02-23    xwj       新增 
 * *******************************************************************/

using System;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// 连接器工厂类
    /// </summary>
    internal class ConnectorFactory
    {
        /// <summary>
        /// 创建连接器
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <returns>连接器</returns>
        public static DBConnector CreateConnector(ConnectType type)
        {
            DBConnector connector = null;
            switch (type)
            {
                case ConnectType.Sybase:
                    connector = new SybaseConnnector();
                    break;

                case ConnectType.SQLite:
                    connector = new SQLiteDBConnector();
                    break;

                case ConnectType.Oracle:
                    connector = new OracleDBConnector();
                    break;

                case ConnectType.SQLServer:
                    connector = new SQLServerDBConnector();
                    break;

                case ConnectType.MsAccess:
                case ConnectType.OleDb:
                    connector = new OleDbConnector();
                    break;

                case ConnectType.MySql:
                    connector = new MySqlConnector();
                    break;

                default:
                    throw new Exception("数据库类型未指定");
            }
            return connector;
        }
    }
}