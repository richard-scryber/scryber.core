using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles
{
    public class StyleMediaGroup : StyleGroup
    {
        public Selectors.MediaMatcher Media { get; set; }

        public StyleMediaGroup() : base()
        {

        }

        public StyleMediaGroup(Selectors.MediaMatcher media) : this()
        {
            this.Media = media;
        }

        private OutputFormat _format = OutputFormat.PDF;

        protected override void DoDataBind(DataContext context, bool includechildren)
        {
            _format = context.OutputFormat;

            base.DoDataBind(context, includechildren);
        }

        public override void MergeInto(Style style, IComponent Component, ComponentState state)
        {
            if (null == this.Media || this.Media.IsMatchedTo(this._format))
                base.MergeInto(style, Component, state);
        }
    }
}
