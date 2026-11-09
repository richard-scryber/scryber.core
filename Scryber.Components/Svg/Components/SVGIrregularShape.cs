namespace Scryber.Svg.Components;

/// <summary>
/// Abstract base class for SVG draing components that define a shape that is not based around a rectangular area.
/// These include the polygon, polyline and path.
/// </summary>
public abstract class SVGIrregularShape : SVGShape, ISVGOffsetShape
{
    /// <summary>
    /// The shape offset will be used to update the bounds, and path when rendering based on any external re-use or other need.
    /// </summary>
    public Scryber.Drawing.Point ShapeOffset { get; set; }

    public SVGIrregularShape(ObjectType type) : base(type)
    {
    }
}