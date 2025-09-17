using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Scryber.Components;


namespace Scryber
{
    /// <summary>
    /// Supports the use of the Remote Requests using async methods
    /// </summary>
    public class RemoteFileAsyncRequestSet : RemoteFileRequestSet
    {
        public const int MAX_WAIT_DURATION = 10 * 1000;

        public int WaitDurationMillisecond { get; private set; }

        public RemoteFileAsyncRequestSet(Document owner) : this(owner, MAX_WAIT_DURATION)
        { }

        public RemoteFileAsyncRequestSet(Document owner, int maxDurationMS)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
            if(this.LogVerbose)
                this.AddVerboseLog( "The async remote requester was initiated, running in an ASYNC mode.");
            this.WaitDurationMillisecond = maxDurationMS;
        }

        

        public async Task<int> EnsureRequestsFullfilledAsync()
        {
            if (this.Requests.Count > 0)
            {
                int completed = 0;
                var all = this.Requests.ToArray();
                
                if(this.LogVerbose)
                    this.BeginVerboseLog( " Starting to await the completion of " + all.Length + " requests asyncronously.");


                foreach (var item in all)
                {
                    if(item.IsExecuting)
                    {
                        var success = await this.AwaitPendingRequestAsync(item, this.Owner.ConformanceMode == ParserConformanceMode.Strict);

                        if (success)
                            completed += 1;

                    }
                    else if (item.IsCompleted == false)
                    {

                        var success = await FullfillRequestAsync(item, this.Owner.ConformanceMode == ParserConformanceMode.Strict);
                        if (success)
                        {
                            if(this.LogVerbose)
                                this.AddVerboseLog( " Successfully completed request for " + item.StubFilePathForLog + " asyncronously.");
                            
                            completed += 1;
                        }
                        else if(this.LogVerbose)
                            this.AddVerboseLog("The request for " + item.StubFilePathForLog + " was completed asyncronously, but not successful");
                    }
                    else if(this.LogVerbose)
                        this.AddVerboseLog("The request for " + item.StubFilePathForLog + " was already marked as completed");
                }

                //base.EnsureRequestsFullfilled();

                if(this.LogVerbose)
                    this.EndVerboseLog(" Completed " + completed + " remote requests out of " + all.Length + " asyncronously.");

                return completed;
            }
            else
                return 0;
        }

        public async Task<bool> AwaitPendingRequestAsync(RemoteFileRequest request, bool raiseErrors)
        {
            if (this.LogVerbose)
                this.AddVerboseLog("Beginning wait for remote request completion to path " + request.StubFilePathForLog);

            int count = 0;
            int step = 250;

            while(count < this.WaitDurationMillisecond)
            {
                await Task.Delay(step);

                if (request.IsCompleted)
                    break;

                count += step;
            }

            if (!request.IsCompleted)
            {

                request.CompleteRequest(null, false, new Exception("Request timed out"));


                if (raiseErrors)
                    throw new InvalidOperationException("The remote request for " + request.StubFilePathForLog + " timed out. Extend the maximum duration of " + (this.WaitDurationMillisecond / 1000.0) + "s for the remote request");
                else
                    this.Log.Add(TraceLevel.Error, RemoteRequestCategory, "Could not complete the remote request for " + request.StubFilePathForLog + " within the available time of " + (this.WaitDurationMillisecond / 1000.0) + "s. Extend the maximum duration for the remote request to appease this error");
                return false;
            }
            else
            {
                if (LogVerbose)
                    this.AddVerboseLog("Completed the request successfully for " + request.StubFilePathForLog + " after " + (count / 1000.0) + "s");

                return true;
            }
        }

        public async Task<bool> FullfillRequestAsync(RemoteFileRequest request, bool raiseErrors)
        {
            
            if (!request.IsCompleted)
            {
                try
                {
                    if(request.ResourceType == "Base64")
                    {
                        this.FullfillDataRequest(request);
                    }
                    else if (Uri.IsWellFormedUriString(request.FilePath, UriKind.Absolute))
                    {
                        await this.FullfillUriRequestAsync(request);
                    }
                    else
                    {
                        this.FullfillFileRequest(request);
                    }
                }
                catch (Exception ex)
                {
                    request.CompleteRequest(null,false, ex);
                }

                if (request.IsCompleted == false && request.IsExecuting == false)
                {
                    if (raiseErrors)
                        throw new InvalidOperationException("Could not complete the request for a remote file", request.Error);
                    else 
                        this.Log.Add(TraceLevel.Error, RemoteRequestCategory, "Could not complete the remote request for " + (request.StubFilePathForLog ?? "Unknown File"));
                }

                else if (request.IsSuccessful == false)
                {
                    if (raiseErrors)
                        throw new InvalidOperationException("The request for the '" + (request.StubFilePathForLog ?? "Unknown File") + "' could not be completed", request.Error);
                    else
                    {
                        this.Log.Add(TraceLevel.Error, RemoteRequestCategory,
                            "Remote request for " + (request.StubFilePathForLog ?? "Unknown File") +
                            " failed with message '" + (request.Error.Message ?? "") + "'");
                        
                        if (null != request.Error && null != request.Error.InnerException)
                        {
                            this.Log.Add(TraceLevel.Error, RemoteRequestCategory,
                                "Inner error for " + (request.StubFilePathForLog ?? "Unknown File") +
                                " message '" + (request.Error.InnerException.Message ?? "") + "'" + "\r\n" + request.Error.InnerException.StackTrace);
                        }
                    }
                }
            }
            return request.IsCompleted;
        }

        /// <summary>
        /// Creates an http client 
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <returns></returns>
        protected virtual async Task<bool> FullfillUriRequestAsync(RemoteFileRequest urlRequest)
        {
            if(this.LogDebug)
                this.AddDebugLog( "ASYNCRONOUSLY fulfilling the request for the URL '" + urlRequest.StubFilePathForLog + "'");

            urlRequest.StartRequest();
            
            var client = this.GetHttpClient();
            var message = new HttpRequestMessage(HttpMethod.Get, urlRequest.FilePath);
            

            using (var response = await client.SendAsync(message))
            {
                if(this.LogDebug)
                    this.AddDebugLog("Stream received from url '" + urlRequest.StubFilePathForLog + "' and starting the callback");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException("Response for " + urlRequest.StubFilePathForLog + " returned a status code of " + ((int)response.StatusCode).ToString() + " " + (response.ReasonPhrase ?? "Unknown Error"));

                var stream = await response.Content.ReadAsStreamAsync();

                var success = urlRequest.Callback(this.Owner, urlRequest, stream);
                
                if(this.LogDebug)
                    this.AddDebugLog("Callback done for url '" + urlRequest.StubFilePathForLog + "' and reported " + (success ? "SUCCESS" : "FAIL"));

                //urlRequest.CompleteRequest(urlRequest, success);
                
                if(this.LogDebug)
                    this.AddDebugLog( "Completed the request for url '" + urlRequest.StubFilePathForLog + "' with result of type " + (urlRequest.Result == null ? "[NO RESULT SET]" : urlRequest.Result.GetType().ToString()));

                return success;
            }
        }

    }
}