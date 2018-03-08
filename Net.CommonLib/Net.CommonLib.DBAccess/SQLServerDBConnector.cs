/*******************************************************************
 * * 文件名： SQLServerDBConnection.cs
 * * 文件作用： Sqlserver连接类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Data.SqlClient;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// Sqlserver连接类
    /// </summary>
    internal class SQLServerDBConnector : DBConnector
    {
        public SQLServerDBConnector()
        {
            supportSqlPaged = false;
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public override System.Data.IDbConnection GetConnection(string connectionString)
        {
            if (Connection == null)
            {
                Connection = new SqlConnection(connectionString);
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
                command = new SqlCommand(sql, (SqlConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                ((SqlCommand)command).Parameters.AddRange(para);
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
            return new SqlParameter(name, value);
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <returns></returns>
        public override System.Data.IDbDataAdapter GetAdapter(System.Data.IDbCommand cmd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter((SqlCommand)cmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
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
            StringBuilder pageSql = new StringBuilder();
            pageSql.Append("select * from (")
                .Append(sql)
                .Append(") limit ")
                .Append(startIndex)
                .Append(",")
                .Append(pageLen).Append("; select count(*) from(").Append(sql).Append(")");
            return pageSql.ToString();
        }
    }
}