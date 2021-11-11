/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Represents the layout of a document
    /// </summary>
    public partial class PDFLayoutDocument : PDFLayoutContainerItem, IDocumentLayout
    {
        //
        // properties
        //

        #region public Document DocumentComponent { get; }

        /// <summary>
        /// Gets or sets the document that started the layout
        /// </summary>
        public Document DocumentComponent { get; private set; }


        IDocument IDocumentLayout.Owner { get { return this.DocumentComponent; } }

        #endregion

        #region public int CurrentPageIndex { get; set; }

        /// <summary>
        /// Gets or sets the current page index in this layout
        /// </summary>
        public int CurrentPageIndex { get; set; }

        #endregion

        #region public PDFLayoutPage CurrentPage { get; }

        private PDFLayoutPage _currpg;

        /// <summary>
        /// Gets the current page layout
        /// </summary>
        public PDFLayoutPage CurrentPage
        {
            get { return _currpg; }
            protected set { _currpg = value; }
        }

        #endregion

        #region public PDFLayoutPageCollection AllPages { get;}

        /// <summary>
        /// Gets all the laid out pages in this document
        /// </summary>
        public PDFLayoutPageCollection AllPages { get; private set; }

        #endregion

        #region public int TotalPageCount {get;}

        /// <summary>
        /// Gets the total number of document pages in this document after layout
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return this.AllPages.Count;
            }
        }

        #endregion

        #region public PageNumbers Numbers {get;}

        private PageNumbers _numbers;
        /// <summary>
        /// Gets the numbering collection. Readonly, but value is set with the InitPageNumbers method
        /// </summary>
        public PageNumbers Numbers
        {
            get
            {
                return _numbers;
            }
        }

        #endregion

        #region public PDFDocumentRenderOptions RenderOptions {get;set;}

        private PDFDocumentRenderOptions _renderopts;

        /// <summary>
        /// Gets the options for rendering the final output
        /// </summary>
        public PDFDocumentRenderOptions RenderOptions
        {
            get { return _renderopts; }
        }

        #endregion

        /// <summary>
        /// Gets the output format for this document (which is always PDF)
        /// </summary>
        public OutputFormat Format { get { return OutputFormat.PDF; } }

        //
        // ctor(s)
        //

        #region public PDFLayoutDocument(PDFDocument doc)

        /// <summary>
        /// Creates a new PDFLayoutDocument
        /// </summary>
        public PDFLayoutDocument(Document doc, IPDFLayoutEngine engine)
            : base(null, doc, engine, new Style())
        {
            AllPages = new PDFLayoutPageCollection();
            CurrentPageIndex = -1;
            this.DocumentComponent = doc;
            this._renderopts = doc.RenderOptions;
            this.ValidateRenderOptions();
        }

        #endregion

        //
        // public methods
        //

        #region public PDFLayoutPage BeginNewContinuationPage()

        /// <summary>
        /// Begins a new page based on the current page's size and content rect. This will then be the current page
        /// </summary>
        /// <returns></returns>
        public PDFLayoutPage BeginNewContinuationPage()
        {
            if (CurrentPageIndex < 0)
                throw new ArgumentOutOfRangeException("Cannot begin a new page based on previous page if there are no previous pages");
            PDFLayoutPage pg = this.CurrentPage;

            var log = this.DocumentComponent.TraceLog;
            if (log.ShouldLog(TraceLevel.Verbose))
                log.Add(TraceLevel.Verbose, "LAYOUT", "Beginning a new continuation page for '" + pg + "'");

            if (!pg.IsClosed)
                pg.Close();

            Size size = pg.Size;
            Style style = pg.FullStyle;
            Page owner = pg.Owner as Page;
            OverflowAction overflow = pg.OverflowAction;

            PDFLayoutPage newpg = this.BeginNewPage(owner, this.Engine, style, overflow);
            return newpg;
        }

        #endregion

        #region public PDFLayoutPage BeginNewPage(PDFPage owner, PDFSize size, PDFStyle full)

        /// <summary>
        /// Begins a new page of the requested size and with the specified bounds. This will then be the current page
        /// </summary>
        /// <param name="size">The actual size of the page</param>
        /// <param name="full">The full style of the page</param>
        /// <returns></returns>
        public PDFLayoutPage BeginNewPage(PageBase owner, IPDFLayoutEngine engine, Style full, OverflowAction action)
        {
            int pgIndex = this.AllPages.Count;
            PDFLayoutPage pg = CreateNewPageInstance(owner, engine, full, action, pgIndex);
            this.CurrentPageIndex = pg.PageIndex;
            this.CurrentPage = pg;
            this.AllPages.Add(pg);

            return pg;
        }

        protected virtual PDFLayoutPage CreateNewPageInstance(PageBase owner, IPDFLayoutEngine engine, Style full, OverflowAction action, int pageIndex)
        {
            PDFLayoutPage pg = new PDFLayoutPage(this, owner, engine, full, action);
            pg.PageIndex = pageIndex;
            return pg;
        }

        #endregion

        
        //
        // abstract overrides
        //

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Returns Zero for the height of a document
        /// </summary>
        public override Unit Height
        {
            get { return Unit.Zero; }
        }

        #endregion

        #region public override PDFUnit Width

        /// <summary>
        /// Returns zero for the width of a document
        /// </summary>
        public override Unit Width
        {
            get { return Unit.Zero; }
        }

        #endregion

        #region protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Overrides the base abstract method to push the arrangements for each page
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            if (context.ShouldLogVerbose)
                context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Starting to push the layouts back onto the components");

            foreach (PDFLayoutPage page in this.AllPages)
            {
                page.PushComponentLayout(context, pageIndex, xoffset, yoffset);
            }

            if (context.ShouldLogVerbose)
                context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Completed pushing the layouts onto the components");
        }

        #endregion

        //
        // Artefacts
        //

        #region protected PDFArtefactCollectionSet Artefacts {get;}

        private PDFArtefactCollectionSet _artefacts = new PDFArtefactCollectionSet();

        /// <summary>
        /// Gets the set of artefact collections
        /// </summary>
        protected PDFArtefactCollectionSet Artefacts
        {
            get
            {
                return _artefacts;
            }
        }

        #endregion

        #region public virtual object RegisterCatalogEntry(PDFRegistrationContext context, string catalogtype, ICatalogEntry entry)

        /// <summary>
        /// Registers the IArtefactEntry values with the artefact collection of type 'catalogtype'.
        /// The method returns a reference object that must be passed back to CloseArtefactEntry when the entry should be closed.
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="catalogtype"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual object RegisterCatalogEntry(PDFLayoutContext context, string catalogtype, IArtefactEntry entry)
        {
            IArtefactCollection col;
            if (!this._artefacts.TryGetCollection(catalogtype, out col))
            {
                col = this.CreateArtefactCollection(catalogtype);
                _artefacts.Add(col);
            }
            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Layout Document", "Registering the artefact '" + entry.ToString() + "' in catalog " + catalogtype);

            return col.Register(entry);
        }

        #endregion

        #region protected virtual ICatalogCollection CreateArtefactCollection(string type)

        /// <summary>
        /// Overridable method that creates the specific collections for individual artefact types
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal protected virtual IArtefactCollection CreateArtefactCollection(string type)
        {
            switch (type)
            {
                case (PDFArtefactTypes.Annotations):
                    return new PDFAnnotationCollection(type);
                case (PDFArtefactTypes.Names):
                    return new PDFCategorisedNameDictionary(type);
                case (PDFArtefactTypes.Outlines):
                    return new PDFOutlineStack(type);
                case (PDFArtefactTypes.AcrobatForms):
                    return new PDFAcrobatFormFieldCollection(type, this.Owner);
                default:
                    throw RecordAndRaise.NotSupported("The catalog type {0} is not a known catalog type", type);

            }
        }

        #endregion

        #region public void CloseArtefactEntry(string catalogtype, object entry)

        /// <summary>
        /// Closes the last artefact entry that was started with the 'RegisterCatalogEntry'
        /// </summary>
        /// <param name="catalogtype"></param>
        /// <param name="entry"></param>
        public void CloseArtefactEntry(string catalogtype, object entry)
        {
            IArtefactCollection col;
            if (this._artefacts.TryGetCollection(catalogtype, out col))
            {
                col.Close(entry);
            }
        }

        #endregion

        //
        // Outputting
        //

        #region protected virtual void ValidateRenderOptions()

        /// <summary>
        /// Checks to make sure the provided renderoptions are valid and within range of supported values
        /// </summary>
        protected virtual void ValidateRenderOptions()
        {
            if (null == this.RenderOptions)
                throw new NullReferenceException("No RenderOptions provided on the document");

            //if (this.RenderOptions.PDFVersion != "1.5")
            //    throw new IndexOutOfRangeException("The version '" + this.RenderOptions.PDFVersion + "' is not supported. The only supported versions of PDF currently output are: 1.5");

            if (this.RenderOptions.OuptputCompliance != OutputCompliance.None.ToString())
                throw new IndexOutOfRangeException("The output Compliance '" + this.RenderOptions.OuptputCompliance + "' is not currently supported. Only currently supported value is : " + OutputCompliance.None.ToString());

        }

        #endregion

        #region public bool ShouldRenderAllNames()

        /// <summary>
        /// Returns true if the layout document should render all the names of the components in the Name dictionary. Otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool ShouldRenderAllNames()
        {
            return this.RenderOptions.ComponentNames == ComponentNameOutput.All;
        }

        #endregion

        #region protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Overrides the base implementation to output the document
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            context.TraceLog.Begin(TraceLevel.Message, "Layout Document", "Outputting document to the PDFWriter");
            writer.OpenDocument();
            PDFObjectRef catalog = this.WriteCatalog(context, writer);

            this.WriteInfo(context, writer);

            PDFDocumentID id = this.DocumentComponent.DocumentID;
            if (null == id)
                id = PDFDocumentID.Create();

            writer.CloseDocument(id);

            context.TraceLog.End(TraceLevel.Message, "Layout Document", "Completed output of the document to the PDFWriter");

            return catalog;
        }

        #endregion

        #region private PDFObjectRef WriteCatalog(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs the document catalog (usually the first Component) and then calls output on each of the documents 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual PDFObjectRef WriteCatalog(PDFRenderContext context, PDFWriter writer)
        {
            
            PDFObjectRef catalog = writer.BeginObject("Catalog");
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Catalog");

            WriteCatalogEntries(context, writer);

            writer.EndDictionary();
            writer.EndObject();

            return catalog;
        }

        protected virtual void WriteCatalogEntries(PDFRenderContext context, PDFWriter writer)
        {
            // Pages
            
            context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Starting to write Pages");
            PDFObjectRef pglist = this.OutputPageTree(context, writer);
            writer.WriteDictionaryObjectRefEntry("Pages", pglist);
            context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Finished writing Pages with page tree " + pglist);

            // Page Labels
            context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Starting to write Page Labels");
            PDFObjectRef pglabels = this.WritePageLabels(context, writer);
            if (null != pglabels)
                writer.WriteDictionaryObjectRefEntry("PageLabels", pglabels);
            context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Finished writing Page Labels");

            // Artefacts
            context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Starting to write document Artefacts");
            this.WriteArtefacts(context, writer);
            context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Finished writing document Artefacts");

            //Viewer Preferences
            context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Starting to write Viewer Preferences");
            this.WriteViewerPreferences(context, writer);
            context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Finished writing Viewer Preferences");
        }

        #endregion

        #region protected virtual void WriteViewerPreferences(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs the positioning and visibility of UI elements for the viewer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void WriteViewerPreferences(PDFRenderContext context, PDFWriter writer)
        {
            DocumentViewPreferences docview = this.DocumentComponent.ViewPreferences;

            if (null != docview)
            {
                PDFObjectRef view = docview.OutputToPDF(context, writer);
                if (null != view)
                {
                    writer.WriteDictionaryObjectRefEntry("ViewerPreferences", view);
                }

                string value = docview.GetPageDisplayName(docview.PageDisplay);
                if (!string.IsNullOrEmpty(value))
                {
                    writer.WriteDictionaryNameEntry("PageMode", value);
                }

                value = docview.GetPageLayoutName(docview.PageLayout);
                if (!string.IsNullOrEmpty(value))
                {
                    writer.WriteDictionaryNameEntry("PageLayout", value);
                }
            }
        }

        #endregion

        #region private void WriteArtefacts(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// If this document has artefacts then the collection is output. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        private void WriteArtefacts(PDFRenderContext context, PDFWriter writer)
        {
            PDFArtefactCollectionSet artefacts = this.Artefacts;
            if (artefacts != null && artefacts.Count > 0)
            {
                foreach (IArtefactCollection col in artefacts)
                {
                    context.TraceLog.Begin(TraceLevel.Verbose, "Layout Document", "Outputting artefact catalog entry collection " + col.CollectionName);
                    writer.BeginDictionaryEntry(col.CollectionName);

                    PDFObjectRef entry = col.OutputToPDF(context, writer);
                    if (entry != null)
                        writer.WriteObjectRef(entry);
                    else
                        writer.WriteNull();
                    writer.EndDictionaryEntry();

                    context.TraceLog.End(TraceLevel.Verbose, "Layout Document", "Finished artefact catalog entry collection " + col.CollectionName);
                }
            }
        }

        #endregion

        #region private void WriteInfo(PDFRenderContext context, PDFWriter writer) + support methods

        /// <summary>
        /// writes the collection of document info entries
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected virtual void WriteInfo(PDFRenderContext context, PDFWriter writer)
        {

            DocumentInfo info = this.DocumentComponent.Info;

            if (null != info)
                info.OutputToPDF(context, writer);            
        }


        #endregion

        #region private PDFObjectRef WritePageLabels(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Writes the collection of page labels and returns a reference to this collection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        private PDFObjectRef WritePageLabels(PDFRenderContext context, PDFWriter writer)
        {
            PageNumbers nums = this.Numbers;
            PDFObjectRef labels = writer.BeginObject("PageLabels");
            writer.BeginDictionary();
            writer.BeginDictionaryEntry("Nums");
            writer.BeginArrayS();

            //PDFPageNumberRegistration def = this.Numbers.;
            //this.WriteAPageLabel(context, writer, def);
            
            foreach (PageNumberRegistration entry in this.Numbers.Registrations)
            {
                WriteAPageLabel(context, writer, entry);
            }
            writer.EndArray();
            writer.EndDictionaryEntry();
            writer.EndDictionary();
            writer.EndObject();
            return labels;

        }

        private void WriteAPageLabel(PDFRenderContext context, PDFWriter writer, PageNumberRegistration entry)
        {
            writer.WriteLine();
            writer.BeginArrayEntry();
            writer.WriteNumberS(entry.FirstPageIndex);
            writer.BeginDictionaryS();
            string type;
            switch (entry.Group.NumberStyle)
            {
                case PageNumberStyle.Decimals:
                    type = "D";
                    break;
                case PageNumberStyle.UppercaseRoman:
                    type = "R";
                    break;
                case PageNumberStyle.LowercaseRoman:
                    type = "r";
                    break;
                case PageNumberStyle.UppercaseLetters:
                    type = "A";
                    break;
                case PageNumberStyle.LowercaseLetters:
                    type = "a";
                    break;
                default:
                    type = "";
                    break;
            }
            if (!string.IsNullOrEmpty(type))
                writer.WriteDictionaryNameEntry("S", type);
            if (entry.Group.NumberStart > 0)
                writer.WriteDictionaryNumberEntry("St", entry.Group.NumberStart + entry.PreviousLinkedRegistrationPageCount);
            writer.EndDictionary();
            writer.EndArrayEntry();

            if(context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose,"Page Labels", "Output the page label entry starting at page index " + entry.FirstPageIndex + " with style " + entry.Group.NumberStyle + ", starting at " + entry.Group.NumberStart);

        }

        #endregion

        #region protected PDFObjectRef OutputPageTree(PDFRenderContext context, PDFWriter writer)

        /// <summary>
        /// Outputs the first Page tree Component and calls Output on each of the layout pages.
        /// </summary>
        /// <param name="context">The current context</param>
        /// <param name="writer">The current writer</param>
        /// <returns>A reference to the current page tree root Component</returns>
        protected virtual PDFObjectRef OutputPageTree(PDFRenderContext context, PDFWriter writer)
        {
            //Begin the Pages object and dictionary
            PDFObjectRef pgs = writer.BeginObject(Const.PageTreeName);
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Pages");

            List<PDFObjectRef> pagerefs = OutputAllPages(pgs, context, writer);

            //write the kids array entry in the dictionary
            writer.BeginDictionaryEntry("Kids");
            writer.BeginArray();
            foreach (PDFObjectRef kid in pagerefs)
            {
                writer.BeginArrayEntry();
                writer.WriteFileObject(kid);
                writer.EndArrayEntry();
            }
            writer.EndArray();
            //Write the total number of pages to the dictionary
            writer.EndDictionaryEntry();
            writer.BeginDictionaryEntry("Count");
            writer.WriteNumber(pagerefs.Count);
            writer.EndDictionaryEntry();

            //close the ditionary and the object
            writer.EndDictionary();

            writer.EndObject();

            return pgs;
        }

        protected virtual List<PDFObjectRef> OutputAllPages(PDFObjectRef parent, PDFRenderContext context, PDFWriter writer)
        {
            List<PDFObjectRef> pagerefs = new List<PDFObjectRef>(this.AllPages.Count);
            context.PageIndex = 0;

            //allow each page to output itself, and add the returned PDFObjectRef to the list for the kids dictionary entry below. 
            foreach (PDFLayoutPage ppe in this.AllPages)
            {
                PDFObjectRef oref = OutputAPage(context, writer, ppe);
                if (oref != null)
                {
                    pagerefs.Add(oref);
                    context.PageIndex += 1;
                }
            }
            return pagerefs;
        }

        protected virtual PDFObjectRef OutputAPage(PDFRenderContext context, PDFWriter writer, PDFLayoutPage ppe)
        {
            PDFObjectRef oref = ppe.OutputToPDF(context, writer);
            return oref;
        }

        #endregion


        //
        // page numbers
        //

        #region public void StartPageNumbering(PageNumberOptions opts)

        /// <summary>
        /// Called at the start of document layout to initialize the numbering collection
        /// </summary>
        /// <param name="style"></param>
        public void StartPageNumbering(PageNumberOptions opts)
        {
            this._numbers = new PageNumbers();
            this._numbers.StartNumbering(opts);
        }

        #endregion

        #region public void RegisterPageNumbering(PDFLayoutPage page, PageNumberOptions num) + 1 overload

        /// <summary>
        /// Registers the current page numbering options for the specified page at the specified index
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="page"></param>
        /// <param name="num"></param>
        public PageNumberGroup RegisterPageNumbering(PDFLayoutPage page, PageNumberOptions options)
        {
            PageNumberGroup grp = this.Numbers.PushPageNumber(options);
            this.Numbers.Register(page.PageIndex);
            return grp;
        }

        /// <summary>
        /// Registers the current page numbering options for the specified page at the specified index
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="page"></param>
        /// <param name="num"></param>
        public PageNumberGroup RegisterPageNumbering(int pageIndex, PageNumberOptions options)
        {
            PageNumberGroup grp = this.Numbers.PushPageNumber(options);
            this.Numbers.Register(pageIndex);
            return grp;
        }

        #endregion

        #region public void EndPageNumbering()

        /// <summary>
        /// Closes the numbering of pages in the document
        /// </summary>
        public void EndPageNumbering()
        {
            this.Numbers.EndNumbering();
        }

        #endregion

        #region public void UnRegisterPageNumbering(PDFLayoutPage page, PageNumberGroup numbering) + 1 overload

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="page"></param>
        /// <param name="numbering"></param>
        public void UnRegisterPageNumbering(PDFLayoutPage page, PageNumberGroup group)
        {
            this.Numbers.UnRegister(page.PageIndex);
            this.Numbers.PopNumberStyle(group);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPageIndex"></param>
        /// <param name="page"></param>
        /// <param name="numbering"></param>
        public void UnRegisterPageNumbering(int pageIndex, PageNumberGroup group)
        {
            this.Numbers.UnRegister(pageIndex);
            this.Numbers.PopNumberStyle(group);
        }

        #endregion

        #region public PageNumberData GetNumbering(int pageindex, out int offset, out int max)

        /// <summary>
        /// Gets the page numbering for the requested page with ZERO BASED Index
        /// </summary>
        /// <param name="pageindex">The index of the page to get the numbering for - ZERO BASED</param>
        /// <param name="offset">The translated offset based on the requested page index</param>
        /// <param name="max">The maximum offset within this page numbering</param>
        /// <returns></returns>
        public PageNumberData GetNumbering(int pageindex)
        {
            if (this.Numbers == null)
            {
                return null;
            }
            else
            {
                return this.Numbers.GetPageData(pageindex);
            }
        }

        #endregion

        //
        // cached styles based on identifier strings
        //

        private Dictionary<string, Style[]> _documentcache = new Dictionary<string, Style[]>();

        private const int AppliedIndex = 0;
        private const int FullIndex = 1;

        #region public bool TryGetStyleWithIdentifier(string identifier, out PDFStyle foundApplied, out PDFStyle foundFull)

        
        /// <summary>
        /// Attempts to retrieve a pair of full and applied styles from this layouts cache, returning true if they were found
        /// otherwise false.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="foundApplied"></param>
        /// <param name="foundFull"></param>
        /// <returns></returns>
        public bool TryGetStyleWithIdentifier(string identifier, out Style foundApplied, out Style foundFull)
        {
            Style[] both;
            if (_documentcache.Count > 0 && _documentcache.TryGetValue(identifier, out both))
            {
                foundApplied = both[AppliedIndex];
                foundFull = both[FullIndex];
                return true;
            }
            else
            {
                foundFull = null;
                foundApplied = null;
                return false;
            }
        }

        #endregion

        #region public void SetStyleWithIdentifier(string identifier, PDFStyle applied, PDFStyle full)

        /// <summary>
        /// Sets the styles associated with the provided identifier to the prameter values. The styles will also be marked as immutable
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="applied"></param>
        /// <param name="full"></param>
        public void SetStyleWithIdentifier(string identifier, Style applied, Style full)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentNullException("identifier");
            if (null == applied)
                throw new ArgumentNullException("applied");
            if (null == full)
                throw new ArgumentNullException("full");

            applied.Immutable = true;
            full.Immutable = true;

            Style[] both = new Style[2];
            both[AppliedIndex] = applied;
            both[FullIndex] = full;
            _documentcache[identifier] = both;
        }

        #endregion
    }
}
