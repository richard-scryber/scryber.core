using System;
using System.Text;
using Scryber.PDF.Native;
using Scryber.PDF;
namespace Scryber.Drawing.Imaging
{
    /// <summary>
    /// Represents a binary image where the data is held as a byte array
    /// </summary>
    public class PDFBinaryImageData : ImageData
    {

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

        public PDFBinaryImageData(string source, int width, int height)
            : base(source, width, height)
        {
        }


        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            if (context.ShouldLogDebug)
                context.TraceLog.Begin(TraceLevel.Message, "Image Data", "Rendering image data for '" + name.ToString() + "'");

            //Legacy class supporting the new render method with XObject filters passed
            this.Filters = filters;

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



        protected virtual void RenderImageInformation(ContextBase context, PDFWriter writer)
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

        protected virtual bool ShouldApplyFilters(ContextBase context)
        {
            if (this.HasFilter)
            {
                if (context.Compression == OutputCompressionType.FlateDecode || this.IsPrecompressedData)
                    return true;
            }
            return false;
        }

        protected void WriteFilterNames(ContextBase context, PDFWriter writer)
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

        protected virtual int RenderImageStreamData(ContextBase context, PDFWriter writer)
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

            if (context.ShouldLogDebug)
                context.TraceLog.Add(TraceLevel.Debug, "Image Data", "Written raw image data to output with length " + data.Length);

            //Added for data length fix - HRB 15/01/2015
            return data.Length;
            //End of add
        }

        protected byte[] ApplyFiltersToData(byte[] data, ContextBase context)
        {
            if (this.ShouldApplyFilters(context))
            {
                int origlength = data.Length;
                if (context.ShouldLogDebug)
                    context.TraceLog.Add(TraceLevel.Debug, "ImageData", "Applying filters to image data for '" + this.SourcePath + "'. Original length = " + origlength);

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

        private string _filtercachekey = null;
        private byte[] _filterdata = null;

        public override void ResetFilterCache()
        {
            this._filtercachekey = null;
            this._filterdata = null;
        }

        public byte[] GetCachedFilteredData(IStreamFilter[] filters, ContextBase context)
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

        protected void SetCachedFilteredData(IStreamFilter[] filters, byte[] data, ContextBase context)
        {
            if (null == filters)
                throw new ArgumentNullException("filters");
            else if (filters.Length == 0)
                throw new ArgumentOutOfRangeException("filters");

            if (null == data)
                throw new ArgumentNullException("data");

            string key = GetFullFilterCacheKey(filters);

            this._filtercachekey = key;
            this._filterdata = data;

        }
    }
}
