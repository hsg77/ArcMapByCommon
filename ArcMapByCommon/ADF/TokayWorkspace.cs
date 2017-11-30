using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using System.Drawing;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;

namespace ArcMapByCommon
{
    public static class TokayWorkspace
    {
        public static bool ComRelease(object ComObject)
        {
            bool rbc = false;
            try
            {
                if (ComObject != null)
                {
                    ComReleaser.ReleaseCOMObject(ComObject);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ComObject);
                    ComObject = null;
                    GC.Collect();
                }
                rbc = true;
            }
            catch (Exception ee)
            {
                //Log.WriteLine(ee);
            }
            return rbc;
        }
        //
        public static IColor GetColor(Color tpColor)
        {
            IColor c = null;
            IRgbColor rgb = new RgbColorClass();
            rgb.Red = int.Parse(tpColor.R.ToString());
            rgb.Green = int.Parse(tpColor.G.ToString());
            rgb.Blue = int.Parse(tpColor.B.ToString());

            c = rgb as IColor;
            return c;
        }
        public static stdole.IFontDisp GetFontDisp(string fontName, float fontSize)
        {
            stdole.IFontDisp fdisp = null;
            Font f = new Font(fontName, fontSize);
            fdisp = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(f) as stdole.IFontDisp;
            return fdisp;
        }
        public static stdole.IFontDisp GetFontDisp(Font f)
        {
            stdole.IFontDisp fdisp = null;
            fdisp = ESRI.ArcGIS.ADF.COMSupport.OLE.GetIFontDispFromFont(f) as stdole.IFontDisp;
            return fdisp;
        }
        public static string GetDatasetName(IDataset ds)
        {
            if (ds != null)
            {
                ISQLSyntax sqlSyntax = ds.Workspace as ISQLSyntax;
                string dbName, ownerName, tableName;
                sqlSyntax.ParseTableName(ds.Name, out dbName, out ownerName, out tableName);

                return tableName;
            }

            return "";
        }
        public static IField CreateNumberField(ITable table, string fieldName, string AliasName)
        {
            return CreateDoubleField(table, fieldName, AliasName);
        }
        /// <summary>
        /// 根据字段名称在ITable中创建数值型字段，若ITable中存在该字段，则不创建
        /// </summary>
        /// 
        public static IField CreateDoubleField(ITable table, string fieldName, string AliasName, int length = 0, int scale = 0, int Precision = 0)
        {
            if (table == null || fieldName.Length <= 0)
            {
                MessageBox.Show("需要创建的字段名和表(ITable)不能为空。");
                return null;
            }

            IField field = GetField(table, fieldName);
            if (field != null)
            {
                //获取存在的字段
                return field;
            }
            else
            {
                try
                {
                    //新建一个字段，类型为Double
                    IField f = new Field();
                    IFieldEdit fEdit = (IFieldEdit)f;
                    fEdit.AliasName_2 = AliasName;
                    fEdit.Name_2 = fieldName;
                    fEdit.Type_2 = esriFieldType.esriFieldTypeDouble;
                    //
                    fEdit.Length_2 = length;
                    fEdit.Scale_2 = scale;
                    fEdit.Precision_2 = Precision;
                    //
                    fEdit.IsNullable_2 = true;
                    fEdit.DefaultValue_2 = 0.0;
                    fEdit.Editable_2 = true;
                    table.AddField(f);

                    return f;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建字段失败，原因：" + ex.Message);
                }
            }
            return null;
        }
        public static IField CreateIntField(ITable table, string fieldName, string AliasName)
        {
            if (table == null || fieldName.Length <= 0)
            {
                MessageBox.Show("需要创建的字段名和表(ITable)不能为空。");
                return null;
            }

            IField field = GetField(table, fieldName);
            if (field != null)
            {
                //获取存在的字段
                return field;
            }
            else
            {
                try
                {
                    //新建一个字段，类型为Double
                    IField f = new Field();
                    IFieldEdit fEdit = (IFieldEdit)f;
                    fEdit.AliasName_2 = AliasName;
                    fEdit.Name_2 = fieldName;
                    fEdit.Type_2 = esriFieldType.esriFieldTypeInteger;
                    fEdit.IsNullable_2 = true;
                    fEdit.DefaultValue_2 = 0;
                    fEdit.Editable_2 = true;
                    table.AddField(f);

                    return f;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建字段失败，原因：" + ex.Message);
                }
            }
            return null;
        }
        public static IField CreateStringField(ITable table, string fieldName, string AliasName)
        {
            return CreateStringField(table, fieldName, AliasName, 4000, "", true);
        }
        public static IField CreateStringField(ITable table, string fieldName, string AliasName, int length, string defaultvalue, bool IsNullable)
        {
            if (table == null || fieldName.Length <= 0)
            {
                MessageBox.Show("需要创建的字段名和表(ITable)不能为空。");
                return null;
            }

            IField field = GetField(table, fieldName);
            if (field != null)
            {
                //获取存在的字段
                return field;
            }
            else
            {
                try
                {
                    //新建一个字段，类型为String
                    IField f = new Field();
                    IFieldEdit fEdit = (IFieldEdit)f;
                    fEdit.AliasName_2 = AliasName;
                    fEdit.Name_2 = fieldName;
                    fEdit.Type_2 = esriFieldType.esriFieldTypeString;
                    fEdit.Length_2 = length;
                    fEdit.IsNullable_2 = IsNullable;
                    fEdit.DefaultValue_2 = defaultvalue;
                    fEdit.Editable_2 = true;
                    table.AddField(f);

                    return f;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建字段失败，原因：" + ex.Message);
                }
            }
            return null;
        }
        public static IField CreateDateField(ITable table, string fieldName, string AliasName)
        {
            if ((table == null) || (fieldName.Length <= 0))
            {
                MessageBox.Show("需要创建的字段名和表(ITable)不能为空。");
                return null;
            }
            IField field = GetField(table, fieldName);
            if (field != null)
            {
                return field;
            }
            try
            {
                IField field2 = new FieldClass();
                IFieldEdit edit = (IFieldEdit)field2;
                edit.AliasName_2 = AliasName;
                edit.Name_2 = fieldName;
                edit.Type_2 = esriFieldType.esriFieldTypeDate;
                edit.IsNullable_2 = true;
                //edit.DefaultValue_2 = 0.0;
                edit.Editable_2 = true;
                table.AddField(field2);
                return field2;
            }
            catch (Exception exception)
            {
                MessageBox.Show("创建字段失败，原因：" + exception.Message);
            }
            return null;
        }
        /// <summary>
        /// 根据字段名称从ITable中获取字段，如果字段不存在，则返回空
        /// </summary>        
        public static IField GetField(ITable table, string fieldName)
        {
            if (table != null && fieldName.Length > 0)
            {
                int fIndex = table.Fields.FindField(fieldName);
                if (fIndex >= 0)
                {
                    return table.Fields.get_Field(fIndex);
                }
            }

            return null;
        }

        /// <summary>
        /// 删除要素类中的字段
        /// </summary>        
        public static bool DeleteField(IFeatureClass fc, string fieldName)
        {
            bool rbc = false;
            int fIndex = fc.FindField(fieldName);
            if (fIndex >= 0)
            {
                IField fd = fc.Fields.get_Field(fIndex);
                fc.DeleteField(fd);
                rbc = true;
            }
            return rbc;
        }

        //
        /// <summary>
        /// 通过名称打开一个FeatureClass层
        /// </summary>
        /// <param name="m_Workspace"></param>
        /// <param name="FeatureClsName"></param>
        /// <returns></returns>
        public static IFeatureClass GetFeatureClassByName(IWorkspace m_Workspace, string FeatureClsName)
        {
            try
            {
                IFeatureClass pFeatClass;
                IFeatureWorkspace pFeatureWorkspace;
                pFeatureWorkspace = (IFeatureWorkspace)m_Workspace;
                pFeatClass = pFeatureWorkspace.OpenFeatureClass(FeatureClsName);
                return pFeatClass;
            }
            catch
            {
                return null;
            }
        }

        public static IFeatureClass GetFeatureClassByID(IWorkspace workspace, int classID)
        {
            IEnumDataset enumDs = workspace.get_Datasets(esriDatasetType.esriDTAny);
            IDataset ds = enumDs.Next();
            while (ds != null)
            {
                if (ds is IFeatureClass)
                {
                    if ((ds as IFeatureClass).FeatureClassID == classID)
                        return (ds as IFeatureClass);
                }
                else if (ds is IFeatureDataset)
                {
                    IEnumDataset enumDs2 = (ds as IFeatureDataset).Subsets;
                    IDataset ds2 = enumDs2.Next();
                    while (ds2 != null)
                    {
                        if (ds2 is IFeatureClass)
                        {
                            if ((ds2 as IFeatureClass).FeatureClassID == classID)
                                return (ds2 as IFeatureClass);
                        }
                        ds2 = enumDs2.Next();
                    }
                }
                ds = enumDs.Next();
            }

            return null;
        }

        public static List<IFeature> GetFeatureList(IFeatureLayer featLayer)
        {
            ISelectionSet selset = GetSelectionSet(featLayer);
            return GetFeatureList(selset);
        }
        public static IFeatureSelection GetFeatureSelection(IFeatureLayer featLayer)
        {
            IFeatureSelection featSel = null;
            featSel = featLayer as IFeatureSelection;
            return featSel;
        }
        public static ISelectionSet GetSelectionSet(IFeatureLayer featLayer)
        {
            IFeatureSelection featSel = null;
            featSel = featLayer as IFeatureSelection;
            if (featSel != null)
                return featSel.SelectionSet;
            return null;
        }
        public static List<IFeature> GetFeatureList(ISelectionSet selset)
        {
            List<IFeature> featlist = new List<IFeature>();
            ISelectionSet ss = selset;
            ICursor iCursor = null;
            ss.Search(null, false, out iCursor);
            IFeatureCursor iFeatureCursor = iCursor as IFeatureCursor;
            IFeature iFeature = iFeatureCursor.NextFeature();
            while (iFeature != null)
            {
                featlist.Add(iFeature);
                iFeature = iFeatureCursor.NextFeature();
            }
            ComRelease(iCursor);
            iCursor = null;
            return featlist;
        }

        public static IEnumDataset getEnumDataset(IWorkspace ws)
        {
            return ws.get_Datasets(esriDatasetType.esriDTAny);
        }
        public static IEnumDataset getEnumSubDataset(IDataset ods)
        {
            IEnumDataset oEnumSubds = null;
            if (ods.Type == esriDatasetType.esriDTFeatureDataset)
            {
                oEnumSubds = ods.Subsets;
            }
            return oEnumSubds;
        }

        public static IFeatureClass[] GetFeatureClassArray(IWorkspace m_workspace)
        {
            IEnumDataset enumDs = m_workspace.get_Datasets(esriDatasetType.esriDTAny);
            enumDs.Reset();
            IDataset ds = enumDs.Next();
            List<IFeatureClass> ds_list = new List<IFeatureClass>();

            while (ds != null)
            {
                IFeatureClass fc = ds as IFeatureClass;
                if (fc != null) ds_list.Add(fc);
                else
                {
                    IFeatureDataset fds = ds as IFeatureDataset;
                    if (fds != null)
                    {
                        IEnumDataset enumDs2 = fds.Subsets;
                        enumDs2.Reset();
                        IFeatureClass fc2 = enumDs2.Next() as IFeatureClass;
                        while (fc2 != null)
                        {
                            ds_list.Add(fc2);
                            fc2 = enumDs2.Next() as IFeatureClass;
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(fds);
                    }
                }
                ds = enumDs.Next();
            }

            return ds_list.ToArray();
        }

        public static IFeatureClass[] GetFeatureClassByDataset(IWorkspace m_workspace, string datasetName)
        {
            List<IFeatureClass> feaList = new List<IFeatureClass>();

            IEnumDataset enumDs = m_workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            IDataset ds = enumDs.Next();
            while (ds != null)
            {
                if (ds is IFeatureDataset && ds.Name.ToUpper().Trim() == datasetName.ToUpper())
                {
                    IEnumDataset enumFs = (ds as IFeatureDataset).Subsets;
                    IDataset ds2 = enumFs.Next();
                    while (ds2 != null)
                    {
                        if (ds2 is IFeatureClass)
                            feaList.Add(ds2 as IFeatureClass);

                        ds2 = enumFs.Next();
                    }
                }

                ds = enumDs.Next();
            }

            return feaList.ToArray();
        }

        public static IDataset GetDatasetByName(IWorkspace m_workspace, string name)
        {
            IEnumDataset enum_ds = m_workspace.get_Datasets(esriDatasetType.esriDTAny);
            IDataset ds = enum_ds.Next();
            while (ds != null)
            {
                if (ds.Name.ToUpper() == name.ToUpper())
                    return ds;
                else if (ds is IFeatureDataset)
                {
                    IFeatureDataset fea_ds = ds as IFeatureDataset;
                    IEnumDataset enum_sub_ds = fea_ds.Subsets;
                    IDataset sub_ds = enum_sub_ds.Next();
                    while (sub_ds != null)
                    {
                        if (sub_ds.Name.ToUpper() == name.ToUpper())
                            return sub_ds;
                        sub_ds = enum_sub_ds.Next();
                    }
                }
                ds = enum_ds.Next();
            }
            return null;
        }

        public static ISpatialReference GetSpatialReference(IFeatureLayer fL)
        {
            ISpatialReference sr = null;
            if (fL.FeatureClass is IGeoDataset)
            {
                sr = (fL.FeatureClass as IGeoDataset).SpatialReference;
            }
            return sr;
        }
        public static ISpatialReference GetSpatialReference(IFeatureClass fc)
        {
            ISpatialReference sr = null;
            if (fc is IGeoDataset)
            {
                sr = (fc as IGeoDataset).SpatialReference;
            }
            return sr;
        }
        public static ISpatialReference GetSpatialReference(IDataset ds)
        {
            ISpatialReference sr = null;
            if (ds is IGeoDataset)
            {
                sr = (ds as IGeoDataset).SpatialReference;
            }
            return sr;
        }

        //更改要素类别名 函数
        public static bool AlterAliasName(IFeatureClass fc, string NewAliasName)
        {
            bool rbc = false;
            ISchemaLock sLock = fc as ISchemaLock;
            IClassSchemaEdit csEdit = fc as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素类别名
                csEdit.AlterAliasName(NewAliasName);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        //更改表别名 函数
        public static bool AlterAliasName(ITable table, string NewAliasName)
        {
            bool rbc = false;
            ISchemaLock sLock = table as ISchemaLock;
            IClassSchemaEdit csEdit = table as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素类别名
                csEdit.AlterAliasName(NewAliasName);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        //更改表模型名 函数
        public static bool AlterModelName(ITable table, string NewModelName)
        {
            bool rbc = false;
            ISchemaLock sLock = table as ISchemaLock;
            IClassSchemaEdit csEdit = table as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素模型名
                csEdit.AlterModelName(NewModelName);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        //更改要素类中字段别名 函数
        public static bool AlterFieldAliasName(IFeatureClass fc, string FieldName, string NewAliasName)
        {
            bool rbc = false;
            ISchemaLock sLock = fc as ISchemaLock;
            IClassSchemaEdit csEdit = fc as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素类中字段别名
                csEdit.AlterFieldAliasName(FieldName, NewAliasName);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        //更改要素类中字段模型名 函数
        public static bool AlterFieldModelName(IFeatureClass fc, string FieldName, string NewModelName)
        {
            bool rbc = false;
            ISchemaLock sLock = fc as ISchemaLock;
            IClassSchemaEdit csEdit = fc as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素类中字段模型名
                csEdit.AlterFieldModelName(FieldName, NewModelName);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        //更改要素类中字段默认值 函数
        public static bool AlterFieldDefaultValue(IFeatureClass fc, string FieldName, object defaultvalue)
        {
            bool rbc = false;
            ISchemaLock sLock = fc as ISchemaLock;
            IClassSchemaEdit csEdit = fc as IClassSchemaEdit;
            if (sLock != null && csEdit != null)
            {
                sLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                //改变要素类中字段别名
                csEdit.AlterDefaultValue(FieldName, defaultvalue);
                //释放锁定
                sLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                rbc = true;
            }
            return rbc;
        }
        public static bool UpdateExtent(IFeatureClass fc)
        {
            bool rbc = false;
            ISchemaLock schemaLock = (ISchemaLock)fc;
            try
            {
                IFeatureClassManage fcManage = (IFeatureClassManage)fc;
                if (fcManage != null)
                {
                    //schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                    fcManage.UpdateExtent();
                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    //Console.WriteLine("Update finished");   
                    rbc = true;
                }
            }
            catch (Exception e)
            {
                //error occured            
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                //Console.WriteLine(e.Message); 
                Log.WriteLine(e);
            }
            return rbc;
        }

        //修改空间参考范围值
        public static bool SetDomain(ISpatialReference sr, double pMinX, double pMinY, double pMaxX, double pMaxY)
        {
            bool rbc = false;
            //设置范围值
            if (sr is ISpatialReference2GEN)
            {
                (sr as ISpatialReference2GEN).SetDomain(pMinX, pMaxX, pMinY, pMaxY);
            }
            else
            {
                sr.SetDomain(pMinX, pMaxX, pMinY, pMaxY);
            }
            rbc = true;
            return rbc;
        }
        public static bool SetDomain_defaultMax9(ISpatialReference sr)
        {
            return SetDomain(sr, 0, 0, 99999999.999999, 9999999.999999);
        }
        //
        /// <summary>
        /// 创建一个新的Geodatabase文件，支持mdb和gdb
        /// </summary>
        /// <param name="geodatabasePath"></param>
        public static void CreateGeodatabase(string geodatabasePath)
        {
            IWorkspaceFactory wf = null;
            switch (System.IO.Path.GetExtension(geodatabasePath).ToUpper())
            {
                case ".GDB":
                    if (Directory.Exists(geodatabasePath))
                        throw new Exception("路径" + geodatabasePath + "已经存在，不能创建geodatabase。");
                    wf = new FileGDBWorkspaceFactoryClass();
                    break;
                case ".MDB":
                    if (File.Exists(geodatabasePath))
                        throw new Exception("文件" + geodatabasePath + "已经存在，不能创建geodatabase。");
                    wf = new AccessWorkspaceFactoryClass();
                    break;
                default:
                    break;
            }

            if (wf != null)
            {
                string text2 = geodatabasePath.Substring(0, geodatabasePath.LastIndexOf(@"\"));
                string text1 = geodatabasePath.Substring(geodatabasePath.LastIndexOf(@"\") + 1);
                text1 = text1.Substring(0, text1.LastIndexOf("."));
                IPropertySet set1 = new PropertySetClass();
                set1.SetProperty("Database", text2);
                IWorkspaceName wn = wf.Create(text2, text1, set1, 0);
            }
            else
                throw new NotSupportedException("不支持传入的文件名。");

        }
        //
        //
    }
}
