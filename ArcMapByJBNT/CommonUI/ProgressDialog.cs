using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    public partial class ProgressDialog : Form, IProgressDialog
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        private void ProgressDialog_Load(object sender, EventArgs e)
        {

        }

        #region IProgressDialog成员

        public bool IsCancel
        {
            get
            {
                return this.btnCancel.Visible;
            }
            set
            {
                this.btnOK.Visible = this.btnCancel.Visible = this.ControlBox = value;
            }
        }

        public string Message
        {
            get
            {
                return labelControl1.Text;
            }
            set
            {
                labelControl1.Text = value;
                this.Refresh();
            }
        }

        #endregion

        #region IProgressBar 成员

        public int Maximum
        {
            get
            {
                return this.xtraProgressControl1.Maximum;
            }
            set
            {
                this.xtraProgressControl1.Maximum = value;
            }
        }

        public int Minimum
        {
            get
            {
                return this.xtraProgressControl1.Minimum;
            }
            set
            {
                this.xtraProgressControl1.Minimum = value;
            }
        }

        public int Step
        {
            get
            {
                return this.xtraProgressControl1.Step;
            }
            set
            {
                this.xtraProgressControl1.Step = value;
            }
        }

        public int Value
        {
            get
            {
                return this.xtraProgressControl1.Value;
            }
            set
            {
                this.xtraProgressControl1.Value = value;
            }
        }

        #endregion

        private ProgressBarStyle m_Style = ProgressBarStyle.Marquee;
        public ProgressBarStyle Style
        {
            get
            {
                return m_Style;
            }
            set
            {
                m_Style = value;
            }
        }
        private int m_MarqueeAnimationSpeed = 1;
        public int MarqueeAnimationSpeed
        {
            get
            {
                return m_MarqueeAnimationSpeed;
            }
            set
            {
                m_MarqueeAnimationSpeed = value;
            }
        }
    }
    //public interface IProgressBar
    //{
    //    /// <summary>
    //    /// 获取或设置最大值
    //    /// </summary>
    //    int Maximum
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// 获取或设置最小值
    //    /// </summary>
    //    int Minimum
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// 获取或设置步增值
    //    /// </summary>
    //    int Step
    //    {
    //        get;
    //        set;
    //    }
    //    /// <summary>
    //    /// 获取或设置进展
    //    /// </summary>
    //    int Value
    //    {
    //        get;
    //        set;
    //    }
    //}

    public interface IProgressDialog : IWin32Window, IProgressBar
    {
        string Message { get; set; }
        bool IsCancel { get; set; }
    }
}
