using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Scryber.OpenType.SubTables;
using Scryber.PDF.Graphics;

namespace Scryber.Drawing
{
    public class GradientLinearDescriptor : GradientDescriptor
    {

        public double Angle { get; set; }
        
        public double? MaxDomain { get; set; }
        
        public double? MinDomain { get; set; }

        public GradientLinearDescriptor() : base(GradientType.Linear)
        { }

        public GradientLinearDescriptor(List<GradientColor> colors, double angle) : base(GradientType.Linear)
        {
            this.Colors = colors;
            this.Angle = angle;
        }


        public double[] GetCoordsForBounds(Point location, Size size, bool flip = true)
        {
            return GetCoordsForBounds(new Rect(location, size), flip);
        }
        
        public double[] GetCoordsForBounds(Rect bounds, bool flip = true)
        {
            double[] coords = new double[4];
            
            var angle = this.Angle;
            if (flip)
            {
                angle = 360 -angle;
                bounds.Height = 0 - bounds.Height;
                bounds.Y -= bounds.Height;
            }

            if (CalculateOptimumCoords(bounds, angle, out var p1, out var p2))
            {
                coords[0] = p1.X.PointsValue;
                coords[1] = p1.Y.PointsValue;
                coords[2] = p2.X.PointsValue;
                coords[3] = p2.Y.PointsValue;
            }
            
            return coords;
        }

        public override PDFGradientFunction GetGradientFunction(Point offset, Size size)
        {
            var func = base.GetGradientFunction(offset, size) ;
            
            if(this.MinDomain.HasValue)
                func.DomainStart = this.MinDomain.Value;
            
            if(this.MaxDomain.HasValue)
                func.DomainEnd = this.MaxDomain.Value;
            
            return func;
        }

        //
        // Calculate Line length for 2 points
        //


        /// <summary>
        /// Given 2 points, calculates the total length of the line between them.
        /// </summary>
        /// <param name="pt1">The starting point</param>
        /// <param name="pt2">The finishing point</param>
        /// <returns>The unit length</returns>
        public static Unit GetLength(Point pt1, Point pt2)
        {
            if (pt1.IsRelative)
                throw new ArgumentOutOfRangeException(nameof(pt1),
                    "Relative dimensions cannot be used to calculate lengths.");
            
            if (pt2.IsRelative)
                throw new ArgumentOutOfRangeException(nameof(pt2),
                    "Relative dimensions cannot be used to calculate lengths.");
            
            
            var x = pt2.X - pt1.X;
            var y = pt2.Y - pt1.Y;

            var l = Math.Sqrt((x.PointsValue * x.PointsValue) + (y.PointsValue * y.PointsValue));
            
            return l;
        }
        
        
        //
        // Calculate coords for bounds
        //


        /// <summary>
        /// Given the boundary and an angle - we calculate the best 2 coordinates a linear gradient path should go for, to.
        /// NOTE one of the points may be outside of the bounds so that the linear gradient will cover the entire area.
        /// </summary>
        /// <param name="bounds">The bounds of the shape that will be covered with the gradient</param>
        /// <param name="angle">The angle of the gradient in degrees - where zero is horizontal and rotates clockwise as the value increases - using cartesian co-ordinates</param>
        /// <param name="p1">Set to the starting point of the line</param>
        /// <param name="p2">Set to the ending point of the line</param>
        /// <returns>True if the coordinates could be calculated</returns>
        /// <remarks>
        /// We calculate the shortest length of line from a corner of the rectangle bounds (based on the actual angle itself) to a point beyond the
        /// perimiter where a perpendicular line would pass through the opposite corner of the rectangle.
        /// Eg.. For 115 degrees we start on the top right corner and come back on our selves passing the bottom of the bounds until reaching a point below there
        /// where the perpendicular line (angle 25 degrees) would intersect with both the left bottom of the boundsand the gradient line.
        /// </remarks>
        public static bool CalculateOptimumCoords(Rect bounds, double angle, out Point p1, out Point p2)
        {
            while (angle < 0.0)
                angle += 360.0;

            while (angle > 360.0)
                angle -= 360.0;

            p1 = Point.Empty;
            p2 = Point.Empty;

            double radians = angle * (Math.PI / 180.0);
            double m1 = Math.Tan(radians);

            double x1;
            double y1;
            //y = mx + c
            // c = y - mx;
            double c1;
            double x2, y2;

            bool result = false;

            if (angle == 360.0 || angle == 0)
            {
                p1 = new Point(bounds.X, bounds.Y);
                p2 = new Point(bounds.X + bounds.Width, bounds.Y);
                result = true;
            }
            else if (angle > 270.0)
            {
                //between 360 and 270
                //top left corner down to the bottom right.
                p1 = new Point(bounds.X, bounds.Y + bounds.Height);
                x1 = p1.X.PointsValue;
                y1 = p1.Y.PointsValue;
                c1 = y1 - (m1 * x1);

                var m2 = -(1 / m1);


                //perpendicular line must pass through bottom right corner
                x2 = bounds.X.PointsValue + bounds.Width.PointsValue;
                y2 = bounds.Y.PointsValue;

                var c2 = y2 - (m2 * x2);

                double xi = (c2 - c1) / (m1 - m2);
                double yi = (m2 * xi) + c2;
                p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));

                result = true;
            }
            else if (angle == 270.0)
            {
                p1 = new Point(bounds.X, bounds.Y + bounds.Height);
                p2 = new Point(bounds.X, bounds.Y);
                result = true;
            }
            else if (angle > 180.0)
            {
                //between 270 and 180
                //top right corner down to the bottom left.
                p1 = new Point(bounds.X + bounds.Width, bounds.Y + bounds.Height);
                x1 = p1.X.PointsValue;
                y1 = p1.Y.PointsValue;
                c1 = y1 - (m1 * x1);

                var m2 = -(1 / m1);


                //perpendicular line must pass through bottom left corner
                x2 = bounds.X.PointsValue;
                y2 = bounds.Y.PointsValue;

                var c2 = y2 - (m2 * x2);

                double xi = (c2 - c1) / (m1 - m2);
                double yi = (m2 * xi) + c2;
                p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));

                result = true;
            }
            else if (angle == 180.0)
            {
                p1 = new Point(bounds.X + bounds.Width, bounds.Y);
                p2 = new Point(bounds.X, bounds.Y);
                result = true;
            }
            else if (angle > 90.0)
            {
                //between 180 and 90
                //bottom right corner up to the top left.
                p1 = new Point(bounds.X + bounds.Width, bounds.Y);
                x1 = p1.X.PointsValue;
                y1 = p1.Y.PointsValue;
                c1 = y1 - (m1 * x1);

                var m2 = -(1 / m1);


                //perpendicular line must pass through top left corner
                x2 = bounds.X.PointsValue;
                y2 = bounds.Y.PointsValue + bounds.Height.PointsValue;

                var c2 = y2 - (m2 * x2);

                double xi = (c2 - c1) / (m1 - m2);
                double yi = (m2 * xi) + c2;
                p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));

                result = true;

            }
            else if (angle == 90.0)
            {
                p1 = new Point(bounds.X, bounds.Y);
                p2 = new Point(bounds.X, bounds.Y + bounds.Height);
                result = true;
            }
            else //< 90
            {
                //from bottom left corner

                x1 = bounds.X.PointsValue;
                y1 = bounds.Y.PointsValue;

                c1 = bounds.Y.PointsValue - (m1 * x1);
                //perpendicular line must pass through far right corner
                var m2 = -(1 / m1);
                y2 = bounds.Y.PointsValue + bounds.Height.PointsValue;
                x2 = bounds.X.PointsValue + bounds.Width.PointsValue;

                var c2 = y2 - (m2 * x2); //20


                //calculate the intersection of the two lines.
                double xi, yi;
                //y = m1x + c1
                //y = m2x + c2
                //x = (c2 - c1)/(m1 - m2)

                xi = (c2 - c1) / (m1 - m2);
                yi = (m2 * xi) + c2;



                p1 = new Point(bounds.X, bounds.Y);
                p2 = new Point(Math.Round(xi, 10), Math.Round(yi, 10));

                result = true;
            }

            return result;
        }



        //
        // static parse linear methods
        //

        #region public static bool TryParseRepeatingLinear(string value, out GradientLinearDescriptor linear)

        public static bool TryParseRepeatingLinear(string value, out GradientLinearDescriptor linear)
        {
            linear = null;
            string[] all = _splitter.Split(value);

            if (all.Length == 0)
                return false;

            int colorStopIndex = 0;
            double angle;

            if (!ParseGradientAngle(all, out angle, ref colorStopIndex)) return false;

            List<GradientColor> colors;

            if (!ParseGradientColors(all, colorStopIndex, out colors, out var hasAtleastOneDistance, padStart: false,
                    padEnd: false)) return false;
            
            FillMiddleDistances(colors);

            const int AccuracyDecimals = 3; //Rounding of double  for distances.

            if (colors.Count < 2)
            {
                linear = null;
                return false;
            }

            decimal start;
            
            if(colors[0].Distance.HasValue)
                start = (decimal)colors[0].Distance.Value;
            else
            {
                colors[0].Distance = 0.0;
                start = 0.0m;
            }

            decimal end;
            
            if (colors[colors.Count - 1].Distance.HasValue)
            {
                end = (decimal)colors[colors.Count - 1].Distance.Value;
            }
            else
            {
                colors[colors.Count - 1].Distance = 1.0;
                end = 1.0m;
            }

            decimal offset = Math.Round(end - start, AccuracyDecimals);
            int count = (int)Math.Ceiling(1.0m / offset);
            

            //To keep the new list of color stops
            List<GradientColor> holder = new List<GradientColor>();

            //reverse enumerate through the color stops a group at a time until the start is <= 0.0
            var baseOffset = Math.Round(start, AccuracyDecimals);
            var loopIndex = 0;
            var minDistance = double.MaxValue;
            var maxDistance = double.MinValue;
            
            while (baseOffset > 0.0m)
            {
                loopIndex++;
                
                for (int i = colors.Count - 1; i >= 0; i--)
                {
                    var orig = colors[i];
                    var color = new GradientColor(orig.Color);
                    color.Distance = (double)Math.Round((decimal)orig.Distance.Value - (offset * loopIndex), AccuracyDecimals);
                    
                    minDistance = Math.Min(minDistance, color.Distance.Value);
                    
                    color.Opacity = orig.Opacity;
                    holder.Insert(0, color);
                }

                baseOffset -= offset;

            }

            //Add the defined colors now.
            
            for (var i = 0; i < colors.Count; i++)
            {
                var orig = colors[i];
                holder.Add(orig);
            }

            //enumerate normally through the color stops a group at a time until the end is >= 1.0
            baseOffset = end;
            while (baseOffset < 1.0m)
            {
                
                for (int i = 0; i < colors.Count; i++)
                {
                    var orig = colors[i];
                    var color = new GradientColor(orig.Color);
                    color.Distance = (double)Math.Round((baseOffset - start + (decimal)(orig.Distance ?? 0.0)), AccuracyDecimals);
                    maxDistance = Math.Max(maxDistance, color.Distance.Value);
                    color.Opacity = orig.Opacity;
                    holder.Add(color);
                }


                baseOffset += offset;
            }

            //TODO: Set the gradient domain to [start,end]
            linear = new GradientLinearDescriptor(holder, angle);
            linear.MinDomain = Math.Min(0.0, minDistance);
            linear.MaxDomain = Math.Max(1.0, maxDistance);
            linear.Repeating = true;
            
            return true;

        }

        #endregion
        
        #region public static bool TryParseLinear(string value, out GradientLinearDescriptor linear)

        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        /// <summary>
        /// Parses a linear gradient from a string without decorations e.g. "to top right, red, green
        /// </summary>
        /// <param name="value"></param>
        /// <param name="linear"></param>
        /// <returns></returns>
        public static bool TryParseLinear(string value, out GradientLinearDescriptor linear)
        {
            linear = null;
            string[] all = _splitter.Split(value);
            
            if (all.Length == 0)
                return false;

            int colorStopIndex = 0;
            double angle;

            if (!ParseGradientAngle(all, out angle, ref colorStopIndex)) return false;

            List<GradientColor> colors;
            
            if (!ParseGradientColors(all, colorStopIndex, out colors, out var hasAtleastOneDistance, padStart: true, padEnd : true)) return false;

            if (hasAtleastOneDistance)
                EnsureDistances(colors);
            
            linear = new GradientLinearDescriptor() { Angle = angle, Colors = new List<GradientColor>(colors) };
            return true;
        }

        internal static bool ParseGradientColors(string[] all, int startIndex, out List<GradientColor> colors, out bool hasAtleastOneDistance, bool padStart, bool padEnd)
        {
            colors = new List<GradientColor>(all.Length - startIndex);
            hasAtleastOneDistance = false;
            
            for (int i = startIndex; i < all.Length; i++)
            {
                GradientColor parsed;
                if (GradientColor.TryParse(all[i], out parsed))
                {
                    if (i == startIndex &&
                        parsed.Distance > 0) // first and offset from zero - so add an initial stop for padding. 
                    {
                        if (padStart)
                            colors.Add(new GradientColor(parsed.Color, 0.0, parsed.Opacity));
                    }

                    if (parsed.Distance.HasValue)
                    {
                        parsed.Distance = parsed.Distance / 100.0;
                        hasAtleastOneDistance = true;
                    }
                    else if
                        (i == all.Length - 1 &&
                         hasAtleastOneDistance) //we are the last one and there are other stops with distances
                        //so even though we don't have an explicit value we sould always be at 100%
                    {
                        if (padEnd)
                            parsed.Distance = 1.0;
                    }
                    
                    colors.Add(parsed);


                    if (i == all.Length - 1 && parsed.Distance.HasValue &&
                        parsed.Distance < 1.0) // last and offset from one - so add an final stop for padding. 
                    {
                        if (padEnd)
                            colors.Add(new GradientColor(parsed.Color, 1.0, parsed.Opacity));
                    }
                }
                else
                    return false;
            }

            return true;
        }

        private static bool ParseGradientAngle(string[] all, out double angle, ref int colorStopIndex)
        {
            angle = 0;
            
            if (all[0].StartsWith("to "))
            {
                var ga = all[0].Substring(3).Trim().Replace(" ", "_");
                GradientAngle parsed;
                if (Enum.TryParse(ga, true, out parsed))
                    angle = (double)parsed;
                else
                    return false;
                
                colorStopIndex = 1;
                
                if(all[1].StartsWith("in "))
                    colorStopIndex = 2;
            }
            else
            {
                if (all[0].StartsWith("in "))
                {
                    colorStopIndex = 1;
                }

                if (char.IsNumber(all[colorStopIndex], 0) || (all[colorStopIndex][0] == '-' && char.IsNumber(all[colorStopIndex], 1)))
                {
                    var deg = all[colorStopIndex].Trim();

                    if (deg.EndsWith("deg"))
                    {
                        deg = deg.Substring(0, deg.Length - 3);

                        if (!double.TryParse(deg, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out angle))
                            return false;
                    }
                    else if (deg.EndsWith("rad"))
                    {
                        deg = deg.Substring(0, deg.Length - 3);

                        if (!double.TryParse(deg, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out angle))
                            return false;

                        angle *= 180 / Math.PI;
                    }
                    else if (deg.EndsWith("turn"))
                    {
                        deg = deg.Substring(0, deg.Length - 4);

                        if (!double.TryParse(deg, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out angle))
                            return false;

                        angle = 360 * angle;

                    }
                    else if (double.TryParse(deg, System.Globalization.NumberStyles.Any,
                                 System.Globalization.CultureInfo.InvariantCulture, out angle))
                    {
                        angle = 0;
                    }
                    else
                    {
                        return false;
                    }

                    ;

                    //0 degrees on the 
                    angle += 270;

                    while (angle >= 360.0)
                        angle -= 360.0;

                    while (angle < 0.0)
                        angle += 360.0;

                    colorStopIndex = 1;
                }
                else
                    angle = (double)GradientAngle.Bottom;
            }

            return true;
        }

        internal static void EnsureDistances(List<GradientColor> colors)
        {
            int lastDistanceIndex = -1;
            double lastDistance = 0;

            if (colors[0].Distance.HasValue == false) //first one does not have a distance so set it to zero.
                colors[0].Distance = 0;
            
            for (int i = 0; i < colors.Count; i++)
            {
                GradientColor c = colors[i];
                if (c.Distance.HasValue)
                {
                    if(lastDistanceIndex != i-1)
                        DivideUpDistances(lastDistanceIndex, i, lastDistance, c.Distance.Value, colors);
                    
                    lastDistanceIndex = i;
                    lastDistance = c.Distance.Value;
                }
            }

            if (lastDistanceIndex > -1 && lastDistanceIndex < colors.Count - 1)
            {
                //Fill the last ones
            }
        }

        

        #endregion

    }
}
