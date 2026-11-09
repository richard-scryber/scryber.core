using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Drawing;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS SVG/Stroke parsers: fill, fill-color, fill-opacity, stroke, stroke-color,
    /// stroke-width, stroke-dasharray, stroke-dashoffset, stroke-linecap, stroke-linejoin,
    /// stroke-opacity, paint-order
    /// Covers 11 SVG-related parsers
    /// </summary>
    [TestClass()]
    public class CSSSVGParsers_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region Helper Methods

        private Style CreateStyle()
        {
            return new Style();
        }

        private CSSStyleItemReader CreateReader(string cssValue)
        {
            var reader = new CSSStyleItemReader(cssValue);
            return reader;
        }

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = CreateReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        #endregion

        #region Fill Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_HexColor_SetsFillColor()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            // Fill parser sets SVGFillValue on SVGFillKey
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_NamedColor_SetsFillColor()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "red");

            Assert.IsTrue(result);
            // Named color should be recognized
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_RgbColor_SetsFillColor()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(255, 0, 0)");

            Assert.IsTrue(result);
            // RGB color should be parsed
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_UrlReference_SetsGradientReference()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url(#gradient1)");

            Assert.IsTrue(result);
            // URL reference to gradient should be recognized
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_None_SetsNone()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            // None value should be recognized
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Fill_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFillParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-fill");

            Assert.IsFalse(result);
        }

        #endregion

        #region Fill Color Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillColor_HexColor_SetsColor()
        {
            var parser = new CSSFillColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#0000FF");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.Fill.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillColor_NamedColor_SetsColor()
        {
            var parser = new CSSFillColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "blue");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.Fill.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillColor_RgbColor_SetsColor()
        {
            var parser = new CSSFillColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(0, 255, 0)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 255, 0), style.Fill.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillColor_RgbaColor_SetsColorAndOpacity()
        {
            var parser = new CSSFillColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgba(0, 255, 0, 0.5)");

            Assert.IsTrue(result);
            // RGBA should set both color and opacity
        }

        #endregion

        #region Fill Opacity Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillOpacity_FullyOpaque_SetsOne()
        {
            var parser = new CSSFillOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1");

            Assert.IsTrue(result);
            Assert.AreEqual(1.0, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillOpacity_HalfTransparent_SetsHalf()
        {
            var parser = new CSSFillOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0.5");

            Assert.IsTrue(result);
            Assert.AreEqual(0.5, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillOpacity_FullyTransparent_SetsZero()
        {
            var parser = new CSSFillOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Fill.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FillOpacity_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFillOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-opacity");

            Assert.IsFalse(result);
        }

        #endregion

        #region Stroke Color Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeColor_HexColor_SetsColor()
        {
            var parser = new CSSStrokeColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Stroke.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeColor_NamedColor_SetsColor()
        {
            var parser = new CSSStrokeColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "red");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Stroke.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeColor_RgbColor_SetsColor()
        {
            var parser = new CSSStrokeColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(0, 128, 255)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 128, 255), style.Stroke.Color);
        }

        #endregion

        #region Stroke Width Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeWidth_ValidUnit_Pt_SetsWidth()
        {
            var parser = new CSSStrokeWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.Stroke.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeWidth_ValidUnit_Px_SetsWidth()
        {
            var parser = new CSSStrokeWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "4px");

            Assert.IsTrue(result);
            // 4px = 4 * 0.75 = 3pt
            Assert.AreEqual(3.0, style.Stroke.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeWidth_ZeroValue_SetsZero()
        {
            var parser = new CSSStrokeWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Stroke.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeWidth_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSStrokeWidthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-width");

            Assert.IsFalse(result);
        }

        #endregion

        #region Stroke Dash Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDash_SingleValue_SetsDashPattern()
        {
            var parser = new CSSStrokeDashParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Stroke.Dash);
            Assert.AreEqual(1, style.Stroke.Dash.Pattern.Length);
            Assert.AreEqual(5, style.Stroke.Dash.Pattern[0]);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDash_TwoValues_SetsDashPattern()
        {
            var parser = new CSSStrokeDashParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5 3");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Stroke.Dash);
            Assert.AreEqual(2, style.Stroke.Dash.Pattern.Length);
            Assert.AreEqual(5, style.Stroke.Dash.Pattern[0]);
            Assert.AreEqual(3, style.Stroke.Dash.Pattern[1]);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDash_MultipleValues_SetsDashPattern()
        {
            var parser = new CSSStrokeDashParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5 3 2 3");

            Assert.IsTrue(result);
            Assert.IsNotNull(style.Stroke.Dash);
            Assert.AreEqual(4, style.Stroke.Dash.Pattern.Length);
            Assert.AreEqual(5, style.Stroke.Dash.Pattern[0]);
            Assert.AreEqual(3, style.Stroke.Dash.Pattern[1]);
            Assert.AreEqual(2, style.Stroke.Dash.Pattern[2]);
            Assert.AreEqual(3, style.Stroke.Dash.Pattern[3]);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDash_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSStrokeDashParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-dash");

            Assert.IsFalse(result);
        }

        #endregion

        #region Stroke Dash Offset Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDashOffset_ValidUnit_Pt_SetsOffset()
        {
            var parser = new CSSStrokeDashOffsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5pt");

            Assert.IsTrue(result);
            var offset = style.GetValue(StyleKeys.StrokeDashOffsetKey, Unit.AutoValue);
            Assert.AreEqual(5.0, offset.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDashOffset_ValidUnit_Px_SetsOffset()
        {
            var parser = new CSSStrokeDashOffsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "8px");

            Assert.IsTrue(result);
            // 8px = 8 * 0.75 = 6pt
            var offset = style.GetValue(StyleKeys.StrokeDashOffsetKey, Unit.AutoValue);
            Assert.AreEqual(6.0, offset.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeDashOffset_ZeroValue_SetsZero()
        {
            var parser = new CSSStrokeDashOffsetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            var offset = style.GetValue(StyleKeys.StrokeDashOffsetKey, Unit.AutoValue);
            Assert.AreEqual(0, offset.PointsValue, 0.001);
        }

        #endregion

        #region Stroke Line Cap Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineCap_Butt_SetsButt()
        {
            var parser = new CSSStrokeLineCapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "butt");

            Assert.IsTrue(result);
            Assert.AreEqual(LineCaps.Butt, style.Stroke.LineCap);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineCap_Round_SetsRound()
        {
            var parser = new CSSStrokeLineCapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "round");

            Assert.IsTrue(result);
            Assert.AreEqual(LineCaps.Round, style.Stroke.LineCap);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineCap_Square_SetsSquare()
        {
            var parser = new CSSStrokeLineCapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "square");

            Assert.IsTrue(result);
            Assert.AreEqual(LineCaps.Square, style.Stroke.LineCap);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineCap_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSStrokeLineCapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "ROUND");

            Assert.IsTrue(result);
            Assert.AreEqual(LineCaps.Round, style.Stroke.LineCap);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineCap_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSStrokeLineCapParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-cap");

            Assert.IsFalse(result);
        }

        #endregion

        #region Stroke Line Join Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineJoin_Bevel_SetsBevel()
        {
            var parser = new CSSStrokeLineJoinParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "bevel");

            Assert.IsTrue(result);
            Assert.AreEqual(LineJoin.Bevel, style.Stroke.LineJoin);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineJoin_Round_SetsRound()
        {
            var parser = new CSSStrokeLineJoinParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "round");

            Assert.IsTrue(result);
            Assert.AreEqual(LineJoin.Round, style.Stroke.LineJoin);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineJoin_Mitre_SetsMitre()
        {
            var parser = new CSSStrokeLineJoinParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "mitre");

            Assert.IsTrue(result);
            Assert.AreEqual(LineJoin.Mitre, style.Stroke.LineJoin);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineJoin_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSStrokeLineJoinParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "ROUND");

            Assert.IsTrue(result);
            Assert.AreEqual(LineJoin.Round, style.Stroke.LineJoin);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeLineJoin_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSStrokeLineJoinParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-join");

            Assert.IsFalse(result);
        }

        #endregion

        #region Stroke Opacity Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeOpacity_FullyOpaque_SetsOne()
        {
            var parser = new CSSStrokeOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1");

            Assert.IsTrue(result);
            Assert.AreEqual(1.0, style.Stroke.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeOpacity_HalfTransparent_SetsHalf()
        {
            var parser = new CSSStrokeOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0.5");

            Assert.IsTrue(result);
            Assert.AreEqual(0.5, style.Stroke.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeOpacity_FullyTransparent_SetsZero()
        {
            var parser = new CSSStrokeOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Stroke.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void StrokeOpacity_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSStrokeOpacityParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-opacity");

            Assert.IsFalse(result);
        }

        #endregion
    }
}
