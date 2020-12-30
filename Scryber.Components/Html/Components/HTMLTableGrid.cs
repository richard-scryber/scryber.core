using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("table")]
    public class HTMLTableGrid : Scryber.Components.TableGrid
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

        public HTMLTableGrid()
            : base()
        {
        }
    }

    public class HTMLTableSection : Scryber.Components.VisualComponent, IPDFInvisibleContainer
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

        public HTMLTableSection(PDFObjectType type) : base(type)
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

        public HTMLTableHead()
            : base((PDFObjectType)"htTH")
        { }

        
    }

    [PDFParsableComponent("tbody")]
    public class HTMLTableBody : HTMLTableSection
    {

        public HTMLTableBody()
            : base((PDFObjectType)"htTB")
        { }


    }

    [PDFParsableComponent("tfoot")]
    public class HTMLTableFooter : HTMLTableSection
    {

        public HTMLTableFooter()
            : base((PDFObjectType)"htTF")
        { }


    }
}
