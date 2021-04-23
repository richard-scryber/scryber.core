using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    public abstract class PDFGradientDescriptor
    {

        public GradientType GradientType { get; protected set; }

        public PDFGradientColor[] Colors
        {
            get;
            set;
        }

        public bool Repeating { get; set; }

        public PDFGradientDescriptor(GradientType type)
        {
            this.GradientType = type;
        }


        public abstract PDFGradientFunction GetGradientFunction();

        public static PDFGradientDescriptor Parse(string value)
        {
            PDFGradientDescriptor desc;
            if (TryParse(value, out desc))
                return desc;
            else
                throw new InvalidOperationException("The gradient descriptor '" + value + "' could not be parsed");
        }

        public static bool TryParse(string value, out PDFGradientDescriptor descriptor)
        {
            descriptor = null;

            if (null == value)
                return false;

            value = value.Trim();


            if (string.IsNullOrEmpty(value))
                return false;

            else if (value.StartsWith("linear-gradient"))
            {
                value = value.Substring("linear-gradient".Length).Trim();
                PDFLinearGradientDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFLinearGradientDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = false;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("repeating-linear-gradient"))
            {
                value = value.Substring("repeating-linear-gradient".Length).Trim();
                PDFLinearGradientDescriptor linear;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFLinearGradientDescriptor.TryParseLinear(value.Substring(1, value.Length - 2), out linear))
                {
                    linear.Repeating = true;
                    descriptor = linear;
                    return true;
                }
            }
            else if(value.StartsWith("radial-gradient"))
            {
                value = value.Substring("radial-gradient".Length).Trim();
                PDFRadialGradientDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFRadialGradientDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
                {
                    radial.Repeating = false;
                    descriptor = radial;
                    return true;
                }
            }
            else if (value.StartsWith("repeating-radial-gradient"))
            {
                value = value.Substring("repeating-radial-gradient".Length).Trim();
                PDFRadialGradientDescriptor radial;

                if (value.StartsWith("(") && value.EndsWith(")") && PDFRadialGradientDescriptor.TryParseRadial(value.Substring(1, value.Length - 2), out radial))
                {
                    radial.Repeating = true;
                    descriptor = radial;
                    return true;
                }
            }

            return false;
        }

        

    }

    public enum GradientType
    {
        Linear,
        Radial
    }

    public enum GradientAngle
    {
        Top = 0,
        Left = 270,
        Bottom = 180,
        Right = 90,
        Top_Left = 315,
        Top_Right = 45,
        Bottom_Left = 225,
        Bottom_Right = 135
    }

    public enum RadialShape
    {
        Ellipse,
        Circle
    }

    public enum RadialSize
    {
        None,
        ClosestSide,
        FarthestSide,
        ClosestCorner,
        FarthestCorner
    }

    public class PDFLinearGradientDescriptor : PDFGradientDescriptor
    {
        
        public double Angle { get; set; }

        public PDFLinearGradientDescriptor() : base(GradientType.Linear)
        { }


        public override PDFGradientFunction GetGradientFunction()
        {
            if (this.Repeating)
            {
                List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();

                double total = 0.0;
                while (total < 1.0)
                {
                    double curr = 0.0;
                    for (int i = 1; i < this.Colors.Length; i++)
                    {
                        var col = this.Colors[i];
                        curr = col.Distance.HasValue ? total + (col.Distance.Value / 100) : total;
                        bounds.Add(new PDFGradientFunctionBoundary(curr));
                    }
                    total = curr;
                }
                List<PDFGradientLinearFunction2> functions = new List<PDFGradientLinearFunction2>();

                var col0Index = 0;
                var col1Index = 1;
                for (int i = 0; i < bounds.Count; i++)
                {
                    var color0 = this.Colors[col0Index].Color;
                    var color1 = this.Colors[col1Index].Color;

                    var func = new PDFGradientLinearFunction2(color0, color1);
                    functions.Add(func);

                    col0Index++;
                    col1Index++;

                    if(col1Index >= this.Colors.Length)
                    {
                        col0Index = 0;
                        col1Index = 1;
                    }
                    //if (col0Index >= this.Colors.Length)
                    //    col0Index = 0;
                    //else if (col1Index >= this.Colors.Length)
                    //    col1Index = 0;
                }

                bounds.RemoveAt(bounds.Count - 1);

                while (bounds[bounds.Count - 1].Bounds > 1)
                {
                    bounds.RemoveAt(bounds.Count - 1);
                    functions.RemoveAt(functions.Count - 1);
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
            else if (this.Colors.Length == 2)
            {
                return new PDFGradientLinearFunction2(this.Colors[0].Color, this.Colors[1].Color);
            }
            else
            {
                List<PDFGradientLinearFunction2> functions = new List<PDFGradientLinearFunction2>();
                List<PDFGradientFunctionBoundary> bounds = new List<PDFGradientFunctionBoundary>();
                double boundsValue = 1.0 / (this.Colors.Length - 1);

                for (int i = 1; i < this.Colors.Length; i++)
                {
                    functions.Add(new PDFGradientLinearFunction2(this.Colors[i - 1].Color, this.Colors[i].Color));

                    if (i < this.Colors.Length - 1)
                    {
                        bounds.Add(new PDFGradientFunctionBoundary(boundsValue * i));
                    }
                }

                return new PDFGradientFunction3(functions.ToArray(), bounds.ToArray());
            }
        }

        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        /// <summary>
        /// Parses a linear gradient from a string without decorations e.g. "to top right, red, green
        /// </summary>
        /// <param name="value"></param>
        /// <param name="linear"></param>
        /// <returns></returns>
        public static bool TryParseLinear(string value, out PDFLinearGradientDescriptor linear)
        {
            linear = null;
            string[] all = _splitter.Split(value);
            if (all.Length == 0)
                return false;

            int colorStopIndex = 0;
            double angle;

            if (all[0].StartsWith("to "))
            {
                var ga = all[0].Substring(3).Trim().Replace(" ", "_");
                GradientAngle parsed;
                if (Enum.TryParse(ga, true, out parsed))
                    angle = (double)parsed;
                else
                    return false;
                colorStopIndex = 1;
            }
            else if (char.IsNumber(all[0], 0))
            {
                var deg = all[0];

                if (deg.EndsWith("deg"))
                    deg = deg.Substring(0, deg.Length - 3);

                if (!double.TryParse(deg, out angle))
                    return false;

                colorStopIndex = 1;
            }
            else
                angle = (double)GradientAngle.Bottom;

            PDFGradientColor[] colors = new PDFGradientColor[all.Length - colorStopIndex];

            for (int i = 0; i < colors.Length; i++)
            {
                PDFGradientColor parsed;
                if (PDFGradientColor.TryParse(all[i + colorStopIndex], out parsed))
                    colors[i] = parsed;
                else
                    return false;
            }

            linear = new PDFLinearGradientDescriptor() { Angle = angle, Colors = colors };
            return true;
        }

        

    }

    public class PDFRadialGradientDescriptor : PDFGradientDescriptor
    {
        
        public RadialShape Shape { get; set; }

        [Obsolete("Not currently used", true)]
        public RadialSize Size { get; set; }

        [Obsolete("Not currently used", true)]
        public double[] SizeOffsets { get; set; }


        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        public PDFRadialGradientDescriptor() : base(GradientType.Radial)
        {

        }

        public override PDFGradientFunction GetGradientFunction()
        {
            throw new NotImplementedException();
        }

        public static bool TryParseRadial(string value, out PDFRadialGradientDescriptor radial)
        {
            radial = null;
            string[] all = _splitter.Split(value);
            if (all.Length == 0)
                return false;

            RadialShape shape = RadialShape.Ellipse;
            RadialSize size = RadialSize.None;

            int colorStopIndex = 0;


            if (all[0].StartsWith("circle"))
            {
                shape = RadialShape.Circle;
                all[0] = all[0].Substring("circle".Length).TrimStart();
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("ellipse"))
            {
                shape = RadialShape.Ellipse;
                all[0] = all[0].Substring("ellipse".Length).TrimStart();
                colorStopIndex = 1;
            }

            if(all[0].StartsWith("closest-side"))
            {
                throw new NotSupportedException("Sides are not supported at the moment");
                //TODO:Parse at percents
                size = RadialSize.ClosestSide;
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("closest-corner"))
            {
                throw new NotSupportedException("Sides are not supported at the moment");
                size = RadialSize.ClosestCorner;
                colorStopIndex = 1;
            }
            else if(all[0].StartsWith("farthest-side"))
            {
                throw new NotSupportedException("Sides are not supported at the moment");
                size = RadialSize.FarthestSide;
                colorStopIndex = 1;
            }
            else if (all[0].StartsWith("farthest-corner"))
            {
                throw new NotSupportedException("Sides are not supported at the moment");
                size = RadialSize.FarthestCorner;
                colorStopIndex = 1;
            }

            PDFGradientColor[] colors = new PDFGradientColor[all.Length - colorStopIndex];

            for (int i = 0; i < colors.Length; i++)
            {
                PDFGradientColor parsed;
                if (PDFGradientColor.TryParse(all[i + colorStopIndex], out parsed))
                    colors[i] = parsed;
                else
                    return false;
            }

            radial = new PDFRadialGradientDescriptor() { Repeating = false, Shape = shape, Colors = colors };
            return true;
        }
    }


    /// <summary>
    /// Defines a single gradient color value with opacity and distance
    /// </summary>
    public class PDFGradientColor
    {
        
        public double? Opacity { get; set; }

        public double? Distance { get; set; }

        public PDFColor Color { get; set; }

        
        public PDFGradientColor(PDFColor color, double? distance, double? opacity)
        {
            this.Color = color;
            this.Distance = distance;
            this.Opacity = opacity;
        }

        public static bool TryParse(string value, out PDFGradientColor color)
        {
            color = null;
            if (null == value)
                return false;

            value = value.Trim();
            PDFColor colVal = PDFColor.Transparent;
            double? opacity = null;
            double? distance = null;

            if (value.StartsWith("rgba(")) //We have opacity
            {
                var end = value.IndexOf(")");
                if (end < 0)
                    return false;
                else if (end >= value.Length - 1)
                {
                    if (!PDFColor.TryParseRGBA(value, out colVal, out opacity))
                        return false;
                }
                else
                {
                    var colS = value.Substring(0, end + 1);
                    if (!PDFColor.TryParseRGBA(value, out colVal, out opacity))
                        return false;

                    value = value.Substring(end + 1).Trim();
                    if (value.EndsWith("%"))
                        value = value.Substring(0, value.Length - 1);
                    double distV;
                    if (double.TryParse(value, out distV))
                        distance = distV;
                }
                color = new PDFGradientColor(colVal, distance, opacity);
                return true;
            }
            else if(value.StartsWith("rgb(")) //We have rbg and maybe a distance
            {
                var end = value.IndexOf(")");
                if (end == value.Length - 1)
                {
                    if (!PDFColor.TryParse(value, out colVal))
                        return false;
                }
                else
                {
                    var colS = value.Substring(0, end);
                    value = value.Substring(end).Trim();
                    if (value.EndsWith("%"))
                        value = value.Substring(0, value.Length - 1);
                    double distV;
                    if (double.TryParse(value, out distV))
                        distance = distV;
                }
                color = new PDFGradientColor(colVal, distance, opacity);
                return true;
            }
            else if(value.IndexOf(" ") > 0) //we have a named or hex color and a distance
            {
                var colS = value.Substring(0, value.IndexOf(" ")).Trim();
                value = value.Substring(value.IndexOf(" ")).Trim();
                if (!PDFColor.TryParse(colS, out colVal))
                    return false;

                if (value.EndsWith("%"))
                    value = value.Substring(0, value.Length - 1);
                double distV;
                if (double.TryParse(value, out distV))
                    distance = distV;

                color = new PDFGradientColor(colVal, distance, opacity);
                return true;
            }
            else if(PDFColor.TryParse(value, out colVal)) //we just have a named or hex color
            {
                color = new PDFGradientColor(colVal, distance, opacity);
                return true;
            }
            else //not recognised
                return false;
        }
    }

}
