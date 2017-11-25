namespace ArcMapByCommon
{
    partial class frmUpdateBsmBatchUI
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
            this.CB_CheckALL = new System.Windows.Forms.CheckBox();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.BtnUpdate = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CB_CheckALL
            // 
            this.CB_CheckALL.AutoSize = true;
            this.CB_CheckALL.Location = new System.Drawing.Point(17, 482);
            this.CB_CheckALL.Name = "CB_CheckALL";
            this.CB_CheckALL.Size = new System.Drawing.Size(90, 16);
            this.CB_CheckALL.TabIndex = 30;
            this.CB_CheckALL.Text = "全选/全不选";
            this.CB_CheckALL.UseVisualStyleBackColor = true;
            this.CB_CheckALL.CheckedChanged += new System.EventHandler(this.CB_CheckALL_CheckedChanged);
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(415, 469);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(79, 41);
            this.Btn_Close.TabIndex = 29;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(283, 469);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(104, 41);
            this.BtnUpdate.TabIndex = 28;
            this.BtnUpdate.Text = "开始批量更新";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(17, 34);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(477, 420);
            this.checkedListBox1.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "选择批量更新BSM字段的图层：";
            // 
            // frmUpdateBsmBatchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 522);
            this.Controls.Add(this.CB_CheckALL);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUpdateBsmBatchUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量更新BSM标识码字段值功能";
            this.Load += new System.EventHandler(this.frmUpdateBsmBatchUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CB_CheckALL;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
    }
}