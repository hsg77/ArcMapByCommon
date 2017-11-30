using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;

namespace ArcMapByCommon
{
    /// <summary>
    /// mdb Access数据库数据访问层
    /// </summary>
    [Serializable]
    public class DbConnectionWrapper_mdb: DbConnectionWrapper
    {
        public DbConnectionWrapper_mdb(string pConnectionString)
            : base(pConnectionString)
        {
            this.m_dbconn = new OleDbConnection(pConnectionString);
            this.m_DbConnState = DbConnState.Free;
        }

        public override DbDataAdapter GetDbDataAdapter()
        {
            return new OleDbDataAdapter();
        }
        public override DbDataAdapter GetDbDataAdapter(DbCommand dbCommand)
        {
            return new OleDbDataAdapter(dbCommand as OleDbCommand);
        }
        public override DbCommand GetDbCommand()
        {
            return new OleDbCommand();
        }
        public override DbConnection GetDbConnection()
        {
            return new OleDbConnection();
        }
        public override DbCommandBuilder GetDbCommandBuilder()
        {
            return new OleDbCommandBuilder();
        }
        public override DataProviderType GetCurrentDataProviderType()
        {
            return DataProviderType.Access;
        }

        public override bool IsExistsTable(string TableName, string UserName)
        {
            #region information
            bool rbc = false;
            //note:OleDB Access MDB
            //string dSql = "select * from MSysObjects where name='" + TableName + "'";
            string dSql = "select * from " + TableName + " where 1<>1";
            DataSet ds = null;
            try
            {
                ds = this.ExecuteDataSet(dSql);
                if (ds != null && ds.Tables.Count > 0)
                {
                    rbc = true;
                }
                else
                {
                    rbc = false;
                }
            }
            catch
            {
                rbc = false;
            }
            finally
            {
                ds = null;
            }

            return rbc;
            #endregion
        }
        public override bool IsExistsTable(string TableName)
        {
            return IsExistsTable(TableName, this.GetDB_UserOfConStr());
        }

        public override bool IsExistsField(string FieldName, string TableName)
        {
            #region information
            bool rbc = false;
            string dSql = "";
            dSql = "select * from " + TableName + " where 1<>1";
            DataSet ds = this.ExecuteDataSet(dSql);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString().ToUpper() == FieldName.ToString().ToUpper())
                    {
                        rbc = true;
                        goto Return_End;
                    }
                }
                dt.Dispose();
                dt = null;
            }
            ds.Dispose();
            ds = null;

        Return_End:

            return rbc;
            #endregion
        }

        public override DataTable GetSchemeTables()
        {
            DataTable Stabs = null;
            if (m_dbconn.State != ConnectionState.Open)
            {
                m_dbconn.Open();
            }
            if (this.m_dbconn is OleDbConnection)
            {
                Stabs = (this.m_dbconn as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            }
            return Stabs; 
        }

        public override char ParameterChar
        {
            get
            {
                return '@';
            }
        }

        public override DbParameter CreateParameter(string name, object value)
        {
            return new OleDbParameter(name, value);
        }

        public override DbParameter CreateParameter(string name)
        {
            DbParameter dbp = new OleDbParameter();
            dbp.ParameterName = name;
            return dbp;
        }

        public override DbParameter CreateParameter(string name, DbType dbtype, object value)
        {
            DbParameter dbp = new OleDbParameter();
            dbp.ParameterName = name;
            dbp.Value = value;
            dbp.DbType = dbtype;
            return dbp;            
        }
        public override DbParameter CreateParameter(string name, DbType dbtype, int size, object value)
        {
            DbParameter dbp = new OleDbParameter();
            dbp.ParameterName = name;
            dbp.Value = value;
            dbp.DbType = dbtype;
            dbp.Size = size;
            return dbp;
        }

        #region SQLSyntax   //mdb
        //
        public override string Syntax_SubString(string express, string pos, string length)
        {
            return string.Format("substring({0},{1},{2})", express, pos, length);
        }
        public override string Syntax_ToDate(string express, string format)
        {
            if (format.Trim() == "") format = "'yyyy-MM-dd'";
            return string.Format("cast({0} as datetime)", express);
        }
        public override string Syntax_ToNumber(string express)
        {
            return string.Format("TO_NUMBER({0})", express);
        }
        public override string Syntax_ToChar(string express, string format)
        {
            if (string.IsNullOrEmpty(format))
            {
                return string.Format("TO_CHAR({0})", express);
            }
            else
            {
                return string.Format("TO_CHAR({0},{1})", express, format);
            }
        }
        public override string Syntax_Length(string express)
        {
            return string.Format("length({0})", express);
        }
        public override string Syntax_Mod(string express, string n2)
        {
            return string.Format("MOD({0},{1})", express, n2);
        }
        public override string Syntax_Now()
        {
            return "now()";
        }
        public override string Syntax_Year(string express)
        {
            return this.Syntax_ToNumber(this.Syntax_ToChar(express, "'yyyy'"));
        }
        public override string Syntax_DateAdd(string express, int day)
        {
            string syntax = "{0}{1}";
            if (day > 0)
            {
                syntax = string.Format(syntax, express, "+" + day);
            }
            else
            {
                syntax = string.Format(syntax, express, day);
            }
            return syntax;
        }
        public override string Syntax_Dual()
        {
            return "";
        }

        #endregion
    }
}
