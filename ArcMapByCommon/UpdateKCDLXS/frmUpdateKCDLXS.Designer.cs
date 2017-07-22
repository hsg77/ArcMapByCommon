namespace ArcMapByCommon
{
    partial class frmUpdateKCDLXS
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
            this.button2 = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_fd4_100 = new System.Windows.Forms.RadioButton();
            this.btnAddCustomField = new System.Windows.Forms.Button();
            this.rb_fd4 = new System.Windows.Forms.RadioButton();
            this.rb_fd2 = new System.Windows.Forms.RadioButton();
            this.cb_IsAwayFromZero = new System.Windows.Forms.CheckBox();
            this.rb_fd2_100 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(310, 234);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 34);
            this.button2.TabIndex = 11;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(190, 234);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(92, 34);
            this.btnUpdate.TabIndex = 10;
            this.btnUpdate.Text = "开始更新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(74, 60);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(328, 20);
            this.comboBox2.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "字段：";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(74, 16);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(328, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "图层：";
            // 
            // rb_fd4_100
            // 
            this.rb_fd4_100.AutoSize = true;
            this.rb_fd4_100.Location = new System.Drawing.Point(73, 97);
            this.rb_fd4_100.Name = "rb_fd4_100";
            this.rb_fd4_100.Size = new System.Drawing.Size(287, 16);
            this.rb_fd4_100.TabIndex = 14;
            this.rb_fd4_100.Text = "除100后保留4位小数(针对【保护图斑】BHTB省标)";
            this.rb_fd4_100.UseVisualStyleBackColor = true;
            // 
            // btnAddCustomField
            // 
            this.btnAddCustomField.Location = new System.Drawing.Point(21, 234);
            this.btnAddCustomField.Name = "btnAddCustomField";
            this.btnAddCustomField.Size = new System.Drawing.Size(132, 38);
            this.btnAddCustomField.TabIndex = 15;
            this.btnAddCustomField.Text = "添加自定义的字段(KCDLXS_XP和TKXS)";
            this.btnAddCustomField.UseVisualStyleBackColor = true;
            this.btnAddCustomField.Click += new System.EventHandler(this.btnAddCustomField_Click);
            // 
            // rb_fd4
            // 
            this.rb_fd4.AutoSize = true;
            this.rb_fd4.Location = new System.Drawing.Point(73, 142);
            this.rb_fd4.Name = "rb_fd4";
            this.rb_fd4.Size = new System.Drawing.Size(89, 16);
            this.rb_fd4.TabIndex = 16;
            this.rb_fd4.Text = "保留4位小数";
            this.rb_fd4.UseVisualStyleBackColor = true;
            // 
            // rb_fd2
            // 
            this.rb_fd2.AutoSize = true;
            this.rb_fd2.Checked = true;
            this.rb_fd2.Location = new System.Drawing.Point(73, 165);
            this.rb_fd2.Name = "rb_fd2";
            this.rb_fd2.Size = new System.Drawing.Size(89, 16);
            this.rb_fd2.TabIndex = 17;
            this.rb_fd2.TabStop = true;
            this.rb_fd2.Text = "保留2位小数";
            this.rb_fd2.UseVisualStyleBackColor = true;
            // 
            // cb_IsAwayFromZero
            // 
            this.cb_IsAwayFromZero.AutoSize = true;
            this.cb_IsAwayFromZero.Location = new System.Drawing.Point(74, 197);
            this.cb_IsAwayFromZero.Name = "cb_IsAwayFromZero";
            this.cb_IsAwayFromZero.Size = new System.Drawing.Size(276, 16);
            this.cb_IsAwayFromZero.TabIndex = 18;
            this.cb_IsAwayFromZero.Text = "四舍五入(AwayFromZero)(舍入绝对值中较大值)";
            this.cb_IsAwayFromZero.UseVisualStyleBackColor = true;
            // 
            // rb_fd2_100
            // 
            this.rb_fd2_100.AutoSize = true;
            this.rb_fd2_100.Location = new System.Drawing.Point(72, 120);
            this.rb_fd2_100.Name = "rb_fd2_100";
            this.rb_fd2_100.Size = new System.Drawing.Size(287, 16);
            this.rb_fd2_100.TabIndex = 19;
            this.rb_fd2_100.Text = "除100后保留2位小数(针对【保护图斑】BHTB国标)";
            this.rb_fd2_100.UseVisualStyleBackColor = true;
            // 
            // frmUpdateKCDLXS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 301);
            this.Controls.Add(this.rb_fd2_100);
            this.Controls.Add(this.cb_IsAwayFromZero);
            this.Controls.Add(this.rb_fd2);
            this.Controls.Add(this.rb_fd4);
            this.Controls.Add(this.btnAddCustomField);
            this.Controls.Add(this.rb_fd4_100);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUpdateKCDLXS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算扣除地类系数值（或面积值）";
            this.Load += new System.EventHandler(this.frmUpdateKCDLXS_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rb_fd4_100;
        private System.Windows.Forms.Button btnAddCustomField;
        private System.Windows.Forms.RadioButton rb_fd4;
        private System.Windows.Forms.RadioButton rb_fd2;
        private System.Windows.Forms.CheckBox cb_IsAwayFromZero;
        private System.Windows.Forms.RadioButton rb_fd2_100;
    }
}