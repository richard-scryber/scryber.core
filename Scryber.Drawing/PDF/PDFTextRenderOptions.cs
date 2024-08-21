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
using System.Globalization;
using System.Text;
using Scryber.Drawing;
using Scryber.Text;
using System.ComponentModel;
using Scryber.PDF.Graphics;

namespace Scryber.PDF
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFTextRenderOptions : ICloneable
    {

        #region public PDFFont Font {get;set;}

        private Font _font;

        /// <summary>
        /// Gets or sets the pdf font for these rendering options
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
            }
        }

        #endregion

        #region public PDFPen Stroke {get;set;}

        private PDFPen _scolor;

        /// <summary>
        /// Gets or sets the stroking pen for these rendering options
        /// </summary>
        public PDFPen Stroke
        {
            get { return _scolor; }
            set { _scolor = value; }
        }

        #endregion

        #region public PDFBrush FillBrush {get;set;}

        private PDFBrush _brush;

        /// <summary>
        /// Gets or sets the brush to fill the text with
        /// </summary>
        public PDFBrush FillBrush
        {
            get { return _brush; }
            set { _brush = value; }
        }

        #endregion

        #region public PDFBrush Background {get;set;}

        private PDFBrush _bg;

        /// <summary>
        /// Gets or sets the background brush for the lines of text
        /// </summary>
        public PDFBrush Background
        {
            get { return _bg; }
            set { _bg = value; }
        }

        #endregion

        #region public PDFPen Border {get;set;}

        private PDFPen _border;

        /// <summary>
        /// Gets or sets the pen that should be used to draw a border around any text.
        /// </summary>
        public PDFPen Border
        {
            get { return _border; }
            set { _border = value; }
        }

        #endregion

        #region public Unit BorderRadius { get; set; }

        /// <summary>
        /// Gets or sets any border radius for the background and border
        /// </summary>
        public Unit BorderRadius { get; set; }

        //TODO: Support multiple values

        #endregion

        #region public Thickness Padding {get;set;}

        private Thickness? _pad;

        /// <summary>
        /// Gets or sets any padding that should be set around the text
        /// </summary>
        public Thickness? Padding
        {
            get { return _pad; }
            set
            {
                _pad = value;
            }
        }

        #endregion;

        #region public Thickness InlineMargins {get;set;}

        private Thickness? _inlineMargins;

        public Thickness? InlineMargins
        {
            get { return _inlineMargins; }
            set { _inlineMargins = value; }
        }

        #endregion

        #region public WordWrap? WrapText {get;set;}

        public static readonly WordWrap DefaultWrapText = WordWrap.Auto;

        private WordWrap? _wrap;

        /// <summary>
        /// Gets or sets the (nullable) word wrapping setting for these rendering options
        /// </summary>
        public WordWrap? WrapText
        {
            get { return _wrap; }
            set { _wrap = value; }
        }

        #endregion

        #region public PDFUnit? FirstLineInset {get;set;}

        private Unit? _fistlineinset = Unit.Empty;

        /// <summary>
        /// Gets or sets the required indent for the first line of text in a paragraph
        /// </summary>
        public Unit? FirstLineInset
        {
            get { return _fistlineinset; }
            set { _fistlineinset = value; }
        }

        #endregion

        #region public PDFUnit? Leading {get;set;}

        private Unit? _leading;

        /// <summary>
        /// Gets or sets the (nullable) line leading for these rendering options
        /// </summary>
        public Unit? Leading
        {
            get { return _leading; }
            set { _leading = value; }
        }

        #endregion

        #region public VerticalAlignment? VerticalAlignment { get; set; }
        
        /// <summary>
        /// Gets or sets any vertical alignment on the text
        /// </summary>
        public VerticalAlignment? VerticalAlignment { get; set; }

        #endregion

        #region public double? WordSpacing {get;set;}

        private Unit? _wordspace;

        /// <summary>
        /// Gets or sets (nullable) the word spacing for this text rendering
        /// </summary>
        public Unit? WordSpacing
        {
            get { return _wordspace; }
            set { _wordspace = value; }
        }

        #endregion

        #region public PDFUnit? CharacterSpacing {get;}

        private Unit? _charSpace;
        /// <summary>
        /// Gets or sets (nullable) the character spacing for text rendering
        /// </summary>
        public Unit? CharacterSpacing
        {
            get { return _charSpace; }
            set { _charSpace = value; }
        }

        #endregion

        #region public double? CharacterHScale {get;set;}

        private double? _charHScale;
        /// <summary>
        /// Gets or sets (nullable) the horizontal scale for this text rendering
        /// </summary>
        public double? CharacterHScale
        {
            get { return _charHScale; }
            set { _charHScale = value; }
        }

        #endregion

        #region public TextDirection? TextDirection {get;set;}

        private TextDirection? _txtdirection;

        /// <summary>
        /// Gets or sets the (nullable) text direction for this text rendering
        /// </summary>
        public TextDirection? TextDirection
        {
            get { return _txtdirection; }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value != Scryber.TextDirection.LTR && value.Value != Scryber.TextDirection.RTL)
                        throw new NotSupportedException("The text direction " + value.Value.ToString() + " is not curently supported");
                }
                _txtdirection = value;
            }
        }

        #endregion


        #region public TextDecoration TextDecoration {get;set;}

        private TextDecoration _decor = TextDecoration.None;

        /// <summary>
        /// Gets or sets the text decoration (Underline etc) for the rendering options
        /// </summary>
        public TextDecoration TextDecoration
        {
            get { return _decor; }
            set { _decor = value; }
        }

        #endregion

        private bool _drawFromTop = true;

        /// <summary>
        /// If true then the measurements are based on the top of the font ascender.
        /// This is the default.
        /// If falue then the drawing positions are based around the baseline of the text.
        /// </summary>
        public bool DrawTextFromTop
        {
            get { return this._drawFromTop; }
            set { this._drawFromTop = value; }
        }


        private PDFHyphenationStrategy _hyphenation = null;

        public PDFHyphenationStrategy HyphenationStrategy
        {
            get { return this._hyphenation; }
            set { this._hyphenation = value; }
        }

        //
        // .ctor
        //

        #region public PDFTextRenderOptions()
        /// <summary>
        /// Creates a new (empty) instance of the PDFTextRenderOptions
        /// </summary>
        public PDFTextRenderOptions()
        {
        }

        #endregion

        //
        // methods
        //

        #region PDFTextRenderOptions Clone()

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Returns a shallow clone of the text render options
        /// </summary>
        /// <returns></returns>
        public PDFTextRenderOptions Clone()
        {
            return this.MemberwiseClone() as PDFTextRenderOptions;
        }

        #endregion

        public Unit GetSize()
        {
            if (null != this.Font)
                return this.Font.Size;
            else
                return Font.DefaultFontSize;
        }

        #region public PDFUnit GetLineHeight()

        /// <summary>
        /// Gets the line height based on either the assigned leading (if set), or the Font metrics line height.
        /// </summary>
        /// <returns></returns>
        public Unit GetLineHeight()
        {
            if (this.Leading.HasValue)
                return this.Leading.Value;
            else if (null != this.Font.FontMetrics)
                return this.Font.FontMetrics.TotalLineHeight;
            else
                return this.Font.Size * 1.2;
        }

        #endregion


        public Unit GetZeroCharWidth()
        {
            if (null != this.Font.FontMetrics)
                return this.Font.FontMetrics.ZeroWidth;
            else
                return this.Font.Size * 0.5;
        }

        public Unit GetBaselineOffset()
        {
            if (null == this.Font)
                return Unit.Zero;
            else if (this.Leading.HasValue)
            {
                if (null != this.Font.FontMetrics)
                {
                    var ascent = this.Font.FontMetrics.Ascent;
                    var size = ascent  + this.Font.FontMetrics.Descent;
                    var space = this.Leading.Value - size;
                    var halflead = space / 2.0;
                    return halflead + ascent;
                }
                else
                    return this.Font.Size * 0.75;
            }
            else if (null != this.Font.FontMetrics)
                return this.Font.FontMetrics.BaseLineOffset;
            else
                return this.Font.Size * 0.75;
        }

        #region public PDFUnit GetAscent()
        /// <summary>
        /// returns the Ascent height of the font in these text options. 
        /// e.g. The distance between the base line and the top of a capital A
        /// </summary>
        /// <returns></returns>
        public Unit GetAscent()
        {
            if (null == this.Font)
                return Unit.Zero;
            else if (null != this.Font.FontMetrics)
                return this.Font.FontMetrics.Ascent;
            else
                return this.Font.Size * 0.75;

        }

        #endregion

        public Unit GetDescender()
        {
            if (null == this.Font)
                return Unit.Zero;
            else if (null != this.Font.FontMetrics)
                return this.Font.FontMetrics.Descent;
            else
                return this.Font.Size * 0.25;
        }

        #region public PDFUnit GetFirstLineInset()

        /// <summary>
        /// Gets the first line inset if assigned, or Zero.
        /// </summary>
        /// <returns></returns>
        public Unit GetFirstLineInset()
        {
            if (this.FirstLineInset.HasValue)
                return this.FirstLineInset.Value;
            else
                return 0.0;
        }

        #endregion

    }
}
