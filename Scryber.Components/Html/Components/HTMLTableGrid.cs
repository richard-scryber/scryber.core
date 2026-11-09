using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;
using Scryber.Components;
using System.Transactions;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("table")]
    public class HTMLTableGrid : Scryber.Components.TableGrid
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

        private string _cellSpacing;

        /// <summary>
        /// The html cellspacing attribute - the space between the cells in pixels (or any css length). 
        /// It is the same as border-spacing, but any css border-spacing will override it.
        /// </summary>
        [PDFAttribute("cellspacing")]
        public string CellSpacing
        {
            get { return _cellSpacing; }
            set { _cellSpacing = value; }
        }

        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();

            if (!string.IsNullOrWhiteSpace(_cellSpacing))
            {
                var text = _cellSpacing.Trim();
                Scryber.Drawing.Unit spacing;

                //A plain number is in pixels
                if (double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var number))
                    spacing = Scryber.Drawing.Unit.Px(number);
                else if (!Scryber.Drawing.Unit.TryParse(text, out spacing))
                    return style;

                if (spacing.IsRelative || spacing > Scryber.Drawing.Unit.Zero)
                {
                    style.SetValue(StyleKeys.TableBorderSpacingHorizontalKey, spacing);
                    style.SetValue(StyleKeys.TableBorderSpacingVerticalKey, spacing);
                }
            }

            return style;
        }

        public HTMLTableGrid()
            : this(HTMLObjectTypes.Table)
        {
        }

        protected HTMLTableGrid(ObjectType type) : base(type)
        { }
    }

}
