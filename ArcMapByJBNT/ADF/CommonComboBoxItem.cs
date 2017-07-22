/*******************************************************************************
 Copyright (C) 
 File Name  : CommonComboBoxItem.cs
 Summary    : 公用下拉项类
 Version    : 1.0
 Create     : 2007-08-24
 Author     : 何仕国
*******************************************************************************/

using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ArcMapByJBNT
{
    public class CommonComboBoxItem : object
    {
        public CommonComboBoxItem()
        {
        }
        public CommonComboBoxItem(string pText, string pValue)
        {
            this.Text = pText;
            this.Value = pValue;
        }
        public CommonComboBoxItem(string pText, string pValue, object pTag)
        {
            this.Text = pText;
            this.Value = pValue;
            this.Tag = pTag;
        }
        private string m_Text = "";
        public string Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
            }
        }

        private string m_Value = "";
        public string Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        private object m_Tag = null;
        public object Tag
        {
            get
            {
                return m_Tag;
            }
            set
            {
                m_Tag = value;
            }
        }

        private object m_Ex1 = null;
        public object Ex1
        {
            get
            {
                return m_Ex1;
            }
            set
            {
                m_Ex1 = value;
            }
        }

        private object m_Ex2 = null;
        public object Ex2
        {
            get
            {
                return m_Ex2;
            }
            set
            {
                m_Ex2 = value;
            }
        }

        private object m_Ex3 = null;
        public object Ex3
        {
            get
            {
                return m_Ex3;
            }
            set
            {
                m_Ex3 = value;
            }
        }

        private object m_Ex4 = null;
        public object Ex4
        {
            get
            {
                return m_Ex4;
            }
            set
            {
                m_Ex4 = value;
            }
        }

        private object m_Ex5 = null;
        public object Ex5
        {
            get
            {
                return m_Ex5;
            }
            set
            {
                m_Ex5 = value;
            }
        }

        public override string ToString()
        {
            return this.Text;
        }

        public override bool Equals(object obj)
        {
            CommonComboBoxItem ccbi=obj as CommonComboBoxItem;
            if (ccbi != null)
            {
                if (this.Text == ccbi.Text &&
                   this.Value == ccbi.Value )
                {
                    return true;
                }                
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class XpComboBoxItem : System.Object
    {
        public string Text = "";
        public string Value = "";
        public object Tag = null;
        public string Ex1 = "";
        public string Ex2 = "";
        public string LabelText = "";

        public XpComboBoxItem()
        {
        }
        public XpComboBoxItem(string pText)
        {
            this.Text = pText;
        }
        public XpComboBoxItem(string pText, string pValue)
        {
            this.Text = pText;
            this.Value = pValue;
        }
        public XpComboBoxItem(string pText, string pValue, object pTag)
        {
            this.Text = pText;
            this.Value = pValue;
            this.Tag = pTag;
        }

        public XpComboBoxItem(string pText, string pValue, object pTag, string pEx1)
        {
            this.Text = pText;
            this.Value = pValue;
            this.Tag = pTag;
            this.Ex1 = pEx1;
        }

        public XpComboBoxItem(string pText, string pValue, object pTag, string pEx1, string pLabel)
        {
            this.Text = pText;
            this.Value = pValue;
            this.Tag = pTag;
            this.Ex1 = pEx1;
            this.LabelText = pLabel;
        }

        public XpComboBoxItem(string pText, string pValue, object pTag, string pEx1, string pLabel, string pEx2)
        {
            this.Text = pText;
            this.Value = pValue;
            this.Tag = pTag;
            this.Ex1 = pEx1;
            this.LabelText = pLabel;
            this.Ex2 = pEx2;
        }

        public override string ToString()
        {
            return this.Text.ToString();
        }
    }        
}
