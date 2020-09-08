using System;
using System.Collections.Generic;
using System.Text;

namespace Scryber.Styles.Selectors
{
    public class PDFStyleCatchAllMatcher : PDFStyleMatcher
    {

        public PDFStyleCatchAllMatcher() : base(null)
        { }

        public override bool IsMatchedTo(IPDFComponent component, ComponentState state)
        {
            return true;
        }

        protected internal override void ToString(StringBuilder sb)
        {
            sb.Append("*");
        }
    }
}
