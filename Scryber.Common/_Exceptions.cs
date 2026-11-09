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

    /// <summary>
    /// Generic exception for the PDF framework
    /// </summary>
    [global::System.Serializable]
    public class PDFException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PDFException() { }
        public PDFException(string message) : base(message) { }
        public PDFException(string message, Exception inner) : base(message, inner) { }
        
    }

    [Serializable]
    public class PDFStreamException : PDFException
    {
        public PDFStreamException() { }
        public PDFStreamException(string message) : base(message) { }
        public PDFStreamException(string message, Exception inner) : base(message, inner) { }
        
    }

    [Serializable]
    public class PDFNativeParserException : PDFException
    {
        public PDFNativeParserException() { }
        public PDFNativeParserException(string message) : base(message) { }
        public PDFNativeParserException(string message, Exception inner) : base(message, inner) { }
        
    }


    /// <summary>
    /// Exceptions that are thrown when there is a general exception during Component layout
    /// </summary>
    [global::System.Serializable]
    public class PDFLayoutException : PDFException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PDFLayoutException() { }
        public PDFLayoutException(string message) : base(message) { }
        public PDFLayoutException(string message, Exception inner) : base(message, inner) { }
        
    }

    /// <summary>
    /// Exceptions that are thrown when there is a general error on databinding
    /// </summary>
    [global::System.Serializable]
    public class PDFBindException : PDFException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PDFBindException() { }
        public PDFBindException(string message) : base(message) { }
        public PDFBindException(string message, Exception inner) : base(message, inner) { }
        
    }

    /// <summary>
    /// Exceptions that are thrown when there is a general error during rendering
    /// </summary>
    [global::System.Serializable]
    public class PDFRenderException : PDFException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public PDFRenderException() { }
        public PDFRenderException(string message) : base(message) { }
        public PDFRenderException(string message, Exception inner) : base(message, inner) { }
        
    }


    //[global::System.Serializable]
    //public class PDFLicenceException : PDFException
    //{
    //    //
    //    // For guidelines regarding the creation of new exception types, see
    //    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    //    // and
    //    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //    //

    //    public PDFLicenceException() { }
    //    public PDFLicenceException(string message) : base(message) { }
    //    public PDFLicenceException(string message, Exception inner) : base(message, inner) { }
    //    protected PDFLicenceException(
    //      System.Runtime.Serialization.SerializationInfo info,
    //      System.Runtime.Serialization.StreamingContext context)
    //        : base(info, context) { }
    //}


    [Serializable]
    public class PDFDataException : PDFException
    {
        public PDFDataException() { }
        public PDFDataException(string message) : base(message) { }
        public PDFDataException(string message, Exception inner) : base(message, inner) { }
        
    }


    [Serializable]
    public class PDFLinkException : PDFException
    {
        public PDFLinkException() { }
        public PDFLinkException(string message) : base(message) { }
        public PDFLinkException(string message, Exception inner) : base(message, inner) { }
        
    }

    [Serializable]
    public class PDFMissingImageException : PDFException
    {
        public PDFMissingImageException() { }
        public PDFMissingImageException(string message) : base(message) { }
        public PDFMissingImageException(string message, Exception inner) : base(message, inner) { }
        
    }

    [Serializable]
    public class PDFMissingAttachmentException : PDFException
    {
        public PDFMissingAttachmentException() { }
        public PDFMissingAttachmentException(string message) : base(message) { }
        public PDFMissingAttachmentException(string message, Exception inner) : base(message, inner) { }
        
    }
}
