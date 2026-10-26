using System;

namespace Scryber.Drawing
{
    /// <summary>
    /// A single shadow for the box-shadow or text-shadow properties
    /// </summary>
    public class BoxShadow
    {
        /// <summary>The horizontal offset of the shadow. Positive is to the right.</summary>
        public Unit OffsetX { get; set; }

        /// <summary>The vertical offset of the shadow. Positive is down.</summary>
        public Unit OffsetY { get; set; }

        /// <summary>The distance over which the edge of the shadow fades. Zero is a sharp edge.</summary>
        public Unit Blur { get; set; }

        /// <summary>The amount the shadow is expanded (or contracted when negative). Not used by text shadows.</summary>
        public Unit Spread { get; set; }

        /// <summary>The color of the shadow, or null to use the color of the text.</summary>
        public Color? Color { get; set; }

        /// <summary>The opacity of the shadow (from an rgba or hsla color). Default is 1.</summary>
        public double Opacity { get; set; } = 1.0;

        /// <summary>True if the shadow is inside the box (inset). Inset shadows are not currently drawn.</summary>
        public bool Inset { get; set; }

        public override string ToString()
        {
            return (Inset ? "inset " : "") + OffsetX + " " + OffsetY + " " + Blur + " " + Spread;
        }
    }
}
