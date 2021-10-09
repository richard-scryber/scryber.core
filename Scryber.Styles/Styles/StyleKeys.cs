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
        public static readonly StyleKey BgItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBackground, NOT_INHERITED);
        public static readonly StyleKey<Color> BgColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BgItemKey);
        public static readonly StyleKey<string> BgImgSrcKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"imgs", BgItemKey);
        public static readonly StyleKey<PatternRepeat> BgRepeatKey = StyleKey.InternalCreateStyleValueKey<PatternRepeat>((ObjectType)"rept", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgXPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xpos", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgYPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ypos", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgXStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xstp", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgYStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ystp", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgXSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xsiz", BgItemKey);
        public static readonly StyleKey<PDFUnit> BgYSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ysiz", BgItemKey);
        public static readonly StyleKey<Drawing.FillType> BgStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((ObjectType)"styl", BgItemKey);
        public static readonly StyleKey<double> BgOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", BgItemKey);

        //Border
        public static readonly StyleKey BorderItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorder, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemKey);
        public static readonly StyleKey<PDFUnit> BorderCornerRadiusKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"crad", BorderItemKey);
        public static readonly StyleKey<Dash> BorderDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemKey);
        public static readonly StyleKey<LineCaps> BorderEndingKey = StyleKey.InternalCreateStyleValueKey<LineCaps>((ObjectType)"endg", BorderItemKey);
        public static readonly StyleKey<LineJoin> BorderJoinKey = StyleKey.InternalCreateStyleValueKey<LineJoin>((ObjectType)"join", BorderItemKey);
        public static readonly StyleKey<float> BorderMitreKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"mitr", BorderItemKey);
        public static readonly StyleKey<double> BorderOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", BorderItemKey);
        public static readonly StyleKey<Sides> BorderSidesKey = StyleKey.InternalCreateStyleValueKey<Sides>((ObjectType)"side", BorderItemKey);
        public static readonly StyleKey<LineType> BorderStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemKey);
        public static readonly StyleKey<PDFUnit> BorderWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", BorderItemKey);

        //Border Top
        public static readonly StyleKey BorderItemTopKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderTop, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderTopColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemTopKey);
        public static readonly StyleKey<LineType> BorderTopStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemTopKey);
        public static readonly StyleKey<PDFUnit> BorderTopWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", BorderItemTopKey);
        public static readonly StyleKey<Dash> BorderTopDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemTopKey);

        //Border Top
        public static readonly StyleKey BorderItemLeftKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderLeft, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderLeftColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemLeftKey);
        public static readonly StyleKey<LineType> BorderLeftStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemLeftKey);
        public static readonly StyleKey<PDFUnit> BorderLeftWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", BorderItemLeftKey);
        public static readonly StyleKey<Dash> BorderLeftDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemLeftKey);

        //Border Top
        public static readonly StyleKey BorderItemBottomKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderBottom, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderBottomColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemBottomKey);
        public static readonly StyleKey<LineType> BorderBottomStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemBottomKey);
        public static readonly StyleKey<PDFUnit> BorderBottomWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", BorderItemBottomKey);
        public static readonly StyleKey<Dash> BorderBottomDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemBottomKey);

        //Border Top
        public static readonly StyleKey BorderItemRightKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderRight, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderRightColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemRightKey);
        public static readonly StyleKey<LineType> BorderRightStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemRightKey);
        public static readonly StyleKey<PDFUnit> BorderRightWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", BorderItemRightKey);
        public static readonly StyleKey<Dash> BorderRightDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemRightKey);

        // Padding
        public static readonly StyleKey PaddingItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePadding, NOT_INHERITED);
        public static readonly StyleKey<PDFUnit> PaddingTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"top ", PaddingItemKey);
        public static readonly StyleKey<PDFUnit> PaddingBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"botm", PaddingItemKey);
        public static readonly StyleKey<PDFUnit> PaddingLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"left", PaddingItemKey);
        public static readonly StyleKey<PDFUnit> PaddingRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"righ", PaddingItemKey);
        public static readonly StyleKey<PDFUnit> PaddingAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"all ", PaddingItemKey);

        // Margins
        public static readonly StyleKey MarginsItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleMargins, NOT_INHERITED);
        public static readonly StyleKey<PDFUnit> MarginsTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"top ", MarginsItemKey);
        public static readonly StyleKey<PDFUnit> MarginsBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"botm", MarginsItemKey);
        public static readonly StyleKey<PDFUnit> MarginsLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"left", MarginsItemKey);
        public static readonly StyleKey<PDFUnit> MarginsRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"righ", MarginsItemKey);
        public static readonly StyleKey<PDFUnit> MarginsAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"all ", MarginsItemKey);

        // Clip
        public static readonly StyleKey ClipItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleClip, NOT_INHERITED);
        public static readonly StyleKey<PDFUnit> ClipLeftKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"left", ClipItemKey);
        public static readonly StyleKey<PDFUnit> ClipRightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"righ", ClipItemKey);
        public static readonly StyleKey<PDFUnit> ClipTopKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"top ", ClipItemKey);
        public static readonly StyleKey<PDFUnit> ClipBottomKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"botm", ClipItemKey);
        public static readonly StyleKey<PDFUnit> ClipAllKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"all ", ClipItemKey);

        // Columns
        public static readonly StyleKey ColumnItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleColumns, NOT_INHERITED);
        public static readonly StyleKey<int> ColumnCountKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cont", ColumnItemKey);
        public static readonly StyleKey<PDFUnit> ColumnAlleyKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ally", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnFlowKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"flow", ColumnItemKey);
        public static readonly StyleKey<PDFColumnWidths> ColumnWidthKey = StyleKey.InternalCreateStyleValueKey<PDFColumnWidths>((ObjectType)"cwid", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnBreakBeforeKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"brcb", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnBreakAfterKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"brca", ColumnItemKey);

        //Fill
        public static readonly StyleKey FillItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleFill, INHERITED);
        public static readonly StyleKey<Color> FillColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", FillItemKey);
        public static readonly StyleKey<string> FillImgSrcKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"imgs", FillItemKey);
        public static readonly StyleKey<PatternRepeat> FillRepeatKey = StyleKey.InternalCreateStyleValueKey<PatternRepeat>((ObjectType)"rept", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillXPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xpos", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillYPosKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ypos", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillXStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xstp", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillYStepKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ystp", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillXSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xsiz", FillItemKey);
        public static readonly StyleKey<PDFUnit> FillYSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ysiz", FillItemKey);
        public static readonly StyleKey<Drawing.FillType> FillStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((ObjectType)"styl", FillItemKey);
        public static readonly StyleKey<double> FillOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", FillItemKey);

        //Font
        public static readonly StyleKey FontItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleFont, INHERITED);
        public static readonly StyleKey<FontSelector> FontFamilyKey = StyleKey.InternalCreateStyleValueKey<FontSelector>((ObjectType)"faml", FontItemKey);
        public static readonly StyleKey<int> FontWeightKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"weig", FontItemKey);
        public static readonly StyleKey<Drawing.FontStyle> FontStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FontStyle>((ObjectType)"styl", FontItemKey);
        public static readonly StyleKey<PDFUnit> FontSizeKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"size", FontItemKey);

        //Overflow
        public static readonly StyleKey OverflowItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleOverflow, INHERITED);
        public static readonly StyleKey<OverflowAction> OverflowActionKey = StyleKey.InternalCreateStyleValueKey<OverflowAction>((ObjectType)"actn", OverflowItemKey);
        public static readonly StyleKey<OverflowSplit> OverflowSplitKey = StyleKey.InternalCreateStyleValueKey<OverflowSplit>((ObjectType)"splt", OverflowItemKey);

        //Position
        public static readonly StyleKey PositionItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePosition, NOT_INHERITED);
        public static readonly StyleKey<PDFUnit> PositionXKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xpos", PositionItemKey);
        public static readonly StyleKey<PDFUnit> PositionYKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ypos", PositionItemKey);
        public static readonly StyleKey<PositionMode> PositionModeKey = StyleKey.InternalCreateStyleValueKey<PositionMode>((ObjectType)"mode", PositionItemKey);
        public static readonly StyleKey<PDFRect> PositionViewPort = StyleKey.InternalCreateStyleValueKey<PDFRect>((ObjectType)"vwpt", PositionItemKey);
        public static readonly StyleKey<FloatMode> PositionFloat = StyleKey.InternalCreateStyleValueKey<FloatMode>((ObjectType)"flot", PositionItemKey);

        //Size
        public static readonly StyleKey SizeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleSize, NOT_INHERITED);
        public static readonly StyleKey<PDFUnit> SizeWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", SizeItemKey);
        public static readonly StyleKey<PDFUnit> SizeHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"heit", SizeItemKey);
        public static readonly StyleKey<PDFUnit> SizeMinimumWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"minw", SizeItemKey);
        public static readonly StyleKey<PDFUnit> SizeMinimumHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"minh", SizeItemKey);
        public static readonly StyleKey<PDFUnit> SizeMaximumWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"maxw", SizeItemKey);
        public static readonly StyleKey<PDFUnit> SizeMaximumHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"maxh", SizeItemKey);
        public static readonly StyleKey<bool> SizeFullWidthKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"fulw", SizeItemKey);

        //Stroke
        public static readonly StyleKey StrokeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleStroke, INHERITED);
        public static readonly StyleKey<Color> StrokeColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", StrokeItemKey);
        public static readonly StyleKey<Dash> StrokeDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", StrokeItemKey);
        public static readonly StyleKey<LineCaps> StrokeEndingKey = StyleKey.InternalCreateStyleValueKey<LineCaps>((ObjectType)"endg", StrokeItemKey);
        public static readonly StyleKey<LineJoin> StrokeJoinKey = StyleKey.InternalCreateStyleValueKey<LineJoin>((ObjectType)"join", StrokeItemKey);
        public static readonly StyleKey<float> StrokeMitreKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"mitr", StrokeItemKey);
        public static readonly StyleKey<double> StrokeOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", StrokeItemKey);
        public static readonly StyleKey<LineType> StrokeStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", StrokeItemKey);
        public static readonly StyleKey<PDFUnit> StrokeWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widt", StrokeItemKey);

        //Text
        public static readonly StyleKey TextItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleText, INHERITED);
        public static readonly StyleKey<PDFUnit> TextFirstLineIndentKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"indt", TextItemKey);
        public static readonly StyleKey<PDFUnit> TextLeadingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"lead", TextItemKey);
        public static readonly StyleKey<bool> TextWhitespaceKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"whit", TextItemKey);
        public static readonly StyleKey<string> TextDateFormatKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"datf", TextItemKey);
        public static readonly StyleKey<string> TextNumberFormatKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"numf", TextItemKey);
        public static readonly StyleKey<PDFUnit> TextWordSpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"wspc", TextItemKey);
        public static readonly StyleKey<PDFUnit> TextCharSpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"cspc", TextItemKey);
        public static readonly StyleKey<double> TextHorizontalScaling = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"thsc", TextItemKey);
        public static readonly StyleKey<TextDirection> TextDirectionKey = StyleKey.InternalCreateStyleValueKey<TextDirection>((ObjectType)"tdir", TextItemKey);
        public static readonly StyleKey<Scryber.Text.WordWrap> TextWordWrapKey = StyleKey.InternalCreateStyleValueKey<Scryber.Text.WordWrap>((ObjectType)"wrap", TextItemKey);
        public static readonly StyleKey<Scryber.Text.TextDecoration> TextDecorationKey = StyleKey.InternalCreateStyleValueKey<Scryber.Text.TextDecoration>((ObjectType)"decr", TextItemKey);
        public static readonly StyleKey<bool> TextPositionFromBaseline = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"tbse", TextItemKey);
        public static readonly StyleKey<VerticalAlignment> PositionVAlignKey = StyleKey.InternalCreateStyleValueKey<VerticalAlignment>((ObjectType)"vlgn", TextItemKey);
        public static readonly StyleKey<HorizontalAlignment> PositionHAlignKey = StyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((ObjectType)"hlgn", TextItemKey);

        //List
        public static readonly StyleKey ListKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleList, NOT_INHERITED);
        public static readonly StyleKey<string> ListLabelKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"labl", ListKey);
        public static readonly StyleKey<string> ListGroupKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"grup", ListKey);
        public static readonly StyleKey<bool> ListConcatKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"conc", ListKey);
        public static readonly StyleKey<string> ListPrefixKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"pref", ListKey);
        public static readonly StyleKey<string> ListPostfixKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"post", ListKey);
        public static readonly StyleKey<ListNumberingGroupStyle> ListNumberStyleKey = StyleKey.InternalCreateStyleValueKey<ListNumberingGroupStyle>((ObjectType)"styl", ListKey);


        public static readonly StyleKey ListItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleListItem, INHERITED);
        public static readonly StyleKey<PDFUnit> ListInsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"inst", ListItemKey);
        public static readonly StyleKey<HorizontalAlignment> ListAlignmentKey = StyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((ObjectType)"alig", ListItemKey);
        public static readonly StyleKey<PDFUnit> ListAlleyKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"ally", ListItemKey);

        

        //Outline
        public static readonly StyleKey OutlineItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleOutline, NOT_INHERITED);
        public static readonly StyleKey<bool> OutlineIsOutlinedKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"outl", OutlineItemKey);
        public static readonly StyleKey<Color> OutlineColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", OutlineItemKey);
        public static readonly StyleKey<bool> OutlineBoldKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"bold", OutlineItemKey);
        public static readonly StyleKey<bool> OutlineItalicKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"ital", OutlineItemKey);
        public static readonly StyleKey<bool> OutlineOpenKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"open", OutlineItemKey);

        //Overlay Grid
        public static readonly StyleKey OverlayItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleOverlayGrid, NOT_INHERITED);
        public static readonly StyleKey<bool> OverlayShowGridKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"show", OverlayItemKey);
        public static readonly StyleKey<PDFUnit> OverlaySpacingKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"spac", OverlayItemKey);
        public static readonly StyleKey<Color> OverlayColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", OverlayItemKey);
        public static readonly StyleKey<double> OverlayOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", OverlayItemKey);
        public static readonly StyleKey<PDFUnit> OverlayXOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xoff", OverlayItemKey);
        public static readonly StyleKey<PDFUnit> OverlayYOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"yoff", OverlayItemKey);
        public static readonly StyleKey<bool> OverlayShowColumnsKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"colm", OverlayItemKey);

        //Page
        public static readonly StyleKey PageItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePage, NOT_INHERITED);
        public static readonly StyleKey<PaperSize> PagePaperSizeKey = StyleKey.InternalCreateStyleValueKey<PaperSize>((ObjectType)"papr", PageItemKey);
        public static readonly StyleKey<PaperOrientation> PageOrientationKey = StyleKey.InternalCreateStyleValueKey<PaperOrientation>((ObjectType)"orit", PageItemKey);
        public static readonly StyleKey<PDFUnit> PageWidthKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"widh", PageItemKey);
        public static readonly StyleKey<PDFUnit> PageHeightKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"heit", PageItemKey);
        public static readonly StyleKey<int> PageAngle = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"angl", PageItemKey);
        public static readonly StyleKey<PageNumberStyle> PageNumberStyleKey = StyleKey.InternalCreateStyleValueKey<PageNumberStyle>((ObjectType)"styl", PageItemKey);
        public static readonly StyleKey<int> PageNumberStartKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"strt", PageItemKey);
        public static readonly StyleKey<string> PageNumberGroupKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"grup", PageItemKey);
        public static readonly StyleKey<string> PageNumberFormatKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"fomt", PageItemKey);
        public static readonly StyleKey<int> PageNumberGroupHintKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"ghnt", PageItemKey);
        public static readonly StyleKey<int> PageNumberTotalHintKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"thnt", PageItemKey);
        public static readonly StyleKey<bool> PageBreakBeforeKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"bkpb", PageItemKey);
        public static readonly StyleKey<bool> PageBreakAfterKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"bkpa", PageItemKey);
        public static readonly StyleKey<string> PageNameGroupKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"pgnm", PageItemKey);

        //Shape
        public static readonly StyleKey ShapeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.ShapeStyle, NOT_INHERITED);
        public static readonly StyleKey<int> ShapeVertexCountKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"vrtx", ShapeItemKey);
        public static readonly StyleKey<int> ShapeVertexStepKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"step", ShapeItemKey);
        public static readonly StyleKey<bool> ShapeClosedKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"clos", ShapeItemKey);
        public static readonly StyleKey<double> ShapeRotationKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"rotn", ShapeItemKey);

        //Table
        public static readonly StyleKey TableItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleTable, NOT_INHERITED);
        public static readonly StyleKey<int> TableCellColumnSpanKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cols", TableItemKey);
        public static readonly StyleKey<TableRowRepeat> TableRowRepeatKey = StyleKey.InternalCreateStyleValueKey<TableRowRepeat>((ObjectType)"rowr", TableItemKey);

        //Badge
        public static readonly StyleKey BadgeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBadge, NOT_INHERITED);
        public static readonly StyleKey<Corner> BadgeCornerKey = StyleKey.InternalCreateStyleValueKey<Corner>((ObjectType)"corn", BadgeItemKey);
        public static readonly StyleKey<PDFUnit> BadgeXOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"xoff", BadgeItemKey);
        public static readonly StyleKey<PDFUnit> BadgeYOffsetKey = StyleKey.InternalCreateStyleValueKey<PDFUnit>((ObjectType)"yoff", BadgeItemKey);
        public static readonly StyleKey<BadgeType> BadgeDisplayKey = StyleKey.InternalCreateStyleValueKey<BadgeType>((ObjectType)"disp", BadgeItemKey);

        //Modify
        public static readonly StyleKey ModifyPageItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleModify, NOT_INHERITED);
        public static readonly StyleKey<ModificationType> ModifyPageTypeKey = StyleKey.InternalCreateStyleValueKey<ModificationType>((ObjectType)"type", ModifyPageItemKey);
        public static readonly StyleKey<ModifiedContentAction> ModifyPageActionKey = StyleKey.InternalCreateStyleValueKey<ModifiedContentAction>((ObjectType)"actn", ModifyPageItemKey);
        public static readonly StyleKey<int> ModifyPageStartIndexKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"strt", ModifyPageItemKey);
        public static readonly StyleKey<int> ModifyPageCountKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cont", ModifyPageItemKey);


        //Transform
        public static readonly StyleKey TransformItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleTransform, NOT_INHERITED);
        public static readonly StyleKey<float> TransformRotateKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"rote", TransformItemKey);
        public static readonly StyleKey<float> TransformXScaleKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"xscl", TransformItemKey);
        public static readonly StyleKey<float> TransformXSkewKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"xskw", TransformItemKey);
        public static readonly StyleKey<float> TransformXOffsetKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"xoff", TransformItemKey);
        public static readonly StyleKey<float> TransformYScaleKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"yscl", TransformItemKey);
        public static readonly StyleKey<float> TransformYSkewKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"yskw", TransformItemKey);
        public static readonly StyleKey<float> TransformYOffsetKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"yoff", TransformItemKey);
        public static readonly StyleKey<TransformationOrigin> TransformOriginKey = StyleKey.InternalCreateStyleValueKey<TransformationOrigin>((ObjectType)"orig", TransformItemKey);

        //FontFace
        public static readonly StyleKey FontFaceItemKey = StyleKey.InternalCreateStyleItemKey((ObjectType)"csff", NOT_INHERITED);
        public static readonly StyleKey<FontSource> FontFaceSrcKey = StyleKey.InternalCreateStyleValueKey<FontSource>((ObjectType)"fsrc", FontFaceItemKey);
        public static readonly StyleKey<string> FontFaceFamilyKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"ffam", FontFaceItemKey);
        public static readonly StyleKey<int> FontFaceWeightKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"fwgt", FontFaceItemKey);
        public static readonly StyleKey<Drawing.FontStyle> FontFaceStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FontStyle>((ObjectType)"fsyl", FontFaceItemKey);



    }
}
