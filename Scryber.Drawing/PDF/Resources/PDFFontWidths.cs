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
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{

    /// <summary>
    /// Represents a standard set of ASCII character widths that will be rendered to the PDF file as a simple array.
    /// </summary>
    /// <remarks>The standard font widths instance does not support composite fonts</remarks>
    /// <remarks>
    /// THREAD SAFETY: a single instance is shared process-wide between concurrently rendering
    /// documents. Non-standard, non-Unicode fonts hand the definition's own instance to every
    /// document (see FontDefinition.GetWidths), and font definitions are cached for the lifetime
    /// of the process by the FontFactory. Implementations must therefore hold no per-call mutable
    /// state - no cached buffers, no accumulators. Keep working state local to the method.
    /// </remarks>
    public abstract class PDFFontWidths : TypedObject
    {
        #region ivars

        private List<int> _widths = new List<int>(255);

        private int _first;

        private int _last;

        #endregion

        public abstract bool IsEmpty { get; }

        #region public int FirstChar {get;set;}

        /// <summary>
        /// Gets or sets the first character this widths array represents
        /// </summary>
        public int FirstChar
        {
            get { return _first; }
            set 
            { 
                _first = value;
                
            }
        }

        #endregion

        #region public int LastChar {get;set;}

        /// <summary>
        /// Gets or sets the last character this widths array supports.
        /// </summary>
        public int LastChar
        {
            get { return _last; }
            set 
            { 
                _last = value; 
                
            }
        }

        #endregion

        #region internal CMapEncoding Encoding {get;}

        /// <summary>
        /// Gets or sets the font encoding for these widths
        /// </summary>
        internal Scryber.OpenType.SubTables.CMapEncoding Encoding
        {
            get;
            set;
        }

        #endregion


        //
        // ctor
        //
        

        public PDFFontWidths() : base(ObjectTypes.FontWidths)
        {
        }

        
        public PDFFontWidths(int first, int last, Scryber.OpenType.SubTables.CMapEncoding encoding): this()
        {
            this._first = first;
            this._last = last;
            this.Encoding = encoding;
        }




        /// <summary>
        /// Renders the widths to the output writer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        public PDFObjectRef RenderToPDF(ContextBase context, PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject();
            this.RenderWidthsArrayToPDF(context, writer);
            writer.EndObject();
            return oref;
        }


        /// <summary>
        /// Renders the widths array to the output writer. 
        /// Does not create an indirect reference, so writer must be positioned appropriately
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        public abstract void RenderWidthsArrayToPDF(ContextBase context, PDFWriter writer);


        /// <summary>
        /// Registers the use of a character in a string and returns the glyph offset in the current font, and this characters width
        /// </summary>
        /// <param name="c"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public abstract char RegisterGlyph(char c);

        public virtual string RegisterGlyphs(string chars)
        {
            if (null == chars)
                throw new ArgumentNullException("chars");
            int start = 0;
            int count = chars.Length;
            return RegisterGlyphs(chars, start, count);
        }

        /// <summary>
        /// Registers the use of all the characters in a string and returns the string as glyph offsets in the current font and the total width for these characters.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="totalwidth"></param>
        /// <returns></returns>
        public virtual string RegisterGlyphs(string chars, int startindex, int count)
        {
            if (null == chars)
                throw new ArgumentNullException(nameof(chars));
            if (count <= 0)
                return string.Empty;

            //All state must stay local. Instances of this class are shared process-wide between
            //concurrently rendering documents, so a cached buffer here would be a data race.
            //Written straight into the result string, so this allocates no more than the previous
            //cached-StringBuilder version did. The state tuple and the static lambda keep it
            //closure free.
            return string.Create(count, (widths: this, chars, startindex), static (span, state) =>
            {
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] = state.widths.RegisterGlyph(state.chars[state.startindex + i]);
                }
            });
        }

        public virtual void RegisterGlyphs(StringBuilder chars, int startindex, int count)
        {
            if (null == chars)
                throw new ArgumentNullException(nameof(chars));

            int end = startindex + count;
            for (int i = startindex; i < end; i++)
            {
                chars[i] = RegisterGlyph(chars[i]);
            }
        }
    }
}
