using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Svg.Components
{
    [PDFParsableComponent("var")]
    public class SVGVar : Scryber.Components.Span
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

        public override bool Visible
        {
            get { return false;} set {} }

        [PDFAttribute("title")]
        public override string OutlineTitle
        {
            get
            {
                return string.Empty;
            }
            set
            { 
            }
        }

        private string _dataId;

        [PDFAttribute("data-id")]
        public string DataID
        {
            get { return _dataId; }
            set { this._dataId = value; }
        }

        private object _dataValue;

        [PDFAttribute("data-value", BindingOnly = true)]
        public object DataValue
        {
            get { return _dataValue; }
            set { this._dataValue = value; this.HasDataValue = true; }
        }

        public bool HasDataValue
        {
            get;set;
        }

        public SVGVar()
            : base()
        {
        }
        

        protected override void OnDataBound(DataContext context)
        {
            if(this.DataValue != null && !string.IsNullOrEmpty(this.DataID))
            {
                this.Document.Params[this.DataID] = this.DataValue;

                //Set the visibility based on having contents
                if (this.HasContent == false)
                    this.Visible = false;
            }

            base.OnDataBound(context);
        }
    }



    
}
