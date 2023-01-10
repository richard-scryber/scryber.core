using System;
using Scryber.Styles.Selectors;

namespace Scryber.Styles
{
    public class StylePageGroup : Style
    {
        public PageMatcher Matcher { get; set; }

        public StylePageGroup(PageMatcher matcher) : this()
        {
            this.Matcher = matcher;
        }

        public StylePageGroup() : base(ObjectTypes.Style)
        { }

        public override void MergeInto(Style style, IComponent Component)
        {
            if (null == this.Matcher)
                base.MergeInto(style, Component);
            else if(this.Matcher.IsMatchedTo(Component))
                base.MergeInto(style, Component);
        }
    }
}
