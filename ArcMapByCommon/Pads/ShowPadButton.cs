using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace ArcMapByCommon
{
    public class ShowPadButton: ESRI.ArcGIS.Desktop.AddIns.Button
    {        
        public ShowPadButton()
        {
        }

        protected override void OnClick()
        {
            //Get dockable window
            UID dockWinID = new UIDClass();
            dockWinID.Value = ThisAddIn.IDs.DockableWindow1;
            IDockableWindow s_dockWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);
            s_dockWindow.Dock(esriDockFlags.esriDockRight);
            s_dockWindow.Show(true);
        }
        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }
}
