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
using Scryber.Text;
using Scryber.PDF.Resources;
using Scryber.Drawing;
using Scryber.Components;

namespace Scryber.PDF.Layout
{
    /// <summary>
    /// Layout engine for text.
    /// </summary>
    public class LayoutEngineText : IPDFLayoutEngine
    {

        private const string LOG_CATEGORY = "Text Layout Engine";

        //
        // properties
        //

        #region public PDFLayoutContext Context{ get;}

        private PDFLayoutContext _ctx;

        /// <summary>
        /// Gets the current layout context
        /// </summary>
        public PDFLayoutContext Context
        {
            get { return this._ctx; }
        }

        #endregion

        #region public IPDFTextComponent TextComponent {get;}

        private ITextComponent _txt;

        /// <summary>
        /// Gets the text component that this engine is laying out the text for
        /// </summary>
        public ITextComponent TextComponent
        {
            get { return _txt; }
        }

        #endregion

        #region PDFFontResource FontRsrc {get;}

        private PDFFontResource _frsrc;

        /// <summary>
        /// Gets the font resource used for this text component
        /// </summary>
        public PDFFontResource FontRsrc
        {
            get;
            private set;
        }

        #endregion

        #region public PDFStyle FullStyle {get; protected set;}

        private Style _fullstyle;

        /// <summary>
        /// Gets the full style associated with this text component
        /// </summary>
        public Style FullStyle
        {
            get { return _fullstyle; }
            protected set { _fullstyle = value; }
        }

        #endregion;

        #region public PDFPositionOptions Position {get; protected set;}

        private PDFPositionOptions _posopts;
        /// <summary>
        /// Gets the position options for the text layout.
        /// Inheritors can set the value.
        /// </summary>
        public PDFPositionOptions Position
        {
            get { return _posopts; }
            protected set { _posopts = value; }
        }

        #endregion

        #region public PDFTextRenderOptions TextRender {get; protected set;}

        private PDFTextRenderOptions _textopts;
        /// <summary>
        /// Gets the text rendering options (font, size, etc) for this components layout.
        /// Inheritors can set the value.
        /// </summary>
        public PDFTextRenderOptions TextRenderOptions
        {
            get { return _textopts; }
            protected set { _textopts = value; }
        }

        #endregion

        #region public PDFTextReader Reader {get; protected set;}

        private PDFTextReader _reader;

        /// <summary>
        /// Gets the current text reader associated with this layout engine
        /// Inheritors can set the value.
        /// </summary>
        public PDFTextReader Reader
        {
            get { return _reader; }
            protected set { _reader = value; }
        }

        #endregion

        #region public IPDFLayoutEngine Parent {get;}

        private IPDFLayoutEngine _par;

        /// <summary>
        /// Gets the parent engine of this instance
        /// </summary>
        public IPDFLayoutEngine ParentEngine
        {
            get { return this._par; }
        }

        #endregion

        #region public PDFLayoutLine CurrentLine {get;}

        private PDFLayoutLine _currline;

        /// <summary>
        /// Gets or sets the current line that we are laying out text to.
        /// </summary>
        public PDFLayoutLine CurrentLine
        {
            get { return this._currline; }
            protected set { _currline = value; }
        }

        #endregion

        #region public PDFTextRunBegin BeginningRun {get;protected set;}

        private PDFTextRunBegin _begrun;

        /// <summary>
        /// Gets the beginning run for this text component. Holds the style and positioning info and a list of lines.
        /// </summary>
        public PDFTextRunBegin BeginningRun
        {
            get { return this._begrun; }
            protected set { this._begrun = value; }
        }

        #endregion

        #region public bool ContinueLayout {get;}

        /// <summary>
        /// If this engine should stop laying out the text then this should be set to true.
        /// </summary>
        public bool ContinueLayout
        {
            get;
            set;
        }

        #endregion

        #region public PDFUnit CurrentLineInset

        /// <summary>
        /// Gets or sets this inset of the current line in comparison to the containing region.
        /// </summary>
        public Unit CurrentLineInset
        {
            get;
            set;
        }

        #endregion

        //
        // ctor
        //
        public static int TextLayoutEngineCount = 0;

        #region public PDFTextLayoutEngine(IPDFTextComponent component, IPDFLayoutEngine parent)

        public LayoutEngineText(ITextComponent component, IPDFLayoutEngine parent)
        {
            if (null == component)
                throw new ArgumentNullException("component");
            this._txt = component;
            this._par = parent;

            TextLayoutEngineCount++;
        }

        #endregion

        
        //
        // layout methods
        //

        #region public void Layout(PDFLayoutContext context, Styles.PDFStyle fullstyle)

        /// <summary>
        /// Measures and lay's out the TextComponent that this engine references.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fullstyle"></param>
        public void Layout(PDFLayoutContext context, Styles.Style fullstyle)
        {
            if (null == context)
                throw new ArgumentNullException("context");
            if (null == fullstyle)
                throw new ArgumentNullException("fullstyle");


            this._ctx = context;
            this._fullstyle = fullstyle;

            
            this._posopts = fullstyle.CreatePostionOptions();
            this._textopts = fullstyle.CreateTextOptions();

            if (this._posopts.PositionMode == PositionMode.Invisible)
            {
                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Layout", "Skipping the layout of the text component " + this.TextComponent.ID + " as it is invisible");
                return;
            }

            this._frsrc = ((Document)this.TextComponent.Document).GetFontResource(this.TextRenderOptions.Font, true);

            this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Text_Layout);

            this._reader = this.TextComponent.CreateReader(context, fullstyle);

            if (null != this._reader)
            {
                this.ContinueLayout = true;
                this.DoLayoutText();
            }

            this.Context.PerformanceMonitor.End(PerformanceMonitorType.Text_Layout);

        }

        #endregion

        #region protected virtual void DoLayoutText()

        /// <summary>
        /// Performs the actual layout of the text by looping through each part of the text reader and 
        /// performing the required operation, based on the OpType.
        /// </summary>
        protected virtual void DoLayoutText()
        {
            if (!this.StartText())
                return;

            Unit lastwidth = Unit.Zero;
            int index = 0;
            while (this.Reader.Read())
            {
                switch (this.Reader.OpType)
                {
                    case PDFTextOpType.None:
                        //Do Nothing
                        break;

                    case PDFTextOpType.LineBreak:
                        this.AddHardReturn(lastwidth);
                        break;

                    case PDFTextOpType.TextContent:
                        string chars = (this.Reader.Value as PDFTextDrawOp).Characters;
                        if (index == 0)
                            chars = OptionallyRemoveWhiteSpaceInLayout(chars);
                        lastwidth = this.AddCharacters(chars);
                        break;

                    case PDFTextOpType.Proxy:
                        lastwidth = this.AddProxyCharacters(this.Reader.Value as PDFTextProxyOp);
                        break;

                    case PDFTextOpType.StyleStart:
                    case PDFTextOpType.StyleEnd:
                    case PDFTextOpType.ClassStart:
                    case PDFTextOpType.ClassEnd:
                    case PDFTextOpType.BeginBlock:
                    case PDFTextOpType.EndBlock:
                    case PDFTextOpType.Unknown:
                    default:
                        throw new PDFLayoutException(String.Format(Errors.TextOptionTypeIsNotSupported, this.Reader.OpType));
                }
                index++;
                if (!this.ContinueLayout)
                    break;
            }
            this.EndText();
        }

        #endregion

        #region protected virtual void StartText()

        /// <summary>
        /// We need to start the text
        /// </summary>
        protected virtual bool StartText()
        {
            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.Begin(TraceLevel.Verbose, LOG_CATEGORY, "Starting the layout of text component " + this.TextComponent.ID);
            bool started;
            PDFLayoutLine line = this.EnsureFirstLineAvailable(out started);
            if (null == line)
            {
                this.ContinueLayout = false;
                return false;
            }

            this.CurrentLine = line;

            this.Context.Graphics.SetCurrentFont(this.TextRenderOptions.Font);

            Unit inset = Unit.Zero;
            if (line.IsEmpty == false)
                inset = line.Width;
            else if (this.TextRenderOptions.FirstLineInset.HasValue && (this.Position.PositionMode != PositionMode.Inline || started))
            {
                inset += this.TextRenderOptions.FirstLineInset.Value;

                if (inset > 0)
                {
                    PDFTextRunSpacer spacer = new PDFTextRunSpacer(inset, 1, line, null);
                    line.AddRun(spacer);
                }
            }

            PDFTextRunBegin begin = new PDFTextRunBegin(this.TextRenderOptions, this.CurrentLine, this.TextComponent);
            begin.LineInset = inset;

            this.CurrentLine.AddRun(begin);
            begin.SetOffsetY(this.CurrentLine.OffsetY);


            this.CurrentLineInset = inset;
            this.BeginningRun = begin;

            if (this.Context.ShouldLogDebug)
                this.Context.TraceLog.End(TraceLevel.Verbose, LOG_CATEGORY, "Completed the layout of text component " + this.TextComponent.ID);
            else if (this.Context.ShouldLogVerbose)
                this.Context.TraceLog.Add(TraceLevel.Verbose, LOG_CATEGORY, "Laid out text component " + this.TextComponent.ID);

            return true;
        }

        #endregion

        #region private PDFLayoutLine EnsureFirstLineAvailable()

        /// <summary>
        /// If there is no open line at the start, or after a hard return, then this created a new one (adding inset as required)
        /// </summary>
        /// <returns></returns>
        private PDFLayoutLine EnsureFirstLineAvailable(out bool startedLine)
        {
            startedLine = false;
            PDFLayoutPage pg = this.Context.DocumentLayout.CurrentPage;
            PDFLayoutBlock block = pg.LastOpenBlock();
            if (null == block || block.IsClosed)
            {
                this.Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, "There is no open block on page '" + pg.ToString() + "' to add content to.");
                return null;
                throw new InvalidOperationException("There is no open block to add the textual content to.");
            }
            PDFLayoutRegion reg = block.CurrentRegion;
            if (null == reg || reg.IsClosed)
            {
                this.Context.TraceLog.Add(TraceLevel.Error, LOG_CATEGORY, "There is no open region in block '" + block.ToString() + "' on page '" + pg.ToString() + "' to add content to.");
                return null;
                throw new InvalidOperationException("There is no open block to add the textual content to.");
            }
            PDFLayoutLine line;
            if (reg.HasOpenItem)
            {
                line = (PDFLayoutLine)reg.CurrentItem;
            }
            else
            {
                line = reg.BeginNewLine();
                startedLine = true;
                //No Inset spacer as this will be handled by the begin run
            }

            return line;
        }

        #endregion

        #region protected virtual PDFTextRunSpacer AddLineInsetRun(PDFUnit w, PDFUnit h, PDFLayoutLine line)

        /// <summary>
        /// Adds a spacer of the required width (and height) to the line - inheritors can override this value
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="line"></param>
        protected virtual PDFTextRunSpacer AddLineInsetRun(Unit w, Unit h, PDFLayoutLine line)
        {
            PDFTextRunSpacer spacer = new PDFTextRunSpacer(w, h, line, this.TextComponent);
            line.AddRun(spacer);
            return spacer;
        }

        #endregion

        #region protected virtual void AddHardReturn()

        /// <summary>
        /// We need to add an explict return
        /// </summary>
        protected virtual void AddHardReturn(Unit widthOfLastTextDraw)
        {
            AddReturn(widthOfLastTextDraw, true);
        }

        #endregion

        #region protected virtual void AddReturn(PDFUnit widthOfLastTextDraw, bool hardReturn)

        protected virtual void AddReturn(Unit widthOfLastTextDraw, bool hardReturn)
        {
            this.AssertCurrentLine();
            PDFLayoutLine line = this.CurrentLine;
            PDFTextRunNewLine br = new PDFTextRunNewLine(false, line, this.TextRenderOptions, this.TextComponent);
            

            line.AddRun(br);

            //The offset is from the start of the last text drawing operation 
            //and the offset of the start of the current line
            Unit lineright = widthOfLastTextDraw;
            
            Unit back = line.Width - lineright;

            //Previous - 27 Feb 2015
            //br.Offset = new PDFSize(back, line.Height);

            //Updated
            if (line.Height == Unit.Zero)
                line.Runs.Add(new PDFTextRunSpacer(1, this.TextRenderOptions.GetLineHeight(), line, this.TextComponent));

            if (line.BaseLineOffset == 0 || this.TextRenderOptions.Leading.HasValue) //we don't have any begins or ends affecting the flow (or an explicit leading)
                br.Offset = new Size(back, line.Height);
            else
                br.Offset = new Size(back, line.Height);
            

            PDFLayoutRegion reg = line.Region;
            reg.CloseCurrentItem();
            line = reg.BeginNewLine();
            this.BeginningRun.Lines.Add(line);

            Unit inset;
            if (hardReturn)
                inset = this.TextRenderOptions.GetFirstLineInset();
            else
                inset = Unit.Zero;

            PDFTextRunSpacer spacer = this.AddLineInsetRun(inset, 0, line);
            br.NextLineSpacer = spacer;
            this.CurrentLine = line;
            this.CurrentLineInset = inset;

        }

        #endregion

        #region protected virtual PDFUnit AddProxyCharacters(PDFTextProxyOp proxy)

        /// <summary>
        /// Add characters to the line(s), and returns the last width of the characters that were laid out.
        /// </summary>
        protected virtual Unit AddProxyCharacters(PDFTextProxyOp proxy)
        {
            this.AssertCurrentLine();

            
            Unit lineheight = this.TextRenderOptions.GetLineHeight();
            ZeroLineCounter zeros = new ZeroLineCounter();

            Size measured = Size.Empty;
            Size required = Size.Empty;

            PDFLayoutLine line = this.CurrentLine;
            PDFLayoutRegion reg = line.Region;

            Unit availH = reg.AvailableHeight;
            Unit availW = line.AvailableWidth;


            if (availH < lineheight)
            {
                if (this.Position.OverflowAction != OverflowAction.Clip)
                    availH = lineheight;
                else
                {
                    this.DoMoveToNextRegion(lineheight);

                    if (!this.ContinueLayout)
                        return Unit.Zero;
                }
            }

            //Measure the string an get the fitted characters

            int fitted;

            this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Text_Measure);

            measured = this.MeasureString(availH, availW, proxy.Text, 0, out fitted);

            this.Context.PerformanceMonitor.End(PerformanceMonitorType.Text_Measure);

            required = new Size(measured.Width, lineheight);

            if (fitted < proxy.Text.Length) //cannot split a proxy - must simply be a single run.
            {
                //try on the next line to see if we can put everything on there.
                this.AddSoftReturn(0);

                if (!zeros.AssertIncrement(this.Context))
                    return Unit.Zero;

                availW = this.CurrentLine.AvailableWidth;

                this.Context.PerformanceMonitor.Begin(PerformanceMonitorType.Text_Measure);

                measured = this.MeasureString(availH, availW, proxy.Text, 0, out fitted);

                this.Context.PerformanceMonitor.End(PerformanceMonitorType.Text_Measure);

                if (fitted < proxy.Text.Length) //Still cannot fit the proxy so not much we can do. Log it and return
                {
                    this.Context.TraceLog.Add(TraceLevel.Warning, LOG_CATEGORY, "The text proxy  for '" + proxy.Text + "' could not fit the characters on a single line. Overflow of proxies is not currently supported.");
                    return measured.Width;
                }

                required = new Size(measured.Width, lineheight);
            }

            // everything fitted on the line
            
            zeros.Reset();
            this.AddProxyToCurrentLine(required, proxy);
            

            return required.Width;
        }

        #endregion

        private string OptionallyRemoveWhiteSpaceInLayout(string chars)
        {
            if (this.CurrentLine != null &&
                (this.CurrentLine.IsEmpty || this.CurrentLine.IsClosed || this.CurrentLine.Runs.Count < 2)) //no runs or just a begin text
            {
                if (char.IsWhiteSpace(chars, 0))
                    chars = chars.TrimStart(PDFXMLFragmentParser.WhiteSpace);
            }
            return chars;
        }

        private bool EndsInASpace(PDFTextRunCharacter runChars)
        {
            if (string.IsNullOrEmpty(runChars.Characters))
                return false;
            char last = runChars.Characters[runChars.Characters.Length - 1];
            return last == ' ';
        }

        #region private void AddProxyToCurrentLine(PDFSize size, PDFTextProxyOp op)

        /// <summary>
        /// Creates a new Text character run and adds it to the current line.
        /// </summary>
        /// <param name="size">the size of the run in PDFUnits</param>
        /// <param name="chars">The characters that should be rendered in the run</param>
        private void AddProxyToCurrentLine(Size size, PDFTextProxyOp op)
        {
            this.AssertCurrentLine();
            PDFTextRunProxy run = new PDFTextRunProxy(size, op, this.CurrentLine, this.TextComponent);
            this.CurrentLine.AddRun(run);
        }

        #endregion

        #region protected virtual PDFUnit AddCharacters(string chars)

        /// <summary>
        /// Add characters to the line(s), and returns the last width of the characters that were laid out.
        /// </summary>
        protected virtual Unit AddCharacters(string chars)
        {
            this.AssertCurrentLine();
           

            Unit lineheight = this.TextRenderOptions.GetLineHeight();
            int offset = 0;
            ZeroLineCounter zeros = new ZeroLineCounter();
            
            Size measured = Size.Empty;
            Size required = Size.Empty;

            while (offset < chars.Length)
            {
                //Check that we have enough space for the next line

                PDFLayoutLine line = this.CurrentLine;
                PDFLayoutRegion reg = line.Region;
                Unit availH = reg.AvailableHeight;
                Unit availW = line.AvailableWidth;

                if (availH < lineheight)
                {
                    if (this.Position.OverflowAction != OverflowAction.Clip)
                    {
                        this.DoMoveToNextRegion(lineheight);

                        if (!this.ContinueLayout)
                            return Unit.Zero;
                        else
                        {
                            line = this.CurrentLine;
                            reg = line.Region;
                            availW = line.AvailableWidth;
                            availH = reg.AvailableHeight;
                        }
                    }
                    else
                        availH = lineheight;
                    
                }

                //Measure the string an get the fitted characters
                
                Context.PerformanceMonitor.Begin(PerformanceMonitorType.Text_Measure);

                int fitted;
                measured =this.MeasureString(availH, availW, chars, offset, out fitted);

                Context.PerformanceMonitor.End(PerformanceMonitorType.Text_Measure);

                required = new Size(measured.Width, lineheight);

                if (fitted <= 0) //nothing fitted on the line
                {
                    this.AddSoftReturn(0);
                    if (!zeros.AssertIncrement(this.Context))
                        return Unit.Zero;
                    
                }
                else if (fitted + offset == chars.Length) // everything fitted on the line
                {
                    zeros.Reset();
                    string all = offset == 0 ? chars : chars.Substring(offset);
                    //if (offset == 0)
                        this.AddCharactersToCurrentLine(required, all);
                    //else
                    //    this.AddCharactersToCurrentLine(required, chars, offset, fitted);
                    offset += fitted;
                }
                else if (IsBrokenInWord(chars, offset, fitted) && CanSplitOnWordsOnly() && !IsEmptyLine()) //don't break on words unless we have to.
                {
                    this.AddSoftReturn(0);
                    if (!zeros.AssertIncrement(this.Context))
                        return Unit.Zero;
                }
                else //partial fit
                {
                    zeros.Reset();

                    string partial = chars.Substring(offset, fitted);
                    this.AddCharactersToCurrentLine(required, partial);
                    //this.AddCharactersToCurrentLine(required, chars, offset, fitted);
                    this.AddSoftReturn(measured.Width);
                    offset += fitted;

                    //Consume any white space as we are now on a new line.
                    //We should never get here for NoWrap as it will alywys fit all characters one one line.
                    while (offset < chars.Length && char.IsWhiteSpace(chars, offset))
                        offset++;
                }
            }
            return required.Width;
        }

        #endregion

        #region private bool CanSplitOnWordsOnly()

        /// <summary>
        /// If the current text render options allows splitting on characters returns false.
        /// For all other scenarios, we should not be splitting text inside words, only on white space boundaries.
        /// </summary>
        /// <returns></returns>
        private bool CanSplitOnWordsOnly()
        {
            bool wordsOnly;
            if (this.TextRenderOptions.WrapText.HasValue)
                wordsOnly = (this.TextRenderOptions.WrapText.Value != WordWrap.Character);
            else
                wordsOnly = true;

            return wordsOnly;
        }

        #endregion

        #region private bool IsBrokenInWord(string chars, int start, int count)

        /// <summary>
        /// Checks to see if the fitted string is a split inside a word, rather than at a word boundary.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private bool IsBrokenInWord(string chars, int start, int count)
        {
            if (char.IsWhiteSpace(chars, start + count))
                return false;
            else if (char.IsWhiteSpace(chars, start + count - 1))
                return false;
            else if (this.CurrentLine.IsEmpty == false)
                return true;
            else
                return false;
                
        }

        #endregion

        #region private bool IsEmptyLine()

        /// <summary>
        /// Retruns true if the current line has no entries or non-character runs
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyLine()
        {
            if (this.CurrentLine.IsEmpty)
                return true;
            else
            {
                for (int i = 0; i < this.CurrentLine.Runs.Count; i++)
                {
                    PDFLayoutRun run = this.CurrentLine.Runs[i];
                    if (run is PDFTextRunBegin)
                        continue;
                    else if (run is PDFTextRunSpacer)
                        continue;
                    else if (run is PDFLayoutPositionedRegionRun)
                        continue;
                    else if (run is PDFLayoutInlineBegin)
                        continue;
                    else
                        return false;
                }
                return true;
            }
        }

        #endregion

        #region private PDFSize MeasureString(string chars, int offset, out int fitted)

        /// <summary>
        /// Measures all the characters on a specified string returning their size 
        /// 6based on how many fitted in the available space.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="offset"></param>
        /// <param name="fitted"></param>
        /// <param name="availh">The available height in the current region</param>
        /// <returns></returns>
        private Size MeasureString(Unit availh, Unit availw, string chars, int offset, out int fitted)
        {
            Size available = new Size(availw, availh);
            PDFTextRenderOptions opts = this.TextRenderOptions;
            
            Size measured;
            measured = this.Context.Graphics.MeasureString(chars, offset, available, opts, out fitted);
            return measured;
        }

        #endregion

        #region private void AddCharactersToCurrentLine(PDFSize size, string chars)

        /// <summary>
        /// Creates a new Text character run and adds it to the current line.
        /// </summary>
        /// <param name="size">the size of the run in PDFUnits</param>
        /// <param name="chars">The characters that should be rendered in the run</param>
        private void AddCharactersToCurrentLine(Size size, string chars)
        {
            this.AssertCurrentLine();

            PDFTextRunCharacter run = new PDFTextRunCharacter(size, chars, this.CurrentLine, this.TextComponent);
            this.CurrentLine.AddRun(run);
        }

        #endregion

        #region private void AddCharactersToCurrentLine(PDFSize size, string chars)

        /// <summary>
        /// Creates a new Text character run and adds it to the current line.
        /// </summary>
        /// <param name="size">the size of the run in PDFUnits</param>
        /// <param name="chars">The characters that should be rendered in the run</param>
        private void AddCharactersToCurrentLine(Size size, string chars, int startOffset, int count)
        {
            this.AssertCurrentLine();

            PDFTextRunPartialCharacter run = new PDFTextRunPartialCharacter(size, chars, startOffset, count, this.CurrentLine, this.TextComponent);
            this.CurrentLine.AddRun(run);
        }

        #endregion

        #region private void AddSoftReturn(PDFUnit widthOfLastTextDraw)

        /// <summary>
        /// Closes the current line and begins a new one
        /// </summary>
        private void AddSoftReturn(Unit widthOfLastTextDraw)
        {
            AddReturn(widthOfLastTextDraw, false);
        }

        #endregion

        #region protected virtual void EndText()

        /// <summary>
        /// Close down and end the text
        /// </summary>
        protected virtual void EndText()
        {
            if (this.ContinueLayout)
            {
                this.AssertCurrentLine();

                PDFTextRunEnd end = new PDFTextRunEnd(this.BeginningRun, this.CurrentLine, this.TextComponent);
                this.CurrentLine.AddRun(end);
            }
        }

        #endregion

        #region private void AssertCurrentLine()

        /// <summary>
        /// Checks to make sure that there is a CurrentLine, and that it is open.
        /// </summary>
        private void AssertCurrentLine()
        {
            if (null == this.CurrentLine)
                throw new ArgumentNullException("CurrentLine");
            if (this.CurrentLine.IsClosed)
                throw new InvalidOperationException("CurrentLine");
        }

        #endregion



        //
        // overflow operations
        //


        protected virtual void DoMoveToNextRegion(Unit lineheight)
        {
            PDFLayoutLine lastline = this.CurrentLine;

            this.EndText(); //Always end this block of text

            bool newPage;
            PDFLayoutRegion origRegion = lastline.Region;
            PDFLayoutBlock origBlock = (PDFLayoutBlock)origRegion.Parent;
            PDFLayoutRegion region = origRegion;
            PDFLayoutBlock block = origBlock;
            LayoutEngineBase engine = this.ParentEngine as LayoutEngineBase;
            if (null == engine)
                throw new NullReferenceException("Parent engine was not the expected BlockLayoutEngine. A Hack that is needed for overflowing textual content");
            else if (engine.MoveToNextRegion(lineheight, ref region, ref block, out newPage))
            {
                if (!this.StartText())
                    return;
            }
            else
            {
                if (this.Context.TraceLog.ShouldLog(TraceLevel.Message))
                    this.Context.TraceLog.Add(TraceLevel.Message, LOG_CATEGORY, "Cannot layout any more text for component '" + this.TextComponent.ID + "'. Available space full and cannot move to another region.");

                if(null != region && IsEmptyText(region))
                {
                    if (region.Parent == block && block.Columns.Length == 1)
                        block.ExcludeFromOutput = true;

                    region.ExcludeFromOutput = true;
                }

                this.ContinueLayout = false;
            }
        }

        private bool IsEmptyBlock(PDFLayoutBlock block)
        {
            if (block.Columns.Length == 1 &&
                block.Position.Width.HasValue == false &&
                block.Position.Height.HasValue == false &&
                block.Position.MinimumHeight.HasValue == false &&
                block.Position.MinimumWidth.HasValue == false)

                return true;

            else
                return false;
        }

        protected bool IsEmptyText(PDFLayoutRegion region)
        {
            var currblock = this.Context.DocumentLayout.CurrentPage.CurrentBlock.LastOpenBlock();

            if (currblock.Position.PositionMode == PositionMode.Relative)
                return false;

            if (null != currblock && null != currblock.CurrentRegion && region == currblock.CurrentRegion)
                return false;

            
            if (region.Contents.Count > 1)
                return false;
            var line = region.Contents[0] as PDFLayoutLine;

            if (null == line) //It's a block not a line
                return false;

            else if (line.IsEmpty) //empty so true
                return true;

            if (line.Runs.Count != 2)
                return false;

            //We have a start and end run without any characters.
            if (line.Runs[0] is PDFTextRunBegin && line.Runs[1] is PDFTextRunEnd)
                return true;

            return false;
        }

        public bool MoveToNextPage(IComponent initiator, Style initiatorStyle, Stack<PDFLayoutBlock> depth, ref PDFLayoutRegion region, ref PDFLayoutBlock block)
        {
            throw new NotSupportedException("TextLayoutEngine should never be the top layout engine - and therefore should never control pages");
        }

        public PDFLayoutBlock CloseCurrentBlockAndStartNewInRegion(PDFLayoutBlock blockToClose, PDFLayoutRegion joinToRegion)
        {
            throw new NotSupportedException("CloseCurrentBlockAndStartNewInRegion should never be called on the text layout engine.");
        }

        #region IDisposable Implementation

        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Releases any unmanaged resources in this instance
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        #endregion

        //
        // innerclasses
        //

        #region private class ZeroLineCounter

        /// <summary>
        /// Counts the number of lines that are zero length and if over a predefined threshold 
        /// will throw an exception to break out of the loop.
        /// </summary>
        private class ZeroLineCounter
        {
            private const int ZeroCharactersPerThreshold = 2;

            private int _count = 0;

            public bool LastWasZero
            {
                get { return _count > 0; }
            }

            public bool AssertIncrement(PDFLayoutContext context)
            {
                this._count++;

                if (this._count > ZeroCharactersPerThreshold)
                {

                    context.TraceLog.Add(TraceLevel.Error, "Text Layout", "Fell into a suspected never ending loop of laying out zero characters per line. Breaking out ungracefully");
                    return false;

                }
                else
                    return true;
            }

            public void Reset()
            {
                _count = 0;
            }
        }

        #endregion
    }
}
