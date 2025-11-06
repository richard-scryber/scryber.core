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

    public class PDFParsedPage : PDFParsedObject
    {
        #region public PDFParsedObjectEntryCollection Entries

        private PDFParsedObjectEntryCollection _existingEntries;

        /// <summary>
        /// Gets a list of all the existing catalog entries in the original document
        /// </summary>
        public PDFParsedObjectEntryCollection Entries
        {
            get { return _existingEntries; }
        }

        #endregion


        #region public PDFObjectRef[] ContentStreams {get;}

        private PDFObjectRef[] _content = null;

        /// <summary>
        /// Gets an array of bject references to the content streams
        /// </summary>
        public PDFObjectRef[] ContentStreams
        {
            get
            {
                if (null == _content)
                {
                    PDFParsedObjectEntry entry = this.Entries["Contents"];
                    IPDFFileObject obj = (null == entry) ? null : entry.OriginalData;
                    if (obj is PDFObjectRef)
                        _content = new PDFObjectRef[] { (PDFObjectRef)obj };
                    else if (obj is PDFArray)
                    {
                        _content = ((PDFArray)obj).ContentAs<PDFObjectRef>();
                    }
                    else
                        throw new InvalidCastException("The page contend stream can only be an object reference or an array of object references.");
                }
                return _content;
            }

        }

        #endregion

        #region public PDFArray MediaBox {get;}

        private PDFArray _mediabox;

        /// <summary>
        /// Gets the media box associated with this page (the size of the page).
        /// </summary>
        public PDFArray MediaBox
        {
            get
            {
                if (null == _mediabox)
                {
                    PDFParsedObjectEntry entry = this.Entries["MediaBox"];
                    IPDFFileObject obj = (null == entry) ? null : entry.OriginalData;

                    //Follow the indirect reference if needed.
                    if (obj is PDFObjectRef)
                        obj = this.Owner.GetContent((PDFObjectRef)obj);

                    if (obj is PDFArray)
                        _mediabox = (PDFArray)obj;
                    else
                        throw new InvalidCastException("The page bounds MediaBox must be an array of 4 numbers");
                }
                return _mediabox;
            }
        }

        #endregion

        #region public PDFParsedResources Resources {get;}

        private PDFParsedResources _resources;

        /// <summary>
        /// Gets the original list of resources in the underlying page
        /// </summary>
        public PDFParsedResources Resources
        {
            get
            {
                if(null == _resources)
                {
                    PDFParsedObjectEntry entry = this.Entries["Resources"];
                    IPDFFileObject obj = (null == entry) ? null : entry.OriginalData;

                    //Follow the indirect reference if needed.
                    if (obj is PDFObjectRef)
                        obj = this.Owner.GetContent((PDFObjectRef)obj);

                    if (obj is PDFDictionary)
                        _resources = new PDFParsedResources((PDFDictionary)obj);
                    else
                        throw new InvalidCastException("The page resource set must be a dictionary of types with object references or value types");
                }
                return _resources;
            }
        }

        #endregion

        //
        // ctors
        //

        internal PDFParsedPage(PDFObjectRef oref, PDFDictionary pgDict, PDFFile owner)
            : base(oref, pgDict, owner)
        {
            bool ignoreType = true;
            this._existingEntries = new PDFParsedObjectEntryCollection(pgDict, ignoreType);
        }


        //
        // methods
        //

        #region public PDFRect GetPageBounds()

        /// <summary>
        /// Returns this pages bounds as an array of 4 double values for x, y, width and height in PDF Units
        /// </summary>
        /// <returns></returns>
        public double[] GetPageBounds()
        {
            //Media box is specified in values for the lower left in points
            PDFReal llx = ConvertToNumber(this.MediaBox[0]);
            PDFReal lly = ConvertToNumber(this.MediaBox[1]);
            PDFReal urx = ConvertToNumber(this.MediaBox[2]);
            PDFReal ury = ConvertToNumber(this.MediaBox[3]);
            double x = llx.Value;
            double y = lly.Value;
            double w = urx.Value - llx.Value;
            double h = ury.Value - lly.Value;
            double[] full = { x, y, w, h };
            
            return full;
        }

        private static PDFReal ConvertToNumber(IPDFFileObject obj)
        {
            if (obj.Type == ObjectTypes.Real)
                return (PDFReal)obj;
            else if (obj.Type == ObjectTypes.Number)
                return new PDFReal(((PDFNumber)obj).Value);
            else
                throw new InvalidCastException("Cannot conver the object of type '" + obj.Type + "' to a valid PDFNumber");
        }

        #endregion

    }
}
