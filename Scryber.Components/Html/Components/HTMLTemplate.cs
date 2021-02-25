using System;
namespace Scryber.Html.Components
{
    [PDFParsableComponent("template")]
    public class HTMLTemplate : Scryber.Data.ForEach
    {
        [PDFTemplate()]
        [PDFElement()]
        public IPDFTemplate TemplateContent
        {
            get { return base.Template; }
            set { base.Template =value; }
        }

        [PDFAttribute("data-bind", BindingOnly = true)]
        public object DataBindValue
        {
            get { return base.Value; }
            set { base.Value = value; }
        }

        [PDFAttribute("data-show", BindingOnly = true)]
        public bool DataShowValue
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        [PDFAttribute("data-cache-styles")]
        public override bool CacheStyles { get => base.CacheStyles; set => base.CacheStyles = value; }

        [PDFAttribute("data-content")]
        public string DataContent
        {
            get; set;
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

        public HTMLTemplate()
        {
        }

        protected override IPDFTemplate GetTemplateForBinding(PDFDataContext context, int index, int count)
        {
            if(null != this.DataContent)
            {
                return this.GetDataContent(this.DataContent, context);
            }
            else
                return base.GetTemplateForBinding(context, index, count);
        }
    }
}
