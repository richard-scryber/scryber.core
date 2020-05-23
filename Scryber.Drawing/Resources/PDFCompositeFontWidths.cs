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

//#define REGISTER_DEFAULT_GLYPHS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.Native;
using Scryber.OpenType.SubTables;

namespace Scryber.Resources
{
    /// <summary>
    /// A grouping of font widths against characters that have been used in the document 
    /// </summary>
    public class PDFCompositeFontWidths : PDFFontWidths
    {
        #region public override bool IsEmpty {get;}

        /// <summary>
        /// Overrides base implmentation to return true is there are no widths in the compositve font
        /// </summary>
        public override bool IsEmpty
        {
            get { return this.Count > 0; }
        }

        #endregion

        /// <summary>
        /// Encapsulates the actual width in PDF Units and the Horizontal metric from the font file
        /// </summary>
        private struct WidthMetric
        {
            //TODO: This can be optimsed as the width will not change for the font (ever).
            public int Width;
            public int Glyph;
        }


        private SortedDictionary<char, WidthMetric> _char2offset;
        private SortedDictionary<int,char> _offset2char;



        protected HorizontalMetrics HMetrics
        {
            get;
            private set;
        }

        protected CMAPSubTable CharacterMappings
        {
            get;
            private set;
        }

        //
        // public methods
        //

        #region public int Count {get;}

        /// <summary>
        /// Gets the number of registered glyphs / characters in this Composite fond widths instance
        /// </summary>
        public int Count
        {
            get { return _char2offset.Count; }
        }

        #endregion

        /// <summary>
        /// Get the number of PDF units in an Em (m)
        /// </summary>
        public int UnitsPerEm
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Minumum glyph offset required by this composite font in the current document
        /// </summary>
        public int MinGlyphIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Maximum glyph offset required by this composite font in the current document
        /// </summary>
        public int MaxGlyphIndex
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the glyph unit conversion value 
        /// </summary>
        public int GlyphUnits
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of registered Glyph Offsets within the CMAP for this font. (sorted by ascending char character value)
        /// </summary>
        public IEnumerable<int> RegisterdGlyphOffsets
        {
            get { return _offset2char.Keys; }
        }

        /// <summary>
        /// Gets the list of registered characters (sorted by ascending char character value)
        /// </summary>
        public IEnumerable<char> RegistedCharacters
        {
            get { return _char2offset.Keys; }
        }


        public PDFCompositeFontWidths(CMapEncoding encoding, CMAPSubTable cmap, HorizontalMetrics hmetrics, int unitsPerEm, int glyphUnits)
        {
            this.HMetrics = hmetrics;
            this.CharacterMappings = cmap;
            this.Encoding = encoding;
            this.UnitsPerEm = unitsPerEm;
            this.GlyphUnits = glyphUnits;
            _char2offset = new SortedDictionary<char, WidthMetric>();
            _offset2char = new SortedDictionary<int, char>();
            this.MinGlyphIndex = ushort.MaxValue;

#if REGISTER_DEFAULT_GLYPHS
            //pre-load the glyph array as 
            this.RegisterDefaultGlyphs();
#endif
        }


        /// <summary>
        /// Returns the offset of a Glyph in the fonts CMAP table based on the requrest character.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        /// <remarks>If the character has not previously been registered this method will return -1</remarks>
        public int GetGlyphOffsetForChar(char c)
        {
            WidthMetric w;
            if (this._char2offset.TryGetValue(c, out w))
                return w.Glyph;
            else
                return -1;
        }

        /// <summary>
        /// Returns the with of a Glyph in PDFUnits based on the requested character.
        /// </summary>
        /// <param name="c">The character to get the width for</param>
        /// <returns></returns>
        /// <remarks>If the character has not previously been registered this method will return -1</remarks>
        public int GetGlyphWidthForCharacter(char c)
        {
            WidthMetric w;
            if (this._char2offset.TryGetValue(c, out w))
                return w.Width;
            else
                return -1;
        }

        /// <summary>
        /// Gets the unicode character that is represented by the glyph at the specified offset
        /// </summary>
        /// <param name="offset">The Glyph offset in the CMAP file of this instances font</param>
        /// <returns>The character represented by the glyph</returns>
        public char GetCharacterForGlyphOffset(int offset)
        {
            char c;
            if (this._offset2char.TryGetValue(offset, out c))
                return c;
            else
                return char.MinValue;
        }


        public override void RenderWidthsArrayToPDF(PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "PDFFontWidths", "Width information will be rendered as a CID lookup set");

            List<char> keys = new List<char>(this._char2offset.Keys);
            keys.Sort();
            writer.BeginArray();

            SortedDictionary<int, int> glyph2width = new SortedDictionary<int, int>();
            foreach (WidthMetric met in this._char2offset.Values)
            {
                glyph2width[met.Glyph] = met.Width;
            }

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "PDFFontWidths", "CID Lookup built with glyph widths");

            //Writes the used characters as [c[w] c[w] c[w]]
            //TODO: Improve rendering
            foreach (int g in glyph2width.Keys)
            {
                int w = glyph2width[g];
                writer.WriteNumber(g);
                writer.BeginArray();
                writer.WriteNumber(w);
                writer.EndArray();
            }
            writer.EndArray();

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "PDFFontWidths", glyph2width.Count.ToString() + " widths written as CID lookup set");
        }

        public override char RegisterGlyph(char c)
        {
            WidthMetric metric;

            if (!_char2offset.TryGetValue(c, out metric))
            {
                int offset = this.CharacterMappings.GetCharacterGlyphOffset(c);
                int hmindex = offset;
                if (hmindex >= this.HMetrics.HMetrics.Count)
                    hmindex = this.HMetrics.HMetrics.Count - 1;
                HMetric found = this.HMetrics.HMetrics[hmindex];

                int width = (found.AdvanceWidth * this.GlyphUnits) / UnitsPerEm;

                metric = new WidthMetric() { Width = width, Glyph = offset };

                _char2offset.Add(c, metric);
                if (!_offset2char.ContainsKey(offset))
                    _offset2char[offset] = c;
                //else
                //{
                //    System.Diagnostics.Debug.WriteLine("offset '" + offset + "' is already assigned to chararcter '" + _offset2char[offset] + "'");
                //}

                if (offset < this.MinGlyphIndex)
                    this.MinGlyphIndex = offset;
                if (offset > this.MaxGlyphIndex)
                    this.MaxGlyphIndex = offset;

                if (c < this.FirstChar)
                    this.FirstChar = c;
                if (c > this.LastChar)
                    this.LastChar = c;

            }

            return (char)metric.Glyph;
        }


        private void RegisterDefaultGlyphs()
        {
            foreach (char c in _defalutglyphs)
            {
                this.RegisterGlyph(c);
            }
        }

        private static char[] _defalutglyphs;

        static PDFCompositeFontWidths()
        {
            List<char> defaults = new List<char>();
            FillChars(defaults, (char)0, '~');
            _defalutglyphs = defaults.ToArray();
        }

        private static void FillChars(List<char> defaults, char min, char max)
        {
            char act = min;
            while (act <= max)
            {
                defaults.Add(act);
                act++;
            }
        }

        
    }


    
}
