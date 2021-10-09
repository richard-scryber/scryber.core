using System;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSFontFamilyParser : CSSStyleValueParser
    {
        public CSSFontFamilyParser()
            : base(CSSStyleItems.FontFamily)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            FontSelector root = null;
            FontSelector curr = null;
            bool hasExpr = false;

            while (reader.ReadNextValue())
            {
                string fontfamily = reader.CurrentTextValue.Trim();
                FontSelector found;
                if(IsExpression(fontfamily))
                {
                    if (null != root)
                        throw new InvalidOperationException("Expressions - calc() and var() cannot be used within part of a font selector, us calc(concat(...)) instead.");

                    hasExpr = AttachExpressionBindingHandler(onStyle, StyleKeys.FontFamilyKey, fontfamily, DoConvertFontSelector);
                }
                if (TryGetActualFontFamily(fontfamily, out found))
                {
                    if(hasExpr)
                        throw new InvalidOperationException("Expressions - calc() and var() cannot be used within part of a font selector, us calc(concat(...)) instead.");

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

            }
            if (null != root)
            {
                onStyle.SetValue(StyleKeys.FontFamilyKey, root);
                return true;
            }
            else
                return hasExpr;

        }

        protected bool DoConvertFontSelector(StyleBase onStyle, object value, out FontSelector selector)
        {
            if(null == value)
            {
                selector = null;
                return false;
            }
            else if(value is FontSelector f)
            {
                selector = f;
                return true;
            }
            else if(TryGetActualFontFamily(value.ToString(), out selector))
            {
                return true;
            }
            else
            {
                selector = null;
                return false;
            }
        }

        public static bool TryGetActualFontFamily(string fontfamily, out FontSelector found)
        {
            found = null;

            if (string.IsNullOrEmpty(fontfamily))
                return false;

            fontfamily = fontfamily.Trim();



            //bool result = false;

            if (fontfamily.EndsWith(","))
                fontfamily = fontfamily.Substring(0, fontfamily.Length - 1);

            if (fontfamily.StartsWith("'") || fontfamily.StartsWith("\""))
                fontfamily = fontfamily.Substring(1, fontfamily.Length - 2);

            if (string.IsNullOrEmpty(fontfamily))
                return false;

            found = new FontSelector(fontfamily);
            return true;

        }
    }
}
