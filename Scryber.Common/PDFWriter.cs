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
using Scryber.Native;

namespace Scryber
{
    /// <summary>
    /// Abstract class that writes PDF data to a stream.
    /// </summary>
    public abstract class PDFWriter : IDisposable
    {
        #region Version {get;}
        /// <summary>
        /// Gets the Version information for the PDF file that will be written
        /// </summary>
        public abstract Version Version { get;}

        #endregion

        #region PDFXRefTable XRefTable {get;}

        private PDFXRefTable _xreftable;

        /// <summary>
        /// Gets the XRefTable that holds all references and their underlying stream positions (inheritors can set the value)
        /// </summary>
        protected PDFXRefTable XRefTable
        {
            get { return this._xreftable; }
            set { this._xreftable = value; }
        }

        #endregion

        #region public long Length {get;}

        /// <summary>
        /// Gets the length of the inner stream in bytes
        /// </summary>
        public long Length
        {
            get
            {
                return this.BaseStream.Length;
            }
        }

        #endregion

        #region protected internal PDFStream BaseStream {get;}

        private PDFStream _innerPDFStream;
        private System.IO.Stream _innerstream;

        /// <summary>
        /// Gets the underlying base stream that all data is eventually written to
        /// </summary>
        protected internal PDFStream BaseStream
        {
            get { return this._innerPDFStream; }
        }     
    

        public System.IO.Stream InnerStream
        {
            get { return _innerstream; }
        }

        #endregion

        #region Stack<PDFStream> Stack {get;}

        private Stack<PDFStream> _stack = new Stack<PDFStream>();

        /// <summary>
        /// Gets the current stack of stream data
        /// </summary>
        protected Stack<PDFStream> Stack
        {
            get { return this._stack; }
        }

        #endregion

        #region protected Stack<PDFObjectRef> ReferenceStack {get;}

        private Stack<PDFObjectRef> _refs = new Stack<PDFObjectRef>();

        protected Stack<PDFObjectRef> ReferenceStack
        {
            get { return _refs; }
        }

        #endregion

        #region protected PDFTraceLog TraceLog

        private PDFTraceLog _log;

        protected PDFTraceLog TraceLog
        {
            get { return _log; }
        }

        #endregion

        #region PDFStream CurrentStream {get;}
        /// <summary>
        /// Gets the curent text writer on the top of the stack, without modifying the stack
        /// </summary>
        protected PDFStream CurrentStream
        {
            get
            {
                if (this._stack.Count == 0)
                    throw new InvalidOperationException("There is no current object in the document");
                return this.Stack.Peek();
            }
        }

        #endregion

        #region IStreamFilter DefaultStreamFilters{get;}

        private IStreamFilter[] _deffil = new IStreamFilter[] { };

        /// <summary>
        /// Gets or sets the default filters for a data stream to use
        /// </summary>
        public IStreamFilter[] DefaultStreamFilters
        {
            get { return _deffil; }
            set { _deffil = value; }
        }

        #endregion

        #region public PDFPageReferenceCollection PageRefs {get;}

        private PDFPageReferenceCollection _pages = new PDFPageReferenceCollection();

        /// <summary>
        /// Gets a collection of page references
        /// </summary>
        public PDFPageReferenceCollection PageRefs
        {
            get { return _pages; }
        }

        #endregion

        #region public bool UseHex {get;set;}

        private bool _usehex = false;

        /// <summary>
        /// Gets or sets the flag to use hexadecimal notatation for the string literals (or simply to use the characters)
        /// </summary>
        public bool UseHex
        {
            get { return _usehex; }
            set { _usehex = value; }
        }

        #endregion

        #region public FontEncoding DefaultFontEncoding

        /// <summary>
        /// Use PDFDocEncoding as the default 
        /// </summary>
        private FontEncoding _defFontEnc = FontEncoding.PDFDocEncoding;

        /// <summary>
        /// Gets or sets the default encoding for the current writer
        /// </summary>
        public FontEncoding DefaultFontEncoding
        {
            get { return _defFontEnc; }
            set { _defFontEnc = value; }
        }

        #endregion

        //
        // ctor
        //

        #region protected PDFWriter(Stream stream, PDFXRefTable table, PDFTraceLog log)

        /// <summary>
        /// Initializes a new PDFWriter with the base stream and an XRef table instance
        /// </summary>
        /// <param name="stream">The underlying base stream</param>
        /// <param name="table">The XRef table to use</param>
        protected PDFWriter(Stream stream, PDFTraceLog log)
        {
            if (null == stream)
                throw new ArgumentNullException("stream");
            if (stream.CanWrite == false)
                throw new ArgumentException(CommonErrors.CannotWriteToThisStream, "stream");

            this._innerstream = stream;

            if (stream.CanSeek == false)
            {
                this._innerPDFStream = new PDFSeekableStreamWrapper(stream, this.DefaultStreamFilters, null, false);
            }
            else
            {
                this._innerPDFStream = new PDFStream(stream, this.DefaultStreamFilters, null, false);
            }

            this.Stack.Push(_innerPDFStream);
            if (null == _log)
                _log = new Scryber.Logging.DoNothingTraceLog(Scryber.TraceRecordLevel.Off);
            this._log = log;
        }

        #endregion


        #region ObjectDictionary + GetObjectRef + TryGetObjectRef

        protected class PDFObjectRefDictionary : Dictionary<string, PDFObjectRef>
        {
        }

        private PDFObjectRefDictionary _keyedobjects = new PDFObjectRefDictionary();
        
        /// <summary>
        /// Gets the dictionary of named objects and their object references
        /// </summary>
        protected PDFObjectRefDictionary ObjectDictionary
        {
            get { return this._keyedobjects; }
        }

        /// <summary>
        /// Gets a PDFObjectRef for the specified name in the Object Dictionary.
        /// </summary>
        /// <param name="name">The name of the object to retrieve</param>
        /// <returns>The object reference, or null if not found</returns>
        public PDFObjectRef GetObjectRef(string name)
        {
            PDFObjectRef oref;
            this.TryGetObjectRef(name, out oref);
            return oref;
        }
        /// <summary>
        /// Tries to get a PDFObjectRef for a specified name in the object dictionary.
        /// </summary>
        /// <param name="name">The name of the object to retrieve</param>
        /// <param name="oref">The reference to the object if found</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetObjectRef(string name, out PDFObjectRef oref)
        {
            return this._keyedobjects.TryGetValue(name, out oref);
        }

        #endregion

        #region protected void InitXRefTable(int startindex, int generation, PDFXRefTable previous)

        /// <summary>
        /// Initializes this writers XRefTable with the startindex for any new 
        /// indirect objects, the base generation number, and a Previous XRefTable if any
        /// </summary>
        /// <param name="startindex"></param>
        /// <param name="generation"></param>
        /// <param name="previous"></param>
        protected void InitXRefTable(int startindex, int generation, PDFXRefTable previous)
        {
            this._xreftable = new PDFXRefTable(generation, startindex, previous);
        }

        #endregion

        #region protected InitializeIndirectObject + ReleaseIndirectObject

        /// <summary>
        /// Initializes a new indirect object, adding it to the XRefTable, and ObjectDictionary if it has a name, and pushing its TextWriter onto the stack
        /// </summary>
        /// <param name="uniquename">A uniquename for the object</param>
        /// <param name="obj">The object to initialize</param>
        /// <returns>A PDFObjectRef to identify the object in the rest of the document</returns>
        protected virtual PDFObjectRef InitializeIndirectObject(string name, IIndirectObject obj)
        {
            this.XRefTable.Append(obj);
            
            this.Stack.Push(obj.ObjectData);
            PDFObjectRef oref = new PDFObjectRef(obj);
            this.ReferenceStack.Push(oref);

            if(String.IsNullOrEmpty(name) == false)
                this.ObjectDictionary.Add(name, oref);

            return oref;
        }

        /// <summary>
        /// Releases the the current object from the PDFStream stack.
        /// </summary>
        protected virtual void ReleaseIndirectObject(IIndirectObject obj)
        {
            this.Stack.Pop();
            this.ReferenceStack.Pop();
        }

        #endregion

        #region OpenDocument() + OpenDocument(PDFFile orig, bool copytoDestination) + CloseDocument()

        /// <summary>
        /// Opens a new PDFDocument file
        /// </summary>
        public abstract void OpenDocument();

        /// <summary>
        /// Opens an existing pdf document ready for appending to
        /// </summary>
        /// <param name="orig">The original document to update</param>
        /// <param name="copytoDestination">If true then the entire PDFFile should be included at the top of this writers base stream)</param>
        public abstract void OpenDocument(PDFFile orig, bool copytoDestination);

        /// <summary>
        /// Closes the current PDF Document file writing the document identifiers in the trailer if supplied
        /// </summary>
        /// <param name="documentid">The document id's to write in the files trailer</param>
        public abstract void CloseDocument(PDFDocumentID id);

        

        #endregion

        #region public PDFObjectRef BeginPage(int pgIndex) + public void EndPage(int pgIndex)

        /// <summary>
        /// Begins a new Page object in the underlying writer with the 
        /// specified ZERO-BASED index and returns a reference to that page object
        /// </summary>
        /// <param name="pgIndex"></param>
        /// <returns></returns>
        public virtual PDFObjectRef BeginPage(int pgIndex)
        {
            PDFObjectRef oref = this.BeginObject();
            this.PageRefs.Add(pgIndex, oref);
            return oref;
        }

        /// <summary>
        /// Ends the current page
        /// </summary>
        /// <param name="pgIndex"></param>
        public virtual void EndPage(int pgIndex)
        {
            this.EndObject();
        }

        #endregion

        #region BeginObject() + BeginObject(name) + EndObject() + GetLastObject()

        /// <summary>
        /// Begins a new IndirectObject automatically generating a unique name. This object will become the CurrentObject where data is written to.
        /// </summary>
        /// <returns>A reference to the newly created object</returns>
        public PDFObjectRef BeginObject()
        {
            return this.BeginObject(string.Empty);
        }

        /// <summary>
        /// Begins a new IndirectObject with the specified (unique) name. This object will become current (where write operation data is sent to)
        /// </summary>
        /// <param name="name">The unique name of the object</param>
        /// <returns>A reference to the newly created object</returns>
        public abstract PDFObjectRef BeginObject(string name);
        
        /// <summary>
        /// Ends the CurrentObject and closes any data streams. The previously created object or stream (if any) will become current.
        /// </summary>
        public abstract void EndObject();

        /// <summary>
        /// Gets a reference to the last object that was begun (and not ended).
        /// </summary>
        /// <returns></returns>
        public PDFObjectRef LastObjectReference()
        {
            return this.ReferenceStack.Peek();
        }

        

        #endregion

        #region BeginStream(onobject) + BeginStream(onobject, filters) + EndStream()

        /// <summary>
        /// Begins a new Data Stream on the specified object using the DefaultStreamFilters. This stream will become current (where write operation data is sent to)
        /// </summary>
        /// <param name="onobject"></param>
        public void BeginStream(PDFObjectRef onobject)
        {
            this.BeginStream(onobject, this.DefaultStreamFilters);
        }

        /// <summary>
        /// Begins a new Data Stream on the specified object using the specified filters. This stream will become current (where write operation data is sent to)
        /// </summary>
        /// <param name="onobject">The indirect object reference to begin the stream on</param>
        /// <param name="filters">The array of filters to apply to the data</param>
        public abstract void BeginStream(PDFObjectRef onobject, IStreamFilter[] filters);

        /// <summary>
        /// Ends the current stream and closes it. This previously created object or stream (if any) will become current.
        /// </summary>
        /// <returns></returns>
        public abstract long EndStream();

        #endregion

        #region WriteComment() + WriteCommentLine()

        /// <summary>
        /// Writes a comment to the current stream. Further comments can be appended until the comment line is closed
        /// </summary>
        /// <param name="comment">The comment to write</param>
        public abstract void WriteComment(string comment);

        /// <summary>
        /// Writes a formatted comment to the current stream. Further comments can be appended until the comment line is closed
        /// </summary>
        /// <param name="comment">the formattable comment to write</param>
        /// <param name="args">The parameters to apply to the format</param>
        public abstract void WriteComment(string comment, params object[] args);

        /// <summary>
        /// Writes a complete empty comment line to the current stream
        /// </summary>
        public abstract void WriteCommentLine();

        /// <summary>
        /// Writes a complete comment to the current stream
        /// </summary>
        /// <param name="comment">The comment to write</param>
        public abstract void WriteCommentLine(string comment);

        /// <summary>
        /// Writes a complete formatted comment to the current stream
        /// </summary>
        /// <param name="comment">the formattable comment to write</param>
        /// <param name="args">The parameters to apply to the format</param>
        public abstract void WriteCommentLine(string comment, params object[] args);

        #endregion

        #region BeginArray() + EndArray() + BeginArrayEntry() + EndArrayEntry()

        /// <summary>
        /// Begins a new data array on the current stream.
        /// </summary>
        public abstract void BeginArray();

        /// <summary>
        /// Writes a space then begins a new array
        /// </summary>
        public void BeginArrayS()
        {
            this.WriteSpace();
            this.BeginArray();
        }

        /// <summary>
        /// Ends the array on the current stream
        /// </summary>
        public abstract void EndArray();

        /// <summary>
        /// Writes a space then ends the current array
        /// </summary>
        public void EndArrayS()
        {
            this.WriteSpace();
            this.EndArray();
        }

        /// <summary>
        /// Begins a new Array entry on the stream
        /// </summary>
        public abstract void BeginArrayEntry();

        /// <summary>
        /// Ends the current array entry on the stream
        /// </summary>
        public abstract void EndArrayEntry();

        #endregion

        #region WriteArrayXXXEntries

        /// <summary>
        /// Begins a new array, writes the numbers and then closes the array
        /// </summary>
        /// <param name="numbers">The numbers to write</param>
        public void WriteArrayNumberEntries(params int[] numbers)
        {
            this.WriteArrayNumberEntries(true, numbers);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the numbers to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="numbers">The numbers to write</param>
        public void WriteArrayNumberEntries(bool createarray, params int[] numbers)
        {
            if(createarray)
                this.BeginArrayS();

            foreach (int f in numbers)
            {
                this.BeginArrayEntry();
                this.WriteNumber(f);
                this.EndArrayEntry();
            }

            if(createarray)
                this.EndArray();
        }


        /// <summary>
        /// Begins a new array, writes the numbers and then closes the array
        /// </summary>
        /// <param name="numbers">The numbers to write</param>
        public void WriteArrayNumberEntries(params long[] numbers)
        {
            this.WriteArrayNumberEntries(true, numbers);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the numbers to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="numbers">The numbers to write</param>
        public void WriteArrayNumberEntries(bool createarray, params long[] numbers)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (long f in numbers)
            {
                this.BeginArrayEntry();
                this.WriteNumber(f);
                this.EndArrayEntry();
            }
            
            if(createarray)
                this.EndArray();
        }

        public void WriteArrayNumberEntries(bool createarray, params PDFNumber[] numbers)
        {
            if (createarray)
                this.BeginArrayS();

            foreach (PDFNumber num in numbers)
            {
                this.BeginArrayEntry();
                this.WriteFileObject(num);
                this.EndArrayEntry();
            }

            if (createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the numbers and then closes the array
        /// </summary>
        /// <param name="reals">The real numbers to write</param>
        public void WriteArrayRealEntries(params float[] reals)
        {
            this.WriteArrayRealEntries(true, reals);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the numbers to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="reals">The numbers to write</param>
        public void WriteArrayRealEntries(bool createarray, params float[] reals)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (float f in reals)
            {
                this.BeginArrayEntry();
                this.WriteReal((decimal)f);
                this.EndArrayEntry();
            }
            
            if(createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the numbers and then closes the array
        /// </summary>
        /// <param name="reals">The real numbers to write</param>
        public void WriteArrayRealEntries(params double[] reals)
        {
            this.WriteArrayRealEntries(true, reals);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the numbers to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="reals">The numbers to write</param>
        public void WriteArrayRealEntries(bool createarray, params double[] reals)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (double f in reals)
            {
                this.BeginArrayEntry();
                this.WriteReal((decimal)f);
                this.EndArrayEntry();
            }
            if(createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the numbers and then closes the array
        /// </summary>
        /// <param name="reals">The numbers to write</param>
        public void WriteArrayRealEntries(params decimal[] reals)
        {
            this.WriteArrayRealEntries(true, reals);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the numbers to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="reals">The numbers to write</param>
        public void WriteArrayRealEntries(bool createarray, params decimal[] reals)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (decimal f in reals)
            {
                this.BeginArrayEntry();
                this.WriteReal(f);
                this.EndArrayEntry();
            }
            if(createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the strings and then closes the array
        /// </summary>
        /// <param name="literals">The strings to write</param>
        public void WriteArrayStringEntries(params string[] literals)
        {
            this.WriteArrayStringEntries(true, literals);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the literals to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="literals">The strings to write</param>
        public void WriteArrayStringEntries(bool createarray, params string[] literals)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (string f in literals)
            {
                this.BeginArrayEntry();
                this.WriteStringLiteral(f);
                this.EndArrayEntry();
            }

            if(createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the names and then closes the array
        /// </summary>
        /// <param name="names">The names to write</param>
        public void WriteArrayNameEntries(params string[] names)
        {
            this.WriteArrayNameEntries(true, names);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the names to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="names">The names to write</param>
        public void WriteArrayNameEntries(bool createarray, params string[] names)
        {
            if(createarray)
                this.BeginArrayS();
            
            foreach (string f in names)
            {
                this.BeginArrayEntry();
                this.WriteName(f);
                this.EndArrayEntry();
            }

            if(createarray)
                this.EndArray();
        }

        /// <summary>
        /// Begins a new array, writes the names and then closes the array
        /// </summary>
        /// <param name="names">The names to write</param>
        public void WriteArrayNameEntries(params PDFName[] names)
        {
            this.WriteArrayNameEntries(true, names);
        }

        /// <summary>
        /// Optionally creates a new array and then writes the names to the array, closing it if opened
        /// </summary>
        /// <param name="createarray">True if a new array should be created</param>
        /// <param name="names">The names to write</param>
        public void WriteArrayNameEntries(bool createarray, params PDFName[] names)
        {
            if (createarray)
                this.BeginArrayS();

            foreach (PDFName f in names)
            {
                this.BeginArrayEntry();
                this.WriteName(f.Value);
                this.EndArrayEntry();
            }

            if (createarray)
                this.EndArray();
        }

        public void WriteArrayRefEntries(params PDFObjectRef[] refs)
        {
            this.WriteArrayRefEntries(true, refs);
        }

        public void WriteArrayRefEntries(bool createarray, params PDFObjectRef[] refs)
        {
            if (createarray)
                this.BeginArrayS();

            foreach (PDFObjectRef f in refs)
            {
                this.BeginArrayEntry();
                this.WriteObjectRef(f);
                this.EndArrayEntry();
            }

            if (createarray)
                this.EndArray();
        }
        #endregion

        
        #region BeginDictionary() + EndDictionary() + BeginDictionaryEntry() + EndDictionaryEntry()

        /// <summary>
        /// Begins a new dictoinary on the current stream
        /// </summary>
        public abstract void BeginDictionary();

        /// <summary>
        /// Writes a space then begins a new dictionary
        /// </summary>
        public void BeginDictionaryS()
        {
            this.WriteSpace();
            this.BeginDictionary();
        }

        public void BeginDictionaryEntry(PDFName name)
        {
            this.BeginDictionaryEntry(name.Value);
        }
        /// <summary>
        /// Begins a new dictionary entry with the specified name on the current stream
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        public abstract void BeginDictionaryEntry(string name);

        /// <summary>
        /// Ends the current dictionary
        /// </summary>
        public abstract void EndDictionary();


        /// <summary>
        /// Ends the current dictionary entry
        /// </summary>
        public abstract void EndDictionaryEntry();

        #endregion

        #region WriteDictionaryXXXEntry

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryNumberEntry(string name, int value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteNumber(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryNumberEntry(string name, long value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteNumber(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryRealEntry(string name, float value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteReal(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryRealEntry(string name, double value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteReal(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryRealEntry(string name, decimal value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteReal(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryStringEntry(string name, string value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteStringLiteral(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryBooleanEntry(string name, bool value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteBoolean(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryNameEntry(string name, string value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteName(value);
            this.EndDictionaryEntry();
        }

        /// <summary>
        /// Writes a new dictionary entry with the specified name and value
        /// </summary>
        /// <param name="name">The name of the dictionary entry</param>
        /// <param name="value">The dictionary entry value</param>
        public void WriteDictionaryObjectRefEntry(string name, PDFObjectRef value)
        {
            this.BeginDictionaryEntry(name);
            this.WriteObjectRef(value);
            this.EndDictionaryEntry();
        }

        #endregion


        #region WriteSpace() + WriteLine()

        /// <summary>
        /// Writes a blank space in the current stream
        /// </summary>
        public abstract void WriteSpace();

        /// <summary>
        /// Writes or ends the current line, moving to the next line to start the data entry.
        /// </summary>
        public abstract void WriteLine();

        #endregion


        #region WriteOp (S)

        /// <summary>
        /// Writes a string operation code to the current stream
        /// </summary>
        /// <param name="op">The operation code to write</param>
        public void WriteOp(string op)
        {
            this.WriteRaw(op);
        }

        /// <summary>
        /// Writes a space, then the string operation code to the curent stream
        /// </summary>
        /// <param name="op">The operation code to write</param>
        public void WriteOpS(string op)
        {
            this.WriteSpace();
            this.WriteRaw(op);
        }

        #endregion

        #region WriteOpCode(S) x 6

        /// <summary>
        /// Writes the string representation of the PDFOpCode to the current stream
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        public void WriteOpCode(PDFOpCode op)
        {
            this.WriteOp(this.GetOpCode(op));
        }

        /// <summary>
        /// Writes a space, then the string representation of the PDFOpCode to the Current stream
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        public void WriteOpCodeS(PDFOpCode op)
        {
            this.WriteSpace();
            this.WriteOpCode(op);
        }

        /// <summary>
        /// Writes a space, then the parameters and string representation of the PDFOpCode to the Current stream.
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        /// <param name="param">The file object parameter for the opcode</param>
        public void WriteOpCodeS(PDFOpCode op, IFileObject param)
        {
            this.WriteSpace();
            this.WriteFileObject(param);
            this.WriteSpace();
            this.WriteOp(this.GetOpCode(op));
        }

        /// <summary>
        /// Writes a space, then the parameters and string representation of the PDFOpCode to the current stream
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        /// <param name="param1">The first file object parameter for the opcode</param>
        /// <param name="param2">The second file object parameter for the opcode</param>
        public void WriteOpCodeS(PDFOpCode op, IFileObject param1, IFileObject param2)
        {
            this.WriteSpace();
            this.WriteFileObject(param1);
            this.WriteSpace();
            this.WriteFileObject(param2);
            this.WriteSpace();
            this.WriteOp(this.GetOpCode(op));
        }
        
        /// <summary>
        /// Writes a space, then the parameters and string representation of the PDFOpCode to the current stream
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        /// <param name="param1">The first file object parameter for the opcode</param>
        /// <param name="param2">The second file object parameter for the opcode</param>
        /// <param name="param3">The third file object parameter for the opcode</param>
        public void WriteOpCodeS(PDFOpCode op, IFileObject param1, IFileObject param2, IFileObject param3)
        {
            this.WriteSpace();
            this.WriteFileObject(param1);
            this.WriteSpace();
            this.WriteFileObject(param2);
            this.WriteSpace();
            this.WriteFileObject(param3);
            this.WriteSpace();
            this.WriteOp(this.GetOpCode(op));
        }

        /// <summary>
        /// Writes a space, then the parameters and string representation of the PDFOpCode to the current stream
        /// </summary>
        /// <param name="op">The PDFOpCode to write the string representation of</param>
        /// <param name="parameters">The list op parameters to write</param>
        public void WriteOpCodeS(PDFOpCode op, params IFileObject[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                foreach (IFileObject p in parameters)
                {
                    this.WriteSpace();
                    this.WriteFileObject(p);
                }
            }
            this.WriteSpace();
            this.WriteOp(this.GetOpCode(op));
        }

        #endregion

        #region protected string GetOpCode(PDFOpcode op)
        /// <summary>
        /// Returns the string representation of the PDFOpcode
        /// </summary>
        /// <param name="op">The PDFOpcode to get teh string representation of.</param>
        /// <returns>A operation string representation</returns>
        protected abstract string GetOpCode(PDFOpCode op);

        #endregion

        #region WriteString (S)

        /// <summary>
        /// Writes the string literal to the Current stream
        /// </summary>
        /// <param name="value">The string to literal to write</param>
        public abstract void WriteStringLiteral(string value, FontEncoding encoding);

        //public abstract void WriteStringLiteral(StringBuilder value, int startindex, int length, FontEncoding encoding);

        //public void WriteStringLiteral(StringBuilder value, FontEncoding encoding)
        //{
        //    this.WriteStringLiteral(value, 0, value.Length, encoding);
        //}

        //public void WriteStringLiteral(StringBuilder value)
        //{
        //    this.WriteStringLiteral(value, 0, value.Length, DefaultFontEncoding);
        //}

        public void WriteStringLiteral(string value)
        {
            this.WriteStringLiteral(value, DefaultFontEncoding);
        }

        /// <summary>
        /// Writes a space then the string literal to the current stream.
        /// </summary>
        /// <param name="value">The string literal to write</param>
        public void WriteStringLiteralS(string value)
        {
            this.WriteSpace();
            this.WriteStringLiteral(value);
        }

        public void WriteStringLiteralS(string value, FontEncoding encoding)
        {
            this.WriteSpace();
            this.WriteStringLiteral(value, encoding);
        }

        //public void WriteStringLiteralS(StringBuilder value, FontEncoding encoding)
        //{
        //    this.WriteSpace();
        //    this.WriteStringLiteral(value, 0, value.Length, encoding);
        //}

        //public void WriteStringLiteralS(StringBuilder value)
        //{
        //    this.WriteSpace();
        //    this.WriteStringLiteral(value, 0, value.Length, DefaultFontEncoding);
        //}

        #endregion

        #region public void WriteByteString (S)

        /// <summary>
        /// Writes the byte string to the current stream
        /// </summary>
        /// <param name="byteString"></param>
        public abstract void WriteByteString(string byteString);

       
        /// <summary>
        /// Writes a space and then writes the byte string to the current stream
        /// </summary>
        /// <param name="byteString"></param>
        public void WriteByteStringS(string byteString)
        {
            this.WriteSpace();
            this.WriteByteString(byteString);
        }

        #endregion

        #region public void WriteStringHex(byte[] value)

        /// <summary>
        /// Writes the bytes as a hexadecimal string
        /// </summary>
        /// <param name="value"></param>
        public abstract void WriteStringHex(byte[] value);

        #endregion

        #region WriteBoolean (S)

        /// <summary>
        /// Writes the boolean value to the current stream
        /// </summary>
        /// <param name="value">The boolean value to write</param>
        public abstract void WriteBoolean(bool value);

        /// <summary>
        /// Writes a space then the boolean value to the current stream
        /// </summary>
        /// <param name="value">the boolean value to write</param>
        public void WriteBooleanS(bool value)
        {
            this.WriteSpace();
            this.WriteBoolean(value);
        }

        #endregion

        #region public void WriteDate (S)

        /// <summary>
        /// Wriate a date value in PDF Date time format
        /// </summary>
        /// <param name="value"></param>
        public abstract void WriteDate(DateTime value);

        /// <summary>
        /// Writes a space then writes the date value in PDF date format
        /// </summary>
        /// <param name="value"></param>
        public void WriteDateS(DateTime value)
        {
            this.WriteSpace();
            this.WriteDate(value);
        }

        #endregion

        #region WriteNumber (S) x 8

        /// <summary>
        /// Writes an integral value to the curent stream
        /// </summary>
        /// <param name="value">The integral value to write</param>
        public abstract void WriteNumber(long value);

        /// <summary>
        /// Writes an integral value to the curent stream
        /// </summary>
        /// <param name="value">The integral value to write</param>
        public abstract void WriteNumber(int value);

        /// <summary>
        /// Writes a space then an integral value to the curent stream
        /// </summary>
        /// <param name="value">The integral value to write</param>
        public void WriteNumberS(long value)
        {
            this.WriteSpace();
            this.WriteNumber(value);
        }

        /// <summary>
        /// Writes a space then an integral value to the curent stream
        /// </summary>
        /// <param name="value">The integral value to write</param>
        public void WriteNumberS(int value)
        {
            this.WriteSpace();
            this.WriteNumber(value);
        }

        /// <summary>
        /// Writes a space then all integral values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">the first value to write</param>
        /// <param name="value2">the second value to write</param>
        public void WriteNumberS(long value, long value2)
        {
            this.WriteSpace();
            this.WriteNumber(value);
            this.WriteSpace();
            this.WriteNumber(value2);
        }

        /// <summary>
        /// Writes a space then all integral values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">the first value to write</param>
        /// <param name="value2">the second value to write</param>
        public void WriteNumberS(int value, int value2)
        {
            this.WriteSpace();
            this.WriteNumber(value);
            this.WriteSpace();
            this.WriteNumber(value2);
        }

        /// <summary>
        /// Writes a space then all integral values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">the first value to write</param>
        /// <param name="value2">the second value to write</param>
        /// <param name="value3">the third value to write</param>
        public void WriteNumberS(long value, long value2, long value3)
        {
            this.WriteSpace();
            this.WriteNumber(value);
            this.WriteSpace();
            this.WriteNumber(value2);
            this.WriteSpace();
            this.WriteNumber(value3);
        }

        /// <summary>
        /// Writes a space then all integral values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">the first value to write</param>
        /// <param name="value2">the second value to write</param>
        /// <param name="value3">the third value to write</param>
        public void WriteNumberS(int value, int value2, long value3)
        {
            this.WriteSpace();
            this.WriteNumber(value);
            this.WriteSpace();
            this.WriteNumber(value2);
            this.WriteSpace();
            this.WriteNumber(value3);
        }

        #endregion

        #region WriteReal (S) x 12

        /// <summary>
        /// Writes a real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public abstract void WriteReal(double value);

        /// <summary>
        /// Writes a real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public abstract void WriteReal(float value);

        /// <summary>
        /// Writes a real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public abstract void WriteReal(decimal value);

        /// <summary>
        /// Writes a space then the real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public void WriteRealS(double value)
        {
            this.WriteSpace();
            this.WriteReal(value);
        }

        /// <summary>
        /// Writes a space then the real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public void WriteRealS(float value)
        {
            this.WriteSpace();
            this.WriteReal(value);
        }

        /// <summary>
        /// Writes a space then the real value to the curent stream
        /// </summary>
        /// <param name="value">The real value to write</param>
        public void WriteRealS(decimal value)
        {
            this.WriteSpace();
            this.WriteReal(value);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        public void WriteRealS(double value, double value2)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        public void WriteRealS(float value, float value2)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        public void WriteRealS(decimal value, decimal value2)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        /// <param name="value3">The third real value to write</param>
        public void WriteRealS(double value, double value2, double value3)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
            this.WriteSpace();
            this.WriteReal(value3);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        /// <param name="value3">The third real value to write</param>
        public void WriteRealS(float value, float value2, float value3)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
            this.WriteSpace();
            this.WriteReal(value3);
        }

        /// <summary>
        /// Writes a space then the real values (separated by spaces) to the curent stream
        /// </summary>
        /// <param name="value">The first real value to write</param>
        /// <param name="value2">The second real value to write</param>
        /// <param name="value3">The third real value to write</param>
        public void WriteRealS(decimal value, decimal value2, decimal value3)
        {
            this.WriteSpace();
            this.WriteReal(value);
            this.WriteSpace();
            this.WriteReal(value2);
            this.WriteSpace();
            this.WriteReal(value3);
        }

        #endregion

        #region WriteName (S) x4

        /// <summary>
        /// Writes a name to the current stream
        /// </summary>
        /// <param name="name">The name to write</param>
        public abstract void WriteName(string name);

        /// <summary>
        /// Writes a space then a name to the current stream
        /// </summary>
        /// <param name="name">The name to write</param>
        public void WriteNameS(string name)
        {
            this.WriteSpace();
            this.WriteName(name);
        }

        /// <summary>
        /// Writes a space then the names (separated by a space) to the current stream
        /// </summary>
        /// <param name="name">The first name to write</param>
        /// <param name="name2">The second name to write</param>
        public void WriteNameS(string name, string name2)
        {
            this.WriteSpace();
            this.WriteName(name);
            this.WriteSpace();
            this.WriteName(name2);
        }

        /// <summary>
        /// Writes a space then the names (separated by a space) to the current stream
        /// </summary>
        /// <param name="name">The first name to write</param>
        /// <param name="name2">The second name to write</param>
        /// <param name="name3">The third name to write</param>
        public void WriteNameS(string name, string name2, string name3)
        {
            this.WriteSpace();
            this.WriteName(name);
            this.WriteSpace();
            this.WriteName(name2);
            this.WriteSpace();
            this.WriteName(name3);
        }

        #endregion

        #region WriteObjectRef (S)

        /// <summary>
        /// Writes an object reference to the current stream
        /// </summary>
        /// <param name="reference">The reference to write</param>
        public void WriteObjectRef(PDFObjectRef reference)
        {
            this.WriteObjectRef(reference.Number, reference.Generation);
        }

        public abstract void WriteObjectRef(int number, int generation);

        /// <summary>
        /// Writes a space then an indirect object reference to the current stream
        /// </summary>
        /// <param name="reference">The reference to write</param>
        public void WriteObjectRefS(PDFObjectRef reference)
        {
            this.WriteSpace();
            this.WriteObjectRef(reference);
        }

        public void WriteObjectRefS(int number, int generation)
        {
            this.WriteSpace();
            this.WriteObjectRef(number, generation);
        }

        #endregion

        #region WriteFileObject (S)

        /// <summary>
        /// Writes one of the file objects (PDFBoolean, PDFName, PDFNumber etc) to the current stream
        /// </summary>
        /// <param name="obj">The file object to write</param>
        public abstract void WriteFileObject(IFileObject obj);

        /// <summary>
        /// Writes a space then one of the file objects (PDFBoolean, PDFName, PDFNumber etc) to the current stream
        /// </summary>
        /// <param name="obj">The file object to write</param>
        public void WriteFileObjectS(IFileObject obj)
        {
            this.WriteSpace();
            this.WriteFileObject(obj);
        }

        #endregion

        #region WiteNull (S)

        /// <summary>
        /// Writes the PDFNull value to the current stream.
        /// </summary>
        public abstract void WriteNull();

        /// <summary>
        /// Writes a space then null to the current stream.
        /// </summary>
        public void WriteNullS()
        {
            this.WriteSpace();
            this.WriteNull();
        }

        #endregion

        #region WriteRaw

        /// <summary>
        /// Writes the raw string to the current stream
        /// </summary>
        /// <param name="data">The string data to write</param>
        public abstract void WriteRaw(string data);

        /// <summary>
        /// Writes the raw binary data to write to the stream
        /// </summary>
        /// <param name="data">The raw dat a to write</param>
        /// <param name="offset">The position at which to begin copying the data from</param>
        /// <param name="length">The number of bytes to copy</param>
        public abstract void WriteRaw(byte[] data, int offset, int length);

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._innerPDFStream != null)
                {
                    this._innerPDFStream.Dispose();
                    this._innerPDFStream = null;
                }
            }
        }

        ~PDFWriter()
        {
            this.Dispose(false);
        }

        #endregion

    }
}
