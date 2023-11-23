using System;
namespace Scryber.Html.Components
{
    [PDFParsableComponent("template")]
    public class HTMLTemplate : Scryber.Data.ForEach
    {
        [PDFTemplate()]
        [PDFElement()]
        public override ITemplate Template
        {
            get { return base.Template; }
            set { base.Template =value; }
        }

        [PDFAttribute("data-bind", BindingOnly = true)]
        public override object Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        [PDFAttribute("data-bind-start")]
        public override int StartIndex
        {
            get => base.StartIndex;
            set => base.StartIndex = value;
        }
        
        [PDFAttribute("data-bind-step")]
        public override int Step
        {
            get => base.Step;
            set => base.Step = value;
        }

        [PDFAttribute("data-bind-max")]
        public override int MaxCount
        {
            get => base.MaxCount;
            set => base.MaxCount = value;
        }
        
        

        [PDFAttribute("data-cache-styles")]
        public override bool CacheStyles
        {
            get => base.CacheStyles; 
            set => base.CacheStyles = value;
        }

        [PDFAttribute("data-content")]
        public override string DataContent
        {
            get => base.DataContent;
            set => base.DataContent = value;
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

        #region public string DataStyleIdentifier

        /// <summary>
        /// Gets the identifer for the style of this component that can uniquely identify any set of style attributes across a document
        /// </summary>
        [PDFAttribute("data-style-identifier")]
        public override string DataStyleIdentifier
        {
            get { return base.DataStyleIdentifier; }
            set { base.DataStyleIdentifier = value; }
        }

        #endregion

        public HTMLTemplate() : this(HTMLObjectTypes.Template)
        {
        }

        protected HTMLTemplate(ObjectType type): base(type)
        { }

        protected override ITemplate GetTemplateForBinding(DataContext context, int index, int count)
        {
            if(!string.IsNullOrEmpty(this.DataContent))
            {
                return this.GetDataContent(this.DataContent, context);
            }
            else
                return base.GetTemplateForBinding(context, index, count);
        }
    }
}
