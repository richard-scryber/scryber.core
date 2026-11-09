using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scryber.PDF;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using System.ComponentModel;
using Scryber.Drawing;

namespace Scryber.PDF.Graphics
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFContainImageBrush : PDFFullImageBrush
    {

        

        public PDFContainImageBrush()
            : this(null)
        {
        }

        public PDFContainImageBrush(string source)
            : base(source)
        {
        }



        protected override Size CalculateAppropriateImageSize(ImageData imgdata, Rect bounds)
        {
            //Find the dimension that makes sure the bounds are fully covered
            var size = imgdata.GetSize();
            
            double scaleX = (bounds.Width.PointsValue / size.Width.PointsValue);
            double scaleY = (bounds.Height.PointsValue / size.Height.PointsValue);
            var scaleMin = Math.Min(scaleX, scaleY);
            
            double resultHeight = size.Height.PointsValue * scaleMin;
            double resultWidth = size.Width.PointsValue * scaleMin;
            

            Unit imgw = size.Width.PointsValue * scaleMin;
            Unit imgh = size.Height.PointsValue * scaleMin;

            if(imgw > bounds.Width)
            {
                this.XPostion = -((imgw.PointsValue - bounds.Width.PointsValue) / 2.0);
            }

            if(imgh > bounds.Height)
            {
                this.YPostion = -((imgh.PointsValue - bounds.Height.PointsValue) / 2.0);
            }

            this.XStep = imgw;
            this.YStep = imgh;

            return new Size(imgw, imgh);

            
        }

        protected override Point CalculateAppropriateImageStart(Rect bounds, Size size, DrawingOrigin origin, Size container)
        {
            Point start = new Point(bounds.X + this.XPostion, bounds.Y + this.YPostion);
                
            if(size.Width < bounds.Width)
                start.X +=  (bounds.Width - size.Width) / 2.0;
            
            if(size.Height < bounds.Height)
                start.Y +=  (bounds.Height - size.Height) / 2.0;
            
            if (origin == DrawingOrigin.TopLeft)
            {
                start.Y = container.Height - start.Y;
            }
            
            return start;
        }
    }
}
