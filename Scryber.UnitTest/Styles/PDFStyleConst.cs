using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Styles
{
    /// <summary>
    /// Default values for the styles used by the unit tests
    /// </summary>
    internal class PDFStyleConst
    {
        public const PaperSize DefaultPaperSize = PaperSize.A4;
        public const PaperOrientation DefaultPaperOrientation = PaperOrientation.Portrait;
        public const LineCaps DefaultLineCaps = LineCaps.Butt;
        public const LineJoin DefaultLineJoin = LineJoin.Bevel;
        public const HorizontalAlignment DefaultHorizontalAlign = HorizontalAlignment.Left;
        public const VerticalAlignment DefaultVerticalAlign = VerticalAlignment.Top;
        public const string DefaultFontFamily = "Helvetica";
        public const float DefaultFontSize = 12.0F;
        public static readonly Color DefaultFillColor = StandardColors.Black;

        public static readonly Unit DefaultListNumberInset = 30;
        public static readonly Unit DefaultDefinitionListInset = 100;
        public const HorizontalAlignment DefaultListNumberAlignment = HorizontalAlignment.Right;
    }
}
