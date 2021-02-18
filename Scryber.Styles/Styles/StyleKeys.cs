/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber;
using Scryber.Drawing;

namespace Scryber.Styles
{
    public static class StyleKeys
    {
        private static bool INHERITED = true;
        private static bool NOT_INHERITED = false;

        //
        // thread safe and guarunteed to be called only once
        // so we can use the internal (Non thread safe initialization)
        //

        // Background
        public static readonly StyleKey BgItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBackground, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BgColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BgItemKey);
        public static readonly PDFStyleKey<string> BgImgSrcKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"imgs", BgItemKey);
        public static readonly PDFStyleKey<PatternRepeat> BgRepeatKey = StyleKey.InternalCreateStyleValueKey<PatternRepeat>((PDFObjectType)"rept", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xstp", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ystp", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xsiz", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ysiz", BgItemKey);
        public static readonly PDFStyleKey<Drawing.FillType> BgStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((PDFObjectType)"styl", BgItemKey);
        public static readonly PDFStyleKey<double> BgOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", BgItemKey);

        //Border
        public static readonly StyleKey BorderItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorder, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemKey);
        public static readonly PDFStyleKey<PDFUnit> BorderCornerRadiusKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"crad", BorderItemKey);
        public static readonly PDFStyleKey<PDFDash> BorderDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemKey);
        public static readonly PDFStyleKey<LineCaps> BorderEndingKey = StyleKey.InternalCreateStyleValueKey<LineCaps>((PDFObjectType)"endg", BorderItemKey);
        public static readonly PDFStyleKey<LineJoin> BorderJoinKey = StyleKey.InternalCreateStyleValueKey<LineJoin>((PDFObjectType)"join", BorderItemKey);
        public static readonly PDFStyleKey<float> BorderMitreKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"mitr", BorderItemKey);
        public static readonly PDFStyleKey<double> BorderOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", BorderItemKey);
        public static readonly PDFStyleKey<Sides> BorderSidesKey = StyleKey.InternalCreateStyleValueKey<Sides>((PDFObjectType)"side", BorderItemKey);
        public static readonly PDFStyleKey<LineType> BorderStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", BorderItemKey);
        public static readonly PDFStyleKey<PDFUnit> BorderWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemKey);

        //Border Top
        public static readonly StyleKey BorderItemTopKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorderTop, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderTopColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemTopKey);
        public static readonly PDFStyleKey<LineType> BorderTopStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", BorderItemTopKey);
        public static readonly PDFStyleKey<PDFUnit> BorderTopWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemTopKey);
        public static readonly PDFStyleKey<PDFDash> BorderTopDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemTopKey);

        //Border Top
        public static readonly StyleKey BorderItemLeftKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorderLeft, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderLeftColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemLeftKey);
        public static readonly PDFStyleKey<LineType> BorderLeftStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", BorderItemLeftKey);
        public static readonly PDFStyleKey<PDFUnit> BorderLeftWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemLeftKey);
        public static readonly PDFStyleKey<PDFDash> BorderLeftDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemLeftKey);

        //Border Top
        public static readonly StyleKey BorderItemBottomKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorderBottom, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderBottomColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemBottomKey);
        public static readonly PDFStyleKey<LineType> BorderBottomStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", BorderItemBottomKey);
        public static readonly PDFStyleKey<PDFUnit> BorderBottomWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemBottomKey);
        public static readonly PDFStyleKey<PDFDash> BorderBottomDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemBottomKey);

        //Border Top
        public static readonly StyleKey BorderItemRightKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorderRight, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderRightColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemRightKey);
        public static readonly PDFStyleKey<LineType> BorderRightStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", BorderItemRightKey);
        public static readonly PDFStyleKey<PDFUnit> BorderRightWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemRightKey);
        public static readonly PDFStyleKey<PDFDash> BorderRightDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemRightKey);

        // Padding
        public static readonly StyleKey PaddingItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePadding, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> PaddingTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", PaddingItemKey);

        // Margins
        public static readonly StyleKey MarginsItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleMargins, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> MarginsTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", MarginsItemKey);

        // Clip
        public static readonly StyleKey ClipItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleClip, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> ClipLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", ClipItemKey);

        // Columns
        public static readonly StyleKey ColumnItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleColumns, NOT_INHERITED);
        public static readonly PDFStyleKey<int> ColumnCountKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cont", ColumnItemKey);
        public static readonly PDFStyleKey<PDFUnit> ColumnAlleyKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ally", ColumnItemKey);
        public static readonly PDFStyleKey<bool> ColumnFlowKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"flow", ColumnItemKey);
        public static readonly PDFStyleKey<PDFColumnWidths> ColumnWidthKey = StyleKey.InternalCreateStyleValueKey<PDFColumnWidths>((PDFObjectType)"cwid", ColumnItemKey);
        public static readonly PDFStyleKey<bool> ColumnBreakBeforeKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"brcb", ColumnItemKey);
        public static readonly PDFStyleKey<bool> ColumnBreakAfterKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"brca", ColumnItemKey);

        //Fill
        public static readonly StyleKey FillItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleFill, INHERITED);
        public static readonly PDFStyleKey<PDFColor> FillColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", FillItemKey);
        public static readonly PDFStyleKey<string> FillImgSrcKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"imgs", FillItemKey);
        public static readonly PDFStyleKey<PatternRepeat> FillRepeatKey = StyleKey.InternalCreateStyleValueKey<PatternRepeat>((PDFObjectType)"rept", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xstp", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ystp", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xsiz", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ysiz", FillItemKey);
        public static readonly PDFStyleKey<Drawing.FillType> FillStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((PDFObjectType)"styl", FillItemKey);
        public static readonly PDFStyleKey<double> FillOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", FillItemKey);

        //Font
        public static readonly StyleKey FontItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleFont, INHERITED);
        public static readonly PDFStyleKey<PDFFontSelector> FontFamilyKey = StyleKey.InternalCreateStyleValueKey<PDFFontSelector>((PDFObjectType)"faml", FontItemKey);
        public static readonly PDFStyleKey<bool> FontBoldKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bold", FontItemKey);
        public static readonly PDFStyleKey<bool> FontItalicKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"ital", FontItemKey);
        public static readonly PDFStyleKey<PDFUnit> FontSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"size", FontItemKey);

        //Overflow
        public static readonly StyleKey OverflowItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOverflow, INHERITED);
        public static readonly PDFStyleKey<OverflowAction> OverflowActionKey = StyleKey.InternalCreateStyleValueKey<OverflowAction>((PDFObjectType)"actn", OverflowItemKey);
        public static readonly PDFStyleKey<OverflowSplit> OverflowSplitKey = StyleKey.InternalCreateStyleValueKey<OverflowSplit>((PDFObjectType)"splt", OverflowItemKey);

        //Position
        public static readonly StyleKey PositionItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePosition, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> PositionXKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", PositionItemKey);
        public static readonly PDFStyleKey<PDFUnit> PositionYKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", PositionItemKey);
        public static readonly PDFStyleKey<PositionMode> PositionModeKey = StyleKey.InternalCreateStyleValueKey<PositionMode>((PDFObjectType)"mode", PositionItemKey);
        public static readonly PDFStyleKey<PDFRect> PositionViewPort = StyleKey.InternalCreateStyleValueKey<PDFRect>((PDFObjectType)"vwpt", PositionItemKey);

        //Size
        public static readonly StyleKey SizeItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleSize, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> SizeWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"heit", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMinimumWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"minw", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMinimumHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"minh", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMaximumWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"maxw", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMaximumHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"maxh", SizeItemKey);
        public static readonly PDFStyleKey<bool> SizeFullWidthKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"fulw", SizeItemKey);

        //Stroke
        public static readonly StyleKey StrokeItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleStroke, INHERITED);
        public static readonly PDFStyleKey<PDFColor> StrokeColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", StrokeItemKey);
        public static readonly PDFStyleKey<PDFDash> StrokeDashKey = StyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", StrokeItemKey);
        public static readonly PDFStyleKey<LineCaps> StrokeEndingKey = StyleKey.InternalCreateStyleValueKey<LineCaps>((PDFObjectType)"endg", StrokeItemKey);
        public static readonly PDFStyleKey<LineJoin> StrokeJoinKey = StyleKey.InternalCreateStyleValueKey<LineJoin>((PDFObjectType)"join", StrokeItemKey);
        public static readonly PDFStyleKey<float> StrokeMitreKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"mitr", StrokeItemKey);
        public static readonly PDFStyleKey<double> StrokeOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", StrokeItemKey);
        public static readonly PDFStyleKey<LineType> StrokeStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((PDFObjectType)"styl", StrokeItemKey);
        public static readonly PDFStyleKey<PDFUnit> StrokeWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", StrokeItemKey);

        //Text
        public static readonly StyleKey TextItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleText, INHERITED);
        public static readonly PDFStyleKey<PDFUnit> TextFirstLineIndentKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"indt", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextLeadingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"lead", TextItemKey);
        public static readonly PDFStyleKey<bool> TextWhitespaceKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"whit", TextItemKey);
        public static readonly PDFStyleKey<string> TextDateFormatKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"datf", TextItemKey);
        public static readonly PDFStyleKey<string> TextNumberFormatKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"numf", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextWordSpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"wspc", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextCharSpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"cspc", TextItemKey);
        public static readonly PDFStyleKey<double> TextHorizontalScaling = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"thsc", TextItemKey);
        public static readonly PDFStyleKey<TextDirection> TextDirectionKey = StyleKey.InternalCreateStyleValueKey<TextDirection>((PDFObjectType)"tdir", TextItemKey);
        public static readonly PDFStyleKey<Scryber.Text.WordWrap> TextWordWrapKey = StyleKey.InternalCreateStyleValueKey<Scryber.Text.WordWrap>((PDFObjectType)"wrap", TextItemKey);
        public static readonly PDFStyleKey<Scryber.Text.TextDecoration> TextDecorationKey = StyleKey.InternalCreateStyleValueKey<Scryber.Text.TextDecoration>((PDFObjectType)"decr", TextItemKey);
        public static readonly PDFStyleKey<bool> TextPositionFromBaseline = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"tbse", TextItemKey);
        public static readonly PDFStyleKey<VerticalAlignment> PositionVAlignKey = StyleKey.InternalCreateStyleValueKey<VerticalAlignment>((PDFObjectType)"vlgn", TextItemKey);
        public static readonly PDFStyleKey<HorizontalAlignment> PositionHAlignKey = StyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((PDFObjectType)"hlgn", TextItemKey);

        //List
        public static readonly StyleKey ListItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleList, NOT_INHERITED);
        public static readonly PDFStyleKey<ListNumberingGroupStyle> ListNumberStyleKey = StyleKey.InternalCreateStyleValueKey<ListNumberingGroupStyle>((PDFObjectType)"styl", ListItemKey);
        public static readonly PDFStyleKey<string> ListGroupKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"grup", ListItemKey);
        public static readonly PDFStyleKey<PDFUnit> ListInsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"inst", ListItemKey);
        public static readonly PDFStyleKey<string> ListPrefixKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"pref", ListItemKey);
        public static readonly PDFStyleKey<string> ListPostfixKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"post", ListItemKey);
        public static readonly PDFStyleKey<HorizontalAlignment> ListAlignmentKey = StyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((PDFObjectType)"alig", ListItemKey);
        public static readonly PDFStyleKey<bool> ListConcatKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"conc", ListItemKey);
        public static readonly PDFStyleKey<string> ListLabelKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"labl", ListItemKey);

        //Outline
        public static readonly StyleKey OutlineItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOutline, NOT_INHERITED);
        public static readonly PDFStyleKey<bool> OutlineIsOutlinedKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"outl", OutlineItemKey);
        public static readonly PDFStyleKey<PDFColor> OutlineColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineBoldKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bold", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineItalicKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"ital", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineOpenKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"open", OutlineItemKey);

        //Overlay Grid
        public static readonly StyleKey OverlayItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOverlayGrid, NOT_INHERITED);
        public static readonly PDFStyleKey<bool> OverlayShowGridKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"show", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlaySpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"spac", OverlayItemKey);
        public static readonly PDFStyleKey<PDFColor> OverlayColorKey = StyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", OverlayItemKey);
        public static readonly PDFStyleKey<double> OverlayOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlayXOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xoff", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlayYOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"yoff", OverlayItemKey);
        public static readonly PDFStyleKey<bool> OverlayShowColumnsKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"colm", OverlayItemKey);

        //Page
        public static readonly StyleKey PageItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePage, NOT_INHERITED);
        public static readonly PDFStyleKey<PaperSize> PagePaperSizeKey = StyleKey.InternalCreateStyleValueKey<PaperSize>((PDFObjectType)"papr", PageItemKey);
        public static readonly PDFStyleKey<PaperOrientation> PageOrientationKey = StyleKey.InternalCreateStyleValueKey<PaperOrientation>((PDFObjectType)"orit", PageItemKey);
        public static readonly PDFStyleKey<PDFUnit> PageWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widh", PageItemKey);
        public static readonly PDFStyleKey<PDFUnit> PageHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"heit", PageItemKey);
        public static readonly PDFStyleKey<int> PageAngle = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"angl", PageItemKey);
        public static readonly PDFStyleKey<PageNumberStyle> PageNumberStyleKey = StyleKey.InternalCreateStyleValueKey<PageNumberStyle>((PDFObjectType)"styl", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberStartKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"strt", PageItemKey);
        public static readonly PDFStyleKey<string> PageNumberGroupKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"grup", PageItemKey);
        public static readonly PDFStyleKey<string> PageNumberFormatKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"fomt", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberGroupHintKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"ghnt", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberTotalHintKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"thnt", PageItemKey);
        public static readonly PDFStyleKey<bool> PageBreakBeforeKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bkpb", PageItemKey);
        public static readonly PDFStyleKey<bool> PageBreakAfterKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bkpa", PageItemKey);
        public static readonly PDFStyleKey<string> PageNameGroupKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"pgnm", PageItemKey);

        //Shape
        public static readonly StyleKey ShapeItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.ShapeStyle, NOT_INHERITED);
        public static readonly PDFStyleKey<int> ShapeVertexCountKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"vrtx", ShapeItemKey);
        public static readonly PDFStyleKey<int> ShapeVertexStepKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"step", ShapeItemKey);
        public static readonly PDFStyleKey<bool> ShapeClosedKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"clos", ShapeItemKey);
        public static readonly PDFStyleKey<double> ShapeRotationKey = StyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"rotn", ShapeItemKey);

        //Table
        public static readonly StyleKey TableItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleTable, NOT_INHERITED);
        public static readonly PDFStyleKey<int> TableCellColumnSpanKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cols", TableItemKey);
        public static readonly PDFStyleKey<TableRowRepeat> TableRowRepeatKey = StyleKey.InternalCreateStyleValueKey<TableRowRepeat>((PDFObjectType)"rowr", TableItemKey);

        //Badge
        public static readonly StyleKey BadgeItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBadge, NOT_INHERITED);
        public static readonly PDFStyleKey<Corner> BadgeCornerKey = StyleKey.InternalCreateStyleValueKey<Corner>((PDFObjectType)"corn", BadgeItemKey);
        public static readonly PDFStyleKey<PDFUnit> BadgeXOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xoff", BadgeItemKey);
        public static readonly PDFStyleKey<PDFUnit> BadgeYOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"yoff", BadgeItemKey);
        public static readonly PDFStyleKey<BadgeType> BadgeDisplayKey = StyleKey.InternalCreateStyleValueKey<BadgeType>((PDFObjectType)"disp", BadgeItemKey);

        //Modify
        public static readonly StyleKey ModifyPageItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleModify, NOT_INHERITED);
        public static readonly PDFStyleKey<ModificationType> ModifyPageTypeKey = StyleKey.InternalCreateStyleValueKey<ModificationType>((PDFObjectType)"type", ModifyPageItemKey);
        public static readonly PDFStyleKey<ModifiedContentAction> ModifyPageActionKey = StyleKey.InternalCreateStyleValueKey<ModifiedContentAction>((PDFObjectType)"actn", ModifyPageItemKey);
        public static readonly PDFStyleKey<int> ModifyPageStartIndexKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"strt", ModifyPageItemKey);
        public static readonly PDFStyleKey<int> ModifyPageCountKey = StyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cont", ModifyPageItemKey);


        //Transform
        public static readonly StyleKey TransformItemKey = StyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleTransform, NOT_INHERITED);
        public static readonly PDFStyleKey<float> TransformRotateKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"rote", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXScaleKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xscl", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXSkewKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xskw", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXOffsetKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xoff", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYScaleKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yscl", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYSkewKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yskw", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYOffsetKey = StyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yoff", TransformItemKey);
        public static readonly PDFStyleKey<TransformationOrigin> TransformOriginKey = StyleKey.InternalCreateStyleValueKey<TransformationOrigin>((PDFObjectType)"orig", TransformItemKey);

        //FontFace
        public static readonly StyleKey FontFaceItemKey = StyleKey.InternalCreateStyleItemKey((PDFObjectType)"csff", NOT_INHERITED);
        public static readonly PDFStyleKey<PDFFontSource> FontFaceSrcKey = StyleKey.InternalCreateStyleValueKey<PDFFontSource>((PDFObjectType)"fsrc", FontFaceItemKey);
        public static readonly PDFStyleKey<string> FontFaceFamilyKey = StyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"ffam", FontFaceItemKey);
        public static readonly PDFStyleKey<bool> FontFaceWeightKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"fwgt", FontFaceItemKey);
        public static readonly PDFStyleKey<bool> FontFaceStyleKey = StyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"fsyl", FontFaceItemKey);



    }
}
