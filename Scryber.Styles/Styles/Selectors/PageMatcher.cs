using System;
using Scryber.Styles;

namespace Scryber.Styles.Selectors
{
    [PDFParsableValue]
    public class PageMatcher
    {

        public string[] Selectors { get; set; }


        public PageMatcher()
        {
        }

        public bool IsMatchedTo(IComponent component)
        {
            if (null == this.Selectors)
                return true;

            else if (component is IStyledComponent)
            {
                var styled = component as IStyledComponent;

                
                var val = styled.StyleClass;

                if (string.IsNullOrEmpty(val))
                    return false;
                else
                {
                    foreach (var sel in Selectors)
                    {
                        if (sel == val)
                            return true;
                    }
                }
            }

            return false;
        }


        private static char[] _whitespaceSplitter = new char[] {' '};
        public static PageMatcher Parse(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return null;
            else
                selector = selector.Trim();

            PageMatcher pgm = new PageMatcher();
            string[] all;
            
            if (selector.IndexOf(" ") > 0)
            {
                all = selector.Split(_whitespaceSplitter, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < all.Length; i++)
                {
                    all[i] = all[i].Trim();
                }
            }
            else
                all = new string[] { selector };

            pgm.Selectors = all;

            return pgm;
        }
    }
}
