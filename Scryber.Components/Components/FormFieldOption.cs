using System.Collections.Generic;

namespace Scryber.Components
{
    /// <summary>
    /// A single choice within a Choice (select/combo/list) form field, mapping to one
    /// entry in the PDF field's /Opt array - Value is the export value, Label the display text.
    /// </summary>
    public class FormFieldOption
    {
        public string Value { get; set; }

        public string Label { get; set; }

        public bool Selected { get; set; }
    }

    public class FormFieldOptionList : List<FormFieldOption>
    {
    }
}
