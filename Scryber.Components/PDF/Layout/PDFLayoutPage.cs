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
using System.Linq;
using System.Text;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.Components;
using Scryber.PDF.Graphics;

namespace Scryber.PDF.Layout
{

   
    /// <summary>
    /// Represents the layout of a single page.
    /// </summary>
    public class PDFLayoutPage : PDFLayoutContainerItem, IResourceContainer
    {
        private static PDFLayoutBlock[] _empty = new PDFLayoutBlock[] {};
        private PDFLayoutBlock _header, _footer;
        private PDFLayoutBlock _current;
        private PDFImageXObject _badgexobj;
        private Size _badgeSize;
        private Point _badgePosition;
        private bool _outputbadge = true;
        private int _pageindex = -1;

        //
        // properties
        //

        #region public PDFLayoutDocument Document { get }

        /// <summary>
        /// Gets the document that contains this page
        /// </summary>
        public PDFLayoutDocument Document { get { return this.Parent as PDFLayoutDocument; } }

        #endregion

        #region public PDFSize Size { get; }

        /// <summary>
        /// Gets the actual size of the content on the page
        /// </summary>
        public Size Size { get; private set; }

        #endregion

        #region public PDFLayoutBlock ContentBlock { get; }

        /// <summary>
        /// Gets the main content block on the page.
        /// </summary>
        /// <remarks>This should be set on the init page method</remarks>
        public PDFLayoutBlock ContentBlock { get; private set; }

        #endregion

        #region public PDFLayoutBlock FooterBlock { get; }

        /// <summary>
        /// Gets the header block associated with this page
        /// </summary>
        public PDFLayoutBlock FooterBlock { get { return _footer; } }

        #endregion

        #region public PDFLayoutBlock HeaderBlock { get; }

        /// <summary>
        /// Gets the footer block associated with this page
        /// </summary>
        public PDFLayoutBlock HeaderBlock { get { return _header; } }

        #endregion

        #region public PDFArtefactCollectionSet Artefacts

        private PDFArtefactCollectionSet _artefacts = null;

        /// <summary>
        /// Gets the set of Artefact collections for this Page
        /// </summary>
        public PDFArtefactCollectionSet Artefacts
        {
            get
            {
                if (null == _artefacts)
                    _artefacts = new PDFArtefactCollectionSet();
                return _artefacts;
            }
        }

        #endregion

        #region public PDFLayoutBlock CurrentBlock {get;}

        /// <summary>
        /// Gets the current top level block being laid out
        /// </summary>
        public PDFLayoutBlock CurrentBlock
        {
            get { return _current; }
            private set { _current = value; }
        }

        #endregion

        #region public PDFPage PageOwner {get;}

        /// <summary>
        /// Gets the Page definition that owns this page layout
        /// </summary>
        public PageBase PageOwner
        {
            get { return this.Owner as PageBase; }
        }

        #endregion

        #region public int PageIndex { get;}

        /// <summary>
        /// Gets the page index for this page (Zero based).
        /// </summary>
        public int PageIndex
        {
            get { return _pageindex; }
            set { _pageindex = value; }
        }

        #endregion

        #region public OverflowAction OverflowAction {get;set;}

        /// <summary>
        /// Determines the overflow action to be taken when the available content is exceeded 
        /// </summary>
        public OverflowAction OverflowAction
        {
            get;
            set;
        }

        #endregion

        #region public PDFPositionOptions PositionOptions { get; set; }

        /// <summary>
        /// Gets or sets the PositionOptions for this page layout
        /// </summary>
        public PDFPositionOptions PositionOptions { get; set; }

        #endregion

        #region public PDFResourceList Resources {get;set;}


        /// <summary>
        /// Gets the list of resources this page and its contents use 
        /// </summary>
        /// <remarks>Also implements the IPDFResourceContainer interface.
        /// Currently this refers back to the owner page. So all resources used on all the
        /// layout pages within an section will be included (rather than
        /// just the resources on that specific layout page).
        /// This could be considered bad practice, but hey - it works</remarks>
        [System.ComponentModel.Browsable(false)]
        public PDFResourceList Resources
        {
            get
            {
                return this.PageOwner.Resources;
            }
            
        }


        #endregion

        /// <summary>
        /// Gets to PDFObjectRef for this page.
        /// </summary>
        public PDFObjectRef PageObjectRef { get; private set; }

        //
        // ctor(s)
        //

        #region public PDFLayoutPage(PDFLayoutDocument doc, PDFSize size, PDFPage page, PDFStyle full)

        /// <summary>
        /// Creates a new instance of the PDFLayoutPage.
        /// </summary>
        /// <param name="doc">The document layout this page belongs to</param>
        /// <param name="page">The page definition this layout is part of </param>
        /// <param name="full">The full style for the page</param>
        /// <remarks>The PDFLayout page has one block. The TopBlock. 
        /// This contains all the regions and content for the page.</remarks>
        public PDFLayoutPage(PDFLayoutDocument doc, PageBase page, IPDFLayoutEngine engine, Style full, OverflowAction overflow)
            : base(doc, page, engine, full)
        {
            this.OverflowAction = overflow;
        }

        #endregion

        //
        // methods
        //

        #region public void InitPage(int index, PDFSize size, PDFThickness margthick ...)

        /// <summary>
        /// Initializes this page with the required top block size and other measurements
        /// </summary>
        /// <param name="size"></param>
        /// <param name="margthick"></param>
        /// <param name="padthick"></param>
        /// <param name="available"></param>
        /// <param name="mode"></param>
        /// <param name="cansplit"></param>
        /// <param name="colcount"></param>
        /// <param name="alley"></param>
        public virtual void InitPage(Size size, PDFPositionOptions options, PDFColumnOptions columns, PDFLayoutContext context)
        {
            this.Size = size;
            this.PositionOptions = options;

            OverflowSplit split = options.OverflowSplit;


            PDFLayoutBlock block = new PDFLayoutBlock(this, this.Owner, this.Engine, this.FullStyle, split);
            Rect totalbounds = new Rect(Point.Empty,this.Size);

            
            block.InitRegions(totalbounds, options, columns, context);
            
            //We need to set the bounds of the page block to the total for a page 
            //(as margins are taken off from the size of the page
            // - this is different to a block where margins increase any explicit sizes).
            block.TotalBounds = totalbounds;

            this.ContentBlock = block;
            this.CurrentBlock = block;
        }

        #endregion

        #region public PDFLayoutBlock BeginNewBlock(IPDFComponent owner, PDFStyle fullstyle)

        /// <summary>
        /// Returns a new un-initialized block that is appended to the current open block
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="fullstyle"></param>
        /// <returns></returns>
        public PDFLayoutBlock BeginNewBlock(IComponent owner, IPDFLayoutEngine engine, Style fullstyle, PositionMode mode)
        {
            if (this.IsClosed)
                throw new InvalidOperationException(Errors.CannotLayoutAPageThatHasBeenClosed);

            PDFLayoutBlock last = this.LastOpenBlock();
            if (null == last)
                throw new InvalidOperationException(Errors.NoOpenBlocksToAppendTo);
            else
                return last.BeginNewBlock(owner, engine, fullstyle, mode);
        }

        #endregion

        #region internal PDFLayoutBlock LastOpenBlock()

        /// <summary>
        /// Returns the last open block in the hierarchy of layout blocks and regions. 
        /// If there are no open blocks then returns null.
        /// </summary>
        /// <returns></returns>
        internal PDFLayoutBlock LastOpenBlock()
        {
            PDFLayoutBlock last = this.CurrentBlock;
            if (this.HeaderBlock != null && this.HeaderBlock.IsClosed == false)
            {
                last = this.HeaderBlock;
            }
            else if (this.FooterBlock != null && this.FooterBlock.IsClosed == false)
            {
                last = this.FooterBlock;
            }
            

            if (last.IsClosed)
                last = null;
            else
                last = last.LastOpenBlock();
            return last;
        }

        #endregion

        #region public void BeginHeader()

        /// <summary>
        /// Begins a header on a the current document page
        /// </summary>
        public void BeginHeader(PDFPageHeader owner, Style full, PDFLayoutContext context)
        {
            if (null != this.HeaderBlock)
                throw RecordAndRaise.LayoutException(Errors.AlreadyAHeaderDefinedOnPage);
            
            PDFLayoutBlock block = new PDFLayoutBlock(this, owner, this.Engine, full, OverflowSplit.Never);
            
            //Take the magins away from the total bounds before initializing the regions
            Rect content = this.ContentBlock.TotalBounds;

            if (this.PositionOptions.Margins.IsEmpty == false)
            {
                content.Width -= this.PositionOptions.Margins.Left + this.PositionOptions.Margins.Right;
                content.Height -= this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom;
            }

            block.InitRegions(content, this.ContentBlock.Position, full.CreateColumnOptions(), context);
            this._header = block;
            //Make this block current
            this.CurrentBlock = block;
        }

        #endregion

        #region public void EndHeader()

        /// <summary>
        /// Ends the header on the current document page
        /// </summary>
        public void EndHeader()
        {
            if (null == this.HeaderBlock)
                throw new PDFLayoutException(Errors.NoHeaderBlockToClose);

            
            this.HeaderBlock.Close();
            
            //this.HeaderBlock.ShrinkToFit(this.HeaderBlock.AvailableBounds.Width, this.HeaderBlock.Height);

            //PDFRect head = this.HeaderBlock.TotalBounds;
            //head.Height = this.HeaderBlock.Height;

            //this.HeaderBlock.TotalBounds = head;
            //this.HeaderBlock.AvailableBounds = head;
            this.CurrentBlock = ContentBlock;

            if (null != this.ContentBlock)
            {
                //Offset the content block by the height of the header
                Unit h = this.HeaderBlock.Height;
                this.ContentBlock.Offset(0, h);
                this.ContentBlock.Shrink(0, h);
            }
        }

        #endregion

        #region public void BeginFooter()

        /// <summary>
        /// Begins the footer on the current document page
        /// </summary>
        public void BeginFooter(PDFPageFooter owner, Style full, PDFLayoutContext context)
        {
            PDFLayoutBlock block = new PDFLayoutBlock(this, owner, this.Engine, full, OverflowSplit.Never);

            //Take the magins away from the total bounds before initializing the regions
            Rect content = this.ContentBlock.TotalBounds;
            if (this.PositionOptions.Margins.IsEmpty == false)
            {
                content.Width -= this.PositionOptions.Margins.Left + this.PositionOptions.Margins.Right;
                content.Height -= this.PositionOptions.Margins.Top + this.PositionOptions.Margins.Bottom;
            }
            
            block.InitRegions(content, this.ContentBlock.Position, full.CreateColumnOptions(), context);
            this._footer = block;
            //Make this block current
            this.CurrentBlock = block;
        }

        #endregion

        #region public void EndFooter()

        /// <summary>
        /// Ends the footer on the current document page
        /// </summary>
        public void EndFooter()
        {
            if (null == this.FooterBlock)
                throw new NullReferenceException(Errors.NoFooterBlockToClose);

            this.FooterBlock.Close();

            //Need to move the footer to the bottom of the page
            Unit height = this.FooterBlock.TotalBounds.Height;
            Unit posY = this.FooterBlock.TotalBounds.Y;
            Unit offset = this.Height - posY - height;

            this.FooterBlock.Offset(0, offset);
            this.CurrentBlock = ContentBlock;

            if (null != this.ContentBlock)
            {
                //reduce the available height of the content block by the height of the footer
                
                //Just shrink
                this.ContentBlock.Shrink(0, height);
            }
        }

        #endregion

        
        //
        // abstract overrides
        //

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the width of this page
        /// </summary>
        public override Unit Width
        {
            get { return this.Size.Width; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        /// Gets the height of this page
        /// </summary>
        public override Unit Height
        {
            get { return this.Size.Height; }
        }

        #endregion

        #region public override void PushComponentLayout(PDFLayoutContext context, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Overrides the base abstract method to push the arrangement on this pages' top block
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex,  Unit xoffset, Unit yoffset)
        {
            pageIndex = this.PageIndex;

            this.PageOwner.SetPageLayoutIndex(pageIndex);
            if (null != this.HeaderBlock)
                this.HeaderBlock.PushComponentLayout(context, pageIndex, xoffset, yoffset);

            this.CurrentBlock.PushComponentLayout(context, pageIndex,  xoffset, yoffset);

            if (null != this.FooterBlock)
                this.FooterBlock.PushComponentLayout(context, pageIndex, xoffset, yoffset);

            

            this.RegisterBadgeData(context);
        }

        #endregion

        protected override void SetOwnerPageIndex(int pageIndex)
        {
            pageIndex = this.PageIndex;
            base.SetOwnerPageIndex(pageIndex);
        }

        #region private void RegisterBadgeData(PDFLayoutContext context)


        private const string BadgeResourceStem = "Scryber.Properties.Resources.";
        private const string BlackOnWhiteResourceName = "scryber_generatedby_bow";
        private const string WhiteOnBlackResourceName = "scryber_generatedby_wob";
        private const string EnvironmentBadgeName = "scryber_environment_flat";
        private const int BadgeWidthInches = 1;


        private void RegisterBadgeData(PDFLayoutContext context)
        {
            _outputbadge = this.PageOwner.ShowBadge;

            if (!_outputbadge)
            {
                return;
            }

            ScryberBadgeStyle style;
            if (!this.FullStyle.TryGetItem(StyleKeys.BadgeItemKey, out style))
                style = new ScryberBadgeStyle();


            //get any existing badge resource registered in the document

            Document doc = context.DocumentLayout.DocumentComponent;

            string rsrcName = BadgeResourceStem;
            if (style.DisplayOption == BadgeType.BlackOnWhite)
                rsrcName = rsrcName + BlackOnWhiteResourceName;
            else if (style.DisplayOption == BadgeType.WhiteOnBlack)
                rsrcName = rsrcName + WhiteOnBlackResourceName;
            else if (style.DisplayOption == BadgeType.Environment)
                rsrcName = rsrcName + EnvironmentBadgeName;

            _badgexobj = doc.GetImageResource(rsrcName, this.PageOwner, false);
            

            if(null == _badgexobj)
            {
                //Document does not have a previous badge registed, so need to register it now.
                System.Resources.ResourceManager mgr = new System.Resources.ResourceManager("Scryber.Components.Properties.Resources", typeof(Page).Assembly);
                
                System.Drawing.Bitmap bmp;
                if (style.DisplayOption == BadgeType.BlackOnWhite)
                    bmp = mgr.GetObject(BlackOnWhiteResourceName) as System.Drawing.Bitmap;
                else if (style.DisplayOption == BadgeType.WhiteOnBlack)
                    bmp = mgr.GetObject(WhiteOnBlackResourceName) as System.Drawing.Bitmap;
                else
                    bmp = mgr.GetObject(EnvironmentBadgeName) as System.Drawing.Bitmap;

                ImageData data = ImageData.LoadImageFromBitmap(rsrcName, bmp, false);

                string name = doc.GetIncrementID(ObjectTypes.ImageXObject);

                _badgexobj = PDFImageXObject.Load(data, this.Document.RenderOptions.Compression, name);
                doc.SharedResources.Add(_badgexobj);
            }

            //Make sure the badge is registered with the page too
            _badgexobj.RegisterUse(this.Resources, this.PageOwner);

            
            // calculate the position based on the X and Y Offset plus the corners.
            _badgePosition = new Point(style.XOffset, style.YOffset);
            _badgeSize = new Size(_badgexobj.ImageData.DisplayWidth, _badgexobj.ImageData.DisplayHeight);
            Size pageSize = this.Size;

            switch (style.Corner)
            { 
                default:
                case Corner.TopLeft:
                    //Do Nothing
                    break;
                case Corner.TopRight:
                    _badgePosition.X = (pageSize.Width - _badgePosition.X) - _badgeSize.Width;
                    break;
                case Corner.BottomRight:
                    _badgePosition.X = (pageSize.Width - _badgePosition.X) - _badgeSize.Width;
                    _badgePosition.Y = (pageSize.Height - _badgePosition.Y) - _badgeSize.Height;
                    break;
                case Corner.BottomLeft:
                    _badgePosition.Y = (pageSize.Height - _badgePosition.Y) - _badgeSize.Height;
                    break;
            }

        }

        #endregion

        #region protected override bool DoClose(ref string msg)

        /// <summary>
        /// Overrides to close all the top level blocks in this page.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override bool DoClose(ref string msg)
        {
            if (null != this.HeaderBlock && this.HeaderBlock.IsClosed == false)
                this.HeaderBlock.Close();
            if (null != this.FooterBlock && this.FooterBlock.IsClosed == false)
                this.FooterBlock.Close();

            if(null != this.CurrentBlock && this.CurrentBlock.IsClosed == false)
                this.CurrentBlock.Close();

            return base.DoClose(ref msg);
        }

        #endregion

        #region public override bool MoveToNextRegion()

        /// <summary>
        /// Overrides the move to the next region - not complete
        /// </summary>
        /// <returns></returns>
        public override bool MoveToNextRegion( Unit requiredHeight, PDFLayoutContext context)
        {
            if (this.ContentBlock.Position.OverflowAction == OverflowAction.NewPage)
            {
                PDFLayoutPage cont = this.Document.BeginNewContinuationPage();
                cont.InitPage(this.Size, this.ContentBlock.Position, this.ContentBlock.ColumnOptions, context);
                return cont != null;
            }
            else
                return false;
        }

        #endregion

        #region public object RegisterPageEntry(PDFRegistrationContext context, string artefactType, IArtefactEntry entry)

        public object RegisterPageEntry(PDFLayoutContext context, string artefactType, IArtefactEntry entry)
        {
            IArtefactCollection col;
            if (!Artefacts.TryGetCollection(artefactType, out col))
            {
                col = context.DocumentLayout.CreateArtefactCollection(artefactType);
                _artefacts.Add(col);
            }
            return col.Register(entry);
        }

        #endregion


        #region public void CloseCatalogEntry(string catalogtype, object entry)

        public void CloseArtefactEntry(string artefacttype, object entry)
        {
            IArtefactCollection col;
            if (this.Artefacts.TryGetCollection(artefacttype, out col))
            {
                col.Close(entry);
            }
        }

        #endregion

        //
        // rendering
        //

        protected override PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            if (context.ShouldLogVerbose)
                context.TraceLog.Begin(TraceLevel.Verbose, "Layout Page", "Starting the render of Page: " + this.PageIndex);
            else if(context.ShouldLogMessage)
                context.TraceLog.Add(TraceLevel.Message, "Layout Page", "Rendering layout page " + this.PageIndex);

            //get the current style and aply it to the style stack
            Style style = this.FullStyle;
            context.StyleStack.Push(style);

            PageSize pagesize = this.PageOwner.GetPageSize(style);
            //PDFPageNumbering num = this.GetNumbering(style);
            //this.Document.RegisterPageNumbering(context.PageIndex, this, num);
            PDFObjectRef last = writer.LastObjectReference();
            PDFObjectRef pg;

            pg = DoWritePage(context, writer, last);

            

            context.StyleStack.Pop();

            if (context.ShouldLogVerbose)
                context.TraceLog.End(TraceLevel.Verbose, "Layout Page", "Completed the rendering of page: " + this.PageIndex);

            return pg;
        }


        #region protected virtual PDFPageNumber GetPageNumber()

        /// <summary>
        /// Gets the full page number details for this page within the layout
        /// </summary>
        /// <returns></returns>
        public virtual PageNumberData GetPageNumber()
        {
            return this.Document.GetNumbering(this.PageIndex);
        }

        #endregion

        private const string ExtraKey = "Scryber Core Licensing";

        protected virtual PDFObjectRef DoWritePage(PDFRenderContext context, PDFWriter writer, PDFObjectRef parent)
        {
            PDFObjectRef pg = writer.BeginPage(context.PageIndex);
            this.PageObjectRef = pg;
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Page");
            writer.WriteDictionaryObjectRefEntry("Parent", parent);
            writer.BeginDictionaryEntry("MediaBox");
            writer.WriteArrayRealEntries(0.0, 0.0, this.Size.Width.ToPoints().Value, this.Size.Height.ToPoints().Value);
            writer.EndDictionaryEntry();
            if(this.FullStyle.IsValueDefined(StyleKeys.PageAngle))
            {
                int value = this.FullStyle.GetValue(StyleKeys.PageAngle, 0);
                writer.WriteDictionaryNumberEntry("Rotate", value);
            }

            context.PageSize = this.Size;
            context.Offset = new Point();
            context.Space = context.PageSize;

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Layout Page", "Rendering the contents of page : " + this.PageIndex);

            PDFObjectRef content = this.OutputContent(context, writer);
            if (content != null)
            {
                writer.WriteDictionaryObjectRefEntry("Contents", content);
            }

            if (context.ShouldLogVerbose)
                context.TraceLog.Add(TraceLevel.Verbose, "Layout Page", "Rendering the resources of page : " + this.PageIndex);

            //PDFObjectRef[] annots = this.DoWriteAnnotations(context, writer);
            //if (null != annots && annots.Length > 0)
            //{
            //    writer.BeginDictionaryEntry("Annots");
            //    writer.WriteArrayRefEntries(true, annots);
            //    writer.EndDictionaryEntry();
            //}

            PDFObjectRef ress = this.DoWriteResource(context, writer);
            if (ress != null)
                writer.WriteDictionaryObjectRefEntry("Resources", ress);

            DoWriteArtefacts(context, writer);
            writer.EndDictionary();
            writer.EndPage(context.PageIndex);

            return pg;
        }

        private PDFObjectRef[] DoWriteAnnotations(PDFRenderContext context, PDFWriter writer)
        {
            IArtefactCollection annots;
            if (this.Artefacts.TryGetCollection(PDFArtefactTypes.Annotations, out annots))
            {
                return annots.OutputContentsToPDF(context, writer);
            }
            else
                return null;
        }

        private IStreamFilter[] _defaultfilters = new IStreamFilter[] { PDFStreamFilters.FlateDecode };

        /// <summary>
        /// Gets the content stream filters for the page.
        /// Inheritors can override this to return their own filter
        /// </summary>
        internal protected virtual IStreamFilter[] PageCompressionFilters
        {
            get { return _defaultfilters; }
        }


        protected virtual PDFObjectRef OutputContent(PDFRenderContext context, PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject();
            

            IStreamFilter[] filters = (context.Compression == OutputCompressionType.FlateDecode) ? this.PageCompressionFilters : null;
            writer.BeginStream(oref, filters);


            Point pt = context.Offset.Clone();
            Size sz = context.Space.Clone();

            using (PDFGraphics g = this.CreateGraphics(writer, context.StyleStack, context))
            {
                context.Graphics = g;

                if (null != this.HeaderBlock)
                    this.HeaderBlock.OutputToPDF(context, writer);

                this.ContentBlock.OutputToPDF(context, writer);

                if (null != this.FooterBlock)
                    this.FooterBlock.OutputToPDF(context, writer);

                if (_outputbadge)
                    this.PaintBadgeXObj(context, writer);

            }
            context.Offset = pt;
            context.Space = sz;

            long len = writer.EndStream();
            writer.BeginDictionary();


            if (null != filters && filters.Length > 0)
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
                writer.BeginDictionaryEntry("Filter");
                writer.BeginArray();

                foreach (IStreamFilter filter in filters)
                {
                    writer.BeginArrayEntry();
                    writer.WriteName(filter.FilterName);
                    writer.EndArrayEntry();
                }
                writer.EndArray();
                writer.EndDictionaryEntry();
            }
            else
            {
                writer.BeginDictionaryEntry("Length");
                writer.WriteNumberS(len);
                writer.EndDictionaryEntry();
            }

            writer.EndDictionary();
            writer.EndObject();
            return oref;
        }

        

        public virtual PDFGraphics CreateGraphics(PDFWriter writer, StyleStack styles, ContextBase context)
        {
           return PDFGraphics.Create(writer, false, this, DrawingOrigin.TopLeft, this.Size, context);
        }


        protected PDFObjectRef DoWriteResource(PDFRenderContext context, PDFWriter writer)
        {
            return this.Resources.WriteResourceList(context, writer);
        }


        protected void DoWriteArtefacts(PDFRenderContext context, PDFWriter writer)
        {
            if (this.Artefacts != null && this.Artefacts.Count > 0)
            {
                foreach (IArtefactCollection col in this.Artefacts)
                {

                    if (context.ShouldLogDebug)
                        context.TraceLog.Begin(TraceLevel.Debug,"Layout Page", "Rendering artefact entry " + col.CollectionName);

                    PDFObjectRef artefact = col.OutputToPDF(context, writer);

                    if (null != artefact)
                        writer.WriteDictionaryObjectRefEntry(col.CollectionName, artefact);

                    if (context.ShouldLogDebug)
                        context.TraceLog.Begin(TraceLevel.Debug, "Layout Page", "Finished artefact entry " + col.CollectionName);


                }
            }
        }

        private PDFObjectRef PaintBadgeXObj(PDFRenderContext context, PDFWriter writer)
        {

            if (null == _badgexobj)
                throw new NullReferenceException("_badgexobj");

            context.Graphics.SaveGraphicsState();

            PDFObjectRef imgref = _badgexobj.EnsureRendered(context, writer);
            context.Graphics.PaintImageRef(_badgexobj, _badgeSize, _badgePosition);

            context.Graphics.RestoreGraphicsState();

            return imgref;
        }

        //
        // IPDFResourceContainer implementation
        //

        IDocument IResourceContainer.Document
        {
            get { return this.PageOwner.Document; }
        }

        string IResourceContainer.MapPath(string path)
        {
            return this.DoMapPath(path);
        }

        protected virtual string DoMapPath(string path)
        {
            return this.PageOwner.MapPath(path);
        }

        string IResourceContainer.Register(ISharedResource rsrc)
        {
            return this.DoRegisterResource((PDFResource)rsrc).Value;
        }

        protected virtual PDFName DoRegisterResource(PDFResource rsrc)
        {
            return this.PageOwner.Register(rsrc);
        }

        //
        // object overrides
        //

        public override string ToString()
        {
            if (this.Owner != null)
                return this.Owner.ToString() + ":Page#" + this.PageIndex;
            else
                return "Orphaned:Page#" + this.PageIndex;
        }
    }

    
    /// <summary>
    /// A collection of pages in a document
    /// </summary>
    public class PDFLayoutPageCollection : List<PDFLayoutPage>
    {
    }


}
