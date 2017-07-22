namespace ArcMapByCommon
{
    partial class frmOutShapeUI
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
            this.label2 = new System.Windows.Forms.Label();
            this.Btn_Close = new System.Windows.Forms.Button();
            this.Btn_OutPutShape = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.IsSelected = new System.Windows.Forms.CheckBox();
            this.cb_IsObjGeo = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Btn_SaveAsShape
            // 
            this.Btn_SaveAsShape.Location = new System.Drawing.Point(360, 88);
            this.Btn_SaveAsShape.Name = "Btn_SaveAsShape";
            this.Btn_SaveAsShape.Size = new System.Drawing.Size(24, 18);
            this.Btn_SaveAsShape.TabIndex = 13;
            this.Btn_SaveAsShape.Text = "…";
            this.Btn_SaveAsShape.UseVisualStyleBackColor = true;
            this.Btn_SaveAsShape.Click += new System.EventHandler(this.Btn_SaveAsShape_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 86);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(347, 21);
            this.textBox1.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "另存为Shape文件：";
            // 
            // Btn_Close
            // 
            this.Btn_Close.Location = new System.Drawing.Point(293, 119);
            this.Btn_Close.Name = "Btn_Close";
            this.Btn_Close.Size = new System.Drawing.Size(67, 30);
            this.Btn_Close.TabIndex = 10;
            this.Btn_Close.Text = "关闭";
            this.Btn_Close.UseVisualStyleBackColor = true;
            this.Btn_Close.Click += new System.EventHandler(this.Btn_Close_Click);
            // 
            // Btn_OutPutShape
            // 
            this.Btn_OutPutShape.Location = new System.Drawing.Point(198, 119);
            this.Btn_OutPutShape.Name = "Btn_OutPutShape";
            this.Btn_OutPutShape.Size = new System.Drawing.Size(69, 30);
            this.Btn_OutPutShape.TabIndex = 9;
            this.Btn_OutPutShape.Text = "开始输出";
            this.Btn_OutPutShape.UseVisualStyleBackColor = true;
            this.Btn_OutPutShape.Click += new System.EventHandler(this.Btn_OutPutShape_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "选择要输出的图层：";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 34);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(348, 20);
            this.comboBox1.TabIndex = 7;
            // 
            // IsSelected
            // 
            this.IsSelected.AutoSize = true;
            this.IsSelected.Location = new System.Drawing.Point(15, 126);
            this.IsSelected.Name = "IsSelected";
            this.IsSelected.Size = new System.Drawing.Size(96, 16);
            this.IsSelected.TabIndex = 14;
            this.IsSelected.Text = "对已选择要素";
            this.IsSelected.UseVisualStyleBackColor = true;
            // 
            // cb_IsObjGeo
            // 
            this.cb_IsObjGeo.AutoSize = true;
            this.cb_IsObjGeo.Location = new System.Drawing.Point(15, 162);
            this.cb_IsObjGeo.Name = "cb_IsObjGeo";
            this.cb_IsObjGeo.Size = new System.Drawing.Size(258, 16);
            this.cb_IsObjGeo.TabIndex = 26;
            this.cb_IsObjGeo.Text = "是否强制输出OID=OBJECTID,SHAPE=GEOMETRY";
            this.cb_IsObjGeo.UseVisualStyleBackColor = true;
            this.cb_IsObjGeo.Visible = false;
            // 
            // frmOutShapeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 202);
            this.Controls.Add(this.cb_IsObjGeo);
            this.Controls.Add(this.IsSelected);
            this.Controls.Add(this.Btn_SaveAsShape);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Btn_Close);
            this.Controls.Add(this.Btn_OutPutShape);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmOutShapeUI";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输出单个Shape文件功能";
            this.Load += new System.EventHandler(this.frmOutShapeUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Btn_SaveAsShape;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Btn_Close;
        private System.Windows.Forms.Button Btn_OutPutShape;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox IsSelected;
        private System.Windows.Forms.CheckBox cb_IsObjGeo;
    }
}