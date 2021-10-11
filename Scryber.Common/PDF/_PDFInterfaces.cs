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
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.PDF
{
    

    #region public interface IPDFFileObject : ITypedObject

    /// <summary>
    /// Base abstract class of all native file objects (PDFBoolean, PDFNumber etc...)
    /// </summary>
    public interface IPDFFileObject : ITypedObject
    {
        /// <summary>
        /// Writes the underlying data of the file object to the passed text writer
        /// </summary>
        /// <param name="tw">The text writer object to write data to</param>
        void WriteData(PDFWriter writer);

    }

    #endregion

    #region public interface IPDFIndirectObject : IDisposable

    /// <summary>
    /// Defines the interface that all indirect objects must adhere to.
    /// </summary>
    public interface IPDFIndirectObject : IDisposable
    {
        /// <summary>
        /// Gets the object number of this indirect object
        /// </summary>
        int Number { get; set; }

        /// <summary>
        /// Gets the generation number of this indirect object
        /// </summary>
        int Generation { get; set; }

        /// <summary>
        /// Gets the byte offset of this indirect object in the base stream
        /// </summary>
        long Offset { get; set; }

        /// <summary>
        /// Gets the associated object data for this indirect object
        /// </summary>
        PDFStream ObjectData { get; }

        /// <summary>
        /// Returns true if this indirect object is deleted.
        /// </summary>
        bool Deleted { get; }

        /// <summary>
        /// Returns true if this indirect object has already been written to the base stream
        /// </summary>
        bool Written { get; set; }

        /// <summary>
        /// Returns true if this indirect object has an inner stream data
        /// </summary>
        bool HasStream { get; }

        /// <summary>
        /// Returns the inner stream data for this indirect object
        /// </summary>
        PDFStream Stream { get; }

        /// <summary>
        /// Gets the associated object data as a byte array
        /// </summary>
        /// <returns></returns>
        byte[] GetObjectData();

        /// <summary>
        /// Gets the associated stream data as a byte array
        /// </summary>
        /// <returns></returns>
        byte[] GetStreamData();
    }

    #endregion

    #region public interface IPDFParsedIndirectObject : IIndirectObject

    /// <summary>
    /// Interface for indirect objects that have been parsed from an existing file
    /// </summary>
    public interface IPDFParsedIndirectObject : IPDFIndirectObject
    {
        /// <summary>
        /// Returns the parsed object data 
        /// </summary>
        /// <returns></returns>
        IPDFFileObject GetContents();
    }

    #endregion

    #region public interface IPDFStreamFactory

    /// <summary>
    /// Interface for instance that creates PDFStreams indirect objects can use
    /// </summary>
    public interface IPDFStreamFactory
    {
        PDFStream CreateStream(IStreamFilter[] filters, IPDFIndirectObject forObject);
    }

    #endregion

    #region public interface IStreamFilter

    /// <summary>
    /// Defines the interface that all Stream Filters must adhere to 
    /// </summary>
    public interface IStreamFilter
    {
        /// <summary>
        /// Gets or Sets the name of the filter
        /// </summary>
        string FilterName
        {
            get;
            set;
        }

        /// <summary>
        /// Filters the stream reading from the TextReader, applying the filter and writing to the TextWriter
        /// </summary>
        /// <param name="read"></param>
        /// <param name="write"></param>
        void FilterStream(System.IO.Stream read, System.IO.Stream write);

        /// <summary>
        /// Performs a filter on the original data array, and returns the filtered data as a new byte[]
        /// </summary>
        /// <param name="orig"></param>
        /// <returns></returns>
        byte[] FilterStream(byte[] orig);
    }

    #endregion

    #region public interface IObjectContainer

    /// <summary>
    /// Interface that defines a container of IFileObjects
    /// </summary>
    public interface IPDFObjectContainer
    {
        void Add(IPDFFileObject obj);
    }

    #endregion

    #region public interface IPDFResource

    /// <summary>
    /// Defines a top level resource that is contained in the  IDocument, and used for rendering the pages - e.g. Font or Image
    /// </summary>
    public interface IPDFResource : ISharedResource
    {


        /// <summary>
        /// If this resource has not been previously rendered, then this resource will render its content within the document.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        PDFObjectRef EnsureRendered(ContextBase context, PDFWriter writer);
    }

    #endregion

}
