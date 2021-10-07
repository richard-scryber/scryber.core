using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Scryber.Drawing;
using Scryber.Imaging.Formatted;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;

namespace Scryber.Imaging
{
    public abstract class ImageFactoryBase : IPDFImageDataFactory
    {
        public ImageFactoryBase()
        {
        }

        public abstract bool ShouldCache { get; }

        public virtual PDFImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            return this.DoLoadImageDataAsync(document, owner, path).Result;
        }

        public async virtual Task<PDFImageData> LoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            return await this.DoLoadImageDataAsync(document, owner, path);
        }

        protected async virtual Task<PDFImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            Stream stream = null;
            PDFImageData data = null;
            try
            {
                if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
                {
                    stream = await LoadDataFromUri(path);
                }
                else if (System.IO.Path.IsPathRooted(path))
                {
                    stream = await LoadDataFromFile(path);
                }

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

        protected virtual async Task<Stream> LoadDataFromUri(string path)
        {
            var httpClient = Scryber.ServiceProvider.GetService<HttpClient>();
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
                if (disposeClient && null != httpClient)
                    httpClient.Dispose();
            }
        }

        protected virtual async Task<Stream> LoadDataFromFile(string path)
        {
            return new System.IO.FileStream(path, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Method all inheritors should implement to return data 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="owner"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected abstract PDFImageData DoDecodeImageData(System.IO.Stream stream, IDocument document, IComponent owner, string path);

        public delegate PDFImageData FactoryCreateInstance(SixLabors.ImageSharp.Image baseImage, string source);

        private static Dictionary<Type, FactoryCreateInstance> _factories;
        private static readonly object _lock;

        public static void RegisterImageFactory(Type forType, FactoryCreateInstance factory)
        {
            if (null == forType)
                throw new ArgumentNullException(nameof(forType));

            if (null == factory)
                throw new ArgumentNullException(nameof(factory));

            lock (_lock)
            {
                _factories[forType] = factory;
            }
        }

        public static FactoryCreateInstance GetImageFactory(Type forType, bool throwIfNotFound = false)
        {
            if(null == forType)
                throw new ArgumentNullException(nameof(forType));

            FactoryCreateInstance method = null;
            lock (_lock)
            {
                if(!_factories.TryGetValue(forType, out method))
                {
                    if (throwIfNotFound)
                        throw new PDFImageFormatException("The image factory for type " + forType.FullName + " could not be found");
                }
            }

            return method;
        }

        
        static ImageFactoryBase()
        {
            _lock = new object();

            lock (_lock)
            {
                
                _factories = new Dictionary<Type, FactoryCreateInstance>();
                _factories.Add(typeof(Image<A8>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Argb32>), (img, src) => {
                    return new PDFImageSharpARGB32Data(img, src);
                });
                _factories.Add(typeof(Image<Bgr24>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Bgr565>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Bgra32>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Bgra4444>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Bgra5551>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Byte4>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<L16>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<L8>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<La16>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<La32>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Rg32>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Rgb24>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Rgb48>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Rgba1010102>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Rgba32>), (img, src) => { return new PDFImageSharpRGBA32Data(img, src); });
                _factories.Add(typeof(Image<Rgba64>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<RgbaVector>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Short2>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<Short4>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<HalfSingle>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<HalfVector2>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<HalfVector4>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<NormalizedByte2>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<NormalizedByte4>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<NormalizedShort2>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
                _factories.Add(typeof(Image<NormalizedShort4>), (img, src) => { throw new PDFImageFormatException("Format not implemented"); });
            }
        }


        protected virtual PDFImageData GetImageDataForImage(Image baseImage, string source)
        {
            if (null == baseImage)
                throw new ArgumentNullException(nameof(baseImage));

            var type = baseImage.GetType();

            var method = GetImageFactory(type, true);

            return method(baseImage, source);
        }

    }
}
