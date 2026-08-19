using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    

    public class PDFAcrobatFormEntry : IPDFFormFieldNode
    {
        public string Name { get; private set; }
        public PDFAcrobatFormFieldEntryList Fields { get; private set; }

        public PDFAcrobatFormEntry(string name)
        {
            this.Name = name;
            this.Fields = new PDFAcrobatFormFieldEntryList();
        }

        public bool RegisterField(IPDFFormField field, PDFLayoutContext context)
        {
            PDFAcrobatFormFieldWidget entry = field.GetFieldEntry(context) as PDFAcrobatFormFieldWidget;

            if (null == entry)
                return false;

            if ((entry.FieldOptions & FormFieldOptions.Radio) == FormFieldOptions.Radio)
            {
                //Every radio sharing a name within this same Form is one group - the canonical
                //PDF structure the group's own /T/FT/Ff/V declared once, each radio just a /Parent
                //kid - not this Form's flat /Kids duplicating /T per radio the way it works for
                //every other (ungrouped) field type.
                var group = this.Fields.OfType<PDFAcrobatRadioGroupEntry>()
                    .FirstOrDefault(g => string.Equals(g.Name, entry.Name, StringComparison.Ordinal));

                if (null == group)
                {
                    group = new PDFAcrobatRadioGroupEntry(entry.Name, entry.FieldOptions);
                    this.Fields.Add(group);
                }

                group.Fields.Add(entry);
            }
            else
            {
                this.Fields.Add(entry);
            }

            return true;
        }

        public virtual IEnumerable<PDFObjectRef> OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if(this.Fields.Count > 0)
            {
                PDFObjectRef parent = writer.BeginObject();

                List<PDFObjectRef> children = new List<PDFObjectRef>();
                foreach (var fld in this.Fields)
                {
                    var child = fld.OutputToPDF(context, writer);
                    if (null != child)
                    {
                        foreach (var oref in child)
                        {
                            children.Add(oref);
                        }
                    }
                }

                writer.BeginDictionary();
                writer.WriteDictionaryStringEntry("T", this.Name);
                writer.BeginDictionaryEntry("Kids");
                writer.WriteArrayRefEntries(true, children.ToArray());
                writer.EndDictionaryEntry();
                writer.EndDictionary();
                writer.EndObject();
                return new PDFObjectRef[] { parent };
            }
            return null;
        }
    }
}
