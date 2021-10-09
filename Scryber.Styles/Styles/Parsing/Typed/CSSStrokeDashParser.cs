using System;
using Scryber.Html;
using Scryber.Drawing;
using System.Collections.Generic;

namespace Scryber.Styles.Parsing.Typed
{
    public class CSSStrokeDashParser : CSSStyleValueParser
    {

        public CSSStrokeDashParser()
            : base(CSSStyleItems.StrokeDash)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            if(reader.ReadNextValue())
            {
                if(IsExpression(reader.CurrentTextValue))
                {
                    return AttachExpressionBindingHandler(style, StyleKeys.StrokeDashKey, reader.CurrentTextValue, DoConvertDashes);
                }
                else
                {
                    var all = new List<int>(1);

                    do
                    {
                        int parsed;
                        if (ParseInteger(reader.CurrentTextValue, out parsed))
                            all.Add(parsed);
                    }
                    while (reader.ReadNextValue());

                    if (all.Count > 0)
                    {
                        style.SetValue(StyleKeys.StrokeDashKey, new Dash(all.ToArray(), 0));
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;
        }

        protected bool DoConvertDashes(StyleBase style, object value, out Dash dash)
        {
            if(null == value)
            {
                dash = null;
                return false;
            }
            else if(value is Dash d)
            {
                dash = d;
                return true;
            }
            else if(TryParseDashes(value.ToString(), out dash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryParseDashes(string all, out Dash dash)
        {
            if(string.IsNullOrEmpty(all))
            {
                dash = null;
                return false;
            }
            else if(all.IndexOf(" ") > 0)
            {
                var each = all.Split(' ');
                List<int> parsed = new List<int>(each.Length);
                foreach (var item in each)
                {
                    if (ParseInteger(item, out int i))
                        parsed.Add(i);
                }
                dash = new Dash(parsed.ToArray(), 0);
                return true;
            }
            else if(ParseInteger(all, out int i))
            {
                dash = new Dash(new int[] { i }, 0);
                return true;
            }
            else
            {
                dash = null;
                return false;
            }
        }
    }
}
