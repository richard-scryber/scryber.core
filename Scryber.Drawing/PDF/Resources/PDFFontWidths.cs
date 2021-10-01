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
    public abstract class PDFFontWidths : PDFObject
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
        public PDFObjectRef RenderToPDF(PDFContextBase context, PDFWriter writer)
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
        public abstract void RenderWidthsArrayToPDF(PDFContextBase context, PDFWriter writer);


        /// <summary>
        /// Registers the use of a character in a string and returns the glyph offset in the current font, and this characters width
        /// </summary>
        /// <param name="c"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public abstract char RegisterGlyph(char c);

        private StringBuilder sb = null;

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
            if (null == sb)
                sb = new StringBuilder();
            else
                sb.Length = 0;

            if (sb.Capacity < count)
                sb.Capacity = count;

            int end = startindex + count;

            for (int i = startindex; i < end; i++)
            {

                char c = chars[i];
                c = RegisterGlyph(c);
                sb.Append(c);
            }
            return sb.ToString();
        }

        public virtual void RegisterGlyphs(StringBuilder chars, int startindex, int count)
        {
            int end = startindex + count;
            for(int i = startindex; i < end; i++)
            {
                char c = chars[i];
                c = RegisterGlyph(c);
                sb[i] = c;
            }
        }
    }
}
