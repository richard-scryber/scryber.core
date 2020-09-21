using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

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
                if (this._innerItems == null)
                    this._innerItems = CreateInnerStyles();
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

        protected override void OnPreLayout(PDFLayoutContext context)
        {
             
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

        protected StyleCollection CreateInnerStyles()
        {
            var collection = new StyleCollection();
            this.AddCssStyles(collection);
            
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

        protected virtual void AddCssStyles(StyleCollection collection)
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

            var parser = new Scryber.Styles.Parsing.CSSStyleParser(this.Contents);
            foreach (var style in parser)
            {
                if (null != style)
                    collection.Add(style);
            }

        }


    }
}
