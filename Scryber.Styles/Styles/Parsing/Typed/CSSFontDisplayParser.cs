using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontDisplayParser : CSSStyleValueParser
    {
        public CSSFontDisplayParser()
            : base(CSSStyleItems.FontDisplay)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="onStyle"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            //Do nothing but skip;
            return reader.SkipToNextAttribute();
        }

        

    }
}
