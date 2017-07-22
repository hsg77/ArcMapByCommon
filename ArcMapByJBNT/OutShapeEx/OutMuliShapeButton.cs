using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
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
                ui.Show();
            }
            catch (Exception ee)
            {               
                MessageBox.Show(ee.Message, "提示");
            }
        }
    }
}
