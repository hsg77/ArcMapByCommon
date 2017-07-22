using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon.JBNTEx
{
    public partial class frmXZDWMJ_KCDLMJ_JBNT : Form
    {
        public frmXZDWMJ_KCDLMJ_JBNT()
        {
            InitializeComponent();
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            try
            {
                double tbmj = CommonClass.TNum(this.fd_TBMJ.Text.Trim());
                double kcdlxs = CommonClass.TNum(this.fd_TKXS.Text.Trim());
                double lxdwmj = CommonClass.TNum(this.fd_LXDWMJ.Text.Trim());
                double checkTBDLMJ = CommonClass.TNum(this.fd_TBDLMJ.Text.Trim());
                //
                double xzdwmj = tbmj - checkTBDLMJ / (1 - kcdlxs) - lxdwmj;
                xzdwmj = Math.Round(xzdwmj, 2);
                //
                double kcdlmj = (tbmj - xzdwmj - lxdwmj) * kcdlxs;                
                kcdlmj = Math.Round(kcdlmj, 2);
                //
                this.fd_XZDWMJ.Text = xzdwmj.ToString();
                this.fd_TKMJ.Text = kcdlmj.ToString();
                //
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "提示");
            }
        }

        private void frmXZDWMJ_KCDLMJ_JBNT_Load(object sender, EventArgs e)
        {

        }
    }
}
