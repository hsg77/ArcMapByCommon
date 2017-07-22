using System;
using System.Collections.Generic;
using System.Text;

namespace ArcMapByJBNT
{
    public class ItemClass
    {
        private string _caption = string.Empty;
        private object _value = null;

        public ItemClass(string caption, object value)
        {
            _caption = caption;
            _value = value;
        }
        public ItemClass()
        {
        }

        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public object Tag
        {
            get;
            set;
        }

        public override string ToString()
        {
            return _caption;
        }
    }
}
