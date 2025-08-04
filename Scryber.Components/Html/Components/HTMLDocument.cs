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
        private HTMLFrameset _frameset;

        /// <summary>
        /// When using the body, new document content can be defined. If the frameset is also set then an <see cref="InvalidOperationException" /> will be raised.
        /// </summary>
        /// <remarks>A body and </remarks>
        [PDFElement("body")]
        public HTMLBody Body
        {
            get { return this._body; }
            set
            {
                if (null != this.Frameset && null != value)
                    throw new InvalidOperationException(
                        "A body and a frameset cannot be applied to the same document - use one or the other.");
                
                if (null != this._body)
                    this.Pages.Remove(this._body);

                this._body = value;

                if (null != this._body)
                    this.Pages.Add(_body);
            }
        }

        /// <summary>
        /// When using the frameset, existing documents can be modifed and new content added. If the body is also set then an <see cref="InvalidOperationException" /> will be raised.
        /// </summary>
        /// <remarks>A body and </remarks>
        [PDFElement("frameset")]
        public HTMLFrameset Frameset
        {
            get { return this._frameset; }
            set
            {
                if (null != this.Body && null != value)
                    throw new InvalidOperationException(
                        "A body and a frameset cannot be applied to the same document - use one or the other.");
                
                if (null != this._frameset)
                    this.InnerContent.Remove(this._frameset);

                this._frameset = value;

                if (null != this._frameset)
                    this.InnerContent.Add(this._frameset);
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

            if (this.HasParams && this.HasRootStyles)
                this.EnsureParamsInRoots();
            return style;
        }

        private StyleCollection _roots;

        /// <summary>
        /// The root styles apply the User Agent style sheet styles to components.
        /// Inheritors can override the AddRootStyles to alter or extend any of these styles,
        /// and they will be applied to each component.
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

        public bool HasRootStyles
        {
            get{ return null != this._roots && this._roots.Count > 0; }
        }

        
        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
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

        
        
        protected virtual void EnsureParamsInRoots()
        {
            StyleDefn innerRoot = new StyleDefn(":root");
            foreach (var key in this.Params.Keys)
            {
                if (key is string str && str.StartsWith("--"))
                {
                    var value = this.Params[str];
                    
                    if (null != value)
                        innerRoot.AddVariable(str, value.ToString());

                }
            }
            
            if(innerRoot.HasVariables)
                this.RootStyles.Add(innerRoot);
        }
    }
}
