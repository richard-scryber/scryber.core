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
    public class PDFHtmlStyleDefnGroup : Scryber.Components.Component
    {
        private string _contents;
        private bool _parsed = false;

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

        private PDFStyleCollection _innerItems;

        protected PDFStyleCollection InnerItems
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
        
        public PDFStyleCollection Styles
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

        public PDFHtmlStyleDefnGroup() : base((PDFObjectType)"htmS")
        {
            
        }

        public override PDFStyle GetAppliedStyle(Component forComponent, PDFStyle baseStyle)
        {
            this.Styles.MergeInto(baseStyle, forComponent, ComponentState.Normal);
            return baseStyle;
        }

        protected void ClearInnerStyles()
        {
            this.InnerItems = null;
            this._parsed = false;
        }

        protected PDFStyleCollection CreateInnerStyles()
        {
            var collection = new PDFStyleCollection();
            this.AddCssStyles(collection);
            this._parsed = true;
            return collection;
        }


        protected virtual void AddCssStyles(PDFStyleCollection collection)
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
