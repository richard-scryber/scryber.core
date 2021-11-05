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
using Scryber.Logging;

namespace Scryber
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class ContextBase
    {
        private OutputFormat _format;
        private ItemCollection _items;
        private TraceLog _log;
        private PerformanceMonitor _perfmon;
        private ParserConformanceMode _conformance;
        private IDocument _doc;
        


        public ParserConformanceMode Conformance
        {
            get { return _conformance; }
            set { _conformance = value; }
        }

        public TraceLog TraceLog
        {
            get { return _log; }
            set { _log = value; }
        }

        public ItemCollection Items
        {
            get { return this._items; }
            set { this._items = value; }
        }

        public PerformanceMonitor PerformanceMonitor
        {
            get { return _perfmon; }
            set { _perfmon = value; }
        }

        /// <summary>
        /// returns true if Debug entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogDebug
        {
            get { return this.TraceLog.RecordLevel <= TraceRecordLevel.Diagnostic; }
        }

        /// <summary>
        /// returns true if Verbose entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogVerbose
        {
            get { return this.TraceLog.RecordLevel <= TraceRecordLevel.Verbose; }
        }

        /// <summary>
        /// returns true if Message entries should be logged based on the current logging level
        /// </summary>
        public bool ShouldLogMessage
        {
            get { return this.TraceLog.RecordLevel <= TraceRecordLevel.Messages; }
        }

        public IDocument Document
        {
            get { return this._doc; }
        }


        public OutputFormat Format
        {
            get { return _format; }
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

        public ContextBase(ItemCollection items, TraceLog log, PerformanceMonitor perfmon, IDocument document, OutputFormat format)
        {

            this._log = log;
            if (null == log)
                _log = new Logging.DoNothingTraceLog(Scryber.TraceRecordLevel.Off);

            

            this._items = items;
            this._perfmon = perfmon;
            this._doc = document;
            this._format = format;
        }

    }
}
