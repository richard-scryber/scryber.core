using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.Text;
using Scryber.Styles.Parsing.Typed;
using System.Globalization;

namespace Scryber.Styles.Parsing
{


    /// <summary>
    /// Abstract base class for all single CSS style value parsers
    /// </summary>
    public abstract class CSSStyleValueParser : IParserStyleFactory
    {
        protected static char[] WhiteSpace = new char[] { ' ' };

        public const double Pixel2Point = (1.0 / 96.0) * 72.0;
        public const double Pica2Point = (1.0 / 12.0) * 72.0;
        public const double Inch2Point = 72.0;
        public const double Centimeter2Point = (72.0 / 2.54);
        public const double Millimetre2Point = (72.0 / 25.4);
        public const double EmToPoint = 12.0;  //1 em = 16px = 12pt

        internal const char HTMLEntityStartMarker = '&';
        internal const char HTMLEntityEndMarker = ';';
        internal const char HTMLEntityNumberMarker = '#';


        #region public string CssName

        /// <summary>
        /// Gets the name of the css style value this instance refers to.
        /// </summary>
        public string CssValueName
        {
            get;
            private set;
        }

        #endregion

        //
        // .ctors
        //

        #region protected CSSStyleValueParser(string cssName)

        protected CSSStyleValueParser(string cssName)
        {
            if (string.IsNullOrEmpty(cssName))
                throw new ArgumentNullException("itemName");

            this.CssValueName = cssName;
        }

        #endregion

        bool IParserStyleFactory.SetStyleValue(IHtmlContentParser parser, IStyledComponent onComponent, CSSStyleItemReader reader)
        {
            return this.SetStyleValue(onComponent.Style, reader, parser.Context);
        }

        public bool SetStyleValue(Style style, CSSStyleItemReader reader, ContextBase context)
        {
            var attr = reader.CurrentAttribute;
            //var val = reader.CurrentTextValue;

            
            bool success = this.DoSetStyleValue(style, reader);

            if (null != context && null != context.TraceLog)
            {
                if (success && context.ShouldLogDebug)
                {
                    context.TraceLog.Add(TraceLevel.Debug, "CSS", "The css style item " + (string.IsNullOrEmpty(attr) ? "[UNKNOWN]" : attr) + " set on style " + style.ToString());
                }
                else if (!success && context.TraceLog.ShouldLog(TraceLevel.Warning))
                {
                    context.TraceLog.Add(TraceLevel.Warning, "CSS", "The css style item " + (string.IsNullOrEmpty(attr) ? "[UNKNOWN]" : attr) + " could not be set on style " + style.ToString());
                }
            }
            return success;
        }


        protected abstract bool DoSetStyleValue(Style style, CSSStyleItemReader reader);


        protected bool AttachExpressionBindingHandler<AttrT>(Style style, StyleKey<AttrT> key, string value, StyleValueConvertor<AttrT> convert)
        {
            StyleValueExpression<AttrT> expression = new StyleValueExpression<AttrT>(key, value, convert);
            style.DataBinding += expression.BindValue;

            if (style.IsValueDefined(key))
                style.RemoveValue(key);

            style.AddValue(expression);
            return true;
        }


        #region protected bool IsExpression(string part)

        protected bool IsExpression(string part)
        {
            if (part.StartsWith("var(") || part.StartsWith("calc("))
                return true;
            else
                return false;
        }

        #endregion

        #region protected bool IsColor(string part)

        protected bool IsColor(string part)
        {
            return part[0] == '#' || CSSColors.Names2Colors.ContainsKey(part) || part.StartsWith("rgb(", StringComparison.OrdinalIgnoreCase) || part.StartsWith("rgba(", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region protected bool IsUrl(string part)

        protected bool IsUrl(string part)
        {
            return part.StartsWith("url(", StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region protected bool IsNumber(string part)

        protected bool IsNumber(string part)
        {
            if (string.IsNullOrEmpty(part))
                return false;
            else if (char.IsDigit(part, 0))
                return true;
            else if (part[0] == '-' && part.Length > 1 && char.IsDigit(part, 1))
                return true;
            else if (part[0] == '+' && part.Length > 1 && char.IsDigit(part, 1))
                return true;
            else if (part[0] == '.' && part.Length > 1 && char.IsDigit(part, 1))
                return true;

            return false;
        }

        #endregion

        #region public static bool ParseCSSColor(string part, out PDFColor color)

        /// <summary>
        /// Tries to parse a colour from the css value. Returns true if successful, otherwise false.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static bool ParseCSSColor(string part, out Color color, out double? opacity)
        {
            bool result = false;
            color = Color.Transparent;

            opacity = null;
            if (string.IsNullOrEmpty(part) == false)
            {
                string hexValue;

                if (CSSColors.Names2Colors.TryGetValue(part, out hexValue))
                    result = Color.TryParse(hexValue, out color);

                else if (part.StartsWith("#"))
                    result = Color.TryParse(part, out color);

                else if (part.StartsWith("rgb(", StringComparison.InvariantCultureIgnoreCase))
                    result = Color.TryParse(part, out color);
                else if (part.StartsWith("rgba(", StringComparison.InvariantCultureIgnoreCase))
                    result = Color.TryParseRGBA(part, out color, out opacity);
            }
            return result;
        }

        #endregion

        #region public static bool TryConvertToUnit(object value, out Unit converted)

        public static bool TryConvertToUnit(object value, out Unit converted)
        {
            if(value is Unit)
            {
                converted = (Unit)value;
                return true;
            }
            else if(value is string)
            {
                return Unit.TryParse((string)value, out converted);
            }
            else if(value is IFormattable)
            {
                var formattable = value as IFormattable;
                var str = formattable.ToString(null, CultureInfo.InvariantCulture);
                return Unit.TryParse(str, out converted);
            }
            else
            {
                var str = value.ToString();
                return Unit.TryParse(str, out converted);
            }
        }

        #endregion

        #region public static bool ParseCSSUnit(string part, out PDFUnit unit)

        /// <summary>
        /// Tries to parse a CSS Unit value (e.g. 20px, 40cm) into a PDFUnit value, returning true if successful
        /// </summary>
        /// <param name="part"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool ParseCSSUnit(string part, out Unit unit)
        {
            
            double factor;
            double parsed;
            int unitLength;
            part = part.ToLower();

            //TODO: THis is really covered now with the Unit.TryParse and can be simplified

            if (EndsWithRelativeUnit(part))
            {
                if (Unit.TryParse(part, out unit))
                    return true;
                else
                {
                    unit = Unit.Zero;
                    return false;
                }
            }
            else if (EndsWithAbsoluteUnit(part, out unitLength, out factor))
            {
                
                if (unitLength > 0)
                {
                    if (double.TryParse(part.Substring(0, part.Length - unitLength), NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
                    {
                        unit = new Unit(parsed * factor, PageUnits.Points);
                        return true;
                    }
                }
            }
            else if (double.TryParse(part, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
            {
                unit = new Unit(parsed, PageUnits.Points);
                return true;
            }

            // could not parse
            unit = Unit.Zero;
            return false;
        }

        protected static bool EndsWithRelativeUnit(string part)
        {
            if (part.EndsWith("%"))
                return true;
            else if (part.EndsWith("ex"))
                return true;
            else if (part.EndsWith("ch"))
                return true;
            else if (part.EndsWith("em"))
                return true;
            else if (part.EndsWith("rem"))
                return true;
            else if (part.EndsWith("vw"))
                return true;
            else if (part.EndsWith("vh"))
                return true;
            else if (part.EndsWith("vmin"))
                return true;
            else if (part.EndsWith("vmax"))
                return true;

            else
                return false;
        }


        public static bool EndsWithAbsoluteUnit(string part, out int unitlength, out double pointsFactor)
        {
            unitlength = 2;
            if (part.EndsWith("pt"))
            {
                pointsFactor = 1.0;
                return true;
            }
            if (part.EndsWith("px"))
            {
                pointsFactor = Pixel2Point;
                return true;
            }
            if (part.EndsWith("pc")) //pica
            {
                pointsFactor = Pica2Point;
                return true;
            }
            if (part.EndsWith("in"))
            {
                pointsFactor = Inch2Point;
                return true;
            }
            if (part.EndsWith("cm"))
            {
                pointsFactor = Centimeter2Point;
                return true;
            }
            if (part.EndsWith("mm"))
            {
                pointsFactor = Millimetre2Point;
                return true;
            }
            unitlength = 0;
            pointsFactor = 1.0;
            return false;
        }

        #endregion

        #region public static bool ParseCSSUrl(string value, out string url)

        /// <summary>
        /// Parses a Css Url reference (e.g. url(picture.jpg)) into the url value, returning true if successful
        /// </summary>
        /// <param name="value"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool ParseCSSUrl(string value, out string url)
        {
            url = null;

            if (!string.IsNullOrEmpty(value) && value.StartsWith("url(", StringComparison.InvariantCultureIgnoreCase))
            {
                if (value.EndsWith(")"))
                {
                    url = value.Substring(4, value.Length - 5);

                    if (url.StartsWith("'") && url.EndsWith("'"))
                        url = url.Substring(1, url.Length - 2);
                    else if (url.StartsWith("\"") && url.EndsWith("\""))
                        url = url.Substring(1, url.Length - 2);
                    url = UnEscapeHtmlString(url);
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region public static string UnEscapeHtmlString(string value)

        public static string UnEscapeHtmlString(string value)
        {
            int ampersandPos = value.IndexOf(HTMLEntityStartMarker);

            if(ampersandPos < 0)
                return value;

            StringBuilder buffer = new StringBuilder();

            StringEnumerator src = new StringEnumerator(value);
            src.MoveNext();

            while (ampersandPos >= 0)
            {
                if(src.Offset< ampersandPos-1)
                    buffer.Append(value, src.Offset,  ampersandPos - (src.Offset)); 

                src.Offset = ampersandPos;
                bool terminated = true;

                while (src.Current != HTMLEntityEndMarker)
                {
                    if (!src.MoveNext())
                    {
                        terminated = false;
                        break;
                    }
                    else if (src.Offset > ampersandPos + 10)
                    {
                        terminated = false;
                        break;
                    }
                }

                if(terminated)
                {
                    int len = 1 + src.Offset - ampersandPos;
                    char found;
                    if (len > 3)
                    {
                        string entity = src.Substring(ampersandPos, len);
                        if(entity[1] == HTMLEntityNumberMarker)
                        {
                            int charNum;
                            if (int.TryParse(entity.Substring(2, entity.Length - 3), out charNum))
                            {
                                found = (char)charNum;
                                src.MoveNext();
                                buffer.Append(found);
                            }
                        }
                        else if (HtmlEntities.DefaultKnownHTMLAndXMLEntities.TryGetValue(entity.Substring(1, entity.Length -2), out found))
                        {
                            buffer.Append(found);
                            src.MoveNext();
                        }
                    }
                }
                else
                {

                    var len = src.Offset - ampersandPos;
                    buffer.Append(HTMLEntityStartMarker);
                    src.Offset = ampersandPos + 1;

                }

                ampersandPos = value.IndexOf(HTMLEntityStartMarker, src.Offset);
            }

            if (src.Offset < src.Length)
                buffer.Append(src.Substring(src.Offset));

            return buffer.ToString();
        }

        #endregion

        #region public static bool ParseDouble(string value, out double number)

        public static bool ParseDouble(string value, out double number)
        {
            return ParseDouble(value, System.Globalization.CultureInfo.InvariantCulture, out number);
        }

        public static bool ParseDouble(string value, IFormatProvider format, out double number)
        {
            if (double.TryParse(value, System.Globalization.NumberStyles.Any, format, out number))
                return true;
            else
                return false;
        }

        #endregion

        #region public static bool ParseInteger(string value, out int number)

        public static bool ParseInteger(string value, out int number)
        {
            return ParseInteger(value, System.Globalization.CultureInfo.InvariantCulture, out number);
        }

        public static bool ParseInteger(string value, IFormatProvider format, out int number)
        {
            if (int.TryParse(value, System.Globalization.NumberStyles.Any, format, out number))
                return true;
            else
                return false;
        }

        #endregion

    }

}
