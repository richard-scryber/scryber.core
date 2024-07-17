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
using Scryber;
using Scryber.Styles;
using Scryber.Components;
using Scryber.Drawing;

namespace Scryber.PDF.Layout
{
    public class LayoutEnginePageGroup : IPDFLayoutEngine
    {
        private PageGroup _group;
        private PDFLayoutContext _context;
        private Style _full;

        #region public IPDFLayoutEngine ParentEngine {get;set;}

        /// <summary>
        /// Gets  the parent engine that called this one
        /// </summary>
        public IPDFLayoutEngine ParentEngine
        {
            get;
            private set;
        }

        #endregion

        public bool ContinueLayout
        {
            get;
            set;
        }

        public LayoutEnginePageGroup(PageGroup group, IPDFLayoutEngine parent, PDFLayoutContext context, Style full)
        {
            if (null == group)
                throw new ArgumentNullException("group");

            this._group = group;
            this.ParentEngine = parent;
            this._context = context;
            this._full = full;
            this.ContinueLayout = true;
        }


        public PDFLayoutContext Context
        {
            get { return _context; }
        }

        public void Layout(PDFLayoutContext context, Styles.Style fullstyle)
        {
            bool first = true;

            PageNumberOptions opts = fullstyle.CreatePageNumberOptions();
            PageNumberGroup grp = null;

            if (null != opts && opts.HasPageNumbering)
            {
                grp = context.DocumentLayout.Numbers.PushPageNumber(opts);
            }


            foreach (PageBase pg in this._group.Pages)
            {

                if (pg.Visible)
                {
                    this.LayoutAPage(pg, first);
                    first = false;
                }

                if (!this.ContinueLayout)
                    break;
            }

            if (null != opts && opts.HasPageNumbering)
                context.DocumentLayout.Numbers.PopNumberStyle(grp);
        }

        public void LayoutAPage(PageBase pg, bool first)
        {
            this.PushGroupHeader(pg, first);
            this.PushGroupFooter(pg, first);

            Style style = pg.GetAppliedStyle();

            if (null != style)
                this.Context.StyleStack.Push(style);

            Style full = this.BuildFullStyle(pg);

            PDFArtefactRegistrationSet artefacts = pg.RegisterLayoutArtefacts(this.Context, full);

            using (IPDFLayoutEngine engine = pg.GetEngine(this, this.Context, full))
            {
                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Begin(TraceLevel.Message, "Page Group Layout", "Starting the layout of page " + pg.ID);

                engine.Layout(this.Context, full);

                if (this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.End(TraceLevel.Message, "Page Group Layout", "Completed the layout of page " + pg.ID + " and now on page index " + this.Context.DocumentLayout.CurrentPageIndex.ToString());
                else if (this.Context.ShouldLogMessage)
                    this.Context.TraceLog.Add(TraceLevel.Message, "Page Group Layout", "Completed the layout of page " + pg.ID + " and now on page index " + this.Context.DocumentLayout.CurrentPageIndex.ToString());
            }

            if (null != artefacts)
                pg.CloseLayoutArtefacts(this.Context, artefacts, full);

            if (null != style)
                this.Context.StyleStack.Pop();
        }

        protected virtual void PushGroupHeader(PageBase topage, bool first)
        {
            if (null == topage.Header)
            {
                if (first == false && null != this._group.ContinuationHeader)
                    topage.Header = this._group.ContinuationHeader;
                else if (null != this._group.Header)
                    topage.Header = this._group.Header;
            }
            //Set the continuation header of inner sections and page-groups if we have one and they don't
            if (null != this._group.ContinuationHeader)
            {
                if (topage is Section)
                {
                    Section section = (Section)topage;
                    if (null == section.ContinuationHeader)
                        section.ContinuationHeader = this._group.ContinuationHeader;
                }

                else if (topage is PageGroup)
                {
                    PageGroup innergrp = (PageGroup)topage;
                    if (null == innergrp.ContinuationHeader)
                        innergrp.ContinuationHeader = this._group.ContinuationHeader;
                }
            }
        }

        private PageSize _pageOptions;
        private PDFTextRenderOptions _textOptions;

        protected virtual Style BuildFullStyle(Component forComponent)
        {
            if (null == this._pageOptions)
                this._pageOptions = this._full.CreatePageSize();

            var pgSize = this._pageOptions.Size;

            if (null == this._textOptions)
                this._textOptions = this._full.CreateTextOptions();

            var fontSize = new Size(this._textOptions.GetLineHeight(), this._textOptions.GetZeroCharWidth());

            if (forComponent is IDocumentPage docPg)
            {
                return this.Context.StyleStack.GetFullStyleForPage(docPg, pgSize, fontSize, Font.DefaultFontSize);
            }
            else
            {
                return this.Context.StyleStack.GetFullStyle(forComponent, pgSize, new ParentComponentSizer(this.GetPageSize), fontSize, Font.DefaultFontSize);
            }
        }


        private Size GetPageSize(IComponent forComponent, Style withStyle, PositionMode andMode)
        {
            var pg = this._full.CreatePageSize();
            var pos = this._full.CreatePostionOptions(this.Context.PositionDepth > 0);

            return pg.Size.Subtract(pos.Margins);

        }

        protected virtual void PushGroupFooter(PageBase topage, bool first)
        {
            if (null == topage.Footer)
            {
                if (first == false && null != this._group.ContinuationFooter)
                    topage.Footer = this._group.ContinuationFooter;
                else if(null != this._group.Footer)
                    topage.Footer = this._group.Footer;
            }

            //Set the continuation header of inner sections and page-groups if we have one and they don't
            if (null != this._group.ContinuationFooter)
            {
                if (topage is Section)
                {
                    Section section = (Section)topage;
                    if (null == section.ContinuationFooter)
                        section.ContinuationFooter = this._group.ContinuationFooter;
                }

                else if (topage is PageGroup)
                {
                    PageGroup innergrp = (PageGroup)topage;
                    if (null == innergrp.ContinuationFooter)
                        innergrp.ContinuationFooter = this._group.ContinuationFooter;
                }
            }
        }

        public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<Layout.PDFLayoutBlock> depth, ref Layout.PDFLayoutRegion region, ref Layout.PDFLayoutBlock block)
        {
            return this.ParentEngine.MoveToNextPage(initiator, initiatorStyle, depth, ref region, ref block);
        }

        public Layout.PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(Layout.PDFLayoutBlock blockToClose, Layout.PDFLayoutRegion joinToRegion)
        {
            return this.ParentEngine.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
        }


        public void Dispose()
        {

        }
    }
}
