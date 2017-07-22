using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;

namespace ArcMapByCommon
{    
    public interface IZHLayer
    {
        /// <summary>
        /// 当前ILayer接口图层
        /// </summary>
        ILayer Layer { get; }

        /// <summary>
        /// 获取当前ILayer接口中的ZHFeatureClass对象
        /// </summary>
        /// <returns></returns>
        ZhFeatureClass getZHFeatureClass();

        /// <summary>
        /// 图层类型 点/线/面    ....注记?
        /// </summary>
        ZhLayerType LayerType { get; }
    }
    /// <summary>
    /// 对ILayer 进行封装
    /// </summary>
    public class ZHLayer : IZHLayer
    {
        public ZHLayer(ILayer pLayer)
        {
            this.m_Layer = pLayer;
        }

        #region IZHLayer 成员
        private ILayer m_Layer = null;
        public ILayer Layer
        {
            get
            {
                return m_Layer;
            }
        }

        private ZhFeatureClass m_ZHFeatureClass = null;
        public ZhFeatureClass getZHFeatureClass()
        {
            if (this.m_ZHFeatureClass == null && this.Layer != null && this.Layer is IFeatureLayer)
            {
                IFeatureLayer featLayer = this.Layer as IFeatureLayer;
                switch (featLayer.FeatureClass.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        this.m_ZHFeatureClass = new ZhPolygonFeatureClass(featLayer.FeatureClass);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                    case esriGeometryType.esriGeometryLine:
                        this.m_ZHFeatureClass = new ZhPolylineFeatureClass(featLayer.FeatureClass);
                        break;
                    case esriGeometryType.esriGeometryPoint:
                    case esriGeometryType.esriGeometryMultipoint:
                        this.m_ZHFeatureClass = new ZhPointFeatureClass(featLayer.FeatureClass);
                        break;
                    default:  //注记?
                        this.m_ZHFeatureClass = null;
                        break;
                }
            }
            return m_ZHFeatureClass;
        }

        public ZhLayerType LayerType
        {
            get
            {
                return getZHFeatureClass().LayerType;
            }
        }

        #endregion
    }

    public interface IZHRenderLayer
    {

        void Render();

    }

    public abstract class ZHRenderLayer : IZHRenderLayer
    {
        protected IFeatureLayer m_layer = null;
        public ZHRenderLayer(IFeatureLayer layer)
        {
            m_layer = layer;
        }

        protected void simpleRender(IFeatureLayer layer, IColor borderColor, double borderWidth)
        {
            ISimpleRenderer simpleRender = null;
            IFillSymbol fillSymbol = null;
            ILineSymbol lineSymbol = null;
            IColor fillColor = null;
            IGeoFeatureLayer geoLayer = null;

            if (layer != null && borderColor != null)
            {
                fillColor = new RgbColorClass();
                fillColor.NullColor = true;

                fillSymbol = new SimpleFillSymbolClass();
                lineSymbol = new SimpleLineSymbolClass();
                fillSymbol.Color = fillColor;

                lineSymbol.Color = borderColor;
                lineSymbol.Width = borderWidth;
                fillSymbol.Outline = lineSymbol;

                geoLayer = layer as IGeoFeatureLayer;
                if (geoLayer != null)
                {
                    simpleRender = new SimpleRendererClass();
                    simpleRender.Symbol = fillSymbol as ISymbol;
                    geoLayer.Renderer = simpleRender as IFeatureRenderer;
                }
            }
        }

        #region IZHRenderLayer 成员

        public virtual void Render()
        {
        }

        #endregion
    }

    //乡镇导航渲染
    public class ZHRenderLayer_NavitorXiangLayerClass : ZHRenderLayer
    {
        public ZHRenderLayer_NavitorXiangLayerClass(IFeatureLayer layer)
            : base(layer)
        {
        }

        public override void Render()
        {
            IRgbColor color = new RgbColorClass();
            color.Red = 133;
            color.Blue = 165;
            color.Green = 168;

            this.simpleRender(this.m_layer, color, 1.5);
        }

    }

    //分幅导航渲染
    public class ZHRenderLayer_NavitorFenFuLayerClass : ZHRenderLayer
    {
        public ZHRenderLayer_NavitorFenFuLayerClass(IFeatureLayer layer)
            : base(layer)
        {
        }

        public override void Render()
        {
            IRgbColor color = new RgbColorClass();
            color.Red = 0;
            color.Blue = 230;
            color.Green = 92;

            this.simpleRender(this.m_layer, color, 1);
        }
    }

    /// <summary>
    /// 自定义图层用于下拉控件以树型方式显示用
    /// vp:hsg
    /// create date:2011-12
    /// </summary>
    public class CustomLayer
    {
        public int TreeLevel = 0;
        private ILayer m_layer;

        public CustomLayer(ILayer layer)
        {
            m_layer = layer;
        }

        public ILayer Layer
        {
            get { return m_layer; }
        }

        public override string ToString()
        {
            if (m_layer != null)
            {
                string prix = "";
                for (int i = 0; i < TreeLevel; i++)
                {
                    prix += " ";
                }
                return prix + m_layer.Name;
            }
            return "";
        }

        public override bool Equals(object obj)
        {
            if (obj is CustomLayer)
            {
                if ((obj as CustomLayer).Layer != null && this.Layer != null)
                {
                    bool b = (obj as CustomLayer).Layer == this.Layer;
                    return b;
                }
            }
            return false;
        }
    }

}
