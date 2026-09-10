using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    /// <summary>
    /// A genuine box-model &lt;hr&gt; - a block box with a default border-bottom, rather than a
    /// stroked diagonal Line. Standard CSS (border-*, background-color, height, width, margin)
    /// behaves normally, matching how every other block element is styled - setting height no
    /// longer draws a diagonal line, and a background-color + height reset (the most common
    /// real-world "coloured bar" hr pattern) now actually renders one.
    /// </summary>
    [PDFParsableComponent("hr")]
    public class HTMLHorizontalRule : Scryber.Components.Div
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

        public HTMLHorizontalRule()
            : this(HTMLObjectTypes.HRule)
        {
        }

        protected HTMLHorizontalRule(ObjectType type) : base(type)
        { }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            style.Margins.Top = Unit.Em(0.5);
            style.Margins.Bottom = Unit.Em(0.5);
            style.Size.FullWidth = true;
            return style;
        }

        /// <summary>
        /// Applies the default visible rule (a plain 1pt solid black border-bottom, matching the
        /// single line a bare &lt;hr&gt; with no author CSS has always rendered as) once the element
        /// (including any style attribute) has been fully parsed - mirroring HTMLImage's own
        /// intrinsic-attribute-vs-styled-dimension check.
        ///
        /// This can't live in GetBaseStyle(): border-side resolution (StyleBase.DoCreateBorderSidePen)
        /// picks between a specific per-side key (e.g. BorderBottomWidthKey) and the general shorthand
        /// key (BorderWidthKey, from a plain `border: ...`) by comparing their StyleValue.Priority, not
        /// by cascade origin - a base-style default and an author's inline style value get the same
        /// (default) priority, so a default set directly on the SPECIFIC per-side key would incorrectly
        /// keep winning over an author's LESS specific `border: ...` shorthand (which only ever sets the
        /// general key), even though the shorthand is what the author actually wrote. Checking
        /// IsValueDefined here and only setting the default when the author hasn't touched border-* at
        /// all sidesteps that entirely - real author CSS of any form always wins outright.
        /// </summary>
        protected override void OnInitialized(InitContext context)
        {
            bool hasBorder = this.Style.IsValueDefined(StyleKeys.BorderWidthKey)
                           || this.Style.IsValueDefined(StyleKeys.BorderStyleKey)
                           || this.Style.IsValueDefined(StyleKeys.BorderColorKey)
                           || this.Style.IsValueDefined(StyleKeys.BorderBottomWidthKey)
                           || this.Style.IsValueDefined(StyleKeys.BorderBottomStyleKey)
                           || this.Style.IsValueDefined(StyleKeys.BorderBottomColorKey);

            if (!hasBorder)
            {
                this.Style.SetValue(StyleKeys.BorderBottomWidthKey, new Unit(1, PageUnits.Points));
                this.Style.SetValue(StyleKeys.BorderBottomColorKey, StandardColors.Black);
                this.Style.SetValue(StyleKeys.BorderBottomStyleKey, LineType.Solid);
            }

            base.OnInitialized(context);
        }
    }
}
