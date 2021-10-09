using System;
using System.Linq;
using Scryber.Drawing;
using Scryber.PDF.Native;

namespace Scryber.PDF.Resources
{
    /// <summary>
    /// Defines a resource shading pattern for a linear gradient
    /// </summary>
    public class PDFLinearShadingPattern : PDFShadingPattern
    {
        private GradientLinearDescriptor _descriptor;

        /// <summary>
        /// Gets the descriptior for the linear gradient
        /// </summary>
        public GradientLinearDescriptor Descriptor
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
        public PDFLinearShadingPattern(IComponent owner, string key, GradientLinearDescriptor descriptor, PDFRect bounds)
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
            writer.BeginDictionaryEntry("Shading");
            writer.BeginDictionary();
            writer.BeginDictionaryEntry("ShadingType");
            writer.WriteNumberS((int)ShadingType.Axial);
            writer.EndDictionaryEntry();
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceRGB");

            writer.BeginDictionaryEntry("BBox");

            PDFPoint offset = new PDFPoint(this.Start.X, this.Start.Y);// this.Start;
            PDFSize size = this.Size;

            PDFSize graphicsSize = new PDFSize(size.Width + offset.X, size.Height + offset.Y);

            writer.WriteArrayRealEntries(true, offset.X.PointsValue,
                                               offset.Y.PointsValue,
                                               offset.X.PointsValue + size.Width.PointsValue,
                                               offset.Y.PointsValue + size.Height.PointsValue);

            writer.EndDictionaryEntry();
            writer.WriteDictionaryBooleanEntry("AntiAlias", true);

            writer.BeginDictionaryEntry("Coords");
            var coords = GetCoords(offset, size, this._descriptor.Angle);

            writer.WriteArrayRealEntries(true, coords);

            var func = this._descriptor.GetGradientFunction(offset, size);
            if (null != func)
            {
                writer.BeginDictionaryEntry("Function");
                func.WriteFunctionDictionary(context, writer);
                writer.EndDictionaryEntry();
            }

            writer.EndDictionary();//shading
            return null;
        }

        

        protected virtual double[] GetCoords(PDFPoint offset, PDFSize size, double angle)
        {

            double[] all = new double[4];
            
            all[0] = offset.X.PointsValue;
            all[1] = offset.Y.PointsValue;
            all[2] = offset.X.PointsValue;
            all[3] = offset.Y.PointsValue;

            //TODO: Change this to support any angle with sin, cos and tan

            if(angle < 45) // Top
            {
                all[1] += size.Height.PointsValue;
            }
            else if(angle < 90) //Top Right
            {
                all[2] += size.Width.PointsValue;
                all[1] += size.Height.PointsValue;
            }
            else if(angle < 135) //Right
            {
                all[2] += size.Width.PointsValue;
            }
            else if(angle < 180) //Bottom Right
            {
                all[2] += size.Width.PointsValue;
                all[3] += size.Height.PointsValue;
            }
            else if(angle < 225) //Bottom
            {
                all[3] += size.Height.PointsValue;
            }
            else if(angle < 270) //Bottom Left
            {
                all[0] += size.Width.PointsValue;
                all[3] += size.Height.PointsValue;
            }
            else if(angle < 315) //Left
            {
                all[0] += size.Width.PointsValue;
            }
            else if(angle < 360) //Top Left
            {
                all[0] += size.Width.PointsValue;
                all[1] += size.Height.PointsValue;
            }
            else
            {
                all[3] += size.Height.PointsValue;
            }
            
            return all;
        }
    }
}
