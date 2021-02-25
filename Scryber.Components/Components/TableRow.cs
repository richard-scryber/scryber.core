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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber.Components
{
    /// <summary>
    /// Represents a complete row in a table grid
    /// </summary>
    [PDFParsableComponent("Row")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_row")]
    public class TableRow : ContainerComponent, IPDFStyledComponent, IPDFDataStyledComponent
    {

        //
        // style attributes - row does not support positioning attributes
        //


        #region public PDFColor BackgroundColor

        /// <summary>
        /// Gets or sets the background color of this component
        /// </summary>
        [PDFAttribute("bg-color", Const.PDFStylesNamespace)]
        public PDFColor BackgroundColor
        {
            get
            {
                StyleValue<PDFColor> bg;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BgColorKey,out bg))
                    return bg.Value;
                else
                    return PDFColors.Transparent;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BgColorKey, value);
            }
        }

        #endregion

        #region public string BackgroundImage

        /// <summary>
        /// Gets or sets the background image of this component
        /// </summary>
        [PDFAttribute("bg-image", Const.PDFStylesNamespace)]
        public string BackgroundImage
        {
            get
            {
                StyleValue<string> src;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BgImgSrcKey, out src))
                    return src.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this.Style.SetValue(StyleKeys.BgImgSrcKey, value);
                else
                    this.Style.RemoveValue(StyleKeys.BgImgSrcKey);
            }
        }

        #endregion

        #region public PatternRepeat BackgroundRepeat

        /// <summary>
        /// Gets or sets the background image repeat of this component
        /// </summary>
        [PDFAttribute("bg-repeat", Const.PDFStylesNamespace)]
        public PatternRepeat BackgroundRepeat
        {
            get
            {
                StyleValue<PatternRepeat> bg;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BgRepeatKey, out bg))
                    return bg.Value;
                else
                    return PatternRepeat.RepeatBoth;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BgRepeatKey, value);
            }
        }

        #endregion

        #region public PDFReal BackgroundOpacity

        /// <summary>
        /// Gets or sets the background fill color opacity of this component
        /// </summary>
        [PDFAttribute("bg-opacity", Const.PDFStylesNamespace)]
        public PDFReal BackgroundOpacity
        {
            get
            {
                StyleValue<double> bg;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BgOpacityKey, out bg))
                    return bg.Value;
                else
                    return PDFReal.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BgOpacityKey, value.Value);
            }
        }

        #endregion

        #region public PDFUnit BorderWidth

        /// <summary>
        /// Gets or sets the border width of this component
        /// </summary>
        [PDFAttribute("border-width", Const.PDFStylesNamespace)]
        public PDFUnit BorderWidth
        {
            get
            {
                StyleValue<PDFUnit> bg;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderWidthKey, out bg))
                    return bg.Value;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BorderWidthKey, value);
            }
        }

        #endregion

        #region public PDFColor BorderColor

        /// <summary>
        /// Gets or sets the border color of this component
        /// </summary>
        [PDFAttribute("border-color", Const.PDFStylesNamespace)]
        public PDFColor BorderColor
        {
            get
            {
                StyleValue<PDFColor> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderColorKey, out val))
                    return val.Value;
                else
                    return PDFColors.Transparent;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.BorderColorKey);
                else
                    this.Style.SetValue(StyleKeys.BorderColorKey, value);
            }
        }

        #endregion

        #region public PDFDash BorderDashPattern

        /// <summary>
        /// Gets or sets the border dash pattern of this component
        /// </summary>
        [PDFAttribute("border-dash", Const.PDFStylesNamespace)]
        public PDFDash BorderDashPattern
        {
            get
            {
                StyleValue<PDFDash> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderDashKey, out val))
                    return val.Value;
                else
                    return PDFDash.None;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.BorderDashKey);
                else
                    this.Style.SetValue(StyleKeys.BorderDashKey, value);
            }
        }

        #endregion

        #region public double BorderOpacity

        /// <summary>
        /// Gets or sets the border opacity of this component
        /// </summary>
        [PDFAttribute("border-opacity", Const.PDFStylesNamespace)]
        public double BorderOpacity
        {
            get
            {
                StyleValue<double> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderOpacityKey, out val))
                    return val.Value;
                else
                    return 1;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BorderOpacityKey, value);
            }
        }

        #endregion

        #region public Sides BorderSides

        /// <summary>
        /// Gets or sets the border sides of this component
        /// </summary>
        [PDFAttribute("border-sides", Const.PDFStylesNamespace)]
        public Sides BorderSides
        {
            get
            {
                StyleValue<Sides> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderSidesKey, out val))
                    return val.Value;
                else
                    return (Sides)0;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BorderSidesKey, value);
            }
        }

        #endregion

        #region public PDFUnit BorderCornerRadius

        /// <summary>
        /// Gets or sets the border corner radii of this component
        /// </summary>
        [PDFAttribute("border-corner-radius", Const.PDFStylesNamespace)]
        public PDFUnit BorderCornerRadius
        {
            get
            {
                StyleValue<PDFUnit> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.BorderCornerRadiusKey, out val))
                    return val.Value;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.BorderCornerRadiusKey, value);
            }
        }

        #endregion

        #region public PDFColor FillColor

        /// <summary>
        /// Gets or sets the Fill color of this component
        /// </summary>
        [PDFAttribute("fill-color", Const.PDFStylesNamespace)]
        public PDFColor FillColor
        {
            get
            {
                StyleValue<PDFColor> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FillColorKey, out val))
                    return val.Value;
                else
                    return PDFColors.Transparent;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.FillColorKey);
                else
                    this.Style.SetValue(StyleKeys.FillColorKey, value);
            }
        }

        #endregion

        #region public string FillImage

        /// <summary>
        /// Gets or sets the Fill image of this component
        /// </summary>
        [PDFAttribute("fill-image", Const.PDFStylesNamespace)]
        public string FillImage
        {
            get
            {
                StyleValue<string> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FillImgSrcKey, out val))
                    return val.Value;
                else
                    return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    this.Style.RemoveValue(StyleKeys.FillImgSrcKey);
                else
                    this.Style.SetValue(StyleKeys.FillImgSrcKey, value);
            }
        }

        #endregion

        #region public PatternRepeat FillRepeat

        /// <summary>
        /// Gets or sets the Fill image repeat of this component
        /// </summary>
        [PDFAttribute("fill-repeat", Const.PDFStylesNamespace)]
        public PatternRepeat FillRepeat
        {
            get
            {
                StyleValue<PatternRepeat> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FillRepeatKey, out val))
                    return val.Value;
                else
                    return PatternRepeat.RepeatBoth;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FillRepeatKey, value);
            }
        }

        #endregion

        #region public PDFReal FillOpacity

        /// <summary>
        /// Gets or sets the Fill color opacity of this component
        /// </summary>
        [PDFAttribute("fill-opacity", Const.PDFStylesNamespace)]
        public PDFReal FillOpacity
        {
            get
            {
                StyleValue<double> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FillOpacityKey, out val))
                    return val.Value;
                else
                    return PDFReal.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FillOpacityKey, value.Value);
            }
        }

        #endregion

        #region public PDFUnit StrokeWidth

        /// <summary>
        /// Gets or sets the Stroke width of this component
        /// </summary>
        [PDFAttribute("stroke-width", Const.PDFStylesNamespace)]
        public PDFUnit StrokeWidth
        {
            get
            {
                StyleValue<PDFUnit> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.StrokeWidthKey, out val))
                    return val.Value;
                else
                    return PDFUnit.Empty;
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeWidthKey, value);
            }
        }

        #endregion

        #region public PDFColor StrokeColor

        /// <summary>
        /// Gets or sets the Stroke color of this component
        /// </summary>
        [PDFAttribute("stroke-color", Const.PDFStylesNamespace)]
        public PDFColor StrokeColor
        {
            get
            {
                StyleValue<PDFColor> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.StrokeColorKey, out val))
                    return val.Value;
                else
                    return PDFColors.Transparent;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.StrokeColorKey);
                else
                    this.Style.SetValue(StyleKeys.StrokeColorKey, value);
            }
        }

        #endregion

        #region public PDFDash StrokeDashPattern

        /// <summary>
        /// Gets or sets the Stroke dash pattern of this component
        /// </summary>
        [PDFAttribute("stroke-dash", Const.PDFStylesNamespace)]
        public PDFDash StrokeDashPattern
        {
            get
            {
                StyleValue<PDFDash> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.StrokeDashKey, out val))
                    return val.Value;
                else
                    return PDFDash.None;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.StrokeDashKey);
                else
                    this.Style.SetValue(StyleKeys.StrokeDashKey, value);

            }
        }

        #endregion

        #region public double StrokeOpacity

        /// <summary>
        /// Gets or sets the Stroke opacity of this component
        /// </summary>
        [PDFAttribute("stroke-opacity", Const.PDFStylesNamespace)]
        public double StrokeOpacity
        {
            get
            {
                StyleValue<double> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.StrokeOpacityKey, out val))
                    return val.Value;
                else
                    return 1;
            }
            set
            {
                this.Style.SetValue(StyleKeys.StrokeOpacityKey, value);
            }
        }

        #endregion

        #region public string FontFamily

        /// <summary>
        /// Gets or sets the Font Family of this component
        /// </summary>
        [PDFAttribute("font-family", Const.PDFStylesNamespace)]
        public PDFFontSelector FontFamily
        {
            get
            {
                StyleValue<PDFFontSelector> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FontFamilyKey, out val))
                    return val.Value;
                else
                    return null;
            }
            set
            {
                if (null == value)
                    this.Style.RemoveValue(StyleKeys.FontFamilyKey);
                else
                    this.Style.SetValue(StyleKeys.FontFamilyKey, value);
            }
        }

        #endregion

        #region public string FontSize

        /// <summary>
        /// Gets or sets the Font Size of this component
        /// </summary>
        [PDFAttribute("font-size", Const.PDFStylesNamespace)]
        public PDFUnit FontSize
        {
            get
            {
                StyleValue<PDFUnit> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FontSizeKey, out val))
                    return val.Value;
                else
                    return PDFUnit.Zero;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontSizeKey, value);
            }
        }

        #endregion

        #region public bool FontBold

        /// <summary>
        /// Gets or sets the Font bold value of this component
        /// </summary>
        [PDFAttribute("font-bold", Const.PDFStylesNamespace)]
        public bool FontBold
        {
            get
            {
                StyleValue<bool> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FontBoldKey, out val))
                    return val.Value;
                else
                    return false;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontBoldKey, value);
            }
        }

        #endregion

        #region public bool FontItalic

        /// <summary>
        /// Gets or sets the Font italic value of this component
        /// </summary>
        [PDFAttribute("font-italic", Const.PDFStylesNamespace)]
        public bool FontItalic
        {
            get
            {
                StyleValue<bool> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.FontItalicKey, out val))
                    return val.Value;
                else
                    return false;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontItalicKey, value);
            }
        }

        #endregion

        #region public TableRowRepeat Repeat {get;set;}

        /// <summary>
        /// Gets or sets the repeating option for this row
        /// </summary>
        [PDFAttribute("repeat")]
        public TableRowRepeat Repeat
        {
            get
            {
                StyleValue<TableRowRepeat> val;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.TableRowRepeatKey, out val))
                    return val.Value;
                else
                    return TableRowRepeat.RepeatAtTop;
            }
            set
            {
                this.Style.SetValue(StyleKeys.TableRowRepeatKey, value);
            }
        }

        #endregion

        //
        // data attributes
        //

        #region public string DataStyleIdentifier {get;set;}

        [PDFAttribute("style-identifier", Const.PDFDataNamespace)]
        public virtual string DataStyleIdentifier
        {
            get;
            set;
        }

        #endregion

        //
        // direct attributes
        //

        #region PDFTableCellList Cells {get;}

        private TableCellList _cells;

        /// <summary>
        /// Gets the list of cells in this row. Returned value will never be null
        /// </summary>
        [PDFArray(typeof(TableCell))]
        [PDFElement("")]
        public TableCellList Cells
        {
            get 
            {
                if (_cells == null)
                    _cells = new TableCellList(this.InnerContent);

                return _cells;
            }
        }

        #endregion

        #region public PDFTable ContainingTable {get;}

        /// <summary>
        /// Gets the table that contains this Row (or null)
        /// </summary>
        /// <remarks>We cannot directly return the parent as this row 
        /// could be within a binding invisible container. So we must walk up the hierarchy 
        /// until we find a table (or not a table).</remarks>
        public TableGrid ContainingTable
        {
            get
            {
                Component Component = this.Parent;
                
                while (Component != null)
                {
                    if (Component is TableGrid)
                        return Component as TableGrid;
                    else if (Component is IPDFInvisibleContainer)
                        Component = Component.Parent;
                    else
                        Component = null;
                }
                return null; //not found.
            }
        }

        #endregion

        #region public PDFStyle Style {get;set;} + bool HasStyle {get;}

        private Style _style;

        /// <summary>
        /// Gets or sets the style data for this TableRow
        /// </summary>
        public virtual Style Style
        {
            get
            {
                if (null == _style)
                    _style = new Style();
                return _style;
            }
            set
            {
                _style = value;
            }
        }

        /// <summary>
        /// Returns true if this row has any style data associated with it.
        /// </summary>
        public bool HasStyle
        {
            get { return null != _style && _style.HasValues; }
        }

        #endregion

        #region public string OutlineTitle {get;set;}

        /// <summary>
        /// Gets or sets the title of the outline for this component.
        /// </summary>
        [PDFAttribute("outline-title")]
        public virtual string OutlineTitle
        {
            get
            {
                if (this.HasOutline)
                    return this.Outline.Title;
                else
                    return string.Empty;
            }
            set
            {
                if (!this.HasOutline && string.IsNullOrEmpty(value))
                {
                    //Nothing to do as we don't have an outline and the value is empty
                    return;
                }
                this.Outline.Title = value;
            }
        }

        #endregion

        #region .ctor() + .ctor(PDFObjectType)

        /// <summary>
        /// Creates a new instance of the table row
        /// </summary>
        public TableRow()
            : this(PDFObjectTypes.TableRow)
        {
        }


        /// <summary>
        /// Protected constructor that sub classes 
        /// can use to create an instance of their class using a different ObjectType
        /// </summary>
        /// <param name="type">The type identifier</param>
        protected TableRow(PDFObjectType type)
            : base(type)
        {

        }

        #endregion

        //
        // override methods
        //

        #region protected override Styles.PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the base method to apply the overflow split of never to the base style
        /// so they will not break apart.
        /// </summary>
        /// <returns></returns>
        protected override Styles.Style GetBaseStyle()
        {
            Style style = base.GetBaseStyle();
            style.Overflow.Split = Drawing.OverflowSplit.Never;
            return style;
        }

        #endregion

        #region public override PDFStyle GetAppliedStyle(PDFComponent forComponent, PDFStyle baseStyle)

        /// <summary>
        /// Overrides the base method to remove any inappropriate styles before returning. 
        /// The Grid row does not support position, margin and padding options.
        /// </summary>
        /// <param name="forComponent"></param>
        /// <param name="baseStyle"></param>
        /// <returns></returns>
        protected override void MergeDeclaredStyles(Style applied)
        {
            base.MergeDeclaredStyles(applied);
            this.RemoveInapplicableStyles(applied);
        }

        /// <summary>
        /// Removes all the styles that cannot be applied to a row explicitly
        /// </summary>
        /// <param name="applied"></param>
        private void RemoveInapplicableStyles(Style applied)
        {
            

            int count = applied.ValueCount;

            applied.RemoveItemStyleValues(StyleKeys.MarginsItemKey);
            applied.RemoveItemStyleValues(StyleKeys.PaddingItemKey);

            //Remove the position mode if it is not set to invisible
            StyleValue<PositionMode> pos;
            if (applied.IsValueDefined(StyleKeys.PositionModeKey) && applied.TryGetValue(StyleKeys.PositionModeKey, out pos) && pos.Value != PositionMode.Invisible)
            {
                applied.RemoveValue(StyleKeys.PositionModeKey);
            }

            applied.RemoveValue(StyleKeys.SizeHeightKey);
            applied.RemoveValue(StyleKeys.PositionXKey);
            applied.RemoveValue(StyleKeys.PositionYKey);

            bool modified = count != applied.ValueCount;



            if (modified)
            {
                PDFTraceLog log = this.Document.TraceLog;
                if (log.ShouldLog(TraceLevel.Message))
                    log.Add(TraceLevel.Verbose, "PDFTableRow", "Removed all unsupported Margins, Padding and Postion style items that are not supported on a table row");
            }
        }

        #endregion
    }




    /// <summary>
    /// Represents a complete header row in table grid. 
    /// Overrides the default style to return an emboldened font style
    /// </summary>
    [PDFParsableComponent("Header-Row")]
    public class TableHeaderRow : TableRow
    {

        #region .ctor() and .ctor(PDFObjectType)

        /// <summary>
        /// Creates a new header row
        /// </summary>
        public TableHeaderRow()
            : this(PDFObjectTypes.TableHeaderRow)
        {
        }

        /// <summary>
        /// Creates a new header row with the specified object type value
        /// </summary>
        /// <param name="type"></param>
        public TableHeaderRow(PDFObjectType type)
            : base(type)
        {
            
        }

        #endregion

        #region protected override Styles.PDFStyle GetBaseStyle()

        /// <summary>
        /// Overrides the base style to apply a bold font to any text in the header
        /// </summary>
        /// <returns></returns>
        protected override Styles.Style GetBaseStyle()
        {
            Styles.Style style = base.GetBaseStyle();
            style.Table.RowRepeat = TableRowRepeat.RepeatAtTop;
            style.Font.FontBold = true;
            return style;
        }

        #endregion
    }




    /// <summary>
    /// Represents a complete footer row in a table grid
    /// </summary>
    [PDFParsableComponent("Footer-Row")]
    public class TableFooterRow : TableRow
    {

        #region .ctor() and .ctor(PDFObjectType)

        /// <summary>
        /// Creates a new instance of a table row
        /// </summary>
        public TableFooterRow()
            : this(PDFObjectTypes.TableFooterRow)
        {
        }

        /// <summary>
        /// Creates a new instance of a table row with the specified object type
        /// </summary>
        /// <param name="type">The object type of this instance</param>
        public TableFooterRow(PDFObjectType type)
            : base(type)
        {
        }

        #endregion

    }





    /// <summary>
    /// A list of table rows. As this is a component wrapping list, if there are inner invisible items, 
    /// they will also be enumerated over to be included in the collection
    /// </summary>
    public class TableRowList : ComponentWrappingList<TableRow>
    {

        #region .ctor(PDFComponentList)

        /// <summary>
        /// Creates the list of rows based on the provided component list. 
        /// </summary>
        /// <param name="inner"></param>
        public TableRowList(ComponentList inner)
            : base(inner)
        {
        }

        #endregion

    }
}
