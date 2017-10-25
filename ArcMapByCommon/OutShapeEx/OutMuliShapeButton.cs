using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 输出多个Shape文件功能
    /// </summary>
    public class OutMuliShapeButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                frmOutMuliShapeUI ui = new frmOutMuliShapeUI();
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
