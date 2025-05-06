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
    public class LayoutEngineTopAndTailedPanel2 : LayoutEnginePanel
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
        public LayoutEngineTopAndTailedPanel2(ContainerComponent container, IPDFLayoutEngine parent)
            : this(container, parent, "header", "footer", "continuation-header", "continuation-footer")
        {
            
        }

        public LayoutEngineTopAndTailedPanel2(ContainerComponent container, IPDFLayoutEngine parent,
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
            if (null != this.TopAndTailed.Header)
            {
                this.LayoutHeader(false);
                
            }
            
            base.DoLayoutChildren();

            if (null != this.TopAndTailed.Footer)
            {
                var block = this.CurrentBlock;
                this.LayoutFooter(false, block, out var footer, removeAfterLayout: false);
            }
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

        // private void UpdateHeaderAndFooterSpace(Unit headerHeight, Unit totalHeightUsedMeasuring, Unit maxHeightNeededForFooters)
        // {
        //     var sectionBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
        //     if (null != sectionBlock && totalHeightUsedMeasuring > Unit.Zero)
        //     {
        //         //The the footer height off the used region
        //         var used = sectionBlock.CurrentRegion.UsedSize;
        //         used.Height -= totalHeightUsedMeasuring;
        //         sectionBlock.CurrentRegion.UsedSize = used;
        //         
        //     }
        //
        //     if (null != sectionBlock && maxHeightNeededForFooters > Unit.Zero)
        //     {
        //         //And reduce the totat available by the footer height so we can add it in at the bottom.
        //         var total = sectionBlock.CurrentRegion.TotalBounds;
        //         total.Height -= maxHeightNeededForFooters;
        //         sectionBlock.CurrentRegion.TotalBounds = total;
        //     }
        //         
        //     
        // }

        
        
        
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
