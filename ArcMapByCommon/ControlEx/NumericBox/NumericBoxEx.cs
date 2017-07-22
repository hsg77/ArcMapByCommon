using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Diagnostics;

namespace ArcMapByCommon
{
    public class NumericBoxEx : UserControl
    {
        #region 内部类
        private class Win32Util
        {
            private const int ETO_CLIPPED = 4;
            private const int ETO_OPAQUE = 2;
            public const int OPAQUE = 2;
            public const int TRANSPARENT = 1;

            public static void ExtTextOut(IntPtr hdc, int x, int y, Rectangle clip, string str)
            {
                RECT rect;
                rect.top = clip.Top;
                rect.left = clip.Left;
                rect.bottom = clip.Bottom;
                rect.right = clip.Right;
                Win32API.ExtTextOut(hdc, x, y, 4, ref rect, str, str.Length, IntPtr.Zero);
            }

            public static void FillRect(IntPtr hdc, Rectangle clip, Color color)
            {
                RECT rect;
                rect.top = clip.Top;
                rect.left = clip.Left;
                rect.bottom = clip.Bottom;
                rect.right = clip.Right;
                int crColor = (((color.B & 0xff) << 0x10) | ((color.G & 0xff) << 8)) | color.R;
                IntPtr hBrush = Win32API.CreateSolidBrush(crColor);
                Win32API.FillRect(hdc, ref rect, hBrush);
            }

            public static int GET_X_LPARAM(int lParam)
            {
                return (lParam & 0xffff);
            }

            public static int GET_Y_LPARAM(int lParam)
            {
                return (lParam >> 0x10);
            }

            public static Point GetPointFromLPARAM(int lParam)
            {
                return new Point(GET_X_LPARAM(lParam), GET_Y_LPARAM(lParam));
            }

            public static Size GetTextExtent(IntPtr hdc, string str)
            {
                SIZE size;
                size.cx = 0;
                size.cy = 0;
                Win32API.GetTextExtentPoint32(hdc, str, str.Length, ref size);
                return new Size(size.cx, size.cy);
            }

            public static int HIGH_ORDER(int param)
            {
                return (param >> 0x10);
            }

            public static int LOW_ORDER(int param)
            {
                return (param & 0xffff);
            }

            public static void SelectObject(IntPtr hdc, IntPtr handle)
            {
                Win32API.SelectObject(hdc, handle);
            }

            public static void SetBkColor(IntPtr hdc, Color color)
            {
                int num = (((color.B & 0xff) << 0x10) | ((color.G & 0xff) << 8)) | color.R;
                Win32API.SetBkColor(hdc, num);
            }

            public static void SetBkMode(IntPtr hdc, int mode)
            {
                Win32API.SetBkMode(hdc, mode);
            }

            public static void SetTextColor(IntPtr hdc, Color color)
            {
                int num = (((color.B & 0xff) << 0x10) | ((color.G & 0xff) << 8)) | color.R;
                Win32API.SetTextColor(hdc, num);
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct SIZE
            {
                public int cx;
                public int cy;
            }

            private class Win32API
            {
                [DllImport("gdi32")]
                internal static extern IntPtr CreateSolidBrush(int crColor);
                [DllImport("gdi32.dll")]
                public static extern int ExtTextOut(IntPtr hdc, int x, int y, int options, ref Win32Util.RECT clip, string str, int len, IntPtr spacings);
                [DllImport("User32.dll", CharSet = CharSet.Auto)]
                internal static extern int FillRect(IntPtr hDC, ref Win32Util.RECT rect, IntPtr hBrush);
                [DllImport("gdi32.dll")]
                public static extern int GetTextExtentPoint32(IntPtr hdc, string str, int len, ref Win32Util.SIZE size);
                [DllImport("gdi32.dll")]
                public static extern int SelectObject(IntPtr hdc, IntPtr hgdiObj);
                [DllImport("gdi32.dll")]
                public static extern int SetBkColor(IntPtr hdc, int color);
                [DllImport("gdi32.dll")]
                public static extern int SetBkMode(IntPtr hdc, int mode);
                [DllImport("gdi32.dll")]
                public static extern int SetTextColor(IntPtr hdc, int color);
            }
        }

        private class XPTheme
        {
            [DllImport("uxtheme.dll")]
            public static extern void CloseThemeData(IntPtr ht);
            [DllImport("uxtheme.dll")]
            public static extern void DrawThemeBackground(IntPtr ht, IntPtr hd, int iPartId, int iStateId, ref RECT rect, ref RECT cliprect);
            [DllImport("uxtheme.dll")]
            public static extern void DrawThemeEdge(IntPtr ht, IntPtr hd, int iPartId, int iStateId, ref RECT destrect, int uedge, int uflags, ref RECT contentrect);
            [DllImport("uxtheme.dll")]
            public static extern void DrawThemeLine(IntPtr ht, IntPtr hd, int iStateId, ref RECT rect, int dwDtlFlags);
            [DllImport("uxtheme.dll")]
            public static extern void DrawThemeParentBackground(IntPtr h, IntPtr hDC, ref RECT rect);
            [DllImport("uxtheme.dll")]
            public static extern void DrawThemeText(IntPtr ht, IntPtr hd, int iPartId, int iStateId, [MarshalAs(UnmanagedType.LPTStr)] string psztext, int charcount, int dwtextflags, int dwtextflags2, ref RECT rect);
            [DllImport("uxtheme.dll")]
            public static extern bool IsAppThemed();
            [DllImport("uxtheme.dll")]
            public static extern int IsThemeActive();
            [DllImport("uxtheme.dll")]
            public static extern IntPtr OpenThemeData(IntPtr h, [MarshalAs(UnmanagedType.LPTStr)] string pszClassList);
            [DllImport("uxtheme.dll")]
            public static extern void SetWindowTheme(IntPtr h, [MarshalAs(UnmanagedType.LPTStr)] string pszSubAppName, [MarshalAs(UnmanagedType.LPTStr)] string pszSubIdList);
        }
        #endregion

        #region 内部结构

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int l, int t, int r, int b)
            {
                this.left = l;
                this.top = t;
                this.right = r;
                this.bottom = b;
            }

            public RECT(Rectangle r)
            {
                this.left = r.Left;
                this.top = r.Top;
                this.right = r.Right;
                this.bottom = r.Bottom;
            }

            public Rectangle ToRectangle()
            {
                return new Rectangle(this.left, this.top, this.right, this.bottom);
            }
        }

        #endregion

        #region Field

        private bool bNegative;
        private Container components;
        private ContextMenu contextMenu1;
        private bool[] cp = new bool[12];
        private char ds = ',';
        private int gn = 3;
        private char gs = '.';
        private bool isCompatibleOS;
        private bool keystrokeProcessed;
        private bool m_bNegative;
        protected IntPtr m_htheme = IntPtr.Zero;
        private int m_nDecimalNumber = 2;
        private Color m_NegativeColor = Color.Red;
        private string m_sText = "";
        private MenuItem menuCopyAll;
        private int nShiftLeft = 4;
        private Point ptPos = Point.Empty;
        private string sDisplay = "";
        private string signsimbol = "-";
        private string sOldText = "";
        private char[] x = new char[40];
        private char[] xk = new char[40];
        private int xkn;
        private char[] y = new char[10];
        private char[] yk = new char[10];

        #endregion

        public NumericBoxEx()
        {
            PlatformID platform = Environment.OSVersion.Platform;
            Version version = Environment.OSVersion.Version;
            Version version2 = new Version("5.1.2600.0");
            this.isCompatibleOS = (version >= version2) && (platform == PlatformID.Win32NT);
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.FixedHeight, true);
            base.SetStyle(ControlStyles.Selectable, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.Height = this.Font.Height + 7;
            base.KeyPress += new KeyPressEventHandler(this.KeyPressed);
            this.Init();
        }

        protected void ChangePosition(Keys k)
        {
            int num;
            switch (k)
            {
                case Keys.End:
                    for (num = 1; num <= (this.m_nDecimalNumber + 2); num++)
                    {
                        if (this.cp[num])
                        {
                            break;
                        }
                    }
                    if (num < (this.m_nDecimalNumber + 2))
                    {
                        this.cp[num] = false;
                        this.cp[this.m_nDecimalNumber + 2] = true;
                        base.Invalidate();
                    }
                    return;

                case Keys.Home:
                    for (num = 1; num <= (this.m_nDecimalNumber + 2); num++)
                    {
                        if (this.cp[num])
                        {
                            break;
                        }
                    }
                    if (num > 1)
                    {
                        this.cp[num] = false;
                        this.cp[1] = true;
                        base.Invalidate();
                    }
                    return;

                case Keys.Left:
                    for (num = 1; num <= (this.m_nDecimalNumber + 2); num++)
                    {
                        if (this.cp[num])
                        {
                            break;
                        }
                    }
                    if (num > 1)
                    {
                        this.cp[num] = false;
                        this.cp[num - 1] = true;
                        base.Invalidate();
                    }
                    return;

                case Keys.Up:
                    return;

                case Keys.Right:
                    for (num = 1; num <= (this.m_nDecimalNumber + 2); num++)
                    {
                        if (this.cp[num])
                        {
                            break;
                        }
                    }
                    if (num < (this.m_nDecimalNumber + 2))
                    {
                        this.cp[num] = false;
                        this.cp[num + 1] = true;
                        base.Invalidate();
                    }
                    return;

                case Keys.Decimal:
                    if (this.m_nDecimalNumber <= 0)
                    {
                        return;
                    }
                    num = 1;
                    while (num <= (this.m_nDecimalNumber + 2))
                    {
                        if (this.cp[num])
                        {
                            break;
                        }
                        num++;
                    }
                    break;

                default:
                    return;
            }
            if (num > 1)
            {
                this.cp[num] = false;
                this.cp[1] = true;
            }
            else
            {
                this.cp[1] = false;
                this.cp[2] = true;
            }
            base.Invalidate();
        }

        protected void ChangeSign(Keys k)
        {
            if (k == Keys.Add)
            {
                if (this.bNegative)
                {
                    this.DisplayToOrginal();
                    this.bNegative = false;
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
            }
            else if ((k == Keys.Subtract) && !this.bNegative)
            {
                this.DisplayToOrginal();
                this.bNegative = true;
                this.OrginalToDisplay();
                base.Invalidate();
            }
        }

        [DllImport("user32.dll")]
        public static extern int CreateCaret(IntPtr hwnd, IntPtr hbm, int cx, int cy);
        protected void DeleteLeftChar()
        {
            if (this.cp[1])
            {
                this.DisplayToOrginal();
                if ((this.xkn == 1) && (this.xk[1] != '0'))
                {
                    this.xk[1] = '0';
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
                else if (this.xkn > 1)
                {
                    for (int i = 1; i <= this.xkn; i++)
                    {
                        this.xk[i] = this.xk[i + 1];
                    }
                    this.xkn--;
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
            }
            else
            {
                int index = 1;
                while (index <= (this.m_nDecimalNumber + 2))
                {
                    if (this.cp[index])
                    {
                        break;
                    }
                    index++;
                }
                if ((index > 2) && (index <= (this.m_nDecimalNumber + 2)))
                {
                    this.cp[index] = false;
                    this.cp[index - 1] = true;
                    this.DisplayToOrginal();
                    this.yk[index - 2] = '0';
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
                else if (index == 2)
                {
                    this.cp[index] = false;
                    this.cp[index - 1] = true;
                    base.Invalidate();
                }
            }
        }

        [DllImport("user32.dll")]
        public static extern int DestroyCaret();
        protected void DisplayToOrginal()
        {
            int num;
            int num2 = (this.sDisplay.Length - ((this.m_nDecimalNumber > 0) ? (this.m_nDecimalNumber + 1) : 0)) - (this.bNegative ? 1 : 0);
            this.xkn = 0;
            for (num = 1; num <= num2; num++)
            {
                if ((num % (this.gn + 1)) != 0)
                {
                    this.xk[num - (num / (this.gn + 1))] = this.sDisplay.Substring((num2 - num) + (this.bNegative ? 1 : 0), 1).ToCharArray()[0];
                    this.xkn++;
                }
            }
            if (this.m_nDecimalNumber > 0)
            {
                for (num = 1; num <= this.m_nDecimalNumber; num++)
                {
                    this.yk[num] = this.sDisplay.Substring((num2 + num) + (this.bNegative ? 1 : 0), 1).ToCharArray()[0];
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.KeyPress -= new KeyPressEventHandler(this.KeyPressed);
                if (this.m_htheme != IntPtr.Zero)
                {
                    XPTheme.CloseThemeData(this.m_htheme);
                }
                if (this.components != null)
                {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [DllImport("user32.dll")]
        public static extern int HideCaret(IntPtr hwnd);
        protected void Init()
        {
            NumberFormatInfo numberFormat = CultureInfo.CurrentCulture.NumberFormat;
            this.ds = numberFormat.NumberDecimalSeparator.ToCharArray()[0];
            this.gs = numberFormat.NumberGroupSeparator.ToCharArray()[0];
            this.gn = numberFormat.NumberGroupSizes[0];
            this.signsimbol = numberFormat.NegativeSign;
            this.bNegative = false;
            for (int i = 1; i <= (this.m_nDecimalNumber + 2); i++)
            {
                this.cp[i] = false;
            }
            this.cp[1] = true;
            this.sDisplay = this.initString();
            this.sOldText = this.sDisplay;
            this.m_sText = this.sDisplay;
        }

        private void InitializeComponent()
        {
            this.contextMenu1 = new ContextMenu();
            this.menuCopyAll = new MenuItem();
            base.SuspendLayout();
            this.contextMenu1.MenuItems.AddRange(new MenuItem[] { this.menuCopyAll });
            this.menuCopyAll.Index = 0;
            this.menuCopyAll.Shortcut = Shortcut.CtrlV;
            this.menuCopyAll.Text = "复制(&C)";
            this.menuCopyAll.Click += new EventHandler(this.menuCopyAll_Click);
            this.BackColor = SystemColors.Window;
            this.Cursor = Cursors.Default;
            base.Name = "NumericBox";
            base.Size = new Size(150, 0x24);
            base.ResumeLayout(false);
        }

        protected string initString()
        {
            string str = "";
            if (this.m_nDecimalNumber > 0)
            {
                str = "0" + this.ds.ToString();
                for (int i = 1; i <= this.m_nDecimalNumber; i++)
                {
                    str = str + "0";
                }
                return str;
            }
            return "0";
        }

        protected void InsertKey(char k)
        {
            if (this.cp[1])
            {
                if (this.xkn <= 20)
                {
                    this.DisplayToOrginal();
                    if ((this.xkn == 1) && (this.xk[1] == '0'))
                    {
                        this.xk[1] = k;
                    }
                    else
                    {
                        for (int i = 1; i <= this.xkn; i++)
                        {
                            this.xk[(this.xkn - i) + 2] = this.xk[(this.xkn - i) + 1];
                        }
                        this.xk[1] = k;
                        this.xkn++;
                    }
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
            }
            else
            {
                int index = 1;
                while (index <= (this.m_nDecimalNumber + 2))
                {
                    if (this.cp[index])
                    {
                        break;
                    }
                    index++;
                }
                if ((index > 1) && (index < (this.m_nDecimalNumber + 2)))
                {
                    this.cp[index] = false;
                    this.cp[index + 1] = true;
                    this.DisplayToOrginal();
                    this.yk[index - 1] = k;
                    this.OrginalToDisplay();
                    base.Invalidate();
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            this.keystrokeProcessed = true;
            switch (keyData)
            {
                case Keys.Back:
                    this.DeleteLeftChar();
                    break;

                case Keys.Escape:
                    break;

                case Keys.End:
                    this.ChangePosition(Keys.End);
                    break;

                case Keys.Home:
                    this.ChangePosition(Keys.Home);
                    break;

                case Keys.Left:
                    this.ChangePosition(Keys.Left);
                    return true;

                case Keys.Up:
                    this.ChangePosition(Keys.Left);
                    return true;

                case Keys.Right:
                    this.ChangePosition(Keys.Right);
                    return true;

                case Keys.Down:
                    this.ChangePosition(Keys.Right);
                    return true;

                case Keys.Delete:
                    {
                        string sDisplay = this.sDisplay;
                        this.Init();
                        this.sOldText = sDisplay;
                        base.Invalidate();
                        break;
                    }
                case Keys.Add:
                    if (this.m_bNegative)
                    {
                        this.ChangeSign(Keys.Add);
                        base.Invalidate();
                    }
                    break;

                case Keys.Subtract:
                case Keys.OemMinus:
                    if (this.m_bNegative)
                    {
                        this.ChangeSign(Keys.Subtract);
                        base.Invalidate();
                    }
                    break;

                default:
                    if (((Keys.Shift & keyData) != Keys.None) && ((Keys.Oemplus & keyData) == Keys.Oemplus))
                    {
                        if (this.m_bNegative)
                        {
                            this.ChangeSign(Keys.Add);
                            base.Invalidate();
                        }
                    }
                    else
                    {
                        if ((Keys.Control & keyData) != Keys.None)
                        {
                            return true;
                        }
                        if ((Keys.Alt & keyData) != Keys.None)
                        {
                            return true;
                        }
                        this.keystrokeProcessed = false;
                    }
                    break;
            }
            return base.IsInputKey(keyData);
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (!this.keystrokeProcessed)
            {
                switch (e.KeyChar)
                {
                    case ',':
                    case '.':
                        this.ChangePosition(Keys.Decimal);
                        return;

                    case '-':
                    case '/':
                        return;

                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        this.InsertKey(e.KeyChar);
                        return;
                }
            }
        }

        private void menuCopyAll_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.m_sText);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            this.OnResize(EventArgs.Empty);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (this.m_nDecimalNumber > 0)
            {
                for (int i = 1; i <= (this.m_nDecimalNumber + 2); i++)
                {
                    this.cp[i] = false;
                }
                this.cp[1] = true;
            }
            base.Invalidate();
            Size size = new Size(1, this.Font.Height);
            CreateCaret(base.Handle, IntPtr.Zero, size.Width, size.Height);
            SetCaretPos(this.ptPos.X, this.ptPos.Y);
            ShowCaret(base.Handle);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (this.sDisplay == (this.signsimbol + this.initString()))
            {
                this.sDisplay = this.initString();
                base.Invalidate();
            }
            HideCaret(base.Handle);
            DestroyCaret();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                base.OnPaint(pe);
                Rectangle clientRectangle = base.ClientRectangle;
                ControlPaint.DrawBorder3D(pe.Graphics, clientRectangle, Border3DStyle.Sunken);
                if ((this.isCompatibleOS && (XPTheme.IsThemeActive() == 1)) && ((this.m_htheme == IntPtr.Zero)/* && (this.boxStyle == Style.XpStyle)*/))
                {
                    this.m_htheme = XPTheme.OpenThemeData(IntPtr.Zero, "Edit");
                }
                IntPtr handle = this.Font.ToHfont();
                IntPtr hdc = pe.Graphics.GetHdc();
                Win32Util.SetBkMode(hdc, 1);
                Win32Util.SelectObject(hdc, handle);
                if (!base.Enabled)
                {
                    Win32Util.SetTextColor(hdc, Color.FromName("ControlDarkDark"));
                }
                else if (this.sDisplay.IndexOf("-") != -1)
                {
                    Win32Util.SetTextColor(hdc, this.m_NegativeColor);
                }
                else
                {
                    Win32Util.SetTextColor(hdc, this.ForeColor);
                }
                if (this.m_htheme != IntPtr.Zero)
                {
                    RECT rect = new RECT(base.ClientRectangle);
                    IntPtr hd = hdc;
                    XPTheme.DrawThemeBackground(this.m_htheme, hd, 1, base.Enabled ? 1 : 4, ref rect, ref rect);
                    Rectangle clip = base.ClientRectangle;
                    clip.Inflate(-1, -1);
                    Win32Util.FillRect(hdc, clip, this.BackColor);
                }
                Size textExtent = Win32Util.GetTextExtent(hdc, this.sDisplay);
                Win32Util.ExtTextOut(hdc, (clientRectangle.Width - textExtent.Width) - this.nShiftLeft, clientRectangle.Y + 3, clientRectangle, this.sDisplay);
                if (this.m_nDecimalNumber > 0)
                {
                    for (int i = 1; i <= (this.m_nDecimalNumber + 2); i++)
                    {
                        if (this.cp[i])
                        {
                            Point point = new Point();
                            point.Y = 3;
                            if (i < (this.m_nDecimalNumber + 2))
                            {
                                string str = this.sDisplay.Substring(this.sDisplay.Length - ((this.m_nDecimalNumber + 2) - i), (this.m_nDecimalNumber + 2) - i);
                                Size size2 = Win32Util.GetTextExtent(hdc, str);
                                point.X = (clientRectangle.Width - size2.Width) - this.nShiftLeft;
                            }
                            else
                            {
                                point.X = clientRectangle.Width - this.nShiftLeft;
                            }
                            this.ptPos = point;
                            if (this.Focused)
                            {
                                SetCaretPos(this.ptPos.X, this.ptPos.Y);
                            }
                        }
                    }
                }
                else
                {
                    Point point2 = new Point();
                    point2.Y = 3;
                    point2.X = clientRectangle.Width - this.nShiftLeft;
                    this.ptPos = point2;
                    if (this.Focused)
                    {
                        SetCaretPos(this.ptPos.X, this.ptPos.Y);
                    }
                }
                pe.Graphics.ReleaseHdc(hdc);
                if (this.sOldText != this.sDisplay)
                {
                    this.sOldText = this.sDisplay;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            try
            {
                base.Height = this.Font.Height + 7;
                base.Invalidate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected void OrginalToDisplay()
        {
            int num;
            this.sDisplay = "";
            for (num = 1; num <= (this.xkn + ((this.xkn - 1) / this.gn)); num++)
            {
                if ((num % (this.gn + 1)) == 0)
                {
                    this.x[num] = this.gs;
                }
                else
                {
                    this.x[num] = this.xk[num - (num / (this.gn + 1))];
                }
                this.sDisplay = this.x[num].ToString() + this.sDisplay;
            }
            if (this.m_nDecimalNumber > 0)
            {
                this.x[0] = this.ds;
                this.sDisplay = this.sDisplay + this.x[0].ToString();
                for (num = 1; num <= this.m_nDecimalNumber; num++)
                {
                    this.y[num] = this.yk[num];
                    this.sDisplay = this.sDisplay + this.y[num].ToString();
                }
            }
            if (this.bNegative)
            {
                this.sDisplay = this.signsimbol + this.sDisplay;
            }
            this.m_sText = this.sDisplay;
        }

        [DllImport("user32.dll")]
        public static extern int SetCaretPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern int ShowCaret(IntPtr hwnd);
        protected override void WndProc(ref Message m)
        {
            bool flag = true;
            switch (m.Msg)
            {
                case 0x7b:
                    {
                        Point pointFromLPARAM = Win32Util.GetPointFromLPARAM((int)m.LParam);
                        pointFromLPARAM = base.PointToClient(pointFromLPARAM);
                        this.contextMenu1.Show(this, pointFromLPARAM);
                        break;
                    }
                case 0x31a:
                    if (this.m_htheme != IntPtr.Zero)
                    {
                        XPTheme.CloseThemeData(this.m_htheme);
                        this.m_htheme = IntPtr.Zero;
                    }
                    base.Invalidate();
                    break;
            }
            if (flag)
            {
                base.WndProc(ref m);
            }
        }

        [Category("Custom"), Browsable(true), Description("Enable negative numbers"), DefaultValue(false)]
        public bool NegativeNumber
        {
            get
            {
                return this.m_bNegative;
            }
            set
            {
                if (this.m_bNegative != value)
                {
                    this.m_bNegative = value;
                    base.Invalidate();
                }
            }
        }

        [Category("Custom"), DefaultValue("Red"), Browsable(true), Description("Color for negative numbers")]
        public Color NegativeNumberColor
        {
            get
            {
                return this.m_NegativeColor;
            }
            set
            {
                if (this.m_NegativeColor != value)
                {
                    this.m_NegativeColor = value;
                    base.Invalidate();
                }
            }
        }

        [Browsable(true), DefaultValue(2), Category("Custom"), TypeConverter(typeof(NoOfDigitConverter)), Description("Number of digits after decimal symbol")]
        public int NoDigitsAfterDecimalSymbol
        {
            get
            {
                return this.m_nDecimalNumber;
            }
            set
            {
                this.m_nDecimalNumber = value;
                string sDisplay = this.sDisplay;
                this.Init();
                this.sOldText = sDisplay;
                base.Invalidate();
            }
        }

        [DefaultValue("0.00"), Description(""), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Custom"), Bindable(true)]
        public override string Text
        {
            get
            {
                return this.m_sText;
            }
            set
            {
                if (this.m_sText != value)
                {
                    if ((value.Length > 10) && (value.Substring(0, 10).ToLower() == "numericbox"))
                    {
                        value = "0.00";
                    }
                    string format = "{0:#,##0";
                    if (this.m_nDecimalNumber > 0)
                    {
                        format = format + ".";
                        for (int i = 0; i < this.m_nDecimalNumber; i++)
                        {
                            format = format + "0";
                        }
                    }
                    format = format + "}";

                    decimal temp_d = 0;
                    decimal.TryParse(value, out temp_d);
                    this.m_sText = string.Format(format, temp_d);
                    this.sDisplay = this.m_sText;
                    if (this.sDisplay.IndexOf("-") != -1)
                    {
                        this.bNegative = true;
                    }
                    else
                    {
                        this.bNegative = false;
                    }
                    base.Invalidate();
                }
                if (this.sOldText != this.sDisplay)
                {
                    this.sOldText = this.sDisplay;
                    this.OnTextChanged(EventArgs.Empty);
                }
            }
        }

        private class NoOfDigitConverter : Int16Converter
        {
            public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return new TypeConverter.StandardValuesCollection(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            }

            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
            {
                return false;
            }

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {
                return true;
            }
        }
    }
}
