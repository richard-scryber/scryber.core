using System;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Text;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("num")]
    public class HTMLNumber : Scryber.Components.Number
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        
        [PDFElement()]
        public string Text { get; set; }

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


        [PDFAttribute("data-format")]
        public override string NumberFormat
        {
            get
            {
                return base.NumberFormat;
            }
            set
            {
                base.NumberFormat = value;
            }
        }

        [PDFAttribute("data-value")]
        public override double Value { 
            get => base.Value; 
            set=> base.Value = value; 
        }


        public HTMLNumber() : this(HTMLObjectTypes.Number)
        {
        }

        protected HTMLNumber(ObjectType type) : base(type)
        {
            
        }

        protected override PDFTextReader CreateReader(ContextBase context, Style fullstyle)
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                double parsed;
                //TODO: Use the lang attribute when converting the number
                if (double.TryParse(this.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out parsed))
                {
                    this.Value = parsed;
                    return base.CreateReader(context, fullstyle);
                }
                else
                    return Scryber.Text.PDFTextReader.Create(this.Text, TextFormat.XHTML, false, context.TraceLog);
            }
            else
                return base.CreateReader(context, fullstyle);
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);
        }
    }
}
