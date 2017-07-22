namespace ArcMapByJBNT
{
    partial class frmProgressBar2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProgressBar2));
            this.Caption1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Caption2 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // Caption1
            // 
            this.Caption1.AutoSize = true;
            this.Caption1.Location = new System.Drawing.Point(3, 8);
            this.Caption1.Name = "Caption1";
            this.Caption1.Size = new System.Drawing.Size(53, 12);
            this.Caption1.TabIndex = 0;
            this.Caption1.Text = "Caption1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(2, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(413, 13);
            this.progressBar1.TabIndex = 1;
            // 
            // Caption2
            // 
            this.Caption2.AutoSize = true;
            this.Caption2.Location = new System.Drawing.Point(2, 56);
            this.Caption2.Name = "Caption2";
            this.Caption2.Size = new System.Drawing.Size(53, 12);
            this.Caption2.TabIndex = 2;
            this.Caption2.Text = "Caption2";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(4, 77);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(410, 14);
            this.progressBar2.TabIndex = 3;
            // 
            // frmProgressBar2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 99);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.Caption2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Caption1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProgressBar2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProgressBar2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label Caption1;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label Caption2;
        public System.Windows.Forms.ProgressBar progressBar2;
    }
}