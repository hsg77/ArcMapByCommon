using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByCommon
{
    /// <summary>
    /// 高斯正反解坐标转换功能类
    /// </summary>
    public abstract class GSCoordConvertionClass : AbsProjectionConversionClass, IProjectionConversion2
    {
        #region IProjectionConversion2 成员
        //需要三个配置参数
        private bool m_IsBigNumber = true;
        private EnumStrip m_Strip = EnumStrip.Strip3;
        private decimal m_L0 = 105;

        public bool IsBigNumber
        {
            get
            {
                return m_IsBigNumber;
            }
            set
            {
                m_IsBigNumber = value;
            }
        }

        public EnumStrip Strip
        {
            get
            {
                return m_Strip;
            }
            set
            {
                m_Strip = value;
            }
        }

        public decimal L0
        {
            get
            {
                return m_L0;
            }
            set
            {
                m_L0 = value;
            }
        }

        public abstract void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L);

        public abstract void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y);
        

        #endregion
    }

    /// <summary>
    /// 北京54坐标转换功能类 (曲算法)
    /// </summary>
    public class GSCoordConvertionClass_Beijing54 : GSCoordConvertionClass, IProjectionConversion2
    {

        #region IProjectionConversion2 成员
        public GSCoordConvertionClass_Beijing54()
        {
            //需要三个配置参数
            IsBigNumber = true;
            Strip = EnumStrip.Strip3;
            L0 = 105;
        }  

        public override void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L)
        {
            this.GetBLFromXY(x, y, ref B, ref L, 0, EnumProjectionDatum.Bejing54, this.Strip, this.L0, this.IsBigNumber);
        }

        public override void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y)
        {
            this.GetXYFromBL(B, L, ref x, ref y, 0, EnumProjectionDatum.Bejing54, this.Strip, this.IsBigNumber);
        }

        #endregion
    }

    /// <summary>
    /// 西安80坐标转换功能类 (曲算法)
    /// </summary>
    public class GSCoordConvertionClass_Xian80 : GSCoordConvertionClass, IProjectionConversion2
    {
        public GSCoordConvertionClass_Xian80()
        {
            //需要三个配置参数
            IsBigNumber = true;
            Strip = EnumStrip.Strip3;
            L0 = 105;
        }        

        public override void GetBLFromXY(decimal x, decimal y, ref decimal B, ref decimal L)
        {
            this.GetBLFromXY(x, y, ref B, ref L, 0, EnumProjectionDatum.Xian80, this.Strip, this.L0, this.IsBigNumber);
        }

        public override void GetXYFromBL(decimal B, decimal L, ref decimal x, ref decimal y)
        {
            this.GetXYFromBL(B, L, ref x, ref y, 0, EnumProjectionDatum.Xian80, this.Strip, this.IsBigNumber);
        }

    }
    
}
