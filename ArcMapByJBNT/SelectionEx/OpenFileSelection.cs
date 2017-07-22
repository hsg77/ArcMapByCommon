using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 打开文件选择 管理类
    /// </summary>
    public class OpenFileSelection
    {
        public OpenFileDialog m_dialog = null;

        public OpenFileSelection(string fileter,bool IsMultSelect)
        {
            this.m_dialog = new OpenFileDialog();
            this.m_dialog.Filter = fileter;
            this.m_dialog.Multiselect = IsMultSelect;

            InitializeUI();
        }

        private void InitializeUI()
        {
            //System.IO.Directory
        }

        public bool IsFileSelected()
        {
            return m_dialog.ShowDialog() == DialogResult.OK;
        }

        public string GetFileName
        {
            get
            {
                return this.m_dialog.FileName;
            }
        }

        public string[] GetFileNames
        {
            get
            {
                return this.m_dialog.FileNames;
            }
        }
    }
}
