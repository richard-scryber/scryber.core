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
        
        /// <summary>
        /// Gets or sets the component if it implments the ITopAndTailedContinuationComponent interface, otherwise null
        /// </summary>
        public ITopAndTailedContinuationComponent ContinuationTopAndTailed { get; set; }
        
        
        public Unit ContinuationFooterHeight { get; set; }
        
        public Unit LastFooterHeight { get; set; }
        
        public Unit HeaderHeight { get; set; }
        
        //
        // ctor
        //

        #region protected LayoutEngineTopAndTailedPanel(PDFContainerComponent container, IPDFLayoutEngine parent) + 1 overload

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public LayoutEngineTopAndTailedPanel(ContainerComponent container, IPDFLayoutEngine parent)
            : this(container, parent, "header", "footer", "continuation-header", "continuation-footer")
        {
            
        }

        public LayoutEngineTopAndTailedPanel(ContainerComponent container, IPDFLayoutEngine parent,
            string headerElementName, string footerElementName, string continuationHeaderElementName, string continuationFooterElementName)
            :base(container, parent)
        {
            this.TopAndTailed = (ITopAndTailedComponent)container;
            
            if(container is ITopAndTailedContinuationComponent conttandt)
                this.ContinuationTopAndTailed = conttandt;
            
            this.HeaderElementName = headerElementName;
            this.FooterElementName = footerElementName;
            this.ContinuationHeaderElementName = continuationHeaderElementName;
            this.ContinuationFooterElementName = continuationFooterElementName; 
            
        }

        #endregion
        
        //
        // overrides
        //

        protected override void DoLayoutChildren()
        {
            
            Unit headerH = Unit.Zero;
            Unit footerH = Unit.Zero;
            Unit continuationHeaderH = Unit.Zero;
            Unit continuationFooterH = Unit.Zero;
            
            bool isContinuation = false;
            bool removeAfterLayout = false;
            
            //
            // First add the header layout to the content
            //
            
            if (null != this.TopAndTailed.Header)
            {
                headerH += this.LayoutHeader(isContinuation);
            }
            else
            {
                headerH = Unit.Zero;
            }

            var block = this.CurrentBlock;
            
            //
            // next Layout any footers, but remove them after layout.
            // We want to know how much space we need, but don't yet know which one it will 
            // be needed.
            
            Unit totalAddedForMeasure = Unit.Zero;
            
            PDFLayoutBlock footBlock = null;
            if (null != this.TopAndTailed.Footer)
            {
                //Footers always appear last, the continuation if set is first
                isContinuation = false;
                removeAfterLayout = true;
                footerH += this.LayoutFooter(isContinuation, block, out footBlock, removeAfterLayout);
                continuationFooterH = footerH;
                this.LastFooterHeight = footerH;
                this.ContinuationFooterHeight = footerH;
                totalAddedForMeasure = footerH;
            }

            if (null != this.ContinuationTopAndTailed)
            {
                if (this.ContinuationTopAndTailed.ContinuationFooter != null)
                {
                    isContinuation = true;
                    continuationFooterH = this.LayoutFooter(isContinuation, block, out footBlock, true);
                    this.ContinuationFooterHeight = continuationFooterH;
                    totalAddedForMeasure += continuationFooterH;
                }
            }

            //
            // based on the size make sure will have enough at the bottom of the page to roll over nicely.
            // or simply add the footer
            //
            
            Unit maxFooterHeight = Unit.Max(this.ContinuationFooterHeight, this.LastFooterHeight);

            this.UpdateHeaderAndFooterSpace(headerH, totalAddedForMeasure, maxFooterHeight);
            
            

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
            
            // We have done what we need
            // standard processing of the content of the container component.
            // Any calls to the move to next region, will capture the roll over and add the footers and headers.
            //

            base.DoLayoutChildren();

            //
            //Now add the Last Footer (the main footer) at the end of the layout
            //
            isContinuation = false;
            if (null != this.TopAndTailed.Footer)
            {
                block = this.CurrentBlock;
                headerH += this.LayoutFooter(isContinuation, block, out footBlock, false);
            }

        }

        public override PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {
            if(this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Closing the current region and stating in a new one, so need to check the headers and footers for " + this.Component);

            var newRegion = joinToRegion;
            var currBlock = blockToClose;
            this.AddAnyTailToTheEndOfTheBlock(currBlock, isContinuation: true);
            
            var newBlock = base.CloseCurrentBlockAndStartNewInRegion(blockToClose, joinToRegion);

            if (newBlock != blockToClose)
            {
                //TODO: Add THe header back in
            }
            
            return newBlock;
        }

        public override bool MoveToNextPage(IComponent initiator, Style inititorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region,
            ref PDFLayoutBlock block)
        {
            if(this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Moving to a new page so need to check the headers and footers for " + this.Component);

            
            var result = base.MoveToNextPage(initiator, inititorStyle, depth, ref region, ref block);


           return result;
        }

        protected internal override bool MoveToNextRegion(Unit requiredHeight, ref PDFLayoutRegion region, ref PDFLayoutBlock block, out bool newPage)
        {
            if(this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Moving to a new region so need to check the headers and footers for " + this.Component);

            
            var result = base.MoveToNextRegion(requiredHeight, ref region, ref block, out newPage);
            return result;
            
        }

        private bool AddAnyTailToTheEndOfTheBlock(PDFLayoutBlock currBlock, bool isContinuation)
        {
            
            ITemplate footerTemplate = this.GetCurrentFooterTemplate(0, isContinuation);
            var total = currBlock.CurrentRegion.TotalBounds;
            total.Height += this.ContinuationFooterHeight;
            currBlock.CurrentRegion.TotalBounds = total;
            var doclose = false;
            
            //Re-open
            if (currBlock.IsClosed)
            {
                currBlock.ReOpen();
                doclose = true;
            }
            
            var size = this.LayoutFooter(isContinuation, currBlock, out var footerBlock, removeAfterLayout: false);

            if (doclose)
                currBlock.Close();
            return true; // size != Unit.Zero;
        }

        //
        // private implementation
        //

        private Unit LayoutHeader(bool isContinuation)
        {
            Unit headerH = Unit.Zero;
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
                var sectblock = this.DocumentLayout.CurrentPage.LastOpenBlock();
                
                this.CurrentBlock = sectblock;
                header.RegisterPreLayout(this.Context);
                
                this.DoLayoutAChild(header);
                header.RegisterLayoutComplete(this.Context);
                
                if(null != headerStyle)
                    this.Context.StyleStack.Pop();

                var sectReg = sectblock.CurrentRegion;
                var headBlock = (PDFLayoutBlock)sectReg.Contents[0] ;  //Should always be the first block

                headerH = headBlock.Height;
                
            }
            return headerH;
        }

        private Unit LayoutFooter(bool isContinuation, PDFLayoutBlock intoBlock, out PDFLayoutBlock footBlock, bool removeAfterLayout = false)
        {
            Unit footH = Unit.Zero;
            ITemplate foottemplate = this.GetCurrentFooterTemplate(this.DocumentLayout.CurrentPageIndex, isContinuation);
            if (null != foottemplate)
            {
                if(this.Context.ShouldLogVerbose)
                    this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Laying out the footer block for top and tailed component " + this.Component);
                
                ComponentFooter footer = new ComponentFooter();
                footer.Parent = this.Component;
                footer.ElementName = this.FooterElementName;
                
                this.InstantiateTemplateForComponent(footer, foottemplate);
                
                Style footStyle = footer.GetAppliedStyle();
                
                if(null != footStyle)
                    this.Context.StyleStack.Push(footStyle);
                
                Style full = this.BuildFooterFullStyle(footer);

                
                var region = intoBlock.CurrentRegion;
                
                this.CurrentBlock = intoBlock;
                footer.RegisterPreLayout(this.Context);
                
                this.DoLayoutAChild(footer);
                
                footer.RegisterLayoutComplete(this.Context);
                
                
                
                var sectBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
                var sectContentRegion = sectBlock.CurrentRegion;
                var lastBlock = sectContentRegion.Contents[sectBlock.CurrentRegion.Contents.Count - 1] as PDFLayoutBlock;
                
                footBlock = lastBlock;
                footH = footBlock.Height;
                
                if (removeAfterLayout)
                {
                    if(this.Context.ShouldLogVerbose)
                        this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Removing the layout artefacts for the template footer on component " + this.Component);
                    
                    //We take it out and add it back in at the end, or when we move to a new region
                    sectBlock.CurrentRegion.Contents.Remove(footBlock);
                }
                else
                {
                    if(this.Context.ShouldLogVerbose)
                        this.Context.TraceLog.Add(TraceLevel.Message, "Top And Tailed Engine", "Artefacts for the template footer on component " + this.Component + " Are remaining on region " + sectContentRegion);

                }
                
                if(null != footStyle)
                    this.Context.StyleStack.Pop();
            }
            else
            {
                footBlock = null;
            }
            return footH;
        }

        private void UpdateHeaderAndFooterSpace(Unit headerHeight, Unit totalHeightUsedMeasuring, Unit maxHeightNeededForFooters)
        {
            var sectionBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            if (null != sectionBlock && totalHeightUsedMeasuring > Unit.Zero)
            {
                //The the footer height off the used region
                var used = sectionBlock.CurrentRegion.UsedSize;
                used.Height -= totalHeightUsedMeasuring;
                sectionBlock.CurrentRegion.UsedSize = used;
                
            }

            if (null != sectionBlock && maxHeightNeededForFooters > Unit.Zero)
            {
                //And reduce the totat available by the footer height so we can add it in at the bottom.
                var total = sectionBlock.CurrentRegion.TotalBounds;
                total.Height -= maxHeightNeededForFooters;
                sectionBlock.CurrentRegion.TotalBounds = total;
            }
                
            
        }

        
        
        
        protected virtual ITemplate GetCurrentHeaderTemplate(int pageIndex, bool isContinuation)
        {
            if (isContinuation && null != this.ContinuationTopAndTailed &&
                null != this.ContinuationTopAndTailed.ContinuationHeader)
                return this.ContinuationTopAndTailed.ContinuationHeader;
            else
                return this.TopAndTailed.Header;
        }

        protected virtual ITemplate GetCurrentFooterTemplate(int pageIndex, bool isContinuation)
        {
            if (isContinuation && null != this.ContinuationTopAndTailed &&
                null != this.ContinuationTopAndTailed.ContinuationFooter)
                return this.ContinuationTopAndTailed.ContinuationHeader;
            else
                return this.TopAndTailed.Footer;
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
