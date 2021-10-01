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
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public abstract class PDFImageData : PDFObject
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


        #region public byte[] Data {get;set;}

        private byte[] _data;

        /// <summary>
        /// Gets the byte array of image data that represent the image
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            internal protected set
            {
                _data = value;
                this.ResetFilterCache();
            }
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

        #region public int BytesPerLine {get; protected internal set;}

        private int _bperline;

        /// <summary>
        /// Gets the total number of bytes in one line
        /// </summary>
        public int BytesPerLine
        {
            get { return _bperline; }
            protected internal set { _bperline = value; }
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

        public PDFUnit DisplayHeight
        {
            get { return new PDFUnit(((double)this.PixelHeight) / (double)this.VerticalResolution, PageUnits.Inches); }
        }

        #endregion

        #region public PDFUnit DisplayWidth {get;}

        public PDFUnit DisplayWidth
        {
            get { return new PDFUnit(((double)this.PixelWidth) / (double)this.HorizontalResolution, PageUnits.Inches); }
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
            internal protected set
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

        //
        // ctor(s)
        //

        #region protected PDFImageData(string source, int w, int h)

        /// <summary>
        /// Protected constructor - accepts the source path, width and height (in pixels)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        protected PDFImageData(string source, int w, int h)
            : base(ObjectTypes.ImageData)
        {
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
        public PDFSize GetSize()
        {
            double hres = (double)this.HorizontalResolution;
            if (hres < 1.0)
                hres = DefaultResolution;
            double vres = (double)this.VerticalResolution;
            if (vres <= 1)
                vres = DefaultResolution;

            double w = ((double)this.PixelWidth) / hres;
            double h = ((double)this.PixelHeight) / vres;

            return new PDFSize(new PDFUnit(w, PageUnits.Inches), new PDFUnit(h, PageUnits.Inches));
        }

        #endregion

        internal PDFObjectRef Render(PDFName name, PDFContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Message, "Image Data", "Rendering image data for '" + name.ToString() + "'");

            PDFObjectRef renderref = writer.BeginObject(name.Value);

            writer.BeginDictionaryS();
            writer.WriteDictionaryNameEntry("Name", name.Value);
            writer.WriteDictionaryNameEntry("Type", "XObject");
            writer.WriteDictionaryNameEntry("Subtype", "Image");
            
            RenderImageInformation(context, writer);
           
            //writer.EndDictionary(); //- commented for data length fix

            writer.BeginStream(renderref);

            int length = this.RenderImageStreamData(context, writer);
           
            writer.EndStream();

            //Added for Data Length fix HRB 15/01/2015
            writer.WriteDictionaryNumberEntry("Length", length);
            writer.EndDictionary();
            //End of add

            writer.EndObject();

            if (context.ShouldLogDebug)
                context.TraceLog.End(TraceLevel.Message, "Image Data", "Completed render of the image data for '" + name + "' with source " + this.SourcePath);
            else
                context.TraceLog.Add(TraceLevel.Message, "Image Data", "Rendered the image data for '" + name.ToString() + "' with source " + this.SourcePath);


            return renderref;
        }

        protected virtual void RenderImageInformation(PDFContextBase context,  PDFWriter writer)
        {
            writer.WriteDictionaryNumberEntry("Width", this.PixelWidth);
            writer.WriteDictionaryNumberEntry("Height", this.PixelHeight);
            //writer.WriteDictionaryNumberEntry("Length", this.Data.LongLength); //- commented for data length fix

            if (this.ColorSpace == ColorSpace.G)
                writer.WriteDictionaryNameEntry("ColorSpace", "DeviceGray");
            else if (this.ColorSpace == ColorSpace.Custom)
                this.RenderCustomColorSpace(writer);
            else if (this.ColorSpace == Drawing.ColorSpace.CMYK)
                writer.WriteDictionaryNameEntry("ColorSpace", "DeviceCMYK");
            else
                writer.WriteDictionaryNameEntry("ColorSpace", "DeviceRGB");

            WriteFilterNames(context, writer);

            writer.WriteDictionaryNumberEntry("BitsPerComponent", this.BitsPerColor);

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Image Data", "Rendered image information dictionary : cs=" + this.ColorSpace + ", w=" + this.PixelWidth + ", h=" + this.PixelHeight + ", bpc=" + BitsPerColor);

            
        }

        protected virtual bool ShouldApplyFilters(PDFContextBase context)
        {
            if (this.HasFilter)
            {
                if (context.Compression == OutputCompressionType.FlateDecode || this.IsPrecompressedData)
                    return true;
            }
            return false;
        }

        protected void WriteFilterNames(PDFContextBase context, PDFWriter writer)
        {
            if (this.ShouldApplyFilters(context))
            {
                if (this.Filters.Length == 1)
                {
                    writer.WriteDictionaryNameEntry("Filter", this.Filters[0].FilterName);

                    if (context.ShouldLogDebug)
                        context.TraceLog.Add(TraceLevel.Debug, "Image Data", "Output Image Filter with name " + this.Filters[0].FilterName);
                }
                else
                {
                    writer.BeginDictionaryEntry("Filter");
                    writer.BeginArray();
                    for (int i = 0; i < this.Filters.Length; i++)
                    {
                        writer.BeginArrayEntry();
                        writer.WriteName(this.Filters[i].FilterName);
                        writer.EndArrayEntry();

                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "Image Data", "Output Image Filter with name " + this.Filters[i].FilterName);
                    }
                    writer.EndArray();
                    writer.EndDictionaryEntry();
                }

            }
            else
            {

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "Image Data", "No image filters to apply or not appropriate");
            }
        }

        protected virtual void RenderCustomColorSpace(PDFWriter writer)
        {
            throw new NotSupportedException("Implementors must use their own custom color space rendering");
        }

        protected virtual int RenderImageStreamData(PDFContextBase context, PDFWriter writer)
        {
            if (this.Data.LongLength > (long)int.MaxValue)
                throw new ArgumentOutOfRangeException("This image is too large to be included in a PDF file");

            byte[] data = this.Data;
            

            if (this.ShouldApplyFilters(context))
            {
                byte[] filtered;
                filtered = this.GetCachedFilteredData(this.Filters, context);
                if (null == filtered)
                {
                    filtered = ApplyFiltersToData(data, context);
                    this.SetCachedFilteredData(this.Filters, filtered, context);
                }
                data = filtered;
            }

            writer.WriteRaw(data, 0, data.Length);

            if(context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Image Data", "Written raw image data to output with length " + data.Length);

            //Added for data length fix - HRB 15/01/2015
            return data.Length;
            //End of add
        }

        protected byte[] ApplyFiltersToData(byte[] data, PDFContextBase context)
        {
            if (this.ShouldApplyFilters(context))
            {
                int origlength = data.Length;
                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug,"ImageData", "Applying filters to image data for '" + this.SourcePath + "'. Original length = " + origlength);

                if (this.Filters.Length == 1)
                    data = this.Filters[0].FilterStream(data);
                else
                {
                    for (int i = 0; i < this.Filters.Length; i++)
                    {
                        if (context.ShouldLogDebug)
                            context.TraceLog.Add(TraceLevel.Debug, "ImageData", "Applying filter '" + this.Filters[i].FilterName + "' to image data.");

                        data = this.Filters[i].FilterStream(data);
                    }
                }

                int finallength = data.Length;

                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "ImageData", "Applied all filters to image data for '" + this.SourcePath + "'. Final length = " + finallength);
                else if (context.ShouldLogMessage)
                    context.TraceLog.Add(TraceLevel.Message, "ImageData", "Filters applied to image data for '" + this.SourcePath + "', and size has gone from " + origlength + " to " + finallength);
            }
            else if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "ImageData", "Should not apply filters to image data for '" + this.SourcePath + "', returning original data");

            return data;
        }

        //
        // filter caching
        // 

        private string _filtercachekey = null;
        private byte[] _filterdata = null;

        public virtual void ResetFilterCache()
        {
            this._filtercachekey = null;
            this._filterdata = null;
        }

        public byte[] GetCachedFilteredData(IStreamFilter[] filters, PDFContextBase context)
        {
            if (null == filters || filters.Length == 0)
                return null;
            else if (null == _filterdata || null == _filtercachekey)
            {
                return null;
            }
            else
            {
                //We have a filter cacke key and some data, so check that they are the same - and if so return the data.

                string full = GetFullFilterCacheKey(filters);
                if (string.Equals(full, _filtercachekey))
                    return _filterdata;
                else
                    return null;
            }
        }

        protected virtual string GetFullFilterCacheKey(IStreamFilter[] filters)
        {
            if (null == filters || filters.Length == 0)
                throw new ArgumentNullException("filters");
            
            string full;
            if (filters.Length == 1)
                full = filters[0].FilterName;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (IStreamFilter filter in filters)
                {
                    if (sb.Length > 0)
                        sb.Append(" ");
                    sb.Append(filter.FilterName);
                }
                full = sb.ToString();
            }
            return full;
        }

        protected void SetCachedFilteredData(IStreamFilter[] filters, byte[] data, PDFContextBase context)
        {
            if (null == filters)
                throw new ArgumentNullException("filters");
            else if (filters.Length == 0)
                throw new ArgumentOutOfRangeException("filters");

            if(null == data)
                throw new ArgumentNullException("data");

            string key = GetFullFilterCacheKey(filters);

            this._filtercachekey = key;
            this._filterdata = data;

        }


        //
        // static methods
        //

        public static PDFImageData LoadImageFromURI(string uri, IComponent owner = null)
        {
            //throw new NotSupportedException("Don't use the loading from a remote uri. Use the Document.RegisterRemoteFileRequest");

            using (System.Net.Http.HttpClient wc = new System.Net.Http.HttpClient())
            {
                //wc. = System.Net.CredentialCache.DefaultNetworkCredentials;

                bool compress = false;

                if (owner is IOptimizeComponent)
                    compress = ((IOptimizeComponent)owner).Compress;

                PDFImageData img;
                byte[] data = wc.GetByteArrayAsync(uri).Result;
                img = InitImageData(uri, data, compress);
                return img;

            }
        }

        public static PDFImageData LoadImageFromStream(string sourceKey, System.IO.Stream stream, IComponent owner = null)
        {
            using (System.Drawing.Image bmp = System.Drawing.Image.FromStream(stream))
            {
                bool compress = false;

                if (null != owner && owner is IOptimizeComponent)
                    compress = ((IOptimizeComponent)owner).Compress;

                PDFImageData img;
                img = InitImageData(sourceKey, bmp, compress);
                return img;
            }
        }

        public static PDFImageData LoadImageFromLocalFile(string path, IComponent owner = null)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (fi.Exists == false)
                throw new ArgumentNullException("path", "The file at the path '" + path + "' does not exist.");

            bool compress = false;

            if (null != owner && owner is IOptimizeComponent)
                compress = ((IOptimizeComponent)owner).Compress;

            using (System.Drawing.Image bmp = System.Drawing.Image.FromFile(path))
            {
                PDFImageData img;
                img = InitImageData(path, bmp, compress);
                return img;
            }
        }

        public static PDFImageData LoadImageFromUriData(string src, IDocument document, IComponent owner)
        {
            if (null == document) throw new ArgumentNullException("document");
            if (null == owner) throw new ArgumentNullException("owner");

            var dataUri = ParseDataURI(src);

            if (dataUri.encoding != "base64") throw new ArgumentException("src", $"unsupported encoding {dataUri.encoding}; expected base64");
            if (dataUri.mediaType != "image/png") throw new ArgumentException("src", $"unsupported encoding {dataUri.mediaType}; expected image/png");

            var binary = Convert.FromBase64String(dataUri.data);

            using (var ms = new System.IO.MemoryStream(binary))
            {
                return PDFImageData.LoadImageFromStream(document.GetIncrementID(owner.Type) + "data_png", ms, owner);
            }
        }

        /// <summary>
        /// Creates a new PDFImageData instance from the specified bitmap.
        /// The sourceKey is a unique reference to this image data in the document
        /// </summary>
        /// <param name="sourcekey"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static PDFImageData LoadImageFromBitmap(string sourcekey, System.Drawing.Bitmap bitmap, bool compress = false)
        {
            if (null == bitmap)
                throw new ArgumentNullException("bitmap");
            PDFImageData data = InitImageData(sourcekey, bitmap, compress);
            
            return data;
        }


        public static PDFImageData InitImageData(string uri, byte[] data, bool compress)
        {
            PDFImageData img;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data))
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms, false);
                img = InitImageData(uri, bmp, compress);
                bmp.Dispose();
            }
            return img;
        }

        private static PDFImageData InitImageData(string uri, System.Drawing.Image bmp, bool compress)
        {
            PDFImageData imgdata;
            bool dispose = false;

            //if this image data is not a bitmap but a metafile (drawing instructions)
            //we need to convert it to a bitmap image.
            if (bmp is System.Drawing.Imaging.Metafile)
            {
                bmp = ConvertMetafileToBitmap(bmp as System.Drawing.Imaging.Metafile);
                dispose = true;
            }
            if(compress)
            {
                bmp = ConvertToJpeg(bmp);
            }
            try
            {
                Imaging.ImageParser parser = Imaging.ImageFormatParser.GetParser(bmp);
                imgdata = parser(uri, bmp);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(String.Format(Errors.CouldNotParseTheImageAtPath, uri, ex.Message), "bmp", ex);
            }
            finally
            {
                if (dispose && null != bmp)
                    bmp.Dispose();
            }

            return imgdata;
        }

        //private static readonly System.Drawing.Imaging.ImageFormat jpegFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
        //private static readonly System.Drawing.Imaging.EncoderParameters jpegParams = new System.Drawing.Imaging.EncoderParameters();

        private static Image ConvertToJpeg(Image bmp)
        {
            //if (bmp.RawFormat.Guid != jpegFormat.Guid)
            //{
            //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //    bmp.Save(ms, jpegFormat);
            //    ms.Flush();
            //    ms.Position = 0;
            //    bmp.Dispose(); //Dispose of this one and use the one with the stream.
            //    return Image.FromStream(ms);

            //}
            //else
            return bmp;
        }

        public static PDFImageData Parse(string data)
        {
            return Parse(data, false);
        }

        public static PDFImageData Parse(string data, bool compress)
        {
            if (string.IsNullOrEmpty(data) == false)
            {
                try
                {
                    byte[] binary = System.Convert.FromBase64String(data);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(binary))
                    {
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
                        return PDFImageData.LoadImageFromBitmap(Guid.NewGuid().ToString(), bmp, compress);
                    }
                }
                catch (Exception ex)
                {
                    throw new PDFException("Cannot convert the BASE64 string to image data", ex);
                }
            }
            else
                return null;
        }




        private static System.Drawing.Bitmap ConvertMetafileToBitmap(System.Drawing.Imaging.Metafile emf)
        {
            using (System.IO.MemoryStream bmpStream = new System.IO.MemoryStream())
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(emf.Width, emf.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    bmp.SetResolution(emf.HorizontalResolution, emf.VerticalResolution);
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                    {
                        g.Clear(System.Drawing.Color.Transparent);
                        g.DrawImage(emf, 0, 0);
                    }
                    bmp.Save(bmpStream, System.Drawing.Imaging.ImageFormat.Png);
                }
                bmpStream.Flush();
                bmpStream.Position = 0;
                System.Drawing.Bitmap final = new System.Drawing.Bitmap(bmpStream);
                return final;
            }


        }

        #region Parse URI data from inline src data:[<MIME-type>][;charset=<encoding>][;base64],<data>

        /**
         * Parse a data uri and return a record 
         * @param uri 
         */
        internal record UriData { public string mediaType; public string parameters; public string encoding; public string data; }

        private static readonly Regex splitter = new Regex(@"^data:([-\w]+\/[-+\w.]+)?((?:;?[\w]+=[-\w]+)*);(base64)?,(.*)");

        


        private static UriData ParseDataURI(string uri)
        {

            var match = splitter.Match(uri);

            return new UriData
            {
                mediaType = match.Groups[1]?.Value,
                parameters = match.Groups[2]?.Value,
                encoding = match.Groups[3]?.Value,
                data = match.Groups[4]?.Value
            };
        }

        

        #endregion
    }
}
