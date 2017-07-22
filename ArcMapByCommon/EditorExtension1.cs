using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Editor;

namespace ArcMapByCommon
{
    /// <summary>
    /// EditorExtension1 class implementing custom ESRI Editor Extension functionalities.
    /// </summary>
    public class EditorExtension1 : ESRI.ArcGIS.Desktop.AddIns.Extension
    {
        public EditorExtension1()
        {
        }

        protected override void OnStartup()
        {
            IEditor theEditor = ArcMap.Editor;
        }

        protected override void OnShutdown()
        {
        }

        #region Editor Events

        #region Shortcut properties to the various editor event interfaces
        private IEditEvents_Event Events
        {
            get { return ArcMap.Editor as IEditEvents_Event; }
        }
        private IEditEvents2_Event Events2
        {
            get { return ArcMap.Editor as IEditEvents2_Event; }
        }
        private IEditEvents3_Event Events3
        {
            get { return ArcMap.Editor as IEditEvents3_Event; }
        }
        private IEditEvents4_Event Events4
        {
            get { return ArcMap.Editor as IEditEvents4_Event; }
        }
        #endregion

        void WireEditorEvents()
        {
            //
            //  TODO: Sample code demonstrating editor event wiring
            //
            Events.OnCurrentTaskChanged += delegate
            {
                if (ArcMap.Editor.CurrentTask != null)
                    System.Diagnostics.Debug.WriteLine(ArcMap.Editor.CurrentTask.Name);
            };
            Events2.BeforeStopEditing += delegate(bool save) { OnBeforeStopEditing(save); };
        }

        void OnBeforeStopEditing(bool save)
        {
        }
        #endregion

    }

}
