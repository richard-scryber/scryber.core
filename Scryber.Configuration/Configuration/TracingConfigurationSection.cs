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
using System.Configuration;

namespace Scryber.Configuration
{
    public class TracingConfigurationSection : System.Configuration.ConfigurationSection
    {
        private const string TraceLevelKey = "trace-level";

        private object _knownTraceLevel = null;
        /// <summary>
        /// The configured trace level
        /// </summary>
        [ConfigurationProperty(TraceLevelKey, IsRequired = false)]
        public TraceRecordLevel TraceLevel
        {
            get
            {
                if (null == _knownTraceLevel)
                {
                    object value = this[TraceLevelKey];
                    if (null == value || !(value is TraceRecordLevel))
                        _knownTraceLevel = PDFTraceLog.DefaultTraceRecordLevel;
                    else
                        _knownTraceLevel = (TraceRecordLevel)value;
                }
                return (TraceRecordLevel)_knownTraceLevel;
            }
            set
            {
                _knownTraceLevel = value;
            }
        }

        private const string LogCollectionKey = "";

        [ConfigurationCollection(typeof(TracingLogElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap,
            AddItemName = "log", RemoveItemName = "remove", ClearItemsName = "clear")]
        [ConfigurationProperty(LogCollectionKey, IsDefaultCollection = true, IsRequired = false)]
        public TracingLogCollection LogEntries
        {
            get
            {
                TracingLogCollection col = this[LogCollectionKey] as TracingLogCollection;

                return col;
            }
            set
            {
                this[LogCollectionKey] = value;
            }
        }


        public PDFTraceLog GetLog()
        {
            TraceRecordLevel level = this.TraceLevel;
            return this.GetLog(level);
        }

        public PDFTraceLog GetLog(TraceRecordLevel level)
        {
            List<PDFTraceLog> all = new List<PDFTraceLog>();
            foreach (TracingLogElement ele in this.LogEntries)
            {
                if (ele.Enabled)
                {
                    IPDFTraceLogFactory fact = ele.GetFactory();
                    all.Add(fact.CreateLog(level,ele.Name));
                }
            }
            if (all.Count == 0)
                return new Logging.DoNothingTraceLog(level);
            else if (all.Count == 1)
                return all[0];
            else
            {
                return new Logging.CompositeTraceLog(all, string.Empty);
            }
        }
    }
}
