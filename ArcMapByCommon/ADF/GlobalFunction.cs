using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

namespace ArcMapByCommon
{
    public class GlobalFunction
    {
        //获取地图下的图层列表 递归组图层
        public static ILayer[] GetAllLayer(IMap map)
        {
            List<ILayer> layers = new List<ILayer>();

            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer l = map.get_Layer(i);
                if (l is ICompositeLayer)
                {
                    foreach (ILayer subLayer in GetAllLayer(l as ICompositeLayer))
                    {
                        layers.Add(subLayer);
                    }
                }

                layers.Add(l);
            }

            return layers.ToArray();
        }

        //获取组图层下的图层列表
        public static ILayer[] GetAllLayer(ICompositeLayer group_layer)
        {
            List<ILayer> layers = new List<ILayer>();

            for (int i = 0; i < group_layer.Count; i++)
            {
                ILayer l = group_layer.get_Layer(i);
                if (l is ICompositeLayer)
                {
                    foreach (ILayer subLayer in GetAllLayer(l as ICompositeLayer))
                    {
                        layers.Add(subLayer);
                    }
                }

                layers.Add(l);
            }

            return layers.ToArray();
        }
    }
}
