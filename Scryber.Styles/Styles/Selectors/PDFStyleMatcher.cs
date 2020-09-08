using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{
    [PDFParsableValue]
    public class PDFStyleMatcher
    {

        #region public PDFStyleMatch Stack

        private PDFStyleSelector _selector;

        /// <summary>
        /// Gets the actual selector for this matcher
        /// </summary>
        public PDFStyleSelector Selector
        {
            get
            {
                if (null == this._selector)
                    _selector = new PDFStyleSelector();
                return _selector;
            }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFStyleMatcher(PDFStyleMatch selector)

        public PDFStyleMatcher(PDFStyleSelector selector)
        {
            this._selector = selector;
        }

        #endregion

        //
        // methods
        // 

        #region public virtual bool IsMatchedTo(IPDFComponent component, ComponentState state)

        public virtual bool IsMatchedTo(IPDFComponent component, ComponentState state)
        {
            if (component is IPDFStyledComponent)
            {
                var curr = this.Selector;
                var matched = curr.IsMatchedTo(component as IPDFStyledComponent, state);

                return matched;
            }
            else
                return false;
        }

        #endregion

        // parsing

        #region public static PDFStyleMatcher Parse(string selector)

        public static PDFStyleMatcher Parse(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return null;

            if (string.Equals("*", selector))
                return new PDFStyleCatchAllMatcher();

            var each = selector.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            
            if (each.Length > 1)
            {
                PDFStyleMatcher root = null;

                foreach (var one in each)
                {
                    var all = one.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    StylePlacement placement = StylePlacement.Any;
                    var parsed = ParseSelectorList(all, all.Length - 1, placement);

                    if (null == root)
                        root = new PDFStyleMatcher(parsed);
                    else
                        root = new PDFStyleMultipleMatcher(parsed, root);
                }
                return root;
            }
            else
            {
                var all = String.IsNullOrEmpty(selector) ? new string[] { } : selector.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                
                StylePlacement placement = StylePlacement.Any;
                var one = ParseSelectorList(all, all.Length - 1, placement);


                return new PDFStyleMatcher(one);
            }
        }

        #endregion

        #region public static PDFStyleMatch ParseSelectorList(string[] selector, int index, StylePlacement placement)

        public static PDFStyleSelector ParseSelectorList(string[] selector, int index, StylePlacement placement)
        {
            if (selector[index] == ">")
            {
                placement = StylePlacement.DirectParent;
                index--;
            }
            var one = ParseSingleSelector(selector[index]);
            one.Placement = placement;

            if (index > 0)
                one.Ancestor = ParseSelectorList(selector, index-1, StylePlacement.Any);

            return one;
        }

        #endregion

        #region public static PDFStyleMatch ParseSingleSelector(string selector)

        public static PDFStyleSelector ParseSingleSelector(string selector)
        {
            string appliedType = null;
            PDFStyleClassSelector appliedClass = null;
            string appliedId = null;

            ParsingType pt = ParsingType.Type; // no selector

            StringBuilder sb = new StringBuilder();
            foreach (var c in selector)
            {
                if (c == '#')
                {
                    if(sb.Length > 0)
                    {
                        switch (pt)
                        {
                            case ParsingType.Type:
                                appliedType = sb.ToString();
                                break;
                            case ParsingType.Class:
                                appliedClass = new PDFStyleClassSelector(sb.ToString(), appliedClass);
                                break;
                            case ParsingType.Id:
                                appliedId = sb.ToString();
                                break;
                            default:

                                break;
                        }
                        sb.Clear(); 
                    }
                    pt = ParsingType.Id;
                }
                else if (c == '.')
                {
                    if (sb.Length > 0)
                    {
                        switch (pt)
                        {
                            case ParsingType.Type:
                                appliedType = sb.ToString();
                                break;
                            case ParsingType.Class:
                                appliedClass = new PDFStyleClassSelector(sb.ToString(), appliedClass);
                                break;
                            case ParsingType.Id:
                                appliedId = sb.ToString();
                                break;
                            default:

                                break;
                        }
                        sb.Clear();
                    }
                    pt = ParsingType.Class;
                }
                else
                    sb.Append(c);

            }
            if(sb.Length > 0)
            {
                switch (pt)
                {
                    case ParsingType.Type:
                        appliedType = sb.ToString();
                        break;
                    case ParsingType.Class:
                        appliedClass = new PDFStyleClassSelector(sb.ToString(), appliedClass); ;
                        break;
                    case ParsingType.Id:
                        appliedId = sb.ToString();
                        break;
                    default:

                        break;
                }
                sb.Clear();
            }
            return new PDFStyleSelector() { AppliedClass = appliedClass, AppliedID = appliedId, AppliedElement = appliedType };
        }

        private enum ParsingType
        {
            Type,
            Class,
            Id
        }

        #endregion

        //
        // ToString
        // 

        #region public override string ToString() + 1 overload

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            this.ToString(builder);
            return builder.ToString();
        }

        protected internal virtual void ToString(StringBuilder sb)
        {
            this.Selector.ToString(sb);
        }

        #endregion

        //
        // Implicic cast from string
        //

        #region public static implicit operator PDFStyleMatcher(string value)

        public static implicit operator PDFStyleMatcher(string value)
        {
            return PDFStyleMatcher.Parse(value);
        }

        #endregion
    }


    

    


    

}
