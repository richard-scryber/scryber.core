using System;
namespace Scryber.Drawing
{
    /// <summary>
    /// Defines a single gradient color value with opacity and distance that can be used in a gradient
    /// </summary>
    public class PDFGradientColor
    {
        [Obsolete("PDF does not support opacity in gradients but retaining incase we can make it work", false)]
        public double? Opacity { get; set; }

        /// <summary>
        /// The offset of the color from the start (10%, 30% etc).
        /// </summary>
        public double? Distance { get; set; }


        /// <summary>
        /// The actual color value to be used in the gradient
        /// </summary>
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
            else if (value.StartsWith("rgb(")) //We have rbg and maybe a distance
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
            else if (value.IndexOf(" ") > 0) //we have a named or hex color and a distance
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
            else if (PDFColor.TryParse(value, out colVal)) //we just have a named or hex color
            {
                color = new PDFGradientColor(colVal, distance, opacity);
                return true;
            }
            else //not recognised
                return false;
        }
    }
}
