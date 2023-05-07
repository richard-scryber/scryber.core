using System;
using System.Threading;
using Scryber.Logging;

namespace Scryber
{

    public delegate void RequestsFullfilledCallback(RequestFullfilledStatus status);


    /// <summary>
    /// Checks and completes remote file requests using a timer,
    /// executing the callback once complete
    /// </summary>
    public class RequestFulfillmentTimer : IDisposable
	{

		/// <summary>
		/// Gets the callback that will be raised once all the requested files have been completed (or an error occurred)
		/// </summary>
		public RequestsFullfilledCallback Callback{ get; private set; }

		/// <summary>
		/// Gets the files that have been requested
		/// </summary>
		protected RemoteFileRequest[] Files { get; private set; }

		/// <summary>
		/// Returns true if the timer is still executing
		/// </summary>
		public bool Executing { get; private set; }

		/// <summary>
		/// The currently executing timer (or null if this instance is no longer executing).
		/// </summary>
		protected Timer Timer { get; private set; }

		protected TraceLog TraceLog { get; private set; }

		/// <summary>
		/// The scyncronous thread lock for the timer checks.
		/// </summary>
		protected object ThreadLock { get; set; }

        /// <summary>
        /// The frequency at which the timer should check the files
        /// </summary>
		protected int CheckFrequency { get; set; }

        /// <summary>
        /// The number of times the timer should check, before it times out and fails
        /// </summary>
		protected int FailAtCount { get; set; }

        /// <summary>
        /// The current number of times the timer has checked.
        /// </summary>
		protected int CurrentCount { get; set; }

        /// <summary>
        /// Creates a new fullfullment timer, that will check the requests at the specified interval, and callback once complete.
        /// Allowing ALL other threads to continue on without inerruption or blocking.
        /// </summary>
        /// <param name="traceLog">The trace log to record messages to</param>
        /// <param name="files">The group of files to check</param>
        /// <param name="callback">A callback to execute once all the files have been processed.</param>
        /// <param name="checkFrequency">The frequency in milliseconds, at which checks will be made.</param>
        /// <param name="failTimeout">The total time </param>
        /// <exception cref="ArgumentNullException"></exception>
        public RequestFulfillmentTimer(Scryber.Logging.TraceLog traceLog,  RemoteFileRequest[] files, RequestsFullfilledCallback callback, int checkFrequency, int failTimeout)
        {
            this.Files = files ?? throw new ArgumentNullException(nameof(files));
            this.Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.TraceLog = traceLog ?? new DoNothingTraceLog(TraceRecordLevel.Off);
            this.ThreadLock = new object();
            this.CheckFrequency = checkFrequency;
            this.FailAtCount = (int)Math.Ceiling((double)failTimeout/(double)checkFrequency);
            this.CurrentCount = 0;

            CheckFirstTime();
        }

        /// <summary>
        /// Runs an initial check to see if there are files that are waiting to execute.
        /// If not then just calls back, otherwise starts the timer
        /// </summary>
        private void CheckFirstTime()
        {
            if (this.Files.Length > 0 && DoCheckFilesFinished(this.Files) == false)
            {
                if (this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose, "Async Timer", "Starting the async processing of " + this.Files.Length + " files with a timer frequency of " + this.CheckFrequency);

                this.Executing = true;
                this.Timer = new Timer(this.TimerCheck, this.Files, 0, this.CheckFrequency);
            }
            else
            {
                if (this.TraceLog.ShouldLog(TraceLevel.Verbose))
                    this.TraceLog.Add(TraceLevel.Verbose, "Async Timer", "No files to process with the timer, or all files are finished, so calling back immediately.");

                this.Executing = false;
                this.Timer = null;

                this.DoFullfilledCallback(this.Files);
            }
        }

        /// <summary>
        /// Actual tick of the timer, to start the check
        /// </summary>
        /// <param name="state"></param>
        private void TimerCheck(object state)
        {
            var files = state as RemoteFileRequest[];

			if (null == files)
				files = new RemoteFileRequest[] { };

            SafeCheckFilesAndCallback(files);
        }


        /// <summary>
        /// Threadsafe check of the file status, and doing either nothing and waiting more, or executing the fulfilled.
        /// Any execption will be captured and reported as best possible.
        /// </summary>
        /// <param name="files"></param>
        private void SafeCheckFilesAndCallback(RemoteFileRequest[] files)
        {
            lock (this.ThreadLock)
            {
                if (Executing)
                {
                    try
                    {
                        bool finished = this.DoCheckFilesFinished(files);
                        this.CurrentCount++;

                        if (finished)
                        {
                            this.Executing = false;
                            this.Timer.Dispose();
                            this.Timer = null;

                            this.DoFullfilledCallback(files);
                        }
                        else if (this.CurrentCount >= FailAtCount)
                        {
                            throw new TimeoutException("Not all files could be loaded within the required time (" + this.FailAtCount + "ms). Falling out un-graciously");
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Executing = false;

                        if (null != this.Timer)
                        {
                            this.Timer.Dispose();
                            this.Timer = null;
                        }
                        DoErrorCallback(ex, files);

                    }
                }

            }
        }

        /// <summary>
        /// Checks each of the files in turn and returns false if one or more are still waiting.
        /// </summary>
        /// <param name="files">The array of files to check</param>
        /// <returns>True if all files are finished. Otherwise false</returns>
        protected virtual bool DoCheckFilesFinished(RemoteFileRequest[] files)
		{
			bool complete = true;

			foreach(var file in files)
			{
				if (file.IsCompleted == false)
					complete = false;
			}
			return complete;
		}

        /// <summary>
        /// Executes the callback successfully.
        /// </summary>
        /// <param name="files"></param>
		protected virtual void DoFullfilledCallback(RemoteFileRequest[] files)
		{
			RequestFullfilledStatus status = new RequestFullfilledStatus(files);
			status.IsComplete = true;
			status.Error = null;

			try
			{
				this.Callback(status);
			}
            catch(Exception ex)
            {
                //Cannot do anything with this except log it.
                this.TraceLog.Add(TraceLevel.Error, "Async Timer", "Execution of the remote requests completed successfully, but the callback raised an exception, that has nowhere to go : " + ex.Message, ex);
            }
		}

        /// <summary>
        /// Executes the call with an error.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="files"></param>
		protected virtual void DoErrorCallback(Exception error, RemoteFileRequest[] files)
		{
            RequestFullfilledStatus status = new RequestFullfilledStatus(files);
            status.IsComplete = true;
            status.Error = error ?? new Exception("An unknown error occurred");

            try
            {
                this.Callback(status);
            }
            catch (Exception ex)
            {
                //Cannot do anything with this except log it.
                this.TraceLog.Add(TraceLevel.Error, "Async Timer", "Execution of the remote requests failed, but the callback on the error, raised an exception, that has nowhere to go : " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Disposes of this instance and the associated timer.
        /// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}


		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
                lock (this.ThreadLock)
                {
                    if (this.Timer != null)
                    {
                        this.Timer.Dispose();
                        this.Timer = null;
                        this.Executing = false;
                    }
                }
			}
		}


		~RequestFulfillmentTimer()
		{
			this.Dispose(false);
		}
	}
}

