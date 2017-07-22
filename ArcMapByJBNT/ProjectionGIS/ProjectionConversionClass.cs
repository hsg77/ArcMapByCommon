using System;
using System.Collections.Generic;
//
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 投影转换功能
    /// </summary>
    public abstract class ProjectionConversionClass : AbsProjectionConversionClass, IDataTransform
    {
        public ProjectionConversionClass()
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
        public void GetXYFromBL(double BX, double LX, ref double x, ref double y, long nearright, EnumProjectionDatum ProjectionType, EnumStrip StripType, bool IsBigNumber)
        {
            double t = 0;
            double B = 0;
            double L = 0;
            double x0 = 0;
            double y2 = 0;
            double E12 = 0;
            double c0 = 0;
            double n1 = 0;
            double m = 0;
            double temp1 = 0, temp2 = 0;
            double LX1 = 0;

            double fc_pi = 3.14159265358979;     //PI值

            B = (BX / 180) * fc_pi;   //转为弧度值bx*(PI/180)
            L = (LX / 180) * fc_pi;   //转为弧度值lx*(PI/180)

            //求带内中央子午线经度
            LX1 = GetLocalLongitude(LX, nearright, StripType);

            m = Math.Cos(B) * fc_pi * LX1 / 180.0;
            t = Math.Tan(B);
            switch (ProjectionType)
            {
                case EnumProjectionDatum.Xian80:   //80坐标
                    c0 = 6399596.65198801;
                    E12 = 0.0067395018195;
                    x0 = 111133.0047 * BX - (32009.8575 * Math.Sin(B) + 133.9978 * (Math.Pow(Math.Sin(B), 3)) + 0.6975 * (Math.Pow(Math.Sin(B), 5)) + 0.0039 * (Math.Pow(Math.Sin(B), 7))) * Math.Cos(B);
                    break;
                case EnumProjectionDatum.Bejing54: //54坐标
                    c0 = 6399698.90178271;
                    E12 = 0.00673852541468;
                    x0 = 111134.8611 * BX - (32005.7798 * Math.Sin(B) + 133.9614 * (Math.Pow(Math.Sin(B), 3)) + 0.6972 * (Math.Pow(Math.Sin(B), 5)) + 0.0039 * (Math.Pow(Math.Sin(B), 7))) * Math.Cos(B);
                    break;
                default:   //其它暂为空
                    break;
            }

            y2 = E12 * (Math.Pow(Math.Cos(B), 2));
            n1 = c0 / Math.Sqrt(1.0 + y2);
            temp1 = (Math.Pow(m, 2)) / 2.0 + (5.0 - (Math.Pow(t, 2)) + 9.0 * y2 + 4 * (Math.Pow(y2, 2))) * (Math.Pow(m, 4)) / 24.0 + (61.0 - 58.0 * (Math.Pow(t, 2)) + (Math.Pow(t, 4))) * (Math.Pow(m, 6)) / 720.0;
            temp2 = m + (1.0 - (Math.Pow(t, 2)) + y2) * (Math.Pow(m, 3)) / 6.0 + (5.0 - 18.0 * (Math.Pow(t, 2)) + (Math.Pow(t, 4)) + 14.0 * y2 - 58.0 * y2 * (Math.Pow(t, 2))) * (Math.Pow(m, 5)) / 120.0;

            x = x0 + n1 * t * temp1;
            y = n1 * temp2 + 500000.0;  //与中央经线的距离m  //500000.0表示平移了500公里距离 目的是把负数坐标转为正数好处理

            //主带数值（整数）
            double tpMainStrip = 0;  //主带数值
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
        public bool GetBLFromXY(double x, double y, ref double BX, ref double LX, int nearright, EnumProjectionDatum ProjectionType, EnumStrip StripType, double L0, bool IsBigNumber)
        {
            double a0 = 0;
            double b0 = 0;
            double c0 = 0;
            double E12 = 0;
            double bf0 = 0;
            double bf = 0;
            double tf = 0;
            double yf2 = 0;
            double n = 0;
            double b1 = 0;
            double l1 = 0;
            double temp1 = 0;
            long iStripNum = 0;
            double fc_pi = 3.14159265358979;

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
                    a0 = 6378245;            //长半轴 a（米）
                    b0 = 6356863.01877305;   //短半轴b（米）
                    c0 = 6399698.90178271;
                    E12 = 0.0067385254147;
                    bf0 = 27.11115372595 + 9.02468257083 * (x - 3) - 0.00579740442 * (Math.Pow((x - 3), 2)) - 0.00043532572 * (Math.Pow((x - 3), 3)) + 0.00004857285 * (Math.Pow((x - 3), 4)) + 0.00000215727 * (Math.Pow((x - 3), 5)) - 0.00000019399 * (Math.Pow((x - 3), 6));
                    break;
                case EnumProjectionDatum.Xian80:   //西安80
                    a0 = 6378140;            //长半轴 a（米）
                    b0 = 6356755.28815753;   //短半轴b（米） 
                    c0 = 6399596.65198801;
                    E12 = 0.0067395018195;
                    bf0 = 27.11162289465 + 9.02483657729 * (x - 3) - 0.00579850656 * (Math.Pow((x - 3), 2)) - 0.00043540029 * (Math.Pow((x - 3), 3)) + 0.00004858357 * (Math.Pow((x - 3), 4)) + 0.00000215769 * (Math.Pow((x - 3), 5)) - 0.00000019404 * (Math.Pow((x - 3), 6));
                    break;
                default:   //暂为空
                    break;
            }
            bf = bf0 * fc_pi / 180;
            tf = Math.Tan(bf);
            yf2 = E12 * (Math.Pow(Math.Cos(bf), 2));
            n = y * Math.Sqrt(1 + yf2) / c0;
            temp1 = 90.0 * (Math.Pow(n, 2)) - 7.5 * (5.0 + 3.0 * (Math.Pow(tf, 2)) + yf2 - 9.0 * yf2 * (Math.Pow(tf, 2))) * (Math.Pow(n, 4)) + 0.25 * (61.0 + 90.0 * (Math.Pow(tf, 2)) + 45.0 * (Math.Pow(tf, 4))) * (Math.Pow(n, 6));

            b1 = bf0 - (1.0 + yf2) * tf * temp1 / fc_pi;
            l1 = (180.0 * n - 30.0 * (1.0 + 2.0 * (Math.Pow(tf, 2)) + yf2) * (Math.Pow(n, 3)) + 1.5 * (5.0 + 28.0 * (Math.Pow(tf, 2)) + 24.0 * (Math.Pow(tf, 4))) * (Math.Pow(n, 5))) / fc_pi / Math.Cos(bf);

            l1 = l1 + L0;
            if (l1 < 0.0) { l1 = l1 + 360.0; }
            if (l1 >= 360.0) { l1 = l1 - 360.0; }
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
        private double GetLocalLongitude(double LX, long nearright, EnumStrip StripType)
        {
            double difl = 0;
            double CM = 0;  //中央子午线 经度
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

                        if (difl - 3.0 > 0.000001)
                            difl = difl - 360.0;
                        else if (difl + 3.0 < -0.000001)
                            difl = difl + 360.0;

                    }
                    break;
                case EnumStrip.Strip3:  //3度分带
                    {
                        if (LX <= 1.5)
                        {
                            if (IsCentralMeridianRight(CM) == true)
                                difl = LX + 360.0 - CM;
                            else
                                difl = LX + 360.0 - GetCentralMeridian(LX, nearright, StripType);
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
        public double GetCentralMeridian(double L, long nearright, EnumStrip StripType)
        {
            double StripNum;
            StripNum = Convert.ToDouble(GetStripNum(L, ref nearright, StripType));

            StripNum = StripNum + nearright;

            double cm = 0;
            switch (StripType)
            {
                case EnumStrip.Strip6:
                    cm = 6.0 * StripNum - 3.0;
                    break;
                case EnumStrip.Strip3:
                    cm = 3.0 * StripNum;
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
        public bool IsCentralMeridianRight(double m)
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
        public double SetCentralMeridian(double m)
        {
            double cm = -999;
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
        public long GetStripNum(double L, ref long nearright, EnumStrip StripType)
        {
            double StripNum;
            long tempstripnum;

            if (StripType == EnumStrip.Strip6)
            {   //6分带
                StripNum = (L / 6.0) + 1.0;
            }
            else
            {
                //3分带
                if (L <= 1.5)
                {
                    StripNum = (L + 360.0 - 1.5) / 3.0 + 1.0;
                }
                else
                {
                    StripNum = (L - 1.5) / 3.0 + 1.0;
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

        #region IDataTransform 成员

        /// <summary>
        /// 虛态方法
        /// </summary>
        /// <returns></returns>
        public virtual bool Transform()
        {
            return false;
        }

        private ISpatialReference m_ImportSpatialReference = null;
        public ISpatialReference ImportSpatialReference
        {
            get
            {
                return m_ImportSpatialReference;
            }
            set
            {
                m_ImportSpatialReference = value;
            }
        }
        private ISpatialReference m_ExportSpatialReference = null;
        public ISpatialReference ExportSpatialReference
        {
            get
            {
                return m_ExportSpatialReference;
            }
            set
            {
                m_ExportSpatialReference = value;
            }
        }


        public string getSpatialReferenceString(ISpatialReference pSpatialReference)
        {
            string SRText = "";
            double num2;
            if (pSpatialReference == null)
            {
                return "";
            }
            if (pSpatialReference is IGeographicCoordinateSystem)
            {
                IGeographicCoordinateSystem system = pSpatialReference as IGeographicCoordinateSystem;
                string text2 = "";
                text2 += "名称： " + system.Name + "\r\n";
                text2 += "别名： " + system.Alias + "\r\n";
                text2 += "缩略名： " + system.Abbreviation + "\r\n";        //get_Abbreviation()
                text2 += "说明：   " + system.Remarks + "\r\n";             //get_Remarks
                text2 += "角度单位：  " + system.CoordinateUnit.Name + "";   //get_CoordinateUnit().get_Name() 
                text2 += "(" + system.CoordinateUnit.RadiansPerUnit.ToString() + ")\r\n";  //get_CoordinateUnit().get_RadiansPerUnit().ToString()
                num2 = 1 / system.Datum.Spheroid.Flattening;  //get_Datum().get_Spheroid().get_Flattening()

                SRText += text2 + "本初子午线：" + system.PrimeMeridian.Name + " ";//get_PrimeMeridian().get_Name() 
                SRText += "(" + system.PrimeMeridian.Longitude.ToString() + ")\r\n "; //get_PrimeMeridian().get_Longitude().ToString()
                SRText += "(数据：      " + system.Datum.Name + "\r\n";//get_Datum().get_Name() 
                SRText += "(  椭球体：    " + system.Datum.Spheroid.Name + "\r\n";//.get_Datum().get_Spheroid().get_Name() 
                SRText += "(长半轴：    " + system.Datum.Spheroid.SemiMajorAxis.ToString() + "\r\n";//get_Datum().get_Spheroid().get_SemiMajorAxis().ToString()
                SRText += "(短半轴：    " + system.Datum.Spheroid.SemiMinorAxis.ToString() + "\r\n";   //get_Datum().get_Spheroid().get_SemiMinorAxis().ToString()
                SRText += "扁率倒数：  " + num2.ToString();
                return SRText;
            }
            if (!(pSpatialReference is IProjectedCoordinateSystem))
            {
                return "";
            }
            IProjectedCoordinateSystem4GEN system3 = pSpatialReference as IProjectedCoordinateSystem4GEN;
            IGeographicCoordinateSystem system2 = system3.GeographicCoordinateSystem;  //.get_GeographicCoordinateSystem();
            IProjection projection = system3.Projection;  //get_Projection();
            IParameter[] parameterArray = new IParameter[26];  //0x1a
            system3.GetParameters(ref parameterArray);   //GetParameters(out parameterArray);
            string text4 = "";
            int upperBound = parameterArray.GetUpperBound(0);
            for (int i = 0; i <= upperBound; i++)
            {
                if (parameterArray[i] != null)
                {
                    text4 += parameterArray[i].Name + "：" + parameterArray[i].Value.ToString() + "\r\n";
                }
            }
            string text3 = "";
            text3 += "名称：  " + system3.Name + "\r\n";
            text3 += "别名：  " + system3.Alias + "\r\n";
            text3 += "缩略名：" + system3.Abbreviation + "\r\n";//get_Abbreviation()
            text3 += "说明：  " + system3.Remarks + "\r\n";//get_Remarks()
            text3 += "投影：  " + system3.Projection.Name + "\r\n";//get_Projection().get_Name()
            text3 += "参数：\n  " + text4 + "\r\n";
            text3 += "线性单位：" + system3.CoordinateUnit.Name + "";//get_CoordinateUnit().get_Name()
            text3 += "(" + system3.CoordinateUnit.MetersPerUnit.ToString() + ")\r\n";//get_CoordinateUnit().get_MetersPerUnit().ToString()
            text3 += "地理坐标系：\r\n" + "  名称：      " + system2.Name + "\r\n";//get_Name()
            text3 += "缩略名：    " + system2.Abbreviation + "\r\n";//get_Abbreviation()
            text3 += "说明：      " + system2.Remarks + "\r\n";//get_Remarks() 
            text3 += "角度单位：  " + system2.CoordinateUnit.Name + "";//get_CoordinateUnit().get_Name() 
            text3 += "(" + system2.CoordinateUnit.RadiansPerUnit.ToString() + ")\r\n";//get_CoordinateUnit().get_RadiansPerUnit().ToString()
            num2 = 1 / system2.Datum.Spheroid.Flattening;//get_Datum().get_Spheroid().get_Flattening()

            SRText += text3 + "本初子午线：" + system2.PrimeMeridian.Name + " ";//get_PrimeMeridian().get_Name()
            SRText += "(" + system2.PrimeMeridian.Longitude.ToString() + ")\r\n";//get_PrimeMeridian().get_Longitude().ToString()
            SRText += "数据：      " + system2.Datum.Name + "\r\n";//get_Datum().get_Name()
            SRText += "椭球体：    " + system2.Datum.Spheroid.Name + "\r\n";//get_Datum().get_Spheroid().get_Name() 
            SRText += "长半轴：    " + system2.Datum.Spheroid.SemiMajorAxis.ToString() + "\r\n";//get_Datum().get_Spheroid().get_SemiMajorAxis().ToString()
            SRText += "短半轴：    " + system2.Datum.Spheroid.SemiMinorAxis.ToString() + "\r\n";//get_Datum().get_Spheroid().get_SemiMinorAxis().ToString()
            SRText += "扁率倒数：  " + num2.ToString();

            return SRText;
        }

        public string getSpatialReferenceProjectName(ISpatialReference pSpatialReference)
        {
            string tpProjectName = "";
            if (pSpatialReference == null)
            {
                return "";
            }
            if (!(pSpatialReference is IProjectedCoordinateSystem))
            {
                return "";
            }
            IProjectedCoordinateSystem4GEN system3 = pSpatialReference as IProjectedCoordinateSystem4GEN;
            tpProjectName = system3.Name;

            return tpProjectName;
        }

        /// <summary>
        /// 获取空间参考接口By通过要素类
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public ISpatialReference getISpatialReference(IFeatureClass pFeatureClass)
        {
            ISpatialReference sr = null;
            IGeoDataset ds = pFeatureClass as IGeoDataset;
            if (ds != null)
            {
                sr = ds.SpatialReference;
            }
            ds = null;
            return sr;
        }

        //Get ISpatialReference by pcsType
        public ISpatialReference GetISpatialReference(int pcsType)
        {
            ISpatialReferenceFactory oisrfactory = new SpatialReferenceEnvironmentClass();
            ISpatialReference oisr = null;
            oisr = oisrfactory.CreateProjectedCoordinateSystem(pcsType);
            return oisr;
        }

        /// <summary>
        /// IGeometry对象投影转换
        /// </summary>
        /// <param name="sourceGeometry">源对象</param>
        /// <param name="SourceProjection">源投影</param>
        /// <param name="SourceStrip">源分带</param>
        /// <param name="L0">源中央子午线</param>
        /// <param name="IsBigNumber_Source">源是/否是大数</param>
        /// <param name="ObjectProjection">目标投影</param>
        /// <param name="ObjectStrip">目标分带</param>
        /// <param name="IsBigNumber_Object">目标是/否是大数</param>
        /// <param name="ObjectSpatialReference">目标空间参考接口</param>
        /// <returns>返加投影转换后对象IGeometry</returns>
        public IGeometry getObjectGeometry(IGeometry sourceGeometry,
                                         EnumProjectionDatum SourceProjection,
                                         EnumStrip SourceStrip, double L0, bool IsBigNumber_Source,
                                         EnumProjectionDatum ObjectProjection,
                                         EnumStrip ObjectStrip, bool IsBigNumber_Object,
                                         ISpatialReference ObjectSpatialReference)
        {
            IGeometry ObjGeometry = null;
            IPoint p = null;
            if (sourceGeometry is IPoint)
            {   //点
                #region Point
                double x = 0;
                double y = 0;
                double B = 0;
                double L = 0;
                x = (sourceGeometry as IPoint).X;
                y = (sourceGeometry as IPoint).Y;
                this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                IPoint objP = new PointClass();
                objP.X = y;
                objP.Y = x;
                //投影到目标点对象
                objP.SpatialReference = ObjectSpatialReference;
                ObjGeometry = objP as IGeometry;
                #endregion
            }
            else if (sourceGeometry is IPolyline)
            {   //线
                #region polyline
                IPolyline objPolyline = new PolylineClass();
                IPointCollection objPc = objPolyline as IPointCollection;
                double x = 0;
                double y = 0;
                double B = 0;
                double L = 0;
                IPointCollection pcol = sourceGeometry as IPointCollection;
                object miss = Type.Missing;
                for (int j = 0; j < pcol.PointCount; j++)
                {
                    p = pcol.get_Point(j);
                    x = p.X;
                    y = p.Y;
                    this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                    this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                    IPoint objPoint = new PointClass();
                    objPoint.X = y;
                    objPoint.Y = x;
                    objPoint.SpatialReference = ObjectSpatialReference;
                    objPc.AddPoint(objPoint, ref miss, ref miss);
                }
                objPolyline.SpatialReference = ObjectSpatialReference;
                ObjGeometry = objPolyline as IGeometry;
                #endregion
            }
            else if (sourceGeometry is IPolygon)
            {   //面   /注记面
                #region polygon
                IPolygon objPolygon = new PolygonClass();
                IGeometryCollection objPc = objPolygon as IGeometryCollection;
                double x = 0;
                double y = 0;
                double B = 0;
                double L = 0;
                IGeometryCollection GeoCol = sourceGeometry as IGeometryCollection;
                object miss = Type.Missing;
                IGeometry tpGeo = null;
                IPointCollection pcol = null;
                IRing newRing = null;
                IPointCollection newRingPointColl = null;
                for (int j = 0; j < GeoCol.GeometryCount; j++)
                {
                    tpGeo = GeoCol.get_Geometry(j);  //面内环ring(内/外环)

                    pcol = tpGeo as IPointCollection;
                    newRing = new RingClass();
                    newRingPointColl = newRing as IPointCollection;
                    for (int k = 0; k < pcol.PointCount; k++)
                    {
                        p = pcol.get_Point(k);
                        x = p.X;
                        y = p.Y;
                        this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                        this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                        IPoint objPoint = new PointClass();
                        objPoint.X = y;
                        objPoint.Y = x;
                        objPoint.SpatialReference = ObjectSpatialReference;
                        newRingPointColl.AddPoint(objPoint, ref miss, ref miss);
                    }
                    newRing.SpatialReference = ObjectSpatialReference;
                    objPc.AddGeometry(newRing as IGeometry, ref miss, ref miss);
                }
                objPolygon.SpatialReference = ObjectSpatialReference;
                ObjGeometry = objPolygon as IGeometry;
                #endregion
            }
            else if (sourceGeometry is IGeometryBag)
            {
                //包
                #region GeometryBag
                GeometryBagClass objgeobag = new GeometryBagClass();
                IGeometryBag geoBag = sourceGeometry as IGeometryBag;
                IGeometryCollection geoColl = geoBag as IGeometryCollection;
                IGeometry geo = null;
                IGeometry objgeo = null;
                object miss = Type.Missing;
                IGeometryCollection objgeobagColl = objgeobag as IGeometryCollection;
                for (int i = 0; i < geoColl.GeometryCount; i++)
                {
                    geo = geoColl.get_Geometry(i);
                    objgeo = this.getObjectGeometry(geo, SourceProjection, SourceStrip, L0, IsBigNumber_Source,
                                               ObjectProjection, ObjectStrip, IsBigNumber_Object, ObjectSpatialReference);
                    objgeo.SpatialReference = ObjectSpatialReference;
                    objgeobagColl.AddGeometry(objgeo, ref miss, ref miss);
                }
                objgeobag.SpatialReference = ObjectSpatialReference;
                ObjGeometry = objgeobag as IGeometry;
                #endregion
            }
            else if (sourceGeometry is IMultipoint)
            {
                //多点
                #region Multipoint
                MultipointClass objMuliPoint = new MultipointClass();
                IPointCollection objPc = objMuliPoint as IPointCollection;
                double x = 0;
                double y = 0;
                double B = 0;
                double L = 0;
                IPointCollection pcol = sourceGeometry as IPointCollection;
                object miss = Type.Missing;
                for (int j = 0; j < pcol.PointCount; j++)
                {
                    p = pcol.get_Point(j);
                    x = p.X;
                    y = p.Y;
                    this.GetBLFromXY(y, x, ref B, ref L, 0, SourceProjection, SourceStrip, L0, IsBigNumber_Source);

                    this.GetXYFromBL(B, L, ref x, ref y, 0, ObjectProjection, ObjectStrip, IsBigNumber_Object);
                    IPoint objPoint = new PointClass();
                    objPoint.X = y;
                    objPoint.Y = x;
                    objPoint.SpatialReference = ObjectSpatialReference;
                    objPc.AddPoint(objPoint, ref miss, ref miss);
                }
                objMuliPoint.SpatialReference = ObjectSpatialReference;
                ObjGeometry = objMuliPoint as IGeometry;
                #endregion
            }
            else
            {
                //暂未写
                ObjGeometry = null;

            }

            return ObjGeometry;

        }

        //set setSpatialReferenceDoMain
        public void SetSpatialReferenceDoMain(ISpatialReference osri, double pMinX, double pMinY, double pMaxX, double pMaxY)
        {
            osri.SetDomain(pMinX, pMaxX, pMinY, pMaxY);
        }

        #endregion
    }


    //投影转换
    public interface IDataTransform : IProjectionConversion
    {

        /// <summary>
        /// 数据投影转换
        /// </summary>
        /// <returns></returns>
        bool Transform();

        /// <summary>
        /// 源投影坐标系统
        /// </summary>
        ISpatialReference ImportSpatialReference { get; set; }
        /// <summary>
        /// 目标投影坐标系统
        /// </summary>
        ISpatialReference ExportSpatialReference { get; set; }

        /// <summary>
        /// 获取空间参考的文本描述信息
        /// </summary>
        /// <param name="pSpatialReference"></param>
        /// <returns></returns>
        string getSpatialReferenceString(ISpatialReference pSpatialReference);

        /// <summary>
        /// 获取空间参考的名称
        /// </summary>
        /// <param name="pSpatialReference"></param>
        /// <returns></returns>
        string getSpatialReferenceProjectName(ISpatialReference pSpatialReference);

        /// <summary>
        /// 获取空间参考接口By通过要素类
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        ISpatialReference getISpatialReference(IFeatureClass pFeatureClass);

        /// <summary>
        /// Get ISpatialReference by pcsType
        /// </summary>
        /// <param name="pcsType"></param>
        /// <returns></returns>
        ISpatialReference GetISpatialReference(int pcsType);

        /// <summary>
        /// IGeometry对象投影转换
        /// </summary>
        /// <param name="sourceGeometry">源对象</param>
        /// <param name="SourceProjection">源投影</param>
        /// <param name="SourceStrip">源分带</param>
        /// <param name="L0">源中央子午线</param>
        /// <param name="IsBigNumber_Source">源是/否是大数</param>
        /// <param name="ObjectProjection">目标投影</param>
        /// <param name="ObjectStrip">目标分带</param>
        /// <param name="IsBigNumber_Object">目标是/否是大数</param>
        /// <param name="ObjectSpatialReference">目标空间参考接口</param>
        /// <returns>返加投影转换后对象IGeometry</returns>
        IGeometry getObjectGeometry(IGeometry sourceGeometry,
                                         EnumProjectionDatum SourceProjection,
                                         EnumStrip SourceStrip, double L0, bool IsBigNumber_Source,
                                         EnumProjectionDatum ObjectProjection,
                                         EnumStrip ObjectStrip, bool IsBigNumber_Object,
                                         ISpatialReference ObjectSpatialReference);

        void SetSpatialReferenceDoMain(ISpatialReference osri, double pMinX, double pMinY, double pMaxX, double pMaxY);
    }


    
}
