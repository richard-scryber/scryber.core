using System;
using System.Collections.Generic;
using System.Net.Http;
using System.IO;
using Scryber.Components;
using Scryber.Logging;


namespace Scryber
{
    /// <summary>
    /// Captures a set of remote file requests and supports the bulk completion
    /// </summary>
    public class RemoteFileRequestSet : IDisposable
    {
        protected const string RemoteRequestCategory = "Remote Requests";
        
        private Dictionary<string, RemoteFileRequest> _keyed;
        private StringComparer _comparer;
        private HttpClient _client;
        private bool _disposeClient;
        private Document _owner;
        private DocumentExecMode _mode;
        private RemoteFileRequestList _requests;


        protected RemoteFileRequestList Requests
        {
            get
            {
                if (null == _requests)
                {
                    _requests = new RemoteFileRequestList();
                    _keyed = new Dictionary<string, RemoteFileRequest>(this.Comparer);
                }
                return _requests;
            }
        }

        public Document Owner
        {
            get { return _owner; }
        }

        public StringComparer Comparer
        {
            get { return this._comparer; }
        }

        protected Logging.TraceLog Log
        {
            get { return this.Owner.TraceLog; }
        }

        public int Count
        {
            get
            {
                if (null == _requests)
                    return 0;
                else
                    return this.Requests.Count;
            }
        }

        public DocumentExecMode ExecMode
        {
            get { return _mode; }
        }

        public bool HasRequests
        {
            get
            {
                return this.Count == 0;
            }
        }

        public RemoteFileRequestSet(Document owner): this(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Immediate, owner)
        {

        }

        public RemoteFileRequestSet(StringComparer comparer, DocumentExecMode mode, Document owner)
        {
            this._comparer = comparer;
            this._owner = owner;
            this._mode = mode;
            
        }

        /// <summary>
        /// Returns all the requests in this set, and optionally clears the collection so more can be added without issue.
        /// </summary>
        /// <returns></returns>
        public RemoteFileRequest[] CaptureRequests(bool clear = true)
        {
            if (this.HasRequests)
            {
                var all = this.Requests.ToArray();
                this.Requests.Clear();
                return all;
            }
            else
                return _empty;
        }

        private readonly RemoteFileRequest[] _empty = Array.Empty<RemoteFileRequest>();

        public virtual bool AddRequest(RemoteFileRequest request)
        {
            this.Requests.Add(request ?? throw new ArgumentNullException(nameof(request)));
            
            if(this.LogVerbose)
                this.AddVerboseLog("Adding the request for url '" + request.FilePath + "' to the current request set, to be fulfilled");

            return true;
        }
        
        public virtual void EnsureRequestsFullfilled()
        {
            if (this.HasRequests)
            {
                //resest, just incase a remote request fulfillment triggers more requests.
                var requests = this.CaptureRequests(clear: true);

                if(this.LogVerbose)
                    this.BeginVerboseLog("Starting to fulfill captured requests for  '" + requests.Length + "'remote items");
                
                int count = 0;
                
                foreach (var req in requests)
                {
                    if (!req.IsCompleted)
                    {
                        this.FullfillRequest(req, this._owner.ConformanceMode == ParserConformanceMode.Strict);
                        
                        if (req.IsCompleted)
                        {
                            if (req.IsSuccessful)
                                count++;
                            else
                                this.Log.Add(TraceLevel.Warning, RemoteRequestCategory, "The request for " + req.FilePath + " completed, but was NOT marked as successful");
                        }
                    }
                    else if(this.LogVerbose)
                        this.AddVerboseLog("The request for " + req.FilePath + " was already completed. No need to fulfill explicitly");
                        
                    
                }
                
                if(this.LogVerbose)
                    this.EndVerboseLog( "Successfully completed " + count + " remote requests out of " + requests.Length + " others were already completed or not successful");
            }
        }


        public void FullfillRequest(RemoteFileRequest request, bool raiseErrors)
        {
            if (!request.IsCompleted)
            {
                if(this.LogVerbose)
                    this.AddVerboseLog("Fulfilling the request for " + request.FilePath + " as it is not completed");
                try
                {
                    if (Uri.IsWellFormedUriString(request.FilePath, UriKind.Absolute))
                    {
                        this.FullfillUriRequest(request);
                    }
                    else
                    {
                        this.FullfillFileRequest(request);
                    }
                }
                catch(Exception ex)
                {
                    request.CompleteRequest(null, false, ex);
                }
                
                if(this.LogVerbose)
                    this.AddVerboseLog("Ended the request for " + request.FilePath + " with a completed status of " + request.IsCompleted + " and a successful status of " + request.IsSuccessful);

                if (request.IsCompleted == false)
                    throw new InvalidOperationException("Could not complete the request for a remote file");

                else if (request.IsSuccessful == false)
                {
                    if (raiseErrors)
                        throw request.Error ?? new InvalidOperationException("The request for the '" + request.FilePath + "' could not be completed");
                    else
                        this._owner.TraceLog.Add(TraceLevel.Error, RemoteRequestCategory, "Could not load the remote request for " + request.FilePath, request.Error);
                }
            }
            else if (raiseErrors && request.IsSuccessful == false && request.Error != null)
            {
                this.Log.Add(TraceLevel.Warning, RemoteRequestCategory, "Re-throwing the remote request error, as it already completed unsuccessfully");

                throw request.Error;
            }
        }

        /// <summary>
        /// Creates an http client 
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <returns></returns>
        protected virtual bool FullfillUriRequest(RemoteFileRequest urlRequest)
        {
            var client = this.GetHttpClient();

            if(this.LogDebug)
                this.AddDebugLog( "SYNC fulfilling the request for the URL '" + urlRequest.FilePath + "' as this is not an async execution");
            
            using (var stream = client.GetStreamAsync(urlRequest.FilePath).Result)
            {
                if(this.LogDebug)
                    this.AddDebugLog("Stream received from url '" + urlRequest.FilePath + "' and starting the callback");

                var success = urlRequest.Callback(this._owner, urlRequest, stream);
                
                if(this.LogDebug)
                    this.AddDebugLog("Callback done for url '" + urlRequest.FilePath + "' and reported " + (success ? "SUCCESS" : "FAIL"));

                urlRequest.CompleteRequest(urlRequest.Result, success);

                if(this.LogDebug)
                    this.AddDebugLog( "Completed the request for url '" + urlRequest.FilePath + "' with result" + (urlRequest.Result == null ? "NO RESULT SET" : urlRequest.Result.ToString()));

                return success;
            }
        }

        /// <summary>
        /// Creates file stream and callsback the owner of the request, then completes.
        /// </summary>
        /// <param name="fileRequest"></param>
        /// <returns>The result of the callback to the owner</returns>
        protected virtual bool FullfillFileRequest(RemoteFileRequest fileRequest)
        {
            if(this.LogDebug)
                this.AddDebugLog("SYNC fulfilling the request for the FILE '" + fileRequest.FilePath + "' as this is not an async execution");

            using (var stream = File.OpenRead(fileRequest.FilePath))
            {
                if(this.LogDebug)
                    this.AddDebugLog( "Stream received from file '" + fileRequest.FilePath + "' and starting the callback");

                var success = fileRequest.Callback(this._owner, fileRequest, stream);
                
                if(this.LogDebug)
                    this.AddDebugLog( "Callback done for file '" + fileRequest.FilePath + "' and reported " + (success ? "SUCCESS" : "FAIL"));

                fileRequest.CompleteRequest(fileRequest.Result, success);

                if(this.LogDebug)
                    this.AddDebugLog( "Completed the request for file '" + fileRequest.FilePath + "' with result" + (fileRequest.Result == null ? "NO RESULT SET" : fileRequest.Result.ToString()));

                return success;
            }
        }

        /// <summary>
        /// Gets an HttpClient, either from a registered service, or creating one for the lifespan of this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual HttpClient GetHttpClient()
        {
            
            if(null == this._client)
            {
                this._client = ServiceProvider.GetService<HttpClient>();
                this._disposeClient = false;

                if (null == this._client)
                {
                    if(Log.ShouldLog(TraceLevel.Debug))
                        Log.Add(TraceLevel.Debug, RemoteRequestCategory, "Creating a new HttpClient as the current service provider for HttpClient is not set, this should be disposed by this request set at the end of execution");
                    
                    this._client = new HttpClient();
                    this._disposeClient = true;
                }
            }

            return _client;
        }

        #region Dispose() + Finalize

        public void Dispose()
        {
            this.Dispose(true);
        }

        ~RemoteFileRequestSet()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {

                if (null != this._client && this._disposeClient)
                {
                    if(null != Log && Log.ShouldLog(TraceLevel.Debug))
                        Log.Add(TraceLevel.Debug, RemoteRequestCategory, "Disposing of the HttpClient as the current service provider for HttpClient was not set");

                    this._client.Dispose();
                }

                this._client = null;
            }
        }

        #endregion

        //
        // logging
        //

        protected bool LogVerbose
        {
            get { return this._owner.TraceLog.ShouldLog(TraceLevel.Verbose); }
        }

        protected bool LogDebug
        {
            get { return this._owner.TraceLog.ShouldLog(TraceLevel.Debug); }
        }

        protected void AddDebugLog(string message)
        {
            this.Log.Add(TraceLevel.Debug, RemoteRequestCategory, message);
        }
        protected void AddVerboseLog(string message)
        {
            this.Log.Add(TraceLevel.Verbose, RemoteRequestCategory, message);
        }

        protected void BeginVerboseLog(string message)
        {
            this.Log.Begin(TraceLevel.Verbose, RemoteRequestCategory, message);
        }

        protected void EndVerboseLog(string message)
        {
            this.Log.End(TraceLevel.Verbose, RemoteRequestCategory, message);
        }
    }
}