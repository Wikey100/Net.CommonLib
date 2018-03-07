/*******************************************************************
 * * 文件名： SybaseConnnection.cs
 * * 文件作用： sybase数据库连接
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using Sybase.Data.AseClient;
using System.Data;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// sysbase数据库连接类
    /// </summary>
    public class SybaseConnnector : DBConnector
    {
        /// <summary>
        /// 初始化 <see cref="SybaseConnnection"/> class的新实例.
        /// </summary>
        public SybaseConnnector()
        {
            supportSqlPaged = true;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="connStr">The conn STR.</param>
        /// <returns></returns>
        public override IDbConnection GetConnection(string connStr)
        {
            if (Connection == null)
            {
                Connection = new AseConnection(connStr);
            }
            return Connection;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public override IDbDataParameter GetParameter(string name, object value)
        {
            return new AseParameter(name, value);
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <returns></returns>
        public override IDbDataAdapter GetAdapter(IDbCommand cmd)
        {
            AseDataAdapter adapter = new AseDataAdapter((AseCommand)cmd);
            // lrh   2010-01-21
            AseCommandBuilder cb = new AseCommandBuilder(adapter);
            adapter.CommandBuilder = cb;
            return adapter;
        }

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="para">Sql参数</param>
        /// <returns></returns>
        public override IDbCommand GetCommand(string sql, IDataParameter[] para)
        {
            //创建新命令
            if (command == null)
            {
                command = new AseCommand(sql, (AseConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                ((AseCommand)command).Parameters.AddRange(para);
            }
            return command;
        }

        /// <summary>
        /// 获取分页Sql语句
        /// </summary>
        /// <param name="sql">SQL.</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="pageLen">页大小</param>
        /// <returns></returns>
        /// <remarks>sybase 不支持order by 嵌套</remarks>
        public override string GetPagedQuerySql(string sql, int startIndex, int pageLen)
        {
            //sybase 不支持order by 嵌套
            string orderby = string.Empty;
            if (sql.ToUpper().IndexOf("ORDER BY") > 0)
            {
                orderby = sql.Substring(sql.ToUpper().IndexOf("ORDER BY"));
                sql = sql.Substring(0, sql.ToUpper().IndexOf("ORDER BY"));
            }

            StringBuilder pageSql = new StringBuilder();
            pageSql.Append("select top ")
                .Append((startIndex + pageLen).ToString())
                .Append(" *,ids=identity(12) into #temp1 from(")
                .Append(sql)
                .Append(") as t ")
                .Append(orderby)
                .Append("\r\n select * from #temp1 where ids>=")
                .Append(startIndex.ToString())
                .Append(" and ids<")
                .Append((startIndex + pageLen).ToString())
                .Append("\r\n select count(*) from (").Append(sql).Append(") as t2")
                .Append("\r\n drop table #temp1");

            return pageSql.ToString();
        }
    }
}