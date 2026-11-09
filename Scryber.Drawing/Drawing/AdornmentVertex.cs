
namespace Scryber.Drawing
{

    /// <summary>
    /// Defines a structure for an adornment vertex, with location and angle.
    /// </summary>
    public struct AdornmentVertex
    {
        /// <summary>
        /// The location of the vertex relative to the current SVG layout
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// The angle in radians for the vertex point.
        /// </summary>
        public double Angle { get; set; }


        public AdornmentVertex(Point location, double angle)
        {
            this.Location = location;
            this.Angle = angle;
        }
    }
}