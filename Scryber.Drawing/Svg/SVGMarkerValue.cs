using System;
using Scryber.PDF.Graphics;

namespace Scryber.Svg
{

    /// <summary>
    /// Defines the marker for an attribute on an SVG drawing component
    /// </summary>
    [PDFParsableValue]
    public struct SVGMarkerValue
    {
        public string MarkerReference { get; set; }



        public SVGMarkerValue(string reference)
        {
            this.MarkerReference = reference;
        }

        public static SVGMarkerValue Empty
        {
            get { return new SVGMarkerValue(string.Empty); }
        }


        public static SVGMarkerValue Parse(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {

                if (value.StartsWith("url(", StringComparison.InvariantCultureIgnoreCase) &&
                    value.EndsWith(")", StringComparison.InvariantCultureIgnoreCase))
                {
                    value = value.Substring(4, value.Length - 5);
                    return new SVGMarkerValue(value);
                }
                else
                {
                    throw new ArgumentException("The value for a marker must be in the format 'url(...)'");
                }
            }

            return SVGMarkerValue.Empty;
        }

    }
}