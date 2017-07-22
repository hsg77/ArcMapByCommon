/*********************************************
 * 功能：定义输出Shape单个文件的窗体
 * vp:hsg
 * create date:2009-06-29
 * 
 *********************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 定义输出Shape单个文件的窗体
    /// </summary>
    public partial class frmOutShapeUI : Form
    {       
        public frmOutShapeUI()
        {
            InitializeComponent();
        }

        //Load 事件
        private void frmOutShapeUI_Load(object sender, EventArgs e)
        {
            this.comboBox1.Items.Clear();
            IFeatureClass fc = null;
            ZhMapClass zhmap = new ZhMapClass(ArcMap.Document.FocusMap);
            IFeatureClass[] fcArray = zhmap.GetFeatureClassArray();
            for (int i = 0; i < fcArray.Length; i++)
            {
                fc = fcArray[i];
                if (fc != null)
                {
                    CommonComboBoxItem itm = new CommonComboBoxItem();
                    itm.Text = fc.AliasName;
                    itm.Tag = fc;
                    this.comboBox1.Items.Add(itm);
                }
            }
        }

        //另存为输出Shape文件  事件
        private void Btn_SaveAsShape_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sf = new SaveFileDialog();
                sf.Title = "另存为输出Shape文件";
                sf.Filter = "Shape文件(*.shp)|*.shp";
                sf.ShowDialog();
                string tpFile = sf.FileName;
                if (tpFile != null && tpFile != "")
                {
                    this.textBox1.Text = tpFile;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

        //关闭 事件
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //开始输出  事件
        private void Btn_OutPutShape_Click(object sender, EventArgs e)
        {
            try
            {
                this.Btn_OutPutShape.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                object obj = this.comboBox1.SelectedItem;
                if (obj != null && obj is CommonComboBoxItem)
                {
                    CommonComboBoxItem item = obj as CommonComboBoxItem;
                    IFeatureClass fc = item.Tag as IFeatureClass;

                    string outShapeFilePath = this.textBox1.Text;// 
                    if (outShapeFilePath.Trim() == "")
                    {
                        MessageBox.Show("请先录入要输出的Shape文件的路径!", "提示");
                        return;
                    }

                    OutShapeClass shpOp = new OutShapeClass();
                    shpOp.ParentForm = this;
                    if (cb_IsObjGeo.Checked == true)
                    {
                        shpOp.OIDFieldName = "OBJECTID";
                        shpOp.GeometryFieldName = "GEOMETRY";
                        shpOp.IsObjGeo = true;
                    }
                    if (IsSelected.Checked == false)
                    {  //对整表要素
                        shpOp.OutPut(fc, outShapeFilePath);
                    }
                    else
                    {  //对已选择要素
                        ZhMapClass zhmap = new ZhMapClass(ArcMap.Document.FocusMap);
                        string fc_Name = (fc as IDataset).Name;
                        fc_Name=TokayWorkspace.GetDatasetName(fc as IDataset);
                        IFeatureLayer featLayer = zhmap.GetFeatureLayerByFeatureClassName(fc_Name);
                        shpOp.OutPutSelectedFeature(featLayer, outShapeFilePath);
                    }
                    MessageBox.Show("输出Shape成功!", "提示");
                }
            }
            catch (Exception ee)
            {                
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.Btn_OutPutShape.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }
        //--
    }
}
