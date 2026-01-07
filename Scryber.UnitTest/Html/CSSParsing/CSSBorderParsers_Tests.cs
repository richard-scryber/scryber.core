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
    /// Tests for CSS border parsers: border shorthand, style, color, radius, and individual sides
    /// Covers 16+ parsers: border (shorthand), border-style, border-color, border-radius,
    /// border-top/right/bottom/left, and side-specific style/color variants
    /// </summary>
    [TestClass()]
    public class CSSBorderParsers_Tests
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

        #region Border Shorthand Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_WidthOnly_SetsWidth()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.Border.Width.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_StyleOnly_SetsStyle()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "solid");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_ColorOnly_SetsColor()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF0000");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Border.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_WidthAndStyle_SetsBoth()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3pt solid");

            Assert.IsTrue(result);
            Assert.AreEqual(3.0, style.Border.Width.PointsValue, 0.001);
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_AllThreeValues_SetsAll()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt dashed #0000FF");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.Border.Width.PointsValue, 0.001);
            Assert.AreEqual(LineType.Dash, style.Border.LineStyle);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.Border.Color);
            Assert.IsNotNull(style.Border.Dash);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_DottedStyle_SetsDashPattern()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "1pt dotted");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.Border.LineStyle);
            Assert.IsNotNull(style.Border.Dash);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_WithNamedColor_ParsesColor()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt solid red");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Border.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Border_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSBorderParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-border");

            Assert.IsFalse(result);
        }

        #endregion

        #region Border Style Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_Solid_SetsSolid()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "solid");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_Dashed_SetsDashed()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dashed");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.Border.LineStyle);
            Assert.IsNotNull(style.Border.Dash);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_Dotted_SetsDotted()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dotted");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.Border.LineStyle);
            Assert.IsNotNull(style.Border.Dash);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_None_SetsNone()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.None, style.Border.LineStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_Double_SetsSolid()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "double");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderStyle_Groove_SetsSolid()
        {
            var parser = new CSSBorderStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "groove");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.Border.LineStyle);
        }

        #endregion

        #region Border Color Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderColor_HexColor_SetsColor()
        {
            var parser = new CSSBorderColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#00FF00");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 255, 0), style.Border.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderColor_NamedColor_SetsColor()
        {
            var parser = new CSSBorderColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "blue");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.Border.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderColor_RgbColor_SetsColor()
        {
            var parser = new CSSBorderColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(128, 64, 32)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(128, 64, 32), style.Border.Color);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderColor_RgbaColor_SetsColorAndOpacity()
        {
            var parser = new CSSBorderColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgba(255, 0, 0, 0.5)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.Border.Color);
            Assert.AreEqual(0.5, style.Border.Opacity, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderColor_InvalidColor_ReturnsFalse()
        {
            var parser = new CSSBorderColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "not-a-color");

            Assert.IsFalse(result);
        }

        #endregion

        #region Border Radius Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRadius_ValidUnit_Pt_SetsRadius()
        {
            var parser = new CSSBorderRadiusParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5pt");

            Assert.IsTrue(result);
            Assert.AreEqual(5.0, style.Border.CornerRadius.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRadius_ValidUnit_Px_SetsRadius()
        {
            var parser = new CSSBorderRadiusParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "8px");

            Assert.IsTrue(result);
            // 8px = 8 * 0.75 = 6pt
            Assert.AreEqual(6.0, style.Border.CornerRadius.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRadius_ZeroValue_SetsZero()
        {
            var parser = new CSSBorderRadiusParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "0");

            Assert.IsTrue(result);
            Assert.AreEqual(0.0, style.Border.CornerRadius.PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRadius_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSBorderRadiusParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid");

            Assert.IsFalse(result);
        }

        #endregion

        #region Border Side Parsers Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderTop_WidthStyleColor_SetsAllThree()
        {
            var parser = new CSSBorderTopParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3pt solid #FF0000");

            Assert.IsTrue(result);
            // Note: BorderTop sets individual top properties
            Assert.AreEqual(3.0, style.GetValue(StyleKeys.BorderTopWidthKey, Unit.Zero).PointsValue, 0.001);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.BorderTopStyleKey, LineType.None));
            Assert.AreEqual(Color.FromRGB(255, 0, 0), style.GetValue(StyleKeys.BorderTopColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRight_WidthOnly_SetsWidth()
        {
            var parser = new CSSBorderRightParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "4pt");

            Assert.IsTrue(result);
            Assert.AreEqual(4.0, style.GetValue(StyleKeys.BorderRightWidthKey, Unit.Zero).PointsValue, 0.001);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderBottom_StyleAndColor_SetsBoth()
        {
            var parser = new CSSBorderBottomParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dashed blue");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.BorderBottomStyleKey, LineType.None));
            Assert.AreEqual(Color.FromRGB(0, 0, 255), style.GetValue(StyleKeys.BorderBottomColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderLeft_AllThreeValues_SetsAll()
        {
            var parser = new CSSBorderLeftParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2pt dotted green");

            Assert.IsTrue(result);
            Assert.AreEqual(2.0, style.GetValue(StyleKeys.BorderLeftWidthKey, Unit.Zero).PointsValue, 0.001);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.BorderLeftStyleKey, LineType.None));
            Assert.AreEqual(Color.FromRGB(0, 128, 0), style.GetValue(StyleKeys.BorderLeftColorKey, Color.Transparent));
        }

        #endregion

        #region Border Side Style Parsers Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderTopStyle_Solid_SetsSolid()
        {
            var parser = new CSSBorderTopStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "solid");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Solid, style.GetValue(StyleKeys.BorderTopStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRightStyle_Dashed_SetsDashed()
        {
            var parser = new CSSBorderRightStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dashed");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.BorderRightStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderBottomStyle_Dotted_SetsDotted()
        {
            var parser = new CSSBorderBottomStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "dotted");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.Dash, style.GetValue(StyleKeys.BorderBottomStyleKey, LineType.None));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderLeftStyle_None_SetsNone()
        {
            var parser = new CSSBorderLeftStyleParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(LineType.None, style.GetValue(StyleKeys.BorderLeftStyleKey, LineType.None));
        }

        #endregion

        #region Border Side Color Parsers Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderTopColor_HexColor_SetsColor()
        {
            var parser = new CSSBorderTopColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "#FF00FF");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 0, 255), style.GetValue(StyleKeys.BorderTopColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderRightColor_NamedColor_SetsColor()
        {
            var parser = new CSSBorderRightColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "yellow");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(255, 255, 0), style.GetValue(StyleKeys.BorderRightColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderBottomColor_RgbColor_SetsColor()
        {
            var parser = new CSSBorderBottomColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(200, 100, 50)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(200, 100, 50), style.GetValue(StyleKeys.BorderBottomColorKey, Color.Transparent));
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void BorderLeftColor_RgbColor_SetsColor()
        {
            var parser = new CSSBorderLeftColorParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "rgb(100, 150, 200)");

            Assert.IsTrue(result);
            Assert.AreEqual(Color.FromRGB(100, 150, 200), style.GetValue(StyleKeys.BorderLeftColorKey, Color.Transparent));
        }

        #endregion
    }
}
