﻿#define RemoveTrailingSpaces
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


        #region public bool HasInlineContent {get;}

        /// <summary>
        /// Returns true if this is an empty line - does not have any runs on it
        /// </summary>
        public bool HasInlineContent
        {
            get { return this._runs != null && this._runs.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Gets or sets any right inset (from a float:right) that was applied to the newline offset.
        /// </summary>
        public Unit RightInset { get; set; }

        /// <summary>
        /// Gets or sets any left inset (from a float:left) that was applied to a text run begin
        /// </summary>
        public Unit LeftInset
        {
            get; 
            set;
        }
        
        
        protected Unit? ExtraCharacterSpace { get; set; }

        protected Unit? ExtraWordSpace { get; set; }
        
        /// <summary>
        /// Gets any extra space added to the line width (usually by justification of the content)
        /// </summary>
        public Unit? ExtraSpace { get; protected set; }

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
            if (null != last)
            {
                if (last.IsClosed == false)
                    last.Close();
#if RemoveTrailingSpaces
                
                var offset = 2;
                if (last is PDFLayoutInlineEnd)
                {
                    if (this.Runs.Count > 2)
                    {
                        last = this.Runs[this.Runs.Count - 2];
                        offset++;
                    }
                    else
                        last = null;
                }
                
                if (last is PDFTextRunEnd end && this.Runs.Count > offset)
                {
                    var prev = this.Runs[this.Runs.Count - offset];
                    //Remove a trailing whitespace from the character
                    if (prev is PDFTextRunCharacter chars)
                    {
                        if (Char.IsWhiteSpace(chars.Characters, chars.Characters.Length - 1))
                        {
                            var origLen = chars.Characters.Length;
                            chars.Characters = chars.Characters.TrimEnd();
                            var newLen = chars.Characters.Length;
                            var space = end.Start.CharSpace; 
                            //TODO: Re-measure the width after the space removal Alter the width of the line by the size of a ' ';
                            space *= (origLen - newLen);
                            chars.SetMaxWidth(chars.Width - space);
                        }
                    }

                }
            }
#endif
            
            if (!this.Parent.IsExplicitLayout)
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


        /// <summary>
        /// Repositions the runs on the line to ensure all baselines are matched
        /// </summary>
        /// <param name="forceUpdate"></param>
        private void EnsureAllRunsOnSameLevel(bool forceUpdate = false)
        {
            Unit totalHeight = Unit.Zero;
            Unit maxHeight = Unit.Zero;
            Unit maxDescender = Unit.Zero;
            Unit lastlineheight = Unit.Zero;
            Unit maxFontSize = Unit.Zero;
            Unit maxBaselineComponent = Unit.Zero;
            bool hasBaseLineComponents = false;
            Unit maxLeading = Unit.Zero;
            bool isComplex = false;


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
                    maxLeading = Unit.Max(maxLeading, begin.TextRenderOptions.GetLineHeight());
                }
                else if (run is PDFLayoutInlineBlockRun blockRun)
                {
                    //We have inline blocks to align.
                    isComplex = true;

                    var rect = blockRun.Region.TotalBounds;
                    rect.X = currWidth;
                    blockRun.Region.TotalBounds = rect;

                    if (blockRun.PositionOptions.VAlign.HasValue)
                        valign = blockRun.PositionOptions.VAlign.Value;

                    //pick the first vertical alignment for the line.
                    if (!blockRun.PositionOptions.VAlign.HasValue ||
                        blockRun.PositionOptions.VAlign.Value == VerticalAlignment.Baseline)
                    {
                        itemH = blockRun.Height + maxDescender;

                        if (itemH > maxHeight)
                            maxHeight = itemH;
                        
                        maxBaselineComponent = Unit.Max(maxBaselineComponent, blockRun.Height);
                    }
                    else if (rect.Height > maxHeight)
                    {
                        maxHeight = rect.Height;
                    }
                }
                else if (run is PDFLayoutComponentRun compRun)
                {
                    isComplex = true;

                    //any component valign is for the component, not the outer line.
                    if (compRun.PositionOptions.VAlign.HasValue) // && !valign.HasValue)
                        valign = compRun.PositionOptions.VAlign.Value;

                    if (!compRun.PositionOptions.VAlign.HasValue ||
                        compRun.PositionOptions.VAlign.Value == VerticalAlignment.Baseline)
                    {
                        itemH = compRun.Height + maxDescender;

                        if (itemH > maxHeight)
                        {
                            maxHeight = itemH;
                        }

                        maxBaselineComponent = Unit.Max(maxBaselineComponent, compRun.Height);
                    }
                }
                else if (run is PDFLayoutXObject xobjRun)
                {
                    isComplex = true;

                    if (xobjRun.PositionOptions.VAlign.HasValue) // && !valign.HasValue)
                        valign = xobjRun.PositionOptions.VAlign.Value;
                }
                else if (run is PDFLayoutPositionedRegionRun posRun)
                {
                    isComplex = true;

                    if (posRun.IsFloating == false && posRun.Region.VAlignment == VerticalAlignment.Baseline &&
                        (posRun.Region.PositionMode == PositionMode.Relative ||
                         posRun.Region.PositionMode == PositionMode.Static))
                    {
                        //affects the line height, so adjust
                        itemH = (posRun.Height + 40) + maxDescender;

                        if (itemH > maxHeight)
                        {
                            maxHeight = itemH;
                        }
                        
                        maxBaselineComponent = Unit.Max(maxBaselineComponent, posRun.Height);
                        
                    }
                }
                else if (run is PDFTextRunNewLine nl)
                {
                    lastlineheight = nl.NewLineOffset.Height;
                }
                else if (run is PDFLayoutInlineBegin inlineBegin)
                {
                    //if (inlineBegin.PositionOptions.VAlign.HasValue)
                    //    valign = inlineBegin.PositionOptions.VAlign.Value;
                }

                currWidth += run.Width;
            }



            if (isComplex || forceUpdate)
            {
                if (!valign.HasValue) //default
                    valign = VerticalAlignment.Baseline;

                totalHeight = maxHeight;
                var leadSpace = (maxLeading - maxFontSize) / 2;
                
                var addLead = false;
                if (maxBaselineComponent + maxDescender > totalHeight)
                {
                    totalHeight = maxBaselineComponent + maxDescender;
                }

                if (maxBaselineComponent > maxLeading)
                {
                    //we have components that push the baseline downn
                    hasBaseLineComponents = true;
                }

                var baselineOffset = totalHeight - (maxDescender + leadSpace);
                


                switch (valign.Value)
                {
                    case VerticalAlignment.Middle:
                        if (totalHeight > maxLeading)
                        {
                            baselineOffset = totalHeight / 2;
                        }

                        //if (mid < baselineOffset) //we have available space to lift the base line up 
                        //    baselineOffset = mid;
                        break;
                    case VerticalAlignment.Top:
                        baselineOffset = ((maxLeading - maxFontSize) / 2) + maxFontSize - maxDescender;
                        break;
                    case VerticalAlignment.Bottom:
                        //baselineOffset -= (maxLeading - maxFontSize) / 2;
                        break;
                    case VerticalAlignment.Baseline:
                        
                    default:
                        break;
                }

                if (baselineOffset < maxBaselineComponent)
                {
                    baselineOffset = maxBaselineComponent;
                    totalHeight = Unit.Max(totalHeight,
                        baselineOffset + maxDescender); // + ((maxLeading - maxFontSize) / 2));
                }

                AlignBlocksFromBaseline(totalHeight, maxBaselineComponent, baselineOffset, lastlineheight, maxDescender,
                    maxFontSize, hasBaseLineComponents, out addLead);

                //baseline aligned, so add the half leading to the bottom
                if (addLead)
                {
                    totalHeight += leadSpace;
                }
                
                //this.BaseLineOffset = baselineOffset;
                this._totalHeight = totalHeight;
                this.VAlignment = valign.Value;
                //this.BaseLineOffset = baselineOffset;
            }

            else if (this.Runs.Count > 0 && this.Runs[0] is PDFTextRunSpacer &&
                     this.LineIndex > 0) // we are are probably a soft return
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
            
            
        }


        /// <summary>
        /// Alignes all the individual runs on a line to their requested vertical position.
        /// </summary>
        /// <param name="totalHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="baselineOffset"></param>
        /// <param name="lastLineHeight"></param>
        /// <param name="maxdescender"></param>
        /// <param name="maxfont"></param>
        /// <param name="addLead">Set to true if the maxfont is a baseline aligned text run so we have ultimately a total height increased by half the leading, should be applied by the caller</param>
        private void AlignBlocksFromBaseline(Unit totalHeight, Unit maxHeight, Unit baselineOffset,
            Unit lastLineHeight, Unit maxdescender, Unit maxfont, bool hasBaseLineComponents, out bool addLead)
        {
            addLead = false;
            this.BaseLineOffset = baselineOffset;
            this.BaseLineToBottom = totalHeight - baselineOffset;
            //Support a single alignments rather than doing stacks for each line.
            VerticalAlignment? explicitAlign = null;
            
            foreach (var run in this.Runs)
            {

                if (run is PDFTextRunBegin begin)
                {
                    if (explicitAlign.HasValue)
                    {
                        switch (explicitAlign.Value)
                        {
                            case(VerticalAlignment.Top):
                                var lh = begin.TextRenderOptions.GetLineHeight();
                                var lead = lh - begin.TextRenderOptions.GetSize();
                                var baseline = begin.TextRenderOptions.GetBaselineOffset();
                                begin.SetOffsetY(baseline - baselineOffset);
                                this.BaseLineToBottom = lead + baselineOffset - baseline;
                                addLead = false;
                                break;
                            case(VerticalAlignment.Middle):
                                var halfh = ((totalHeight - begin.TextRenderOptions.GetLineHeight()) / 2) ;
                                begin.SetOffsetY(begin.TextRenderOptions.GetDescender() - halfh);
                                
                                this.BaseLineToBottom = halfh;
                                addLead = false;
                                break;
                            case(VerticalAlignment.Bottom) :
                                this.BaseLineToBottom =
                                    (begin.TextRenderOptions.GetLineHeight() - begin.TextRenderOptions.GetSize()) / 2 +
                                    begin.TextRenderOptions.GetDescender();
                                addLead = addLead || hasBaseLineComponents;
                                break;
                            case VerticalAlignment.Baseline:
                                if (begin.TextRenderOptions.GetSize() == maxfont && hasBaseLineComponents)
                                    addLead = true;
                                break;
                            default:
                                break;
                        }
                    }
                    else if (begin.TextRenderOptions.GetSize() == maxfont && hasBaseLineComponents)
                        addLead = true; //we are baselined
                }
                else if (run is PDFTextRunSpacer spacer && spacer.IsNewLineSpacer == true)
                {
                    if (explicitAlign.HasValue)
                    {
                        ;
                    }
                    else if(this.LineIndex > 0)
                    {
                        var prev = spacer.PreviousNewLine;
                        var offset = prev.NewLineOffset;
                        var btob = prev.Line.BaseLineToBottom;

                        // if (hasBaseLineComponents)
                        //     btob += ((prev.TextOptions.GetLineHeight() - prev.TextOptions.GetSize()) / 2) +
                        //             prev.TextOptions.GetDescender();
                        //
                        if (offset.Height < btob + baselineOffset)
                        {
                            offset.Height = btob + baselineOffset ;
                            prev.NewLineOffset = offset;
                        }
                    }
                }
                else if (run is PDFLayoutComponentRun componentRun)
                {
                    var valign = componentRun.PositionOptions.VAlign ?? (explicitAlign ?? VerticalAlignment.Baseline);
                    Unit top;
                    switch (valign)
                    {
                        case VerticalAlignment.Top:
                            top = 0;
                            componentRun.SetOffsetY(top);
                            break;
                        case VerticalAlignment.Middle:
                            if (componentRun.Height >= totalHeight)
                            {
                                componentRun.SetOffsetY(0);
                            }
                            else
                            {
                                top = (totalHeight - componentRun.Height);
                                top /= 2.0;
                                componentRun.SetOffsetY(top);
                            }
                            break;
                        case VerticalAlignment.Bottom:
                            top = totalHeight - componentRun.Height;
                            componentRun.SetOffsetY(top);
                            break;
                        case (VerticalAlignment.Baseline):
                        default:
                            top = baselineOffset;
                            top -= componentRun.Height;
                            componentRun.SetOffsetY(top);
                            if (componentRun.Height > maxfont)
                                hasBaseLineComponents = true;
                            break;
                    }
                }
                else if (run is PDFLayoutInlineBlockRun ibRun)
                {
                    Unit offset;
                    var valign = ibRun.PositionOptions.VAlign ?? (explicitAlign ?? VerticalAlignment.Baseline);
                    switch (valign)
                    {
                        case(VerticalAlignment.Bottom):
                            offset = totalHeight;
                            offset -= ibRun.Height;
                            ibRun.SetOffsetY(offset);
                            break;
                        case(VerticalAlignment.Top):
                            ibRun.SetOffsetY(0);
                            break;
                        case(VerticalAlignment.Middle):
                            if (ibRun.Height >= totalHeight)
                            {
                                ibRun.SetOffsetY(0);
                            }
                            else
                            {
                                offset = (totalHeight - ibRun.Height);
                                offset /= 2.0;
                                ibRun.SetOffsetY(offset);
                            }
                            break;
                        case (VerticalAlignment.Baseline):
                        default:
                            offset = baselineOffset;
                            offset -= ibRun.Height;
                            ibRun.SetOffsetY(offset);
                            break;
                    }
                }
                else if (run is PDFLayoutInlineBegin inlineBegin)
                {
                    if (inlineBegin.PositionOptions.VAlign.HasValue)
                    {
                        explicitAlign = inlineBegin.PositionOptions.VAlign.Value;
                    }
                }
                else if (run is PDFLayoutInlineEnd)
                {
                    explicitAlign = null;
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
            PDFLayoutPositionedRegionRun run = new PDFLayoutPositionedRegionRun(postioned, this, component, postioned.PositionOptions);
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

        public virtual void RemoveRun(PDFLayoutRun run)
        {
            if (this.Runs.Remove(run))
            {
                string msg = String.Empty;
                if (this.IsClosed)
                    this.EnsureAllRunsOnSameLevel(forceUpdate: true);
            }
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
            var pushOffset = availableSpace;
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
                    else if (run is PDFLayoutComponentRun comp)
                    {
                        comp.SetOffsetX( comp.OffsetX + pushOffset);
                        pushedRight = true;
                    }
                    else if (run is PDFLayoutInlineBlockRun ib)
                    {
                        ib.SetOffsetX(ib.OffsetX + pushOffset);
                    }
                    else if (run is PDFLayoutPositionedRegionRun posRun)
                    {
                        
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
                        Size offset;
                        if (last.IsHardReturn)
                        {
                            offset = new Size(this.Width - prev.Width, prev.Height);
                            this.RightInset = pushOffset; // offset.Width;
                            last.NewLineOffset = offset;
                        }
                        else
                        {
                            offset = last.NewLineOffset;
                            offset.Width -= (pushOffset - prevPushOffset);
                            this.RightInset = pushOffset;
                            last.NewLineOffset = offset;
                            pushedRight = true;
                        }
                    }
                }

                foreach (var run in this.Runs)
                {
                    if (run is PDFTextRunBegin begin)
                    {
                        begin.LineInset += pushOffset;
                        pushedRight = true;
                    }
                    else if (run is PDFLayoutComponentRun comp)
                    {
                        comp.SetOffsetX( comp.OffsetX + pushOffset);
                    }
                    else if (run is PDFLayoutInlineBlockRun ib)
                    {
                        ib.SetOffsetX(ib.OffsetX + pushOffset);
                    }
                    else if (run is PDFLayoutPositionedRegionRun posRun)
                    {
                        
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
            var pushOffset = availableSpace / 2;
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
                    else if (run is PDFLayoutComponentRun comp)
                    {
                        comp.SetOffsetX( comp.OffsetX + pushOffset);
                        pushedCenter = true;
                    }
                    else if (run is PDFLayoutInlineBlockRun ib)
                    {
                        ib.SetOffsetX(ib.OffsetX + pushOffset);
                        pushedCenter = true;
                    }
                    else if (run is PDFLayoutPositionedRegionRun posRun)
                    {
                        
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
                        else if (run is PDFLayoutComponentRun comp)
                        {
                            comp.SetOffsetX( comp.OffsetX + pushOffset);
                        }
                        else if (run is PDFLayoutInlineBlockRun ib)
                        {
                            ib.SetOffsetX(ib.OffsetX + pushOffset);
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
            if (last is PDFTextRunNewLine && (last as PDFTextRunNewLine).IsHardReturn == false) //TODO: Support justify-all
                shouldJustify = true;


            if(shouldJustify)
            {
                runCache.Clear();
                bool intext = (null != currOptions); //if we have text render options then even if we are the first run we can be considered as inside a text block
                int charCount = 0;
                int spaceCount = 0;
                PDFTextRunCharacter lastchars = null;
                Unit lineExtra = Unit.Zero;

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
                                lineExtra += chars.ExtraSpace;
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

                if (spaceCount > 0)
                    this.ExtraSpace = lineExtra;
                else if (this.LineSpacingOptions != null)
                    this.ExtraSpace = this.LineSpacingOptions.WordSpace;
            }

            return shouldJustify;
        }

        private ExtraSpacingOptions MeasureLineSpaces(PDFTextRenderOptions currOptions, int charCount, int spaceCount, Unit totalWidth, Unit currentWidth, Unit available, PDFLayoutContext context)
        {
            int fitted = 0;
            if (spaceCount == 0)
            {
                return new ExtraSpacingOptions()
                    { CharSpace = 0.0, WordSpace = totalWidth - currentWidth, Options = currOptions, SpaceWidth = totalWidth - currentWidth };
            }
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
            if (this.HasInlineContent == false || this.Runs.Count < 2)
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
