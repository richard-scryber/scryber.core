using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Scryber.Drawing
{

    /// <summary>
    /// Defines one or more font family names as a single linked structure
    /// </summary>
    [PDFParsableComponent("font-family")]
    [PDFParsableValue]
    public class FontSelector
    {

        private FontSelector _next;
        private string _familyName;

        public string FamilyName
        {
            get { return this._familyName; }
        }

        public FontSelector Next
        {
            get { return this._next; }
            set
            {
                this._next = value;
                AssertChain(value);
            }
        }

        public FontSelector(string name) : this(name, null)
        { }

        public FontSelector(string name, FontSelector next)
        {
            this._familyName = name;
            this._next = next;
        }

        private void AssertChain(FontSelector entry)
        {
            while (null != entry)
            {
                if (entry == this)
                    throw new InvalidOperationException("The linked font selector list is circular and links back to itself. This is not allowed");
                else
                    entry = entry.Next;
            }

        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            this.ToString(buffer);
            return buffer.ToString();
        }

        public void ToString(StringBuilder buffer)
        {
            if (this.FamilyName.Contains(" "))
            {
                buffer.Append("'");
                buffer.Append(this.FamilyName);
                buffer.Append("'");
            }
            else
                buffer.Append(this.FamilyName);

            if(null != this.Next)
            {
                buffer.Append(", ");
                this.Next.ToString(buffer);
            }
        }

        public static explicit operator FontSelector(string value)
        {
            return FontSelector.Parse(value);
        }

        public static explicit operator string(FontSelector font)
        {
            return null == font ? null : font.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FontSelector);
        }

        public bool Equals(FontSelector selector)
        {
            if (null == selector)
                return false;

            else if (string.Equals(this.FamilyName, selector.FamilyName, StringComparison.OrdinalIgnoreCase))
            {
                if (null == selector.Next)
                    return null == this.Next;
                else if (null == this.Next)
                    return false;
                else
                    return this.Next.Equals(selector.Next);
            }
            else
                return false;

        }

        public static FontSelector Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var index = value.IndexOf(',');
            if (index < 0)
            {
                var selector = ParseSingle(value);
                return selector;
            }
            else
            {
                FontSelector root = null;
                FontSelector curr = null;

                string[] all = value.Split(',');

                foreach (var f in all)
                {
                    var one = ParseSingle(f);
                    if (null != one)
                    {
                        if (null == root)
                            root = one;
                        if (null != curr)
                            curr.Next = one;

                        curr = one;
                    }
                }

                //Return the linked list of names
                return root;
            }
        }



        private static FontSelector ParseSingle(string value)
        {
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
                return null;

            //Single selector (could have quotes on)
            if (value.StartsWith("\""))
            {
                value = value.Substring(1);
                if (value.EndsWith("\""))
                    value = value.Substring(0, value.Length - 1);
                return new FontSelector(value);
            }
            else if (value.StartsWith("'"))
            {
                value = value.Substring(1);
                if (value.EndsWith("'"))
                    value = value.Substring(0, value.Length - 1);
                return new FontSelector(value);
            }
            else
                return new FontSelector(value);
        }

        
    }
}
