using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArcMapByCommon
{
    public class Combo1 : ESRI.ArcGIS.Desktop.AddIns.ComboBox
    {
        public Combo1()
        {
        }

        protected override void OnUpdate()
        {
            Enabled = ArcMap.Application != null;
        }
    }

}
