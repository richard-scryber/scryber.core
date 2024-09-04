using System;
using Scryber.Components;
using Scryber.Styles;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Scryber.PDF;

namespace Scryber.Html.Components
{
    public abstract class HTMLHeadFootContainer : Scryber.Components.Panel
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

        

        protected HTMLHeadFootContainer(ObjectType type) : base(type)
        {
        }

        protected override void OnInitialized(InitContext context)
        {
            base.OnInitialized(context);
        }



        protected override void OnPreLayout(LayoutContext context)
        {
            this.ArrangeHeadersAndFooters();
            base.OnPreLayout(context);  
        }

        private void ArrangeHeadersAndFooters()
        {
            int topIndex = 0;
            int bottomIndex = this.Contents.Count - 1;

            List<HTMLComponentHeader> tops = new List<HTMLComponentHeader>(1);
            List<HTMLComponentFooter> footers = new List<HTMLComponentFooter>(1);

            

            for (int i = 0; i < this.Contents.Count; i++)
            {
                var one = this.Contents[i];
                if (one is HTMLComponentHeader)
                    tops.Add(one as HTMLComponentHeader);

                else if (one is HTMLComponentFooter)
                    footers.Add(one as HTMLComponentFooter);

            }
            if (tops.Count > 0)
            {
                foreach (var one in tops)
                {
                    this.Contents.MoveTo(one, topIndex++);
                }
            }
            if(footers.Count > 0)
            {
                foreach (var one in footers)
                {
                    this.Contents.MoveTo(one, -1);
                }
            }
        }

        public IPDFLayoutEngine GetEngine(IPDFLayoutEngine parent, PDFLayoutContext context, Style fullstyle)
        {
            return new PDF.Layout.LayoutEnginePanel(this, parent);
        }
    }


    [PDFParsableComponent("header")]
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


    [PDFParsableComponent("footer")]
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
