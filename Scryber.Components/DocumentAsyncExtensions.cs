using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Scryber.Components
{
    

    


    public static class DocumentAsyncExtensions
    {

        //Asyncronous execution with Task

        public static async Task SaveAsPDFAsync(this Document doc, string path)
        {
            using(var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await SaveAsAsync(doc, stream, true, OutputFormat.PDF); 
            }
        }

        public static async Task SaveAsPDFAsync(this Document doc, Stream stream)
        {
            await SaveAsAsync(doc, stream, true, OutputFormat.PDF);
        }

        public static async Task SaveAsAsync(this Document doc, Stream stream, bool bind, OutputFormat format)
        {
            if (null == stream)
                throw new ArgumentNullException(nameof(stream));
            

            var asyncRemotes = new RemoteFileAsyncRequestSet(doc);
            doc.RemoteRequests = asyncRemotes;
                        
            int completed = await doc.InitializeAndLoadAsync(format);

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of InitializeAndLoad");

            completed = await asyncRemotes.EnsureRequestsFullfilledAsync();

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of " + completed + " requests after Load");

            if (bind)
                completed = await doc.DataBindAsync(format);

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of DataBind");

            completed = await asyncRemotes.EnsureRequestsFullfilledAsync();

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of " + completed + " requests after DataBind");

            completed = await doc.RenderToAsync(stream, format);

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of RenderTo");

            completed = await asyncRemotes.EnsureRequestsFullfilledAsync();

            if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the asynchronous execution of " + completed + " requests after Render");

        }



        private static async Task<int> InitializeAndLoadAsync(this Document doc, OutputFormat format)
        {
            return await Task<int>.Run(() => {
                doc.InitializeAndLoad(format);
                return 1;
            });
            
        }

        private static async Task<int> DataBindAsync(this Document doc, OutputFormat format)
        {
            return await Task<int>.Run(() => {
                doc.DataBind(format);
                return 1;
            });

        }

        private static async Task<int> RenderToAsync(this Document doc, System.IO.Stream stream, OutputFormat format)
        {
            return await Task<int>.Run(() => {
                if (format == OutputFormat.PDF)
                    doc.RenderToPDF(stream);
                else
                {
                    throw new ArgumentOutOfRangeException("The format " + format +
                                                          " is not supported in asyncronous mode");
                }
                return 1;
            });

        }

    }
}
