using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Scryber.Components;


namespace Scryber
{
    

    

    /// <summary>
    /// Supports the use of the Remote Requests using a timer, to not block an STA application (e.g. Forms or Blazor)
    /// </summary>
    public class RemoteFileTimerRequestSet : RemoteFileRequestSet, IDisposable
    {
        public const int MAX_WAIT_DURATION = 5 * 1000;
        public const int CHECK_DURATION = 100;

        public int WaitDurationMillisecond { get; private set; }

        public int CheckFrequencyMillisecond { get; private set; }

        protected RequestFulfillmentTimer RequestTimer { get; set; }


        public RemoteFileTimerRequestSet(Document owner) : this(owner, MAX_WAIT_DURATION, CHECK_DURATION, new object())
        { }

        public RemoteFileTimerRequestSet(Document owner, int maxDurationMS, int checkFrequencyMS, object threadLock)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
            if(this.LogVerbose)
                this.AddVerboseLog( "The async remote requester was initiated, running in an ASYNC mode.");
            this.WaitDurationMillisecond = maxDurationMS;
            this.CheckFrequencyMillisecond = checkFrequencyMS;
            
        }


        

        public void EnsureRequestsFullfilled(RequestsFullfilledCallback callback)
        {
            int count = 0;

            if (this.Requests.Count > 0)
            {

                var all = this.CaptureRequests();

                if (this.LogVerbose)
                    this.BeginVerboseLog(" Starting to await the completion of " + all.Length + " requests asyncronously.");



                foreach (var file in all)
                {
                    if (file.IsCompleted)
                    {
                        //do nothing
                    }
                    else if (file.IsExecuting == false)
                    {
                        this.BeginExecuting(file);
                        count++;
                    }
                    else if (file.IsCompleted == false)
                    {
                        count++;
                    }
                }

                this.RequestTimer = new RequestFulfillmentTimer(this.Log, all, callback, this.CheckFrequencyMillisecond, this.WaitDurationMillisecond);
            }
            else
            {
                //we have no files so just callback
                RequestFullfilledStatus status = new RequestFullfilledStatus(new RemoteFileRequest[] { });
                status.Error = null;
                status.IsComplete = true;

                callback(status);
            }
        }

        /// <summary>
        /// Begins the execution of a single request.
        /// </summary>
        /// <param name="request"></param>
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

            
            Task.Run(async () =>
            {
                var message = new HttpRequestMessage(HttpMethod.Get, urlRequest.FilePath);
                var productValue = new ProductInfoHeaderValue("Paperwork", "1.0");
                var commentValue = new ProductInfoHeaderValue("(+https://www.paperworkday.com/)");

                message.Headers.UserAgent.Clear();
                message.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var response = await client.SendAsync(message);
                var success = false;
                if (!response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("FAILED request for " + urlRequest.FilePath);
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


                    urlRequest.CompleteRequest(stream, success, null);

                    if (this.LogVerbose)
                        this.AddVerboseLog("Completed the request for url '" + urlRequest.StubFilePathForLog + "' with result of type " + (urlRequest.Result == null ? "[NO RESULT SET]" : urlRequest.Result.GetType().ToString()));
                    
                }
            });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(null != this.RequestTimer)
                {
                    this.RequestTimer.Dispose();
                    this.RequestTimer = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}