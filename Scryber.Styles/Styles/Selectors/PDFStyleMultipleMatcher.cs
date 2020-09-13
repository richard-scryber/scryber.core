using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{
    public class PDFStyleMultipleMatcher : PDFStyleMatcher
    {

        #region public PDFStyleMatcher Next  {get; private set;}

        /// <summary>
        /// Gets the next matcher in this sequence
        /// </summary>
        public PDFStyleMatcher Next
        {
            get;
            private set;
        }

        #endregion

        //
        // ..ctor
        //

        #region public PDFStyleMultipleMatcher(PDFStyleMatch current, PDFStyleMatcher next) : base(current)

        public PDFStyleMultipleMatcher(PDFStyleSelector current, PDFStyleMatcher next) : base(current)
        {
            this.Next = next;
        }

        #endregion

        public override bool IsMatchedTo(IPDFComponent component, ComponentState state, out int priority)
        {
            if (base.IsMatchedTo(component, state, out priority))
                return true;
            else if (null != this.Next)
                return this.Next.IsMatchedTo(component, state, out priority);
            else
            {
                priority = 0;
                return false;
            }
        }

        protected internal override void ToString(StringBuilder sb)
        {
            //Reverse order for route

            if (null != this.Next)
            {
                this.Next.ToString(sb);
                sb.Append(",");
            }
            base.ToString(sb);
        }
    }
}
