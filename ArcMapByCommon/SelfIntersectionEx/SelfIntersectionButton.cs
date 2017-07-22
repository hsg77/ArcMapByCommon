using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace ArcMapByCommon
{
    public class SelfIntersectionButton: ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private IEngineEditor m_editor = null;
        private frmSelfIntersectionFeature ui = null;
        public SelfIntersectionButton()
        {
            m_editor = new EngineEditorClass();
            (m_editor as IEngineEditEvents_Event).OnSelectionChanged += new IEngineEditEvents_OnSelectionChangedEventHandler(SelfIntersectionFeatureCommand_OnSelectionChanged);
        }
        private void SelfIntersectionFeatureCommand_OnSelectionChanged()
        {
            if (ui != null && !ui.IsDisposed)
                ui.Editor = m_editor;
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
                    ui = new frmSelfIntersectionFeature();                    
                }
                ui.Editor = m_editor;
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
