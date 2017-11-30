using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ArcMapByCommon
{
    /// <summary>
    /// 数值参数集合
    /// vp:huwen
    /// create date:2013
    /// </summary>
    [Serializable]
    public class DBParameterCollection : IList<DBParameter>
    {
        private List<DBParameter> m_Parameters = null;

        public DBParameterCollection()
        {
            m_Parameters = new List<DBParameter>();
        }

        public int IndexOf(DBParameter item)
        {
            return m_Parameters.IndexOf(item);
        }

        public void Insert(int index, DBParameter item)
        {
            m_Parameters.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_Parameters.RemoveAt(index);
        }

        public DBParameter this[int index]
        {
            get
            {
                return m_Parameters[index];
            }
            set
            {
                m_Parameters[index] = value;
            }
        }

        public void Add(DBParameter item)
        {
            m_Parameters.Add(item);
        }

        public void Clear()
        {
            m_Parameters.Clear();
        }

        public bool Contains(DBParameter item)
        {
            return m_Parameters.Contains(item);
        }

        public void CopyTo(DBParameter[] array, int arrayIndex)
        {
            m_Parameters.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return m_Parameters.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(DBParameter item)
        {
            return m_Parameters.Remove(item);
        }

        public IEnumerator<DBParameter> GetEnumerator()
        {
            return m_Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Parameters.GetEnumerator();
        }
    }
}
