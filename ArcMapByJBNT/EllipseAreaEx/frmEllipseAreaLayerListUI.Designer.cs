
using System.Windows.Forms;
namespace ArcMapByJBNT
{
    partial class frmEllipseAreaLayerListUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.CB_FD = new System.Windows.Forms.CheckBox();
            this.CB_IsBigNumber = new System.Windows.Forms.CheckBox();
            this.txt_DD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Compute = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_Fields = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Btn_addEllipField = new System.Windows.Forms.Button();
            this.rb2000 = new System.Windows.Forms.RadioButton();
            this.rb_xian80 = new System.Windows.Forms.RadioButton();
            this.rb_beijing54 = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "选择要量算的图层：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(14, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(425, 20);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_beijing54);
            this.groupBox2.Controls.Add(this.rb_xian80);
            this.groupBox2.Controls.Add(this.rb2000);
            this.groupBox2.Controls.Add(this.CB_FD);
            this.groupBox2.Controls.Add(this.CB_IsBigNumber);
            this.groupBox2.Controls.Add(this.txt_DD);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(14, 316);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(425, 87);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坐标系";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.DarkBlue;
            this.button1.Location = new System.Drawing.Point(28, 409);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 25);
            this.button1.TabIndex = 9;
            this.button1.Text = "开始计算图形面积";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CB_FD
            // 
            this.CB_FD.AutoSize = true;
            this.CB_FD.Checked = true;
            this.CB_FD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_FD.Location = new System.Drawing.Point(15, 53);
            this.CB_FD.Name = "CB_FD";
            this.CB_FD.Size = new System.Drawing.Size(66, 16);
            this.CB_FD.TabIndex = 7;
            this.CB_FD.Text = "是3分度";
            this.CB_FD.UseVisualStyleBackColor = true;
            // 
            // CB_IsBigNumber
            // 
            this.CB_IsBigNumber.AutoSize = true;
            this.CB_IsBigNumber.Checked = true;
            this.CB_IsBigNumber.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_IsBigNumber.Location = new System.Drawing.Point(112, 54);
            this.CB_IsBigNumber.Name = "CB_IsBigNumber";
            this.CB_IsBigNumber.Size = new System.Drawing.Size(84, 16);
            this.CB_IsBigNumber.TabIndex = 4;
            this.CB_IsBigNumber.Text = "是大数坐标";
            this.CB_IsBigNumber.UseVisualStyleBackColor = true;
            // 
            // txt_DD
            // 
            this.txt_DD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_DD.Location = new System.Drawing.Point(251, 50);
            this.txt_DD.Name = "txt_DD";
            this.txt_DD.Size = new System.Drawing.Size(53, 21);
            this.txt_DD.TabIndex = 6;
            this.txt_DD.Text = "35";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "度数";
            // 
            // Btn_Compute
            // 
            this.Btn_Compute.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Compute.FlatAppearance.BorderSize = 0;
            this.Btn_Compute.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Compute.ForeColor = System.Drawing.Color.DarkBlue;
            this.Btn_Compute.Location = new System.Drawing.Point(305, 409);
            this.Btn_Compute.Name = "Btn_Compute";
            this.Btn_Compute.Size = new System.Drawing.Size(133, 25);
            this.Btn_Compute.TabIndex = 2;
            this.Btn_Compute.Text = "开始计算椭球面积";
            this.Btn_Compute.UseVisualStyleBackColor = true;
            this.Btn_Compute.Click += new System.EventHandler(this.Btn_Compute_Click);
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Silver;
            this.textBox3.Location = new System.Drawing.Point(13, 154);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox3.Size = new System.Drawing.Size(426, 156);
            this.textBox3.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "源文件投影坐标系统描述：";
            // 
            // CB_Fields
            // 
            this.CB_Fields.FormattingEnabled = true;
            this.CB_Fields.Location = new System.Drawing.Point(13, 96);
            this.CB_Fields.Name = "CB_Fields";
            this.CB_Fields.Size = new System.Drawing.Size(426, 20);
            this.CB_Fields.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 22;
            this.label4.Text = "请选择要更新的字段：";
            // 
            // Btn_addEllipField
            // 
            this.Btn_addEllipField.BackColor = System.Drawing.Color.Transparent;
            this.Btn_addEllipField.FlatAppearance.BorderSize = 0;
            this.Btn_addEllipField.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_addEllipField.ForeColor = System.Drawing.Color.DarkBlue;
            this.Btn_addEllipField.Location = new System.Drawing.Point(256, 122);
            this.Btn_addEllipField.Name = "Btn_addEllipField";
            this.Btn_addEllipField.Size = new System.Drawing.Size(182, 25);
            this.Btn_addEllipField.TabIndex = 24;
            this.Btn_addEllipField.Text = "添加椭球面积字段(TQMJ)";
            this.Btn_addEllipField.UseVisualStyleBackColor = true;
            this.Btn_addEllipField.Click += new System.EventHandler(this.Btn_addEllipField_Click);
            // 
            // rb2000
            // 
            this.rb2000.AutoSize = true;
            this.rb2000.Checked = true;
            this.rb2000.Location = new System.Drawing.Point(14, 19);
            this.rb2000.Name = "rb2000";
            this.rb2000.Size = new System.Drawing.Size(83, 16);
            this.rb2000.TabIndex = 10;
            this.rb2000.TabStop = true;
            this.rb2000.Text = "2000坐标系";
            this.rb2000.UseVisualStyleBackColor = true;
            // 
            // rb_xian80
            // 
            this.rb_xian80.AutoSize = true;
            this.rb_xian80.Location = new System.Drawing.Point(120, 19);
            this.rb_xian80.Name = "rb_xian80";
            this.rb_xian80.Size = new System.Drawing.Size(95, 16);
            this.rb_xian80.TabIndex = 11;
            this.rb_xian80.Text = "西安80坐标系";
            this.rb_xian80.UseVisualStyleBackColor = true;
            // 
            // rb_beijing54
            // 
            this.rb_beijing54.AutoSize = true;
            this.rb_beijing54.Location = new System.Drawing.Point(251, 19);
            this.rb_beijing54.Name = "rb_beijing54";
            this.rb_beijing54.Size = new System.Drawing.Size(95, 16);
            this.rb_beijing54.TabIndex = 25;
            this.rb_beijing54.Text = "北京54坐标系";
            this.rb_beijing54.UseVisualStyleBackColor = true;
            // 
            // frmEllipseAreaLayerListUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(453, 449);
            this.Controls.Add(this.Btn_addEllipField);
            this.Controls.Add(this.CB_Fields);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Btn_Compute);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmEllipseAreaLayerListUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "椭球面积计算";
            this.Load += new System.EventHandler(this.frmEllipseAreaLayerListUI_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private CheckBox CB_FD;
        private CheckBox CB_IsBigNumber;
        private TextBox txt_DD;
        private System.Windows.Forms.Label label2;
        private Button Btn_Compute;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CB_Fields;
        private System.Windows.Forms.Label label4;
        private Button Btn_addEllipField;
        private Button button1;
        private RadioButton rb_xian80;
        private RadioButton rb2000;
        private RadioButton rb_beijing54;
    }
}