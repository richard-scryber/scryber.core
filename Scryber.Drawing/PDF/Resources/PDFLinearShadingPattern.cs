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
        public PDFLinearShadingPattern(IComponent owner, string key, GradientLinearDescriptor descriptor, Rect bounds)
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
            //writer.WriteDictionaryEntry - Transformation
            
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
            writer.BeginDictionaryEntry("Shading");
            writer.BeginDictionary();
            writer.BeginDictionaryEntry("ShadingType");
            writer.WriteNumberS((int)ShadingType.Axial);
            writer.EndDictionaryEntry();
            writer.WriteDictionaryNameEntry("ColorSpace", "DeviceRGB");

            writer.BeginDictionaryEntry("BBox");

            Point offset = new Point(this.Start.X, this.Start.Y);// this.Start;
            Size size = this.Size;

            Size graphicsSize = new Size(size.Width + offset.X, size.Height + offset.Y);

            //TODO: Apply the matrix
            writer.WriteArrayRealEntries(true, offset.X.PointsValue,
                                               offset.Y.PointsValue,
                                               offset.X.PointsValue + size.Width.PointsValue,
                                               offset.Y.PointsValue + size.Height.PointsValue);

            writer.EndDictionaryEntry();
            writer.WriteDictionaryBooleanEntry("AntiAlias", true);

            writer.BeginDictionaryEntry("Coords");
            var coords = GetCoords(offset, size, this._descriptor.Angle);
            
            
            writer.WriteArrayRealEntries(true, coords);
            writer.EndDictionaryEntry();

            
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

        

        public virtual double[] GetCoords(Point offset, Size size, double angle)
        {
            var len = GetMaxLengthBoundingBox(new Rect(offset.X, offset.Y, size.Width, size.Height), angle, out double patternStartOffset, out Point start,
                out Point end);

            
            double[] all = new double[4];
            
            all[0] = start.X.PointsValue;
            all[1] = start.Y.PointsValue;
            all[2] = end.X.PointsValue;
            all[3] = end.Y.PointsValue;

            return all;
            
            //TODO: Change this to support any angle with sin, cos and tan

            var radians = angle / (180.0 / Math.PI);

            
            
            if (angle < 90)
            {
                //start from bottom left position
                
                var slope = Math.Tan(radians);
                var pt1 = new Point(offset.X, offset.Y + size.Height);
                var yIntercept = pt1.Y - (pt1.X * slope);
                
                
            }
            else if (angle < 180)
            {
                //start from top left - moving down
                angle = 270 - angle;
                radians = angle * ( Math.PI / 180);
                var m = Math.Tan(radians);
                var pt1 = new Point(offset.X, offset.Y);
                //y = mx + c
                //c = y - mx;
                var c = pt1.Y - (pt1.X * m);
                
                //calculate the perpendicular that passes through the bottom right corner
                var m2 = -(1 / m); //negative inverse slope
                var pt2 = new Point(offset.X + size.Width, offset.Y + size.Height);
                var c2 = pt2.Y - (pt2.X * m2);
                
                
                //now calculate the intersection of the 2 lines.
                //mx + c = m2x + c2
                //mx - m2x + c = c2
                //mx - m2x = c2 - c
                //(m - m2)x = c2 - c
                //x = (c2 -c)/(m - m2)
                
                var x = (c2 - c) / (m - m2);
                var y = (x * m) + c;

                var checkY = (x * m2) + c2;
                // ReSharper disable once CompareOfFloatsByEqualityOperator - rounding for equality precision
                if (Math.Round(checkY.PointsValue) != Math.Round(a: y.PointsValue))
                    throw new InvalidOperationException("The perpendicuar lines do not meet at the same place!");

                pt2 = new Point(x, y);

                all[0] = pt1.X.PointsValue;
                all[1] = pt1.Y.PointsValue;
                all[2] = pt2.X.PointsValue;
                all[3] = pt2.Y.PointsValue;

                return all;

            }
            // if (angle < 90)
            // {
            //     all[2] += opposite;
            //     all[3] += adjacent;
            // }
            // else if (angle < 180)
            // {
            //     all[2] += adjacent;
            //     all[3] += opposite;
            // }
            //
            //
            // return all;
            
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

        /// <summary>
        /// Calculates the shortest line at the specified angle that will ensure a linear pattern at right angles will cover the area.
        /// </summary>
        /// <param name="box">The box size to cover</param>
        /// <param name="angleDegrees">The pattern angle to use for the line, where 0 is horizontally across.</param>
        /// <param name="start">The calculated start point of the line that will cover the box</param>
        /// <param name="end">The calculated end point of the line that will cover the box</param>
        /// <returns>The total length of the line.</returns>
        public static Unit GetMaxLengthBoundingBox(Rect box, double angleDegrees, out double patternStartOffset, out Point start, out Point end)
        {
            
            Unit length;
            var rounded = (int)Math.Round(angleDegrees);
            
            while (rounded >= 360)
            {
                rounded -= 360;
            }

            patternStartOffset = 0.0;
            
            //use a quick lookup for the simple ones.
            switch (rounded)
            {
                case(0):
                    //horizontal right
                    length = box.Width;
                    start = new Point(box.X, box.Y);
                    end = new Point(box.X + box.Width, box.Y);
                    patternStartOffset = box.X.PointsValue;
                    break;
                
                case(45):
                    //top left to bottom right
                    length = Math.Sqrt((box.Width.PointsValue * box.Width.PointsValue) + (box.Height.PointsValue *  box.Height.PointsValue));
                    start = new Point(box.X, box.Y);
                    end = new Point(box.X + box.Width, box.Y + box.Height);
                    patternStartOffset = Math.Sqrt((box.X.PointsValue * box.X.PointsValue) +
                                                   (box.Y.PointsValue * box.Y.PointsValue));
                    break;
                case(90):
                    //vertical down
                    length = box.Height;
                    start = new Point(box.X, box.Y);
                    end = new Point(box.X, box.Y + box.Height);
                    patternStartOffset = box.Y.PointsValue;
                    break;
                
                case(135):
                    //top right to bottom left
                    length = Math.Sqrt((box.Width.PointsValue * box.Width.PointsValue) + (box.Height.PointsValue *  box.Height.PointsValue));
                    start = new Point(box.X + box.Width, box.Y);
                    end = new Point(box.X, box.Y + box.Height);
                    break;
                
                case(180):
                    //horizontal left
                    length = box.Width;
                    start = new Point(box.X + box.Width, box.Y);
                    end = new Point(box.X, box.Y);
                    break;
                
                case(225):
                    //bottom right to top left
                    length = Math.Sqrt((box.Width.PointsValue * box.Width.PointsValue) + (box.Height.PointsValue *  box.Height.PointsValue));
                    start = new Point(box.X + box.Width, box.Y + box.Height);
                    end = new Point(box.X, box.Y);
                    break;
                case(270):
                    //vertical up
                    length = box.Height;
                    start = new Point(box.X, box.Y + box.Height);
                    end = new Point(box.X, box.Y);
                    break;
                
                case(315):
                    //bottom left to top right
                    length = Math.Sqrt((box.Width.PointsValue * box.Width.PointsValue) + (box.Height.PointsValue *  box.Height.PointsValue));
                    start = new Point(box.X, box.Y + box.Height);
                    end = new Point(box.X + box.Width, box.Y);
                    break;
                default:
                    length = GetAngularBoundingBox(box, angleDegrees, out Point midPt, out start, out end);
                    break;
            }

            return Math.Abs(length.PointsValue);
        }

        public static Unit GetAngularBoundingBox(Rect box, double angle, out Point midPt, out Point start, out Point end)
        {
            //We work from the mid point and calculate the length of line needed at the specified angle,
            //where the permendicular line would pass through the farthest corners.
            
            //this give us the maximum extent of the required gradients.
            midPt = new Point(box.X + (box.Width / 2), box.Y + (box.Height / 2));
            
            
            while (angle < 0.0)
            {
                angle += 360.0;
            }

            while (angle > 360.0)
            {
                angle -= 360.0;
            }

            double radians;
            Point offset;
            
            if (angle < 90)
            {
                //start from bottom left - moving up and across
                angle = 90 - angle;
                radians = angle * (Math.PI / 180);
                
                
                //get the line equation for the angle that passes through the middle.
                var m = Math.Tan(radians);
                
                //y = mx + c
                //c = y - mx;
                var c = midPt.Y - (midPt.X * m);
                
                //get the equation for the line that is perpendicular to this line and passes through the top right
                
                var m2 = -(1 / m); //negative inverse slope
                var pt2 = new Point(box.X + box.Width, box.Y);
                var c2 = pt2.Y - (pt2.X * m2);
                
                //now calculate the intersection of the 2 lines for main line and top right permendicular
                //mx + c = m2x + c2
                //mx - m2x + c = c2
                //mx - m2x = c2 - c
                //(m - m2)x = c2 - c
                //x = (c2 -c)/(m - m2)

                var ptTR = new Point();
                ptTR.X = (c2 - c) / (m - m2);
                ptTR.Y = (ptTR.X * m) + c;
                
#if DEBUG
                var checkY = (ptTR.X * m2) + c2;
                // ReSharper disable once CompareOfFloatsByEqualityOperator - rounding for equality precision
                if (Math.Round(checkY.PointsValue, 3) != Math.Round(ptTR.Y.PointsValue, 3))
                    throw new InvalidOperationException("The perpendicuar lines do not meet at the same place - replace with generic values!");
#endif
                
                //get the equation for the line that is perpendicular and passes through the bottom left
                var m3 = m2; //same slope as other permendicular
                var pt3 = new Point(box.X, box.Y + box.Height);
                var c3 = pt3.Y - (pt3.X * m3);
                
                //now calculate the intersection of the 2 lines for main line and bottom left permendicular
                //mx + c = m3x + c3
                //mx - m3x + c = c3
                //mx - m3x = c3 - c
                //(m - m3)x = c3 - c
                //x = (c3 -c)/(m - m3)
                var ptBL = new Point();
                ptBL.X = (c3 - c) / (m - m3);
                ptBL.Y = (ptBL.X * m) + c;
                
                
#if DEBUG
                checkY = (ptBL.X * m3) + c3;
                
                if (Math.Round(checkY.PointsValue, 3) != Math.Round(ptBL.Y.PointsValue, 3))
                    throw new InvalidOperationException("The perpendicuar lines do not meet at the same place - replace with generic values!");
#endif

               
                //now calculate the intersection of the 2 lines for main line and bottom left permendicular

                
                var xSq = ptTR.X.PointsValue - ptBL.X.PointsValue;
                xSq = xSq * xSq;

                var ySq = ptTR.Y.PointsValue - ptBL.Y.PointsValue;
                ySq = ySq * ySq;
                
                var len = new Unit(Math.Sqrt(xSq + ySq));
                
                start = ptBL;
                end = ptTR;
                return len;

            }



            start = new Point(box.X, box.Y);
            end = new Point(box.X, box.Y + box.Height);
            return box.Height;
        }
    }
}
