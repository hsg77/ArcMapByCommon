using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ArcMapByCommon
{
    [Serializable]
    public class DbTableCols
    {
        public string tbName = "";
        public string colName = "";
        public string colType = "";
        public int colLength = 0;
        public int colScale = 0;
        public string IsNullable = "";  //T/F
        //
        public DbTableCols()
        {
        }
        public DbTableCols(DataRow dr)
        {
            if (dr != null)
            {
                this.tbName = dr["tbName"].ToString();
                this.colName = dr["colName"].ToString().ToUpper();
                this.colType = this.ChangeToCSharpType(dr["colType"].ToString().ToLower());
                this.colLength = this.TInt(dr["colLength"]);
                this.colScale = this.TInt(dr["colScale"]);
            }
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
        //
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
        //
    }
}
