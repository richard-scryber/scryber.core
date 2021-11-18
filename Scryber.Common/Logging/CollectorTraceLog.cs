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

namespace Scryber.Logging
{

    /// <summary>
    /// A Trace log that collects all the entries into a list of
    /// PDFCollectorTraceLogEntry instances, that can be enumerated over later.
    /// </summary>
    #region public class PDFCollectorTraceLog : PDFTraceLog, IEnumerable<PDFCollectorTraceLogEntry>

    public class CollectorTraceLog : TraceLog, IEnumerable<CollectorTraceLogEntry>
    {

        private List<CollectorTraceLogEntry> _entries;
        private bool _collecting;

        /// <summary>
        /// Gets the number of entries in this collector
        /// </summary>
        public int Count
        {
            get { return _entries.Count; }
        }

        /// <summary>
        /// Retrieves the entry at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CollectorTraceLogEntry this[int index]
        {
            get { return _entries[index]; }
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="recordlevel"></param>
        public CollectorTraceLog(TraceRecordLevel recordlevel, string name, bool autostart)
            : base(recordlevel, name)
        {
            this._entries = new List<CollectorTraceLogEntry>();
            this._collecting = autostart;
        }

        /// <summary>
        /// Overrides the base implementation to add a new entry to this collector
        /// </summary>
        /// <param name="inset"></param>
        /// <param name="level"></param>
        /// <param name="timestamp"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        protected internal override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
        {
            if(this._collecting)
                this._entries.Add(new CollectorTraceLogEntry(inset, level, timestamp, category, message, ex));
        }

        public void StopCollecting()
        {
            this._collecting = false;
        }

        public void StartCollecting()
        {
            this._collecting = true;
        }

        /// <summary>
        /// Removes all the entries from this current log
        /// </summary>
        public void Clear()
        {
            this._entries.Clear();
        }

        /// <summary>
        /// Returns a new enumerator that can loop over each of the entries in this collector
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CollectorTraceLogEntry> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        /// <summary>
        /// Explicit interface implementation of the IEnumerable interface
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    #endregion


    /// <summary>
    /// A single log entry that is created by the CollectorTraceLog, and holds all the information about a single log entry.
    /// </summary>
    #region public class CollectorTraceLogEntry

    public class CollectorTraceLogEntry
    {

        /// <summary>
        /// Gets the inset string (depth)
        /// </summary>
        public string Inset { get; }

        /// <summary>
        /// Gets the trace level of this entry
        /// </summary>
        public TraceLevel Level { get; }

        /// <summary>
        /// Gets the timestamp of this entry
        /// </summary>
        public TimeSpan TimeStamp { get; }

        /// <summary>
        /// Gets the Category for this entry
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the message for this entry
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets any exception associated with this entry
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Returns true if this entry has a non-null exception associated with it.
        /// </summary>
        public bool HasException => null != Exception;

        /// <summary>
        /// Creates a new fully initialized collector log entry.
        /// </summary>
        /// <param name="inset"></param>
        /// <param name="level"></param>
        /// <param name="timestamp"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public CollectorTraceLogEntry(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
        {
            this.Inset = inset;
            this.Level = level;
            this.TimeStamp = timestamp;
            this.Category = category;
            this.Message = message;
            this.Exception = ex;
        }

        
    }

    #endregion


    /// <summary>
    /// Implements the IPDFTraceLogFactory to have an empty constructor and rapidly create instances of the PDFCollectorTraceLog
    /// </summary>
    #region public class CollectorTraceLogFactory : IPDFTraceLogFactory

    public class CollectorTraceLogFactory : IPDFTraceLogFactory
    {

        public CollectorTraceLogFactory()
        {
        }

        /// <summary>
        /// Creates a new instance of the CollectorTraceLog and returns it.
        /// </summary>
        /// <param name="level">The standard recording level of the collector log to create</param>
        /// <param name="name">The internal name of the trace log to create</param>
        /// <returns></returns>
        public TraceLog CreateLog(TraceRecordLevel level, string name)
        {
            return new CollectorTraceLog(level, name, true);
        }
    }

    #endregion
}
