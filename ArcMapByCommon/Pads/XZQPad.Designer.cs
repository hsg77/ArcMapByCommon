namespace ArcMapByCommon.Pads
{
    partial class XZQPad
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPlaceholder = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlaceholder
            // 
            this.labelPlaceholder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPlaceholder.Location = new System.Drawing.Point(0, 0);
            this.labelPlaceholder.Name = "labelPlaceholder";
            this.labelPlaceholder.Size = new System.Drawing.Size(340, 314);
            this.labelPlaceholder.TabIndex = 0;
            this.labelPlaceholder.Text = "Place controls on the canvas for your dockable window definition";
            this.labelPlaceholder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelPlaceholder.Click += new System.EventHandler(this.labelPlaceholder_Click);
            // 
            // XZQPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelPlaceholder);
            this.Name = "XZQPad";
            this.Size = new System.Drawing.Size(340, 314);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPlaceholder;
    }
}
