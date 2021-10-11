using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{

    /// <summary>
    /// Matches on a single class name. Can also have another linked class name to match on.
    /// </summary>
    public class StyleClassSelector
    {
        public string ClassName { get; private set; }

        public StyleClassSelector AndClass { get; private set; }

        public StyleClassSelector(string name)
            : this(name, null)
        {
        }

        public StyleClassSelector(string name, StyleClassSelector and)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            this.ClassName = name;
            this.AndClass = and;
        }

        public bool IsMatchedTo(IStyledComponent component, ComponentState state)
        {
            
            var equals = component.StyleClass.Equals(this.ClassName);
            if(equals)
            {
                if (null != this.AndClass)
                    return AndClass.IsMatchedTo(component, state);
                else
                    return true;
            }
            else
            {
                int foundIndex = 0;

                do
                {
                    if (ContainsClassName(component.StyleClass, foundIndex, out foundIndex))
                    {
                        if (null != this.AndClass)
                            return AndClass.IsMatchedTo(component, state);
                        else
                            return true;
                    }
                    else if (foundIndex > -1)
                        foundIndex++;
                }
                while (foundIndex >= 0);

                return false;
            }

            //We are not equal, so if we are shorted or the same length return false
            if (component.StyleClass.Length <= this.ClassName.Length)
                return false;

            //Now we know we are longer

            var starts = component.StyleClass.StartsWith(this.ClassName);
            if (starts && Char.IsWhiteSpace(component.StyleClass, this.ClassName.Length))
            {
                if (null != this.AndClass)
                    return AndClass.IsMatchedTo(component, state);
                else
                    return true;
            }

            var ends = component.StyleClass.EndsWith(this.ClassName);
            int index = component.StyleClass.Length - this.ClassName.Length;
            if (ends && Char.IsWhiteSpace(component.StyleClass, index))

            

            if (null != this.AndClass)
                return this.AndClass.IsMatchedTo(component, state);
            else
                return true;
        }

        private bool ContainsClassName(string styleClass, int startIndex, out int foundIndex)
        {
            foundIndex = styleClass.IndexOf(this.ClassName, startIndex);

            if (foundIndex < 0) //not found
                return false;

            else if (styleClass.Length == this.ClassName.Length) // exact match
                return true;

            else if (foundIndex == 0) //starting
            {
                if (Char.IsWhiteSpace(styleClass, foundIndex + this.ClassName.Length) == false)
                    return false;
            }
            else if (styleClass.Length == foundIndex + this.ClassName.Length) //ending
            {
                if (Char.IsWhiteSpace(styleClass, foundIndex - 1) == false)
                    return false;
            }
            else  //in the middle
            {
                if (Char.IsWhiteSpace(styleClass, foundIndex + this.ClassName.Length) == false)
                    return false;

                if (Char.IsWhiteSpace(styleClass, foundIndex - 1) == false)
                    return false;
            }

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
