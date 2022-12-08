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

        public RemoteFileAsyncRequestSet(Document owner)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
            if(this.LogVerbose)
                this.AddVerboseLog( "The async remote requester was initiated, running in an ASYNC mode.");
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
                    if (item.IsCompleted == false)
                    {

                        var success = await FullfillRequestAsync(item, this.Owner.ConformanceMode == ParserConformanceMode.Strict);
                        if (success)
                        {
                            if(this.LogVerbose)
                                this.AddVerboseLog( " Successfully completed request for " + item.FilePath + " asyncronously.");
                            
                            completed += 1;
                        }
                        else if(this.LogVerbose)
                            this.AddVerboseLog("The request for " + item.FilePath + " was completed asyncronously, but not successful");
                    }
                    else if(this.LogVerbose)
                        this.AddVerboseLog("The request for " + item.FilePath + " was already marked as completed");
                }

                base.EnsureRequestsFullfilled();

                if(this.LogVerbose)
                    this.EndVerboseLog(" Completed " + completed + " remote requests out of " + all.Length + " asyncronously.");

                return completed;
            }
            else
                return 0;
        }


        public async Task<bool> FullfillRequestAsync(RemoteFileRequest request, bool raiseErrors)
        {
            
            if (!request.IsCompleted)
            {
                try
                {
                    if (Uri.IsWellFormedUriString(request.FilePath, UriKind.Absolute))
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

                if (request.IsCompleted == false)
                {
                    if (raiseErrors)
                        throw new InvalidOperationException("Could not complete the request for a remote file", request.Error);
                    else
                        this.Log.Add(TraceLevel.Error, RemoteRequestCategory, "Could not complete the remote request for " + (request.FilePath ?? "Unknown File"));
                }

                else if (request.IsSuccessful == false)
                {
                    if (raiseErrors)
                        throw new InvalidOperationException("The request for the '" + (request.FilePath ?? "Unknown File") + "' could not be completed", request.Error);
                    else
                        this.Log.Add(TraceLevel.Error, RemoteRequestCategory, "Remote request for " + (request.FilePath ?? "Unknown File") + " failed with message '" + (request.Error.Message ?? "") + "'");
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
                this.AddDebugLog( "ASYNCRONOUSLY fulfilling the request for the URL '" + urlRequest.FilePath + "'");

            var client = this.GetHttpClient();
            var message = new HttpRequestMessage(HttpMethod.Get, urlRequest.FilePath);
            //message.Headers.Add("user-agent", "-/-");
            //message.Headers.Add("Access-Control-Allow-Origin", "*");
            //message.Headers.Add("Access-Control-Allow-Headers", "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers");
            //message.Headers.Add("Access-Control-Allow-Methods", "GET,HEAD,OPTIONS,POST,PUT");
            //message.Headers.Add("Access-Control-Allow-Credentials", "true");

            using (var response = await client.SendAsync(message))
            {
                if(this.LogDebug)
                    this.AddDebugLog("Stream received from url '" + urlRequest.FilePath + "' and starting the callback");

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException("Response for " + urlRequest.FilePath + " returned a status code of " + ((int)response.StatusCode).ToString() + " " + (response.ReasonPhrase ?? "Unknown Error"));

                var stream = await response.Content.ReadAsStreamAsync();

                var success = urlRequest.Callback(this.Owner, urlRequest, stream);
                
                if(this.LogDebug)
                    this.AddDebugLog("Callback done for url '" + urlRequest.FilePath + "' and reported " + (success ? "SUCCESS" : "FAIL"));

                //urlRequest.CompleteRequest(urlRequest, success);
                
                if(this.LogDebug)
                    this.AddDebugLog( "Completed the request for url '" + urlRequest.FilePath + "' with result of type " + (urlRequest.Result == null ? "[NO RESULT SET]" : urlRequest.Result.GetType().ToString()));

                return success;
            }
        }

    }
}