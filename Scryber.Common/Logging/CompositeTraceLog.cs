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
    /// A TraceLog implementation that sends the same message to multiple child logs
    /// </summary>
    public class CompositeTraceLog : TraceLog
    {

        private IEnumerable<TraceLog> _inner;
        
        public CompositeTraceLog(params TraceLog[] entries) : this((IEnumerable<TraceLog>)entries, "Composite")
        {}

        public CompositeTraceLog(IEnumerable<TraceLog> entries, string name)
            : base(GetRecordLevelFromEntries(entries), name)
        {
            if (null == entries)
                throw new ArgumentNullException("entries");
            this._inner = entries;
        }

        /// <summary>
        /// Returns the most verbose record level for the passed logs
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        private static TraceRecordLevel GetRecordLevelFromEntries(IEnumerable<TraceLog> entries)
        {
            TraceRecordLevel min = TraceRecordLevel.Off;
            foreach (TraceLog log in entries)
            {
                if (log.RecordLevel < min)
                    min = log.RecordLevel;
            }
            return min;
        }

        public override void SetRecordLevel(TraceRecordLevel level)
        {
            base.SetRecordLevel(level);
            foreach (TraceLog inner in this._inner)
            {
                inner.SetRecordLevel(level);
            }
        }

        public override TraceLog GetLogWithName(string name)
        {
            TraceLog log = base.GetLogWithName(name);
            if(null == log)
            {
                foreach (TraceLog inner in this._inner)
                {
                    log = inner.GetLogWithName(name);
                    if (null != log)
                        break;
                }
            }
            return log;
        }

        internal protected override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
        {
            foreach (TraceLog log in _inner)
            {
                if (log.ShouldLog(level))
                    log.Record(inset, level, timestamp, category, message, ex);
            }

        }
    }
}
