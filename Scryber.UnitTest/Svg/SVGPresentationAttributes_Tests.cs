using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Styles;
using Scryber.Svg.Components;

namespace Scryber.Core.UnitTests.Svg
{
    /// <summary>
    /// SVG presentation attributes that only worked in a style attribute, and a style attribute on a gradient stop
    /// (found by the documentation checks against the engine).
    /// </summary>
    [TestClass()]
    public class SVGPresentationAttributes_Tests
    {
        private const string Html = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
<svg xmlns='http://www.w3.org/2000/svg' width='300' height='120'>
  <defs>
    <linearGradient id='grad'>
      <stop id='stop1' offset='0' style='stop-color: #FF0000' />
      <stop id='stop2' offset='1' stop-color='#0000FF' />
    </linearGradient>
  </defs>
  <rect id='rect' x='10' y='10' width='80' height='80' fill='url(#grad)' stroke='black' stroke-width='4'
        stroke-dasharray='6 3' stroke-dashoffset='2' stroke-opacity='0.5' stroke-linejoin='round' />
  <path id='path' d='M 110 10 L 190 90' stroke='black' stroke-width='4' stroke-dasharray='5 5' stroke-dashoffset='3' />
  <circle id='circle' cx='250' cy='50' r='40' fill='none' stroke='black' stroke-opacity='0.25' />
</svg>
</body>
</html>";

        private static Document Render(out string pdf)
        {
            var doc = Document.ParseDocument(new StringReader(Html), ParseSourceType.DynamicContent);
            doc.RenderOptions.Compression = OutputCompressionType.None;
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);
                pdf = Encoding.Latin1.GetString(ms.ToArray());
                using (var output = DocStreams.GetOutputStream("SVGPresentationAttributes.pdf"))
                    output.Write(ms.ToArray(), 0, (int)ms.Length);
            }
            return doc;
        }

        [TestMethod()]
        public void SVGAttributes_01_StrokeOpacityAndDashOffsetAttributes()
        {
            var doc = Render(out var pdf);

            var rect = doc.FindAComponentById("rect") as SVGRect;
            Assert.IsNotNull(rect);
            Assert.AreEqual(0.5, rect.Style.GetValue(StyleKeys.StrokeOpacityKey, 1.0), 0.0001, "rect stroke-opacity attribute");
            Assert.AreEqual((Unit)2, rect.Style.GetValue(StyleKeys.StrokeDashOffsetKey, Unit.Zero), "rect stroke-dashoffset attribute");
            Assert.AreEqual(LineJoin.Round, rect.Style.Stroke.LineJoin, "rect stroke-linejoin attribute");

            var path = doc.FindAComponentById("path") as SVGPath;
            Assert.IsNotNull(path);
            Assert.AreEqual((Unit)3, path.Style.GetValue(StyleKeys.StrokeDashOffsetKey, Unit.Zero), "path stroke-dashoffset attribute");

            var circle = doc.FindAComponentById("circle") as Component;
            Assert.AreEqual(0.25, circle.GetAppliedStyle().GetValue(StyleKeys.StrokeOpacityKey, 1.0), 0.0001, "circle stroke-opacity attribute");

            //and they are drawn - the dash phase on the line dash operator, and the stroke opacity in a graphics state
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"\[\s*6(\.0+)?\s+3(\.0+)?\s*\]\s*2(\.0+)?\s+d"), "The rect dash with its offset");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"\[\s*5(\.0+)?\s+5(\.0+)?\s*\]\s*3(\.0+)?\s+d"), "The path dash with its offset");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"/CA\s+0?\.50*\s"), "The rect stroke opacity");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"/CA\s+0?\.250*\s"), "The circle stroke opacity");
        }

        [TestMethod()]
        public void SVGAttributes_02_StyleOnAGradientStop()
        {
            var doc = Render(out _);

            //the gradient is in the defs of the svg
            var svg = doc.FindAComponentById("rect").Parent as SVGCanvas;
            Assert.IsNotNull(svg);
            SVGLinearGradient gradient = null;
            foreach (var def in svg.Definitions)
            {
                if (def is SVGLinearGradient lg && lg.ID == "grad")
                    gradient = lg;
            }
            Assert.IsNotNull(gradient, "The gradient is parsed");
            Assert.AreEqual(2, gradient.Stops.Count, "Both stops are parsed");

            var stop1 = gradient.Stops[0];
            Assert.AreEqual("stop1", stop1.ID);
            Assert.AreEqual(new Color(255, 0, 0), stop1.Style.GetValue(StyleKeys.SVGGradientStopColorKey, StandardColors.Black), "stop-color from the style attribute");

            var stop2 = gradient.Stops[1];
            Assert.AreEqual(new Color(0, 0, 255), stop2.StopColor, "stop-color attribute");
        }
    }
}
