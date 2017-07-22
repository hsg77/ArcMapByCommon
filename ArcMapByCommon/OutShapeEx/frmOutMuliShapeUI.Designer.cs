namespace ArcMapByCommon
{
    partial class frmOutMuliShapeUI
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
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_OutPutShape = new System.Windows.Forms.Button();
            this.CB_CheckALL = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Btn_SaveAsShape = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 276);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "另存到目录内为：";
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(372, 342);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(67, 30);
            this.Btn_Close.TabIndex = 14;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_OutPutShape
            // 
            this.Btn_OutPutShape.Location = new System.Drawing.Point(277, 342);
            this.Btn_OutPutShape.Name = "Btn_OutPutShape";
            this.Btn_OutPutShape.Size = new System.Drawing.Size(69, 30);
            this.Btn_OutPutShape.TabIndex = 13;
            this.Btn_OutPutShape.Text = "开始输出";
            this.Btn_OutPutShape.UseVisualStyleBackColor = true;
            this.Btn_OutPutShape.Click += new System.EventHandler(this.Btn_OutPutShape_Click);
            // 
            // CB_CheckALL
            // 
            this.CB_CheckALL.AutoSize = true;
            this.CB_CheckALL.Location = new System.Drawing.Point(12, 241);
            this.CB_CheckALL.Name = "CB_CheckALL";
            this.CB_CheckALL.Size = new System.Drawing.Size(90, 16);
            this.CB_CheckALL.TabIndex = 18;
            this.CB_CheckALL.Text = "全选/全不选";
            this.CB_CheckALL.UseVisualStyleBackColor = true;
            this.CB_CheckALL.CheckedChanged += new System.EventHandler(this.CB_CheckALL_CheckedChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 23);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(452, 212);
            this.checkedListBox1.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "选择要输出的图层：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 295);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(415, 21);
            this.textBox1.TabIndex = 19;
            // 
            // Btn_SaveAsShape
            // 
            this.Btn_SaveAsShape.Location = new System.Drawing.Point(431, 297);
            this.Btn_SaveAsShape.Name = "Btn_SaveAsShape";
            this.Btn_SaveAsShape.Size = new System.Drawing.Size(24, 18);
            this.Btn_SaveAsShape.TabIndex = 20;
            this.Btn_SaveAsShape.Text = "…";
            this.Btn_SaveAsShape.UseVisualStyleBackColor = true;
            this.Btn_SaveAsShape.Click += new System.EventHandler(this.Btn_SaveAsShape_Click);
            // 
            // frmOutMuliShapeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 384);
            this.Controls.Add(this.Btn_SaveAsShape);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.CB_CheckALL);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_OutPutShape);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmOutMuliShapeUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出多个Shape文件功能";
            this.Load += new System.EventHandler(this.frmOutMuliShapeUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button Btn_OutPutShape;
        private System.Windows.Forms.CheckBox CB_CheckALL;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button Btn_SaveAsShape;
    }
}