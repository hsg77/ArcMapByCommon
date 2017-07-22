using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 椭球面积计算工具类
    /// </summary>
    public class EllipseAreaComputeButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmEllipseAreaComputeUI ui = null;
        public EllipseAreaComputeButton()
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
                    ui = new frmEllipseAreaComputeUI();
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
