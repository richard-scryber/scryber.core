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

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_Subgrid_NotSupported_ReturnsFalse()
        {
            // subgrid is not supported - the parser must reject it (rather than storing it as
            // an opaque track-list string that would be silently misinterpreted at layout
            // time), so the property is left unset.
            var parser = new CSSGridTemplateColumnsParser();
            var style = CreateStyle();
            Assert.IsFalse(ParseValue(parser, style, "subgrid"));
            Assert.IsNull(style.Grid.TemplateColumns);
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

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_Subgrid_NotSupported_ReturnsFalse()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style = CreateStyle();
            Assert.IsFalse(ParseValue(parser, style, "subgrid"));
            Assert.IsNull(style.Grid.TemplateRows);
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

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridRow_SpanNamedLine()
        {
            var parser = new CSSGridRowParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2 / span row-end"));
            Assert.AreEqual(2, style.Grid.RowStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsNamed && style.Grid.RowEnd.IsSpan);
            Assert.AreEqual("row-end", style.Grid.RowEnd.Name);
        }

        // -----------------------------------------------------------------------
        // grid-template-columns / grid-template-rows with [name] tokens
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_WithNamedLines()
        {
            var parser = new CSSGridTemplateColumnsParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "[col-start] 1fr [col-mid] 1fr [col-end]"));
            Assert.IsNotNull(style.Grid.TemplateColumns);
            StringAssert.Contains(style.Grid.TemplateColumns, "col-start");
            StringAssert.Contains(style.Grid.TemplateColumns, "col-end");
            StringAssert.Contains(style.Grid.TemplateColumns, "1fr");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateRows_WithNamedLines()
        {
            var parser = new CSSGridTemplateRowsParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "[row-start] 50pt [row-mid] 80pt [row-end]"));
            Assert.IsNotNull(style.Grid.TemplateRows);
            StringAssert.Contains(style.Grid.TemplateRows, "row-start");
            StringAssert.Contains(style.Grid.TemplateRows, "80pt");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateColumns_WithNamedLines_FullCSS()
        {
            var allParser = new CSSStyleItemAllParser();
            var style = new Style();
            var reader = new CSSStyleItemReader("grid-template-columns: [main-start] 200pt [content-start] 1fr [content-end] 200pt [main-end]");
            reader.ReadNextAttributeName();
            allParser.SetStyleValue(style, reader, null);
            Assert.IsNotNull(style.Grid.TemplateColumns);
            StringAssert.Contains(style.Grid.TemplateColumns, "main-start");
            StringAssert.Contains(style.Grid.TemplateColumns, "content-start");
        }

        // -----------------------------------------------------------------------
        // grid-area — single identifier (named template area)
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridArea_SingleName()
        {
            var parser = new CSSGridAreaParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "header"));
            Assert.AreEqual("header", style.Grid.AreaName);
            Assert.IsTrue(style.Grid.ColumnStart.IsUnset, "No positional keys set for named area");
            Assert.IsTrue(style.Grid.RowStart.IsUnset,    "No positional keys set for named area");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridArea_SingleName_Sidebar()
        {
            var parser = new CSSGridAreaParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "sidebar"));
            Assert.AreEqual("sidebar", style.Grid.AreaName);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridArea_FourPartSlash()
        {
            // grid-area: row-start / col-start / row-end / col-end
            var parser = new CSSGridAreaParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "1 / 2 / 3 / 4"));
            Assert.IsTrue(style.Grid.RowStart.IsExplicit);
            Assert.AreEqual(1, style.Grid.RowStart.Value);
            Assert.IsTrue(style.Grid.ColumnStart.IsExplicit);
            Assert.AreEqual(2, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsExplicit);
            Assert.AreEqual(3, style.Grid.RowEnd.Value);
            Assert.IsTrue(style.Grid.ColumnEnd.IsExplicit);
            Assert.AreEqual(4, style.Grid.ColumnEnd.Value);
            Assert.IsNull(style.Grid.AreaName, "4-part form must not set area name");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridArea_FourPartSlash_WithSpan()
        {
            var parser = new CSSGridAreaParser();
            var style  = CreateStyle();
            Assert.IsTrue(ParseValue(parser, style, "2 / 1 / span 2 / span 3"));
            Assert.AreEqual(2, style.Grid.RowStart.Value);
            Assert.AreEqual(1, style.Grid.ColumnStart.Value);
            Assert.IsTrue(style.Grid.RowEnd.IsSpan && style.Grid.RowEnd.Value == 2);
            Assert.IsTrue(style.Grid.ColumnEnd.IsSpan && style.Grid.ColumnEnd.Value == 3);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridArea_FullCSSParse()
        {
            var allParser = new CSSStyleItemAllParser();
            var style = new Style();
            var reader = new CSSStyleItemReader("grid-area: main");
            reader.ReadNextAttributeName();
            allParser.SetStyleValue(style, reader, null);
            Assert.AreEqual("main", style.Grid.AreaName);
        }

        // -----------------------------------------------------------------------
        // GridTemplateAreasValue struct — TryParse, TryGetAreaBounds, AreaNames
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryParse_TwoByTwo()
        {
            bool ok = Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);
            Assert.IsTrue(ok);
            Assert.AreEqual(2, areas.RowCount);
            Assert.AreEqual(2, areas.ColCount);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryParse_ThreeByThree()
        {
            bool ok = Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header header\" \"sidebar content content\" \"footer footer footer\"", out var areas);
            Assert.IsTrue(ok);
            Assert.AreEqual(3, areas.RowCount);
            Assert.AreEqual(3, areas.ColCount);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryParse_DotNotation()
        {
            bool ok = Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\". header .\" \"sidebar content content\"", out var areas);
            Assert.IsTrue(ok);
            Assert.AreEqual(2, areas.RowCount);
            Assert.AreEqual(3, areas.ColCount);

            // "header" starts at col 2, row 1
            bool found = areas.TryGetAreaBounds("header", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found);
            Assert.AreEqual(1, rs); Assert.AreEqual(2, re);
            Assert.AreEqual(2, cs); Assert.AreEqual(3, ce);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryParse_MultiDotEmpty()
        {
            bool ok = Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"....... header header\" \"sidebar content content\"", out var areas);
            Assert.IsTrue(ok);
            Assert.AreEqual(2, areas.RowCount);
            Assert.AreEqual(3, areas.ColCount);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryParse_Empty_ReturnsFalse()
        {
            bool ok = Scryber.Drawing.GridTemplateAreasValue.TryParse("", out _);
            Assert.IsFalse(ok);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryGetAreaBounds_Header()
        {
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);

            bool found = areas.TryGetAreaBounds("header", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found, "header area must be found");
            Assert.AreEqual(1, rs, "header row start = 1");
            Assert.AreEqual(2, re, "header row end   = 2 (exclusive)");
            Assert.AreEqual(1, cs, "header col start = 1");
            Assert.AreEqual(3, ce, "header col end   = 3 (spans cols 1-2)");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryGetAreaBounds_Sidebar()
        {
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);

            bool found = areas.TryGetAreaBounds("sidebar", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found);
            Assert.AreEqual(2, rs); Assert.AreEqual(3, re);
            Assert.AreEqual(1, cs); Assert.AreEqual(2, ce);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryGetAreaBounds_Main()
        {
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);

            bool found = areas.TryGetAreaBounds("main", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found);
            Assert.AreEqual(2, rs); Assert.AreEqual(3, re);
            Assert.AreEqual(2, cs); Assert.AreEqual(3, ce);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryGetAreaBounds_NotFound()
        {
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);

            bool found = areas.TryGetAreaBounds("footer", out _, out _, out _, out _);
            Assert.IsFalse(found, "Non-existent area must return false");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_TryGetAreaBounds_SpannedRows()
        {
            // sidebar spans both rows
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header header\" \"sidebar content content\" \"sidebar footer footer\"", out var areas);

            bool found = areas.TryGetAreaBounds("sidebar", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found);
            Assert.AreEqual(2, rs, "sidebar starts at row 2");
            Assert.AreEqual(4, re, "sidebar ends at row 4 (spans rows 2-3)");
            Assert.AreEqual(1, cs); Assert.AreEqual(2, ce);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_AreaNames_Order()
        {
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header\" \"sidebar main\"", out var areas);

            var names = new System.Collections.Generic.List<string>(areas.AreaNames());
            Assert.AreEqual(3, names.Count);
            Assert.AreEqual("header",  names[0]);
            Assert.AreEqual("sidebar", names[1]);
            Assert.AreEqual("main",    names[2]);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_AreaNames_NoDuplicates()
        {
            // header appears in both columns of row 1 — must appear only once
            Scryber.Drawing.GridTemplateAreasValue.TryParse(
                "\"header header header\" \"sidebar content content\" \"footer footer footer\"", out var areas);

            var names = new System.Collections.Generic.List<string>(areas.AreaNames());
            Assert.AreEqual(4, names.Count);
            Assert.AreEqual(4, new System.Collections.Generic.HashSet<string>(names).Count,
                "Each name must appear exactly once");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreasValue_IsEmpty_WhenDefault()
        {
            var areas = default(Scryber.Drawing.GridTemplateAreasValue);
            Assert.IsTrue(areas.IsEmpty);
        }

        // -----------------------------------------------------------------------
        // grid-template-areas via CSSStyleItemAllParser
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreas_FullCSSParse_TwoByTwo()
        {
            var allParser = new CSSStyleItemAllParser();
            var style = new Style();
            var reader = new CSSStyleItemReader("grid-template-areas: \"header header\" \"sidebar main\"");
            reader.ReadNextAttributeName();
            allParser.SetStyleValue(style, reader, null);

            Assert.IsFalse(style.Grid.TemplateAreas.IsEmpty);
            Assert.AreEqual(2, style.Grid.TemplateAreas.RowCount);
            Assert.AreEqual(2, style.Grid.TemplateAreas.ColCount);

            bool found = style.Grid.TemplateAreas.TryGetAreaBounds("header", out int rs, out int re, out int cs, out int ce);
            Assert.IsTrue(found);
            Assert.AreEqual(1, cs); Assert.AreEqual(3, ce);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridTemplateAreas_RoundTrip_RemoveRestoresDefault()
        {
            var style = CreateStyle();
            Scryber.Drawing.GridTemplateAreasValue.TryParse("\"header header\" \"sidebar main\"", out var areas);
            style.Grid.TemplateAreas = areas;
            Assert.IsFalse(style.Grid.TemplateAreas.IsEmpty);

            style.Grid.RemoveTemplateAreas();
            Assert.IsTrue(style.Grid.TemplateAreas.IsEmpty, "After remove TemplateAreas should be default/empty");
        }

        // -----------------------------------------------------------------------
        // GridLineValue struct edge cases
        // -----------------------------------------------------------------------

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Unset_IsUnset()
        {
            var v = Scryber.Drawing.GridLineValue.Unset;
            Assert.IsTrue(v.IsUnset);
            Assert.IsFalse(v.IsSet);
            Assert.IsFalse(v.IsAuto);
            Assert.IsFalse(v.IsExplicit);
            Assert.IsFalse(v.IsNamed);
            Assert.IsFalse(v.IsSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Auto_IsAuto()
        {
            var v = Scryber.Drawing.GridLineValue.Auto;
            Assert.IsTrue(v.IsAuto);
            Assert.IsTrue(v.IsSet);
            Assert.IsFalse(v.IsExplicit);
            Assert.IsFalse(v.IsNamed);
            Assert.IsFalse(v.IsSpan);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Line_IsExplicit()
        {
            var v = Scryber.Drawing.GridLineValue.Line(3);
            Assert.IsTrue(v.IsExplicit);
            Assert.AreEqual(3, v.Value);
            Assert.IsFalse(v.IsSpan);
            Assert.IsFalse(v.IsNamed);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Span_IsExplicitAndIsSpan()
        {
            var v = Scryber.Drawing.GridLineValue.Span(2);
            Assert.IsTrue(v.IsExplicit);
            Assert.IsTrue(v.IsSpan);
            Assert.AreEqual(2, v.Value);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Named_IsNamed()
        {
            var v = Scryber.Drawing.GridLineValue.Named("col-start");
            Assert.IsTrue(v.IsNamed);
            Assert.IsFalse(v.IsSpan);
            Assert.AreEqual("col-start", v.Name);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_Named_WithSpan()
        {
            var v = Scryber.Drawing.GridLineValue.Named("col-end", span: true);
            Assert.IsTrue(v.IsNamed);
            Assert.IsTrue(v.IsSpan);
            Assert.AreEqual("col-end", v.Name);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_ResolveStart_ExplicitLine()
        {
            var v = Scryber.Drawing.GridLineValue.Line(3);
            int start = v.ResolveStart(null);
            Assert.AreEqual(2, start, "1-based line 3 → 0-based index 2");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_ResolveStart_NamedLine()
        {
            var lineNames = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>>
            {
                { "col-start", new System.Collections.Generic.List<int> { 1 } },
                { "col-mid",   new System.Collections.Generic.List<int> { 2 } }
            };
            var v = Scryber.Drawing.GridLineValue.Named("col-mid");
            int start = v.ResolveStart(lineNames);
            Assert.AreEqual(1, start, "col-mid → line index 2 → 0-based index 1");
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_ResolveSpan_ExplicitSpan()
        {
            var v = Scryber.Drawing.GridLineValue.Span(3);
            int span = v.ResolveSpan(0, 4, null);
            Assert.AreEqual(3, span);
        }

        [TestMethod()][TestCategory("CSS")][TestCategory("CSS-Grid")]
        public void GridLineValue_ResolveSpan_EndLine()
        {
            // end line 4, start line 2 → span = 4-2 = 2
            var v = Scryber.Drawing.GridLineValue.Line(4);
            int span = v.ResolveSpan(1, 4, null); // resolvedStart=1 = 0-based line 2
            Assert.AreEqual(2, span);
        }
    }
}
