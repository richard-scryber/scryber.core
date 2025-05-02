using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("var")]
    public class HTMLVar : Scryber.Components.Span
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

        public HTMLVar()
            : this(HTMLObjectTypes.Var)
        {
        }

        protected HTMLVar(ObjectType type): base(type)
        { }


        protected override Style GetBaseStyle()
        {
            var style = base.GetBaseStyle();
            //Var is by default italic
            style.Font.FontFaceStyle = Drawing.FontStyle.Italic;
            return style;
        }

        protected override void OnDataBound(DataContext context)
        {
            if(this.DataValue != null && !string.IsNullOrEmpty(this.DataID))
            {
                this.Document.Params[this.DataID] = this.DataValue;
            }

            base.OnDataBound(context);
        }
    }



    
}
