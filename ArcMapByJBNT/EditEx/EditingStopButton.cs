using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;

namespace ArcMapByJBNT
{
    public class EditingStopButton: ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public EditingStopButton()
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
                EngineEditorClass engineEditSession = new EngineEditorClass();
                if (MessageBox.Show("是否要保存已修改的数据", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    engineEditSession.StopEditing(true);
                }
                else
                {
                    engineEditSession.StopEditing(false);
                }
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
