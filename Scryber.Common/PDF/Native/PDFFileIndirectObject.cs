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

namespace Scryber.PDF.Native
{
    /// <summary>
    /// Represents an existing IIndirectObject within a read file
    /// </summary>
    public sealed class PDFFileIndirectObject : IParsedIndirectObject, IDisposable
    {


        private int _num, _gen;
        private long _offset;
        private bool _deleted = false;
        private byte[] _stream;
        private string _data;
        private IFileObject _parsed;

        public int Number
        {
            get
            {
                return _num;
            }
            set
            {
                throw new NotSupportedException("Cannot update the Number on written indirect objects");
            }
        }

        public int Generation
        {
            get
            {
                return _gen;
            }
            set
            {
                throw new NotSupportedException("Cannot update the Generation on written indirect objects");
            }
        }

        public long Offset
        {
            get
            {
                return this._offset;
            }
            set
            {
                throw new NotSupportedException("Cannot update the offset on written indirect objects");
            }
        }

        public PDFObjectRef Reference
        {
            get { return new PDFObjectRef(this.Number, this.Generation); }
        }

        public PDFStream ObjectData
        {
            get { throw new NotSupportedException(); }
        }

        public bool Deleted
        {
            get { return this._deleted; }
        }

        public bool Written
        {
            get
            {
                return true;
            }
            set
            {
                throw new NotSupportedException("This object has already been written to the file");
            }
        }

        public bool HasStream
        {
            get { return null != this._stream && this._stream.Length > 0; }
        }

        public PDFStream Stream
        {
            get { throw new NotSupportedException(); }
        }

        private PDFFileIndirectObject(int number, int generation, string data)
        {
            this._num = number;
            this._gen = generation;
            this._data = data;
        }

        public byte[] GetObjectData()
        {
            return Encoding.ASCII.GetBytes(_data);
        }

        public byte[] GetStreamData()
        {
            return this._stream;
        }

        private void SetStreamData(byte[] data)
        {
            this._stream = data;
        }

        public IFileObject GetContents()
        {
            this.EnsureDataParsed();
            return this._parsed;
        }

        public void Dispose()
        {
            
        }

        private void EnsureDataParsed()
        {
            int end;
            if (null == this._parsed)
                this._parsed = PDFParserHelper.InferAndParseNextObject(_data, 0, out end);
        }

        public static PDFFileIndirectObject Parse(PDFTextSearcher searcher, int number, int gen)
        {
            string start = number.ToString() + " " + gen.ToString() + " obj";

            string end = "endobj";
            string endstream = "endstream";
            string stream = "stream";

            string matchstart = searcher.GetInnerText(start.Length);
            if (matchstart != start)
                throw new PDFNativeParserException(string.Format(CommonErrors.IndirectObjectCannotBeParsed, start));
            long startPos = searcher.Position;

            PDFFileRange endobjPos = searcher.MatchForwardString(end);
            if (endobjPos.Found == false)
                throw new PDFNativeParserException(string.Format(CommonErrors.IndirectObjectCannotBeParsed, start));

            PDFFileRange endstreamPos = searcher.MatchBackwardString(endstream, startPos);

            PDFFileRange startstreamPos;
            if (endstreamPos.Found)
            {
                startstreamPos = searcher.MatchBackwardString(stream, startPos);
                if (startstreamPos.Found == false)
                    throw new PDFNativeParserException(string.Format(CommonErrors.IndirectObjectCannotBeParsed, start));
                
                endobjPos = startstreamPos;
            }
            else
                startstreamPos = PDFFileRange.NotFound;
            int length = (int)(endobjPos.StartOffset - startPos);

            string data = searcher.GetInnerText(startPos, length);

            PDFFileIndirectObject parsed = new PDFFileIndirectObject(number, gen, data);
            parsed._offset = startPos;

            if (startstreamPos.Found)
                parsed.SetStreamData(searcher.GetInnerBytes(startstreamPos, endstreamPos));
            parsed.EnsureDataParsed();

            return parsed;
        }
    }
}
