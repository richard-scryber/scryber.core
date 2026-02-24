using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSColumnFillParser : CSSEnumStyleParser<ColumnFillMode>
    {
        public CSSColumnFillParser() : base(CSSStyleItems.ColumnFill, StyleKeys.ColumnFillKey)
        {}
    }
}