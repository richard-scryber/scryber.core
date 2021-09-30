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

namespace Scryber.PDF
{
    internal class PDFDocumentGenerationData
    {
        public long DocumentSize { get; set; }

        public TimeSpan DocumentGenerationTime { get; set; }

        public TraceRecordLevel TraceLevel { get; set; }

        public Version ScryberVersion { get; set; }

        public Version ScryberFileVersion { get; set; }

        public PDFDocumentID DocumentID { get; set; }

        public string TemplatePath { get; set; }

        public Scryber.Logging.PDFCollectorTraceLog TraceLog { get; set; }

        public Scryber.Data.PDFXmlNamespaceCollection Namespaces { get; set; }

        public System.Collections.Specialized.NameValueCollection ExtraInfo { get; set; }

        public Scryber.Components.DocumentInfo DocumentInfo { get; set; }

        public Scryber.Components.DocumentViewPreferences DocumentViewerPrefs { get; set; }

        public Scryber.PDFPerformanceMonitor PerformanceMetrics { get; set; }

    }
}
