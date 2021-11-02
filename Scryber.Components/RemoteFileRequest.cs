using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber
{

    public delegate bool RemoteRequestCallback(IComponent raiser, RemoteFileRequest request, Stream result);

    public class RemoteFileRequest
    {

        public string FilePath { get; private set; }

        public IComponent Owner { get; private set; }

        public object Arguments { get; private set; }

        public RemoteRequestCallback Callback { get; private set; }

        public Exception Error { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsSuccessful { get; private set; }

        public RemoteFileRequest(string path, RemoteRequestCallback callback, IComponent owner = null, object args = null)
        {
            this.FilePath = path ?? throw new ArgumentNullException(nameof(path));
            this.Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.Owner = owner;
            this.Arguments = args;
            this.IsCompleted = false;
        }

        public void CompleteRequest(bool success, Exception error = null)
        {
            this.IsCompleted = true;
            this.IsSuccessful = success;
            this.Error = error;
        }
    }

    public class RemoteFileRequestList : List<RemoteFileRequest>
    {

    }


    /// <summary>
    /// Captures a set of remote file requests and supports the bulk completion
    /// </summary>
    public class RemoteFileRequestSet : IDisposable
    {
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
                return new RemoteFileRequest[] { };
        }


        public virtual bool AddRequest(RemoteFileRequest request)
        {
            this.Requests.Add(request ?? throw new ArgumentNullException(nameof(request)));
            return true;
        }


        public virtual void EnsureRequestsFullfilled()
        {
            if (this.HasRequests)
            {
                //resest, just incase a remote request fulfillment triggers more requests.
                var requests = this.CaptureRequests(clear: true);

                foreach (var req in requests)
                {
                    if (!req.IsCompleted)
                        this.FullfillRequest(req); 
                }
            }
        }


        public void FullfillRequest(RemoteFileRequest request, bool raiseErrors = true)
        {
            if (!request.IsCompleted)
            {
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
                    request.CompleteRequest(false, ex);
                }

                if (request.IsCompleted == false)
                    throw new InvalidOperationException("Could not complete the request for a remote file");

                else if (request.IsSuccessful == false && raiseErrors)
                    throw request.Error ?? new InvalidOperationException("The request for the '" + request.FilePath + "' could not be completed");
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

            using (var stream = client.GetStreamAsync(urlRequest.FilePath).Result)
            {
                var success = urlRequest.Callback(this._owner, urlRequest, stream);
                urlRequest.CompleteRequest(success);

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
            using (var stream = File.OpenRead(fileRequest.FilePath))
            {
                var success = fileRequest.Callback(this._owner, fileRequest, stream);
                fileRequest.CompleteRequest(success);

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
                    this._client.Dispose();

                this._client = null;
            }
        }

        #endregion

    }

    public class PDFRemoteFileAsyncRequestSet : RemoteFileRequestSet
    {

        public PDFRemoteFileAsyncRequestSet(Document owner)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
        }

        

        public async Task<int> EnsureRequestsFullfilledAsync()
        {
            if (this.Requests.Count > 0)
            {
                int completed = 0;
                var all = this.Requests.ToArray();
                
                foreach (var item in all)
                {
                    if (item.IsCompleted == false && await FullfillRequestAsync(item, true))
                        completed += 1;
                }

                base.EnsureRequestsFullfilled();

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
                    request.CompleteRequest(false, ex);
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
        protected async virtual Task<bool> FullfillUriRequestAsync(RemoteFileRequest urlRequest)
        {
            var client = this.GetHttpClient();

            using (var stream = await client.GetStreamAsync(urlRequest.FilePath))
            {
                var success = urlRequest.Callback(this.Owner, urlRequest, stream);
                urlRequest.CompleteRequest(success);

                return success;
            }
        }

    }
}
