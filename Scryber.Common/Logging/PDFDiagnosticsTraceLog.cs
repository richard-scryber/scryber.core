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
    /// A TraceLog implementation that writes to System.Diagnostics.Trace
    /// </summary>
    public class DiagnosticsTraceLog : PDFTraceLog
    {
        public DiagnosticsTraceLog(TraceRecordLevel level, string name)
            : base(level, name, "   ")
        {
        }

        private int _traceindex = 1;
        internal protected override void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex)
        {
            _traceindex++;
            StringBuilder sb = new StringBuilder();
            string s = _traceindex.ToString().PadLeft(5, '0');
            sb.Append(s);
            sb.Append(InsetString);
            s = timestamp.ToString();
            sb.Append(s);
            sb.Append(InsetString);
            string levelS;
            switch (level)
            {
                case TraceLevel.Failure:
                    levelS = "FAILURE:";
                    break;
                case TraceLevel.Error:
                    levelS = "ERROR:";
                    break;
                case TraceLevel.Warning:
                    levelS = "Warning:";
                    break;
                case TraceLevel.Message:
                    levelS = "Message:";
                    break;
                case TraceLevel.Debug:
                    levelS = "Debug:";
                    break;
                default:
                    levelS = " ";
                    break;
            }
            levelS = levelS.PadRight(11);
            sb.Append(levelS);
            if (string.IsNullOrEmpty(inset) == false)
                sb.Append(inset);
            if (string.IsNullOrEmpty(category) == false)
            {
                sb.Append(category);
                sb.Append(" ");
            }
            sb.AppendLine(message);

            while (ex != null)
            {
                sb.Append(InsetString);
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
                sb.AppendLine("");
                ex = ex.InnerException;
            }
            System.Diagnostics.Trace.Write(sb.ToString());
        }
    }

    public class PDFDiagnoticsTraceLogFactory : IPDFTraceLogFactory
    {

        public PDFTraceLog CreateLog(TraceRecordLevel level, string name)
        {
            return new DiagnosticsTraceLog(level, name);
        }
    }
}
