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

namespace Scryber.Layout
{
    public class LayoutEnginePageGroup : IPDFLayoutEngine
    {
        private PDFPageGroup _group;
        private IPDFLayoutEngine _parent;
        private PDFLayoutContext _context;
        private PDFStyle _full;
        
        public bool ContinueLayout
        {
            get;
            set;
        }

        public LayoutEnginePageGroup(PDFPageGroup group, IPDFLayoutEngine parent, PDFLayoutContext context, PDFStyle full)
        {
            if (null == group)
                throw new ArgumentNullException("group");

            this._group = group;
            this._parent = parent;
            this._context = context;
            this._full = full;
            this.ContinueLayout = true;
        }


        public PDFLayoutContext Context
        {
            get { return _context; }
        }

        public void Layout(PDFLayoutContext context, Styles.PDFStyle fullstyle)
        {
            bool first = true;

            PDFPageNumberOptions opts = fullstyle.CreatePageNumberOptions();
            PDFPageNumberGroup grp = null;

            if (null != opts && opts.HasPageNumbering)
            {
                grp = context.DocumentLayout.Numbers.PushPageNumber(opts);
            }


            foreach (PDFPageBase pg in this._group.Pages)
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

        public void LayoutAPage(PDFPageBase pg, bool first)
        {
            this.PushGroupHeader(pg, first);
            this.PushGroupFooter(pg, first);

            PDFStyle style = pg.GetAppliedStyle();

            if (null != style)
                this.Context.StyleStack.Push(style);

            PDFStyle full = this.Context.StyleStack.GetFullStyle(pg);

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

        protected virtual void PushGroupHeader(PDFPageBase topage, bool first)
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
                if (topage is PDFSection)
                {
                    PDFSection section = (PDFSection)topage;
                    if (null == section.ContinuationHeader)
                        section.ContinuationHeader = this._group.ContinuationHeader;
                }

                else if (topage is PDFPageGroup)
                {
                    PDFPageGroup innergrp = (PDFPageGroup)topage;
                    if (null == innergrp.ContinuationHeader)
                        innergrp.ContinuationHeader = this._group.ContinuationHeader;
                }
            }
        }

        protected virtual void PushGroupFooter(PDFPageBase topage, bool first)
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
                if (topage is PDFSection)
                {
                    PDFSection section = (PDFSection)topage;
                    if (null == section.ContinuationFooter)
                        section.ContinuationFooter = this._group.ContinuationFooter;
                }

                else if (topage is PDFPageGroup)
                {
                    PDFPageGroup innergrp = (PDFPageGroup)topage;
                    if (null == innergrp.ContinuationFooter)
                        innergrp.ContinuationFooter = this._group.ContinuationFooter;
                }
            }
        }

        public bool MoveToNextPage(Stack<Layout.PDFLayoutBlock> depth, ref Layout.PDFLayoutRegion region, ref Layout.PDFLayoutBlock block)
        {
            return this._parent.MoveToNextPage(depth, ref region, ref block);
        }

        public Layout.PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(Layout.PDFLayoutBlock blockToClose, Layout.PDFLayoutRegion joinToRegion)
        {
            return _parent.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);
        }


        public void Dispose()
        {

        }
    }
}
