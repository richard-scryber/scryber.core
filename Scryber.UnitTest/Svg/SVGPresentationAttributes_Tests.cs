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

        [TestMethod()]
        public void SVGAttributes_03_OpacityAttributeAndPathDashOffsetStyle()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<body>
<svg xmlns='http://www.w3.org/2000/svg' width='300' height='120'>
  <rect id='rect' x='10' y='10' width='80' height='80' fill='red' opacity='0.4' />
  <circle id='circle' cx='150' cy='50' r='40' fill='blue' opacity='0.6' />
  <path id='path' d='M 200 10 L 290 90' stroke='black' stroke-width='4' style='stroke-dasharray: 7 2; stroke-dashoffset: 1' />
</svg>
</body>
</html>";
            var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent);
            doc.RenderOptions.Compression = OutputCompressionType.None;
            string pdf;
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);
                pdf = Encoding.Latin1.GetString(ms.ToArray());
            }

            //the opacity attribute is the fill opacity, as the css opacity property
            Assert.AreEqual(0.4, (doc.FindAComponentById("rect") as Component).GetAppliedStyle().GetValue(StyleKeys.FillOpacityKey, 1.0), 0.0001, "rect opacity attribute");
            Assert.AreEqual(0.6, (doc.FindAComponentById("circle") as Component).GetAppliedStyle().GetValue(StyleKeys.FillOpacityKey, 1.0), 0.0001, "circle opacity attribute");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"/ca\s+0?\.40*\s"), "The rect fill opacity");
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"/ca\s+0?\.60*\s"), "The circle fill opacity");

            //a dash offset in the style of a path is drawn
            Assert.IsTrue(System.Text.RegularExpressions.Regex.IsMatch(pdf, @"\[\s*7(\.0+)?\s+2(\.0+)?\s*\]\s*1(\.0+)?\s+d"), "The path dash with its offset from the style");
        }

        [TestMethod()]
        public void SVGAttributes_04_PaintOrderStrokeBeforeFill()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>.under { paint-order: stroke; }</style></head>
<body>
<svg xmlns='http://www.w3.org/2000/svg' width='400' height='140'>
  <circle id='normal' cx='60' cy='70' r='40' fill='#F97316' stroke='#2563EB' stroke-width='16' />
  <circle id='circle' cx='160' cy='70' r='40' fill='#F97316' stroke='#2563EB' stroke-width='16' paint-order='stroke' />
  <rect id='rect' x='220' y='30' width='70' height='80' fill='#F97316' stroke='#2563EB' stroke-width='16' paint-order='stroke fill' />
  <path id='path' class='under' d='M 320 30 L 380 30 L 350 110 Z' fill='#F97316' stroke='#2563EB' stroke-width='16' />
</svg>
</body>
</html>";
            var doc = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent);
            doc.RenderOptions.Compression = OutputCompressionType.None;
            string pdf;
            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);
                pdf = Encoding.Latin1.GetString(ms.ToArray());
                using (var output = DocStreams.GetOutputStream("SVGPaintOrder.pdf"))
                    output.Write(ms.ToArray(), 0, (int)ms.Length);
            }

            Assert.IsFalse((doc.FindAComponentById("normal") as Component).GetAppliedStyle().GetValue(StyleKeys.SVGGeometryPaintOrderKey, PaintOrder.Default).IsStrokeBeforeFill, "The default paints the fill first");
            foreach (var id in new[] { "circle", "rect", "path" })
                Assert.IsTrue((doc.FindAComponentById(id) as Component).GetAppliedStyle().GetValue(StyleKeys.SVGGeometryPaintOrderKey, PaintOrder.Default).IsStrokeBeforeFill, "The paint-order of " + id);

            //the normal circle is filled and stroked in one (B), the others are stroked (S) and then the same path filled (f)
            Assert.AreEqual(1, System.Text.RegularExpressions.Regex.Matches(pdf, @"\sh B\s").Count, "One fill and stroke");
            Assert.AreEqual(3, System.Text.RegularExpressions.Regex.Matches(pdf, @"\sh S\s[^SBQ]+\sh f\s").Count, "Three strokes, each followed by the fill of the same path");
        }
    }
}
