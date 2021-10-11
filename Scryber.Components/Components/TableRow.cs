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
using Scryber.PDF.Native;

namespace Scryber.Components
{
    /// <summary>
    /// Represents a complete row in a table grid
    /// </summary>
    [PDFParsableComponent("Row")]
    [PDFJSConvertor("scryber.studio.design.convertors.pdf_row")]
    public class TableRow : ContainerComponent, IStyledComponent, IDataStyledComponent
    {

        //
        // style attributes - row does not support positioning attributes
        //


        #region public PDFColor BackgroundColor

        /// <summary>
        /// Gets or sets the background color of this component
        /// </summary>
        [PDFAttribute("bg-color", Const.PDFStylesNamespace)]
        public Color BackgroundColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);
                else
                    return StandardColors.Transparent;
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BgImgSrcKey, string.Empty);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BgRepeatKey, PatternRepeat.RepeatBoth);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BgOpacityKey, 0.0);
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
        public Unit BorderWidth
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderWidthKey, Unit.Empty);
                else
                    return Unit.Empty;
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
        public Color BorderColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderColorKey, StandardColors.Transparent);
                else
                    return StandardColors.Transparent;
            }
            set
            {
                if (value.IsEmpty)
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
        public Dash BorderDashPattern
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderDashKey, Dash.None);
                else
                    return Dash.None;
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderOpacityKey, 1);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderSidesKey, (Sides.Bottom | Sides.Top | Sides.Left | Sides.Right));
                else
                    return (Sides.Bottom | Sides.Top | Sides.Left | Sides.Right);
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
        public Unit BorderCornerRadius
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.BorderCornerRadiusKey, Unit.Zero);
                else
                    return Unit.Zero;
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
        public Color FillColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent);
                else
                    return StandardColors.Transparent;
            }
            set
            {
                if (value.IsEmpty)
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillImgSrcKey, string.Empty);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillRepeatKey, PatternRepeat.RepeatBoth);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FillOpacityKey, 0.0);
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
        public Unit StrokeWidth
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeWidthKey, Unit.Empty);
                else
                    return Unit.Empty;
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
        public Color StrokeColor
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeColorKey, StandardColors.Transparent);
                else
                    return StandardColors.Transparent;
            }
            set
            {
                if (value.IsEmpty)
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
        public Dash StrokeDashPattern
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeDashKey, Dash.None);
                else
                    return Dash.None;
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.StrokeOpacityKey, 1);
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
        public FontSelector FontFamily
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontFamilyKey, null);
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
        public Unit FontSize
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontSizeKey, Unit.Zero);
                else
                    return Unit.Zero;
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
        [PDFJSConvertor("scryber.studio.design.convertors.bold_css", JSParams = "\"font-weight\"")]
        [PDFDesignable("Bold", Category = "Font", Priority = 2, Type = "Boolean")]
        public virtual bool FontBold
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontWeightKey, FontWeights.Regular) >= FontWeights.Bold;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.Style.SetValue(StyleKeys.FontWeightKey, FontWeights.Bold);
                else
                    this.Style.SetValue(StyleKeys.FontWeightKey, FontWeights.Regular);
            }
        }

        #endregion

        #region public virtual int FontWeight {get;set;}

        /// <summary>
        /// Gets or sets the weight (light, regular, bold, black, etc) of this font
        /// </summary>
        public virtual int FontWeight
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontWeightKey, FontWeights.Regular);
                else
                    return FontWeights.Regular;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontWeightKey, value);

            }
        }

        #endregion

        #region public bool FontItalic

        /// <summary>
        /// Gets or sets the Font italic value of this component
        /// </summary>
        [PDFAttribute("font-italic", Const.PDFStylesNamespace)]
        [PDFJSConvertor("scryber.studio.design.convertors.italic_css", JSParams = "\"font-style\"")]
        [PDFDesignable("Italic", Category = "Font", Priority = 2, Type = "Boolean")]
        public virtual bool FontItalic
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontStyleKey, Drawing.FontStyle.Regular) == Drawing.FontStyle.Italic;
                else
                    return false;
            }
            set
            {
                if (value)
                    this.Style.SetValue(StyleKeys.FontFaceStyleKey, Drawing.FontStyle.Italic);
                else
                    this.Style.SetValue(StyleKeys.FontFaceStyleKey, Drawing.FontStyle.Regular);
            }
        }

        #endregion

        #region public virtual FontFaceStyle FontStyle {get; set; }

        /// <summary>
        /// Gets or sets the font face style (italic, oblique, regular) of this component
        /// </summary>
        public virtual Drawing.FontStyle FontStyle
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.FontStyleKey, Drawing.FontStyle.Regular);
                else
                    return Drawing.FontStyle.Regular;
            }
            set
            {
                this.Style.SetValue(StyleKeys.FontStyleKey, value);
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
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.TableRowRepeatKey, TableRowRepeat.RepeatAtTop);
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
                    else if (Component is IInvisibleContainer)
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
            : this(ObjectTypes.TableRow)
        {
        }


        /// <summary>
        /// Protected constructor that sub classes 
        /// can use to create an instance of their class using a different ObjectType
        /// </summary>
        /// <param name="type">The type identifier</param>
        protected TableRow(ObjectType type)
            : base(type)
        {

        }

        #endregion

        //
        // override methods
        //

        #region Databind()

        /// <summary>
        /// Inheritors should override this method to provide their own databing implementations.
        /// </summary>
        /// <param name="includeChildren">Flag to identifiy if children should be databound also</param>
        protected override void DoDataBind(DataContext context, bool includeChildren)
        {
            if (includeChildren && this.HasStyle)
                this.Style.DataBind(context);
            base.DoDataBind(context, includeChildren);
        }

        #endregion

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
            if (applied.IsValueDefined(StyleKeys.PositionModeKey) && applied.TryGetValue(StyleKeys.PositionModeKey, out pos) && pos.Value(applied) != PositionMode.Invisible)
            {
                applied.RemoveValue(StyleKeys.PositionModeKey);
            }

            applied.RemoveValue(StyleKeys.SizeHeightKey);
            applied.RemoveValue(StyleKeys.PositionXKey);
            applied.RemoveValue(StyleKeys.PositionYKey);

            bool modified = count != applied.ValueCount;



            if (modified)
            {
                var log = this.Document.TraceLog;
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
            : this(ObjectTypes.TableHeaderRow)
        {
        }

        /// <summary>
        /// Creates a new header row with the specified object type value
        /// </summary>
        /// <param name="type"></param>
        public TableHeaderRow(ObjectType type)
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
            : this(ObjectTypes.TableFooterRow)
        {
        }

        /// <summary>
        /// Creates a new instance of a table row with the specified object type
        /// </summary>
        /// <param name="type">The object type of this instance</param>
        public TableFooterRow(ObjectType type)
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
