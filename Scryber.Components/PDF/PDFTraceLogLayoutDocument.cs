using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Components;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    internal class PDFTraceLogLayoutDocument : Scryber.PDF.Layout.PDFLayoutDocument
    {
        private PDFDictionary _existCatalog;
        private PDFObjectRef _existPageTree;
        private PDFObjectRef _newPageTree;
        private PDFObjectRef[] _existPageRefs;

        /// <summary>
        /// Gets the existing document catalog
        /// </summary>
        protected PDFDictionary ExistingCatalog
        {
            get { return _existCatalog; }
        }

        //
        // properties
        //

        #region protected PDFFile OriginalFile {get;}

        PDFFile _originalFile;

        /// <summary>
        /// Gets the original file this modifed document is based on
        /// </summary>
        public PDFFile OriginalFile
        {
            get { return _originalFile; }
        }

        #endregion

        

        protected PDFObjectRef[] OriginalPageRefs
        {
            get { return _existPageRefs; }
        }

        public PDFTraceLogLayoutDocument(Document doc, IPDFLayoutEngine engine, PDFFile originalfile)
            : base(doc, engine)
        {
            this._originalFile = originalfile;

            if (null != this._originalFile)
            {

                PDFFileIndirectObject catRef = originalfile.DocumentCatalogRef;
                if (null == catRef)
                    throw new NullReferenceException("Document catalog could not be found in the original source document");

                this._existCatalog = originalfile.DocumentCatalog;
                this._existPageTree = originalfile.PageTree;

                if (null != this._existPageTree)
                    this._existPageRefs = this.GetAllPages(this._existPageTree, originalfile);
                else
                    this._existPageRefs = new PDFObjectRef[] { };
            }
        }

        private PDFObjectRef[] GetAllPages(PDFObjectRef pageTreeRef, PDFFile origFile)
        {
            IPDFFileObject fo = origFile.GetContent(pageTreeRef);
            if (null == fo || !(fo is PDFDictionary))
                return new PDFObjectRef[] { };

            PDFDictionary dict = fo as PDFDictionary;
            if (null == dict)
                return new PDFObjectRef[] { };

            PDFName type = dict["Type"] as PDFName;
            if (null == type || type.Value != "Pages")
                return new PDFObjectRef[] { };
            

            PDFArray kids = dict["Kids"] as PDFArray;
            if (null == kids || kids.Count == 0)
                return new PDFObjectRef[] { };

            List<PDFObjectRef> allPages = new List<PDFObjectRef>();
            foreach (IPDFFileObject item in kids)
            {
                PDFObjectRef pageRef = item as PDFObjectRef;
                if (null != pageRef)
                    allPages.Add(pageRef);
            }
            return allPages.ToArray();
        }


        protected override List<PDFObjectRef> OutputAllPages(PDFObjectRef parent, PDFRenderContext context, PDFWriter writer)
        {
            if (null != this.OriginalFile)
            {
                List<PDFObjectRef> all = new List<PDFObjectRef>(this.OriginalPageRefs);

                List<PDFObjectRef> added = base.OutputAllPages(parent, context, writer);

                all.AddRange(added);

                return all;
            }
            else
                return base.OutputAllPages(parent, context, writer);
        }

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (null != this.OriginalFile)
            {
                writer.OpenDocument(this.OriginalFile, true);
                PDFObjectRef catalog = this.WriteCatalog(context, writer);
                this.WriteInfo(context, writer);

                PDFDocumentID id = this.DocumentComponent.DocumentID;
                if (null == id)
                    id = PDFDocumentID.Create();

                writer.CloseDocument(id);

                return catalog;
            }
            else
                return base.DoOutputToPDF(context, writer);
        }

        protected override PDFObjectRef OutputPageTree(PDFRenderContext context, PDFWriter writer)
        {
            this._newPageTree = base.OutputPageTree(context, writer);
            return this._newPageTree;
        }

        protected override void WriteCatalogEntries(PDFRenderContext context, PDFWriter writer)
        {
            if (null != this.OriginalFile)
            {
                PDFObjectRef catalog = writer.LastObjectReference();
                PDFObjectRef pageTree = this.OutputPageTree(context, writer);

                foreach (KeyValuePair<PDFName, IPDFFileObject> item in this.ExistingCatalog)
                {
                    if (item.Key.Value == "Pages")
                    {
                        //PDFObjectRef pages = this.OutputAllPages
                        writer.BeginDictionaryEntry(item.Key);
                        pageTree.WriteData(writer);
                        writer.EndDictionaryEntry();
                    }
                    else
                    {
                        writer.BeginDictionaryEntry(item.Key);
                        item.Value.WriteData(writer);
                        writer.EndDictionaryEntry();
                    }
                }
            }
            else
                base.WriteCatalogEntries(context, writer);
        }
    }
}
