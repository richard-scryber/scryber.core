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
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineDocument : LayoutEngineBase
    {

        //
        // properties
        //

        #region public PDFDocument Document {get;}

        /// <summary>
        /// Gets the document associated with this layout engine
        /// </summary>
        public Document Document
        {
            get { return base.Component as Document; }
        }

        #endregion

        //
        // ctor(s)
        //

        #region internal DocumentLayoutEngine(PDFDocument doc, IPDFLayoutEngine parent, PDFLayoutContext context)

        /// <summary>
        /// Creates a new instance of the document layout engine which can arrange all the pages and the components for this document into a PDFDocumentLayout
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="parent"></param>
        /// <param name="context"></param>
        protected internal LayoutEngineDocument(Document doc, IPDFLayoutEngine parent, PDFLayoutContext context)
            : base(doc, parent)
        {
        }

        #endregion

        //
        // implementation methods
        //

        
        /// <summary>
        /// Main calling method that lays out each of the pages in this document
        /// </summary>
        /// <param name="avail">Available size (ignored)</param>
        /// <param name="startPageIndex">The starting page index</param>
        /// <returns>an empty size</returns>
        protected override void DoLayoutComponent()
        {

            this.Context.DocumentLayout = CreateDocumentLayout();
            
            //If we are pre-pending a file then pass it down the line.
            if (null != this.Document.PrependedFile)
                this.Context.DocumentLayout.PrependFile = this.Document.PrependedFile;
            
            Style style = this.FullStyle;

            this.StartPageNumbering(style);

            //Do the registration of top level document artefacts
            PDFArtefactRegistrationSet artefacts = this.Document.RegisterLayoutArtefacts(this.Context, style);

            LayoutAdditions(style);

            //this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Layout_Pages);

            LayoutAllPages();

            //this.Context.PerformanceMonitor.End(PerformanceMonitorType.Layout_Pages);

            if (null != artefacts)
                this.Document.CloseLayoutArtefacts(Context, artefacts, style);

            EndPageNumbering(style);

            this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Push_Component_Layout);

            this.Context.DocumentLayout.PushComponentLayout(this.Context, 0, Unit.Zero, Unit.Zero);

            this.Context.PerformanceMonitor.End(PerformanceMonitorType.Push_Component_Layout);
            
        }


        protected virtual void LayoutAdditions(Style docStyle)
        {
            if(this.Document.HasAdditions)
            {
                foreach (IComponent comp in this.Document.Additions)
                {
                    if (comp is Component)
                    {
                        Component full = comp as Component;
                        PDFArtefactRegistrationSet artefacts = full.RegisterLayoutArtefacts(this.Context, docStyle);
                        if(full is IPDFViewPortComponent)
                        {
                            using (IPDFLayoutEngine engine = ((IPDFViewPortComponent)full).GetEngine(this, this.Context, docStyle))
                                engine.Layout(this.Context, docStyle);
                        }

                        if (null != artefacts)
                            full.CloseLayoutArtefacts(this.Context, artefacts, docStyle);
                    }
                }
            }
        }

        

        protected virtual void LayoutAllPages()
        {

            foreach (PageBase page in this.Document.Pages)
            {
                if (page.Visible)
                    this.LayoutPage(page);
            }
        }


        protected virtual void StartPageNumbering(Style full)
        {
            this.DocumentLayout.StartPageNumbering(full.CreatePageNumberOptions());
        }

        protected virtual void EndPageNumbering(Style full)
        {
            this.DocumentLayout.EndPageNumbering();
        }

        #region protected virtual PDFLayoutDocument CreateDocumentLayout()

        /// <summary>
        /// Creates a new PDFLayoutDocument and returns. Inheritors can override this implementation to return their own instance
        /// </summary>
        protected virtual PDFLayoutDocument CreateDocumentLayout()
        {
            var definedStyle = this.Document.GetPageStyle(string.Empty);
            var size = definedStyle.CreatePageSize();

            var layout = new PDFLayoutDocument(this.Document, this);

            layout.PushPageSize(string.Empty, size);

            // if(null != definedStyle)
            // {
            //     layout.PushPageSize(string.Empty, definedStyle.CreatePageSize());
            // }

            return layout;
        }

        #endregion

        #region protected virtual PDFSize LayoutPage(PDFPageBase pg)

        /// <summary>
        /// Lays out the individual page specified within the document - by calling the pages IPDFViewPortComponent interface
        /// </summary>
        /// <param name="pg">The page to perform the layout on</param>
        protected virtual void LayoutPage(PageBase pg)
        {
            Style style = pg.GetAppliedStyle();
            
            if(null != style)
                this.StyleStack.Push(style);


            Style full = this.BuildFullStyle(pg);

            LayoutPageWithStyle(pg, full);


            if (null != style)
                this.Context.StyleStack.Pop();
        }

        protected void LayoutPageWithStyle(PageBase pg, Style full)
        {
            PDFArtefactRegistrationSet artefacts = pg.RegisterLayoutArtefacts(this.Context, full);

            using (IPDFLayoutEngine engine = pg.GetEngine(this, this.Context, full))
            {
                if (this.Context.TraceLog.ShouldLog(TraceLevel.Message))
                    this.Context.TraceLog.Begin(TraceLevel.Message, "Document Layout", "Starting the layout of page " + pg.ID);

                engine.Layout(this.Context, full);

                if (this.Context.TraceLog.ShouldLog(TraceLevel.Message))
                    this.Context.TraceLog.End(TraceLevel.Message, "Document Layout", "Completed the layout of page " + pg.ID + " and now on page index " + this.DocumentLayout.CurrentPageIndex.ToString());
            }

            if (null != artefacts)
                pg.CloseLayoutArtefacts(this.Context, artefacts, full);
        }

        #endregion

        protected override Style BuildFullStyle(Component forComponent)
        {
           
            var pg = this.FullStyle.CreatePageSize();
            var txt = this.FullStyle.CreateTextOptions();
            var fs = new Size(txt.GetZeroCharWidth(), txt.GetSize());

            
            if (forComponent is IDocumentPage documentPage)
                return this.StyleStack.GetFullStyleForPage(documentPage, pg.Size, fs, Font.DefaultFontSize);                
            else
                return this.StyleStack.GetFullStyle(forComponent, pg.Size, new ParentComponentSizer(this.GetPageSize), fs, Font.DefaultFontSize);

            
        }

        private Size GetPageSize(IComponent forComponent, Style withStyle, PositionMode andMode)
        {
            var pg = this.FullStyle.CreatePageSize();
            var pos = this.FullStyle.CreatePostionOptions(false);

            return pg.Size.Subtract(pos.Margins);

        }
    }
}
