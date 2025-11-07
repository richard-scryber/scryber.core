using Scryber.PDF.Graphics;


namespace Scryber.Drawing
{
    public class PathAdornmentInfo
    {
        public Point Location { get;  set; }
        public double AngleRadians { get;  set; }
        public PDFBrush CurrentBrush { get; private set; }
        public PDFPen CurrentPen { get; private set; }
        
        public double? ExplicitAngle { get; private set; }
        
        public bool ReverseAngleAtStart { get; private set; }

        public PathAdornmentInfo(Point initialLocation, double? initialAngle, bool reverseAngleAtStart, PDFBrush brush, PDFPen pen)
        {
            this.CurrentBrush = brush;
            this.CurrentPen = pen;
            this.Location = initialLocation;
            
            if (initialAngle.HasValue)
                this.AngleRadians = initialAngle.Value;
            
            this.ExplicitAngle = initialAngle;
            this.ReverseAngleAtStart = reverseAngleAtStart;
        }
    }
}