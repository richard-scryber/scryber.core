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
                bool hasName   = this.Matcher.Selectors != null && this.Matcher.Selectors.Length > 0;
                bool hasPseudo = this.Matcher.PseudoClass != ComponentState.Normal;

                // Specificity mirrors CSS Paged Media spec:
                // @page name:pseudo = 30, @page :pseudo = 20, @page name = 10, @page = 0
                int priority;
                if      (hasName && hasPseudo) priority = 30;
                else if (hasPseudo)            priority = 20;
                else if (hasName)              priority = 10;
                else                           priority = 0;

                this.MergeInto(style, priority);
            }
        }
    }
}
