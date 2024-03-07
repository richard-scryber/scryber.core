﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Styles.Parsing.Typed;
using Scryber.PDF;

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

        #region protected StyleCollection InnerItems

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

        #region public StyleCollection Styles

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

        public HTMLStyle() : this(HTMLObjectTypes.StyleTag)
        {
        }

        protected HTMLStyle(ObjectType type) : base(type)
        { }

        private void AssertInnerStyles(ContextBase context)
        {
            //TODO: Check with the hidden attribute whether it should be included
            if(!string.IsNullOrEmpty(this.Contents))
            {
                if (null == this._innerItems)
                    this._innerItems = CreateInnerStyles(context);
            }
        }

        protected override void OnLoaded(LoadContext context)
        {
            AssertInnerStyles(context);
            base.OnLoaded(context);
        }

        protected override void OnDataBinding(DataContext context)
        {
            AssertInnerStyles(context);
            base.OnDataBinding(context);

            if (null != this._innerItems)
                this._innerItems.DataBind(context);
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);
            this.AddStylesToDocument();
        }

        protected override void OnPreLayout(LayoutContext context)
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

        protected StyleCollection CreateInnerStyles(ContextBase context)
        {
            var collection = new StyleCollection(this);
            this.AddCssStyles(collection, context);
            
            return collection;
        }

        protected virtual void AddStylesToDocument()
        {
            if (this.Visible && null != this.Styles)
            {
                StyleGroup grp;

                if (!string.IsNullOrEmpty(this.LoadedSource))
                    grp = new StyleRemoteGroup() { LoadedSource = this.LoadedSource };
                else
                    grp = new StyleGroup();

                foreach (var style in this.Styles)
                {
                    grp.Styles.Add(style);
                }

                this.Document.Styles.Add(grp);
                this._parsedGroup = grp;
            }
        }

        protected virtual void AddCssStyles(StyleCollection collection, ContextBase context)
        {
            if(!string.IsNullOrEmpty(this.StyleType))
            {
                if (this.StyleType.Equals("text/css", StringComparison.OrdinalIgnoreCase) == false)
                    return;
            }

            if(!string.IsNullOrEmpty(this.Media))
            {
                if (this.Media == "all" || this.Media.StartsWith("all "))
                {
                    //we want to use this
                }
                else if (this.Media == "print" || this.Media.StartsWith("print "))
                {
                    //we want to use this too
                }
                else //we dont want it
                    return;
            }

            var parser = new Scryber.Styles.Parsing.CSSStyleParser(this.Contents, context);
            foreach (var style in parser)
            {
                if (null != style)
                    collection.Add(style);
            }

        }

        public override string MapPath(string source, out bool isfile)
        {
            return base.MapPath(source, out isfile);
        }


    }
}
