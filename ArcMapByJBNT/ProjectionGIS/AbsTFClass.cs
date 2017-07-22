
/*****************************************
 *  功能:抽象类 图幅类 (分幅图类)
 *  vp:hsg
 *  date:2008-12-22
 *  email:hsg77@163.com
 *****************************************/

namespace ArcMapByJBNT
{
    using System;
    using System.Collections.Generic;    
    using System.Text;    

    /// <summary>
    /// 抽象类
    /// 图幅类 (分幅图类)
    /// </summary>
    public abstract class AbsTFClass : ITF
    {
        protected GSCoordConvertionClass gassConv = new GSCoordConvertionClass_Beijing54();
        //需要三个配置参数
        public EnumProjectionDatum Datum = EnumProjectionDatum.Xian80;
        public bool IsBigNumber = true;
        public EnumStrip Strip = EnumStrip.Strip3;
        public double L0 = 105;

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

        public AbsTFClass()
        {
            //默认为西安80
            this.m_EllipseParameter = new Xian80_EllipseParameter();  //还可以使用ISpatialReference
            this.Datum = EnumProjectionDatum.Xian80;
        }

        #region ITF 成员

        public virtual bool Compute(string TFH)
        {
            return false;
        }

        private string m_SZTFH = "";
        public string SZTFH
        {
            get
            {
                return m_SZTFH;
            }
            set
            {
                m_SZTFH = value;
            }
        }

        private string m_NewTFH = "";
        public string NewTFH
        {
            get
            {
                return m_NewTFH;
            }
            set
            {
                m_NewTFH = value;
            }
        }

        //是新图幅号吗?
        public virtual bool IsNewTFH(string NewTFH)
        {
            string th = NewTFH.ToUpper();
            int code = 0;
            char headChar;

            //最小长度判断
            if (th.Length != 10)
            {
                return false;
            }

            //判断第二字母 列号 1：100万
            headChar = char.Parse(th.Substring(0, 1));
            code = (int)headChar;
            if (code < (int)'A' || code > (int)'V')
            {
                //isOk = false;
                return false;
            }

            //判断行数 1：100万
            try
            {
                if (int.Parse(th.Substring(1, 2)) > 60)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            //判断 比例尺
            headChar = char.Parse(th.Substring(3, 1));
            code = (int)headChar;

            if (code < (int)'A' || code > (int)'I')
            {
                //isOk = false;
                return false;
            }


            //是否后几位为数值
            try
            {
                int.Parse(th.Substring(4, 3));
            }
            catch
            {
                return false;
            }

            //是否后几位为数值
            try
            {
                int.Parse(th.Substring(7, 3));
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string m_OldTFH = "";
        public string OldTFH
        {
            get
            {
                return m_OldTFH;
            }
            set
            {
                m_OldTFH = value;
            }
        }

        //是旧图幅号吗?
        public virtual  bool IsOldTFH(string OldTFH)
        {
            string th = OldTFH;

            int code = 0;
            char[] splitChar = new char[1];
            string[] oldTFHArr = null;
            string tempStr = "";
            char tempChar = ' ';
            //
            splitChar[0] = '-';
            oldTFHArr = th.Split(splitChar);

            if (oldTFHArr != null)
            {
                if (oldTFHArr.Length < 4 || oldTFHArr.Length > 5) // -格式判断
                {
                    return false;
                }

                //判断首字母 行号
                tempStr = oldTFHArr.GetValue(0).ToString().ToUpper();

                if (tempStr.Length != 1)
                {
                    return false;
                }

                try
                {
                    code = (int)char.Parse(tempStr);
                }
                catch
                {
                    return false;
                }

                if (code < (int)'A' || code > (int)'V')
                {
                    return false;
                }

                //判断 列号
                try
                {
                    tempStr = oldTFHArr.GetValue(1).ToString();
                    if (int.Parse(tempStr) > 60)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

                //判断 1：10万
                try
                {
                    if (int.Parse(oldTFHArr.GetValue(2).ToString()) > 144)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

                //判断 1：1万
                try
                {
                    tempStr = oldTFHArr.GetValue(3).ToString().Replace('(', ' ');
                    tempStr = tempStr.Replace(')', ' ').Trim();
                    if (int.Parse(tempStr) > 64)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }

                //判断1：5000 
                if (oldTFHArr.Length == 5)
                {
                    tempStr = oldTFHArr.GetValue(4).ToString().ToUpper();
                    try
                    {
                        tempChar = char.Parse(tempStr);
                        if ((int)tempChar >= (int)'E')
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                return true;  //如果上述规则都通过，则认为合格
            }
            else
            {
                return false;
            }
        }

        protected double m_GetTFGeometryArea = 0;
        public double GetTFGeometryArea
        {
            get { return m_GetTFGeometryArea; }
        }

        protected double m_GetTFEllipseArea = 0;
        public double GetTFEllipseArea
        {
            get { return m_GetTFEllipseArea; }
        }

        private TFgsRect m_TFgsRect = new TFgsRect();
        public TFgsRect TFgsRect
        {
            get { return m_TFgsRect; }
        }

        private TFBLRect m_TFBLRect = new TFBLRect();
        public TFBLRect TFBLRect
        {
            get { return m_TFBLRect; }
        }

        #endregion
    }

    /// <summary>
    /// 图幅接口
    /// </summary>
    public interface ITF
    {
        //计算图幅相关属性
        bool Compute(string TFH);

        /// <summary>
        /// 所在图幅号
        /// </summary>
        string SZTFH { get; set; }

        /// <summary>
        /// 新图幅号
        /// </summary>
        string NewTFH { get; set; }

        //是新图幅号吗?
        bool IsNewTFH(string NewTFH);

        /// <summary>
        /// 旧图幅号
        /// </summary>
        string OldTFH { get; set; }

        //是旧图幅号吗?
        bool IsOldTFH(string OldTFH);

        /// <summary>
        /// 获取图幅几何面积
        /// </summary>
        double GetTFGeometryArea { get; }

        /// <summary>
        /// 获取图幅椭球面积
        /// </summary>
        double GetTFEllipseArea { get; }

        /// <summary>
        /// 获取图幅的高斯坐标
        /// </summary>
        TFgsRect TFgsRect { get; }

        /// <summary>
        /// 获取图幅的经纬度坐标
        /// </summary>
        TFBLRect TFBLRect { get; }

    }


    /// <summary>
    /// 抽象类
    /// 图幅号转换类
    /// </summary>
    public abstract class AbsTFHConvertClass : IAbsTFHConvert
    {

        #region IAbsTFHConvert 成员

        //根据图幅名称解析图廓左下角经纬度
        public abstract bool FileName2BL(string fileName, ref double B, ref double L, ref double deltaB, ref double deltaL);

        //根据经纬度生成图幅号
        public abstract bool BL2FileName(string sScale, double B, double L, ref string fileName) ;
        
        #endregion
    }

    public interface IAbsTFHConvert
    {
        //文件名称(分幅号)-->转为-->BL经纬度坐标
        bool FileName2BL(string fileName, ref double B, ref double L, ref double deltaB, ref double deltaL);

        //BL经纬度坐标-->转为--->文件名称(分幅号)
        bool BL2FileName(string sScale, double B, double L, ref string fileName);
    }

    /// <summary>
    /// 土详(TX) 图幅号转换类
    /// </summary>
    public class TFHConvertClass_TX : AbsTFHConvertClass
    {

        public override bool FileName2BL(string fileName, ref double B, ref double L, ref double deltaB, ref double deltaL)
        {
            return this.FileName2BL_tx(fileName, ref B, ref L, ref deltaB, ref deltaL);
        }

        public override bool BL2FileName(string sScale, double B, double L, ref string fileName)
        {
            return BL2FileName_tx(sScale, B, L, ref fileName);
        }


        /// <summary>
        /// 根据图名解析图廓左下角经纬度坐标（新图号）
        /// </summary>
        /// <param name="fileName">图幅号</param>
        /// <param name="B">经度</param>
        /// <param name="L">纬度</param>
        /// <param name="deltaB">经差</param>
        /// <param name="deltaL">纬差</param>
        /// <returns></returns>
        private bool FileName2BL_tx(string fileName,
                                ref double B,
                                ref double L,
                                ref double deltaB,
                                ref double deltaL)
        {
            try
            {
                string mFileName;
                string Code;
                long a, bb, c, d;
                double addlongitude, addlatitude;
                int bigrow, bigcol, row, col;
                string mStr;
                char mChar;

                mFileName = fileName.ToLower();

                if (fileName.Length < 8)
                {
                    return false;
                }

                Code = mFileName.Substring(0, 1);

                mStr = mFileName.Substring(1, 1);
                mChar = mStr[0];
                bigrow = (int)mChar;

                mStr = mFileName.Substring(2, 2);
                bigcol = System.Convert.ToInt16(mStr);

                if (Code == "i")
                {
                    mStr = mFileName.Substring(4, 3);
                    row = System.Convert.ToInt16(mStr);

                    mStr = mFileName.Substring(7, 3);
                    col = System.Convert.ToInt16(mStr);

                }
                else
                {
                    mStr = mFileName.Substring(4, 2);
                    row = System.Convert.ToInt16(mStr);

                    mStr = mFileName.Substring(6, 2);
                    col = System.Convert.ToInt16(mStr);
                }

                switch (Code.ToUpper())
                {
                    case "B": // 1:250000   
                        addlongitude = 1.5;//     //'1度30'
                        addlatitude = 1.0;       //'1度
                        break;
                    case "C": //1:200000
                        addlongitude = 1.0; //      '1度30'
                        addlatitude = 2.0 / 3.0;//       '1度
                        break;
                    case "D":// 1:100000
                        addlongitude = 0.5;     //'30'
                        addlatitude = 1.0 / 3.0;  //'20'
                        break;
                    case "E": //1:50000
                        addlongitude = 0.25;    //'15'
                        addlatitude = 1.0 / 6.0;  //'10'
                        break;
                    case "F":// 1:25000
                        addlongitude = 0.125;   //'7'30"
                        addlatitude = 1.0 / 12.0; //'5'
                        break;
                    case "G":// 1:10000
                        addlongitude = 0.0625;  //3'45"
                        addlatitude = 1.0 / 24.0; //2'30"
                        break;
                    case "H"://1:5000
                        addlongitude = 0.03125; //1'52.5"
                        addlatitude = 1.0 / 48.0; //1'15"
                        break;
                    case "I": //1:2000
                        addlongitude = 1.5 / 120.0;   //1'52.5"
                        addlatitude = 1.0 / 120.0;   //1'15"
                        break;
                    default:
                        return false;
                    //break;

                }

                a = (bigrow - 97) + 1;
                bb = bigcol;
                if (bb >= 30) bb = bb - 30; //保证在东经
                c = row;
                d = col;

                L = (bb - 1.0) * 6.0 + (d - 1.0) * addlongitude;
                B = (a - 1.0) * 4.0 + (4.0 / addlatitude - c) * addlatitude;

                deltaL = addlongitude;
                deltaB = addlatitude;

                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 根据经纬度生成图号（新图号）
        /// </summary>
        /// <param name="sScale">比例尺</param>
        /// <param name="B">经度</param>
        /// <param name="L">纬度</param>
        /// <param name="fileName">生成的图幅号（新图号）</param>
        /// <returns>执行是否成功</returns>
        private bool BL2FileName_tx(string sScale,
                                double B,
                                double L,
                                ref string fileName)
        {
            long a, bb;
            string Code;
            double stepB, stepL;
            long c, d;
            //char mChar;

            if (sScale == null || sScale.Trim() == "")
            {
                return false;
            }

            Code = sScale.ToUpper();

            //if ((int)(Code[0]) < (int)('b'))
            //{
            //    mChar = Convert.ToChar((int)(Code[0]) + (int)('a') - (int)('A'));
            //    Code = mChar.ToString();
            //}

            if (((int)(Code[0]) < (int)('B')) || ((int)(Code[0]) > (int)('I')))
            {
                return false;
            }

            switch (Code.ToUpper())
            {
                case "B": //1:250000   已经小写过了
                    stepL = 1.5;    //1度30'
                    stepB = 1.0;     //1度
                    break;
                case "C": //1:200000
                    stepL = 1.0;      //1度30'
                    stepB = 2.0 / 3.0;      //1度
                    break;
                case "D": //1:100000
                    stepL = 0.5;    //30'
                    stepB = 1.0 / 3.0; //20'
                    break;
                case "E": //1:50000
                    stepL = 0.25;   //15'
                    stepB = 1.0 / 6.0; // 10'
                    break;
                case "F": //1:25000
                    stepL = 0.125;      //7'30"
                    stepB = 1.0 / 12.0; //'5'
                    break;
                case "G": //1:10000
                    stepL = 0.0625; //3'45"
                    stepB = 1.0 / 24.0; //2'30"
                    break;
                case "H": //1:5000
                    stepL = 0.03125; //1'52.5"
                    stepB = 1.0 / 48.0; //1'15"
                    break;
                case "I": //1:2000
                    stepL = 1.5 / 120.0;
                    stepB = 1.0 / 120.0;
                    break;
                default:
                    return false;
            }

            if (B <= 0 || L <= 0)
            {
                return false;
            }

            a = (int)(Math.Truncate(B / 4.0) + 1);
            bb = (int)(Math.Truncate(L / 6.0) + 1);

            c = (int)(4.0 / stepB - Math.Truncate(Math.Truncate(((B - (a - 1.0) * 4.0) / stepB) * 10000 + 1) / 10000));

            //ma
            d = (int)Math.Truncate((L - (bb - 1.0) * 6.0) / stepL) + 1;

            //2007-7-6 ma
            string c2 = c.ToString();
            string d2 = d.ToString();

            if (Code != "H" && Code != "I")
            {
                if (c < 10)
                {
                    c2 = "0" + c.ToString();
                }
                if (d < 10)
                {
                    d2 = "0" + d.ToString();
                }
            }
            else if (Code == "I")
            {
                if (c < 10)
                {
                    c2 = "00" + c.ToString();
                }
                else if (c >= 10 && c < 100)
                {
                    c2 = "0" + c.ToString();
                }

                if (d < 10)
                {
                    c2 = "00" + d.ToString();
                }
                else if (d >= 10 && d < 100)
                {
                    d2 = "0" + d.ToString();
                }
            }



            if (bb < 30) bb = bb + 30; //保证在东经

            fileName = Convert.ToChar((int)(Code[0]) - (int)('A') + (int)('A')).ToString() +
                    Convert.ToChar((int)('A') + a - 1).ToString() +
                    bb +
                    c2 +
                    d2;
            return true;
        }
    }

    /// <summary>
    /// 重庆土祥 (CQTX) 图幅号转换类
    /// </summary>
    public class TFHConvertClass_CQTX : AbsTFHConvertClass
    {

        public override bool FileName2BL(string fileName, ref double B, ref double L, ref double deltaB, ref double deltaL)
        {
            return this.FileName2BL_cqtx(fileName, ref B, ref L, ref deltaB, ref deltaL);
        }

        public override bool BL2FileName(string sScale, double B, double L, ref string fileName)
        {
            return this.BL2FileName_cqtx(sScale, B, L, ref fileName);
        }

        // <summary>
        // 根据图名解析图廓左下角经纬度坐标（旧图号 H-48-94-3）
        //'Co. Ltd.
        //'***********************************************
        //' Description:
        //'   根据图名解析图廓左下角经纬度坐标——土详标准
        //' Examples:
        //'   图名规范执行：重庆土地利用现状标准图幅数据文件命名规则，例如 H-48-94-(19), H-48-94-19，其中：
        //'   老图号如H-48-94-(4)，H-48为一幅1:100万的图，下面分为144幅1:10万的图，编号由左到右，从上到下，94为第94幅图，
        //'   每个1:10万图又可分为64幅1:1万图，编号规则跟上面一样，4表示第四幅图。
        //'   新图号没有分两级，而是1:100万下的96*96幅1:1万图按行列号表示，如H48 G 057075. --?
        //'   H-48-94-3：106.3730,29.3730, 106.4115,29.4000
        //'   H-48-94-4：106.4115,29.3730, 106.4500,29.4000
        //'   H-48-94-10：106.3345,29.3500, 106.3730,29.3730: 3'45", 2'30"
        //'   H-48-94-11：106.3730,29.3500, 106.4115,29.3730
        //'   H-48-94-12：106.4115,29.3500, 106.4500,29.3730
        //'   H-48-94-19：106.3730,29.3230, 106.4115,29.3500
        //' Parameters:
        //'   filename           - 输入，图幅文件名，可能带有后缀
        //'   B, L               - 输出，图幅左下角经纬度坐标
        //'   DeltaB, DeltaL,    - 输出，图幅经纬度坐标跨度
        /// </summary>
        /// <param name="fileName">图幅号</param>
        /// <param name="B">经度</param>
        /// <param name="L">纬度</param>
        /// <param name="deltaB">经度差</param>
        /// <param name="deltaL">纬度差</param>
        /// <returns></returns>
        private bool FileName2BL_cqtx(string fileName,
                                      ref double B,
                                      ref double L,
                                      ref double deltaB,
                                      ref double deltaL)
        {
            long row;
            long col;
            long bigrow;
            long bigcol;
            string temp;
            long a, bb, c, d;
            //double addlongitude;
            //int pos;
            //string sTemp;
            int Mod1, Mod2;

            // ma
            char[] splitChar = new char[1];
            string[] oldTFHArr = null;
            string tempStr = "";
            bool isok = true;
            int row5Q = 0;
            int col5Q = 0;
            //double deltaB5q = 0;
            //double deltaL5q = 0;
            double b5q = 0;
            double l5q = 0;
            int e = 0;

            try
            {
                if (fileName != "" && fileName != null)
                {
                    fileName = fileName.ToLower();
                    fileName = fileName.Replace('(', ' ');
                    fileName = fileName.Replace(')', ' ');
                    //name = fileName.ToUpper();
                    //pos = 0;

                    //'================================
                    //'下面开始对图幅编号进行解析。 例：    'H -48 - 94 - (19)
                    //ma 2007-6-6

                    splitChar[0] = '-';
                    oldTFHArr = fileName.Split(splitChar);
                    tempStr = oldTFHArr.GetValue(0).ToString();
                    bigrow = System.Convert.ToInt16(char.Parse(tempStr));  // 1:100万图幅列号，应该为叫行号

                    tempStr = oldTFHArr.GetValue(1).ToString();
                    bigcol = System.Convert.ToInt16(tempStr); // 1:100万图幅行号，应该叫列号

                    tempStr = oldTFHArr.GetValue(2).ToString();
                    row = System.Convert.ToInt16(tempStr);    // 1:10万图幅号

                    tempStr = oldTFHArr.GetValue(3).ToString().Trim();
                    col = System.Convert.ToInt16(tempStr);   // 1:1万图幅号

                    //ma 2007-6-6  1：5000 的处理
                    if (oldTFHArr.Length >= 5)
                    {
                        if (fileName.Contains("a") || fileName.Contains("b") || fileName.Contains("c") || fileName.Contains("d")) //1:5000
                        {
                            deltaL = 1.0 / 60.0 + 52.5 / 3600.0;      //1'52.5" 纬度差
                            deltaB = 1.0 / 60.0 + 15.0 / 3600.0;      //1'15"   经度差

                            temp = oldTFHArr.GetValue(oldTFHArr.Length - 1).ToString();
                            switch (temp)
                            {
                                case "a":
                                    row5Q = 1;
                                    col5Q = 1;
                                    e = 1;
                                    break;
                                case "b":
                                    row5Q = 1;
                                    col5Q = 2;
                                    e = 2;
                                    break;
                                case "c":
                                    row5Q = 2;
                                    col5Q = 1;
                                    e = 3;
                                    break;
                                case "d":
                                    row5Q = 2;
                                    col5Q = 2;
                                    e = 4;
                                    break;
                            }
                            b5q = row5Q * deltaL;
                            l5q = col5Q * deltaB;
                        }
                    }
                    else
                    {
                        deltaL = 0.0625;   //3'45"
                        deltaB = 1.0 / 24.0; //2'30"
                    }

                    a = (bigrow - 97) + 1;
                    bb = bigcol;
                    c = row;
                    d = col;

                    double c1;
                    c1 = (c - 1) / 12.0;
                    Mod1 = System.Convert.ToInt16((c - 1) - Math.Truncate(c1) * 12);

                    double d1;
                    d1 = (d - 1) / 8.0;
                    Mod2 = System.Convert.ToInt16((d - 1) - Math.Truncate(d1) * 8);

                    //ma 2007-6-6
                    double e1;
                    int Mod3 = 0;
                    if (oldTFHArr.Length >= 5) //1:5000　的处理
                    {
                        e1 = (e - 1) / 2.0;
                        Mod3 = System.Convert.ToInt16((e - 1) - Math.Truncate(e1) * 2);
                        L = (bb - 1.0) * 6.0 + Mod1 * 0.5 + Mod2 * 0.0625 + Mod3 * deltaL;
                    }
                    else
                    {
                        L = (bb - 1.0) * 6.0 + Mod1 * 0.5 + Mod2 * 0.0625;
                    }
                    //e1 = (e - 1) / 2.0;
                    //int Mod3 = System.Convert.ToInt16((e - 1) - Math.Truncate(e1) * 2);

                    //L = (bb - 1.0) * 6.0 + Mod1 * 0.5 + Mod2 * 0.0625 + Mod3 * deltaL;
                    if (L > 180) L = L - 180;
                    //B = (a - 1.0) * 4.0 + (11.0 - (int)((c - 1) / 12)) / 3.0 + (7.0 - (int)((d - 1) / 8)) / 24.0 ;

                    //ma 2007-6-6
                    B = (a - 1.0) * 4.0 + (11.0 - (int)((c - 1) / 12)) / 3.0 + (7.0 - (int)((d - 1) / 8)) / 24.0 + (1.0 - (int)((e - 1) / 2)) / 48.0;
                }
            }
            catch
            {
                isok = false;
            }
            return isok;
        }

        //
        /// <summary>
        /// 根据经纬度度生成图幅号（旧图号 重庆土祥）
        /// </summary>
        /// <param name="sScale">比例尺字符：H,G....</param>
        /// <param name="B">经度</param>
        /// <param name="L">纬度</param>
        /// <param name="fileName">生成的图幅号（旧图号）</param>
        /// <returns>执行是否成功</returns>
        private bool BL2FileName_cqtx(string sScale, double B, double L, ref string fileName)
        {
            long a, bb;
            string Code;
            double stepB, stepL;
            long c, d, e, f;
            char mChar, nChar;

            stepB = 0;
            stepL = 0;

            Code = sScale;
            mChar = Code[0];
            nChar = 'b';
            //System.Text.ASCIIEncoding

            //ascii = Encoding.ASCII;
            if (sScale == null || sScale.Trim() == "")
            {
                return false;
            }


            if ((int)mChar < (int)nChar)
            {
                mChar = Convert.ToChar(((int)(mChar) + (int)('a') - (int)('A')));
                Code = mChar.ToString();
            }

            if (((int)(mChar) < (int)('b')) | ((int)(mChar) > (int)('h')))
            {
                return false;
            }

            switch (Code.ToUpper())
            {
                case "B": //1:250000   已经小写过了
                    stepL = 1.5;    //1度30'
                    stepB = 1.0;     //1度
                    break;
                case "C": //1:200000
                    stepL = 1.5;      //1度30'
                    stepB = 1.0;      //1度
                    break;
                case "D": //1:100000
                    stepL = 0.5;    //30'
                    stepB = 1.0 / 3.0; //20'
                    break;
                case "E": //1:50000
                    stepL = 0.25;   //15'
                    stepB = 1.0 / 6.0; // 10'
                    break;
                case "F": //1:25000
                    stepL = 0.125;      //7'30"
                    stepB = 1.0 / 12.0; //'5'
                    break;
                case "G": //1:10000
                    stepL = 0.0625; //3'45"
                    stepB = 1.0 / 24.0; //2'30"
                    break;
                case "H": //1:5000
                    stepL = 0.03125; //1'52.5"
                    stepB = 1.0 / 48.0; //1'15"
                    break;
                default:
                    return false;
            }

            //ma 2007-6-6
            a = Convert.ToInt32(Math.Truncate(B / 4.0 + 1));
            bb = Convert.ToInt32(Math.Truncate(L / 6.0 + 1));
            //a = Convert.ToInt32(Math.Truncate(B / 4.0) + 1);
            //bb = Convert.ToInt32(Math.Truncate(L / 6.0) + 1);
            c = Convert.ToInt32(11 - Math.Truncate((B - (a - 1.0) * 4.0) * 3.0 + 0.0000001));
            d = Convert.ToInt32(Math.Truncate((L - (bb - 1.0) * 6.0) / 0.5 + 1));

            //d = Convert.ToInt32((L - (bb - 1.0) * 6.0) / 0.5) + 1;

            e = Convert.ToInt32(7 - Math.Truncate(((B - (a - 1.0) * 4.0) * 24.0 + 0.0000001 - (11 - c) * 8.0)));
            f = Convert.ToInt32(Math.Truncate(((L - (bb - 1.0) * 6.0) - (d - 1) * 0.5) / 0.0625 + 1));
            //f = Convert.ToInt32(((L - (bb - 1.0) * 6.0) - (d - 1) * 0.5) / 0.0625) + 1;

            //ma 2007-6-6 
            int g = 0;
            int h = 0;
            string scale5q = "";
            if (Code.ToUpper() == "H") //1:5000
            {
                g = Convert.ToInt32(2 - Math.Truncate((B - (a - 1.0) * 4.0 - (11 - c) / 3.0 - (7 - e) / 24.0) * 48.0 + 0.0000001));
                //g = Convert.ToInt32(2 - Math.Truncate((B - (a - 1.0) * 4.0 - (11 - e) / 24.0) * 48.0 + 0.0000001));

                //if ((L - (bb - 1.0) * 6.0 - (d - 1) * 0.5 - (f - 1) * 0.0625) % 0.03125==0)
                //{
                //    h = Convert.ToInt32(Math.Truncate((L - (bb - 1.0) * 6.0 - (d - 1) * 0.5 - (f - 1) * 0.0625) / 0.03125));
                //}
                //else
                //{
                h = Convert.ToInt32(Math.Truncate((L - (bb - 1.0) * 6.0 - (d - 1) * 0.5 - (f - 1) * 0.0625) / 0.03125 + 1));
                //}
                switch ((g - 1) * 2 + h)
                {
                    case 1:
                        scale5q = "a";
                        break;
                    case 2:
                        scale5q = "b";
                        break;
                    case 3:
                        scale5q = "c";
                        break;
                    case 4:
                        scale5q = "d";
                        break;
                }
            }

            if (bb < 30) bb = bb + 30; //保证在东经

            if (Code.ToUpper() == "H")
            {
                fileName = Convert.ToChar((int)('A') + a - 1).ToString() +
                "-" +
                bb.ToString() +
                "-" +
                Convert.ToString(c * 12 + d) +
                "-(" +
                Convert.ToString(e * 8 + f)
                + ")" + "-" + scale5q;
            }
            else
            {
                fileName = Convert.ToChar((int)('A') + a - 1).ToString() +
                "-" +
                bb.ToString() +
                "-" +
                Convert.ToString(c * 12 + d) +
                "-(" +
                Convert.ToString(e * 8 + f)
                + ")";
            }
            return true;
        }
        //
    }

    /// <summary>
    /// 国标(Standard) 图幅号转换类
    /// </summary>
    public class TFHConvertClass_Standard : AbsTFHConvertClass
    {

        public override bool FileName2BL(string fileName, ref double B, ref double L, ref double deltaB, ref double deltaL)
        {
            return this.FileName2BL_standard(fileName, ref B, ref L, ref deltaB, ref deltaL);
        }

        public override bool BL2FileName(string sScale, double B, double L, ref string fileName)
        {
            return BL2FileName_standard(sScale, B, L, ref fileName);
        }

        /// <summary>
        //图名解析图廓左下角经纬度[国家标准]
        //'***********************************************
        //' 图名解析与合成——国标
        //' , 0205
        //' , 0207
        //' Chengdu KODY Co. Ltd.
        //' 0307    VC->VB(将原来的VC版程序改成VB代码)
        //'Co. Ltd.
        //'***********************************************
        //' Description:
        //'   根据图名解析图廓左下角经纬度坐标
        //' Examples:
        //'   图名规范执行国标，例如1:50,000地形图图幅编号：J50E015008，其中：
        //'   J50是1:1,000,000图幅编号，J代表从赤道（A）起按纬度4°的编号，50代表从经度180°起自西向东按经度6°的编号；
        //'   E是比例尺代码，代表1:50,000；
        //'   015代表在J50图幅内自北向南第15行（每行10′）；
        //'   008代表在J50图幅内自西向东第8列（每列15′）；
        //' Parameters:
        //'   filename           - 输入，图幅文件名，可能带有后缀
        //'   B, L               - 输出，图幅左下角经纬度坐标
        //'   DeltaB, DeltaL,    - 输出，图幅经纬度坐标跨度
        //' Notes:
        //'   得到的经度为[0,360)度,东经均大于180度,没有等于360的
        //' Return Value:
        //'   false - 不能正确解析图名，可能不符合图幅命名规范
        //'   true  - 正常执行
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="deltaB"></param>
        /// <param name="deltaL"></param>
        /// <returns></returns>
        private bool FileName2BL_standard(string fileName,
                                ref double B,
                                ref double L,
                                ref double deltaB,
                                ref double deltaL)
        {
            long row, col;
            long bigrow, bigcol;
            string Code;
            string mFileName;
            long a, bb, c, d;
            double addlongitude, addlatitude;
            long pos;
            string mStr;

            pos = 0;

            if ((fileName == "") | (fileName == null))
            {
                return false;
            }
            mFileName = fileName.ToLower();


            if (mFileName.Length < 10)
            {
                return false;
            }
            mStr = mFileName.Substring(0, 1);
            bigrow = System.Convert.ToInt16(mStr);

            mStr = mFileName.Substring(1, 2);
            bigcol = System.Convert.ToInt16(mStr);

            Code = mFileName.Substring(3, 1);


            mStr = mFileName.Substring(4, 3);
            row = System.Convert.ToInt16(mStr);

            mStr = mFileName.Substring(7, 3);
            col = System.Convert.ToInt16(mStr);


            switch (Code.ToUpper())
            {
                case "B": // 1:250000   
                    addlongitude = 1.5;//     //'1度30'
                    addlatitude = 1.0;       //'1度
                    break;
                case "C": //1:200000
                    addlongitude = 1.5; //      '1度30'
                    addlatitude = 1.0;//       '1度
                    break;
                case "D":// 1:100000
                    addlongitude = 0.5;     //'30'
                    addlatitude = 1.0 / 3.0;  //'20'
                    break;
                case "E": //1:50000
                    addlongitude = 0.25;    //'15'
                    addlatitude = 1.0 / 6.0;  //'10'
                    break;
                case "F":// 1:25000
                    addlongitude = 0.125;   //'7'30"
                    addlatitude = 1.0 / 12.0; //'5'
                    break;
                case "G":// 1:10000
                    addlongitude = 0.0625;  //3'45"
                    addlatitude = 1.0 / 24.0; //2'30"
                    break;
                case "H"://1:5000
                    addlongitude = 0.03125; //1'52.5"
                    addlatitude = 1.0 / 48.0; //1'15"
                    break;
                case "I": //1:2000
                    addlongitude = 0.03125;   //1'52.5"
                    addlatitude = 1.0 / 48.0;   //1'15"
                    break;
                default:
                    return false;

            }
            a = (bigrow - 97) + 1;
            bb = bigcol;
            //if (bb >= 30) bb = bb - 30; //保证在东经
            c = row;
            d = col;

            L = (bb - 1.0) * 6.0 + (d - 1.0) * addlongitude;
            B = (a - 1.0) * 4.0 + (4.0 / addlatitude - c) * addlatitude;

            deltaL = addlongitude;
            deltaB = addlatitude;
            return true;
        }


        /// <summary>
        /// 根据经纬度生成图号（新图号）
        /// </summary>
        /// <param name="sScale">比例尺</param>
        /// <param name="B">经度</param>
        /// <param name="L">纬度</param>
        /// <param name="fileName">生成的图幅号（新图号）</param>
        /// <returns>执行是否成功</returns>
        //public bool BL2FileName_tx2(string sScale,
        //                double B,
        //                double L,
        //                ref string fileName)
        //{
        //        //long  bb ;
        //string Code;
        //double stepB ;
        //double stepL ;
        //double cc ;
        //long c , d ;

        //if ((int)(Code[0]) < (int)('b'))
        //{
        //    mChar = Convert.ToChar((int)(Code[0]) + (int)('a') - (int)('A'));
        //    Code = mChar.ToString();
        //}

        //if (((int)(Code[0]) < (int)('b')) | ((int)(Code[0]) > (int)('h')))
        //{
        //    return false;
        //}
        //switch (Code.ToUpper())
        //{
        //    case "B": //1:250000   已经小写过了
        //        stepL = 1.5;    //1度30'
        //        stepB = 1.0;     //1度
        //        break;
        //    case "C": //1:200000
        //        stepL = 1.5;      //1度30'
        //        stepB = 1.0;      //1度
        //        break;
        //    case "D": //1:100000
        //        stepL = 0.5;    //30'
        //        stepB = 1.0 / 3.0; //20'
        //        break;
        //    case "E": //1:50000
        //        stepL = 0.25;   //15'
        //        stepB = 1.0 / 6.0; // 10'
        //        break;
        //    case "F": //1:25000
        //        stepL = 0.125;      //7'30"
        //        stepB = 1.0 / 12.0; //'5'
        //        break;
        //    case "G": //1:10000
        //        stepL = 0.0625; //3'45"
        //        stepB = 1.0 / 24.0; //2'30"
        //        break;
        //    case "H": //1:5000
        //        stepL = 0.03125; //1'52.5"
        //        stepB = 1.0 / 48.0; //1'15"
        //        break;
        //    default:
        //        return false;
        //}


        ////        1：1百万图幅行、列号的计算：
        ////        a = [φ／4°] + 1
        ////        B = [λ／6°] + 31
        ////  式中：[ ]表示商取整；
        ////        a 表示百万分之一图幅所在纬度带数字码所对应的字母码；
        ////'        b表示百万分之一图幅所在经度带的数字码；
        ////        λ表示图幅内某点的经度或图幅西南廓点的经度；
        ////        φ表示图幅内某点的纬度或图幅西南廓点的纬度?
        ////相应比例尺的图幅行?列号的计算:
        ////        c ＝４°／Δφ-[(φ／4°)／Δφ]
        ////        d = [(λ/6°)／Δλ] + 1
        ////  式中：（ ）表示商取余；
        ////        [  ]表示商取整；

        ////瞿永志根据国标文档的算法：
        //a = Fix(B / 4#) + 1
        //bb = Fix(L / 6#) + 31
        //cc = (B - 4 * (B \ 4)) / stepB
        //if( cc < 0 )
        //{
        //    c = 4# / stepB - Fix(Fix(((B - (a - 1#) * 4#) / stepB) * 10000 + 1) / 10000)
        //}
        //else
        //{
        //    if( cc - Fix(cc) > 0.999999 Then cc = Round(cc))
        //    c = 4 / stepB - Fix(cc)
        //}

        //cc = (L - 6 * (L \ 6)) / stepL
        //If cc - Fix(cc) > 0.999999 Then cc = Round(cc)
        //d = Fix(cc) + 1

        //If Asc(Code) >= 104 Then
        //    //1:2000 1:5000
        //    fileName = Chr(Asc(Code) - Asc("a") + Asc("A")) & Chr(Asc("A") + a - 1) & bb & Format(c, "000") & Format(d, "000")
        //Else
        //    fileName = Chr(Asc(Code) - Asc("a") + Asc("A")) & Chr(Asc("A") + a - 1) & bb & Format(c, "00") & Format(d, "00")
        //End If
        //BL2FileName_tx = 0
        //}

        /// <param name="fileName">生成的图幅号</param>
        /// <returns>执行是否成功</returns>
        private bool BL2FileName_standard(string sScale,
                                 double B,
                                 double L,
                                 ref string fileName)
        {
            long a, bb;
            string Code;
            double stepB, stepL;
            long c, d;
            char mChar;
            Code = sScale;
            if ((Convert.ToInt16(Code) < Convert.ToInt16("B")) | (Convert.ToInt16(Code) > Convert.ToInt16("H") &
                    Convert.ToInt16(Code) < Convert.ToInt16("b")) | (Convert.ToInt16(Code) > Convert.ToInt16("h")))
            {
                return false;
            }
            if (Convert.ToInt16(Code) < Convert.ToInt16("b"))
            {
                mChar = Convert.ToChar((Convert.ToInt16(Code) + Convert.ToInt16("a") - Convert.ToInt16("A")));
                Code = mChar.ToString();
            }

            switch (Code.ToUpper())
            {
                case "B":// 1:500000   已经小写过了
                    stepL = 3.0;     //3度
                    stepB = 2.0;     //2度
                    break;
                case "C": //1:250000
                    stepL = 1.5;    //1度30'
                    stepB = 1.0;     //1度
                    break;
                case "D": //1:100000
                    stepL = 0.5;    //30'
                    stepB = 1.0 / 3.0; //20'
                    break;
                case "E": //1:50000
                    stepL = 0.25;   //15'
                    stepB = 1.0 / 6.0; //10'
                    break;
                case "F": //1:25000
                    stepL = 0.125;      //7'30"
                    stepB = 1.0 / 12.0; //5'
                    break;
                case "G": //1:10000
                    stepL = 0.0625; //3'45"
                    stepB = 1.0 / 24.0; //2'30"
                    break;
                case "H":// 1:5000
                    stepL = 0.03125; //1'52.5"
                    stepB = 1.0 / 48.0; //1'15"
                    break;
                default:
                    return false;

            }

            a = Convert.ToInt16(B / 4.0) + 1;
            bb = Convert.ToInt16(L / 6.0) + 1;
            c = Convert.ToInt16(4.0 / stepB) - Convert.ToInt16((B - (a - 1.0) * 4.0) / stepB);
            d = Convert.ToInt16((L - (bb - 1.0) * 6.0) / stepL) + 1;

            fileName = Convert.ToChar(Convert.ToInt16("A") + a - 1).ToString() +
                    bb +
                    Convert.ToChar(Convert.ToInt16(Code) - Convert.ToInt16("a") + Convert.ToInt16("A")).ToString() +
                    c +
                    d;
            return true;
        }
       
    }




    /// <summary>
    /// 高斯矩形
    /// /**************************
    ///     *LeftUp_Point          RightUp_Point
    ///     *            -----------
    ///     *            |         |
    ///     *            |         |
    ///     *            -----------
    ///     *LeftLow_Point         RightLow_Point 
    ///     * ***********************/
    /// </summary>
    public struct TFgsRect
    {
        public TFPoint LeftLow_Point;
        public TFPoint RightUp_Point;

        public TFPoint LeftUp_Point;
        public TFPoint RightLow_Point;
    }

    public struct TFPoint
    {
        public double X;
        public double Y;
        public double Z;

        public TFPoint(double pX, double pY)
        {
            X = pX;
            Y = pY;
            Z = 0;
        }

        public TFPoint(double pX,double pY,double pZ)
        {
             X=pX;
             Y=pY;
             Z=pZ;
        }

        public override string ToString()
        {
            return "X=" + X + ",Y=" + Y + ",Z=" + Z;
        }
    }

    public struct BLPoint
    {
        public double B;
        public double L;
        public double H;

        public BLPoint(double pB, double pL)
        {
            B = pB;
            L = pL;
            H = 0;
        }

        public BLPoint(double pB, double pL, double pH)
        {
            B = pB;
            L = pL;
            H = pH;
        }

        public override string ToString()
        {
            return "B=" + B + ",L=" + L + ",H=" + H;
        }
    }

    /// <summary>
    /// 经纬度矩形
    /// /**************************
    ///     *LeftUp_Point          RightUp_Point
    ///     *            -----------
    ///     *            |         |
    ///     *            |         |
    ///     *            -----------
    ///     *LeftLow_Point         RightLow_Point 
    ///     * ***********************/
    /// </summary>
    public struct TFBLRect
    {
        public BLPoint LeftLow_Point;
        public BLPoint RightUp_Point;

        public BLPoint LeftUp_Point;
        public BLPoint RightLow_Point;
    }



}
