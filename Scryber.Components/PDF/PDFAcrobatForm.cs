using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    public class PDFAcrobatFormEntry : IArtefactEntry
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

            if (null != entry)
            {
                this.Fields.Add(entry);
                return true;
            }
            else
                return false;
        }

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
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
                return parent;
            }
            return null;
        }
    }
}
