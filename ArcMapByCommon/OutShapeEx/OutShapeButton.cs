/*********************************************
 * 功能：输出单个shape文件
 * vp:hsg
 * create date:2009-06-29
 * 
 *********************************************/
using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 输出单个shape文件 
    /// </summary>
    public class OutShapeButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        protected override void OnClick()
        {
            try
            {
                frmOutShapeUI ui = new frmOutShapeUI();                
                ui.Show();
            }
            catch (Exception ee)
            {                
                MessageBox.Show(ee.Message, "提示");
            }
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
    //
}
