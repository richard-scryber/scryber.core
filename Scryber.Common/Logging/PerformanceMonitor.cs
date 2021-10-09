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
    /// Encapsulates the performance monitoring of the srcyber library. 
    /// Use the begin and end methods to start and stop methods to measure timings against a particular monitor type.
    /// Use the Record method to measure individual action timings (based on a key) for a particular 
    /// monitor type - also increments the tpye entry.
    /// </summary>
    public class PerformanceMonitor : IEnumerable<PerformanceMonitorEntry>
    {

        #region ivars

        PerformanceMonitorEntry[] _entries;
        bool _recordMeasurements;

        #endregion

        //
        // properties
        //

        #region public PDFPerformanceMonitorEntry this[PerformanceMonitorType type]

        /// <summary>
        /// Gets access to each monitor entry based on the performance monitor type
        /// </summary>
        /// <param name="type">The type of the monitor</param>
        /// <returns>An entry that holds the timing details</returns>
        public PerformanceMonitorEntry this[PerformanceMonitorType type]
        {
            get { return _entries[(int)type]; }
        }

        #endregion

        #region public int Count
        /// <summary>
        /// Gets the number of monitor entries in this instance
        /// </summary>
        public int Count
        {
            get { return _entries.Length; }
        }

        #endregion

        #region public bool RecordMeasurements {get; set;}

        /// <summary>
        /// Gets of sets the flag to indicate if this monitor should record measurements
        /// </summary>
        public bool RecordMeasurements
        {
            get { return _recordMeasurements; }
            set 
            {
                if (value != _recordMeasurements)
                {
                    _recordMeasurements = value;
                    for (int i = 0; i < _entries.Length; i++)
                    {
                        PerformanceMonitorEntry entry = _entries[i];
                        entry.RecordMeasurements = value;
                    }
                }
            }
        }

        #endregion

        //
        // ctor
        //

        

        #region public PDFPerformanceMonitor()

        /// <summary>
        /// Creates a new instance of the PDFPerformanceMonitor
        /// </summary>
        public PerformanceMonitor(bool recordMeasurements)
        {
            this._recordMeasurements = recordMeasurements;
            InitEntries(recordMeasurements);
        }

        #endregion

        #region protected virtual void InitEntries(bool recordMeasurements)


        /// <summary>
        /// Sets up the array of entries - called from the constructor
        /// </summary>
        protected virtual void InitEntries(bool recordMeasurements)
        {
            int len = (int)PerformanceMonitorType.Count;
            _entries = new PerformanceMonitorEntry[len];
            for (int i = 0; i < len; i++)
            {
                PerformanceMonitorType type = (PerformanceMonitorType)i;
                _entries[i] = new PerformanceMonitorEntry(type, recordMeasurements);
            }
        }
        
        #endregion

        //
        // public methods
        //

        #region public void Begin(PerformanceMonitorType type)

        /// <summary>
        /// Starts the recording of the performance monitor entry associated with the provided type
        /// </summary>
        /// <param name="type"></param>
        public void Begin(PerformanceMonitorType type)
        {
            this[type].Begin();
        }

        #endregion

        #region public void End(PerformanceMonitorType type)

        /// <summary>
        /// Stops the recording of the performance monitor entry associated with the provided type
        /// </summary>
        /// <param name="type"></param>
        public void End(PerformanceMonitorType type)
        {
            this[type].End();
        }

        #endregion

        #region public IDisposable Record(PerformanceMonitorType type, string key)

        /// <summary>
        /// Starts the recording of the performance monitor entry associated with the specified type, and encapsualtes the
        /// duration associated with the profided key within the entry.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns>An IDisposable instance that when disposed will stop the recording against the entry and key</returns>
        public IDisposable Record(PerformanceMonitorType type, string key)
        {
            return this[type].Record(key);
        }

        #endregion

        #region public IEnumerator<PDFPerformanceMonitorEntry> GetEnumerator()

        /// <summary>
        /// IEnumerable implementation that will return 
        /// an enumerator to loop over all the performance monitor entries
        /// </summary>
        /// <returns></returns>
        public IEnumerator<PerformanceMonitorEntry> GetEnumerator()
        {
            return _entries.AsEnumerable<PerformanceMonitorEntry>().GetEnumerator();
        }

        /// <summary>
        /// Non-Generic explicit infterface implementation
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region public void OutputToTraceLog(PDFTraceLog log)

        /// <summary>
        /// Adds all the performance monitor entries onto the specified trace log
        /// </summary>
        /// <param name="log"></param>
        public void OutputToTraceLog(TraceLog log)
        {
            if (null == log)
                return;

            foreach (PerformanceMonitorEntry entry in this)
            {
                log.Add(TraceLevel.Message, "Performance Timings", "Total for " + entry.MonitorKey + ": " + entry.MonitorElapsed + " for " + entry.MonitorCount + " calls");
                if (entry.HasMeasurements && log.ShouldLog(TraceLevel.Verbose))
                {
                    foreach (PerformanceMonitorMeasurement measure in entry.GetMeasurements())
                    {
                        log.Add(TraceLevel.Verbose, "Perfromance Timings", "Measured " + entry.MonitorKey + ": " + measure.Key + " took " + measure.Elapsed);
                    }
                }
            }

        }

        #endregion
    }


    /// <summary>
    /// Encapsulates the performance monitoring for a single type so that the library can validate the 
    /// execution speed of key aspects of the library.
    /// </summary>
    public class PerformanceMonitorEntry
    {
        #region ivars

        private PerformanceMonitorType _type;
        private System.Diagnostics.Stopwatch _timer;
        private int _indexer;
        private List<PerformanceMonitorMeasurement> _measurements;
        private bool _recordMeasurements;

        #endregion

        //
        // properties
        //

        #region public string MonitorKey {get;}

        /// <summary>
        /// Returns the name of the monitor in a normalized format
        /// </summary>
        public string MonitorKey
        {
            get { return _type.ToString().Replace('_', ' '); }
        }

        #endregion

        #region public PerformanceMonitorType MonitorType {get;}

        /// <summary>
        /// Gets the type of this monitor (that relates directly to the index in the PDFPerformanceMonitor array.
        /// </summary>
        public PerformanceMonitorType MonitorType
        {
            get { return _type; }
        }

        #endregion

        #region public TimeSpan MonitorElapsed {get;}

        /// <summary>
        /// Gets the current total elapsed time of this monitor
        /// </summary>
        public TimeSpan MonitorElapsed
        {
            get { return _timer.Elapsed; }
        }

        #endregion

        #region public int MonitorCount {get;}

        /// <summary>
        /// Gets the current total number of times this monitor was Begun and Ended.
        /// </summary>
        public int MonitorCount
        {
            get { return _indexer; }
        }

        #endregion

        #region public bool RecordMeasurements {get;set;}

        /// <summary>
        /// Gets or sets the flag to indicate if this entry 
        /// should record individual measurements
        /// </summary>
        public bool RecordMeasurements
        {
            get { return _recordMeasurements; }
            set { _recordMeasurements = value; }
        }

        #endregion

        #region public bool HasMeasurements {get;}

        /// <summary>
        /// Returns true if this monitor should / does record individual measurements.
        /// </summary>
        public bool HasMeasurements
        {
            get { return _measurements != null && _measurements.Count > 0; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFPerformanceMonitorEntry(PerformanceMonitorType type)

        /// <summary>
        /// Creates a new performance monitor entry for recording timings against a particular monitor type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="recordItems"></param>
        public PerformanceMonitorEntry(PerformanceMonitorType type, bool recordMeasurements)
        {
            this._type = type;
            this._timer = new System.Diagnostics.Stopwatch();
            this._indexer = 0;
            this._recordMeasurements = recordMeasurements;
        }

        #endregion

        //
        // public methods
        //

        #region public PDFPerformanceMonitorMeasurement[] GetMeasurements()

        /// <summary>
        /// Gets all the string timespan pairs in this monitor. 
        /// Throws InvalidOperationException if this entry does not have any measurements (use HasMeasurements to check)
        /// </summary>
        /// <returns></returns>
        public PerformanceMonitorMeasurement[] GetMeasurements()
        {
            if (null == _measurements || _measurements.Count == 0)
                throw new InvalidOperationException("This performance monitor is not set to record individual items, or has no recorded events - use the HasMeasurements property to check");

            return _measurements.ToArray();
        }


        #endregion

        #region public void Begin()

        /// <summary>
        /// Invoke when measurement should start recording
        /// </summary>
        public void Begin()
        {
            this._timer.Start();
        }

        #endregion

        #region public void End()

        /// <summary>
        /// Invoke when measurement should end recording
        /// </summary>
        public void End()
        {
            this._timer.Stop();
            this._indexer++;
        }

        #endregion

        #region public IDisposable Record(string key)

        /// <summary>
        /// A single encapsulated action that will be recorded in this entry based on the key provided.
        /// NOTE - calls to End and Begin on this entry will affect the results of any Recording until the Recording is stoped (Disposing the instance)
        /// </summary>
        /// <param name="key">The identifier for this measurement. It does not need to be unique, but matching keys will not be aggregated</param>
        /// <returns></returns>
        public IDisposable Record(string key)
        {
            PerformanceMonitorMeasurement indiv = new PerformanceMonitorMeasurement(this, key);

            this.Begin();
            return indiv;

        }

        #endregion

        #region public void Reset()

        /// <summary>
        /// Resets this entries counters to zero and clears any existing moniter measurements.
        /// </summary>
        /// <remarks>If there are undisposed recording measurements going on, then they will not be removed with the reset, 
        /// but if later disposed will measure their duration against the newly reset time</remarks>
        public void Reset()
        {
            this._timer.Reset();
            this._indexer = 0;

            if (null != _measurements)
                _measurements.Clear();
        }

        #endregion

        //
        // implementation methods
        //

       

        #region internal int RegisterMeasurement(PDFPerformanceMonitorMeasurement item)

        /// <summary>
        /// Normally called by the measurement itself to ensure it is in this entries list of
        /// measurements, and returns the index of the measurement after it is inserted in the list
        /// </summary>
        /// <param name="item">The measurement item to register</param>
        /// <returns>The index of the item in the list</returns>
        internal int RegisterMeasurement(PerformanceMonitorMeasurement item)
        {
            int count = -1;
            if (this._recordMeasurements)
            {
                if (null == _measurements)
                {
                    _measurements = new List<PerformanceMonitorMeasurement>();
                }
                count = _measurements.Count;
                _measurements.Add(item);
            }
            return count;
        }

        #endregion

        

        
    }

   



    /// <summary>
    /// Encapsulates a single specific measurement associated with a key against a performance moniter entry. 
    /// </summary>
    public class PerformanceMonitorMeasurement : IDisposable
    {
        #region iVars

        private PerformanceMonitorEntry _owner;
        private TimeSpan _start, _end;
        private string _key;
        private int _index;

        #endregion

        //
        // properties
        //

        #region public string Key {get;}

        /// <summary>
        /// Gets the key identiier for the measurement
        /// </summary>
        public string Key
        {
            get { return _key; }
        }

        #endregion

        #region public TimeSpan Elapsed {get;}

        /// <summary>
        /// Gets the final (or current if still measuring) elapsed time for this measurement
        /// </summary>
        public TimeSpan Elapsed
        {
            get { return _end == TimeSpan.Zero ? _owner.MonitorElapsed - _start : _end - _start; }
        }

        #endregion

        #region public TimeSpan StartedTime

        /// <summary>
        /// Gets the Timespan when this measurement started.
        /// </summary>
        public TimeSpan StartedTime
        {
            get { return _start; }
        }

        #endregion

        #region public TimeSpan EndedTime {get;}

        /// <summary>
        /// Get the time when this measurement was stopped.
        /// </summary>
        public TimeSpan EndedTime
        {
            get { return _end; }
        }

        #endregion

        //
        // ctor
        //

        #region public PDFPerformanceMonitorMeasurement(PDFPerformanceMonitorEntry owner, string key)

        /// <summary>
        /// Creates a new instance of a measurement with the owner (cannot be Null) and key
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="key"></param>
        /// <remarks>Easiest way to create and start a meausurement is to use the entries.Record(key) method in a using block</remarks>
        public PerformanceMonitorMeasurement(PerformanceMonitorEntry owner, string key)
        {
            this._owner = owner;
            this._start = owner.MonitorElapsed;
            this._end = TimeSpan.Zero;
            if (null == key)
                key = string.Empty;
            this._key = key;
        }

        #endregion

        //
        // methods
        //

        #region public void BeginRecording()

        /// <summary>
        /// Manually starts the recording of this measurement (previous measurements in this isntance will be lost).
        /// </summary>
        public void BeginRecording()
        {
            this._start = _owner.MonitorElapsed;
            this._end = TimeSpan.Zero;
            this._owner.Begin();
        }

        #endregion

        #region public void EndRecording()

        /// <summary>
        /// Manually ends the recording of this measurement
        /// </summary>
        public void EndRecording()
        {
            if(_end == TimeSpan.Zero)
            {
                this._owner.End();
                this._end = this._owner.MonitorElapsed;
                this._index = this._owner.RegisterMeasurement(this);
            }
        }

        #endregion

        #region public void Dispose()

        /// <summary>
        /// Disposes of this measurement - ending the measurement if any. Data is NOT lost.
        /// </summary>
        public void Dispose()
        {
            this.EndRecording();
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Returns a string representation of this measurement
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._owner.MonitorKey + " measurement for key '" + this.Key + "' was " + this.Elapsed;
        }

        #endregion
    }

    /// <summary>
    /// Empty non measureing instance that can be used if not recording
    /// </summary>
    internal class NonRecordingMeasurement : IDisposable
    {
        internal static NonRecordingMeasurement Instance = new NonRecordingMeasurement();

        public void Dispose() { }
    }


}