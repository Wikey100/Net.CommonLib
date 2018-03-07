/*******************************************************************
 * * 文件名： ConnectionBase.cs
 * * 文件作用： 数据库连接基类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using System;
using System.Data;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// 数据库连接基类
    /// </summary>
    public abstract class DBConnector
    {
        protected IDbConnection connection;
        public int ArrayCount { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        /// <summary>
        ///
        /// </summary>
        protected bool supportSqlPaged;

        public bool SupportSqlPaged
        {
            get { return supportSqlPaged; }
            set { supportSqlPaged = value; }
        }

        #region abastract Method

        /// <summary>
        /// 执行命令
        /// </summary>
        protected IDbCommand command;

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public abstract IDbConnection GetConnection(string connectionString);

        /// <summary>
        /// 创建命令
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="para">Sql参数</param>
        /// <returns></returns>
        public abstract IDbCommand GetCommand(string sql, IDataParameter[] para);

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public abstract IDbDataParameter GetParameter(string name, object value);

        /// <summary>
        /// 创建适配器
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <returns></returns>
        public abstract IDbDataAdapter GetAdapter(IDbCommand cmd);

        /// <summary>
        /// 获取分页Sql语句
        /// </summary>
        /// <param name="sql">SQL.</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="pageLen">页大小</param>
        /// <returns>分页Sql语句</returns>
        public abstract string GetPagedQuerySql(string sql, int startIndex, int pageLen);

        //public abstract string GetPagedQuerySqlByOrder(string sql, int startIndex, int pageLen, string orderProperty);

        #endregion abastract Method

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="type">类型</param>
        /// <param name="size">参数长度</param>
        /// <returns>参数接口</returns>
        public virtual IDbDataParameter GetParameter(string name, object value, DbType type, int size)
        {
            IDbDataParameter para = GetParameter(name, value);
            para.DbType = type;
            para.Size = size;
            return para;
        }

        public virtual IDbDataParameter GetParameter(string name, object value, DbType type, int size, ParameterDirection direction)
        {
            IDbDataParameter para = GetParameter(name, value, type, size);
            para.Direction = direction;
            para.Size = size;
            return para;
        }

        public virtual IDbDataParameter GetParameter(string name, object value, Int32 dbType, int size, ParameterDirection direction)
        {
            IDbDataParameter para = GetParameter(name, value, (DbType)dbType, size);
            para.Direction = direction;
            para.Size = size;
            return para;
        }

        //2012-5-17
        /// <summary>
        /// 获取当前正在执行命令
        /// </summary>
        /// <returns>当前正在执行命令</returns>
        public virtual IDbCommand GetCommand()
        {
            return command;
        }
    }
}