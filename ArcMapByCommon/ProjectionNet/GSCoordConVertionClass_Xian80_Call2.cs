using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByCommon
{
    /// <summary>
    /// 西安80坐标转换功能类 (二调算法)
    /// vp:hsg
    /// ceate date:2010
    /// </summary>
    public class GSCoordConVertionClass_Xian80_Call2 : GSCoordConvertionClass, IProjectionConversion2
    {
        //需要三个配置参数
        //private bool m_IsBigNumber = true;
        //private EnumStrip m_Strip = EnumStrip.Strip3;
        //private decimal m_L0 = 105;

        //public bool IsBigNumber
        //{
        //    get
        //    {
        //        return m_IsBigNumber;
        //    }
        //    set
        //    {
        //        m_IsBigNumber = value;
        //    }
        //}
        //public EnumStrip Strip
        //{
        //    get
        //    {
        //        return m_Strip;
        //    }
        //    set
        //    {
        //        m_Strip = value;
        //    }
        //}
        //public decimal L0
        //{
        //    get
        //    {
        //        return m_L0;
        //    }
        //    set
        //    {
        //        m_L0 = value;
        //    }
        //}

        //相关常数
        private decimal  K0 = 1.57048687472752E-07M;
        private decimal K1 = 5.05250559291393E-03M;
        private decimal K2 = 2.98473350966158E-05M;
        private decimal K3 = 2.41627215981336E-07M;
        private decimal K4 = 2.22241909461273E-09M;

        private decimal a = 6378140M;      //椭球长半轴
        private decimal b = 6356755.29M;   //椭球短半轴
        private decimal α = 1 / 298.257M;            //椭求扁率 α
        private decimal e2 = 6.69438499958795E-03M;   //椭球第一偏心率 e2
        private decimal e12 = 6.73950181947292E-03M;  //椭球第二偏心率 e12
        private decimal c = 6399596.65198801M;     //极点子午圈曲率半径 c

        public GSCoordConVertionClass_Xian80_Call2()
        {

        }

        //反算
        public override void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L)
        {
            //this.GetBLFromXY(x, y, ref B, ref L, 0, EnumProjectionDatum.Xian80, this.Strip, this.L0, this.IsBigNumber);
            //去掉大数和东移500公里
            decimal y1 = y - 500000.0M;
            if (this.IsBigNumber == true)
            {
                y1 = y1 - (this.L0 / (int)this.Strip) * 1000000M;
            }
            //计算临时参数
            decimal E = K0 * x;
            decimal CosE = Maths.Cos(E);
            decimal SinE = Maths.Sin(E);
            decimal SinE3 = SinE * SinE * SinE;
            decimal SinE5 = SinE3 * SinE * SinE;
            decimal SinE7 = SinE5 * SinE * SinE;

            //求底点纬度
            decimal Bf = E + CosE * (K1 * SinE - K2 * SinE3 + K3 * SinE5 - K4 * SinE7);

            //计算临时参数
            decimal CosBf = Maths.Cos(Bf);
            decimal CosBf2 = CosBf * CosBf;
            decimal t = Maths.Tan(Bf);
            decimal η2 = e12 * CosBf2;
            decimal C = a * a / b;
            decimal V = Maths.Sqrt(1 + η2);
            decimal N = C / V;
            decimal V2 = V * V;
            decimal t2 = t * t;
            decimal t4 = t2 * t2;
            decimal y1_N = y1 / N;
            decimal y1_N2 = y1_N * y1_N;
            decimal y1_N3 = y1_N2 * y1_N;
            decimal y1_N4 = y1_N3 * y1_N;
            decimal y1_N5 = y1_N4 * y1_N;
            decimal y1_N6 = y1_N5 * y1_N;
            decimal V2t = V * V * t;

            //计算经纬度值B,L
            B = Bf - (V2t * y1_N2) / 2 + ((5 + 3 * t2 + η2 - 9 * η2 * t2) * V2t * y1_N4 * 1.0M) / 24 - ((61 + 90 * t2 + 45 * t4) * V2t * y1_N6 * 1.0M) / 720;

            L = y1_N * 1.0M / CosBf - (1 + 2 * t2 + η2) * (1.0M / CosBf) * y1_N3 / 6 + (5 + 28 * t2 + 24 * t2 + 6 * η2 + 8 * η2 * t2) * (1.0M / CosBf) * y1_N5 * 1.0M / 120;
            AngleUnit_D dUnit = new AngleUnit_D(this.L0);
            L = L + dUnit.getArcDValue();  //弧度

            //弧度->度
            AngleUnit_ArcD acUnit = new AngleUnit_ArcD(B);
            dUnit = new AngleUnit_D(acUnit);
            B = (decimal)dUnit.getAngle();

            acUnit = new AngleUnit_ArcD(L);
            dUnit = new AngleUnit_D(acUnit);
            L = (decimal)dUnit.getAngle();
        }

        //正算
        public override void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y)
        {
            this.GetXYFromBL(B, L, ref x, ref y, 0, EnumProjectionDatum.Xian80, this.Strip, this.IsBigNumber);
            //--
        }

    }

}
