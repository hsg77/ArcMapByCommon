using System;
using System.Collections.Generic;
using System.Text;
using ArcMapByJBNT.UpdateBHIndex;
using System.Windows.Forms;

namespace ArcMapByJBNT
{    
    public class updateFieldAddZeroPrexButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmUpdateFieldAddZeroPrexUI ui = null;
        public updateFieldAddZeroPrexButton()
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
                    ui = new frmUpdateFieldAddZeroPrexUI();
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
