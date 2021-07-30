using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Scryber.Components
{
    public static class DocumentAsyncExtensions
    {

        public static async Task SaveAsPDFAsync(this Document doc, string path)
        {
            using(var stream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                await SaveAsAsync(doc, stream, true, OutputFormat.PDF); 
            }
        }

        public static async Task SaveAsAsync(this Document doc, System.IO.Stream stream, bool bind, OutputFormat format)
        {
            if (null == stream)
                throw new ArgumentNullException(nameof(stream));

            var asyncRemotes = new PDFRemoteFileAsyncRequestSet(doc);
            doc.RemoteRequests = asyncRemotes;
            
            
            doc.InitializeAndLoad();

            await asyncRemotes.EnsureRequestsFullfilledAsync();

            if (bind)
                doc.DataBind();

            await asyncRemotes.EnsureRequestsFullfilledAsync();

            doc.RenderTo(stream, format);

            await asyncRemotes.EnsureRequestsFullfilledAsync();
        }

    }
}
