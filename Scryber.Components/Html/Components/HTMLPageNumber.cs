using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Text;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Explicit sub class of the PDFDiv under the HTML Namespace that can be styled independantly
    /// </summary>
    [PDFParsableComponent("page")]
    public class HTMLPageNumber : Scryber.Components.PageNumberLabel
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("data-format")]
        public override string DisplayFormat { get => base.DisplayFormat; set => base.DisplayFormat = value; }

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

        [PDFAttribute("property")]
        public string Property { get; set; }

        [PDFAttribute("for")]
        public string ForComponent { get; set; }

        [PDFAttribute("data-page-hint")]
        public override int TotalPageCountHint { get => base.TotalPageCountHint; set => base.TotalPageCountHint = value; }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get => base.OutlineTitle;
            set => base.OutlineTitle = value;
        }

        public HTMLPageNumber()
            : this(HTMLObjectTypes.PageNumber)
        {
        }

        protected HTMLPageNumber(ObjectType type) : base(type)
        { }

        private const string CurrentPage = "{0}";
        private const string TotalPageCountFormat = "{1}";
        private const string SectionPage = "{2}";
        private const string SectionTotal = "{3}";

        protected override string GetDisplayText(int pageindex, Style style, bool rendering)
        {
            int index = pageindex;
            if(!string.IsNullOrEmpty(this.ForComponent))
            {
                index = -1;
                var found = this.LookupExternalComponent(rendering, this.ForComponent);
                if (null != found)
                {
                    index = found.PageLayoutIndex;
                }
            }
            return base.GetDisplayText(index, style, rendering);
        }


        protected override string GetPageFormat(Style full)
        {
            if(!string.IsNullOrEmpty(this.Property))
            {
                switch (this.Property.ToLower())
                {
                    case "current":
                    case "c":
                        return CurrentPage;
                    case ("total"):
                    case ("t"):
                        return TotalPageCountFormat;
                    case ("section"):
                    case ("s"):
                        return SectionPage;
                    case ("sectiontotal"):
                    case ("st"):
                        return SectionTotal;
                    default:
                        break;
                }
            }

            return base.GetPageFormat(full);
        }

        #region private void LookupExternalComponent(PDFLayoutContext context, string name)

        /// <summary>
        /// Looks for the component with the specified name or ID and sets instance variables appropriately
        /// </summary>
        /// <param name="rendering"></param>
        /// <param name="name"></param>
        private Component LookupExternalComponent(bool rendering, string name)
        {
            Component comp;

            if (name.StartsWith(Const.ComponentIDPrefix))
            {
                name = name.Substring(Const.ComponentIDPrefix.Length);
                comp = this.Document.FindAComponentById(name);
            }
            else
            {
                comp = Document.FindAComponentByName(name);
            }

            return comp;
        }

        #endregion

    }
    
    
    
    [PDFParsableComponent("page-number")]
    public class HTMLCurrentPageNumber : HTMLPageNumber
    {
        public HTMLCurrentPageNumber()
        {
            this.Property = "current";
        }
    }

    [PDFParsableComponent("page-count")]
    public class HTMLPageCount : HTMLPageNumber
    {
        public HTMLPageCount()
        {
            this.Property = "total";
        }
    }
}
