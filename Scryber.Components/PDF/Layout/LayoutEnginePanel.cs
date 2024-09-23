﻿/*  Copyright 2012 PerceiveIT Limited
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
    public class LayoutEnginePanel : LayoutEngineBase
    {



        //
        // ctor
        //

        #region protected LayoutEnginePanel(PDFContainerComponent container, IPDFLayoutEngine parent)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public LayoutEnginePanel(ContainerComponent container, IPDFLayoutEngine parent)
            : base(container, parent)
        {
            
        }


        #endregion



        protected override void DoLayoutComponent()
        {
            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Begin(TraceLevel.Verbose, "Panel Layout Engine", "Beginning layout of component '" + this.Component + "' as a viewport component");

            PDFPositionOptions pos = this.FullStyle.CreatePostionOptions(this.Context.PositionDepth > 0);
            PDFColumnOptions columns = this.FullStyle.CreateColumnOptions();
            
            if (pos.DisplayMode != Drawing.DisplayMode.Inline)
            {
                this.DoLayoutBlockComponent(pos, columns);
            }
            else
            {
                PDFLayoutInlineBegin begin = CreateAndAddInlineBegin(pos);

                this.DoLayoutChildren();

                this.CreateAndAddInlineEnd(pos, begin);
            }

            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.End(TraceLevel.Verbose, "Panel Layout Engine", "Completed layout of component '" + this.Component + "' as a viewport component");

        }

        private PDFLayoutInlineBegin CreateAndAddInlineBegin(PDFPositionOptions pos)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            PDFLayoutLine currline = containerRegion.CurrentItem as PDFLayoutLine;
            PDFTextRenderOptions text = this.FullStyle.CreateTextOptions();
            PDFLayoutInlineBegin begin = currline.AddInlineRunStart(this, this.Component, pos, text, this.FullStyle);
            return begin;
        }

        private PDFLayoutInlineEnd CreateAndAddInlineEnd(PDFPositionOptions pos, PDFLayoutInlineBegin begin)
        {
            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            PDFLayoutLine currline = containerRegion.CurrentItem as PDFLayoutLine;
            PDFLayoutInlineEnd end = currline.AddInlineRunEnd(this, this.Component, begin, pos);
            return end;
        }

        


        /// <summary>
        /// Lays out all the content of this panel
        /// </summary>
        /// <param name="position"></param>
        protected virtual void DoLayoutBlockComponent(PDFPositionOptions position, PDFColumnOptions columnOptions)
        {
            PDFLayoutBlock containerBlock = CreateContainerBlock(position);
            if (null == containerBlock)
                return;


            CreateBlockRegions(containerBlock, position, columnOptions);

            this.DoLayoutChildren();

            EnsureContentsFit();
            
        }

        

        

        

        

        


        protected void EnsureContentsFit()
        {
            bool newPage;
            PDFLayoutBlock block = this.CurrentBlock;
            //re-retrieve our container block and region
            //it could have changed whilst laying out other regions / pages.
            PDFLayoutBlock containerBlock = block.Parent as PDFLayoutBlock;

            if (block.IsClosed == false)
                block.Close();

            bool updateSize = false;
            Size updated = block.Size;

            

            if (block.Position.MinimumHeight.HasValue && block.Height < block.Position.MinimumHeight.Value)
            {
                updated.Height = block.Position.MinimumHeight.Value - (block.Position.Padding.Top + block.Position.Padding.Bottom);
                updateSize = true;
            }

            if (block.Position.MinimumWidth.HasValue && block.Width < block.Position.MinimumWidth.Value)
            {
                updated.Width = block.Position.MinimumWidth.Value - (block.Position.Padding.Left + block.Position.Padding.Right);
                updateSize = true;
            }

            

            if (updateSize)
                block.SetContentSize(updated.Width,updated.Height);

            

            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;

            //ADDED to support blocks flowing to the next region or page
            Unit vspace = containerRegion.TotalBounds.Height - containerRegion.UsedSize.Height;
            Unit req = block.Height;

            

            bool canfitvertical = req <= vspace;


            if (!canfitvertical && containerRegion.Contents.Count > 1)
            {
                
                PDFLayoutRegion prev = containerRegion;
                if (IsLastInClippedBlock(prev))
                {
                    this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Cannot fit the block for '" + this.Component.UniqueID + "', but overflow of the content within the region is allowed becasue we are clipping");
                    containerRegion.AddToSize(block);
                }
                else if (this.MoveToNextRegion(req, ref containerRegion, ref containerBlock, out newPage))
                {
                    prev.Contents.Remove(block);
                    containerRegion.AddExistingItem(block);
                }
                else
                {
                    this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "Cannot fit the block for component " + this.Component.UniqueID + " in the avilable height (required = '" + req + "', available = '" + vspace + "'), and we cannot overflow to a new region. Layout of component stopped and returning.");

                    this.ContinueLayout = false;
                }
            }
            else
            {
                //Region has not updated its size as the block was directly closed.
                containerRegion.AddToSize(block);
            }
        }


        
        
    }
}
