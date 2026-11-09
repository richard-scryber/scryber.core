using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("style")]
	public class SVGStyleElement : Scryber.Components.Component
	{

        #region public string Contents

        private string _contents;

        [PDFElement()]
        [PDFCDataContent()]
		public string Contents
		{
			get { return _contents; }
			set { _contents = value;
				this.ClearInnerStyles();
			}
		}

        #endregion

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

        #region public StyleCollection Styles

        /// <summary>
        /// Gets all the styles in this style element, parsed from the contents
        /// </summary>

        public StyleCollection Styles
        {
            get { return this.InnerItems; }
        }

        #endregion

        #region protected StyleGroup ParsedGroup

        private StyleGroup _parsedGroup;

        /// <summary>
        /// Gets the group of styles that have been added to the parent SVGCanvas, parsed from the contents.
        /// </summary>
        protected StyleGroup ParsedGroup
        {
            get { return this._parsedGroup; }
        }

        #endregion

        #region public string StyleType

        [PDFAttribute("type")]
        public string StyleType
        {
            get;
            set;
        }

        #endregion

        #region public string Media

        /// <summary>
        /// The media mime-type for this set of styles
        /// </summary>
        [PDFAttribute("media")]
        public string Media
        {
            get;
            set;
        }

        #endregion

        #region public string Hidden

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

        #endregion

        public SVGStyleElement() : base((ObjectType)"svgS")
		{
		}


        private void AssertInnerStyles(ContextBase context)
        {
            if (!string.IsNullOrEmpty(this.Contents))
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
            this.AddStylesToSVGCanvas(context);
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            if (null == this._parsedGroup)
                this.AddStylesToSVGCanvas(context);

            base.OnPreLayout(context);
        }

        protected void ClearInnerStyles()
        {
            this.InnerItems = null;
            if (null != this._parsedGroup)
                this.RemoveStylesFromSVGCanvas(this._parsedGroup);

            this._parsedGroup = null;
        }

        protected virtual SVGCanvas GetParentCanvas()
        {
            var parent = this.Parent;

            while (null != parent && !(parent is SVGCanvas))
                parent = parent.Parent;

            return parent as SVGCanvas;
        }

        protected virtual StyleCollection CreateInnerStyles(ContextBase context)
        {
            var collection = new StyleCollection(this);
            this.AddCssStyles(collection, context);

            return collection;
        }

        protected virtual void AddStylesToSVGCanvas(ContextBase context)
        {
            if(this.Visible && null != this.Styles)
            {
                var svg = this.GetParentCanvas();
                if (null == svg)
                {
                    context.TraceLog.Add(TraceLevel.Warning, "SVG", "The inner style element contents could not be added to a parent, as they are not contained within a canvas");
                    return;
                }

                StyleGroup grp = new StyleGroup();
                foreach (var style in this.Styles)
                {
                    grp.Styles.Add(style);
                }

                svg.Styles.Add(grp);
                this._parsedGroup = grp;
            }
        }

        protected virtual void RemoveStylesFromSVGCanvas(StyleGroup toRemove)
        {
            var svg = this.GetParentCanvas();
            if(null != svg)
            {
                svg.Styles.Remove(toRemove);
            }
        }

        public void AddCssStyles(StyleCollection toCollection, ContextBase context)
        {
            if (!string.IsNullOrEmpty(this.StyleType))
            {
                if (this.StyleType.Equals("text/css", StringComparison.OrdinalIgnoreCase) == false)
                    return;
            }

            if (!string.IsNullOrEmpty(this.Media))
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

            if (!string.IsNullOrEmpty(this.Contents))
            {
                var parser = new Scryber.Styles.Parsing.CSSStyleParser(this.Contents, context);
                foreach (var style in parser)
                {
                    if (null != style)
                        toCollection.Add(style);
                }
            }

        }
    }
}

