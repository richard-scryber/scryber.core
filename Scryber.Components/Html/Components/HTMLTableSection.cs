using Scryber.Styles;
using Scryber.Components;


namespace Scryber.Html.Components
{
    /// <summary>
    /// Base class for all the sections in a complex table - thead, tbody, tfoot
    /// </summary>
    /// <remarks>Because this is an IInvisibleContainer the outer table collection will be enumerated over with the inner contents</remarks>
    public abstract class HTMLTableSection : Scryber.Components.VisualComponent, IInvisibleContainer
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        /// <summary>
        /// Global Html hidden attribute used with xhtml as hidden='hidden'
        /// </summary>
        [PDFAttribute("hidden")]
        public string Hidden
        {
            get
            {
                if (this.Visible)
                    return string.Empty;
                else
                    return "hidden";
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value != "hidden")
                    this.Visible = true;
                else
                    this.Visible = false;
            }
        }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        private TableRowList _rows;

        [PDFArray(typeof(TableRow))]
        [PDFElement()]
        public TableRowList Rows
        {
            get
            {
                if (null == this._rows)
                    this._rows = new TableRowList(this.InnerContent);

                return this._rows;
            }
        }

        public HTMLTableSection(ObjectType type) : base(type)
        {

        }

        Style _applied = null;
        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            if (forComponent is TableRow && this.Rows.Contains((TableRow)forComponent))
            {
                if (null == _applied)
                    _applied = this.GetAppliedStyle();
                _applied.MergeInto(baseStyle, forComponent, ComponentState.Normal);
            }
            return base.GetAppliedStyle(forComponent, baseStyle);
        }
    }

    [PDFParsableComponent("thead")]
    public class HTMLTableHead : HTMLTableSection
    {
        /// <summary>
        /// Gets or sets the flag that identifies if this
        /// header should repeat at the top of each page/column.
        /// Default is true
        /// </summary>
        [PDFAttribute("data-repeat")]
        public bool Repeating
        {
            get
            {
                StyleValue<TableRowRepeat> found;
                if (this.HasStyle && this.Style.TryGetValue(StyleKeys.TableRowRepeatKey, out found))
                    return found.Value(this.Style) == TableRowRepeat.RepeatAtTop;
                else
                    return true; //Default from the base
            }
            set
            {
                if (value)
                    this.Style.SetValue(StyleKeys.TableRowRepeatKey, TableRowRepeat.RepeatAtTop);
                else
                    this.Style.SetValue(StyleKeys.TableRowRepeatKey, TableRowRepeat.None);
            }
        }

        public HTMLTableHead()
            : base((ObjectType)"htTH")
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Table.RowRepeat = TableRowRepeat.RepeatAtTop;
            style.Font.FontBold = true;

            return style;
        }
    }

    [PDFParsableComponent("tbody")]
    public class HTMLTableBody : HTMLTableSection
    {

        public HTMLTableBody()
            : base((ObjectType)"htTB")
        { }


    }

    [PDFParsableComponent("tfoot")]
    public class HTMLTableFooter : HTMLTableSection
    {

        public HTMLTableFooter()
            : base((ObjectType)"htTF")
        { }


    }
}