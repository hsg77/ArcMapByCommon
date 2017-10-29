
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    //ProgressDialog2
    public partial class ProgressDialog2 : Form, IProgressDialog2
    {
        public ProgressDialog2()
        {
            InitializeComponent();
        }
        #region IProgressDialog2 成员

        public string Message1
        {
            get
            {
                return lb1.Text;
            }
            set
            {
                lb1.Text = value;
                this.Refresh();
            }
        }

        public string Message2
        {
            get
            {
                return lb2.Text;
            }
            set
            {
                lb2.Text = value;
                this.Refresh();
            }
        }

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

        #endregion

        #region IProgressBar2 成员

        public int Maximum1
        {
            get
            {
                return pb1.Maximum;
            }
            set
            {
                pb1.Maximum = value;
            }
        }

        public int Minimum1
        {
            get
            {
                return pb1.Minimum;
            }
            set
            {
                pb1.Minimum = value;
            }
        }

        public int Step1
        {
            get
            {
                return pb1.Step;
            }
            set
            {
                pb1.Step = value;
            }
        }

        public int Value1
        {
            get
            {
                return pb1.Value;
            }
            set
            {
                pb1.Value = value;
            }
        }

        public int Maximum2
        {
            get
            {
                return pb2.Maximum;
            }
            set
            {
                pb2.Maximum = value;
            }
        }

        public int Minimum2
        {
            get
            {
                return pb2.Minimum;
            }
            set
            {
                pb2.Minimum = value;
            }
        }

        public int Step2
        {
            get
            {
                return pb2.Step;
            }
            set
            {
                pb2.Step = value;
            }
        }

        public int Value2
        {
            get
            {
                return pb2.Value;
            }
            set
            {
                pb2.Value = value;
            }
        }

        #endregion
    }


    interface IProgressBar2
    {
        // 获取或设置最大值
        int Maximum1
        {
            get;
            set;
        }
        // 获取或设置最小值
        int Minimum1
        {
            get;
            set;
        }
        // 获取或设置步增值
        int Step1
        {
            get;
            set;
        }
        // 获取或设置进展
        int Value1
        {
            get;
            set;
        }

        // 获取或设置最大值
        int Maximum2
        {
            get;
            set;
        }
        // 获取或设置最小值
        int Minimum2
        {
            get;
            set;
        }
        // 获取或设置步增值
        int Step2
        {
            get;
            set;
        }
        // 获取或设置进展
        int Value2
        {
            get;
            set;
        }
    }

    interface IProgressDialog2 : IWin32Window, IProgressBar2
    {
        string Message1 { get; set; }
        string Message2 { get; set; }
        bool IsCancel { get; set; }
    }
        
}
