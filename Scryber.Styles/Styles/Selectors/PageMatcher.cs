using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Scryber.Styles;

namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// Matches against the (non-parsable and non-structural) AtPage component. 
    /// This allows for multiple page styles to be defined and applied to different pages in a document, and they will be matched based on their name.
    /// </summary>
    [PDFParsableValue]
    public class PageMatcher
    {

        public string[] Selectors { get; set; }
        
        public ComponentState PageState { get; set; }

        public bool HasState()
        {
            return this.PageState != ComponentState.Normal;
        }


        public PageMatcher()
        {
            ComponentState state = ComponentState.Normal;
        }

        public bool IsMatchedTo(IComponent component)
        {
            if(component.ElementName != "@page")
            {
                return false;
            }

            if (null == this.Selectors || this.Selectors.Length == 0)
                return true;

            else if (component is IStyledComponent)
            {
                var styled = component as IStyledComponent;
                if(IsSelectorMatchedTo(styled)) 
                    return true;
            }

            return false;
        }

        private bool IsSelectorMatchedTo(IStyledComponent styled)
        {
                
                var val = styled.StyleClass;
                
                if(string.IsNullOrEmpty(val) && null == this.Selectors || this.Selectors.Length == 0)
                    return true;

                else if (string.IsNullOrEmpty(val))
                    return false;
                else
                {
                    foreach (var sel in Selectors)
                    {
                        if (sel == val)
                            return true;
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

            ComponentState state = ComponentState.Normal;
            if (selector.EndsWith(":first"))
            {
                state = ComponentState.First;
                selector = selector.Substring(0, selector.Length - ":first".Length);
            }
            else if (selector.EndsWith(":right"))
            {
                state = ComponentState.Right;
                selector = selector.Substring(0, selector.Length - ":right".Length);
            }
            else if (selector.EndsWith(":left"))
            {
                state = ComponentState.Left;
                selector = selector.Substring(0, selector.Length - ":left".Length).Trim();
            }

            if (selector.Length == 0)
            {
                return new PageMatcher() { PageState = state };
            }


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
            pgm.PageState = state;

            return pgm;
        }
    }
}
