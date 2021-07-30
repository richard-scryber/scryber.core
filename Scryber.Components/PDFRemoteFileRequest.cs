using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber
{

    public delegate bool RemoteRequestCallback(IPDFComponent raiser, PDFRemoteFileRequest request, Stream result);

    public class PDFRemoteFileRequest
    {

        public string FilePath { get; private set; }

        public IPDFComponent Owner { get; private set; }

        public object Arguments { get; private set; }

        public RemoteRequestCallback Callback { get; private set; }

        public Exception Error { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsSuccessful { get; private set; }

        public PDFRemoteFileRequest(string path, RemoteRequestCallback callback, IPDFComponent owner = null, object args = null)
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

    public class PDFRemoteFileRequestList : List<PDFRemoteFileRequest>
    {

    }


    /// <summary>
    /// Captures a set of remote file requests and supports the bulk completion
    /// </summary>
    public class PDFRemoteFileRequestSet : IDisposable
    {
        private Dictionary<string, PDFRemoteFileRequest> _keyed;
        private StringComparer _comparer;
        private HttpClient _client;
        private bool _disposeClient;
        private Document _owner;
        private DocumentExecMode _mode;
        private PDFRemoteFileRequestList _requests;

        protected PDFRemoteFileRequestList Requests
        {
            get
            {
                if (null == _requests)
                {
                    _requests = new PDFRemoteFileRequestList();
                    _keyed = new Dictionary<string, PDFRemoteFileRequest>(this.Comparer);
                }
                return _requests;
            }
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

        public PDFRemoteFileRequestSet(Document owner): this(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Immediate, owner)
        {

        }

        public PDFRemoteFileRequestSet(StringComparer comparer, DocumentExecMode mode, Document owner)
        {
            this._comparer = comparer;
            this._owner = owner;
            this._mode = mode;
        }

        /// <summary>
        /// Returns all the requests in this set, and optionally clears the collection so more can be added without issue.
        /// </summary>
        /// <returns></returns>
        public PDFRemoteFileRequest[] CaptureRequests(bool clear = true)
        {
            if (this.HasRequests)
            {
                var all = this.Requests.ToArray();
                this.Requests.Clear();
                return all;
            }
            else
                return new PDFRemoteFileRequest[] { };
        }


        public bool AddRequest(PDFRemoteFileRequest request)
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


        public void FullfillRequest(PDFRemoteFileRequest request, bool raiseErrors = true)
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
                    throw request.Error ?? new InvalidOperationException("The request for the '" + request.FilePath + "' could not be completted");
            }
        }

        /// <summary>
        /// Creates an http client 
        /// </summary>
        /// <param name="urlRequest"></param>
        /// <returns></returns>
        protected virtual bool FullfillUriRequest(PDFRemoteFileRequest urlRequest)
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
        protected virtual bool FullfillFileRequest(PDFRemoteFileRequest fileRequest)
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

        ~PDFRemoteFileRequestSet()
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

    public class PDFRemoteFileAsyncRequestSet : PDFRemoteFileRequestSet
    {
        private List<Task> _captured;

        public PDFRemoteFileAsyncRequestSet(Document owner)
            : base(StringComparer.OrdinalIgnoreCase, DocumentExecMode.Asyncronous, owner)
        {
            _captured = new List<Task>();
        }

        public async Task EnsureRequestsFullfilledAsync()
        {
            if(_captured.Count > 0)
            {
                var all = _captured.ToArray();
                _captured.Clear();

                await Task.Run(() =>
                {
                    Task.WaitAll(_captured.ToArray());
                });
                this.EnsureRequestsFullfilled();
                
            }
        }

    }
}
