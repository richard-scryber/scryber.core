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

namespace Scryber.Components
{
    public abstract class PDFWriterFactory : IDisposable
    {
        private bool _disposed = false;

        public PDFWriter GetInstance(Document forDoc, System.IO.Stream stream, int generation, PDFDocumentRenderOptions options, PDFTraceLog log)
        {
            if (null == stream)
                throw new ArgumentNullException("stream");
            else if (null == options)
                throw new ArgumentNullException("options");
            else if (null == log)
                throw new ArgumentNullException("log");
            else if (_disposed)
                throw new InvalidOperationException(Errors.DocumentHasBeenDisposed);
            else
            {
                return DoGetInstance(forDoc, stream, generation, options, log);
            }
        }

        protected abstract PDFWriter DoGetInstance(Document forDoc, System.IO.Stream stream, int generation, PDFDocumentRenderOptions options, PDFTraceLog log);

        public void Dispose()
        {
            this.Dispose(true);
            this._disposed = true;
        }

        protected void Dispose(bool disposing)
        {
        }

        ~PDFWriterFactory()
        {
            this.Dispose(false);
        }

    }

    [PDFParsableComponent("Writer")]
    public class PDFDocumentWriter : PDFWriterFactory
    {
        /// <summary>
        /// If true (default) then object streams will be pooled and reused - which improves speed significantly.
        /// If false then the layout of the final PDF document will be linear
        /// </summary>
        [PDFAttribute("pooled")]
        public bool PooledStreams
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version number or the PDF standard this document should / does conform to.
        /// </summary>
        [PDFAttribute("pdf-version")]
        public string PDFVersion
        {
            get;
            set;
        }

        public PDFDocumentWriter() :
            this(true, "1.4")
        {

        }

        protected PDFDocumentWriter(bool pooled, string version)
        {
            this.PooledStreams = pooled;
            this.PDFVersion = version;
        }

        protected override PDFWriter DoGetInstance(Document forDoc, System.IO.Stream stream, int generation, PDFDocumentRenderOptions options, PDFTraceLog log)
        {
            Version vers = (string.IsNullOrEmpty(this.PDFVersion)) ? new Version(1, 4) : System.Version.Parse(this.PDFVersion);

            if (this.PooledStreams)
                return new PDFWriterPooled14(stream, generation, log, vers);
            else
                return new PDFWriter14(stream, generation, log, vers);
        }

    }
}
