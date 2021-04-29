using System;
using System.Linq;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber.Resources
{
    /// <summary>
    /// Defines a resource shading pattern for a linear gradient
    /// </summary>
    public class PDFRadialShadingPattern : PDFShadingPattern
    {
        private PDFGradientRadialDescriptor _descriptor;

        /// <summary>
        /// Gets the descriptior for the linear gradient
        /// </summary>
        public PDFGradientRadialDescriptor Descriptor
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
        public PDFRadialShadingPattern(IPDFComponent owner, string key, PDFGradientRadialDescriptor descriptor, PDFRect bounds)
            : base(owner, key, bounds)
        {
            this._descriptor = descriptor;
            
        }


        protected override PDFObjectRef DoRenderToPDF(PDFContextBase context, PDFWriter writer)
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

        protected virtual PDFObjectRef RenderShadingDictionary(PDFContextBase context, PDFWriter writer)
        {
            PDFPoint offset = new PDFPoint(this.Start.X, this.Start.Y);// this.Start;
            PDFSize size = this.Size;

            PDFSize graphicsSize = new PDFSize(size.Width + offset.X, size.Height + offset.Y);
            var coords = GetCoords(offset, size, _descriptor.Size, _descriptor.XCentre, _descriptor.YCentre);
            var func = this._descriptor.GetGradientFunction();

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


        protected virtual double[] GetCoords(PDFPoint offset, PDFSize size, RadialSize radiusSize, PDFUnit? centreX, PDFUnit? centreY)
        {

            double[] all = new double[6];

            var height = Math.Abs(size.Height.PointsValue);
            var width = size.Width.PointsValue;

            //default centre and radii
            var rX = width / 2;
            var rY = height / 2;

            var cX = rX;
            var cY = rY;

            //calculate any explicit centres from the bbox origin of the rectangle.
            if(centreX.HasValue)
            {
                if (centreX.Value == PDFUnit.Zero)
                    cX = 0.0;
                else if (centreX.Value.PointsValue == double.MaxValue)
                    cX = width;
                else
                    cX = centreX.Value.PointsValue;
            }

            if(centreY.HasValue)
            {
                if (centreY.Value == PDFUnit.Zero)
                    cY = 0.0;
                else if (centreX.Value.PointsValue == double.MaxValue)
                    cY = height;
                else
                    cY = centreY.Value.PointsValue;
            }

            double minx, miny, maxX, maxY, radius;

            switch (radiusSize)
            {
                case (RadialSize.ClosestCorner):
                    minx = Math.Min(cX, width - cX);
                    miny = Math.Min(cY, height - cY);
                    radius = Math.Sqrt((minx * minx) + (miny * miny));
                    break;
                case (RadialSize.FarthestCorner):
                    maxX = Math.Max(cX, width - cX);
                    maxY = Math.Max(cY, height - cY);
                    radius = Math.Sqrt((maxX * maxX) + (maxY * maxY));
                    break;
                case (RadialSize.ClosestSide):
                    minx = Math.Min(cX, width - cX);
                    miny = Math.Min(cY, height - cY);
                    radius = Math.Min(minx, miny);
                    break;
                case (RadialSize.FarthestSide):
                case (RadialSize.None):
                default:
                    maxX = Math.Max(cX, width - cX);
                    maxY = Math.Max(cY, height - cY);
                    radius = Math.Max(maxX, maxY);
                    break;
            }

            if (radius <= 0)
                radius = 0.01;
            
            //apply the centres based on the page location
            all[0] = cX + offset.X.PointsValue;
            all[1] = offset.Y.PointsValue - cY;
            all[2] = 0.0;
            all[3] = cX + offset.X.PointsValue;
            all[4] = offset.Y.PointsValue - cY;
            all[5] = radius;

            

            return all;
        }
    }
}
