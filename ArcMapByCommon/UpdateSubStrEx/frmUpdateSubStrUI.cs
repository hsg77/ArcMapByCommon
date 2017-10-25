using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapByCommon
{
    public partial class frmUpdateSubStrUI : Form
    {
        public frmUpdateSubStrUI()
        {
            InitializeComponent();
        }

        private void frmUpdateSubStrUI_Load(object sender, EventArgs e)
        {
            try
            {
                this.comboBox1.Items.Clear();
                IMap map = ArcMap.Document.FocusMap;
                ILayer[] layerList = GlobalFunction.GetAllLayer(map);
                foreach (ILayer layer in layerList)
                {
                    ItemClass item = new ItemClass(layer.Name, layer);
                    this.comboBox1.Items.Add(item);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, "提示");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.comboBox2.Items.Clear();
                this.comboBox3.Items.Clear();
                ItemClass item = this.comboBox1.SelectedItem as ItemClass;
                if (item == null)
                {
                    MessageBox.Show("请先选择要更新的图层", "提示");
                    return;
                }
                ILayer layer = item.Value as ILayer;
                IFeatureLayer featLayer = layer as IFeatureLayer;
                if (featLayer != null)
                {
                    IFeatureClass fc = featLayer.FeatureClass;
                    if (fc != null)
                    {
                        for (int i = 0; i < fc.Fields.FieldCount; i++)
                        {
                            string name = fc.Fields.get_Field(i).Name.ToLower();
                            ItemClass fd_item = new ItemClass();
                            fd_item.Value = fc.Fields.get_Field(i).AliasName;
                            fd_item.Caption = fc.Fields.get_Field(i).Name;
                            if (name.Contains("shape") || name.Contains("objectid"))
                            {
                                continue;
                            }
                            else
                            {
                                this.comboBox2.Items.Add(fd_item);
                                this.comboBox3.Items.Add(fd_item);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, "提示");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            ProgressDialog pd = null;
            try
            {
                ItemClass item = this.comboBox1.SelectedItem as ItemClass;
                if (item == null)
                {
                    MessageBox.Show("请先选择要更新的图层", "提示");
                    return;
                }
                ItemClass fd_item = this.comboBox2.SelectedItem as ItemClass;
                if (fd_item == null)
                {
                    MessageBox.Show("请先选择要更新的字段", "提示");
                    return;
                }

                ItemClass fd_from = this.comboBox3.SelectedItem as ItemClass;
                if (fd_from == null)
                {
                    MessageBox.Show("请先选择值来源的字段", "提示");
                    return;
                }
                int index_from = CommonClass.TInt(this.fd_indexFrom.Value);
                int index_to = CommonClass.TInt(this.fd_indexTo.Value);
                if (index_from < 0)
                {
                    MessageBox.Show("取子字符串只能从大于等于0开始！", "提示");
                    return;
                }
                //
                ILayer layer = item.Value as ILayer;
                IFeatureLayer featLayer = layer as IFeatureLayer;
                IFeatureClass fc = featLayer.FeatureClass;
                //                
                string fdName = fd_item.Caption;
                string fdfromName = fd_from.Caption;
                //
                int fCount = fc.FeatureCount(null);
                if (fCount > 0)
                {
                    pd = new ProgressDialog();
                    pd.Text = "进度";
                    pd.Message = "计算扣除地类系数中......";
                    pd.Minimum = 0;
                    pd.Maximum = fCount;
                    pd.Show(this);
                    //
                    Application.DoEvents();
                    //
                    IFeatureCursor pcursor = fc.Update(null, false);
                    IFeature pf = pcursor.NextFeature();
                    int index = fc.FindField(fdfromName);
                    int indexUpdate = fc.FindField(fdName);
                    //
                    int n = 0;
                    while (pf != null)
                    {
                        n = n + 1;
                        if (n % 200 == 0)
                        {
                            pd.Value = n;
                            pd.Message = "取子字符串中......" + pd.Value.ToString() + "/" + pd.Maximum.ToString();
                            Application.DoEvents();
                            pcursor.Flush();
                        }
                        object v = pf.get_Value(index);
                        if (v != null)
                        {
                            string value_from = v.ToString();
                            string result_v = value_from;
                            if (index_to < value_from.Length)
                            {
                                result_v = value_from.Substring(index_from, index_to);                                
                            }
                            else
                            {
                                result_v = value_from.Substring(index_from);  
                            }
                            //
                            pf.set_Value(indexUpdate, result_v);
                            pcursor.UpdateFeature(pf);
                        }
                        pf = pcursor.NextFeature();
                    }
                    pcursor.Flush();
                    if (pcursor != null)
                    {
                        TokayWorkspace.ComRelease(pcursor);
                        pcursor = null;
                    }
                    if (pd != null)
                    {
                        pd.Dispose();
                        pd = null;
                    }
                    MessageBox.Show("更新完毕！", "提示");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, "提示");
            }
            finally
            {
                if (pd != null)
                {
                    pd.Dispose();
                    pd = null;
                }
            }
        }
    }
}
