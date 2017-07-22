namespace ArcMapByJBNT
{
    partial class frmOutMuliShapeBySelectedGeometryUI
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
            this.Btn_SaveAsShape = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.CB_CheckALL = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_OutPutShape = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // Btn_SaveAsShape
            // 
            this.Btn_SaveAsShape.Location = new System.Drawing.Point(433, 364);
            this.Btn_SaveAsShape.Name = "Btn_SaveAsShape";
            this.Btn_SaveAsShape.Size = new System.Drawing.Size(24, 18);
            this.Btn_SaveAsShape.TabIndex = 28;
            this.Btn_SaveAsShape.Text = "…";
            this.Btn_SaveAsShape.UseVisualStyleBackColor = true;
            this.Btn_SaveAsShape.Click += new System.EventHandler(this.Btn_SaveAsShape_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 362);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(415, 21);
            this.textBox1.TabIndex = 27;
            // 
            // CB_CheckALL
            // 
            this.CB_CheckALL.AutoSize = true;
            this.CB_CheckALL.Location = new System.Drawing.Point(14, 308);
            this.CB_CheckALL.Name = "CB_CheckALL";
            this.CB_CheckALL.Size = new System.Drawing.Size(90, 16);
            this.CB_CheckALL.TabIndex = 26;
            this.CB_CheckALL.Text = "全选/全不选";
            this.CB_CheckALL.UseVisualStyleBackColor = true;
            this.CB_CheckALL.CheckedChanged += new System.EventHandler(this.CB_CheckALL_CheckedChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(14, 90);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(452, 212);
            this.checkedListBox1.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "选择要输出的图层：";
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(374, 409);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(67, 30);
            this.Btn_Close.TabIndex = 23;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_OutPutShape
            // 
            this.Btn_OutPutShape.Location = new System.Drawing.Point(279, 409);
            this.Btn_OutPutShape.Name = "Btn_OutPutShape";
            this.Btn_OutPutShape.Size = new System.Drawing.Size(69, 30);
            this.Btn_OutPutShape.TabIndex = 22;
            this.Btn_OutPutShape.Text = "开始输出";
            this.Btn_OutPutShape.UseVisualStyleBackColor = true;
            this.Btn_OutPutShape.Click += new System.EventHandler(this.Btn_OutPutShape_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 343);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "另存到目录内为：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 29;
            this.label1.Text = "选中范围图层：（如行政区图层）";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(14, 33);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(452, 20);
            this.comboBox1.TabIndex = 30;
            // 
            // frmOutMuliShapeBySelectedGeometryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 452);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Btn_SaveAsShape);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.CB_CheckALL);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_OutPutShape);
            this.Controls.Add(this.label2);
            this.Name = "frmOutMuliShapeBySelectedGeometryUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选中范围批量导出shp文件功能";
            this.Load += new System.EventHandler(this.frmOutMuliShapeBySelectedGeometryUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_SaveAsShape;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox CB_CheckALL;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button Btn_OutPutShape;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}