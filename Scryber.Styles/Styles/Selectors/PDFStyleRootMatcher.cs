using System;
using System.Text;

namespace Scryber.Styles.Selectors
{
    public class PDFStyleRootMatcher : PDFStyleMatcher
    {
        public PDFStyleRootMatcher() : base(null)
        {
        }

        public override bool IsMatchedTo(IPDFComponent component, ComponentState state, out int priority)
        {
            if (component is IPDFDocument)
            {
                priority = 0;
                return true;
            }
            else
            {
                priority = -1;
                return false;
            }
        }

        protected internal override void ToString(StringBuilder sb)
        {
            sb.Append(":root");
        }
    }
}
