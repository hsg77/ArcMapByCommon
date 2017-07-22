using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByCommon
{
    /// <summary>
    /// 抽象类 角度单位
    /// </summary>
    public abstract class AbstractAngleUnit : IAngleUnit
    {
        private IAngleUnit m_AngleUnit = null;

        protected const decimal PI = 3.14159265358979M;
        protected const decimal AngleToArc_xs = PI / 180.0M;
        protected const decimal ArcToAngle_xs = 180.0M / PI;

        public AbstractAngleUnit()
        {
        }

        public AbstractAngleUnit(IAngleUnit pAngleUnit)
        {
            m_AngleUnit = pAngleUnit;
        }

        #region IAngleUnit 成员

        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns></returns>
        public virtual object getAngle()
        {
            return 0;
        }

        protected decimal innerArcDValue = 0.0M;
        /// <summary>
        /// 获取弧度值innerArcDValue
        /// </summary>
        /// <returns></returns>
        public decimal getArcDValue()
        {
            return innerArcDValue;
        }


        public virtual enumAngleUnit enumAngleUnit
        {
            get { return enumAngleUnit.弧度; }
        }





        #endregion

        /// <summary>
        /// 角度转为弧度
        /// </summary>
        /// <param name="Angle"></param>
        /// <returns></returns>
        protected decimal AngleToArc(decimal Angle)
        {
            return Angle * AngleToArc_xs;
        }

        /// <summary>
        /// 弧度转为角度
        /// </summary>
        /// <param name="Arc"></param>
        /// <returns></returns>
        protected decimal ArcToAngle(decimal Arc)
        {
            return Arc * ArcToAngle_xs;
        }


    }

    /// <summary>
    /// 角度单位 接口
    /// </summary>
    public interface IAngleUnit
    {
        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns></returns>
        object getAngle();

        /// <summary>
        /// 获取弧度值
        /// </summary>
        /// <returns></returns>
        decimal getArcDValue();

        /// <summary>
        /// 角度单位枚举
        /// </summary>
        enumAngleUnit enumAngleUnit { get; }
    }

    public enum enumAngleUnit
    {
        度 = 0,
        度分秒 = 1,
        弧度 = 2,
    }

    //OK
    /// <summary>
    /// 角度单位  : 度
    /// </summary>
    public class AngleUnit_D : AbstractAngleUnit
    {
        private decimal m_d = 0.0M;
        public AngleUnit_D(decimal d)
            : base()
        {
            this.m_d = d;
            //度转弧度
            this.innerArcDValue = this.AngleToArc(d);
        }


        public AngleUnit_D(IAngleUnit pAngleUnit)
            : base(pAngleUnit)
        {
            //转换
            this.m_d = this.ArcToAngle(pAngleUnit.getArcDValue());
            this.innerArcDValue = pAngleUnit.getArcDValue();
        }

        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns>double</returns>
        public override object getAngle()
        {
            return this.m_d;
        }

        public override enumAngleUnit enumAngleUnit
        {
            get
            {
                return enumAngleUnit.度;
            }
        }
    }

    //OK
    /// <summary>
    /// 角度单位  : 弧度
    /// </summary>
    public class AngleUnit_ArcD : AbstractAngleUnit
    {
        private decimal m_Arcd = 0.0M;
        public AngleUnit_ArcD(decimal Arcd)
            : base()
        {
            this.m_Arcd = Arcd;
            this.innerArcDValue = Arcd;
        }



        public AngleUnit_ArcD(IAngleUnit pAngleUnit)
            : base(pAngleUnit)
        {

            //转换
            this.m_Arcd = pAngleUnit.getArcDValue();
            this.innerArcDValue = pAngleUnit.getArcDValue();
        }

        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns>double</returns>
        public override object getAngle()
        {
            return this.m_Arcd;
        }

        public override enumAngleUnit enumAngleUnit
        {
            get
            {
                return enumAngleUnit.弧度;
            }
        }
    }

    //OK
    /// <summary>
    /// 角度单位  : 度分秒 格式:0°0′0″″
    /// </summary>
    public class AngleUnit_DMS : AbstractAngleUnit
    {
        private string m_dms = "0°0′0″";
        public AngleUnit_DMS(string dms)
            : base()
        {
            this.m_dms = dms;
            this.innerArcDValue = this.AngleToArc(this.DmsToAngle(dms));
        }



        public AngleUnit_DMS(IAngleUnit pAngleUnit)
            : base(pAngleUnit)
        {
            //转换
            this.m_dms = this.AngleToDMS(this.ArcToAngle(pAngleUnit.getArcDValue()));
            this.innerArcDValue = pAngleUnit.getArcDValue();
        }

        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns>string =0°1′15″</returns>
        public override object getAngle()
        {
            return this.m_dms;
        }

        public override enumAngleUnit enumAngleUnit
        {
            get
            {
                return enumAngleUnit.度分秒;
            }
        }

        /// <summary>
        /// 度分秒转为度 格式：0°0′0″-> 0.0度
        /// </summary>
        /// <param name="dms">输入度分秒 格式：0°0′0″</param>
        /// <returns>返回度(double)</returns>
        protected decimal DmsToAngle(string dms)
        {
            decimal angle = 0.0M;
            try
            {
                if (dms.IndexOf('°') > 0 && dms.IndexOf('′') > 0 && dms.IndexOf('″') > 0)
                {
                    string[] dmsArray = dms.Split(new char[] { '°', '′', '″' });
                    string d = dmsArray[0]; //度
                    string m = dmsArray[1]; //分
                    string s = dmsArray[2]; //秒

                    int sgn = Math.Sign(decimal.Parse(d));

                    angle = Math.Abs(decimal.Parse(d)) + decimal.Parse(m) / 60.0M + decimal.Parse(s) / (60.0M * 60.0M);

                    angle = angle * sgn;

                }
                else
                {
                    angle = 0.0M;
                }
            }
            catch
            {
                angle = 0.0M;
            }
            return angle;

        }

        /// <summary>
        /// 度转为度分秒  格式： 0.0度->0°0′0″
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        protected string AngleToDMS(decimal angle)
        {
            string dms = "0°0′0″";

            try
            {
                int sgn = Math.Sign(angle);

                decimal d = 0;
                decimal m = 0;
                decimal s = 0;

                d = Math.Truncate(Math.Abs(angle));                    //取度
                m = Math.Truncate(Math.Abs(angle) * 60.0M - d * 60.0M);  //取分
                s = Math.Abs(angle) * 3600.0M - d * 3600.0M - m * 60.0M;  //取秒

                d = d * sgn;
                
                dms = d.ToString() + "°" + m.ToString() + "′" + Math.Round(s,0).ToString() + "″";
            }
            catch
            {
                dms = "0°0′0″";
            }

            return dms;

        }
    }

    //OK
    /// <summary>
    /// 角度单位  : 度分秒 格式:0:0:0″
    /// </summary>
    public class AngleUnit_DMS_Colon : AbstractAngleUnit
    {
        private string m_dms = "0:0:0";
        public AngleUnit_DMS_Colon(string dms)
            : base()
        {
            this.m_dms = dms;
            this.innerArcDValue = this.AngleToArc(this.DmsToAngle(dms));
        }



        public AngleUnit_DMS_Colon(IAngleUnit pAngleUnit)
            : base(pAngleUnit)
        {
            //转换
            this.m_dms = this.AngleToDMS(this.ArcToAngle(pAngleUnit.getArcDValue()));
            this.innerArcDValue = pAngleUnit.getArcDValue();
        }

        /// <summary>
        /// 获取角度值 double/string
        /// </summary>
        /// <returns>string =0:1:15</returns>
        public override object getAngle()
        {
            return this.m_dms;
        }

        public override enumAngleUnit enumAngleUnit
        {
            get
            {
                return enumAngleUnit.度分秒;
            }
        }

        /// <summary>
        /// 度分秒转为度 格式：0:0:0-> 0.0度
        /// </summary>
        /// <param name="dms">输入度分秒 格式：0:0:0</param>
        /// <returns>返回度(double)</returns>
        protected decimal DmsToAngle(string dms)
        {
            decimal angle = 0.0M;
            try
            {
                if (dms.IndexOf(':') > 0)
                {
                    string[] dmsArray = dms.Split(new char[] { ':' });
                    string d = dmsArray[0]; //度
                    string m = dmsArray[1]; //分
                    string s = dmsArray[2]; //秒

                    int sgn = Math.Sign(decimal.Parse(d));

                    angle = Math.Abs(decimal.Parse(d)) + decimal.Parse(m) / 60.0M + decimal.Parse(s) / (60.0M * 60.0M);

                    angle = angle * sgn;

                }
                else
                {
                    angle = 0.0M;
                }
            }
            catch
            {
                angle = 0.0M;
            }
            return angle;

        }

        /// <summary>
        /// 度转为度分秒  格式： 0.0度->0:0:0
        /// </summary>
        /// <param name="angle">0.0度</param>
        /// <returns>0:0:0</returns>
        protected string AngleToDMS(decimal angle)
        {
            string dms = "0:0:0";

            try
            {
                int sgn = Math.Sign(angle);

                decimal d = 0;
                decimal m = 0;
                decimal s = 0;

                d = Math.Truncate(Math.Abs(angle));                    //取度
                m = Math.Truncate(Math.Abs(angle) * 60.0M - d * 60.0M);  //取分
                s = Math.Abs(angle) * 3600.0M - d * 3600.0M - m * 60.0M;  //取秒

                d = d * sgn;                
                dms = d.ToString() + ":" + m.ToString() + ":" + Math.Round(s,0).ToString();
            }
            catch
            {
                dms = "0:0:0";
            }

            return dms;

        }
    }
}
