using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Drawing
{
    public class PDFGradientRadialDescriptor : PDFGradientDescriptor
    {

        public RadialShape Shape { get; set; }

       
        public RadialSize Size { get; set; }

        public PDFUnit? XCentre { get; set; }

        public PDFUnit? YCentre { get; set; }


        private static Regex _splitter = new Regex(",(?![^\\(]*\\))");

        public PDFGradientRadialDescriptor() : base(GradientType.Radial)
        {

        }

        

        public static bool TryParseRadial(string value, out PDFGradientRadialDescriptor radial)
        {
            radial = null;
            string[] all = _splitter.Split(value);
            if (all.Length == 0)
                return false;

            RadialShape shape = RadialShape.Ellipse;
            RadialSize size = RadialSize.None;
            PDFUnit? xpos = null;
            PDFUnit? ypos = null;

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

            //TODO: Support explicit radii e.g. 150px 40px at ....


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
                            PDFUnit found;
                            if (PDFUnit.TryParse(item, out found))
                            {
                                if (xpos.HasValue)
                                    ypos = found;
                                else
                                    xpos = found;
                            }
                            break;
                    }
                }
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

            radial = new PDFGradientRadialDescriptor() { Repeating = false, Shape = shape, Size = size, XCentre = xpos, YCentre = ypos, Colors = colors };
            return true;
        }
    }
}
