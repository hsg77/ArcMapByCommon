using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Data.Common;

namespace ArcMapByCommon
{
    /// <summary>
    /// 数据库连接 封装类
    /// </summary>
    [Serializable]
    public abstract class DbConnectionWrapper : IDbConnectionWrapper, ISQLSelecter, IDisposable 
    {
        protected DbConnection m_dbconn = null;
        protected DbCommand m_cmd=null;
        protected DbTransaction m_transaction=null;
        protected DbConnState m_DbConnState = DbConnState.Free;
        protected string m_ConnectionString = "";

        //构造函数
        public DbConnectionWrapper(DbConnection dbconn)
        {
            m_dbconn = dbconn;
            m_DbConnState = DbConnState.Free;
        }
        public DbConnectionWrapper(string pConnectionString)
        {
            //需初始化连接对象
            m_ConnectionString = pConnectionString;
            m_DbConnState = DbConnState.Free;
        }

        //测试连接操作方法 OK
        public bool TestConnection(string connStr)
        {
            bool rbc = false;
            DbConnection TestDbConn = null;
            try
            {
                TestDbConn=this.GetDbConnection();
                TestDbConn.ConnectionString = connStr;
                TestDbConn.Open();
                if (TestDbConn.State == ConnectionState.Open)
                {
                    rbc = true;
                }
            }
            catch
            {
            }
            finally
            {
                if (TestDbConn != null)
                {
                    TestDbConn.Close();
                    TestDbConn.Dispose();
                    TestDbConn = null;
                }
            }
            
            return rbc;
        }

        //测试连接操作方法 OK
        public bool TestConnection()
        {
            return this.TestConnection(this.GetConnectionString);
        }
        

        //获取连接对象
        public DbConnection GetDbConn()
        {
            if (m_DbConnState == DbConnState.Free)
            {
                return m_dbconn;
            }
            return null;
        }
        public void SetDbConnState(DbConnState pDbConnState)
        {
            this.m_DbConnState = pDbConnState;
        }
        public DbConnState GetDbConnState
        {
            get
            {
                return this.m_DbConnState;
            }
        }

        //对数据库的操作
        public string GetConnectionString
        {
            get
            {
                return m_ConnectionString;
            }
        }

        //抽象方法        
        public abstract DbDataAdapter GetDbDataAdapter();
        public abstract DbDataAdapter GetDbDataAdapter(DbCommand dbCommand);
        public abstract DbCommand GetDbCommand();
        public abstract DbConnection GetDbConnection();
        public abstract DbCommandBuilder GetDbCommandBuilder();
        //
        public abstract char ParameterChar { get; }
        //
        public abstract DbParameter CreateParameter(string name);
        public abstract DbParameter CreateParameter(string name, object value);
        public abstract DbParameter CreateParameter(string name, DbType dbtype, object value);
        public abstract DbParameter CreateParameter(string name, DbType dbtype,int size, object value);
        //
        public abstract DataProviderType GetCurrentDataProviderType();
        //        
        public abstract bool IsExistsField(string FieldName, string TableName);
        public abstract bool IsExistsTable(string TableName, string UserName);
        public abstract bool IsExistsTable(string TableName);

        //保护方法
        protected void PrepareCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            if (m_dbconn.State != ConnectionState.Open)
            {
                m_dbconn.Open();
            }
            if (m_cmd == null)
            {
                m_cmd = this.GetDbCommand();
            }

            m_cmd.Connection = m_dbconn;
            m_cmd.CommandText = commandText;
            m_cmd.CommandType = commandType;

            if (m_transaction != null)
            {
                m_cmd.Transaction = m_transaction;
            }

            if (commandParameters != null)
            {
                this.m_cmd.Parameters.Clear();
               
                foreach (IDataParameter param in commandParameters)
                {
                    ///////////////yongtao add 2010-08-23///////////////
                    if (param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    ///////////////////////////////////////////////////
                    m_cmd.Parameters.Add(param);
                }
            }
        }
        //带事务 ,DbConnection dbconn, DbTransaction dbtrans
        protected DbCommand PrepareCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters, DbConnection dbconn, DbTransaction dbtrans)
        {
            if (dbconn.State != ConnectionState.Open)
            {
                dbconn.Open();
            }
            DbCommand tmpcmd = this.GetDbCommand();

            tmpcmd.Connection = dbconn;
            tmpcmd.CommandText = commandText;
            tmpcmd.CommandType = commandType;
            if (dbtrans != null)
            {
                tmpcmd.Transaction = dbtrans;
            }
            if (commandParameters != null)
            {
                tmpcmd.Parameters.Clear();
                foreach (IDataParameter param in commandParameters)
                {
                    ///////////////yongtao add 2010-08-23///////////////
                    if (param.Value == null)
                    {
                        param.Value = DBNull.Value;
                    }
                    ///////////////////////////////////////////////////
                    tmpcmd.Parameters.Add(param);
                }
            }
            return tmpcmd;
        }

        //公共方法
        // Generic methods implementation
        public string ADODBConnectionString = "";

        #region Database Transaction

        //获取内部事务对象
        public DbTransaction Get_InnerTransaction()
        {
            return m_transaction;
        }

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (m_transaction != null)
            {
                return;
            }

            try
            {
                // open connection
                if (m_dbconn.State != ConnectionState.Open)
                {
                    m_dbconn.Open();
                }
                // begin a database transaction with a read committed isolation level
                m_transaction = m_dbconn.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                m_dbconn.Close();
                throw;
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (m_transaction == null)
                return;

            try
            {
                // Commit transaction
                m_transaction.Commit();
            }
            catch
            {
                // rollback transaction
                RollbackTransaction();
                throw;
            }
            finally
            {
                m_transaction = null;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            if (m_transaction == null)
                return;

            try
            {
                m_transaction.Rollback();
            }
            catch { }
            finally
            {
                m_transaction = null;
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return this.ExecuteDataReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            IDataReader dr = null;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                if (m_transaction == null)
                    // Generate the reader. CommandBehavior.CloseConnection causes the
                    // the connection to be closed when the reader object is closed
                    dr = m_cmd.ExecuteReader(CommandBehavior.CloseConnection);
                else
                    dr = m_cmd.ExecuteReader();

                
            }
            catch(Exception ee)
            {
                if (m_transaction != null)
                {
                    RollbackTransaction();
                }
                if(m_cmd!=null)
                {
                    m_cmd.Dispose();
                }
                throw ee;
            }
            finally
            {
                this.SetDbConnState(DbConnState.Free);
            }
            return dr;
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, null);
        }


        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return this.ExecuteDataSet(commandText, commandType, null);
        }

        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return this.ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DataSet ds = null;
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                //create the DataAdapter & DataSet
                DbDataAdapter da = this.GetDbDataAdapter();
                da.SelectCommand = m_cmd;
                ds = new DataSet();

                //fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds);
                                
                //return the dataset
                return ds;
            }
            catch (Exception ee)
            {
                if (m_transaction != null)
                {
                    RollbackTransaction();
                }
                ee.HelpLink = commandText;
                ds= null;
                throw ee;
            }
            finally
            {
                if (commandParameters != null)
                {
                    this.m_cmd.Parameters.Clear();
                }
                this.SetDbConnState(DbConnState.Free);
            }
            return ds;
        }

        /// <summary>
        ///  获取数据集By执行带参数的存储过程 
        /// </summary>
        public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            IDbCommand cmd = this.m_cmd;
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            if (cmd.Connection == null)
            {
                cmd.Connection = this.m_dbconn;
            }
            for (int i = 0; i < parameterValues.Length; i++)
            {
                object obj = parameterValues[i];

                if (obj == null)
                {
                    obj = string.Empty;
                }

                DbParameter para = null;

                if (this.ParameterChar != '@')
                {
                    para = this.CreateParameter(this.ParameterChar + "p" + (i + 1).ToString(), obj);
                }
                else
                {
                    para = this.CreateParameter("p" + (i + 1).ToString(), obj);
                }

                cmd.Parameters.Add(para);
            }

            DbDataAdapter da = this.GetDbDataAdapter(cmd as DbCommand);
            DataSet ds = new DataSet();
            da.Fill(ds);

            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;

            this.SetDbConnState(DbConnState.Free);
            return ds;
        }
        #endregion

        #region ExecuteQuery

        public int ExecuteQuery(DbCommand command)
        {
            return this.ExecuteQuery(command.CommandText, command.CommandType);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, null);
        }
        //不带事务 ,DbConnection dbconn, DbTransaction dbtrans
        public int ExecuteQuery(string commandText,DbConnection dbconn, DbTransaction dbtrans)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, null,dbconn,dbtrans);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteQuery(commandText, commandType, null);
        }
        public int ExecuteQuery(CommandType commandType,string commandText)
        {
            return this.ExecuteQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes an SQL parameterized statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                int intAffectedRows = m_cmd.ExecuteNonQuery();
                // return no of affected records
                               
                return intAffectedRows;
            }
            catch (Exception ex)
            {
                if (m_transaction != null)
                {
                    RollbackTransaction();
                }
                ex.HelpLink = commandText;
                throw ex;
            }
            finally
            {
                if (commandParameters != null)
                {
                    this.m_cmd.Parameters.Clear();
                }
                this.SetDbConnState(DbConnState.Free);
            }
        }

        //带事务 ,DbConnection dbconn, DbTransaction dbtrans
        public int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters, DbConnection dbconn, DbTransaction dbtrans)
        {
            DbCommand tmpCmd = null;
            try
            {
                tmpCmd=PrepareCommand(commandType, commandText, commandParameters,dbconn,dbtrans);

                // execute command
                int intAffectedRows = tmpCmd.ExecuteNonQuery();
                // return no of affected records
                
                return intAffectedRows;
            }
            catch (Exception ex)
            {                
                ex.HelpLink = commandText;
                throw ex;
            }
            finally
            {
                if (tmpCmd != null)
                {
                    tmpCmd.Parameters.Clear();
                }
                this.SetDbConnState(DbConnState.Free);
            }
        }

        //
        public int ExecuteQuery(string storedProcedureName, params object[] parameterValues)
        {
            IDbCommand cmd = this.m_cmd;
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            if (cmd.Connection == null)
            {
                cmd.Connection = this.m_dbconn;
            }
            for (int i = 0; i < parameterValues.Length; i++)
            {
                object obj = parameterValues[i];

                if (obj == null)
                {
                    obj = string.Empty;
                }

                DbParameter para = null;

                if (this.ParameterChar != '@')
                {
                    para = this.CreateParameter(this.ParameterChar + "p" + (i + 1).ToString(), obj);
                }
                else
                {
                    para = this.CreateParameter("p" + (i + 1).ToString(), obj);
                }

                cmd.Parameters.Add(para);
            }

            int ret=cmd.ExecuteNonQuery();
            
            cmd.Parameters.Clear();
            cmd.CommandType = CommandType.Text;

            this.SetDbConnState(DbConnState.Free);
            return ret;
        }
                
        #endregion

        public DataTable ExecuteDataTable(string pSQL)
        {
            DataTable dt = null;
            DataSet ds = this.ExecuteDataSet(pSQL);
            if (ds != null && ds.Tables.Count>0)
            {
                dt = ds.Tables[0];
            }
            ds = null;
            return dt;
        }

        public virtual DataTable GetSchemeTables()
        {
            DataTable dt = null;
            if (m_dbconn.State != ConnectionState.Open)
            {
                m_dbconn.Open();
            }
            dt = this.m_dbconn.GetSchema("Tables", new string[] { null, null, null, "TABLE" });
            return dt;
        }
        
        //更新BLOB字段值
        public bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereEqFieldName, string WhereEqFdValue)
        {
            bool rbc = false;            
            DbConnection dbconn = null;
            DbTransaction dbtrans = null;
            DbCommand dbCmd1 = null;
            try
            {
                dbconn =this.GetDbConnection();//
                dbconn.ConnectionString = this.GetConnectionString;
                if (dbconn.State != ConnectionState.Open)
                {
                    dbconn.Open();
                }
                dbtrans = dbconn.BeginTransaction();

                string PrixChar = this.ParameterChar.ToString();// PrixChar;
                string x = "update " + UpdateTableName + " set " + UpdateBLOBFieldName + "=" + PrixChar + "Img where " + WhereEqFieldName + "=" + PrixChar + "tmpguid ";
                byte[] byteArray = UpdateBLOBFdValue as byte[];
                //
                dbCmd1 = dbconn.CreateCommand();
                dbCmd1.CommandText = x;
                dbCmd1.CommandType = CommandType.Text;
                dbCmd1.Connection = dbconn;
                dbCmd1.Transaction = dbtrans;
                //
                DbParameter dbparam = this.CreateParameter("" + PrixChar + "Img", byteArray);
                dbparam.Direction = ParameterDirection.Input;
                dbparam.DbType = System.Data.DbType.Binary;
                dbparam.Value = byteArray;
                dbparam.SourceColumn = UpdateBLOBFieldName;
                dbCmd1.Parameters.Add(dbparam);
                dbCmd1.Parameters.Add(this.CreateParameter("" + PrixChar + "tmpguid", WhereEqFdValue));
                if (dbCmd1.ExecuteNonQuery() > 0)
                {
                    rbc = true;
                    dbtrans.Commit();
                }
            }
            catch (Exception ee)
            {
                if (dbtrans != null)
                {
                    dbtrans.Rollback();
                }                
                rbc = false;
            }
            finally
            {
                if (dbCmd1 != null)
                {
                    dbCmd1.Dispose();
                    dbCmd1 = null;
                }
                //----
                if (dbtrans != null)
                {
                    dbtrans.Dispose();
                    dbtrans = null;
                }
                if (dbconn != null)
                {
                    dbconn.Dispose();
                    dbconn = null;
                }
            }
            return rbc;
        }

        //更新BLOB字段值
        public bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereClause)
        {
            bool rbc = false;
            DbConnection dbconn = null;
            DbTransaction dbtrans = null;
            DbCommand dbCmd1 = null;
            try
            {
                dbconn = this.GetDbConnection();//
                dbconn.ConnectionString = this.GetConnectionString;
                if (dbconn.State != ConnectionState.Open)
                {
                    dbconn.Open();
                }
                dbtrans = dbconn.BeginTransaction();

                string PrixChar = this.ParameterChar.ToString();// PrixChar;
                string x = "update " + UpdateTableName + " set " + UpdateBLOBFieldName + "=" + PrixChar + "Img where " + WhereClause;
                byte[] byteArray = UpdateBLOBFdValue as byte[];
                //
                dbCmd1 = dbconn.CreateCommand();
                dbCmd1.CommandText = x;
                dbCmd1.CommandType = CommandType.Text;
                dbCmd1.Connection = dbconn;
                dbCmd1.Transaction = dbtrans;
                //
                DbParameter dbparam = this.CreateParameter("" + PrixChar + "Img", byteArray);
                dbparam.Direction = ParameterDirection.Input;
                dbparam.DbType = System.Data.DbType.Binary;
                dbparam.Value = byteArray;
                dbparam.SourceColumn = UpdateBLOBFieldName;
                dbCmd1.Parameters.Add(dbparam);
                if (dbCmd1.ExecuteNonQuery() > 0)
                {
                    rbc = true;
                    dbtrans.Commit();
                }
            }
            catch (Exception ee)
            {
                if (dbtrans != null)
                {
                    dbtrans.Rollback();
                }
                rbc = false;
            }
            finally
            {
                if (dbCmd1 != null)
                {
                    dbCmd1.Dispose();
                    dbCmd1 = null;
                }
                //----
                if (dbtrans != null)
                {
                    dbtrans.Dispose();
                    dbtrans = null;
                }
                if (dbconn != null)
                {
                    dbconn.Dispose();
                    dbconn = null;
                }
            }
            return rbc;
        }
        
        
        //更新BLOB字段值 （内部不带事务)需外部处理事务
        public bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereEqFieldName, string WhereEqFdValue, DbConnection dbconn, DbTransaction dbtrans)
        {
            bool rbc = false;            
            DbCommand dbCmd1 = null;
            try
            {                
                if (dbconn.State != ConnectionState.Open)
                {
                    dbconn.Open();
                }
                string PrixChar = this.ParameterChar.ToString();// PrixChar;
                string x = "update " + UpdateTableName + " set " + UpdateBLOBFieldName + "=" + PrixChar + "Img where " + WhereEqFieldName + "=" + PrixChar + "tmpguid ";
                byte[] byteArray = UpdateBLOBFdValue as byte[];
                //
                dbCmd1 = dbconn.CreateCommand();
                dbCmd1.CommandText = x;
                dbCmd1.CommandType = CommandType.Text;
                dbCmd1.Connection = dbconn;
                dbCmd1.Transaction = dbtrans;
                //
                DbParameter dbparam = this.CreateParameter("" + PrixChar + "Img", byteArray);
                dbparam.Direction = ParameterDirection.Input;
                dbparam.DbType = System.Data.DbType.Binary;
                dbparam.Value = byteArray;
                dbparam.SourceColumn = UpdateBLOBFieldName;
                dbCmd1.Parameters.Add(dbparam);
                dbCmd1.Parameters.Add(this.CreateParameter("" + PrixChar + "tmpguid", WhereEqFdValue));
                if (dbCmd1.ExecuteNonQuery() > 0)
                {
                    rbc = true;                    
                }
            }
            catch (Exception ee)
            {
                rbc = false;
                throw ee;                
            }
            finally
            {
                if (dbCmd1 != null)
                {
                    dbCmd1.Dispose();
                    dbCmd1 = null;
                }
                //----                
            }
            return rbc;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.SQLSelecterDataTable != null)
            {
                this.SQLSelecterDataTable.Dispose();
                this.SQLSelecterDataTable = null;
            }
            if (m_dbconn != null)
            {
                m_dbconn.Close();
                m_dbconn.Dispose();
                m_dbconn = null;
            }
            SetDbConnState(DbConnState.Free);
        }

        #endregion
        
        #region ISQLSelecter 成员
        //vp:hsg date:2006-12-05
        private string mSQL = "";
        public string SQL
        {
            get
            {
                return mSQL;
            }
            set
            {
                mSQL = value;
            }
        }
        private DataTable SQLSelecterDataTable = null;
        public virtual void Execute()
        {
            this.SQLSelecterDataTable = null;
            DataSet ds = this.ExecuteDataSet(this.SQL);
            if (ds != null)
            {
                SQLSelecterDataTable = ds.Tables[0];
            }
            ds = null;
        }

        public string getFieldValue(string FieldName)
        {
            string rbc = "";
            if (FieldName != "")
            {
                try
                {
                    if (this.SQLSelecterDataTable != null)
                    {
                        rbc = this.SQLSelecterDataTable.Rows[0][FieldName].ToString();
                    }
                    else
                    {
                        rbc = "";
                    }
                }
                catch
                {
                    rbc = "";
                }
            }
            return rbc;
        }

        

        #endregion

        #region IDbConnectionWrapper 成员
        public bool Open()
        {
            bool rbc = false;
            if (this.m_dbconn != null)
            {
                if (this.m_dbconn.State == ConnectionState.Closed)
                {
                    this.m_dbconn.Open();
                }
                if (this.m_dbconn.State == ConnectionState.Broken)
                {
                    this.m_dbconn.Close();
                    this.m_dbconn.Open();
                }
                rbc = true;
            }
            return rbc;
        }
        public bool Close()
        {
            bool rbc = false;
            if (this.m_dbconn != null)
            {
                if (this.m_dbconn.State != ConnectionState.Closed)
                {
                    this.m_dbconn.Close();
                }
                rbc = true;
            }
            return rbc;
        }
        public virtual string get_table_fd_val(string tablename, string wherestr, string getval_fdname)
        {
            string rbc = "";
            string x = "select " + getval_fdname + " from " + tablename + " " + wherestr;
            DataTable dt = this.ExecuteDataTable(x);
            if (dt != null && dt.Rows.Count > 0)
            {
                rbc = dt.Rows[0][getval_fdname].ToString();
            }
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            return rbc;
        }
        //获取最大值
        public virtual long GetMaxValue_1(string tableName, string MaxFieldName)
        {
            long rbc = 0;
            rbc = GetMaxValue(tableName, MaxFieldName, "");
            rbc += 1;
            return rbc;
        }
        public virtual long GetMaxValue_1(string tableName, string MaxFieldName, string whereStr)
        {
            long rbc = 0;
            rbc = GetMaxValue(tableName, MaxFieldName, whereStr);
            rbc += 1;
            return rbc;
        }
        public virtual long GetMaxValue(string tableName, string MaxFieldName)
        {
            return GetMaxValue(tableName, MaxFieldName, "");
        }
        public virtual long GetMaxValue(string tableName, string MaxFieldName, string whereStr)
        {
            long rbc = 0;
            string x = "select max(" + MaxFieldName + ") as fd_max_value from " + tableName + " " + whereStr;
            DataTable dt = this.ExecuteDataTable(x);
            if (dt != null && dt.Rows.Count > 0)
            {
                rbc = this.TLng(dt.Rows[0]["fd_max_value"]);
            }
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            return rbc;
        }
        //获取记录个数
        public virtual long GetRowCount(string tableName, string CountFieldName, string whereStr)
        {
            long rbc = 0;
            string x = "select count("+CountFieldName+") as fdrow_count from " + tableName + " " + whereStr;
            DataTable dt = this.ExecuteDataTable(x);
            if (dt != null && dt.Rows.Count > 0)
            {
                rbc = this.TInt(dt.Rows[0]["fdrow_count"]);
            }
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            return rbc;
        }
        public virtual decimal GetSumValue(string tableName, string sumFieldName, string whereStr)
        {
            decimal rbc = 0;
            string x = "select sum(" + sumFieldName + ") as fd_sum_value from " + tableName + " " + whereStr;
            DataTable dt = this.ExecuteDataTable(x);
            if (dt != null && dt.Rows.Count > 0)
            {
                rbc = this.TDec(dt.Rows[0]["fd_sum_value"]);
            }
            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            return rbc;
        }
        //
        protected long TLng(object WantToDoubleNumber)
        {
            #region 实现细节
            long Rbc = 0;
            try
            {
                Rbc = long.Parse(WantToDoubleNumber.ToString());
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }
        protected int TInt(object WantToDoubleNumber)
        {
            #region 实现细节
            int Rbc = 0;
            try
            {
                if (int.TryParse(WantToDoubleNumber.ToString(), out Rbc) == false)
                {
                    Rbc = 0;
                }
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }
        protected decimal TDec(object WantToDoubleNumber)
        {
            #region 实现细节
            decimal Rbc = 0;
            try
            {
                Rbc = System.Decimal.Parse(WantToDoubleNumber.ToString());
            }
            catch
            {
                Rbc = 0;
            }
            return Rbc;
            #endregion
        }
        private DateTime ToDateTime(string srcStr)
        {
            srcStr = srcStr.Replace("/", "-");
            srcStr = srcStr.Replace("\\", "-");
            #region 实现细节
            DateTime dt = System.DateTime.Now;
            if (srcStr == "")
            {
                return dt;
            }
            try
            {
                bool rbc = DateTime.TryParse(srcStr, out dt);
                if (rbc == false)
                {
                    dt = System.DateTime.Now;
                }
            }
            catch (Exception ee)
            {
                System.Diagnostics.Debug.WriteLine(ee.ToString());
            }
            return dt;
            #endregion
        }
        public virtual DateTime getDbSysDateTime()
        {
            DateTime date = DateTime.Now;
            string sys_now = this.Syntax_Now();
            string dual = this.Syntax_Dual();
            string x = "select " + sys_now + "";
            if (dual.Trim() != "")
            {
                x += " " + dual;
            }
            DataTable dt = this.ExecuteDataTable(x);
            if (dt != null && dt.Rows.Count > 0)
            {
                string s = dt.Rows[0][0].ToString();                
                date = this.ToDateTime(s);
            }
            return date;
        }
        #endregion
        //
        #region 参数法 插入修改删除功能的函数
        public int Insert(string tableName, DBParameterCollection paras)
        {
            return this.Insert(null, tableName, paras);
        }
        public int Insert(DbTransaction trans, string tableName, DBParameterCollection paras)
        {
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("参数空异常");
            }
            string sql = "insert into " + tableName + " (";
            foreach (DBParameter para in paras)
            {
                sql += para.FieldName + ",";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql += ") values (";
            foreach (DBParameter para in paras)
            {
                sql += this.ParameterChar + para.FieldName + ",";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            sql += ")";

            DbCommand t_cmd = this.GetDbCommand();
            t_cmd.CommandText = sql;
            t_cmd.CommandType = CommandType.Text;
            t_cmd.Connection = this.m_dbconn;
            foreach (DBParameter para in paras)
            {
                DbParameter dbpara = this.CreateParameter(para.FieldName, para.FieldType, para.FieldValue);
                t_cmd.Parameters.Add(dbpara);
            }
            if (trans != null)
            {
                t_cmd.Transaction = trans;
                return t_cmd.ExecuteNonQuery();
            }
            else
            {
                return t_cmd.ExecuteNonQuery();
            }
        }

        public int Update(string tableName, DBParameterCollection paras, int whereStart)
        {
            return this.Update(null, tableName, paras, whereStart);
        }

        public int Update(DbTransaction trans, string tableName, DBParameterCollection paras, int whereStart)
        {
            if (paras == null || paras.Count <= 0)
            {
                throw new ArgumentNullException("参数空异常");
            }

            string sql = "update " + tableName + " set ";

            for (int i = 0; i < whereStart; i++)
            {
                sql += paras[i].FieldName + " = " + this.ParameterChar + "" + paras[i].FieldName + ",";
            }
            sql = sql.Remove(sql.Length - 1, 1);
            if (whereStart > 0 && whereStart < paras.Count)
            {
                sql += " where ";
                for (int j = whereStart; j < paras.Count; j++)
                {
                    sql += paras[j].FieldName + " = " + this.ParameterChar + "" + paras[j].FieldName + " and ";
                }
                sql = sql.Remove(sql.Length - 5, 5);
            }
            //
            DbCommand t_cmd = this.GetDbCommand();
            t_cmd.CommandText = sql;
            t_cmd.CommandType = CommandType.Text;
            t_cmd.Connection = this.m_dbconn;
            foreach (DBParameter para in paras)
            {
                DbParameter dbpara = this.CreateParameter(para.FieldName, para.FieldType, para.FieldValue);
                t_cmd.Parameters.Add(dbpara);
            }
            if (trans != null)
            {
                t_cmd.Transaction = trans;
                return t_cmd.ExecuteNonQuery();
            }
            else
            {
                return t_cmd.ExecuteNonQuery();
            }
        }

        public int Delete(string tableName, DBParameterCollection paras)
        {
            return this.Delete(null, tableName, paras);
        }

        public int Delete(DbTransaction trans, string tableName, DBParameterCollection paras)
        {
            string sql = "delete * from " + tableName + " ";
            if (paras != null && paras.Count > 0)
            {
                sql += "where ";
                foreach (DBParameter para in paras)
                {
                    sql += para.FieldName + " = " + this.ParameterChar + para.FieldName + " and ";
                }
                sql.Remove(sql.Length - 5, 5);

                DbCommand t_cmd = this.GetDbCommand();
                t_cmd.CommandText = sql;
                t_cmd.CommandType = CommandType.Text;
                t_cmd.Connection = this.m_dbconn;
                foreach (DBParameter para in paras)
                {
                    DbParameter dbpara = this.CreateParameter(para.FieldName, para.FieldType, para.FieldValue);
                    t_cmd.Parameters.Add(dbpara);
                }
                if (trans != null)
                {
                    t_cmd.Transaction = trans;
                    return t_cmd.ExecuteNonQuery();
                }
                else
                {
                    return t_cmd.ExecuteNonQuery();
                }
            }
            else
            {
                DbCommand t_cmd = this.GetDbCommand();
                t_cmd.CommandText = sql;
                t_cmd.CommandType = CommandType.Text;
                t_cmd.Connection = this.m_dbconn;
                if (trans != null)
                {
                    t_cmd.Transaction = trans;
                    return t_cmd.ExecuteNonQuery();
                }
                else
                {
                    return t_cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
        //
        #region SQLSyntax 抽象函数
        public abstract string Syntax_SubString(string express, string pos, string length);
        public abstract string Syntax_ToDate(string express, string format);
        public abstract string Syntax_ToNumber(string express);
        public abstract string Syntax_ToChar(string express, string format);
        public abstract string Syntax_Length(string express);
        public abstract string Syntax_Mod(string express, string n2);
        public abstract string Syntax_Now();
        public abstract string Syntax_Year(string express);
        public abstract string Syntax_DateAdd(string express, int day);
        public abstract string Syntax_Dual();
        #endregion
        //
        
        public string GetDB_UserOfConStr()
        {
            string rbc = "";
            string s = this.GetConnectionString;            
            string[] sArray = s.Split(';');
            string DataSource = "", UserID = "";
            foreach (string tmpStr in sArray)
            {
                string[] tmpArray = tmpStr.Split('=');
                if (tmpArray.Length == 2)
                {
                    string key = tmpArray[0].ToUpper();
                    key = key.Replace(" ", "");
                    string value = tmpArray[1];
                    switch (key)
                    {
                        case "DATASOURCE": //DataSource
                        case "DATA SOURCE": //DataSource
                        case "DATABASE":
                        case "INITIAL CATALOG":
                        case "INITIALCATALOG":
                            DataSource = value;
                            DataSource = value;
                            break;
                        case "USERID":     //User ID
                        case "USER ID":     //User ID
                        case "UID":
                            UserID = value;
                            break;
                        default:
                            break;
                    }
                }
            }
            switch (this.GetCurrentDataProviderType())
            {
                case DataProviderType.Oracle:
                    rbc = UserID;
                    break;
                default:
                    rbc = DataSource;
                    break;
            }
            return rbc;
        }
        /// <summary>
        /// 数据库中与C#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ChangeToCSharpType(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "int":
                    reval = "Int32";                    
                    break;
                case "text":
                    reval = "String";
                    break;
                case "bigint":
                    reval = "Int64";                    
                    break;
                case "binary":
                    reval = "Byte[]";
                    break;
                case "bit":
                    reval = "Boolean";                    
                    break;
                case "char":
                    reval = "String";
                    break;
                case "datetime":
                    reval = "DateTime";
                    break;
                case "date":
                    reval = "DateTime";
                    break;
                case "decimal":
                    reval = "Decimal";
                    break;
                case "float":
                    reval = "Double";                    
                    break;
                case "image":
                    reval = "Byte[]";
                    break;
                case "money":
                    reval = "Decimal";
                    break;
                case "nchar":
                    reval = "String";
                    break;
                case "ntext":
                    reval = "String";
                    break;
                case "numeric":
                    reval = "Decimal";
                    break;
                case "nvarchar":
                    reval = "String";
                    break;
                case "real":
                    reval = "Single";                    
                    break;
                case "smalldatetime":
                    reval = "DateTime";
                    break;
                case "smallint":
                    reval = "Int16";                    
                    break;
                case "smallmoney":
                    reval = "Decimal";
                    break;
                case "timestamp":
                    reval = "DateTime";
                    break;
                case "tinyint":
                    reval = "Byte";
                    break;
                case "uniqueidentifier":
                    reval = "Guid";
                    break;
                case "varbinary":
                    reval = "Byte[]";
                    break;
                case "varchar":
                    reval = "String";
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = "String";
                    break;
            }
            return reval;
        }
        //获取数据库中表名列表
        public virtual List<string> GetDataBaseTableNameList()
        {
            List<string> tList = new List<string>();
            tList.Clear();
            return tList;
        }
        public virtual List<string> GetDataBaseTableNameList(string db_user_name)
        {
            List<string> tList = new List<string>();
            tList.Clear();
            return tList;
        }
        public virtual  List<string> GetDataBaseList()
        {
            List<string> tList = new List<string>();
            tList.Clear();
            return tList;
        }         
        //
        public virtual List<DbTableCols> GetDataBaseTableCols(string tableName)
        {
            List<DbTableCols> tList = new List<DbTableCols>();
            tList.Clear();
            return tList;
        }
        public virtual List<string> GetPKeyColList(string tableName)
        {
            List<string> tList = new List<string>();
            tList.Clear();
            return tList;
        }
        //
        public virtual DataTable GetDataBaseDt()
        {
            return null;
        }
        public virtual DataTable GetDataBaseTableNameDt()
        {
            return null;
        }
        public virtual DataTable GetDataBaseTableNameDt(string db_user_name)
        {
            return null;
        }
        public virtual DataTable GetDataBaseTableColsDt(string tableName)
        {
            return null;
        }
        public virtual DataTable GetPKeyColDt(string tableName)
        {
            return null;
        }
        //
    }


    //vp:hsg define interface
    public interface ISQLSelecter
    {
        string SQL { get; set; }
        void Execute();
        string getFieldValue(string FieldName);
        void Dispose();
    }
}
