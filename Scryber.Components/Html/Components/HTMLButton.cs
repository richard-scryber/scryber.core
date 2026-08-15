using System;
using Scryber.Components;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("button")]
    public class HTMLButton : FormInputField
    {
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
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);
            this.HarvestInnerTextAsValueIfEmpty();
        }
    }
}
