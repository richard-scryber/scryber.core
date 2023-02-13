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
        public static readonly StyleKey<Unit> BgXPosKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xpos", BgItemKey);
        public static readonly StyleKey<Unit> BgYPosKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ypos", BgItemKey);
        public static readonly StyleKey<Unit> BgXStepKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xstp", BgItemKey);
        public static readonly StyleKey<Unit> BgYStepKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ystp", BgItemKey);
        public static readonly StyleKey<Unit> BgXSizeKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xsiz", BgItemKey);
        public static readonly StyleKey<Unit> BgYSizeKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ysiz", BgItemKey);
        public static readonly StyleKey<Drawing.FillType> BgStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((ObjectType)"styl", BgItemKey);
        public static readonly StyleKey<double> BgOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", BgItemKey);

        //Border
        public static readonly StyleKey BorderItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorder, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemKey);
        public static readonly StyleKey<Unit> BorderCornerRadiusKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"crad", BorderItemKey);
        public static readonly StyleKey<Dash> BorderDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemKey);
        public static readonly StyleKey<LineCaps> BorderEndingKey = StyleKey.InternalCreateStyleValueKey<LineCaps>((ObjectType)"endg", BorderItemKey);
        public static readonly StyleKey<LineJoin> BorderJoinKey = StyleKey.InternalCreateStyleValueKey<LineJoin>((ObjectType)"join", BorderItemKey);
        public static readonly StyleKey<float> BorderMitreKey = StyleKey.InternalCreateStyleValueKey<float>((ObjectType)"mitr", BorderItemKey);
        public static readonly StyleKey<double> BorderOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", BorderItemKey);
        public static readonly StyleKey<Sides> BorderSidesKey = StyleKey.InternalCreateStyleValueKey<Sides>((ObjectType)"side", BorderItemKey);
        public static readonly StyleKey<LineType> BorderStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemKey);
        public static readonly StyleKey<Unit> BorderWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", BorderItemKey);

        //Border Top
        public static readonly StyleKey BorderItemTopKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderTop, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderTopColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemTopKey);
        public static readonly StyleKey<LineType> BorderTopStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemTopKey);
        public static readonly StyleKey<Unit> BorderTopWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", BorderItemTopKey);
        public static readonly StyleKey<Dash> BorderTopDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemTopKey);

        //Border Top
        public static readonly StyleKey BorderItemLeftKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderLeft, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderLeftColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemLeftKey);
        public static readonly StyleKey<LineType> BorderLeftStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemLeftKey);
        public static readonly StyleKey<Unit> BorderLeftWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", BorderItemLeftKey);
        public static readonly StyleKey<Dash> BorderLeftDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemLeftKey);

        //Border Top
        public static readonly StyleKey BorderItemBottomKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderBottom, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderBottomColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemBottomKey);
        public static readonly StyleKey<LineType> BorderBottomStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemBottomKey);
        public static readonly StyleKey<Unit> BorderBottomWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", BorderItemBottomKey);
        public static readonly StyleKey<Dash> BorderBottomDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemBottomKey);

        //Border Top
        public static readonly StyleKey BorderItemRightKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBorderRight, NOT_INHERITED);
        public static readonly StyleKey<Color> BorderRightColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", BorderItemRightKey);
        public static readonly StyleKey<LineType> BorderRightStyleKey = StyleKey.InternalCreateStyleValueKey<LineType>((ObjectType)"styl", BorderItemRightKey);
        public static readonly StyleKey<Unit> BorderRightWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", BorderItemRightKey);
        public static readonly StyleKey<Dash> BorderRightDashKey = StyleKey.InternalCreateStyleValueKey<Dash>((ObjectType)"dash", BorderItemRightKey);

        // Padding
        public static readonly StyleKey PaddingItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePadding, NOT_INHERITED);
        public static readonly StyleKey<Unit> PaddingTopKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"top ", PaddingItemKey);
        public static readonly StyleKey<Unit> PaddingBottomKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"botm", PaddingItemKey);
        public static readonly StyleKey<Unit> PaddingLeftKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"left", PaddingItemKey);
        public static readonly StyleKey<Unit> PaddingRightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"righ", PaddingItemKey);
        public static readonly StyleKey<Unit> PaddingAllKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"all ", PaddingItemKey);

        // Margins
        public static readonly StyleKey MarginsItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleMargins, NOT_INHERITED);
        public static readonly StyleKey<Unit> MarginsTopKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"top ", MarginsItemKey);
        public static readonly StyleKey<Unit> MarginsBottomKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"botm", MarginsItemKey);
        public static readonly StyleKey<Unit> MarginsLeftKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"left", MarginsItemKey);
        public static readonly StyleKey<Unit> MarginsRightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"righ", MarginsItemKey);
        public static readonly StyleKey<Unit> MarginsAllKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"all ", MarginsItemKey);

        // Clip
        public static readonly StyleKey ClipItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleClip, NOT_INHERITED);
        public static readonly StyleKey<Unit> ClipLeftKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"left", ClipItemKey);
        public static readonly StyleKey<Unit> ClipRightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"righ", ClipItemKey);
        public static readonly StyleKey<Unit> ClipTopKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"top ", ClipItemKey);
        public static readonly StyleKey<Unit> ClipBottomKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"botm", ClipItemKey);
        public static readonly StyleKey<Unit> ClipAllKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"all ", ClipItemKey);

        // Columns
        public static readonly StyleKey ColumnItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleColumns, NOT_INHERITED);
        public static readonly StyleKey<int> ColumnCountKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cont", ColumnItemKey);
        public static readonly StyleKey<Unit> ColumnAlleyKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ally", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnFlowKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"flow", ColumnItemKey);
        public static readonly StyleKey<ColumnWidths> ColumnWidthKey = StyleKey.InternalCreateStyleValueKey<ColumnWidths>((ObjectType)"cwid", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnBreakBeforeKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"brcb", ColumnItemKey);
        public static readonly StyleKey<bool> ColumnBreakAfterKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"brca", ColumnItemKey);

        //Fill
        public static readonly StyleKey FillItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleFill, INHERITED);
        public static readonly StyleKey<Color> FillColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", FillItemKey);
        public static readonly StyleKey<string> FillImgSrcKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"imgs", FillItemKey);
        public static readonly StyleKey<PatternRepeat> FillRepeatKey = StyleKey.InternalCreateStyleValueKey<PatternRepeat>((ObjectType)"rept", FillItemKey);
        public static readonly StyleKey<Unit> FillXPosKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xpos", FillItemKey);
        public static readonly StyleKey<Unit> FillYPosKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ypos", FillItemKey);
        public static readonly StyleKey<Unit> FillXStepKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xstp", FillItemKey);
        public static readonly StyleKey<Unit> FillYStepKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ystp", FillItemKey);
        public static readonly StyleKey<Unit> FillXSizeKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xsiz", FillItemKey);
        public static readonly StyleKey<Unit> FillYSizeKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ysiz", FillItemKey);
        public static readonly StyleKey<Drawing.FillType> FillStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FillType>((ObjectType)"styl", FillItemKey);
        public static readonly StyleKey<double> FillOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", FillItemKey);

        //Font
        public static readonly StyleKey FontItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleFont, INHERITED);
        public static readonly StyleKey<FontSelector> FontFamilyKey = StyleKey.InternalCreateStyleValueKey<FontSelector>((ObjectType)"faml", FontItemKey);
        public static readonly StyleKey<int> FontWeightKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"weig", FontItemKey);
        public static readonly StyleKey<Drawing.FontStyle> FontStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FontStyle>((ObjectType)"styl", FontItemKey);
        public static readonly StyleKey<Unit> FontSizeKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"size", FontItemKey);

        //Overflow
        public static readonly StyleKey OverflowItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleOverflow, NOT_INHERITED);
        public static readonly StyleKey<OverflowAction> OverflowActionKey = StyleKey.InternalCreateStyleValueKey<OverflowAction>((ObjectType)"actn", OverflowItemKey);
        public static readonly StyleKey<OverflowSplit> OverflowSplitKey = StyleKey.InternalCreateStyleValueKey<OverflowSplit>((ObjectType)"splt", OverflowItemKey);

        //Position
        public static readonly StyleKey PositionItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePosition, NOT_INHERITED);
        public static readonly StyleKey<Unit> PositionXKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xpos", PositionItemKey);
        public static readonly StyleKey<Unit> PositionYKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ypos", PositionItemKey);
        public static readonly StyleKey<PositionMode> PositionModeKey = StyleKey.InternalCreateStyleValueKey<PositionMode>((ObjectType)"mode", PositionItemKey);
        public static readonly StyleKey<Rect> PositionViewPort = StyleKey.InternalCreateStyleValueKey<Rect>((ObjectType)"vwpt", PositionItemKey);
        public static readonly StyleKey<FloatMode> PositionFloat = StyleKey.InternalCreateStyleValueKey<FloatMode>((ObjectType)"flot", PositionItemKey);

        //Size
        public static readonly StyleKey SizeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleSize, NOT_INHERITED);
        public static readonly StyleKey<Unit> SizeWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", SizeItemKey);
        public static readonly StyleKey<Unit> SizeHeightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"heit", SizeItemKey);
        public static readonly StyleKey<Unit> SizeMinimumWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"minw", SizeItemKey);
        public static readonly StyleKey<Unit> SizeMinimumHeightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"minh", SizeItemKey);
        public static readonly StyleKey<Unit> SizeMaximumWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"maxw", SizeItemKey);
        public static readonly StyleKey<Unit> SizeMaximumHeightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"maxh", SizeItemKey);
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
        public static readonly StyleKey<Unit> StrokeWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widt", StrokeItemKey);

        //Text
        public static readonly StyleKey TextItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleText, INHERITED);
        public static readonly StyleKey<Unit> TextFirstLineIndentKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"indt", TextItemKey);
        public static readonly StyleKey<Unit> TextLeadingKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"lead", TextItemKey);
        public static readonly StyleKey<bool> TextWhitespaceKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"whit", TextItemKey);
        public static readonly StyleKey<string> TextDateFormatKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"datf", TextItemKey);
        public static readonly StyleKey<string> TextNumberFormatKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"numf", TextItemKey);
        public static readonly StyleKey<Unit> TextWordSpacingKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"wspc", TextItemKey);
        public static readonly StyleKey<Unit> TextCharSpacingKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"cspc", TextItemKey);
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
        public static readonly StyleKey<Unit> ListInsetKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"inst", ListItemKey);
        public static readonly StyleKey<HorizontalAlignment> ListAlignmentKey = StyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((ObjectType)"alig", ListItemKey);
        public static readonly StyleKey<Unit> ListAlleyKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"ally", ListItemKey);

        

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
        public static readonly StyleKey<Unit> OverlaySpacingKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"spac", OverlayItemKey);
        public static readonly StyleKey<Color> OverlayColorKey = StyleKey.InternalCreateStyleValueKey<Color>((ObjectType)"colr", OverlayItemKey);
        public static readonly StyleKey<double> OverlayOpacityKey = StyleKey.InternalCreateStyleValueKey<double>((ObjectType)"opac", OverlayItemKey);
        public static readonly StyleKey<Unit> OverlayXOffsetKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xoff", OverlayItemKey);
        public static readonly StyleKey<Unit> OverlayYOffsetKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"yoff", OverlayItemKey);
        public static readonly StyleKey<bool> OverlayShowColumnsKey = StyleKey.InternalCreateStyleValueKey<bool>((ObjectType)"colm", OverlayItemKey);

        //Page
        public static readonly StyleKey PageItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StylePage, NOT_INHERITED);
        public static readonly StyleKey<PaperSize> PagePaperSizeKey = StyleKey.InternalCreateStyleValueKey<PaperSize>((ObjectType)"papr", PageItemKey);
        public static readonly StyleKey<PaperOrientation> PageOrientationKey = StyleKey.InternalCreateStyleValueKey<PaperOrientation>((ObjectType)"orit", PageItemKey);
        public static readonly StyleKey<Unit> PageWidthKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"widh", PageItemKey);
        public static readonly StyleKey<Unit> PageHeightKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"heit", PageItemKey);
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

        public static readonly StyleKey<Unit> ShapeCornerRadiusXKey =
            StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType) "radX", ShapeItemKey);
        public static readonly StyleKey<Unit> ShapeCornerRadiusYKey =
            StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType) "radY", ShapeItemKey);

        //Table
        public static readonly StyleKey TableItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleTable, NOT_INHERITED);
        public static readonly StyleKey<int> TableCellColumnSpanKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cols", TableItemKey);
        public static readonly StyleKey<TableRowRepeat> TableRowRepeatKey = StyleKey.InternalCreateStyleValueKey<TableRowRepeat>((ObjectType)"rowr", TableItemKey);

        //Badge
        public static readonly StyleKey BadgeItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleBadge, NOT_INHERITED);
        public static readonly StyleKey<Corner> BadgeCornerKey = StyleKey.InternalCreateStyleValueKey<Corner>((ObjectType)"corn", BadgeItemKey);
        public static readonly StyleKey<Unit> BadgeXOffsetKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"xoff", BadgeItemKey);
        public static readonly StyleKey<Unit> BadgeYOffsetKey = StyleKey.InternalCreateStyleValueKey<Unit>((ObjectType)"yoff", BadgeItemKey);
        public static readonly StyleKey<BadgeType> BadgeDisplayKey = StyleKey.InternalCreateStyleValueKey<BadgeType>((ObjectType)"disp", BadgeItemKey);

        //Modify
        public static readonly StyleKey ModifyPageItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleModify, NOT_INHERITED);
        public static readonly StyleKey<ModificationType> ModifyPageTypeKey = StyleKey.InternalCreateStyleValueKey<ModificationType>((ObjectType)"type", ModifyPageItemKey);
        public static readonly StyleKey<ModifiedContentAction> ModifyPageActionKey = StyleKey.InternalCreateStyleValueKey<ModifiedContentAction>((ObjectType)"actn", ModifyPageItemKey);
        public static readonly StyleKey<int> ModifyPageStartIndexKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"strt", ModifyPageItemKey);
        public static readonly StyleKey<int> ModifyPageCountKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"cont", ModifyPageItemKey);


        //Transform
        public static readonly StyleKey TransformItemKey = StyleKey.InternalCreateStyleItemKey(ObjectTypes.StyleTransform, NOT_INHERITED);
        public static readonly StyleKey<TransformOperation> TransformOperationKey = StyleKey.InternalCreateStyleValueKey<TransformOperation>((ObjectType)"trop", TransformItemKey);
        public static readonly StyleKey<TransformationOrigin> TransformOriginKey = StyleKey.InternalCreateStyleValueKey<TransformationOrigin>((ObjectType)"orig", TransformItemKey);

        //FontFace
        public static readonly StyleKey FontFaceItemKey = StyleKey.InternalCreateStyleItemKey((ObjectType)"csff", NOT_INHERITED);
        public static readonly StyleKey<FontSource> FontFaceSrcKey = StyleKey.InternalCreateStyleValueKey<FontSource>((ObjectType)"fsrc", FontFaceItemKey);
        public static readonly StyleKey<string> FontFaceFamilyKey = StyleKey.InternalCreateStyleValueKey<string>((ObjectType)"ffam", FontFaceItemKey);
        public static readonly StyleKey<int> FontFaceWeightKey = StyleKey.InternalCreateStyleValueKey<int>((ObjectType)"fwgt", FontFaceItemKey);
        public static readonly StyleKey<Drawing.FontStyle> FontFaceStyleKey = StyleKey.InternalCreateStyleValueKey<Drawing.FontStyle>((ObjectType)"fsyl", FontFaceItemKey);

        //Content
        public static readonly StyleKey ContentItemKey = StyleKey.InternalCreateStyleItemKey((ObjectType)"cont", NOT_INHERITED);
        public static readonly StyleKey<ContentDescriptor> ContentTextKey = StyleKey.InternalCreateStyleValueKey<ContentDescriptor>((ObjectType)"ctxt", ContentItemKey);

        //Counter
        public static readonly StyleKey CounterItemKey = StyleKey.InternalCreateStyleItemKey((ObjectType)"coun", NOT_INHERITED);
        public static readonly StyleKey<CounterStyleValue> CounterIncrementKey = StyleKey.InternalCreateStyleValueKey<CounterStyleValue>((ObjectType)"cnin", CounterItemKey);
        public static readonly StyleKey<CounterStyleValue> CounterResetKey = StyleKey.InternalCreateStyleValueKey<CounterStyleValue>((ObjectType)"cnre", CounterItemKey);

    }
}
