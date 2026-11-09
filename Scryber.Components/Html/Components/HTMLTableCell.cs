using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{

    [PDFParsableComponent("td")]
    public class HTMLTableCell : Scryber.Components.TableCell
    {
        [PDFAttribute("class")]
        public override string StyleClass { get => base.StyleClass; set => base.StyleClass = value; }

        [PDFAttribute("style")]
        public override Style Style { get => base.Style; set => base.Style = value; }

        [PDFAttribute("colspan")]
        public override int CellColumnSpan { get => base.CellColumnSpan; set => base.CellColumnSpan = value; }

        [PDFAttribute("rowspan")]
        public override int CellRowSpan { get => base.CellRowSpan; set => base.CellRowSpan = value; }

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

        [PDFAttribute("align")]
        public override HorizontalAlignment HorizontalAlignment
        {
            get => base.HorizontalAlignment;
            set => base.HorizontalAlignment = value;
        }

        [PDFAttribute("valign")]
        public override VerticalAlignment VerticalAlignment
        {
            get => base.VerticalAlignment;
            set => base.VerticalAlignment = value;
        }
        
        public HTMLTableCell()
            : this(HTMLObjectTypes.TableCell)
        {
        }

        protected HTMLTableCell(ObjectType type) : base(type)
        { }
    }

    [PDFParsableComponent("th")]
    public class HTMLTableHeaderCell : Scryber.Components.TableHeaderCell
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

        /// <summary>
        /// Allows definition of whether a header cell is a header for a column, row, or group of columns or rows. Has no effect on output
        /// </summary>
        [PDFAttribute("scope")]
        public string HeaderScope
        {
            get;
            set;
        }
        
        [PDFAttribute("align")]
        public override HorizontalAlignment HorizontalAlignment
        {
            get => base.HorizontalAlignment;
            set => base.HorizontalAlignment = value;
        }

        [PDFAttribute("valign")]
        public override VerticalAlignment VerticalAlignment
        {
            get => base.VerticalAlignment;
            set => base.VerticalAlignment = value;
        }
        
        [PDFAttribute("colspan")]
        public override int CellColumnSpan { get => base.CellColumnSpan; set => base.CellColumnSpan = value; }

        [PDFAttribute("rowspan")]
        public override int CellRowSpan { get => base.CellRowSpan; set => base.CellRowSpan = value; }

        public HTMLTableHeaderCell()
            : this(HTMLObjectTypes.TableHeaderCell)
        {
        }

        protected HTMLTableHeaderCell(ObjectType type) : base(type)
        { }
    }

}
