namespace ArcMapByCommon
{
    partial class frmSelfIntersectionFeature
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelfIntersectionFeature));
            this.treeFeatures = new System.Windows.Forms.TreeView();
            this.lblFeatureCount = new System.Windows.Forms.Label();
            this.listPointCollection = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cbxRings = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRemovePoint = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // treeFeatures
            // 
            this.treeFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeFeatures.FullRowSelect = true;
            this.treeFeatures.HideSelection = false;
            this.treeFeatures.Location = new System.Drawing.Point(12, 43);
            this.treeFeatures.Name = "treeFeatures";
            this.treeFeatures.ShowRootLines = false;
            this.treeFeatures.Size = new System.Drawing.Size(186, 465);
            this.treeFeatures.TabIndex = 3;
            // 
            // lblFeatureCount
            // 
            this.lblFeatureCount.AutoSize = true;
            this.lblFeatureCount.Location = new System.Drawing.Point(12, 15);
            this.lblFeatureCount.Name = "lblFeatureCount";
            this.lblFeatureCount.Size = new System.Drawing.Size(107, 12);
            this.lblFeatureCount.TabIndex = 4;
            this.lblFeatureCount.Text = "自相交要素（0）：";
            // 
            // listPointCollection
            // 
            this.listPointCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listPointCollection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listPointCollection.FullRowSelect = true;
            this.listPointCollection.GridLines = true;
            this.listPointCollection.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listPointCollection.HideSelection = false;
            this.listPointCollection.Location = new System.Drawing.Point(204, 43);
            this.listPointCollection.MultiSelect = false;
            this.listPointCollection.Name = "listPointCollection";
            this.listPointCollection.Size = new System.Drawing.Size(337, 465);
            this.listPointCollection.TabIndex = 5;
            this.listPointCollection.UseCompatibleStateImageBehavior = false;
            this.listPointCollection.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "X";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Y";
            this.columnHeader3.Width = 120;
            // 
            // cbxRings
            // 
            this.cbxRings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxRings.FormattingEnabled = true;
            this.cbxRings.Location = new System.Drawing.Point(251, 12);
            this.cbxRings.Name = "cbxRings";
            this.cbxRings.Size = new System.Drawing.Size(228, 20);
            this.cbxRings.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "圈号：";
            // 
            // btnRemovePoint
            // 
            this.btnRemovePoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePoint.Enabled = false;
            this.btnRemovePoint.Image = ((System.Drawing.Image)(resources.GetObject("btnRemovePoint.Image")));
            this.btnRemovePoint.Location = new System.Drawing.Point(547, 43);
            this.btnRemovePoint.Name = "btnRemovePoint";
            this.btnRemovePoint.Size = new System.Drawing.Size(25, 23);
            this.btnRemovePoint.TabIndex = 8;
            this.btnRemovePoint.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(488, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "仅重复节点";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // frmSelfIntersectionFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 520);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnRemovePoint);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxRings);
            this.Controls.Add(this.lblFeatureCount);
            this.Controls.Add(this.treeFeatures);
            this.Controls.Add(this.listPointCollection);
            this.Name = "frmSelfIntersectionFeature";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自相交要素(两个开始编辑同时打开）";
            this.Load += new System.EventHandler(this.frmSelfIntersectionFeature_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeFeatures;
        private System.Windows.Forms.Label lblFeatureCount;
        private System.Windows.Forms.ListView listPointCollection;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ComboBox cbxRings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRemovePoint;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}