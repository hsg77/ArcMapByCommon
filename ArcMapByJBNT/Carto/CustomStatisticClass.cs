using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using System.Collections;

namespace ArcMapByJBNT
{    
    /// <summary>
    /// 自定义ArcEngine 统计类 Statistic
    /// </summary>
    public static class CustomStatisticClass
    {
        private static IQueryFilter CreateQueryFilter(string whereClause)
        {
            IQueryFilter queryFilter = null;
            if (whereClause.Length > 0)
            {
                queryFilter = new QueryFilterClass();
                queryFilter.WhereClause = whereClause;
            }
            return queryFilter;
        }

        public static IDataStatistics DataStatistics(ITable table, string fieldName, string whereClause)
        {
            return DataStatistics(table, fieldName, CreateQueryFilter(whereClause));
        }

        public static IDataStatistics DataStatistics(ITable table, string fieldName, IQueryFilter queryFilter)
        {
            ICursor cursor = (ICursor)table.Search(queryFilter, false);
            IDataStatistics dataStatistics = new DataStatisticsClass();
            if (fieldName.Length > 0)
                dataStatistics.Field = fieldName;
            dataStatistics.Cursor = cursor;

            Marshal.ReleaseComObject(cursor);
            cursor = null;

            return dataStatistics;
        }

        public static double Sum(ITable table, string fieldName, string whereClause)
        {
            return DataStatistics(table, fieldName, whereClause).Statistics.Sum;
        }

        public static double Max(ITable table, string fieldName, string whereClause)
        {
            return DataStatistics(table, fieldName, whereClause).Statistics.Maximum;
        }

        public static double Min(ITable table, string fieldName, string whereClause)
        {
            return DataStatistics(table, fieldName, whereClause).Statistics.Minimum;
        }

        public static object[] Unique(ITable table, string fieldName, string whereClause)
        {
            List<object> rObjects = new List<object>();

            IEnumerator enumerator = DataStatistics(table, fieldName, whereClause).UniqueValues;
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object myObject = enumerator.Current;
                rObjects.Add(myObject);
            }
            return rObjects.ToArray();
        }

        public static int Count(ITable table, string whereClause)
        {
            return table.RowCount(CreateQueryFilter(whereClause));
        }
    }
}
