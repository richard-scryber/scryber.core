using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;
using Scryber.Styles.Parsing;
using Scryber.Styles.Parsing.Typed;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Html.CSSParsers
{
    /// <summary>
    /// Tests for CSS grid parsers: grid-template-columns, grid-template-rows.
    /// </summary>
    [TestClass()]
    public class CSSGridParsers_Tests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private Style CreateStyle() => new Style();

        private bool ParseValue(CSSStyleValueParser parser, Style style, string cssValue)
        {
            var reader = new CSSStyleItemReader(cssValue);
            return parser.SetStyleValue(style, reader, null);
        }

        // -----------------------------------------------------------------------
        // grid-template-columns
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_FrUnits()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1fr 2fr"));
            Assert.AreEqual("1fr 2fr", style.Grid.TemplateColumns);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_EqualFrUnits()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1fr 1fr 1fr"));
            Assert.AreEqual("1fr 1fr 1fr", style.Grid.TemplateColumns);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_FixedUnits()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "200pt 100pt"));
            Assert.AreEqual("200pt 100pt", style.Grid.TemplateColumns);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_MixedUnits()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "200pt 1fr"));
            Assert.AreEqual("200pt 1fr", style.Grid.TemplateColumns);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_RepeatNotation()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            // The reader tokenises on whitespace, so repeat(3, 1fr) arrives as two tokens.
            Assert.IsTrue(ParseValue(parser, style, "repeat(3, 1fr)"));
            Assert.IsNotNull(style.Grid.TemplateColumns);
            StringAssert.Contains(style.Grid.TemplateColumns, "repeat");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_SingleColumn()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1fr"));
            Assert.AreEqual("1fr", style.Grid.TemplateColumns);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_OverwriteValue()
        {
            // Second parse should overwrite the first value stored in the style.
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            ParseValue(parser, style, "1fr 1fr");
            Assert.AreEqual("1fr 1fr", style.Grid.TemplateColumns);

            var reader = new CSSStyleItemReader("1fr 2fr 1fr");
            parser.SetStyleValue(style, reader, null);
            Assert.AreEqual("1fr 2fr 1fr", style.Grid.TemplateColumns, "Second parse should overwrite first");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_FourColumns()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1fr 2fr 1fr 1fr"));
            Assert.AreEqual("1fr 2fr 1fr 1fr", style.Grid.TemplateColumns);
        }

        // -----------------------------------------------------------------------
        // grid-template-rows
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_FrUnits()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1fr 2fr"));
            Assert.AreEqual("1fr 2fr", style.Grid.TemplateRows);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_FixedUnits()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "100pt 200pt"));
            Assert.AreEqual("100pt 200pt", style.Grid.TemplateRows);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_SingleRow()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "50pt"));
            Assert.AreEqual("50pt", style.Grid.TemplateRows);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_OverwriteValue()
        {
            // Second parse should overwrite the first value.
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            ParseValue(parser, style, "100pt");
            Assert.AreEqual("100pt", style.Grid.TemplateRows);

            var reader = new CSSStyleItemReader("100pt 200pt");
            parser.SetStyleValue(style, reader, null);
            Assert.AreEqual("100pt 200pt", style.Grid.TemplateRows, "Second parse should overwrite first");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_ThreeRows()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "100pt 200pt 50pt"));
            Assert.AreEqual("100pt 200pt 50pt", style.Grid.TemplateRows);
        }

        // -----------------------------------------------------------------------
        // Full CSS string round-trip via CSSStyleItemAllParser
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_FullCSSParse()
        {
            var allParser = new CSSStyleItemAllParser();
            var style = new Style();
            var reader = new CSSStyleItemReader("grid-template-columns: 1fr 2fr");
            reader.ReadNextAttributeName();
            allParser.SetStyleValue(style, reader, null);
            Assert.AreEqual("1fr 2fr", style.Grid.TemplateColumns,
                "Full CSS parse should store grid-template-columns in style.Grid");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_FullCSSParse()
        {
            var allParser = new CSSStyleItemAllParser();
            var style = new Style();
            var reader = new CSSStyleItemReader("grid-template-rows: 100pt 200pt");
            reader.ReadNextAttributeName();
            allParser.SetStyleValue(style, reader, null);
            Assert.AreEqual("100pt 200pt", style.Grid.TemplateRows,
                "Full CSS parse should store grid-template-rows in style.Grid");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_IndependentOfRows()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            ParseValue(parser, style, "1fr 1fr");
            Assert.IsNull(style.Grid.TemplateRows, "Setting columns should not affect rows");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_IndependentOfColumns()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            ParseValue(parser, style, "100pt");
            Assert.IsNull(style.Grid.TemplateColumns, "Setting rows should not affect columns");
        }

        // -----------------------------------------------------------------------
        // Remove / round-trip on GridStyle directly
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RemoveTemplateColumns()
        {
            var style = CreateStyle();
            style.Grid.TemplateColumns = "1fr 2fr";
            Assert.AreEqual("1fr 2fr", style.Grid.TemplateColumns);

            style.Grid.RemoveTemplateColumns();
            Assert.IsNull(style.Grid.TemplateColumns, "After RemoveTemplateColumns should be null");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RemoveTemplateRows()
        {
            var style = CreateStyle();
            style.Grid.TemplateRows = "100pt 200pt";
            Assert.AreEqual("100pt 200pt", style.Grid.TemplateRows);

            style.Grid.RemoveTemplateRows();
            Assert.IsNull(style.Grid.TemplateRows, "After RemoveTemplateRows should be null");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_ColumnSpanDefault()
        {
            var style = CreateStyle();
            Assert.AreEqual(1, style.Grid.ColumnSpan, "Default ColumnSpan should be 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RowSpanDefault()
        {
            var style = CreateStyle();
            Assert.AreEqual(1, style.Grid.RowSpan, "Default RowSpan should be 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_ColumnSpanRoundTrip()
        {
            var style = CreateStyle();
            style.Grid.ColumnSpan = 3;
            Assert.AreEqual(3, style.Grid.ColumnSpan);

            style.Grid.RemoveColumnSpan();
            Assert.AreEqual(1, style.Grid.ColumnSpan, "After remove ColumnSpan should revert to 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RowSpanRoundTrip()
        {
            var style = CreateStyle();
            style.Grid.RowSpan = 2;
            Assert.AreEqual(2, style.Grid.RowSpan);

            style.Grid.RemoveRowSpan();
            Assert.AreEqual(1, style.Grid.RowSpan, "After remove RowSpan should revert to 1");
        }

        // -----------------------------------------------------------------------
        // grid-column parser
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_SpanN()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "span 3"));
            Assert.AreEqual(3, style.Grid.ColumnSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_PlainInteger_SpanIsOne()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2"));
            Assert.AreEqual(1, style.Grid.ColumnSpan, "Plain start line → span defaults to 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_StartSlashSpanN()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / span 2"));
            Assert.AreEqual(2, style.Grid.ColumnSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_StartSlashEnd_ComputesSpan()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2 / 4"));
            Assert.AreEqual(2, style.Grid.ColumnSpan, "Lines 2→4 = span 2");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_SpanOne()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "span 1"));
            Assert.AreEqual(1, style.Grid.ColumnSpan);
        }

        // -----------------------------------------------------------------------
        // grid-row parser
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_SpanN()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "span 2"));
            Assert.AreEqual(2, style.Grid.RowSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_PlainInteger_SpanIsOne()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1"));
            Assert.AreEqual(1, style.Grid.RowSpan, "Plain start line → span defaults to 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_StartSlashSpanN()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / span 3"));
            Assert.AreEqual(3, style.Grid.RowSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_StartSlashEnd_ComputesSpan()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / 3"));
            Assert.AreEqual(2, style.Grid.RowSpan, "Lines 1→3 = span 2");
        }
    }
}
