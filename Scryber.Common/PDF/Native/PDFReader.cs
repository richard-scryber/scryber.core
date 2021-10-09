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
using System.IO;
using System.Linq;
using System.Text;
using Scryber.Logging;

namespace Scryber.PDF.Native
{
    /// <summary>
    /// Abstract base class that supports a seekable PDF reader, and can load information and objects from a raw PDFFile
    /// </summary>
    public abstract class PDFReader
    {
        //
        // public properties
        //

        #region public abstract PDFXRefTable XRefTable {get;}

        /// <summary>
        /// Gets the XRefTable in the PDFDocument
        /// </summary>
        public abstract PDFXRefTable XRefTable { get; protected set; }

        #endregion

        #region public abstract PDFDictionary DocumentCatalog {get;}

        /// <summary>
        /// Gets the reference to document catalog from the reader
        /// </summary>
        public abstract PDFFileIndirectObject DocumentCatalogRef { get; protected set; }

        #endregion

        #region public abstract PDFFileIndirectObject DocumentInfoRef {get;}

        /// <summary>
        /// Gets the document info from the reader.
        /// </summary>
        public abstract PDFFileIndirectObject DocumentInfoRef { get; protected set; }

        #endregion

        #region public abstract PDFDictionary DocumentTrailer { get; }

        /// <summary>
        /// Gets the document trailer dictionary for the reader file stream
        /// </summary>
        public abstract PDFDictionary DocumentTrailer { get; protected set; }

        #endregion

        #region public abstract System.IO.Stream InnerStream { get; }

        /// <summary>
        /// Gets the underlying stream this reader is using
        /// </summary>
        public abstract System.IO.Stream InnerStream { get; }

        #endregion

        //
        // .ctor(s)
        //

        #region protected PDFReader()

        /// <summary>
        /// Protected empty constructor
        /// </summary>
        protected PDFReader()
        {
        }

        #endregion

        //
        // public methods
        //

        #region public abstract IParsedIndirectObject GetObject(PDFObjectRef oref);

        /// <summary>
        /// Returns the parsed indirect object that corresponds to the reference provided.
        /// </summary>
        /// <param name="oref"></param>
        /// <returns></returns>
        public abstract IParsedIndirectObject GetObject(PDFObjectRef oref);

        #endregion

        //
        // abstract methods
        //

        /// <summary>
        /// Allows the instance to build any required data and perform and initialization.
        /// </summary>
        /// <param name="log"></param>
        protected abstract void InitData(TraceLog log);

        //
        // factory method
        //

        #region public static PDFReader Create(System.IO.Stream seekableStream)

        private const int HeaderByteLength = 8;
        private const int MajorVersionOffset = 5;
        private const int MinorVersionOffset = 7;
        /// <summary>
        /// Creates a new PDFReader for the specified stream (which must be seekable)
        /// </summary>
        /// <param name="seekableStream"></param>
        /// <returns></returns>
        public static PDFReader Create(System.IO.Stream seekableStream, TraceLog log)
        {
            if (seekableStream.CanSeek == false)
                throw new ArgumentException("Cannot read a PDF File from a non-seekable stream");

            seekableStream.Position = 0;
            Version vers = ReadHeaderVersion(seekableStream);

            seekableStream.Position = 0;
            PDFReader reader;
            if (vers < new Version(1, 5))
                reader = new PDFReader14(seekableStream, false, log);
            else if (vers < new Version(1, 8))
                reader = new PDFReader17(seekableStream, false, log);
            else
            {
                log.Add(Scryber.TraceLevel.Warning, "PDFReader", "Source PDF File version is greater than the supported version 1.7. Parsing of the file will continue, but may not succeed.");
                reader = new PDFReader17(seekableStream, false, log);
            }
            reader.InitData(log);

            return reader;
        }

        private static Version ReadHeaderVersion(Stream seekableStream)
        {
            try
            {
                byte[] header = new byte[HeaderByteLength];
                seekableStream.Read(header, 0, HeaderByteLength);
                string headerValue = System.Text.Encoding.ASCII.GetString(header);
                int major = int.Parse(headerValue[MajorVersionOffset].ToString());
                int minor = int.Parse(headerValue[MinorVersionOffset].ToString());
                return new Version(major, minor);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not reader the PDF file version header of the source document", ex);
            }
        }

        #endregion
    }



    /// <summary>
    /// Implements the PDFReader abstract class. Supports the reading of an existing pdf file.
    /// </summary>
    /// <remarks>Loads the XRefTable and initializes the</remarks>
    internal class PDFReader14 : PDFReader
    {
        //
        // properties
        //

        #region public override System.IO.Stream InnerStream

        private System.IO.Stream _innerStream;

        public override System.IO.Stream InnerStream
        {
            get { return _innerStream; }
        }

        #endregion

        #region internal PDFTextSearcher Searcher

        private PDFTextSearcher _searcher;

        internal PDFTextSearcher Searcher
        {
            get { return _searcher; }
        }

        #endregion

        #region public bool OwnsInnerStream

        private bool _ownsstream = false;

        public bool OwnsInnerStream
        {
            get { return _ownsstream; }
        }

        #endregion

        #region public override PDFXRefTable XRefTable

        private PDFXRefTable _xreftable;

        public override PDFXRefTable XRefTable
        {
            get { return _xreftable; }
            protected set { _xreftable = value; }
        }

        #endregion

        #region public override PDFFileIndirectObject DocumentCatalogRef

        private PDFFileIndirectObject _catalog;
        /// <summary>
        /// Gets the indirect object reference for the document catalog
        /// </summary>
        public override PDFFileIndirectObject DocumentCatalogRef
        {
            get
            {
                return _catalog;
            }
            protected set { _catalog = value; }
        }

        #endregion

        #region public override PDFDictionary DocumentTrailer {get;}

        private PDFDictionary _trailer;
        /// <summary>
        /// Gets the trailer for this document
        /// </summary>
        public override PDFDictionary DocumentTrailer
        {
            get { return _trailer; }
            protected set { _trailer = value; }
        }

        #endregion

        #region public override PDFIndirectObject DocumentInfoRef

        private PDFFileIndirectObject _info;

        /// <summary>
        /// Gets the indirect object reference for the document info
        /// </summary>
        public override PDFFileIndirectObject DocumentInfoRef
        {
            get
            {
                return _info;
            }
            protected set { _info = value; }
        }

        #endregion


        //
        // .ctor
        //

        #region private PDFReader14(System.IO.Stream stream, bool ownStream)

        /// <summary>
        /// Constructs an new PDFReader14
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="ownStream"></param>
        internal PDFReader14(System.IO.Stream stream, bool ownStream, TraceLog log)
        {
            this._innerStream = stream;
            this._ownsstream = ownStream;
            if (stream.CanSeek == false)
                throw new ArgumentException("Cannot read a PDF File from a non-seekable stream");

            this._searcher = new PDFTextSearcher(stream);
        }

        #endregion

        //
        // public methods
        //

        #region public IIndirectObject GetIndirectObject(PDFObjectRef oref)

        /// <summary>
        /// Reads and returns the object data returned based on the provided reference
        /// </summary>
        /// <param name="oref"></param>
        /// <returns></returns>
        public override IParsedIndirectObject GetObject(PDFObjectRef oref)
        {
            PDFXRefTableEntry entry = this.XRefTable[oref];
            if (null != entry)
            {
                if (entry.Free == false)
                {
                    if (null == entry.Reference)
                    {
                        this.Searcher.Position = entry.Offset;
                        entry.Reference = PDFFileIndirectObject.Parse(this.Searcher, oref.Number, oref.Generation);
                    }
                    return (IParsedIndirectObject)entry.Reference;
                }
            }
            return null;
        }

        #endregion


        //
        // initialization methods
        //

        /// <summary>
        /// The '%%EOF' end of file marker
        /// </summary>
        protected const string EndOfFileMarker = "%%EOF";

        /// <summary>
        /// The 'startxref' marker that comes before the long integer offset of the actual xref table.
        /// </summary>
        protected const string StartXRefMarker = "startxref";

        /// <summary>
        /// The 'xref' marker that comes at the start of the full xref table
        /// </summary>
        protected const string XRefMarker = "xref";

        /// <summary>
        /// The 'trailer' marker that indicates the start of the trailer dictionary 
        /// </summary>
        protected const string TrailerMarker = "trailer";

        private static readonly PDFName CatalogObjName = (PDFName)"Root";
        private static readonly PDFName InfoObjName = (PDFName)"Info";
        private static readonly PDFName PrevXRefName = (PDFName)"Prev";

        #region protected override void InitData(PDFTraceLog log)

        /// <summary>
        /// Initializes the known PDF file data such as trailers, xref tables and catalogs
        /// </summary>
        protected override void InitData(TraceLog log)
        {
            try
            {
                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "Finding end of file, startxref and trailer positions");

                this.Searcher.Position = this.Searcher.Length;
                PDFFileRange eofPos = AssertFoundRange(Searcher.MatchBackwardString(EndOfFileMarker), EndOfFileMarker);
                PDFFileRange startxrefPos = AssertFoundRange(Searcher.MatchBackwardString(StartXRefMarker), StartXRefMarker);

                PDFFileRange trailerPos = AssertFoundRange(Searcher.MatchBackwardString(TrailerMarker), TrailerMarker);

                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "Markers found, loading the trailer dictionary");

                PDFDictionary trailer = GetTrailerDictionary(trailerPos, startxrefPos);
                this._trailer = trailer;

                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "Markers found, loading the XRef table");

                PDFObjectRef catalogRef = AssertGetObjectRef(trailer, CatalogObjName, "The '" + CatalogObjName + "' entry couldnot be found in the documents trailer dictionary");
                PDFObjectRef infoRef = AssertGetObjectRef(trailer, InfoObjName, "The '" + InfoObjName + "' entry couldnot be found in the documents trailer dictionary");
                IFileObject prevXRefObj;
                trailer.TryGetValue(PrevXRefName, out prevXRefObj);
                long prevOffset = -1;
                if (prevXRefObj is PDFNumber)
                    prevOffset = ((PDFNumber)prevXRefObj).Value;
                else if (prevXRefObj is PDFReal)
                    prevOffset = (long)((PDFNumber)prevXRefObj).Value;

                PDFXRefTable xref = GetXRefTable(startxrefPos, eofPos, prevOffset);


                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "References for the catalog and document info found");

                this._xreftable = xref;
                this._info = (PDFFileIndirectObject)this.GetObject(infoRef);

                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "Loaded the document Info indirect object");

                this._catalog = (PDFFileIndirectObject)this.GetObject(catalogRef);

                if (log.ShouldLog(TraceLevel.Debug))
                    log.Add(TraceLevel.Debug, "PDFReader", "Loaded the document Catalog indirect object");

                //TODO: Look for more updates and read those in too
            }
            catch (Exception ex)
            {
                throw new PDFNativeParserException(CommonErrors.CouldNotInitializeThePDFReader, ex);
            }

        }

        #endregion

        #region protected PDFDictionary GetTrailerDictionary(PDFFileRange trailerPos, PDFFileRange startxrefPos)

        /// <summary>
        /// Matches and parses the trailer dictionary that must be present in a PDF document file that is between the trailer marker and the startxref marker
        /// </summary>
        /// <param name="trailerPos"></param>
        /// <param name="startxrefPos"></param>
        /// <returns></returns>
        protected PDFDictionary GetTrailerDictionary(PDFFileRange trailerPos, PDFFileRange startxrefPos)
        {
            string fulltrailer = Searcher.GetInnerText(trailerPos, startxrefPos);
            fulltrailer = fulltrailer.Trim();
            PDFDictionary trailer = PDFDictionary.Parse(fulltrailer);
            return trailer;
        }

        #endregion

        #region protected PDFXRefTable GetXRefTable(PDFFileRange startxrefPos, PDFFileRange eofPos, long trailerPrevOffset)

        /// <summary>
        /// Matches and parses the XRefTable in the this instances PDF data stream that is
        /// specified pased on the position value between the startXRefPos and the end of file marker
        /// </summary>
        /// <param name="startxrefPos"></param>
        /// <param name="eofPos"></param>
        /// <param name="trailerPrevOffset">The offset marked in the trailer dictionary of any previous XRefTable this file contains.</param>
        /// <returns></returns>
        protected PDFXRefTable GetXRefTable(PDFFileRange startxrefPos, PDFFileRange eofPos, long trailerPrevOffset)
        {
            long xrefPos = long.Parse(this.Searcher.GetInnerText(startxrefPos, eofPos).Trim());
            this.Searcher.Position = xrefPos;
            string fullXRef = this.Searcher.GetInnerText((int)(eofPos.StartOffset - xrefPos));
            int end;
            PDFXRefTable table = PDFXRefTable.Parse(fullXRef, 0, out end);
            table.Offset = xrefPos;

            if (trailerPrevOffset > 0) // we have a previous entry
            {
                this.Searcher.Position = trailerPrevOffset;

                if (startxrefPos.Found)
                {
                    eofPos = this.Searcher.MatchForwardString(EndOfFileMarker);
                    startxrefPos = AssertFoundRange(this.Searcher.MatchBackwardString(XRefMarker), XRefMarker);
                    PDFFileRange trailerPos = AssertFoundRange(this.Searcher.MatchBackwardString(TrailerMarker), TrailerMarker);

                    PDFDictionary trailer = GetTrailerDictionary(trailerPos, startxrefPos);
                    IFileObject prevEntry;

                    if (trailer.TryGetValue(PrevXRefName, out prevEntry))
                    {
                        if (prevEntry is PDFNumber)
                            trailerPrevOffset = ((PDFNumber)prevEntry).Value;
                        else if (prevEntry is PDFReal)
                            trailerPrevOffset = (long)((PDFReal)prevEntry).Value;
                        else
                            trailerPrevOffset = -1;
                    }
                    else
                        trailerPrevOffset = -1;

                    PDFXRefTable prevTable = GetXRefTable(startxrefPos, eofPos, trailerPrevOffset);
                    table.Previous = prevTable;
                }
            }

            return table;
        }

        #endregion

        #region protected static PDFObjectRef AssertGetObjectRef(PDFDictionary dict, PDFName entry, string errorMessage)

        protected static PDFObjectRef AssertGetObjectRef(PDFDictionary dict, PDFName entry, string errorMessage)
        {
            IFileObject found;
            if (!dict.TryGetValue(entry, out found))
                throw new PDFNativeParserException(errorMessage);

            if (found.Type != ObjectTypes.ObjectRef)
                throw new PDFNativeParserException(errorMessage);

            return (PDFObjectRef)found;
        }

        #endregion


        #region protected static PDFFileRange AssertFoundRange(PDFFileRange range, string marker)

        protected static PDFFileRange AssertFoundRange(PDFFileRange range, string marker)
        {
            if (range.Found == false)
                throw new PDFNativeParserException(String.Format(CommonErrors.MarkerNotFoundByReader, marker));
            return range;
        }

        #endregion



    }
}
