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
using System.IO;
using Scryber.PDF.Native;

namespace Scryber.PDF
{
    public class PDFWriter14 : PDFWriter, IStreamFactory
    {
        private const string TraceCategory = "PDFWriter";
        private const TraceLevel TraceDefaultLevel = TraceLevel.Debug;
        public static readonly Version DefaultVersion = new Version(1, 4);

        private Version _docvers;
        public override Version Version { get { return _docvers; } }

        #region Constants

        internal protected static class Constants
        {
            public const string StartObject = "obj\r\n";
            public const string CommentStart = "%";
            public const string EndObject = "\r\nendobj\r\n";
            public const string StartStream = "\r\nstream\r\n";
            public const string EndStream = "\r\nendstream";
            public const string StartDictionary = "<< ";
            public const string EndDictionary = " >>";
            public const string StartName = "/";
            public const string StartArray = "[";
            public const string EndArray = "]";
            public const string ArrayEntrySeparator = " ";
            public const string DictionaryEntrySeparator = " "; //"\r\n";
            public const string DictionaryNameValueSeparator = " ";
            public const string CommentPrePend = "%";
            public const string StartString = "(";
            public const string EndString = ")";
            public const string StartHexString = "<";
            public const string EndHexString = ">";
            public const string WhiteSpace = " ";
            public const string NullString = "null";
            public const string XRefEntrySeparator = "\r\n";
            
        }

        internal protected static class Constants_min
        {
            public const string StartObject = "obj";
            public const string CommentStart = "%";
            public const string EndObject = "endobj";
            public const string StartStream = "stream";
            public const string EndStream = "endstream";
            public const string StartDictionary = "<<";
            public const string EndDictionary = " >>";
            public const string StartName = "/";
            public const string StartArray = "[";
            public const string EndArray = "]";
            public const string ArrayEntrySeparator = " ";
            public const string DictionaryEntrySeparator = "";
            public const string DictionaryNameValueSeparator = " ";
            public const string CommentPrePend = "%";
            public const string StartString = "(";
            public const string EndString = ")";
            public const string StartHexString = "<";
            public const string EndHexString = ">";
            public const string WhiteSpace = " ";
            public const string NullString = "null";
            public const string XRefEntrySeparator = " ";

        }

        #endregion

        //
        // properties
        //


        #region protected bool FinishedEntry {get;set;}

        private bool _finishedentry = false;
        /// <summary>
        /// 
        /// </summary>
        protected bool FinishedEntry
        {
            get { return _finishedentry; }
            set { _finishedentry = value; }
        }

        #endregion

        #region  protected bool IsUpdateToExisting

        /// <summary>
        /// Returns true if this writer is adding content to an existing PDF file
        /// </summary>
        protected bool IsUpdateToExisting
        {
            get { return this.XRefTable.Previous != null; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFWriter14(Stream stream, PDFTraceLog log)

        /// <summary>
        /// Creates a new PDFWriter14 to update the specified stream, and a tracelog to write to as required.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="log"></param>
        public PDFWriter14(Stream stream, TraceLog log)
            : this(stream, 0, log, DefaultVersion)
        {
        }

        #endregion

        #region public PDFWriter14(Stream stream, int gen, PDFTraceLog log, Version vers)

        /// <summary>
        /// Creates a new PDFWriter14 to update the specified stream, and a tracelog to write to as required. With the generation number and version
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="gen"></param>
        /// <param name="log"></param>
        /// <param name="vers"></param>
        public PDFWriter14(Stream stream, int gen, TraceLog log, Version vers)
            : base(stream, log)
        {
            this._docvers = vers;
        }



        #endregion


        //
        // methods
        //

        #region protected virtual PDFIndirectObject CreateIndirectObject()

        /// <summary>
        /// Creates a new PDFIndirectObject in this Writers' document stream
        /// </summary>
        /// <returns></returns>
        protected virtual PDFIndirectObject CreateIndirectObject()
        {
            return new PDFIndirectObject(this);
        }

        #endregion

        #region public virtual PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)

        /// <summary>
        /// Creates a new stream data entry on the indirect object provided.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="forobject"></param>
        /// <returns></returns>
        public virtual PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)
        {
            PDFIndirectObject indirect = (PDFIndirectObject)forobject;
            return new PDFStream(filters, indirect);
        }

        #endregion

        #region public override void OpenDocument() + 1 overload

        /// <summary>
        /// Opens a new PDF document for writing too.
        /// </summary>
        public override void OpenDocument()
        {
            this.InitXRefTable(0, 0, null);
        }

        /// <summary>
        /// Opens a new PDF document for writing to appending as a overwrite of the original file if provided, and copying the content of the original file as specified.
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="copytoDestination"></param>
        public override void OpenDocument(PDFFile orig, bool copytoDestination)
        {
            PDFXRefTable prev = null;
            int startindex = 0;
            int gen = 0;
            
            if (null != orig)
            {
                if (copytoDestination)
                    orig.WriteTo(this.BaseStream);

                prev = orig.DocumentXRefs;
                startindex = prev.MaxReference + 1;
                gen = prev.Generation;
                
            }
            this.InitXRefTable(startindex, gen, prev);
        }

        #endregion

        #region public override void CloseDocument(PDFDocumentID documentid)

        /// <summary>
        /// Closes the document, writing all the indirect objects, the xreftable, and the trailer to the underlying base stream
        /// </summary>
        /// <param name="documentid"></param>
        public override void CloseDocument(PDFDocumentID documentid)
        {
            this.Log(TraceLevel.Message, "Closing the writer and outputting indirect object data onto the underlying stream");
            this.WritePDFHeader();
            this.WriteAllIndirectObjects(this.XRefTable);
            this.WriteXRefTable(this.XRefTable);
            this.BaseStream.WriteLine();
            this.WriteTrailerObject(documentid);
            this.WritePDFEndOfFile();
        }

        #endregion

        #region protected void WritePDFEndOfFile()

        /// <summary>
        /// Outputs the xref table offset and end of file marker
        /// </summary>
        protected void WritePDFEndOfFile()
        {
            this.BaseStream.Flush();
            this.BaseStream.WriteLine("startxref");
            this.BaseStream.WriteLine(this.XRefTable.Offset.ToString());
            this.BaseStream.Write("%%EOF");
            this.Log("Written end of file marker");
            this.BaseStream.Flush();
        }

        #endregion

        #region protected virtual void WritePDFHeader()

        /// <summary>
        /// Writes the PDF Header information to the base stream (PDF Version and byte stream)
        /// </summary>
        protected virtual void WritePDFHeader()
        {
            if (this.IsUpdateToExisting == false)
            {
                this.BaseStream.Flush();
                this.BaseStream.WriteLine(String.Format("%PDF-{0}.{1}", this.Version.Major, this.Version.Minor));
                this.BaseStream.Write(Constants.CommentStart);
                this.BaseStream.Write("????");
                this.BaseStream.WriteLine();
            }
        }

        #endregion

        #region protected virtual void WriteAllIndirectObjects(PDFXRefTable table)

        /// <summary>
        /// Writes all the indirect object references that are not free, and have no already beew written to the underlying base stream
        /// </summary>
        protected virtual void WriteAllIndirectObjects(PDFXRefTable table)
        {
            foreach (PDFXRefTableSection section in table.Sections)
            {
                foreach (PDFXRefTableEntry entry in section.Entries)
                {
                    if (entry.Free == false && entry.Reference != null && entry.Reference.Written == false)
                    {
                        WriteAnIndirectObject(entry.Reference);
                    }

                }
            }
        }

        #endregion

        #region protected virtual void WriteAnIndirectObject(IIndirectObject pfo)

        /// <summary>
        /// Writes an individual IndirectObject to the base stream
        /// </summary>
        /// <param name="pfo"></param>
        protected virtual void WriteAnIndirectObject(IIndirectObject pfo)
        {
            if (pfo.Written == true)
                throw new InvalidOperationException(Errors.IndirectObjectHasAlreadyBeenWritten);

            try
            {
                this.Log("Writing indirect object data " + pfo.Number);
                this.BaseStream.Flush();
                pfo.Offset = this.BaseStream.Position;
                this.BaseStream.Write(pfo.Number.ToString());
                this.BaseStream.Write(" ");
                this.BaseStream.Write(pfo.Generation.ToString());
                this.BaseStream.Write(" ");
                this.BaseStream.Write(Constants.StartObject);

                WriteIndirecObjectData(pfo);
                if (pfo.HasStream)
                {
                    WriteIndirectStreamData(pfo);
                }
                this.BaseStream.Write(Constants.EndObject);
                this.BaseStream.WriteLine();
                pfo.Written = true;
            }
            catch (Exception ex)
            {
                throw new PDFException("Could not write the indirect object '" + pfo.Number + " " + pfo.Generation + " R' to the underlying base stream: " + ex.Message, ex);
            }
        }

        #endregion

        #region protected virtual void WriteIndirectStreamData(IIndirectObject pfo)

        /// <summary>
        /// Outputs the IndirectObjects stream data onto the base stream
        /// </summary>
        /// <param name="pfo"></param>
        protected virtual void WriteIndirectStreamData(IIndirectObject pfo)
        {
            this.BaseStream.Write(Constants.StartStream);
            pfo.Stream.WriteTo(this.BaseStream);
            this.BaseStream.Write(Constants.EndStream);
        }

        #endregion

        #region protected virtual void WriteIndirecObjectData(IIndirectObject pfo)

        /// <summary>
        /// Outputs the IndirectObjects object data onto the base stream
        /// </summary>
        /// <param name="pfo"></param>
        protected virtual void WriteIndirecObjectData(IIndirectObject pfo)
        {
            pfo.ObjectData.WriteTo(this.BaseStream);
        }

        #endregion

        #region protected virtual void WriteTrailerObject(PDFDocumentID documentid)

        /// <summary>
        /// Writes the trailer flag and the trailer dictionary to the base stream
        /// </summary>
        /// <param name="documentid"></param>
        protected virtual void WriteTrailerObject(PDFDocumentID documentid)
        {
            this.BaseStream.WriteLine("trailer");
            this.BaseStream.Write(Constants.StartDictionary);
            WriteTrailerDictionaryEntries(documentid);
            this.BaseStream.WriteLine(Constants.EndDictionary);
        }

        #endregion

        #region protected virtual void WriteTrailerDictionaryEntries(PDFDocumentID documentid)

        /// <summary>
        /// Outputs all the trailer dictionary entries - size, catalog, info and id to the base stream
        /// </summary>
        /// <param name="documentid"></param>
        protected virtual void WriteTrailerDictionaryEntries(PDFDocumentID documentid)
        {
            this.BaseStream.Write(Constants.StartName);
            this.BaseStream.Write("Size");
            this.BaseStream.Write(Constants.WhiteSpace);
            this.BaseStream.WriteLine(this.XRefTable.ReferenceCount.ToString());

            PDFObjectRef oref;
            if (this.ObjectDictionary.TryGetValue("Catalog", out oref))
            {
                this.BaseStream.Write(Constants.StartName);
                this.BaseStream.Write("Root");
                this.BaseStream.Write(Constants.WhiteSpace);
                this.BaseStream.Write(oref.Number + Constants.WhiteSpace + oref.Generation + " R");
                this.BaseStream.WriteLine();
            }

            if (this.IsUpdateToExisting)
            {
                this.BaseStream.Write(Constants.StartName);
                this.BaseStream.Write("Prev");
                this.BaseStream.Write(Constants.WhiteSpace);
                this.BaseStream.Write(this.XRefTable.Previous.Offset.ToString());
                this.BaseStream.WriteLine();
            }

            if (this.ObjectDictionary.TryGetValue("Info", out oref))
            {
                this.BaseStream.Write(Constants.StartName);
                this.BaseStream.Write("Info");
                this.BaseStream.Write(Constants.WhiteSpace);
                this.BaseStream.Write(oref.Number + Constants.WhiteSpace + oref.Generation + " R");

            }
            
            if (documentid != null)
            {
                this.BaseStream.WriteLine();
                this.BaseStream.Write(Constants.StartName);
                this.BaseStream.Write("ID");
                this.BaseStream.Write(Constants.WhiteSpace);
                this.BaseStream.Write(Constants.StartArray);
                
                this.WriteStringHex(documentid.One);
                this.BaseStream.Write(" ");
                this.WriteStringHex(documentid.Two);
                this.BaseStream.WriteLine(Constants.EndArray);
            }
        }

        #endregion

        #region protected void WriteXRefTable(PDFXRefTable table)

        /// <summary>
        /// Outputs a complete XRefTable onto the base stream
        /// </summary>
        /// <param name="table"></param>
        protected void WriteXRefTable(PDFXRefTable table)
        {
            this.Log("Outputting XRefTable onto the stream at position " + this.BaseStream.Position.ToString());
            
            this.BaseStream.Flush();
            table.Offset = this.BaseStream.Position;

            this.BaseStream.WriteLine("xref");
            foreach (PDFXRefTableSection section in table.Sections)
            {


                this.BaseStream.Write(section.Start.ToString());
                this.BaseStream.Write(" ");
                this.BaseStream.WriteLine(section.Count.ToString());
                foreach (PDFXRefTableEntry entry in section.Entries)
                {
                    if (entry.Free)
                    {
                        int next = 0;
                        if (null != entry.NextFree)
                            next = entry.NextFree.Index;

                        this.BaseStream.Write(next.ToString("0000000000"));
                        this.BaseStream.Write(entry.Generation.ToString(" 00000"));
                        this.BaseStream.Write(" f");
                    }
                    else
                    {
                        this.BaseStream.Write(entry.Offset.ToString("0000000000"));
                        this.BaseStream.Write(entry.Generation.ToString(" 00000"));
                        this.BaseStream.Write(" n");
                    }
                    this.BaseStream.Write(Constants.XRefEntrySeparator);
                }
            }
            this.BaseStream.Flush();
        }

        #endregion



        //
        // current stream methods
        //


        #region public override void WriteLine()

        /// <summary>
        /// Writes a end of line marker to the current stream
        /// </summary>
        public override void WriteLine()
        {
            this.CurrentStream.WriteLine();
        }

        #endregion

        #region public override void WriteCommentLine(string comment) + 2 overloads

        /// <summary>
        /// Writes an empty comment line
        /// </summary>
        public override void WriteCommentLine()
        {
            this.CurrentStream.WriteLine(Constants.CommentPrePend);
        }

        /// <summary>
        /// Writes a comment line with the specified message to the current stream
        /// </summary>
        /// <param name="comment"></param>
        public override void WriteCommentLine(string comment)
        {
            this.CurrentStream.Write(Constants.CommentPrePend);
            this.CurrentStream.WriteLine(comment);
        }

        /// <summary>
        /// Writes a comment line with the specified formatted comment and arguments to the current stream
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="args"></param>
        public override void WriteCommentLine(string comment, params object[] args)
        {
            this.CurrentStream.Write(Constants.CommentPrePend);
            this.CurrentStream.WriteLine(String.Format(comment, args));
        }

        #endregion

        #region public override void WriteComment(string comment)

        /// <summary>
        /// Writes a comment with the specified message
        /// </summary>
        /// <param name="comment"></param>
        public override void WriteComment(string comment)
        {
            this.CurrentStream.Write(Constants.CommentPrePend);
            this.CurrentStream.Write(comment);
        }

        /// <summary>
        /// Writes a comment with the specified formatted comment and arguments to the current stream
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="args"></param>
        public override void WriteComment(string comment, params object[] args)
        {
            this.CurrentStream.Write(Constants.CommentPrePend);
            this.CurrentStream.Write(String.Format(comment, args));
        }

        #endregion

        #region public override PDFObjectRef BeginObject(string name)

        /// <summary>
        /// Begins a new indirect object returning a reference to the newly started indirect object
        /// </summary>
        /// <param name="name">The optional name of the object</param>
        /// <returns></returns>
        public override PDFObjectRef BeginObject(string name)
        {
            PDFIndirectObject obj = CreateIndirectObject();
            PDFObjectRef oref = this.InitializeIndirectObject(name, obj);

            if (this.TraceLog.ShouldLog(TraceLevel.Debug))
                this.Log(TraceLevel.Debug, "Begun a new indirect object: " + oref + (string.IsNullOrEmpty(name) ? "" : (" with name " + name)));

            return oref;
        }

        #endregion

        #region public override void EndObject()

        /// <summary>
        /// Ends the current indirect object and releases
        /// </summary>
        public override void EndObject()
        {
            PDFIndirectObject obj = this.Stack.Peek().IndirectObject;

            if (this.TraceLog.ShouldLog(TraceLevel.Debug))
                this.Log(TraceLevel.Debug, "Ended indirect object " + obj.Number + " " + obj.Generation + " R");
            
            this.ReleaseIndirectObject(obj);
        }

        #endregion

        #region public override void BeginStream(PDFObjectRef onobject, IStreamFilter[] filters)

        /// <summary>
        /// Begns a new data stream on the current indirect object and sets the stream as the new CurrentStream
        /// </summary>
        /// <param name="onobject"></param>
        /// <param name="filters"></param>
        public override void BeginStream(PDFObjectRef onobject, IStreamFilter[] filters)
        {
            PDFIndirectObject pio = onobject.Reference as PDFIndirectObject;
            pio.InitStream(filters);
            this.Stack.Push(pio.Stream);
        }

        #endregion

        #region public override long EndStream()

        /// <summary>
        /// Ends the current object data stream and pops it from the stack.
        /// </summary>
        /// <returns>The (optionally filtered) length of the stream</returns>
        public override long EndStream()
        {
            PDFStream str = this.Stack.Pop();
            str.Flush();
            if (str.HasFilters)
                return str.FilteredLength;
            else
                return str.Length;
        }

        #endregion

        public override void BeginDictionary()
        {
            CurrentStream.Write(Constants.StartDictionary);
            this.FinishedEntry = false;
        }

        public override void BeginDictionaryEntry(string name)
        {
            if (FinishedEntry)
                CurrentStream.Write(Constants.DictionaryNameValueSeparator);
            FinishedEntry = false;
            CurrentStream.Write(Constants.StartName);
            CurrentStream.Write(ValidateName(name));
            CurrentStream.Write(Constants.WhiteSpace);
        }

        
        public override void EndDictionaryEntry()
        {
            this.CurrentStream.Write(Constants.DictionaryEntrySeparator);
        }

        public override void EndDictionary()
        {
            this.FinishedEntry = false;
            CurrentStream.Write(Constants.EndDictionary);
        }


        public override void BeginArray()
        {
            CurrentStream.Write(Constants.StartArray);
            this.FinishedEntry = false;
        }

        public override void BeginArrayEntry()
        {
            if (FinishedEntry)
                CurrentStream.Write(Constants.ArrayEntrySeparator);
            this.FinishedEntry = false;
        }

        public override void EndArrayEntry()
        {
            CurrentStream.Write(Constants.ArrayEntrySeparator);
        }

        
        public override void EndArray()
        {
            this.FinishedEntry = false;
            CurrentStream.Write(Constants.EndArray);
        }


        public override void WriteByteString(string byteString)
        {
            CurrentStream.Write(Constants.StartHexString);
            CurrentStream.Write(byteString);
            CurrentStream.Write(Constants.EndHexString);
        }

        public override void WriteStringLiteral(string value, Scryber.FontEncoding encoding)
        {
            if (UseHex && encoding != FontEncoding.PDFDocEncoding)
            {
                //TODO Write encoding prefix
                CurrentStream.Write(Constants.StartHexString);
                value = ToHex(value, encoding);
                CurrentStream.Write(value);
                CurrentStream.Write(Constants.EndHexString);
            }
            else
            {
                Scryber.PDF.PDFEncoding enc = Scryber.PDF.PDFEncoding.GetEncoding(encoding);
                CurrentStream.Write(Constants.StartString);
                if (null != enc.Prefix && enc.Prefix.Length > 0)
                    CurrentStream.Write(enc.Prefix);
                CurrentStream.Write(enc.GetBytes(value));
                CurrentStream.Write(Constants.EndString);
            }
        }

        public override void WriteStringHex(byte[] value)
        {
            CurrentStream.Write(Constants.StartHexString);
            CurrentStream.Write(ToHex(value));
            CurrentStream.Write(Constants.EndHexString);
        }

        #region private string ToHex(string value, FontEncoding encoding)

        /// <summary>
        /// Converts a string with the specified font encoding to a byte[] 
        /// and then returns the Hexadecimal string of that byte array
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="encoding">The encoding of the value</param>
        /// <returns>A hexadecimal string representation</returns>
        private string ToHex(string value, FontEncoding encoding)
        {
            Scryber.PDF.PDFEncoding enc = Scryber.PDF.PDFEncoding.GetEncoding(encoding);
            byte[] all = enc.GetBytes(value);
            return ToHex(all);
        }

        #endregion

        #region private string ToHex(byte[] data)

        private static char[] _lookup = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private StringBuilder _buffer = new StringBuilder();
        private object _bufferInUse = new object();

        /// <summary>
        /// Converts the binary data to a hexadecimal string and returns the value
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToHex(byte[] data)
        {
            string result = string.Empty;

            lock (_bufferInUse)
            {
                //reset the local _buffer
                _buffer.Length = 0;
                int requiredCapacity = (data.Length * 2) + 2;
                _buffer.EnsureCapacity(requiredCapacity);

                //append the characters in turn from the lookup
                for (int buc = 0; buc < data.Length; buc++)
                {
                    _buffer.Append(_lookup[(data[buc] >> 4) & 0x0F]);
                    _buffer.Append(_lookup[data[buc] & 0x0F]);
                }

                result = _buffer.ToString();
            }

            return result;
        }

        #endregion

        #region public override void WriteBoolean(bool value)

        /// <summary>
        /// Writes a boolean value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteBoolean(bool value)
        {
            if (value)
                CurrentStream.Write("true");
            else
                CurrentStream.Write("false");
        }

        #endregion

        #region public override void WriteDate(DateTime value)

        /// <summary>
        /// Writes a date value in the appropriate format to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteDate(DateTime value)
        {
            string d = DateToString(value);
            this.WriteStringLiteral(d);
        }

        #endregion

        #region private string DateToString(DateTime value)

        /// <summary>
        /// Converts a .Net DateTime value to a string that a PDFReader will recognise
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string DateToString(DateTime value)
        {
            int dtLength = 22;
            string result;

            lock (_bufferInUse)
            {
                _buffer.Length = 0;
                _buffer.EnsureCapacity(dtLength);

                _buffer.Append(value.ToString("\\D:yyyyMMddhhmmss"));
                string offset = value.ToString("zzz");
                if (offset != "00:00")
                {
                    offset = offset.Replace(':', '\'');
                    _buffer.Append(offset);
                    _buffer.Append('\'');
                }
                result = _buffer.ToString();
            }
            return result;
        }

        #endregion

        #region public override void WriteNumber(int value) + 1 overload

        /// <summary>
        /// Writes an integer value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteNumber(int value)
        {
            CurrentStream.Write(value.ToString());
        }

        /// <summary>
        /// Writes a long value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteNumber(long value)
        {
            CurrentStream.Write(value.ToString());
        }

        #endregion

        #region public override void WriteReal(double value) + 2 overloads

        /// <summary>
        /// Writes a double value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteReal(double value)
        {
            CurrentStream.Write(value.ToString("F",System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes a decimal value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteReal(decimal value)
        {
            CurrentStream.Write(value.ToString("F", System.Globalization.CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Writes a real value to the current stream
        /// </summary>
        /// <param name="value"></param>
        public override void WriteReal(float value)
        {
            CurrentStream.Write(value.ToString("F", System.Globalization.CultureInfo.InvariantCulture));
        }

        #endregion

        #region public override void WriteName(string name)

        /// <summary>
        /// Writes a PDFName string to the current stream
        /// </summary>
        /// <param name="name"></param>
        public override void WriteName(string name)
        {
            CurrentStream.Write(Constants.StartName);
            CurrentStream.Write(ValidateName(name));
        }

        #endregion

        #region public override void WriteObjectRef(int number, int generation)

        /// <summary>
        /// Writes an object reference value to the current stream
        /// </summary>
        /// <param name="number"></param>
        /// <param name="generation"></param>
        public override void WriteObjectRef(int number, int generation)
        {
            this.CurrentStream.Write(number.ToString());
            this.CurrentStream.Write(" ");
            this.CurrentStream.Write(generation.ToString());
            this.CurrentStream.Write(" R");
        }

        #endregion

        #region public override void WriteNull()

        /// <summary>
        /// Writes a null string identifier to the current stream
        /// </summary>
        public override void WriteNull()
        {
            this.CurrentStream.Write(Constants.NullString);
        }

        #endregion

        #region public override void WriteFileObject(IFileObject obj)

        /// <summary>
        /// Writes the  file object to the current stream (by calling WriteData on the passed IFileObject)
        /// </summary>
        /// <param name="obj"></param>
        public override void WriteFileObject(IFileObject obj)
        {
            obj.WriteData(this);
        }

        #endregion

        #region public override void WriteRaw(string data)

        /// <summary>
        /// Writes the exact passed string to the current stream as is
        /// </summary>
        /// <param name="data"></param>
        public override void WriteRaw(string data)
        {
            this.CurrentStream.Write(data);
        }

        #endregion

        #region public override void WriteRaw(byte[] buffer, int offset, int length)

        /// <summary>
        /// Writes length bytes of the raw binary data to the stream, starting from the specified offset 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public override void WriteRaw(byte[] buffer, int offset, int length)
        {
            this.CurrentStream.Write(buffer, offset, length);
        }

        #endregion

        #region public override void WriteSpace()

        /// <summary>
        /// Writes a space value to the the current stream
        /// </summary>
        public override void WriteSpace()
        {
            this.CurrentStream.Write(Constants.WhiteSpace);
        }

        #endregion

        #region protected override string GetOpCode(PDFOpCode op)

        /// <summary>
        /// Returns the appropriate graphic operation characters for the PDFOpCode value
        /// </summary>
        /// <param name="op">The PDFOpCode to get the characters for</param>
        /// <returns>A string value that represents the op code</returns>
        protected override string GetOpCode(PDFOpCode op)
        {
            string s = String.Empty;

            switch (op)
            {
                case PDFOpCode.ColorSetFillSpace:
                    s = "cs";
                    break;
                case PDFOpCode.ColorSetStrokeSpace:
                    s = "CS";
                    break;
                case PDFOpCode.ColorFillPattern:
                    s = "scn";
                    break;
                case PDFOpCode.ColorStrokePattern:
                    s = "SCN";
                    break;
                case PDFOpCode.ColorFillGrayscaleSpace:
                    s = "g";
                    break;
                case PDFOpCode.ColorStrokeGrayscaleSpace:
                    s = "G";
                    break;
                case PDFOpCode.ColorFillRGBSpace:
                    s = "rg";
                    break;
                case PDFOpCode.ColorStrokeRGBSpace:
                    s = "RG";
                    break;
                case PDFOpCode.ColorFillCMYK:
                    s = "k";
                    break;
                case PDFOpCode.ColorStrokeCMYK:
                    s = "K";
                    break;
                case PDFOpCode.ColorFillOpacity:
                    s = "ca";
                    break;
                case PDFOpCode.ColorStrokeOpacity:
                    s = "CA";
                    break;
                case PDFOpCode.SaveState:
                    s = "q";
                    break;
                case PDFOpCode.RestoreState:
                    s = "Q";
                    break;
                case PDFOpCode.XobjPaint:
                    s = "Do";
                    break;
                case PDFOpCode.XobjBegin:
                    s = "BI";
                    break;
                case PDFOpCode.XobjImageData:
                    s = "ID";
                    break;
                case PDFOpCode.XobjEndImage:
                    s = "EI";
                    break;
                case PDFOpCode.MarkedContentBegin:
                    s = "BMC";
                    break;
                case PDFOpCode.MarkedContentEnd:
                    s = "EMC";
                    break;
                case PDFOpCode.TxtBegin:
                    s = "BT";
                    break;
                case PDFOpCode.TxtEnd:
                    s = "ET";
                    break;
                case PDFOpCode.TxtCharSpacing:
                    s = "Tc";
                    break;
                case PDFOpCode.TxtWordSpacing:
                    s = "Tw";
                    break;
                case PDFOpCode.TxtHScaling:
                    s = "Tz";
                    break;
                case PDFOpCode.TxtLeading:
                    s = "TL";
                    break;
                case PDFOpCode.TxtFont:
                    s = "Tf";
                    break;
                case PDFOpCode.TxtPaint:
                    s = "Tj";
                    break;
                case PDFOpCode.TxtPaintArray:
                    s = "TJ";
                    break;
                case PDFOpCode.TxtRenderMode:
                    s = "Tr";
                    break;
                case PDFOpCode.TxtRise:
                    s = "Ts";
                    break;
                case PDFOpCode.TxtMoveNextOffset:
                    s = "Td";
                    break;
                case PDFOpCode.TxtTransformMatrix:
                    s = "Tm";
                    break;
                case PDFOpCode.TxtNextLine:
                    s = "T*";
                    break;
                case PDFOpCode.GraphTransformMatrix:
                    s = "cm";
                    break;
                case PDFOpCode.GraphLineWidth:
                    s = "w";
                    break;
                case PDFOpCode.GraphLineCap:
                    s = "J";
                    break;
                case PDFOpCode.GraphLineJoin:
                    s = "j";
                    break;
                case PDFOpCode.GraphMiterLimit:
                    s = "M";
                    break;
                case PDFOpCode.GraphDashPattern:
                    s = "d";
                    break;
                case PDFOpCode.GraphRenderingIntent:
                    s = "ri";
                    break;
                case PDFOpCode.GraphFlatness:
                    s = "i";
                    break;
                case PDFOpCode.GraphApplyState:
                    s = "gs";
                    break;
                case PDFOpCode.GraphMove:
                    s = "m";
                    break;
                case PDFOpCode.GraphLineTo:
                    s = "l";
                    break;
                case PDFOpCode.GraphCurve2Handle:
                    s = "c";
                    break;
                case PDFOpCode.GraphCurve1HandleEnd:
                    s = "v";
                    break;
                case PDFOpCode.GraphCurve1HandleBegin:
                    s = "y";
                    break;
                case PDFOpCode.GraphClose:
                    s = "h";
                    break;
                case PDFOpCode.GraphRect:
                    s = "re";
                    break;
                case PDFOpCode.GraphStrokePath:
                    s = "S";
                    break;
                case PDFOpCode.GraphCloseAndStroke:
                    s = "s";
                    break;
                case PDFOpCode.GraphFillPath:
                    s = "f";
                    break;
                case PDFOpCode.GraphFillPathEvenOdd:
                    s = "f*";
                    break;
                case PDFOpCode.GraphFillAndStroke:
                    s = "B";
                    break;
                case PDFOpCode.GraphFillAndStrokeEvenOdd:
                    s = "B*";
                    break;
                case PDFOpCode.GraphCloseFillStroke:
                    s = "b";
                    break;
                case PDFOpCode.GraphCloseFileStrokeEvenOdd:
                    s = "b*";
                    break;
                case PDFOpCode.GraphNoOp:
                case PDFOpCode.GraphEndPath:
                    s = "n";
                    break;
                case PDFOpCode.GraphSetClip:
                    s = "W";
                    break;
                default:
                    break;
            }
            return s;
        }

        #endregion

        #region ValidateName and ValidateLiteral Methods

        private string ValidateName(string name)
        {
            //TODO: Increase validation support
            name = name.Replace(" ", "#20");
            return name;
        }

        private string ValidateLiteral(string value)
        {
            //TODO: Implement Literal Validation
            return value;
        }

        #endregion

        #region protected void Log(string message) + 2 overloads

        /// <summary>
        /// Logs a message to the current TraceLog with the default log level and category
        /// </summary>
        /// <param name="message"></param>
        protected void Log(string message)
        {
            this.Log(TraceDefaultLevel, TraceCategory, message);
        }

        /// <summary>
        /// Logs a message at the specified level to the current TraceLog with the default category
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        protected void Log(TraceLevel level, string message)
        {
            this.Log(level, TraceCategory, message);
        }

        /// <summary>
        /// Logs a message at the specified level, and with the specified category to the current TraceLog
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        protected virtual void Log(TraceLevel level, string category, string message)
        {
            if (null != this.TraceLog)
                this.TraceLog.Add(level, category, message);
        }

        #endregion

        #region protected override void Dispose(bool disposing)

        /// <summary>
        /// Overrides the base disposing method to dispose of any indirect object references
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
                if (null == this.XRefTable)
                    Log(TraceLevel.Message, TraceCategory, "No XRefTable to dispose");
                else
                {
                    Log(TraceLevel.Verbose, TraceCategory, "Disposing PDF writer XRef Table");
                    try
                    {
                        foreach (PDFXRefTableSection section in this.XRefTable.Sections)
                        {
                            foreach (PDFXRefTableEntry entry in section.Entries)
                            {
                                if (null != entry.Reference)
                                {
                                    IIndirectObject pfo = entry.Reference;
                                    Log(TraceLevel.Debug, "Disposing indirect object '" + pfo.ToString());
                                    pfo.Dispose();
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Log(TraceLevel.Error, TraceCategory, "Could not dispose of the PDFWriter14 : " + ex.ToString());
                    }
                }
            }
            
            base.Dispose(disposing);

        }

        #endregion
    }
}
