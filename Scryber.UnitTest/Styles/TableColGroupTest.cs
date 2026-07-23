using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    /// <summary>
    /// Basic tests for the HTMLCol / HTMLColGroup components (col and colgroup table tags) - property
    /// defaults, HTML attribute parsing, and coded table construction.
    ///
    /// A span of N covers exactly N columns; an unset span (-1) defaults to covering exactly 1 column.
    /// LayoutEngineTable now assigns each col/colgroup's ColumnOffset once per table layout and merges
    /// matching column styles/classes onto each cell's applied style without overwriting the cell's own
    /// values (see Scryber.UnitLayouts/TableColGroup_Tests.cs for the thorough layout-effect coverage,
    /// including document class-selector rules matched via a col's class attribute).
    /// </summary>
    [TestClass()]
    public class TableColGroupTest
    {
        #region Property Tests (Category A)

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_Span_DefaultValue()
        {
            var col = new HTMLCol();
            Assert.AreEqual(-1, col.Span, "Default (unset) span should be -1");
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_Span_SetAndGet()
        {
            var col = new HTMLCol();
            col.Span = 2;
            Assert.AreEqual(2, col.Span);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_ColumnOffset_DefaultValue()
        {
            var col = new HTMLCol();
            Assert.AreEqual(-1, col.ColumnOffset, "Default (unset) column offset should be -1");
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_ColumnOffset_SetAndGet()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 4;
            Assert.AreEqual(4, col.ColumnOffset);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_Width_SetAndGet()
        {
            var col = new HTMLCol();
            col.Width = new Unit(75, PageUnits.Points);
            Assert.AreEqual(new Unit(75, PageUnits.Points), col.Width);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_BackgroundColor_SetAndGet()
        {
            var col = new HTMLCol();
            col.BackgroundColor = StandardColors.Aqua;
            Assert.AreEqual(StandardColors.Aqua, col.BackgroundColor);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_HorizontalAlignment_SetAndGet()
        {
            var col = new HTMLCol();
            col.HorizontalAlignment = HorizontalAlignment.Center;
            Assert.AreEqual(HorizontalAlignment.Center, col.HorizontalAlignment);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_VerticalAlignment_SetAndGet()
        {
            var col = new HTMLCol();
            col.VerticalAlignment = VerticalAlignment.Middle;
            Assert.AreEqual(VerticalAlignment.Middle, col.VerticalAlignment);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLColGroup_HasColumns_FalseByDefault()
        {
            var group = new HTMLColGroup();
            Assert.IsFalse(group.HasColumns);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLColGroup_HasColumns_TrueAfterAddingCol()
        {
            var group = new HTMLColGroup();
            group.Columns.Add(new HTMLCol());
            Assert.IsTrue(group.HasColumns);
            Assert.AreEqual(1, group.Columns.Count);
        }

        [TestMethod()]
        [TestCategory("Style Values")]
        public void HTMLCol_ApplyStyleToColumnCell_NoColumnOffset_Throws()
        {
            var col = new HTMLCol();
            col.Span = 0;
            Assert.ThrowsException<InvalidOperationException>(() => col.ApplyStyleToColumnCell(0, new TableCell()));
        }

        #endregion

        #region HTML Parser Tests (Category C)

        /// <summary>
        /// Parses a table with a single colgroup containing two explicit col elements and verifies
        /// the HTML attributes (span, width, align, valign, bgcolor, class) are read onto the parsed
        /// HTMLCol instances, and that the colgroup/cols do not affect the table's row/cell counts.
        /// </summary>
        [TestMethod()]
        [TestCategory("HTML Parser")]
        public void TableColGroup_Parser_ColGroupWithTwoCols_WithVerification()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <colgroup>
                                <col span=""2"" width=""50pt"" align=""center"" valign=""middle"" bgcolor=""#ff0000"" class=""first-col"" />
                                <col width=""100pt"" class=""second-col"" />
                            </colgroup>
                            <tr>
                                <td>Cell A</td>
                                <td>Cell B</td>
                            </tr>
                            <tr>
                                <td>Cell C</td>
                                <td>Cell D</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                Assert.IsNotNull(doc, "Document should be parsed successfully");
                Assert.IsTrue(doc.Pages.Count > 0, "Document should have at least one page");

                Section section = doc.Pages[0] as Section;
                Assert.IsNotNull(section, "First page should be a Section");

                TableGrid grid = null;
                foreach (var content in section.Contents)
                {
                    if (content is TableGrid tbl)
                    {
                        grid = tbl;
                        break;
                    }
                }
                Assert.IsNotNull(grid, "Section should contain a parsed TableGrid");

                // The colgroup/col elements should not appear as rows.
                Assert.AreEqual(2, grid.Rows.Count, "Table should have 2 rows - the colgroup must not be counted as a row");
                Assert.AreEqual(2, grid.Rows[0].Cells.Count, "Row 0 should have 2 cells");
                Assert.AreEqual(2, grid.Rows[1].Cells.Count, "Row 1 should have 2 cells");

                // Find the parsed colgroup amongst the table's raw content.
                HTMLColGroup colgroup = null;
                foreach (var content in ((IContainerComponent)grid).Content)
                {
                    if (content is HTMLColGroup grp)
                    {
                        colgroup = grp;
                        break;
                    }
                }
                Assert.IsNotNull(colgroup, "Table content should contain the parsed HTMLColGroup");
                Assert.IsTrue(colgroup.HasColumns, "Colgroup should have parsed col children");
                Assert.AreEqual(2, colgroup.Columns.Count, "Colgroup should have 2 col children");

                HTMLCol col0 = colgroup.Columns[0];
                Assert.AreEqual(2, col0.Span, "First col should have span=2");
                Assert.AreEqual(new Unit(50, PageUnits.Points), col0.Width, "First col should have width=50pt");
                Assert.AreEqual(HorizontalAlignment.Center, col0.HorizontalAlignment, "First col should have align=center");
                Assert.AreEqual(VerticalAlignment.Middle, col0.VerticalAlignment, "First col should have valign=middle");
                Assert.AreEqual(new Color(255, 0, 0), col0.BackgroundColor, "First col should have bgcolor=#ff0000");
                Assert.AreEqual("first-col", col0.StyleClass, "First col should have class=first-col");

                HTMLCol col1 = colgroup.Columns[1];
                Assert.AreEqual(new Unit(100, PageUnits.Points), col1.Width, "Second col should have width=100pt");
                Assert.AreEqual("second-col", col1.StyleClass, "Second col should have class=second-col");
            }
        }

        /// <summary>
        /// Parses a table with a colgroup that has an explicit span and no nested col elements
        /// (the HTML shorthand for N identical columns) and verifies the group's own attributes parse.
        /// </summary>
        [TestMethod()]
        [TestCategory("HTML Parser")]
        public void TableColGroup_Parser_ColGroupWithSpanShorthand_WithVerification()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <colgroup span=""3"" bgcolor=""#00ff00""></colgroup>
                            <tr>
                                <td>A</td>
                                <td>B</td>
                                <td>C</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                Section section = doc.Pages[0] as Section;
                TableGrid grid = null;
                foreach (var content in section.Contents)
                {
                    if (content is TableGrid tbl) { grid = tbl; break; }
                }
                Assert.IsNotNull(grid, "Section should contain a parsed TableGrid");
                Assert.AreEqual(1, grid.Rows.Count, "The colgroup shorthand should not be counted as a row");

                HTMLColGroup colgroup = null;
                foreach (var content in ((IContainerComponent)grid).Content)
                {
                    if (content is HTMLColGroup grp) { colgroup = grp; break; }
                }
                Assert.IsNotNull(colgroup, "Table content should contain the parsed HTMLColGroup");
                Assert.AreEqual(3, colgroup.Span, "Colgroup should have span=3");
                Assert.IsFalse(colgroup.HasColumns, "Colgroup with a span shorthand should have no col children");
                Assert.AreEqual(new Color(0, 255, 0), colgroup.BackgroundColor, "Colgroup should have bgcolor=#00ff00");
            }
        }

        #endregion

        #region Coded Table Tests (Category B)

        /// <summary>
        /// Builds a table entirely in code (TableGrid/TableRow/TableCell), adds an HTMLColGroup with
        /// per-column HTMLCol definitions as raw content on the table, and confirms the table's row/cell
        /// structure is unaffected by their presence.
        /// </summary>
        [TestMethod()]
        [TestCategory("Coded Table")]
        public void TableColGroup_CodedTable_ColGroupDoesNotAffectRowsOrCells()
        {
            var doc = new Document();
            var section = new Section();
            doc.Pages.Add(section);

            var grid = new TableGrid();
            section.Contents.Add(grid);

            var group = new HTMLColGroup();
            var col0 = new HTMLCol { Span = 0 };
            var col1 = new HTMLCol { Span = 0 };
            group.Columns.Add(col0);
            group.Columns.Add(col1);

            // Add the colgroup as raw content on the table (as the HTML parser would).
            ((IContainerComponent)grid).Content.Add(group);

            for (int r = 0; r < 2; r++)
            {
                var row = new TableRow();
                grid.Rows.Add(row);
                for (int c = 0; c < 2; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("R" + r + "C" + c));
                    row.Cells.Add(cell);
                }
            }

            Assert.AreEqual(2, grid.Rows.Count, "Rows should only include the TableRow instances, not the colgroup");
            Assert.AreEqual(2, grid.Rows[0].Cells.Count);
            Assert.AreEqual(2, grid.Rows[1].Cells.Count);
        }

        /// <summary>
        /// A coded table with a colgroup/col pair, where ApplyStyleToColumnCell is called directly against
        /// each cell (simulating the per-column application a layout engine integration would perform),
        /// verifying the resulting background colours and widths land on the correct cells.
        /// </summary>
        [TestMethod()]
        [TestCategory("Coded Table")]
        public void TableColGroup_CodedTable_ManuallyAppliedColumnStyles_LandOnCorrectCells()
        {
            var doc = new Document();
            var section = new Section();
            doc.Pages.Add(section);

            var grid = new TableGrid();
            section.Contents.Add(grid);

            var row = new TableRow();
            grid.Rows.Add(row);

            var cellA = new TableCell();
            var cellB = new TableCell();
            row.Cells.Add(cellA);
            row.Cells.Add(cellB);

            var group = new HTMLColGroup();
            group.ColumnOffset = 0; // the group's own offset - required before ApplyStyleToColumnCell will delegate to its children
            var colA = new HTMLCol { ColumnOffset = 0, Span = 1, BackgroundColor = StandardColors.Red, Width = new Unit(60, PageUnits.Points) };
            var colB = new HTMLCol { ColumnOffset = 1, Span = 1, BackgroundColor = StandardColors.Blue, Width = new Unit(120, PageUnits.Points) };
            group.Columns.Add(colA);
            group.Columns.Add(colB);
            ((IContainerComponent)grid).Content.Add(group);

            Assert.IsTrue(group.ApplyStyleToColumnCell(0, cellA));
            Assert.IsTrue(group.ApplyStyleToColumnCell(1, cellB));

            Assert.AreEqual(StandardColors.Red, cellA.BackgroundColor);
            Assert.AreEqual(new Unit(60, PageUnits.Points), cellA.Width);

            Assert.AreEqual(StandardColors.Blue, cellB.BackgroundColor);
            Assert.AreEqual(new Unit(120, PageUnits.Points), cellB.Width);
        }

        /// <summary>
        /// Basic smoke test for the table layout engine: a table containing a colgroup/col should still
        /// parse and lay out to a PDF without error, and the table's normal row/cell content should
        /// render as expected.
        /// </summary>
        [TestMethod()]
        [TestCategory("Coded Table")]
        public void TableColGroup_LayoutEngine_TableWithColGroupStillLaysOutSuccessfully()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <colgroup>
                                <col span=""3"" />
                            </colgroup>
                            <tr>
                                <td>One</td>
                                <td>Two</td>
                                <td>Three</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                using (var ms = new MemoryStream())
                {
                    doc.SaveAsPDF(ms);
                    Assert.IsTrue(ms.Length > 0, "A PDF should have been generated even with a colgroup present");
                }
            }
        }

        /// <summary>
        /// Basic end-to-end check that the layout engine now actually applies a colgroup's background
        /// colour to its cells - a real PDF-generation-level counterpart to the more thorough layout
        /// assertions in Scryber.UnitLayouts/TableColGroup_Tests.cs.
        /// </summary>
        [TestMethod()]
        [TestCategory("Coded Table")]
        public void TableColGroup_LayoutEngine_ColGroupBackgroundColorAppliesToCells()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <colgroup>
                                <col bgcolor=""#ff0000"" />
                                <col bgcolor=""#0000ff"" />
                            </colgroup>
                            <tr>
                                <td>One</td>
                                <td>Two</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            PDFLayoutDocument layout = null;
            void OnLayoutComplete(object sender, LayoutEventArgs args) => layout = args.Context.GetLayout<PDFLayoutDocument>();

            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                using (var ms = new MemoryStream())
                {
                    doc.LayoutComplete += OnLayoutComplete;
                    doc.SaveAsPDF(ms);
                    Assert.IsTrue(ms.Length > 0);
                }
            }

            var tblBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var cellA = rowBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            var cellB = rowBlock.Columns[1].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(new Color(0xff, 0, 0), cellA.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent),
                "Cell A's layout should reflect the first col's background colour");
            Assert.AreEqual(new Color(0, 0, 0xff), cellB.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent),
                "Cell B's layout should reflect the second col's background colour");
        }

        #endregion
    }
}
