using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;

namespace ArcMapByCommon
{
    //public class Tool1 : ESRI.ArcGIS.Desktop.AddIns.Tool
    //{
    //    public Tool1()
    //    {
    //    }

    //    protected override void OnUpdate()
    //    {
    //        Enabled = ArcMap.Application != null;
    //    }
    //}
    public class Tool1 : ESRI.ArcGIS.Desktop.AddIns.Tool
    {
        public Tool1()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            MessageBox.Show("hello tool");
        }
        protected override void OnMouseDown(MouseEventArgs arg)
        {
            base.OnMouseDown(arg);
            IPoint p = ArcMap.Document.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(arg.X, arg.Y);
            MessageBox.Show("hello tool OnMouseDown" + p.X.ToString() + " " + p.Y.ToString());
        }
    }

}
