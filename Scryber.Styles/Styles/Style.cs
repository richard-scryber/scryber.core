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

#define LOCALSTYLEITEMS //If declared then references to the local style items will be stored as variables in the instance

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber;
using Scryber.Drawing;
using System.ComponentModel;
using Scryber.Styles.Parsing;
using Scryber.PDF;
using Scryber.PDF.Graphics;

namespace Scryber.Styles
{
    /// <summary>
    /// Concrete implementation of a Style containing properties for accessing 
    /// each of the Style Items where values can be read or set.
    /// </summary>
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Style : StyleBase, IBindableComponent
    {
        #region InnerClass - StateList

        /// <summary>
        /// A linked list of states with styles associated. Accessed via the Styles.GetState() method.
        /// There can only ever be one entry for a specific state. 
        /// </summary>
        protected class StatedStyle
        {
            public ComponentState State
            {
                get;
                private set;
            }

            public Style Style
            {
                get;
                private set;
            }

            public StatedStyle Next
            {
                get;
                private set;
            }

            public StatedStyle(ComponentState state, Style style)
            {
                this.State = state;
                this.Style = style ?? throw new ArgumentNullException(nameof(style));
                this.Next = null;
            }

            /// <summary>
            /// Either merges the style into an existing style for that state, or adds the style onto the end of this linked list.
            /// </summary>
            /// <param name="forState"></param>
            /// <param name="withStyle"></param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public virtual StatedStyle Apply(ComponentState forState, Style withStyle)
            {
                if (forState == this.State)
                {
                    withStyle.MergeInto(this.Style);
                    return this;
                }
                else if (null == this.Next)
                {
                    this.Next = new StatedStyle(forState, withStyle);
                    return this.Next;
                }
                else
                    return this.Next.Apply(forState, withStyle);
            }

            public virtual StatedStyle Find(ComponentState state)
            {
                if (this.State == state)
                    return this;
                else if (null == this.Next)
                    return null;
                else
                    return this.Next.Find(state);
            }

            /// <summary>
            /// Creates a shallow clone of this StatedStyle and any linked StatedStyles
            /// </summary>
            /// <returns></returns>
            public StatedStyle Clone()
            {
                var clone = new StatedStyle(this.State, this.Style);
                if (null != this.Next)
                    clone.Next = this.Next.Clone();

                return clone;
            }
        }

        #endregion

        #region public const string PDFStylesNamespace

        /// <summary>
        /// The fully qualified assemblynamespace
        /// </summary>
        public const string PDFStylesNamespace = "Scryber.Styles, Scryber.Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";

        #endregion

        public const int DirectStylePriority = int.MaxValue - 1;

        // events

        #region public event PDFDataBindEventHandler DataBinding + OnDataBinding(args)

        [PDFAttribute("on-databinding")]
        public event DataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(DataContext context)
        {
            if (this.DataBinding != null)
                this.DataBinding(this, new DataBindEventArgs(context));
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound + OnDataBound(args)

        [PDFAttribute("on-databound")]
        public event DataBindEventHandler DataBound;

        protected virtual void OnDataBound(DataContext context)
        {
            if (this.DataBound != null)
                this.DataBound(this, new DataBindEventArgs(context));
        }

        #endregion


        #region public string ID {get;set;}

        private string _id;

        /// <summary>
        /// Gets or sets the ID for this instance
        /// </summary>
        [PDFAttribute("id")]
        public string ID
        {
            get
            {
                if (String.IsNullOrEmpty(_id))
                {
                    _id = string.Empty;
                }
                return this._id;
            }
            set
            {
                _id = value;
            }
        }

        #endregion


        #region public int Priority {get;set;}

        private int _priority;

        /// <summary>
        /// Gets or sets the priority of this style
        /// </summary>
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        #endregion

        #region public StyleVariableSet Variables {get;} + HasVariables {get;}

        private StyleVariableSet _variables;

        public StyleVariableSet Variables
        {
            get
            {
                return _variables;
            }
            set
            {
                _variables = value;
            }
        }

        public bool HasVariables
        {
            get
            {
                return null != _variables && _variables.Count > 0;
            }
        }

        #endregion


        #region protected StateList States {get;} + public bool HasStates {get;}

        private StatedStyle _states = null;

        protected virtual StatedStyle States
        {
            get { return _states; }
        }

        public bool HasStates
        {
            get { return null != _states; }
        }

        #endregion

        // style accessor properties

        #region public PDFBackgroundStyle Background {get;}

#if LOCALSTYLEITEMS

        private BackgroundStyle _bg;

        /// <summary>
        /// Gets the background associated with this Style
        /// </summary>
        public BackgroundStyle Background
        {
            get
            {

                if (null == _bg)
                    _bg = this.GetOrCreateItem<BackgroundStyle>(StyleKeys.BgItemKey);

                return _bg;
            }
        }
#else
        /// <summary>
        /// Gets the background associated with this Style
        /// </summary>
        public PDFBackgroundStyle Background
        {
            get
            {
                return this.GetOrCreateItem<PDFBackgroundStyle>(PDFStyleKeys.BgItemKey);
            }
        }
#endif
        #endregion

        #region public PDFPaddingStyle Padding {get;}

#if LOCALSTYLEITEMS
        private PaddingStyle _padding;

        /// <summary>
        /// Gets the padding associated with this Style
        /// </summary>
        public PaddingStyle Padding
        {
            get
            {

                if (null == _padding)
                    _padding = this.GetOrCreateItem<PaddingStyle>(StyleKeys.PaddingItemKey);
                
                return _padding;
            }
        }
#else
        /// <summary>
        /// Gets the padding associated with this Style
        /// </summary>
        public PDFPaddingStyle Padding
        {
            get
            {
                return this.GetOrCreateItem<PDFPaddingStyle>(PDFStyleKeys.PaddingItemKey);
            }
        }
#endif
        #endregion

        #region public PDFMarginsStyle Margins {get;}

#if LOCALSTYLEITEMS

        private MarginsStyle _margins;

        /// <summary>
        /// Gets the margins associated with this Style
        /// </summary>
        public MarginsStyle Margins
        {
            get
            {
                if (null == _margins)
                {
                    _margins = this.GetOrCreateItem<MarginsStyle>(StyleKeys.MarginsItemKey);
                }
                return _margins;
            }
        }
#else
        /// <summary>
        /// Gets the margins associated with this Style
        /// </summary>
        public PDFMarginsStyle Margins
        {
            get
            {
                return this.GetOrCreateItem<PDFMarginsStyle>(PDFStyleKeys.MarginsItemKey);
            }
        }

#endif

        #endregion

        #region public PDFFontStyle Font {get;}

#if LOCALSTYLEITEMS

        private FontStyle _font;

        /// <summary>
        /// Gets the Font information associated with this style
        /// </summary>
        public FontStyle Font
        {
            get
            {
                if (null == _font)
                {
                    _font = this.GetOrCreateItem<FontStyle>(StyleKeys.FontItemKey);
                }
                return _font;
            }
        }

#else
        public PDFFontStyle Font
        {
            get
            {
                return this.GetOrCreateItem<PDFFontStyle>(PDFStyleKeys.FontItemKey);
            }
        }
#endif

        #endregion

        #region public PDFFillStyle Fill {get;}

#if LOCALSTYLEITEMS

        private FillStyle _fill;

        /// <summary>
        /// Gets the fill associated with this Style
        /// </summary>
        public FillStyle Fill
        {
            get
            {

                if (null == _fill)
                    _fill = this.GetOrCreateItem<FillStyle>(StyleKeys.FillItemKey);

                return _fill;
            }
        }
#else
        /// <summary>
        /// Gets the fill associated with this Style
        /// </summary>
        public PDFFillStyle Fill
        {
            get
            {
                return this.GetOrCreateItem<PDFFillStyle>(PDFStyleKeys.FillItemKey);
            }
        }
#endif
        #endregion

        #region public PDFColumnsStyle Columns {get;}

#if LOCALSTYLEITEMS

        private ColumnsStyle _cols;

        /// <summary>
        /// Gets the Column info associated with this style
        /// </summary>
        public ColumnsStyle Columns
        {
            get
            {
                if (null == _cols)
                {
                    _cols = this.GetOrCreateItem<ColumnsStyle>(StyleKeys.ColumnItemKey);
                }
                return _cols;
            }
        }

#else
        /// <summary>
        /// Gets the Column info associated with this style
        /// </summary>
        public PDFColumnsStyle Columns
        {
            get
            {
                return this.GetOrCreateItem<PDFColumnsStyle>(PDFStyleKeys.ColumnItemKey);
            }
        }
#endif

        #endregion

        #region public PDFOverflowStyle Overflow {get;}

#if LOCALSTYLEITEMS

        private OverflowStyle _over;

        /// <summary>
        /// Gets the Overflow info associated with this Style
        /// </summary>
        public OverflowStyle Overflow
        {
            get
            {
                if (null == _over)
                {
                    _over = this.GetOrCreateItem<OverflowStyle>(StyleKeys.OverflowItemKey);
                }
                return _over;
            }
        }

#else
        /// <summary>
        /// Gets the Overflow info associated with this Style
        /// </summary>
        public PDFOverflowStyle Overflow
        {
            get
            {
                return this.GetOrCreateItem<PDFOverflowStyle>(PDFStyleKeys.OverflowItemKey);
            }
        }
#endif

        #endregion

        #region public PDFPositionStyle Position {get;}

#if LOCALSTYLEITEMS

        private PositionStyle _pos;

        /// <summary>
        /// Gets or sets the position info associated with this style
        /// </summary>
        public PositionStyle Position
        {
            get
            {
                if (null == _pos)
                {
                    _pos = this.GetOrCreateItem<PositionStyle>(StyleKeys.PositionItemKey);
                }
                return _pos;
            }
        }

#else
        /// <summary>
        /// Gets or sets the position info associated with this style
        /// </summary>
        public PDFPositionStyle Position
        {
            get
            {
                return this.GetOrCreateItem<PDFPositionStyle>(PDFStyleKeys.PositionItemKey);
            }
        }
#endif

        #endregion

        #region public PDFSizeStyle Size {get;}

#if LOCALSTYLEITEMS

        private SizeStyle _sz;

        /// <summary>
        /// Gets or sets the size info associated with this style
        /// </summary>
        public SizeStyle Size
        {
            get
            {
                if (null == _sz)
                {
                    _sz = this.GetOrCreateItem<SizeStyle>(StyleKeys.SizeItemKey);
                }
                return _sz;
            }
        }

#else
        /// <summary>
        /// Gets or sets the size info associated with this style
        /// </summary>
        public PDFSizeStyle Size
        {
            get
            {
                return this.GetOrCreateItem<PDFSizeStyle>(PDFStyleKeys.SizeItemKey);
            }
        }
#endif

        #endregion

        #region public PDFClipStyle Clipping {get;}

#if LOCALSTYLEITEMS

        private ClipStyle _clip;

        /// <summary>
        /// Gets the Clipping info associated with this style
        /// </summary>
        public ClipStyle Clipping
        {
            get
            {
                if (null == _clip)
                {
                    _clip = this.GetOrCreateItem<ClipStyle>(StyleKeys.ClipItemKey);
                }
                return _clip;
            }
        }

#else
        /// <summary>
        /// Gets the Clipping info associated with this style
        /// </summary>
        public PDFClipStyle Clipping
        {
            get
            {
                return  this.GetOrCreateItem<PDFClipStyle>(PDFStyleKeys.ClipItemKey);
            }
        }
#endif

        #endregion

        #region public PDFBorderStyle Border {get;}

#if LOCALSTYLEITEMS

        private BorderStyle _border;

        /// <summary>
        /// Gets the border associated with this Style
        /// </summary>
        public BorderStyle Border
        {
            get
            {

                if (null == _border)
                    _border = this.GetOrCreateItem<BorderStyle>(StyleKeys.BorderItemKey);

                return _border;
            }
        }
#else
        /// <summary>
        /// Gets the border associated with this Style
        /// </summary>
        public PDFBorderStyle Border
        {
            get
            {
                return this.GetOrCreateItem<PDFBorderStyle>(PDFStyleKeys.BorderItemKey);
            }
        }
#endif
        #endregion

        #region public PDFStrokeStyle Stroke {get;}

#if LOCALSTYLEITEMS

        private StrokeStyle _stroke;

        /// <summary>
        /// Gets the stroke style item associated with this Style
        /// </summary>
        public StrokeStyle Stroke
        {
            get
            {

                if (null == _stroke)
                    _stroke = this.GetOrCreateItem<StrokeStyle>(StyleKeys.StrokeItemKey);

                return _stroke;
            }
        }
#else
        /// <summary>
        /// Gets the stroke style item associated with this Style
        /// </summary>
        public PDFStrokeStyle Stroke
        {
            get
            {
                return this.GetOrCreateItem<PDFStrokeStyle>(PDFStyleKeys.StrokeItemKey);
            }
        }
#endif
        #endregion

        #region public PDFTextStyle Text {get;}

#if LOCALSTYLEITEMS

        private TextStyle _text;

        /// <summary>
        /// Gets the text style item associated  with this Style
        /// </summary>
        public TextStyle Text
        {
            get
            {

                if (null == _text)
                    _text = this.GetOrCreateItem<TextStyle>(StyleKeys.TextItemKey);

                return _text;
            }
        }
#else
        /// <summary>
        /// Gets the background associated with this Style
        /// </summary>
        public PDFTextStyle Text
        {
            get
            {
                return this.GetOrCreateItem<PDFTextStyle>(PDFStyleKeys.TextItemKey);
            }
        }
#endif
        #endregion

        #region public PDFListStyle List {get;}

#if LOCALSTYLEITEMS

        private ListStyle _list;

        /// <summary>
        /// Gets the list style item associated  with this Style
        /// </summary>
        public ListStyle List
        {
            get
            {

                if (null == _list)
                    _list = this.GetOrCreateItem<ListStyle>(StyleKeys.ListItemKey);

                return _list;
            }
        }
#else
        /// <summary>
        /// Gets the list style item associated with this Style
        /// </summary>
        public PDFListStyle List
        {
            get
            {
                return this.GetOrCreateItem<PDFListStyle>(PDFStyleKeys.ListItemKey);
            }
        }
#endif
        #endregion

        #region public PDFModifyPageStyle PageModifications {get;}

#if LOCALSTYLEITEMS

        private ModifyPageStyle _modify;

        /// <summary>
        /// Gets the ModifyPage style item associated with this Style
        /// </summary>
        public ModifyPageStyle PageModifications
        {
            get
            {

                if (null == _modify)
                    _modify = this.GetOrCreateItem<ModifyPageStyle>(StyleKeys.ModifyPageItemKey);

                return _modify;
            }
        }

#else

        /// <summary>
        /// Gets the ModifyPage style item associated with this Style
        /// </summary>
        public PDFModifyPageStyle PageModifications
        {
            get
            {
                return this.GetOrCreateItem<PDFModifyPageStyle>(PDFStyleKeys.ModifyPageItemKey);
            }
        }

#endif

        #endregion

        #region public PDFOutlineStyle Outline {get;}

#if LOCALSTYLEITEMS

        private OutlineStyle _outline;

        /// <summary>
        /// Gets the outline style item associated  with this Style
        /// </summary>
        public OutlineStyle Outline
        {
            get
            {

                if (null == _outline)
                    _outline = this.GetOrCreateItem<OutlineStyle>(StyleKeys.OutlineItemKey);

                return _outline;
            }
        }
#else
        /// <summary>
        /// Gets the outline style item associated with this Style
        /// </summary>
        public PDFOutlineStyle Outline
        {
            get
            {
                return this.GetOrCreateItem<PDFOutlineStyle>(PDFStyleKeys.OutlineItemKey);
            }
        }
#endif

        #endregion

        #region public PDFOverlayGridStyle OverlayGrid {get;}

#if LOCALSTYLEITEMS

        private OverlayGridStyle _overlay;

        /// <summary>
        /// Gets the Overlay Grid style item associated  with this Style
        /// </summary>
        public OverlayGridStyle OverlayGrid
        {
            get
            {

                if (null == _overlay)
                    _overlay = this.GetOrCreateItem<OverlayGridStyle>(StyleKeys.OverlayItemKey);

                return _overlay;
            }
        }
#else
        /// <summary>
        /// Gets the Overlay Grid style item associated with this Style
        /// </summary>
        public PDFOverlayGridStyle OverlayGrid
        {
            get
            {
                return this.GetOrCreateItem<PDFOverlayGridStyle>(PDFStyleKeys.OverlayItemKey);
            }
        }
#endif

        #endregion

        #region public PDFPageStyle PageStyle {get;}

#if LOCALSTYLEITEMS

        private PageStyle _pg;

        /// <summary>
        /// Gets the page style item associated  with this Style
        /// </summary>
        public PageStyle PageStyle
        {
            get
            {

                if (null == _pg)
                    _pg = this.GetOrCreateItem<PageStyle>(StyleKeys.PageItemKey);

                return _pg;
            }
        }

#else

        /// <summary>
        /// Gets the page style item associated with this Style
        /// </summary>
        public PDFPageStyle PageStyle
        {
            get
            {
                return this.GetOrCreateItem<PDFPageStyle>(PDFStyleKeys.PageItemKey);
            }
        }

#endif

        #endregion

        #region public PDFShapeStyle Shape {get;}

#if LOCALSTYLEITEMS

        private ShapeStyle _shape;

        /// <summary>
        /// Gets the shape style item associated  with this Style
        /// </summary>
        public ShapeStyle Shape
        {
            get
            {

                if (null == _shape)
                    _shape = this.GetOrCreateItem<ShapeStyle>(StyleKeys.ShapeItemKey);

                return _shape;
            }
        }

#else

        /// <summary>
        /// Gets the shape style item associated with this Style
        /// </summary>
        public PDFShapeStyle Shape
        {
            get
            {
                return this.GetOrCreateItem<PDFShapeStyle>(PDFStyleKeys.ShapeItemKey);
            }
        }

#endif

        #endregion

        #region public PDFTableStyle Table {get;}

#if LOCALSTYLEITEMS

        private TableStyle _table;

        /// <summary>
        /// Gets the Table style item associated  with this Style
        /// </summary>
        public TableStyle Table
        {
            get
            {

                if (null == _table)
                    _table = this.GetOrCreateItem<TableStyle>(StyleKeys.TableItemKey);

                return _table;
            }
        }

#else

        /// <summary>
        /// Gets the Table style item associated with this Style
        /// </summary>
        public PDFTableStyle Table
        {
            get
            {
                return this.GetOrCreateItem<PDFTableStyle>(PDFStyleKeys.TableItemKey);
            }
        }

#endif

        #endregion

        #region public PDFTransformStyle Transform {get;}

        //Transformations are not currently suported.

#if USETRANSFORM

#if LOCALSTYLEITEMS

        private PDFTransformStyle _transform;

        /// <summary>
        /// Gets the Transform style item associated  with this Style
        /// </summary>
        public PDFTransformStyle Transform
        {
            get
            {

                if (null == _transform)
                    _transform = this.GetOrCreateItem<PDFTransformStyle>(PDFStyleKeys.TransformItemKey);

                return _transform;
            }
        }

#else

        /// <summary>
        /// Gets the Transform style item associated with this Style
        /// </summary>
        public PDFTransformStyle Transform
        {
            get
            {
                return this.GetOrCreateItem<PDFTransformStyle>(PDFStyleKeys.TransformItemKey);
            }
        }

#endif

#endif

#endregion

        // Items override

        #region public override PDFStyleItemCollection Items {get;}

        [PDFElement("")]
        [PDFArray(typeof(StyleItemBase))]
        public override StyleItemCollection StyleItems
        {
            get
            {
                return base.StyleItems;
            }
        }

        #endregion

        //
        // .ctors
        //

        public Style()
            : this(ObjectTypes.Style)
        {
        }

        protected Style(ObjectType type)
            : base(type)
        {
        }

        //
        // public methods
        //

        public void DataBind(DataContext context)
        {
            this.OnDataBinding(context);
            this.DoDataBind(context, true);
            this.OnDataBound(context);
        }

        
        
        //
        // createXXX methods
        //

        #region public PDFPositionOptions CreatePostionOptions()

        /// <summary>
        /// Creates and returns the position options for this style.
        /// </summary>
        /// <returns></returns>
        public PDFPositionOptions CreatePostionOptions()
        {
           return this.DoCreatePositionOptions();
        }

        #endregion

        #region public PDFTextRenderOptions CreateTextOptions()

        /// <summary>
        /// Creates and returns the text options for this style
        /// </summary>
        /// <returns></returns>
        public PDFTextRenderOptions CreateTextOptions()
        {
            return this.DoCreateTextOptions();
        }

        #endregion

        #region public PDFColumnOptions CreateColumnOptions()

        /// <summary>
        /// Creates and returns the column options for this style
        /// </summary>
        /// <returns></returns>
        public PDFColumnOptions CreateColumnOptions()
        {
            return this.DoCreateColumnOptions();
        }

        #endregion

        #region public PDFPageSize CreatePageSize()

        /// <summary>
        /// Creates and returns the page size options for this style
        /// </summary>
        /// <returns></returns>
        public PageSize CreatePageSize()
        {
            return this.DoCreatePageSize();
        }

        #endregion

        #region public PDFFont CreateFont()

        /// <summary>
        /// Creates and returns the font for this style.
        /// </summary>
        /// <returns></returns>
        public Font CreateFont()
        {
            return this.DoCreateFont(true);
        }

        #endregion

        #region public PDFThickness CreatePaddingThickness()

        /// <summary>
        /// Returns the PDFThickness associated with the Padding for this style
        /// </summary>
        /// <returns></returns>
        public Thickness CreatePaddingThickness()
        {
            return this.DoCreatePaddingThickness();
        }

        #endregion

        #region public PDFThickness CreateMarginsThickness()

        /// <summary>
        /// Returns the PDFThickness associated with the Margins for this style
        /// </summary>
        /// <returns></returns>
        public Thickness CreateMarginsThickness()
        {
            return this.DoCreateMarginsThickness();
        }

        #endregion

        #region public PDFThickness CreateClippingThickness()

        /// <summary>
        /// the PDFThickness associated with the Clipping for this style
        /// </summary>
        /// <returns></returns>
        public Thickness CreateClippingThickness()
        {
            return this.DoCreateClippingThickness();
        }

        #endregion

        #region public PDFPageNumberOptions CreatePageNumberOptions()

        /// <summary>
        /// Gets the page numbering options for this style
        /// </summary>
        /// <returns></returns>
        public PageNumberOptions CreatePageNumberOptions()
        {
            return this.DoCreatePageNumberOptions();
        }

        #endregion

        #region public PDFPen CreateBorderPen()

        /// <summary>
        /// Creates a new appropriate PDFPen if this style has any border attributes assigned, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public PDFPenBorders CreateBorderPen()
        {
            return this.DoCreatePenBorders();
        }

        #endregion

        #region public PDFBrush CreateBackgroundBrush()

        /// <summary>
        /// Creates a new appropriate PDFBrush 
        /// for this style if it has any background attributes set, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public PDFBrush CreateBackgroundBrush()
        {
            return this.DoCreateBackgroundBrush();
        }

        #endregion

        #region public PDFBrush CreateFillBrush()

        /// <summary>
        /// Creates a new appropriate PDFBrush 
        /// for this style if it has any fill attributes set, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public PDFBrush CreateFillBrush()
        {
            return this.DoCreateFillBrush();
        }

        #endregion

        #region public PDFPen CreateStrokePen()

        /// <summary>
        /// Creates a new appropriate PDFPen 
        /// for this style if it has any stroke attributes set, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public PDFPen CreateStrokePen()
        {
            try
            {
                return this.DoCreateStrokePen();
            }
            catch (Exception ex)
            {
                throw new PDFException(string.Format(Errors.CouldNotCreateTheGraphicObjectFromTheStyle, "Stroke pen", ex.Message), ex);
            }
        }

        #endregion

        #region public PDFPen CreateOverlayGridPen()

        /// <summary>
        /// Creates a new appropriate PDFPen 
        /// for this style if it has any overlay grid attributes set, otherwise returns null
        /// </summary>
        /// <returns></returns>
        public PDFPen CreateOverlayGridPen()
        {
            return this.DoCreateOverlayGridPen();
        }

        #endregion


        #region public virtual PDFStyle Flatten()

        /// <summary>
        /// Returns a flat version of the PDFStyle, calculating any relative sizes to absolute.
        /// </summary>
        /// <returns></returns>
        public virtual Style Flatten(Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            if (this.InheritedValues.Count > 0)
            {
                foreach (var key in this.InheritedValues.Keys)
                {
                    if (key.CanBeRelative)
                    {
                        key.FlattenValue(this, pageSize, containerSize, fontSize, rootFontSize);
                    }
                }
            }
            if (this.DirectValues.Count > 0)
            {
                foreach (var key in this.DirectValues.Keys)
                {
                    if (key.CanBeRelative)
                    {
                        key.FlattenValue(this, pageSize, containerSize, fontSize, rootFontSize);
                    }
                }
            }
            return this;
        }

        #endregion

        //
        // variables
        //

        public bool AddVariable(string name, string value)
        {
            if (null == this._variables)
                _variables = new StyleVariableSet();
            _variables.Add(new StyleVariable(name, value));

            return true;
        }

        internal void AddVariable(StyleVariable variable)
        {
            if (null == this._variables)
                _variables = new StyleVariableSet();

            _variables.Add(variable ?? throw new ArgumentNullException(nameof(variable)));
        }

        //
        // States
        //

        public Style GetStyleState(ComponentState state, bool create)
        {
            StatedStyle found = null;

            if(this.HasStates)
            {
                found = this.States.Find(state);
            }

            if(null == found && create)
            {
                found = this.AppendStyleState(state, CreateStyleForState(state));

            }
            return null == found ? null : found.Style;
        }

        protected virtual StatedStyle AppendStyleState(ComponentState state, Style style)
        {
            StatedStyle found = null;
            if (!this.HasStates)
            {
                this._states = new StatedStyle(state, style);
                found = this._states;
            }
            else
                found = this._states.Apply(state, style);

            return found;
        }

        protected virtual Style CreateStyleForState(ComponentState state)
        {
            return new Style();
        }


        public bool TryGetStyleState(ComponentState state, out Style style)
        {
            if (this.HasStates)
            {
                StatedStyle stated = this.States.Find(state);
                if(null != stated)
                {
                    style = stated.Style;
                    return true; //We know Style property cannot be null.
                }
            }

            style = null;
            return false;
        }

        public bool CopyStatesFrom(Style other)
        {
            if (other.HasStates)
            {
                this._states = other.States.Clone();
                return true;
            }
            else
                return false;
        }

        //
        // Parsing
        //

        public static Style Parse(string value)
        {
            
            Style style = new Style();

            if (!string.IsNullOrEmpty(value))
            {
                CSSStyleItemReader reader = new CSSStyleItemReader(value);
                var parser = new CSSStyleItemAllParser();

                while (reader.ReadNextAttributeName())
                {
                    parser.SetStyleValue(style, reader, null);
                }
            }

            return style;
        }
    }


}
