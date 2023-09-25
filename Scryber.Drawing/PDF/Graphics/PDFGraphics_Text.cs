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
using System.Text;
using Scryber.Text;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF;
using System.Runtime.CompilerServices;

using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{

    public partial class PDFGraphics
    {

        #region protected double CustomWordSpace {get;} + HasCustomWordSpace + ResetCustomWordSpace()

        private double? _custWordSpace = null;

        /// <summary>
        /// Returns true if the current text rendering mode has a custom word spacing
        /// </summary>
        protected bool HasCustomWordSpace
        {
            get { return _custWordSpace.HasValue; }
        }

        /// <summary>
        /// Returns the custom word spacing associated with the current text options.
        /// </summary>
        protected double CustomWordSpace
        {
            get { return _custWordSpace.HasValue ? _custWordSpace.Value : 0.0; }
            set { _custWordSpace = value; }
        }

        /// <summary>
        /// Clears any custom word spacing so the graphics context does not have a custom word space
        /// </summary>
        public void ResetCustomWordSpace()
        {
            this._custWordSpace = null;
        }

        #endregion

        #region protected TextDirection TextDirection

        private TextDirection _direction = TextDirection.LTR;

        /// <summary>
        /// Gets or sets the current text direction for rendering
        /// </summary>
        protected TextDirection TextDirection
        {
            get { return _direction; }
            set { _direction = value; }
        }

        #endregion

        #region protected PDFFontResource CurrentFont {get; set;}

        private PDFFontResource _fontrsrc;

        /// <summary>
        /// Gets the current font (resource) associated with the Graphics context
        /// </summary>
        protected PDFFontResource CurrentFontResource
        {
            get { return _fontrsrc; }
            set { _fontrsrc = value; }
        }

        #endregion

        #region public PDFUnit CurrentFont {get; private set;}

        /// <summary>
        /// Gets the current font size
        /// </summary>
        public Drawing.Font CurrentFont
        {
            get;
            private set;
        }

        #endregion

        
        /// <summary>
        /// Gets the metrics associated with the current graphics font
        /// </summary>
        /// <returns></returns>
        public FontMetrics GetCurrentFontMetrics()
        {
            if (null == this.CurrentFont || null == this.CurrentFontResource)
                return null;
            else
                return this.CurrentFontResource.Definition.GetFontMetrics(this.CurrentFont.Size);

        }

        #region protected SetCurrentFont(PDFFont font)

        public void SetCurrentFont(Drawing.Font font)
        {
            PDFResource rsrc = font.Resource; // this.Container.Document.GetResource(PDFResource.FontDefnResourceType, font.FullName, true);
            if (null == rsrc)
                throw new NullReferenceException(String.Format("The font resource has not bee set on '{0}'", font.FullName));

            string name = this.Container.Register(rsrc);
            this.CurrentFontResource = (PDFFontResource)rsrc;
            this.CurrentFont = font;

            if(null != this.Writer)
                this.Writer.WriteOpCodeS(PDFOpCode.TxtFont, (PDFName)name, font.Size.RealValue);
            
        }

        #endregion

        #region protected RenderCurrentTextMode(TextRenderMode mode)

        protected void RenderCurrentTextMode(TextRenderMode mode)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.TxtRenderMode, new PDFNumber((int)mode));
        }

        #endregion


        /// <summary>
        /// All the character glyph widths used to the Widths collection
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        private void RegisterStringUse(string chars, int startIndex, int count)
        {
            PDFFontResource frsrc = this.CurrentFontResource;
            if (frsrc.Widths != null) 
                frsrc.Widths.RegisterGlyphs(chars, startIndex, count);
        }

        /// <summary>
        /// Measures a single line of text (no wrapping) and returns the size required, 
        /// and the number of characters fitted into that line. Uses the current font and size.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="startIndex"></param>
        /// <param name="available"></param>
        /// <param name="options"></param>
        /// <param name="charsfitted"></param>
        /// <returns></returns>
        /// <remarks>Uses the TTFFontFile if available, otherwise will default to using the GDI+ measure,
        /// which is slower but supports postscript fonts</remarks>
        public Drawing.Size MeasureString(string chars, int startIndex, Drawing.Size available, PDFTextRenderOptions options, out int charsfitted, out char? appendChar)
        {
            appendChar = null;

            PDFFontResource frsc = this.CurrentFontResource;
            Unit fsize = this.CurrentFont.Size;

            if (null == frsc)
                throw new InvalidOperationException("Current font has not be set on the graphics class, so strings cannot be measured");

            FontDefinition defn = frsc.Definition;
            Drawing.Size measured;
            if (defn.CanMeasureStrings)
            {
                bool trimtoword = true;
                
                if (options.WrapText.HasValue)
                {
                    if (options.WrapText == WordWrap.NoWrap)
                        available.Width = new Unit(Scryber.Text.TextConst.NoWrappingLineWidthPoints, PageUnits.Points);
                    else if (options.WrapText == WordWrap.Character)
                        trimtoword = false;
                }
                
                //inspect the spacing values - if any are set then we must use them in the calculations
                bool complexSpacing = false;
                bool vertical = false; // Not currently supported.
                double? wordSpace = null;
                double? charSpace = null;
                double? hScale = null;

                if(options.WordSpacing.HasValue)
                {
                    complexSpacing = true;
                    wordSpace = options.WordSpacing.Value.PointsValue;
                }
                if (options.CharacterSpacing.HasValue)
                {
                    complexSpacing = true;
                    charSpace = options.CharacterSpacing.Value.PointsValue;
                }
                if (options.CharacterHScale.HasValue)
                {
                    complexSpacing = true;
                    hScale = options.CharacterHScale.Value;
                }
                if (complexSpacing)
                {
                    //TODO: Check if we can do this on the widths rather than the ttfile.
                    measured = defn.MeasureStringWidth(chars, startIndex, fsize.PointsValue, available.Width.PointsValue, wordSpace, charSpace, hScale, vertical, trimtoword, out charsfitted);

                    //TODO: 
                }
                else
                {
                    //TODO: Check if we can do this on the widths rather than the ttfile.
                    measured = defn.MeasureStringWidth(chars, startIndex, fsize.PointsValue, available.Width.PointsValue, trimtoword, out charsfitted);

                    if (trimtoword == false && charsfitted > 0 && (startIndex + charsfitted < chars.Length))
                    {
                        var strategy = options.HyphenationStrategy;
                        var opportunity = this.GetWordHypenation(chars, startIndex, charsfitted, strategy);

                        if (opportunity.NewLength > 0)
                        {
                            var tomeasure = chars.Substring(startIndex, opportunity.NewLength);
                            appendChar = opportunity.AppendHyphenCharacter;

                            if (appendChar.HasValue)
                                tomeasure += opportunity.AppendHyphenCharacter.Value;

                            measured = defn.MeasureStringWidth(tomeasure, 0, fsize.PointsValue, (double)(int.MaxValue), true, out charsfitted);
                        }
                        charsfitted = opportunity.NewLength; //override anyway

                    }
                }

                if (charsfitted > 0)
                    this.RegisterStringUse(chars, startIndex, charsfitted);

                if (appendChar.HasValue)
                    this.RegisterStringUse(appendChar.Value.ToString(), 0, 1);
            }
            else
            {
                throw new NotSupportedException("The font definition cannot measure strings");

                //if (startIndex > 0)
                //   chars = chars.Substring(startIndex);
                //measured = this.MeasureGraphicsString(chars, available, options.WrapText.HasValue ? options.WrapText.Value : WordWrap.Auto, out charsfitted);
            }

            return measured;
        }

        private HyphenationOpportunity GetWordHypenation(string chars, int startindex, int charsfitted, PDFHyphenationStrategy strategy)
        {
            if (null == strategy)
                strategy = PDFHyphenationStrategy.Default;

            return PDFHyphenationRule.HyphenateLine(strategy, chars, startindex, charsfitted);
        }

        private double GetLineLeft(Drawing.Size size, PDFTextRenderOptions options)
        {
            if (options != null && options.FirstLineInset != Unit.Empty)
                return options.FirstLineInset.Value.ToPoints().Value;
            else
                return 0.0;
        }

        


        private void ReleaseTextRenderOptions(PDFTextRenderOptions options, Rect bounds)
        {
            if (options.FillBrush != null)
                options.FillBrush.ReleaseGraphics(this, bounds);
            if (options.Stroke != null)
                options.Stroke.ReleaseGraphics(this, bounds);
        }

        public TextRenderMode SetTextRenderOptions(PDFTextRenderOptions options, Rect bounds)
        {
            
            TextRenderMode mode = TextRenderMode.NoOp;
            if (options == null)
                throw new ArgumentNullException("options");
            
            if (options.FillBrush != null)
            {
                mode = TextRenderMode.Fill;
                options.FillBrush.SetUpGraphics(this, bounds);
            }
            if (options.Stroke != null)
            {
                if (mode == TextRenderMode.Fill)
                    mode = TextRenderMode.FillAndStroke;
                else
                    mode = TextRenderMode.Stroke;

                options.Stroke.SetUpGraphics(this, bounds);
            }
            if (options.Font != null)
                this.SetCurrentFont(options.Font);

            if (options.TextDirection.HasValue)
                this.TextDirection = options.TextDirection.Value;
            else
                this.TextDirection = Scryber.TextDirection.LTR;

            SetTextLeading(options);
            SetTextSpacing(options);
            SetTextRenderMode(mode);
            return mode;
        }

        public void SetTextLeading(PDFTextRenderOptions options)
        {
            Unit h;
            if (options.Leading.HasValue)
                h = options.Leading.Value;
            else if (options.Font.FontMetrics != null)
                h = options.Font.FontMetrics.TotalLineHeight;
            else if (null != this.CurrentFontResource && options.Font != null && this.CurrentFontResource.FontName == options.Font.FullName)
            {
                options.Font.FontMetrics = this.CurrentFontResource.Definition.GetFontMetrics(options.Font.Size);
                h = options.Font.FontMetrics.TotalLineHeight;
            }

            else
                h = options.Font.Size.PointsValue * 1.2;

            this.Writer.WriteOpCodeS(PDFOpCode.TxtLeading, h.RealValue);
        }

        public void SetTextSpacing(PDFTextRenderOptions options)
        {
            if (options.WordSpacing.HasValue == false)
                this.ResetCustomWordSpace();
            SetTextSpacing(options.WordSpacing, options.CharacterSpacing, options.Font.Size);

            if (options.CharacterHScale.HasValue)
                this.Writer.WriteOpCodeS(PDFOpCode.TxtHScaling, (PDFReal)options.CharacterHScale.Value * 100.0);
        }

        public void SetTextSpacing(Unit? wordSpacingOffset, Unit? charSpacingOffset, Unit fontSize)
        {
            if (charSpacingOffset.HasValue)
                this.Writer.WriteOpCodeS(PDFOpCode.TxtCharSpacing, (PDFReal)charSpacingOffset.Value.PointsValue);

            if (wordSpacingOffset.HasValue)
            {
                this.Writer.WriteOpCodeS(PDFOpCode.TxtWordSpacing, wordSpacingOffset.Value.RealValue);
                
                //Removed with the use of the actual point size calculation
                //if (null == this.CurrentFontResource || null == this.CurrentFontResource.Definition)
                //{
                //    this.Context.TraceLog.Add(TraceLevel.Warning, "Graphics", "Could not load the current font definition to set word spacing");
                //    return;
                //}
                //else
                //{
                //    FontDefinition defn = this.CurrentFontResource.Definition;

                //    //calculate the required width of a space in PDF Text units
                //    double spaceOffsetPDFUnits = ((wordSpacingOffset.Value.PointsValue) * (double)PDFOpenTypeFontDefinition.PDFGlyphUnits) / fontSize.PointsValue;

                //    //caclulate the normal width of a space in PDF Text units
                //    double spaceWidthPDFUnits = (defn.SpaceWidthFontUnits / defn.FontUnitsPerEm) * (double)PDFOpenTypeFontDefinition.PDFGlyphUnits;

                //    //calculate the actual space size (normal width + offset) in PDFTextUnits
                //    double spaceActualPDFUnits = spaceWidthPDFUnits + spaceOffsetPDFUnits;

                //    //calculate the factor (1 = normal size, 2 = double size, etc)
                //    double spaceFactor = spaceActualPDFUnits / spaceWidthPDFUnits;

                //    this.CustomWordSpace = spaceActualPDFUnits;

                //    this.Writer.WriteOpCodeS(PDFOpCode.TxtWordSpacing, (PDFReal)spaceFactor);
                //}
            }
        }

        

        public void SetTextRenderMode(TextRenderMode mode)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.TxtRenderMode, (PDFNumber)(int)mode);
        }

        public void BeginMarkedContent(PDFName name)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.MarkedContentBegin, name);
        }

        public void EndMarkedContent(PDFName name)
        {
            this.Writer.WriteOpCodeS(PDFOpCode.MarkedContentEnd);
        }


        public void BeginText()
        {
            this.Writer.WriteOpCodeS(PDFOpCode.TxtBegin);
        }

        public void EndText()
        {
            this.Writer.WriteOpCodeS(PDFOpCode.TxtEnd);
        }

        public void FillText(string chars, int offset, int count)
        {
            if (!this.Writer.UseHex)
            {
                if (chars.IndexOfAny(TextConst.EscapeChars, offset,count) > -1)
                {
                    //cannot optimize the replacement of escape characters so substring and return
                    this.FillText(chars.Substring(offset, count));
                    return;
                }
            }

            string glyphs = this.CurrentFontResource.Widths.RegisterGlyphs(chars, offset, count);

            if (this.TextDirection == Scryber.TextDirection.RTL)
                glyphs = ReverseString(glyphs);

            this.Writer.WriteStringLiteralS(glyphs, this.CurrentFontResource.Encoding);
            this.Writer.WriteOpCodeS(PDFOpCode.TxtPaint);
        }

        private StringBuilder _stringCache = null;

        protected string ReverseString(string chars)
        {
            if (null == _stringCache)
                _stringCache = new StringBuilder(chars.Length);
            else
                _stringCache.Clear();
            
            _stringCache.EnsureCapacity(chars.Length);
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                _stringCache.Append(chars[i]);
            }
            return _stringCache.ToString();
        }

        

        public void FillText(string chars)
        {
            if (!this.Writer.UseHex)
            {
                if (chars.IndexOfAny(TextConst.EscapeChars) > -1)
                {
                    for (int i = 0; i < TextConst.EscapeStrings.Length; i++)
                    {
                        chars = chars.Replace(TextConst.EscapeStrings[i], TextConst.ReplaceStrings[i]);
                    }
                }
            }


            if (this.TextDirection == Scryber.TextDirection.RTL)
                //Updated for string builder
                chars = ReverseString(chars);

            if (this.HasCustomWordSpace && chars.IndexOf(' ') > -1)
            {
                //Getting weird spacing on justified so testing without this complexity.

                string glyphs = this.CurrentFontResource.Widths.RegisterGlyphs(chars);
                //this.CurrentFontResource.Widths.RegisterGlyphs(_stringCache, 0, _stringCache.Length);
                this.Writer.WriteStringLiteralS(glyphs, this.CurrentFontResource.Encoding);
                this.Writer.WriteOpCodeS(PDFOpCode.TxtPaint);
                return;

                //we have custom word spacing and there's at least one space in there space 
                int pos = 0;
                int nextspace = 0;
                string glyph;

                //with custom word spacing we write the blocks of text as an array
                this.Writer.BeginArray();

                int PDFUnitSpaceOffset =  -(int)(this.CustomWordSpace); //negative offset of the word spacing * the PDF unit size of a Space character.

                while (pos < chars.Length && (nextspace = chars.IndexOf(' ', pos)) > -1)
                {
                    glyph = this.CurrentFontResource.Widths.RegisterGlyphs(chars, pos, nextspace - pos);
                    //this.CurrentFontResource.Widths.RegisterGlyphs(_stringCache, pos, nextspace - pos);

                    this.Writer.WriteStringLiteralS(glyph, this.CurrentFontResource.Encoding);
                    this.Writer.WriteNumberS(PDFUnitSpaceOffset);

                    pos = nextspace + 1;
                }
                if (pos < chars.Length)
                {
                    glyph = this.CurrentFontResource.Widths.RegisterGlyphs(chars, pos, chars.Length - pos);
                    //this.CurrentFontResource.Widths.RegisterGlyphs(_stringCache, pos, nextspace - pos);

                    this.Writer.WriteStringLiteralS(glyph, this.CurrentFontResource.Encoding);
                }

                this.Writer.EndArray();
                this.Writer.WriteOpCodeS(PDFOpCode.TxtPaintArray);
            }
            else
            {
                string glyphs = this.CurrentFontResource.Widths.RegisterGlyphs(chars);
                //this.CurrentFontResource.Widths.RegisterGlyphs(_stringCache, 0, _stringCache.Length);
                this.Writer.WriteStringLiteralS(glyphs, this.CurrentFontResource.Encoding);
                this.Writer.WriteOpCodeS(PDFOpCode.TxtPaint);
            }
        }


        /// <summary>
        /// Moves the current text cursor along by the specified amount. 
        /// If Absolute then this is from the top left of the current page. 
        /// If not absolute, then this is relative to the current cursor position.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="absolute"></param>
        public void MoveTextCursor(Drawing.Size offset, bool absolute)
        {
            if (absolute)
            {
                this.Writer.WriteOpCodeS(PDFOpCode.TxtMoveNextOffset, GetXPosition(offset.Width), GetYPosition(offset.Height));
            }
            else if (offset == Drawing.Size.Empty)
            {
                this.Writer.WriteOpCodeS(PDFOpCode.TxtNextLine);
            }
            else
            {
                this.Writer.WriteOpCodeS(PDFOpCode.TxtMoveNextOffset, GetXOffset(offset.Width), GetYOffset(offset.Height));
            }
        }

    }
}
