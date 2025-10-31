using System.IO;
using System.Text.RegularExpressions;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Imaging;
using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Implements the IImageFactory for a referenced image 
/// </summary>
public class SVGImagingFactory : ImageFactoryBase
{
    
    private static readonly Regex SvgMatch = new Regex("\\.(svg)?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
    private static readonly string SvgName = "SVG Image factory";
    private static readonly MimeType SvgType = MimeType.SvgImage;
    private static readonly bool SvgShouldCache = false; //Don't cache the images so the layout is stored.

    public SVGImagingFactory() : this(SvgMatch, SvgType, SvgName, SvgShouldCache)
    {
        
    }
    
    protected SVGImagingFactory(Regex match, MimeType type, string name, bool shouldCache) : base(match, type, name, shouldCache)
    {
    }

    protected override ImageData DoLoadRawImageData(IDocument document, IComponent owner, byte[] rawData, MimeType type)
    {
        using (var stream = new MemoryStream(rawData))
        {
            var name = document.GetIncrementID(ObjectTypes.ImageData) + ".svg";
            return this.DoDecodeImageData(stream, document, owner, name);
        }
    }

    
    

    protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
    {
        var component = Scryber.Components.Document.Parse(path, stream);
        if(null == component)
            return null;
        if(!(component is SVGCanvas))
            return null;
        var svg = component as SVGCanvas;
        var data = new SVGPDFImageData(path, svg);

        return data;


    }

    
}