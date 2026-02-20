using System;
using System.Runtime.CompilerServices;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Native;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Scryber.Imaging
{
    public abstract class PDFImageDataBase : ImageRasterData
    {
        

        public long ImageOutputLength { get; protected set; }
        
        public long ImageDataLength { get; protected set; }
        
        public long AlphaOutputLength { get; protected set; }

        public long AlphaDataLength { get; protected set; }
        
        
        public PDFImageDataBase(SixLabors.ImageSharp.Image img, string source)
            : base(source, img.Width, img.Height)
        {
        }

        public PDFImageDataBase(string source, int width, int height)
            : base(source, width, height)
        {

        }
        
        protected virtual void InitPixelData(bool hasAlpha, ColorSpace color, int bitsPerColor, int colorsPerSample, double horizResolution, double vertResolution, PixelResolutionUnit resolutionUnits)
        {
            this.ColorSpace = color;
            this.HasAlpha = hasAlpha;
            this.BitsPerColor = bitsPerColor;
            this.ColorsPerSample = colorsPerSample;
            this.HorizontalResolution = GetResolution(horizResolution, resolutionUnits);
            this.VerticalResolution = GetResolution(vertResolution, resolutionUnits);

        }

        /// <summary>
        /// Used by the ImageFactories after the image has loaded to update the format.
        /// </summary>
        /// <param name="imageFormat"></param>
        /// <param name="bitDepth"></param>
        /// <param name="hasAlpha"></param>
        /// <param name="colorSpace"></param>
        public virtual void SetSourceImageFormat(ImageFormat imageFormat, int bitDepth, bool hasAlpha, ColorSpace colorSpace)
        {
            this.HasAlpha = hasAlpha;
            this.ColorSpace = colorSpace;
        }

        private int GetResolution(double res, PixelResolutionUnit units)
        {
            switch (units)
            {
                case PixelResolutionUnit.PixelsPerCentimeter:
                    return (int) Math.Round(res * 2.54);
                case PixelResolutionUnit.PixelsPerInch:
                    return (int) Math.Round(res);
                case PixelResolutionUnit.PixelsPerMeter:
                    return (int) Math.Round(((res * 2.54) / 100.0));
                case PixelResolutionUnit.AspectRatio:
                default:
                    return (int) Math.Round(res);
            }
        }

        public override PDFObjectRef Render(PDFName name, IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            bool logverbose = context.TraceLog.ShouldLog(TraceLevel.Verbose);
            bool logmessages = context.TraceLog.ShouldLog(TraceLevel.Message);

            //Set it as the property for visibility, but keep there thread safe a instance parameters
            if (logverbose)
                context.TraceLog.Begin(TraceLevel.Message, "Images", "Began rendering the primary image " + (this.SourcePath ?? "[UNKNOWN]") + " to the output");

            this.Filters = filters;
            PDFObjectRef oref = writer.BeginObject(name.Value);

            try
            {
                writer.BeginDictionaryS();
                writer.WriteDictionaryNameEntry("Name", name.Value);
                writer.WriteDictionaryNameEntry("Type", "XObject");
                writer.WriteDictionaryNameEntry("Subtype", "Image");

                this.WriteImageInformation(filters, context, writer);

                if (logverbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Images", "Rendered the primary image source information with size " + this.PixelWidth + ", " + this.PixelHeight + ", depth " + this.BitsPerColor + " and color space " + this.ColorSpace);


                if (this.HasAlpha)
                {
                    PDFObjectRef alphaRef = this.RenderAlpha(filters, context, writer);
                    if (null != alphaRef)
                    {
                        writer.WriteDictionaryObjectRefEntry("SMask", alphaRef);

                        if (logverbose)
                            context.TraceLog.Add(TraceLevel.Verbose, "Images", "Updated the primary image alpha object reference to " + alphaRef.ToString());
                    }
                    else
                        context.TraceLog.Add(TraceLevel.Warning, "Images", "The primary image alpha object is declared as having an alpha channel, but no object reference was returned.");
                }
                else if (logverbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Images", "The primary image does not have an alpha channel");


                writer.BeginStream(oref, filters);

                this.ImageDataLength = this.DoRenderImageData(filters, context, writer);

                

                this.ImageOutputLength = writer.EndStream();

                if (this.ShouldWriteFilter(filters, this.ImageDataLength, this.ImageOutputLength))
                {
                    if (logverbose)
                        context.TraceLog.Add(TraceLevel.Verbose, "Images",
                            "Updated the primary image data with filters '" + GetFilterName(filters) +
                            " and stream size has gone from " + this.ImageDataLength + " to " + this.ImageOutputLength + " bytes");

                    this.WriteDataStreamInformation(this.Filters, this.ImageOutputLength, writer);
                    
                    if (logverbose)
                        context.TraceLog.Add(TraceLevel.Verbose, "Images", "Rendered the image data with " + this.ImageOutputLength + " bytes");

                }
                else
                {
                    if (logverbose)
                        context.TraceLog.Add(TraceLevel.Verbose, "Images", "Rendered the image data with " + this.ImageDataLength + " bytes and no compression");

                    this.WriteDataStreamInformation(null, this.ImageDataLength, writer);
                }

                writer.EndDictionary();

            }
            catch
            {
                context.TraceLog.Add(TraceLevel.Error, "Images", "Could not render the primary information for image " + (this.SourcePath ?? "[UNKNOWN]"));
                throw;
            }
            finally
            {
                writer.EndObject();

                if (logverbose)
                    context.TraceLog.End(TraceLevel.Message, "Images", "Completed the rendering of primary image object for " + (this.SourcePath ?? "[UNKNOWN]") + " to the output with size " + this.ImageOutputLength + "bytes, and filters " + GetFilterName(filters));
                else if (logmessages)
                    context.TraceLog.Add(TraceLevel.Message, "Images", "Rendered the primary image object for " + (this.SourcePath ?? "[UNKNOWN]") + " to the output with size " + this.ImageOutputLength + "bytes");
            }

            return oref;
        }

        #region protected virtual void WriteImageInformation(IStreamFilter[] filters, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Writes the image details including width, height and bpc to the writer's current dictionary.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        protected virtual void WriteImageInformation(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {

            writer.WriteDictionaryNumberEntry("Width", this.PixelWidth);
            writer.WriteDictionaryNumberEntry("Height", this.PixelHeight);
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceRGB");
            writer.WriteDictionaryNumberEntry("BitsPerComponent", this.BitsPerColor);
        }

        #endregion

        /// <summary>
        /// Inheritors should override this method to render any image data
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected abstract long DoRenderImageData(IStreamFilter[] filters, ContextBase context, PDFWriter writer);

        /// <summary>
        /// Inheritors should override this method to render any aplha channel data
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected abstract long DoRenderAlphaData(IStreamFilter[] filters, ContextBase context, PDFWriter writer);

        #region protected PDFObjectRef RenderAlpha(IStreamFilter[] filters, PDFContextBase context, PDFWriter writer)

        /// <summary>
        /// Renderes the full Alpha channel XObject to the current writer
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="context"></param>
        /// <param name="writer"></param>
        /// <returns></returns>
        protected PDFObjectRef RenderAlpha(IStreamFilter[] filters, ContextBase context, PDFWriter writer)
        {
            bool logverbose = context.TraceLog.ShouldLog(TraceLevel.Verbose);
            bool logmessages = context.TraceLog.ShouldLog(TraceLevel.Message);

            if (logverbose)
                context.TraceLog.Begin(TraceLevel.Message, "Images", "Began rendering the alpha image " + (this.SourcePath ?? "[UNKNOWN]") + " to the output");

            PDFObjectRef mask = writer.BeginObject();

            try
            {
                writer.BeginDictionary();
                writer.WriteDictionaryNameEntry("Type", "XObject");
                writer.WriteDictionaryNameEntry("Subtype", "Image");
                writer.WriteDictionaryNumberEntry("Width", this.PixelWidth);
                writer.WriteDictionaryNumberEntry("Height", this.PixelHeight);
                writer.WriteDictionaryNameEntry("ColorSpace", "DeviceGray");
                writer.WriteDictionaryNumberEntry("BitsPerComponent", 8);

                if (logverbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Images", "Rendered the alpha source information with size " + this.PixelWidth + ", " + this.PixelHeight + " and depth 8");


                writer.BeginStream(mask, filters);

                this.AlphaDataLength = this.DoRenderAlphaData(filters, context, writer);

                if (logverbose)
                    context.TraceLog.Add(TraceLevel.Verbose, "Images", "Rendered the alpha image data with " + this.AlphaDataLength + " bytes");

                this.AlphaOutputLength = writer.EndStream();


                if (this.ShouldWriteFilter(filters, AlphaDataLength, AlphaOutputLength))
                {
                    if (logmessages)
                        context.TraceLog.Add(TraceLevel.Message, "Images", $"Updated the alpha image data with filters '{GetFilterName(filters)}' and stream size has gone from {AlphaDataLength} to {AlphaOutputLength} bytes");

                    this.WriteDataStreamInformation(this.Filters, AlphaOutputLength, writer);
                }
                else
                {
                    this.WriteDataStreamInformation(null, AlphaOutputLength, writer);
                }
                writer.EndDictionary();

            }
            catch
            {
                context.TraceLog.Add(TraceLevel.Error, "Images", "Could not render the alpha channel information for image " + (this.SourcePath ?? "[UNKNOWN]"));
                throw;
            }
            finally
            {
                writer.EndObject();

                if (logverbose)
                    context.TraceLog.End(TraceLevel.Message, "Images", "Completed the rendering of alpha image object for " + (this.SourcePath ?? "[UNKNOWN]") + " to the output with size " + this.AlphaOutputLength + "bytes, " + GetFilterName(filters));
                else if (logmessages)
                    context.TraceLog.Add(TraceLevel.Message, "Images", "Rendered the alpha image object for " + (this.SourcePath ?? "[UNKNOWN]") + " to the output with size " + this.AlphaOutputLength + "bytes");
            }

            return mask;
        }

        #endregion

        #region protected void WriteDataStreamInformation(IStreamFilter[] filters, long streamLength, PDFWriter writer)

        /// <summary>
        /// Writes the names of any filters to the current writer's dictionary and the length of the stream
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="streamLength"></param>
        /// <param name="writer"></param>
        protected void WriteDataStreamInformation(IStreamFilter[] filters, long streamLength, PDFWriter writer)
        {
            if(null == filters || filters.Length == 0)
            {
                //DO nothing
            }
            else if(filters.Length == 1)
            {
                writer.WriteDictionaryNameEntry("Filter", filters[0].FilterName);
            }
            else
            {
                writer.BeginDictionaryEntry("Filter");
                writer.BeginArray();
                foreach (var filter in filters)
                {
                    writer.BeginArrayEntry();
                    writer.WriteName(filter.FilterName);
                    writer.EndArrayEntry();
                }
                writer.EndArray();
                writer.EndDictionaryEntry();
            }

            writer.WriteDictionaryNumberEntry("Length", streamLength);
        }

        #endregion

        public override void ResetFilterCache()
        {
            
        }

        /// <summary>
        /// Returns true if the filters and filtered length should be written to the output otherwise false.
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="dataLength"></param>
        /// <param name="filteredLength"></param>
        /// <returns></returns>
        /// <remarks>Sometimes if we are using Deflate zip compression, the compression does not cause any reduction in size.
        /// If this is the case the raw data is written, rather than the compressed data. So we check to see if there are filters,
        /// and if there is we check to make sure the data filter name is FlateDecode, and it was compressed</remarks>
        private bool ShouldWriteFilter(IStreamFilter[] filters, long dataLength, long filteredLength)
        {
            if (null == filters || filters.Length == 0)
                return false;
            else if (filters.Length == 1 && filters[0].FilterName == PDFDeflateStreamFilter.DefaultFilterName)
                return dataLength != filteredLength;
            else
                return true;
        }

        #region private string GetFilterName(IStreamFilter[] filters)

        /// <summary>
        /// Gets the name(s) of the filter(s) as a string
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        private string GetFilterName(IStreamFilter[] filters)
        {
            if (null == filters || filters.Length == 0)
                return string.Empty;
            else if (filters.Length == 1)
            {
                return filters[0].FilterName;
            }
            else
            {
                var str = "";
                foreach (var item in filters)
                {
                    if (str.Length > 0)
                        str += ", ";
                    str += item.FilterName;
                }
                return str;
            }
        }

        #endregion
    }

    
}
