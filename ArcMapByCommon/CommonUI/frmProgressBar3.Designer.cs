namespace ArcMapByCommon
{
    partial class frmProgressBar3
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgressBar3));
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.Caption2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Caption1 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.Caption3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(6, 78);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(410, 14);
            this.progressBar2.TabIndex = 7;
            // 
            // Caption2
            // 
            this.Caption2.AutoSize = true;
            this.Caption2.Location = new System.Drawing.Point(4, 57);
            this.Caption2.Name = "Caption2";
            this.Caption2.Size = new System.Drawing.Size(53, 12);
            this.Caption2.TabIndex = 6;
            this.Caption2.Text = "Caption2";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 30);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(413, 13);
            this.progressBar1.TabIndex = 5;
            // 
            // Caption1
            // 
            this.Caption1.AutoSize = true;
            this.Caption1.Location = new System.Drawing.Point(5, 9);
            this.Caption1.Name = "Caption1";
            this.Caption1.Size = new System.Drawing.Size(53, 12);
            this.Caption1.TabIndex = 4;
            this.Caption1.Text = "Caption1";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(6, 122);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(410, 14);
            this.progressBar3.TabIndex = 9;
            // 
            // Caption3
            // 
            this.Caption3.AutoSize = true;
            this.Caption3.Location = new System.Drawing.Point(4, 101);
            this.Caption3.Name = "Caption3";
            this.Caption3.Size = new System.Drawing.Size(53, 12);
            this.Caption3.TabIndex = 8;
            this.Caption3.Text = "Caption3";
            // 
            // frmProgressBar3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 147);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.Caption3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.Caption2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Caption1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProgressBar3";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProgressBar3";
            this.Load += new System.EventHandler(this.frmProgressBar3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ProgressBar progressBar2;
        public System.Windows.Forms.Label Caption2;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label Caption1;
        public System.Windows.Forms.ProgressBar progressBar3;
        public System.Windows.Forms.Label Caption3;
    }
}