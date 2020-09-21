using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("link")]
    public class HTMLLink : Scryber.Components.Component
    {
        private string _href;
        private string _relationship;
        private StyleGroup _parsedGroup = null;

        [PDFAttribute("href")]
        public string Href
        {
            get { return _href; }
            set
            {
                this._href = value;
                this.ClearInnerStyles();
                this.DoLoadReference();
            }
        }

        #region protected PDFStyleCollection InnerItems

        private StyleCollection _innerItems;

        protected StyleCollection InnerItems
        {
            get
            {
                if (this._innerItems == null)
                    this.DoLoadReference();
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


        [PDFAttribute("rel")]
        public string Relationship
        {
            get { return this._relationship; }
            set
            {
                this._relationship = value;
                this.ClearInnerStyles();
                this.DoLoadReference();
            }
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

        public HTMLLink()
            : base((PDFObjectType)"htmL")
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

        protected virtual void DoLoadReference()
        {
            if (String.IsNullOrEmpty(this.Href) || String.IsNullOrEmpty(this.Relationship) || this.Relationship != "stylesheet")
                return;
            if (null == this.Document)
                return;

            bool isFile;
            
            var path = this.MapPath(this.Href, out isFile);

            if(!isFile && Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                DoLoadRemoteReference(path);
            }
            else if(isFile && System.IO.File.Exists(path))
            {
                var css = System.IO.File.ReadAllText(path);
                this.InnerItems = this.CreateInnerStyles(css);
            }
        }

        protected virtual void DoLoadRemoteReference(string path)
        {

        }

        protected StyleCollection CreateInnerStyles(string content)
        {
            var collection = new StyleCollection();
            this.AddCssStyles(collection, content);
            
            return collection;
        }

        protected virtual void AddStylesToDocument()
        {
            if (this.Visible && this.Styles.Count > 0)
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

        protected virtual void AddCssStyles(StyleCollection collection, string content)
        {
            if(!string.IsNullOrEmpty(this.Relationship))
            {
                if (this.Relationship.Equals("stylesheet", StringComparison.OrdinalIgnoreCase) == false)
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

            var parser = new Scryber.Styles.Parsing.CSSStyleParser(content);
            foreach (var style in parser)
            {
                if (null != style)
                    collection.Add(style);
            }

        }


    }
}
