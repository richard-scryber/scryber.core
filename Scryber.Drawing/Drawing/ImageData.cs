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


        #region public int PixelWidth {get;set;}

        private int _w;
        /// <summary>
        /// Gets the total number of pixels in one row of the image
        /// </summary>
        public int PixelWidth
        {
            get { return _w; }
            protected set { _w = value; }
        }

        #endregion

        #region public int PixelHeight {get;set;}

        private int _h;

        public int PixelHeight
        {
            get { return _h; }
            protected set { _h = value; }
        }

        #endregion


        #region  public int BitsPerColor {get;set;}

        private int _bitdepth;

        /// <summary>
        /// Gets or set the number of bits per single color sample
        /// </summary>
        public int BitsPerColor
        {
            get { return _bitdepth; }
            internal protected set { _bitdepth = value; }
        }

        #endregion


        #region public ColorSpace ColorSpace {get;set;}

        private ColorSpace _cs = ColorSpace.RGB;

        public ColorSpace ColorSpace
        {
            get { return _cs; }
            internal protected set { _cs = value; }
        }

        #endregion

        #region public int ColorsPerSample {get; set;}

        private int _colspsample;

        /// <summary>
        /// Gets the numbers of colour values that make up an individual pixel.
        /// </summary>
        public int ColorsPerSample
        {
            get { return _colspsample; }
            protected internal set { _colspsample = value; }
        }

        #endregion

        #region public int HorizontalResolution {get;set;}

        private int _hres;
        /// <summary>
        /// Gets the horizontal resolution of the image (pixels per inch)
        /// </summary>
        public int HorizontalResolution
        {
            get { return _hres; }
            internal protected set { _hres = value; }
        }

        #endregion

        #region public int VerticalResolution {get;set;}

        private int _vres;
        /// <summary>
        /// Gets the vertical resolution of the image (pixels per inch)
        /// </summary>
        public int VerticalResolution
        {
            get { return _vres; }
            internal protected set { _vres = value; }
        }

        #endregion

        #region public PDFUnit DisplayHeight {get;}

        /// <summary>
        /// Gets the natural height of the image in inches based on the number of pixels high and the vertical Resolution
        /// </summary>
        public Unit DisplayHeight
        {
            get { return new Unit(((double)this.PixelHeight) / (double)this.VerticalResolution, PageUnits.Inches); }
        }

        #endregion

        #region public PDFUnit DisplayWidth {get;}

        /// <summary>
        /// Gets the natural width of the image in inches based on the number of pixels wide and the horizontal Resolution
        /// </summary>
        public Unit DisplayWidth
        {
            get { return new Unit(((double)this.PixelWidth) / (double)this.HorizontalResolution, PageUnits.Inches); }
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
        /// If true then this image has a standard color channel of image data, and an alpha mask channel.
        /// Inheritors can set the value.
        /// </summary>
        public bool HasAlpha
        {
            get;
            protected set;
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
        
        
        //
        // ctor(s)
        //

        #region protected PDFImageData(string source, int w, int h) + 1 overload

        /// <summary>
        /// Protected constructor - accepts the source path, width and height (in pixels)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        protected ImageData(string source, int w, int h)
            : this(ObjectTypes.ImageData, source, w, h)
        {
            this._path = source;
            this._w = w;
            this._h = h;
        }

        protected ImageData(ObjectType type, string source, int w, int h)
        {
            this._type = type;
            this._path = source;
            this._w = w;
            this._h = h;
        }

        #endregion

        //
        // rendering

        #region public PDFSize GetSize()

        private const double DefaultResolution = 72.0;

        /// <summary>
        /// Gets the natural size of the image in page units
        /// </summary>
        /// <returns></returns>
        public virtual Size GetSize()
        {
            double hres = (double)this.HorizontalResolution;
            if (hres < 1.0)
                hres = DefaultResolution;
            double vres = (double)this.VerticalResolution;
            if (vres <= 1.0)
                vres = DefaultResolution;

            double w = ((double)this.PixelWidth) / hres;
            double h = ((double)this.PixelHeight) / vres;

            return new Size(new Unit(w, PageUnits.Inches), new Unit(h, PageUnits.Inches));
        }

        #endregion
        
        
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


        public static ImageData Parse(string value)
        {
            throw new NotImplementedException("Not currently implemented, but may add support");
        }
    }
}
