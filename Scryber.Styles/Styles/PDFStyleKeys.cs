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
    public static class PDFStyleKeys
    {
        private static bool INHERITED = true;
        private static bool NOT_INHERITED = false;

        //
        // thread safe and garunteed to be called only once
        // so we can use the internal (Non thread safe initialization)
        //

        // Background
        public static readonly PDFStyleKey BgItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBackground, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BgColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BgItemKey);
        public static readonly PDFStyleKey<string> BgImgSrcKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"imgs", BgItemKey);
        public static readonly PDFStyleKey<PatternRepeat> BgRepeatKey = PDFStyleKey.InternalCreateStyleValueKey<PatternRepeat>((PDFObjectType)"rept", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXPosKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYPosKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXStepKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xstp", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYStepKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ystp", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgXSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xsiz", BgItemKey);
        public static readonly PDFStyleKey<PDFUnit> BgYSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ysiz", BgItemKey);
        public static readonly PDFStyleKey<FillStyle> BgStyleKey = PDFStyleKey.InternalCreateStyleValueKey<FillStyle>((PDFObjectType)"styl", BgItemKey);
        public static readonly PDFStyleKey<double> BgOpacityKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", BgItemKey);

        //Border
        public static readonly PDFStyleKey BorderItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBorder, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFColor> BorderColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", BorderItemKey);
        public static readonly PDFStyleKey<PDFUnit> BorderCornerRadiusKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"crad", BorderItemKey);
        public static readonly PDFStyleKey<PDFDash> BorderDashKey = PDFStyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", BorderItemKey);
        public static readonly PDFStyleKey<LineCaps> BorderEndingKey = PDFStyleKey.InternalCreateStyleValueKey<LineCaps>((PDFObjectType)"endg", BorderItemKey);
        public static readonly PDFStyleKey<LineJoin> BorderJoinKey = PDFStyleKey.InternalCreateStyleValueKey<LineJoin>((PDFObjectType)"join", BorderItemKey);
        public static readonly PDFStyleKey<float> BorderMitreKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"mitr", BorderItemKey);
        public static readonly PDFStyleKey<double> BorderOpacityKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", BorderItemKey);
        public static readonly PDFStyleKey<Sides> BorderSidesKey = PDFStyleKey.InternalCreateStyleValueKey<Sides>((PDFObjectType)"side", BorderItemKey);
        public static readonly PDFStyleKey<LineStyle> BorderStyleKey = PDFStyleKey.InternalCreateStyleValueKey<LineStyle>((PDFObjectType)"styl", BorderItemKey);
        public static readonly PDFStyleKey<PDFUnit> BorderWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", BorderItemKey);

        // Padding
        public static readonly PDFStyleKey PaddingItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePadding, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> PaddingTopKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingBottomKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingLeftKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingRightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", PaddingItemKey);
        public static readonly PDFStyleKey<PDFUnit> PaddingAllKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", PaddingItemKey);

        // Margins
        public static readonly PDFStyleKey MarginsItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleMargins, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> MarginsTopKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsBottomKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsLeftKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsRightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", MarginsItemKey);
        public static readonly PDFStyleKey<PDFUnit> MarginsAllKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", MarginsItemKey);

        // Clip
        public static readonly PDFStyleKey ClipItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleClip, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> ClipLeftKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"left", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipRightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"righ", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipTopKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"top ", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipBottomKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"botm", ClipItemKey);
        public static readonly PDFStyleKey<PDFUnit> ClipAllKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"all ", ClipItemKey);

        // Columns
        public static readonly PDFStyleKey ColumnItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleColumns, NOT_INHERITED);
        public static readonly PDFStyleKey<int> ColumnCountKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cont", ColumnItemKey);
        public static readonly PDFStyleKey<PDFUnit> ColumnAlleyKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ally", ColumnItemKey);
        public static readonly PDFStyleKey<bool> ColumnFlowKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"flow", ColumnItemKey);
        public static readonly PDFStyleKey<PDFColumnWidths> ColumnWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColumnWidths>((PDFObjectType)"cwid",ColumnItemKey);
        //Fill
        public static readonly PDFStyleKey FillItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleFill, INHERITED);
        public static readonly PDFStyleKey<PDFColor> FillColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", FillItemKey);
        public static readonly PDFStyleKey<string> FillImgSrcKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"imgs", FillItemKey);
        public static readonly PDFStyleKey<PatternRepeat> FillRepeatKey = PDFStyleKey.InternalCreateStyleValueKey<PatternRepeat>((PDFObjectType)"rept", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXPosKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYPosKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXStepKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xstp", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYStepKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ystp", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillXSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xsiz", FillItemKey);
        public static readonly PDFStyleKey<PDFUnit> FillYSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ysiz", FillItemKey);
        public static readonly PDFStyleKey<FillStyle> FillStyleKey = PDFStyleKey.InternalCreateStyleValueKey<FillStyle>((PDFObjectType)"styl", FillItemKey);
        public static readonly PDFStyleKey<double> FillOpacityKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", FillItemKey);

        //Font
        public static readonly PDFStyleKey FontItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleFont, INHERITED);
        public static readonly PDFStyleKey<string> FontFamilyKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"faml", FontItemKey);
        public static readonly PDFStyleKey<bool> FontBoldKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bold", FontItemKey);
        public static readonly PDFStyleKey<bool> FontItalicKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"ital", FontItemKey);
        public static readonly PDFStyleKey<PDFUnit> FontSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"size", FontItemKey);

        //Overflow
        public static readonly PDFStyleKey OverflowItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOverflow, INHERITED);
        public static readonly PDFStyleKey<OverflowAction> OverflowActionKey = PDFStyleKey.InternalCreateStyleValueKey<OverflowAction>((PDFObjectType)"actn", OverflowItemKey);
        public static readonly PDFStyleKey<OverflowSplit> OverflowSplitKey = PDFStyleKey.InternalCreateStyleValueKey<OverflowSplit>((PDFObjectType)"splt", OverflowItemKey);

        //Position
        public static readonly PDFStyleKey PositionItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePosition, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> PositionXKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xpos", PositionItemKey);
        public static readonly PDFStyleKey<PDFUnit> PositionYKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"ypos", PositionItemKey);
        public static readonly PDFStyleKey<PositionMode> PositionModeKey = PDFStyleKey.InternalCreateStyleValueKey<PositionMode>((PDFObjectType)"mode", PositionItemKey);
        public static readonly PDFStyleKey<VerticalAlignment> PositionVAlignKey = PDFStyleKey.InternalCreateStyleValueKey<VerticalAlignment>((PDFObjectType)"vlgn", PositionItemKey);
        public static readonly PDFStyleKey<HorizontalAlignment> PositionHAlignKey = PDFStyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((PDFObjectType)"hlgn", PositionItemKey);
        
        //Size
        public static readonly PDFStyleKey SizeItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleSize, NOT_INHERITED);
        public static readonly PDFStyleKey<PDFUnit> SizeWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeHeightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"heit", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMinimumWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"minw", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMinimumHeightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"minh", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMaximumWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"maxw", SizeItemKey);
        public static readonly PDFStyleKey<PDFUnit> SizeMaximumHeightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"maxh", SizeItemKey);
        public static readonly PDFStyleKey<bool> SizeFullWidthKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"fulw", SizeItemKey);

        //Stroke
        public static readonly PDFStyleKey StrokeItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleStroke, INHERITED);
        public static readonly PDFStyleKey<PDFColor> StrokeColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", StrokeItemKey);
        public static readonly PDFStyleKey<PDFDash> StrokeDashKey = PDFStyleKey.InternalCreateStyleValueKey<PDFDash>((PDFObjectType)"dash", StrokeItemKey);
        public static readonly PDFStyleKey<LineCaps> StrokeEndingKey = PDFStyleKey.InternalCreateStyleValueKey<LineCaps>((PDFObjectType)"endg", StrokeItemKey);
        public static readonly PDFStyleKey<LineJoin> StrokeJoinKey = PDFStyleKey.InternalCreateStyleValueKey<LineJoin>((PDFObjectType)"join", StrokeItemKey);
        public static readonly PDFStyleKey<float> StrokeMitreKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"mitr", StrokeItemKey);
        public static readonly PDFStyleKey<double> StrokeOpacityKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", StrokeItemKey);
        public static readonly PDFStyleKey<LineStyle> StrokeStyleKey = PDFStyleKey.InternalCreateStyleValueKey<LineStyle>((PDFObjectType)"styl", StrokeItemKey);
        public static readonly PDFStyleKey<PDFUnit> StrokeWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widt", StrokeItemKey);

        //Text
        public static readonly PDFStyleKey TextItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleText, INHERITED);
        public static readonly PDFStyleKey<PDFUnit> TextFirstLineIndentKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"indt", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextLeadingKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"lead", TextItemKey);
        public static readonly PDFStyleKey<bool> TextWhitespaceKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"whit", TextItemKey);
        public static readonly PDFStyleKey<string> TextDateFormatKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"datf", TextItemKey);
        public static readonly PDFStyleKey<string> TextNumberFormatKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"numf", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextWordSpacingKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"wspc", TextItemKey);
        public static readonly PDFStyleKey<PDFUnit> TextCharSpacingKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"cspc", TextItemKey);
        public static readonly PDFStyleKey<double> TextHorizontalScaling = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"thsc", TextItemKey);
        public static readonly PDFStyleKey<TextDirection> TextDirectionKey = PDFStyleKey.InternalCreateStyleValueKey<TextDirection>((PDFObjectType)"tdir", TextItemKey);
        public static readonly PDFStyleKey<Scryber.Text.WordWrap> TextWordWrapKey = PDFStyleKey.InternalCreateStyleValueKey<Scryber.Text.WordWrap>((PDFObjectType)"wrap", TextItemKey);
        public static readonly PDFStyleKey<Scryber.Text.TextDecoration> TextDecorationKey = PDFStyleKey.InternalCreateStyleValueKey<Scryber.Text.TextDecoration>((PDFObjectType)"decr", TextItemKey);
        
        //List
        public static readonly PDFStyleKey ListItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleList, NOT_INHERITED);
        public static readonly PDFStyleKey<ListNumberingGroupStyle> ListNumberStyleKey = PDFStyleKey.InternalCreateStyleValueKey<ListNumberingGroupStyle>((PDFObjectType)"styl", ListItemKey);
        public static readonly PDFStyleKey<string> ListGroupKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"grup", ListItemKey);
        public static readonly PDFStyleKey<PDFUnit> ListInsetKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"inst", ListItemKey);
        public static readonly PDFStyleKey<string> ListPrefixKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"pref", ListItemKey);
        public static readonly PDFStyleKey<string> ListPostfixKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"post", ListItemKey);
        public static readonly PDFStyleKey<HorizontalAlignment> ListAlignmentKey = PDFStyleKey.InternalCreateStyleValueKey<HorizontalAlignment>((PDFObjectType)"alig", ListItemKey);
        public static readonly PDFStyleKey<bool> ListConcatKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"conc", ListItemKey);
        public static readonly PDFStyleKey<string> ListLabelKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"labl", ListItemKey);

        //Outline
        public static readonly PDFStyleKey OutlineItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOutline, NOT_INHERITED);
        public static readonly PDFStyleKey<bool> OutlineIsOutlinedKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"outl", OutlineItemKey);
        public static readonly PDFStyleKey<PDFColor> OutlineColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineBoldKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"bold", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineItalicKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"ital", OutlineItemKey);
        public static readonly PDFStyleKey<bool> OutlineOpenKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"open", OutlineItemKey);

        //Overlay Grid
        public static readonly PDFStyleKey OverlayItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleOverlayGrid, NOT_INHERITED);
        public static readonly PDFStyleKey<bool> OverlayShowGridKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"show", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlaySpacingKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"spac", OverlayItemKey);
        public static readonly PDFStyleKey<PDFColor> OverlayColorKey = PDFStyleKey.InternalCreateStyleValueKey<PDFColor>((PDFObjectType)"colr", OverlayItemKey);
        public static readonly PDFStyleKey<double> OverlayOpacityKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"opac", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlayXOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xoff", OverlayItemKey);
        public static readonly PDFStyleKey<PDFUnit> OverlayYOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"yoff", OverlayItemKey);
        public static readonly PDFStyleKey<bool> OverlayShowColumnsKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"colm", OverlayItemKey);

        //Page
        public static readonly PDFStyleKey PageItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StylePage, NOT_INHERITED);
        public static readonly PDFStyleKey<PaperSize> PagePaperSizeKey = PDFStyleKey.InternalCreateStyleValueKey<PaperSize>((PDFObjectType)"papr", PageItemKey);
        public static readonly PDFStyleKey<PaperOrientation> PageOrientationKey = PDFStyleKey.InternalCreateStyleValueKey<PaperOrientation>((PDFObjectType)"orit", PageItemKey);
        public static readonly PDFStyleKey<PDFUnit> PageWidthKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"widh", PageItemKey);
        public static readonly PDFStyleKey<PDFUnit> PageHeightKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"heit", PageItemKey);
        public static readonly PDFStyleKey<int> PageAngle = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"angl", PageItemKey);
        public static readonly PDFStyleKey<PageNumberStyle> PageNumberStyleKey = PDFStyleKey.InternalCreateStyleValueKey<PageNumberStyle>((PDFObjectType)"styl", PageItemKey);
        public static readonly PDFStyleKey<string> PageNumberPrefixKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"pref", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberStartKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"strt", PageItemKey);
        public static readonly PDFStyleKey<string> PageNumberGroupKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"grup", PageItemKey);
        public static readonly PDFStyleKey<string> PageNumberFormatKey = PDFStyleKey.InternalCreateStyleValueKey<string>((PDFObjectType)"fomt", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberGroupHintKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"ghnt", PageItemKey);
        public static readonly PDFStyleKey<int> PageNumberTotalHintKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"thnt", PageItemKey);

        //Shape
        public static readonly PDFStyleKey ShapeItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.ShapeStyle, NOT_INHERITED);
        public static readonly PDFStyleKey<int> ShapeVertexCountKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"vrtx", ShapeItemKey);
        public static readonly PDFStyleKey<int> ShapeVertexStepKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"step", ShapeItemKey);
        public static readonly PDFStyleKey<bool> ShapeClosedKey = PDFStyleKey.InternalCreateStyleValueKey<bool>((PDFObjectType)"clos", ShapeItemKey);
        public static readonly PDFStyleKey<double> ShapeRotationKey = PDFStyleKey.InternalCreateStyleValueKey<double>((PDFObjectType)"rotn", ShapeItemKey);

        //Table
        public static readonly PDFStyleKey TableItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleTable, NOT_INHERITED);
        public static readonly PDFStyleKey<int> TableCellColumnSpanKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cols", TableItemKey);
        public static readonly PDFStyleKey<TableRowRepeat> TableRowRepeatKey = PDFStyleKey.InternalCreateStyleValueKey<TableRowRepeat>((PDFObjectType)"rowr", TableItemKey);

        //Badge
        public static readonly PDFStyleKey BadgeItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleBadge, NOT_INHERITED);
        public static readonly PDFStyleKey<Corner> BadgeCornerKey = PDFStyleKey.InternalCreateStyleValueKey<Corner>((PDFObjectType)"corn", BadgeItemKey);
        public static readonly PDFStyleKey<PDFUnit> BadgeXOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"xoff", BadgeItemKey);
        public static readonly PDFStyleKey<PDFUnit> BadgeYOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<PDFUnit>((PDFObjectType)"yoff", BadgeItemKey);
        public static readonly PDFStyleKey<BadgeType> BadgeDisplayKey = PDFStyleKey.InternalCreateStyleValueKey<BadgeType>((PDFObjectType)"disp", BadgeItemKey);

        //Modify
        public static readonly PDFStyleKey ModifyPageItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleModify, NOT_INHERITED);
        public static readonly PDFStyleKey<ModificationType> ModifyPageTypeKey = PDFStyleKey.InternalCreateStyleValueKey<ModificationType>((PDFObjectType)"type", ModifyPageItemKey);
        public static readonly PDFStyleKey<ModifiedContentAction> ModifyPageActionKey = PDFStyleKey.InternalCreateStyleValueKey<ModifiedContentAction>((PDFObjectType)"actn", ModifyPageItemKey);
        public static readonly PDFStyleKey<int> ModifyPageStartIndexKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"strt", ModifyPageItemKey);
        public static readonly PDFStyleKey<int> ModifyPageCountKey = PDFStyleKey.InternalCreateStyleValueKey<int>((PDFObjectType)"cont", ModifyPageItemKey);


        //Transform
        public static readonly PDFStyleKey TransformItemKey = PDFStyleKey.InternalCreateStyleItemKey(PDFObjectTypes.StyleTransform, NOT_INHERITED);
        public static readonly PDFStyleKey<float> TransformRotateKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"rote", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXScaleKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xscl", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXSkewKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xskw", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformXOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"xoff", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYScaleKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yscl", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYSkewKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yskw", TransformItemKey);
        public static readonly PDFStyleKey<float> TransformYOffsetKey = PDFStyleKey.InternalCreateStyleValueKey<float>((PDFObjectType)"yoff", TransformItemKey);
        public static readonly PDFStyleKey<TransformationOrigin> TransformOriginKey = PDFStyleKey.InternalCreateStyleValueKey<TransformationOrigin>((PDFObjectType)"orig", TransformItemKey);

    }
}
