using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.Text;
using Scryber.Styles.Parsing.Typed;

namespace Scryber.Styles.Parsing
{

    #region public abstract class CSSStyleValueParser : IParserStyleFactory

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

        bool IParserStyleFactory.SetStyleValue(IHtmlContentParser parser, IPDFStyledComponent onComponent, CSSStyleItemReader reader)
        {
            return this.SetStyleValue(onComponent.Style, reader, parser.Context);
        }

        public bool SetStyleValue(Style style, CSSStyleItemReader reader, PDFContextBase context)
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


        protected bool AttachExpressionBindingHandler<AttrT>(Style style, PDFStyleKey<AttrT> key, string value, StyleValueConvertor<AttrT> convert)
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
        public static bool ParseCSSColor(string part, out PDFColor color, out double? opacity)
        {
            bool result = false;
            color = null;
            opacity = null;
            if (string.IsNullOrEmpty(part) == false)
            {
                string hexValue;

                if (CSSColors.Names2Colors.TryGetValue(part, out hexValue))
                    result = PDFColor.TryParse(hexValue, out color);

                else if (part.StartsWith("#"))
                    result = PDFColor.TryParse(part, out color);

                else if (part.StartsWith("rgb(", StringComparison.InvariantCultureIgnoreCase))
                    result = PDFColor.TryParse(part, out color);
                else if (part.StartsWith("rgba(", StringComparison.InvariantCultureIgnoreCase))
                    result = PDFColor.TryParseRGBA(part, out color, out opacity);
            }
            return result;
        }

        #endregion

        #region public static bool ParseCSSUnit(string part, out PDFUnit unit)

        /// <summary>
        /// Tries to parse a CSS Unit value (e.g. 20px, 40cm) into a PDFUnit value, returning true if successful
        /// </summary>
        /// <param name="part"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool ParseCSSUnit(string part, out PDFUnit unit)
        {
            
            double factor;
            double parsed;
            int unitLength;
            if (EndsWithRelativeUnit(part))
            {
                unit = PDFUnit.Zero;
                return false;
            }
            else if (EndsWithAbsoluteUnit(part, out unitLength, out factor))
            {
                
                if (unitLength > 0)
                {
                    if (double.TryParse(part.Substring(0, part.Length - unitLength), out parsed))
                    {
                        unit = new PDFUnit(parsed * factor, PageUnits.Points);
                        return true;
                    }
                }
            }
            else if (double.TryParse(part, out parsed))
            {
                unit = new PDFUnit(parsed * Pixel2Point, PageUnits.Points);
                return true;
            }

            // could not parse
            unit = PDFUnit.Zero;
            return false;
        }

        protected static bool EndsWithRelativeUnit(string part)
        {
            if (part.EndsWith("%", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("ex", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("ch", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("rem", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("vw", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("vh", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("vmin", StringComparison.OrdinalIgnoreCase))
                return true;
            else if (part.EndsWith("vmax", StringComparison.OrdinalIgnoreCase))
                return true;
            
            else
                return false;
        }


        public static bool EndsWithAbsoluteUnit(string part, out int unitlength, out double pointsFactor)
        {
            unitlength = 2;
            if (part.EndsWith("pt", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = 1.0;
                return true;
            }
            if (part.EndsWith("px", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = Pixel2Point;
                return true;
            }
            if (part.EndsWith("pc", StringComparison.OrdinalIgnoreCase)) //pica
            {
                pointsFactor = Pica2Point;
                return true;
            }
            if (part.EndsWith("in", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = Inch2Point;
                return true;
            }
            if (part.EndsWith("cm", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = Centimeter2Point;
                return true;
            }
            if (part.EndsWith("mm", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = Millimetre2Point;
                return true;
            }
            if (part.EndsWith("em", StringComparison.OrdinalIgnoreCase))
            {
                pointsFactor = EmToPoint;
                return true;
            }
            unitlength = 0;
            //default is pixels - this is 96 per inch, points are 72 per inch
            pointsFactor = Pixel2Point;
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
                        //else if (HTMLParserSettings.DefaultEscapedHTMLEntities.TryGetValue(entity, out found))
                        //{
                        //    buffer.Append(found);
                        //    src.MoveNext();
                        //}
                    }
                }

                ampersandPos = value.IndexOf(HTMLEntityStartMarker, src.Offset);
            }

            if (src.Offset < src.Length)
                buffer.Append(src.Substring(src.Length - src.Offset));

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

    #endregion


    #region public abstract class CSSStyleAttributeParser<T> : CSSStyleValueParser

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
            if (null == styleAttr)
                throw new ArgumentNullException("styleAttr");

            _styleAttr = styleAttr;
        }


        protected void SetValue(Style onStyle, T value)
        {
            onStyle.SetValue(_styleAttr, value);
        }


    }

    #endregion



    // letter spacing

    #region public class CSSLetterSpacingParser : CSSStyleValueParser

    

    #endregion

    // word spacing

    #region public class CSSWordSpacingParser : CSSStyleValueParser

    

    #endregion

    // display

    #region public class CSSDisplayParser : CSSEnumStyleParser<Text.TextDecoration>

    

    #endregion

    // overflow

    #region public class CSSOverflowActionParser : CSSEnumStyleParser<OverflowAction>

    

    #endregion

    // white-space


    #region public class CSSWhiteSpaceParser : CSSStyleValueParser

    

    #endregion

    // position mode

    


    // list-style-type


    #region public class CSSListStyleTypeParser : CSSEnumStyleParser<Text.TextDecoration>

    

    #endregion

    // list style

    #region public class CSSListStyleParser : CSSStyleValueParser

    public class CSSListStyleParser : CSSStyleValueParser
    {
        
        public CSSListStyleParser()
            : base(CSSStyleItems.ListStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;
            ListNumberingGroupStyle type;
            if (!reader.ReadNextValue()
                || !CSSListStyleTypeParser.TryGetListTypeEnum(reader.CurrentTextValue, out type))
                result = false;

            else
            {
                onStyle.SetValue(StyleKeys.ListNumberStyleKey, type);
                result = true;
            }

            //make sure we read to the end of the style value
            while (reader.ReadNextValue())
                ;

            return result;

        }
    }

    #endregion

    // column-breaks

    #region public class CSSColumnBreakInsideParser : CSSStyleValueParser

    public class CSSColumnBreakInsideParser : CSSStyleValueParser
    {

        public CSSColumnBreakInsideParser()
            : base(CSSStyleItems.BreakInside)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;

            else if (reader.CurrentTextValue == "auto")
            {
                onStyle.SetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Any);
                result = true;
            }
            else if (reader.CurrentTextValue == "avoid")
            {
                onStyle.SetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Never);
                onStyle.SetValue(StyleKeys.OverflowActionKey, OverflowAction.None);
                result = true;
            }
            else
            {
                result = false;
            }


            return result;

        }
    }

    #endregion

    #region public class CSSColumnBreakBeforeParser : CSSStyleValueParser

    public class CSSColumnBreakBeforeParser : CSSStyleValueParser
    {

        public CSSColumnBreakBeforeParser()
            : base(CSSStyleItems.BreakBefore)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                switch (reader.CurrentTextValue)
                {
                    case ("always"):
                    case ("left"):
                    case ("right"):
                        onStyle.SetValue(StyleKeys.ColumnBreakBeforeKey, true);
                        result = true;
                        break;
                    case ("avoid"):
                        onStyle.SetValue(StyleKeys.ColumnBreakBeforeKey, false);
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
            }


            return result;

        }
    }

    #endregion

    #region public class CSSColumnBreakAfterParser : CSSStyleValueParser

    public class CSSColumnBreakAfterParser : CSSStyleValueParser
    {

        public CSSColumnBreakAfterParser()
            : base(CSSStyleItems.BreakAfter)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                switch (reader.CurrentTextValue)
                {
                    case ("always"):
                    case ("left"):
                    case ("right"):
                        onStyle.SetValue(StyleKeys.ColumnBreakAfterKey, true);
                        result = true;
                        break;
                    case ("avoid"):
                        onStyle.SetValue(StyleKeys.ColumnBreakAfterKey, false);
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
            }


            return result;

        }
    }

    #endregion

    // page-breaks

    #region public class CSSPageBreakInsideParser : CSSStyleValueParser

    public class CSSPageBreakInsideParser : CSSStyleValueParser
    {

        public CSSPageBreakInsideParser()
            : base(CSSStyleItems.PageBreakInside)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            
            else if (reader.CurrentTextValue == "auto")
            {
                onStyle.SetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Any);
                result = true;
            }
            else if (reader.CurrentTextValue == "avoid")
            {
                onStyle.SetValue(StyleKeys.OverflowSplitKey, OverflowSplit.Never);
                onStyle.SetValue(StyleKeys.OverflowActionKey, OverflowAction.None);
                result = true;
            }
            else
            {
                result = false;
            }

           
            return result;

        }
    }

    #endregion

    #region public class CSSPageBreakBeforeParser : CSSStyleValueParser

    public class CSSPageBreakBeforeParser : CSSStyleValueParser
    {

        public CSSPageBreakBeforeParser()
            : base(CSSStyleItems.PageBreakBefore)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                switch (reader.CurrentTextValue)
                {
                    case ("always"):
                    case ("left"):
                    case ("right"):
                        onStyle.SetValue(StyleKeys.PageBreakBeforeKey, true);
                        result = true;
                        break;
                    case ("avoid"):
                        onStyle.SetValue(StyleKeys.PageBreakBeforeKey, false);
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
            }


            return result;

        }
    }

    #endregion

    #region public class CSSPageBreakAfterParser : CSSStyleValueParser

    public class CSSPageBreakAfterParser : CSSStyleValueParser
    {

        public CSSPageBreakAfterParser()
            : base(CSSStyleItems.PageBreakAfter)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result;

            if (!reader.ReadNextValue())
                result = false;
            else
            {
                switch (reader.CurrentTextValue)
                {
                    case ("always"):
                    case ("left"):
                    case ("right"):
                        onStyle.SetValue(StyleKeys.PageBreakAfterKey, true);
                        result = true;
                        break;
                    case ("avoid"):
                        onStyle.SetValue(StyleKeys.PageBreakAfterKey, false);
                        result = true;
                        break;
                    default:
                        result = false;
                        break;
                }
            }


            return result;

        }
    }

    #endregion


    //page-names

    public class CSSPageNameParser : CSSStyleValueParser
    {
        public CSSPageNameParser(): base(CSSStyleItems.PageGroupName)
        {
        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            if (reader.MoveToNextValue() && !string.IsNullOrEmpty(reader.CurrentTextValue))
            {
                style.PageStyle.PageNameGroup = reader.CurrentTextValue;
                return true;
            }
            else
                return false;
        }
    }

    public class CSSPageSizeParser : CSSStyleValueParser
    {
        public CSSPageSizeParser() : base(CSSStyleItems.PageSize)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            if(reader.MoveToNextValue())
            {
                PaperSize paper;
                PDFUnit width;
                PaperOrientation orient;

                if (Enum.TryParse<PaperSize>(reader.CurrentTextValue, out paper))
                {
                    style.PageStyle.PaperSize = paper;

                    if (reader.MoveToNextValue() && Enum.TryParse<PaperOrientation>(reader.CurrentTextValue, true, out orient))
                        style.PageStyle.PaperOrientation = orient;

                    return true;
                }
                else if(Enum.TryParse<PaperOrientation>(reader.CurrentTextValue, true, out orient))
                {
                    style.PageStyle.PaperOrientation = orient;

                    return true;
                }
                else if(ParseCSSUnit(reader.CurrentTextValue, out width))
                {
                    PDFUnit height;
                    if (!reader.MoveToNextValue() || !ParseCSSUnit(reader.CurrentTextValue, out height))
                        height = width;

                    style.PageStyle.Width = width;
                    style.PageStyle.Height = height;

                    return true;
                }
            }
            return false;
        }
    }

    public class CSSStrokeOpacityParser: CSSOpacityParser
    {
        public CSSStrokeOpacityParser(): base(CSSStyleItems.StrokeOpacity, StyleKeys.StrokeOpacityKey)
        { }
    }

    public class CSSStrokeWidthParser : CSSUnitStyleParser
    {
        public CSSStrokeWidthParser() : base(CSSStyleItems.StrokeWidth, StyleKeys.StrokeWidthKey)
        { }
    }

    public class CSSStrokeColorParser : CSSColorStyleParser
    {
        public CSSStrokeColorParser()
            : base(CSSStyleItems.StrokeColor, StyleKeys.StrokeColorKey, StyleKeys.StrokeOpacityKey)
        {

        }
    }


    public class CSSStrokeDashParser : CSSStyleValueParser
    {

        public CSSStrokeDashParser()
            : base(CSSStyleItems.StrokeDash)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            var all = new List<int>(1);
            while (reader.ReadNextValue())
            {
                int parsed;
                if (int.TryParse(reader.CurrentTextValue, out parsed))
                    all.Add(parsed);
            }

            if (all.Count > 0)
            {
                style.SetValue(StyleKeys.StrokeDashKey, new PDFDash(all.ToArray(), 0));
                return true;
            }
            else
                return false;
        }
    }


    public class CSSStrokeLineCapParser : CSSStyleValueParser
    {
        public CSSStrokeLineCapParser(): base(CSSStyleItems.StrokeLineCap)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            var key = StyleKeys.StrokeEndingKey;
            bool result = false;
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue.ToLower();
                switch (val)
                {
                    case ("butt"):
                        style.SetValue(key, LineCaps.Butt);
                        result = true;
                        break;
                    case ("round"):
                        style.SetValue(key, LineCaps.Round);
                        result = true;
                        break;
                    case ("square"):
                        style.SetValue(key, LineCaps.Square);
                        result = true;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }

    public class CSSStrokeLineJoinParser : CSSStyleValueParser
    {
        public CSSStrokeLineJoinParser() : base(CSSStyleItems.StrokeLineCap)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            var key = StyleKeys.StrokeJoinKey;
            var result = false;
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue.ToLower();
                switch (val)
                {
                    case ("bevel"):
                        style.SetValue(key, LineJoin.Bevel);
                        result = true;
                        break;
                    case ("round"):
                        style.SetValue(key, LineJoin.Round);
                        result = true;
                        break;
                    case ("mitre"):
                        style.SetValue(key, LineJoin.Mitre);
                        result = true;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }


}
