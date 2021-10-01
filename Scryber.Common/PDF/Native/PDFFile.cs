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
    /// Represents a complete PDFDocument that already exists as a file (or binary data).
    /// </summary>
    public class PDFFile : PDFObject, IDisposable
    {

        public const string CatalogName = "Catalog";
        public const string PagesName = "Pages";
        public const string TypeName = "Type";
        private const string PDFFileLogCategory = "PDF Source File";

        #region ivars

        private PDFReader _reader;
        private PDFTraceLog _log;
        private System.IO.Stream _innerStream;
        private PDFObjectRef _pageTree;
        private bool _canAppend;
        private string _origpath;

        #endregion

        #region public PDFObjectRef PageTree{get;}

        /// <summary>
        /// Gets the object reference to the tree of all the pages in file
        /// </summary>
        public PDFObjectRef PageTree
        {
            get { return _pageTree; }
        }

        #endregion

        #region public PDFXRefTable DocumentXRefs {get;}

        /// <summary>
        /// Gets the complete XRefTable for this document
        /// </summary>
        public PDFXRefTable DocumentXRefs
        {
            get { return this._reader.XRefTable; }
        }

        #endregion

        #region public PDFFileIndirectObject DocumentCatalogRef {get;}

        /// <summary>
        /// Gets the Catalog dictionary entry in this PDF File
        /// </summary>
        public PDFFileIndirectObject DocumentCatalogRef
        {
            get { return this._reader.DocumentCatalogRef; }
        }

        #endregion

        #region public PDFFileIndirectObject DocumentInfoRef {get;}

        /// <summary>
        /// Gets the PDFDictionary that is the original DocumentInfo object
        /// </summary>
        public PDFFileIndirectObject DocumentInfoRef
        {
            get { return this._reader.DocumentInfoRef; }
        }

        #endregion

        #region public PDFDictionary DocumentCatalog {get;}

        /// <summary>
        /// Gets the Catalog dictionary entry in this PDF File
        /// </summary>
        public PDFDictionary DocumentCatalog
        {
            get { return this._reader.DocumentCatalogRef.GetContents() as PDFDictionary; }
        }

        #endregion

        #region public PDFDictionary DocumentInfo {get;}

        /// <summary>
        /// Gets the PDFDictionary that is the original DocumentInfo object
        /// </summary>
        public PDFDictionary DocumentInfo
        {
            get { return _reader.DocumentInfoRef.GetContents() as PDFDictionary; }
        }

        #endregion

        #region public PDFDictionary DocumentTrailer {get;}

        /// <summary>
        /// Gets the trailer dictionary in the PDF File
        /// </summary>
        public PDFDictionary DocumentTrailer
        {
            get { return this._reader.DocumentTrailer; }
        }

        #endregion

        #region public bool CanAppend {get;}

        /// <summary>
        /// Returns true if the PDFFile this instance refers to can be appended to (as a pdf update).
        /// </summary>
        public bool CanAppend
        {
            get { return _canAppend; }
        }

        #endregion

        #region public string OriginalPath {get;}

        /// <summary>
        /// Gets the original source path of this file
        /// </summary>
        public string OriginalPath
        {
            get { return _origpath; }
        }

        #endregion

        //
        // .ctor(s)
        //

        protected PDFFile()
            : this(ObjectTypes.File)
        {
        }

        protected PDFFile(ObjectType type)
            : base(type)
        {
        }


        //
        // initialization
        //

        #region protected virtual void Init()

        /// <summary>
        /// Called from the factory methods once instantiated to initialize the data.
        /// </summary>
        protected virtual void Init()
        {
            this._pageTree = this.DocumentCatalog[PagesName] as PDFObjectRef;
        }

        #endregion

        // not used

        /*
        #region private PDFObjectRef[] InitPageRefs()

        private PDFObjectRef[] InitPageRefs()
        {
            List<PDFObjectRef> all = new List<PDFObjectRef>();
            PDFObjectRef pagetree = this.DocumentCatalog[PagesName] as PDFObjectRef;
            if (null != pagetree)
                this.InitAPageOrTree(pagetree, all, null);

            return all.ToArray();
        }

        #endregion

        #region private void InitAPageOrTree(PDFObjectRef pagetree, List<PDFObjectRef> all)

        private void InitAPageOrTree(PDFObjectRef pageref, List<PDFObjectRef> all, PDFDictionary inherited)
        {
            IParsedIndirectObject obj = this._reader.GetObject(pageref);
            if (null == obj)
                throw new PDFNativeParserException("Cannot find object with reference '" + pageref.ToString());

            PDFDictionary dict = obj.GetContents() as PDFDictionary;
            if (null == dict)
                return;

            PDFName type = dict[TypeName] as PDFName;
            if (null == type)
            {
                return;
            }
            else if (type.Value == "Pages")
            {
                PDFDictionary newinherited = null;
                PDFArray kids = null;
                //Type, Parent, Kids, Count are standard
                //Others are then inherited in the page.
                foreach (KeyValuePair<PDFName, IFileObject> item in dict)
                {
                    switch (item.Key.Value)
                    {
                        case ("Type"):
                        case ("Parent"):
                        case ("Count"):
                            //Do nothing
                            break;
                        case ("Kids"):
                            kids = item.Value as PDFArray;
                            break;
                        default:
                            if (null == newinherited)
                                newinherited = new PDFDictionary();

                            newinherited.Add(item.Key, item.Value);
                            break;
                    }

                }
                if (null != newinherited && newinherited.Count > 0)
                {
                    if (null != inherited && inherited.Count > 0)
                    {
                        foreach (KeyValuePair<PDFName, IFileObject> item in inherited)
                        {
                            newinherited.Add(item.Key, item.Value);
                        }
                    }
                }
                else
                    newinherited = inherited;

                if (null != kids)
                {
                    foreach (PDFObjectRef oref in kids)
                    {
                        InitAPageOrTree(oref, all, newinherited);
                    }
                }
            }
            else if (type.Value == "Page")
            {
                all.Add(pageref);
                
            }

        }

        #endregion
        */

        //
        // public interface
        //

        #region public IFileObject GetContent(PDFObjectRef oref)

        /// <summary>
        /// Gets the main content of object based on the reference
        /// </summary>
        /// <param name="oref"></param>
        /// <returns></returns>
        public IFileObject GetContent(PDFObjectRef oref)
        {
            IParsedIndirectObject obj = this._reader.GetObject(oref);
            if (null == obj)
                return null;
            else
                return obj.GetContents();
        }

        #endregion

        #region public IFileObject AssertGetContent(PDFObjectRef oref)

        /// <summary>
        /// Gets the content of the indirect object specifed by the object reference. 
        /// If the result is not found or does not exist, then an exception is thrown.
        /// </summary>
        /// <param name="oref"></param>
        /// <returns></returns>
        public IFileObject AssertGetContent(PDFObjectRef oref)
        {
            IFileObject result = this.GetContent(oref);
            if (null == result)
                throw new NullReferenceException(String.Format(CommonErrors.AnIndirectObjectWithReferenceCouldNotBeFound, oref));

            return result;
        }

        #endregion

        #region public bool TryGetStreamData(PDFObjectRef oref, out byte[] data)

        /// <summary>
        /// Returns true if the object in the file (with reference oref) has an associated stream, 
        /// and if so sets the data value to that stream data
        /// </summary>
        /// <param name="oref"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryGetStreamData(PDFObjectRef oref, out byte[] data)
        {
            data = null;
            IParsedIndirectObject obj = this._reader.GetObject(oref);

            if (null == obj)
                return false;
            else
            {
                data = obj.GetStreamData();
                return null != data;
            }
        }

        #endregion

        #region public void WriteTo(PDFStream stream)

        /// <summary>
        /// Writes all the file data from the current stream to the provided stream
        /// </summary>
        /// <param name="stream"></param>
        public void WriteTo(PDFStream stream)
        {
            long origpos = this._innerStream.Position;

            this._innerStream.Position = 0;
            long srclen = this._innerStream.Length;
            long destlen = stream.Length;

            byte[] buffer = new byte[4096];
            int count = buffer.Length;
            while ((count = this._innerStream.Read(buffer, 0, count)) > 0)
            {
                stream.Write(buffer, 0, count);
            }

            stream.Flush();
            System.Diagnostics.Debug.Assert(srclen + destlen == stream.Length);

            this._innerStream.Position = origpos;
        }

        #endregion

        //
        // disposal
        //

        #region public void Dispose() + 1 overload

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this._innerStream)
                    this._innerStream.Dispose();

                this._innerStream = null;
                this._reader = null;
            }
        }

        #endregion

        #region ~PDFFile()

        ~PDFFile()
        {
            this.Dispose(false);
        }

        #endregion

        //
        // factory methods
        //

        #region public static PDFFile Load(string path, PDFTraceLog log)

        /// <summary>
        /// Loads a new PDFFile with the data from the specified path - Must be disposed after use.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static PDFFile Load(string path, PDFTraceLog log)
        {
            if (log.ShouldLog(TraceLevel.Message))
                log.Begin(TraceLevel.Message, PDFFileLogCategory, "Creating a new PDFFile to read the existing data from a file at path '" + path + "'");

            PDFFile file = new PDFFile();
            file._log = log;
            file._innerStream = GetFileStreamForPath(path);
            file._reader = PDFReader.Create(file._innerStream, log);
            file._canAppend = true;
            file._origpath = path;
            file.Init();

            if (log.ShouldLog(TraceLevel.Message))
                log.End(TraceLevel.Message, PDFFileLogCategory, "A new PDFFile was read from the existing data from the file");

            return file;
        }

        #endregion

        #region public static PDFFile Load(System.IO.Stream stream, PDFTraceLog log)

        /// <summary>
        /// Loads a PDFFile from the specified stream recoding process in the log. The stream position
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public static PDFFile Load(System.IO.Stream stream, PDFTraceLog log)
        {
            if (log.ShouldLog(TraceLevel.Message))
                log.Begin(TraceLevel.Message, PDFFileLogCategory, "Creating a new PDFFile to read the existing data from a stream");

            PDFFile file = new PDFFile();
            file._log = log;
            file._innerStream = stream;
            file._reader = PDFReader.Create(stream, log);
            file._canAppend = stream.CanWrite;
            file._origpath = string.Empty;

            file.Init();

            if (log.ShouldLog(TraceLevel.Message))
                log.End(TraceLevel.Message, PDFFileLogCategory, "A new PDFFile was read from the existing data from a stream");

            return file;

        }

        #endregion

        #region private static System.IO.Stream GetFileStreamForPath(string fullpath)

        /// <summary>
        /// Wraps the filestream opening for security.
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        private static System.IO.Stream GetFileStreamForPath(string fullpath)
        {
            System.IO.Stream stream;
            try
            {
                stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            }
            catch (Exception ex)
            {
                throw new PDFNativeParserException(CommonErrors.CannotOpenTheFileAtThePath, ex);
            }
            return stream;
        }

        #endregion
    }
}
