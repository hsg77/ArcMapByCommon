﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public class updateSubStrButton : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        private frmUpdateSubStrUI ui = null;
        public updateSubStrButton()
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
                    ui = new frmUpdateSubStrUI();
                }
                Control  m=Form.FromHandle(new IntPtr(ArcMap.Application.hWnd));
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
