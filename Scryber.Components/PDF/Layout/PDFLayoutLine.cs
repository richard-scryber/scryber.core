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
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// A single line within a column
    /// </summary>
    public class PDFLayoutLine : PDFLayoutItem
    {

        #region public PDFLayoutRegion Region {get;}

        /// <summary>
        /// returns the region that contains this line
        /// </summary>
        public PDFLayoutRegion Region
        {
            get { return this.Parent as PDFLayoutRegion; }
        }

        #endregion 

        #region public PDFUnit Height

        private Unit? _totalHeight;
        /// <summary>
        /// Gets the total height of this line. 
        /// This is normally only calculated once the line is closed.
        /// </summary>
        public override Unit Height
        {
            get
            {
                if (_totalHeight.HasValue)
                    return _totalHeight.Value;

                if (null == this._runs || this._runs.Count == 0)
                    return Unit.Zero;
                else if (this._runs.Count == 1)
                    return this._runs[0].Height;
                else
                {
                    Unit max = Unit.Zero;
                    foreach (PDFLayoutRun run in this._runs)
                    {
                        max = Unit.Max(max, run.Height);
                    }
                    return max;
                }
            }
        }

        #endregion

        #region public PDFUnit FullWidth { get; }

        /// <summary>
        /// Gets the complete line width.
        /// This is the total width available to the line to use for all runs.
        /// </summary>
        public Unit FullWidth { get; private set; }

        #endregion

        #region public PDFUnit AvailableWidth {get;}

        /// <summary>
        /// Gets the available width of the line.
        /// This is the remaining space after the used width is removed from the full width 
        /// </summary>
        public Unit AvailableWidth
        {
            get
            {
                Unit used = this.Width;
                return this.FullWidth - used;
            }
        }

        #endregion

        #region public PDFUnit Width { get; }

        /// <summary>
        /// Gets the hoziontal space used by all the runs in this line
        /// </summary>
        public override Unit Width
        {
            get
            {
                if (this._runs == null || _runs.Count == 0)
                    return Unit.Zero;
                else if (this._runs.Count == 1)
                    return this._runs[0].Width;
                else
                {
                    Unit total = Unit.Zero;

                    foreach (PDFLayoutRun run in this._runs)
                    {
                        total += run.Width;
                    }

                    return total;
                }
            }
        }

        #endregion

        #region public PDFLayoutRunCollection Runs {get;}

        private PDFLayoutRunCollection _runs = null;

        /// <summary>
        /// Gets all the runs in the this line
        /// </summary>
        public PDFLayoutRunCollection Runs
        {
            get
            {
                if (null == _runs)
                    _runs = new PDFLayoutRunCollection();
                return _runs;
            }
        }

        #endregion

        #region public PDFUnit OffsetY { get; set; }

        private Unit _xoffset;
        private Unit _yoffset;

        /// <summary>
        /// Gets or sets the offset of this line in it's container
        /// </summary>
        public override Unit OffsetY { get { return _yoffset; } }

        #endregion

        /// <summary>
        /// Gets or sets the offset of this line in it's container
        /// </summary>
        public override Unit OffsetX { get { return _xoffset; } }


        #region public HorizontalAlignment HAlignment {get;set;}

        /// <summary>
        /// Gets or sets the horizontal alignment of the content in this line
        /// </summary>
        public HorizontalAlignment HAlignment
        {
            get;
            set;
        }

        #endregion

        #region public VerticalAlignment VAlignment {get;set;}

        /// <summary>
        /// Gets or sets the vertical alignment of this line.
        /// </summary>
        public VerticalAlignment VAlignment
        {
            get;
            set;
        }

        #endregion

        #region public int LineIndex { get;  set; }

        /// <summary>
        /// Gets or sets the index of this line within the paragraph
        /// </summary>
        public int LineIndex { get;  set; }

        #endregion

        #region public PDFUnit BaseLineOffset { get; set; }

        /// <summary>
        /// Gets the offset from the top left of the line to the baseline of any text or component.
        /// </summary>
        public Unit BaseLineOffset { get; set; }

        #endregion

        #region public Unit BaseLineToBottom { get; set; }

        /// <summary>
        /// Gets or sets the value bottom leading plus the bottom descender.
        /// to make a full LineHeight = BaseLineOffset + BaseLineToBottom.
        /// </summary >
        public Unit BaseLineToBottom { get; set; }

        #endregion


        #region public bool IsEmpty {get;}

        /// <summary>
        /// Returns true if this is an empty line - does not have any runs on it
        /// </summary>
        public bool IsEmpty
        {
            get { return this._runs == null || this._runs.Count == 0; }
        }

        #endregion

        /// <summary>
        /// Gets or sets any right inset (from a float:right) that was applied to the newline offset.
        /// </summary>
        public Unit RightInset { get; set; }
        
        
        protected Unit? ExtraCharacterSpace { get; set; }

        protected Unit? ExtraWordSpace { get; set; }

        //
        // ctor(s)
        // 

        #region public PDFLayoutLine(PDFLayoutRegion region, PDFUnit fullwidth)

        /// <summary>
        /// Creates a new line in the specified region.
        /// </summary>
        /// <param name="region">The region that contains this line</param>
        /// <param name="fullwidth">The full available horizontal space for this line </param>
        public PDFLayoutLine(PDFLayoutRegion region, Unit fullwidth, HorizontalAlignment halign, VerticalAlignment valign, int lineindex)
            : base(region, null)
        {
            this.FullWidth = fullwidth;
            this.HAlignment = halign;
            this.VAlignment = valign;
            this.LineIndex = lineindex;
            this.BaseLineOffset = 0;
        }

        #endregion 

        //
        // methods
        //

        #region public PDFLayoutRun LastRun()

        /// <summary>
        /// Returns the last run in this line
        /// </summary>
        /// <returns></returns>
        public PDFLayoutRun LastRun()
        {
            if (null == this._runs || this._runs.Count == 0)
                return null;
            else
                return this._runs[this._runs.Count - 1];
        }

        #endregion

        #region protected override bool DoClose(ref string msg)

        /// <summary>
        /// Overrides default behaviour to calculate the maximum height.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected override bool DoClose(ref string msg)
        {
            PDFLayoutRun last = this.LastRun();
            if (null != last && last.IsClosed == false)
                last.Close();
            EnsureAllRunsOnSameLevel();
            return base.DoClose(ref msg);
        }

        #endregion

        #region public virtual bool CanFitWidth(PDFUnit width)
        
        /// <summary>
        /// Checks the required width against the space available on this line
        /// and returns true if this requirement can be accomodated
        /// </summary>
        /// <param name="width">The required width</param>
        /// <returns>True if the width can be accomdated, otherwise false</returns>
        public virtual bool CanFitWidth(Unit width)
        {
            return width < this.AvailableWidth;
        }

        #endregion


        private void EnsureAllRunsOnSameLevel()
        {

            Unit totalHeight = Unit.Zero;
            Unit maxHeight = Unit.Zero;
            Unit maxDescender = Unit.Zero;
            Unit lastlineheight = Unit.Zero;
            Unit maxFontSize = Unit.Zero;

            bool isComplex = false;


            if (false && this.Runs.Count == 3 && (this.Runs[0] is PDFTextRunSpacer || this.Runs[0] is PDFTextRunBegin))
            {
                //No Alignment needed as we are a simple text run
            }
            else
            {
                VerticalAlignment? valign = null;
                var currWidth = Unit.Zero;

                for (var index = 0; index < this.Runs.Count; index++)
                {
                    var run = this.Runs[index];
                    var itemH = run.Height;

                    if (itemH > maxHeight)
                        maxHeight = itemH;

                    if (run is PDFTextRunBegin begin && begin.TextRenderOptions != null)
                    {
                        maxDescender = Unit.Max(maxDescender, begin.TextRenderOptions.GetDescender());
                        maxFontSize = Unit.Max(maxFontSize, begin.TextRenderOptions.GetSize());
                    }
                    else if (run is PDFLayoutInlineBlockRun blockRun)
                    {
                        //We have inline blocks to align.
                        isComplex = true;

                        var rect = blockRun.Region.TotalBounds;
                        rect.X = currWidth;
                        blockRun.Region.TotalBounds = rect;

                        //pick the first vertical alignment for the line.
                        if (blockRun.PositionOptions.VAlign.HasValue && !valign.HasValue)
                            valign = blockRun.PositionOptions.VAlign.Value;
                    }
                    else if (run is PDFLayoutComponentRun compRun)
                    {
                        isComplex = true;

                        if (compRun.PositionOptions.VAlign.HasValue && !valign.HasValue)
                            valign = compRun.PositionOptions.VAlign.Value;

                    }
                    else if (run is PDFLayoutXObject xobjRun)
                    {
                        isComplex = true;

                        if (xobjRun.PositionOptions.VAlign.HasValue && !valign.HasValue)
                            valign = xobjRun.PositionOptions.VAlign.Value;
                    }
                    else if (run is PDFTextRunNewLine nl)
                    {
                        lastlineheight = nl.NewLineOffset.Height;
                    }
                    else if (run is PDFLayoutInlineBegin inlineBegin)
                    {
                        if (inlineBegin.PositionOptions.VAlign.HasValue)
                            valign = inlineBegin.PositionOptions.VAlign.Value;
                    }
                    currWidth += run.Width;
                }

             

                if (isComplex)
                {
                    if (!valign.HasValue) //default
                        valign = VerticalAlignment.Baseline;

                    totalHeight = maxHeight;

                    var baselineOffset = totalHeight - maxDescender;
                    

                    switch (valign.Value)
                    {
                        case VerticalAlignment.Top:
                            AlignBlocksFromTop(totalHeight, maxHeight, baselineOffset, lastlineheight, maxDescender);
                            break;
                        case VerticalAlignment.Middle:
                            AlignBlocksFromMiddle(totalHeight, maxHeight, baselineOffset, lastlineheight, maxDescender);
                            break;
                        case VerticalAlignment.Bottom:
                            AlignBlocksFromBottom(totalHeight, maxHeight, baselineOffset, lastlineheight, maxDescender);
                            break;
                        case VerticalAlignment.Baseline:
                        default:
                            AlignBlocksFromBaseline(totalHeight, maxHeight, baselineOffset, lastlineheight, maxDescender, maxFontSize);
                            break;
                    }
                    

                    //this.BaseLineOffset = baselineOffset;
                    this._totalHeight = totalHeight;
                    this.VAlignment = valign.Value;
                    //this.BaseLineOffset = baselineOffset;
                }

                else if (this.Runs.Count > 0 && this.Runs[0] is PDFTextRunSpacer && this.LineIndex > 0) // we are are probably a soft return
                {
                    var prevLine = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                    if (null != prevLine)
                    {
                        PDFTextRunNewLine prevReturn = prevLine.Runs[prevLine.Runs.Count - 1] as PDFTextRunNewLine;
                        if (null != prevReturn)
                        {
                            //we want to make sure the offset of the return is correct.
                            var baseline = this.BaseLineOffset;

                            if (baseline == Unit.Zero)
                                baseline = prevReturn.TextOptions.GetBaselineOffset();

                            var tobottom = prevLine.BaseLineToBottom;

                            var reqVOffset = baseline + tobottom;
                            if (reqVOffset > prevReturn.NewLineOffset.Height)
                            {
                                var size = prevReturn.NewLineOffset;
                                size.Height = reqVOffset;
                                prevReturn.NewLineOffset = size;
                            }
                        }
                    }
                }
                switch (valign)
                {
                    default:
                        break;
                }

            }

            //Check the line height of the previous line and move down if needed
            //PDFTextRunNewLine prev;
            //if (PrevLineIsTextReturn(out prev))
            //{
            //    if (prev.NewLineOffset.Height < this.Height)
            //        prev.NewLineOffset = new Size(prev.NewLineOffset.Width, this.Height);
            //
            //}


        }


        private void AlignBlocksFromTop(Unit totalHeight, Unit maxHeight, Unit baselineOffset, Unit lastLineHeight, Unit maxdescender)
        {
            //This is top

            foreach (var run in this.Runs)
            {
                if (run is PDFTextRunSpacer spacer && this.Runs.IndexOf(spacer) == 0 && this.LineIndex > 0) //first continuation line
                {
                    var prev = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                    var newLine = prev.LastRun() as PDFTextRunNewLine;
                    var size = newLine.NewLineOffset;

                    size.Height = prev.Height;
                    newLine.NewLineOffset = size;

                    //this.BaseLineOffset = newLine.TextOptions.GetBaselineOffset();

                }
                else if (run is PDFTextRunBegin begin)
                {
                    //Do nothing on a begin as this is the default line alignment based an leading
                    //without any inline blocks.
                    continue;
                }
                else if (run is PDFLayoutInlineBlockRun inline)
                {
                    //if (inline.Height < maxHeight)
                    //    inline.SetOffsetY(maxHeight - inline.Height);
                }
                else if (run is PDFTextRunNewLine nl)
                {
                    //Need to calculate the Baseline to bottom

                    //max ascent character
                    var ascent = nl.TextOptions.GetAscent();
                    //half the leading
                    var halflead = (nl.TextOptions.GetLineHeight() - nl.TextOptions.Font.Size) / 2.0;

                    //take half the leading and the max ascent from the total line height
                    var offset = totalHeight - (ascent + halflead);

                    //and this is the offset from the baseline to the bottom of the full line height
                    this.BaseLineToBottom = offset;
                }
            }
        }

        private void AlignBlocksFromMiddle(Unit totalHeight, Unit maxHeight, Unit baselineOffset, Unit lastLineHeight, Unit maxdescender)
        {
            

            foreach (var run in this.Runs)
            {
                if (run is PDFTextRunSpacer spacer && this.Runs.IndexOf(spacer) == 0 && this.LineIndex > 0) //first continuation line
                {
                    var prev = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                    var newLine = prev.LastRun() as PDFTextRunNewLine;

                    var lastDescender = newLine.TextOptions.GetDescender();
                    var lastHalfLead = (prev.Height - lastLineHeight) / 2;

                    var size = newLine.NewLineOffset;

                    var newHalfLead = (maxHeight - newLine.TextOptions.GetLineHeight()) / 2;

                    size.Height = (lastHalfLead + lastDescender  + baselineOffset - newHalfLead);
                    newLine.NewLineOffset = size;

                }
                else if (run is PDFTextRunBegin begin)
                {
                    
                    var h = begin.TextRenderOptions.GetLineHeight();
                    if (h < maxHeight)
                    {
                        var desc = begin.TextRenderOptions.GetDescender();
                        var halflead = (maxHeight - h) / 2.0;
                        
                        var offset = (h - desc) + halflead; //half the space + (the line height - the descender)
                        //h += 10;

                        begin.SetOffsetY(baselineOffset - offset); //move to the baseline
                    }

                }
                else if (run is PDFLayoutInlineBlockRun inline)
                {
                    Thickness margin;
                    if (inline.Height < maxHeight)
                    {
                        var halflead = (maxHeight - inline.Height) / 2;
                        inline.SetOffsetY(halflead);
                    }
                    //fix for margins being applied and pushing content up rather than down.
                    else if (inline.ContentHasMargins(out margin))
                        inline.SetOffsetY(inline.OffsetY + margin.Top); //Just the top for middle alignment
                }
                else if(run is PDFTextRunNewLine nl)
                {
                    //end of the middle spaced line - set the offset to this baseline to bottom

                    //max ascent character
                    var baseline = nl.TextOptions.GetBaselineOffset();
                    var descent = nl.TextOptions.GetDescender();
                    //half the leading
                    var halfThisLead = (maxHeight - nl.TextOptions.GetLineHeight()) / 2.0;
                    var halfNextLead = (nl.TextOptions.GetLineHeight() - nl.TextOptions.GetSize()) / 2.0;
                    //take half the leading of this line, the descender and the half the leading of a normal line (for the next one)

                    var offset = descent + halfThisLead + halfNextLead;

                    //and this is the offset from the baseline to the bottom of the full line height
                    this.BaseLineToBottom = offset;
                }
            }
        }

        private void AlignBlocksFromBaseline(Unit totalHeight, Unit maxHeight, Unit baselineOffset, Unit lastLineHeight, Unit maxdescender, Unit maxfont)
        {
            
            //This is baseline - if its max height, then do it from the bottom
            //otherwise move to the baseline.

            this._totalHeight += maxdescender;

            foreach (var run in this.Runs)
            {
                if (run is PDFTextRunSpacer spacer && this.Runs.IndexOf(spacer) == 0 && this.LineIndex > 0) //first continuation line
                {
                    var prev = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                    var newLine = prev.LastRun() as PDFTextRunNewLine;
                    var size = newLine.NewLineOffset;

                    size.Height = totalHeight;
                    newLine.NewLineOffset = size;

                }
                else if (run is PDFTextRunBegin begin)
                {
                    var h = begin.TextRenderOptions.GetLineHeight();
                    var desc = begin.TextRenderOptions.GetDescender();
                    var aboveBaseline = h - desc;
                    //h += 10;

                    begin.SetOffsetY(baselineOffset - aboveBaseline); //just move to the baseline

                }
                else if (run is PDFLayoutInlineBlockRun inline)
                {
                    Thickness margin;
                    if (inline.Height < maxHeight)
                    {
                        if (inline.Height < baselineOffset)
                        {
                            var halflead = (maxHeight - maxfont) / 2;
                            var baseline = halflead + maxfont - maxdescender;
                            var offset = baseline - inline.Height;

                            if (offset < 0)
                                offset = 0;

                            inline.SetOffsetY(offset);
                        }
                        //inline.SetOffsetY(maxHeight - inline.Height);
                    }
                    //fix for margins being applied and pushing content up rather than down.
                    else if (inline.ContentHasMargins(out margin))
                        inline.SetOffsetY(inline.OffsetY + margin.Top + margin.Bottom);
                }
                
            }
        }

        private void AlignBlocksFromBottom(Unit totalHeight, Unit maxHeight, Unit baselineOffset, Unit lastLineHeight, Unit maxdescender)
        {
            //This is bottom

            foreach (var run in this.Runs)
            {
                if (run is PDFTextRunSpacer spacer && this.Runs.IndexOf(spacer) == 0 && this.LineIndex > 0) //first continuation line
                {
                    var prev = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                    var newLine = prev.LastRun() as PDFTextRunNewLine;
                    var size = newLine.NewLineOffset;

                    size.Height = totalHeight;
                    newLine.NewLineOffset = size;

                }
                else if (run is PDFTextRunBegin begin)
                {
                    var h = begin.TextRenderOptions.GetLineHeight();
                    var desc = begin.TextRenderOptions.GetDescender();
                    var offset = h - desc;
                    //h += 10;

                    begin.SetOffsetY(baselineOffset - offset); //just move to the baseline

                }
                else if (run is PDFLayoutInlineBlockRun inline)
                {
                    Thickness margin;
                    if (inline.Height < maxHeight)
                    {
                        inline.SetOffsetY(maxHeight - inline.Height);
                    }
                    //fix for margins being applied and pushing content up rather than down.
                    else if (inline.ContentHasMargins(out margin))
                        inline.SetOffsetY(inline.OffsetY + margin.Top + margin.Bottom);
                }
            }
        }

        private bool PrevLineIsTextReturn(out PDFTextRunNewLine prev)
        {
            if (this.LineIndex > 0)
            {
                var prevLine = this.Region.Contents[this.LineIndex - 1] as PDFLayoutLine;
                if (null != prevLine && prevLine.Runs.Count > 0)
                {
                    if (prevLine.Runs[prevLine.Runs.Count - 1] is PDFTextRunNewLine newLine)
                    {
                        prev = newLine;
                        return true;
                    }
                }
            }
            prev = null;
            return false;
        }

        public Unit GetLastTextHeight(Unit defaultValue)
        {
            var value = defaultValue;
            if(this.Runs.Count > 0)
            {
                foreach (var run in this.Runs)
                {
                    if (run is PDFTextRunCharacter chars)
                        value = chars.Height;
                }
            }
            return value;
        }

        public PDFLayoutInlineBegin AddInlineRunStart(IPDFLayoutEngine engine, IComponent component, PDFPositionOptions posOptions, PDFTextRenderOptions textOptions, Style full)
        {
            PDFLayoutInlineBegin begin = new PDFLayoutInlineBegin(this, component, posOptions, textOptions, full);
            this.Runs.Add(begin);
            return begin;
        }

        public PDFLayoutInlineEnd AddInlineRunEnd(IPDFLayoutEngine engine, IComponent component, PDFLayoutInlineBegin start, PDFPositionOptions options)
        {
            PDFLayoutInlineEnd end = new PDFLayoutInlineEnd(this, start, component, options);
            this.Runs.Add(end);
            return end;
        }

        public PDFLayoutXObject AddXObjectRun(IPDFLayoutEngine engine, IComponent component, PDFLayoutRegion container, PDFPositionOptions options, Style full)
        {
            PDFLayoutXObject xobject = new PDFLayoutXObject(this, container, options, component);
            this.Runs.Add(xobject);
            return xobject;
        }

        public PDFLayoutMarkedContentBegin AddMarkedContentStart(IPDFLayoutEngine engine, IComponent component, PDFMarkedContentType type)
        {
            PDFLayoutMarkedContentBegin begin = new PDFLayoutMarkedContentBegin(this, component, type);
            this.Runs.Add(begin);
            return begin;
        }

        public PDFLayoutMarkedContentEnd AddMarkedContentEnd(IPDFLayoutEngine engine, PDFLayoutMarkedContentBegin start)
        {
            PDFLayoutMarkedContentEnd end = new PDFLayoutMarkedContentEnd(start);
            this.Runs.Add(end);
            return end;
        }


        #region public virtual PDFLayoutRun AddComponentRun(IPDFComponent comp,.... )

        /// <summary>
        /// Adds the component to the line using <i>all</i> the provided options
        /// </summary>
        /// <param name="comp">the component to add to the line</param>
        /// <param name="total">the total bounds of the component</param>
        /// <param name="border">the border rectangle wrt the total bounds</param>
        /// <param name="content">the content rectangle wrt the total bounds</param>
        /// <param name="baselineOffset">The required offset of the baseline of this line from it's top</param>
        /// <param name="options">the positioning options</param>
        /// <param name="style">the full style of the component</param>
        /// <returns>The created run</returns>
        public virtual PDFLayoutRun AddComponentRun(IComponent comp, Rect total, Rect border, 
                                            Rect content, Unit baselineOffset,
                                            PDFPositionOptions options, Style style)
        {
            PDFLayoutComponentRun comprun = new PDFLayoutComponentRun(this, comp, style);
            this.Runs.Add(comprun);

            total = total.Offset(this.Width, this.OffsetY);
            comprun.InitSize(total, border, content, options);
            
            comprun.Close();

            //Added 13th June 2016
            //this.BaseLineOffset = Unit.Max(this.BaseLineOffset, baselineOffset);
            
            return comprun;
        }


        #endregion

        public virtual PDFLayoutPositionedRegionRun AddPositionedRun(PDFLayoutPositionedRegion postioned, IComponent component)
        {
            PDFLayoutPositionedRegionRun run = new PDFLayoutPositionedRegionRun(postioned, this, component);
            postioned.AssociatedRun = run;

            this.Runs.Add(run);
            return run;
        }

        public virtual PDFLayoutInlineBlockRun AddInlineBlockRun(PDFLayoutPositionedRegion positioned, IComponent component)
        {
            PDFLayoutInlineBlockRun run = new PDFLayoutInlineBlockRun(positioned, this, component, positioned.PositionOptions);
            positioned.AssociatedRun = run;

            this.Runs.Add(run);
            return run;
        }

        /// <summary>
        /// Justs adds a run
        /// </summary>
        /// <param name="run"></param>
        public virtual void AddRun(PDFLayoutRun run)
        {
            if (run is PDFTextRunBegin begin)
            {
                var ascent = begin.TextRenderOptions.GetAscent();
                var descent = begin.TextRenderOptions.GetDescender();
                var line = begin.TextRenderOptions.GetLineHeight();
                var lead = line - (ascent + descent);
                var halfLead = lead / 2;

                var offset = halfLead + ascent;

                if (offset > this.BaseLineOffset || this.Runs.Count == 0)
                {
                    this.BaseLineOffset = offset;
                    this.BaseLineToBottom = halfLead + descent;
                }
            }
            else if (run is PDFTextRunEnd end)
            {
                begin = end.Start;

                var ascent = begin.TextRenderOptions.GetAscent();
                var descent = begin.TextRenderOptions.GetDescender();
                var line = begin.TextRenderOptions.GetLineHeight();
                var lead = line - (ascent + descent);
                var halfLead = lead / 2;

                var offset = halfLead + ascent;

                if (offset > this.BaseLineOffset)
                {
                    this.BaseLineOffset = offset;
                    this.BaseLineToBottom = halfLead + descent;
                }
            }
            this.Runs.Add(run);
        }

        /// <summary>
        /// Overides the base (empty) implementation so that the FullWidth is set correctly
        /// </summary>
        /// <param name="width"></param>
        public override void SetMaxWidth(Unit width)
        {
            if (this.FullWidth > width)
                this.FullWidth = width;
        }

        public virtual void SetOffset(Unit x, Unit y)
        {
            this._xoffset = x;
            this._yoffset = y;
        }

        /// <summary>
        /// Overrides the default behaviour to enumerate over the line contents
        /// </summary>
        /// <param name="context"></param>
        protected override void DoPushComponentLayout(PDFLayoutContext context, int pageindex, Unit xoffset, Unit yoffset)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Debug, "Layout Line", "Pushing component layout onto runs in the line " + this.ToString());


            foreach (PDFLayoutRun run in this.Runs)
            {
                run.PushComponentLayout(context, pageindex, xoffset, yoffset);
            }

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Debug, "Layout Line", "Pushed all the component layouts onto the runs in the line " + this.ToString());
        }

        private PDFLayoutRun GetFirstSignificantLinetRun()
        {
            if (this.Runs.Count < 1)
                return null;
            for (var i = 0; i < this.Runs.Count; i++)
            {
                var run = this.Runs[i];
                if (run is PDFTextRunBegin || run is PDFTextRunSpacer)
                    return run;
                
            }

            return null;
        }

        internal bool RightAlignContent(Unit totalWidth, Unit currentWidth, Unit availableSpace, Unit leftInset, Unit rightInset, List<PDFTextRunCharacter> runCache, PDFLayoutContext context)
        {
            var pushOffset = this.AvailableWidth;
            var pushedRight = false;
            if (this.LineIndex == 0  || (this.Runs.Count > 0 && this.Runs[0] is PDFTextRunBegin))
            {
                for (var i = 0; i < this.Runs.Count; i++)
                {
                    PDFLayoutRun run = this.Runs[i];
                    if (run is PDFTextRunBegin begin)
                    {
                        begin.LineInset += pushOffset;
                        this.RightInset = pushOffset;
                        pushedRight = true;
                    }
                }
            }
            else
            {
                var index  = this.LineIndex;
                var prev = this.Region.Contents[index -1 ] as PDFLayoutLine;
                
                if (null == prev || prev.LineIndex != index - 1)
                {
                    index = this.Region.Contents.IndexOf(this);
                    prev = this.Region.Contents[index - 1] as PDFLayoutLine;
                }

                if (null != prev && prev.HAlignment == HorizontalAlignment.Right)
                {
                    var prevPushOffset = prev.RightInset;
                    
                    var last = prev.Runs[prev.Runs.Count - 1] as PDFTextRunNewLine;
                    if (null != last)
                    {
                        var offset = last.NewLineOffset;
                        offset.Width -= (pushOffset - prevPushOffset);
                        this.RightInset = pushOffset;
                        last.NewLineOffset = offset;
                        pushedRight = true;
                    }
                }

                if (this.Runs.Count > 3)
                {
                    //complex line - shift any inner begin runs to the right as well
                    foreach (var run in this.Runs)
                    {
                        if (run is PDFTextRunBegin begin)
                        {
                            begin.LineInset += pushOffset;
                            pushedRight = true;
                        }
                    }
                }
            }
            return pushedRight;
        }
        
        internal bool RightAlignContent_old(Unit totalWidth, Unit currentWidth, Unit availableSpace, Unit leftInset, Unit prevLeftInset, Unit rightInset,
            List<PDFTextRunCharacter> runCache, PDFLayoutContext context)
        {

            Unit offset;
            var updated = false;
            if (this.Runs.Count == 3)
            {
                //We are a simple line
                PDFLayoutRun run = this.Runs[0];
                if (run is PDFTextRunBegin begin)
                {
                    begin.LineInset += availableSpace - rightInset;
                    this.RightInset = rightInset;
                }
                else if (run is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer && this.LineIndex > 0)
                {
                    var prev = (PDFLayoutLine)this.Region.Contents[this.LineIndex - 1];
                    var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                    var prevWidth = totalWidth - prev.Width;
                    var newWidth = totalWidth - currentWidth;
                    offset = newWidth - prevWidth;
                    var newoffset = last.NewLineOffset;
                    newoffset.Width -= offset - (rightInset - prev.RightInset) + (prevLeftInset - leftInset);
                    this.RightInset = rightInset;
                    last.NewLineOffset = newoffset;
                    updated = true;
                }
            }
            else if(this.Runs.Count > 0)
            {
                
                for (var i = 0; i < this.Runs.Count; i++)
                {
                    PDFLayoutRun run = this.Runs[i];
                    if (run is PDFTextRunBegin begin)
                    {
                        begin.LineInset += availableSpace - rightInset;
                        this.RightInset = rightInset;
                    }
                    else if (run is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer && this.LineIndex > 0)
                    {
                        var prev = (PDFLayoutLine)this.Region.Contents[this.LineIndex - 1];
                        var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                        var prevWidth = totalWidth - prev.Width;
                        var newWidth = totalWidth - currentWidth;
                        offset = newWidth - prevWidth;
                        var newoffset = last.NewLineOffset;
                        newoffset.Width -= offset - (rightInset - prev.RightInset);
                        last.NewLineOffset = newoffset;
                        this.RightInset = rightInset;
                        updated = true;
                    }
                }
            }
            
            
            return updated;
        }
        
        internal bool CenterAlignContent(Unit totalWidth, Unit currentWidth, Unit availableSpace, Unit leftInset, Unit rightInset, List<PDFTextRunCharacter> runCache, PDFLayoutContext context)
        {
            var pushOffset = this.AvailableWidth / 2;
            var pushedCenter = false;
            if (this.LineIndex == 0 || (this.Runs.Count > 0 && this.Runs[0] is PDFTextRunBegin))
            {
                for (var i = 0; i < this.Runs.Count; i++)
                {
                    PDFLayoutRun run = this.Runs[i];
                    if (run is PDFTextRunBegin begin)
                    {
                        begin.LineInset += pushOffset;
                        this.RightInset = pushOffset;
                        pushedCenter = true;
                    }
                }
            }
            else
            {
                var index  = this.LineIndex;
                var prev = this.Region.Contents[index -1 ] as PDFLayoutLine;
                
                if (null == prev || prev.LineIndex != index - 1)
                {
                    index = this.Region.Contents.IndexOf(this);
                    prev = this.Region.Contents[index - 1] as PDFLayoutLine;
                }

                if (null != prev && prev.HAlignment == HorizontalAlignment.Center)
                {
                    var prevPushOffset = prev.RightInset;
                    
                    var last = prev.Runs[prev.Runs.Count - 1] as PDFTextRunNewLine;
                    if (null != last)
                    {
                        var offset = last.NewLineOffset;
                        offset.Width -= (pushOffset - prevPushOffset);
                        this.RightInset = pushOffset;
                        last.NewLineOffset = offset;
                        pushedCenter = true;
                    }
                }

                if (this.Runs.Count > 3)
                {
                    //complex line - shift any inner begin runs to the right as well
                    foreach (var run in this.Runs)
                    {
                        if (run is PDFTextRunBegin begin)
                        {
                            begin.LineInset += pushOffset;
                            pushedCenter = true;
                        }
                    }
                }
            }
            return pushedCenter;
        }
        internal bool CenterAlignContent_old(Unit totalWidth, Unit currentWidth, Unit availableSpace, Unit rightInset,
            List<PDFTextRunCharacter> runCache, PDFLayoutContext context)
        {
            Unit offset;
            var updated = false;
            if (this.Runs.Count == 3)
            {
                //We are a simple line
                PDFLayoutRun run = this.Runs[0];
                if (run is PDFTextRunBegin begin)
                {
                    begin.LineInset += (availableSpace - rightInset) / 2;
                    this.RightInset = rightInset / 2;
                }
                else if (run is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer && this.LineIndex > 0)
                {
                    var prev = (PDFLayoutLine)this.Region.Contents[this.LineIndex - 1];
                    var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                    var prevWidth = totalWidth - prev.Width;
                    var newWidth = totalWidth - currentWidth;
                    offset = (newWidth - prevWidth) / 2;
                    var newoffset = last.NewLineOffset;
                    newoffset.Width -= offset - (rightInset - prev.RightInset);
                    this.RightInset = rightInset / 2;
                    last.NewLineOffset = newoffset;
                    updated = true;
                }
            }
            else if(this.Runs.Count > 0)
            {
                
                for (var i = 0; i < this.Runs.Count; i++)
                {
                    PDFLayoutRun run = this.Runs[i];
                    if (run is PDFTextRunBegin begin)
                    {
                        begin.LineInset += (availableSpace - rightInset) / 2;
                        this.RightInset = rightInset / 2;
                    }
                    else if (run is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer && this.LineIndex > 0)
                    {
                        var prev = (PDFLayoutLine)this.Region.Contents[this.LineIndex - 1];
                        var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                        var prevWidth = totalWidth - prev.Width;
                        var newWidth = totalWidth - currentWidth;
                        offset = (newWidth - prevWidth) / 2;
                        var newoffset = last.NewLineOffset;
                        newoffset.Width -= offset - (rightInset - prev.RightInset);
                        last.NewLineOffset = newoffset;
                        this.RightInset = rightInset / 2;
                        updated = true;
                    }
                }
            }
            
            
            return updated;
        }

        internal bool JustifyContent(Unit total, Unit current, Unit available, bool all, List<PDFTextRunCharacter> runCache, PDFLayoutContext context, ref PDFTextRenderOptions currOptions)
        {
            if(this.Runs.Count < 1)
                return false;

            bool shouldJustify = all;

            PDFLayoutRun last = this.Runs[this.Runs.Count - 1];
            if (last is PDFTextRunNewLine && (last as PDFTextRunNewLine).IsHardReturn == false)
                shouldJustify = true;


            if(shouldJustify)
            {
                runCache.Clear();
                bool intext = (null != currOptions); //if we have text render options then even if we are the first run we can be considered as inside a text block
                int charCount = 0;
                int spaceCount = 0;
                PDFTextRunCharacter lastchars = null;

                for (int i = 0; i < this.Runs.Count; i++)
                {
                    PDFLayoutRun cur = this.Runs[i];

                    if (cur is PDFTextRunBegin begin)
                    {
                        currOptions = begin.TextRenderOptions;
                        if (!intext)
                            intext = true;
                    }
                    else if (cur is PDFTextRunCharacter chars && intext)
                    {
                        if (!(currOptions.WordSpacing.HasValue || currOptions.CharacterSpacing.HasValue))
                        {
                            AddCharactersAndSpaces(chars.Characters, ref charCount, ref spaceCount);

                            lastchars = chars;
                        }
                    }
                    else if (cur is PDFLayoutComponentRun || cur is PDFLayoutInlineBlockRun)
                        lastchars = null;
                }

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, LOG_CATEGORY, "Counted " + charCount + " characters and " + spaceCount + " spaces on line " + this.LineIndex);

                
                // Post process to calculate the required spacing
                // if we have some text in our line.

                if (intext && (spaceCount + charCount > 0))
                {
                    if (null != lastchars && lastchars.Characters.EndsWith(" "))
                    {
                        lastchars.Characters = lastchars.Characters.Substring(0, lastchars.Characters.Length - 1);
                        spaceCount -= 1;
                    }
                    int totalCharCount = charCount;
                    int totalSpaceCount = spaceCount;

                    //reset the running totals for widths.
                    charCount = 0;
                    spaceCount = 0;
                    Unit currWidth = 0;
                    Unit change = Unit.Zero;

                    for (int i = 0; i < this.Runs.Count; i++)
                    {
                        PDFLayoutRun cur = this.Runs[i];
                        if (cur is PDFTextRunBegin begin)
                        {
                            currOptions = begin.TextRenderOptions; //Set the options to be used on this line and any following lines for this textual content

                            this._linespacingOptions = MeasureLineSpaces(currOptions, totalCharCount, totalSpaceCount, total, current, available, context);
                            if (i > 0)
                            {
                                change = (_linespacingOptions.WordSpace * spaceCount) + (_linespacingOptions.CharSpace * charCount);
                                begin.LineInset += change;
                            }
                        }
                        else if (cur is PDFTextRunCharacter chars)
                        {
                            if (null == currOptions)
                                throw new InvalidOperationException("Cannot justify contents of characters without having a TextBegin run beforehand, either on this line or a previous line");
                            if (null == this._linespacingOptions)
                                this._linespacingOptions = MeasureLineSpaces(currOptions, totalCharCount, totalSpaceCount, total, current, available, context);

                            if (!currOptions.WordSpacing.HasValue || !currOptions.CharacterSpacing.HasValue)
                            {
                                int runChars = 0;
                                int runSpaces = 0;
                                AddCharactersAndSpaces(chars.Characters, ref runChars, ref runSpaces);
                                charCount += runChars;
                                spaceCount += runSpaces;
                                chars.ExtraSpace = (_linespacingOptions.WordSpace * runSpaces) + (_linespacingOptions.CharSpace * runChars);
                            }
                        }
                        else if (cur is PDFLayoutComponentRun comprun)
                        {
                            if (i > 0)
                            {
                                Rect bounds = comprun.TotalBounds;
                                bounds.X += (_linespacingOptions.WordSpace * spaceCount) + (_linespacingOptions.CharSpace * charCount);
                                comprun.TotalBounds = bounds;
                            }
                        }
                        else if (cur is PDFLayoutInlineBlockRun blockrun)
                        {
                            if (i > 0)
                            {
                                Rect bounds = blockrun.Region.TotalBounds;
                                bounds.X += (_linespacingOptions.WordSpace * spaceCount) + (_linespacingOptions.CharSpace * charCount);
                                blockrun.Region.TotalBounds = bounds;
                            }
                        }
                        else if (cur is PDFLayoutXObject xobj)
                        {
                            
                        }
                        else if (cur is PDFTextRunNewLine newLine)
                        {
                            newLine.NewLineOffset = new Size(newLine.NewLineOffset.Width + change, newLine.NewLineOffset.Height);
                        }
                    }


                }

                
            }

            return shouldJustify;
        }

        private ExtraSpacingOptions MeasureLineSpaces(PDFTextRenderOptions currOptions, int charCount, int spaceCount, Unit totalWidth, Unit currentWidth, Unit available, PDFLayoutContext context)
        {
            int fitted = 0;
            Size spaceSize = currOptions.Font.Resource.Definition.MeasureStringWidth(" ", 0, currOptions.Font.Size.PointsValue, 2000.0, true, out fitted);

            Unit extraSpaceSpace = available.PointsValue / spaceCount;
            Unit newSpaceSize = spaceSize.Width + extraSpaceSpace;
            
            double spacesFactor = newSpaceSize.PointsValue - spaceSize.Width.PointsValue;

            

            return new ExtraSpacingOptions() { CharSpace = 0.0, WordSpace = spacesFactor, Options = currOptions, SpaceWidth = spaceSize.Width };
        }

        private ExtraSpacingOptions _linespacingOptions;

        
        public ExtraSpacingOptions LineSpacingOptions
        {
            get { return _linespacingOptions; }
        }

        public class ExtraSpacingOptions
        {
            public Unit WordSpace;
            public Unit CharSpace;
            public Unit SpaceWidth;

            public PDFTextRenderOptions Options;

            public override string ToString()
            {
                return "Word space: " + WordSpace.ToString() + ", Char space: " + CharSpace.ToString() + ", Font Size: " + Options.Font.Size.ToString();
            }
        }

        public void ResetJustifySpacing(PDFTextRenderOptions options)
        {
            _linespacingOptions = new ExtraSpacingOptions() { WordSpace = Unit.Zero, CharSpace = Unit.Zero, Options = options };
        }

        private void AddCharactersAndSpaces(string chars, ref int charCount, ref int spaceCount)
        {
            int spaces = 0;
            int letters = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsWhiteSpace(chars, i))
                    spaces++;
                else if (char.IsControl(chars, i) == false)
                    letters++;
            }
            charCount += letters;
            spaceCount += spaces;
        }

        private bool IsSpecialTextAlignmentCase()
        {
            if (this.VAlignment == VerticalAlignment.Top)
                return false;
            if (this.IsEmpty == true || this.Runs.Count < 2)
                return false;
            else
            {
                return true;
            }
        }

        protected override Native.PDFObjectRef DoOutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            bool first = true;
            
            foreach (PDFLayoutRun run in this.Runs)
            {

                if (null != _linespacingOptions)
                {
                    if (run is PDFTextRunBegin)
                    {
                        (run as PDFTextRunBegin).SetTextSpacing(this._linespacingOptions.WordSpace, this._linespacingOptions.CharSpace);
                    }
                    else if (run is PDFTextRunSpacer)
                        continue;

                    else if (first)
                        context.Graphics.SetTextSpacing(this._linespacingOptions.WordSpace, this._linespacingOptions.CharSpace, this._linespacingOptions.Options.Font.Size);
                }
                
                run.OutputToPDF(context, writer);

                first = false;
                
            }
            
            return base.DoOutputToPDF(context, writer);
        }
    }
}
