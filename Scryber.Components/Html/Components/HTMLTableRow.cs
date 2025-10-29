using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("tr")]
    public class HTMLTableRow : Scryber.Components.TableRow
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

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

        [PDFAttribute("align")]
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.PositionHAlignKey, HorizontalAlignment.Left);
                else
                    return HorizontalAlignment.Left;
            }
            set
            {
                this.Style.SetValue(StyleKeys.PositionHAlignKey, value);
            }
        }

        [PDFAttribute("valign")]
        public VerticalAlignment VerticalAlignment
        {
            get
            {
                if (this.HasStyle)
                    return this.Style.GetValue(StyleKeys.PositionVAlignKey, VerticalAlignment.Top);
                else
                    return VerticalAlignment.Top;
            }
            set
            {
                this.Style.SetValue(StyleKeys.PositionVAlignKey, value);
            }
        }
        

        public HTMLTableRow()
            : this(HTMLObjectTypes.TableRow)
        {
        }

        protected HTMLTableRow(ObjectType type) : base(type)
        { }
    }

}
