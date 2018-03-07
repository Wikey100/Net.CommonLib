/*******************************************************************
 * * 文件名： SQLiteDBConnection.cs
 * * 文件作用： SQLite数据库连接
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using System.Data;
using System.Data.SQLite;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// SQLite数据库连接
    /// </summary>
    public class SQLiteDBConnector : DBConnector
    {
        /// <summary>
        /// 初始化 <see cref="SQLiteDBConnection"/> class的新实例.
        /// </summary>
        public SQLiteDBConnector()
        {
            supportSqlPaged = true;
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <param name="connStr">The conn STR.格式Data Source=a.s3db;Pooling=true;</param>
        /// <returns></returns>
        public override IDbConnection GetConnection(string connStr)
        {
            if (Connection == null)
            {
                Connection = new SQLiteConnection(connStr);
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
            return new SQLiteParameter(name, value);
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public override IDbDataAdapter GetAdapter(IDbCommand cmd)
        {
            SQLiteDataAdapter adapter = new SQLiteDataAdapter((SQLiteCommand)cmd);
            // lrh   2010-01-21
            SQLiteCommandBuilder cb = new SQLiteCommandBuilder(adapter);
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
                command = new SQLiteCommand(sql, (SQLiteConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                ((SQLiteCommand)command).Parameters.AddRange(para);
            }
            return command;
        }

        /// <summary>
        /// 获取分页Sql语句
        /// </summary>
        /// <param name="sql">SQL.</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="pageLen">页大小</param>
        /// <returns>
        /// 分页Sql语句
        /// </returns>
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