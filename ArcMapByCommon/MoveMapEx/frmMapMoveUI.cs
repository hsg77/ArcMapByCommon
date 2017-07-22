using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

namespace ArcMapByCommon
{
    /// <summary>
    /// 定义 地图平移窗体功能
    /// vp:hsg
    /// create date:2010-11
    /// </summary>
    public partial class frmMapMoveUI : Form
    {        
        public frmMapMoveUI()
        {
            InitializeComponent();
        }
        //Load 事件
        private void frmMapMoveUI_Load(object sender, EventArgs e)
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

        //关闭 事件
        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //开始平移 事件
        private void Btn_MapMove_Click(object sender, EventArgs e)
        {
            frmProgressBar2 pb2 = null;
            try
            {
                this.Btn_MapMove.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                string LineText = "";

                double dx = 0.0;
                double dy = 0.0;

                dx = CommonClass.TNum(this.numericBox1.Text.Trim());
                dy = CommonClass.TNum(this.numericBox2.Text.Trim());

                if (MessageBox.Show("请确认是否要对选中项进行整体平移操作?", "提示", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.Btn_MapMove.Enabled = true;
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
                    pb2.Text = "平移进度...";
                    pb2.progressBar1.Maximum = fcList.Count + 1;
                    pb2.progressBar1.Value = 0;
                    pb2.Caption1.Text = "正在平移...";
                    pb2.Show(this);
                    Application.DoEvents();
                    //
                    ZHFeaturePoint zhfeat = null;
                    int featIndex = 0;
                    int featCount = 0;
                    for (int i =0; i < fcList.Count; i++)
                    {
                        featIndex = 0;
                        IFeatureClass fc = fcList[i];                        
                        pb2.Caption1.Text = "正在平移图层:" + fc.AliasName + "...";
                        pb2.progressBar1.Value = i;
                        Application.DoEvents();
                        //
                        featCount = fc.FeatureCount(null);
                        pb2.progressBar2.Value = 0;
                        pb2.progressBar2.Maximum = featCount + 1;                        
                        pb2.Caption2.Text = "正在平移要素...";
                        Application.DoEvents();
                        //
                        IFeatureCursor fcur = fc.Update(null, false);
                        IFeature feat = fcur.NextFeature();
                        while (feat != null)
                        {
                            featIndex += 1;
                            if (featIndex % 200 == 0)
                            {
                                pb2.Caption2.Text = "正在平移图层:" + fc.AliasName + "(当前:" + featIndex.ToString() + "/" + featCount.ToString() + ")";
                                pb2.progressBar2.Value = featIndex;
                                Application.DoEvents();
                                fcur.Flush();
                            }
                            zhfeat = new ZHFeaturePoint(feat);
                            if (fc.FeatureType == esriFeatureType.esriFTAnnotation)
                            {   //注记要素移动 OK
                                IAnnotationFeature anFeat = feat as IAnnotationFeature;                                
                                IElement TextEl = (IElement)((IClone)anFeat.Annotation).Clone();
                                ITransform2D trans = TextEl as ITransform2D;
                                trans.Move(dx, dy);                                
                                anFeat.Annotation = TextEl;
                                fcur.UpdateFeature(feat);
                            }
                            else
                            {   //一般要素移动 OK
                                zhfeat.Move(dx, dy);
                                fcur.UpdateFeature(feat);
                            }
                            //
                            feat = fcur.NextFeature();
                        }
                        fcur.Flush();
                        if (fcur != null)
                        {
                            TokayWorkspace.ComRelease(fcur);
                            fcur = null;
                        }
                        pb2.Caption2.Text = "正在重新计算Extent范围...";
                        Application.DoEvents();
                        //重新计算Extent范围
                        TokayWorkspace.UpdateExtent(fc);                                              
                    }
                    if (pb2 != null)
                    {
                        pb2.Close();
                        pb2.Dispose();
                        pb2 = null;
                    }
                    MessageBox.Show("地图平移完毕!", "提示");
                }
            }
            catch (Exception ee)
            {
                Log.WriteLine(ee);
                MessageBox.Show(ee.Message, "提示");
            }
            finally
            {
                this.Btn_MapMove.Enabled = true;
                this.Cursor = Cursors.Default;
                if (pb2 != null)
                {
                    pb2.Close();
                    pb2.Dispose();
                    pb2 = null;
                }
            }
        }

        private void CB_CheckALL_CheckedChanged(object sender, EventArgs e)
        {
            bool IsCheckALL = this.CB_CheckALL.Checked;
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, IsCheckALL);
            }
        }

        private void numericBox1_Load(object sender, EventArgs e)
        {

        }
    }
}
