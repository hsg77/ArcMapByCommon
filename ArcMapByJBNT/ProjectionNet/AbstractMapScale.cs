using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    public interface IMapScale
    {
        /// <summary>
        /// 获取 获取
        /// </summary>
        int Scale { get; }

        /// <summary>
        /// 获取 描述
        /// </summary>
        string ScaleDescription { get; }

        /// <summary>
        /// 获取 比例尺代码
        /// I H G F E D C B 其中的一个
        /// </summary>
        string ScaleCode { get; }

        /// <summary>
        /// 获取 比例尺 枚举类型
        /// </summary>
        EnumMapScale MapScale { get; }

        /// <summary>
        /// 获取 经差
        /// </summary>
        decimal L2_L1 { get; }

        /// <summary>
        /// 获取 纬差
        /// </summary>
        decimal B2_B1 { get; }

    }

    public enum EnumMapScale
    {
        EnumMapScale_I_2000 = 2000,
        EnumMapScale_H_5000 = 5000,
        EnumMapScale_G_10000 = 10000,
        EnumMapScale_F_25000 = 25000,
        EnumMapScale_E_50000 = 50000,
        EnumMapScale_D_100000 = 100000,
        EnumMapScale_C_200000 = 200000,
        EnumMapScale_B_250000 = 250000,

        //50万---100万
        EnumMapScale_500000 = 500000,
        EnumMapScale_1000000 = 1000000,

    }

    /// <summary>
    /// 抽象类 比例尺
    /// 默认为1:5000
    /// </summary>
    public abstract class AbstractMapScale : IMapScale
    {
        public AbstractMapScale()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_H_5000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "H";
            this.m_ScaleDescription = "1:5千";
            this.m_L2_L1 = 0.03125M; //经差=1'52.5"
            this.m_B2_B1 = 1.0M / 48.0M; //纬差=1'15"
        }

        #region IMapScale 成员

        protected int m_Scale = 5000;
        public int Scale
        {
            get { return m_Scale; }
        }

        protected string m_ScaleDescription = "1:5千";
        public string ScaleDescription
        {
            get { return m_ScaleDescription; }
        }

        protected string m_ScaleCode = "H";
        public string ScaleCode
        {
            get { return m_ScaleCode; }
        }

        protected EnumMapScale m_MapScale = EnumMapScale.EnumMapScale_H_5000;
        public EnumMapScale MapScale
        {
            get { return m_MapScale; }
        }

        protected decimal m_L2_L1 = 0.03125M; //经差=1'52.5"
        public decimal L2_L1
        {
            get { return m_L2_L1; }
        }

        protected decimal m_B2_B1 = 1.0M / 48.0M; //纬差=1'15"
        public decimal B2_B1
        {
            get { return m_B2_B1; }
        }

        #endregion
    }

    /// <summary>
    /// 比例尺 I 2000 1:2千
    /// </summary>
    public class MapScale_2000 : AbstractMapScale
    {
        public MapScale_2000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_I_2000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "I";
            this.m_ScaleDescription = "1:2千";
            this.m_L2_L1 = 1.5M / 120.0M;
            this.m_B2_B1 = 1.0M / 120.0M;
        }
    }

    /// <summary>
    /// 比例尺 H 5000  1:5千
    /// </summary>
    public class MapScale_5000 : AbstractMapScale
    {
        public MapScale_5000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_H_5000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "H";
            this.m_ScaleDescription = "1:5千";
            this.m_L2_L1 = 0.03125M; //1'52.5"
            this.m_B2_B1 = 1.0M / 48.0M; //1'15"
        }
    }

    /// <summary>
    /// 比例尺 G_10000 1:1万
    /// </summary>
    public class MapScale_10000 : AbstractMapScale
    {
        public MapScale_10000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_G_10000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "G";
            this.m_ScaleDescription = "1:1万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("0°3′45″");
            this.m_L2_L1 = au.getArcDValue();  //3'45"

            au = new AngleUnit_DMS("0°2′30″");
            this.m_B2_B1 = au.getArcDValue();  //2'30"

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 F_25000 1:2.5万
    /// </summary>
    public class MapScale_25000 : AbstractMapScale
    {
        public MapScale_25000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_F_25000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "F";
            this.m_ScaleDescription = "1:2.5万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("0°7′30″");
            this.m_L2_L1 = au.getArcDValue();  //7'30"

            au = new AngleUnit_DMS("0°5′0″");
            this.m_B2_B1 = au.getArcDValue();  //5'

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 E_50000  1:5万
    /// </summary>
    public class MapScale_50000 : AbstractMapScale
    {
        public MapScale_50000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_E_50000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "E";
            this.m_ScaleDescription = "1:5万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("0°15′0″");
            this.m_L2_L1 = au.getArcDValue();  //15'

            au = new AngleUnit_DMS("0°10′0″");
            this.m_B2_B1 = au.getArcDValue();  //10'

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 D_100000  1:10万
    /// </summary>
    public class MapScale_100000 : AbstractMapScale
    {
        public MapScale_100000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_D_100000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "D";
            this.m_ScaleDescription = "1:10万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("0°30′0″");
            this.m_L2_L1 = au.getArcDValue();  //30'

            au = new AngleUnit_DMS("0°20′0″");
            this.m_B2_B1 = au.getArcDValue();  //20'

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 C_200000   1:20万
    /// </summary>
    public class MapScale_200000 : AbstractMapScale
    {
        public MapScale_200000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_C_200000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "C";
            this.m_ScaleDescription = "1:20万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("1°0′0″");
            this.m_L2_L1 = au.getArcDValue();  //1°

            au = new AngleUnit_DMS("0°40′0″");
            this.m_B2_B1 = au.getArcDValue();  //40'

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 B_250000  1:25万
    /// </summary>
    public class MapScale_250000 : AbstractMapScale
    {
        public MapScale_250000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_B_250000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "B";
            this.m_ScaleDescription = "1:25万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("1°30′0″");
            this.m_L2_L1 = au.getArcDValue();  //1°30'

            au = new AngleUnit_DMS("1°0′0″");
            this.m_B2_B1 = au.getArcDValue();  //1°

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 500000  1:50万
    /// </summary>
    public class MapScale_500000 : AbstractMapScale
    {
        public MapScale_500000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_500000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "A";  //??
            this.m_ScaleDescription = "1:50万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("3°0′0″");
            this.m_L2_L1 = au.getArcDValue();  //3°

            au = new AngleUnit_DMS("2°0′0″");
            this.m_B2_B1 = au.getArcDValue();  //2°

            au = null;
        }
    }

    /// <summary>
    /// 比例尺 1000000  1:100万
    /// </summary>
    public class MapScale_1000000 : AbstractMapScale
    {
        public MapScale_1000000()
        {
            this.m_MapScale = EnumMapScale.EnumMapScale_1000000;
            this.m_Scale = (int)this.m_MapScale;
            this.m_ScaleCode = "Z";   //??
            this.m_ScaleDescription = "1:100万";

            AbstractAngleUnit au = null;

            au = new AngleUnit_DMS("6°0′0″");
            this.m_L2_L1 = au.getArcDValue();  //6°

            au = new AngleUnit_DMS("4°0′0″");
            this.m_B2_B1 = au.getArcDValue();  //4°

            au = null;
        }
    }

    //
}
