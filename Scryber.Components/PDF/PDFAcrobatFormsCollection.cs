using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    public class PDFAcrobatFormFieldCollection : IArtefactCollection
    {
        public string CollectionName { get; private set; }

        public List<PDFAcrobatFormFieldWidget> Fields { get; private set; }

        public PDFAcrobatFormEntry Current { get; private set; }

        public IComponent Owner { get; private set; }

        public PDFAcrobatFormFieldCollection(string collectionName, IComponent owner)
        {
            this.CollectionName = collectionName;
            this.Fields = new List<PDFAcrobatFormFieldWidget>();
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
                //OutputDefaultResources(context, writer);

                writer.EndDictionary();
                writer.EndObject();

                return fields;
            }
            else
                return null;
        }

        [Obsolete("Needs updating from hard coding",false)]
        private void OutputDefaultResources(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionaryEntry("DR");
            writer.BeginDictionaryS();

            writer.BeginDictionaryEntry("Font");

            writer.BeginDictionary();
            writer.WriteDictionaryObjectRefEntry("frsc1", new PDFObjectRef(7, 0));
            writer.EndDictionary();

            writer.EndDictionaryEntry();

            writer.EndDictionary();

            writer.EndDictionaryEntry();

        }

        private void OutputFields(PDFRenderContext context, PDFWriter writer)
        {
            writer.BeginDictionaryEntry("Fields");

            List<PDFObjectRef> entries = new List<PDFObjectRef>();
            Resources.PDFResourceCollection all = new Resources.PDFResourceCollection(this.Owner);

            foreach (PDFAcrobatFormFieldWidget entry in this.Fields)
            {
                PDFObjectRef oref = entry.OutputToPDF(context, writer);
                if (null != oref)
                    entries.Add(oref);
            }
            writer.WriteArrayRefEntries(true, entries.ToArray());
            List<IPDFResource> rsrs = new List<IPDFResource>();

            writer.EndDictionaryEntry();
        }

        public object Register(IArtefactEntry catalogobject)
        {
            PDFAcrobatFormFieldWidget field = (PDFAcrobatFormFieldWidget)catalogobject;
            this.Fields.Add(field);
            return field;
        }
    }
}
