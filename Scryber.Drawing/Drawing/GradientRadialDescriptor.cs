using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public class GradientRadialDescriptor : GradientDescriptor
    {

        public RadialShape Shape { get; set; }

        public RadialSize Size { get; set; }

        public Unit? XCentre { get; set; }

        public Unit? YCentre { get; set; }


        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        public GradientRadialDescriptor() : this(RadialShape.Circle, RadialSize.FarthestCorner)
        { }

        public GradientRadialDescriptor(RadialShape shape, RadialSize size) : base(GradientType.Radial)
        {
            Shape = shape;
            Size = size;
        }

        

        /// <summary>
        /// Override so that the radial pattern extends beyond the set size to the farthest corner.
        /// With the repeat at the distances
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public override List<PDFGradientFunctionBoundary> GetRepeatingBoundaries(Point offset, Size size)
        {
            var items = base.GetRepeatingBoundaries(offset, size);
            if (this.Size != RadialSize.None && this.Size != RadialSize.FarthestCorner)
            {
                var height = Math.Abs(size.Height.PointsValue);
                var width = size.Width.PointsValue;

                //calculate the centre and radii for the names side and farthest corner
                var newItems = new List<PDFGradientFunctionBoundary>(items.Count);
                Point centre = PDFRadialShadingPattern.CacluateRadialCentre(this.XCentre, this.YCentre, height, width);
                var radiusActual = Math.Abs(PDFRadialShadingPattern.CalculateRadiusForSize(this.Size, height, width, centre.X.PointsValue, centre.Y.PointsValue));
                var radiusRequired = Math.Abs(PDFRadialShadingPattern.CalculateRadiusForSize(RadialSize.FarthestCorner, height, width, centre.X.PointsValue, centre.Y.PointsValue));

                //all our boundaries will be adjusted for the factor so we extend out.
                var factor = radiusActual / radiusRequired;

                var start = 0.0;
                var boundary = 0.0;
                var index = 0;

                while(boundary < 1)
                {
                    var one = items[index];
                    boundary = (one.Bounds * factor) + start;
                    one = new PDFGradientFunctionBoundary(boundary);
                    newItems.Add(one);

                    index++;

                    if(index >= items.Count)
                    {
                        index = 0;
                        start = boundary;
                        boundary = 0.0;
                    }
                }

                this.Size = RadialSize.FarthestCorner;
                items = newItems;
            }

            return items;
        }

        public static bool TryParseRadial(string value, out GradientRadialDescriptor radial)
        {
            radial = null;
            string[] all = _splitter.Split(value);
            if (all.Length == 0)
                return false;

            RadialShape shape = RadialShape.Circle;
            RadialSize size = RadialSize.FarthestCorner;
            Unit? xpos = null;
            Unit? ypos = null;

            int colorStopIndex = 0;


            if (all[0].StartsWith("circle"))
            {
                shape = RadialShape.Circle;
                all[0] = all[0].Substring("circle".Length).TrimStart();
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("ellipse"))
            {
                radial = null;
                return false;
                //shape = RadialShape.Ellipse;
                //all[0] = all[0].Substring("ellipse".Length).TrimStart();
                //colorStopIndex = 1;
            }

            if (all[0].StartsWith("closest-side"))
            {
                //TODO:Parse at percents
                size = RadialSize.ClosestSide;
                all[0] = all[0].Substring("closest-side".Length).TrimStart();
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("closest-corner"))
            {
                size = RadialSize.ClosestCorner;
                all[0] = all[0].Substring("closest-corner".Length).TrimStart();
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("farthest-side"))
            {
                size = RadialSize.FarthestSide;
                all[0] = all[0].Substring("farthest-side".Length).TrimStart();
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("farthest-corner"))
            {
                size = RadialSize.FarthestCorner;
                all[0] = all[0].Substring("farthest-corner".Length).TrimStart();
                colorStopIndex = 1;
            }

            //TODO: Support relative radii positions e.g. 10% 40% at ....


            if(all[0].StartsWith("at"))
            {
                all[0] = all[0].Substring("at".Length).TrimStart().ToLower();

                var parts = all[0].Split(' ');
                
                foreach (var part in parts)
                {
                    if (string.IsNullOrWhiteSpace(part))
                        continue;
                    var item = part.Trim();

                    switch (item)
                    {
                        case ("top"):
                            ypos = 0;
                            break;
                        case ("left"):
                            xpos = 0;
                            break;
                        case ("bottom"):
                            ypos = Double.MaxValue;
                            break;
                        case ("right"):
                            xpos = Double.MaxValue;
                            break;
                        default:
                            Unit found;
                            if (Unit.TryParse(item, out found))
                            {
                                if (xpos.HasValue)
                                    ypos = found;
                                else
                                    xpos = found;
                            }
                            break;
                    }
                }
                colorStopIndex = 1;
            }

            GradientColor[] colors = new GradientColor[all.Length - colorStopIndex];

            for (int i = 0; i < colors.Length; i++)
            {
                GradientColor parsed;
                if (GradientColor.TryParse(all[i + colorStopIndex], out parsed))
                    colors[i] = parsed;
                else
                    return false;
            }

            radial = new GradientRadialDescriptor() { Repeating = false, Shape = shape, Size = size, XCentre = xpos, YCentre = ypos, Colors = new List<GradientColor>(colors) };
            return true;
        }
    }
}
