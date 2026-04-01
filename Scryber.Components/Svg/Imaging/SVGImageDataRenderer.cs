using Scryber.Svg.Components;

namespace Scryber.Svg.Imaging;

/// <summary>
/// Manages the rendering of the SVG Content as an image
/// </summary>
public class SVGImageDataRenderer
{
    
    #region public SVGCanvas Canvas {get; private set;}

    private SVGCanvas _svgCanvas = null;

    /// <summary>
    /// Gets the canvas for this SVG Image data that is parsed from the loaded content
    /// </summary>
    public SVGCanvas Canvas
    {
        get { return _svgCanvas; }
        private set { _svgCanvas = value; }
    }

    #endregion
    
}