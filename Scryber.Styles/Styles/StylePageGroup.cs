using System;
using System.Threading;
using Scryber.Styles.Selectors;

namespace Scryber.Styles
{
    public class StylePageGroup : Style, INamingContainer
    {
        public PageMatcher Matcher { get; set; }


        #region public IComponent Parent {get;set;}

        private IComponent _owner;

        /// <summary>
        /// Gets or sets the owning component for this style - could be a remote style link, or a style group collection
        /// </summary>
        public IComponent Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
            }
        }

        #endregion

        public StylePageGroup(PageMatcher matcher) : this()
        {
            this.Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        public StylePageGroup() : base(ObjectTypes.Style)
        { }

        public override void MergeInto(Style style, IComponent Component)
        {
            if (null == this.Matcher)
                base.MergeInto(style, Component);
            else if(this.Matcher.IsMatchedTo(Component))
            {
                if(this.Matcher.Selectors != null && this.Matcher.Selectors.Length > 0)
                {
                   //We matched on a selector name, so increase the priority.
                   this.MergeInto(style, 10);
                }
                else
                {
                    //We matched on a page rule with no selectors, so merge with the default priority.
                    this.MergeInto(style, 0);
                }
            }
        }
    }
}
