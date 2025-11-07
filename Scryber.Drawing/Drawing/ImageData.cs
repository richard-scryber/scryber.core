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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Scryber.Caching;
using Scryber.Options;
using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber.Drawing
{
    [PDFParsableValue]
    public abstract class ImageData : ITypedObject
    {

        //
        // properties
        //

        #region public string SourcePath {get;set;}

        private string _path;
        /// <summary>
        /// Gets the source path this image data was loaded from
        /// </summary>
        public string SourcePath
        {
            get { return _path; }
            protected set { _path = value; }
        }

        #endregion


        #region public string Filter {get;set;}

        private IStreamFilter[] _filters;

        /// <summary>
        /// Gets the stream filter
        /// </summary>
        public IStreamFilter[] Filters
        {
            get { return _filters; }
            set
            {
                _filters = value;
                this.ResetFilterCache();
            }
        }

        #endregion

        #region  public bool HasFilter {get;}

        public bool HasFilter
        {
            get { return null != _filters && _filters.Length > 0; }
        }

        #endregion

        
        #region public ObjectType Type { get;}
        
        private ObjectType _type;

        /// <summary>
        /// Gets the type of object this is
        /// </summary>
        public ObjectType Type
        {
            get { return _type; }
        }
        
        #endregion
        
        
        public virtual ImageType ImageType { get; protected set; }
        
        
        #region public virtual bool IsPrecompressedData {get;}

        /// <summary>
        /// If true then the raw image data is precompressed otherwise it is raw binary pixel data
        /// </summary>
        public virtual bool IsPrecompressedData
        {
            get { return false; }
        }

        #endregion
        
        #region public bool HasAlpha {get; protected set;}
        
        /// <summary>
        /// If true then this image has standard image data, and an alpha mask channel.
        /// Inheritors can set the value.
        /// </summary>
        public bool HasAlpha
        {
            get;
            protected set;
        }
        
        #endregion
        
        //
        // ctor(s)
        //

        #region protected PDFImageData(string source, int w, int h) + 1 overload

        /// <summary>
        /// Protected constructor - accepts the source path, width and height (in pixels)
        /// </summary>
        /// <param name="source">The path or other identifier for the image</param>
        /// <param name="imgType">The type of image - raster, vectore</param>
        /// <param name="h"></param>
        protected ImageData(string source, ImageType imgType)
            : this(ObjectTypes.ImageData, source, imgType)
        {
        }

        protected ImageData(ObjectType type, string source, ImageType imgType)
        {
            this._type = type;
            this._path = source;
            this.ImageType = imgType;
        }

        #endregion

        //
        // rendering

        public abstract Size GetSize();

        public virtual Rect? GetClippingRect(Point offset, Size available, ContextBase context)
        {
            return null;
        }
        
        public virtual Size GetRequiredSizeForRender(Point offset, Size available, ContextBase context)
        {
            return available;
        }
        
        public virtual Point GetRequiredOffsetForRender(Point offset, Size available, ContextBase context)
        {
            return offset;
        }

        public abstract PDFObjectRef Render(PDFName name, IStreamFilter[] filters,  ContextBase context, PDFWriter writer);

        
        public abstract void ResetFilterCache();


        public static ImageData Parse(string source)
        {
            throw new NotImplementedException("The image data is not currently parsable");
        }
    }
}
