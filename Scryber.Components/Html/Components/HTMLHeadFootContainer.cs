using System;
using Scryber.Components;
using Scryber.Styles;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Scryber.PDF;

namespace Scryber.Html.Components
{
    public abstract class HTMLHeadFootContainer : Scryber.Components.Panel, ITopAndTailedComponent, IPDFViewPortComponent
    {
        
        [PDFElement("header")]
        [PDFTemplate(IsBlock= true)]
        public ITemplate Header { get; set; }

        [PDFElement("footer")]
        [PDFTemplate(IsBlock = true)]
        public ITemplate Footer { get; set; }

        [PDFElement("continuation-header")]
        [PDFTemplate(IsBlock = true)]
        public ITemplate ContinuationHeader { get; set; }

        [PDFElement("continuation-footer")]
        [PDFTemplate(IsBlock = true)]
        public ITemplate ContinuationFooter { get; set; }
        
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
        }

        
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

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents => base.Contents;

        

        protected HTMLHeadFootContainer(ObjectType type) : base(type)
        {
        }

        protected override void OnInitialized(InitContext context)
        {
            base.OnInitialized(context);
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }

        
        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
        {
            return new PDF.Layout.LayoutEngineTopAndTailedPanel(this, parent);
        }
    }


    [PDFParsableComponent("header_old")]
    public class HTMLComponentHeader : Scryber.Components.Panel
    {
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
        }


        [PDFAttribute("repeat")]
        public HeadTailRepeat Repeat { get; set; } = HeadTailRepeat.Repeat;


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

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents => base.Contents;

        public HTMLComponentHeader() : this(HTMLObjectTypes.Header)
        { }

        protected HTMLComponentHeader(ObjectType type) : base(type)
        {

        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }
    }


    [PDFParsableComponent("footer_old")]
    public class HTMLComponentFooter : Scryber.Components.Panel
    {
        [PDFAttribute("class")]
        public override string StyleClass
        {
            get => base.StyleClass;
            set => base.StyleClass = value;
        }

        [PDFAttribute("style")]
        public override Style Style
        {
            get => base.Style;
            set => base.Style = value;
        }


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

        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents => base.Contents;

        public HTMLComponentFooter() : this(HTMLObjectTypes.Footer)
        { }

        protected HTMLComponentFooter(ObjectType type) : base(type)
        {
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Size.FullWidth = true;
            style.Position.DisplayMode = Drawing.DisplayMode.Block;
            return style;
        }
    }


}
