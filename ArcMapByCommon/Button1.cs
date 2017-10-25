using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ArcMapByCommon.UpdateBHIndex;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public class Button1 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmUpdateBHIndexUI ui = null;
        public Button1()
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
                if (ui==null || ui.IsDisposed == true)
                {
                    ui = new frmUpdateBHIndexUI();
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
