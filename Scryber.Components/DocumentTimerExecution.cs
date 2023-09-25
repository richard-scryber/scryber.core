using System;
using System.IO;

namespace Scryber.Components
{
    /// <summary>
    /// Delegate callback method from the Document time execution methods.
    /// </summary>
    /// <param name="args">The arguments that will be passed back to the delegate handler. And should be handled appropriately</param>
    public delegate void SaveAsCallback(SaveAsCallbackArgs args);


    /// <summary>
    /// The arguments passed back to a SaveAsCallback execution.
    /// If an error was raised then it can either be handled (<see cref="SetErrorHandled(bool, bool)"/>, or it will be re-thrown.
    /// If successful, then close/dispose of the stream if it was created outside of the call.
    /// </summary>
    public class SaveAsCallbackArgs
    {
        /// <summary>
        /// Set to true if execution has completed (Stage == Written). Otherwise false.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Set to any error that was raised up to this stage within the processing.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// The stream that is currently being written to. It is IMPORTANT to dispose of it, if created outside the SaveAs call.
        /// </summary>
        public Stream OutputStream { get; private set; }

        /// <summary>
        /// The original path of the document (if any) being processed.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// The stage within the processing of the document the callback was made. Written if complete (or a different stage if an error has occured).
        /// </summary>
        public DocumentGenerationStage Stage{ get; private set; }

        /// <summary>
        /// Should be set to true with the <see cref="SetErrorHandled(bool, bool)"/> if the exection error, should not be re-thrown.
        /// </summary>
        public bool ErrorHandled { get; private set; }

        /// <summary>
        /// Should be set to true with the <see cref="SetErrorHandled(bool, bool)"/> if the processing of the document should continue on. This could be in an unknown state.
        /// </summary>
        public bool ContinueExecution { get; private set; }

        

        internal SaveAsCallbackArgs(bool success, Stream output, string path, DocumentGenerationStage stage, Exception error)
        {
            this.Success = success;
            this.OutputStream = output;
            this.Path = path;
            this.Stage = stage;
            this.Error = error;
            this.ErrorHandled = false;
            this.ContinueExecution = !success;
        }

        /// <summary>
        /// Should be invoked by the called delegate method handler, if execution should not re-throw the error, and/or execution should continue.
        /// </summary>
        /// <param name="handled"></param>
        /// <param name="continueExec"></param>
        public void SetErrorHandled(bool handled, bool continueExec)
        {
            this.ErrorHandled = handled;
            this.ContinueExecution = continueExec;
        }

    }

    public static class DocumentTimerExecution
    {
        //Asyncronous execution with Timer

        public static void SaveAsPDF(this Document doc, string path, SaveAsCallback callback)
        {
            var stream = new FileStream(path, FileMode.Create, FileAccess.Write);

            SaveAs(doc, stream, path, true, true, OutputFormat.PDF, callback);

        }

        public static void SaveAsPDF(this Document doc, Stream stream, SaveAsCallback callback)
        {
            SaveAs(doc, stream, "", true, false, OutputFormat.PDF, callback);
        }

        public static void SaveAs(this Document doc, Stream stream, string path, bool bind, bool ownsStream, OutputFormat format, SaveAsCallback callback)
        {
            if (null == stream)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var timerRemotes = new RemoteFileTimerRequestSet(doc);
            doc.RemoteRequests = timerRemotes;

            //Execute the Initialize and load

            try
            {
                doc.InitializeAndLoad(format);
            }
            catch (Exception ex)
            {
                if (!HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.Initialized, callback, ex))
                {
                    timerRemotes.Dispose();
                    return;
                }
                else if (doc.TraceLog != null)
                    doc.TraceLog.Add(TraceLevel.Warning, "Asyncronous", "Continuing after error occurred in InitializeAndLoad : " + ex.Message);

            }

            //fulfill any requests from InitializeAndLoad, then next stage

            timerRemotes.EnsureRequestsFullfilled((resultInit) =>
            {
                if (resultInit.Error != null)
                {
                    if (!HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.Loaded, callback, resultInit.Error))
                    {
                        timerRemotes.Dispose();
                        return;
                    }
                    else if (doc.TraceLog != null)
                        doc.TraceLog.Add(TraceLevel.Warning, "Asyncronous", "Continuing after error occurred in the timed request fulfillment, after InitializeAndLoad : " + resultInit.Error.Message);
                }
                else if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                    doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the timed execution of Databind");

                //Execute the data bind

                try
                {
                    doc.DataBind(format); //this runs synchronously
                }
                catch (Exception ex)
                {
                    if (!HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.Bound, callback, ex))
                    {
                        timerRemotes.Dispose();
                        return;
                    }
                    else if (doc.TraceLog != null)
                        doc.TraceLog.Add(TraceLevel.Warning, "Asyncronous", "Continuing after error occurred in Databind : " + ex.Message);
                }

                //Nest again to execute any queued requests post data-bind

                timerRemotes.EnsureRequestsFullfilled((resultBind) =>
                {
                    if (resultBind.Error != null)
                    {
                        if (!HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.LayingOut, callback, resultBind.Error))
                        {
                            timerRemotes.Dispose();
                            return;
                        }
                        else if (doc.TraceLog != null)
                            doc.TraceLog.Add(TraceLevel.Warning, "Asyncronous", "Continuing after error occurred in the timed request fulfillment, after Databinding : " + resultBind.Error.Message);
                    }
                    else if (doc.TraceLog != null && doc.TraceLog.ShouldLog(TraceLevel.Message))
                        doc.TraceLog.Add(TraceLevel.Message, "Asyncronous", "Completed the timed execution of Databind and any remote requests");


                    //Finally render to..

                    if (format == OutputFormat.PDF)
                    {
                        try
                        {
                            doc.RenderToPDF(stream);
                        }
                        catch (Exception ex)
                        {
                            if (!HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.Written, callback, ex))
                            {
                                timerRemotes.Dispose();
                                return;
                            }
                            else if (doc.TraceLog != null)
                                doc.TraceLog.Add(TraceLevel.Warning, "Asyncronous", "Continuing after error occurred in Render : " + ex.Message);
                        }

                        //WE ARE DONE
                        if (null != callback)
                        {
                            timerRemotes.Dispose();
                            var args = new SaveAsCallbackArgs(true, stream, path, DocumentGenerationStage.Written, null);
                            callback(args);
                        }
                    }
                    else
                    {
                        timerRemotes.Dispose();
                        HandleRequestError(stream, path, ownsStream, DocumentGenerationStage.Written, callback, new PDFRenderException("The output format " + format + " is not known"));
                    }
                });

            });
        }

        private static bool HandleRequestError(Stream stream, string path, bool ownsStream, DocumentGenerationStage stage, SaveAsCallback callback, Exception error)
        {
            if (null == error)
                error = new Exception("Unknown Exception: an error occurred, but was not passed to the handler");

            if (null != callback)
            {
                
                var args = new SaveAsCallbackArgs(false, stream, path, stage, error);

                callback(args);

                if (args.ErrorHandled == false)
                {
                    if (ownsStream)
                        stream.Dispose();

                    throw new PDFException("Could not complete the remote requests after initialization. See the inner exception for more details", error);
                }
                else if (args.ContinueExecution)
                    return true;
                else
                    return false;
            }
            else
            {
                if (ownsStream)
                    stream.Dispose();

                throw new PDFException("Could not complete the remote requests after initialization. See the inner exception for more details", error);
            }

        }
    }
}

