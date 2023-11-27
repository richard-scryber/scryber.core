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

        public virtual bool IsMediaMatched()
        {
            if (null == this.Media || this.Media.IsMatchedTo(this._format))
                return true;
            else
                return false;
        }
       
        protected override void DoDataBind(DataContext context, bool includechildren)
        {
            this._format = context.Format;

            base.DoDataBind(context, includechildren);
        }

        public override void MergeInto(Style style, IComponent Component)
        {
            if (this.IsMediaMatched())
                base.MergeInto(style, Component);
        }
    }
}
