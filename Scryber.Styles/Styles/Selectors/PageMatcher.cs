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

            else if (component is IPDFStyledComponent)
            {
                var styled = component as IPDFStyledComponent;

                
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


        public static PageMatcher Parse(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return null;
            else
                selector = selector.Trim();

            PageMatcher pgm = new PageMatcher();
            string[] all;
            if (selector.IndexOf(" ") > 0)
                all = selector.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            else
                all = new string[] { selector };

            pgm.Selectors = all;

            return pgm;
        }
    }
}
