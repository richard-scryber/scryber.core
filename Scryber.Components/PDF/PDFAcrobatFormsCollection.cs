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

                if (HasSignatureField())
                    writer.WriteDictionaryNumberEntry("SigFlags", 1); //bit 1 = SignaturesExist

                writer.EndDictionary();
                writer.EndObject();

                return fields;
            }
            else
                return null;
        }

        /// <summary>
        /// /NeedAppearances is a single, document-wide flag - it can't be set per field - so this
        /// is the pragmatic compromise: true (readers should regenerate) if any registered field -
        /// flat, or nested inside a Form's /Kids group - still relies on that (i.e. isn't a
        /// pushbutton, the only field type with a complete, independently self-rendered /AP for
        /// every state). A document made up entirely of such fields gets false, letting readers
        /// trust the real /AP instead of ignoring/mis-rendering it the way Acrobat and Chrome were
        /// found to when /NeedAppearances was set unconditionally.
        /// </summary>
        private bool AnyFieldNeedsAppearances()
        {
            return AllWidgets(this.Fields).Any(w => w.NeedsAppearances);
        }

        /// <summary>
        /// True if any registered field - at any nesting depth (flat, inside a Form's /Kids group,
        /// or a radio group nested inside one of those) - is a signature field, in which case
        /// /SigFlags must be set on the AcroForm dictionary.
        /// </summary>
        private bool HasSignatureField()
        {
            return AllWidgets(this.Fields).Any(w => w.FieldType == FormInputFieldType.Signature);
        }

        /// <summary>
        /// Flattens every terminal PDFAcrobatFormFieldWidget out of a mixed widget/group node
        /// list, recursing into nested groups (a Form, or a radio group inside one) to whatever
        /// depth they go - rather than assuming a fixed 2-level root/Form shape, which broke once
        /// radio groups started nesting inside Forms too.
        /// </summary>
        private static IEnumerable<PDFAcrobatFormFieldWidget> AllWidgets(IEnumerable<IPDFFormFieldNode> nodes)
        {
            foreach (var node in nodes)
            {
                if (node is PDFAcrobatFormFieldWidget widget)
                    yield return widget;
                else if (node is PDFAcrobatFormEntry group)
                {
                    foreach (var w in AllWidgets(group.Fields))
                        yield return w;
                }
            }
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
