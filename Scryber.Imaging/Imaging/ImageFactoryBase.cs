using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;


namespace Scryber.Imaging
{
    public abstract class ImageFactoryBase : IPDFImageDataFactory
    {
        public bool ShouldCache { get; }

        public Regex Match { get; }
        
        public string Name { get; }
        
        public ImageFactoryBase(Regex match, string name, bool shouldCache)
        {
            this.Match = match ?? throw new ArgumentNullException(nameof(match));
            this.Name = name;
            this.ShouldCache = shouldCache;
        }

        public bool IsMatch(string forPath)
        {
            return this.Match.IsMatch(forPath);
        }
        
        public virtual ImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            if (document is IResourceRequester resourceRequester)
            {
                return GetProxyImageData(document, resourceRequester, owner, path);
            }
            else
            {
                var data = this.DoLoadImageDataAsync(document, owner, path).Result;
                return data;
            }
        }

        public virtual async Task<ImageData> LoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            if (document is IResourceRequester resourceRequester)
            {
                return GetProxyImageData(document, resourceRequester, owner, path);
            }
            else
            {
                return await this.DoLoadImageDataAsync(document, owner, path);
            }
        }

        private static readonly System.Runtime.InteropServices.OSPlatform _wasm;
        protected virtual async Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            Stream stream = null;
            ImageData data = null;
            try
            {
                if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                {
                    stream = await LoadDataFromUriAsync(path);
                }
                else if (System.IO.Path.IsPathRooted(path))
                {
                    stream = await LoadDataFromFileAsync(path);
                }

                if (stream != null)
                    data = DoDecodeImageData(stream, document, owner, path);

            }
            catch (Exception ex)
            {
                if (document.ConformanceMode == ParserConformanceMode.Strict)
                    throw new System.IO.IOException("Could not load the image for component " + (owner == null ? "UNKNOWN" : owner.ID) + ". See the inner exception for more details", ex);
                else
                    document.TraceLog.Add(TraceLevel.Error, "Imaging", " Could not load the image for component " + (owner == null ? "UNKNOWN" : owner.ID) + " from path: " + path, ex);
            }

            return data;

        }

        protected virtual ImageData GetProxyImageData(IDocument document, IResourceRequester requester, IComponent owner, string path)
        {
            var callback = new RemoteRequestCallback((comp, request, asyncStream) =>
            {
                if (null == asyncStream && request.IsCompleted)
                    //no stream to decode - will happen if there is a cache hit for the image
                    return request.IsSuccessful;
                else
                {
                    object asyncData = null;
                    Exception error = null;
                    try
                    {
                        asyncData = this.DoDecodeImageData(asyncStream, document, comp, path);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                        asyncData = null;
                    }
                    request.CompleteRequest(asyncData, asyncData != null, error);
                    return request.IsSuccessful;
                }
            });
            var req = requester.RequestResource(Scryber.PDF.Resources.PDFResource.XObjectResourceType, path, callback, owner, this);

            return new ImageDataProxy(req, path);
        }

        protected virtual async Task<Stream> LoadDataFromUriAsync(string path)
        {
            var httpClient = GetHttpClient();
            var disposeClient = false;

            if (null == httpClient)
            {
                httpClient = new HttpClient();
                disposeClient = true;
            }

            try
            {
                var result = await httpClient.GetStreamAsync(path);

                return result;
            }
            finally
            {
                if (disposeClient)
                    httpClient.Dispose();
            }
        }

        protected virtual HttpClient GetHttpClient()
        {
            return Scryber.ServiceProvider.GetService<HttpClient>();
        }
        
        /// <summary>
        /// Runs syncronously, but matches the pattern and can be overriden to perform any async operation
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected virtual Task<Stream> LoadDataFromFileAsync(string path)
        {
            Stream stream = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read);
            return Task.FromResult(stream);
        }
        
        /// <summary>
        /// Method all inheritors should implement to return data 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="owner"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected abstract ImageData DoDecodeImageData(System.IO.Stream stream, IDocument document, IComponent owner, string path);

        


        protected virtual PDFImageDataBase GetImageDataForImage(ImageFormat format, Image baseImage, string source, int bitdepth, bool hasAlpha, ColorSpace colorSpace)
        {
            if (null == baseImage)
                throw new ArgumentNullException(nameof(baseImage));

            var data = GetImageDataForImage(baseImage, source);
            data.SetSourceImageFormat(format, bitdepth, hasAlpha, colorSpace);
            return data;
        }

        public static PDFImageDataBase GetImageDataForImage(Image image, string source)
        {
            PDFImageDataBase data;
            
            switch (image)
            {
                case Image<Argb32> argb32:
                    data = new PDFImageSharpARGB32Data(argb32, source);
                    break;
                case Image<Rgba32> rgba32:
                    data = new PDFImageSharpRGBA32Data(rgba32, source);
                    break;
                case Image<Rgb24> rgb24:
                    data = new PDFImageSharpRGB24Data(rgb24, source);
                    break;
                case Image<Bgr24> bgr24:
                    data = new PDFImageSharpBgr24Data(bgr24, source);
                    break;
                default:
                    throw new NotSupportedException("The image type " + image.GetType().Name + " is not supported");
            }

            return data;
        }

    }
}
