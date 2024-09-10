using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Styles;
using Scryber.Html;
using Scryber.Styles.Parsing.Typed;
using System.Net.Http.Headers;

namespace Scryber.Styles.Parsing
{
    /// <summary>
    /// Implements the Style Item Parser, delegating to one of it's known inner item parsers.
    /// </summary>
    public class CSSStyleItemAllParser : IParserStyleFactory
    {
        private IDictionary<string, IParserStyleFactory> _knownStyles;

        public CSSStyleItemAllParser()
            : this(_allknown)
        {
        }

        public CSSStyleItemAllParser(IDictionary<string, IParserStyleFactory> known)
        {
            if (null == known)
                throw new ArgumentNullException("known");

            _knownStyles = known;
        }


        public bool SetStyleValue(IHtmlContentParser parser, IStyledComponent component, CSSStyleItemReader reader)
        {
            IParserStyleFactory found;

            if (string.IsNullOrEmpty(reader.CurrentAttribute) && reader.ReadNextAttributeName() == false)
                return false;

            if (_knownStyles.TryGetValue(reader.CurrentAttribute, out found))
                return found.SetStyleValue(parser, component, reader);
            else
            {
                if (null != parser && parser.IsLogging)
                    parser.Context.TraceLog.Add(TraceLevel.Warning, "CSS", "Could not set the style value on attribute '" + reader.CurrentAttribute + "' as it is not a known style attribute.");
                reader.MoveToNextAttribute();
            }
            return false;
        }

        public bool SetStyleValue(Style style, CSSStyleItemReader reader, ContextBase context)
        {
            IParserStyleFactory found;

            if (string.IsNullOrEmpty(reader.CurrentAttribute) && reader.ReadNextAttributeName() == false)
                return false;

            string variable;
            if(IsVariableName(reader, out variable))
            {
                if(style is StyleDefn defn)
                {
                    var id = reader.CurrentAttribute;
                    if (reader.ReadNextValue(';', ignoreWhiteSpace: true))
                    {
                        defn.AddVariable(variable, reader.CurrentTextValue);
                        return true;
                    }
                }
                else if (null != context && context.TraceLog.ShouldLog(TraceLevel.Warning))
                    context.TraceLog.Add(TraceLevel.Warning, "CSS", "Can only declare variables within style definitions '" + reader.CurrentAttribute + "' is not beinf set on a definition.");

                return false;
            }
            else if (_knownStyles.TryGetValue(reader.CurrentAttribute, out found))
                return found.SetStyleValue(style, reader, context);
            else
            {
                if (null != context && context.TraceLog.ShouldLog(TraceLevel.Verbose))
                    context.TraceLog.Add(TraceLevel.Warning, "CSS", "Could not set the style value on attribute '" + reader.CurrentAttribute + "' as it is not a known style attribute.");
                while (reader.MoveToNextValue())
                {
                    ;
                }
                return false;
            }
        }

        private const string CSSVariableIdentifier = "--";
        private const int CSSVariableIdentifierLength = 2;

        protected bool IsVariableName(CSSStyleItemReader reader, out string name)
        {
            if (reader.CurrentAttribute.StartsWith(CSSVariableIdentifier))
            {
                var queue = new Queue<string>();

                name = reader.CurrentAttribute;
                return true;
            }
            else
            {
                name = null;
                return false;
            }
        }


        private static ReadOnlyDictionary<string, IParserStyleFactory> _allknown;

        static CSSStyleItemAllParser()
        {
            Dictionary<string, IParserStyleFactory> all = new Dictionary<string, IParserStyleFactory>(StringComparer.OrdinalIgnoreCase);

            all.Add(CSSStyleItems.Border, new CSSBorderParser());
            all.Add(CSSStyleItems.BorderStyle, new CSSBorderStyleParser());
            all.Add(CSSStyleItems.BorderColor, new CSSBorderColorParser());
            all.Add(CSSStyleItems.BorderWidth, new CSSBorderWidthParser());
            all.Add(CSSStyleItems.BorderRadius, new CSSBorderRadiusParser());
            all.Add(CSSStyleItems.BorderLeft, new CSSBorderLeftParser());
            all.Add(CSSStyleItems.BorderRight, new CSSBorderRightParser());
            all.Add(CSSStyleItems.BorderTop, new CSSBorderTopParser());
            all.Add(CSSStyleItems.BorderBottom, new CSSBorderBottomParser());

            all.Add(CSSStyleItems.BorderLeftWidth, new CSSBorderLeftWidthParser());
            all.Add(CSSStyleItems.BorderRightWidth, new CSSBorderRightWidthParser());
            all.Add(CSSStyleItems.BorderTopWidth, new CSSBorderTopWidthParser());
            all.Add(CSSStyleItems.BorderBottomWidth, new CSSBorderBottomWidthParser());

            all.Add(CSSStyleItems.BorderLeftColor, new CSSBorderLeftColorParser());
            all.Add(CSSStyleItems.BorderRightColor, new CSSBorderRightColorParser());
            all.Add(CSSStyleItems.BorderTopColor, new CSSBorderTopColorParser());
            all.Add(CSSStyleItems.BorderBottomColor, new CSSBorderBottomColorParser());

            all.Add(CSSStyleItems.BorderLeftStyle, new CSSBorderLeftStyleParser());
            all.Add(CSSStyleItems.BorderRightStyle, new CSSBorderRightStyleParser());
            all.Add(CSSStyleItems.BorderTopStyle, new CSSBorderTopStyleParser());
            all.Add(CSSStyleItems.BorderBottomStyle, new CSSBorderBottomStyleParser());

            all.Add(CSSStyleItems.FillColor, new CSSFillColourParser());


            all.Add(CSSStyleItems.Background, new CSSBackgroundParser());
            all.Add(CSSStyleItems.BackgroundColor, new CSSBackgroundColorParser());
            all.Add(CSSStyleItems.BackgroundImage, new CSSBackgroundImageParser());
            all.Add(CSSStyleItems.BackgroundRepeat, new CSSBackgroundRepeatParser());
            all.Add(CSSStyleItems.BackgroundPosition, new CSSBackgroundPositionParser());
            all.Add(CSSStyleItems.BackgroundPositionX, new CSSBackgroundPositionXParser());
            all.Add(CSSStyleItems.BackgroundPositionY, new CSSBackgroundPositionYParser());
            all.Add(CSSStyleItems.BackgroundSize, new CSSBackgroundSizeParser());

            all.Add(CSSStyleItems.FontStyle, new CSSFontStyleParser());
            all.Add(CSSStyleItems.FontWeight, new CSSFontWeightParser());
            all.Add(CSSStyleItems.FontSize, new CSSFontSizeParser());
            all.Add(CSSStyleItems.FontLineHeight, new CSSFontLineHeightParser());
            all.Add(CSSStyleItems.FontFamily, new CSSFontFamilyParser());
            all.Add(CSSStyleItems.Font, new CSSFontParser());
            all.Add(CSSStyleItems.FontSource, new CSSFontSourceParser());
            all.Add(CSSStyleItems.FontDisplay, new CSSFontDisplayParser());
            all.Add(CSSStyleItems.FontStretch, new CSSFontStretchParser());
            
            all.Add(CSSStyleItems.MarginsLeft, new CSSMarginsLeftParser());
            all.Add(CSSStyleItems.MarginsRight, new CSSMarginsRightParser());
            all.Add(CSSStyleItems.MarginsBottom, new CSSMarginsBottomParser());
            all.Add(CSSStyleItems.MarginsTop, new CSSMarginsTopParser());
            all.Add(CSSStyleItems.Margins, new CSSMarginsAllParser());

            all.Add(CSSStyleItems.MarginInlineAll, new CSSMarginsInlineBothParser());
            all.Add(CSSStyleItems.MarginInlineStart, new CSSMarginsInlineStartParser());
            all.Add(CSSStyleItems.MarginInlineEnd, new CSSMarginsInlineEndParser());

            all.Add(CSSStyleItems.PaddingLeft, new CSSPaddingLeftParser());
            all.Add(CSSStyleItems.PaddingRight, new CSSPaddingRightParser());
            all.Add(CSSStyleItems.PaddingBottom, new CSSPaddingBottomParser());
            all.Add(CSSStyleItems.PaddingTop, new CSSPaddingTopParser());
            all.Add(CSSStyleItems.Padding, new CSSPaddingAllParser());

            all.Add(CSSStyleItems.Opacity, new CSSOpacityParser());

            all.Add(CSSStyleItems.ColumnCount, new CSSColumnCountParser());
            all.Add(CSSStyleItems.ColumnWidths, new CSSColumnWidthParser());
            all.Add(CSSStyleItems.ColumnGap, new CSSColumnGapParser());
            all.Add(CSSStyleItems.ColumnSpan, new CSSColumnSpanParser());

            all.Add(CSSStyleItems.Left, new CSSLeftParser());
            all.Add(CSSStyleItems.Top, new CSSTopParser());
            all.Add(CSSStyleItems.Float, new CSSPositionFloatParser());
            all.Add(CSSStyleItems.Bottom, new CSSBottomParser());
            all.Add(CSSStyleItems.Right, new CSSRightParser());

            all.Add(CSSStyleItems.Width, new CSSWidthParser());
            all.Add(CSSStyleItems.Height, new CSSHeightParser());
            all.Add(CSSStyleItems.MinimumHeight, new CSSMinHeightParser());
            all.Add(CSSStyleItems.MinimumWidth, new CSSMinWidthParser());
            all.Add(CSSStyleItems.MaximumHeight, new CSSMaxHeightParser());
            all.Add(CSSStyleItems.MaximumWidth, new CSSMaxWidthParser());

            all.Add(CSSStyleItems.TextAlign, new CSSTextAlignParser());
            all.Add(CSSStyleItems.VerticalAlign, new CSSVerticalAlignParser());

            all.Add(CSSStyleItems.TextDecoration, new CSSTextDecorationParser());
            all.Add(CSSStyleItems.TextDecorationLine, new CSSTextDecorationParser());
            all.Add(CSSStyleItems.LetterSpacing, new CSSLetterSpacingParser());
            all.Add(CSSStyleItems.WordSpacing, new CSSWordSpacingParser());
            all.Add(CSSStyleItems.Hyphenation, new CSSHyphensParser());
            all.Add(CSSStyleItems.HyphenationMinBefore, new CSSHyphensMinBeforeParser());
            all.Add(CSSStyleItems.HyphenationMinAfter, new CSSHyphensMinAfterParser());
            all.Add(CSSStyleItems.HyphenationCharAppend, new CSSHyphensCharAppendParser());
            all.Add(CSSStyleItems.TextAnchorType, new CSSTextAnchorParser());
            all.Add(CSSStyleItems.DominantBaselineType, new CSSDominantBaselineParser());

            all.Add(CSSStyleItems.WhiteSpace, new CSSWhiteSpaceParser());
            all.Add(CSSStyleItems.OverflowX, new CSSOverflowXParser());
            all.Add(CSSStyleItems.OverflowY, new CSSOverflowYParser());

            all.Add(CSSStyleItems.Display, new CSSDisplayParser());
            all.Add(CSSStyleItems.Overflow, new CSSOverflowActionParser());

            all.Add(CSSStyleItems.ListStyleType, new CSSListStyleTypeParser());
            all.Add(CSSStyleItems.ListStyle, new CSSListStyleParser());
            //custom list parsers
            all.Add(CSSStyleItems.ListItemGroup, new CSSListItemGroupParser());
            all.Add(CSSStyleItems.ListItemConcatenate, new CSSListItemConcatenationParser());
            all.Add(CSSStyleItems.ListItemAlignment, new CSSListItemAlignmentParser());
            all.Add(CSSStyleItems.ListItemInset, new CSSListItemInsetParser());

            all.Add(CSSStyleItems.ListItemPrefix, new CSSListItemPrefixParser());
            all.Add(CSSStyleItems.ListItemPostfix, new CSSListItemPostFixParser());

            all.Add(CSSStyleItems.PageBreakInside, new CSSPageBreakInsideParser());
            all.Add(CSSStyleItems.PageBreakAfter, new CSSPageBreakAfterParser());
            all.Add(CSSStyleItems.PageBreakBefore, new CSSPageBreakBeforeParser());
            all.Add(CSSStyleItems.PageSize, new CSSPageSizeParser());
            all.Add(CSSStyleItems.PageGroupName, new CSSPageNameParser());

            all.Add(CSSStyleItems.BreakInside, new CSSColumnBreakInsideParser());
            all.Add(CSSStyleItems.BreakAfter, new CSSColumnBreakAfterParser());
            all.Add(CSSStyleItems.BreakBefore, new CSSColumnBreakBeforeParser());

            all.Add(CSSStyleItems.PositionModeType, new CSSPositionModeParser());
            

            all.Add(CSSStyleItems.StrokeColor, new CSSStrokeColorParser());
            all.Add(CSSStyleItems.StrokeOpacity, new CSSStrokeOpacityParser());
            all.Add(CSSStyleItems.StrokeWidth, new CSSStrokeWidthParser());
            all.Add(CSSStyleItems.StrokeDash, new CSSStrokeDashParser());
            all.Add(CSSStyleItems.StrokeLineCap, new CSSStrokeLineCapParser());
            all.Add(CSSStyleItems.StrokeLineJoin, new CSSStrokeLineJoinParser());

            all.Add(CSSStyleItems.FillOpacity, new CSSFillOpacityParser());
            all.Add(CSSStyleItems.Fill, new CSSFillParser());

            all.Add(CSSStyleItems.Transform, new CSSTransformParser());
            all.Add(CSSStyleItems.Content, new CSSContentParser());

            all.Add(CSSStyleItems.CounterReset, new CSSCounterResetParser());
            all.Add(CSSStyleItems.CounterIncrement, new CSSCounterIncrementParser());

            _allknown = new ReadOnlyDictionary<string, IParserStyleFactory>(all);
        }
    }
}
