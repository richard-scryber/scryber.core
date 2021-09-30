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

namespace Scryber.PDF.Native
{
    /// <summary>
    /// Encapsulates the stream of data and / or operations within a PDF File
    /// </summary>
    public class PDFStream : IFileObject, IDisposable
    {

        public ObjectType Type { get { return PDFObjectTypes.Stream; } }
        private System.IO.Stream _ms = null;
        private System.IO.MemoryStream _filtered;
        bool _ownsstream;
        private PDFIndirectObject _indobj;
        private bool _hasinitfilterstream = false;
        private byte[] _lineterminator;
        private bool _readonly = false;

        /// <summary>
        /// Gets or sets the series of bytes that should be written to the 
        /// stream as a line terminator
        /// </summary>
        protected byte[] LineTerminator
        {
            get { return _lineterminator; }
            set { _lineterminator = value; }
        }

        public long Position
        {
            get { return this.InnerStream.Position; }
        }
        /// <summary>
        /// Gets the underlying stream of the TextWriter
        /// </summary>
        protected System.IO.Stream InnerStream
        {
            get { return this._ms; }
        }

        /// <summary>
        /// True if this PDFStream owns the inner stream, and hence should dispose of it.
        /// </summary>
        protected bool OwnsStream
        {
            get { return _ownsstream; }
        }

        public PDFIndirectObject IndirectObject
        {
            get { return _indobj; }
            set { _indobj = value; }
        }

        protected System.IO.MemoryStream FilteredStream
        {
            get { return _filtered; }
            set { _filtered = value; }
        }

        protected bool HasInitializedFilterStream
        {
            get { return _hasinitfilterstream; }
            set { _hasinitfilterstream = value; }
        }

        public bool HasFilters
        {
            get { return null != _filters && _filters.Length > 0; }
        }

        public long FilteredLength
        {
            get
            {
                if (HasFilters)
                {
                    EnsureFilteredStream();
                    return _filtered.Length;
                }
                else
                    return -1;
            }
        }

        /// <summary>
        /// Creates a new PDFStream
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="indobj"></param>
        public PDFStream(IStreamFilter[] filters, PDFIndirectObject indobj)
            : this(new System.IO.MemoryStream(), filters, indobj, true)
        {
        }

        /// <summary>
        /// Creates a new instance of the PDFStream with the specified encoding and filters
        /// </summary>
        /// <param name="stream">The stream to write to. If it is not a MemoryStream then functionality is seriously impaired.</param>
        /// <param name="encoding">The text encoding of the TextWiter stream</param>
        /// <param name="filters">The filters to apply to the stream</param>
        /// <param name="indobj">The PDF indirect object that has this stream</param>
        /// <param name="ownstream">If true then this PDFStream owns the actual stream, and can dispose of it when wanted.</param>
        public PDFStream(System.IO.Stream stream, IStreamFilter[] filters, PDFIndirectObject indobj, bool ownstream)
        {
            if (null == stream)
                throw new ArgumentNullException("stream");
            this._ms = stream;
            this._filters = filters;
            this._indobj = indobj;
            this._ownsstream = ownstream;

            InitLineTerminator();
        }

        private void InitLineTerminator()
        {
            byte[] term = GetISO("\r\n");
            this.LineTerminator = term;
        }

        /// <summary>
        /// Gets the current length of the InnerStream
        /// </summary>
        public long Length
        {
            get
            {
                return this.InnerStream.Length;
            }
        }

        #region public IStreamFilter[] Filters {get; set;}

        private IStreamFilter[] _filters;

        /// <summary>
        /// Gets of sets the current set of filter s to apply to the stream
        /// </summary>
        public IStreamFilter[] Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;

                if (this.HasInitializedFilterStream)
                {
                    this._filtered.SetLength(0);
                    this._hasinitfilterstream = false;
                }
            }
        }

        #endregion

        public bool ReadOnly
        {
            get { return _readonly; }
            set { _readonly = value; }
        }
        public void WriteLine()
        {
            this.Write(this.LineTerminator);
        }

        public void WriteLine(string data)
        {
            this.Write(data);
            this.Write(this.LineTerminator);
        }

        public void Write(string data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                char c = data[i];
                if (c > 255)
                    throw new PDFStreamException(string.Format(CommonErrors.InvalidChararcterForStreamUseBytes, c));
                else
                    this.InnerStream.WriteByte((byte)c);
            }
            //byte[] buf = this.GetISO(data);
            //buf = Encoding.Convert(this.TextEncoding,this.StreamEncoding, buf);
            //this.Write(buf);
        }





        private byte[] GetISO(string data)
        {
            byte[] iso = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                iso[i] = (byte)data[i];
            }
            return iso;
        }

        public void Write(byte[] data)
        {
            this.Write(data, 0, data.Length);
        }

        public void Write(byte[] data, int offset, int length)
        {
            this.InnerStream.Write(data, offset, length);
        }

        public void WriteData(PDFWriter writer)
        {
            throw new NotSupportedException("The stream object cannot write data");
        }

        public byte[] GetStreamData()
        {
            this.InnerStream.Flush();
            if (this.InnerStream is System.IO.MemoryStream)
            {
                System.IO.MemoryStream read;

                if (this.HasFilters)
                {
                    this.EnsureFilteredStream();
                    read = this.FilteredStream;
                }
                else
                    read = this.InnerStream as System.IO.MemoryStream;

                return read.ToArray();
            }
            else
            {
                throw new NotSupportedException("The to array method is only supported on memory streams");
            }
        }

        public void WriteTo(PDFStream stream)
        {
            this.WriteTo(stream.InnerStream);
        }

        public void WriteTo(System.IO.Stream stream)
        {
            this.InnerStream.Flush();
            if (this.InnerStream is System.IO.MemoryStream)
            {
                System.IO.MemoryStream read;

                if (this.HasFilters)
                {
                    this.EnsureFilteredStream();
                    read = this.FilteredStream;
                }
                else
                    read = this.InnerStream as System.IO.MemoryStream;

                read.WriteTo(stream);
            }
            else
            {
                throw new NotSupportedException(CommonErrors.WriteToOnlySupportedForMemoryStreams);
            }
        }

        public void EnsureFilteredStream()
        {
            if (this.HasInitializedFilterStream == false)
            {
                if (!HasFilters)
                    throw new InvalidOperationException(CommonErrors.StreamDoesNotHaveFiltersDefined);

                if (!(this.InnerStream is System.IO.MemoryStream))
                    throw new NotSupportedException(CommonErrors.WriteToOnlySupportedForMemoryStreams);

                try
                {
                    this.InnerStream.Flush();
                    System.IO.MemoryStream read = this.InnerStream as System.IO.MemoryStream;
                    byte[] filtered = this.ApplyFilters(this.Filters, read.ToArray());

                    if (null == this.FilteredStream)
                        this.FilteredStream = CreateStream();

                    if (null != filtered && filtered.Length > 0)
                        this.FilteredStream.Write(filtered, 0, filtered.Length);

                    this.HasInitializedFilterStream = true;
                }
                catch (Exception ex)
                {
                    throw new PDFException("Could not filter the stream : " + ex.Message, ex);
                }
            }
        }

        private byte[] ApplyFilters(IStreamFilter[] filters, byte[] input)
        {
            byte[] output = null;
            try
            {
                for (int i = 0; i < filters.Length; i++)
                {

                    IStreamFilter filter = this.Filters[i];
                    output = filter.FilterStream(input);
                    input = output;
                }

            }
            catch (Exception ex)
            {
                string msg = string.Format(CommonErrors.CouldNotApplyStreamFilters, this.IndirectObject.ToString(), ex.Message);
                throw new PDFStreamException(msg, ex);
            }

            return output;
        }

        public System.IO.MemoryStream CreateStream()
        {
            return new System.IO.MemoryStream();
        }

        public void Flush()
        {
            this.InnerStream.Flush();
            if (null != _filtered)
                _filtered.Flush();
        }

        public virtual void Reset()
        {
            if (null != this.FilteredStream)
                this.FilteredStream.SetLength(0);
            this.HasInitializedFilterStream = false;
            this.InnerStream.SetLength(0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._ownsstream)
                {
                    this._ms.Dispose();
                }
                this._ms = null;

                if (null != _filtered)
                    _filtered.Dispose();
                _filtered = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        ~PDFStream()
        {
            this.Dispose(false);
        }
    }
}
