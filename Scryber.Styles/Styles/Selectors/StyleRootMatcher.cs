using System;
using System.Text;

namespace Scryber.Styles.Selectors
{
    public class StyleRootMatcher : StyleMatcher
    {
        public StyleRootMatcher() : base(null)
        {
        }

        public override bool IsMatchedTo(IComponent component, ComponentState state, out int priority)
        {
            if (component is IDocument)
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
