using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace Scryber.Drawing
{
    public class GradientRadialDescriptor : GradientDescriptor
    {
        public RadialShape Shape { get; set; }

        public RadialSize Size { get; set; }

        public Point? StartCenter { get; set; }
        
        public Point? EndCenter { get; set; }
        
        public Unit? StartRadius { get; set; }
        
        public Unit? EndRadius { get; set; }

        public GradientRadialDescriptor(): this(new List<GradientColor>(), RadialShape.Circle, RadialSize.FarthestSide)
        {}

        public GradientRadialDescriptor(List<GradientColor> colors, RadialShape shape, RadialSize size) : base(GradientType.Radial)
        {
            this.Colors = colors;
            Shape = shape;
            Size = size;
            StartCenter = new Point(Unit.Percent(50), Unit.Percent(50));
            EndCenter = StartCenter;
        }
        
        private Unit GetStartingRadius(Point parentLocation, Size parentSize)
        {
            if (!StartRadius.HasValue)
            {
                StartRadius = 0d;
            }
            else if (StartRadius.Value.IsRelative)
            {
                //TODO: Convert to absolute
            }
            return StartRadius.Value;
        }
        
        private Unit GetEndingRadius(Point parentLocation, Size parentSize)
        {
            if (!EndRadius.HasValue)
            {
                if (this.Size == RadialSize.None)
                    this.Size = RadialSize.FarthestCorner;

                var pt = this.GetStartingCentre(parentLocation, parentSize);
                var x1 = pt.X.PointsValue;
                var y1 = pt.Y.PointsValue;
                var x2 = parentSize.Width.PointsValue - x1;
                var y2 = parentSize.Height.PointsValue - y1;
                double radius = 0.0;
                double h1, h2, h3, h4;
                
                switch (this.Size)
                {
                    case RadialSize.FarthestCorner:
                        h1 = Math.Sqrt(x1 * x1 + y1 * y1);
                        h2 = Math.Sqrt(x2 * x2 + y1 * y1);
                        h3 = Math.Sqrt(x2 * x2 + y2 * y2);
                        h4 = Math.Sqrt(x1 * x1 + y2 * y2);
                        radius = Math.Max(Math.Max(h1, h2), Math.Max(h3, h4));
                        break;
                    case RadialSize.FarthestSide:
                        radius = Math.Max(Math.Max(x1, y1), Math.Max(x2, y2));
                        break;
                    case RadialSize.ClosestSide:
                        radius = Math.Min(Math.Min(x1, y1), Math.Min(x2, y2));
                        break;
                    case RadialSize.ClosestCorner:
                    default:
                        h1 = Math.Sqrt(x1 * x1 + y1 * y1);
                        h2 = Math.Sqrt(x2 * x2 + y1 * y1);
                        h3 = Math.Sqrt(x2 * x2 + y2 * y2);
                        h4 = Math.Sqrt(x1 * x1 + y2 * y2);
                        radius = Math.Min(Math.Min(h1, h2), Math.Min(h3, h4));
                        break;
                }
                
                EndRadius = new Unit(radius);
            }
            else if (this.EndRadius.Value.IsRelative)
            {
                this.EndRadius = GetAbsoluteRadiusFromPercent(this.EndRadius.Value, parentLocation, parentSize);
            }
            
            return EndRadius.Value;
        }

        private static Unit GetAbsoluteRadiusFromPercent(Unit relativeRadius, Point parentLocation, Size parentSize)
        {
            if(relativeRadius.Units != PageUnits.Percent)
                throw new ArgumentOutOfRangeException("Can only calculate absolute radii from percent relative values.");
            
            var value = relativeRadius.Value / 100.0;
            var size = Math.Max(parentSize.Width.PointsValue, parentSize.Height.PointsValue);
            size *= value;
            
            return new Unit(size);
            
        }
        
        private Point GetStartingCentre(Point parentLocation, Size parentSize)
        {
            if (!StartCenter.HasValue)
            {
                StartCenter = new Point((parentSize.Width / 2) + parentLocation.X, (parentSize.Height / 2) + parentLocation.Y);
            }
            else if (StartCenter.Value.IsRelative)
            {
                var x = StartCenter.Value.X;
                var y = StartCenter.Value.Y;

                if (x.IsRelative)
                {
                    if (x.Units == PageUnits.Percent)
                        x = parentSize.Width * (x.Value * 0.01);
                    else
                        throw new NotSupportedException(
                            "The stating centre values must be a point value or a percentage of the final size");
                }
                
                if (y.IsRelative)
                {
                    if (y.Units == PageUnits.Percent)
                        y = parentSize.Height * (y.Value * 0.01);
                    else
                        throw new NotSupportedException(
                            "The stating centre values must be a point value or a percentage of the final size");
                }
                
                StartCenter = new Point(x, y);
            }
            
            return StartCenter.Value;
        }
        private Point GetEndingCentre(Point parentLocation, Size parentSize)
        {
            if (!EndCenter.HasValue)
            {
                //TODO: Convert from Top, Bottom, Left and Right
                EndCenter = new Point((parentSize.Width / 2) + parentLocation.X, (parentSize.Height / 2) + parentLocation.Y);
            }
            else if (EndCenter.Value.IsRelative)
            {
                var x = EndCenter.Value.X;
                var y = EndCenter.Value.Y;

                if (x.IsRelative)
                {
                    if (x.Units == PageUnits.Percent)
                        x = parentSize.Width * (x.Value * 0.01);
                    else
                        throw new NotSupportedException(
                            "The stating centre values must be a point value or a percentage of the final size");
                }
                
                if (y.IsRelative)
                {
                    if (y.Units == PageUnits.Percent)
                        y = parentSize.Height * (y.Value * 0.01);
                    else
                        throw new NotSupportedException(
                            "The stating centre values must be a point value or a percentage of the final size");
                }
                
                EndCenter = new Point(x, y);
            }
            
            return EndCenter.Value;
        }

        private static void EnsureNonRelative(Unit x, Unit y)
        {
            if (x.IsRelative)
                throw new ArgumentOutOfRangeException(nameof(x),
                    "The values for size and location of a radial gradients filled object cannot be relative dimensions");
            
            if (y.IsRelative)
                throw new ArgumentOutOfRangeException(nameof(y),
                    "The values for size and location of a radial gradients filled object cannot be relative dimensions");
        }
        
        public virtual double[] GetCoordsForBounds(Point location, Size size, bool flip = true )
        {
            EnsureNonRelative(location.X, location.Y); //just a quick check before we calculate the bounds
            EnsureNonRelative(size.Width, size.Height);
            
            var startRadius = GetStartingRadius(location, size);
            var endRadius = GetEndingRadius(location, size);
            var startCentre = GetStartingCentre(location, size);
            var endCentre = GetEndingCentre(location, size);
            
            double max = Math.Max(Math.Abs(size.Width.PointsValue), Math.Abs(size.Height.PointsValue));
            double[] coords = new double[6];
            coords[0] = location.X.PointsValue + startCentre.X.PointsValue;
            
            if (flip)
                coords[1] = location.Y.PointsValue + startCentre.Y.PointsValue;
            else
                coords[1] = location.Y.PointsValue + startCentre.Y.PointsValue;
            
            coords[2] = startRadius.PointsValue;


            coords[3] = location.X.PointsValue + endCentre.X.PointsValue;

            if (flip)
                coords[4] = location.Y.PointsValue + endCentre.Y.PointsValue;
            else
                coords[4] = location.Y.PointsValue + endCentre.Y.PointsValue;
            
            coords[5] = max * endRadius.PointsValue;
            
            return coords;
        }


        public static GradientRadialDescriptor ParseRadial(string value)
        {
            GradientRadialDescriptor desc;
            if(TryParseRadial(value, out desc))
                return desc;
            else
            {
                throw new ArgumentException($"Invalid radial value: {value}");
            }

        }
        
        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        public static bool TryParseRadial(string value, out GradientRadialDescriptor result)
        {
            result = null;
            
            value = value.Trim().ToLowerInvariant();
            
            if(value.StartsWith("radial-gradient"))
                value = value.Substring("radial-gradient".Length);
            
            value = value.Trim();

            if (value.StartsWith("("))
            {
                if(value.EndsWith(")"))
                    value = value.Substring(0, value.Length - 1);
                else
                {
                    return false;
                }
            }

            var all = _splitter.Split(value);

            RadialSize size;
            RadialShape shape;
            Point centre;
            List<GradientColor> colors;
            int colorOffset = 0;

            if (TryParseShape(all, colorOffset, out shape) && TryParseSize(all, colorOffset, out size) && TryParseCentre(all, colorOffset, out centre))
            {
                if (all[0].Length == 0)
                    colorOffset++;
                
                if (TryParseColors(all, colorOffset, out colors))
                {
                    result = new GradientRadialDescriptor(colors, shape, size);
                    result.StartCenter = centre;
                    result.EndCenter = centre;
                    return true;
                }
            }
            
            result = null;
            return false;
            
        }

        private static bool TryParseShape(string[] all, int offset, out RadialShape shape)
        {
            var one = all[offset];
            if (one.StartsWith("circle"))
            {
                shape = RadialShape.Circle;
                one = one.Substring("circle".Length).Trim();
            }
            else if (one.StartsWith("ellipse"))
            {
                shape = RadialShape.Ellipse;
                one = one.Substring("ellipse".Length).Trim();
            }
            else
            {
                shape = RadialShape.Circle;
            }

            all[offset] = one;
            return true;
        }

        private static bool TryParseSize(string[] all, int offset, out RadialSize size)
        {
            var one = all[offset];
            if (one.StartsWith("closest-side"))
            {
                size = RadialSize.ClosestSide;
                one = one.Substring("closest-side".Length).Trim();
            }
            else if (one.StartsWith("farthest-side"))
            {
                size = RadialSize.FarthestSide;
                one = one.Substring("farthest-side".Length).Trim();
            }
            else if (one.StartsWith("closest-corner"))
            {
                size = RadialSize.ClosestCorner;
                one = one.Substring("closest-corner".Length).Trim();
            }
            else if (one.StartsWith("farthest-corner"))
            {
                size = RadialSize.FarthestCorner;
                one = one.Substring("farthest-corner".Length).Trim();
            }
            else
            {
                size = RadialSize.FarthestSide;
            }
            
            all[offset] = one;

            return true;
        }

        private static bool TryParseCentre(string[] all, int offset, out Point centre)
        {
            var one = all[offset];
            Unit h = Unit.Percent(50);
            Unit v = Unit.Percent(50);
            
            if (one.StartsWith("at "))
            {
                one = one.Substring("at".Length).Trim();
                var dims = one.Split(' ');
                
                if (dims.Length > 0)
                {
                    bool vdone = false;
                    bool hdone = false;
                    
                    switch (dims[0])
                    {
                        case "top":
                            vdone = true;
                            v = Unit.Percent(0);
                            break;
                        case "bottom":
                            vdone = true;
                            v = Unit.Percent(100);
                            break;
                        case "left":
                            hdone = true;
                            h = Unit.Percent(0);
                            break;
                        case "right":
                            hdone = true;
                            h = Unit.Percent(100);
                            break;
                        default:
                            Unit.TryParse(dims[0], out h);
                            break;
                    }

                    if (dims.Length > 1)
                    {
                        //Could be nasty and throw on vdone or hdone already true - but keep it open and 50% is default.
                        switch (dims[1])
                        {
                            case "top":
                                vdone = true;
                                v = Unit.Percent(0);
                                break;
                            case "bottom":
                                vdone = true;
                                v = Unit.Percent(100);
                                break;
                            case "left":
                                hdone = true;
                                h = Unit.Percent(0);
                                break;
                            case "right":
                                hdone = true;
                                h = Unit.Percent(100);
                                break;
                            default:
                                Unit.TryParse(dims[1], out v);
                                break;
                        }
                    }
                    else
                        v = h;
                }

                one = ""; //should always be the last.
            }
            centre = new Point(h, v);
            all[offset] = one;
            
            return true;
        }

        private static bool TryParseColors(string[] all, int offset, out List<GradientColor> colors)
        {
            colors = new List<GradientColor>();
            bool hasAtleastOneDistance = false;
            
            for (var i = offset; i < all.Length; i++)
            {
                var one = all[i];
                
                if (GradientColor.TryParse(one, out var color))
                {
                    
                    if(i == offset && color.Distance.HasValue && color.Distance.Value > 0)
                        colors.Add(new GradientColor(color.Color, 0.0, color.Opacity));


                    if (color.Distance.HasValue)
                    {
                        color.Distance = color.Distance.Value / 100.0;
                        hasAtleastOneDistance = true;
                    }

                    colors.Add(color);
                    
                    if(i == all.Length - 1 && color.Distance.HasValue && color.Distance.Value < 1.0)
                        colors.Add(new GradientColor(color.Color, 1.0, color.Opacity));
                }
            }

            if (hasAtleastOneDistance)
            {
                FillMiddleDistances(colors);
            }
            return true;
        }

        
        
        
    }
}