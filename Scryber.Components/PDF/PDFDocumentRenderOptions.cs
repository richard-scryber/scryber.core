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
using System.Security;
using System.Text;
using Scryber.Components;
using Scryber.PDF.Secure;
using Scryber.Logging;

namespace Scryber.PDF
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

        [PDFAttribute("font-substitute")]
        public bool UseFontSubstitution
        {
            get;set;
        }

        
        [PDFElement("")]
        public Scryber.PDF.PDFWriterFactory WriterFactory
        {
            get;
            set;
        }


        public PDFDocumentRenderOptions()
        {
            var config = ServiceProvider.GetService<IScryberConfigurationService>();

            if (null == config)
                config = new Scryber.Utilities.ScryberDefaultConfigurationService();

            var output = config.OutputOptions;
            var imging = config.ImagingOptions;
            var font = config.FontOptions;

            this.UseFontSubstitution = font.FontSubstitution;
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
            sb.Append("PDF Compliance = ");
            sb.Append(this.OuptputCompliance);
            sb.Append(", Compression = ");
            sb.Append(this.Compression);
            sb.Append(", Names = ");
            sb.Append(this.ComponentNames);
            return sb.ToString();
        }

        public PDFWriter CreateWriter(Document forDoc, System.IO.Stream outputStream, int generation, TraceLog log)
        {
            if (null == this.WriterFactory)
            {
                IDocumentPasswordSettings settings = null;
                if (forDoc.PasswordProvider != null && forDoc.PasswordProvider.IsSecure(forDoc.LoadedSource, out settings))
                    this.WriterFactory = GetSecureWriter(forDoc, settings);

                else if(forDoc.Permissions.HasRestrictions)
                {
                    if (forDoc.ConformanceMode == ParserConformanceMode.Lax)
                    {
                        forDoc.TraceLog.Add(TraceLevel.Error, "Writer", "No Password provider has been set on the document, so using a random password for this generation. As a minimum an owner password should be set.");
                        Guid pass = Guid.NewGuid();
                        var passS = pass.ToString().Substring(14);

                        forDoc.PasswordProvider = new DocumentPasswordProvider(passS);
                        forDoc.PasswordProvider.IsSecure(forDoc.LoadedSource, out settings);

                        this.WriterFactory = GetSecureWriter(forDoc, settings);
                    }
                    else
                        throw new System.Security.SecurityException("No Password provider has been set on the document, so restrictions cannot be applied. As a minimum an owner password should be set.");
                }
                else
                    this.WriterFactory = GetStandardWriter(forDoc);
            }
            return this.WriterFactory.GetInstance(forDoc, outputStream, generation, this, log);
            
        }

        protected virtual PDF.PDFWriterFactory GetSecureWriter(Document forDoc, IDocumentPasswordSettings settings)
        {
            if (null == forDoc)
                throw new ArgumentNullException("forDoc");
            
            SecureString owner = null;
            SecureString user = null;

            PDFDocumentID id = forDoc.DocumentID;
            var permissions = forDoc.Permissions;
            

            if (null != settings)
            {
                owner = settings.OwnerPassword;
                user = settings.UserPassword;
            }
            
            if(null == id)
            {
                id = PDFDocumentID.Create();
                forDoc.DocumentID = id;
            }

            if (null == permissions)
                permissions = new DocumentPermissions();

            var encFactory = permissions.GetFactory();
            var enc = encFactory.InitEncrypter(owner, user, id, permissions.GetRestrictions(), forDoc.PerformanceMonitor);
            
            return new PDF.Secure.PDFSecureWrite14Factory(enc);
        }

        protected virtual PDF.PDFWriterFactory GetStandardWriter(Document doc)
        {
            return new PDF.PDFDocumentWriterFactory();
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
