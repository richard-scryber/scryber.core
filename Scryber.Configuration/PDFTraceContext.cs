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
using System.Threading;

namespace Scryber
{
    /// <summary>
    /// The PDFTraceContext is a static class used to access the current PDFTraceLog
    /// </summary>
    /// <remarks>The log is thread specific and stored in either the 
    /// HttpContext items collection or a ThreadStatic variable</remarks>
    public static class PDFTraceContext
    {
        private const string TraceLogContextKey = "Scryber.TraceContext";

        /// <summary>
        /// Thread static variable (one per thread) to hold the current log.
        /// </summary>
        [ThreadStatic()]
        private static PDFTraceLog _threadlog;

        private static bool IsInWebContext
        {
            get { return System.Web.HttpContext.Current != null; }
        }


        /// <summary>
        /// Gets or sets the current PDFTraceLog from the HttpContext Items collection. 
        /// Check this is in a web request first!
        /// </summary>
        private static PDFTraceLog WebLog
        {
            get
            {
                return System.Web.HttpContext.Current.Items[TraceLogContextKey] as PDFTraceLog;
            }
            set
            {
                System.Web.HttpContext.Current.Items[TraceLogContextKey] = value;
            }
        }

        /// <summary>
        /// Gets the current log - thread static.
        /// If one does not exist for this thread then it will be created
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the document TraceLog instead",true)]
        public static PDFTraceLog GetLog()
        {
            PDFTraceLog log ;
            if (IsInWebContext)
            {
                log =  WebLog;
                if (null == log)
                {
                    log = CreateTraceLog();
                    WebLog = log;
                }
            }
            else
            {
                log = _threadlog;
                //This is thread safe
                if (log == null)
                {
                    log = CreateTraceLog();
                    _threadlog = log;
                }
            }
            
            return log;
        }

         [Obsolete("Use the document TraceLog instead", true)]
        public static void AddLog(PDFTraceLog log)
        {
            //if (null == log)
            //    throw new ArgumentNullException("log");

            //PDFTraceLog current = GetLog();
            //PDFTraceLog composite;

            //if (current is Scryber.Logging.DoNothingTraceLog)
            //    composite = log;
            //else
            //    composite = new Logging.CompositeTraceLog(new PDFTraceLog[] { current, log });

            //if (IsInWebContext)
            //    WebLog = composite;
            //else
            //    _threadlog = composite;

        }

        private static PDFTraceLog CreateTraceLog()
        {
            return Configuration.ScryberConfiguration.GetLog();
        }

        private static PDFTraceLog CreateTraceLog(TraceRecordLevel level)
        {
            return Configuration.ScryberConfiguration.GetLog(level);
        }

        /// <summary>
        /// Clears any existing trace logs and instantiates a new log
        /// based on the configuration.
        /// </summary>
        /// <returns></returns>
        [Obsolete("Use the document TraceLog instead", true)]
        public static PDFTraceLog InitTraceLog()
        {
            PDFTraceLog log;
            if (IsInWebContext)
                log = WebLog;
            else
                log = _threadlog;

            if (log is IDisposable)
                ((IDisposable)log).Dispose();

            log = CreateTraceLog();

            if (IsInWebContext)
                WebLog = log;
            else
                _threadlog = log;
            

            return log;
        }

        /// <summary>
        /// Clears any existing trace logs and instantiates a new log 
        /// based on the configuration with the specified level
        /// </summary>
        /// <param name="level">The level of logging required</param>
        /// <returns>Any configured trace log, or a log instance that does nothing</returns>
         [Obsolete("Use the document TraceLog instead", true)]
        public static PDFTraceLog InitTraceLog(TraceRecordLevel level)
        {
            PDFTraceLog log;
            if (IsInWebContext)
                log = WebLog;
            else
                log = _threadlog;

            if (log is IDisposable)
                ((IDisposable)log).Dispose();

            log = CreateTraceLog(level);

            if (IsInWebContext)
                WebLog = log;
            else
                _threadlog = log;


            return log;
        }


        /// <summary>
        /// Removes the current trace log.
        /// </summary>
         [Obsolete("Use the document TraceLog instead", true)]
        public static void ClearTraceLog()
        {
            PDFTraceLog log;
            if (IsInWebContext)
                log = WebLog;
            else
                log = _threadlog;

            if (log is IDisposable)
                ((IDisposable)log).Dispose();

            if (IsInWebContext)
                WebLog = null;
            else
                _threadlog = null;
            
        }
    }
}
