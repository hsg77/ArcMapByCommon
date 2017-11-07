using System.Windows.Forms;
namespace ArcMapByCommon
{
    partial class frmScanUnionOneJpgUI
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
            this.labelControl6 = new System.Windows.Forms.Label();
            this.labelControl5 = new System.Windows.Forms.Label();
            this.labelControl3 = new System.Windows.Forms.Label();
            this.btnDirSelect = new System.Windows.Forms.Button();
            this.simpleButton2 = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.labelControl2 = new System.Windows.Forms.Label();
            this.buttonEdit1 = new System.Windows.Forms.Button();
            this.labelControl1 = new System.Windows.Forms.Label();
            this.labelControl4 = new System.Windows.Forms.Label();
            this.labelControl7 = new System.Windows.Forms.Label();
            this.btnScanJpg = new System.Windows.Forms.Button();
            this.labelControl8 = new System.Windows.Forms.Label();
            this.labelControl9 = new System.Windows.Forms.Label();
            this.labelControl10 = new System.Windows.Forms.Label();
            this.labelControl11 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(27, 139);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(375, 14);
            this.labelControl6.TabIndex = 16;
            this.labelControl6.Text = "XX县保护卡数据/乡镇1/XXX村/姓名(身份证号)位置示意图.jpg";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(28, 116);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(315, 14);
            this.labelControl5.TabIndex = 15;
            this.labelControl5.Tag = " ";
            this.labelControl5.Text = "XX县保护卡数据/乡镇1/XXX村/姓名(身份证号).jpg";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(27, 91);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(81, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "注：/ 表示目录";
            // 
            // btnDirSelect
            // 
            this.btnDirSelect.Location = new System.Drawing.Point(430, 59);
            this.btnDirSelect.Name = "btnDirSelect";
            this.btnDirSelect.Size = new System.Drawing.Size(50, 23);
            this.btnDirSelect.TabIndex = 40;
            this.btnDirSelect.Text = "...";
            this.btnDirSelect.Click += new System.EventHandler(this.btnDirSelect_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(435, 317);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(81, 41);
            this.simpleButton2.TabIndex = 18;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(324, 317);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(88, 41);
            this.btnImport.TabIndex = 17;
            this.btnImport.Text = "开始合并";
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(415, 14);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "请先选择原先导出保护卡目录:(选择乡镇的上级目录）即 XX县保护卡数据 目录";
            // 
            // buttonEdit1
            // 
            this.buttonEdit1.Location = new System.Drawing.Point(432, 275);
            this.buttonEdit1.Name = "buttonEdit1";
            this.buttonEdit1.Size = new System.Drawing.Size(45, 23);
            this.buttonEdit1.TabIndex = 39;
            this.buttonEdit1.Text = "...";
            this.buttonEdit1.Click += new System.EventHandler(this.buttonEdit1_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(14, 258);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(148, 14);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "选择导出合并后保护卡目录:";
            // 
            // labelControl4
            // 
            this.labelControl4.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Location = new System.Drawing.Point(12, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(147, 24);
            this.labelControl4.TabIndex = 22;
            this.labelControl4.Text = "选择合并前目录";
            // 
            // labelControl7
            // 
            this.labelControl7.ForeColor = System.Drawing.Color.Red;
            this.labelControl7.Location = new System.Drawing.Point(14, 228);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(189, 24);
            this.labelControl7.TabIndex = 23;
            this.labelControl7.Text = "选择合并后存储目录";
            // 
            // btnScanJpg
            // 
            this.btnScanJpg.Location = new System.Drawing.Point(14, 317);
            this.btnScanJpg.Name = "btnScanJpg";
            this.btnScanJpg.Size = new System.Drawing.Size(133, 41);
            this.btnScanJpg.TabIndex = 24;
            this.btnScanJpg.Text = "扫描识别重命名功能";
            this.btnScanJpg.Click += new System.EventHandler(this.btnScanJpg_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(410, 170);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(148, 14);
            this.labelControl8.TabIndex = 36;
            this.labelControl8.Text = "(组级目录存放方式)";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(410, 116);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(148, 14);
            this.labelControl9.TabIndex = 35;
            this.labelControl9.Text = "(村级目录存放方式)";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(27, 193);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(402, 14);
            this.labelControl10.TabIndex = 38;
            this.labelControl10.Text = "XX县保护卡数据/乡镇1/XXX村/yyy社组/姓名(身份证号)位置示意图.jpg";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(28, 170);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(363, 14);
            this.labelControl11.TabIndex = 37;
            this.labelControl11.Tag = " ";
            this.labelControl11.Text = "XX县保护卡数据/乡镇1/XXX村/yyy社组/姓名(身份证号).jpg";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(413, 21);
            this.textBox1.TabIndex = 41;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(16, 276);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(413, 21);
            this.textBox2.TabIndex = 42;
            // 
            // frmScanUnionOneJpgUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 382);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.btnScanJpg);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.buttonEdit1);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.btnDirSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmScanUnionOneJpgUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扫描合并保护卡功能";
            this.Load += new System.EventHandler(this.frmScanUnionOneJpgUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelControl6;
        private Label labelControl5;
        private Label labelControl3;
        private Button btnDirSelect;
        private Button simpleButton2;
        private Button btnImport;
        private Label labelControl2;
        private Button buttonEdit1;
        private Label labelControl1;
        private Label labelControl4;
        private Label labelControl7;
        private Button btnScanJpg;
        private Label labelControl8;
        private Label labelControl9;
        private Label labelControl10;
        private Label labelControl11;
        private TextBox textBox1;
        private TextBox textBox2;
    }
}