using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    //投影变换
    public interface IProjectionConversion
    {
        /// <summary>
        /// 大地坐标转换为高斯投影坐标
        ///  Description:
        ///'   大地坐标转换为高斯投影坐标
        ///' Parameters:
        ///'   LX,BX      - 输入，经纬度坐标
        ///'   x,y        - 输出，高斯投影坐标
        ///'   nearright  - 输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）
        ///'   ProjectionType  - 输入，1 - 80坐标系，0 - 54坐标系
        ///'   StripType  - 输入，1 - 6度分带，0 - 3度分带
        /// </summary>
        /// <param name="BX">输入，纬度坐标</param>
        /// <param name="LX">输入，经度坐标</param>
        /// <param name="x">输出，高斯投影坐标</param>
        /// <param name="y">输出，高斯投影坐标</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="ProjectionType">输入，1 - 80坐标系，0 - 54坐标系</param>
        /// <param name="StripType">输入，1 - 6度分带，0 - 3度分带</param>
        /// <param name="IsBigNumber">输入, 是大数坐标吗？</param>
        void GetXYFromBL(decimal BX,
                         decimal LX,
                         ref decimal x,
                         ref decimal y,
                         long nearright,
                         EnumProjectionDatum ProjectionType,
                         EnumStrip StripType, bool IsBigNumber);
        /// <summary>
        /// 高斯投影坐标转换为大地坐标
        /// Parameters:
        ///   x,y        - 输入，高斯投影坐标
        ///   LX,BX      - 输出，经纬度坐标
        ///   nearright  - 输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）
        ///   ProjectionType  - 输入，1 - 80坐标系，0 - 54坐标系
        ///   StripType  - 输入，1 - 6度分带，0 - 3度分带
        ///   L0         - 输入, 3度或6度分带的中央子午线 单位为度
        /// </summary>
        /// <param name="x"> 输入，高斯投影坐标</param>
        /// <param name="y"> 输入，高斯投影坐标</param>
        /// <param name="BX">输入，纬度坐标</param>
        /// <param name="LX">输入，经度坐标</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="ProjectionType">输入，1 - 80坐标系，0 - 54坐标系</param>
        /// <param name="StripType">输入，1 - 6度分带，0 - 3度分带</param>
        /// <param name="L0">输入, 3度或6度分带的中央子午线 单位为度</param>
        /// <param name="IsBigNumber">输入, 是大数坐标吗？</param>
        /// <returns></returns>
        bool GetBLFromXY(decimal x,
                         decimal y,
                         ref decimal BX,
                         ref decimal LX,
                         int nearright,
                         EnumProjectionDatum ProjectionType,
                         EnumStrip StripType,
                         decimal L0, bool IsBigNumber);

    }

    public interface IProjectionConversion2
    {
        void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L);
        void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y);

        bool IsBigNumber { get; set; }
        EnumStrip Strip { get; set; }
        decimal L0 { get; set; }
    }
}
