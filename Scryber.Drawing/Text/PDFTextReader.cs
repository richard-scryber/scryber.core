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

namespace Scryber.Text
{
    /// <summary>
    /// Base class for reading a text stream. 
    /// </summary>
    public abstract class PDFTextReader : IDisposable
    {
        public abstract bool Read();

        public abstract PDFTextOp Value { get;}

        public abstract int Length { get;}

        public abstract bool EOF { get;}

        public PDFTextOpType OpType
        {
            get
            {
                return this.Value.OpType;
            }
        }


        public void Reset()
        {
            this.ResetTextMarkers();
        }

        protected abstract void ResetTextMarkers();


        public PDFTextReader()
        {
            
            
        }

        

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            
        }

        ~PDFTextReader()
        {
            this.Dispose(false);
        }

        public static PDFTextReader Create(string text, TextFormat format, bool preserveWhitespace, PDFTraceLog log)
        {
            switch (format)
            {
                case TextFormat.Plain:
                    return new PDFPlainTextReader(text, preserveWhitespace);

                case TextFormat.XML:
                    return new PDFXMLFragmentReader(text, preserveWhitespace, log);

                //case TextFormat.RTF:
                //    throw new NotSupportedException("RTF is not currently supported");

                default:
                    throw new NotSupportedException("This format is not currently supported");

            }
        }
    }

    
}
