using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Scryber.Components;

namespace Scryber
{

    

    public class RemoteFileRequest : IRemoteRequest
    {

        public event RequestCompletedEventHandler Completed;

        protected virtual void OnCompleted(IComponent owner, object result)
        {
            if(null != this.Completed)
            {
                RequestCompletedEventArgs args = new RequestCompletedEventArgs(owner, this, result);
                Completed(this, args);
            }
        }

        public string ResourceType { get; private set; } 
        
        public string FilePath { get; private set; }

        public IComponent Owner { get; private set; }

        public object Arguments { get; private set; }

        public RemoteRequestCallback Callback { get; private set; }

        public Exception Error { get; private set; }

        public  object Result { get; set; }

        public bool IsCompleted { get; private set; }

        public bool IsExecuting { get; private set; }

        public bool IsSuccessful { get; private set; }
        
        public TimeSpan CacheDuration { get; set; }

        public RemoteFileRequest(string type, string path, RemoteRequestCallback callback, IComponent owner = null, object args = null)
        {
            this.ResourceType = type ?? throw new ArgumentNullException(nameof(type));
            this.FilePath = path ?? throw new ArgumentNullException(nameof(path));
            this.Callback = callback ?? throw new ArgumentNullException(nameof(callback));
            this.Owner = owner;
            this.Arguments = args;
            this.IsCompleted = false;
            this.IsExecuting = false;
            this.IsSuccessful = false;
            this.CacheDuration = Scryber.Caching.PDFCacheProvider.DefaultCacheDuration;
        }

        public void StartRequest()
        {
            if (this.IsExecuting)
                throw new InvalidOperationException("This request is already executing");

            this.IsExecuting = true;
        }

        public void CompleteRequest(object result, bool success, Exception error = null)
        {
            this.Result = result;
            this.IsCompleted = true;
            this.IsExecuting = false;
            this.IsSuccessful = success;
            this.Error = error;

            this.OnCompleted(this.Owner, this.Result);
        }


    }

    public class RemoteFileRequestList : List<RemoteFileRequest>
    {

    }
    
}
