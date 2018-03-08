/*******************************************************************
 * * 文件名： DBAccessor.cs
 * * 文件作用： 数据库访问类
 * *
 * *-------------------------------------------------------------------
 * *修改历史记录：
 * *修改时间      修改人    修改内容概要
 * *2013-02-23    xwj       新增
 * *******************************************************************/

using Suntek.Common.Log;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Net.CommonLib.DBAccess
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    /// <remarks>
    /// 屏蔽各个数据库类型不同的访问方式，调用每次查询或Sql执行都会启动连接并自动关闭，
    /// 防止连接忘记关闭。在执行事务处理时连接不自动关闭，直至调用<seealso cref="Commit"/>或
    /// Rollback时关闭
    /// </remarks>
    public class DBAccessor
    {
        #region field

        /// <summary>
        /// 连接字符串
        /// </summary>
        private string connectionString = string.Empty;

        /// <summary>
        /// 连接类型
        /// </summary>
        private ConnectType connectType;

        private DBConnector connector;
        private IDbTransaction transaction;
        private bool isTranscation = false;

        private int arrayCount = -1;

        /// <summary>
        /// 参数数组长度 OracleDb Only
        /// </summary>
        public int ArrayCount
        {
            get { return arrayCount; }
            set
            {
                arrayCount = value;
                this.DBConnector.ArrayCount = value;
            }
        }

        private ILog log;

        public ILog Log
        {
            get { return log; }
            set { log = value; }
        }

        private Stopwatch sw = new Stopwatch();

        #endregion field

        #region property

        /// <summary>
        /// 数据库连接
        /// </summary>
        public DBConnector DBConnector
        {
            get
            {
                if (connector == null)
                {
                    connector = ConnectorFactory.CreateConnector(connectType);
                    connector.GetConnection(connectionString);
                }
                return connector;
            }
            set { connector = value; }
        }

        #endregion property

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectString">连接字符串</param>
        /// <param name="type">数据库类型</param>
        public DBAccessor(string connectString, ConnectType type)
        {
            if (connectString == string.Empty)
            {
                throw new Exception("数据库连接字符串未指定");
            }
            this.connectionString = connectString;
            this.connectType = type;
        }

        public DBAccessor(DBConnector connector)
        {
            this.connector = connector;
        }

        /// <summary>
        /// 无查询执行
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <exception>DataException</exception>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, null);
        }

        /// <summary>
        /// 无查询执行
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <param name="param">参数</param>
        /// <exception>DataException</exception>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string sql, IDbDataParameter[] param)
        {
            return ExecuteNonQuery(sql, param, false);
        }

        /// <summary>
        /// 无查询执行
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <param name="param">参数</param>
        /// <param name="isStoredProcedure">是否是存储过程</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string sql, IDbDataParameter[] param, bool isStoredProcedure)
        {
            try
            {
                //打开连接
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(sql, param);
                //是否是事务
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                //是否是存储过程
                if (isStoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //关闭连接 当正在执行事务时连接不关闭
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行查询，并返回结果集的第一行第一列，忽略额外的行列
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <returns>查询对象</returns>
        public object ExecuteScalar(string sql)
        {
            return ExecuteScalar(sql, null);
        }

        /// <summary>
        /// 执行查询，并返回结果集的第一行第一列，忽略额外的行列
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <param name="param">参数</param>
        /// <returns>查询对象</returns>
        public object ExecuteScalar(string sql, IDbDataParameter[] param)
        {
            return ExecuteScalar(sql, param, false);
        }

        /// <summary>
        /// 执行查询，并返回结果集的第一行第一列，忽略额外的行列
        /// </summary>
        /// <param name="sql">执行SQL</param>
        /// <param name="param">参数</param>
        /// <param name="isStoredProcedure">是否是存储过程</param>
        /// <returns>查询对象</returns>
        public object ExecuteScalar(string sql, IDbDataParameter[] param, bool isStoredProcedure)
        {
            try
            {
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(sql, param);
                //cmd.Prepare();
                //是否是事务
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                //是否是存储过程
                if (isStoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                return cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 更新一个包含名为"Table"的数据表的数据集，为每个已经新增，修改，
        /// 删除的行执行相应的INSERT,UPDATE,DELETE语句
        /// </summary>
        /// <param name="selectSql">数据表查询Sql</param>
        /// <param name="dataSet">包含名为"Table"的数据表的数据集</param>
        /// <param name="parms">查询Sql参数</param>
        /// <exception cref="DBConcurrencyException"></exception>
        /// <returns>影响的行数</returns>
        public int Update(string selectSql, DataSet dataSet, IDbDataParameter[] parms)
        {
            try
            {
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(selectSql, parms);
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                IDbDataAdapter adapter = connector.GetAdapter(cmd);
                return adapter.Update(dataSet);
            }
            catch (DBConcurrencyException e)
            {
                throw e;
            }
            finally
            {
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 更新数据表的数据，为每个已经新增，修改，删除的行执行相应的INSERT,UPDATE,DELETE语句
        /// </summary>
        /// <param name="selectSql">数据表查询Sql</param>
        /// <param name="table">数据表</param>
        /// <param name="parms">查询Sql参数</param>
        /// <returns></returns>
        public int Update(string selectSql, DataTable table, IDbDataParameter[] parms)
        {
            try
            {
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(selectSql, parms);
                IDbDataAdapter adapter = connector.GetAdapter(cmd);
                table.TableName = "Table";
                DataSet ds = new DataSet();
                ds.Tables.Add(table);
                return adapter.Update(ds);
            }
            catch (DBConcurrencyException e)
            {
                throw e;
            }
            finally
            {
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 更新数据表的数据，为每个已经新增，修改，删除的行执行相应的INSERT,UPDATE,DELETE语句
        /// </summary>
        /// <param name="selectSql">数据表查询Sql</param>
        /// <param name="table">要更新的数据表</param>
        /// <returns></returns>
        public int Update(string selectSql, DataTable table)
        {
            return Update(selectSql, table, null);
        }

        /// <summary>
        /// 更新一个包含名为"Table"的数据表的数据集，为每个已经新增，
        /// 修改，删除的行执行相应的INSERT,UPDATE,DELETE语句
        /// </summary>
        /// <param name="selectSql">数据表查询Sql</param>
        /// <param name="dataSet">数据集</param>
        /// <returns></returns>
        public int Update(string selectSql, DataSet dataSet)
        {
            return Update(selectSql, dataSet, null);
        }

        /// <summary>
        /// 填充dataset
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="dataset">dataset</param>
        /// <exception>DataException</exception>
        /// <returns></returns>
        public int Fill(string sql, DataSet dataset)
        {
            return Fill(sql, dataset, null);
        }

        /// <summary>
        /// 填充dataset,并生成一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="dataset">dataset</param>
        /// <param name="param">参数</param>
        /// <exception>DataException</exception>
        /// <returns></returns>
        public int Fill(string sql, DataSet dataset, IDataParameter[] param)
        {
            return Fill(sql, dataset, param, false);
        }

        /// <summary>
        /// 填充dataset,并生成一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="dataset">dataset</param>
        /// <param name="param">参数</param>
        /// <param name="isStoredProcedure">是否是存储过程</param>
        /// <param name="startIndex"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int Fill(string sql, DataSet dataset, IDataParameter[] param,
            bool isStoredProcedure, int startIndex, int len)
        {
            sql = DBConnector.GetPagedQuerySql(sql, startIndex, len);
            return Fill(sql, dataset, param, isStoredProcedure);
        }

        /// <summary>
        /// 填充dataset,并生成一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="dataset">dataset</param>
        /// <param name="param">参数</param>
        /// <param name="isStoredProcedure">是否是存储过程</param>
        /// <returns></returns>
        public int Fill(string sql, DataSet dataset, IDataParameter[] param, bool isStoredProcedure)
        {
            try
            {
#if DEBUG
                sw.Reset();
                sw.Start();
#endif
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(sql, param);
                //是否是事务
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                //是否是存储过程
                if (isStoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }

                IDbDataAdapter adapter = GetAdapter(cmd);

                return adapter.Fill(dataset);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
#if DEBUG
                sw.Stop();
                if (isStoredProcedure)
                {
                    StringBuilder b = new StringBuilder();
                    foreach (var item in param)
                    {
                        b.Append(item.ParameterName + ":" + item.Value.ToString() + ";");
                    }
                    LogD(string.Format("EXEC:StoredProcedure[{0}],Parameter[{1}], Elapsed:[{2}]", sql, b.ToString(), sw.ElapsedMilliseconds));
                }
                else
                {
                    LogD(string.Format("EXEC:Sql[{0}], Elapsed:[{1}]", sql, sw.ElapsedMilliseconds));
                }

#endif
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 填充datatable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="table">数据表</param>
        [Obsolete("请使用ExecuteTable")]
        public void Fill(string sql, DataTable table)
        {
            Fill(sql, table, null);
        }

        /// <summary>
        /// 填充datatable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="table">数据表</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        [Obsolete("请使用ExecuteTable")]
        public void Fill(string sql, DataTable table, IDbDataParameter[] param)
        {
            Fill(sql, table, param, false);
        }

        /// <summary>
        /// 填充datatable
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="table">数据表</param>
        /// <param name="param">参数</param>
        /// <param name="isStoredProcedure">是否是存储过程</param>
        [Obsolete("请使用ExecuteTable")]
        public void Fill(string sql, DataTable table, IDbDataParameter[] param, bool isStoredProcedure)
        {
            try
            {
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(sql, param);
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                //是否是存储过程
                if (isStoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                IDataReader reader = cmd.ExecuteReader();

                table.Load(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (!isTranscation)
                {
                    DBConnector.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行返回一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="pageLen">页面大小</param>
        /// <param name="rowCount">查询总长度</param>
        /// <returns>数据表</returns>
        public DataTable ExecuteTable(string sql, IDbDataParameter[] param, int startIndex,
            int pageLen, ref int rowCount)
        {
            if (DBConnector.SupportSqlPaged)
            {
                DataSet ds = new DataSet();
                sql = DBConnector.GetPagedQuerySql(sql, startIndex, pageLen);
                Fill(sql, ds, param, false);
                rowCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                return ds.Tables[0];
            }
            else
            {
                DataSet ds = new DataSet();
                sql = DBConnector.GetPagedQuerySql(sql, startIndex, pageLen);
                Fill(sql, ds, param, false);
                rowCount = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                DataTableReader dtr = ds.Tables[0].CreateDataReader();
                for (int i = 0; i < startIndex; i++)
                {
                    dtr.Read();
                }
                DataTable dt = new DataTable();
                dt.Load(dtr);
                dtr.Close();
                return dt;
            }
        }

        /// <summary>
        /// 执行返回一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        /// <param name="isStoredProcedure">是否设成 <c>true</c> [是否是存储过程].</param>
        /// <returns>数据表</returns>
        public DataTable ExecuteTable(string sql, IDbDataParameter[] param, bool isStoredProcedure)
        {
            DataSet ds = new DataSet();
            Fill(sql, ds, param, isStoredProcedure);
            return ds.Tables[0];
        }

        /// <summary>
        /// 执行返回一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        /// <returns>数据表</returns>
        public DataTable ExecuteTable(string sql, IDbDataParameter[] param)
        {
            return ExecuteTable(sql, param, false);
        }

        /// <summary>
        /// 执行返回一个名为Table的DataTable
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns>数据表</returns>
        public DataTable ExecuteTable(string sql)
        {
            return ExecuteTable(sql, null, false);
        }

        /// <summary>
        /// 执行返回DataReader
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <remarks>该操作没有关闭数据库连接，必须手动调用<see cref="CloseConnection()"/>关闭数据库连接</remarks>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql)
        {
            return ExecuteReader(sql, null, false);
        }

        /// <summary>
        /// 执行返回DataReader.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        /// <param name="isStoredProcedure">是否设成 <c>true</c> [是否是存储过程].</param>
        /// <remarks>该操作没有关闭数据库连接，必须手动关闭DataReader或者调用<see cref="CloseConnection()"/>关闭数据库连接</remarks>
        /// <remarks>如果该操作处于事务过程中，请不要关闭DataReader或者调用<see cref="CloseConnection()"/>关闭数据库连接</remarks>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, IDbDataParameter[] param,
            bool isStoredProcedure, int startIndex, int pageLen)
        {
            sql = DBConnector.GetPagedQuerySql(sql, startIndex, pageLen);
            return ExecuteReader(sql, param, isStoredProcedure);
        }

        /// <summary>
        /// 执行返回DataReader.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="param">The param.</param>
        /// <param name="isStoredProcedure">是否设成 <c>true</c> [是否是存储过程].</param>
        /// <remarks>该操作没有关闭数据库连接，必须手动关闭DataReader或者调用<see cref="CloseConnection()"/>关闭数据库连接</remarks>
        /// <remarks>如果该操作处于事务过程中，请不要关闭DataReader或者调用<see cref="CloseConnection()"/>关闭数据库连接</remarks>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, IDbDataParameter[] param, bool isStoredProcedure)
        {
            try
            {
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }

                IDbCommand cmd = DBConnector.GetCommand(sql, param);
                if (isTranscation)
                {
                    cmd.Transaction = transaction;
                }
                if (isStoredProcedure)
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }

                IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 执行查询，并返回结果集的第一行第一列，且第一行第一列必须为数字型，忽略额外的行列
        /// <example>例：select count(*) from table</example>
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <returns>查询结果，查询失败时返回 -1</returns>
        public int GetNumberValue(string sql)
        {
            return GetNumberValue(sql, null);
        }

        /// <summary>
        /// 执行查询，并返回结果集的第一行第一列，且第一行第一列必须为数字型，忽略额外的行列
        /// <example>例：select count(*) from table</example>
        ///
        /// </summary>
        /// <param name="sql">查询SQL</param>
        /// <param name="param"></param>
        /// <returns>查询结果，查询失败时返回 -1</returns>
        public int GetNumberValue(string sql, IDbDataParameter[] param)
        {
            try
            {
#if DEBUG
                sw.Reset();
                sw.Start();
#endif
                object o = ExecuteScalar(sql, param);
#if DEBUG
                sw.Stop();
                LogD(string.Format("EXEC:[{0}] Elapsed:[{1}]", sql, sw.ElapsedMilliseconds));
#endif
                return int.Parse(o.ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 开始事务
        /// <remarks>必须使用Commit或Rollback结束事务</remarks>
        /// </summary>
        /// <param name="level">事务级别</param>
        public void BeginTransaction(IsolationLevel level)
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
            connector.Connection.Open();
            transaction = connector.Connection.BeginTransaction(level);
            isTranscation = true;
        }

        /// <summary>
        /// 开始事务 事务开始后将导致线程不安全
        /// </summary>
        /// <example><code>string sql = "insert into newtable([ID],[Name]) values(@ID,@Name)";
        ///try
        ///{
        ///    //开始事务
        ///    dbAccessor.BeginTransaction();
        ///
        ///    //执行操作
        ///    dbAccessor.ExecuteNonQuery(sql, new IDbDataParameter[]
        ///                              {
        ///                                 dbAccessor.GetParameter("@ID",3),
        ///                                 dbAccessor.GetParameter("@Name","name"),
        ///                              });
        ///
        ///    sql = "update newtable set [Name]=@Name where ID=@ID";
        ///    dbAccessor.ExecuteNonQuery(sql, new IDbDataParameter[]
        ///                              {
        ///                                 dbAccessor.GetParameter("@ID",3),
        ///                                 dbAccessor.GetParameter("@Name","name2"),
        ///                              });
        ///    //提交
        ///    dbAccessor.Commit();
        ///}
        ///catch (System.Exception e)
        ///{
        ///    dbAccessor.Log.Error(e.Message);
        ///    try
        ///    {
        ///        //回滚
        ///        dbAccessor.Rollback();
        ///    }catch(Exception exp)
        ///    {
        ///        dbAccessor.Log.Error("数据库回滚失败");
        ///        dbAccessor.Log.Error(exp.Message);
        ///    }
        ///}</code></example>
        public void BeginTransaction()
        {
            lock (this)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (DBConnector.Connection.State == ConnectionState.Closed)
                {
                    DBConnector.Connection.Open();
                }
                //DBConnector.Connection.Open();
                transaction = DBConnector.Connection.BeginTransaction();
                isTranscation = true;
            }
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        public void Commit()
        {
            lock (this)
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                        DBConnector.Connection.Close();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    transaction = null;
                    isTranscation = false;
                }
            }
        }

        /// <summary>
        /// 事务回滚
        /// </summary>
        public void Rollback()
        {
            lock (this)
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                        DBConnector.Connection.Close();
                        transaction = null;
                        isTranscation = false;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    transaction = null;
                    isTranscation = false;
                }
            }
        }

        /// <summary>
        /// 提交并不关闭连接 事务仍然可继续
        /// </summary>
        public void CommitNoDisconnect()
        {
            this.transaction.Commit();
        }

        /// <summary>
        /// 回滚并不关闭连接 事务仍然可继续
        /// </summary>
        public void RollBackNoDisconnect()
        {
            this.transaction.Rollback();
        }

        /// <summary>
        /// 当调用<seealso cref="RollBackNoDisconnect"/>或<seealso cref="CommitNoDisconnect"/>时
        /// 结束事务
        /// </summary>
        public void EndTransaction()
        {
            transaction = null;
            isTranscation = false;
            this.DBConnector.Connection.Close();
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseConnection()
        {
            if (DBConnector.Connection.State != ConnectionState.Closed)
            {
                DBConnector.Connection.Close();
            }
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public IDbDataParameter GetParameter(string name, object value)
        {
            return DBConnector.GetParameter(name, value);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>参数</returns>
        public IDbDataParameter GetParameter(string name, object value, DbType type, int size)
        {
            return DBConnector.GetParameter(name, value, type, size);
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数方向</param>
        /// <returns>参数</returns>
        public IDbDataParameter GetParameter(string name, object value, DbType type, int size, ParameterDirection direction)
        {
            return DBConnector.GetParameter(name, value, type, size, direction);
        }

        public IDbDataParameter GetParameter(string name, object value, Int32 type, int size, ParameterDirection direction)
        {
            return DBConnector.GetParameter(name, value, type, size, direction);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public object GetParameterValue(string name)
        {
            return (DBConnector.GetCommand().Parameters[name] as IDbDataParameter).Value;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="name">参数id</param>
        /// <returns>参数值</returns>
        public object GetParameterValue(int index)
        {
            return (DBConnector.GetCommand().Parameters[index] as IDbDataParameter).Value;
        }

        /// <summary>
        /// 获取适配器
        /// </summary>
        /// <param name="cmd">数据库访问命令</param>
        /// <returns>适配器接口</returns>
        public IDbDataAdapter GetAdapter(IDbCommand cmd)
        {
            return DBConnector.GetAdapter(cmd);
        }

        /// <summary>
        /// 获取连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return DBConnector.Connection;
        }

        private void LogD(string p)
        {
            try
            {
                if (log != null)
                {
                    log.Debug(p);
                }
            }
            catch { }
        }
    }
}