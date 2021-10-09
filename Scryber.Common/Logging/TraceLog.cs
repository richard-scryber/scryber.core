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

namespace Scryber.Logging
{
    /// <summary>
    /// Base implementation of all trace logs for the Scryber library
    /// </summary>
    public abstract class TraceLog : IDisposable
    {

        public const string ScryberAppendTraceLogName = "_scryber.appendcollectorlog_";

        public static TraceRecordLevel DefaultTraceRecordLevel = TraceRecordLevel.Messages;
        protected const string InsetString = "   ";

        #region ivars

        private System.Diagnostics.Stopwatch _stopwatch;
        private TraceRecordLevel _level;
        private string _inset = string.Empty;
        private string _indent = string.Empty;
        private string _logname = string.Empty;

        #endregion

        //
        // properties
        //

        #region public TraceRecordLevel RecordLevel {get;}

        /// <summary>
        /// Gets the level at or above which this trace log should record messages
        /// </summary>
        public TraceRecordLevel RecordLevel
        {
            get { return this._level; }
        }

        #endregion

        #region public string Indent {get;}

        /// <summary>
        /// Gets the indentation string 
        /// </summary>
        public string Indent
        {
            get { return this._indent; }
        }

        #endregion

        #region public string Name {get;}

        /// <summary>
        /// Gets the configured name of this log
        /// </summary>
        public string Name
        {
            get { return this._logname; }
        }

        #endregion

        //
        // ctors
        //

        #region protected PDFTraceLog(TraceRecordLevel recordlevel, string name)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordlevel"></param>
        protected TraceLog(TraceRecordLevel recordlevel, string name)
            : this(recordlevel, name, InsetString)
        {
        }

        #endregion

        #region protected PDFTraceLog(TraceRecordLevel recordlevel, string name, string insetstring)

        protected TraceLog(TraceRecordLevel recordlevel, string name, string insetstring)
        {
            this._level = recordlevel;
            this._stopwatch = new System.Diagnostics.Stopwatch();
            this._inset = insetstring;
            this._logname = name;
            //this.Add(TraceLevel.Message, "Trace log started");
            this._stopwatch.Start();
            
        }

        #endregion

        //
        // public interface
        //

        #region public TimeSpan GetTimeStamp()

        /// <summary>
        /// Returns the current elapsed time span since this trace log was initiated
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTimeStamp()
        {
            if (_stopwatch == null)
                return TimeSpan.Zero;
            else
                return _stopwatch.Elapsed;
        }

        #endregion

        #region public void Begin(TraceLevel level, string message) + 1 overload

        /// <summary>
        /// Starts a new section of logging with the specified level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void Begin(TraceLevel level, string category, string message)
        {
            
            this.Add(level, category, message);
            this.IncrementIndent();
        }

        /// <summary>
        /// Starts a new section of logging at the Message level
        /// </summary>
        /// <param name="message"></param>
        public void Begin(string category, string message)
        {
            this.Add(TraceLevel.Message, category, message);
            this.IncrementIndent();
        }

        #endregion

        #region public void End(string message) + 1 overload

        /// <summary>
        /// Ends a section of logging at the Message level
        /// </summary>
        /// <param name="message"></param>
        public void End(string category, string message)
        {
            this.DecrementIndent();
            this.Add(TraceLevel.Message, category, message);
            
        }

        /// <summary>
        /// Ends a section of logging at the specfied level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void End(TraceLevel level, string category, string message)
        {
            this.DecrementIndent();
            this.Add(level, category, message);
            
        }

        #endregion

        #region public void Add(string category, string message) + 4 overloads

        /// <summary>
        /// Adds a new entry to the log with the Message trace level in the specified category and with the specified message.
        /// If this trace log's record level is higher than the Message level - it will not be recorded.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public void Add(string category, string message)
        {
            this.Add(TraceLevel.Message, category, message);
        }

        /// <summary>
        /// Adds a new entry to the log with the specified trace level in the specified category and with the specified message.
        /// If this trace log's record level is higher than the specified level - it will not be recorded.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        public void Add(TraceLevel level, string category, string message)
        {
            if (ShouldLog(level))
                this.Record(this.Indent, level, GetTimeStamp(), category, message, null);
        }

        /// <summary>
        /// Adds a new exception entry to the log with the Error trace level.
        /// If this trace log's record level is higher than the Error level (Off) - it will not be recorded.
        /// </summary>
        /// <param name="ex"></param>
        public void Add(Exception ex)
        {
            if(ex != null)
                this.Add(TraceLevel.Error, ex.Message, ex);
        }

        /// <summary>
        /// Adds a new exception entry to the log with the specified trace level.
        /// If this trace log's record level is higher than the specified level - it will not be recorded.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void Add(TraceLevel level, string message, Exception ex)
        {
            if (ShouldLog(level))
                this.Record(this.Indent, level, this.GetTimeStamp(), ex.GetType().Name, message, ex);
        }

        /// <summary>
        /// Adds a new exception entry to the log with the specified trace level along with the Category and message.
        /// If this trace log's record level is higher than the specified level - it will not be recorded.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public void Add(TraceLevel level, string category, string message, Exception ex)
        {
            if (ShouldLog(level))
                this.Record(this.Indent, level, this.GetTimeStamp(), category, message, ex);
        }

        #endregion

        #region public bool ShouldLog(TraceLevel traceLevel)

        /// <summary>
        /// Returns true if this log would acturally record an entry at the specified level. 
        /// Usefull for entries that have string formatting or concatenation in them, which should be avoided if they would not actually be needed.
        /// </summary>
        /// <param name="traceLevel"></param>
        /// <returns></returns>
        public bool ShouldLog(TraceLevel traceLevel)
        {
            return (int)traceLevel >= (int)this._level;
        }

        #endregion

        #region public virtual void SetRecordLevel(TraceRecordLevel level)

        /// <summary>
        /// Changes the current recording level to the specified value
        /// </summary>
        /// <param name="level"></param>
        public virtual void SetRecordLevel(TraceRecordLevel level)
        {
            this._level = level;
        }

        #endregion

        public virtual TraceLog GetLogWithName(string name)
        {
            if(string.Equals(this.Name,name))
                return this;
            else
                return null;
        }

        //
        // protected implementation
        //

        #region protected virtual void IncrementIndent()

        /// <summary>
        /// Increases the current indentation of this log by one
        /// </summary>
        protected virtual void IncrementIndent()
        {
            _indent += _inset;
        }

        #endregion

        #region protected virtual void DecrementIndent()

        /// <summary>
        /// decreases the current indetation of this log by one
        /// </summary>
        protected virtual void DecrementIndent()
        {
            if (_indent.Length > _inset.Length)
                _indent = _indent.Substring(0, _indent.Length - _inset.Length);
            else
                _indent = string.Empty;
        }

        #endregion

        /// <summary>
        /// Inheritors must override this method to actually perform the recording of the message against the implementors log
        /// </summary>
        /// <param name="inset"></param>
        /// <param name="level"></param>
        /// <param name="timestamp"></param>
        /// <param name="category"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        internal protected abstract void Record(string inset, TraceLevel level, TimeSpan timestamp, string category, string message, Exception ex);

        #region IDisposable implementation

        /// <summary>
        /// Disposes of this instance and any unmanager resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// virtual method that inheritors can override to perform any clean up required.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                this._stopwatch.Stop();
                this._stopwatch = null;
            }
        }

        ~TraceLog()
        {
            this.Dispose(false);
        }

        #endregion

    }



   
}
