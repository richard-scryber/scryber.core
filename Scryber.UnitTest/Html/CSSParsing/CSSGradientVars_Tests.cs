using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    [TestClass]
    public class CSSGradientVars_Tests
    {
        private static int CountGradients(string css, string body, object model = null, ParserConformanceMode? mode = null)
        {
            var pi = mode == ParserConformanceMode.Strict ? "<?scryber parser-mode='Strict' ?>" : "";
            var src = pi + "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var text = System.Text.Encoding.Latin1.GetString(ms.ToArray());
                int count = 0, idx = 0;
                while ((idx = text.IndexOf("/ShadingType", idx, StringComparison.Ordinal)) >= 0) { count++; idx++; }
                return count;
            }
        }

        private const string Body = "<div class='g' style='height:25pt'>x</div>";

        [TestMethod]
        public void Gradient_Literal_Baseline()
        {
            Assert.IsTrue(CountGradients(".g{background-image: linear-gradient(90deg, #1f3347, #5980a6);}", Body) > 0);
        }

        [TestMethod]
        public void Gradient_WithLiteralCustomProperties()
        {
            Assert.IsTrue(CountGradients(":root{--a:#1f3347;--b:#5980a6;} .g{background-image: linear-gradient(90deg, var(--a), var(--b));}", Body) > 0);
        }

        [TestMethod]
        public void Gradient_WithBoundVarAndFallback()
        {
            var css = ".g{background-image: linear-gradient(90deg, var(model.dark, #1f3347), #5980a6);}";
            Assert.IsTrue(CountGradients(css, Body, new { dark = "#7a1f1f" }) > 0, "Bound");
            Assert.IsTrue(CountGradients(css, Body, new { other = 1 }) > 0, "Fallback");
        }

        [TestMethod]
        public void Gradient_WithVars_Strict()
        {
            Assert.IsTrue(CountGradients(":root{--a:#1f3347;--b:#5980a6;} .g{background-image: linear-gradient(90deg, var(--a), var(--b));}", Body, null, ParserConformanceMode.Strict) > 0);
        }

        [TestMethod]
        public void BackgroundShorthand_LinearGradient()
        {
            Assert.IsTrue(CountGradients(".g{background: linear-gradient(90deg, #1f3347, #5980a6);}", Body) > 0);
        }

        [TestMethod]
        public void BackgroundShorthand_RadialGradient()
        {
            Assert.IsTrue(CountGradients(".g{background: radial-gradient(circle, #1f3347, #5980a6);}", Body) > 0);
        }

        [TestMethod]
        public void BackgroundShorthand_GradientWithVars()
        {
            Assert.IsTrue(CountGradients(":root{--a:#1f3347;--b:#5980a6;} .g{background: linear-gradient(90deg, var(--a), var(--b));}", Body) > 0);
        }

        [TestMethod]
        public void NoGradient_HasNone()
        {
            Assert.AreEqual(0, CountGradients(".g{background-color: red;}", Body));
        }
    }
}
