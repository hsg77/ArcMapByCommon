using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 图形面积计算工具
    /// vp:hsg
    /// create date:2017-05-14
    /// </summary>
    public class GeoAreaComputeButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmGeoAreaComputeUI ui = null;
        public GeoAreaComputeButton()
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
                    ui = new frmGeoAreaComputeUI();
                }
                Control m = Form.FromHandle(new IntPtr(ArcMap.Application.hWnd));
                ui.Show(m);
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
