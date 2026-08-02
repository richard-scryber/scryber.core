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
        public void GridStyle_ColumnStartDefault()
        {
            var style = CreateStyle();
            Assert.IsTrue(style.Grid.ColumnStart.IsUnset, "Default ColumnStart should be Unset");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RowStartDefault()
        {
            var style = CreateStyle();
            Assert.IsTrue(style.Grid.RowStart.IsUnset, "Default RowStart should be Unset");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_ColumnEndRoundTrip()
        {
            var style = CreateStyle();
            style.Grid.ColumnEnd = Scryber.Drawing.GridLineValue.Span(3);
            Assert.IsTrue(style.Grid.ColumnEnd.IsSpan);
            Assert.AreEqual(3, style.Grid.ColumnEnd.Value);

            style.Grid.RemoveColumnEnd();
            Assert.IsTrue(style.Grid.ColumnEnd.IsUnset, "After remove ColumnEnd should revert to Unset");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridStyle_RowEndRoundTrip()
        {
            var style = CreateStyle();
            style.Grid.RowEnd = Scryber.Drawing.GridLineValue.Span(2);
            Assert.IsTrue(style.Grid.RowEnd.IsSpan);
            Assert.AreEqual(2, style.Grid.RowEnd.Value);

            style.Grid.RemoveRowEnd();
            Assert.IsTrue(style.Grid.RowEnd.IsUnset, "After remove RowEnd should revert to Unset");
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
            Assert.IsTrue(style.Grid.ColumnStart.IsSpan, "span keyword → IsSpan");
            Assert.AreEqual(3, style.Grid.ColumnStart.Value, "span 3 → Value=3");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_PlainInteger()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2"));
            Assert.IsTrue(style.Grid.ColumnStart.IsExplicit, "Integer → IsExplicit");
            Assert.AreEqual(2, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.ColumnEnd.IsUnset, "No end specified → Unset");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_StartSlashSpanN()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / span 2"));
            Assert.IsTrue(style.Grid.ColumnStart.IsExplicit && !style.Grid.ColumnStart.IsSpan);
            Assert.AreEqual(1, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.ColumnEnd.IsSpan, "Right side span → IsSpan");
            Assert.AreEqual(2, style.Grid.ColumnEnd.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_StartSlashEndLine()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2 / 4"));
            Assert.AreEqual(2, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.ColumnEnd.IsExplicit && !style.Grid.ColumnEnd.IsSpan);
            Assert.AreEqual(4, style.Grid.ColumnEnd.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_SpanOne()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "span 1"));
            Assert.IsTrue(style.Grid.ColumnStart.IsSpan);
            Assert.AreEqual(1, style.Grid.ColumnStart.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_NamedLine()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "col-start / col-end"));
            Assert.IsTrue(style.Grid.ColumnStart.IsNamed);
            Assert.AreEqual("col-start", style.Grid.ColumnStart.Name);
            Assert.IsTrue(style.Grid.ColumnEnd.IsNamed);
            Assert.AreEqual("col-end", style.Grid.ColumnEnd.Name);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridColumn_SpanNamedLine()
        {
            var parser = new CSSGridColumnParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / span col-end"));
            Assert.AreEqual(1, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.ColumnEnd.IsNamed && style.Grid.ColumnEnd.IsSpan);
            Assert.AreEqual("col-end", style.Grid.ColumnEnd.Name);
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
            Assert.IsTrue(style.Grid.RowStart.IsSpan);
            Assert.AreEqual(2, style.Grid.RowStart.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_PlainInteger()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1"));
            Assert.IsTrue(style.Grid.RowStart.IsExplicit && !style.Grid.RowStart.IsSpan);
            Assert.AreEqual(1, style.Grid.RowStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsUnset);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_StartSlashSpanN()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / span 3"));
            Assert.AreEqual(1, style.Grid.RowStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsSpan);
            Assert.AreEqual(3, style.Grid.RowEnd.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_StartSlashEndLine()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / 3"));
            Assert.AreEqual(1, style.Grid.RowStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsExplicit && !style.Grid.RowEnd.IsSpan);
            Assert.AreEqual(3, style.Grid.RowEnd.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_NamedLine()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "row-start / row-end"));
            Assert.IsTrue(style.Grid.RowStart.IsNamed);
            Assert.AreEqual("row-start", style.Grid.RowStart.Name);
            Assert.IsTrue(style.Grid.RowEnd.IsNamed);
            Assert.AreEqual("row-end", style.Grid.RowEnd.Name);
        }
    }
}
