using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Html;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Windows.Markup;
using Scryber.Text;
using System.Security.Cryptography;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using Scryber.Resources;
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

    //
    // generic sub classes
    //

    #region public abstract class CSSStyleAttributeParser<T> : CSSStyleValueParser

    /// <summary>
    /// Abstract class that has a known 
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

    

    //Border Sides

    

    // background

    #region public class CSSBackgroundParser : CSSStyleValueParser

    

    #endregion

    #region public class CSSBackgroundColorParser : CSSStyleValueParser

    
    

    #endregion

    #region public class CSSBackgroundImageParser : CSSStyleValueParser

    

    #endregion

    #region public class CSSBackgroundSizeParser : CSSStyleValueParser

    

    #endregion


    #region public class CSSBackgroundPositionParser : CSSStyleValueParser

    

    #endregion

    #region public class CSSBackgroundPositionXParser : CSSThicknessValueParser

    public class CSSBackgroundPositionXParser : CSSThicknessValueParser
    {
        public CSSBackgroundPositionXParser()
            : base(CSSStyleItems.BackgroundPositionX, StyleKeys.BgXPosKey)
        {
        }
    }

    #endregion

    #region public class CSSBackgroundPositionYParser : CSSThicknessValueParser

    public class CSSBackgroundPositionYParser : CSSThicknessValueParser
    {
        public CSSBackgroundPositionYParser()
            : base(CSSStyleItems.BackgroundPositionY, StyleKeys.BgYPosKey)
        {
        }
    }

    #endregion

    #region public class CSSBackgroundRepeatParser : CSSEnumStyleParser<PatternRepeat>

    /// <summary>
    /// Parses and sets the components background repeat option based on the CSS names
    /// </summary>
    public class CSSBackgroundRepeatParser : CSSEnumStyleParser<PatternRepeat>
    {
        public CSSBackgroundRepeatParser()
            : base(CSSStyleItems.BackgroundRepeat, StyleKeys.BgRepeatKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PatternRepeat repeat;
            if (reader.ReadNextValue() && TryGetRepeatEnum(reader.CurrentTextValue, out repeat))
                this.SetValue(onStyle, repeat);
            else
                result = false;

            return result;
        }

        public static bool IsRepeatEnum(string value)
        {
            PatternRepeat repeat;
            return TryGetRepeatEnum(value, out repeat);
        }

        public static bool TryGetRepeatEnum(string value, out PatternRepeat repeat)
        {
            switch (value.ToLower())
            {
                case ("repeat"):
                    repeat = PatternRepeat.RepeatBoth;
                    return true;

                case ("repeat-x"):
                    repeat = PatternRepeat.RepeatX;
                    return true;

                case ("repeat-y"):
                    repeat = PatternRepeat.RepeatY;
                    return true;

                case ("no-repeat"):
                    repeat = PatternRepeat.None;
                    return true;

                default:
                    repeat = PatternRepeat.RepeatBoth;
                    return false;

            }
        }
        
    }

    #endregion

    // font

    #region public class CSSFontStyleParser : CSSStyleValueParser

    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontStyleParser : CSSStyleValueParser
    {
        public CSSFontStyleParser()
            : base(CSSStyleItems.FontStyle)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool italic = true;
            bool result = true;
            if (reader.ReadNextValue() && TryGetFontStyle(reader.CurrentTextValue, out italic))
                onStyle.SetValue(StyleKeys.FontItalicKey, italic);
            else
                result = false;

            return result;
        }

        public static bool IsFontStyle(string value)
        {
            bool italic;
            return TryGetFontStyle(value, out italic);
        }

        public static bool TryGetFontStyle(string value, out bool italic)
        {
            switch (value.ToLower())
            {
                case ("italic"):
                case ("oblique"):
                    italic = true;
                    return true;

                case ("normal"):
                    italic = false;
                    return true;

                default:
                    italic = false;
                    return false;

            }
        }

    }

    #endregion

    #region public class CSSFontWeightParser : CSSStyleValueParser

    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontWeightParser : CSSStyleValueParser
    {
        public CSSFontWeightParser()
            : base(CSSStyleItems.FontWeight)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool bold = true;
            bool result = true;
            if (reader.ReadNextValue() && TryGetFontWeight(reader.CurrentTextValue, out bold))
                onStyle.SetValue(StyleKeys.FontBoldKey, bold);
            else
                result = false;

            return result;
        }

        public static bool IsFontWeight(string value)
        {
            bool italic;
            return TryGetFontWeight(value, out italic);
        }

        public static bool TryGetFontWeight(string value, out bool bold)
        {
            switch (value.ToLower())
            {
                case ("bold"):
                case ("bolder"):
                    bold = true;
                    return true;

                case ("normal"):
                case ("lighter"):
                    bold = false;
                    return true;

                case ("100"):
                case ("200"):
                case ("300"):
                case ("400"):
                case ("500"):
                    bold = false;
                    return true;

                case ("600"):
                case ("700"):
                case ("800"):
                case ("900"):
                    bold = true;
                    return true;
                default:
                    bold = false;
                    return false;

            }
        }

    }

    #endregion

    #region public class CSSFontSizeParser : CSSStyleValueParser

    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSFontSizeParser : CSSStyleValueParser
    {
        public static readonly PDFUnit XXSmallFontSize = new PDFUnit(6.0, PageUnits.Points);
        public static readonly PDFUnit XSmallFontSize = new PDFUnit(8.0, PageUnits.Points);
        public static readonly PDFUnit SmallFontSize = new PDFUnit(10.0, PageUnits.Points);
        public static readonly PDFUnit MediumFontSize = new PDFUnit(12.0, PageUnits.Points);
        public static readonly PDFUnit LargeFontSize = new PDFUnit(16.0, PageUnits.Points);
        public static readonly PDFUnit XLargeFontSize = new PDFUnit(24.0, PageUnits.Points);
        public static readonly PDFUnit XXLargeFontSize = new PDFUnit(32.0, PageUnits.Points);
        
        public CSSFontSizeParser()
            : base(CSSStyleItems.FontSize)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PDFUnit size = PDFUnit.Zero;
            bool result = true;
            if (reader.ReadNextValue() && TryGetFontSize(reader.CurrentTextValue, out size))
                onStyle.SetValue(StyleKeys.FontSizeKey, size);
            else
                result = false;

            return result;
        }

        public static bool IsFontSize(string value)
        {
            PDFUnit size;
            return TryGetFontSize(value, out size);
        }

        public static bool TryGetFontSize(string value, out PDFUnit size)
        {
            switch (value.ToLower())
            {
                case ("medium"):
                    size = MediumFontSize;
                    return true;

                case ("small"):
                    size = SmallFontSize;
                    return true;

                case ("x-small"):
                    size = XSmallFontSize;
                    return true;

                case ("xx-small"):
                    size = XXSmallFontSize;
                    return true;

                case ("large"):
                    size = LargeFontSize;
                    return true;

                case ("x-large"):
                    size = XLargeFontSize;
                    return true;

                case ("xx-large"):
                    size = XXLargeFontSize;
                    return true;

                case ("larger"):
                case("smaller"):
                    size = PDFUnit.Zero;
                    return false;

                default:

                    if (CSSStyleValueParser.ParseCSSUnit(value, out size))
                        return true;
                    else
                        return false;

            }
        }

    }

    #endregion

    #region public class CSSFontLineHeightParser : CSSUnitStyleParser

    public class CSSFontLineHeightParser : CSSUnitStyleParser
    {
        public CSSFontLineHeightParser()
            : base(CSSStyleItems.FontLineHeight, StyleKeys.TextLeadingKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            double proportional;
            PDFUnit absolute;

            if (reader.ReadNextValue())
            {
                // special case of a single figure - this is proportional to the font size.
                // so if we have a font size, then we can do it.
                if (double.TryParse(reader.CurrentTextValue, out proportional))
                {
                    StyleValue<PDFUnit> fsize;
                    if (onStyle.TryGetValue(StyleKeys.FontSizeKey, out fsize))
                    {
                        onStyle.Text.Leading = fsize.Value(onStyle) * proportional;
                        return true;
                    }
                }

                //otherwise treat as an absolute unit.
                else if (ParseCSSUnit(reader.CurrentTextValue, out absolute))
                {
                    onStyle.Text.Leading = absolute;
                    return true;
                }
            }
            return false;

        }
    }

    #endregion

    #region public class CSSFontFamilyParser : CSSStyleAttributeParser<string>

    public class CSSFontFamilyParser : CSSStyleValueParser
    {
        public CSSFontFamilyParser()
            : base(CSSStyleItems.FontFamily)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            PDFFontSelector root = null;
            PDFFontSelector curr = null;

            while(reader.ReadNextValue())
            {
                string fontfamily = reader.CurrentTextValue.Trim();
                PDFFontSelector found;
                if(TryGetActualFontFamily(fontfamily, out found))
                {
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
                return false;

        }

        public static bool TryGetActualFontFamily(string fontfamily, out PDFFontSelector found)
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

            found = new PDFFontSelector(fontfamily);
            return true;

        }
    }

    #endregion

    public class CSSFontSourceParser : CSSStyleValueParser
    {
        public CSSFontSourceParser(): base(CSSStyleItems.FontSource)
        { }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            PDFFontSource root = null;
            PDFFontSource curr = null;
            StringBuilder all = new StringBuilder();

            while (reader.ReadNextValue(',',';', true))
            {
                string src = reader.CurrentTextValue.Trim();
                PDFFontSource found;
                if (TryGetFontSource(src, out found))
                {
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

        private bool TryGetFontSource(string value, out PDFFontSource found)
        {
            if (PDFFontSource.TryParseOneValue(value, out found))
                return true;
            else
                return false;
        }
    }


    #region public class CSSFontParser : CSSStyleValueParser

    public class CSSFontParser : CSSStyleValueParser
    {
        public static readonly PDFFont CaptionFont = new PDFFont("Helvetica", 12, Drawing.FontStyle.Bold);
        public static readonly PDFFont IconFont = new PDFFont("Helvetica", 8, Drawing.FontStyle.Bold);
        public static readonly PDFFont MenuFont = new PDFFont("Times", 10, Drawing.FontStyle.Regular);
        public static readonly PDFFont MessageBoxFont = new PDFFont("Times", 10, Drawing.FontStyle.Bold);
        public static readonly PDFFont SmallCaptionFont = new PDFFont("Helvetica", 8, Drawing.FontStyle.Italic);
        public static readonly PDFFont StatusBarFont = new PDFFont("Courier", 10, Drawing.FontStyle.Bold);

        public CSSFontParser()
            : base(CSSStyleItems.Font)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (!reader.ReadNextValue())
                return result;


            //compound values - caption|icon|menu|message-box|small-caption|status-bar
            // invalid values - initial|inherit
            switch (reader.CurrentTextValue.ToLower())
            {
                case ("caption"):
                    ApplyFont(onStyle, CaptionFont);
                    return true;

                case ("icon"):
                    ApplyFont(onStyle, IconFont);
                    return true;

                case ("menu"):
                    ApplyFont(onStyle, MenuFont);
                    return true;

                case ("message-box"):
                    ApplyFont(onStyle, MessageBoxFont);
                    return true;

                case ("small-caption"):
                    ApplyFont(onStyle, SmallCaptionFont);
                    return true;

                case ("status-bar"):
                    ApplyFont(onStyle, StatusBarFont);
                    return true;

                default:
                    //Just fall back to parsing each individual item
                    break;
            }

            result = true;

            //discreet values - font-style font-variant font-weight font-size/line-height font-family
            bool italic;
            bool bold;
            PDFUnit fsize, lineheight;
            double relativeLeading;
            

            if (CSSFontStyleParser.TryGetFontStyle(reader.CurrentTextValue, out italic))
            {
                onStyle.Font.FontItalic = italic;
                if (!reader.MoveToNextValue())
                    return result;
            }

            if (IsDefinedFontVariant(reader.CurrentTextValue))
            {
                //We dont support variant but we should honour it being there.
                if (!reader.MoveToNextValue())
                    return result;
            }
            if (CSSFontWeightParser.TryGetFontWeight(reader.CurrentTextValue, out bold))
            {
                onStyle.Font.FontBold = bold;
                if (!reader.MoveToNextValue())
                    return result;
            }

            //font-size/line height are special and will return the both values from the reader if both are present
            //other wise it is just the font-size
            if (reader.CurrentTextValue.IndexOf('/') > 0)
            {
                // we have both
                string part = reader.CurrentTextValue.Substring(0, reader.CurrentTextValue.IndexOf('/'));
                if (ParseCSSUnit(part, out fsize))
                    onStyle.Font.FontSize = fsize;
                else
                    result = false;

                part = reader.CurrentTextValue.Substring(reader.CurrentTextValue.IndexOf('/') + 1);

                //if we have a simple double value then it is relative leading based on font size.
                if (double.TryParse(part, out relativeLeading))
                    onStyle.Text.Leading = onStyle.Font.FontSize * relativeLeading;
                else if (ParseCSSUnit(part, out lineheight))
                    onStyle.Text.Leading = lineheight;
                else
                    result = false;

                if (!reader.MoveToNextValue())
                    return result;
            }
            else if (ParseCSSUnit(reader.CurrentTextValue, out fsize))
            {
                onStyle.Font.FontSize = fsize;
                if (!reader.MoveToNextValue())
                    return result;
            }

            //last one is multiple font families
            bool foundFamily = false;
            PDFFontSelector root = null;

            do
            {
                PDFFontSelector selector;
                
                PDFFontSelector curr = null;

                if (CSSFontFamilyParser.TryGetActualFontFamily(reader.CurrentTextValue, out selector))
                {
                    if (null == root)
                        root = selector;

                    if (null == curr)
                        curr = selector;
                    else
                    {
                        curr.Next = selector;
                        curr = selector;
                    }
                    foundFamily = true;
                }

            }
            while (reader.MoveToNextValue());

            if(foundFamily)
            {
                onStyle.SetValue(StyleKeys.FontFamilyKey, root);
            }
            else
                result = false;

            return result;
        }

        private void ApplyFont(Style onStyle, PDFFont font)
        {
            onStyle.SetValue(StyleKeys.FontFamilyKey, font.Selector);
            onStyle.SetValue(StyleKeys.FontSizeKey, font.Size);
            onStyle.SetValue(StyleKeys.FontBoldKey, (font.FontStyle & Drawing.FontStyle.Bold) > 0);
            onStyle.SetValue(StyleKeys.FontItalicKey, (font.FontStyle & Drawing.FontStyle.Italic) > 0);
        }



        private bool IsDefinedFontVariant(string value)
        {
            // font-variant: normal|small-caps|initial|inherit;
            switch (value.ToLower())
            {
                case("normal"):
                case("small-caps"):
                case("initial"):
                case("inherit"):
                    return true;
                default:
                    return false;
            }
        }
    }

    #endregion


    // thickness

    #region public abstract class CSSThicknessValueParser : CSSUnitStyleParser

    /// <summary>
    /// Generic CSSUnit Parser for thicknesses - supports the 'Auto' option as well as units.
    /// </summary>
    public abstract class CSSThicknessValueParser : CSSUnitStyleParser
    {
        protected PDFUnit AutoValue { get; set; }
        public CSSThicknessValueParser(string cssName, PDFStyleKey<PDFUnit> pdfAttr)
            : base(cssName, pdfAttr)
        {
            AutoValue = PDFUnit.Zero;
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if(reader.ReadNextValue())
            {
                PDFUnit found;

                if (ParseThicknessValue(reader, this.AutoValue, out found))
                {
                    this.SetValue(onStyle, found);
                    return true;
                }
                    
            }
            return false;
            
        }

        public static bool ParseThicknessValue(CSSStyleItemReader reader, PDFUnit auto, out PDFUnit found)
        {
            if (reader.CurrentTextValue.Equals("auto", StringComparison.OrdinalIgnoreCase))
            {
                found = auto;
                return true;
            }
            else if (ParseCSSUnit(reader.CurrentTextValue, out found))
            {
                return true;
            }

            return false;
        }
    }

    #endregion

    #region public abstract class CSSThicknessAllParser : CSSStyleValueParser

    public abstract class CSSThicknessAllParser : CSSStyleValueParser
    {
        private PDFStyleKey<PDFUnit> _all;
        private PDFStyleKey<PDFUnit> _left;
        private PDFStyleKey<PDFUnit> _right;
        private PDFStyleKey<PDFUnit> _top;
        private PDFStyleKey<PDFUnit> _bottom;

        public CSSThicknessAllParser(string cssAttr, PDFStyleKey<PDFUnit> all, PDFStyleKey<PDFUnit> left, PDFStyleKey<PDFUnit> top, PDFStyleKey<PDFUnit> right, PDFStyleKey<PDFUnit> bottom)
            : base(cssAttr)
        {
            _all = all;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
        }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int count = 0;
            PDFUnit[] all = new PDFUnit[4];

            while (reader.ReadNextValue())
            {
                PDFUnit found;
                if (CSSThicknessValueParser.ParseThicknessValue(reader, PDFUnit.Zero, out found))
                {
                    all[count] = found;
                    count++;

                    if (count == 4)
                        break;
                }
            }

            bool result = false;

            if (count == 1)
            {
                onStyle.SetValue(_all, all[0]);
                onStyle.SetValue(_top, all[0]);
                onStyle.SetValue(_bottom, all[0]);

                onStyle.SetValue(_left, all[0]);
                onStyle.SetValue(_right, all[0]);
                result = true;
            }
            else if(count == 2)
            {
                // first top and bottom then left and right
                onStyle.SetValue(_top, all[0]);
                onStyle.SetValue(_bottom, all[0]);

                onStyle.SetValue(_left, all[1]);
                onStyle.SetValue(_right, all[1]);

                result = true;
            }
            else if(count == 3)
            {
                // top then left and right and finally bottom
                onStyle.SetValue(_top, all[0]);
                
                onStyle.SetValue(_left, all[1]);
                onStyle.SetValue(_right, all[1]);

                onStyle.SetValue(_bottom, all[2]);

                result = true;
            }
            else if (count == 4)
            {
                // all 4 individually top then right and then bottom and finally left
                onStyle.SetValue(_top, all[0]);

                onStyle.SetValue(_right, all[1]);

                onStyle.SetValue(_bottom, all[2]);

                onStyle.SetValue(_left, all[3]);
                
                result = true;
            }

            return result;
        }
    }

    #endregion

    // margins

    #region public class CSSMarginsLeftParser : CSSThicknessValueParser

    public class CSSMarginsLeftParser : CSSThicknessValueParser
    {
        public CSSMarginsLeftParser()
            : base(CSSStyleItems.MarginsLeft, StyleKeys.MarginsLeftKey)
        {
        }
    }

    #endregion

    #region public class CSSMarginsRightParser : CSSThicknessValueParser

    public class CSSMarginsRightParser : CSSThicknessValueParser
    {
        public CSSMarginsRightParser()
            : base(CSSStyleItems.MarginsRight, StyleKeys.MarginsRightKey)
        {
        }
    }

    #endregion

    #region public class CSSMarginsBottomParser : CSSThicknessValueParser

    public class CSSMarginsBottomParser : CSSThicknessValueParser
    {
        public CSSMarginsBottomParser()
            : base(CSSStyleItems.MarginsBottom, StyleKeys.MarginsBottomKey)
        {
        }
    }

    #endregion

    #region public class CSSMarginsTopParser : CSSThicknessValueParser

    public class CSSMarginsTopParser : CSSThicknessValueParser
    {
        public CSSMarginsTopParser()
            : base(CSSStyleItems.MarginsTop, StyleKeys.MarginsTopKey)
        {
        }
    }

    #endregion

    #region public class CSSMarginsParser : CSSThicknessAllParser

    public class CSSMarginsParser : CSSThicknessAllParser
    {
        public CSSMarginsParser()
            : base(CSSStyleItems.Margins, StyleKeys.MarginsAllKey, StyleKeys.MarginsLeftKey, StyleKeys.MarginsTopKey, StyleKeys.MarginsRightKey, StyleKeys.MarginsBottomKey)
        {

        }
    }

    #endregion

    // padding

    #region public class CSSPaddingLeftParser : CSSThicknessValueParser

    public class CSSPaddingLeftParser : CSSThicknessValueParser
    {
        public CSSPaddingLeftParser()
            : base(CSSStyleItems.PaddingLeft, StyleKeys.PaddingLeftKey)
        {
        }
    }

    #endregion

    #region public class CSSPaddingRightParser : CSSThicknessValueParser

    public class CSSPaddingRightParser : CSSThicknessValueParser
    {
        public CSSPaddingRightParser()
            : base(CSSStyleItems.PaddingRight, StyleKeys.PaddingRightKey)
        {
        }
    }

    #endregion

    #region public class CSSPaddingBottomParser : CSSThicknessValueParser

    public class CSSPaddingBottomParser : CSSThicknessValueParser
    {
        public CSSPaddingBottomParser()
            : base(CSSStyleItems.PaddingBottom, StyleKeys.PaddingBottomKey)
        {
        }
    }

    #endregion

    #region public class CSSPaddingTopParser : CSSThicknessValueParser

    public class CSSPaddingTopParser : CSSThicknessValueParser
    {
        public CSSPaddingTopParser()
            : base(CSSStyleItems.PaddingTop, StyleKeys.PaddingTopKey)
        {
        }
    }

    #endregion

    #region public class CSSPaddingParser : CSSThicknessAllParser

    public class CSSPaddingParser : CSSThicknessAllParser
    {
        public CSSPaddingParser()
            : base(CSSStyleItems.Padding, StyleKeys.PaddingAllKey, StyleKeys.PaddingLeftKey, StyleKeys.PaddingTopKey, StyleKeys.PaddingRightKey, StyleKeys.PaddingBottomKey)
        {

        }
    }

    #endregion

    // fill

    #region public class CSSOpacityParser : CSSStyleAttributeParser<double>

    public class CSSFillOpacityParser : CSSOpacityParser
    {
        public CSSFillOpacityParser()
            : base(CSSStyleItems.FillOpacity, StyleKeys.FillOpacityKey)
        {
        }
    }

    public class CSSOpacityParser : CSSStyleAttributeParser<double>
    {
        public CSSOpacityParser()
            : this(CSSStyleItems.Opacity, StyleKeys.FillOpacityKey)
        {
        }

        public CSSOpacityParser(string css, PDFStyleKey<double> key)
            : base(css, key)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            double number;

            if (reader.ReadNextValue() && ParseDouble(reader.CurrentTextValue, out number) && number >= 0.0 && number <= 1.0)
            {
                this.SetValue(onStyle, number);
                return true;
            }
            return false;
        }
    }

    #endregion

    #region public class CSSColourParser : CSSColorStyleParser

    public class CSSFillColourParser : CSSColorStyleParser
    {
        public CSSFillColourParser()
            : this(CSSStyleItems.FillColor, StyleKeys.FillColorKey, StyleKeys.FillOpacityKey)
        {
        }

        public CSSFillColourParser(string css, PDFStyleKey<PDFColor> key, PDFStyleKey<double> opacity)
            : base(css, key, opacity)
        { }

    }

    #endregion


    #region public class CSSFillParser : CSSColorStyleParser

    public class CSSFillParser : CSSColorStyleParser
    {
        public CSSFillParser()
            : this(CSSStyleItems.FillColor, StyleKeys.FillColorKey, StyleKeys.FillOpacityKey)
        {
        }

        public CSSFillParser(string css, PDFStyleKey<PDFColor> key, PDFStyleKey<double> opacity)
            : base(css, key, opacity)
        { }


        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {

            bool result = false;
            PDFColor color;

            if (reader.ReadNextValue() && !string.IsNullOrEmpty(reader.CurrentTextValue))
            {
                var val = reader.CurrentTextValue;
                if(val.StartsWith("url("))
                {
                    val = val.Substring(4);
                    if (val.EndsWith(")"))
                        val = val.Substring(0, val.Length - 1);

                    if (val.StartsWith("#"))
                    {
                        throw new NotSupportedException("Setting background fills to patterns is not currently supported");
                    }
                    else
                        onStyle.SetValue(StyleKeys.FillImgSrcKey, val);
                }
                else if (ParseCSSColor(val, out color, out double? opacity))
                {
                    onStyle.SetValue(this.StyleAttribute, color);

                    if (opacity.HasValue && null != this.OpacityStyleKey)
                        onStyle.SetValue(this.OpacityStyleKey, opacity.Value);

                    result = true;
                }
            }
            return result;
        }
    }

    #endregion

    // columns

    #region public class CSSColumnCountParser : CSSStyleAttributeParser<int>

    public class CSSColumnCountParser : CSSStyleAttributeParser<int>
    {
        public CSSColumnCountParser()
            : base(CSSStyleItems.ColumnCount, StyleKeys.ColumnCountKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int number;

            if (reader.ReadNextValue())
            {
                if (ParseInteger(reader.CurrentTextValue, out number))
                    this.SetValue(onStyle, number);
                return true;
            }
            return false;
        }
    }

    #endregion

    public class CSSColumnWidthParser : CSSStyleAttributeParser<PDFColumnWidths>
    {
        public CSSColumnWidthParser()
            : base(CSSStyleItems.ColumnWidths, StyleKeys.ColumnWidthKey)
        {

        }

        protected override bool DoSetStyleValue(Style style, CSSStyleItemReader reader)
        {
            string all = string.Empty;
            while(reader.ReadNextValue())
            {
                if (all.Length > 0)
                    all += " ";
                all += reader.CurrentTextValue;
            }
            if (!string.IsNullOrEmpty(all))
            {
                PDFColumnWidths widths = PDFColumnWidths.Parse(all);
                this.SetValue(style, widths);
                return true;
            }
            else
                return false;
        }
    }

    #region public class CSSColumnGapParser : CSSUnitStyleParser

    public class CSSColumnGapParser : CSSUnitStyleParser
    {
        public CSSColumnGapParser()
            : base(CSSStyleItems.ColumnGap, StyleKeys.ColumnAlleyKey)
        {
        }

        
    }

    #endregion

    #region public class CSSColumnSpanParser : CSSStyleAttributeParser<int>

    public class CSSColumnSpanParser : CSSStyleAttributeParser<int>
    {
        public CSSColumnSpanParser()
            : base(CSSStyleItems.ColumnSpan, StyleKeys.TableCellColumnSpanKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            int number;

            if (reader.ReadNextValue() && ParseInteger(reader.CurrentTextValue, out number) && number >= 1 && number < 100)
            {
                this.SetValue(onStyle, number);
                return true;
            }
            return false;
        }
    }

    #endregion


    // position

    #region public class CSSLeftParser : CSSUnitStyleParser

    public class CSSLeftParser : CSSUnitStyleParser
    {
        public CSSLeftParser()
            : base(CSSStyleItems.Left, StyleKeys.PositionXKey)
        {
        }

    }

    #endregion

    #region public class CSSTopParser : CSSUnitStyleParser

    public class CSSTopParser : CSSUnitStyleParser
    {
        public CSSTopParser()
            : base(CSSStyleItems.Top, StyleKeys.PositionYKey)
        {
        }

    }

    #endregion

    #region public class CSSPositionFloatParser : CSSEnumStyleParser<FloatMode>


    public class CSSPositionFloatParser : CSSEnumStyleParser<FloatMode>
    {

        public CSSPositionFloatParser(): base(CSSStyleItems.Float, StyleKeys.PositionFloat)
        { }
    }


    #endregion

    // size

    #region public class CSSWidthParser : CSSUnitStyleParser

    public class CSSWidthParser : CSSUnitStyleParser
    {
        public CSSWidthParser()
            : base(CSSStyleItems.Width, StyleKeys.SizeWidthKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = false;
            if (reader.ReadNextValue())
            {
                string value = reader.CurrentTextValue;

                PDFUnit parsed;
                if(value == "100%")
                {
                    onStyle.SetValue(StyleKeys.SizeFullWidthKey, true);
                    result = true;
                }
                else if (ParseCSSUnit(value, out parsed))
                {
                    onStyle.SetValue(this.StyleAttribute, parsed);
                    result = true;
                }
            }
            return result;
        }

    }

    #endregion

    #region public class CSSHeightParser : CSSUnitStyleParser

    public class CSSHeightParser : CSSUnitStyleParser
    {
        public CSSHeightParser()
            : base(CSSStyleItems.Height, StyleKeys.SizeHeightKey)
        {
        }

    }

    #endregion

    // min-size

    #region public class CSSMinWidthParser : CSSUnitStyleParser

    public class CSSMinWidthParser : CSSUnitStyleParser
    {
        public CSSMinWidthParser()
            : base(CSSStyleItems.MinimumWidth, StyleKeys.SizeMinimumWidthKey)
        {
        }

    }

    #endregion

    #region public class CSSMinHeightParser : CSSUnitStyleParser

    public class CSSMinHeightParser : CSSUnitStyleParser
    {
        public CSSMinHeightParser()
            : base(CSSStyleItems.MinimumHeight, StyleKeys.SizeMinimumHeightKey)
        {
        }

    }

    #endregion

    // max size

    #region public class CSSMaxWidthParser : CSSUnitStyleParser

    public class CSSMaxWidthParser : CSSUnitStyleParser
    {
        public CSSMaxWidthParser()
            : base(CSSStyleItems.MaximumWidth, StyleKeys.SizeMaximumWidthKey)
        {
        }

    }

    #endregion

    #region public class CSSMaxHeightParser : CSSUnitStyleParser

    public class CSSMaxHeightParser : CSSUnitStyleParser
    {
        public CSSMaxHeightParser()
            : base(CSSStyleItems.MaximumHeight, StyleKeys.SizeMaximumHeightKey)
        {
        }

    }

    #endregion

    //alignment

    #region public class CSSTextAlignParser : CSSStyleValueParser

    public class CSSTextAlignParser : CSSStyleValueParser
    {
        public CSSTextAlignParser()
            : base(CSSStyleItems.TextAlign)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            HorizontalAlignment align;
            if (!reader.ReadNextValue())
            {
                align = HorizontalAlignment.Left;
                success = false;
            }
            else
            {
                switch (reader.CurrentTextValue.ToLower())
                {
                    case "left":
                        align = HorizontalAlignment.Left;
                        break;
                    case "right":
                        align = HorizontalAlignment.Right;
                        break;
                    case "center":
                        align = HorizontalAlignment.Center;
                        break;
                    case "justify":
                        align = HorizontalAlignment.Justified;
                        break;
                    default:
                        align = HorizontalAlignment.Left;
                        success = false;
                        break;
                }
            }
            if (success)
                onStyle.SetValue(StyleKeys.PositionHAlignKey, align);

            return success;
        }
    }

    #endregion

    #region public class CSSVerticalAlignParser : CSSStyleValueParser

    public class CSSVerticalAlignParser : CSSStyleValueParser
    {
        public CSSVerticalAlignParser()
            : base(CSSStyleItems.VerticalAlign)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            VerticalAlignment align;

            if (!reader.ReadNextValue())
            {
                align = VerticalAlignment.Top;
                success = false;
            }
            else
            {
                switch (reader.CurrentTextValue.ToLower())
                {
                    case "top":
                        align = VerticalAlignment.Top;
                        break;
                    case "bottom":
                        align = VerticalAlignment.Bottom;
                        break;
                    case "middle":
                        align = VerticalAlignment.Middle;
                        break;
                    default:
                        align = VerticalAlignment.Top;
                        success = false;
                        break;
                }
            }
            if (success)
                onStyle.SetValue(StyleKeys.PositionVAlignKey, align);

            return success;
        }
    }

    #endregion

    // text decoration

    #region public class CSSTextDecorationParser : CSSEnumStyleParser<Text.TextDecoration>

    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSTextDecorationParser : CSSEnumStyleParser<Text.TextDecoration>
    {
        public CSSTextDecorationParser()
            : base(CSSStyleItems.TextDecoration, StyleKeys.TextDecorationKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            Text.TextDecoration decor;
            TextDecoration final = TextDecoration.None;

            while (reader.ReadNextValue() && TryGetOneDecoration(reader.CurrentTextValue, out decor))
            {
                final |= decor;
                result = true;
            }

            if (result)
                onStyle.SetValue(StyleKeys.TextDecorationKey, final);
            
            return result;
        }

        public static bool IsDecorationEnum(string value)
        {
            Text.TextDecoration decor;
            return TryGetOneDecoration(value, out decor);
        }

        

        private static bool TryGetOneDecoration(string value, out TextDecoration decoration)
        {
            switch (value.ToLower())
            {
                case ("underline"):
                    decoration = Text.TextDecoration.Underline;
                    return true;

                case ("overline"):
                    decoration = Text.TextDecoration.Overline;
                    return true;

                case ("line-through"):
                    decoration = Text.TextDecoration.StrikeThrough;
                    return true;

                case ("none"):
                    decoration = Text.TextDecoration.None;
                    return true;

                default:
                    decoration = Text.TextDecoration.None;
                    return false;

            }
        }

    }

    #endregion

    // letter spacing

    #region public class CSSLetterSpacingParser : CSSStyleValueParser

    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSLetterSpacingParser : CSSStyleValueParser
    {
        public static readonly PDFUnit NormalSpacing = PDFUnit.Zero;

        public CSSLetterSpacingParser()
            : base(CSSStyleItems.LetterSpacing)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PDFUnit size = PDFUnit.Zero;
            bool result = true;
            if (reader.ReadNextValue() && TryGetLetterSpacing(reader.CurrentTextValue, out size))
                onStyle.SetValue(StyleKeys.TextCharSpacingKey, size);
            else
                result = false;

            return result;
        }


        public static bool TryGetLetterSpacing(string value, out PDFUnit size)
        {
            switch (value.ToLower())
            {
                case ("normal"):
                    size = NormalSpacing;
                    return true;

                default:

                    if (CSSStyleValueParser.ParseCSSUnit(value, out size))
                        return true;
                    else
                        return false;

            }
        }

    }

    #endregion

    // word spacing

    #region public class CSSWordSpacingParser : CSSStyleValueParser

    /// <summary>
    /// Parses and sets the components italic option based on the CSS names
    /// </summary>
    public class CSSWordSpacingParser : CSSStyleValueParser
    {
        public static readonly PDFUnit NormalSpacing = PDFUnit.Zero;

        public CSSWordSpacingParser()
            : base(CSSStyleItems.WordSpacing)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            PDFUnit size = PDFUnit.Zero;
            bool result = true;
            if (reader.ReadNextValue() && TryGetWordSpacing(reader.CurrentTextValue, out size))
                onStyle.SetValue(StyleKeys.TextWordSpacingKey, size);
            else
                result = false;

            return result;
        }


        public static bool TryGetWordSpacing(string value, out PDFUnit size)
        {
            switch (value.ToLower())
            {
                case ("normal"):
                    size = NormalSpacing;
                    return true;

                default:

                    if (CSSStyleValueParser.ParseCSSUnit(value, out size))
                        return true;
                    else
                        return false;

            }
        }

    }

    #endregion

    // display

    #region public class CSSDisplayParser : CSSEnumStyleParser<Text.TextDecoration>

    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSDisplayParser : CSSEnumStyleParser<PositionMode>
    {
        public CSSDisplayParser()
            : base(CSSStyleItems.Display, StyleKeys.PositionModeKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PositionMode display;
            if (reader.ReadNextValue()) {
                if (TryGetPositionEnum(reader.CurrentTextValue, out display))
                    this.SetValue(onStyle, display);
            }
            else
                result = false;

            return result;
        }

        

        public static bool TryGetPositionEnum(string value, out PositionMode display)
        {
            switch (value.ToLower())
            {
                case ("inline"):
                    display = PositionMode.Inline;
                    return true;

                case ("block"):
                    display = PositionMode.Block;
                    return true;
                case ("none"):
                    display = PositionMode.Invisible;
                    return true;
               default:
                    display = PositionMode.Block;
                    return false;

            }
        }

    }

    #endregion

    // overflow

    #region public class CSSOverflowActionParser : CSSEnumStyleParser<OverflowAction>

    /// <summary>
    /// Parses and sets the components overflow option based on the CSS names
    /// </summary>
    public class CSSOverflowActionParser : CSSEnumStyleParser<OverflowAction>
    {
        public CSSOverflowActionParser()
            : base(CSSStyleItems.Overflow, StyleKeys.OverflowActionKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            OverflowAction over;
            if (reader.ReadNextValue() && TryGetOverflowEnum(reader.CurrentTextValue, out over))
                this.SetValue(onStyle, over);
            else
                result = false;

            return result;
        }

        public static bool IsDecorationEnum(string value)
        {
            OverflowAction over;
            return TryGetOverflowEnum(value, out over);
        }

        public static bool TryGetOverflowEnum(string value, out OverflowAction over)
        {
            switch (value.ToLower())
            {
                case ("auto"):
                case("visible"):
                    over = OverflowAction.None;
                    return true;

                case ("hidden"):
                    over = OverflowAction.Truncate;
                    return true;

                default:
                    over = OverflowAction.None;
                    return false;

            }
        }

    }

    #endregion

    // white-space

    public class CSSOverflowXParser : CSSStyleValueParser
    {
        public CSSOverflowXParser() : base(CSSStyleItems.OverflowX)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                switch (val)
                {
                    case ("hidden"):
                        onStyle.SetValue(StyleKeys.ClipLeftKey, (PDFUnit)0.1);
                        onStyle.SetValue(StyleKeys.ClipRightKey, (PDFUnit)0.1);
                        return true;
                    case ("auto"):
                    case ("initial"):
                    case ("visible"):
                        onStyle.RemoveValue(StyleKeys.ClipLeftKey);
                        onStyle.RemoveValue(StyleKeys.ClipRightKey);
                        return true;
                    default:
                        break;
                }
            }

            return false;
            
        }
    }

    public class CSSOverflowYParser : CSSStyleValueParser
    {
        public CSSOverflowYParser() : base(CSSStyleItems.OverflowX)
        {

        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            if (reader.ReadNextValue())
            {
                var val = reader.CurrentTextValue;

                switch (val)
                {
                    case ("hidden"):
                        onStyle.SetValue(StyleKeys.ClipTopKey, (PDFUnit)0.1);
                        onStyle.SetValue(StyleKeys.ClipBottomKey, (PDFUnit)0.1);
                        return true;
                    case ("auto"):
                    case ("initial"):
                    case ("visible"):
                        onStyle.RemoveValue(StyleKeys.ClipTopKey);
                        onStyle.RemoveValue(StyleKeys.ClipBottomKey);
                        return true;
                    default:
                        break;
                }
            }

            return false;

        }
    }

    #region public class CSSWhiteSpaceParser : CSSStyleValueParser

    public class CSSWhiteSpaceParser : CSSStyleValueParser
    {
        public CSSWhiteSpaceParser()
            : base(CSSStyleItems.WhiteSpace)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool success = true;
            Text.WordWrap wrap;
            bool preserve;

            if (!reader.ReadNextValue())
            {
                wrap = Text.WordWrap.Auto;
                preserve = false;
                success = false;
            }
            else
            {
                switch (reader.CurrentTextValue.ToLower())
                {
                    case "normal":
                        wrap = Text.WordWrap.Auto;
                        preserve = false;
                        success = true;
                        break;
                    case "pre":
                        wrap = Text.WordWrap.NoWrap;
                        preserve = true;
                        success = true;
                        break;
                    case "nowrap":
                        wrap = Text.WordWrap.NoWrap;
                        preserve = false;
                        success = true;
                        break;
                    case "pre-wrap":
                        wrap = Text.WordWrap.Auto;
                        preserve = true;
                        success = true;
                        break;
                    case "pre-line":
                        wrap = Text.WordWrap.Auto;
                        preserve = false;
                        success = true;
                        break;
                    default:
                        wrap = Text.WordWrap.Auto;
                        preserve = false;
                        success = false; //not supported
                        break;
                }
            }
            if (success)
            {
                onStyle.SetValue(StyleKeys.TextWordWrapKey, wrap);
                onStyle.SetValue(StyleKeys.TextWhitespaceKey, preserve);
            }
            return success;
        }
    }

    #endregion

    // position mode

    public class CSSPositionModeParser: CSSEnumStyleParser<PositionMode>
    {
        public CSSPositionModeParser() : base(CSSStyleItems.PositionModeType, StyleKeys.PositionModeKey)
        { }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            PositionMode found = PositionMode.Block;
            if (reader.ReadNextValue() && TryGetPositionMode(reader.CurrentTextValue, out found))
                this.SetValue(onStyle, found);
            else
                result = false;

            return result;
        }

        protected virtual bool TryGetPositionMode(string value, out PositionMode mode)
        {
            mode = PositionMode.Block;
            if (string.IsNullOrEmpty(value))
                return false;

            switch (value.ToLower())
            {
                case ("relative"):
                    mode = PositionMode.Relative;
                    return true;

                case ("absolute"):
                    mode = PositionMode.Absolute;
                    return true;

                default:
                    return false;
            }
        }

    }


    // list-style-type


    #region public class CSSListStyleTypeParser : CSSEnumStyleParser<Text.TextDecoration>

    /// <summary>
    /// Parses and sets the components text decoration option based on the CSS names
    /// </summary>
    public class CSSListStyleTypeParser : CSSEnumStyleParser<ListNumberingGroupStyle>
    {
        public CSSListStyleTypeParser()
            : base(CSSStyleItems.ListStyleType, StyleKeys.ListNumberStyleKey)
        {
        }

        protected override bool DoSetStyleValue(Style onStyle, CSSStyleItemReader reader)
        {
            bool result = true;
            ListNumberingGroupStyle type;
            if (reader.ReadNextValue() && TryGetListTypeEnum(reader.CurrentTextValue, out type))
                this.SetValue(onStyle, type);
            else
                result = false;

            return result;
        }

        public static bool IsDecorationEnum(string value)
        {
            ListNumberingGroupStyle type;
            return TryGetListTypeEnum(value, out type);
        }

        public static bool TryGetListTypeEnum(string value, out ListNumberingGroupStyle type)
        {
            switch (value.ToLower())
            {
                case ("disc"):
                case("circle"):
                    type = ListNumberingGroupStyle.Bullet;
                    return true;

                case ("decimal"):
                    type = ListNumberingGroupStyle.Decimals;
                    return true;

                case("none"):
                    type = ListNumberingGroupStyle.None;
                    return true;

                case("lower-roman"):
                    type = ListNumberingGroupStyle.LowercaseRoman;
                    return true;

                case("lower-alpha"):
                    type = ListNumberingGroupStyle.LowercaseLetters;
                    return true;

                case("upper-roman"):
                    type = ListNumberingGroupStyle.UppercaseRoman;
                    return true;

                case("upper-alpha"):
                    type = ListNumberingGroupStyle.UppercaseLetters;
                    return true;
                default:
                    type = ListNumberingGroupStyle.Decimals;
                    return false;

            }
        }

    }

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
