using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Components;

namespace Scryber.PDF
{
    public class PDFAcrobatFormFieldCollection : IArtefactCollection
    {
        public string CollectionName { get; private set; }

        public List<IPDFFormFieldNode> Fields { get; private set; }

        public PDFAcrobatFormEntry Current { get; private set; }

        public IComponent Owner { get; private set; }

        public PDFAcrobatFormFieldCollection(string collectionName, IComponent owner)
        {
            this.CollectionName = collectionName;
            this.Fields = new List<IPDFFormFieldNode>();
            this.Owner = owner;
        }

        public void Close(object registration)
        {
            if (null == Current)
                throw new PDFLayoutException("There is no current form to close");

            if (registration.Equals(Current))
                Current = null;
            else
                throw new PDFLayoutException("The currently open form is not this object registration");


        }

        public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
        {
            throw new NotImplementedException();
        }

        public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Fields.Count > 0)
            {
                PDFObjectRef fields = writer.BeginObject();
                writer.BeginDictionary();

                OutputFields(context, writer);
                OutputDefaultResources(context, writer);
                writer.WriteDictionaryBooleanEntry("NeedAppearances", true);

                writer.EndDictionary();
                writer.EndObject();

                return fields;
            }
            else
                return null;
        }

        /// <summary>
        /// Writes the /DR default resources dictionary for the AcroForm, listing every font resource
        /// actually registered/used by the document, so readers can fall back on it for fields whose
        /// own appearance stream doesn't carry its own font resource (e.g. NeedAppearances regeneration).
        /// </summary>
        private void OutputDefaultResources(PDFRenderContext context, PDFWriter writer)
        {
            Document doc = this.Owner as Document;
            if (null == doc)
                return;

            writer.BeginDictionaryEntry("DR");
            writer.BeginDictionary();

            writer.BeginDictionaryEntry("Font");
            writer.BeginDictionary();

            foreach (PDFResource rsrc in doc.SharedResources)
            {
                if (rsrc.ResourceType == PDFResource.FontDefnResourceType)
                {
                    PDFObjectRef oref = rsrc.EnsureRendered(context, writer);
                    if (null != oref)
                        writer.WriteDictionaryObjectRefEntry(rsrc.Name.Value, oref);
                }
            }

            writer.EndDictionary();
            writer.EndDictionaryEntry();

            writer.EndDictionary();
            writer.EndDictionaryEntry();
        }

        private void OutputFields(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionaryEntry("Fields");

            List<PDFObjectRef> entries = new List<PDFObjectRef>();

            foreach (IPDFFormFieldNode entry in this.Fields)
            {
                IEnumerable<PDFObjectRef> orefs = entry.OutputToPDF(context, writer);
                if (null != orefs)
                {
                    foreach (var oref in orefs)
                    {
                        entries.Add(oref);
                    }
                }
            }
            writer.WriteArrayRefEntries(true, entries.ToArray());

            writer.EndDictionaryEntry();
        }

        public object Register(IArtefactEntry catalogobject)
        {
            IPDFFormFieldNode field = (IPDFFormFieldNode)catalogobject;
            this.Fields.Add(field);
            return field;
        }
    }
}
