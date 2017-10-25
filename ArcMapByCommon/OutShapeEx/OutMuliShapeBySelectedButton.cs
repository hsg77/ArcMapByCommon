using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public class OutMuliShapeBySelectedButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                frmOutMuliShapeBySelectedGeometryUI ui = new frmOutMuliShapeBySelectedGeometryUI();
                //
                Control m = Form.FromHandle(new IntPtr(ArcMap.Application.hWnd));
                ui.Show(m);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }
    }
}
