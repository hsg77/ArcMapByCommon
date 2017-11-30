


using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;


using System.Xml;
using System.Xml.XPath;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;
using ArcMapByCommon;


namespace GIS.ArcGIS
{    
    /// <summary>
    /// 管理GDB(SDE/gdb/mdb)服务器数据库相关信息的类 功能
    /// vp:hsg
    /// create date:2009
    /// modify date:2010-07
    /// </summary>
    public class gdbUtil : IGDBServerInfoConfig, IDisposable
    {
        public gdbUtil()
        {
        }

        #region IServerInfoConfig 成员
        //==========================================================
        private EnumSDEServer m_enumSDEServer = EnumSDEServer.Oracle;
        public EnumSDEServer enumSDEServer
        {
            get
            {
                return m_enumSDEServer;
            }
            set
            {
                m_enumSDEServer = value;
            }
        }
        //==========================================================
        private string m_SDEConnectionString = "";
        public string SDEConnectionString
        {
            get
            {
                return m_SDEConnectionString;
            }
            set
            {
                m_SDEConnectionString = value;
            }
        }
        //==========================================================
        private string m_Server = "";
        public string SDE_Server
        {
            get
            {
                return m_Server;
            }
            set
            {
                m_Server = value;
            }
        }
        //==========================================================
        private string m_Port = "5151";
        public string SDE_Port
        {
            get
            {
                return m_Port;
            }
            set
            {
                m_Port = value;
            }
        }
        //==========================================================
        private string m_User = "";
        public string SDE_User
        {
            get
            {
                return m_User;
            }
            set
            {
                m_User = value;
            }
        }
        //==========================================================
        private string m_PWD = "";
        public string SDE_PWD
        {
            get
            {
                return m_PWD;
            }
            set
            {
                m_PWD = value;
            }
        }
        //==========================================================
        private string m_DataBase = "";
        public string SDE_DataBase
        {
            get
            {
                return m_DataBase;
            }
            set
            {
                m_DataBase = value;
            }
        }
        //==========================================================
        private string m_SDEVersion = "SDE.DEFAULT";
        public string SDE_Version
        {
            get
            {
                return m_SDEVersion;
            }
            set
            {
                m_SDEVersion = value;
            }
        }
        //==========================================================
        private string mdb_Server = "";
        public string DB_Server
        {
            get
            {
                return mdb_Server;
            }
            set
            {
                mdb_Server = value;
            }
        }
        //==========================================================
        private string mdb_DataBase = "";
        public string DB_DataBase
        {
            get
            {
                return mdb_DataBase;
            }
            set
            {
                mdb_DataBase = value;
            }
        }
        //==========================================================
        private string mdb_User = "";
        public string DB_User
        {
            get
            {
                return mdb_User;
            }
            set
            {
                mdb_User = value;
            }
        }
        //==========================================================
        private string mdb_PWD = "";
        public string DB_PWD
        {
            get
            {
                return mdb_PWD;
            }
            set
            {
                mdb_PWD = value;
            }
        }
        //==========================================================

        public IPropertySet GetPropertySet()
        {
            IPropertySet set2 = new PropertySetClass();
            switch (this.enumSDEServer)
            {
                case EnumSDEServer.Oracle:
                case EnumSDEServer.SQLServer:
                    {
                        set2.SetProperty("SERVER", this.SDE_Server);
                        set2.SetProperty("INSTANCE", this.SDE_Port);
                        set2.SetProperty("DATABASE", this.SDE_DataBase);
                        set2.SetProperty("USER", this.SDE_User);
                        set2.SetProperty("PASSWORD", this.SDE_PWD);
                        set2.SetProperty("VERSION", this.SDE_Version);
                    }
                    break;
                case EnumSDEServer.Access:
                    set2.SetProperty("DATABASE", this.SDE_DataBase);
                    break;
                case EnumSDEServer.FileGDB:
                    set2.SetProperty("DATABASE", this.SDE_DataBase);
                    break;
            }
            return set2;

        }

        private IWorkspaceFactory factory = null;
        public bool OpenSDEConnection()
        {
            bool flag = false;
            switch (this.enumSDEServer)
            {
                case EnumSDEServer.Oracle:
                case EnumSDEServer.SQLServer:
                    #region SqlServer|Oracle
                    try
                    {
                        IPropertySet propertySet = this.GetPropertySet();
                        factory = new SdeWorkspaceFactoryClass();
                        this.DefaultWorkSpace = factory.Open(propertySet, 0);
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        MessageBox.Show(this.GetType().Name + "->OpenConnection() 连接Sde错误！\r\n" + ex.Message + ":请先查看连接数据库参数是否正确？", "连接数据库提示");
                    }
                    break;
                    #endregion
                case EnumSDEServer.Access:
                    #region Access
                    try
                    {
                        factory = new AccessWorkspaceFactoryClass();
                        this.DefaultWorkSpace = factory.OpenFromFile(this.SDE_DataBase, 0);

                        if (this.DefaultWorkSpace != null)
                        {
                            flag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        MessageBox.Show(this.GetType().Name + "->OpenConnection() 连接Access错误！\r\n" + ex.Message + ":请先查看连接数据库参数是否正确？", "连接数据库提示");
                    }
                    break;
                    #endregion
                case EnumSDEServer.FileGDB:
                    #region FileGDB
                    try
                    {
                        factory = new FileGDBWorkspaceFactoryClass();

                        this.DefaultWorkSpace = factory.OpenFromFile(this.SDE_DataBase, 0); //open_fGDB_Workspace(SDE_DataBase);
                        if (this.DefaultWorkSpace != null)
                        {
                            flag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        MessageBox.Show(this.GetType().Name + "->OpenConnection() 连接文件数据库错误！\r\n" + ex.Message + ":请先查看连接数据库参数是否正确？", "连接数据库提示");
                    }
                    break;
                    #endregion
            }
            return flag;
        }

        //这里不用
        private IWorkspace open_fGDB_Workspace(string database)
        {
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            propertySet.SetProperty("DATABASE", database);
            IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            return workspaceFactory.Open(propertySet, 0);
        }

        private IWorkspace m_DefaultWorkSpace = null;
        /// <summary>
        /// read/write
        /// </summary>
        public IWorkspace DefaultWorkSpace
        {
            get
            {
                //if (m_DefaultWorkSpace == null)
                //    OpenSDEConnection();

                return m_DefaultWorkSpace;
            }
            set
            {
                m_DefaultWorkSpace = value;
            }
        }

        public ISQLSyntax SQLSyntax
        {
            get
            {
                return m_DefaultWorkSpace as ISQLSyntax;
            }
        }

        public bool ReadXml(string mconfigFile)
        {
            bool rbc = false;
            try
            {
                XmlDocument myConfig = new XmlDocument();
                XmlElement root;
                XmlAttributeCollection myAttColl;
                string mSDEServerType = "";
                myConfig.Load(mconfigFile);
                root = myConfig.DocumentElement;
                System.Xml.XmlNode node;
                //begin
                System.Xml.XmlNamespaceManager nsmgr = new XmlNamespaceManager(myConfig.NameTable);
                nsmgr.AddNamespace("XCconfig", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
                node = myConfig.SelectSingleNode("//XCconfig:appSettings", nsmgr);
                //end
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    myAttColl = node.ChildNodes[i].Attributes;

                    switch (myAttColl["key"].Value.ToString())
                    {
                        case "SDEServerType":
                            if (myAttColl["value"].Value == "SQLServer") this.enumSDEServer = EnumSDEServer.SQLServer;
                            if (myAttColl["value"].Value == "Oracle") this.enumSDEServer = EnumSDEServer.Oracle;
                            if (myAttColl["value"].Value == "Access") this.enumSDEServer = EnumSDEServer.Access;
                            mSDEServerType = myAttColl["value"].Value;
                            break;
                        case "SDEServer":
                            this.SDE_Server = myAttColl["value"].Value;
                            break;
                        case "SDEPort":
                            this.SDE_Port = myAttColl["value"].Value;
                            break;
                        case "SDEUser":
                            this.SDE_User = myAttColl["value"].Value;
                            break;
                        case "SDEPWD":
                            this.SDE_PWD = myAttColl["value"].Value;
                            break;
                        case "SDEDataBase":
                            this.SDE_DataBase = myAttColl["value"].Value;
                            break;
                        case "SDEVersion":
                            this.SDE_Version = myAttColl["value"].Value;
                            break;
                        case "DBServer":
                            this.DB_Server = myAttColl["value"].Value;
                            break;
                        case "DBDataBase":
                            this.DB_DataBase = myAttColl["value"].Value;
                            break;
                        case "DBUser":
                            this.DB_User = myAttColl["value"].Value;
                            break;
                        case "DBPWD":
                            this.DB_PWD = myAttColl["value"].Value;
                            break;
                        case "DataProviderType":
                            break;
                        case "ConnectionString":
                            break;
                    }
                }
                this.enumSDEServer = (EnumSDEServer)Enum.Parse(typeof(EnumSDEServer), mSDEServerType, true);
                rbc = true;
            }
            catch
            {
                rbc = false;
            }
            return rbc;
        }

        public bool SaveXml(string mconfigFile)
        {
            bool rbc = false;
            XmlDocument myConfig = new XmlDocument();
            XmlAttributeCollection myAttColl;
            XmlElement root;
            string configFile;
            configFile = mconfigFile;

            try
            {
                myConfig.Load(configFile);
                root = myConfig.DocumentElement;
                System.Xml.XmlNode node;

                //begin
                System.Xml.XmlNamespaceManager nsmgr = new XmlNamespaceManager(myConfig.NameTable);
                nsmgr.AddNamespace("XCconfig", "http://schemas.microsoft.com/.NetConfiguration/v2.0");
                node = myConfig.SelectSingleNode("//XCconfig:appSettings", nsmgr);
                //end
                //node=root.SelectSingleNode("appSettings");   //原来.Net1.1框架可用代码
                for (int i = 0; i < node.ChildNodes.Count; i++)
                {
                    myAttColl = node.ChildNodes[i].Attributes;

                    switch (myAttColl["key"].Value.ToString())
                    {
                        case "SDEServerType":
                            myAttColl["value"].Value = this.enumSDEServer.ToString();
                            break;
                        case "SDEServer":
                            myAttColl["value"].Value = this.SDE_Server;
                            break;
                        case "SDEPort":
                            myAttColl["value"].Value = this.SDE_Port;
                            break;
                        case "SDEUser":
                            myAttColl["value"].Value = this.SDE_User;
                            break;
                        case "SDEPWD":
                            myAttColl["value"].Value = this.SDE_PWD;
                            break;
                        case "SDEDataBase":
                            myAttColl["value"].Value = this.SDE_DataBase;
                            break;
                        case "SDEVersion":
                            myAttColl["value"].Value = this.SDE_Version;
                            break;
                        case "DBServer":
                            myAttColl["value"].Value = this.DB_Server;
                            break;
                        case "DBDataBase":
                            myAttColl["value"].Value = this.DB_DataBase;
                            break;
                        case "DBUser":
                            myAttColl["value"].Value = this.DB_User;
                            break;
                        case "DBPWD":
                            myAttColl["value"].Value = this.DB_PWD;
                            break;
                        case "DataProviderType":
                            myAttColl["value"].Value = "";//this.getDataProviderType().ToString();
                            break;
                        case "ConnectionString":
                            myAttColl["value"].Value = this.getProfessionalConnectionString;
                            break;
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
            myConfig.Save(configFile);
            return rbc;
        }

        public string OleDbConnectionString
        {
            get
            {
                string rbc = "";
                switch (this.enumSDEServer)
                {
                    case EnumSDEServer.Access:
                    case EnumSDEServer.Oracle:
                        break;
                    case EnumSDEServer.SQLServer:
                        rbc= string.Format("Provider=SQLOLEDB;Data Source={0};User ID={1};Initial Catalog={2};Password={3}",
                            this.DB_Server, this.DB_User, this.DB_DataBase, this.DB_PWD);
                        break;
                    case EnumSDEServer.FileGDB:
                        break;
                    default:
                        break;
                }
                return rbc;
            }
        }

        public string getProfessionalConnectionString
        {
            get
            {
                string constr = "";

                switch (this.enumSDEServer)
                {
                    case EnumSDEServer.Access:
                        constr = this.getADOConnectionString;
                        break;
                    case EnumSDEServer.Oracle:
                        constr = "Data Source=" + this.DB_DataBase
                            + ";Persist Security Info=True;User ID=" + this.DB_User
                            + ";Password=" + this.DB_PWD;
                        break;
                    case EnumSDEServer.SQLServer:
                        constr = "Data Source=" + this.DB_Server
                            + ";Persist Security Info=True;User ID=" + this.DB_User
                            + ";Initial Catalog=" + this.DB_DataBase + ";Password=" + this.DB_PWD;
                        break;
                }

                return constr;
            }
        }

        public string getADOConnectionString
        {
            get
            {
                string url = null;
                switch (this.enumSDEServer)
                {
                    case EnumSDEServer.Access:
                        {
                            url = "Provider=Microsoft.Jet.OLEDB.4.0;";
                            url += " Data Source=" + this.DB_Server + ";";
                            //url+=" user id=" + this.User  + ";";
                            //url+=" password=" + this.PWD + ";";
                            break;
                        }
                    case EnumSDEServer.SQLServer:
                        {
                            url = "Provider=MSDASQL.1;";
                            url += " Data Source=" + this.DB_Server + ";";
                            url += " Initial Catalog=" + this.DB_DataBase + ";";
                            url += " user id=" + this.DB_User + ";";
                            url += " password=" + this.DB_PWD + ";";
                            url += " Persist Security Info=True;";
                            break;
                        }
                    case EnumSDEServer.Oracle:
                        {
                            url = "Provider=OraOLEDB.Oracle.1;";
                            url += " Data Source=" + this.DB_DataBase + ";";
                            url += " user id=" + this.DB_User + ";";
                            url += " password=" + this.DB_PWD + ";";
                            url += " Connection Lifetime=10";
                            break;
                        }
                    default:
                        {
                            url = string.Empty;
                            break;
                        }
                }
                return url;
            }
        }


        //public DataProviderType getDataProviderType()
        //{
        //    DataProviderType dpt = DataProviderType.Oracle;
        //    switch (this.enumSDEServer)
        //    {
        //        case EnumSDEServer.Access:
        //            dpt = DataProviderType.Access;
        //            break;
        //        case EnumSDEServer.Oracle:
        //            dpt = DataProviderType.Oracle;
        //            break;
        //        case EnumSDEServer.SQLServer:
        //            dpt = DataProviderType.Sql;
        //            break;
        //    }
        //    return dpt;
        //}

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.DefaultWorkSpace != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.DefaultWorkSpace);
            }
            if (this.factory != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(this.factory);
            }
            this.DefaultWorkSpace = null;
            this.factory = null;
        }

        #endregion


        #region IServerInfoConfig 成员

        public IFeatureWorkspace featWs
        {
            get
            {
                return this.DefaultWorkSpace as IFeatureWorkspace;
            }
        }
        public IFeatureClass getFeatureClass(string LayerName)
        {
            if (this.featWs != null)
            {
                return this.featWs.OpenFeatureClass(LayerName);
            }
            return null;
        }

        public ITable getTable(string LayerName)
        {
            if (this.featWs != null)
            {
                return this.featWs.OpenTable(LayerName);
            }
            return null;
        }
        
        /// <summary>
        /// //设置投影的XYDoMain的功能 通过this.DefaultWorkSpace属性 
        /// 这需要先执行OpenSDEConnection()方法
        /// </summary>
        /// <param name="pMinX"></param>
        /// <param name="pMinY"></param>
        /// <param name="pMaxX"></param>
        /// <param name="pMaxY"></param>
        /// <returns></returns>
        public bool SetSpatialReferenceXYDoMainToDB(double pMinX, double pMinY, double pMaxX, double pMaxY)
        {
            bool rbc = false;
            if (this.DefaultWorkSpace == null)
            {
                rbc = false;
                return rbc;
            }
            List<IDataset> AllGisDataSetList = new List<IDataset>();
            //获取本空间连接通道中的所有要素类和要素集对象
            IWorkspace ws = this.DefaultWorkSpace;
            IEnumDataset topEnumDataset = this.getEnumDataset(ws);
            if (topEnumDataset != null)
            {
                topEnumDataset.Reset();
                IDataset ds = topEnumDataset.Next();
                while (ds != null)
                {
                    switch (ds.Type)
                    {
                        case esriDatasetType.esriDTFeatureDataset:
                            //--
                            AllGisDataSetList.Add(ds);
                            //--
                            IEnumDataset subEnumDs = this.getEnumSubDataset(ds);
                            IDataset subds = subEnumDs.Next();
                            while (subds != null)
                            {
                                switch (subds.Type)
                                {
                                    case esriDatasetType.esriDTFeatureClass:
                                        rbc = this.SetDataSetByXYDoMain(subds, pMinX, pMinY, pMaxX, pMaxY);
                                        break;
                                }
                                subds = subEnumDs.Next();
                            }
                            rbc = this.SetDataSetByXYDoMain(ds, pMinX, pMinY, pMaxX, pMaxY);
                            break;
                        case esriDatasetType.esriDTFeatureClass:
                            //--
                            rbc = this.SetDataSetByXYDoMain(ds, pMinX, pMinY, pMaxX, pMaxY);
                            //--
                            break;
                    }
                    ds = topEnumDataset.Next();
                }
            }
            return rbc;
        }

        private bool SetDataSetByXYDoMain(IDataset ds, double pMinX, double pMinY, double pMaxX, double pMaxY)
        {
            bool rbc = false;
            ISpatialReference sr = null;
            IGeoDataset geods = null;

            geods = ds as IGeoDataset;
            sr = geods.SpatialReference;
            //---
            if (geods is IFeatureClassManage && geods is ISchemaLock)
            {
                ISchemaLock schemaLock = (ISchemaLock)geods;
                try
                {
                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                    IFeatureClassManage featureClassManage = (IFeatureClassManage)geods;
                    featureClassManage.UpdateExtent();

                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    //("updateExtent_Update完成:" + (geods as IDataset).Name);

                }
                catch (Exception k)
                {   //error occured            
                    schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    //("updateExtent_" + k.Message + " " + (geods as IDataset).Name);
                    //new data
                }
            }
            //---modify xydomain
            //设置范围值
            if (sr is ISpatialReference2GEN)
            {
                (sr as ISpatialReference2GEN).SetDomain(pMinX, pMaxX, pMinY, pMaxY);
            }
            else
            {
                sr.SetDomain(pMinX, pMaxX, pMinY, pMaxY);
            }
            //---
            if ((geods as IDataset).Type == esriDatasetType.esriDTFeatureDataset
                        && geods is IGeoDatasetSchemaEdit
                        && geods is ISchemaLock)
            {
                //设置目标投影
                IGeoDatasetSchemaEdit schemaEdit = geods as IGeoDatasetSchemaEdit;
                if (schemaEdit.CanAlterSpatialReference == true)
                {
                    ISchemaLock schemaLock = (ISchemaLock)geods;
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                        schemaEdit.AlterSpatialReference(sr);

                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        rbc = true;
                        //AppLogErrWrite.WriteErrLog("修改投影XYDoMain完成:" + (geods as IDataset).Name);

                    }
                    catch (Exception ex)
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        //AppLogErrWrite.WriteErrLog("修改投影XYDoMain出错:" + (geods as IDataset).Name + "\r\b" + ex.ToString());
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    //AppLogErrWrite.WriteErrLog((geods as IDataset).Name + "图层没权限修改空间投影!");
                    System.Diagnostics.Debug.WriteLine((geods as IDataset).Name + "图层没权限修改空间投影!");
                }
            }
            return rbc;
        }
        private IEnumDataset getEnumDataset(IWorkspace ws)
        {
            return ws.get_Datasets(esriDatasetType.esriDTAny);
        }
        private IEnumDataset getEnumSubDataset(IDataset ods)
        {
            IEnumDataset oEnumSubds = null;
            if (ods.Type == esriDatasetType.esriDTFeatureDataset)
            {
                oEnumSubds = ods.Subsets;
            }
            return oEnumSubds;
        }

        //-------------

        public bool SetSpatialReferenceToDB(ISpatialReference sr)
        {
            bool rbc = false;
            if (this.DefaultWorkSpace == null)
            {
                rbc = false;
                return rbc;
            }
            List<IDataset> AllGisDataSetList = new List<IDataset>();
            //获取本空间连接通道中的所有要素类和要素集对象
            IWorkspace ws = this.DefaultWorkSpace;
            IEnumDataset topEnumDataset = this.getEnumDataset(ws);
            if (topEnumDataset != null)
            {
                topEnumDataset.Reset();
                IDataset ds = topEnumDataset.Next();
                while (ds != null)
                {
                    switch (ds.Type)
                    {
                        case esriDatasetType.esriDTFeatureDataset:
                            //--
                            AllGisDataSetList.Add(ds);
                            //--
                            IEnumDataset subEnumDs = this.getEnumSubDataset(ds);
                            IDataset subds = subEnumDs.Next();
                            while (subds != null)
                            {
                                switch (subds.Type)
                                {
                                    case esriDatasetType.esriDTFeatureClass:
                                        rbc = this.SetSpatialReference(subds, sr);
                                        break;
                                }
                                System.Runtime.InteropServices.Marshal.ReleaseComObject(subds);
                                subds = subEnumDs.Next();
                            }
                            rbc = this.SetSpatialReference(ds, sr);
                            break;
                        case esriDatasetType.esriDTFeatureClass:
                            //--
                            rbc = this.SetSpatialReference(ds, sr);
                            //--
                            break;
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ds);
                    ds = topEnumDataset.Next();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(topEnumDataset);
            }
            return rbc;
        }

        private bool SetSpatialReference(IDataset ds, ISpatialReference sr)
        {
            bool rbc = false;
            IGeoDataset geods = null;

            geods = ds as IGeoDataset;
            if ((geods as IDataset).Type == esriDatasetType.esriDTFeatureDataset
                        && geods is IGeoDatasetSchemaEdit
                        && geods is ISchemaLock)
            {
                //设置目标投影
                IGeoDatasetSchemaEdit schemaEdit = geods as IGeoDatasetSchemaEdit;
                if (schemaEdit.CanAlterSpatialReference == true)
                {
                    ISchemaLock schemaLock = (ISchemaLock)geods;
                    try
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                        schemaEdit.AlterSpatialReference(sr);

                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        rbc = true;
                        //AppLogErrWrite.WriteErrLog("修改投影完成:" + (geods as IDataset).Name);

                    }
                    catch (Exception ex)
                    {
                        schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                        //AppLogErrWrite.WriteErrLog("修改投影出错:" + (geods as IDataset).Name + "\r\b" + ex.ToString());
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    //AppLogErrWrite.WriteErrLog((geods as IDataset).Name + "图层没权限修改空间投影!");
                }
            }
            return rbc;
        }
                

        /// <summary>
        /// //从本空间数据库中删除所有拓扑对象
        /// </summary>
        /// <returns></returns>
        public bool DeleteALLTopolgyFromGISDB()
        {
            bool rbc = true;
            try
            {
                IWorkspace ws = this.DefaultWorkSpace;
                if (ws != null)
                {
                    //读取所有拓扑
                    IEnumDataset topEnumDataset = this.getEnumDataset(ws);
                    if (topEnumDataset != null)
                    {
                        topEnumDataset.Reset();
                        IDataset ds = topEnumDataset.Next();
                        while (ds != null)
                        {
                            switch (ds.Type)
                            {
                                case esriDatasetType.esriDTFeatureDataset:
                                    if (ds is ITopologyContainer)
                                    {
                                        ITopologyContainer topContainer = ds as ITopologyContainer;
                                        ISchemaLock schemaLock = (ISchemaLock)ds;
                                        try
                                        {
                                            schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
                                            int tc = topContainer.TopologyCount;
                                            for (int i = tc - 1; i >= 0; i--)
                                            {
                                                ITopology top = topContainer.get_Topology(i);
                                                if (top != null && top is IDataset)
                                                {
                                                    //delete top's ITopologyRuleContainer 
                                                    ITopologyRuleContainer topruleList = top as ITopologyRuleContainer;
                                                    IEnumRule ER = topruleList.Rules;
                                                    ER.Reset();
                                                    IRule r = ER.Next();
                                                    while (r != null && r is ITopologyRule)
                                                    {
                                                        topruleList.DeleteRule(r as ITopologyRule);
                                                        r = ER.Next();
                                                    }
                                                    //delete top's featureclass
                                                    IFeatureClassContainer topFcList = top as IFeatureClassContainer;
                                                    for (int d = topFcList.ClassCount - 1; d >= 0; d--)
                                                    {
                                                        top.RemoveClass(topFcList.get_Class(d) as IClass);
                                                    }

                                                    //delete top object
                                                    (top as IDataset).Delete();
                                                    rbc = true;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //AppLogErrWrite.WriteErrLog(ex.ToString());
                                            System.Diagnostics.Debug.WriteLine(ex.ToString());
                                        }
                                        finally
                                        {
                                            schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                                        }
                                    }
                                    break;
                                case esriDatasetType.esriDTFeatureClass:
                                    break;
                            }
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(ds);
                            ds = topEnumDataset.Next();
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(topEnumDataset);
                    }//
                } //end ws!=null
            }
            catch (Exception ee)
            {
                rbc = false;
                //AppLogErrWrite.WriteErrLog(ee.ToString());
                System.Diagnostics.Debug.WriteLine(ee.ToString());

            }
            return rbc;
        }

        #endregion

        public IFeatureClass CreateFeatureClass(object p_WorkspaceOrDataSet, string p_Name, ISpatialReference p_SpatialReference, esriFeatureType p_FeatureType, esriGeometryType p_GeometryType, IFields p_Fields, string p_ConfigWord)
        {
            int num;  

            #region 输入参数判断
            if (p_WorkspaceOrDataSet == null)
            {
                MessageBox.Show("输入的工作空间或要素集为空！", "参数p_Workspace为Nothing！");
                return null;
            }

            if (!((p_WorkspaceOrDataSet is IWorkspace) || (p_WorkspaceOrDataSet is IFeatureDataset)))
            {
                MessageBox.Show("工作空间必需是IWorkSpace或IFeatureDataset类型！", "参数p_Workspace必需是IWorkSpace或IFeatureDataset类型！");
                return null;
            }

            if (string.IsNullOrEmpty(p_Name))
            {
                MessageBox.Show("输入的要素集名称为空！", "参数p_Name为Nothing！");
                return null;
            }
            
            if ((p_WorkspaceOrDataSet is IWorkspace) && (p_SpatialReference == null))
            {
                MessageBox.Show("空间参考信息为空！", "参数p_SpatialReference为空！");
                return null;
            }
            #endregion

            #region 处理mCLSID和mEXTCLSID
            UID uid = null;
            UID uid2 = null;
            switch (p_FeatureType)
            {
                case  esriFeatureType.esriFTSimple:  //1:
                    {
                        IObjectClassDescription description4 = new FeatureClassDescriptionClass();
                        uid = description4.InstanceCLSID; //.get_InstanceCLSID();
                        uid2 = description4.ClassExtensionCLSID;//.get_ClassExtensionCLSID();
                        break;
                    }
                case  (esriFeatureType)7:
                    new UIDClass().Value="{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                    p_GeometryType = (esriGeometryType)1;
                    break;

                case (esriFeatureType)8:
                    new UIDClass().Value="{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                    p_GeometryType = (esriGeometryType)3;
                    break;

                case (esriFeatureType)9:
                    new UIDClass().Value="{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                    break;

                case (esriFeatureType)10:
                    new UIDClass().Value="{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                    p_GeometryType = (esriGeometryType)3;
                    break;

                case (esriFeatureType)11:
                    {
                        IObjectClassDescription description = new AnnotationFeatureClassDescriptionClass();
                        uid = description.InstanceCLSID;//.get_InstanceCLSID();
                        uid2 = description.ClassExtensionCLSID;//.get_ClassExtensionCLSID();
                        p_GeometryType = (esriGeometryType)4;
                        break;
                    }
                case (esriFeatureType)12:
                    new UIDClass().Value="{4AEDC069-B599-424B-A374-49602ABAD308}";
                    p_GeometryType = (esriGeometryType)4;
                    break;

                case (esriFeatureType)13:
                    {
                        IObjectClassDescription description2 = new DimensionClassDescriptionClass();
                        uid = description2.InstanceCLSID;// get_InstanceCLSID();
                        uid2 = description2.ClassExtensionCLSID;//.get_ClassExtensionCLSID();
                        p_GeometryType = (esriGeometryType)4;
                        break;
                    }
                case (esriFeatureType)14:
                    {
                        IObjectClassDescription description3 = new RasterCatalogClassDescriptionClass();
                        uid = description3.InstanceCLSID;//.get_InstanceCLSID();
                        uid2 = description3.ClassExtensionCLSID;//.get_ClassExtensionCLSID();
                        p_GeometryType = (esriGeometryType)7;
                        break;
                    }
            }
            #endregion

            #region 处理字段
            bool flag2 = false;
            bool flag = false;
            IField field = null;
            IField field2 = null;
            if (p_Fields != null)
            {
                int num2 = p_Fields.FieldCount - 1;
                for (num = 0; num <= num2; num++)
                {
                    if (p_Fields.get_Field(num).Type == (esriFieldType)6)
                    {
                        field = p_Fields.get_Field(num);
                        flag2 = true;
                    }
                    if (p_Fields.get_Field(num).Type == (esriFieldType)7)
                    {
                        field2 = p_Fields.get_Field(num);
                        flag = true;
                    }
                }
            }
            IFields fields = new FieldsClass();
            IFieldsEdit edit = fields as IFieldsEdit;
            if (!flag2)
            {
                IField field3 = new FieldClass();
                IFieldEdit edit2 = field3 as IFieldEdit;
                edit2.Name_2="OBJECTID";
                edit2.AliasName_2="OBJECTID";
                edit2.Type_2 = esriFieldType.esriFieldTypeOID;// (6);
                edit.AddField(field3);
            }
            else
            {
                edit.AddField(field);
            }
            if (!flag)
            {
                IGeometryDef def = new GeometryDefClass();
                IGeometryDefEdit edit4 = def as IGeometryDefEdit;
                edit4.GeometryType_2=p_GeometryType;
                edit4.GridCount_2=1;
                edit4.set_GridSize(0, 1000.0);
                edit4.AvgNumPoints_2=2;
                edit4.HasM_2=false;
                edit4.HasZ_2=false;
                if (p_WorkspaceOrDataSet is IWorkspace)
                {
                    edit4.SpatialReference_2=p_SpatialReference;
                }
                IField field4 = new FieldClass();
                IFieldEdit edit3 = field4 as IFieldEdit;
                edit3.Name_2="SHAPE";
                edit3.AliasName_2="SHAPE";
                edit3.Type_2=(esriFieldType)7;
                edit3.GeometryDef_2=def;
                edit.AddField(field4);
                field2 = field4;
            }
            else
            {
                IGeometryDefEdit edit5 = field2.GeometryDef as IGeometryDefEdit;//.get_GeometryDef();
                if (p_WorkspaceOrDataSet is IWorkspace)
                {
                    edit5.SpatialReference_2 = p_SpatialReference;
                }
                edit.AddField(field2);
            }
            if (p_Fields != null)
            {
                int num3 = p_Fields.FieldCount - 1;
                for (num = 0; num <= num3; num++)
                {
                    IField field5 = p_Fields.get_Field(num);
                    if ((field5.Type != (esriFieldType)6) & (field5.Type != (esriFieldType)7))
                    {
                        edit.AddField(field5);
                    }
                }
            }

            #endregion

            string ShapeFieldName = field2.Name; //图形字段

            IFeatureClass class3 = null;
            if (p_WorkspaceOrDataSet is IWorkspace)
            {
                IWorkspace workspace2 = (IWorkspace)p_WorkspaceOrDataSet;
                IFeatureWorkspace workspace = workspace2 as IFeatureWorkspace;
                try
                {
                    class3 = workspace.CreateFeatureClass(p_Name, fields, uid, uid2, p_FeatureType, ShapeFieldName, p_ConfigWord);
                }
                catch (Exception exception1)
                {
                    Log.WriteLine(exception1);
                    MessageBox.Show(exception1.ToString(), "创建要素类错误！" + exception1.Message);
                   
                    return null;
                }
                return class3;
            }
            if (p_WorkspaceOrDataSet is IDataset)
            {
                IFeatureDataset dataset = (IFeatureDataset)p_WorkspaceOrDataSet;
                try
                {
                    class3 = dataset.CreateFeatureClass(p_Name, fields, uid, uid2, p_FeatureType, ShapeFieldName, p_ConfigWord);
                }
                catch (Exception exception3)
                {
                    Log.WriteLine(exception3);
                    MessageBox.Show(exception3.ToString(),"创建要素类错误！" + exception3.Message );

                    return null;
                }
            }
            return class3;
        }

        public IDataset CreateFeatureDataSet(IWorkspace p_WorkSpace, string p_Name, ISpatialReference p_SpatialReference)
        {
            if (p_WorkSpace == null)
            {
                MessageBox.Show("输入的工作空间为空！", "参数p_Workspace为Nothing！");
                return null;
            }

            if (string.IsNullOrEmpty(p_Name))
            {
                MessageBox.Show("输入的要素集名称为空！", "参数p_Name为Nothing！");
                return null;
            }

            if (p_SpatialReference == null)
            {
                MessageBox.Show("当前要素集的空间参考信息为空！", "参数p_SpatialReference为Nothing！");
                return null;
            }

            IFeatureWorkspace workspace = p_WorkSpace as IFeatureWorkspace;
            IDataset dataset2 = null;
            try
            {
                try
                {
                    dataset2 = workspace.OpenFeatureDataset(p_Name);
                }
                catch
                {
                    
                }
                if (dataset2 == null)
                {
                    dataset2 = workspace.CreateFeatureDataset(p_Name, p_SpatialReference);
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("创建要素集错误，可能是已经存同名要素集！" + exception1.Message, exception1.ToString());
                return null;
            }
            return dataset2;
        }

        public bool OpenFeatureDataSet(IWorkspace p_WorkSpace, string p_FeatureDataSetName, ref IDataset p_FeatureDataSet)
        {
            bool flag;
            try
            {
                IDataset dataset = (p_WorkSpace as IFeatureWorkspace).OpenFeatureDataset(p_FeatureDataSetName);
                p_FeatureDataSet = dataset;
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "打开要素集[" + p_FeatureDataSetName + "]错误！" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public bool OpenFeatureClass(IWorkspace p_WorkSpace, string p_FeatureClassName, ref IFeatureClass p_FeatureClass)
        {
            bool flag;
            try
            {
                IFeatureClass class2 = (p_WorkSpace as IFeatureWorkspace).OpenFeatureClass(p_FeatureClassName);
                p_FeatureClass = class2;
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show( exception.ToString(),"打开要素类[" + p_FeatureClassName + "]错误！" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public bool OpenFeatureClass(string p_ShapeFileName, ref IFeatureClass p_FeatureClass)
        {
            bool flag;
            string str2 = p_ShapeFileName.Substring(0, p_ShapeFileName.LastIndexOf(@"\"));
            string str = p_ShapeFileName.Substring(p_ShapeFileName.LastIndexOf(@"\") + 1);
            if (!File.Exists(p_ShapeFileName))
            {
                return false;
            }
            try
            {
                IFeatureWorkspace workspace = new ShapefileWorkspaceFactoryClass().OpenFromFile(str2, 0) as IFeatureWorkspace;
                IFeatureClass class2 = workspace.OpenFeatureClass(str.Substring(0, str.LastIndexOf(".")));
                p_FeatureClass = class2;
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(),"打开要素类错误！" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public bool OpenTable(IWorkspace p_WorkSpace, string p_TableName, ref ITable p_Table)
        {
            bool flag;
            try
            {
                ITable table = (p_WorkSpace as IFeatureWorkspace).OpenTable(p_TableName);
                p_Table = table;
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "打开表[" + p_TableName + "]错误！" + exception.Message);
                flag = false;
            }
            return flag;
        }

        public bool OpenTopology(IWorkspace p_WorkSpace, string p_TopologyName, ref ITopology p_Topology)
        {
            bool flag;
            try
            {
                p_Topology = (p_WorkSpace as ITopologyWorkspace).OpenTopology(p_TopologyName);
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(),"打开拓扑[" + p_TopologyName + "]错误！" + exception.Message );
                flag = false;
            }
            return flag;
        }


        public bool DeleteDataSet(IWorkspace p_WorkSpace, string p_DataSetName)
        {
            bool obj2;
            try
            {
                IFeatureWorkspace workspace = p_WorkSpace as IFeatureWorkspace;
                workspace.OpenFeatureDataset(p_DataSetName).Delete();
                obj2 = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(),"删除要素集[" + p_DataSetName + "]错误！" );
                obj2 = false;
            }
            return obj2;
        }

        public bool DeleteTopology(IWorkspace p_WorkSpace, string p_TopologyName)
        {
            bool flag;
            try
            {
                IDataset dataset = (p_WorkSpace as ITopologyWorkspace).OpenTopology(p_TopologyName) as IDataset;
                dataset.Delete();
                flag = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(),"删除拓扑[" + p_TopologyName + "]错误！" + exception.Message );
                flag = false;
            }
            return flag;
        }

        public bool DeleteLayer(IWorkspace p_WorkSpace, string p_LayerName)
        {
            bool obj2;
            try
            {
                IDataset dataset = (p_WorkSpace as IFeatureWorkspace).OpenFeatureClass(p_LayerName) as IDataset;
                dataset.Delete();
                obj2 = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(),"删除图层[" + p_LayerName + "]错误！" + exception.Message );
                obj2 = false;
            }
            return obj2;
        }


        //获取工作空间对象 getWorkSpace 
        public IWorkspace getWorkspaceByLayer(ILayer oLayer)
        {
            #region  实现细节
            if (oLayer == null) return null;

            IFeatureLayer oFLayer;
            IFeatureClass oFClass;
            IDataset oDataset;

            oFLayer = (IFeatureLayer)oLayer;
            oFClass = oFLayer.FeatureClass;
            oDataset = (IDataset)oFClass;

            if (oDataset == null)
                return null;
            else
                return oDataset.Workspace;
            #endregion
        }

        public IWorkspace getWorkspaceByIFeatureClass(IFeatureClass oFClass)
        {
            #region  实现细节
            if (oFClass == null) return null;

            IDataset oDataset;

            oDataset = (IDataset)oFClass;

            if (oDataset == null)
                return null;
            else
                return oDataset.Workspace;
            #endregion
        }

        public IWorkspace getWorkspaceByIDataset(IDataset oDataset)
        {
            #region  实现细节
            if (oDataset == null)
                return null;
            else
                return oDataset.Workspace;
            #endregion
        }

        //删除版本 delete Version
        public bool deleteVersionName(string tpVersionName, IWorkspace p_Workspace)
        {
            #region infomation
            bool rbc = false;
            IVersion oiv = null;
            IVersionedWorkspace oivWs = (IVersionedWorkspace)p_Workspace;
            try
            {
                oiv = oivWs.FindVersion(tpVersionName);
                oiv.Delete();
                rbc = true;
            }
            catch (Exception ee)
            {
                rbc = false;
                throw ee;
            }
            oivWs = null;
            oiv = null;
            return rbc;
            #endregion
        }


        //创建版本 Create Version
        public bool CreateVersionName(string tpVersionName, IWorkspace p_Workspace)
        {
            #region infomation
            bool rbc = false;
            IVersion oiv = null;
            IVersionedWorkspace oivWs = (IVersionedWorkspace)p_Workspace;

            try
            {
                oiv = oivWs.FindVersion(tpVersionName);
                if (oiv == null)
                {
                    oiv = (IVersion)oivWs;
                    oiv.CreateVersion(tpVersionName);
                    rbc = true;
                }
            }
            catch
            {
                if (oiv == null)
                {
                    oiv = (IVersion)oivWs;
                    oiv.CreateVersion(tpVersionName);
                    rbc = true;
                }
            }
            return rbc;
            #endregion
        }

        //提交版本ByIWorkspace /Post Version By IWorkspace
        public bool PostVersion(IWorkspace p_WorkSpace)
        {
            bool rbc = false;
            IWorkspaceEdit owsEdit = p_WorkSpace as IWorkspaceEdit;
            try
            {
                owsEdit.StartEditing(false);
                IVersionEdit3 oVersionEdit3 = p_WorkSpace as IVersionEdit3;
                IVersion oVersion = p_WorkSpace as IVersion;
                if (oVersionEdit3.Reconcile3(oVersion.VersionInfo.VersionName, true, true) == false)
                {
                    if (oVersion.VersionInfo.VersionName.ToUpper().IndexOf("DEFAULT") < 0)
                    {
                        if (oVersionEdit3.Reconcile3(oVersion.VersionInfo.Parent.VersionName, true, true) == false && oVersionEdit3.CanPost() == true)
                        {
                            oVersionEdit3.Post(oVersion.VersionInfo.Parent.VersionName);
                            rbc = true;

                        }
                    }
                    else
                    {
                        rbc = false;
                        MessageBox.Show("默认版本不能提交版本！", "警告");
                    }
                }
                owsEdit.StopEditing(true);
            }
            catch (Exception ee)
            {
                rbc = false;
                MessageBox.Show("提交版本时发现错误！" + ee.Message, "PostVersion");
            }
            return rbc;
        }

        //提交版本ByIWorkspace and TargetVersionName/Post Version By IWorkspace and p_TargetVersionName
        public bool PostVersion(IWorkspace p_WorkSpace, string p_TargetVersionName)
        {
            bool rbc = false;
            IWorkspaceEdit owsEdit = p_WorkSpace as IWorkspaceEdit;
            try
            {
                owsEdit.StartEditing(false);
                IVersionEdit3 oVersionEdit3 = p_WorkSpace as IVersionEdit3;
                IVersion oVersion = p_WorkSpace as IVersion;
                if (oVersionEdit3.Reconcile3(oVersion.VersionInfo.VersionName, true, true) == false)
                {
                    if (oVersion.VersionInfo.VersionName.ToUpper().IndexOf("DEFAULT") < 0)
                    {
                        if (oVersionEdit3.Reconcile3(p_TargetVersionName, true, true) == false && oVersionEdit3.CanPost() == true)
                        {
                            oVersionEdit3.Post(p_TargetVersionName);
                            rbc = true;

                        }
                    }
                    else
                    {
                        rbc = false;
                        MessageBox.Show("默认版本不能提交版本！", "警告");
                    }
                }
                owsEdit.StopEditing(true);
            }
            catch (Exception ee)
            {
                rbc = false;
                MessageBox.Show("提交版本时发现错误！" + ee.Message, "PostVersion");
            }
            return rbc;

        }
 
        
        public IWorkspace GetAccessWorkSpace(string p_FileName)
        {
            IWorkspace workspace;
            try
            {
                workspace = new AccessWorkspaceFactoryClass().OpenFromFile(p_FileName, 0);
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.ToString(),"获得个人数据库工作空间错误！" + exception1.Message );
                workspace = null;
                
            }
            return workspace;
        }

        public ICadDrawingDataset GetCadDrawingDataset(ref string strCadWorkspacePath, ref string strCadFileName)
        {
            IWorkspaceName name3 = new WorkspaceNameClass();
            name3.WorkspaceFactoryProgID="esriDataSourcesFile.CadWorkspaceFactory";
            name3.PathName=strCadWorkspacePath;
            IDatasetName name = new CadDrawingNameClass();
            name.Name=strCadFileName;
            name.WorkspaceName=name3;
            IName name2 = name as IName;
            return (ICadDrawingDataset)name2.Open();
        }


        public static object CreateFilePropertySet(string p_FileName)
        {
            IPropertySet set = new PropertySetClass();
            set.SetProperty("DATABASE", p_FileName);
            return set;
        }
        public static object CreateOLEDBPropertySet(string p_FileName)
        {
            IPropertySet set = new PropertySetClass();
            set.SetProperty("CONNECTSTRING", "Provider=MSDASQL.1;Data Source=" + p_FileName);
            return set;
        }
        public static IPropertySet CreateSdePropertySet(string p_SdeDataSourceName, string p_SdeInstance, string p_SdeDataUserName, string p_SdeDataPassword, string p_Version)
        {
            IPropertySet set2 = new PropertySetClass();
            IPropertySet set3 = set2;
            set3.SetProperty("SERVER", p_SdeDataSourceName);
            set3.SetProperty("INSTANCE", p_SdeInstance);
            set3.SetProperty("DATABASE", "");
            set3.SetProperty("USER", p_SdeDataUserName);
            set3.SetProperty("PASSWORD", p_SdeDataPassword);
            set3.SetProperty("VERSION", p_Version);
            set3 = null;
            return set2;
        }

        //另存为一个本地*.sde连接文件
        public static bool SaveAsPropertySet_sdefile(IPropertySet ps,string sdefile)
        {
            IWorkspaceFactory wf = null;
            //保存空间数据库连接
            object o = ps.GetProperty("Database");
            if (o != null)
            {
                switch (System.IO.Path.GetExtension(o.ToString()).ToUpper())
                {
                    case ".MDB":
                        wf = new AccessWorkspaceFactoryClass();
                        break;
                    case ".GDB":
                        wf = new FileGDBWorkspaceFactoryClass();
                        break;
                    default:
                        wf = new SdeWorkspaceFactoryClass();
                        break;
                }
            }
            string f = System.IO.Path.GetFileName(sdefile);
            string p = sdefile.Substring(0, sdefile.Length - f.Length);
            if (wf != null)
            {
                wf.Create(p, f, ps, 0);
            }
            return true;
        }

        //压缩数据库方法
        public static bool CompressDataBase(IWorkspace ws)
        {
            IVersionedWorkspace c = ws as IVersionedWorkspace;
            if (c != null)
            {
                c.Compress();
            }
            return true;
        }

 

    }

    public interface IGDBServerInfoConfig
    {
        /// <summary>
        /// 数据库连接类型
        /// </summary>
        EnumSDEServer enumSDEServer { get; set; }

        string SDEConnectionString { get; set; }

        string SDE_Server { get; set; }
        string SDE_Port { get; set; }
        string SDE_User { get; set; }
        string SDE_PWD { get; set; }
        string SDE_DataBase { get; set; }
        string SDE_Version { get; set; }


        string DB_Server { get; set; }
        string DB_DataBase { get; set; }
        string DB_User { get; set; }
        string DB_PWD { get; set; }

        IPropertySet GetPropertySet();
        bool OpenSDEConnection();

        IWorkspace DefaultWorkSpace { get; set; }

        IFeatureWorkspace featWs { get; }
        IFeatureClass getFeatureClass(string LayerName);
        ITable getTable(string LayerName);
        IWorkspace GetAccessWorkSpace(string p_FileName);
        ICadDrawingDataset GetCadDrawingDataset(ref string strCadWorkspacePath, ref string strCadFileName);

        IFeatureClass CreateFeatureClass(object p_WorkspaceOrDataSet, string p_Name, ISpatialReference p_SpatialReference, esriFeatureType p_FeatureType, esriGeometryType p_GeometryType, IFields p_Fields, string p_ConfigWord);
        IDataset CreateFeatureDataSet(IWorkspace p_WorkSpace, string p_Name, ISpatialReference p_SpatialReference);

        bool OpenFeatureDataSet(IWorkspace p_WorkSpace, string p_FeatureDataSetName, ref IDataset p_FeatureDataSet);
        bool OpenFeatureClass(IWorkspace p_WorkSpace, string p_FeatureClassName, ref IFeatureClass p_FeatureClass);
        bool OpenFeatureClass(string p_ShapeFileName, ref IFeatureClass p_FeatureClass);
        bool OpenTable(IWorkspace p_WorkSpace, string p_TableName, ref ITable p_Table);
        bool OpenTopology(IWorkspace p_WorkSpace, string p_TopologyName, ref ITopology p_Topology);

        bool DeleteDataSet(IWorkspace p_WorkSpace, string p_DataSetName);
        bool DeleteTopology(IWorkspace p_WorkSpace, string p_TopologyName);
        bool DeleteLayer(IWorkspace p_WorkSpace, string p_LayerName);

        bool ReadXml(string mconfigFile);
        bool SaveXml(string mConfigFile);

        string getProfessionalConnectionString { get; }
        string getADOConnectionString { get; }

        //DataProviderType getDataProviderType();

        //设置投影的XYDoMain的功能
        bool SetSpatialReferenceXYDoMainToDB(double pMinX, double pMinY, double pMaxX, double pMaxY);

        //设置投影的功能
        bool SetSpatialReferenceToDB(ISpatialReference sr);

        //从本空间数据库中删除所有拓扑对象
        bool DeleteALLTopolgyFromGISDB();

    }

    public enum EnumSDEServer
    {
        Access,
        Oracle,
        SQLServer,
        FileGDB,   //文件GDB数据库格式
    }    
    
}
