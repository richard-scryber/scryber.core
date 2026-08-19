using System.Collections.Generic;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    /// <summary>
    /// The canonical PDF structure for a group of radio buttons sharing one field name: a parent
    /// node declaring /T (the shared name), /FT /Btn, /Ff (the Radio bit) and /V (the currently
    /// selected on-value) once, with each radio as a /Kids entry that carries only /AS (its own
    /// current state) and /Parent back to this node - rather than every radio in the group flatly
    /// duplicating /T/FT/Ff/V the way an ungrouped field does. Distinguishing "radio vs checkbox"
    /// by this structure (not just the /Ff Radio bit) is apparently what some readers use for
    /// their own visual rendering, so getting the grouping right matters beyond just correctness.
    /// </summary>
    public class PDFAcrobatRadioGroupEntry : PDFAcrobatFormEntry
    {
        public FormFieldOptions FieldOptions { get; set; }

        public PDFAcrobatRadioGroupEntry(string name, FormFieldOptions options) : base(name)
        {
            this.FieldOptions = options;
        }

        public override IEnumerable<PDFObjectRef> OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Fields.Count == 0)
                return null;

            PDFObjectRef parent = writer.BeginObject();

            //Wire each kid's Parent before it renders itself, so it can write /Parent instead of
            //duplicating /T/FT/Ff/V, and work out which one (if any) is the group's selected value.
            string selectedValue = null;
            foreach (var fld in this.Fields)
            {
                if (fld is PDFAcrobatFormCheckWidget check)
                {
                    check.Parent = parent;
                    if (check.IsChecked)
                        selectedValue = string.IsNullOrEmpty(check.OnStateName) ? "on" : check.OnStateName;
                }
            }

            List<PDFObjectRef> children = new List<PDFObjectRef>();
            foreach (var fld in this.Fields)
            {
                var child = fld.OutputToPDF(context, writer);
                if (null != child)
                {
                    foreach (var oref in child)
                        children.Add(oref);
                }
            }

            writer.BeginDictionary();
            writer.WriteDictionaryStringEntry("T", this.Name);
            writer.WriteDictionaryNumberEntry("Ff", (int)this.FieldOptions);
            writer.WriteDictionaryNameEntry("FT", "Btn");
            if (!string.IsNullOrEmpty(selectedValue))
                writer.WriteDictionaryNameEntry("V", selectedValue);

            writer.BeginDictionaryEntry("Kids");
            writer.WriteArrayRefEntries(true, children.ToArray());
            writer.EndDictionaryEntry();
            writer.EndDictionary();
            writer.EndObject();

            return new PDFObjectRef[] { parent };
        }
    }
}
