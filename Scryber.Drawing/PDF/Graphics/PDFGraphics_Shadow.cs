using System;
using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF.Graphics
{
    public partial class PDFGraphics
    {
        /// <summary>
        /// The maximum number of layers used to approximate the blur of a shadow
        /// </summary>
        public const int MaxShadowBlurLayers = 12;

        /// <summary>
        /// Draws a shadow behind (and not within) a box. The shadow is the box, offset, expanded by the spread, and blurred.
        /// </summary>
        /// <param name="box">The border box the shadow is for</param>
        /// <param name="topLeft">The corner radii of the box</param>
        /// <param name="offsetX">The horizontal offset of the shadow from the box</param>
        /// <param name="offsetY">The vertical offset of the shadow from the box</param>
        /// <param name="blur">The distance over which the shadow edge fades (0 for a sharp edge)</param>
        /// <param name="spread">The amount the shadow is expanded (or shrunk if negative)</param>
        /// <param name="color">The shadow color</param>
        /// <param name="opacity">The overall opacity of the shadow, 0 to 1</param>
        public void DrawBoxShadow(Rect box, Unit topLeft, Unit topRight, Unit bottomLeft, Unit bottomRight,
            Unit offsetX, Unit offsetY, Unit blur, Unit spread, Color color, double opacity)
        {
            if (opacity <= 0.0 || box.Width <= Unit.Zero || box.Height <= Unit.Zero)
                return;

            double blurPoints = Math.Max(0.0, blur.PointsValue);
            double spreadPoints = spread.PointsValue;

            //The more layers, the smoother the fade. One layer for no blur.
            int layers = blurPoints <= 0.0 ? 1 : Math.Min(MaxShadowBlurLayers, Math.Max(3, (int)Math.Ceiling(blurPoints)));

            //Each layer adds to the ones below, so the opacity of a layer is such that all the layers together give the full opacity
            double layerOpacity = layers == 1 ? opacity : 1.0 - Math.Pow(1.0 - Math.Min(opacity, 1.0), 1.0 / layers);

            this.SaveGraphicsState();

            //Everything is clipped to outside the box, so the shadow is not seen through a transparent box.
            double reach = Math.Abs(offsetX.PointsValue) + Math.Abs(offsetY.PointsValue) + Math.Abs(spreadPoints) + blurPoints + 10.0;
            Rect outer = new Rect(box.X.PointsValue - reach, box.Y.PointsValue - reach, box.Width.PointsValue + (reach * 2), box.Height.PointsValue + (reach * 2));
            this.SetClipExcludingRect(outer, box, topLeft, topRight, bottomLeft, bottomRight);

            var brush = new PDFSolidBrush(color, layerOpacity);

            for (int i = 0; i < layers; i++)
            {
                //from the outermost layer to the innermost, spanning the blur around the edge of the spread
                double inflate = layers == 1 ? spreadPoints : spreadPoints + (blurPoints / 2.0) - (blurPoints * (i + 0.5) / layers);

                double x = box.X.PointsValue + offsetX.PointsValue - inflate;
                double y = box.Y.PointsValue + offsetY.PointsValue - inflate;
                double w = box.Width.PointsValue + (inflate * 2);
                double h = box.Height.PointsValue + (inflate * 2);

                if (w <= 0 || h <= 0)
                    continue;

                //A square corner stays square when the shadow is expanded, a rounded corner grows with it.
                double tl = topLeft.PointsValue > 0 ? Math.Max(0.0, topLeft.PointsValue + inflate) : 0.0;
                double tr = topRight.PointsValue > 0 ? Math.Max(0.0, topRight.PointsValue + inflate) : 0.0;
                double bl = bottomLeft.PointsValue > 0 ? Math.Max(0.0, bottomLeft.PointsValue + inflate) : 0.0;
                double br = bottomRight.PointsValue > 0 ? Math.Max(0.0, bottomRight.PointsValue + inflate) : 0.0;

                //corners cannot be bigger than half the size
                double max = Math.Min(w, h) / 2.0;
                tl = Math.Min(tl, max);
                tr = Math.Min(tr, max);
                bl = Math.Min(bl, max);
                br = Math.Min(br, max);

                if (tl > 0 || tr > 0 || bl > 0 || br > 0)
                    this.FillRoundRectangle(brush, new Unit(x, PageUnits.Points), new Unit(y, PageUnits.Points), new Unit(w, PageUnits.Points), new Unit(h, PageUnits.Points),
                        new Unit(tl, PageUnits.Points), new Unit(tr, PageUnits.Points), new Unit(bl, PageUnits.Points), new Unit(br, PageUnits.Points));
                else
                    this.FillRectangle(brush, new Unit(x, PageUnits.Points), new Unit(y, PageUnits.Points), new Unit(w, PageUnits.Points), new Unit(h, PageUnits.Points));
            }

            this.RestoreGraphicsState();
        }

        /// <summary>
        /// Sets the clipping path to the outer rectangle, less the hole (with optional rounded corners) - so subsequent drawing is only outside the hole.
        /// </summary>
        public void SetClipExcludingRect(Rect outer, Rect hole, Unit topLeft, Unit topRight, Unit bottomLeft, Unit bottomRight)
        {
            this.RenderRectangle(outer.X, outer.Y, outer.Width, outer.Height);

            if (topLeft > Unit.Zero || topRight > Unit.Zero || bottomLeft > Unit.Zero || bottomRight > Unit.Zero)
                this.DoOutputRoundRectangleWithSidesFill(hole.X, hole.Y, hole.Width, hole.Height,
                    Sides.Top | Sides.Right | Sides.Bottom | Sides.Left, topLeft, topRight, bottomLeft, bottomRight);
            else
                this.RenderRectangle(hole.X, hole.Y, hole.Width, hole.Height);

            this.Writer.WriteOpCodeS(PDFOpCode.GraphSetClipEvenOdd);
            this.Writer.WriteOpCodeS(PDFOpCode.GraphNoOp);
        }
    }
}
