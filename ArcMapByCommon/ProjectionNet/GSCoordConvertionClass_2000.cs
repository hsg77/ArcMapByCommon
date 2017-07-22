using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByCommon
{
    /// <summary>
    /// 2000坐标系
    /// vp:hsg
    /// create date:2015-12
    /// </summary>
    public class GSCoordConvertionClass_CGCS : GSCoordConvertionClass, IProjectionConversion2
    {
        protected decimal a = 6378137M;           //2000椭球长半轴
        protected decimal b = 6356752.3141M;      //2000椭球短半轴
        protected decimal f = 1 / 298.257222101M;     //椭球扁率f
        protected decimal e = 0.0818191910428M;       //第一偏心率
        protected decimal e2 = 6.693421622966E-03M;   //椭球第一偏心率平方 e2
        protected decimal e12 = 6.738525414683E-03M;  //椭球第二偏心率平方 e12
        protected decimal c = 6399593.62586M;         //极点子午圈曲率半径 c
        protected decimal p = 206264.806252992M;      //弧度秒=180*3600/pi
        protected decimal pi = 3.1415926535M;

        protected double toNum(decimal num)
        {
            //return double.Parse(num.ToString());
            return (double)num;
        }
        protected decimal toDec(double num)
        {
            //return decimal.Parse(num.ToString());
            return (decimal)num;
        }
        protected decimal sin(decimal num)
        {
            return this.toDec(Math.Sin(this.toNum(num)));
        }
        protected decimal cos(decimal num)
        {
            return this.toDec(Math.Cos(this.toNum(num)));
        }
        protected decimal sqrt(decimal num)
        {
            return this.toDec(Math.Sqrt(this.toNum(num)));
        }
        protected decimal tan(decimal num)
        {
            return this.toDec(Math.Tan(this.toNum(num)));
        }
        protected decimal pow(decimal x, decimal y)
        {
            return toDec(Math.Pow(toNum(x), toNum(y)));
        }

        //高斯反算方法  OK   (x,y)=>(B,L)
        public override void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L)
        {
            //去掉大数和东移500公里
            decimal y1 = y - 500000.0M;
            if (this.IsBigNumber == true)
            {
                y1 = y1 - (this.L0 / (int)this.Strip) * 1000000M;
            }
            y = y1;
            decimal l0 = this.L0 * 3600;  //中央子午线转为秒值 如=105*3600
            //计算临时值
            //decimal e4 = e2 * e2;
            //decimal e6 = e4 * e2;
            //decimal e8 = e6 * e2;
            //decimal e10 = e8 * e2;
            //decimal e_12 = e10 * e2;
            decimal e4 = pow(e2, 2); // e2 * e2;
            decimal e6 = pow(e2, 3); //e4 * e2;
            decimal e8 = pow(e2, 4); //e6 * e2;
            decimal e10 = pow(e2, 5); //e8 * e2;
            decimal e_12 = pow(e2, 6); //e10 * e2;
            //
            decimal A1 = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536 + 693693 * e_12 / 1048576;
            decimal B1 = 3 * e2 / 8 + 15 * e4 / 32 + 525 * e6 / 1024 + 2205 * e8 / 4096 + 72765 * e10 / 131072 + 297297 * e_12 / 524288;
            decimal C1 = 15 * e4 / 256 + 105 * e6 / 1024 + 2205 * e8 / 16384 + 10395 * e10 / 65536 + 1486485 * e_12 / 8388608;
            decimal D1 = 35 * e6 / 3072 + 105 * e8 / 4096 + 10395 * e10 / 262144 + 55055 * e_12 / 1048576;
            decimal E1 = 315 * e8 / 131072 + 3465 * e10 / 524288 + 99099 * e_12 / 8388608;
            decimal F1 = 693 * e10 / 1310720 + 9009 * e_12 / 5242880;
            decimal G1 = 1001 * e_12 / 8388608;
            //求底点纬度值Bf
            decimal B0 = x / (a * (1 - e2) * A1);
            decimal Bf = 0.0M;
            decimal FB = 0.0M;
            decimal FB1 = 0.0M;
            decimal a0 = a * (1 - e2);
            decimal delta = Math.Abs(Bf - B0);
            while (delta > 4.8E-11M)   //0.000000000048M
            {
                Bf = B0;
                FB = a0 * (A1 * Bf - B1 * sin(2 * Bf) + C1 * sin(4 * Bf) - D1 * sin(6 * Bf) + E1 * sin(8 * Bf) - F1 * sin(10 * Bf) + G1 * sin(12 * Bf));
                FB1 = a0 * (A1 - 2 * B1 * cos(2 * Bf) + 4 * C1 * cos(4 * Bf) - 6 * D1 * cos(6 * Bf) + 8 * E1 * cos(8 * Bf) - 10 * F1 * cos(10 * Bf) + 12 * G1 * cos(12 * Bf));
                B0 = Bf + (x - FB) / FB1;
                //
                delta = Math.Abs(Bf - B0);
            }
            //
            decimal sinBf = sin(Bf);
            decimal sinBf2 = sinBf * sinBf;
            decimal W = sqrt(1 - e2 * sinBf2);
            decimal W3 = W * W * W;
            decimal N = a / W;
            decimal t = tan(Bf);
            decimal t2 = t * t;
            decimal t4 = t2 * t2;
            decimal cosBf = cos(Bf);
            decimal cosBf2 = cosBf * cosBf;
            decimal yy = e12 * cosBf2;   //η2
            decimal Mf = a0 / W3;
            //
            decimal y_N = y / N;
            decimal y_N2 = y_N * y_N;
            decimal y_N4 = y_N2 * y_N2;
            //
            //计算经伟度值B,L
            decimal t_B = Bf * p - (p * t / (2 * Mf) * y * y_N) * (1 - (5 + 3 * t2 + yy - 9 * yy * t2) * y_N2 + (61 + 90 * t2 + 45 * t4) * y_N4 / 360);
            decimal t_L = (p / cosBf) * y_N * (1 - (1 + 2 * t2 + yy) * y_N2 / 6 + (5 + 28 * t2 + 24 * t4 + 6 * yy + 8 * yy * t2) * y_N4 / 120);
            //
            L = t_L + l0;
            //
            B = t_B / 3600;   //转为度
            L = L / 3600;   //转为度
            //--the--end--
        }

        //高斯正算方法 (B,L)=>(x,y) OK
        public override void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y)
        {
            //计算临时值
            decimal e4 = pow(e2, 2); // e2 * e2;
            decimal e6 = pow(e2, 3); //e4 * e2;
            decimal e8 = pow(e2, 4); //e6 * e2;
            decimal e10 = pow(e2, 5); //e8 * e2;
            decimal e_12 = pow(e2, 6); //e10 * e2;
            //
            decimal A1 = 1 + 3 * e2 / 4 + 45 * e4 / 64 + 175 * e6 / 256 + 11025 * e8 / 16384 + 43659 * e10 / 65536 + 693693 * e_12 / 1048576;
            decimal B1 = 3 * e2 / 8 + 15 * e4 / 32 + 525 * e6 / 1024 + 2205 * e8 / 4096 + 72765 * e10 / 131072 + 297297 * e_12 / 524288;
            decimal C1 = 15 * e4 / 256 + 105 * e6 / 1024 + 2205 * e8 / 16384 + 10395 * e10 / 65536 + 1486485 * e_12 / 8388608;
            decimal D1 = 35 * e6 / 3072 + 105 * e8 / 4096 + 10395 * e10 / 262144 + 55055 * e_12 / 1048576;
            decimal E1 = 315 * e8 / 131072 + 3465 * e10 / 524288 + 99099 * e_12 / 8388608;
            decimal F1 = 693 * e10 / 1310720 + 9009 * e_12 / 5242880;
            decimal G1 = 1001 * e_12 / 8388608;
            //
            decimal l0 = this.L0 * 3600;  //中央子午线 度转为秒值 如=105*3600
            decimal LL = L * 3600;                   //转为秒值
            //
            decimal t_B = B * this.pi / 180;     //转为弧度值  b
            decimal t_L = (LL - l0) / p;          //转为秒值    l
            decimal L2 = pow(t_L, 2);// t_L * t_L;
            decimal L4 = pow(t_L, 4);// L2 * L2;
            //
            decimal sinB = sin(t_B);
            decimal sinB2 = sinB * sinB;
            decimal W = sqrt(1 - e2 * sinB2);
            //decimal W3 = pow(W, 3);// W * W * W;
            decimal N = a / W;
            decimal t = tan(t_B);
            decimal t2 = t * t;
            decimal t4 = t2 * t2;
            decimal cosB = cos(t_B);
            decimal cosB2 = cosB * cosB;
            decimal cosB4 = cosB2 * cosB2;
            decimal y2 = e12 * cosB2;   //η2
            decimal y4 = y2 * y2;
            //
            decimal l_p = t_L;  //t_L/p;  //上面t_L已经除了p值,这里就不再除p值
            decimal l2_p2 = L2;   //l2/(p*p);
            decimal l4_p4 = L4;   //l4/(p*p*p*p);
            //
            decimal a0 = a * (1 - e2);
            //计算子午弧长公式xx
            decimal xx = a0 * (A1 * t_B - B1 * sin(2 * t_B) + C1 * sin(4 * t_B) - D1 * sin(6 * t_B) + E1 * sin(8 * t_B) - F1 * sin(10 * t_B) + G1 * sin(12 * t_B));
            //计度平面坐标值x,y
            x = xx + N * t * cosB2 * l2_p2 * (0.5M + (5 - t2 + 9 * y2 + 4 * y4) * cosB2 * l2_p2 / 24 + (61 - 58 * t2 + t4) * cosB4 * l4_p4 / 720);
            y = N * cosB * l_p * (1 + (1 - t2 + y2) * cosB2 * l2_p2 / 6 + (5 - 18 * t2 + t4 + 14 * y2 - 58 * y2 * t2) * cosB4 * l4_p4 / 120);
            //
            if (this.IsBigNumber == true)  //转为高斯投影是大数投影吗？即Zone 35带数  （小数投影为CM_105E)
            {
                y = y + (this.L0 / (int)this.Strip) * 1000000M;
            }
            y = y + 500000.0M;
            //--the--end--
        }

    }

    /// <summary>
    /// 2000坐标系
    /// vp:hsg
    /// create date:2015-12
    /// </summary>
    public class GSCoordConvertionClass_CGCS_2000 : GSCoordConvertionClass_CGCS, IProjectionConversion2
    {
        public GSCoordConvertionClass_CGCS_2000()
        {
            this.a = 6378137M;           //2000椭球长半轴
            this.b = 6356752.3141M;      //2000椭球短半轴
            this.f = 1 / 298.257222101M;     //椭球扁率f
            this.e = 0.0818191910428M;       //第一偏心率
            this.e2 = 6.693421622966E-03M;   //椭球第一偏心率平方 e2
            this.e12 = 6.738525414683E-03M;  //椭球第二偏心率平方 e12
            this.c = 6399593.62586M;         //极点子午圈曲率半径 c
            this.p = 206264.806252992M;      //弧度秒=180*3600/pi
            this.pi = 3.1415926535M;
        }

    }
    /// <summary>
    /// 西安80坐标
    /// vp:hsg
    /// create date:2015-12
    /// </summary>
    public class GSCoordConvertionClass_CGCS_Xian80 : GSCoordConvertionClass_CGCS, IProjectionConversion2
    {
        public GSCoordConvertionClass_CGCS_Xian80()
        {
            this.a = 6378140M;           //西安80椭球长半轴
            this.b = 6356755.29M;        //西安80椭球短半轴
            this.f = 1 / 298.257M;     //椭球扁率f
            this.e = 0.081819221455523M;       //第一偏心率
            this.e2 = 6.69438499958795E-03M;   //椭球第一偏心率平方 e2
            this.e12 = 6.73950181947292E-03M;  //椭球第二偏心率平方 e12
            this.c = 6399596.65198801M;         //极点子午圈曲率半径 c
            this.p = 206264.806252992M;      //弧度秒=180*3600/pi
            this.pi = 3.1415926535M;
        }
    }
    /// <summary>
    /// 北京54坐标
    /// vp:hsg
    /// create date:2015-12
    /// </summary>
    public class GSCoordConvertionClass_CGCS_beijing54 : GSCoordConvertionClass_CGCS, IProjectionConversion2
    {
        public GSCoordConvertionClass_CGCS_beijing54()
        {
            this.a = 6378245M;             //北京54椭球长半轴
            this.b = 6356863.0188M;        //北京54椭球短半轴
            this.f = 1 / 298.3M;         //椭球扁率f
            this.e = 0.08181333401693M;       //第一偏心率
            this.e2 = 6.693421622966E-03M;   //椭球第一偏心率平方 e2
            this.e12 = 6.738525414683E-03M;  //椭球第二偏心率平方 e12
            this.c = 6399698.90176M;         //极点子午圈曲率半径 c
            this.p = 206264.806252992M;      //弧度秒=180*3600/pi
            this.pi = 3.1415926535M;
        }
    }

    //测试案例数据
    //static void Main(string[] args)
    //    {
    //        decimal x = 0;
    //        decimal y = 0;
    //        decimal B = 0;
    //        decimal L = 0;

    //        GSCoordConvertionClass_CGCS_2000 cc = new GSCoordConvertionClass_CGCS_2000();
    //        cc.IsBigNumber = true;
    //        cc.Strip = EnumStrip.Strip3;
    //        cc.L0 = 105;
    //        //
    //        //---------------------------------
    //        //反算 OK
    //        x = 4016159.7706M;
    //        y = 35358852.807M;
    //        cc.GetBLFromXY(x, y, ref B, ref L);
    //        //
    //        B = Math.Round(B, 8);
    //        L = Math.Round(L, 8);
    //        System.Console.WriteLine("X, Y = (" + x + ", " + y + "=>B,L=(" + B + "," + L + ")");
    //        //
    //        //正算
    //        //B = 36.155619734M;
    //        //L = 105.254854607M;
    //        cc.GetXYFromBL(B, L, ref x, ref y);
    //        System.Console.WriteLine("B,L=(" + B + "," + L + ")=>X,Y=(" + x + "," + y + "");
    //        //
    //        System.Console.Read();
    //    }
}
