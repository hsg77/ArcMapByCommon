namespace ArcMapByCommon
{
    partial class frmMapMoveUI
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
            this.numericBox2 = new NumericBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericBox1 = new NumericBoxEx();
            this.CB_CheckALL = new System.Windows.Forms.CheckBox();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_MapMove = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox2.Cursor = System.Windows.Forms.Cursors.Default;
            this.numericBox2.Location = new System.Drawing.Point(81, 386);
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.NegativeNumber = true;
            this.numericBox2.NegativeNumberColor = System.Drawing.Color.Red;
            this.numericBox2.Size = new System.Drawing.Size(150, 21);
            this.numericBox2.TabIndex = 22;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 359);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "X平移量：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 389);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Y平移量：";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.SystemColors.Window;
            this.numericBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.numericBox1.Location = new System.Drawing.Point(81, 356);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NegativeNumber = true;
            this.numericBox1.NegativeNumberColor = System.Drawing.Color.Red;
            this.numericBox1.Size = new System.Drawing.Size(150, 21);
            this.numericBox1.TabIndex = 23;
            this.numericBox1.Load += new System.EventHandler(this.numericBox1_Load);
            // 
            // CB_CheckALL
            // 
            this.CB_CheckALL.AutoSize = true;
            this.CB_CheckALL.Location = new System.Drawing.Point(21, 320);
            this.CB_CheckALL.Name = "CB_CheckALL";
            this.CB_CheckALL.Size = new System.Drawing.Size(90, 16);
            this.CB_CheckALL.TabIndex = 21;
            this.CB_CheckALL.Text = "全选/全不选";
            this.CB_CheckALL.UseVisualStyleBackColor = true;
            this.CB_CheckALL.CheckedChanged += new System.EventHandler(this.CB_CheckALL_CheckedChanged);
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(420, 366);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(79, 41);
            this.Btn_Close.TabIndex = 20;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_MapMove
            // 
            this.Btn_MapMove.Location = new System.Drawing.Point(313, 366);
            this.Btn_MapMove.Name = "Btn_MapMove";
            this.Btn_MapMove.Size = new System.Drawing.Size(79, 41);
            this.Btn_MapMove.TabIndex = 19;
            this.Btn_MapMove.Text = "开始平移";
            this.Btn_MapMove.UseVisualStyleBackColor = true;
            this.Btn_MapMove.Click += new System.EventHandler(this.Btn_MapMove_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(22, 38);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(477, 276);
            this.checkedListBox1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "选择平移的图层：";
            // 
            // frmMapMoveUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 426);
            this.Controls.Add(this.CB_CheckALL);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_MapMove);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMapMoveUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "地图平移功能";
            this.Load += new System.EventHandler(this.frmMapMoveUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericBoxEx numericBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private NumericBoxEx numericBox1;
        private System.Windows.Forms.CheckBox CB_CheckALL;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button Btn_MapMove;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label3;
    }
}