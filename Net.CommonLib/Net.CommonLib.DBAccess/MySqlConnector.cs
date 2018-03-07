/*******************************************************************
 * * 文件名： MySqlConnector.cs
 * * 文件作用： MySql连接类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using MySql.Data.MySqlClient;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// MySql连接类
    /// </summary>
    public class MySqlConnector : DBConnector
    {
        public MySqlConnector()
        {
            SupportSqlPaged = true;
        }

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public override System.Data.IDbConnection GetConnection(string connectionString)
        {
            if (this.Connection == null)
                this.Connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
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
                command = new MySql.Data.MySqlClient.MySqlCommand(sql, (MySql.Data.MySqlClient.MySqlConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                ((MySql.Data.MySqlClient.MySqlCommand)command).Parameters.AddRange(para);
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
            return new MySqlParameter(name, value);
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <returns></returns>
        public override System.Data.IDbDataAdapter GetAdapter(System.Data.IDbCommand cmd)
        {
            MySqlDataAdapter adapter = new MySqlDataAdapter((MySqlCommand)cmd);
            MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter);
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
                .Append(") _temp limit ")
                .Append(startIndex)
                .Append(",")
                .Append(pageLen)
                .Append(";")
                .Append("select count(*) from (").Append(sql).Append(") _tempCount");
            return pageSql.ToString();
        }
    }
}