namespace ArcMapByCommon
{
    partial class frmEllipseAreaComputeUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Btn_Compute = new System.Windows.Forms.Button();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.CB_IsBigNumber = new System.Windows.Forms.CheckBox();
            this.CB_Datum = new System.Windows.Forms.CheckBox();
            this.txt_DD = new System.Windows.Forms.TextBox();
            this.CB_FD = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_Fields = new System.Windows.Forms.ComboBox();
            this.Btn_addEllipField = new System.Windows.Forms.Button();
            this.Btn_JWDComputeElliArea = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 236);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选择数据源投影文件(shp格式)";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Silver;
            this.textBox3.Location = new System.Drawing.Point(7, 74);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(413, 156);
            this.textBox3.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "源文件投影坐标系统描述：";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(393, 29);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(29, 21);
            this.button3.TabIndex = 1;
            this.button3.Text = "…";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(384, 21);
            this.textBox1.TabIndex = 0;
            // 
            // Btn_Compute
            // 
            this.Btn_Compute.Location = new System.Drawing.Point(229, 33);
            this.Btn_Compute.Name = "Btn_Compute";
            this.Btn_Compute.Size = new System.Drawing.Size(74, 34);
            this.Btn_Compute.TabIndex = 2;
            this.Btn_Compute.Text = "开始计算";
            this.Btn_Compute.UseVisualStyleBackColor = true;
            this.Btn_Compute.Click += new System.EventHandler(this.Btn_Compute_Click);
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(358, 274);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(74, 26);
            this.Btn_Close.TabIndex = 3;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // CB_IsBigNumber
            // 
            this.CB_IsBigNumber.AutoSize = true;
            this.CB_IsBigNumber.Checked = true;
            this.CB_IsBigNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_IsBigNumber.Location = new System.Drawing.Point(10, 55);
            this.CB_IsBigNumber.Name = "CB_IsBigNumber";
            this.CB_IsBigNumber.Size = new System.Drawing.Size(84, 16);
            this.CB_IsBigNumber.TabIndex = 4;
            this.CB_IsBigNumber.Text = "是大数坐标";
            this.CB_IsBigNumber.UseVisualStyleBackColor = true;
            // 
            // CB_Datum
            // 
            this.CB_Datum.AutoSize = true;
            this.CB_Datum.Checked = true;
            this.CB_Datum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_Datum.Location = new System.Drawing.Point(10, 33);
            this.CB_Datum.Name = "CB_Datum";
            this.CB_Datum.Size = new System.Drawing.Size(96, 16);
            this.CB_Datum.TabIndex = 5;
            this.CB_Datum.Text = "是西安坐标系";
            this.CB_Datum.UseVisualStyleBackColor = true;
            // 
            // txt_DD
            // 
            this.txt_DD.Location = new System.Drawing.Point(139, 55);
            this.txt_DD.Name = "txt_DD";
            this.txt_DD.Size = new System.Drawing.Size(53, 21);
            this.txt_DD.TabIndex = 6;
            this.txt_DD.Text = "35";
            // 
            // CB_FD
            // 
            this.CB_FD.AutoSize = true;
            this.CB_FD.Checked = true;
            this.CB_FD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_FD.Location = new System.Drawing.Point(112, 33);
            this.CB_FD.Name = "CB_FD";
            this.CB_FD.Size = new System.Drawing.Size(66, 16);
            this.CB_FD.TabIndex = 7;
            this.CB_FD.Text = "是3分度";
            this.CB_FD.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "度数";
            // 
            // CB_Fields
            // 
            this.CB_Fields.FormattingEnabled = true;
            this.CB_Fields.Location = new System.Drawing.Point(135, 259);
            this.CB_Fields.Name = "CB_Fields";
            this.CB_Fields.Size = new System.Drawing.Size(196, 20);
            this.CB_Fields.TabIndex = 9;
            // 
            // Btn_addEllipField
            // 
            this.Btn_addEllipField.Location = new System.Drawing.Point(13, 259);
            this.Btn_addEllipField.Name = "Btn_addEllipField";
            this.Btn_addEllipField.Size = new System.Drawing.Size(116, 22);
            this.Btn_addEllipField.TabIndex = 10;
            this.Btn_addEllipField.Text = "添加椭球面积字段";
            this.Btn_addEllipField.UseVisualStyleBackColor = true;
            this.Btn_addEllipField.Click += new System.EventHandler(this.Btn_addEllipField_Click);
            // 
            // Btn_JWDComputeElliArea
            // 
            this.Btn_JWDComputeElliArea.Location = new System.Drawing.Point(136, 35);
            this.Btn_JWDComputeElliArea.Name = "Btn_JWDComputeElliArea";
            this.Btn_JWDComputeElliArea.Size = new System.Drawing.Size(155, 23);
            this.Btn_JWDComputeElliArea.TabIndex = 11;
            this.Btn_JWDComputeElliArea.Text = "开始经纬度计算椭球面积";
            this.Btn_JWDComputeElliArea.UseVisualStyleBackColor = true;
            this.Btn_JWDComputeElliArea.Click += new System.EventHandler(this.Btn_JWDComputeElliArea_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CB_FD);
            this.groupBox2.Controls.Add(this.CB_IsBigNumber);
            this.groupBox2.Controls.Add(this.CB_Datum);
            this.groupBox2.Controls.Add(this.txt_DD);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.Btn_Compute);
            this.groupBox2.Location = new System.Drawing.Point(12, 290);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 104);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "北京54/西安80坐标";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.Btn_JWDComputeElliArea);
            this.groupBox3.Location = new System.Drawing.Point(13, 400);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(318, 86);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "大地坐标系(经纬度)";
            // 
            // frmEllipseAreaComputeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 494);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Btn_addEllipField);
            this.Controls.Add(this.CB_Fields);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmEllipseAreaComputeUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "椭球面积计算工具";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Btn_Compute;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.CheckBox CB_IsBigNumber;
        private System.Windows.Forms.CheckBox CB_Datum;
        private System.Windows.Forms.TextBox txt_DD;
        private System.Windows.Forms.CheckBox CB_FD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CB_Fields;
        private System.Windows.Forms.Button Btn_addEllipField;
        private System.Windows.Forms.Button Btn_JWDComputeElliArea;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
    }
}