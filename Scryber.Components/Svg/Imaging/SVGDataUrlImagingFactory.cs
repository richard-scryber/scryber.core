using System.Text.RegularExpressions;
using System.IO;
using Scryber.Drawing;
using Scryber.Imaging.Utilities;
using System.Threading.Tasks;

namespace Scryber.Svg.Imaging
{

    public class SVGDataUrlImagingFactory : SVGImagingFactory
    {
        private static readonly Regex SvgDataMatch = new Regex("^\\s*data:image/svg\\+xml;",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        private const string SvgDataName = "SVG inline data image factory";
        private const bool SvgDataShouldCache = false;
        
        private const string DataImagePrefix = "data:image/svg+xml;base64,";


        public SVGDataUrlImagingFactory() : base(SvgDataMatch, MimeType.SvgImage, SvgDataName, SvgDataShouldCache)
        {
            
        }
        
        public override ImageData LoadImageData(IDocument document, IComponent owner, string path)
        {
            return this.DoLoadImageDataAsync(document, owner, path).Result;
        }

        public override Task<ImageData> LoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            return this.DoLoadImageDataAsync(document, owner, path);
        }

        protected override Task<ImageData> DoLoadImageDataAsync(IDocument document, IComponent owner, string path)
        {
            var bin = DataUrlHelper.ExtractBase64Data(path, MimeType.SvgImage.ToString());
            
            using (var ms = new MemoryStream(bin))
            {
                var data = DoDecodeImageData(ms, document, owner, path);
                return Task.FromResult(data);
            }
        }
    }
}