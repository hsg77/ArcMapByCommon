using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 执行命令接口
    /// </summary>
    public interface IGeoProcessCommand
    {
        bool Execute(Geoprocessor gp);

        IGPProcess Process { get; }

        string Message { get; set; }
    }

    /// <summary>
    /// 默认适配器 执行命令接口
    /// </summary>
    public abstract class GeoProcessCommandClass : IGeoProcessCommand, IDisposable
    {
        private string m_msg = "";
        protected IGPProcess m_process = null;

        #region IGeoProcessCommand 成员

        public virtual bool Execute(Geoprocessor gp)
        {
            if (gp == null) gp = new Geoprocessor();

            bool rbc = false;
            gp.OverwriteOutput = true;
            gp.AddOutputsToMap = false;
            //输入参数 end
            gp.ClearMessages();
            IGeoProcessorResult results = (IGeoProcessorResult)gp.Execute(this.Process, null);
            if (results != null && results.Status == esriJobStatus.esriJobSucceeded)
            {
                object sf = new object();
                Message += gp.GetMessages(ref sf);
                rbc = true;
            }
            else
            {
                Message = "执行失败.";
                object sf = new object();
                Message += gp.GetMessages(ref sf);
                rbc = false;
            }
            if (gp != null)
            {
                gp = null;
            }
            return rbc;
        }

        public string Message
        {
            get
            {
                return m_msg;
            }
            set
            {
                m_msg = value;
            }
        }

        /// <summary>
        /// read/write
        /// </summary>
        public IGPProcess Process
        {
            get { return m_process; }
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            if (this.Process != null)
            {
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(this.Process);
                //GIS.ArcGIS.TokayWorkspace.ComRelease(this.m_process);
                this.m_process = null;
            }
        }

        #endregion
    }
}
