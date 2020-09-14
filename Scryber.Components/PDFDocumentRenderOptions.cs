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
using Scryber.Components;

namespace Scryber
{
    /// <summary>
    /// Wrapper class that defines the options used when rendering the document. 
    /// </summary>
    /// <remarks>Most of these options are initialized from configuration values, 
    /// but can be independantly set on the document itself.</remarks>
    [PDFParsableComponent("Render-Options")]
    public class PDFDocumentRenderOptions : IDisposable
    {

        /// <summary>
        /// Specifies if the document should render all component names, or just the required names (for navigation etc)
        /// </summary>
        [PDFAttribute("component-names")]
        public ComponentNameOutput ComponentNames
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies whether the fnial output images and or streams should be compressed.
        /// </summary>
        [PDFAttribute("compression-type")]
        public OutputCompressionType Compression
        {
            get;
            set;
        }

        [PDFAttribute("image-compression")]
        public ImageCompressionType ImageCompression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version number or the PDF standard this document should / does conform to.
        /// </summary>
        [Obsolete("PDF Version is now set on the writer factory instance, rather than the render options.", true)]
        public string PDFVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the PDF output compliance for the final document - Not currently supported.
        /// </summary>
        [PDFAttribute("output-compliance")]
        public string OuptputCompliance
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies how textual content should be included in the stream data or other PDF indirect objects.
        /// </summary>
        [PDFAttribute("string-output")]
        public OutputStringType StringOutput
        {
            get;
            set;
        }

        /// <summary>
        /// Specifies the default duration of caching for images loaded and used by this document
        /// </summary>
        [PDFAttribute("img-cache-mins")]
        public int ImageCacheDurationMinutes
        {
            get;
            set;
        }

        /// <summary>
        /// If True then images that cannot be loaded will be replaced with broken image data. 
        /// If false then an error will be thrown, and all document processing halted.
        /// </summary>
        [PDFAttribute("allow-missing-images")]
        public bool AllowMissingImages
        {
            get;
            set;
        }

        /// <summary>
        /// If true (default) then object streams will be pooled and reused - which improves speed significantly.
        /// If false then the layout of the final PDF document will be linear
        /// </summary>
        [Obsolete("Pooled streams is now set on the writer factory instance, rather than the render options.", true)]
        public bool PooledStreams
        {
            get;
            set;
        }

        [PDFElement("")]
        public Scryber.Components.PDFWriterFactory WriterFactory
        {
            get;
            set;
        }


        public PDFDocumentRenderOptions()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();
            var output = config.OutputOptions;
            var imging = config.ImagingOptions;

            
            this.Compression = output.Compression;
            this.ComponentNames = output.NameOutput;
            //this.PDFVersion = section.PDFVersion;
            this.OuptputCompliance = output.Compliance.ToString();
            this.StringOutput = output.StringType;
            this.ImageCacheDurationMinutes = imging.ImageCacheDuration;
            this.AllowMissingImages = imging.AllowMissingImages;
            //this.PooledStreams = true;
            this.WriterFactory = null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("PDF Version = ");
            //sb.Append(this.PDFVersion);
            sb.Append("PDF Compliance = ");
            sb.Append(this.OuptputCompliance);
            sb.Append(", Compression = ");
            sb.Append(this.Compression);
            sb.Append(", Names = ");
            sb.Append(this.ComponentNames);
            //sb.Append(", Pooled = ");
            //sb.Append(this.PooledStreams);
            return sb.ToString();
        }

        public PDFWriter CreateWriter(Document forDoc, System.IO.Stream outputStream, int generation, PDFTraceLog log)
        {
            if (null == this.WriterFactory)
                this.WriterFactory = new Scryber.Components.PDFDocumentWriter();

            return this.WriterFactory.GetInstance(forDoc, outputStream, generation, this, log);
            
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != this.WriterFactory)
                    this.WriterFactory.Dispose();
            }
        }

        ~PDFDocumentRenderOptions()
        {
            this.Dispose(false);
        }
        
    }
}
