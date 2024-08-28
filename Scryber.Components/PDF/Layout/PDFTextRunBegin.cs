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
using Scryber.Expressive.Functions.Date;
using Scryber.PDF.Graphics;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Marks the beginning of a bunch of characters on the page
    /// </summary>
    public class PDFTextRunBegin : PDFTextRun
    {
        #region ivars

        private PDFTextRenderOptions _renderopts;
        private List<PDFLayoutLine> _lines = new List<PDFLayoutLine>(); //All the lines in the entire block
        private Rect[] _caclulatedBounds; //An array of 3 rects for the bounds - top line, all inner lines, and last

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

        /// <summary>
        /// Gets the (up to) 3 rects that fully describe the calculated bounds for this text run begin to end - top line, block and end line.
        /// </summary>
        public Rect[] CalculatedBounds
        {
            get { return _caclulatedBounds; }
        }
        

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
        public override Unit Height
        {
            get { return Unit.Zero; }
        }

        #endregion

        #region public override PDFUnit Width {get;}

        private Unit _width;

        /// <summary>
        /// Gets the height of this begin text run - PDFUnit.Zero or Any left padding on the inline run style
        /// </summary>
        public override Unit Width
        {
            get { return _width; }
        }

        #endregion

        
        /// <summary>
        /// Gets or sets the first line inset for this beginning text run.
        /// </summary>
        public Unit LineInset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the total bounds of this text run
        /// </summary>
        public Rect TotalBounds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the start text cursor point within the page for this text
        /// </summary>
        public Size StartTextCursor
        {
            get;
            private set;
        }
        
        private Unit _offsetY;

        /// <summary>
        /// Gets the Y offset for this text run.
        /// </summary>
        public override Unit OffsetY
        {
            get { return _offsetY; }
        }

        private Unit _charspace, _wordspace;
        private bool _hascustomspace;

        public Unit CharSpace
        {
            get { return _charspace; }
        }

        public Unit WordSpace
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


        public PDFTextRunBegin(PDFTextRenderOptions renderopts, PDFLayoutLine line, IComponent owner)
            : base(line, owner)
        {
            this.TextRenderOptions = renderopts;
            this.Lines.Add(line);
        }

        //
        // implementation
        //

        
        
        public override void SetOffsetY(Unit y)
        {
            Rect bounds = this.TotalBounds;
            bounds.Y -= this._offsetY;
            this._offsetY = y;
            bounds.Y += this._offsetY;
            this.TotalBounds = bounds;
            
            base.SetOffsetY(y);
            //Rect bounds = this.TotalBounds;
            //bounds.Y += y;
            //this.TotalBounds = bounds;
        }

        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            Rect final = TotalBounds;
            if (yoffset != Unit.Zero)
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
        public void PushCompleteTextBlock(PDFTextRunEnd ending, PDFLayoutContext context, int pageIndex, Unit xoffset, Unit yoffset)
        {
            Rect[] all = new Rect[3];

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



        private Rect CalculateOnlyLineBounds(PDFTextRunEnd end)
        {
            PDFLayoutLine line = this._lines[0];
            Rect full = new Rect(this.TotalBounds.Location, Size.Empty);

            bool counting = false;
            Unit linewidth = Unit.Zero;
            Unit lineOffset = line.RightInset;
            bool first = true;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (run == end)
                    break;
                else if (counting)
                {
                    linewidth += run.Width;
                    if (run is PDFTextRunCharacter chars)
                        linewidth += chars.ExtraSpace;
                }
                else if(!first)
                {
                    lineOffset += run.Width;
                    if (run is PDFTextRunCharacter chars)
                        lineOffset += chars.ExtraSpace;
                }
                first = false;
            }
            full.Width = linewidth;
            full.Height = line.Height;
            full.X += lineOffset;

            return full;
        }

        private Rect CalculateFirstLineBounds()
        {
            PDFLayoutLine line = this._lines[0];
            Rect full = new Rect(this.TotalBounds.Location, Size.Empty);

            bool counting = false;
            Unit linewidth = Unit.Zero;
            Unit lineOffset = line.RightInset;
            bool first = true;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (counting)
                {
                    linewidth += run.Width;
                    if (run is PDFTextRunCharacter chars)
                        linewidth += chars.ExtraSpace;
                }
                else if(!first)
                {
                    lineOffset += run.Width;
                    if (run is PDFTextRunCharacter chars)
                        lineOffset += chars.ExtraSpace;
                }

                first = false;
            }
            full.Width = linewidth;
            full.Height = line.Height;
            full.X += lineOffset;

            return full;
        }

        private Rect CalculateLastLineBounds(Unit voffset, PDFTextRunEnd end)
        {
            PDFLayoutLine line = this._lines[this._lines.Count -1];
            Rect full = new Rect(this.TotalBounds.Location, Size.Empty);
            full.X += line.RightInset;
            full.Y += voffset;
            Unit linewidth = Unit.Zero;
            full.Height = line.Height;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == end)
                    break;
                if (run is PDFTextRunSpacer && linewidth == 0) //spacer at the start of the line - so push right
                {
                    full.X += run.Width;
                }
                else
                {
                    linewidth += run.Width;
                    if (run is PDFTextRunCharacter chars)
                        linewidth += chars.ExtraSpace;
                }
            }

            full.Width = linewidth;
            return full;
        }

        private Rect CalculateInnerTotalBounds(Unit voffset, PDFTextRunEnd ending)
        {

            Rect full = this.TotalBounds.Clone();
            full.Size = Size.Empty;
            full.Y += voffset;

            //only want the lines that are between the first and last line
            int firstIndex = 1;
            int lastIndex = this.Lines.Count - 2;

            Unit maxright = Unit.Zero;
            Unit minleft = new Unit(Double.MaxValue);

            for (int i = firstIndex; i <= lastIndex; i++)
            {
                PDFLayoutLine line = this.Lines[i];
                
#if FULL_WIDTH
                minleft = 0;
                maxright = PDFUnit.Max(maxright, line.FullWidth);
#else

                Unit x = Unit.Zero; // line.RightInset;
                Unit w = line.Width + line.RightInset;
                
                if (line.ExtraSpace.HasValue)
                    w += line.ExtraSpace.Value;
                
                if (line.Runs[0] is PDFTextRunSpacer)
                {
                    x += line.Runs[0].Width;
                }
                maxright = Unit.Max(maxright, w);
                minleft = Unit.Min(minleft, x);
                full.Height += line.Height;
#endif
            }

            if (minleft > 0)
                full.X = minleft;
            full.Width = maxright - minleft;

            return full;
        }

        #endregion

        public void SetTextSpacing(Unit wordSpace, Unit charSpace)
        {
            this._charspace = charSpace;
            this._wordspace = wordSpace;
            this._hascustomspace = true;
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            Rect bounds = this.TotalBounds;
            bounds.X += context.Offset.X;
            bounds.Y += context.Offset.Y;

            //Set the arrangement for this text run
            Component owner = this.Owner as Component;
            // THIS IS NOT DONE ON THER RENDER BACKGROUND SO THAT INDIVIDUAL LINES AND OFFSETS FROM FLOATS ARE CAPTURED
            // if (owner != null && null != this._caclulatedBounds)
            // {
            //     for (int i = 0; i < this._caclulatedBounds.Length; i++)
            //     {
            //         Rect b = this._caclulatedBounds[i];
            //         if (b.IsEmpty == false)
            //         {
            //             if (i == 0)
            //                 b.X += this.LineInset;
            //
            //             b.X += context.Offset.X;
            //             b.Y += context.Offset.Y;
            //             owner.SetArrangement(context, context.FullStyle, b);
            //         }
            //     }
            //    
            // }

            

            bounds.X += this.LineInset;
            //bounds.Y += this.OffsetY;
            Size cursor = new Size(bounds.X,bounds.Y);


            //pdf text rendering is done from the baseline, we need the rendering to appear to start from the top
            //so add the ascent of the font metrics

            //Previous 27 Feb 20
            //cursor.Height += this.TextRenderOptions.Font.FontMetrics.Ascent;

            //With mixed content
            cursor.Height += this.Line.BaseLineOffset;
            

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Text Begin", "Marker to beginning to render the text for component " + this.Owner.ToString() + " at cursor position " + cursor);

            var firstArrangement = this.RenderTextBackgroundAndArrange(context, writer);

            context.Graphics.SaveGraphicsState();
            context.Graphics.BeginText();

            var block = this.GetParentBlock();

            //if (null != block && block.Position != null && block.Position.TransformMatrix != null)
            //    context.Graphics.SetTransformationMatrix(block.Position.TransformMatrix, true, false);

            context.Graphics.SetTextRenderOptions(this.TextRenderOptions, bounds);
            if (_hascustomspace && this.TextRenderOptions.CharacterSpacing.HasValue == false && this.TextRenderOptions.WordSpacing.HasValue == false)
                context.Graphics.SetTextSpacing(this._wordspace, this._charspace, this.TextRenderOptions.Font.Size);

            if (this.ShouldRenderBorder(context))
                this.RenderTextBorder(context, writer, firstArrangement);

            context.Graphics.MoveTextCursor(cursor, true);
            this.StartTextCursor = cursor;


            

            return null;
        }

        

        public bool ShouldRenderBorder(PDFRenderContext context)
        {
            return this.TextRenderOptions.Border != null;
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

        public ComponentMultiArrangement RenderTextBackgroundAndArrange(PDFRenderContext context, PDFWriter writer)
        {
            ComponentMultiArrangement firstArrangement = null;
            if (null == this.CalculatedBounds || this.CalculatedBounds.Length == 0)
            {
                if (context.ShouldLogVerbose)
                {
                    context.TraceLog.Add(TraceLevel.Warning, "Text", "The calculated bounds for a text run begin was null or empty so cannot render background for component " + (this.Owner == null ? "unknown" : this.Owner.ID));
                }
                return null;
            }
            
            
            Component toArrange = this.Owner as Component;
            
            var brush = this.TextRenderOptions.Background;
            var pad = this.TextRenderOptions.Padding.HasValue ? this.TextRenderOptions.Padding.Value : Thickness.Empty();
            var rad = this.TextRenderOptions.BorderRadius;
            var metrics = this.TextRenderOptions.Font.FontMetrics;
            var textLeft = Unit.Zero;
            var rect = this.CalculatedBounds[0];


            Unit height = metrics.TotalLineHeight;// this.TextRenderOptions.GetLineHeight(); //The height of the line
            PDFTextRunNewLine rn = null;
            var halfH = (height - (metrics.Ascent + metrics.Descent)) / 2;
            Unit ascOffset = this.Line.BaseLineOffset - (this.TextRenderOptions.GetAscent() + halfH); //The ascender offset from the top of the line - usually zero unless we have leading.
                
            

            if (this.HasCustomSpace && this.LineInset > pad.Left)
            {
                //We are not the first run on the line, so we use the inset to know where we are.
                rect.X = this.LineInset - pad.Left;
            }

            if (!rect.IsEmpty && rect.Width > 0)
            {
                rect = rect.Offset(context.Offset);
                rect.Y += ascOffset;
                rect.Height = height;

                var padRect = rect;
                
                if (pad.IsEmpty == false)
                {
                    if (this.Lines.Count == 1)
                        padRect = rect.Inflate(pad);
                    else
                    {
                        //We only pad horizontally on the first and last run
                        padRect.X -= pad.Left;
                        padRect.Y -= pad.Top;
                        padRect.Width += pad.Left;
                        padRect.Height += pad.Top + pad.Bottom;
                    }
                }
                if (null != brush)
                {
                    if (this.Lines.Count > 1  && this.Line.HAlignment != HorizontalAlignment.Justified)
                        padRect.Width += this.TextRenderOptions.GetLeftSideBearing();
                    
                    if (rad > 0)
                    {
                        if (this.CalculatedBounds.Length == 3 && (this.CalculatedBounds[1].IsEmpty == false ||
                                                                  this.CalculatedBounds[2].IsEmpty == false))
                        {
                            //we have further lines so we only apply the corners to the top left and bottom right.
                            context.Graphics.FillRoundRectangle(brush, padRect, Sides.Left | Sides.Top | Sides.Bottom,
                                rad);
                        }
                        else
                        {
                            context.Graphics.FillRoundRectangle(brush, padRect, rad);
                        }
                    }
                    else
                        context.Graphics.FillRectangle(brush, padRect);
                }

                if (null != toArrange)
                {
                    firstArrangement = toArrange.SetArrangement(context, context.FullStyle, padRect) as ComponentMultiArrangement;
                }

                textLeft = rect.X;

                
                //move the rect down to the next line
                var first = this.Lines[0];
                rn = first.Runs[first.Runs.Count - 1] as PDFTextRunNewLine;
                if (null != rn)
                    rect.Y += rn.NewLineOffset.Height ;
                else
                    rect.Y += this.TextRenderOptions.GetLineHeight();
            }

            if (this.Lines.Count > 1)
            {

                
                rect.Height = height;
                rect.X = textLeft;
                //we render the background for the second to the pen-ultimate line

                for (var l = 1; l < this.Lines.Count; l++)
                {
                    var isLastRunOnLine = false;
                    var lastLine = false;
                    var line = this.Lines[l];
                    var lineRect = rect.Clone();

                    lineRect.X += line.RightInset;

                    if (l == this.Lines.Count - 1) // we are the last line in this inline block so calculate the width
                    {
                        lastLine = true;
                        lineRect.Width = 0;
                        for (var r = 0; r < line.Runs.Count; r++)
                        {
                            var run = line.Runs[r];
                            if (run is PDFTextRunEnd)
                            {
                                if (r == line.Runs.Count - 1)
                                    isLastRunOnLine = true;
                                else if (r == line.Runs.Count - 2 && line.Runs[r + 1] is PDFLayoutInlineEnd)
                                    isLastRunOnLine = true;
                                break;
                            }
                            else
                            {
                                lineRect.Width += run.Width;
                            }
                        }
                        
                    }
                    else
                    {
                        lineRect.Width = line.Width;
                        isLastRunOnLine = true;
                    }

                    if (line.ExtraSpace.HasValue)
                    {
                        lineRect.Width += line.ExtraSpace.Value;
                    }

                    if (line.Runs.Count > 0 && line.Runs[0] is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer)
                    {
                        var prev = this.Lines[l - 1];
                        var newLine = prev.Runs[prev.Runs.Count - 1] as PDFTextRunNewLine;
                        if (null != newLine)
                        {
                            lineRect.X = spacer.Width + textLeft - newLine.NewLineOffset.Width;
                        }

                        lineRect.Width -= spacer.Width;
                    }

                    if (lineRect.Width > Unit.Zero)
                    {
                        var padRect = lineRect.Clone();

                        if (isLastRunOnLine && this.Line.HAlignment != HorizontalAlignment.Justified)
                        {
                            //add extra space for the left side bearing of the first character so the last of the line ends correctly
                            padRect.Width += this.TextRenderOptions.GetLeftSideBearing(); 

                        }

                        padRect.Y -= pad.Top;
                        padRect.X += pad.Left;
                        padRect.Height += pad.Top + pad.Bottom;

                        if (lastLine)
                        {
                            padRect.Width += pad.Right;
                        }

                        if (null != brush)
                        {
                            if (rad > 0 && lastLine)
                            {
                                //edge case where we overflow, but there are no (significant) characters after. Show the radii
                                context.Graphics.FillRoundRectangle(brush, padRect,
                                    Sides.Right | Sides.Top | Sides.Bottom, rad);
                            }
                            else
                            {
                                //otherwise no rounded corners on intermediate lines
                                context.Graphics.FillRectangle(brush, padRect);
                            }
                        }

                        if (null != toArrange)
                        {
                            toArrange.SetArrangement(context, context.FullStyle, padRect);
                        }

                    }

                    rn = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;

                    //move the actual rect down to the next line
                    if (null != rn)
                        rect.Y += rn.NewLineOffset.Height;
                    else
                        rect.Y += this.TextRenderOptions.GetLineHeight();

                    textLeft = lineRect.X;
                }
            }

            
            return firstArrangement;
        }


        public void RenderTextBorder(PDFRenderContext context, PDFWriter writer, ComponentMultiArrangement firstArrangement)
        {
            var arrange = firstArrangement;
            
            var rad = this.TextRenderOptions.BorderRadius;
            var pen = this.TextRenderOptions.Border;
            Rect rect;
            
            if (null == pen) return; //nothing to draw with
            
            if (this.TextRenderOptions.Padding.HasValue && this.TextRenderOptions.Padding.Value.IsEmpty == false)
            {
                var pad = this.TextRenderOptions.Padding.Value;
                
                while (null != arrange)
                {
                    rect = arrange.RenderBounds;
                    //rect = rect.Inflate(pad);
                    
                    var sides = Sides.Top | Sides.Bottom;
                    
                    if (arrange == firstArrangement)
                    {
                        //First so left
                        sides |= Sides.Left;
                    }

                    if (null == arrange.NextArrangement)
                    {
                        //Last so right
                        sides |= Sides.Right;
                    }
                    
                    if(rad > 0)
                        context.Graphics.DrawRoundRectangle(pen, rect, sides, rad);
                    else
                        context.Graphics.DrawRectangle(pen, rect, sides);

                    arrange = arrange.NextArrangement;
                }
            }
            else
            {
                while (null != arrange)
                {
                    rect = arrange.RenderBounds;

                    var sides = Sides.Top | Sides.Bottom;
                    
                    if (arrange == firstArrangement)
                    {
                        //First so left
                        sides |= Sides.Left;
                    }

                    if (null == arrange.NextArrangement)
                    {
                        //Last so right
                        sides |= Sides.Right;
                    }
                    
                    if(rad > 0)
                        context.Graphics.DrawRoundRectangle(pen, rect, sides, rad);
                    else
                        context.Graphics.DrawRectangle(pen, rect, sides);

                    arrange = arrange.NextArrangement;

                }
            }
            
        }

        public static double ThicknessFactor = 12.0;
        public static double UnderlineOffsetFactor = 6.0;
        public static double StrikeThroughOffset = 0.4;

        public void RenderUnderlines(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            
            Unit offsetV = this.TextRenderOptions.GetAscent() / UnderlineOffsetFactor;
            Unit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        public void RenderStrikeThrough(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            //Strike through is up offset one third the font ascent
            Unit offsetV = new Unit(-this.TextRenderOptions.GetAscent().PointsValue * StrikeThroughOffset,PageUnits.Points);

            Unit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        public void RenderOverLines(PDFTextRunEnd end, PDFRenderContext context, PDFWriter writer)
        {
            Unit offsetV = new Unit(-this.TextRenderOptions.GetAscent().PointsValue, PageUnits.Points);
            Unit linethickness = this.TextRenderOptions.Font.Size / ThicknessFactor;
            this.RenderTextDecoration(end, context, writer, offsetV, linethickness);
        }

        protected virtual void RenderTextDecoration(PDFTextRunEnd endRun, PDFRenderContext context, PDFWriter writer, Unit vOffset, Unit linethickness)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Text Decoration", "Starting to render text decorations");
            
            //set up the graphics context
            PDFPen pen = PDFPen.Create(this.TextRenderOptions.FillBrush, linethickness);
            
            context.Graphics.SaveGraphicsState();
            pen.SetUpGraphics(context.Graphics, this.TotalBounds);

            bool drawing = false;
            Point linestart = new Point(this.StartTextCursor.Width, this.StartTextCursor.Height + vOffset);
            

            foreach (PDFLayoutLine line in this.Lines)
            {
                Unit linewidth = 0;

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
                            Point start = new Point(linestart.X + linewidth, linestart.Y);
                            Point end = new Point(linestart.X + linewidth + chars.Width + chars.ExtraSpace, linestart.Y);
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
                            Size offset = newline.NewLineOffset;
                            if (null != newline.NextLineSpacer)
                            {
                                Unit nextoffset = newline.NextLineSpacer.Width;
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
                            Point start = new Point(linestart.X + linewidth, linestart.Y);
                            Point end = new Point(linestart.X + linewidth + chars.Width, linestart.Y);
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
