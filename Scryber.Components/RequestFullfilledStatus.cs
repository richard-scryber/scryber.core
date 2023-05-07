using System;
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
}

