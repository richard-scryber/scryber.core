using System;

namespace Scryber.Styles.Parsing.Typed
{
    /// <summary>
    /// Parses the text-indent css property - the indent of the first line of a block of text. Can be a length or a percentage of the container width.
    /// </summary>
    public class CSSTextIndentParser : CSSUnitStyleParser
    {
        public CSSTextIndentParser()
            : base(Scryber.Html.CSSStyleItems.TextIndent, StyleKeys.TextFirstLineIndentKey)
        {
        }
    }
}
