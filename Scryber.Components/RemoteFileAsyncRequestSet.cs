using System;
using System.IO;
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

                        var success = await FullfillRequestAsync(item, true);
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


        public async Task<bool> FullfillRequestAsync(RemoteFileRequest request, bool raiseErrors = true)
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
                    throw new InvalidOperationException("Could not complete the request for a remote file");

                else if (request.IsSuccessful == false && raiseErrors)
                    throw request.Error ?? new InvalidOperationException("The request for the '" + request.FilePath + "' could not be completed");
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

            using (var stream = await client.GetStreamAsync(urlRequest.FilePath))
            {
                if(this.LogDebug)
                    this.AddDebugLog("Stream received from url '" + urlRequest.FilePath + "' and starting the callback");

                var success = urlRequest.Callback(this.Owner, urlRequest, stream);
                
                if(this.LogDebug)
                    this.AddDebugLog("Callback done for url '" + urlRequest.FilePath + "' and reported " + (success ? "SUCCESS" : "FAIL"));

                urlRequest.CompleteRequest(urlRequest, success);
                
                if(this.LogDebug)
                    this.AddDebugLog( "Completed the request for url '" + urlRequest.FilePath + "' with result of type " + (urlRequest.Result == null ? "[NO RESULT SET]" : urlRequest.Result.GetType().ToString()));

                return success;
            }
        }

    }
}