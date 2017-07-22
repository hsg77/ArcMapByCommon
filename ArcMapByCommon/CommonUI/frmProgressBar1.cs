using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public partial class frmProgressBar1 : Form, IKDProgressBar1
    {
        public frmProgressBar1()
        {
            InitializeComponent();
            this.Caption1.Text = "";
        }

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