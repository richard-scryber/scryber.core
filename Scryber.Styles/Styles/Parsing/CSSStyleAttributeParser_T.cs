using System;
namespace Scryber.Styles.Parsing
{
    /// <summary>
    /// Abstract class that has a known style key of a specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CSSStyleAttributeParser<T> : CSSStyleValueParser
    {
        private PDFStyleKey<T> _styleAttr;

        public PDFStyleKey<T> StyleAttribute
        {
            get { return _styleAttr; }
        }

        public CSSStyleAttributeParser(string itemName, PDFStyleKey<T> styleAttr)
            : base(itemName)
        {
            _styleAttr = styleAttr ?? throw new ArgumentNullException("styleAttr");
        }


        protected void SetValue(Style onStyle, T value)
        {
            onStyle.SetValue(_styleAttr, value);
        }


    }
}
