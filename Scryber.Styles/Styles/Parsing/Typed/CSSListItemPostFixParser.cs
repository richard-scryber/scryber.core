using System;
namespace Scryber.Styles.Parsing.Typed
{
    public class CSSListItemPostFixParser : CSSStringParser
    {
        public CSSListItemPostFixParser()
            : base("-pdf-li-postfix", StyleKeys.ListPostfixKey)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            return base.DoSetStyleValue(style, reader);
        }
    }
}
