using Scryber.Drawing;
using Scryber.PDF.Graphics;

namespace Scryber.Svg
{
    public class SVGGradientReferenceBrush : PDFBrush
    {
        public string ReferenceId { get; set; }


        public double Opacity { get; set; }
        
        public SVGGradientReferenceBrush(string refId, double opacity)
        {
            this.ReferenceId = refId;
            this.Opacity = opacity;
        }


        public override FillType FillStyle
        {
            get { return FillType.Pattern; }
        }

        private IPDFGraphicsAdapter _myAdapter;
        public override bool SetUpGraphics(PDFGraphics graphics, Rect bounds)
        {
            return false;

        }

        public override void ReleaseGraphics(PDFGraphics g, Rect bounds)
        {
            if(null != _myAdapter)
                _myAdapter.ReleaseGraphics(g, bounds);
            
            base.ReleaseGraphics(g, bounds);
        }
    }
}