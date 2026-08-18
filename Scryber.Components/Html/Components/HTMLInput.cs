using System;
using System.Diagnostics;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("input")]
    public class HTMLInput : FormInputField
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

        public HTMLInput() : this(HTMLObjectTypes.FormInput)
        {
        }

        protected HTMLInput(ObjectType type) : base(type)
        {
            this.Size = 20;
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            if (this.FieldType == FormInputFieldType.Button)
            {
                if (this.ButtonType == FormButtonFieldType.PushButton)
                {
                    style.Background.Color = StandardColors.Silver;
                    style.Fill.Color = StandardColors.Black;
                    style.Border.Color = StandardColors.Black;
                    style.Border.LineStyle = LineType.Solid;
                    style.Border.Width = 1;
                }
                else if (this.ButtonType == FormButtonFieldType.CheckBox ||
                         this.ButtonType == FormButtonFieldType.Radio)
                {
                    style.Margins.Top = Unit.Zero;
                    style.Margins.Bottom = Unit.Zero;
                    style.Margins.Left = Unit.Zero;
                    style.Margins.Right = Unit.Zero;
                }
            }
            else if (this.FieldType == FormInputFieldType.Signature)
            {
                style.Size.Width = Unit.Ex(20);
                style.Size.Height = Unit.Em(4);
            }
            else if (this.Size > 0)
            {
            }
            else
            {
                style.Size.FullWidth = true;
            }
            style.Position.DisplayMode = DisplayMode.InlineBlock;
            return style;
        }
    }
}
