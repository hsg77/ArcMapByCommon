namespace ArcMapByCommon
{
    partial class frmUpdateYsdmBatchUI
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSelectFileTxt = new System.Windows.Forms.Button();
            this.cb_IsLayerLastPreStr = new System.Windows.Forms.CheckBox();
            this.txt_LayerPreStr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CB_CheckALL
            // 
            this.CB_CheckALL.AutoSize = true;
            this.CB_CheckALL.Location = new System.Drawing.Point(13, 534);
            this.CB_CheckALL.Name = "CB_CheckALL";
            this.CB_CheckALL.Size = new System.Drawing.Size(90, 16);
            this.CB_CheckALL.TabIndex = 35;
            this.CB_CheckALL.Text = "全选/全不选";
            this.CB_CheckALL.UseVisualStyleBackColor = true;
            this.CB_CheckALL.CheckedChanged += new System.EventHandler(this.CB_CheckALL_CheckedChanged);
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(506, 509);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(79, 41);
            this.Btn_Close.TabIndex = 34;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Location = new System.Drawing.Point(374, 509);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(104, 41);
            this.BtnUpdate.TabIndex = 33;
            this.BtnUpdate.Text = "开始批量更新";
            this.BtnUpdate.UseVisualStyleBackColor = true;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(10, 27);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(578, 420);
            this.checkedListBox1.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(173, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "选择批量更新YSDM字段的图层：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(341, 12);
            this.label1.TabIndex = 36;
            this.label1.Text = "先选择YSDM对照表txt文件，格式为(YSDM,YSDMName,LayerName)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 477);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(575, 21);
            this.textBox1.TabIndex = 37;
            // 
            // btnSelectFileTxt
            // 
            this.btnSelectFileTxt.Location = new System.Drawing.Point(544, 455);
            this.btnSelectFileTxt.Name = "btnSelectFileTxt";
            this.btnSelectFileTxt.Size = new System.Drawing.Size(44, 20);
            this.btnSelectFileTxt.TabIndex = 38;
            this.btnSelectFileTxt.Text = "...";
            this.btnSelectFileTxt.UseVisualStyleBackColor = true;
            this.btnSelectFileTxt.Click += new System.EventHandler(this.btnSelectFileTxt_Click);
            // 
            // cb_IsLayerLastPreStr
            // 
            this.cb_IsLayerLastPreStr.AutoSize = true;
            this.cb_IsLayerLastPreStr.Checked = true;
            this.cb_IsLayerLastPreStr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_IsLayerLastPreStr.Location = new System.Drawing.Point(13, 512);
            this.cb_IsLayerLastPreStr.Name = "cb_IsLayerLastPreStr";
            this.cb_IsLayerLastPreStr.Size = new System.Drawing.Size(132, 16);
            this.cb_IsLayerLastPreStr.TabIndex = 40;
            this.cb_IsLayerLastPreStr.Text = "是后缀图层字符串：";
            this.cb_IsLayerLastPreStr.UseVisualStyleBackColor = true;
            // 
            // txt_LayerPreStr
            // 
            this.txt_LayerPreStr.Location = new System.Drawing.Point(144, 508);
            this.txt_LayerPreStr.Name = "txt_LayerPreStr";
            this.txt_LayerPreStr.Size = new System.Drawing.Size(189, 21);
            this.txt_LayerPreStr.TabIndex = 41;
            // 
            // frmUpdateYsdmBatchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 562);
            this.Controls.Add(this.txt_LayerPreStr);
            this.Controls.Add(this.cb_IsLayerLastPreStr);
            this.Controls.Add(this.btnSelectFileTxt);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CB_CheckALL);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.BtnUpdate);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmUpdateYsdmBatchUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量更新YSDM字段值功能";
            this.Load += new System.EventHandler(this.frmUpdateYsdmBatchUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CB_CheckALL;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button BtnUpdate;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSelectFileTxt;
        private System.Windows.Forms.CheckBox cb_IsLayerLastPreStr;
        private System.Windows.Forms.TextBox txt_LayerPreStr;
    }
}