/*******************************************************************
 * * 文件名： OracleDBConnection.cs
 * * 文件作用： Oracle数据库连接类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// Oracle数据库连接类
    /// </summary>
    public class OracleDBConnector : DBConnector
    {
        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public override System.Data.IDbConnection GetConnection(string connectionString)
        {
            if (Connection == null)
            {
                Connection = new OracleConnection(connectionString);
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
            //更新参数占位符
            sql = sql.Replace('@', ':');
            //创建新命令
            if (command == null)
            {
                command = new OracleCommand(sql, (OracleConnection)Connection);
            }
            //更新命令
            else
            {
                command.Parameters.Clear();
                command.CommandText = sql;
            }
            if (para != null)
            {
                foreach (var p in para)
                {
                    ((OracleCommand)command).Parameters.Add(p);
                }
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
            name = name.Replace("@", "");
            if (value is Array)
            {
                OracleParameter param = new OracleParameter();
                param.DbType = DbType.Int32;
                param.ParameterName = name;
                param.Value = value;
                param.Direction = ParameterDirection.Input;
                return param;
            }
            else
            {
                return new OracleParameter(name, value);
            }
        }

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public override System.Data.IDbDataAdapter GetAdapter(System.Data.IDbCommand cmd)
        {
            OracleDataAdapter adapter = new OracleDataAdapter((OracleCommand)cmd);
            OracleCommandBuilder cb = new OracleCommandBuilder(adapter);            // lrh   2010-01-21

            return adapter;
        }

        public override IDbDataParameter GetParameter(string name, object value, DbType dataType, int size)
        {
            name = name.Replace("@", "");
            OracleParameter param = new OracleParameter();
            param.DbType = dataType;
            param.ParameterName = name;
            param.Value = value;
            param.Direction = ParameterDirection.Input;
            return param;
        }

        public override IDbDataParameter GetParameter(string name, object value, int dbType, int size, ParameterDirection direction)
        {
            name = name.Replace("@", "");
            OracleParameter param = new OracleParameter();
            if (dbType == 6)
            {
                param.OracleType = OracleType.DateTime;
            }
            else if (dbType == 28)
            {
                param.OracleType = OracleType.Cursor;
            }
            else
            {
                param.OracleType = (OracleType)dbType;
            }
            param.ParameterName = name;
            param.Direction = direction;
            param.Size = size;
            if (param.Direction == ParameterDirection.Input)
            {
                param.Value = value;
            }
            return param;
        }

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