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
using System.Text;
using System.Configuration;

namespace Scryber.Configuration
{
    public class ScryberConfigurationSection : ConfigurationSection
    {

        #region public string _xmlns
        
        /// <summary>
        /// Backing property so that the xml namespace 
        /// can be declared on the configuration section so we can get intelisense.
        /// </summary>
        [ConfigurationProperty("xmlns")]
        public string @__xmlns_do_not_use
        {
            get { return null; }
            set { }
        }

        #endregion

        private const string TraceKey = "tracing";
        internal const TraceRecordLevel DefaultTraceRecordingLevel = TraceRecordLevel.Messages;

        [ConfigurationProperty(TraceKey,IsRequired=false)]
        public TracingConfigurationSection Tracing
        {
            get 
            {
                TracingConfigurationSection section = this[TraceKey] as TracingConfigurationSection;
                return section;
            }
            set
            {
                this[TraceKey] = value;
            }

        }

        private const string FontMappingSectionKey = "fonts";

        [ConfigurationProperty(FontMappingSectionKey,IsRequired=false)]
        public FontsConfigurationSection FontMappings
        {
            get { return this[FontMappingSectionKey] as FontsConfigurationSection; }
            set { this[FontMappingSectionKey] = value; }
        }
    }
}
