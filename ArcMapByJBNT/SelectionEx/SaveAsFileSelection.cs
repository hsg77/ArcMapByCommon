using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 另存为文件选择 管理类
    /// </summary>
    public class SaveAsFileSelection
    {
        public SaveFileDialog  m_dialog = null;

        public SaveAsFileSelection(string Title,string fileter)
        {
            this.m_dialog = new SaveFileDialog();
            this.m_dialog.Title = Title;
            this.m_dialog.Filter = fileter;
            
            InitializeUI();
        }

        private void InitializeUI()
        {

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
