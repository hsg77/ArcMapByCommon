using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public partial class frmScanPhone2DValue : Form
    {
        public frmScanPhone2DValue()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.fd_mmValue.Text = "";
                //
                string qrCode = this.fd_phoneValue.Text.Trim();
                string[] qrarray = qrCode.Split(',');
                if (qrarray != null && qrarray.Length > 1)
                {
                    string xzqcode = qrarray[0];
                    string xm = qrarray.Length > 1 ? qrarray[1] : "";
                    string sfzh = qrarray.Length > 2 ? qrarray[2] : "";
                    //
                    sfzh = !string.IsNullOrEmpty(sfzh) ? CryptoUtil.DecryptString(sfzh, "") : "";
                    this.fd_mmValue.Text = xzqcode + "," + xm + "," + sfzh;
                    //
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message+"/r/n"+ee.StackTrace, "提示");
            }
        }

        private void frmScanPhone2DValue_Load(object sender, EventArgs e)
        {

        }
    }
}
