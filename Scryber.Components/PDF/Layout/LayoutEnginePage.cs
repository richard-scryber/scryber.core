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
using System.Text;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Graphics;

namespace Scryber.PDF.Layout
{
    public class LayoutEnginePage : LayoutEngineBase
    {

        private StyleStack _origStyleStack;

        //
        // properties
        //

        #region protected PDFStyleStack PageStyleStack {get;set;}

        /// <summary>
        /// Gets or sets the style stack for the page
        /// (so continuation headers and footers can use this when content is overflowed, rather than the full stack).
        /// </summary>
        protected StyleStack PageStyleStack
        {
            get { return _origStyleStack; }
            set { _origStyleStack = value; }
        }

        #endregion

        #region protected PDFPage Page {get;}

        /// <summary>
        /// Gets the page definition associated with this engine
        /// </summary>
        protected PageBase Page
        {
            get { return this.Component as PageBase; }
        }

        #endregion


        //
        // ctor(s)
        //

        #region public PageLayoutEngine(PDFPageBase pg, IPDFLayoutEngine parent)

        /// <summary>
        /// Creates a new instance of the page layout engine that laysout the content of the page.
        /// </summary>
        /// <param name="pg">The page definition that contains all the components to layout</param>
        /// <param name="context">The layout context</param>
        /// <param name="style">The full style associated with the page </param>
        public LayoutEnginePage(PageBase pg, IPDFLayoutEngine parent)
            : base(pg, parent)
        {
        }

        #endregion


        //
        // overrides
        //


        /// <summary>
        /// Main overridden method
        /// </summary>
        protected override void DoLayoutComponent()
        {
            IDisposable record = this.Context.PerformanceMonitor.Record(PerformanceMonitorType.Layout_Pages, "Page " + this.Component.ID);

            //Take a copy of the style stack for the header and footer
            this.PageStyleStack = this.Context.StyleStack.Clone();


            //Get the page size and position options
            PageSize pgsize = this.FullStyle.CreatePageSize();
            pgsize.Size = this.GetNextPageSize(this.Component, this.FullStyle, pgsize.Size);

            PDFPositionOptions options = this.FullStyle.CreatePostionOptions();


            
            


            //Size, border, margins
            PDFRect bounds = new PDFRect(PDFPoint.Empty, pgsize.Size);
            PDFRect contentrect = GetContentRectFromBounds(bounds, options.Margins, options.Padding);


            //Columns
            PDFColumnOptions colOpts = this.FullStyle.CreateColumnOptions();

            //Overflow
            OverflowAction action = options.OverflowAction;
            
            

            PDFLayoutPage pg = BuildNewPage(pgsize.Size, options, colOpts, action);

            //Graphics
            PDFGraphics g = pg.CreateGraphics(null, this.StyleStack, this.Context);

            this.Context.Graphics = g;

            //Register page numbering
            PDFPageNumberOptions numbers = this.GetPageNumbering(this.FullStyle);
            this.RegisterPageNumbering(pg, numbers);

            this.LayoutPageContent();
            


            //close the last page
            PDFLayoutPage last = this.DocumentLayout.CurrentPage;
            if(last.IsClosed == false)
                last.Close();

            //Unregister the page numbers.
            this.UnRegisterPageNumbering(last, numbers);
            
            //release graphics
            this.Context.Graphics = null;

            g.Dispose();
            record.Dispose();

        }

        private PageNumberGroup _numbergroup;
        private int _firstpageIndex;

        protected virtual void RegisterPageNumbering(PDFLayoutPage page, PDFPageNumberOptions options)
        {
            this._numbergroup = this.DocumentLayout.RegisterPageNumbering(page, options);
            _firstpageIndex = page.PageIndex;

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, LayoutEnginePage.LOG_CATEGORY, "Registered the page numbering");
        }

        protected virtual void UnRegisterPageNumbering(PDFLayoutPage last, PDFPageNumberOptions options)
        {
            this.DocumentLayout.UnRegisterPageNumbering(last, _numbergroup);

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Add(TraceLevel.Debug, LayoutEnginePage.LOG_CATEGORY, "Un-registered the page numbering");
        }

        private PDFSize GetNextPageSize(IComponent owner, Style full, PDFSize orig)
        {
            var name = full.GetValue(StyleKeys.PageNameGroupKey, string.Empty);

            if(!string.IsNullOrEmpty(name))
            {
                var style = new Style();
                var prev = this.Page.StyleClass;

                if (name != "initial") //The use of initial will make this the default sizes, otherwise we look for the page selector with the class name selector.
                    this.Page.StyleClass = name;

                style = this.DocumentLayout.DocumentComponent.GetAppliedStyle(this.Page, style);

                var pgSize = style.CreatePageSize();
                orig = pgSize.Size;

                this.Page.StyleClass = prev;
            }

            return orig;
        }

        /// <summary>
        /// Checks the overflow style and if new pages are supported closes the current page layout and
        /// creates a new page layout (becomming the current page) and returns true. 
        /// If overflow is not supported - returns false
        /// </summary>
        /// <param name="region">If there is a change in current page, this is set to the new region</param>
        /// <param name="block">If there is a change in current page, this is set to the new block</param>
        /// <returns></returns>
        public override bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {
            
            
            StyleValue<OverflowAction> action;
            if (this.FullStyle.TryGetValue(StyleKeys.OverflowActionKey, out action) && action.Value(this.FullStyle) == OverflowAction.NewPage)
            {
                PDFLayoutPage lastpage = this.DocumentLayout.CurrentPage;
                PDFLayoutBlock open = lastpage.ContentBlock;
                if (open.IsClosed)
                    open = null;
                else
                    open = open.LastOpenBlock();

                List<PDFLayoutBlock> toclose = new List<PDFLayoutBlock>(depth);

                for (int i = toclose.Count - 1; i >= 0; i--)
                {
                    open = toclose[i];
                    if (open.CurrentRegion != null && open.CurrentRegion.IsClosed == false)
                    {
                        PDFLayoutRegion openRegion = open.CurrentRegion;
                        openRegion.Close();
                    }

                    PDFLayoutBlock parent = open.Parent as PDFLayoutBlock;
                    if (null != parent)
                    {
                        PDFLayoutRegion parentRegion = parent.CurrentRegion;
                        if (null != parentRegion)
                        {
                            open.Close();
                            parentRegion.AddToSize(open);
                        }
                    }

                    //open = parent;
                }
                lastpage.Close();
                var pgSize = this.GetNextPageSize(initiator, initiatorStyle, lastpage.Size);
                PDFLayoutPage page = BuildContinuationPage(lastpage, pgSize);

                block = page.CurrentBlock;
                region = block.CurrentRegion;

                if(this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Built a new continuation page for " + this.Component + " and recreated the " + toclose.Count + " blocks and regions on the new page");
                return true;
            }
            else
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Cannot overflow content for page " + this.Component + " halting the continued layout by returning false");
                
                return false; //Cannot overflow
            }
        }

        #region protected virtual PDFLayoutPage BuildContinuationPage(PDFLayoutPage copyfrom)

        /// <summary>
        /// Based on the sizeing and style of the last page buids a continuation page that content can flow into
        /// </summary>
        /// <param name="copyfrom"></param>
        /// <returns></returns>
        protected virtual PDFLayoutPage BuildContinuationPage(PDFLayoutPage copyfrom, PDFSize size)
        {
            //Take a reference of the current stack and replace with the page stack from first page
            StyleStack orig = this.Context.StyleStack;
            this.Context.StyleStack = this.PageStyleStack;

            

            PDFLayoutPage page = this.BuildNewPage(size, copyfrom.PositionOptions, copyfrom.ContentBlock.ColumnOptions, copyfrom.OverflowAction);

            //becasue we are a continuation page, we have the same number style, so let's just register it with null
            this.DocumentLayout.RegisterPageNumbering(page, null);

            this.LayoutPageHeaderAndFooter();

            //Restore the context stack with the reference
            this.Context.StyleStack = orig;

            return page;
        }

        #endregion
        
        //
        // support methods
        //

        #region protected virtual void BuildNewPage(PDFPageSize pgsize, PDFPositionOptions options ...)

        /// <summary>
        /// Creates a new page with the specified options and adds it to the current layout
        /// </summary>
        /// <param name="pgsize"></param>
        /// <param name="options"></param>
        /// <param name="alley"></param>
        /// <param name="colcount"></param>
        /// <param name="action"></param>
        protected virtual PDFLayoutPage BuildNewPage(PDFSize pgsize, PDFPositionOptions options, PDFColumnOptions colOpts, OverflowAction action)
        {
            PDFLayoutDocument doclayout = this.DocumentLayout;
            PDFLayoutPage pg = doclayout.BeginNewPage(this.Page, this, this.FullStyle, action);

            pg.InitPage(pgsize, options, colOpts, this.Context);
            
            return pg;
        }

        #endregion

        #region protected void LayoutPageContent()

        /// <summary>
        /// Lays out all the content for the page inc Headers and Footers
        /// </summary>
        protected void LayoutPageContent()
        {
            if (this.Context.ShouldLogMessage)
                this.Context.TraceLog.Add(TraceLevel.Message,LOG_CATEGORY, "Laying out the page component '" + this.Page.ID + "' at page index " + this.Context.DocumentLayout.CurrentPageIndex);

            this.LayoutPageHeaderAndFooter();
            //We have to push the arangement for this engine onto the page
            this.DoLayoutChildren();
            
        }

        #endregion

        #region private void LayoutPageHeaderAndFooter()

        /// <summary>
        /// Performs the layout of a single page, with header and footer generated and included as per the definition.
        /// Does not adjust the content size of the page
        /// </summary>
        protected void LayoutPageHeaderAndFooter()
        {
            ITemplate headtemplate = this.GetCurrentHeaderTemplate(this.DocumentLayout.CurrentPageIndex);
            ITemplate foottemplate = this.GetCurrentFooterTemplate(this.DocumentLayout.CurrentPageIndex);

            if (headtemplate != null)
            {
                PDFPageHeader header = new PDFPageHeader();
                this.Page.AddGeneratedHeader(header, this.DocumentLayout.CurrentPageIndex);
                //Create the content inside the header component
                InstantiateTemplateForPage(header, headtemplate);

                Style head = header.GetAppliedStyle();
                if (null != head)
                    this.Context.StyleStack.Push(head);

                Style full = this.Context.StyleStack.GetFullStyle(header);

                this.DocumentLayout.CurrentPage.BeginHeader(header, full, this.Context);

                header.RegisterPreLayout(this.Context);
                this.DoLayoutAChild(header);
                header.RegisterLayoutComplete(this.Context);

                this.DocumentLayout.CurrentPage.EndHeader();

                if (null != head)
                    this.Context.StyleStack.Pop();

            }

            if (foottemplate != null)
            {
                PDFPageFooter footer = new PDFPageFooter();
                this.Page.AddGeneratedFooter(footer, this.DocumentLayout.CurrentPageIndex);

                InstantiateTemplateForPage(footer, foottemplate);

                Style foot = footer.GetAppliedStyle();
                if (null != foot)
                    this.Context.StyleStack.Push(foot);

                Style full = this.Context.StyleStack.GetFullStyle(footer);

                
                this.DocumentLayout.CurrentPage.BeginFooter(footer, full, this.Context);

                footer.RegisterPreLayout(this.Context);
                this.DoLayoutAChild(footer);
                footer.RegisterLayoutComplete(this.Context);

                this.DocumentLayout.CurrentPage.EndFooter();

            }
        }

        #endregion

        #region protected virtual IPDFTemplate GetCurrentHeaderTemplate(int pageIndex)

        /// <summary>
        /// Gets the IPDFTemplate for the Header definition on this page
        /// </summary>
        /// <param name="pageIndex">The current page index in the document layout</param>
        /// <returns></returns>
        protected virtual ITemplate GetCurrentHeaderTemplate(int pageIndex)
        {
            return this.Page.Header;
        }

        #endregion

        #region protected virtual IPDFTemplate GetCurrentFooterTemplate(int pageIndex)

        /// <summary>
        /// Gets the IPDFTemplate for the Footer definition on the page
        /// </summary>
        /// <param name="pageIndex">The current page index in the document layout</param>
        /// <returns></returns>
        protected virtual ITemplate GetCurrentFooterTemplate(int pageIndex)
        {
            return this.Page.Footer;
        }

        #endregion

        #region protected virtual void InstantiateTemplateForPage(PDFLayoutTemplateComponent container, IPDFTemplate template)

        /// <summary>
        /// Creates a concrete instance of the correct template into the PDFLayoutTemplate component
        /// </summary>
        /// <param name="container">A non null instance of the layout template component to contain the generated template components</param>
        protected virtual void InstantiateTemplateForPage(LayoutTemplateComponent container, ITemplate template)
        {
            if (null == template)
                throw new ArgumentNullException("template");
            if (null == container)
                throw new ArgumentNullException("container");

            PDFRect avail =  this.DocumentLayout.CurrentPage.LastOpenBlock().CurrentRegion.TotalBounds;
            int pg = this.DocumentLayout.CurrentPageIndex;

            container.InstantiateTemplate(template, this.Context, avail, pg);
        }

        #endregion

        #region protected PDFPageNumbering GetPageNumbering(PDFStyle style)

        /// <summary>
        /// Gets the current page numbering style for this page
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        protected PDFPageNumberOptions GetPageNumbering(Style style)
        {
            return style.CreatePageNumberOptions();
        }

        
        #endregion

        #region protected PDFPageSize GetPageSize(PDFStyle style)

        /// <summary>
        /// Gets the current page size style for this page
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        protected PageSize GetPageSize(Style style)
        {
            return style.CreatePageSize();
        }

        #endregion



    }
}
