using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

namespace ArcMapByJBNT
{
    /// <summary>
    /// .Net颜色选择 管理类
    /// </summary>
    public class NetColorSelection
    {
        private ColorDialog m_dialog = null; 

        public NetColorSelection()
        {
            this.m_dialog = new ColorDialog();

            InitializeUI();
        }

        private void InitializeUI()
        {
        }

        public bool IsColorSelected()
        {
            return m_dialog.ShowDialog() == DialogResult.OK;
        }

        public System.Drawing.Color NetColor
        {
            get
            {
                return this.m_dialog.Color;
            }
        }
    }

    /// <summary>
    /// 颜色模板 选择 管理类
    /// </summary>
    public class ColorPalette
    {
        private ColorDialog _colorDialog;

        public ColorPalette()
        {
            _colorDialog = new ColorDialog();

            InitializeUI();

            SetDefaultColor();
        }

        private void InitializeUI()
        {
            _colorDialog.FullOpen = true;
        }

        private void SetDefaultColor()
        {
            _colorDialog.Color = Color.Yellow;
        }

        public bool IsColorSelected()
        {
            return _colorDialog.ShowDialog() == DialogResult.OK;
        }

        public int Red
        {
            get
            {
                return (int)_colorDialog.Color.R;
            }
        }

        public int Green
        {
            get
            {
                return (int)_colorDialog.Color.G;
            }
        }

        public int Blue
        {
            get
            {
                return (int)_colorDialog.Color.B;
            }
        }
    }
}
