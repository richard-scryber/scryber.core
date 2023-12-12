using System;
using System.Collections.Generic;
using System.Text;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("html")]
    public class HTMLDocument : Document
    {

        private HTMLHead _head;

        [PDFElement("head")]
        public HTMLHead Head
        {
            get { return this._head; }
            set
            {
                if (null != _head)
                    this.InnerContent.Remove(_head);

                this._head = value;

                if (null != this._head)
                    this.InnerContent.Add(_head);
            }
        }


        [PDFAttribute("lang")]
        public string Language
        {
            get;
            set;
        }

        private HTMLBody _body;

        [PDFElement("body")]
        public HTMLBody Body
        {
            get { return this._body; }
            set
            {
                if (null != this._body)
                    this.Pages.Remove(this._body);

                this._body = value;

                if (null != this._body)
                    this.Pages.Add(_body);
            }
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
        public string OutlineTitle
        {
            get { return this.Outline.Title; }
            set { this.Outline.Title = value; }
        }
        
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        
        [PDFParserIgnore()]
        public override PageList Pages { get { return base.Pages; } }

        public HTMLDocument()
            : this(HTMLObjectTypes.Document)
        {
        }

        protected HTMLDocument(ObjectType type): base(type)
        {
            this.AddRootStyles();
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.SetValue(StyleKeys.FontSizeKey, Scryber.Drawing.Font.DefaultFontSize); //matches the HTML document
            return style;
        }

        private StyleCollection _roots;

        /// <summary>
        /// The root styles apply the User Agent style sheet styles to components. Inheritors can override the AddRootStyles to alter or extend any of these styles, and they will be applied to each component.
        /// </summary>
        protected StyleCollection RootStyles
        {
            get
            {
                if (null == _roots)
                {
                    _roots = new StyleCollection(this);
                }
                return _roots;
            }
        }
        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            //if (null == baseStyle)
            //    baseStyle = new Style();

            //this.RootStyles.MergeInto(baseStyle, forComponent);

            var applied = base.GetAppliedStyle(forComponent, baseStyle);
            
            return applied;
        }

        protected virtual void AddRootStyles()
        {
            //Quote has before and after
            var defn = new StyleDefn("q:before");
            defn.SetValue(StyleKeys.ContentTextKey, new Drawing.ContentQuoteDescriptor("open-quote", "“"));
            this.RootStyles.Add(defn);

            defn = new StyleDefn("q:after");
            defn.SetValue(StyleKeys.ContentTextKey, new Drawing.ContentQuoteDescriptor("close-quote", "”"));
            this.RootStyles.Add(defn);




        }
    }
}
