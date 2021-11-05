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

        public string ResourceType { get; private set; } 
        
        public string FilePath { get; private set; }

        public IComponent Owner { get; private set; }

        public object Arguments { get; private set; }

        public RemoteRequestCallback Callback { get; private set; }

        public Exception Error { get; private set; }

        public  object Result { get; set; }
        public bool IsCompleted { get; private set; }

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
            this.CacheDuration = Scryber.Caching.PDFCacheProvider.DefaultCacheDuration;
        }

        public void CompleteRequest(object result, bool success, Exception error = null)
        {
            this.Result = result;
            this.IsCompleted = true;
            this.IsSuccessful = success;
            this.Error = error;
        }
    }

    public class RemoteFileRequestList : List<RemoteFileRequest>
    {

    }
    
}
