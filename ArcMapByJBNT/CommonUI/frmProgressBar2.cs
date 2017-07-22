using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    public partial class frmProgressBar2 : Form, IKDProgressBar2
    {
        public frmProgressBar2()
        {
            InitializeComponent();
            this.Caption1.Text = "";
            this.Caption2.Text = "";
        }

        #region IKDProgressBar2 成员

        public string KDCaption2
        {
            get
            {
                return this.Caption2.Text;
            }
            set
            {
                this.Caption2.Text = value;
            }
        }

        public System.Windows.Forms.ProgressBar KDProgressBar2
        {
            get
            {
                return this.progressBar2;
            }
            set
            {
                this.progressBar2 = value;
            }
        }

        #endregion

        #region IKDProgressBar1 成员

        public string KDCaption1
        {
            get
            {
                return this.Caption1.Text;
            }
            set
            {
                this.Caption1.Text = value;
            }
        }

        public System.Windows.Forms.ProgressBar KDProgressBar1
        {
            get
            {
                return this.progressBar1;
            }
            set
            {
                this.progressBar1 = value;
            }
        }

        #endregion

        #region IKDProgressBar 成员

        public string KDTitle
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }
        public void KDShow()
        {
            this.Show();
        }

        public void KDClose()
        {
            this.Close();
        }

        public void KDHide()
        {
            this.Hide();
        }
        public void KDRefresh()
        {
            this.Refresh();
        }

        #endregion
    }
}