using System;
using System.Collections.Generic;
using System.Collections;

using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var doc = PDFDocument.ParseDocument(fullpath);
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
        /// <param name="content">The xml template content within the stream</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF(this ControllerBase controller, System.IO.Stream content, bool inline = true, string outputFileName = "")
        {
            var doc = PDFDocument.ParseDocument(content, ParseSourceType.DynamicContent);

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
        public static IActionResult PDF(this ControllerBase controller, PDFDocument document, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();
            document.ProcessDocument(ms);
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
            var doc = PDFDocument.ParseDocument(fullpath);
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
        /// <param name="content">The xml template content within the stream</param>
        /// <param name="model">The instance of the model to use when binding</param>
        /// <param name="inline">If true, then the output PDF will be directly streamed to the output, otherwise it will be as an attachment</param>
        /// <param name="outputFileName">The name of the file to be downloaded if it is an attachment, if not set the document file name or controller request action name will be used</param>
        /// <returns>A file action result with the appropriate headers and content type</returns>
        public static IActionResult PDF<T>(this ControllerBase controller, System.IO.Stream content, T model, bool inline = true, string outputFileName = "")
        {
            var doc = PDFDocument.ParseDocument(content, ParseSourceType.DynamicContent);

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
        public static IActionResult PDF<T>(this ControllerBase controller, PDFDocument document, T model, bool inline = true, string outputFileName = "")
        {
            var ms = new System.IO.MemoryStream();
            document.Params["Model"] = model;
            document.Params["Controller"] = controller;

            document.ProcessDocument(ms);
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


        private static string GetDocumentName(PDFDocument doc, ControllerBase controller)
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

    }
}
