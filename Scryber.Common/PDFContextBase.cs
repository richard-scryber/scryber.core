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

namespace Scryber
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class PDFContextBase
    {

        private PDFItemCollection _items;
        private OutputFormat _format;
        private PDFTraceLog _log;
        private PDFPerformanceMonitor _perfmon;
        private ParserConformanceMode _conformance;
        private IDocument _doc;
        private bool _shouldLogDebug;
        private bool _shouldLogVerbose;
        private bool _shouldLogMessage;


        public ParserConformanceMode Conformance
        {
            get { return _conformance; }
            set { _conformance = value; }
        }

        public PDFTraceLog TraceLog
        {
            get { return _log; }
            set { _log = value; }
        }

        public PDFItemCollection Items
        {
            get { return this._items; }
            set { this._items = value; }
        }

        public PDFPerformanceMonitor PerformanceMonitor
        {
            get { return _perfmon; }
            set { _perfmon = value; }
        }

        public OutputFormat OutputFormat
        {
            get { return this._format; }
            set { this._format = value; }
        }

        /// <summary>
        /// returns true if Debug entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogDebug
        {
            get { return _shouldLogDebug; }
        }

        /// <summary>
        /// returns true if Verbose entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogVerbose
        {
            get { return _shouldLogVerbose; }
        }

        /// <summary>
        /// returns true if Message entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogMessage
        {
            get { return _shouldLogMessage; }
        }

        public IDocument Document
        {
            get { return this._doc; }
        }


        #region public OutputCompression Compression {get;set;}

        private OutputCompressionType _compress = OutputCompressionType.FlateDecode;

        /// <summary>
        /// Gets or sets the use of compression in the content streams
        /// </summary>
        public OutputCompressionType Compression
        {
            get { return _compress; }
            set { _compress = value; }
        }

        #endregion

        public PDFContextBase(PDFItemCollection items, PDFTraceLog log, PDFPerformanceMonitor perfmon, IDocument document)
        {
            this._format = OutputFormat.PDF;

            this._log = log;
            if (null == log)
                _log = new Logging.DoNothingTraceLog(Scryber.TraceRecordLevel.Off);
            _shouldLogDebug = TraceRecordLevel.Diagnostic >= _log.RecordLevel;
            _shouldLogVerbose = TraceRecordLevel.Verbose >= _log.RecordLevel;
            _shouldLogMessage = TraceRecordLevel.Messages >= _log.RecordLevel;
            this._items = items;
            this._perfmon = perfmon;
            this._doc = document;
        }

    }
}
