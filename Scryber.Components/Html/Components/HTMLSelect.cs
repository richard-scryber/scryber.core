using System;
using System.Linq;
using Scryber.Components;
using Scryber.PDF;

namespace Scryber.Html.Components
{
    [PDFParsableComponent("select")]
    public class HTMLSelect : FormInputField
    {
        /// <summary>
        /// Re-declared (with [PDFElement]) so the parser attaches the &lt;option&gt; children
        /// to Contents - FormInputField itself doesn't mark Contents as an element target,
        /// since &lt;input&gt; has no children in HTML.
        /// </summary>
        [PDFElement()]
        [PDFArray(typeof(Component))]
        public override ComponentList Contents
        {
            get { return base.Contents; }
        }

        [PDFAttribute("multiple")]
        public HtmlBoolean Multiple
        {
            get { return (this.Options & FormFieldOptions.Multiselect) == FormFieldOptions.Multiselect; }
            set
            {
                if (value)
                    this.Options |= FormFieldOptions.Multiselect;
                else
                    this.Options &= ~FormFieldOptions.Multiselect;
            }
        }

        /// <summary>
        /// The choices for this select, harvested from its &lt;option&gt; children during data binding.
        /// </summary>
        public FormFieldOptionList Choices { get; private set; }

        public HTMLSelect() : this(HTMLObjectTypes.Select)
        {
        }

        protected HTMLSelect(ObjectType type) : base(type)
        {
            this.FieldType = FormInputFieldType.Choice;
            this.Choices = new FormFieldOptionList();
        }

        protected override void OnDataBound(DataContext context)
        {
            base.OnDataBound(context);

            foreach (var option in this.Contents.OfType<HTMLOption>().ToList())
            {
                var value = string.IsNullOrEmpty(option.Value) ? option.Label : option.Value;
                var label = string.IsNullOrEmpty(option.Label) ? value : option.Label;

                this.Choices.Add(new FormFieldOption { Value = value, Label = label, Selected = option.Selected });
                this.Contents.Remove(option);
            }

            if (string.IsNullOrEmpty(this.Value))
            {
                var selected = this.Choices.FirstOrDefault(c => c.Selected) ?? this.Choices.FirstOrDefault();
                if (null != selected)
                    this.Value = selected.Value;
            }
        }

        protected override PDFAcrobatFormFieldWidget GetFieldEntry(ContextBase context)
        {
            var entry = base.GetFieldEntry(context);
            entry.Choices = this.Choices;
            return entry;
        }
    }
}
