using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("body")]
    public class HTMLBody : Scryber.Components.Section
    {

        private List<IComponent> _boundAdornments;

        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
        [PDFElement("")]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        [PDFElement("header")]
        [PDFTemplate(IsBlock= true)]
        public override ITemplate Header { get => base.Header; set => base.Header = value; }

        [PDFElement("footer")]
        [PDFTemplate(IsBlock = true)]
        public override ITemplate Footer { get => base.Footer; set => base.Footer = value; }

        [PDFElement("continuation-header")]
        [PDFTemplate(IsBlock = true)]
        public override ITemplate ContinuationHeader { get => base.ContinuationHeader; set => base.ContinuationHeader = value; }

        [PDFElement("continuation-footer")]
        [PDFTemplate(IsBlock = true)]
        public override ITemplate ContinuationFooter { get => base.ContinuationFooter; set => base.ContinuationFooter = value; }

        protected virtual bool HasAdornments
        {
            get
            {
                return null != this.Header || null != this.ContinuationHeader ||
                       null != this.Footer  || null != this.ContinuationFooter;
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
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLBody()
            : this(HTMLObjectTypes.Body)
        {
            
        }

        protected HTMLBody(ObjectType type): base(type)
        {

        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.SetValue(StyleKeys.MarginsAllKey, 8);
            return base.GetBaseStyle();
        }

        protected override void OnDataBinding(DataContext context)
        {
            if (this.HasAdornments)
            {
                this.PrepareAdornmentBinding(context);
                this.DoBindAdornments(context);
            }

            //This will take care of the content binding.
            base.OnDataBinding(context);
        }

        protected virtual int DoBindAdornments(DataContext context)
        {
            var boundCount = 0;
            
            if (null != this.Header)
            {
                boundCount += this.DoBindTemplateIntoContent(this.Header, boundCount, context);
            }
                
            if(null != this.ContinuationHeader)
            {
                boundCount += this.DoBindTemplateIntoContent(this.ContinuationHeader, boundCount, context);
            }

            if (null != this.ContinuationFooter)
            {
                boundCount += this.DoBindTemplateIntoContent(this.ContinuationFooter, boundCount, context);
            }

            if (null != this.Footer)
            {
                boundCount += this.DoBindTemplateIntoContent(this.Footer, boundCount, context);
            }

            return boundCount;
        }

        protected virtual void PrepareAdornmentBinding(DataContext context)
        {
            this._boundAdornments = new List<IComponent>();
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            base.OnPreLayout(context);
            this.ClearBoundContent();
        }
        
        private int DoBindTemplateIntoContent(ITemplate template, int index, DataContext context)
        {
            var bound = 0;
            var components = template.Instantiate(index, this);
            var container = (IContainerComponent)this;
            IComponentList list = container.Content;

            if (null != components)
            {
                var init = new InitContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document, context.Format);
                var load = new LoadContext(context.Items, context.TraceLog, context.PerformanceMonitor, this.Document, context.Format);

                foreach (var c in components)
                {
                    list.Insert(index, c);
                    c.Init(init);
                    this._boundAdornments.Add(c);
                    c.Load(load);
                    bound++;
                }
            }
            return bound;
        }

        private void ClearBoundContent()
        {
            if (null != this._boundAdornments && this._boundAdornments.Count > 0)
            {
                var container = (IContainerComponent)this;
                IComponentList list = container.Content;

                for (int i = this._boundAdornments.Count - 1; i >= 0; i--)
                {
                    list.Remove(this._boundAdornments[i]);
                }
            }
        }

    }
}
