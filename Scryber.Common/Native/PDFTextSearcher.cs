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

namespace Scryber.Native
{

    public class PDFTextSearcher
    {
        const int DefaultBufferLength = 16384;

        private System.IO.Stream _innerStream;
        private System.Text.Encoding _coding;
        private byte[] _readBuffer;


        public long Position
        {
            get { return _innerStream.Position; }
            set { _innerStream.Position = value; }
        }

        public long Length
        {
            get { return _innerStream.Length; }
        }

        public Encoding Encoding
        {
            get { return _coding; }
        }

        //
        // .ctor(s)
        //

        public PDFTextSearcher(System.IO.Stream stream)
            : this(stream, Encoding.ASCII, DefaultBufferLength)
        {
        }

        public PDFTextSearcher(System.IO.Stream stream, System.Text.Encoding encoding)
            : this(stream, encoding, DefaultBufferLength)
        {
        }


        public PDFTextSearcher(System.IO.Stream stream, System.Text.Encoding encoding, int buffersize)
        {
            _innerStream = stream;
            _coding = encoding;
            
            _readBuffer = new byte[buffersize];
        }

        //
        // public interface
        //

        public PDFFileRange MatchForwardString(string value)
        {
            return MatchForwardString(value, this.Length);
        }

        /// <summary>
        /// Finds the specified text in the file, searching forward from the current position.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PDFFileRange MatchForwardString(string value, long max)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");


            byte[] tomatch = _coding.GetBytes(value);
            long start = MatchForward(tomatch, max);
            if (start > -1 && start <= max)
                return new PDFFileRange(value, start, tomatch.Length + start);
            else
                return PDFFileRange.NotFound;
        }

        public PDFFileRange MatchBackwardString(string value)
        {
            return MatchBackwardString(value, 0);
        }

        /// <summary>
        /// Finds the specified text in the file, searching backwards from the current position. 
        /// Sets the current position to the starting position of the matched text, if found, otherwise 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The found range.</returns>
        public PDFFileRange MatchBackwardString(string value, long min)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            byte[] tomatch = _coding.GetBytes(value);
            long start = MatchBackward(tomatch, min);
            if (start > -1 && start >= min)
                return new PDFFileRange(value, start, tomatch.Length + start);
            else
                return PDFFileRange.NotFound;
        }



        /// <summary>
        /// Gets the text between the 2 ranges (exclusive)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public string GetInnerText(PDFFileRange from, PDFFileRange to)
        {
            return GetInnerText(from.EndOffset, (int)(to.StartOffset - from.EndOffset));
        }

        public string GetInnerText(int length)
        {
            return this.GetInnerText(this.Position, length);
        }

        /// <summary>
        /// Gets the text from the specified start position in the file with the specified length.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetInnerText(long start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException("start");
            else if (start >= this.Length)
                throw new ArgumentOutOfRangeException("start");
            else if (length <= 0)
                throw new ArgumentOutOfRangeException("length");
            else if (start + length > this.Length)
                throw new ArgumentOutOfRangeException("length");

            FillBuffer(start, length);
            return _coding.GetString(_readBuffer,0,(int)length);

        }


        public byte[] GetInnerBytes(PDFFileRange from, PDFFileRange to)
        {
            return this.GetInnerBytes(from.EndOffset, (int)(to.StartOffset - from.EndOffset));
        }

        public byte[] GetInnerBytes(int length)
        {
            return this.GetInnerBytes(this.Position, length);
        }

        public byte[] GetInnerBytes(long start, int length)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException("start");
            else if (start >= this.Length)
                throw new ArgumentOutOfRangeException("start");
            else if (length <= 0)
                throw new ArgumentOutOfRangeException("length");
            else if (start + length > this.Length)
                throw new ArgumentOutOfRangeException("length");

            FillBuffer(start, length);
            byte[] copy = new byte[length];
            Array.Copy(_readBuffer, copy, length);
            //_readBuffer.CopyTo(copy, 0);
            return copy;
        }

        //
        // protected implementation
        //

        protected byte[] ConvertToBytes(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            return _coding.GetBytes(value);
        }

        protected string ConvertToString(byte[] data)
        {
            if (null == data || data.Length == 0)
                throw new ArgumentNullException("data");

            return _coding.GetString(data);
        }

        protected int FillBuffer(long start, int length)
        {
            if (_readBuffer.Length < length)
            {
                Array.Resize<byte>(ref _readBuffer, length);
            }

            this._innerStream.Position = start;
            return this._innerStream.Read(_readBuffer, 0, length);
        }

        protected int FillBufferAtOffset(int offset, int length)
        {
            return this._innerStream.Read(_readBuffer, offset, length);
        }

        protected long MatchForward(byte[] tomatch, long max)
        {

            int bytesRead = 0; // number of bytes read
            int offset = 0; // offset inside read-buffer
            int length = this._readBuffer.Length;
            long filePos = this.Position; // position inside the file

            while ((bytesRead = FillBufferAtOffset(offset,length)) > 0)
            {
                int last = bytesRead + offset - tomatch.Length;
                long found = MatchForwardInBuffer(last, tomatch);

                if (found > -1)
                {
                    this.Position = filePos + offset + found;
                    return this.Position - offset;
                }
                
                // store the last few characters to ensure matches on "chunk boundaries"
                offset = tomatch.Length;
                MoveBufferDataToStart(last, offset);

                // store file position before next read
                filePos += last;
                if (filePos > max)
                    return -1;
                length = this._readBuffer.Length - offset;
            }

            return -1;
        }

        private void MoveBufferDataToStart(int fromPos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int copyFrom = fromPos + i;
                _readBuffer[i] = _readBuffer[copyFrom];
            }
        }

        protected long MatchForwardInBuffer(int upto, byte[] tomatch)
        {
            for (int i = 0; i <= upto; i++)
            {
                if (_readBuffer[i] == tomatch[0])
                {
                    bool match = true;

                    for (int j = 1; j < tomatch.Length; j++)
                    {
                        if (tomatch[j] != _readBuffer[i + j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        protected long MatchBackward(byte[] tomatch, long min)
        {
            int bytesRead = 0; // number of bytes read
            
            int bytesToRead = (int)Math.Min(this.Position, this._readBuffer.LongLength); //number of bytes to read
            long filePos = this.Position; // position inside the file
            this.Position -= bytesToRead;

            int offset = 0;
            if (bytesToRead < this._readBuffer.LongLength)
                offset = this._readBuffer.Length - bytesToRead;
            else
                this.Position += 1;
            
            while ((bytesRead = FillBufferAtOffset(offset, bytesToRead)) > 0)
            {
                this.Position -= bytesRead; //move the position back to the start of the read op.

                int last = bytesRead + offset - tomatch.Length;
                long found = MatchBackwardInBuffer(offset, tomatch);

                if (found > -1)
                {
                    this.Position += found - offset;
                    return this.Position;
                }
                // store the first few characters at the end of
                // the buffer to ensure matches on "chunk boundaries"
                offset = tomatch.Length;
                MoveBufferDataToEnd(last, offset);

                // store file position before next read
                filePos -= last;
                bytesToRead = (int)Math.Min(this.Position, this._readBuffer.LongLength - offset);
                this.Position -= bytesToRead;
                //we should read the next set of bytes into the end of the buffer
                offset = 0;

                if (filePos < min)
                    return -1;

                if (bytesToRead < this._readBuffer.LongLength - tomatch.Length)
                    offset = this._readBuffer.Length - ( bytesToRead + tomatch.Length);
            }

            return -1;
        }

        protected long MatchBackwardInBuffer(int end, byte[] tomatch)
        {
            int start = _readBuffer.Length - (tomatch.Length);
 
            for (int i = start; i >= end; i--)
            {
                if (_readBuffer[i] == tomatch[0])
                {
                    bool match = true;

                    for (int j = 1; j < tomatch.Length; j++)
                    {
                        if (tomatch[j] != _readBuffer[i + j])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void MoveBufferDataToEnd(int toPos, int count)
        {
            for (int i = 0; i < count; i++)
            {
                int copyFrom = i;
                int copyTo = i + toPos;
                _readBuffer[copyTo] = _readBuffer[copyFrom];
            }
        }
    }

}
