namespace ArcMapByCommon
{
    partial class frmUpdateFeatureByGeoSpatialOverlapUI
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
            this.CB_LayerList1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CB_LayerList2 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.CB_Fields1 = new System.Windows.Forms.ComboBox();
            this.CB_Fields2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.fd_IsQuick = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // CB_LayerList1
            // 
            this.CB_LayerList1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LayerList1.FormattingEnabled = true;
            this.CB_LayerList1.Location = new System.Drawing.Point(33, 58);
            this.CB_LayerList1.Name = "CB_LayerList1";
            this.CB_LayerList1.Size = new System.Drawing.Size(360, 20);
            this.CB_LayerList1.TabIndex = 19;
            this.CB_LayerList1.SelectedIndexChanged += new System.EventHandler(this.CB_LayerList1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "选择要更新的目标图层1：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "选择要与之叠加的源图层2：";
            // 
            // CB_LayerList2
            // 
            this.CB_LayerList2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_LayerList2.FormattingEnabled = true;
            this.CB_LayerList2.Location = new System.Drawing.Point(33, 191);
            this.CB_LayerList2.Name = "CB_LayerList2";
            this.CB_LayerList2.Size = new System.Drawing.Size(360, 20);
            this.CB_LayerList2.TabIndex = 22;
            this.CB_LayerList2.SelectedIndexChanged += new System.EventHandler(this.CB_LayerList2_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "选择要更新的目标字段1：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(278, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 39);
            this.button1.TabIndex = 27;
            this.button1.Text = "开始更新处理";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // CB_Fields1
            // 
            this.CB_Fields1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Fields1.FormattingEnabled = true;
            this.CB_Fields1.Location = new System.Drawing.Point(33, 108);
            this.CB_Fields1.Name = "CB_Fields1";
            this.CB_Fields1.Size = new System.Drawing.Size(360, 20);
            this.CB_Fields1.TabIndex = 30;
            // 
            // CB_Fields2
            // 
            this.CB_Fields2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Fields2.FormattingEnabled = true;
            this.CB_Fields2.Location = new System.Drawing.Point(33, 249);
            this.CB_Fields2.Name = "CB_Fields2";
            this.CB_Fields2.Size = new System.Drawing.Size(360, 20);
            this.CB_Fields2.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "选择更新的值来源的字段2：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 37;
            this.label8.Text = "要写入的图层：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 145);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 38;
            this.label9.Text = "数据来源图层：";
            // 
            // fd_IsQuick
            // 
            this.fd_IsQuick.AutoSize = true;
            this.fd_IsQuick.Checked = true;
            this.fd_IsQuick.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fd_IsQuick.Location = new System.Drawing.Point(214, 333);
            this.fd_IsQuick.Name = "fd_IsQuick";
            this.fd_IsQuick.Size = new System.Drawing.Size(48, 16);
            this.fd_IsQuick.TabIndex = 39;
            this.fd_IsQuick.Text = "快速";
            this.fd_IsQuick.UseVisualStyleBackColor = true;
            // 
            // frmUpdateFeatureByGeoSpatialOverlapUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 372);
            this.Controls.Add(this.fd_IsQuick);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CB_Fields2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CB_Fields1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CB_LayerList1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CB_LayerList2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUpdateFeatureByGeoSpatialOverlapUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "空间叠加传值功能";
            this.Load += new System.EventHandler(this.frmUpdateFeatureByGeoSpatialOverlapUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_LayerList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CB_LayerList2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox CB_Fields1;
        private System.Windows.Forms.ComboBox CB_Fields2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox fd_IsQuick;
    }
}