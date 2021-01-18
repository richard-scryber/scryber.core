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

namespace Scryber.Layout
{
    /// <summary>
    /// Marks the beginning of a bunch of characters on the page
    /// </summary>
    public class PDFTextRunBegin : PDFTextRun
    {
        #region ivars

        private PDFTextRenderOptions _renderopts;
        private List<PDFLayoutLine> _lines = new List<PDFLayoutLine>(); //All the lines in the entire block
        private PDFRect[] _caclulatedBounds; //An array of 3 rects for the bounds - top line, all inner lines, and last

        #endregion

        //
        // properties
        //

        #region public List<PDFLayoutLine> Lines {get;}

        /// <summary>
        /// Gets all the lines that have some or all of their content as part of the text component
        /// </summary>
        public List<PDFLayoutLine> Lines
        {
            get { return this._lines; }
        }

        #endregion

        #region public PDFTextRenderOptions TextRenderOptions



        /// <summary>
        /// Gets the text rendering options (brush, stroke, etc) that this block of text should be rendered with
        /// </summary>
        public PDFTextRenderOptions TextRenderOptions
        {
            get { return _renderopts; }
            set { _renderopts = value; }
        }

        #endregion

        #region public override PDFUnit Height {get;}

        /// <summary>
        ///  Gets the width of this begin text run - PDFUnit.Zero
        /// </summary>
        public override PDFUnit Height
        {
            get { return PDFUnit.Zero; }
        }

        #endregion

        #region public override PDFUnit Width {get;}

        /// <summary>
        /// Gets the height of this begin text run - PDFUnit.Zero
        /// </summary>
        public override PDFUnit Width
        {
            get { return PDFUnit.Zero; }
        }

        #endregion

        
        /// <summary>
        /// Gets or sets the first line inset for this beginning text run.
        /// </summary>
        public PDFUnit LineInset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total bounds of this text run
        /// </summary>
        public PDFRect TotalBounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the start text cursor point within the page for this text
        /// </summary>
        protected PDFSize StartTextCursor
        {
            get;
            private set;
        }

        private PDFUnit _charspace, _wordspace;
        private bool _hascustomspace;

        public PDFUnit CharSpace
        {
            get { return _charspace; }
        }

        public PDFUnit WordSpace
        {
            get { return _wordspace; }
        }

        public bool HasCustomSpace
        {
            get { return _hascustomspace; }
        }

        //
        // ctor
        //


        public PDFTextRunBegin(PDFTextRenderOptions renderopts, PDFLayoutLine line, IPDFComponent owner)
            : base(line, owner)
        {
            this.TextRenderOptions = renderopts;
            this.Lines.Add(line);
        }

        //
        // implementation
        //

        public override void SetOffsetY(PDFUnit y)
        {
            base.SetOffsetY(y);
            PDFRect bounds = this.TotalBounds;
            bounds.Y += y;
            this.TotalBounds = bounds;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            PDFRect final = TotalBounds;
            if (yoffset != PDFUnit.Zero)
            {
                final.Y += yoffset;
                
            }
            if (xoffset > 0)
            {
                final.X += xoffset;
            }
            this.TotalBounds = final;

            base.DoPushComponentLayout(context, pageIndex, xoffset, yoffset);
        }


        #region public void PushCompleteTextBlock(PDFTextRunEnd ending, PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)

        /// <summary>
        /// Called from the PDFTextRunEnd layout item - so the text has a full set of pushed component layouts and we can calculate the bounds
        /// </summary>
        /// <param name="ending"></param>
        /// <param name="context"></param>
        /// <param name="pageIndex"></param>
        /// <param name="xoffset"></param>
        /// <param name="yoffset"></param>
        public void PushCompleteTextBlock(PDFTextRunEnd ending, PDFLayoutContext context, int pageIndex, PDFUnit xoffset, PDFUnit yoffset)
        {
            PDFRect[] all = new PDFRect[3];

            if (this.Lines.Count == 1)
            {
                all[0] = this.CalculateOnlyLineBounds(ending);
            }
            else if (this.Lines.Count == 2)
            {
                all[0] = CalculateFirstLineBounds();
                all[2] = CalculateLastLineBounds(all[0].Height, ending);
            }
            else
            {
                all[0] = CalculateFirstLineBounds();
                all[1] = CalculateInnerTotalBounds(all[0].Height, ending);
                all[2] = CalculateLastLineBounds(all[0].Height + all[1].Height, ending);
            }
            _caclulatedBounds = all;
        }



        private PDFRect CalculateOnlyLineBounds(PDFTextRunEnd end)
        {
            PDFLayoutLine line = this._lines[0];
            PDFRect full = new PDFRect(this.TotalBounds.Location, PDFSize.Empty);

            bool counting = false;
            PDFUnit linewidth = PDFUnit.Zero;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (run == end)
                    break;
                else if (counting)
                    linewidth += run.Width;
            }
            full.Width = linewidth;
            full.Height = line.Height;

            return full;
        }

        private PDFRect CalculateFirstLineBounds()
        {
            PDFLayoutLine line = this._lines[0];
            PDFRect full = new PDFRect(this.TotalBounds.Location, PDFSize.Empty);

            bool counting = false;
            PDFUnit linewidth = PDFUnit.Zero;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (counting)
                    linewidth += run.Width;
            }
            full.Width = linewidth;
            full.Height = line.Height;

            return full;
        }

        private PDFRect CalculateLastLineBounds(PDFUnit voffset, PDFTextRunEnd end)
        {
            PDFLayoutLine line = this._lines[this._lines.Count -1];
            PDFRect full = new PDFRect(this.TotalBounds.Location, PDFSize.Empty);
            full.Y += voffset;
            PDFUnit linewidth = PDFUnit.Zero;
            full.Height = line.Height;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == end)
                    break;
                if (run is PDFTextRunSpacer)
                    full.X = run.Width;
                else
                    linewidth += run.Width;
            }

            full.Width = linewidth;
            return full;
        }

        private PDFRect CalculateInnerTotalBounds(PDFUnit voffset, PDFTextRunEnd ending)
        {

            PDFRect full = this.TotalBounds.Clone();
            full.Size = PDFSize.Empty;
            full.Y += voffset;

            //only want the lines that are between the first and last line
            int firstIndex = 1;
            int lastIndex = this.Lines.Count - 2;

            PDFUnit maxright = PDFUnit.Zero;
            PDFUnit minleft = new PDFUnit(Double.MaxValue);

            for (int i = firstIndex; i <= lastIndex; i++)
            {
                PDFLayoutLine line = this.Lines[i];
#if FULL_WIDTH
                minleft = 0;
                maxright = PDFUnit.Max(maxright, line.FullWidth);
#else

                PDFUnit x = PDFUnit.Zero;
                PDFUnit w = line.Width;
                if (line.Runs[0] is PDFTextRunSpacer)
                {
                    x = line.Runs[0].Width;
                }
                maxright = PDFUnit.Max(maxright, w);
                minleft = PDFUnit.Min(minleft, x);
                full.Height += line.Height;
#endif
            }

            if (minleft > 0)
                full.X = minleft;
            full.Width = maxright - minleft;

            return full;
        }

        #endregion

        public void SetTextSpacing(PDFUnit wordSpace, PDFUnit charSpace)
        {
            this._charspace = charSpace;
            this._wordspace = wordSpace;
            this._hascustomspace = true;
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            PDFRect bounds = this.TotalBounds;
            bounds.X += context.Offset.X;
            bounds.Y += context.Offset.Y;

            //Set the arrangement for this text run
            Component owner = this.Owner as Component;
            if (owner != null && null != this._caclulatedBounds)
            {
                for (int i = 0; i < this._caclulatedBounds.Length; i++)
                {
                    PDFRect b = this._caclulatedBounds[i];
                    if (b.IsEmpty == false)
                    {
                        if (i == 0)
                            b.X += this.LineInset;

                        b.X += context.Offset.X;
                        b.Y += context.Offset.Y;
                        owner.SetArrangement(context, context.FullStyle, b);
                    }
                }
               
            }

            //TODO:Add any backgrounds and borders based on the 3 rects in the calculated bounds.
            PDFFontMetrics metrics = this.TextRenderOptions.Font.FontMetrics;

            bounds.X += this.LineInset;

            PDFSize cursor = new PDFSize(bounds.X,bounds.Y);


            //pdf text rendering is done from the baseline, we need the rendering to appear to start from the top
            //so add the ascent of the font metrics

            //Previous 27 Feb 20
            //cursor.Height += this.TextRenderOptions.Font.FontMetrics.Ascent;

            //With mixed content
            if (this.TextRenderOptions.Leading.HasValue)
                cursor.Height += this.TextRenderOptions.Leading.Value - metrics.Descent;
            else
                cursor.Height += this.Line.BaseLineOffset;

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Text Begin", "Marker to beginning to render the text for component " + this.Owner.ToString() + " at cursor position " + cursor);

            if (this.ShouldRenderBackground(context))
                this.RenderTextBackground(context, writer);

            context.Graphics.SaveGraphicsState();
            context.Graphics.BeginText();

            context.Graphics.SetTextRenderOptions(this.TextRenderOptions, bounds);
            if (_hascustomspace && this.TextRenderOptions.CharacterSpacing.HasValue == false && this.TextRenderOptions.WordSpacing.HasValue == false)
                context.Graphics.SetTextSpacing(this._wordspace, this._charspace, this.TextRenderOptions.Font.Size);

            context.Graphics.MoveTextCursor(cursor, true);
            this.StartTextCursor = cursor;
            
            return null;
        }

        public bool ShouldRenderBackground(PDFRenderContext context)
        {
            return this.TextRenderOptions.Background != null;
        }

        public bool ShouldRenderUnderline(PDFRenderContext context)
        {
            return (this.TextRenderOptions.TextDecoration & Text.TextDecoration.Underline) > 0;
        }

        public bool ShouldRenderStrikeThrough(PDFRenderContext context)
        {
            return (this.TextRenderOptions.TextDecoration & Text.TextDecoration.StrikeThrough) > 0;
        }

        public bool ShouldRenderOverline(PDFRenderContext context)
        {
            return (this.TextRenderOptions.TextDecoration & Text.TextDecoration.Overline) > 0;
        }

        public void RenderTextBackground(PDFRenderContext context, PDFWriter writer)
        {

        }

        public static double ThicknessFactor = 12.0;
        public static double UnderlineOffsetFactor = 6.0;
        public static double StrikeThroughOffset = 0.4;

        public void RenderUnderlines(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            
            PDFUnit offsetV = this.TextRenderOptions.GetAscent() / UnderlineOffsetFactor;
            PDFUnit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        public void RenderStrikeThrough(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            //Strike through is up offset one third the font ascent
            PDFUnit offsetV = new PDFUnit(-this.TextRenderOptions.GetAscent().PointsValue * StrikeThroughOffset,PageUnits.Points);

            PDFUnit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        public void RenderOverLines(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            PDFUnit offsetV = new PDFUnit(-this.TextRenderOptions.GetAscent().PointsValue, PageUnits.Points);
            PDFUnit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        protected virtual void RenderTextDecoration(PDFTextRunEnd endRun, PDFRenderContext context, PDFWriter writer, PDFUnit vOffset, PDFUnit linethickness)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Text Decoration", "Starting to render text decorations");
            
            //set up the graphics context
            PDFPen pen = PDFPen.Create(this.TextRenderOptions.FillBrush, linethickness);
            
            context.Graphics.SaveGraphicsState();
            pen.SetUpGraphics(context.Graphics, this.TotalBounds);

            bool drawing = false;
            PDFPoint linestart = new PDFPoint(this.StartTextCursor.Width, this.StartTextCursor.Height + vOffset);
            

            foreach (PDFLayoutLine line in this.Lines)
            {
                PDFUnit linewidth = 0;

                foreach (PDFLayoutRun run in line.Runs)
                {
                    if (run == this)
                    {
                        drawing = true;
                        //lineoffset += this.LineInset;
                    }
                    else if (run == endRun)
                    {
                        drawing = false;
                        break;
                    }
                    else if (drawing)
                    {
                        if (run is PDFTextRunCharacter)
                        {
                            PDFTextRunCharacter chars = (PDFTextRunCharacter)run;
                            PDFPoint start = new PDFPoint(linestart.X + linewidth, linestart.Y);
                            PDFPoint end = new PDFPoint(linestart.X + linewidth + chars.Width + chars.ExtraSpace, linestart.Y);
                            context.Graphics.DrawLine(start, end);
                            if (context.ShouldLogDebug)
                                context.TraceLog.Add(TraceLevel.Debug, "Text Decoration", "Drawn line from " + start + " to " + end);
                            linewidth += chars.Width;
                        }
                        else if (run is PDFTextRunEnd)
                        {
                        }
                        else if (run is PDFTextRunNewLine)
                        {
                            PDFTextRunNewLine newline = (PDFTextRunNewLine)run;
                            PDFSize offset = newline.Offset;
                            if (null != newline.NextLineSpacer)
                            {
                                PDFUnit nextoffset = newline.NextLineSpacer.Width;
                                offset.Width = nextoffset - offset.Width;
                            }
                            linestart.X += offset.Width;
                            linestart.Y += offset.Height;
                        }
                        else if (run is PDFTextRunSpacer)
                        {
                            //PDFTextRunSpacer spacer = (PDFTextRunSpacer)run;
                            //cursor.X += spacer.Width;
                        }
                        else if (run is PDFTextRunProxy)
                        {
                            PDFTextRunProxy chars = (PDFTextRunProxy)run;
                            PDFPoint start = new PDFPoint(linestart.X + linewidth, linestart.Y);
                            PDFPoint end = new PDFPoint(linestart.X + linewidth + chars.Width, linestart.Y);
                            context.Graphics.DrawLine(start, end);
                            if (context.ShouldLogDebug)
                                context.TraceLog.Add(TraceLevel.Debug, "Text Decoration", "Drawn line from " + start + " to " + end);
                            linewidth += chars.Width;
                        }
                    }
                    
                }

            }

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Debug, "Text Decoration", "Completed the renderering of text decorations");

            //tear down the current graphics state
            context.Graphics.RestoreGraphicsState();
        }

        
    }
}
