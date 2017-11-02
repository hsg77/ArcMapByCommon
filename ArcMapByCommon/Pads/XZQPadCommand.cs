using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.esriSystem;

namespace ArcMapByCommon.Pads
{
    /// <summary>
    /// Summary description for dockable window toggle command
    /// </summary>
    [Guid("99cbb30a-5bf9-41a1-877f-5b77a779369f")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapByCommon.Pads.CommandXZQPadCommand")]
    public sealed class XZQPadCommand : BaseCommand
    {
        private IApplication m_application;
        private IDockableWindow m_dockableWindow;

        private const string DockableWindowGuid = "{460405a1-215c-4701-bed2-7f3f5ad7bb69}";

        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxCommands.Unregister(regKey);

        }

        #endregion
        #endregion

        public XZQPadCommand()
        {
            //
            // TODO: Define values for the public properties
            //
            base.m_category = "DeveloperTemplate"; //localizable text
            base.m_caption = "Show/Hide Window";  //localizable text
            base.m_message = "Command toggles dockable window visibility (C#)";  //localizable text 
            base.m_toolTip = "Toggle Dockable Window (C#)";  //localizable text 
            base.m_name = "DeveloperTemplate_XZQPadCommand";   //unique id, non-localizable (e.g. "MyCategory_ArcMapCommand")

            try
            {
                //
                // TODO: change bitmap name if necessary
                //
                string bitmapResourceName = GetType().Name + ".bmp";
                base.m_bitmap = new Bitmap(GetType(), bitmapResourceName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, "Invalid Bitmap");
            }
        }

        #region Overridden Class Methods

        /// <summary>
        /// Occurs when this command is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (hook != null)
                m_application = hook as IApplication;

            if (m_application != null)
            {
                SetupDockableWindow();
                base.m_enabled = m_dockableWindow != null;
            }
            else
                base.m_enabled = false;
        }

        /// <summary>
        /// Toggle visiblity of dockable window and show the visible state by its checked property
        /// </summary>
        public override void OnClick()
        {
            if (m_dockableWindow == null)
                return;

            if (m_dockableWindow.IsVisible())
                m_dockableWindow.Show(false);
            else
                m_dockableWindow.Show(true);

            base.m_checked = m_dockableWindow.IsVisible();
        }

        public override bool Checked
        {
            get
            {
                return m_dockableWindow != null && m_dockableWindow.IsVisible();
            }
        }
        #endregion

        private void SetupDockableWindow()
        {
            if (m_dockableWindow == null)
            {
                IDockableWindowManager dockWindowManager = m_application as IDockableWindowManager;
                if (dockWindowManager != null)
                {
                    UID windowID = new UIDClass();
                    windowID.Value = DockableWindowGuid;
                    m_dockableWindow = dockWindowManager.GetDockableWindow(windowID);
                }
            }
        }
    }
}
