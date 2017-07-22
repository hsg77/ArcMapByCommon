using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    /// <summary>
    /// 目录选择 管理类
    /// </summary>
    public class DirectorySelection
    {
        public FolderBrowserDialog  m_dialog = null;

        public DirectorySelection(System.Environment.SpecialFolder pSpecialFolder, string pSelectedPath, bool pShowNewFolderButton)
        {
            this.m_dialog = new FolderBrowserDialog();
            this.m_dialog.RootFolder = pSpecialFolder;
            if (pSelectedPath.Trim() != "")
            {
                this.m_dialog.SelectedPath = pSelectedPath;
            }
            this.m_dialog.ShowNewFolderButton = pShowNewFolderButton;
            
            InitializeUI();
        }

        private void InitializeUI()
        {

        }

        public bool IsDirectorySelected()
        {
            return m_dialog.ShowDialog() == DialogResult.OK;
        }

        public string GetSelectedPath
        {
            get
            {
                return this.m_dialog.SelectedPath;
            }
        }
    }
}
