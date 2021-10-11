using System;
using System.Linq;
using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Defines a resource shading pattern for a linear gradient
    /// </summary>
    public class PDFRadialShadingPattern : PDFShadingPattern
    {
        private GradientRadialDescriptor _descriptor;

        /// <summary>
        /// Gets the descriptior for the linear gradient
        /// </summary>
        public GradientRadialDescriptor Descriptor
        {
            get { return _descriptor; }
        }
        
        /// <summary>
        /// Creates a new Linear gradient shading pattern with the specified key and gradient descriptor that will be renedered
        /// in the page bounds.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="key"></param>
        /// <param name="descriptor">The gradient descriptor</param>
        /// <param name="bounds">The bounds of the gradient on the page (rather than component level)</param>
        public PDFRadialShadingPattern(IComponent owner, string key, GradientRadialDescriptor descriptor, Rect bounds)
            : base(owner, key, bounds)
        {
            this._descriptor = descriptor;
            
        }


        protected override PDFObjectRef DoRenderToPDF(ContextBase context, PDFWriter writer)
        {
            PDFObjectRef oref = writer.BeginObject(this.Name.Value);
            writer.BeginDictionary();
            writer.WriteDictionaryNameEntry("Type", "Pattern");
            writer.WriteDictionaryNumberEntry("PatternType", (int)this.PatternType);
            //Actual shading dictionary
            var shading = this.RenderShadingDictionary(context, writer);
            if (null != shading)
                writer.WriteDictionaryObjectRefEntry("Shading", shading);

            writer.EndDictionary();
            writer.EndObject();

            return oref;

        }

        protected virtual PDFObjectRef RenderShadingDictionary(ContextBase context, PDFWriter writer)
        {
            Point offset = new Point(this.Start.X, this.Start.Y);// this.Start;
            Size size = this.Size;

            Size graphicsSize = new Size(size.Width + offset.X, size.Height + offset.Y);
            var func = this._descriptor.GetGradientFunction(offset, size);
            var coords = GetCoords(offset, size, _descriptor.Size, _descriptor.XCentre, _descriptor.YCentre);
            

            writer.BeginDictionaryEntry("Shading");
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("ShadingType", (int)ShadingType.Radial);
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceRGB");
            writer.WriteDictionaryBooleanEntry("AntiAlias", true);

            writer.BeginDictionaryEntry("BBox");
            writer.WriteArrayRealEntries(true, offset.X.PointsValue,
                                               offset.Y.PointsValue,
                                               offset.X.PointsValue + size.Width.PointsValue,
                                               offset.Y.PointsValue + size.Height.PointsValue);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("Coords");
            writer.WriteArrayRealEntries(true, coords);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("Extend");
            writer.BeginArray();

            writer.BeginArrayEntry();
            writer.WriteBooleanS(true);
            writer.EndArrayEntry();

            writer.BeginArrayEntry();
            writer.WriteBooleanS(true);
            writer.EndArrayEntry();

            writer.EndArray();
            writer.EndDictionaryEntry();

            if (null != func)
            {
                writer.BeginDictionaryEntry("Function");
                func.WriteFunctionDictionary(context, writer);
                writer.EndDictionaryEntry();
            }


            writer.EndDictionary();//shading
            return null;
        }


        protected virtual double[] GetCoords(Point offset, Size size, RadialSize radiusSize, Unit? centreX, Unit? centreY)
        {

            double[] all = new double[6];

            var height = Math.Abs(size.Height.PointsValue);
            var width = size.Width.PointsValue;

            //Get the centre and the radius
            Point c = CacluateRadialCentre(centreX, centreY, height, width);
            double radius = CalculateRadiusForSize(radiusSize, height, width, c.X.PointsValue, c.Y.PointsValue);

            if (radius <= 0)
                radius = 0.01;

            //apply the centres based on the page location
            all[0] = c.X.PointsValue + offset.X.PointsValue;
            all[1] = offset.Y.PointsValue - c.Y.PointsValue;
            all[2] = 0.0;
            all[3] = c.X.PointsValue + offset.X.PointsValue;
            all[4] = offset.Y.PointsValue - c.Y.PointsValue;
            all[5] = radius;



            return all;
        }

        public static Point CacluateRadialCentre(Unit? centreX, Unit? centreY, double height, double width)
        {
            var rX = width / 2;
            var rY = height / 2;
            Point c = new Point(rX, rY);

            //calculate any explicit centres from the bbox origin of the rectangle.
            if (centreX.HasValue)
            {
                if (centreX.Value == Unit.Zero)
                    c.X = 0.0;
                else if (centreX.Value.PointsValue == double.MaxValue)
                    c.X = width;
                else
                    c.X = centreX.Value.PointsValue;
            }

            if (centreY.HasValue)
            {
                if (centreY.Value == Unit.Zero)
                    c.Y = 0.0;
                else if (centreX.Value.PointsValue == double.MaxValue)
                    c.Y = height;
                else
                    c.Y = centreY.Value.PointsValue;
            }

            return c;
        }

        public static double CalculateRadiusForSize(RadialSize radiusSize, double height, double width, double centreX, double centreY)
        {
            double minx, miny, maxX, maxY, radius;

            switch (radiusSize)
            {
                case (RadialSize.ClosestCorner):
                    minx = Math.Min(centreX, width - centreX);
                    miny = Math.Min(centreY, height - centreY);
                    radius = Math.Sqrt((minx * minx) + (miny * miny));
                    break;
                case (RadialSize.FarthestCorner):
                    maxX = Math.Max(centreX, width - centreX);
                    maxY = Math.Max(centreY, height - centreY);
                    radius = Math.Sqrt((maxX * maxX) + (maxY * maxY));
                    break;
                case (RadialSize.ClosestSide):
                    minx = Math.Min(centreX, width - centreX);
                    miny = Math.Min(centreY, height - centreY);
                    radius = Math.Min(minx, miny);
                    break;
                case (RadialSize.FarthestSide):
                case (RadialSize.None):
                default:
                    maxX = Math.Max(centreX, width - centreX);
                    maxY = Math.Max(centreY, height - centreY);
                    radius = Math.Max(maxX, maxY);
                    break;
            }

            return radius;
        }
    }
}
