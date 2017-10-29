using System.Windows.Forms;
namespace ArcMapByCommon
{
    partial class frmScan2DRenameJpgFileUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl4 = new System.Windows.Forms.Label();
            this.btnDirSelect = new System.Windows.Forms.Button();
            this.labelControl7 = new System.Windows.Forms.Label();
            this.buttonEdit1 = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.simpleButton2 = new System.Windows.Forms.Button();
            this.labelControl5 = new System.Windows.Forms.Label();
            this.labelControl3 = new System.Windows.Forms.Label();
            this.labelControl2 = new System.Windows.Forms.Label();
            this.labelControl6 = new System.Windows.Forms.Label();
            this.labelControl8 = new System.Windows.Forms.Label();
            this.labelControl1 = new System.Windows.Forms.Label();
            this.btnOpenErrorLogFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelControl4
            // 
            this.labelControl4.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(12, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(189, 24);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "选择扫描识别前目录";
            // 
            // btnDirSelect
            // 
            this.btnDirSelect.Location = new System.Drawing.Point(418, 48);
            this.btnDirSelect.Name = "btnDirSelect";
            this.btnDirSelect.Size = new System.Drawing.Size(39, 23);
            this.btnDirSelect.TabIndex = 38;
            this.btnDirSelect.Text = "...";
            this.btnDirSelect.Click += new System.EventHandler(this.btnDirSelect_Click);
            // 
            // labelControl7
            // 
            this.labelControl7.ForeColor = System.Drawing.Color.Red;
            this.labelControl7.Location = new System.Drawing.Point(12, 178);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(231, 24);
            this.labelControl7.TabIndex = 28;
            this.labelControl7.Text = "选择扫描识别后存储目录";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(417, 204);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Size = new System.Drawing.Size(39, 23);
            this.buttonEdit1.TabIndex = 37;
            this.buttonEdit1.Text = "...";
            this.buttonEdit1.Click += new System.EventHandler(this.buttonEdit1_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(292, 256);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 41);
            this.btnImport.TabIndex = 25;
            this.btnImport.Text = "开始扫描识别";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(418, 256);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(81, 41);
            this.simpleButton2.TabIndex = 29;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 100);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(318, 14);
            this.labelControl5.TabIndex = 31;
            this.labelControl5.Tag = " ";
            this.labelControl5.Text = "XX县保护卡数据/乡镇1/XXX村/保护卡签章后扫描的文件.jpg";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 75);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(81, 14);
            this.labelControl3.TabIndex = 30;
            this.labelControl3.Text = "注：/ 表示目录";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 125);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(365, 14);
            this.labelControl2.TabIndex = 32;
            this.labelControl2.Tag = " ";
            this.labelControl2.Text = "XX县保护卡数据/乡镇1/XXX村/yyy社组/保护卡签章后扫描的文件.jpg";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(393, 100);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(132, 14);
            this.labelControl6.TabIndex = 33;
            this.labelControl6.Text = "(村级目录存放方式)";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(393, 125);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(132, 14);
            this.labelControl8.TabIndex = 34;
            this.labelControl8.Text = "(组级目录存放方式)";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(162, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(352, 14);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "选择乡镇的上级目录   即 XX县保护卡或责任书数据 目录";
            // 
            // btnOpenErrorLogFile
            // 
            this.btnOpenErrorLogFile.Location = new System.Drawing.Point(12, 256);
            this.btnOpenErrorLogFile.Name = "btnOpenErrorLogFile";
            this.btnOpenErrorLogFile.Size = new System.Drawing.Size(107, 41);
            this.btnOpenErrorLogFile.TabIndex = 36;
            this.btnOpenErrorLogFile.Text = "打开日志文件";
            this.btnOpenErrorLogFile.Click += new System.EventHandler(this.btnOpenErrorLogFile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 21);
            this.textBox1.TabIndex = 39;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 205);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(400, 21);
            this.textBox2.TabIndex = 40;
            // 
            // frmScan2DRenameJpgFileUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 309);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnOpenErrorLogFile);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.btnDirSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmScan2DRenameJpgFileUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫描识别重命名功能";
            this.Load += new System.EventHandler(this.frmScan2DRenameJpgFileUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelControl4;
        private Button btnDirSelect;
        private Label labelControl7;
        private Button buttonEdit1;
        private Button btnImport;
        private Button simpleButton2;
        private Label labelControl5;
        private Label labelControl3;
        private Label labelControl2;
        private Label labelControl6;
        private Label labelControl8;
        private Label labelControl1;
        private Button btnOpenErrorLogFile;
        private TextBox textBox1;
        private TextBox textBox2;
    }
}