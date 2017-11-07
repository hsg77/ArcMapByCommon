using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public class ShowPadButton: ESRI.ArcGIS.Desktop.AddIns.Button
    {        
        public ShowPadButton()
        {
        }

        protected override void OnClick()
        {
            try
            {
                //Get dockable window
                UID dockWinID = new UIDClass();
                dockWinID.Value = ThisAddIn.IDs.DockableWindow1;
                IDockableWindow s_dockWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);
                s_dockWindow.Caption = "面板1";
                s_dockWindow.Dock(esriDockFlags.esriDockTabbed);
                s_dockWindow.Show(true);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + " " + ee.StackTrace);
            }
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
