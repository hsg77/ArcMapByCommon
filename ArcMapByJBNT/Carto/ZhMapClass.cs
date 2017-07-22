using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ArcMapByJBNT
{        
    public class ZhMapClass : IZhMap
    {
        private IMap m_Map = null;

        public ZhMapClass(IMap pMap)
        {
            this.m_Map = pMap;
        }

        #region IZHMap 成员
        public IMap Map
        {
            get
            {
                return m_Map;
            }
        }
        //OK Loop
        public bool IsEdited
        {
            get
            {
                bool rbc = false;
                ILayer layer = null;
                if (this.Map != null)
                {
                    for (int i = 0; i < this.Map.LayerCount; i++)
                    {
                        layer = this.Map.get_Layer(i);
                        if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                        {
                            LoopLayerEditing(layer as ICompositeLayer);
                            break;
                        }
                        else if (layer is IFeatureLayer && (layer as IFeatureLayer).FeatureClass != null)
                        {
                            rbc = ((layer as IFeatureLayer).FeatureClass as IDatasetEdit).IsBeingEdited();
                            break;
                        }
                    }
                }
                return rbc;
            }
        }
        private bool LoopLayerEditing(ICompositeLayer layer)
        {
            bool rbc = false;
            ICompositeLayer cplayer = layer as ICompositeLayer;
            ILayer tmplayer = null;
            for (int j = 0; j < cplayer.Count; j++)
            {
                tmplayer = cplayer.get_Layer(j);
                if (tmplayer is ICompositeLayer)
                {
                    rbc = LoopLayerEditing(tmplayer as ICompositeLayer);
                }
                else if (tmplayer is IFeatureLayer && (tmplayer as IFeatureLayer).FeatureClass != null)
                {
                    IDatasetEdit dsEdit = (tmplayer as IFeatureLayer).FeatureClass as IDatasetEdit;
                    rbc = dsEdit.IsBeingEdited();
                }
                if (rbc == true)
                {
                    break;
                }
            }
            return rbc;
        }
        //OK Loop
        public List<string> GetWorkSpacePathList()
        {
            List<string> workspaceList = new List<string>();
            ILayer layer = null;
            IFeatureClass fc = null;
            string tmp_ws = "";
            if (this.Map != null)
            {
                for (int i = 0; i < this.Map.LayerCount; i++)
                {
                    layer = this.Map.get_Layer(i);
                    if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                    {
                        LoopGetWorkspacePathName(layer as ICompositeLayer, ref workspaceList);
                        break;
                    }
                    else if (layer is IFeatureLayer && (layer as IFeatureLayer).FeatureClass != null)
                    {
                        fc = (layer as IFeatureLayer).FeatureClass;
                        tmp_ws = (fc as IDataset).Workspace.PathName;
                        if (workspaceList.Contains(tmp_ws) == false)
                        {
                            workspaceList.Add(tmp_ws);
                        }
                    }
                }
            }
            return workspaceList;
        }
        private void LoopGetWorkspacePathName(ICompositeLayer layer, ref List<string> workspaceList)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            IFeatureClass fc = null;
            ILayer tmpLayer = null;
            string tmp_ws = "";
            for (int j = 0; j < cplayer.Count; j++)
            {
                tmpLayer = cplayer.get_Layer(j);
                if (tmpLayer is ICompositeLayer)
                {
                    LoopGetWorkspacePathName(tmpLayer as ICompositeLayer, ref workspaceList);
                }
                else if (tmpLayer is IFeatureLayer && (tmpLayer as IFeatureLayer).FeatureClass != null)
                {
                    fc = (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass;
                    tmp_ws = (fc as IDataset).Workspace.PathName;
                    if (workspaceList.Contains(tmp_ws) == false)
                    {
                        workspaceList.Add(tmp_ws);
                    }
                }
            }
        }
        //OK Loop
        public List<string> GetLayerNameOfWorkSpace(string WorkSpacePath)
        {
            List<string> fcNameList = new List<string>();
            ILayer layer = null;
            ICompositeLayer cplayer = null;
            IFeatureClass fc = null;
            string fcName = null;
            string tmp_ws = "";
            if (this.Map != null)
            {
                for (int i = 0; i < this.Map.LayerCount; i++)
                {
                    layer = this.Map.get_Layer(i);
                    if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                    {
                        //
                        break;
                    }
                    else if (layer is IFeatureLayer && (layer as IFeatureLayer).FeatureClass != null)
                    {
                        fcName = (layer as IFeatureLayer).Name;
                        fc = (layer as IFeatureLayer).FeatureClass;
                        tmp_ws = (fc as IDataset).Workspace.PathName;
                        if (tmp_ws == WorkSpacePath)
                        {
                            if (fcNameList.Contains(fcName) == false)
                            {
                                fcNameList.Add(fcName);
                            }
                        }
                    }
                }
            }
            return fcNameList;
        }
        private void LoopGetLayerNameOfWorkSpace(ICompositeLayer layer, ref List<string> fcNameList, string WorkSpacePath)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            ILayer tmplayer = null;
            IFeatureClass fc = null;
            string fcName = "";
            string tmp_ws = "";
            for (int j = 0; j < cplayer.Count; j++)
            {
                tmplayer = cplayer.get_Layer(j);
                if (tmplayer is ICompositeLayer)
                {
                    LoopGetLayerNameOfWorkSpace(tmplayer as ICompositeLayer, ref fcNameList, WorkSpacePath);
                }
                else if (tmplayer is IFeatureLayer && (tmplayer as IFeatureLayer).FeatureClass != null)
                {
                    fcName = (cplayer.get_Layer(j) as IFeatureLayer).Name;
                    fc = (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass;
                    tmp_ws = (fc as IDataset).Workspace.PathName;
                    if (tmp_ws == WorkSpacePath)
                    {
                        if (fcNameList.Contains(fcName) == false)
                        {
                            fcNameList.Add(fcName);
                        }
                    }
                }
            }
        }
        //OK Loop
        public List<string> GetFeatureClassNameOfWorkSpace(string WorkSpacePath)
        {
            List<string> fcNameList = new List<string>();
            ILayer layer = null;
            IFeatureClass fc = null;
            string fcName = null;
            string tmp_ws = "";
            if (this.Map != null)
            {
                for (int i = 0; i < this.Map.LayerCount; i++)
                {
                    layer = this.Map.get_Layer(i);
                    if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                    {
                        //
                        LoopGetFeatureClassNameOfWorkSpace(layer as ICompositeLayer, ref fcNameList, WorkSpacePath);
                    }
                    else if (layer is IFeatureLayer && (layer as IFeatureLayer).FeatureClass != null)
                    {
                        fc = (layer as IFeatureLayer).FeatureClass;
                        fcName = fc.AliasName;
                        tmp_ws = (fc as IDataset).Workspace.PathName;
                        if (tmp_ws == WorkSpacePath)
                        {
                            if (fcNameList.Contains(fcName) == false)
                            {
                                fcNameList.Add(fcName);
                            }
                        }
                    }
                }
            }
            return fcNameList;
        }
        private void LoopGetFeatureClassNameOfWorkSpace(ICompositeLayer layer, ref List<string> fcNameList, string WorkSpacePath)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            ILayer tempLayer = null;
            IFeatureClass fc = null;
            string fcName = "";
            string tmp_ws = "";
            for (int j = 0; j < cplayer.Count; j++)
            {
                tempLayer = cplayer.get_Layer(j);
                if (tempLayer is ICompositeLayer)
                {
                    LoopGetFeatureClassNameOfWorkSpace(tempLayer as ICompositeLayer, ref fcNameList, WorkSpacePath);
                }
                else if (tempLayer is IFeatureLayer && (tempLayer as IFeatureLayer).FeatureClass != null)
                {
                    fc = (tempLayer as IFeatureLayer).FeatureClass;
                    fcName = fc.AliasName;
                    tmp_ws = (fc as IDataset).Workspace.PathName;
                    if (tmp_ws == WorkSpacePath)
                    {
                        if (fcNameList.Contains(fcName) == false)
                        {
                            fcNameList.Add(fcName);
                        }
                    }
                }
            }
        }
        //OK
        public string GetDataSetName(IFeatureClass featureClass)
        {
            IDataset dataSet = featureClass as IDataset;
            ISQLSyntax sqlSyntax = dataSet.Workspace as ISQLSyntax;
            if (sqlSyntax != null)
            {
                string dataName = string.Empty;
                string dbName = string.Empty;
                string ownerName = string.Empty;
                sqlSyntax.ParseTableName(dataSet.Name, out dbName, out ownerName, out dataName);
                return dataName;
            }
            else
            {
                return dataSet.Name;
            }
        }
        //OK
        public IFeatureLayer GetFeatureLayer(string featureClassName)
        {
            return GetFeatureLayerByFeatureClassName(featureClassName);
        }
        //OK Loop
        public IFeatureLayer GetFeatureLayerByFeatureClassName(string featureClassName)
        {
            ILayer layer = null;
            IFeatureLayer fLayer = null;
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                layer = this.Map.get_Layer(i);
                if (layer is IFeatureLayer)
                {
                    fLayer = layer as IFeatureLayer;
                    if ((fLayer != null)
                        && (fLayer.FeatureClass != null)
                        && (fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple
                            || fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        && this.GetDataSetName(fLayer.FeatureClass).ToUpper() == featureClassName.ToUpper())
                    {
                        return fLayer;
                    }
                }
                else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                {
                    //
                    fLayer = LoopGetFeatureLayerByFeatureClassName(layer as ICompositeLayer, featureClassName);
                    if (fLayer != null)
                    {
                        return fLayer;
                    }
                }

            }
            return null;
        }
        private IFeatureLayer LoopGetFeatureLayerByFeatureClassName(ICompositeLayer layer, string featureClassName)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            IFeatureLayer fLayer = null;
            IFeatureClass fc = null;
            for (int j = 0; j < cplayer.Count; j++)
            {
                if (cplayer.get_Layer(j) is ICompositeLayer)
                {
                    fLayer = LoopGetFeatureLayerByFeatureClassName(cplayer.get_Layer(j) as ICompositeLayer, featureClassName);
                    if (fLayer != null)
                    {
                        return fLayer;
                    }
                }
                else if (cplayer.get_Layer(j) is IFeatureLayer && (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass != null)
                {
                    fLayer = cplayer.get_Layer(j) as IFeatureLayer;
                    fc = fLayer.FeatureClass;
                    if (this.GetDataSetName(fc).ToUpper() == featureClassName.ToUpper())
                    {
                        return fLayer;
                    }
                }
            }
            return null;
        }
        //OK Loop
        public IFeatureLayer GetFeatureLayerByLayerName(string LayerName)
        {
            ILayer layer = null;
            IFeatureLayer fLayer = null;
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                layer = this.Map.get_Layer(i);
                if (layer is IFeatureLayer)
                {
                    fLayer = layer as IFeatureLayer;
                    if ((fLayer != null)
                        && (fLayer.FeatureClass != null)
                        && (fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple
                            || fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        && fLayer.Name.ToUpper() == LayerName.ToUpper())
                    {
                        return fLayer;
                    }
                }
                else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                {
                    //
                    fLayer = LoopGetFeatureLayerByLayerName(layer as ICompositeLayer, LayerName);
                    if (fLayer != null)
                    {
                        return fLayer;
                    }
                }

            }
            return null;
        }
        private IFeatureLayer LoopGetFeatureLayerByLayerName(ICompositeLayer layer, string LayerName)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            IFeatureLayer fLayer = null;
            for (int j = 0; j < cplayer.Count; j++)
            {
                if (cplayer.get_Layer(j) is ICompositeLayer)
                {
                    fLayer = LoopGetFeatureLayerByLayerName(cplayer.get_Layer(j) as ICompositeLayer, LayerName);
                    if (fLayer != null)
                    {
                        return fLayer;
                    }
                }
                else if (cplayer.get_Layer(j) is IFeatureLayer && (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass != null)
                {
                    fLayer = cplayer.get_Layer(j) as IFeatureLayer;
                    if (fLayer.Name.ToUpper() == LayerName.ToUpper())
                    {
                        return fLayer;
                    }
                }
            }
            return null;
        }
        //OK Loop
        public string GetLayerName(string featureClassName)
        {
            string LayerName = "";
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                if (this.Map.get_Layer(i) is ICompositeLayer)
                {
                    LayerName = LoopGetLayerName(this.Map.get_Layer(i) as ICompositeLayer, featureClassName);
                    if (LayerName != "")
                    {
                        return LayerName;
                    }
                }
                else if (this.Map.get_Layer(i) is IFeatureLayer)
                {
                    IFeatureLayer fLayer = this.Map.get_Layer(i) as IFeatureLayer;
                    if ((fLayer != null)
                        && (fLayer.FeatureClass != null)
                        && (fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple
                            || fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        && this.GetDataSetName(fLayer.FeatureClass).ToUpper() == featureClassName.ToUpper())
                    {
                        return fLayer.Name;
                    }
                }
            }
            return "";
        }
        private string LoopGetLayerName(ICompositeLayer layer, string featureClassName)
        {
            string LayerName = "";
            ICompositeLayer cplayer = layer;
            for (int j = 0; j < cplayer.Count; j++)
            {
                if (cplayer.get_Layer(j) is ICompositeLayer)
                {
                    LayerName = LoopGetLayerName(cplayer.get_Layer(j) as ICompositeLayer, featureClassName);
                    if (LayerName != "")
                    {
                        return LayerName;
                    }
                }
                else if (cplayer.get_Layer(j) is IFeatureLayer)
                {
                    IFeatureLayer fLayer = cplayer.get_Layer(j) as IFeatureLayer;
                    if ((fLayer != null)
                        && (fLayer.FeatureClass != null)
                        && (fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple
                            || fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        && this.GetDataSetName(fLayer.FeatureClass).ToUpper() == featureClassName.ToUpper())
                    {
                        return fLayer.Name;
                    }
                }
            }
            return "";
        }

        //OK Loop
        public ZhFeatureClass GetZhFeatureClass(string featureClassName)
        {
            ZhFeatureClass ZHfeatcls = null;
            IFeatureClass fc = GetFeatureClass(featureClassName);
            if (fc != null)
            {
                switch (fc.ShapeType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        ZHfeatcls = new ZhPolygonFeatureClass(fc);
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                    case esriGeometryType.esriGeometryLine:
                        ZHfeatcls = new ZhPolylineFeatureClass(fc);
                        break;
                    case esriGeometryType.esriGeometryPoint:
                    case esriGeometryType.esriGeometryMultipoint:
                        ZHfeatcls = new ZhPointFeatureClass(fc);
                        break;

                }
            }
            return ZHfeatcls;
        }

        //OK Loop
        public IFeatureClass GetFeatureClass(string featureClassName)
        {
            IFeatureClass fc = null;
            ILayer layer = null;
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                layer = this.Map.get_Layer(i);
                if (layer is IFeatureLayer)
                {
                    IFeatureLayer fLayer = layer as IFeatureLayer;
                    if ((fLayer != null)
                        && (fLayer.FeatureClass != null)
                        && (fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple
                            || fLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                        && this.GetDataSetName(fLayer.FeatureClass).ToUpper() == featureClassName.ToUpper())
                    {
                        return fLayer.FeatureClass;
                    }
                }
                else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
                {
                    //
                    fc = LoopGetFeatureClass(layer as ICompositeLayer, featureClassName);
                    if (fc != null) return fc;
                }
            }
            return null;
        }
        private IFeatureClass LoopGetFeatureClass(ICompositeLayer layer, string featureClassName)
        {
            ICompositeLayer cplayer = layer as ICompositeLayer;
            IFeatureClass fc = null;
            for (int j = 0; j < cplayer.Count; j++)
            {
                if (cplayer.get_Layer(j) is ICompositeLayer)
                {
                    fc = LoopGetFeatureClass(cplayer.get_Layer(j) as ICompositeLayer, featureClassName);
                    if (fc != null) return fc;
                }
                if (cplayer.get_Layer(j) is IFeatureLayer && (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass != null)
                {
                    fc = (cplayer.get_Layer(j) as IFeatureLayer).FeatureClass;
                    if (this.GetDataSetName(fc).ToUpper() == featureClassName.ToUpper())
                    {
                        return fc;
                    }
                }
            }
            return null;
        }

        //OK Loop
        public IFeatureClass[] GetFeatureClassArray()
        {
            List<IFeatureClass> fcList = new List<IFeatureClass>();
            ILayer layer = null;
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                layer = this.Map.get_Layer(i);
                //
                LoopGetFeatureClassArray(layer, ref fcList);
            }
            return fcList.ToArray();
        }
        private void LoopGetFeatureClassArray(ILayer layer, ref List<IFeatureClass> fcList)
        {
            if (layer is IFeatureLayer)
            {
                IFeatureLayer fLayer = layer as IFeatureLayer;
                if ((fLayer != null) && (fLayer.FeatureClass != null))
                {
                    fcList.Add(fLayer.FeatureClass);
                }
            }
            else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
            {
                ICompositeLayer cplayer = layer as ICompositeLayer;
                for (int j = 0; j < cplayer.Count; j++)
                {
                    LoopGetFeatureClassArray(cplayer.get_Layer(j), ref fcList);
                }
            }
        }
        //OK Loop
        public IFeatureLayer[] GetFeatureLayerArray()
        {
            List<IFeatureLayer> fcList = new List<IFeatureLayer>();
            ILayer layer = null;
            for (int i = 0; i < this.Map.LayerCount; i++)
            {
                layer = this.Map.get_Layer(i);
                //
                LoopGetFeatureLayerArray(layer, ref fcList);
            }
            return fcList.ToArray();
        }
        private void LoopGetFeatureLayerArray(ILayer layer, ref List<IFeatureLayer> fcList)
        {
            if (layer is IFeatureLayer)
            {
                IFeatureLayer fLayer = layer as IFeatureLayer;
                if ((fLayer != null) && (fLayer.FeatureClass != null))
                {
                    fcList.Add(fLayer);
                }
            }
            else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
            {
                ICompositeLayer cplayer = layer as ICompositeLayer;
                for (int j = 0; j < cplayer.Count; j++)
                {
                    LoopGetFeatureLayerArray(cplayer.get_Layer(j), ref fcList);
                }
            }
        }

        //OK Loop
        public IFeatureLayer[] GetFeatureLayerBySelection()
        {
            List<IFeatureLayer> featLayerList = new List<IFeatureLayer>();
            if (this.m_Map.LayerCount > 0)
            {
                ILayer layer = null;
                for (int i = 0; i < this.m_Map.LayerCount; i++)
                {
                    layer = this.Map.get_Layer(i);
                    //
                    LoopGetFeatureLayerBySelection(layer, ref featLayerList);
                }
            }

            return featLayerList.ToArray();
        }
        private void LoopGetFeatureLayerBySelection(ILayer layer, ref List<IFeatureLayer> featLayerList)
        {
            if (layer is IFeatureLayer)
            {
                IFeatureSelection fLayer = layer as IFeatureSelection;
                if ((fLayer != null) && (fLayer.SelectionSet.Count > 0))
                {
                    featLayerList.Add(layer as IFeatureLayer);
                }
            }
            else if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
            {
                ICompositeLayer cplayer = layer as ICompositeLayer;
                for (int j = 0; j < cplayer.Count; j++)
                {
                    LoopGetFeatureLayerBySelection(cplayer.get_Layer(j), ref featLayerList);
                }
            }
        }

        //OK Loop
        /// <summary>
        /// //判断图层是否存在 >=0表示存在此图层,=-1表示不存在此图层
        /// </summary>
        public int LayerExist(string layer_name)
        {
            int layer_index = -1;
            for (int i = 0; i < this.m_Map.LayerCount; i++)
            {
                layer_index = LoopLayerExist(this.m_Map.get_Layer(i), i, layer_name);
                if (layer_index != -1) return layer_index;
            }
            return layer_index;
        }
        private int LoopLayerExist(ILayer layer, int i, string layer_name)
        {
            int layer_index = -1;
            if (layer is ICompositeLayer)
            {
                ICompositeLayer mCompositeLayer = layer as ICompositeLayer;
                if (mCompositeLayer != null)
                {
                    for (int j = 0; j < mCompositeLayer.Count; j++)
                    {
                        layer_index = LoopLayerExist(mCompositeLayer.get_Layer(j), i * j, layer_name);
                        if (layer_index != -1) return layer_index;
                    }
                }
            }
            else
            {
                if (layer.Name == layer_name)
                {
                    layer_index = i;
                    return layer_index;
                }
            }
            return -1;
        }

        //OK Loop
        //隐藏图例 方法
        public void HideLegend()
        {
            IMap map = this.m_Map;
            ILayer layer = null;
            for (int i = 0; i < map.LayerCount; i++)
            {
                layer = map.get_Layer(i);
                //
                LoopHideLegend(layer);
            }
        }
        private void LoopHideLegend(ILayer layer)
        {
            ILegendGroup lgroup = null;
            if (layer is ICompositeLayer && (layer as ICompositeLayer).Count > 0)
            {
                ICompositeLayer cplayer = layer as ICompositeLayer;
                ILayer sublayer = null;
                for (int j = 0; j < cplayer.Count; j++)
                {
                    sublayer = cplayer.get_Layer(j);
                    LoopHideLegend(sublayer);
                }
            }
            else
            {
                ILegendInfo lInfo = layer as ILegendInfo;
                if (lInfo != null && lInfo.LegendGroupCount >= 0)
                {
                    for (int g = 0; g < lInfo.LegendGroupCount; g++)
                    {
                        lgroup = lInfo.get_LegendGroup(g);
                        lgroup.Visible = false;
                    }
                }
            }
        }

        //OK Loop
        public void CloseAllLayer()
        {
            if (this.Map != null && this.Map.LayerCount > 0)
            {
                ILayer layer = null;
                for (int i = this.Map.LayerCount - 1; i >= 0; i--)
                {
                    layer = this.Map.get_Layer(i);
                    //
                }
                this.Map.ClearLayers();
            }
        }
        private void LoopCloseAllLayer(ILayer layer)
        {
            if (layer is ICompositeLayer)
            {
                ICompositeLayer cplayer = layer as ICompositeLayer;
                for (int j = cplayer.Count - 1; j >= 0; j--)
                {
                    LoopCloseAllLayer(cplayer.get_Layer(j));
                }
            }
            if (layer is IDataLayer2)
            {
                IDataLayer2 pDataLayer = (IDataLayer2)layer;
                pDataLayer.Disconnect();
            }
            if (layer is IFeatureLayer)
            {
                IFeatureClass fc = (layer as IFeatureLayer).FeatureClass;
                TokayWorkspace.ComRelease(fc);
                fc = null;
            }
            this.Map.DeleteLayer(layer);
        }


        public List<CustomLayer> GetListCustomLayerDisplayTreeStyle()
        {
            List<CustomLayer> List_cl = new List<CustomLayer>();
            List_cl.Clear();
            if (m_Map != null)
            {
                ILayer layer = null;
                ILayer tmpLayer = null;
                for (int i = 0; i < m_Map.LayerCount; i++)
                {
                    layer = m_Map.get_Layer(i);
                    if (layer is ICompositeLayer)
                    {   //图层组
                        #region 添加图层组到下拉列表中
                        CustomLayer itemCps = new CustomLayer(layer);
                        itemCps.TreeLevel = 0;
                        List_cl.Add(itemCps);
                        #endregion
                        Loop(layer as ICompositeLayer, 0, ref List_cl);
                    }
                    else
                    {   //图层
                        #region 添加图层到下拉列表中
                        tmpLayer = layer;
                        CustomLayer item = new CustomLayer(tmpLayer);
                        item.TreeLevel = 0;
                        List_cl.Add(item);
                        #endregion
                    }
                }
            }
            return List_cl;
        }
        private void Loop(ICompositeLayer cpsLayers, int ParentTreeLevel, ref List<CustomLayer> List_cl)
        {
            ParentTreeLevel += 3;
            ILayer Layer = null;
            for (int j = 0; j < cpsLayers.Count; j++)
            {
                Layer = cpsLayers.get_Layer(j);
                if (Layer is ICompositeLayer)
                {
                    #region 添加图层组到下拉列表中
                    CustomLayer itemCps = new CustomLayer(Layer);
                    itemCps.TreeLevel = ParentTreeLevel;
                    List_cl.Add(itemCps);
                    #endregion
                    //子图层
                    Loop(Layer as ICompositeLayer, ParentTreeLevel, ref List_cl);
                }
                else
                {
                    #region 添加图层到下拉列表中
                    CustomLayer item = new CustomLayer(Layer);
                    item.TreeLevel = ParentTreeLevel;
                    List_cl.Add(item);
                    #endregion
                }
            }
        }

        #endregion
    }

    public interface IZhMap
    {
        IMap Map { get; }

        bool IsEdited { get; }

        string GetDataSetName(IFeatureClass featureClass);

        IFeatureLayer GetFeatureLayerByFeatureClassName(string featureClassName);
        IFeatureLayer GetFeatureLayerByLayerName(string LayerName);

        string GetLayerName(string featureClassName);

        ZhFeatureClass GetZhFeatureClass(string featureClassName);

        IFeatureClass GetFeatureClass(string featureClassName);

        List<CustomLayer> GetListCustomLayerDisplayTreeStyle();

        void CloseAllLayer();
    }
}
