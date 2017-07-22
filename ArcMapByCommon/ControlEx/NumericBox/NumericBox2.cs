using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ArcMapByCommon
{
    public partial class NumericBox2 : System.Windows.Forms.TextBox
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool MessageBeep(uint beep);

        private float minimum = 0;
        private float maximum = 100;
        private bool useRange = false;
        private string lastChar;

        public NumericBox2()
        {
            base.Text = "";
        }

        /// <summary>
        /// 获取或设置可以接受的数据集范围
        /// 如果设置的最大值大于等于设置的最小值，
        /// 则表示允许的值在这两个设置值之间。
        /// 如果设置的最大值小于设置的最小值，
        /// 则表示允许的值在这两个设置值之外。
        /// </summary>
        public SizeF SetRange
        {
            get
            {
                return new SizeF(minimum, maximum);
            }
            set
            {
                if (this.SetRange == value)
                    return;

                minimum = value.Width;
                maximum = value.Height;

                // 空文本的数值范围验证
                if (this.useRange && base.Text == "")
                {
                    base.Text = GetMinimumText();
                    return;
                }

                this.ValidateText();
            }
        }

        // 以下两个辅助方法是 VS.NET 设计器所需的。
        public bool ShouldSerializeSetRange() 
        { 
            return (new SizeF(0, 100) != this.SetRange); 
        }

        public void ResetSetRange() 
        { 
            this.maximum = 100; 
            this.minimum = 0; 
        }

        /// <summary>
        /// 获取或设置是否要使用限定值的范围
        /// </summary>
        [System.ComponentModel.DefaultValue(false)]
        public bool UseRange
        {
            get
            {
                return this.useRange;
            }
            set
            {
                if (this.useRange == value)
                    return;
                this.useRange = value;

                // 空文本的数值范围验证
                if (this.useRange && base.Text == "")
                {
                    base.Text = GetMinimumText();
                    return;
                }
                this.ValidateText();
            }
        }

        /// <summary>
        /// 为了检查程序设置的文本值有效性，所以隐藏了基类的 Text 属性。
        /// </summary>
        [System.ComponentModel.DefaultValue("0")]
        public new string Text
        {
            get
            {
                string t = base.Text;
                double d = 0;
                double.TryParse(t, out d);

                return d.ToString();
            }
            set
            {
                // 检查是否是一个数字
                try
                {
                    Double.Parse(value);
                }
                catch
                {
                    // 空文本的数值范围验证
                    if (this.useRange && base.Text == "")
                    {
                        base.Text = GetMinimumText();
                    }
                    return;
                }
                base.Text = value;
                this.ValidateText();
            }
        }
        /// <summary>
        /// 获取或设置数值
        /// </summary>
        public double Value
        {
            get
            {
                return double.Parse(this.Text);
            }
            set
            {
                this.Text = value.ToString();
            }
        }

        /// <summary>
        /// 复盖基类的 OnKeyPress 方法
        /// 用以检测按键是否符合要求。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            // 回车符，检查文本数字值的有效性
            if (e.KeyChar == 13)
            {
                this.ValidateText();

                // 注意，此处要先检查后再调用基类方法
                // 因为基类方法引发的事件处理回调中需要使用到控件的值。
                // 为了回调能够得到准确的值，所以要先进行检查处理。
                base.OnKeyPress(e);

                return;
            }

            // 调用基类方法。先由基类来处理此次按键事件。
            base.OnKeyPress(e);

            // 如果基类的调用已处理了此键，直接返回吧。
            if (e.Handled)
                return;

            // 如果是 - 或 +，只能在第一个位置输入，且不能重复输入。
            if (e.KeyChar == '-' || e.KeyChar == '+')
            {
                if (base.Text.StartsWith("-") ||
                 base.Text.StartsWith("+") ||
                 base.SelectionStart != 0) // 检测光标是否在第一个位置上
                    e.Handled = true;
            }
            else if (e.KeyChar == '.') // 小数点最多只能输入一个。
            {
                e.Handled = (base.Text.IndexOf('.') >= 0);
            }
            else
            {
                // 其他情况只能是数字字符和后退符。
                // 可自行修改补充，以适合更广泛的要求。
                e.Handled = !(Char.IsDigit(e.KeyChar) || e.KeyChar == 8);
            }

            // 如果是不合要求的字符，则发出一个警告音。
            if (e.Handled)
                MessageBeep(0x00000030);
        }

        /// <summary>
        /// 复盖基类的 OnValidated 方法
        /// 用以检测文本是否符合范围要求。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
            this.ValidateText();
        }

        /// <summary>
        /// 当非空文本时，验证文本的数值范围。
        /// </summary>
        protected virtual void ValidateText()
        {
            // 如果没有使用范围验证，直接返回
            if (this.useRange == false)
                return;

            // 如果文本为空、-、+，不在此作任何验证。
            if (this.IsEmpty())
                return;

            decimal val = 0;
            bool normal = true;
            try
            {
                val = Convert.ToDecimal(base.Text);
            }
            catch
            {
                normal = false;
            }

            // 如果解析正常
            if (normal)
            {
                // 如果设置的最大值大于等于设置的最小值，
                // 则表示允许的值在这两个设置值之间。
                if (maximum >= minimum)
                {
                    if (val > (decimal)maximum)
                    {
                        base.Text = maximum.ToString();
                    }
                    else if (val < (decimal)minimum)
                    {
                        base.Text = minimum.ToString();
                    }
                }
                // 如果设置的最大值小于设置的最小值，
                // 则表示允许的值在这两个设置值之外。
                else
                {
                    if (val < (decimal)minimum && val > (decimal)maximum)
                    {
                        base.Text = maximum.ToString();
                        // 也可使用以下方法，结果相同。
                        // base.Text = GetMinimumText();
                    }
                }
            }
            else
            {
                // 如有异常，则使用设置的最小值。
                base.Text = GetMinimumText();
            }


            // 如果除了 - 和 + 以外的文本长度为 1，记住它。
            // 因为用户可能会在删除了最后一个字符后，将输入焦点转移到其他控件。
            // 这么做是为了能在失去焦点时保持文本框不为空。
            // 有关操作，请见 OnLostFocus。
            if (base.Text.StartsWith("-") || base.Text.StartsWith("+"))
            {
                if (base.Text.Length == 2)
                {
                    lastChar = base.Text;
                }
            }
            else
            {
                if (base.Text.Length == 1)
                {
                    lastChar = base.Text;
                }
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            // 当用户的输入焦点离开了此文本框时，
            // 并且当设定了数值的范围，并且文本的宽度为 0 时，
            // 将以前记住的最后有效字符串重新填充。
            // 因为设置了范围后，就不能让文本框中为空。
            if (this.IsEmpty() && useRange)
            {
                if (lastChar != null)
                    base.Text = lastChar;
                else
                    base.Text = GetMinimumText();
            }
        }

        protected string GetMinimumText()
        {
            if (maximum >= minimum)
            {
                return minimum.ToString();
            }
            else
            {
                return maximum.ToString();
            }
        }

        protected bool IsEmpty()
        {
            return (base.Text == "" || base.Text == "-" || base.Text == "+");
        }
    }
}
