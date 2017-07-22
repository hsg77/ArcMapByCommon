using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;


namespace ArcMapByCommon
{
    public partial class frmSelfIntersectionFeature : Form
    {
        private IEngineEditor m_Editor = null;

        public frmSelfIntersectionFeature()
        {
            InitializeComponent();

            this.treeFeatures.AfterSelect += new TreeViewEventHandler(treeFeatures_AfterSelect);
            this.cbxRings.SelectedIndexChanged += new EventHandler(cbxRings_SelectedIndexChanged);
            this.timer1.Tick += new EventHandler(timer1_Tick);
            this.btnRemovePoint.Click += new EventHandler(btnRemovePoint_Click);
            this.checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
        }

        void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (checkBox1.Checked)
                cbxRings_SelectedIndexChanged(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //删除节点事件
        void btnRemovePoint_Click(object sender, EventArgs e)
        {
            try
            {
                object misobj = Type.Missing;
                m_Editor.StartOperation();

                IGeometry ring = (cbxRings.SelectedItem as CommonComboBoxItem).Tag as IGeometry;
                IPointCollection pc = ring as IPointCollection;
                pc.RemovePoints((int)listPointCollection.SelectedItems[0].Tag, 1);

                IFeature feature = (treeFeatures.SelectedNode.Tag as IFeature);
                IGeometryCollection geoCol = cbxRings.Tag as IGeometryCollection;
                geoCol.AddGeometry(ring, ref misobj, ref misobj);
                geoCol.RemoveGeometries(cbxRings.SelectedIndex, 1);
                feature.Shape = geoCol as IGeometry;
                feature.Store();

                int removePointIndex = listPointCollection.SelectedItems[0].Index;
                listPointCollection.SelectedItems[0].Remove();

                m_Editor.StopOperation("RemovePoint");
                this.cbxRings_SelectedIndexChanged(sender, e);
                if (listPointCollection.Items.Count > removePointIndex)
                {
                    listPointCollection.Items[removePointIndex].Selected = true;
                    listPointCollection.TopItem = listPointCollection.SelectedItems[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void cbxRings_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //加载坐标点
                listPointCollection.BeginUpdate();
                listPointCollection.Items.Clear();
                IGeometry geo = (cbxRings.SelectedItem as CommonComboBoxItem).Tag as IGeometry;
                IPointCollection pc = geo as IPointCollection;
                Dictionary<int, string> pointDict = new Dictionary<int, string>();

                for (int i = 0; i < pc.PointCount; i++)
                {
                    IPoint p = pc.get_Point(i);
                    long xInt = (long)(p.X * 1000);
                    long yInt = (long)(p.Y * 1000);
                    double x = xInt / 1000.0;
                    double y = yInt / 1000.0;
                    //判断是否重复点
                    bool isSelfIntersection = false;
                    isSelfIntersection = pointDict.ContainsValue(x.ToString() + y.ToString());
                    pointDict.Add(i, x.ToString() + y.ToString());

                    ListViewItem item = new ListViewItem();
                    item.Text = (i + 1).ToString();
                    item.SubItems.Add(x.ToString());
                    item.SubItems.Add(y.ToString());
                    item.Tag = i;
                    if (checkBox1.Checked)
                    {
                        if (isSelfIntersection && i != pc.PointCount - 1)
                        {
                            //将重复点加入列表
                            foreach (int tmpKey in this.GetKeyByValue(pointDict, x.ToString() + y.ToString()))
                            {
                                if (tmpKey != i)
                                {
                                    ListViewItem tmpItem = new ListViewItem();
                                    tmpItem.Text = (tmpKey + 1).ToString();
                                    tmpItem.SubItems.Add(x.ToString());
                                    tmpItem.SubItems.Add(y.ToString());
                                    tmpItem.Tag = tmpKey;
                                    listPointCollection.Items.Add(tmpItem);
                                }
                            }
                            listPointCollection.Items.Add(item);
                        }
                    }
                    else
                    {
                        listPointCollection.Items.Add(item);
                    }

                    if (isSelfIntersection && i != pc.PointCount - 1)
                    {
                        //设置重复点为红色
                        foreach (int tmpKey in this.GetKeyByValue(pointDict, x.ToString() + y.ToString()))
                        {
                            foreach (ListViewItem tmpItem in listPointCollection.Items)
                            {
                                if ((int)tmpItem.Tag == tmpKey)
                                {
                                    tmpItem.ForeColor = Color.Red;
                                    break;
                                }
                            }
                        }
                        item.ForeColor = Color.Red;
                    }
                }

                listPointCollection.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        int[] GetKeyByValue(Dictionary<int, string> dict, string value)
        {
            List<int> keys=new List<int>();
            foreach (KeyValuePair<int, string> pair in dict)
            {
                if (pair.Value == value)
                    keys.Add(pair.Key);
            }
            return keys.ToArray();
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            btnRemovePoint.Enabled = listPointCollection.SelectedItems.Count > 0;
        }

        void treeFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            cbxRings.Tag = null;
            cbxRings.Items.Clear();

            if (treeFeatures.SelectedNode != null)
            {
                IFeature feature = treeFeatures.SelectedNode.Tag as IFeature;
                IGeometry geometry = feature.ShapeCopy;
                (geometry as ITopologicalOperator).Simplify();

                switch (geometry.GeometryType)
                {
                    case esriGeometryType.esriGeometryPolygon:
                        //圈号
                        foreach (KeyValuePair<string, IRing> ring in PolygonHelper.GetAllRings(geometry as IPolygon))
                        {
                            IGeometry geo = ring.Value as IGeometry;
                            CommonComboBoxItem item = new CommonComboBoxItem("", "", geo);
                            item.Text = ring.Key;
                            cbxRings.Items.Add(item);
                        }
                        break;
                    case esriGeometryType.esriGeometryPolyline:
                        IGeometryCollection geoCol = geometry as IGeometryCollection;
                        for (int i = 0; i < geoCol.GeometryCount; i++)
                        {
                            cbxRings.Items.Add(new CommonComboBoxItem("第" + (i + 1).ToString() + "部分", "", geoCol.get_Geometry(i)));
                        }
                        break;
                }

                cbxRings.Tag = geometry;
                if (cbxRings.Items.Count > 0)
                    cbxRings.SelectedIndex = 0;
            }
        }

        public IEngineEditor Editor
        {
            get
            {
                return m_Editor;
            }
            set
            {
                m_Editor = value;

                this.treeFeatures.BeginUpdate();
                this.treeFeatures.Nodes.Clear();
                IEnumFeature featureSelection = m_Editor.EditSelection;
                IFeature feature = featureSelection.Next();
                while (feature != null)
                {
                    TreeNode node = new TreeNode();
                    int fieldIndex = feature.Fields.FindField("BSM");
                    if (fieldIndex != -1)
                        node.Text = feature.get_Value(fieldIndex).ToString();
                    else
                        node.Text = string.Empty;
                    node.Tag = feature;
                    this.treeFeatures.Nodes.Add(node);

                    feature = featureSelection.Next();
                }
                this.treeFeatures.EndUpdate();
                this.lblFeatureCount.Text = string.Format("选择要素（{0}）：", this.treeFeatures.Nodes.Count);

                if (this.treeFeatures.Nodes.Count > 0)
                    this.treeFeatures.SelectedNode = this.treeFeatures.Nodes[0];
            }
        }

        private void frmSelfIntersectionFeature_Load(object sender, EventArgs e)
        {

        }
    }
}
