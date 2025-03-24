using System;
using Scryber.Drawing;

using Scryber.PDF.Graphics;

namespace Scryber.Svg
{
    [PDFParsableValue()]
    public abstract class SVGFillValue
    {

        public string Value
        {
            get;
            protected set;
        }

        public SVGFillType FillType
        {
            get;
            protected set;
        }

        public SVGFillValue(SVGFillType type, string value)
        {
            this.Value = value;
            this.FillType = type;
        }

        public abstract PDFBrush GetBrush(double opacity);

        public static SVGFillValue Parse(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                Color parsed;
                if (value.StartsWith("url(", StringComparison.InvariantCultureIgnoreCase) && value.EndsWith(")", StringComparison.InvariantCultureIgnoreCase))
                {
                    value = value.Substring(4, value.Length - 5);
                    return new SVGFillReferenceValue(null, value);
                }
                else if (IsNamedValue(value, out PDFBrush brush ))
                {
                    return new SVGFillNamedValue(brush, value);
                }
                else if(Color.TryParse(value, out parsed))
                {
                    return new SVGFillColorValue(parsed, value);
                }
            }

            return null;
        }
        
        public static bool IsNamedValue(string value, out PDFBrush brush)
        {
            switch (value)
            {
                case ("none"):
                case("transparent"):
                    brush = null;
                    return true;
                case ("context-fill"):
                case ("context-stroke"):
                    //not supported, but we know the value
                    brush = null;
                    return true;
                default:
                    brush = null;
                    return false;
            }
        }
    }

    public enum SVGFillType
    {
        Color,
        Named,
        Reference
    }

    public class SVGFillColorValue : SVGFillValue
    {

        public Color FillColor { get; protected set; }
        
        public SVGFillColorValue(Color fillColor) : this(fillColor, fillColor.ToString()) { }
        
        public SVGFillColorValue(Color color, string value)
        : base(SVGFillType.Color, value)
        {
            this.FillColor = color;
        }

        public override PDFBrush GetBrush(double opacity)
        {
            return new PDFSolidBrush(this.FillColor, opacity);
        }

        public static SVGFillValue Black
        {
            get => new SVGFillColorValue(StandardColors.Black, "black"); 
        }
    }

    public class SVGFillNamedValue : SVGFillValue
    {
        public IPDFGraphicsAdapter Adapter { get; set; }

        public SVGFillNamedValue(IPDFGraphicsAdapter adapter, string name) :
            base(SVGFillType.Named, name)
        {
            this.Adapter = adapter;
        }

        public override PDFBrush GetBrush(double opacity)
        {
            return this.Adapter as PDFBrush;
        }
    }

    public class SVGFillReferenceValue : SVGFillValue
    {
        public IPDFGraphicsAdapter Adapter { get; set; }

        public SVGFillReferenceValue(string reference)
            : this(null, reference)
        {
        }

        public SVGFillReferenceValue(IPDFGraphicsAdapter adaptor, string reference) 
            : base(SVGFillType.Reference, reference)
        {
            this.Adapter = adaptor;
        }

        public override PDFBrush GetBrush(double opacity)
        {
            return this.Adapter as PDFBrush;
            
        }
    }
}