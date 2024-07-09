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
using Scryber.Components;
using Scryber.Styles;
using Scryber.PDF.Resources;
using Scryber.PDF.Native;
using Scryber.Svg.Components;
using System.Runtime.CompilerServices;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Implements the layout engine for a canvas component. This will set all the canvas' child
    /// components, to relatively positioned unless explictly set to Absolute
    /// </summary>
    public class LayoutEngineCanvas : LayoutEnginePanel
    {

        public PDFLayoutLine Line { get; set; }

        public LayoutEngineCanvas(ContainerComponent component, IPDFLayoutEngine parent)
            : base(component, parent)
        {
        }

        
        protected override void DoLayoutChildren()
        {
            PDFPositionOptions position = this.FullStyle.CreatePostionOptions();
            PDFLayoutXObject canvas = null;
            if (position.ViewPort.HasValue)
            {
                canvas = this.ApplyViewPort(position, position.ViewPort.Value);
            }

            base.DoLayoutChildren();

            if (null != canvas)
            {
                canvas.Close();

                this.CloseCurrentLine();

                canvas.OutPutName = (PDFName)this.Context.Document.GetIncrementID(ObjectTypes.CanvasXObject);
                var rsrc = new PDFLayoutXObjectResource(PDFResource.XObjectResourceType, ((Canvas)Component).UniqueID, canvas);
                var ratio = this.FullStyle.GetValue(SVGAspectRatio.AspectRatioStyleKey, SVGAspectRatio.Default);

                var size = new Size(canvas.Width, canvas.Height);
                canvas.Matrix = CalculateMatrix(size, position.ViewPort.Value, ratio);
                canvas.ClipRect = new Rect(position.X ?? Unit.Zero,
                                              position.Y ?? Unit.Zero,
                                              canvas.Width, canvas.Height);
                this.Context.DocumentLayout.CurrentPage.PageOwner.Register(rsrc);
                this.Context.Document.EnsureResource(rsrc.ResourceType, rsrc.ResourceKey, rsrc);
            }
        }


        private PDFTransformationMatrix CalculateMatrix(Size available, Rect view, SVGAspectRatio ratio)
        {
            
            PDFTransformationMatrix matrix = PDFTransformationMatrix.Identity();
            
            if (ratio.Align == AspectRatioAlign.None)
            {
                SVGAspectRatio.ApplyMaxNonUniformScaling(matrix, available, view);
            }
            else if (ratio.Meet == AspectRatioMeet.Meet)
            {
                SVGAspectRatio.ApplyUniformScaling(matrix, available, view, ratio.Align);
            }
            else if (ratio.Meet == AspectRatioMeet.Slice)
            {
                SVGAspectRatio.ApplyUniformStretching(matrix, available, view, ratio.Align);
            }
            else throw new ArgumentOutOfRangeException(nameof(ratio));

            if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Verbose, "Canvas", "Set the XObject transformation matrix to " + matrix.ToString());

            return matrix;
        }


        protected virtual PDFLayoutXObject ApplyViewPort(PDFPositionOptions oldpos, Rect viewPort)
        {
            //Set the size to the viewport size
            var newpos = oldpos.Clone();
            newpos.X = viewPort.X;
            newpos.Y = viewPort.Y;

            //update to new widths
            newpos.Width = viewPort.Width;
            newpos.Height = viewPort.Height;

            //Set the style values to the viewport too. (and reset the cache)

            this.FullStyle.Size.Width = newpos.Width.Value;
            this.FullStyle.Size.Height = newpos.Height.Value;

            if (this.FullStyle is Scryber.Styles.StyleFull)
                (this.FullStyle as StyleFull).ClearFullRefs();

            PDFLayoutBlock containerBlock = this.DocumentLayout.CurrentPage.LastOpenBlock();
            PDFLayoutRegion containerRegion = containerBlock.CurrentRegion;
            if (containerRegion.HasOpenItem == false)
                containerRegion.BeginNewLine();
            //pos.Y = 200;
            PDFLayoutRegion container = containerBlock.BeginNewPositionedRegion(newpos, this.DocumentLayout.CurrentPage, this.Component, this.FullStyle, isfloating: false, addAssociatedRun: false);

            this.Line = containerRegion.CurrentItem as PDFLayoutLine;

            

            PDFLayoutXObject begin = this.Line.AddXObjectRun(this, this.Component, container, newpos, this.FullStyle);
            //begin.Matrix = PDFTransformationMatrix.Identity();
            //begin.Matrix.SetTranslation(50, 50);

            begin.SetOutputSize(oldpos.Width, oldpos.Height);

            
            //this.CurrentBlock.IsFormXObject = true;
            //this.CurrentBlock.XObjectViewPort = pos.ViewPort.Value;

            return begin;
        }

        #region protected override void DoLayoutAChild(IPDFComponent comp, Styles.PDFStyle full)

        /// <summary>
        /// Overrides the base implementation to set the explict position mode before
        /// continuing on as normal
        /// </summary>
        /// <param name="comp"></param>
        /// <param name="full"></param>
        protected override void DoLayoutAChild(IComponent comp, Styles.Style full)
        {

            //For each child if there is not an explict Absolute setting then
            //we should treat them as relative
            Styles.PositionStyle pos = full.Position;
            PositionMode mode = pos.PositionMode;
            
            if (mode != PositionMode.Absolute || mode != PositionMode.Fixed)
            {
                pos.PositionMode = PositionMode.Absolute;
            }

            base.DoLayoutAChild(comp, full);

        }

        #endregion

        protected virtual void AdjustContainerForTextBaseline(PDFPositionOptions pos, IComponent comp, Style full)
        {
            var text = full.CreateTextOptions();
            
            if (text.DrawTextFromTop == false)
            {
                Unit y;
                var font = full.CreateFont();
                if (pos.Y.HasValue)
                    y = pos.Y.Value;
                else
                    y = 0;

                var doc = this.Component.Document;
                var frsrc = doc.GetFontResource(font, true);
                var metrics = frsrc.Definition.GetFontMetrics(font.Size);

                //TODO: Register the font so that we can get the metrics. Or call later on and move
                // But for now it works (sort of).

                if (null != metrics)
                    y -= metrics.Ascent;
                else
                    y -= font.Size * 0.8;

                pos.Y = y;

            }
        }

        protected override PDFLayoutRegion BeginNewAbsoluteRegionForChild(PDFPositionOptions pos, IComponent comp, Style full)
        {
            this.AdjustContainerForTextBaseline(pos, comp, full);
            return base.BeginNewAbsoluteRegionForChild(pos, comp, full);
        }

        private void CloseCurrentLine()
        {

            if (!this.Line.IsClosed)
                this.Line.Region.CloseCurrentItem();
        }
    }

   
}
