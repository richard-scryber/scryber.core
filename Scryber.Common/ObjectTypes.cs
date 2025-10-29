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
using System.Text;

namespace Scryber
{
    /// <summary>
    /// Standard ObjectType values. Full uppercase are reserved for native objects (Number, Array, etc).
    /// Full lower case are reserved for internal known types.
    /// </summary>
    public static class ObjectTypes
    {
        //Native

        public static readonly ObjectType Number = (ObjectType)"NUM ";
        public static readonly ObjectType Array = (ObjectType)"ARRY";
        public static readonly ObjectType String = (ObjectType)"STR_";
        public static readonly ObjectType Dictionary = (ObjectType)"DICT";
        public static readonly ObjectType Object = (ObjectType)"OBJ ";
        public static readonly ObjectType ObjectRef = (ObjectType)"OREF";
        public static readonly ObjectType Name = (ObjectType)"NAME";
        public static readonly ObjectType Real = (ObjectType)"REAL";
        public static readonly ObjectType Stream = (ObjectType)"STEA";
        public static readonly ObjectType Null = (ObjectType)"NULL";
        public static readonly ObjectType Boolean = (ObjectType)"BOOL";
        public static readonly ObjectType IndirectObject = (ObjectType)"IOBJ";
        public static readonly ObjectType NoOp = (ObjectType)"NOOP";
        public static readonly ObjectType File = (ObjectType)"FILE";
        
        //Drawing

        public static readonly ObjectType Color = (ObjectType)"colr";
        public static readonly ObjectType Thickness = (ObjectType)"thck";
        public static readonly ObjectType ProcSet = (ObjectType)"proc";
        public static readonly ObjectType Font = (ObjectType)"font";
        public static readonly ObjectType FontResource = (ObjectType)"frsc";
        public static readonly ObjectType FontWidths = (ObjectType)"fwdt";
        public static readonly ObjectType FontDescriptor = (ObjectType)"fdec";
        public static readonly ObjectType ImageXObject = (ObjectType)"imgx";
        public static readonly ObjectType CanvasXObject = (ObjectType)"canx";
        public static readonly ObjectType ExtGState = (ObjectType)"gxst";
        public static readonly ObjectType GraphicsPath = (ObjectType)"gpth";
        public static readonly ObjectType Pattern = (ObjectType)"patt";
        public static readonly ObjectType PatternLayout = (ObjectType)"patl";

        //Components

        public static readonly ObjectType Document = (ObjectType)"doc_";
        public static readonly ObjectType Text = (ObjectType)"text";
        public static readonly ObjectType Whitespace = (ObjectType)"whsp";
        public static readonly ObjectType Label = (ObjectType)"labl";
        public static readonly ObjectType Field = (ObjectType)"fild";
        public static readonly ObjectType Page = (ObjectType)"page";
        public static readonly ObjectType PageGroup = (ObjectType)"pggp";
        public static readonly ObjectType Section = (ObjectType)"sect";
        public static readonly ObjectType PageBreak = (ObjectType)"pgbk";
        public static readonly ObjectType ColumnBreak = (ObjectType)"clbr";
        public static readonly ObjectType LineBreak = (ObjectType)"lnbr";
        public static readonly ObjectType PageSize = (ObjectType)"pgsz";
        public static readonly ObjectType Image = (ObjectType)"img ";
        public static readonly ObjectType ImageData = (ObjectType)"imgd";
        public static readonly ObjectType GraphicsPathData = (ObjectType)"gphd";
        public static readonly ObjectType Table = (ObjectType)"tble";

        public static readonly ObjectType TableRow = (ObjectType)"trow";
        public static readonly ObjectType TableHeaderRow = (ObjectType)"thrw";
        public static readonly ObjectType TableFooterRow = (ObjectType)"tfrw";

        public static readonly ObjectType List = (ObjectType)"list";
        public static readonly ObjectType OrderedList = (ObjectType)"orli";
        public static readonly ObjectType UnorderedList = (ObjectType)"unli";
        public static readonly ObjectType DefinitionList = (ObjectType)"dfli";
        public static readonly ObjectType ListItem = (ObjectType)"litm";
        public static readonly ObjectType ListItemTitle = (ObjectType)"liti";
        public static readonly ObjectType DefinitionListItem = (ObjectType)"dlii";
        public static readonly ObjectType DefinitionListTerm = (ObjectType)"dlit";

        public static readonly ObjectType TopAndTail = (ObjectType)"tatp";
        public static readonly ObjectType TableCell = (ObjectType)"tcel";
        public static readonly ObjectType TableHeaderCell = (ObjectType)"thcl";
        public static readonly ObjectType TableFooterCell = (ObjectType)"tfcl";

        public static readonly ObjectType Canvas = (ObjectType)"canv";
        public static readonly ObjectType Panel = (ObjectType)"panl";
        public static readonly ObjectType Span = (ObjectType)"span";
        public static readonly ObjectType ItalicSpan = (ObjectType)"ital";
        public static readonly ObjectType BoldSpan = (ObjectType)"bold";
        public static readonly ObjectType UnderlineSpan = (ObjectType)"undl";
        public static readonly ObjectType Div = (ObjectType)"div_";
        public static readonly ObjectType BlockQuote = (ObjectType)"bkqu";
        public static readonly ObjectType Paragraph = (ObjectType)"para";
        public static readonly ObjectType Layer = (ObjectType)"layr";
        public static readonly ObjectType LayerGroup = (ObjectType)"lygp";
        public static readonly ObjectType UserComponent = (ObjectType)"ucmp";
        public static readonly ObjectType Link = (ObjectType)"link";
        public static readonly ObjectType Template = (ObjectType)"temp";
        public static readonly ObjectType DataOtherwise = (ObjectType)"doth";
        public static readonly ObjectType DataWhen = (ObjectType)"dwhn";
        public static readonly ObjectType PageHeader = (ObjectType)"pghd";
        public static readonly ObjectType PageFooter = (ObjectType)"pgft";
        public static readonly ObjectType Preformatted = (ObjectType)"pred";
        public static readonly ObjectType DateComp = (ObjectType)"date";
        public static readonly ObjectType NumberComp = (ObjectType)"numb";
        public static readonly ObjectType Barcode = (ObjectType)"barc";
        public static readonly ObjectType PlaceHolder = (ObjectType)"plac";
        public static readonly ObjectType ComponentHeader = (ObjectType)"cphd";
        public static readonly ObjectType ComponentFooter = (ObjectType)"cpft";

        public static readonly ObjectType ComponentLogEntry = (ObjectType)"clog";

        // Data Sources

        public static readonly ObjectType XmlData = (ObjectType)"xdat";
        public static readonly ObjectType SqlCommandType = (ObjectType)"sqlc";
        public static readonly ObjectType SqlDataSourceType = (ObjectType)"sqld";
        public static readonly ObjectType SourceDataStyleType = (ObjectType)"dsty";
        public static readonly ObjectType SqlParameterType = (ObjectType)"sqlp";
        public static readonly ObjectType SqlParameterValueType = (ObjectType)"sqlv";
        public static readonly ObjectType SqlNestedRelationType = (ObjectType)"sqln";
        public static readonly ObjectType SqlRelationMappingType = (ObjectType)"sqlm";
        public static readonly ObjectType ObjectCommandType = (ObjectType)"objc";
        public static readonly ObjectType DataWith = (ObjectType)"with";

        //Forms

        public static readonly ObjectType Form = (ObjectType)"form";
        public static readonly ObjectType FormInputField = (ObjectType)"inpt";

        //Headings

        public static readonly ObjectType H1 = (ObjectType)"txh1";
        public static readonly ObjectType H2 = (ObjectType)"txh2";
        public static readonly ObjectType H3 = (ObjectType)"txh3";
        public static readonly ObjectType H4 = (ObjectType)"txh4";
        public static readonly ObjectType H5 = (ObjectType)"txh5";
        public static readonly ObjectType H6 = (ObjectType)"txh6";

        //Styles

        public static readonly ObjectType StyleMargins = (ObjectType)"smar";
        public static readonly ObjectType StyleFill = (ObjectType)"sfil";
        public static readonly ObjectType StyleBackground = (ObjectType)"sbak";
        public static readonly ObjectType StyleBorder = (ObjectType)"sbdr";
        public static readonly ObjectType StyleBorderTop = (ObjectType)"sbdT";
        public static readonly ObjectType StyleBorderLeft = (ObjectType)"sbdL";
        public static readonly ObjectType StyleBorderBottom = (ObjectType)"sbdB";
        public static readonly ObjectType StyleBorderRight = (ObjectType)"sbdR";

        public static readonly ObjectType StyleClip = (ObjectType)"sclp";
        public static readonly ObjectType StyleDocument = (ObjectType)"sdoc";
        public static readonly ObjectType StyleText = (ObjectType)"stxt";
        public static readonly ObjectType StyleTable = (ObjectType)"stbl";
        public static readonly ObjectType StyleTransform = (ObjectType)"stra";
        public static readonly ObjectType StyleStroke = (ObjectType)"sstk";
        public static readonly ObjectType StylePage = (ObjectType)"spag";
        public static readonly ObjectType StyleOverflow = (ObjectType)"sovr";
        public static readonly ObjectType StylePosition = (ObjectType)"spos";
        public static readonly ObjectType StyleSize = (ObjectType)"ssiz";
        public static readonly ObjectType StyleFont = (ObjectType)"sfnt";
        public static readonly ObjectType StyleOutline = (ObjectType)"sout";
        public static readonly ObjectType Style = (ObjectType)"styl";
        public static readonly ObjectType StyleColumns = (ObjectType)"scol";
        public static readonly ObjectType StylePadding = (ObjectType)"spad";
        public static readonly ObjectType StyleGroup = (ObjectType)"sgrp";
        public static readonly ObjectType StyleRef = (ObjectType)"sref";
        public static readonly ObjectType StyleOverlayGrid = (ObjectType)"grid";
        public static readonly ObjectType StyleBadge = (ObjectType)"badg";
        public static readonly ObjectType StyleBarcode = (ObjectType)"sbar";
        public static readonly ObjectType StyleList = (ObjectType)"slst";
        public static readonly ObjectType StyleListItem = (ObjectType)"slli";
        public static readonly ObjectType StyleModify = (ObjectType)"smod";
        public static readonly ObjectType StyleSVGGeometry = (ObjectType)"svgm";

        //Shapes

        public static readonly ObjectType ShapeStyle = (ObjectType)"sshp";
        public static readonly ObjectType ShapeLine = (ObjectType)"line";
        public static readonly ObjectType ShapeCircle = (ObjectType)"circ";
        public static readonly ObjectType ShapeElipse = (ObjectType)"elps";
        public static readonly ObjectType ShapePath = (ObjectType)"path";
        public static readonly ObjectType ShapePolygon = (ObjectType)"poly";
        public static readonly ObjectType ShapePolyline = (ObjectType)"poll";
        public static readonly ObjectType ShapePolygram = (ObjectType)"polg";
        public static readonly ObjectType ShapeTriangle = (ObjectType)"tria";
        public static readonly ObjectType ShapeRectangle = (ObjectType)"rect";

        public static readonly ObjectType LinearGradient = (ObjectType)"ling";
        public static readonly ObjectType RadialGradient = (ObjectType)"radg";
        public static readonly ObjectType GradientStop = (ObjectType)"ggst";
        
        public static readonly ObjectType GraphicPattern = (ObjectType)"gpat";

        public static readonly ObjectType Marker = (ObjectType)"mker";

        // Modifications
        
        public static readonly ObjectType ModifyFrame = (ObjectType)"mFrm";
        public static readonly ObjectType ModifyFrameSet = (ObjectType)"mFrs";

        public static readonly ObjectType ModifyInsertPage = (ObjectType)"mPgi";
        public static readonly ObjectType ModifyAppendPage = (ObjectType)"mPga";
        public static readonly ObjectType ModifyDeletePage = (ObjectType)"mPgd";
        public static readonly ObjectType ModifyUpdatePage = (ObjectType)"mPgu";


    }
}
