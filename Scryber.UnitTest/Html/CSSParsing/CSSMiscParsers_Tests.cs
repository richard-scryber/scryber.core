using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Drawing;
using Scryber.Html;
using Scryber.Text;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for miscellaneous CSS parsers: position mode/float, hyphenation properties,
    /// font-display/source/stretch, content, counter-reset/increment
    /// Covers 13 specialized parsers
    /// </summary>
    [TestClass()]
    public class CSSMiscParsers_Tests
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

        #region Position Mode Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_Static_SetsStatic()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "static");

            Assert.IsTrue(result);
            Assert.AreEqual(PositionMode.Static, style.Position.PositionMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_Relative_SetsRelative()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "relative");

            Assert.IsTrue(result);
            Assert.AreEqual(PositionMode.Relative, style.Position.PositionMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_Absolute_SetsAbsolute()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "absolute");

            Assert.IsTrue(result);
            Assert.AreEqual(PositionMode.Absolute, style.Position.PositionMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_Fixed_SetsFixed()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "fixed");

            Assert.IsTrue(result);
            Assert.AreEqual(PositionMode.Fixed, style.Position.PositionMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "ABSOLUTE");

            Assert.IsTrue(result);
            Assert.AreEqual(PositionMode.Absolute, style.Position.PositionMode);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionMode_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPositionModeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-position");

            Assert.IsFalse(result);
        }

        #endregion

        #region Position Float Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionFloat_None_SetsNone()
        {
            var parser = new CSSPositionFloatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(FloatMode.None, style.Position.Float);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionFloat_Left_SetsLeft()
        {
            var parser = new CSSPositionFloatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "left");

            Assert.IsTrue(result);
            Assert.AreEqual(FloatMode.Left, style.Position.Float);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionFloat_Right_SetsRight()
        {
            var parser = new CSSPositionFloatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "right");

            Assert.IsTrue(result);
            Assert.AreEqual(FloatMode.Right, style.Position.Float);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void PositionFloat_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSPositionFloatParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-float");

            Assert.IsFalse(result);
        }

        #endregion

        #region Hyphens Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Hyphens_Auto_SetsAuto()
        {
            var parser = new CSSHyphensParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(WordHyphenation.Auto, style.Text.Hyphenation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Hyphens_None_SetsNone()
        {
            var parser = new CSSHyphensParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            Assert.AreEqual(WordHyphenation.None, style.Text.Hyphenation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Hyphens_CaseInsensitive_ParsesCorrectly()
        {
            var parser = new CSSHyphensParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "AUTO");

            Assert.IsTrue(result);
            Assert.AreEqual(WordHyphenation.Auto, style.Text.Hyphenation);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Hyphens_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSHyphensParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-hyphen");

            Assert.IsFalse(result);
        }

        #endregion

        #region Hyphenate Limits Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateLimits_SingleValue_SetsMinLength()
        {
            var parser = new CSSHyphenateLimitsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "6");

            Assert.IsTrue(result);
            Assert.AreEqual(6, style.Text.HyphenationLimitChars);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateLimits_TwoValues_SetsMinLengthAndBefore()
        {
            var parser = new CSSHyphenateLimitsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "6 2");

            Assert.IsTrue(result);
            Assert.AreEqual(6, style.Text.HyphenationLimitChars);
            Assert.AreEqual(2, style.Text.HyphenationMinCharsBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateLimits_ThreeValues_SetsAll()
        {
            var parser = new CSSHyphenateLimitsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "6 2 3");

            Assert.IsTrue(result);
            Assert.AreEqual(6, style.Text.HyphenationLimitChars);
            Assert.AreEqual(2, style.Text.HyphenationMinCharsBefore);
            Assert.AreEqual(3, style.Text.HyphenationMinCharsAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateLimits_AutoValue_SetsAuto()
        {
            var parser = new CSSHyphenateLimitsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Text.HyphenationLimitChars); // AutoValue
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateLimits_AutoInValues_SetsAutoForPosition()
        {
            var parser = new CSSHyphenateLimitsParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto 2 3");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Text.HyphenationLimitChars); // AutoValue
            Assert.AreEqual(2, style.Text.HyphenationMinCharsBefore);
            Assert.AreEqual(3, style.Text.HyphenationMinCharsAfter);
        }

        #endregion

        #region Hyphenate Min Length Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateMinLength_IntegerValue_SetsLength()
        {
            var parser = new CSSHyphenateMinLengthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "5");

            Assert.IsTrue(result);
            Assert.AreEqual(5, style.Text.HyphenationLimitChars);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateMinLength_AutoValue_SetsAuto()
        {
            var parser = new CSSHyphenateMinLengthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Text.HyphenationLimitChars); // AutoValue
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphenateMinLength_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSHyphenateMinLengthParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-length");

            Assert.IsFalse(result);
        }

        #endregion

        #region Hyphens Min Before Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinBefore_IntegerValue_SetsBefore()
        {
            var parser = new CSSHyphensMinBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "2");

            Assert.IsTrue(result);
            Assert.AreEqual(2, style.Text.HyphenationMinCharsBefore);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinBefore_AutoValue_SetsAuto()
        {
            var parser = new CSSHyphensMinBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Text.HyphenationMinCharsBefore); // AutoValue
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinBefore_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSHyphensMinBeforeParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-before");

            Assert.IsFalse(result);
        }

        #endregion

        #region Hyphens Min After Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinAfter_IntegerValue_SetsAfter()
        {
            var parser = new CSSHyphensMinAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "3");

            Assert.IsTrue(result);
            Assert.AreEqual(3, style.Text.HyphenationMinCharsAfter);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinAfter_AutoValue_SetsAuto()
        {
            var parser = new CSSHyphensMinAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            Assert.IsTrue(result);
            Assert.AreEqual(-1, style.Text.HyphenationMinCharsAfter); // AutoValue
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void HyphensMinAfter_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSHyphensMinAfterParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-after");

            Assert.IsFalse(result);
        }

        #endregion

        #region Font Display Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontDisplay_AnyValue_ReturnsTrue()
        {
            var parser = new CSSFontDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "swap");

            // FontDisplayParser is a skip parser - always returns true
            Assert.IsTrue(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontDisplay_Auto_ReturnsTrue()
        {
            var parser = new CSSFontDisplayParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "auto");

            // Skip parser - no validation, always succeeds
            Assert.IsTrue(result);
        }

        #endregion

        #region Font Source Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSource_UrlValue_ParsesSource()
        {
            var parser = new CSSFontSourceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url('fonts/myfont.ttf')");

            Assert.IsTrue(result);
            
            var source = style.GetValue(StyleKeys.FontFaceSrcKey, null);
            Assert.IsNotNull(source);
            Assert.AreEqual("url('fonts/myfont.ttf')", source.Source);
            Assert.AreEqual(FontSourceFormat.Default, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNull(source.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSource_UrlWithFormat_ParsesSource()
        {
            var parser = new CSSFontSourceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url('myfont.woff') format('woff')");

            Assert.IsTrue(result);
            
            var source = style.GetValue(StyleKeys.FontFaceSrcKey, null);
            Assert.IsNotNull(source);
            Assert.AreEqual("url('myfont.woff')", source.Source);
            Assert.AreEqual(FontSourceFormat.WOFF, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNull(source.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSource_MultipleUrls_ParsesAll()
        {
            var parser = new CSSFontSourceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url('font.woff2'), url('font.woff'), url('font.ttf')");

            Assert.IsTrue(result);
            
            // First source should be set, with Next chain for fallbacks
            
            var source = style.GetValue(StyleKeys.FontFaceSrcKey, null);
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.woff2')", source.Source);
            Assert.AreEqual(FontSourceFormat.Default, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
            source = source.Next;
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.woff')", source.Source);
            Assert.AreEqual(FontSourceFormat.Default, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
            source = source.Next;
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.ttf')", source.Source);
            Assert.AreEqual(FontSourceFormat.Default, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
        }
        
        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSource_MultipleUrlsAndFormats_ParsesAll()
        {
            var parser = new CSSFontSourceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "url('font.woff2') format('woff2'), url('font.woff') format('woff'), url('font.ttf') format('ttf')");

            Assert.IsTrue(result);
            
            // First source should be set, with Next chain for fallbacks and formats
            
            var source = style.GetValue(StyleKeys.FontFaceSrcKey, null);
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.woff2')", source.Source);
            Assert.AreEqual(FontSourceFormat.WOFF2, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
            source = source.Next;
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.woff')", source.Source);
            Assert.AreEqual(FontSourceFormat.WOFF, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
            source = source.Next;
            Assert.IsNotNull(source);
            Assert.AreEqual("url('font.ttf')", source.Source);
            Assert.AreEqual(FontSourceFormat.TrueType, source.Format);
            Assert.IsFalse(source.Mapped);
            Assert.IsNotNull(source.Next);
            
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontSource_InvalidValue_ReturnsFalse()
        {
            var parser = new CSSFontSourceParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "invalid-source");

            Assert.IsFalse(result);
        }

        #endregion

        #region Font Stretch Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStretch_AnyValue_ReturnsTrue()
        {
            var parser = new CSSFontStretchParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "condensed");

            // FontStretchParser is a skip parser - always returns true
            Assert.IsTrue(result);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void FontStretch_Expanded_ReturnsTrue()
        {
            var parser = new CSSFontStretchParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "expanded");

            // Skip parser - no validation, always succeeds
            Assert.IsTrue(result);
        }

        #endregion

        #region Content Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Content_StringValue_SetsContent()
        {
            var parser = new CSSContentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "'Chapter '");

            Assert.IsTrue(result);
            var content = style.GetValue(StyleKeys.ContentTextKey, null);
            Assert.IsNotNull(content);
            Assert.AreEqual(ContentDescriptorType.Text, content.Type);
            Assert.AreEqual("'Chapter '", ((ContentTextDescriptor)content).Text);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Content_CounterValue_SetsContent()
        {
            var parser = new CSSContentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "counter(chapter)");

            Assert.IsTrue(result);
            var content = style.GetValue(StyleKeys.ContentTextKey, null);
            Assert.IsNotNull(content);
            Assert.AreEqual(ContentDescriptorType.Counter, content.Type);
            Assert.AreEqual("chapter", ((ContentCounterDescriptor)content).CounterName);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Content_MultipleValues_SetsContent()
        {
            var parser = new CSSContentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "'Chapter ' counter(chapter) '. '");

            Assert.IsTrue(result);
            var content = style.GetValue(StyleKeys.ContentTextKey, null);
            Assert.IsNotNull(content);
            Assert.AreEqual(ContentDescriptorType.Text, content.Type);
            Assert.AreEqual("'Chapter '", ((ContentTextDescriptor)content).Text);
            Assert.IsNotNull(content.Next);
            
            content = content.Next;
            Assert.AreEqual(ContentDescriptorType.Counter, content.Type);
            Assert.AreEqual("chapter", ((ContentCounterDescriptor)content).CounterName);
            Assert.IsNotNull(content.Next);
            
            content = content.Next;
            Assert.AreEqual(ContentDescriptorType.Text, content.Type);
            Assert.AreEqual(". ", ((ContentTextDescriptor)content).Text);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void Content_AttrValue_SetsContent()
        {
            var parser = new CSSContentParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "attr(data-label)");

            Assert.IsTrue(result);
            var content = style.GetValue(StyleKeys.ContentTextKey, null);
            Assert.IsNotNull(content);
            Assert.AreEqual(ContentDescriptorType.Attribute, content.Type);
            Assert.AreEqual("data-label", ((ContentAttributeDescriptor)content).Attribute);
            Assert.IsNotNull(content.Next);
        }

        #endregion

        #region Counter Reset Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterReset_NameOnly_SetsCounter()
        {
            var parser = new CSSCounterResetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterResetKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNull(counter.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterReset_NameAndValue_SetsCounter()
        {
            var parser = new CSSCounterResetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter 5");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterResetKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(5, counter.Value);
            Assert.IsNull(counter.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterReset_MultipleCounters_SetsBoth()
        {
            var parser = new CSSCounterResetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter 1 section 0");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterResetKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(1, counter.Value);
            Assert.IsNotNull(counter.Next);
            
            counter = counter.Next;
            Assert.IsNotNull(counter);
            Assert.AreEqual("section", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNotNull(counter.Next);
            
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterReset_None_SetsNone()
        {
            var parser = new CSSCounterResetParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterResetKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("none", counter.Name);
            Assert.AreEqual(0, counter.Value);
            Assert.IsNotNull(counter.Next);
        }

        #endregion

        #region Counter Increment Parser Tests

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterIncrement_NameOnly_SetsCounter()
        {
            var parser = new CSSCounterIncrementParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterIncrementKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(1, counter.Value); //default for increment is 1
            Assert.IsNull(counter.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterIncrement_NameAndValue_SetsCounter()
        {
            var parser = new CSSCounterIncrementParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter 2");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterIncrementKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(2, counter.Value); //explicit 2 increment
            Assert.IsNull(counter.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterIncrement_MultipleCounters_SetsBoth()
        {
            var parser = new CSSCounterIncrementParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "chapter 1 section 1");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterIncrementKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("chapter", counter.Name);
            Assert.AreEqual(1, counter.Value);
            Assert.IsNotNull(counter.Next);
            
            counter = counter.Next;
            Assert.IsNotNull(counter);
            Assert.AreEqual("section", counter.Name);
            Assert.AreEqual(1, counter.Value);
            Assert.IsNotNull(counter.Next);
        }

        [TestMethod()]
        [TestCategory("CSS")]
        [TestCategory("CSS-Parsers")]
        public void CounterIncrement_None_SetsNone()
        {
            var parser = new CSSCounterIncrementParser();
            var style = CreateStyle();
            var result = ParseValue(parser, style, "none");

            Assert.IsTrue(result);
            var counter = style.GetValue(StyleKeys.CounterIncrementKey, null);
            Assert.IsNotNull(counter);
            Assert.AreEqual("none", counter.Name);
        }

        #endregion
    }
}
