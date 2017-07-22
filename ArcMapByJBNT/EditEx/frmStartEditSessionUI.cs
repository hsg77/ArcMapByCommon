using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ArcMapByJBNT;


namespace ArcMapByJBNT
{
    /// <summary>
    /// 定义开始编辑窗体
    /// </summary>
    public partial class frmStartEditSessionUI : Form
    {
        public ZhMapClass zhmap = null;
        public frmStartEditSessionUI()
        {
            InitializeComponent();
        }

        public void initWorkSpacePathList(List<string> WorkSpacePathList)
        {
            this.listBox1.Items.Clear();
            foreach (string t in WorkSpacePathList)
            {
                this.listBox1.Items.Add(t);
            }
        }

        //Load 事件
        private void frmStartEditSessionUI_Load(object sender, EventArgs e)
        {
            if (ArcMap.Document.FocusMap.LayerCount > 0)
            {
                this.zhmap = new ZhMapClass(ArcMap.Document.FocusMap);
                List<string> WorkspaceList = zhmap.GetWorkSpacePathList();                
                this.initWorkSpacePathList(WorkspaceList);                
            }
            //
            this.listBox2.Enabled = true;
            this.listBox2.ScrollAlwaysVisible = true;
            //
            this.initCurrentEditLayer();
        }

        private void initCurrentEditLayer()
        {
            EngineEditorClass eds = new EngineEditorClass();
            ILayer L = eds.TargetLayer;
            if (L != null)
            {
                this.lab_CurrentEditLayer.Text = "当前编辑图层：" + L.Name;
            }
            else
            {
                this.lab_CurrentEditLayer.Text = "当前编辑图层：";
            }
        }

        //关闭
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //开始编辑
        private void Btn_Start_Click(object sender, EventArgs e)
        {
            try
            {
                string Layername = this.listBox2.Text;

                IFeatureLayer featLayer = this.zhmap.GetFeatureLayerByLayerName(Layername);
                if (featLayer != null)
                {
                    EngineEditorClass engineEditSession = new EngineEditorClass();
                    engineEditSession.StartEditing((featLayer.FeatureClass as IDataset).Workspace, this.zhmap.Map);
                    engineEditSession.SetTargetLayer(featLayer, 0);
                    //
                    initCurrentEditLayer();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }            
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.zhmap != null)
            {
                string workspacepath = this.listBox1.Text;
                List<string> LayerNameList = this.zhmap.GetLayerNameOfWorkSpace(workspacepath);
                this.listBox2.Items.Clear();
                foreach (string t in LayerNameList)
                {
                    this.listBox2.Items.Add(t);
                }
            }
        }
        //
    }
}
