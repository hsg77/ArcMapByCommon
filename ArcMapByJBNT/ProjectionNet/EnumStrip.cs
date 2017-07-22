using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 设影类型  54/80/2000
    /// </summary>
    public enum EnumProjectionDatum
    {
        /// <summary>
        /// 北京54
        /// </summary>
        Bejing54 = 0,

        /// <summary>
        /// 西安80
        /// </summary>
        Xian80 = 1,

        /// <summary>
        /// 2000坐标系
        /// </summary>
        CGCS2000=2,
    }

    /// <summary>
    /// 分带类型 3/6分带
    /// </summary>
    public enum EnumStrip : int
    {
        /// <summary>
        /// 6分带
        /// </summary>
        Strip6 = 6,
        /// <summary>
        /// 3分带
        /// </summary>
        Strip3 = 3,

    }
    //
   
}
