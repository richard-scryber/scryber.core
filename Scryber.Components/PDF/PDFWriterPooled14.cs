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
using Scryber.PDF.Native;
using Scryber.Logging;

namespace Scryber.PDF
{
    

    /// <summary>
    /// The pooled writer has pre-loaded a set of memory streams for use
    /// </summary>
    public class PDFWriterPooled14 : PDFWriter14
    {
        private static int DefaultPoolCount = 10;
        private static TraceLevel PoolTraceLevel = TraceLevel.Debug;
        private static string PoolTraceCategory = "PDF Pooled Writer";

        /// <summary>
        /// Flag to identify if the header has been writen to the underlying stream
        /// </summary>
        private bool _writtenHeader = false;

        /// <summary>
        /// The allocated pool of objects - initialized to this value, but can grow.
        /// </summary>
        private int _poolcount;

        /// <summary>
        /// A list of streams that have previously been allocated and then closed, and can now be used.
        /// </summary>
        private List<PDFStream> _available;

        /// <summary>
        /// All the stream that are currently in use and not closed
        /// </summary>
        private List<PDFStream> _inuse;

        public PDFWriterPooled14(System.IO.Stream stream, int generation, TraceLog log, Version vers)
            : base(stream, generation, log, vers)
        {
            InitStreamPool(DefaultPoolCount);
        }

        protected virtual void InitStreamPool(int poolcount)
        {
            _available = new List<PDFStream>(poolcount);
            _inuse = new List<PDFStream>(poolcount);
            _poolcount = poolcount;
        }

        public override void OpenDocument()
        {
            base.OpenDocument();
            this.WritePDFHeader();
        }

        public override void OpenDocument(PDFFile orig, bool copyToDestination)
        {
            base.OpenDocument(orig, copyToDestination);
        }

        protected override void WritePDFHeader()
        {
            if (_writtenHeader == false)
            {
                base.WritePDFHeader();
                _writtenHeader = true;
            }
        }

        public override PDFObjectRef BeginObject(string name)
        {
            return base.BeginObject(name);
        }

        public override void EndObject()
        {
            PDFIndirectObject obj = this.Stack.Peek().IndirectObject;
            this.WriteAnIndirectObject(obj);
            base.EndObject();
        }

        protected override void ReleaseIndirectObject(IIndirectObject obj)
        {
            base.ReleaseIndirectObject(obj);
            if (obj is PDFIndirectObject)
            {
                if (this.TraceLog.ShouldLog(PoolTraceLevel))
                    this.TraceLog.Add(PoolTraceLevel, PoolTraceCategory, "Writing object data for " + obj.ToString() + " and releasing back onto the pool");

                PDFIndirectObject known = (PDFIndirectObject)obj;

                if (known.Written == false)
                    this.WriteAnIndirectObject(known);
                
                PDFStream data = known.ObjectData;
                PDFStream content = known.HasStream ? known.Stream : null;

                if (data != null)
                    this.ReleaseAndRePool(data);

                if (content != null)
                    this.ReleaseAndRePool(content);

                known.ReleaseStreams(false);
            }
        }

        private void ReleaseAndRePool(PDFStream stream)
        {
            stream.Reset();
            this._available.Add(stream);
        }

        public override PDFStream CreateStream(IStreamFilter[] filters, IIndirectObject forobject)
        {
            if (this._available.Count > 0 && forobject is PDFIndirectObject)
            {
                if (this.TraceLog.ShouldLog(PoolTraceLevel))
                    this.TraceLog.Add(PoolTraceLevel, PoolTraceCategory, "Dequeued a previously allocated PDFStream");

                int index = this._available.Count-1;
                PDFStream last = this._available[index];
                last.Filters = filters;
                last.IndirectObject = (PDFIndirectObject)forobject;
                this._available.RemoveAt(index);

                return last;
            }
            else
                return base.CreateStream(filters, forobject);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose, PoolTraceCategory, "Disposing of the " + this._available.Count + " pooled streams");

                foreach (PDFStream pooled in this._available)
                {
                    pooled.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }


}
