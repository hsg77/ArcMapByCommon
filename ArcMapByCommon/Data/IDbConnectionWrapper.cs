using System;
//
using System.Data.Common;
using System.Data;
using System.Collections.Generic;

namespace ArcMapByCommon
{
    /// <summary>
    /// 数据访问接口
    /// vp:hsg
    /// create date:2012-08-23
    /// </summary>
    public interface IDbConnectionWrapper
    {
        bool Open();
        bool Close();
        //
        DbTransaction Get_InnerTransaction();
        void BeginTransaction();
        void CommitTransaction();
        DbParameter CreateParameter(string name, object value);
        DbParameter CreateParameter(string name, DbType dbtype, int size, object value);
        DbParameter CreateParameter(string name, DbType dbtype, object value);
        DbParameter CreateParameter(string name);
        void Dispose();
        void Execute();
        IDataReader ExecuteDataReader(string commandText, CommandType commandType);
        IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters);
        IDataReader ExecuteDataReader(string commandText);
        IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(string commandText, CommandType commandType);
        DataSet ExecuteDataSet(string commandText);
        DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] commandParameters);
        DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters);
        DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues);
        DataTable ExecuteDataTable(string pSQL);
        int ExecuteQuery(CommandType commandType, string commandText);
        int ExecuteQuery(string commandText);
        int ExecuteQuery(DbCommand command);
        int ExecuteQuery(string commandText, DbConnection dbconn, DbTransaction dbtrans);
        int ExecuteQuery(string commandText, CommandType commandType);
        int ExecuteQuery(string commandText, IDataParameter[] commandParameters);
        int ExecuteQuery(string storedProcedureName, params object[] parameterValues);
        int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters, DbConnection dbconn, DbTransaction dbtrans);
        int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters);
        string GetConnectionString { get; }
        DataProviderType GetCurrentDataProviderType();
        DbCommand GetDbCommand();
        DbCommandBuilder GetDbCommandBuilder();
        DbConnection GetDbConn();
        DbConnection GetDbConnection();
        DbConnState GetDbConnState { get; }
        DbDataAdapter GetDbDataAdapter(DbCommand dbCommand);
        DbDataAdapter GetDbDataAdapter();
        string getFieldValue(string FieldName);
        DataTable GetSchemeTables();
        bool IsExistsField(string FieldName, string TableName);
        bool IsExistsTable(string TableName, string UserName);
        bool IsExistsTable(string TableName);
        char ParameterChar { get; }
        void RollbackTransaction();
        void SetDbConnState(DbConnState pDbConnState);
        string SQL { get; set; }
        bool TestConnection();
        bool TestConnection(string connStr);
        bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereEqFieldName, string WhereEqFdValue);
        bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereEqFieldName, string WhereEqFdValue, DbConnection dbconn, DbTransaction dbtrans);
        bool UpdateBLOBFieldValue(string UpdateTableName, string UpdateBLOBFieldName, object UpdateBLOBFdValue, string WhereClause);
        //==数据库操作通用应用函数
        string get_table_fd_val(string tablename, string wherestr, string getval_fdname);
        long GetMaxValue_1(string tableName, string MaxFieldName);
        long GetMaxValue_1(string tableName, string MaxFieldName, string whereStr);
        long GetMaxValue(string tableName, string MaxFieldName);
        long GetMaxValue(string tableName, string MaxFieldName, string whereStr);
        long GetRowCount(string tableName, string CountFieldName, string whereStr);
        decimal GetSumValue(string tableName, string sumFieldName, string whereStr);
        //
        DateTime getDbSysDateTime();
        //==
        /// <summary>
        /// 执行插入操作
        /// </summary>
        /// <param name="tableName">要插入数据的表名</param>
        /// <param name="paras">参数集合</param>
        /// <returns>受影响行数</returns>
        int Insert(string tableName, DBParameterCollection paras);
        int Insert(DbTransaction trans, string tableName, DBParameterCollection paras);

        /// <summary>
        /// 执行更新操作
        /// </summary>
        /// <param name="tableName">要更新的表名</param>
        /// <param name="paras">参数集合</param>
        /// <param name="whereStart">从第几个参数开始为where条件中的参数,paras[whereStart]开始</param>
        /// <returns>受影响行数</returns>
        int Update(string tableName, DBParameterCollection paras, int whereStart);
        int Update(DbTransaction trans, string tableName, DBParameterCollection paras, int whereStart);

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="tableName">要删除数据的表名</param>
        /// <param name="paras">参数集合</param>
        /// <returns>受影响行数</returns>
        int Delete(string tableName, DBParameterCollection paras);
        int Delete(DbTransaction trans, string tableName, DBParameterCollection paras);
        //
        //SQLSyntax函数
        string Syntax_SubString(string express, string pos, string length);
        string Syntax_ToDate(string express, string format);
        string Syntax_ToNumber(string express);
        string Syntax_ToChar(string express, string format);
        string Syntax_Length(string express);
        string Syntax_Mod(string express, string n2);
        string Syntax_Now();
        string Syntax_Year(string express);
        string Syntax_DateAdd(string express, int day); 
        string Syntax_Dual();
        //
        string GetDB_UserOfConStr();
        //
        List<string> GetDataBaseList();
        List<string> GetDataBaseTableNameList();
        List<string> GetDataBaseTableNameList(string db_user_name);
        List<DbTableCols> GetDataBaseTableCols(string tableName);
        List<string> GetPKeyColList(string tableName);
        //
        DataTable GetDataBaseDt();
        DataTable GetDataBaseTableNameDt();
        DataTable GetDataBaseTableNameDt(string db_user_name);
        DataTable GetDataBaseTableColsDt(string tableName);
        DataTable GetPKeyColDt(string tableName);
        //
    }

    [Serializable]
    public enum DataProviderType : int
    {
        Access = 1,
        Odbc = 2,
        OleDb = 3,
        Oracle = 4,
        Sql = 5,
        //
        SQLite = 6,      //新增数据库类型
        MySQL = 7,       //新增数据库类型
        PostgreSQL = 8,    //新增数据库类型
    }
    [Serializable]
    public enum DbConnState
    {
        Using = 1,
        Free = 2,
    }
}
