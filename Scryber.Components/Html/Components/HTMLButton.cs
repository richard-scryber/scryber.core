using System;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("button")]
    public class HTMLButton : FormInputField
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
        /// Re-declared (with [PDFElement]) so the parser attaches literal inner text
        /// (e.g. &lt;button&gt;Save&lt;/button&gt;) to Contents - FormInputField itself
        /// doesn't mark Contents as an element target, since &lt;input&gt; has no inner
        /// text content in HTML.
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        public HTMLButton() : this(HTMLObjectTypes.Button)
        {
        }

        protected HTMLButton(ObjectType type) : base(type)
        {
            //HTML defaults a <button> with no type= attribute to "submit" - unlike <input>,
            //which defaults to "text". An explicit type= attribute overrides this afterwards.
            this.FieldType = FormInputFieldType.Button;
            this.Options |= FormFieldOptions.Pushbutton;
            this.SubmitBehavior = FormSubmitBehavior.Submit;
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);
            this.HarvestInnerTextAsValueIfEmpty();
        }
    }
}
