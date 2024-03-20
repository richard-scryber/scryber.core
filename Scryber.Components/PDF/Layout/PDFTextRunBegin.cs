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
            base.SetOffsetY(y);
            Rect bounds = this.TotalBounds;
            bounds.Y += y;
            this.TotalBounds = bounds;
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
            Unit lineOffset = Unit.Zero;
            bool first = true;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (run == end)
                    break;
                else if (counting)
                    linewidth += run.Width;
                else if(!first)
                {
                    lineOffset += run.Width;
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
            Unit lineOffset = Unit.Zero;
            bool first = true;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == this)
                    counting = true;
                else if (counting)
                    linewidth += run.Width;
                else if(!first)
                {
                    lineOffset += run.Width;
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
            full.Y += voffset;
            Unit linewidth = Unit.Zero;
            full.Height = line.Height;

            foreach (PDFLayoutRun run in line.Runs)
            {
                if (run == end)
                    break;
                if (run is PDFTextRunSpacer && linewidth == 0) //spacer at the start of the line - so push right
                    full.X = run.Width;
                else
                    linewidth += run.Width;
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

                Unit x = Unit.Zero;
                Unit w = line.Width;
                if (line.Runs[0] is PDFTextRunSpacer)
                {
                    x = line.Runs[0].Width;
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
            if (owner != null && null != this._caclulatedBounds)
            {
                for (int i = 0; i < this._caclulatedBounds.Length; i++)
                {
                    Rect b = this._caclulatedBounds[i];
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
            //FontMetrics metrics = this.TextRenderOptions.Font.FontMetrics;

            bounds.X += this.LineInset;

            Size cursor = new Size(bounds.X,bounds.Y);


            //pdf text rendering is done from the baseline, we need the rendering to appear to start from the top
            //so add the ascent of the font metrics

            //Previous 27 Feb 20
            //cursor.Height += this.TextRenderOptions.Font.FontMetrics.Ascent;

            //With mixed content
            cursor.Height += this.Line.BaseLineOffset;
            

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Text Begin", "Marker to beginning to render the text for component " + this.Owner.ToString() + " at cursor position " + cursor);

            if (this.ShouldRenderBackground(context))
                this.RenderTextBackground(context, writer);

            context.Graphics.SaveGraphicsState();
            context.Graphics.BeginText();

            var block = this.GetParentBlock();

            //if (null != block && block.Position != null && block.Position.TransformMatrix != null)
            //    context.Graphics.SetTransformationMatrix(block.Position.TransformMatrix, true, false);

            context.Graphics.SetTextRenderOptions(this.TextRenderOptions, bounds);
            if (_hascustomspace && this.TextRenderOptions.CharacterSpacing.HasValue == false && this.TextRenderOptions.WordSpacing.HasValue == false)
                context.Graphics.SetTextSpacing(this._wordspace, this._charspace, this.TextRenderOptions.Font.Size);

            if (this.ShouldRenderBorder(context))
                this.RenderTextBorder(context, writer);

            context.Graphics.MoveTextCursor(cursor, true);
            this.StartTextCursor = cursor;


            

            return null;
        }

        public bool ShouldRenderBackground(PDFRenderContext context)
        {
            return this.TextRenderOptions.Background != null;
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

        public void RenderTextBackground(PDFRenderContext context, PDFWriter writer)
        {
            if (null == this.CalculatedBounds || this.CalculatedBounds.Length == 0)
            {
                if (context.ShouldLogVerbose)
                {
                    context.TraceLog.Add(TraceLevel.Warning, "Text", "The calculated bounds for a text run begin was null or empty so cannot render background for component " + (this.Owner == null ? "unknown" : this.Owner.ID));
                }
                return;
            }
            var brush = this.TextRenderOptions.Background;
            var pad = this.TextRenderOptions.Padding.HasValue ? this.TextRenderOptions.Padding.Value : Thickness.Empty();
            var rad = this.TextRenderOptions.BorderRadius;
            var metrics = this.TextRenderOptions.Font.FontMetrics;

            var rect = this.CalculatedBounds[0];

            Unit ascOffset = 0; //The ascender offset from the top of the line - usually zero unless we have leading.
            Unit height = rect.Height; //The height of the line

            if (this.TextRenderOptions.Leading.HasValue)
            {
                //We have spacing around the line (or negative spacing) but the background of inline
                //is always dependant on the actual font heights + any padding.
                ascOffset = (this.TextRenderOptions.GetBaselineOffset() - this.TextRenderOptions.GetAscent());
                height = (this.TextRenderOptions.GetAscent() + this.TextRenderOptions.GetDescender());
            }

            if (!rect.IsEmpty && rect.Width > 0)
            {
                rect = rect.Offset(context.Offset);
                rect.Y += ascOffset;
                rect.Height = height;

                //Left and right are part of the calculated bounds with the spacer.
                //So just add the top and bottom
                rect.Y -= pad.Top;
                rect.Height += pad.Top + pad.Bottom;
                if (rad > 0)
                {
                    if (this.CalculatedBounds.Length == 3 && (this.CalculatedBounds[1].IsEmpty == false || this.CalculatedBounds[2].IsEmpty == false))
                    {
                        //we have further lines so we only apply the corners to the top left and bottom right.
                        context.Graphics.FillRoundRectangle(brush, rect, Sides.Left | Sides.Top | Sides.Bottom, rad);
                    }
                    else
                    {
                        context.Graphics.FillRoundRectangle(brush, rect, rad);
                    }
                }
                else
                    context.Graphics.FillRectangle(brush, rect);
            }

            if (this.CalculatedBounds.Length > 1)
            {
                rect = this.CalculatedBounds[1];
                if (!rect.IsEmpty)
                {
                    rect = rect.Offset(context.Offset);
                    rect.Y += ascOffset;
                    rect.Y -= pad.Top;

                    rect.Height = height;
                    rect.Height += pad.Top + pad.Bottom;

                    //we render the background for the second to the pen-ultimate line
                    for(var l = 1; l < this.Lines.Count -1; l++) {
                        var line = this.Lines[l];

                        rect.Width = line.Width;

                        if(line.Runs.Count > 0 && line.Runs[0] is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer)
                        {
                            rect.X = spacer.Width + context.Offset.X;
                            rect.Width -= spacer.Width;
                        }
                        else
                        {
                            rect.X = context.Offset.X;
                        }
                        if (rect.Width > 0)
                        {
                            if(rad > 0 && this.CalculatedBounds.Length > 2 && this.CalculatedBounds[2].Width <= 0)
                            {
                                //edge case where we overflow, but there are no (significant) characters after. Show the radii
                                context.Graphics.FillRoundRectangle(brush, rect, Sides.Right | Sides.Top | Sides.Bottom, rad);
                            }
                            else
                            {
                                //otherwise no rounded corners on intermediate lines
                                context.Graphics.FillRectangle(brush, rect);
                            }
                            
                            
                        }

                        //move the rect down to the next line
                        rect.Y += line.Height;
                    }

                    
                }
            }

            if(this.CalculatedBounds.Length > 2)
            {
                rect = this.CalculatedBounds[2];
                if (!rect.IsEmpty && rect.Width > 0)
                {
                    rect = rect.Offset(context.Offset);

                    rect.Y += ascOffset;
                    rect.Y -= pad.Top;

                    rect.Height = height;
                    rect.Height += pad.Top + pad.Bottom;
                    if (rad > 0)
                    {
                        //We are the last line so only round the top right and bottom right corners.
                        context.Graphics.FillRoundRectangle(brush, rect, Sides.Right | Sides.Top | Sides.Bottom, rad);
                    }
                    else
                    {
                        context.Graphics.FillRectangle(brush, rect);
                    }
                }
            }

        }


        private const Sides StartSides = Sides.Left | Sides.Top | Sides.Bottom;
        private const Sides EndSides = Sides.Right | Sides.Top | Sides.Bottom;
        private const Sides MidSides = Sides.Top | Sides.Bottom;
        private const Sides AllSides = Sides.Left | Sides.Right | Sides.Top | Sides.Bottom;



        public void RenderTextBorder(PDFRenderContext context, PDFWriter writer)
        {
            if (null == this.CalculatedBounds || this.CalculatedBounds.Length == 0)
            {
                if (context.ShouldLogVerbose)
                {
                    context.TraceLog.Add(TraceLevel.Warning, "Text", "The calculated bounds for a text run begin was null or empty so cannot render background for component " + (this.Owner == null ? "unknown" : this.Owner.ID));
                }
                return;
            }
            var pen = this.TextRenderOptions.Border;
            var pad = this.TextRenderOptions.Padding.HasValue ? this.TextRenderOptions.Padding.Value : Thickness.Empty();
            var rad = this.TextRenderOptions.BorderRadius;
            var metrics = this.TextRenderOptions.Font.FontMetrics;

            var rect = this.CalculatedBounds[0];

            Unit ascOffset = 0; //The ascender offset from the top of the line - usually zero unless we have leading.
            Unit height = rect.Height; //The height of the line

            if (this.TextRenderOptions.Leading.HasValue)
            {
                //We have spacing around the line (or negative spacing) but the background of inline
                //is always dependant on the actual font heights + any padding.
                ascOffset = (this.TextRenderOptions.GetBaselineOffset() - this.TextRenderOptions.GetAscent());
                height = (this.TextRenderOptions.GetAscent() + this.TextRenderOptions.GetDescender());
            }

            if (!rect.IsEmpty && rect.Width > 0)
            {
                rect = rect.Offset(context.Offset);
                rect.Y += ascOffset;
                rect.Height = height;

                //Left and right are part of the calculated bounds with the spacer.
                //So just add the top and bottom
                rect.Y -= pad.Top;
                rect.Height += pad.Top + pad.Bottom;
                if (rad > 0)
                {
                    if (this.CalculatedBounds.Length == 3 && (this.CalculatedBounds[1].IsEmpty == false || this.CalculatedBounds[2].IsEmpty == false))
                    {
                        //we have further lines so we only apply the corners to the top left and bottom right.
                        context.Graphics.DrawRoundRectangle(pen, rect, Sides.Left | Sides.Top | Sides.Bottom, rad);
                    }
                    else
                    {
                        context.Graphics.DrawRoundRectangle(pen, rect, rad);
                    }
                }
                else
                    context.Graphics.DrawRectangle(pen, rect);
            }

            if (this.CalculatedBounds.Length > 1)
            {
                rect = this.CalculatedBounds[1];
                if (!rect.IsEmpty)
                {
                    rect = rect.Offset(context.Offset);
                    rect.Y += ascOffset;
                    rect.Y -= pad.Top;

                    rect.Height = height;
                    rect.Height += pad.Top + pad.Bottom;

                    //we render the background for the second to the pen-ultimate line
                    for (var l = 1; l < this.Lines.Count - 1; l++)
                    {
                        var line = this.Lines[l];

                        rect.Width = line.Width;

                        if (line.Runs.Count > 0 && line.Runs[0] is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer)
                        {
                            rect.X = spacer.Width + context.Offset.X;
                            rect.Width -= spacer.Width;
                        }
                        else
                        {
                            rect.X = context.Offset.X;
                        }
                        if (rect.Width > 0)
                        {
                            if (rad > 0 && this.CalculatedBounds.Length > 2 && this.CalculatedBounds[2].Width <= 0)
                            {
                                //edge case where we overflow, but there are no (significant) characters after. Show the radii
                                context.Graphics.DrawRoundRectangle(pen, rect, Sides.Right | Sides.Top | Sides.Bottom, rad);
                            }
                            else
                            {
                                //otherwise no rounded corners on intermediate lines
                                context.Graphics.DrawRectangle(pen, rect);
                            }


                        }

                        //move the rect down to the next line
                        rect.Y += line.Height;
                    }


                }
            }

            if (this.CalculatedBounds.Length > 2)
            {
                rect = this.CalculatedBounds[2];
                if (!rect.IsEmpty && rect.Width > 0)
                {
                    rect = rect.Offset(context.Offset);

                    rect.Y += ascOffset;
                    rect.Y -= pad.Top;

                    rect.Height = height;
                    rect.Height += pad.Top + pad.Bottom;
                    if (rad > 0)
                    {
                        //We are the last line so only round the top right and bottom right corners.
                        context.Graphics.DrawRoundRectangle(pen, rect, Sides.Right | Sides.Top | Sides.Bottom, rad);
                    }
                    else
                    {
                        context.Graphics.DrawRectangle(pen, rect);
                    }
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
