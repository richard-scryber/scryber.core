using System;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Wraps a bool for HTML boolean attributes (selected, checked, multiple, disabled, readonly)
    /// which HtmlAgilityPack normalizes to a self-valued attribute (selected="selected") when
    /// converting to XHTML - a value the standard bool.Parse-based converter always rejects,
    /// unconditionally, in both parser conformance modes.
    /// </summary>
    [PDFParsableValue()]
    public struct HtmlBoolean
    {
        public bool Value { get; }

        public HtmlBoolean(bool value)
        {
            this.Value = value;
        }

        public static HtmlBoolean Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return new HtmlBoolean(false);

            switch (value.Trim().ToLowerInvariant())
            {
                case "false":
                case "0":
                case "no":
                    return new HtmlBoolean(false);
                default:
                    //true, 1, yes, or a self-valued HTML boolean attribute e.g. selected="selected"
                    return new HtmlBoolean(true);
            }
        }

        public static implicit operator bool(HtmlBoolean value) => value.Value;

        public static implicit operator HtmlBoolean(bool value) => new HtmlBoolean(value);

        public override string ToString() => this.Value ? "true" : "false";
    }
}
