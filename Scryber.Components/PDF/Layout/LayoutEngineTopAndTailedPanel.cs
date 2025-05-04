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
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    public class LayoutEngineTopAndTailedPanel : LayoutEnginePanel
    {

        public string HeaderElementName { get; set; }

        public string FooterElementName { get; set; }
        
        public string ContinuationHeaderElementName { get; set; }

        public string ContinuationFooterElementName { get; set; }
        
        /// <summary>
        /// Gets or sets the top and tailed component (i.e. the component with headers and footers)
        /// </summary>
        public ITopAndTailedComponent TopAndTailed { get; set; }


        public PDFLayoutBlock LastFooter { get; set; }
        
        //
        // ctor
        //

        #region protected LayoutEngineTopAndTailedPanel(PDFContainerComponent container, IPDFLayoutEngine parent)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public LayoutEngineTopAndTailedPanel(ContainerComponent container, IPDFLayoutEngine parent)
            : this(container, parent, "header", "footer")
        {
            
        }

        public LayoutEngineTopAndTailedPanel(ContainerComponent container, IPDFLayoutEngine parent,
            string headerElementName, string footerElementName)
            :base(container, parent)
        {
            this.TopAndTailed = (ITopAndTailedComponent)container;
            this.HeaderElementName = headerElementName;
            this.FooterElementName = footerElementName;
        }

        #endregion
        
        //
        // overrides
        //

        protected override void DoLayoutChildren()
        {
            Unit h = Unit.Zero;

            bool isContinuation = false;
            if (null != this.TopAndTailed.Header)
                h += this.LayoutHeader(isContinuation);

            PDFLayoutBlock footBlock = null;
            if (null != this.TopAndTailed.Footer)
                h += this.LayoutFooter(isContinuation, out footBlock);

            this.UpdateHeaderAndFooterSpace(h);
            
            this.LastFooter = footBlock;

            if (this.Component.HasContent)
            {
                var container = this.Component as IContainerComponent;
                if (container != null && container.Content.Count == 1 && container.Content[0] is TextLiteral)
                {
                    Span span = new Span();
                    span.Contents.Add(container.Content[0]);
                    container.Content.RemoveAt(0);
                    container.Content.Add(span);
                }
            }

            base.DoLayoutChildren();

            if (null != this.LastFooter)
                this.AddFooterBackIn(LastFooter);
        }

        protected internal override bool MoveToNextRegion(Unit requiredHeight, ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)
        {
            //We put our last footer back in on the region it was lait out for, and then 
            //We can move again
            if(null != this.LastFooter)
                this.AddFooterBackIn(this.LastFooter);
            
            var result = base.MoveToNextRegion(requiredHeight, ref region, ref block, out newPage);
            if (result)
            {
                Unit h = Unit.Zero;
                
                var isContinuation = true;
                
                if (null != this.TopAndTailed.Header)
                    h += this.LayoutHeader(isContinuation);

                PDFLayoutBlock footBlock = null;
                if (null != this.TopAndTailed.Footer)
                    h += this.LayoutFooter(isContinuation, out footBlock);

                this.UpdateHeaderAndFooterSpace(h);
                
                this.LastFooter = footBlock;
                
            }
            
            return result;
            
        }

        //
        // private implementation
        //

        private Unit LayoutHeader(bool isContinuation)
        {
            ITemplate headtemplate = this.GetCurrentHeaderTemplate(this.DocumentLayout.CurrentPageIndex, isContinuation);
            if (null != headtemplate)
            {
                ComponentHeader header = new ComponentHeader();
                header.Parent = this.Component;
                header.ElementName = this.HeaderElementName;
                
                this.InstantiateTemplateForComponent(header, headtemplate);
                
                Style headerStyle = header.GetAppliedStyle();
                
                if(null != headerStyle)
                    this.Context.StyleStack.Push(headerStyle);
                
                Style full = this.BuildHeaderFullStyle(header);
                var block = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var region = block.CurrentRegion;
                
                this.CurrentBlock = block;
                header.RegisterPreLayout(this.Context);
                
                this.DoLayoutAChild(header);
                header.RegisterLayoutComplete(this.Context);
            }
            return Unit.Zero;
        }

        private Unit LayoutFooter(bool isContinuation, out PDFLayoutBlock footBlock)
        {
            footBlock = null;
            return Unit.Zero;
        }

        private void UpdateHeaderAndFooterSpace(Unit h)
        {
            
        }

        private void AddFooterBackIn(PDFLayoutBlock block)
        {
            
        }
        
        
        protected virtual ITemplate GetCurrentHeaderTemplate(int pageIndex, bool isContinuation)
        {
            //TODO: Check the interface for continuation top and tailed
            
            return this.TopAndTailed.Header;
        }
        
        #region protected virtual void InstantiateTemplateForPage(PDFLayoutTemplateComponent container, IPDFTemplate template)

        /// <summary>
        /// Creates a concrete instance of the correct template into the PDFLayoutTemplate component
        /// </summary>
        /// <param name="container">A non null instance of the layout template component to contain the generated template components</param>
        protected virtual void InstantiateTemplateForComponent(LayoutTemplateComponent container, ITemplate template)
        {
            if (null == template)
                throw new ArgumentNullException("template");
            if (null == container)
                throw new ArgumentNullException("container");

            Rect avail =  this.DocumentLayout.CurrentPage.LastOpenBlock().CurrentRegion.TotalBounds;
            int pg = this.DocumentLayout.CurrentPageIndex;

            container.InstantiateTemplate(template, this.Context, avail, pg);
        }

        #endregion
        
        
        protected virtual Style BuildHeaderFullStyle(ComponentHeader header)
        {
            return this.BuildFullStyle(header);
        }

        protected virtual Style BuildFooterFullStyle(ComponentFooter footer)
        {
            return this.BuildFullStyle(footer);
        }
    }
}
