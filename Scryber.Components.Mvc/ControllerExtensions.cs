using System;
using System.Collections.Generic;
using System.Collections;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Scryber.Components.Mvc
{
    public static class ControllerExtensions
    {

        private const string PDFMimeType = "application/pdf";

        #region No Model

        /// <summary>
        /// Generate and return a PDF document file result from the template at the specified document path
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="fullpath">The full path to the document in the file system</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF(this ControllerBase controller,  string fullpath, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(fullpath);
            if(inline == false)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = System.IO.Path.GetFileNameWithoutExtension(fullpath) + ".pdf";
            }

            return PDF(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF(this ControllerBase controller, System.IO.Stream content, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);

            return PDF(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="path">The path to the file for any relative references</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF(this ControllerBase controller, System.IO.Stream content, string path, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, path, ParseSourceType.DynamicContent);

            return PDF(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the PDFDocument instance
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="document">The PDF document to render to the output stream</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF(this ControllerBase controller, Document document, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();
            document.SaveAsPDF(ms);
            ms.Flush();

            ms.Position = 0;
            if (!inline)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = GetDocumentName(document, controller);
                return controller.File(ms, PDFMimeType, outputFileName);
            }
            else
                return controller.File(ms, PDFMimeType);
        }

        #endregion


        #region Generic type model

        /// <summary>
        /// Generate and return a PDF document file result from the template at the specified document path
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="fullpath">The full path to the document in the file system</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF<T>(this ControllerBase controller, string fullpath, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(fullpath);
            if (inline == false)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = System.IO.Path.GetFileNameWithoutExtension(fullpath) + ".pdf";
            }

            return PDF(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF<T>(this ControllerBase controller, System.IO.Stream content, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);

            return PDF(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="path">The path of the file for any relative references</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF<T>(this ControllerBase controller, System.IO.Stream content, string path, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content,path, ParseSourceType.DynamicContent);

            return PDF(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the PDFDocument instance
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="document">The PDF document to render to the output stream</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF<T>(this ControllerBase controller, Document document, T model, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();
            document.Params["Model"] = model;
            document.Params["Controller"] = controller;

            document.SaveAsPDF(ms);
            ms.Flush();

            ms.Position = 0;
            if (!inline)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = GetDocumentName(document, controller);
                return controller.File(ms, PDFMimeType, outputFileName);
            }
            else
                return controller.File(ms, PDFMimeType);
        }

        #endregion

        //
        // Async requests
        //

        #region No Model

        /// <summary>
        /// Generate and return a PDF document file result from the template at the specified document path
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="fullpath">The full path to the document in the file system</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync(this ControllerBase controller, string fullpath, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(fullpath);
            if (inline == false)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = System.IO.Path.GetFileNameWithoutExtension(fullpath) + ".pdf";
            }

            return await PDFAsync(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync(this ControllerBase controller, System.IO.Stream content, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);

            return await PDFAsync(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="path">The path to the file for any relative references</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync(this ControllerBase controller, System.IO.Stream content, string path, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, path, ParseSourceType.DynamicContent);

            return await PDFAsync(controller, doc, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the PDFDocument instance
        /// </summary>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="document">The PDF document to render to the output stream</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync(this ControllerBase controller, Document document, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();

            await document.SaveAsPDFAsync(ms);

            ms.Flush();

            ms.Position = 0;
            if (!inline)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = GetDocumentName(document, controller);
                return controller.File(ms, PDFMimeType, outputFileName);
            }
            else
                return controller.File(ms, PDFMimeType);
        }

        #endregion

        #region Generic type model

        /// <summary>
        /// Generate and return a PDF document file result from the template at the specified document path
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="fullpath">The full path to the document in the file system</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync<T>(this ControllerBase controller, string fullpath, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(fullpath);
            if (inline == false)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = System.IO.Path.GetFileNameWithoutExtension(fullpath) + ".pdf";
            }

            return await PDFAsync(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync<T>(this ControllerBase controller, System.IO.Stream content, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, ParseSourceType.DynamicContent);

            return await PDFAsync(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the template in the content stream
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="content">The XML template content within the stream</param>
        /// <param name="path">The path of the file for any relative references</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync<T>(this ControllerBase controller, System.IO.Stream content, string path, T model, bool inline = true, string outputFileName = "")
        {
            var doc = Document.ParseDocument(content, path, ParseSourceType.DynamicContent);

            return await PDFAsync(controller, doc, model, inline, outputFileName);
        }

        /// <summary>
        /// Generate and return a PDF document file result from the PDFDocument instance
        /// </summary>
        /// <typeparam name="T">The generic type declaration for the model to be passed to the document when binding</typeparam>
        /// <param name="controller">Self reference to the controller as an extension method</param>
        /// <param name="document">The PDF document to render to the output stream</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static async Task<IActionResult> PDFAsync<T>(this ControllerBase controller, Document document, T model, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();
            document.Params["Model"] = model;
            document.Params["Controller"] = controller;

            await document.SaveAsPDFAsync(ms);

            ms.Flush();

            ms.Position = 0;
            if (!inline)
            {
                if (string.IsNullOrEmpty(outputFileName))
                    outputFileName = GetDocumentName(document, controller);
                return controller.File(ms, PDFMimeType, outputFileName);
            }
            else
                return controller.File(ms, PDFMimeType);
        }

        #endregion


        #region ParseView[<TModel>](this Controller controller...)

        public static async Task<Document> ParseDocumentView<TModel>(this Controller controller, string viewName, TModel model = null, bool partial = false) where TModel : class
        {
            StringBuilder builder = new StringBuilder();
            string path;

            using (StringWriter sw = new StringWriter(builder))
            {
                path = await RenderViewAsync(controller, viewName, model, sw, partial);

                if (string.IsNullOrEmpty(path))
                    return null;
            }

            using (StringReader reader = new StringReader(builder.ToString()))
            {
                var request = controller.Request;
                path = request.Scheme + "://" + request.Host.Value + request.Path.Value;
                var doc = Document.ParseDocument(reader, path, ParseSourceType.LocalFile);

                doc.LoadedSource = path;

                return doc;
            }

        }


        internal static async Task<string> RenderViewAsync<TModel>(Controller controller, string viewName, TModel model, TextWriter writer, bool partial) where TModel : class
        {
            string path;

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            if (null != model)
                controller.ViewData.Model = model;


            IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, !partial);

            if (viewResult.Success == false)
            {
                path = string.Empty;
                return path;
            }

            path = viewResult.View.Path;

            ViewContext viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                writer,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);

            return path;

        }


        #endregion

        #region GetDocumentName(Document doc, ControllerBase controller)

        private static string GetDocumentName(Document doc, ControllerBase controller)
        {
            string name = null;
            if(!string.IsNullOrEmpty(doc.FileName))
            {
                name = doc.FileName;
            }
            else if(!string.IsNullOrEmpty(doc.Name))
            {
                name = doc.Name; 
            }
            else if (controller.Request.Path.HasValue)
            {
                name = controller.Request.Path.Value;
                var index = name.LastIndexOf("/");
                if (index == name.Length)
                {
                    name = name.Substring(0, name.Length - 1);
                    index = name.LastIndexOf("/");
                }
                if (index > -1)
                    name = name.Substring(name.LastIndexOf("/") + 1);
                
            }

            if (string.IsNullOrEmpty(System.IO.Path.GetExtension(name)))
                name += ".pdf";

            return name;
        }

        #endregion

    }
}
