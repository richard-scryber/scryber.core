using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Styles.Parsing.Typed;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("style")]
    public class HTMLStyle : Scryber.Components.Component
    {
        private string _contents;
        private StyleGroup _parsedGroup = null;

        [PDFElement()]
        public string Contents
        {
            get { return _contents; }
            set
            {
                this._contents = value;
                this.ClearInnerStyles();
            }
        }

        #region protected PDFStyleCollection InnerItems

        private StyleCollection _innerItems;

        protected StyleCollection InnerItems
        {
            get
            {
                return this._innerItems;
            }
            set
            {
                this._innerItems = value;
            }
        }

        #endregion

        #region public PDFStyleCollection Styles

        /// <summary>
        /// Gets all the styles in this group
        /// </summary>
        
        public StyleCollection Styles
        {
            get { return this.InnerItems; }
        }

        #endregion


        [PDFAttribute("type")]
        public string StyleType
        {
            get;
            set;
        }

        [PDFAttribute("media")]
        public string Media
        {
            get;
            set;
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

        public HTMLStyle() : base((PDFObjectType)"htmS")
        {
            
        }

        private void AssertInnerStyles(PDFContextBase context)
        {
            if(!string.IsNullOrEmpty(this.Contents))
            {
                if (null == this._innerItems)
                    this._innerItems = CreateInnerStyles(context);
            }
        }

        protected override void OnLoaded(PDFLoadContext context)
        {
            AssertInnerStyles(context);
            base.OnLoaded(context);
        }

        protected override void OnDataBinding(PDFDataContext context)
        {
            AssertInnerStyles(context);
            base.OnDataBinding(context);

            if (null != this._innerItems)
                this._innerItems.DataBind(context);
        }

        protected override void OnDataBound(PDFDataContext context)
        {
            base.OnDataBound(context);
            this.AddStylesToDocument();
        }

        protected override void OnPreLayout(PDFLayoutContext context)
        {
            if (null == this._parsedGroup)
                this.AddStylesToDocument();

            base.OnPreLayout(context);
        }

        protected void ClearInnerStyles()
        {
            this.InnerItems = null;
            if (this._parsedGroup != null)
                this.Document.Styles.Remove(this._parsedGroup);
            this._parsedGroup = null;
        }

        protected StyleCollection CreateInnerStyles(PDFContextBase context)
        {
            var collection = new StyleCollection(this);
            this.AddCssStyles(collection, context);
            
            return collection;
        }

        protected virtual void AddStylesToDocument()
        {
            if (this.Visible)
            {
                StyleGroup grp = new StyleGroup();
                foreach (var style in this.Styles)
                {
                    grp.Styles.Add(style);
                }

                this.Document.Styles.Add(grp);
                this._parsedGroup = grp;
            }
        }

        protected virtual void AddCssStyles(StyleCollection collection, PDFContextBase context)
        {
            if(!string.IsNullOrEmpty(this.StyleType))
            {
                if (this.StyleType.Equals("text/css", StringComparison.OrdinalIgnoreCase) == false)
                    return;
            }

            if(!string.IsNullOrEmpty(this.Media))
            {
                if (this.Media == "all" || this.Media.StartsWith("all "))
                    ;
                else if (this.Media == "print" || this.Media.StartsWith("print "))
                    ;
                else
                    return;
            }

            var parser = new Scryber.Styles.Parsing.CSSStyleParser(this.Contents, context);
            foreach (var style in parser)
            {
                if (null != style)
                    collection.Add(style);
            }

        }


    }
}
