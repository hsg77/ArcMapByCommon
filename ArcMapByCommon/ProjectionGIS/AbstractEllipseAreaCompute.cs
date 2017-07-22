/*******************************************************************************
 Copyright (C) xp
 File Name  : AbstractEllipseAreaCompute.cs
 Summary    : 抽象类 椭球面面积计算 类
 Version    : 1.0
 Create     : 2007-10-31
 Author     : 何仕国
*******************************************************************************/
using System;
using System.Collections.Generic;
//
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace ArcMapByCommon
{
    /// <summary>
    /// 椭球面理论面积计算 接口
    /// </summary>
    public interface IEllipseAreaCompute
    {
        /// <summary>
        /// 计算IGeometry的椭球面的 理论面积
        /// </summary>
        /// <param name="geo"></param>
        /// <returns>椭球面的面积</returns>
        decimal Compute(IGeometry geo);

        IEllipseParameter EllipseParameter { get; set; }
    }

    /// <summary>
    /// 抽象类 椭球面面积计算 类
    /// </summary>
    public abstract class AbstractEllipseAreaCompute : IEllipseAreaCompute
    {
        /// <summary>
        /// 常数π=PI = 3.14159265358979
        /// </summary>
        public const decimal PI = 3.14159265358979M;



        public AbstractEllipseAreaCompute()
        {
        }


        #region IEllipseAreaCompute 成员

        /// <summary>
        /// 计算IGeometry的椭球面的面积
        /// 虚态  
        /// </summary>
        /// <param name="geo">要计算的IGeometry几何对象</param>
        /// <returns>(IGeometry as IArea).Area</returns>
        public virtual decimal Compute(IGeometry geo)
        {
            decimal rbc = 0.0M;
            if (geo is IArea)
            {
                rbc = Maths.Todecimal((geo as IArea).Area);
            }
            return rbc;
        }


        protected IEllipseParameter m_EllipseParameter = new WGS84_EllipseParameter();
        /// <summary>
        /// 获取椭球体参数
        /// 默认为WGS84椭球体参数
        /// return m_EllipseParameter
        /// read only
        /// </summary>
        public virtual IEllipseParameter EllipseParameter
        {
            get { return m_EllipseParameter; }
            set { m_EllipseParameter = value; }
        }

        #endregion
    }


    /// <summary>
    /// 图幅（分幅图)椭球面面理论面积
    /// </summary>
    public class TuFuEllipseAreaCompute : AbstractEllipseAreaCompute
    {
        
        public TuFuEllipseAreaCompute()
        {
            //默认为西安80
            //this.ep = new Xian80_EllipseParameter();  //还可以使用ISpatialReference
        }

        /// <summary>
        ///  计算IGeometry的椭球面的 理论面积
        /// </summary>
        /// <param name="geo">输入的分幅图的几何对象IGeometry(矩形状)</param>
        /// <returns>(单位：平方米)</returns>
        public override decimal Compute(IGeometry geo)
        {
            decimal P = 0.0M;
            //(1)最大最小角计算出WGS84大地经纬度坐标(四个点) 经差/纬差
            decimal xmin = Maths.Todecimal(geo.Envelope.XMin);
            decimal ymin = Maths.Todecimal(geo.Envelope.YMin);

            decimal xmax = Maths.Todecimal(geo.Envelope.XMax);
            decimal ymax = Maths.Todecimal(geo.Envelope.YMax);

            //(2)转为经纬度坐标
            decimal Bmin = 0.0M;//ToWGS84(xmin,ymin)
            decimal Lmin = 0.0M;

            decimal BMax = 0.0M;//ToWGS84(xmax,ymax)
            decimal Lmax = 0.0M;

            //double dL = Lmax - Lmin; //经差△L
            //double dB = BMax - Bmin; //纬差(B2-B1)

            //比例尺 5千
            IMapScale scale = new MapScale_5000();
            decimal dL = scale.L2_L1; //(单位：弧度)
            decimal dB = scale.B2_B1; //(单位：弧度)

            //default=西安椭球体参数
            decimal b = this.EllipseParameter.b;
            decimal a = this.EllipseParameter.a;
            decimal A = this.EllipseParameter.A;
            decimal B = this.EllipseParameter.B;
            decimal C = this.EllipseParameter.C;
            decimal D = this.EllipseParameter.D;
            decimal E = this.EllipseParameter.E;

            decimal Bm = (Bmin + BMax) / 2;
            decimal CosBm = Maths.Cos(Bm);
            decimal Cos3Bm = Maths.Cos(3 * Bm);
            decimal Cos5Bm = Maths.Cos(5 * Bm);
            decimal Cos7Bm = Maths.Cos(7 * Bm);
            decimal Cos9Bm = Maths.Cos(9 * Bm);

            P = ((4*PI* b * b * dL)/(360*60)) * (A * Maths.Sin(dB * 1 / 2) * CosBm
                                  - B * Maths.Sin(dB * 3 / 2) * Cos3Bm
                                  + C * Maths.Sin(dB * 5 / 2) * Cos5Bm
                                  - D * Maths.Sin(dB * 7 / 2) * Cos7Bm
                                  + E * Maths.Sin(dB * 9 / 2) * Cos9Bm);



            return P; //(单位：平方米)

        }

    }


    /// <summary>
    /// 任意梯形面积椭球面面理论面积
    /// </summary>
    public abstract class AnyTrapeziaEllipseAreaCompute : AbstractEllipseAreaCompute
    {
        protected GSCoordConvertionClass gassConv = new GSCoordConvertionClass_Beijing54();
        //需要三个配置参数
        private EnumProjectionDatum m_datum = EnumProjectionDatum.Xian80;
        public virtual  EnumProjectionDatum Datum
        {
            get
            {
                return m_datum;
            }
            set
            {
                m_datum = value;
                switch (m_datum)
                {
                    case EnumProjectionDatum.Bejing54:
                        gassConv = new GSCoordConvertionClass_Beijing54();
                        break;
                    case EnumProjectionDatum.Xian80:
                        gassConv = new GSCoordConVertionClass_Xian80_Call2();
                        break;
                    case EnumProjectionDatum.CGCS2000:
                        gassConv = new GSCoordConvertionClass_CGCS_2000();
                        break;
                }
            }
        }
        public bool IsBigNumber = true;
        public EnumStrip Strip = EnumStrip.Strip3;
        public decimal L0 = 105;

        public AnyTrapeziaEllipseAreaCompute()
        {
            //默认为西安80
            this.m_EllipseParameter = new Xian80_EllipseParameter();  //还可以使用ISpatialReference
            this.Datum = EnumProjectionDatum.Xian80;
        }

        /// <summary>
        ///  计算IGeometry的椭球面的 理论面积
        /// </summary>
        /// <param name="geo">输入的分幅图的几何对象IGeometry(矩形状)</param>
        /// <returns>(单位：平方米)</returns>
        public override decimal Compute(IGeometry geo)
        {
            decimal P = 0.0M;
            decimal Lany = 60.0M;
            decimal ShapeArea = 0;
            //(1)最大最小角计算出WGS84大地经纬度坐标(四个点) 经差/纬差
            decimal xmin = Maths.Todecimal(geo.Envelope.XMin);
            decimal ymin = Maths.Todecimal(geo.Envelope.YMin);

            decimal xmax = Maths.Todecimal(geo.Envelope.XMax);
            decimal ymax = Maths.Todecimal(geo.Envelope.YMax);

            ShapeArea = Maths.Todecimal((geo as IArea).Area);
            //double blxs = (geo as IArea).Area / (geo.Envelope as IArea).Area;

            //(2)转为经纬度坐标   //ToWGS84(xmax,ymax)
            decimal Bmin = 0, Lmin = 0, Bmax = 0, Lmax = 0;
            this.gassConv.GetBLFromXY(ymin, xmin, ref Bmin, ref Lmin, 0, this.Datum, this.Strip, this.L0, this.IsBigNumber);//ToWGS84(xmin,ymin)
            this.gassConv.GetBLFromXY(ymax, xmax, ref Bmax, ref Lmax, 0, this.Datum, this.Strip, this.L0, this.IsBigNumber);//ToWGS84(xmin,ymin)


            decimal dL = (Lmax + Lmin) / 2 - Lany; //经差△L     
            decimal dB = Bmax - Bmin; //纬差(B2-B1) 

            dL = dL * PI / 180;   //(单位：弧度)
            dB = dB * PI / 180;   //(单位：弧度)

            //default=西安椭球体参数
            decimal b = this.EllipseParameter.b;
            decimal a = this.EllipseParameter.a;
            decimal A = this.EllipseParameter.A;
            decimal B = this.EllipseParameter.B;
            decimal C = this.EllipseParameter.C;
            decimal D = this.EllipseParameter.D;
            decimal E = this.EllipseParameter.E;

            decimal Bm = (Bmin + Bmax) / 2;
            Bm = Bm * PI / 180;  //(单位：弧度)
            decimal CosBm = Maths.Cos(Bm);
            decimal Cos3Bm = Maths.Cos(3 * Bm);
            decimal Cos5Bm = Maths.Cos(5 * Bm);
            decimal Cos7Bm = Maths.Cos(7 * Bm);
            decimal Cos9Bm = Maths.Cos(9 * Bm);

            //求geo几何体的包络矩形的椭球面积
            P = (2* b * b * dL)  * (A * Maths.Sin(dB * 1 / 2) * CosBm
                                  - B * Maths.Sin(dB * 3 / 2) * Cos3Bm
                                  + C * Maths.Sin(dB * 5 / 2) * Cos5Bm
                                  - D * Maths.Sin(dB * 7 / 2) * Cos7Bm
                                  + E * Maths.Sin(dB * 9 / 2) * Cos9Bm);

            //正算经纬度矩形的另外两个点的投影坐标值
            decimal x2 = 0, y2 = 0, x4 = 0, y4 = 0;
            this.gassConv.GetXYFromBL(Bmin, Lmax, ref y2, ref x2, 0, this.Datum, EnumStrip.Strip3, this.IsBigNumber);
            this.gassConv.GetXYFromBL(Bmax, Lmin, ref y4, ref x4, 0, this.Datum, EnumStrip.Strip3, this.IsBigNumber);
            //求经纬度矩形的投影梯形面积(上底+下底)*高/2
            decimal BLTrapeziaArea = ((x2 - xmin) + (xmax - x4)) * (ymax - ymin) / 2;

            decimal LLmj = P * ShapeArea / BLTrapeziaArea;
            return LLmj; //(单位：平方米)

        }

        /// <summary>
        /// 求(B1,L1)-(B2,L2)的任意梯形椭球面积
        /// </summary>
        /// <param name="B1"></param>
        /// <param name="L1"></param>
        /// <param name="B2"></param>
        /// <param name="L2"></param>
        /// <returns></returns>
        public decimal Compute(decimal B1, decimal L1, decimal B2, decimal L2)
        {
            decimal P = 0;
            decimal Lany = 60.0M;

            decimal dL = (L2 + L1) / 2 - Lany; //经差△L     
            decimal dB = B2 - B1; //纬差(B2-B1) 

            dL = dL * PI / 180;   //(单位：弧度)
            dB = dB * PI / 180;   //(单位：弧度)

            //default=西安椭球体参数
            decimal b = this.EllipseParameter.b;
            decimal a = this.EllipseParameter.a;
            decimal A = this.EllipseParameter.A;
            decimal B = this.EllipseParameter.B;
            decimal C = this.EllipseParameter.C;
            decimal D = this.EllipseParameter.D;
            decimal E = this.EllipseParameter.E;

            decimal Bm = (B1 + B2) / 2;
            Bm = Bm * PI / 180;  //(单位：弧度)
            decimal CosBm = Maths.Cos(Bm);
            decimal Cos3Bm = Maths.Cos(3 * Bm);
            decimal Cos5Bm = Maths.Cos(5 * Bm);
            decimal Cos7Bm = Maths.Cos(7 * Bm);
            decimal Cos9Bm = Maths.Cos(9 * Bm);

            //求任意梯形椭球面积
            P = (2 * b * b * dL) * (A * Maths.Sin(dB * 1 / 2) * CosBm
                                  - B * Maths.Sin(dB * 3 / 2) * Cos3Bm
                                  + C * Maths.Sin(dB * 5 / 2) * Cos5Bm
                                  - D * Maths.Sin(dB * 7 / 2) * Cos7Bm
                                  + E * Maths.Sin(dB * 9 / 2) * Cos9Bm);
            return P;
        }

    }

    /// <summary>
    /// 任意梯形面积椭球面面理论面积 北京54 BeiJing54
    /// </summary>
    public class AnyTrapeziaEllipseAreaCompute_BeiJing54 : AnyTrapeziaEllipseAreaCompute
    {
        public AnyTrapeziaEllipseAreaCompute_BeiJing54()
        {
            this.m_EllipseParameter = new Beijin54_EllipseParameter();
            this.Datum = EnumProjectionDatum.Bejing54;
        }

    }

    /// <summary>
    /// 任意梯形面积椭球面面理论面积 西安80 XiAn80
    /// </summary>
    public class AnyTrapeziaEllipseAreaCompute_XiAn80 : AnyTrapeziaEllipseAreaCompute
    {
        public AnyTrapeziaEllipseAreaCompute_XiAn80()
        {
            this.m_EllipseParameter = new Xian80_EllipseParameter();
            this.Datum = EnumProjectionDatum.Xian80;
        }
    }

    /// <summary>
    /// 积分方法求任意面的椭球面积(北京54/西安80/2000坐标系)
    /// </summary>
    public class AnyTrapeziaEllipseAreaCompute_JF : AnyTrapeziaEllipseAreaCompute
    {
        public override decimal Compute(IGeometry geo)
        {
            decimal InterRin = 0;
            decimal ExtenRin = 0;
            IGeometry TempGeo=null ;
            IGeometryCollection PGeocol = geo as IGeometryCollection;
            for (int i = 0; i < PGeocol.GeometryCount; i++)
            {
                TempGeo = PGeocol.get_Geometry(i);
                if ((TempGeo as IRing).IsExterior)
                {
                    ExtenRin += GetRingEllipse(TempGeo);
                }
                else
                {
                    InterRin += GetRingEllipse(TempGeo);
                }
            }
            return ExtenRin - InterRin;
           
        }
        public decimal GetRingEllipse(IGeometry geo)
        {
            IPointCollection pointColl = geo as IPointCollection;
            IPoint point = null;
            decimal x = 0;
            decimal y = 0;
            decimal[] B = new decimal[pointColl.PointCount];
            decimal[] L = new decimal[pointColl.PointCount];
            int intPart = 0;
            if (this.Datum == EnumProjectionDatum.Xian80)
            {   //西安80系
                for (int i = 0; i < pointColl.PointCount; i++)
                {
                    point = pointColl.get_Point(i);
                    x = Maths.Todecimal(point.X); y = Maths.Todecimal(point.Y);
                    intPart=(int)x;
                    if (intPart.ToString().Length == 6)
                    {   //x坐标实质是小数坐标
                        this.IsBigNumber = false;
                    }
                    if (intPart.ToString().Length ==8)
                    {   //x坐标实质是小数坐标
                        this.IsBigNumber = true;
                    }
                    //高斯反算
                    if (this.gassConv is GSCoordConVertionClass_Xian80_Call2)
                    {
                        (this.gassConv as GSCoordConVertionClass_Xian80_Call2).IsBigNumber = this.IsBigNumber;
                        (this.gassConv as GSCoordConVertionClass_Xian80_Call2).Strip = this.Strip;
                        (this.gassConv as GSCoordConVertionClass_Xian80_Call2).L0 = this.L0;
                        (this.gassConv as GSCoordConVertionClass_Xian80_Call2).GetBLFromXY(y, x, ref B[i], ref L[i]);
                    }
                    else
                    {
                        this.gassConv.GetBLFromXY(y, x, ref B[i], ref L[i], 1, this.Datum, this.Strip, this.L0, this.IsBigNumber);
                    }
                }
            }
            else if (this.Datum == EnumProjectionDatum.CGCS2000)
            {   //2000坐标系
                for (int i = 0; i < pointColl.PointCount; i++)
                {
                    point = pointColl.get_Point(i);
                    x = Maths.Todecimal(point.X); y = Maths.Todecimal(point.Y);
                    intPart = (int)x;
                    if (intPart.ToString().Length == 6)
                    {   //x坐标实质是小数坐标
                        this.IsBigNumber = false;
                    }
                    if (intPart.ToString().Length == 8)
                    {   //x坐标实质是小数坐标
                        this.IsBigNumber = true;
                    }
                    //高斯反算
                    if (this.gassConv is GSCoordConvertionClass_CGCS_2000)
                    {
                        (this.gassConv as GSCoordConvertionClass_CGCS_2000).IsBigNumber = this.IsBigNumber;
                        (this.gassConv as GSCoordConvertionClass_CGCS_2000).Strip = this.Strip;
                        (this.gassConv as GSCoordConvertionClass_CGCS_2000).L0 = this.L0;
                        (this.gassConv as GSCoordConvertionClass_CGCS_2000).GetBLFromXY(y, x, ref B[i], ref L[i]);
                    }
                    else
                    {
                        this.gassConv.GetBLFromXY(y, x, ref B[i], ref L[i], 1, this.Datum, this.Strip, this.L0, this.IsBigNumber);
                    }
                }
            }
            else
            {   //北京54
                for (int i = 0; i < pointColl.PointCount; i++)
                {
                    point = pointColl.get_Point(i);
                    x = Maths.Todecimal(point.X); y = Maths.Todecimal(point.Y);
                    //高斯反算
                    this.gassConv.GetBLFromXY(y, x, ref B[i], ref L[i], 1, this.Datum, this.Strip, this.L0, this.IsBigNumber);
                }
            }
            decimal sum = 0;
            for (int i = 0; i < B.Length; i++)
            {
                if (i < B.Length - 1)
                {
                    //double n = GetSign(L[i], L[i + 1]) * (Math.Abs(B[i] + B[i + 1]) * Math.Abs(L[i + 1] - L[i])) / 2;
                    decimal n = this.Compute(B[i + 1], L[i + 1], B[i], L[i]);
                    //System.Diagnostics.Debug.WriteLine(n.ToString());
                    sum += n;
                }
                else
                {
                    //double r = GetSign(L[B.Length - 1], L[0]) * (Math.Abs(B[0] + B[B.Length - 1]) * Math.Abs(L[0] - L[B.Length - 1])) / 2;
                    decimal r = this.Compute(B[0], L[0], B[B.Length - 1], L[L.Length - 1]);
                    //System.Diagnostics.Debug.WriteLine(r.ToString());
                    sum += r;
                }
            }
            sum = Math.Abs(sum);

            return sum;
        }
        
        private int GetSign(double FistL, double LastL)
        {
            if ((FistL - LastL) > 0)
                return 1;
            else
                return -1;
        }
    }

    /// <summary>
    /// 积分方法求任意面的椭球面积(大地坐标WGS1984)经纬度坐标
    /// </summary>
    public class AnyTrapeziaEllipseAreaCompute_JF_WGS1984 : AnyTrapeziaEllipseAreaCompute
    {
        public override decimal Compute(IGeometry geo)
        {
            decimal InterRin = 0, ExtenRin = 0;
            IGeometry TempGeo = null;
            IGeometryCollection PGeocol = geo as IGeometryCollection;
            for (int i = 0; i < PGeocol.GeometryCount; i++)
            {
                TempGeo = PGeocol.get_Geometry(i);
                if ((TempGeo as IRing).IsExterior)
                {
                    ExtenRin += GetRingEllipse(TempGeo);
                }
                else
                {
                    InterRin += GetRingEllipse(TempGeo);
                }
            }
            return ExtenRin - InterRin;

        }
        public decimal GetRingEllipse(IGeometry geo)
        {
            IPointCollection pointColl = geo as IPointCollection;
            IPoint point = null;
            decimal x = 0, y = 0;
            decimal[] B = new decimal[pointColl.PointCount];
            decimal[] L = new decimal[pointColl.PointCount];

            for (int i = 0; i < pointColl.PointCount; i++)
            {
                point = pointColl.get_Point(i);
                x = Maths.Todecimal(point.X); y = Maths.Todecimal(point.Y);
                B[i] = y;
                L[i] = x;
            }
            decimal sum = 0;
            for (int i = 0; i < B.Length; i++)
            {
                if (i < B.Length - 1)
                {
                    //double n = GetSign(L[i], L[i + 1]) * (Math.Abs(B[i] + B[i + 1]) * Math.Abs(L[i + 1] - L[i])) / 2;
                    decimal n = this.Compute(B[i + 1], L[i + 1], B[i], L[i]);
                    //System.Diagnostics.Debug.WriteLine(n.ToString());
                    sum += n;
                }
                else
                {
                    //double r = GetSign(L[B.Length - 1], L[0]) * (Math.Abs(B[0] + B[B.Length - 1]) * Math.Abs(L[0] - L[B.Length - 1])) / 2;
                    decimal r = this.Compute(B[0], L[0], B[B.Length - 1], L[L.Length - 1]);
                    //System.Diagnostics.Debug.WriteLine(r.ToString());
                    sum += r;
                }
            }
            sum = Math.Abs(sum);

            return sum;
        }

        private int GetSign(decimal FistL, decimal LastL)
        {
            if ((FistL - LastL) > 0)
                return 1;
            else
                return -1;
        }
    }


    /// <summary>
    /// 椭球体参数 接口
    /// </summary>
    public interface IEllipseParameter
    {
        decimal a { get; }
        decimal b { get; }
        decimal alphi { get; }

        decimal A { get; }
        decimal B { get; }
        decimal C { get; }
        decimal D { get; }
        decimal E { get; }

        /// <summary>
        /// 计算A-E参数
        /// </summary>
        void ComputerAE_Parameter();

    }

    /// <summary>
    ///抽象类 椭球体参数 类
    /// </summary>
    public abstract class EllipseParameter : IEllipseParameter
    {
        public EllipseParameter()
        {
        }

        #region IEllipseParameter 成员

        protected decimal m_a = 0.0M;
        public decimal a
        {
            get { return m_a; }
        }

        protected decimal m_b = 0.0M;
        public decimal b
        {
            get { return m_b; }
        }

        protected decimal m_alphi = 0.0M;
        public decimal alphi
        {
            get { return m_alphi; }
        }


        //

        private decimal m_A = 0.0M;
        public decimal A
        {
            get { return m_A; }
        }

        private decimal m_B = 0.0M;
        public decimal B
        {
            get { return m_B; }
        }

        private decimal m_C = 0.0M;
        public decimal C
        {
            get { return m_C; }
        }

        private decimal m_D = 0.0M;
        public decimal D
        {
            get { return m_D; }
        }

        private decimal m_E = 0.0M;
        public decimal E
        {
            get { return m_E; }
        }


        /// <summary>
        /// 计算A-E参数 A-E=f(a,b);   
        /// e2=(a^2-b^2)/a^2
        /// e4=e2*e2
        /// e6=e4*e2
        /// e8=e6*e2
        /// this.A = 1 + (3 / 6) * e2 + (30 / 80) * e4 + (35 / 112) * e6 + (630 / 2304) * e8;
        /// this.B =     (1 / 6) * e2 + (15 / 80) * e4 + (21 / 112) * e6 + (420 / 2304) * e8;
        /// this.C =                     (3 / 80) * e4 + ( 7 / 112) * e6 + (180 / 2304) * e8;
        /// this.D =                                      (1 / 112) * e6 + ( 45 / 2304) * e8;
        /// this.E =                                                       (  5 / 2304) * e8;
        /// </summary>
        public void ComputerAE_Parameter()
        {
            decimal e2 = (this.a * this.a - this.b * this.b) / (this.a * this.a);
            decimal e4 = e2 * e2;
            decimal e6 = e4 * e2;
            decimal e8 = e6 * e2;
            this.m_A = 1 + (3.0M / 6) * e2 + (30.0M / 80) * e4 + (35.0M / 112) * e6 + (630.0M / 2304) * e8;
            this.m_B = (1.0M / 6) * e2 + (15.0M / 80) * e4 + (21.0M / 112) * e6 + (420.0M / 2304) * e8;
            this.m_C = (3.0M / 80) * e4 + (7.0M / 112) * e6 + (180.0M / 2304) * e8;
            this.m_D = (1.0M / 112) * e6 + (45.0M / 2304) * e8;
            this.m_E = (5.0M / 2304) * e8;

        }

        #endregion
    }

    /// <summary>
    /// 西安80椭球体参数
    /// </summary>
    public class Xian80_EllipseParameter : EllipseParameter
    {
        public Xian80_EllipseParameter()
        {
            this.m_a = 6378140.0M;
            this.m_b = 6356755.2881575283M;
            this.m_alphi = 1 / 298.257M;  //0.0033528131778969144060323814696721

            //计算A-E参数
            this.ComputerAE_Parameter();
        }
    }

    /// <summary>
    /// 北京54椭球体参数
    /// </summary>
    public class Beijin54_EllipseParameter : EllipseParameter
    {
        public Beijin54_EllipseParameter()
        {
            this.m_a = 6378245.0M;
            this.m_b = 6356863.01877305M;
            this.m_alphi = 1 / 298.3M;

            //计算A-E参数
            this.ComputerAE_Parameter();
        }
    }

    /// <summary>
    /// WGS84大地椭球体参数
    /// </summary>
    public class WGS84_EllipseParameter : EllipseParameter
    {
        public WGS84_EllipseParameter()
        {
            this.m_a = 6378137.0M;
            this.m_b = 6356752.3142451793M;
            this.m_alphi = 1 / 298.2572236M;

            //计算A-E参数
            this.ComputerAE_Parameter();
        }
    }

    /// <summary>
    /// 2000坐标系椭球体参数
    /// </summary>
    public class GB2000_EllipseParameter : EllipseParameter
    {
        public GB2000_EllipseParameter()
        {
            this.m_a = 6378137.0M;
            this.m_b = 6356752.31414M;
            this.m_alphi = 1 / 298.257222101M;

            //计算A-E参数
            this.ComputerAE_Parameter();
        }
    }

    /// <summary>
    ///使用 ISpatialReference 接口获取椭球体参数
    /// </summary>
    public class SpatialReference_EllipseParameter : EllipseParameter
    {
        public SpatialReference_EllipseParameter(ISpatialReference sr)
        {
            if (sr is IGeographicCoordinateSystem)
            {
                IGeographicCoordinateSystem system = sr as IGeographicCoordinateSystem;

                this.m_a = Maths.Todecimal(system.Datum.Spheroid.SemiMajorAxis);  //长半轴
                this.m_b = Maths.Todecimal(system.Datum.Spheroid.SemiMinorAxis);  //短半轴
                this.m_alphi = Maths.Todecimal(system.Datum.Spheroid.Flattening); //扁率
            }


            //计算A-E参数
            this.ComputerAE_Parameter();
        }
    }

}
