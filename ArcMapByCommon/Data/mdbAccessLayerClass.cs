using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByCommon
{
    public class mdbAccessLayerClass:IDisposable 
    {
        private string m_mdbPath = "";
        public mdbAccessLayerClass(string mdbPath)
        {
            m_mdbPath = mdbPath;
        }

        private DbConnectionWrapper m_GetMdbDB = null;
        public DbConnectionWrapper GetMdbDB
        {
            get
            {
                if (m_GetMdbDB == null)
                {
                    string mdbName = m_mdbPath;
                    string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}", mdbName);
                    m_GetMdbDB = new DbConnectionWrapper_mdb(connectionString);
                }
                return m_GetMdbDB;
            }
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (m_GetMdbDB != null)
            {
                m_GetMdbDB.Dispose();
                m_GetMdbDB = null;
            }
        }

        #endregion
    }
}
