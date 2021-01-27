using System;
using Scryber.Drawing;
using Scryber.Styles;


namespace Scryber.Svg.Components
{
    [PDFParsableValue()]
    public struct SVGAspectRatio
    {

        public static readonly PDFStyleKey<SVGAspectRatio> AspectRatioStyleKey = StyleKey.CreateStyleValueKey<SVGAspectRatio>((PDFObjectType)"SVar", StyleKeys.PositionItemKey);

        public AspectRatioAlign Align { get; set; }

        public AspectRatioMeet Meet { get; set; }

        public SVGAspectRatio(AspectRatioAlign align) : this(align, AspectRatioMeet.None)
        {
        }

        public SVGAspectRatio(AspectRatioAlign align, AspectRatioMeet meet)
        {
            this.Align = align;
            this.Meet = meet;
        }


        public override string ToString()
        {
            if (this.Align == AspectRatioAlign.None && this.Meet == AspectRatioMeet.None)
                return "none";
            else if(this.Meet == AspectRatioMeet.None)
            {
                return this.Align.ToString();
            }
            else
            {
                return this.Align.ToString() + " " + this.Meet.ToString().ToLower();
            }
        }

        public static SVGAspectRatio Default
        {
            get { return new SVGAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet); }
        }

        public static SVGAspectRatio None
        {
            get { return new SVGAspectRatio(AspectRatioAlign.None); }
        }

        public static SVGAspectRatio Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new SVGAspectRatio(AspectRatioAlign.None);
            }
            else
            {
                value = value.Trim();
                AspectRatioAlign align;
                AspectRatioMeet meet;

                if (value.IndexOf(" ") > 0)
                {
                    var each = value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    var one = each[0];
                    var two = each[1]; //safe with this, as we have definitely got at least 2

                    if (Enum.TryParse(one, true, out align) && Enum.TryParse(two, true, out meet))
                        return new SVGAspectRatio(align, meet);
                    else
                        throw new PDFParserException("Could not parse the value '" + value + "' into an SVGAspectRatio value. Please constult the documentation on supported value and format");
                }
                else if(Enum.TryParse(value, true, out align))
                {
                    return new SVGAspectRatio(align);
                }
                else
                    throw new PDFParserException("Could not parse the value '" + value + "' into an SVGAspectRatio value. Please constult the documentation on supported value and format");
            }
        }

        //
        // scaling calculations
        //

        public static void ApplyMaxNonUniformScaling(PDFTransformationMatrix onmatrix, PDFSize dest, PDFRect viewport)
        {
            PDFSize source = viewport.Size;
            double scalex =  dest.Width.PointsValue / source.Width.PointsValue;
            double scaley =  dest.Height.PointsValue / source.Height.PointsValue;
            double offx = 0; // viewport.X.PointsValue;
            double offy = 0; // viewport.Y.PointsValue;

            onmatrix.SetTranslation(offx, offy);
            onmatrix.SetScale((float)scalex, (float)scaley);
        }

        public static void ApplyUniformScaling(PDFTransformationMatrix onmatrix, PDFSize dest, PDFRect viewport, AspectRatioAlign align)
        {
            PDFSize source = viewport.Size;
            double scalex = dest.Width.PointsValue / source.Width.PointsValue;
            double scaley = dest.Height.PointsValue / source.Height.PointsValue;
            double offx = 0; // viewport.X.PointsValue;
            double offy = 0; // viewport.Y.PointsValue;
            float min = (float)Math.Min(scalex, scaley);
            switch (align)
            {
                case (AspectRatioAlign.xMinYMin):
                    //Scale to fit within the size without overflow and show at the top (so no x or y offset)
                    onmatrix.SetScale(min, min);
                    break;
                case (AspectRatioAlign.xMidYMid):
                    //Scale to fit within the size without overflow and then position in the middle
                    onmatrix.SetScale(min, min);
                    break;
                default:
                    break;
            }

            //onmatrix.SetTranslation(offx, offy);
            //onmatrix.SetScale((float)scalex, (float)scaley);

        }

        public static void ApplyUniformStretching(PDFTransformationMatrix onmatrix, PDFSize destm, PDFRect viewport, AspectRatioAlign align)
        {

        }
    }
}
