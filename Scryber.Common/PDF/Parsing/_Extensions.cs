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
using Scryber.PDF;
using Scryber.PDF.Native;

namespace Scryber.PDF.Parsing
{
    /// <summary>
    /// Adds extension methods to the PDFFile object to retrieve strongly typed instances
    /// </summary>
    public static class PDFFileExtensions
    {

        public static PDFParsedPage GetPage(this PDFFile file, PDFObjectRef oref)
        {
            PDFDictionary obj = (PDFDictionary)file.GetContent(oref);
            return new PDFParsedPage(oref, obj, file);
        }

        public static PDFParsedCatalog GetCatalog(this PDFFile file)
        {
            PDFDictionary catalog = file.DocumentCatalog;
            return new PDFParsedCatalog(file.DocumentCatalogRef.Reference, catalog, file);
        }


        public static PDFParsedTrailer GetTrailer(this PDFFile file)
        {
            PDFDictionary trailer = file.DocumentTrailer;
            return new PDFParsedTrailer(trailer, file);
        }
        
    }

    public static class PDFDictionaryExtensions
    {
        public static bool TryGet<T>(this PDFDictionary dict, string name, out T value)  where T : class, IPDFFileObject
        {
            value = null;
            IPDFFileObject val;
            if (dict.TryGetValue(name, out val))
                value = val as T;
            return null != value;
        }

        public static bool TryGet<T>(this PDFDictionary dict, PDFName name, out T value)  where T : class,  IPDFFileObject
        {
            value = null;
            IPDFFileObject val;
            if (dict.TryGetValue(name, out val))
                value = val as T;
            return null != value;
        }

    }


}
