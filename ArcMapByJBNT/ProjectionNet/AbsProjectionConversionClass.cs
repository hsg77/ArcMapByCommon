using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 投影转换功能
    /// </summary>
    public abstract class AbsProjectionConversionClass : IProjectionConversion
    {
        public AbsProjectionConversionClass()
        {
        }


        #region IProjectionConversion 成员
        /// <summary>
        /// 大地坐标转换为高斯投影坐标  正解公式
        /// WGC84->高斯投影
        ///  Description:
        ///'   大地坐标转换为高斯投影坐标
        ///' Parameters:
        ///'   LX,BX      - 输入，经纬度坐标
        ///'   x,y        - 输出，高斯投影坐标(大数)
        ///'   nearright  - 输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）
        ///'   ProjectionType  - 输入，1 - 80坐标系，0 - 54坐标系
        ///'   StripType  - 输入，1 - 6度分带，0 - 3度分带
        /// 当只有一个已知控制点时，根据平移参数调整东伪偏移、北纬偏移值实现WGS84到北京54的转换
        /// 东伪偏移=东伪偏移值=tpMainStrip * 1000000+500000.0
        /// 北纬偏移值=0;
        /// </summary>
        /// <param name="BX">大地纬度B</param>
        /// <param name="LX">大地经度L</param>
        /// <param name="x">高斯投影X</param>
        /// <param name="y">高斯投影Y</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="ProjectionType"> 输入，1 - 80坐标系，0 - 54坐标系</param>
        /// <param name="StripType"> 输入，1 - 6度分带，0 - 3度分带</param>
        /// <param name="IsBigNumber">输入, 是大数坐标吗？</param>
        public void GetXYFromBL(decimal BX, decimal LX, ref decimal x, ref decimal y, long nearright, EnumProjectionDatum ProjectionType, EnumStrip StripType, bool IsBigNumber)
        {
            decimal t = 0;
            decimal B = 0;
            decimal L = 0;
            decimal x0 = 0;
            decimal y2 = 0;
            decimal E12 = 0;
            decimal c0 = 0;
            decimal n1 = 0;
            decimal m = 0;
            decimal temp1 = 0, temp2 = 0;
            decimal LX1 = 0;

            decimal fc_pi = 3.14159265358979M;     //PI值

            B = (BX / 180) * fc_pi;   //转为弧度值bx*(PI/180)
            L = (LX / 180) * fc_pi;   //转为弧度值lx*(PI/180)

            //求带内中央子午线经度
            LX1 = GetLocalLongitude(LX, nearright, StripType);

            m = Maths.Cos(B) * fc_pi * LX1 / 180.0M;
            t = Maths.Tan(B);
            switch (ProjectionType)
            {
                case EnumProjectionDatum.Xian80:   //80坐标
                    c0 = 6399596.65198801M;
                    E12 = 0.0067395018195M;
                    x0 = 111133.0047M * BX - (32009.8575M * Maths.Sin(B) + 133.9978M * (Maths.Pow(Maths.Sin(B), 3)) + 0.6975M * (Maths.Pow(Maths.Sin(B), 5)) + 0.0039M * (Maths.Pow(Maths.Sin(B), 7))) * Maths.Cos(B);
                    break;
                case EnumProjectionDatum.Bejing54: //54坐标
                    c0 = 6399698.90178271M;
                    E12 = 0.00673852541468M;
                    x0 = 111134.8611M * BX - (32005.7798M * Maths.Sin(B) + 133.9614M * (Maths.Pow(Maths.Sin(B), 3)) + 0.6972M * (Maths.Pow(Maths.Sin(B), 5)) + 0.0039M * (Maths.Pow(Maths.Sin(B), 7))) * Maths.Cos(B);
                    break;
                default:   //其它暂为空
                    break;
            }

            y2 = E12 * (Maths.Pow(Maths.Cos(B), 2));
            n1 = c0 / Maths.Sqrt(1.0M + y2);
            temp1 = (Maths.Pow(m, 2)) / 2.0M + (5.0M - (Maths.Pow(t, 2)) + 9.0M * y2 + 4 * (Maths.Pow(y2, 2))) * (Maths.Pow(m, 4)) / 24.0M + (61.0M - 58.0M * (Maths.Pow(t, 2)) + (Maths.Pow(t, 4))) * (Maths.Pow(m, 6)) / 720.0M;
            temp2 = m + (1.0M - (Maths.Pow(t, 2)) + y2) * (Maths.Pow(m, 3)) / 6.0M + (5.0M - 18.0M * (Maths.Pow(t, 2)) + (Maths.Pow(t, 4)) + 14.0M * y2 - 58.0M * y2 * (Maths.Pow(t, 2))) * (Maths.Pow(m, 5)) / 120.0M;

            x = x0 + n1 * t * temp1;
            y = n1 * temp2 + 500000.0M;  //与中央经线的距离m  //500000.0表示平移了500公里距离 目的是把负数坐标转为正数好处理

            //主带数值（整数）
            decimal tpMainStrip = 0;  //主带数值
            switch (StripType)
            {
                case EnumStrip.Strip3:
                    tpMainStrip = (int)Math.Truncate(LX / 3) + 1;
                    break;
                case EnumStrip.Strip6:
                    tpMainStrip = (int)Math.Truncate(LX / 6) + 1;
                    break;
                default:  //则自动计算 ,默认为3度带
                    tpMainStrip = (int)Math.Truncate(LX / 3) + 1;
                    break;
            }

            //转为大数坐标y=带数*1000000+与中央经线的距离m
            //东伪偏移值=tpMainStrip * 1000000+500000.0
            if (IsBigNumber == true)  //转为高斯投影是大数投影吗？即Zone 35带数  （小数投影为CM_105E)
            {
                y = y + tpMainStrip * 1000000;
            }

        }

        /// <summary>
        /// 高斯投影坐标转换为大地坐标 反解公式
        /// Parameters:
        ///   x,y        - 输入，高斯投影坐标(大数)
        ///   LX,BX      - 输出，经纬度坐标
        ///   nearright  - 输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）
        ///   ProjectionType  - 输入，1 - 80坐标系，0 - 54坐标系
        ///   StripType  - 输入，1 - 6度分带，0 - 3度分带
        ///   L0         - 输入, 3度或6度分带的中央子午线 单位为度
        /// </summary>
        /// <param name="x"> 输入，高斯投影X坐标</param>
        /// <param name="y"> 输入，高斯投影Y坐标(大数)</param>
        /// <param name="BX">输入，纬度坐标</param>
        /// <param name="LX">输入，经度坐标</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="ProjectionType">输入，1 - 80坐标系，0 - 54坐标系</param>
        /// <param name="StripType">输入，1 - 6度分带，0 - 3度分带</param>
        /// <param name="L0">输入, 3度或6度分带的中央子午线 单位为度</param>
        /// <param name="IsBigNumber">输入, 是大数坐标吗？</param>
        /// <returns></returns>
        public bool GetBLFromXY(decimal x, decimal y, ref decimal BX, ref decimal LX, int nearright, EnumProjectionDatum ProjectionType, EnumStrip StripType, decimal L0, bool IsBigNumber)
        {
            decimal a0 = 0;
            decimal b0 = 0;
            decimal c0 = 0;
            decimal E12 = 0;
            decimal bf0 = 0;
            decimal bf = 0;
            decimal tf = 0;
            decimal yf2 = 0;
            decimal n = 0;
            decimal b1 = 0;
            decimal l1 = 0;
            decimal temp1 = 0;
            long iStripNum = 0;
            decimal fc_pi = 3.14159265358979M;

            //要素的Y坐标中是否含有大地坐标的大数？
            switch (StripType)
            {
                case EnumStrip.Strip6:
                    iStripNum = (long)Math.Round(L0 / 6);
                    break;
                case EnumStrip.Strip3:
                    iStripNum = (long)Math.Round(L0 / 3);
                    break;
            }

            //认为高斯投影Y坐标为大数(y=带数*1000000 + 与中央经线的距离m
            //要素的Y坐标包含有大地坐标吗？y=y+带数*1000000 + 与中央经线的距离m(500公里)
            if (IsBigNumber == true)
            {
                y = y - iStripNum * 1000000;
            }
            x = x / 1000000;
            y = y - 500000;

            switch (ProjectionType)
            {
                case EnumProjectionDatum.Bejing54:  //北京54
                    a0 = 6378245M;            //长半轴 a（米）
                    b0 = 6356863.01877305M;   //短半轴b（米）
                    c0 = 6399698.90178271M;
                    E12 = 0.0067385254147M;
                    bf0 = 27.11115372595M + 9.02468257083M * (x - 3) - 0.00579740442M * (Maths.Pow((x - 3), 2)) - 0.00043532572M * (Maths.Pow((x - 3), 3)) + 0.00004857285M * (Maths.Pow((x - 3), 4)) + 0.00000215727M * (Maths.Pow((x - 3), 5)) - 0.00000019399M * (Maths.Pow((x - 3), 6));
                    break;
                case EnumProjectionDatum.Xian80:   //西安80
                    a0 = 6378140M;            //长半轴 a（米）
                    b0 = 6356755.28815753M;   //短半轴b（米） 
                    c0 = 6399596.65198801M;
                    E12 = 0.0067395018195M;
                    bf0 = 27.11162289465M + 9.02483657729M * (x - 3) - 0.00579850656M * (Maths.Pow((x - 3), 2)) - 0.00043540029M * (Maths.Pow((x - 3), 3)) + 0.00004858357M * (Maths.Pow((x - 3), 4)) + 0.00000215769M * (Maths.Pow((x - 3), 5)) - 0.00000019404M * (Maths.Pow((x - 3), 6));
                    break;
                default:   //暂为空
                    break;
            }
            bf = bf0 * fc_pi / 180;
            tf = Maths.Tan(bf);
            yf2 = E12 * (Maths.Pow(Maths.Cos(bf), 2));
            n = y * Maths.Sqrt(1 + yf2) / c0;
            temp1 = 90.0M * (Maths.Pow(n, 2)) - 7.5M * (5.0M + 3.0M * (Maths.Pow(tf, 2)) + yf2 - 9.0M * yf2 * (Maths.Pow(tf, 2))) * (Maths.Pow(n, 4)) + 0.25M * (61.0M + 90.0M * (Maths.Pow(tf, 2)) + 45.0M * (Maths.Pow(tf, 4))) * (Maths.Pow(n, 6));

            b1 = bf0 - (1.0M + yf2) * tf * temp1 / fc_pi;
            l1 = (180.0M * n - 30.0M * (1.0M + 2.0M * (Maths.Pow(tf, 2)) + yf2) * (Maths.Pow(n, 3)) + 1.5M * (5.0M + 28.0M * (Maths.Pow(tf, 2)) + 24.0M * (Maths.Pow(tf, 4))) * (Maths.Pow(n, 5))) / fc_pi / Maths.Cos(bf);

            l1 = l1 + L0;
            if (l1 < 0.0M) { l1 = l1 + 360.0M; }
            if (l1 >= 360.0M) { l1 = l1 - 360.0M; }
            BX = b1;
            LX = l1;
            return true;
        }

        #endregion

        //OK
        /// <summary>
        ///  求带内相对于中央子午线的经度
        /// </summary>
        /// <param name="LX"> 输入，经度坐标</param>
        /// <param name="nearright">输入，是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="DivBy6or3">输入，1 - 6度分带，0 - 3度分带</param>
        /// <returns></returns>
        private decimal GetLocalLongitude(decimal LX, long nearright, EnumStrip StripType)
        {
            decimal difl = 0;
            decimal CM = 0;  //中央子午线 经度
            CM = GetCentralMeridian(LX, nearright, StripType);
            CM = SetCentralMeridian(CM);

            switch (StripType)
            {
                case EnumStrip.Strip6:  //6度分带
                    {
                        if (IsCentralMeridianRight(CM) == true)
                            difl = LX - CM;
                        else
                            difl = LX - GetCentralMeridian(LX, nearright, StripType);

                        if (difl - 3.0M > 0.000001M)
                            difl = difl - 360.0M;
                        else if (difl + 3.0M < -0.000001M)
                            difl = difl + 360.0M;

                    }
                    break;
                case EnumStrip.Strip3:  //3度分带
                    {
                        if (LX <= 1.5M)
                        {
                            if (IsCentralMeridianRight(CM) == true)
                                difl = LX + 360.0M - CM;
                            else
                                difl = LX + 360.0M - GetCentralMeridian(LX, nearright, StripType);
                        }
                        else
                        {
                            if (IsCentralMeridianRight(CM) == true)
                                difl = LX - CM;
                            else
                                difl = LX - GetCentralMeridian(LX, nearright, StripType);
                        }
                    }
                    break;
            }
            return difl;
        }

        //OK
        /// <summary>
        ///  根据经度坐标求中央子午线
        /// </summary>
        /// <param name="L">输入，经度坐标</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="StripType"> 输入，1 - 6度分带，0 - 3度分带</param>
        /// <returns></returns>
        public decimal GetCentralMeridian(decimal L, long nearright, EnumStrip StripType)
        {
            decimal StripNum;
            StripNum =GetStripNum(L, ref nearright, StripType);

            StripNum = StripNum + nearright;

            decimal cm = 0;
            switch (StripType)
            {
                case EnumStrip.Strip6:
                    cm = 6.0M * StripNum - 3.0M;
                    break;
                case EnumStrip.Strip3:
                    cm = 3.0M * StripNum;
                    break;
            }
            return cm;
        }

        //OK
        /// <summary>
        /// 判断中央子午线的正确性
        /// 是否在[0,360)度范围内
        /// </summary>
        /// <param name="m">要判断的中央子午线值</param>
        /// <returns></returns>
        public bool IsCentralMeridianRight(decimal m)
        {
            if ((m >= 0) && (m < 360))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //OK
        /// <summary>
        /// 设置坐标求中央子午线
        /// </summary>
        /// <param name="m">输入，中央子午线,要求m∈[0,360)</param>
        /// <returns>返回经过处理的中央子午线[经度数]</returns>
        public decimal SetCentralMeridian(decimal m)
        {
            decimal cm = -999;
            if (IsCentralMeridianRight(m) == true)
            {
                cm = m;
            }
            return cm;
        }

        //OK
        /// <summary>
        /// 根据经度坐标求带号
        /// </summary>
        /// <param name="L">输入，经度坐标</param>
        /// <param name="nearright">输入，L是相邻两带的分界线时的取法，0 - 左带（小），1 - 右带（大）</param>
        /// <param name="StripType">输入，6度分带/3度分带</param>
        /// <returns></returns>
        public long GetStripNum(decimal L, ref long nearright, EnumStrip StripType)
        {
            decimal StripNum;
            long tempstripnum;

            if (StripType == EnumStrip.Strip6)
            {   //6分带
                StripNum = (L / 6.0M) + 1.0M;
            }
            else
            {
                //3分带
                if (L <= 1.5M)
                {
                    StripNum = (L + 360.0M - 1.5M) / 3.0M + 1.0M;
                }
                else
                {
                    StripNum = (L - 1.5M) / 3.0M + 1.0M;
                }
            }

            StripNum = (int)StripNum;
            tempstripnum = (long)StripNum;

            long tpMainStrip = 0;   //主带数(整数)

            if (StripType == EnumStrip.Strip6)
            {   //6分带
                tpMainStrip = (int)Math.Truncate(L / 6) + 1;
                if (tempstripnum > 60) tempstripnum = tempstripnum - 60;
                if (tempstripnum < 1) tempstripnum = tempstripnum + 60;
            }
            else
            {
                //3分带
                tpMainStrip = (int)Math.Truncate(L / 3) + 1;
                if (tempstripnum > 120) tempstripnum = tempstripnum - 120;
                if (tempstripnum < 1) tempstripnum = tempstripnum + 120;
            }

            nearright = tpMainStrip - tempstripnum;
            return tempstripnum;
        }

        
    }

}
