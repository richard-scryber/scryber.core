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
            style.Position.DisplayMode = DisplayMode.InlineBlock;
            
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
                    this.Size = 1;
                    style.Margins.Top = 0;
                    style.Margins.Bottom = 0;
                    style.Margins.Left = 0;
                    style.Margins.Right = 0;
                    style.Font.FontFamily = (FontSelector)"zapf dingbats";
                    style.Size.Width = Unit.Em(1);
                    style.Size.Height = Unit.Em(1);
                    
                    style.Border.Color = StandardColors.Black;
                    style.Border.LineStyle = LineType.Solid;
                    if (this.ButtonType == FormButtonFieldType.Radio)
                    {
                        style.Border.CornerRadius = Unit.Em(0.5);
                        style.Border.Width = 2;
                    }
                    else
                    {
                        style.Border.Width = 1;
                    }

                    style.Font.FontSize = Unit.Percent(75);
                    style.Position.HAlign = HorizontalAlignment.Center;
                    //style.Position.DisplayMode = DisplayMode.Inline;
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
            
            return style;
        }
    }
}
