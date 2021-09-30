using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{
    [PDFParsableValue]
    public class StyleMatcher
    {

        #region public StyleSelector Stack

        private StyleSelector _selector;

        /// <summary>
        /// Gets the actual selector for this matcher
        /// </summary>
        public StyleSelector Selector
        {
            get
            {
                if (null == this._selector)
                    _selector = new StyleSelector();
                return _selector;
            }
        }

        #endregion
       

        //
        // .ctor
        //

        #region public StyleMatcher(StyleSelector selector)

        public StyleMatcher(StyleSelector selector)
        {
            this._selector = selector;
        }

        #endregion

        //
        // methods
        // 

        #region public virtual bool IsMatchedTo(IPDFComponent component, ComponentState state)

        public virtual bool IsMatchedTo(IComponent component, ComponentState state, out int priority)
        {
            if (component is IPDFStyledComponent)
            {

                var curr = this.Selector;
                var matched = curr.IsMatchedTo(component as IPDFStyledComponent, state, out priority);

                return matched;
            }
            else
            {
                priority = 0;
                return false;
            }
        }

        #endregion

        

        // parsing

        #region public static StyleMatcher Parse(string selector)

        public static StyleMatcher Parse(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return null;

            selector = selector.Trim();

            if (string.IsNullOrEmpty(selector))
                return null;

            if (string.Equals("*", selector))
                return new StyleCatchAllMatcher();

            if(string.Equals(":root", selector))
            {
                return new StyleRootMatcher();
            }

            StringBuilder buffer = new StringBuilder();

            var each = selector.Split(", ", StringSplitOptions.RemoveEmptyEntries);
            
            if (each.Length > 1)
            {
                StyleMatcher root = null;

                foreach (var one in each)
                {
                    var all = one.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    StylePlacement placement = StylePlacement.Any;
                    var parsed = ParseSelectorList(all, all.Length - 1, placement, buffer);

                    if (null == root)
                        root = new StyleMatcher(parsed);
                    else
                        root = new StyleMultipleMatcher(parsed, root);
                }
                return root;
            }
            else
            {
                var all = String.IsNullOrEmpty(selector) ? new string[] { } : selector.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                
                StylePlacement placement = StylePlacement.Any;
                var one = ParseSelectorList(all, all.Length - 1, placement, buffer);


                return new StyleMatcher(one);
            }
        }

        #endregion

        #region public static StyleSelector ParseSelectorList(string[] selector, int index, StylePlacement placement)

        public static StyleSelector ParseSelectorList(string[] selector, int index, StylePlacement placement, StringBuilder buffer)
        {
            if (selector[index] == ">")
            {
                placement = StylePlacement.DirectParent;
                index--;
            }
            var one = ParseSingleSelector(selector[index], buffer);
            one.Placement = placement;

            if (index > 0)
                one.Ancestor = ParseSelectorList(selector, index-1, StylePlacement.Any, buffer);

            return one;
        }

        #endregion

        #region public static StyleSelector ParseSingleSelector(string selector)

        public static StyleSelector ParseSingleSelector(string selector, StringBuilder buffer)
        {
            string appliedType = null;
            StyleClassSelector appliedClass = null;
            string appliedId = null;

            int stateIndex = -1;
            ParsingType statePreviousType = ParsingType.Type;

            ComponentState appliedState = ComponentState.Normal;

            ParsingType pt = ParsingType.Type; // no selector

            StringBuilder sb = buffer;
            sb.Clear();

            for(var currIndex = 0; currIndex < selector.Length; currIndex++)
            {
                char c = selector[currIndex];

                if (c == '#')
                {
                    if (sb.Length > 0)
                    {
                        switch (pt)
                        {
                            case ParsingType.Type:
                                appliedType = sb.ToString();
                                break;
                            case ParsingType.Class:
                                appliedClass = new StyleClassSelector(sb.ToString(), appliedClass);
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
                                appliedClass = new StyleClassSelector(sb.ToString(), appliedClass);
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
                {
                    if (c == ':')
                    {
                        stateIndex = currIndex;
                        statePreviousType = pt;
                    }
                    sb.Append(c);
                }

            }
            //At the end of the string
            if(sb.Length > 0)
            {
                switch (pt)
                {
                    case ParsingType.Type:
                        appliedType = sb.ToString();
                        break;
                    case ParsingType.Class:
                        appliedClass = new StyleClassSelector(sb.ToString(), appliedClass); ;
                        break;
                    case ParsingType.Id:
                        appliedId = sb.ToString();
                        break;
                    default:

                        break;
                }
                sb.Clear();
            }

            if(stateIndex >= 0)
            {
                //TODO: Check for state
                if(stateIndex == 0 && appliedType == ":root")
                {

                }
            }

            return new StyleSelector() { AppliedClass = appliedClass, AppliedID = appliedId, AppliedElement = appliedType, AppliedState = appliedState };
        }

        private enum ParsingType
        {
            Type,
            Class,
            Id,
            State
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

        #region public static implicit operator StyleMatcher(string value)

        public static implicit operator StyleMatcher(string value)
        {
            return StyleMatcher.Parse(value);
        }

        #endregion
    }


    

    


    

}
