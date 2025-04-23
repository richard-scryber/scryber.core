using System;
using Scryber.Components;
using Scryber.PDF.Native;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace Scryber.PDF;

public class PDFEmbeddedAttachmentDictionary : IArtefactCollection
{

    private Dictionary<string, PDFEmbeddedAttachment> _embeddedAttachments;
    public PDFEmbeddedAttachmentDictionary(Document owner)
    {
        
    }
    
    public string CollectionName { get => PDFArtefactTypes.EmbeddedFiles; }
    
    
    public object Register(IArtefactEntry catalogobject)
    {
        if(null == catalogobject)
            return null;
        
        if (catalogobject is PDFEmbeddedAttachment attach)
        {
            if(null == this._embeddedAttachments)
                this._embeddedAttachments = new Dictionary<string, PDFEmbeddedAttachment>();
            var cat = (ICategorisedArtefactNamesEntry)attach;
            
            if (!this._embeddedAttachments.ContainsKey(cat.FullName))
                this._embeddedAttachments.Add(cat.FullName, attach);
            
            return attach;
        }
        else
        {
            throw new ArgumentException(
                "The artefact for the EmbeddedFiles collection must be a PDFEmbeddedAttachment");
        }
        
    }

    public void Close(object registration)
    {
        
    }

    public PDFObjectRef[] OutputContentsToPDF(PDFRenderContext context, PDFWriter writer)
    {
        if (this._embeddedAttachments != null && this._embeddedAttachments.Count > 0)
        {
            List<PDFObjectRef> objectRefs = new List<PDFObjectRef>(this._embeddedAttachments.Count);

            var values =this._embeddedAttachments.Values;
            
            
            
            foreach (var attachment in values)
            {
                var oref = attachment.OutputToPDF(context, writer);
                objectRefs.Add(oref);
            }
            return objectRefs.ToArray();
        }
        else
        {
            return [];
        }
    }

    public PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
    {
        if (this._embeddedAttachments != null && this._embeddedAttachments.Count > 0)
        {
            var keys = new List<string>(this._embeddedAttachments.Keys);
            keys.Sort();

            string first = null; // keys[0];
            string last = null; //keys[keys.Count - 1];

            var oref = writer.BeginObject();
            writer.BeginDictionary();


            writer.BeginDictionaryEntry("Names");
            writer.BeginArray();

            foreach (var key in keys)
            {
                var attach = this._embeddedAttachments[key];
                var childoref = attach.OutputToPDF(context, writer);
                if (null != childoref)
                {
                    writer.BeginArrayEntry();
                    writer.WriteStringLiteralS(key);
                    writer.EndArrayEntry();

                    writer.BeginArrayEntry();
                    writer.WriteObjectRef(childoref);
                    writer.EndArrayEntry();

                    if (string.IsNullOrEmpty(first))
                        first = key;

                    last = key;
                }
            }

            writer.EndArray();
            writer.EndDictionaryEntry();

            if (!string.IsNullOrEmpty(last) && !string.IsNullOrEmpty(first))
            {

                writer.BeginDictionaryEntry("Limits");
                writer.BeginArray();

                writer.BeginArrayEntry();
                writer.WriteStringLiteralS(first);
                writer.EndArrayEntry();

                writer.BeginArrayEntry();
                writer.WriteStringLiteralS(last);
                writer.EndArrayEntry();

                writer.EndArray();
                writer.EndDictionaryEntry(); //Limits
            }


            writer.EndDictionary();
            writer.EndObject();

            return oref;
        }
        else
            return null;
    }
}