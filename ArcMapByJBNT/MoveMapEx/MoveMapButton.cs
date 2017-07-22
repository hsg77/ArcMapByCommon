using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    public class MoveMapButton: ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmMapMoveUI ui = null;
        public MoveMapButton()
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
                    ui = new frmMapMoveUI();
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
