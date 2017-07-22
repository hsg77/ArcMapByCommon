using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    public class OutMuliShapeBySelectedButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                frmOutMuliShapeBySelectedGeometryUI ui = new frmOutMuliShapeBySelectedGeometryUI();
                ui.Show(ArcMap.Application as Form);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }
    }
}
