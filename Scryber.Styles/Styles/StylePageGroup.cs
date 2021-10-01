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

        public override void MergeInto(Style style, IComponent Component, ComponentState state)
        {
            if (null == this.Matcher)
                base.MergeInto(style, Component, state);
            else if(this.Matcher.IsMatchedTo(Component))
                base.MergeInto(style, Component, state);
        }
    }
}
