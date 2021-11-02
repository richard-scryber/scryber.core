using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Text;
using System.Configuration;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFontSourceParser : CSSStyleValueParser
    {
        public CSSFontSourceParser() : base(CSSStyleItems.FontSource)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            FontSource root = null;
            FontSource curr = null;
            bool hasExpr = false;

            while (reader.ReadNextValue(',', ';', true))
            {
                string src = reader.CurrentTextValue.Trim();
                FontSource found;
                if(IsExpression(src))
                {
                    if (hasExpr)
                        throw new InvalidOperationException("Connot bind mupltiple values onto a Font src property");

                    if (this.AttachExpressionBindingHandler(style, StyleKeys.FontFaceSrcKey, src, DoConvertFontSource))
                        hasExpr = true;

                }
                if (TryGetFontSource(src, out found))
                {
                    if (hasExpr)
                        throw new InvalidOperationException("Connot bind mupltiple values onto a Font src property");

                    if (null == root)
                        root = found;
                    if (null == curr)
                        curr = found;
                    else
                    {
                        curr.Next = found;
                        curr = found;
                    }
                }
                //We need this to go past the original comma on the following ones,
                //becase we ignore white space in values, bit of a hack
                if (!reader.InnerEnumerator.EOS && reader.InnerEnumerator.Current == ',')
                    reader.InnerEnumerator.MoveNext();
            }
            if (null != root)
            {
                style.SetValue(StyleKeys.FontFaceSrcKey, root);
                return true;
            }
            else
                return false;
        }

        protected bool DoConvertFontSource(StyleBase onstyle, object value, out FontSource source)
        {
            if(null == value)
            {
                source = null;
                return false;
            }
            else if(value is FontSource src)
            {
                source = src;
                return true;
            }
            else if(TryGetFontSource(value.ToString(), out source))
            {
                return true;
            }
            else
            {
                source = null;
                return false;
            }
        }

        private bool TryGetFontSource(string value, out FontSource found)
        {
            if (FontSource.TryParseOneValue(value, out found))
                return true;
            else
                return false;
        }
    }
}
