using System;
using Newtonsoft.Json;
using Scryber.Styles;

namespace Scryber.Styles.Selectors
{
    /// <summary>
    /// Matches against the (non-parsable and non-structural) AtPage component.
    /// This allows for multiple page styles to be defined and applied to different pages in a document, and they will be matched based on their name.
    /// Also supports @page pseudo-classes: :first, :left, :right.
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

        /// <summary>
        /// The pseudo-class for this page matcher. Normal means no pseudo-class (matches any page).
        /// First matches page index 0 only. Right matches even indices (0, 2, 4…). Left matches odd indices (1, 3, 5…).
        /// </summary>
        public ComponentState PseudoClass { get; set; } = ComponentState.Normal;


        public PageMatcher()
        {
        }

        public bool IsMatchedTo(IComponent component)
        {
            if(component.ElementName != "@page")
                return false;

            bool nameMatched;
            if (null == this.Selectors || this.Selectors.Length == 0)
                nameMatched = true;
            else if (component is IStyledComponent styled)
                nameMatched = IsSelectorMatchedTo(styled);
            else
                nameMatched = false;

            if (!nameMatched)
                return false;

            if (this.PseudoClass == ComponentState.Normal)
                return true;

            if (component is IPageIndexProvider pip)
                return IsPseudoClassMatchedAt(pip.LayoutPageIndex);

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

        private bool IsPseudoClassMatchedAt(int pageIndex)
        {
            if (pageIndex < 0)
                return false;

            switch (this.PseudoClass)
            {
                case ComponentState.First: return pageIndex == 0;
                case ComponentState.Right: return pageIndex % 2 == 0;
                case ComponentState.Left:  return pageIndex % 2 == 1;
                default:                   return false;
            }
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
                    all[i] = all[i].Trim();
            }
            else
                all = new string[] { selector };

            var names = new System.Collections.Generic.List<string>();

            foreach (var token in all)
            {
                ComponentState pseudo;
                string namepart;

                if (TryParsePseudoSuffix(token, out pseudo, out namepart))
                {
                    pgm.PseudoClass = pseudo;
                    pgm.PageState = pseudo;
                    if (!string.IsNullOrEmpty(namepart))
                        names.Add(namepart);
                }
                else
                {
                    names.Add(token);
                }
            }

            pgm.Selectors = names.Count > 0 ? names.ToArray() : null;

            return pgm;
        }

        /// <summary>
        /// Checks whether token is or ends with a known pseudo-class suffix (:first, :left, :right).
        /// Returns true if matched, setting pseudo and the name portion that preceded it (may be empty).
        /// </summary>
        private static bool TryParsePseudoSuffix(string token, out ComponentState pseudo, out string namePart)
        {
            if (token.Equals(":first", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.First; namePart = string.Empty; return true;
            }
            if (token.Equals(":left", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.Left; namePart = string.Empty; return true;
            }
            if (token.Equals(":right", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.Right; namePart = string.Empty; return true;
            }
            if (token.EndsWith(":first", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.First; namePart = token.Substring(0, token.Length - 6); return true;
            }
            if (token.EndsWith(":left", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.Left; namePart = token.Substring(0, token.Length - 5); return true;
            }
            if (token.EndsWith(":right", StringComparison.OrdinalIgnoreCase))
            {
                pseudo = ComponentState.Right; namePart = token.Substring(0, token.Length - 6); return true;
            }

            pseudo = ComponentState.Normal;
            namePart = token;
            return false;
        }
    }
}
