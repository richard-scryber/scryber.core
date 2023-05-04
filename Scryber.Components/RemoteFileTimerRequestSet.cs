using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Scryber.Components;


namespace Scryber
{
    public class RequestFullfilledStatus
    {
        public RemoteFileRequest[] Files { get; private set; }

        public Exception Error { get; set; }

        public bool IsComplete { get; set; }

        public RequestFullfilledStatus(RemoteFileRequest[] files)
        {
            this.Files = files;
        }
    }

    public delegate void RequestsFullfilledCallback(RequestFullfilledStatus status);

    /// <summary>
    /// Supports the use of the Remote Requests using a timer, to not block an STA application (e.g. Forms or Blazor)
    /// </summary>
    public class RemoteFileTimerRequestSet : RemoteFileRequestSet, IDisposable
    {
        public const int MAX_WAIT_DURATION = 5 * 1000;
        public const int CHECK_DURATION = 150;

        public int WaitDurationMillisecond { get; private set; }

        public int CheckFrequencyMillisecond { get; private set; }


        public object ThreadLock { get; set; }

        public RequestFullfilledStatus CurrentStatus{
            get;
            private set;
        }

        protected System.Threading.Timer CurrentTimer { get; set; }

        protected int CurrentExecTime { get; set; }

        public RequestsFullfilledCallback RequestCallback { get; set; }

        public RemoteFileTimerRequestSet(Document owner) : this(owner, MAX_WAIT_DURATION, CHECK_DURATION, new object())
        { }

        public RemoteFileTimerRequestSet(Document owner, int maxDurationMS, int checkFrequencyMS, object threadLock)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
            if(this.LogVerbose)
                this.AddVerboseLog( "The async remote requester was initiated, running in an ASYNC mode.");
            this.WaitDurationMillisecond = maxDurationMS;
            this.CheckFrequencyMillisecond = checkFrequencyMS;
            this.ThreadLock = threadLock;
        }


        private void TimerCallback(object state)
        {
            bool stillWaiting = false;
            
            if (null == this.CurrentTimer)
                return;

            try
            {
                lock (this.ThreadLock)
                {
                    foreach (var file in CurrentStatus.Files)
                    {
                        if (!file.IsCompleted)
                        {
                            Console.WriteLine("Still waiting for " + file.FilePath);
                            stillWaiting = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //catch all for a faulure here - just in case
                this.CurrentStatus.Error = ex;
                this.CurrentStatus.IsComplete = true;
                stillWaiting = false;
            }

            CurrentExecTime += this.CheckFrequencyMillisecond;

            if (stillWaiting && CurrentExecTime > this.WaitDurationMillisecond)
            {
                stillWaiting = false;
                var waitingPaths = "";

                foreach (var file in CurrentStatus.Files)
                {
                    if (file.IsCompleted == false)
                    {
                        if (waitingPaths.Length > 0) { waitingPaths += "; "; }
                        waitingPaths += file.StubFilePathForLog;
                    }
                }

                this.CurrentStatus.Error = new Exception("Timer was waiting over " + (this.WaitDurationMillisecond / 1000.0) + " seconds to complete and interuped. Waiting for the following files : " + waitingPaths);
                this.CurrentStatus.IsComplete = true;

            }

            if (!stillWaiting && null != this.CurrentTimer)
            {
                var collected = CurrentStatus;
                var callback = RequestCallback;

                //Cleanup
                CurrentTimer.Dispose();
                CurrentTimer = null;
                CurrentStatus = null;
                RequestCallback = null;

                //Execute callback
                callback(collected);
            }
            
        }

        

        public void EnsureRequestsFullfilled(RequestsFullfilledCallback callback)
        {
            if(this.Requests.Count > 0)
            {
                int completed = 0;
                var all = this.CaptureRequests();

                if (this.LogVerbose)
                    this.BeginVerboseLog(" Starting to await the completion of " + all.Length + " requests asyncronously.");

                lock (this.ThreadLock)
                {
                    if (this.RequestCallback != null)
                        throw new InvalidOperationException("This timer request set already has a callback executing. Timer request fulfillment can only be executed syncronously.");

                    this.RequestCallback = callback;

                }

                this.CurrentStatus = new RequestFullfilledStatus(all);
                int count = 0;
                foreach(var file in all)
                {
                    if (!file.IsExecuting && !file.IsCompleted)
                    {
                        this.BeginExecuting(file);
                        count++;
                    }
                }
                
                if (count > 0)
                {
                    //We have requests to make - so execute them
                    this.CurrentExecTime = 0;
                    this.CurrentTimer = new System.Threading.Timer(this.TimerCallback, null, 0, this.CheckFrequencyMillisecond);
                }
                else
                {
                    var collected = CurrentStatus;
                    callback = RequestCallback;

                    //Cleanup
                    if (null != CurrentTimer)
                    {
                        CurrentTimer.Dispose();
                        CurrentTimer = null;
                    }
                    CurrentStatus = null;
                    RequestCallback = null;

                    //Execute callback
                    callback(collected);
                }
            }
            else
            {
                this.CurrentStatus = new RequestFullfilledStatus(new RemoteFileRequest[] { });
                this.CurrentStatus.IsComplete = true;
                this.CurrentStatus.Error = null;
                
                callback(this.CurrentStatus);
                this.CurrentStatus = null;
            }
        }

        public void BeginExecuting(RemoteFileRequest request)
        {
            if (request.ResourceType == "Base64")
            {
                this.FullfillDataRequest(request);
            }
            else if (Uri.IsWellFormedUriString(request.FilePath, UriKind.Absolute))
            {
                this.BeginUriRequest(request);
            }
            else
            {
                this.FullfillFileRequest(request);
            }
        }


        protected virtual void BeginUriRequest(RemoteFileRequest urlRequest)
        {
            if (this.LogDebug)
                this.AddDebugLog("ASYNCRONOUSLY fulfilling the request for the URL '" + urlRequest.StubFilePathForLog + "' inside timer");

            var client = this.GetHttpClient();

            //message.Headers.Add("user-agent", "-/-");
            //message.Headers.Add("Access-Control-Allow-Origin", "*");
            //message.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers");
            //message.Headers.Add("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT");
            //message.Headers.Add("Access-Control-Allow-Credentials", "true");
            Task.Run(async () =>
            {
                var message = new HttpRequestMessage(HttpMethod.Get, urlRequest.FilePath);
                var response = await client.SendAsync(message);
                var success = false;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("FAILED request for " + urlRequest.FilePath);
                    urlRequest.CompleteRequest(null, false, new System.Net.Http.HttpRequestException("Error: " + response.StatusCode + "; request for '" + urlRequest.StubFilePathForLog + "' failed with message " + (response.ReasonPhrase ?? "UNKNOWN ERROR")));
                }
                else
                {
                    if (this.LogDebug)
                        this.AddDebugLog("Stream received from url '" + urlRequest.StubFilePathForLog + "' and starting the callback");

                    var stream = await response.Content.ReadAsStreamAsync();

                    success = urlRequest.Callback(this.Owner, urlRequest, stream);

                    if (this.LogDebug)
                        this.AddDebugLog("Callback done for url '" + urlRequest.StubFilePathForLog + "' and reported " + (success ? "SUCCESS" : "FAIL"));


                    lock (this.ThreadLock)
                    {
                        Console.WriteLine("Completed request for " + urlRequest.FilePath);
                        urlRequest.CompleteRequest(stream, success, null);

                        if (this.LogVerbose)
                            this.AddVerboseLog("Completed the request for url '" + urlRequest.StubFilePathForLog + "' with result of type " + (urlRequest.Result == null ? "[NO RESULT SET]" : urlRequest.Result.GetType().ToString()));
                    }
                }
            });
        }


        protected override void Dispose(bool disposing)
        {

            if (disposing && null != this.CurrentTimer)
            {
                this.CurrentTimer.Dispose();
                this.CurrentTimer = null;
            }

            base.Dispose(disposing);
        }
    }
}