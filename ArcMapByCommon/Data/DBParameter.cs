using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ArcMapByCommon
{
    /// <summary>
    /// 数值参数类，用于SQL语句中的参数传值
    /// vp:huwen
    /// create date:2013
    /// </summary>
    [Serializable]
    public class DBParameter
    {
        private string m_FieldName = string.Empty;
        private object m_FieldVlaue = null;
        private DbType m_FieldType = DbType.String;

        /// <summary>
        /// 数值参数 DbType默认为DbType.String
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        public DBParameter(string fieldName, object fieldValue)
        {
            m_FieldName = fieldName;
            m_FieldVlaue = fieldValue;
        }

        /// <summary>
        /// 数值参数
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="fieldType">字段类型</param>
        public DBParameter(string fieldName, object fieldValue, DbType fieldType)
        {
            m_FieldName = fieldName;
            m_FieldVlaue = fieldValue;
            m_FieldType = fieldType;
        }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName
        {
            get { return m_FieldName; }
        }

        /// <summary>
        /// 字段值
        /// </summary>
        public object FieldValue
        {
            get { return m_FieldVlaue; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        public DbType FieldType
        {
            get { return m_FieldType; }
        }
    }
}
