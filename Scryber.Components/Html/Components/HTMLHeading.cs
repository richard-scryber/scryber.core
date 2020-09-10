using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("h1")]
    public class HTMLHead1 : Scryber.Components.Head1
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead1()
            : base()
        {
        }
    }

    [PDFParsableComponent("h2")]
    public class HTMLHead2 : Scryber.Components.Head2
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead2()
            : base()
        {
        }
    }


    [PDFParsableComponent("h3")]
    public class HTMLHead3 : Scryber.Components.Head3
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead3()
            : base()
        {
        }
    }

    [PDFParsableComponent("h4")]
    public class HTMLHead4 : Scryber.Components.Head4
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead4()
            : base()
        {
        }
    }

    [PDFParsableComponent("h5")]
    public class HTMLHead5 : Scryber.Components.Head5
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead5()
            : base()
        {
        }
    }

    [PDFParsableComponent("h6")]
    public class HTMLHead6 : Scryber.Components.Head6
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override PDFStyle Style { get => base.Style; set => base.Style = value; }

        public HTMLHead6()
            : base()
        {
        }
    }
}
