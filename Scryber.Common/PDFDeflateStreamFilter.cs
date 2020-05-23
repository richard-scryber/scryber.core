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

namespace Scryber
{
    public sealed class PDFDeflateStreamFilter : IStreamFilter
    { 
        private const int BufferLength = 1024;
        private const string DefaultFilterName = "FlateDecode";

        private string _defalteName = DefaultFilterName;
       
        public string FilterName
        {
            get
            {
                return _defalteName;
            }
            set
            {
                _defalteName = value;
            }
        }

        public PDFDeflateStreamFilter()
        {
        }

        public void FilterStream(System.IO.Stream read, System.IO.Stream write)
        {

            if (!(read is System.IO.MemoryStream))
                throw new PDFStreamException(CommonErrors.WriteToOnlySupportedForMemoryStreams);
            //if (!(write is System.IO.MemoryStream))
            //    throw new PDFStreamException(Errors.WriteToOnlySupportedForMemoryStreams);
            byte[] input = (read as System.IO.MemoryStream).ToArray();
            byte[] output = FilterStream(input);
            
            if (null != output && output.LongLength > 0)
            {
                //TODO: Lazy - Use a buffer if length is greater than MaxInt
                if (output.LongLength > (long)int.MaxValue)
                    throw new PDFStreamException("Stream is too long to write in one blob");
                
                write.Write(output, 0, output.Length);
            }
        }

        public byte[] FilterStream(byte[] orig)
        {
            try
            {
                byte[] output;
                PDFDeflateZLib zlib = new PDFDeflateZLib();
                output = zlib.Compress(orig);
                return output;
            }
            catch (Exception ex)
            {
                throw new PDFException(CommonErrors.CouldNotCompressStreamFilter, ex);
            }
        }
    }
}
