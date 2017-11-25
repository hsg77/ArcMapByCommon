using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace ArcMapByCommon
{
    public partial class frmUpdateBsmBatchUI : Form
    {
        public frmUpdateBsmBatchUI()
        {
            InitializeComponent();
        }
        private void frmUpdateBsmBatchUI_Load(object sender, EventArgs e)
        {
            //初始化图层
            this.checkedListBox1.Items.Clear();
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
                    this.checkedListBox1.Items.Add(itm);
                }
            }
        }
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //
        private void CB_CheckALL_CheckedChanged(object sender, EventArgs e)
        {
            bool IsCheckALL = this.CB_CheckALL.Checked;
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, IsCheckALL);
            }
        }
        //开始批量更新 事件
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            frmProgressBar2 pb2 = null;
            try
            {
                this.BtnUpdate.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                string LineText = "";
                                
                if (MessageBox.Show("请确认是否要对选中项进行批量更新BSM字段的值操作?", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.BtnUpdate.Enabled = true;
                    this.Cursor = Cursors.Default;
                    return;
                }

                //获取选中的要素类
                List<IFeatureClass> fcList = new List<IFeatureClass>();
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    if (this.checkedListBox1.GetItemChecked(i) == true)
                    {   //选中要素类
                        CommonComboBoxItem cbitem = this.checkedListBox1.Items[i] as CommonComboBoxItem;
                        fcList.Add(cbitem.Tag as IFeatureClass);
                    }
                }
                if (fcList != null && fcList.Count > 0)
                {
                    pb2 = new frmProgressBar2();
                    pb2.Text = "批量更新BSM字段进度...";
                    pb2.progressBar1.Maximum = fcList.Count + 1;
                    pb2.progressBar1.Value = 0;
                    pb2.Caption1.Text = "正在更新...";
                    pb2.Show(this);
                    Application.DoEvents();
                    //
                    ZHFeaturePoint zhfeat = null;
                    int featIndex = 0;
                    int featCount = 0;
                    //
                    int BSMIndex=0;
                    for (int i = 0; i < fcList.Count; i++)
                    {
                        featIndex = 0;
                        IFeatureClass fc = fcList[i];
                        pb2.Caption1.Text = "正在更新图层:" + fc.AliasName + "...";
                        pb2.progressBar1.Value = i;
                        Application.DoEvents();
                        //
                        featCount = fc.FeatureCount(null);
                        pb2.progressBar2.Value = 0;
                        pb2.progressBar2.Maximum = featCount + 1;
                        pb2.Caption2.Text = "正在更新要素...";
                        Application.DoEvents();
                        //
                        IFeatureCursor fcur = fc.Update(null, false);
                        IFeature feat = fcur.NextFeature();
                        while (feat != null)
                        {
                            featIndex += 1;
                            //---
                            BSMIndex+=1;
                            //---
                            if (featIndex % 200 == 0)
                            {
                                pb2.Caption2.Text = "正在更新图层:" + fc.AliasName + "(当前:" + featIndex.ToString() + "/" + featCount.ToString() + ")";
                                pb2.progressBar2.Value = featIndex;
                                Application.DoEvents();
                                fcur.Flush();
                            }
                            zhfeat = new ZHFeaturePoint(feat);
                            zhfeat.setFieldValue("BSM", BSMIndex);
                            fcur.UpdateFeature(feat);
                            //
                            feat = fcur.NextFeature();
                        }
                        fcur.Flush();
                        if (fcur != null)
                        {
                            TokayWorkspace.ComRelease(fcur);
                            fcur = null;
                        }                        
                    }
                    if (pb2 != null)
                    {
                        pb2.Close();
                        pb2.Dispose();
                        pb2 = null;
                    }
                    MessageBox.Show("更新BSM完毕!", "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.BtnUpdate.Enabled = true;
                this.Cursor = Cursors.Default;
                if (pb2 != null)
                {
                    pb2.Close();
                    pb2.Dispose();
                    pb2 = null;
                }
            }
        }
        //
    }
}
