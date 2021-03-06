﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ArcMapByCommon.JBNTEx;

namespace ArcMapByCommon
{    
    public class ComputeXZDWMJTKMJByJBNTButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmXZDWMJ_KCDLMJ_JBNT ui = null;
        public ComputeXZDWMJTKMJByJBNTButton()
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
                    ui = new frmXZDWMJ_KCDLMJ_JBNT();
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
            this.Enabled = true;
        }
    }
}
