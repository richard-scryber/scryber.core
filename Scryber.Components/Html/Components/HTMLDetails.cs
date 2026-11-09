using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// Explicit sub class of the PDFDiv under the HTML Namespace that can be styled independantly
    /// </summary>
    [PDFParsableComponent("details")]
    public class HTMLDetails : Scryber.Components.Panel
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

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

        [PDFAttribute("open")]
        public string IsOpen
        {
            get;
            set;
        }

        [PDFArray(typeof(Component))]
        [PDFElement()]
        public override ComponentList Contents => base.Contents;


        public HTMLDetails()
            : this(HTMLObjectTypes.Details)
        {
        }

        protected HTMLDetails(ObjectType type): base(type)
        { }


        public override Style GetAppliedStyle(Component forComponent, Style baseStyle)
        {
            var style = base.GetAppliedStyle(forComponent, baseStyle);
            style.Size.FullWidth = true;
            return style;
        }

        protected override void OnPreLayout(LayoutContext context)
        {
            HTMLDetailsSummary summary = null;
            foreach (var item in this.Contents)
            {
                if(item is HTMLDetailsSummary sum)
                {
                    if (null == summary)
                        summary = sum;
                    else
                        context.TraceLog.Add(TraceLevel.Warning, "Details Component", "Multiple summary tags found within a details component. Only the first will appear at the top");
                }
            }

            if(null != summary)
            {
                this.Contents.MoveTo(summary, 0);
            }

            if(!string.IsNullOrEmpty(this.IsOpen) &&
                string.Equals(this.IsOpen, "false", StringComparison.InvariantCultureIgnoreCase) || string.Equals(this.IsOpen, "closed", StringComparison.InvariantCultureIgnoreCase))
            {
                context.TraceLog.Add(TraceLevel.Message, "Details Component", "Hiding all inner details content for the component " + this.ID + " as the open flag is set to false");
                int first = 0;

                if (this.Contents[0] is HTMLDetailsSummary)
                    first = 1;

                for (var i = first; i < this.Contents.Count; i++)
                {
                    this.Contents[i].Visible = false;
                }
            }

            base.OnPreLayout(context);
        }
    }
}
