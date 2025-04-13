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
    private static readonly bool SvgShouldCache = false; //Don't cache the images so the layout is stored.

    public SVGImagingFactory() : this(SvgMatch, SvgName, SvgShouldCache)
    {
        
    }
    
    protected SVGImagingFactory(Regex match, string name, bool shouldCache) : base(match, name, shouldCache)
    {
    }
    

    protected override ImageData DoDecodeImageData(Stream stream, IDocument document, IComponent owner, string path)
    {
        var component = Scryber.Components.Document.Parse(path, stream);
        if(null == component)
            return null;
        if(!(component is SVGCanvas))
            return null;
        var svg = component as SVGCanvas;
        var data = new SVGPDFImageData(path, svg, 1, 1);

        return data;


    }

    
}