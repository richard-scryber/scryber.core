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
    public class HTMLTableGrid : Scryber.Components.PDFTableGrid
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLTableGrid()
            : base()
        {
        }
    }

    public class HTMLTableSection : Scryber.Components.PDFVisualComponent, IPDFInvisibleContainer
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        private PDFTableRowList _rows;

        [PDFArray(typeof(PDFTableRow))]
        [PDFElement()]
        public PDFTableRowList Rows
        {
            get
            {
                if (null == this._rows)
                    this._rows = new PDFTableRowList(this.InnerContent);

                return this._rows;
            }
        }

        public HTMLTableSection(PDFObjectType type) : base(type)
        {

        }

        PDFStyle _applied = null;
        public override PDFStyle GetAppliedStyle(PDFComponent forComponent, PDFStyle baseStyle)
        {
            if (forComponent is PDFTableRow && this.Rows.Contains((PDFTableRow)forComponent))
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
