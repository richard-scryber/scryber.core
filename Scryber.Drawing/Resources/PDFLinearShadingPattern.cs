using System;
using System.Linq;
using Scryber.Drawing;
using Scryber.Native;

namespace Scryber.Resources
{
    public class PDFLinearShadingPattern : PDFShadingPattern
    {
        private PDFLinearGradientDescriptor _descriptor;


        

        public PDFLinearShadingPattern(IPDFComponent owner, string key, PDFLinearGradientDescriptor descriptor, PDFRect bounds)
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

            var func = this._descriptor.GetGradientFunction();
            if (null != func)
            {
                writer.BeginDictionaryEntry("Function");
                func.WriteFunctionDictionary(context, writer);
                writer.EndDictionaryEntry();
            }

            /* 
             if (this._descriptor.Colors.Length == 2)
                this.WriteColorFunction2(this._descriptor.Colors[0].Color, this._descriptor.Colors[1].Color, 0, 1, writer);
             else
                this.WriteColorFunction3(this._descriptor.Colors, writer);

               writer.EndDictionaryEntry();
            */

            writer.EndDictionary();//shading
            return null;
        }

        protected virtual void WriteColorFunction3(PDFGradientColor[] colors, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("FunctionType", 3);

            writer.BeginDictionaryEntry("Domain");
            writer.WriteArrayRealEntries(0.0, 1.0);
            writer.EndDictionaryEntry();

            int entries = (colors.Length - 1);
            double bounds = 1.0 / (double)entries;

            //The bounds is the function extents of the functions
            writer.BeginDictionaryEntry("Bounds");
            writer.BeginArray();
            for (int i = 1; i < entries; i++)
            {
                writer.BeginArrayEntry();
                writer.WriteRealS(bounds);
                writer.EndArrayEntry();
                bounds += bounds;
            }
            writer.EndArray();
            writer.EndDictionaryEntry();

            //Write the array of function 2 (Axial aka Linear between 2 colours)
            
            writer.BeginDictionaryEntry("Functions");
            writer.BeginArray();
            //Start the for loop at 1 and use the previous colour as the first
            for (int i = 1; i < colors.Length; i++)
            {
                writer.BeginArrayEntry();
                this.WriteColorFunction2(colors[i - 1].Color, colors[i].Color, 0, 1, writer);
                writer.EndArrayEntry();
            }

            writer.EndArray();
            writer.EndDictionaryEntry();

            //Write the encodes for each of the functions 0 1 in a single array
            writer.BeginDictionaryEntry("Encode");
            writer.BeginArray();

            for (int i = 1; i < colors.Length; i++)
            {
                //Don't create an array each time, simply use the encoding
                writer.WriteArrayNumberEntries(false, 0, 1);
            }

            writer.EndArray();

            writer.EndDictionaryEntry();
            writer.EndDictionary();


        }

        protected virtual void WriteColorFunction2(PDFColor color1, PDFColor color2, double domainStart, double domainEnd, PDFWriter writer)
        {
            writer.BeginDictionary();
            writer.WriteDictionaryNumberEntry("FunctionType", 2);

            writer.BeginDictionaryEntry("Domain");
            writer.WriteArrayRealEntries(domainStart, domainEnd);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("C0");
            writer.WriteArrayRealEntries(color1.Red.Value, color1.Green.Value, color1.Blue.Value);
            writer.EndDictionaryEntry();

            writer.BeginDictionaryEntry("C1");
            writer.WriteArrayRealEntries(color2.Red.Value, color2.Green.Value, color2.Blue.Value);
            writer.EndDictionaryEntry();

            writer.WriteDictionaryNumberEntry("N", 1);
            writer.EndDictionary(); //function
        }

        protected virtual double[] GetCoords(PDFPoint offset, PDFSize size, double angle)
        {

            double[] all = new double[4];
            
            all[0] = offset.X.PointsValue;
            all[1] = offset.Y.PointsValue;
            all[2] = offset.X.PointsValue;
            all[3] = offset.Y.PointsValue;

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
            switch (angle)
            {
                case (0):
                case (360): //to top
                    
                    break;
                case (45): //to top right
                    
                    break;
                case (90): // to right
                    
                    break;
                case (135): // to bottom right
                    
                    break;
                case (180): // to bottom (default)
                    
                    break;
                case (225): // to bottom left
                    
                    break;
                case (270): // to left
                    
                    break;
                case (315): // to top left
                    
                    break;
                default:
                    //TODO: Support differing angles
                    
                    break;
            }

            return all;
        }
    }
}
