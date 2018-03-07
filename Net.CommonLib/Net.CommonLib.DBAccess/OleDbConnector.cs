/*******************************************************************
 * * 文件名： OleDbConnector.cs
 * * 文件作用：
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Data;
using System.Data.OleDb;

namespace Net.CommonLib.DBAccess
{
    internal class OleDbConnector : DBConnector
    {
        public OleDbConnector()
        {
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString">连接字符串 格式Provider=Microsoft.ACE.OLEDB.12.0;Data Source=G:\\Database1.accdb;</param>
        /// <returns></returns>
        public override IDbConnection GetConnection(string connectionString)
        {
            if (Connection == null)
            {
                Connection = new System.Data.OleDb.OleDbConnection(connectionString);
            }
            return Connection;
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="para">Sql参数</param>
        /// <returns></returns>
        public override System.Data.IDbCommand GetCommand(string sql, System.Data.IDataParameter[] para)
        {
            //创建新命令
            if (command == null)
            {
                command = new System.Data.OleDb.OleDbCommand(sql, (OleDbConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                ((OleDbCommand)command).Parameters.AddRange(para);
            }
            return command;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public override System.Data.IDbDataParameter GetParameter(string name, object value)
        {
            return new OleDbParameter(name, value);
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <returns></returns>
        public override System.Data.IDbDataAdapter GetAdapter(System.Data.IDbCommand cmd)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter((OleDbCommand)cmd);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);            // lrh   2010-01-21

            return adapter;
        }

        /// <summary>
        /// 获取分页Sql语句
        /// </summary>
        /// <param name="sql">SQL.</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="pageLen">页大小</param>
        /// <returns></returns>
        public override string GetPagedQuerySql(string sql, int startIndex, int pageLen)
        {
            throw new NotImplementedException();
        }
    }
}