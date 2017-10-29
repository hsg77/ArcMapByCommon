namespace ArcMapByCommon
{
    partial class frmScanPhone2DValue
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
            this.fd_phoneValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fd_mmValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fd_phoneValue
            // 
            this.fd_phoneValue.Location = new System.Drawing.Point(21, 25);
            this.fd_phoneValue.Multiline = true;
            this.fd_phoneValue.Name = "fd_phoneValue";
            this.fd_phoneValue.Size = new System.Drawing.Size(610, 188);
            this.fd_phoneValue.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(485, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "录入手机扫描后值:格式如：yzh:01bhk511025001005,习小小,8F636682FCF34......AC3D24C";
            // 
            // fd_mmValue
            // 
            this.fd_mmValue.Location = new System.Drawing.Point(21, 241);
            this.fd_mmValue.Multiline = true;
            this.fd_mmValue.Name = "fd_mmValue";
            this.fd_mmValue.Size = new System.Drawing.Size(610, 188);
            this.fd_mmValue.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 219);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "读取的明码值:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 438);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 34);
            this.button1.TabIndex = 4;
            this.button1.Text = "开始读取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmScanPhone2DValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 486);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fd_mmValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fd_phoneValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmScanPhone2DValue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "读取手机扫描值";
            this.Load += new System.EventHandler(this.frmScanPhone2DValue_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fd_phoneValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox fd_mmValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}