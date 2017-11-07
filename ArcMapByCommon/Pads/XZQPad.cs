using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;

namespace ArcMapByCommon.Pads
{
    [Guid("460405a1-215c-4701-bed2-7f3f5ad7bb69")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("ArcMapByCommon.Pads.XZQPad")]
    public partial class XZQPad : UserControl, IDockableWindowDef
    {
        private IApplication m_application;

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
            MxDockableWindows.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxDockableWindows.Unregister(regKey);

        }

        #endregion
        #endregion

        public XZQPad(object hook)
        {
            InitializeComponent();
            this.Hook = hook;
        }
        /// <summary>
        /// Host object of the dockable window
        /// </summary>
        private object Hook
        {
            get;
            set;
        }
        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private XZQPad m_windowUI;

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new XZQPad(this.Hook);
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose(disposing);

                base.Dispose(disposing);
            }

        }

        #region IDockableWindowDef Members

        string IDockableWindowDef.Caption
        {
            get
            {
                //TODO: Replace with locale-based initial title bar caption
                return "行政区地标";
            }
        }

        int IDockableWindowDef.ChildHWND
        {
            get { return this.Handle.ToInt32(); }
        }

        string IDockableWindowDef.Name
        {
            get
            {
                //TODO: Replace with any non-localizable string
                return this.Name;
            }
        }

        void IDockableWindowDef.OnCreate(object hook)
        {
            m_application = hook as IApplication;
        }

        void IDockableWindowDef.OnDestroy()
        {
            //TODO: Release resources and call dispose of any ActiveX control initialized
        }

        object IDockableWindowDef.UserData
        {
            get { return null; }
        }

        #endregion

        private void labelPlaceholder_Click(object sender, EventArgs e)
        {

        }
    }
}
