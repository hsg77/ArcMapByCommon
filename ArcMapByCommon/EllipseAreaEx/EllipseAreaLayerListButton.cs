using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 定义 对LayerList图层列表的椭球面积计算 功能
    /// </summary>
    public class EllipseAreaLayerListButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmEllipseAreaLayerListUI ui = null;
        public EllipseAreaLayerListButton()
        {
        }

        protected override void OnClick()
        {
            //
            //  TODO: Sample code showing how to access button host
            //
            ArcMap.Application.CurrentTool = null;
            //
            try
            {
                if (ui == null || ui.IsDisposed == true)
                {
                    ui = new frmEllipseAreaLayerListUI();
                }
                ui.Show();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.StackTrace, "提示");
            }
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
