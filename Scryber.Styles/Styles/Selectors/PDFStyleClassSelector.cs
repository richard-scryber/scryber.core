using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{

    /// <summary>
    /// Matches on a single class name. Can also have another linked class name to match on.
    /// </summary>
    public class PDFStyleClassSelector
    {
        public string ClassName { get; private set; }

        public PDFStyleClassSelector AndClass { get; private set; }

        public PDFStyleClassSelector(string name)
            : this(name, null)
        {
        }

        public PDFStyleClassSelector(string name, PDFStyleClassSelector and)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.ClassName = name;
            this.AndClass = and;
        }

        public bool IsMatchedTo(IPDFStyledComponent component, ComponentState state)
        {
            var index = component.StyleClass.IndexOf(this.ClassName);

            if (index < 0) //not found
                return false;

            else if (component.StyleClass.Length == this.ClassName.Length) // exact match
                ;
            else if (index == 0) //starting
            {
                if (Char.IsWhiteSpace(component.StyleClass, index + this.ClassName.Length) == false)
                    return false;
            }
            else if (component.StyleClass.Length == index + this.ClassName.Length) //ending
            {
                if (Char.IsWhiteSpace(component.StyleClass, index - 1) == false)
                    return false;
            }
            else  //in the middle
            {
                if (Char.IsWhiteSpace(component.StyleClass, index + this.ClassName.Length) == false)
                    return false;

                if (Char.IsWhiteSpace(component.StyleClass, index - 1) == false)
                    return false;
            }

            if (null != this.AndClass)
                return this.AndClass.IsMatchedTo(component, state);
            else
                return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.ToString(sb);
            return sb.ToString();
        }

        protected internal void ToString(StringBuilder buffer)
        {
            if (null != this.AndClass)
                this.AndClass.ToString(buffer);

            buffer.Append(".");
            buffer.Append(this.ClassName);
        }
    }

}
