using System;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.Styles;

namespace Scryber.Components
{
    /// <summary>
    /// The disc, circle or square marker of a list item, drawn as a shape in the colour of the text (as a browser does),
    /// rather than as a character - the standard fonts do not have a hollow circle or a square.
    /// </summary>
    /// <remarks>The marker is laid out inline in the number box of the item, with its bottom on the baseline.
    /// Its size, height above the baseline and the space after it are proportions of the font size, measured from Chrome.</remarks>
    public class ListItemMarker : ShapeComponent
    {
        /// <summary>The width and height of the marker, as a proportion of the font size</summary>
        public const double MarkerSizeFactor = 0.31;

        /// <summary>The space between the bottom of the marker and the baseline, as a proportion of the font size</summary>
        public const double MarkerRaiseFactor = 0.15;

        /// <summary>The space after the marker (in addition to the alley of the list item), as a proportion of the font size</summary>
        public const double MarkerSpaceFactor = 0.07;

        /// <summary>The width of the line of a circle marker, as a proportion of the font size (1px at 16px)</summary>
        public const double MarkerStrokeFactor = 1.0 / 16.0;

        /// <summary>
        /// Gets or sets the shape of the marker - Disc, Circle or Square
        /// </summary>
        public ListNumberingGroupStyle Shape { get; set; }

        /// <summary>
        /// Gets or sets the font size the marker is sized from
        /// </summary>
        public Unit FontSize { get; set; }

        /// <summary>
        /// Gets or sets the distance from the top of the line to the baseline of the text of the item.
        /// The marker is drawn its raise above the baseline. If zero the marker is at the top of the line.
        /// </summary>
        public Unit BaselineOffset { get; set; }

        public ListItemMarker()
            : this(ListNumberingGroupStyle.Disc, Font.DefaultFontSize, Unit.Zero)
        {
        }

        public ListItemMarker(ListNumberingGroupStyle shape, Unit fontSize, Unit baselineOffset)
            : base(ObjectTypes.ShapeCircle)
        {
            this.Shape = shape;
            this.FontSize = fontSize;
            this.BaselineOffset = baselineOffset;
        }

        protected override GraphicsPath CreatePath(Size available, Style fullstyle)
        {
            var em = this.FontSize.PointsValue;
            var size = em * MarkerSizeFactor;
            var raise = em * MarkerRaiseFactor;

            //the top of the marker, so its bottom is the raise above the baseline
            var top = Math.Max(0.0, this.BaselineOffset.PointsValue - raise - size);
            var path = new GraphicsPath();

            if (this.Shape == ListNumberingGroupStyle.Square)
            {
                path.MoveTo(new Point(0, top));
                path.LineTo(new Point(size, top));
                path.LineTo(new Point(size, top + size));
                path.LineTo(new Point(0, top + size));
                path.ClosePath(true);
            }
            else
            {
                //a circle is stroked on its centre line, so the outer edge is the size of the marker
                var inset = this.Shape == ListNumberingGroupStyle.Circle ? (em * MarkerStrokeFactor) / 2.0 : 0.0;
                AddCircle(path, size / 2.0, top + (size / 2.0), (size / 2.0) - inset);
            }

            //The space after the marker, and from the top of the line to the baseline, are part of the size of the marker
            path.BeginPath();
            path.MoveTo(new Point(0, 0));
            path.MoveTo(new Point(size + (em * MarkerSpaceFactor), top + size + raise));

            return path;
        }

        private static void AddCircle(GraphicsPath path, double cx, double cy, double r)
        {
            //the standard approximation of a quarter circle with a cubic bezier
            var k = r * 0.5522847498;

            path.MoveTo(new Point(cx + r, cy));
            path.CubicCurveTo(new Point(cx, cy + r), new Point(cx + r, cy + k), new Point(cx + k, cy + r));
            path.CubicCurveTo(new Point(cx - r, cy), new Point(cx - k, cy + r), new Point(cx - r, cy + k));
            path.CubicCurveTo(new Point(cx, cy - r), new Point(cx - r, cy - k), new Point(cx - k, cy - r));
            path.CubicCurveTo(new Point(cx + r, cy), new Point(cx + k, cy - r), new Point(cx + r, cy - k));
            path.ClosePath(true);
        }

        public override PDFObjectRef OutputToPDF(PDFRenderContext context, PDFWriter writer)
        {
            var graphics = context.Graphics;
            if (null == graphics || null == this.Path)
                return null;

            //the marker is the colour of the text
            var color = null != context.FullStyle
                ? context.FullStyle.GetValue(StyleKeys.FillColorKey, StandardColors.Black)
                : StandardColors.Black;

            if (this.Shape == ListNumberingGroupStyle.Circle)
            {
                var width = new Unit(this.FontSize.PointsValue * MarkerStrokeFactor, PageUnits.Points);
                graphics.DrawPath(new PDFSolidPen(color, width), context.Offset, this.Path);
            }
            else
                graphics.FillPath(new PDFSolidBrush(color), context.Offset, this.Path);

            return null;
        }
    }
}
