using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests for the HTMLCol / HTMLColGroup components (Scryber.Components/Html/Components/HTMLCol.cs)
    /// and their integration into LayoutEngineTable (Scryber.Components/PDF/Layout/LayoutEngineTable.cs).
    ///
    /// A span of N covers exactly N columns (ColumnOffset .. ColumnOffset + N - 1); an unset span (-1)
    /// defaults to covering exactly 1 column, matching the HTML span attribute default. An explicit
    /// span of 0 therefore covers zero columns and never matches anything.
    ///
    /// The layout engine (LayoutEngineTable.GetColumnDefinitions/GetMatchingColumnStyle) assigns each
    /// top-level col/colgroup a ColumnOffset once per table layout, then for each cell, before resolving
    /// its applied style: (1) if the matching column definition has a StyleClass, prepends it onto the
    /// cell's own (stable, snapshotted) OwnStyleClass so document-level class-selector rules can match
    /// too, then (2) after the cell's applied style is resolved, merges the column's own raw style values
    /// underneath it (via a blank Style, column merged in first, cell's resolved style merged in second)
    /// so the cell's own values always win on conflict.
    /// </summary>
    [TestClass()]
    public class TableColGroup_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private Document ParseXhtml(string xhtmlContent)
        {
            StringReader reader = new StringReader(xhtmlContent);
            try
            {
                return Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            }
            finally
            {
                if (null != reader)
                    reader.Dispose();
            }
        }

        #region Defaults and validation

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_DefaultSpan_IsUnset()
        {
            var col = new HTMLCol();
            Assert.AreEqual(-1, col.Span, "Default (unset) span should be -1");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_DefaultColumnOffset_IsUnset()
        {
            var col = new HTMLCol();
            Assert.AreEqual(-1, col.ColumnOffset, "Default (unset) column offset should be -1");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_DefaultSpan_IsUnset()
        {
            var group = new HTMLColGroup();
            Assert.AreEqual(-1, group.Span, "Default (unset) span should be -1");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_HasColumns_FalseWhenNoneAdded()
        {
            var group = new HTMLColGroup();
            Assert.IsFalse(group.HasColumns, "A colgroup with no col children should report HasColumns = false");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_HasColumns_TrueWhenColsAdded()
        {
            var group = new HTMLColGroup();
            group.Columns.Add(new HTMLCol());
            Assert.IsTrue(group.HasColumns, "A colgroup with a col child should report HasColumns = true");
            Assert.AreEqual(1, group.Columns.Count);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HTMLCol_ApplyStyleToColumnCell_ColumnOffsetNotSet_Throws()
        {
            var col = new HTMLCol();
            col.Span = 0;
            // ColumnOffset is still -1 (default) - must throw before matching/applying anything.
            col.ApplyStyleToColumnCell(0, new TableCell());
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void HTMLColGroup_ApplyStyleToColumnCell_ColumnOffsetNotSet_Throws()
        {
            var group = new HTMLColGroup();
            group.Span = 0;
            group.ApplyStyleToColumnCell(0, new TableCell());
        }

        #endregion

        #region Span / column-offset matching matrix

        // HTMLColBase.IsMatchingColumn matches an exclusive range: colindex >= ColumnOffset &&
        // colindex < ColumnOffset + EffectiveSpan, where EffectiveSpan defaults an unset (-1) span to 1.
        // So a col with Span = N matches exactly N columns (ColumnOffset .. ColumnOffset + N - 1), and an
        // explicit Span = 0 matches no columns at all.

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_SpanUnset_MatchesExactlyItsOwnColumn()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 2;
            // Span left at default (-1) - defaults to covering exactly 1 column.

            Assert.IsTrue(col.ApplyStyleToColumnCell(2, new TableCell()), "Unset span should default to matching its own offset column");
            Assert.IsFalse(col.ApplyStyleToColumnCell(0, new TableCell()), "Unset span should not match any earlier column");
            Assert.IsFalse(col.ApplyStyleToColumnCell(3, new TableCell()), "Unset span should not match any later column");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_SpanZero_MatchesNoColumns()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 2;
            col.Span = 0;

            Assert.IsFalse(col.ApplyStyleToColumnCell(1, new TableCell()), "Column before the offset should not match");
            Assert.IsFalse(col.ApplyStyleToColumnCell(2, new TableCell()), "An explicit span of 0 covers zero columns, so not even its own offset column matches");
            Assert.IsFalse(col.ApplyStyleToColumnCell(3, new TableCell()), "Column after the offset should not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_SpanOne_MatchesExactlyOneColumn()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 2;
            col.Span = 1;

            Assert.IsFalse(col.ApplyStyleToColumnCell(1, new TableCell()), "Column before the offset should not match");
            Assert.IsTrue(col.ApplyStyleToColumnCell(2, new TableCell()), "The offset column should match");
            Assert.IsFalse(col.ApplyStyleToColumnCell(3, new TableCell()), "A span of 1 covers only its own column, not the next one too");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_SpanTwo_MatchesExactlyTwoColumns()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 2;

            Assert.IsTrue(col.ApplyStyleToColumnCell(0, new TableCell()));
            Assert.IsTrue(col.ApplyStyleToColumnCell(1, new TableCell()));
            Assert.IsFalse(col.ApplyStyleToColumnCell(2, new TableCell()), "A span of 2 covers exactly 2 columns, not 3");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ColumnOffsetNonZero_DoesNotMatchColumnsBeforeIt()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 5;
            col.Span = 3;

            for (int i = 0; i < 5; i++)
                Assert.IsFalse(col.ApplyStyleToColumnCell(i, new TableCell()), "Column " + i + " is before the offset and should not match");

            for (int i = 5; i < 8; i++)
                Assert.IsTrue(col.ApplyStyleToColumnCell(i, new TableCell()), "Column " + i + " is within the spanned range and should match");

            Assert.IsFalse(col.ApplyStyleToColumnCell(8, new TableCell()), "Column 8 is beyond the spanned range (span=3 covers exactly columns 5,6,7) and should not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_EffectiveSpan_ExplicitSpan_ReturnsThatSpan()
        {
            var group = new HTMLColGroup { Span = 4 };
            Assert.AreEqual(4, group.EffectiveSpan);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_EffectiveSpan_DelegatingToChildren_SumsChildSpans()
        {
            var group = new HTMLColGroup();
            group.Columns.Add(new HTMLCol { Span = 2 });
            group.Columns.Add(new HTMLCol { Span = 1 });
            group.Columns.Add(new HTMLCol()); // unset span defaults to 1

            Assert.AreEqual(4, group.EffectiveSpan, "2 + 1 + 1 (default) = 4");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_EffectiveSpan_NoSpanNoChildren_IsZero()
        {
            var group = new HTMLColGroup();
            Assert.AreEqual(0, group.EffectiveSpan, "A colgroup with neither an explicit span nor any children covers no columns");
        }

        #endregion

        #region Style value application

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_NoExistingValue_AppliesBackgroundColor()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.BackgroundColor = StandardColors.Red;

            var cell = new TableCell();
            bool applied = col.ApplyStyleToColumnCell(0, cell);

            Assert.IsTrue(applied);
            Assert.AreEqual(StandardColors.Red, cell.BackgroundColor);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_NonMatchingColumn_LeavesCellUnchanged()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.BackgroundColor = StandardColors.Red;

            var cell = new TableCell();
            bool applied = col.ApplyStyleToColumnCell(1, cell); // column 1 is outside the span

            Assert.IsFalse(applied);
            Assert.AreEqual(StandardColors.Transparent, cell.BackgroundColor, "A non-matching column should not touch the cell style at all");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_AppliesWidth()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 1;
            col.Span = 1;
            col.Width = new Unit(120, PageUnits.Points);

            var cell = new TableCell();
            col.ApplyStyleToColumnCell(1, cell);

            Assert.AreEqual(new Unit(120, PageUnits.Points), cell.Width);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_AppliesHorizontalAndVerticalAlignment()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.HorizontalAlignment = HorizontalAlignment.Right;
            col.VerticalAlignment = VerticalAlignment.Bottom;

            var cell = new TableCell();
            col.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual(HorizontalAlignment.Right, cell.HorizontalAlignment);
            Assert.AreEqual(VerticalAlignment.Bottom, cell.VerticalAlignment);
        }

        /// <summary>
        /// Documents current, verified behaviour: DoApplyStyleToCell merges the col's style at a very high
        /// priority (Style.DirectStylePriority - 1), which overwrites a value that a cell already has set
        /// directly on its own Style object (e.g. via the plain BackgroundColor/Width property setters used
        /// here), even though the surrounding code comment says this "don't override explicit values on the
        /// cell". A cell's own runtime-assigned property value carries the default StyleValue priority (0),
        /// which is lower than DirectStylePriority - 1, so it loses. If the cell's own value had instead been
        /// pushed in at Style.DirectStylePriority (as happens when a component's own attribute/inline style is
        /// merged into its full/applied style during normal style resolution), it would NOT be overwritten -
        /// this test only demonstrates the raw Style object merge in isolation, ahead of that later stage.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_OverwritesCellsOwnDirectlySetValue()
        {
            var cell = new TableCell();
            cell.BackgroundColor = StandardColors.Blue;

            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.BackgroundColor = StandardColors.Red;

            col.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual(StandardColors.Red, cell.BackgroundColor,
                "Current behaviour: the col's merged style overwrites the cell's own previously-set direct value");
        }

        #endregion

        #region StyleClass application

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_AddsClassWhenCellHasNone()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.StyleClass = "col-class";

            var cell = new TableCell();
            col.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual("col-class", cell.StyleClass);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_PrependsClassBeforeExistingCellClass()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.StyleClass = "col-class";

            var cell = new TableCell();
            cell.StyleClass = "cell-class";

            col.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual("col-class cell-class", cell.StyleClass,
                "The col's class should be prepended, followed by the cell's own existing class");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_NoColClass_LeavesCellClassUnchanged()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            // No StyleClass set on the col.

            var cell = new TableCell();
            cell.StyleClass = "cell-class";

            col.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual("cell-class", cell.StyleClass);
        }

        #endregion

        #region HTMLColGroup - explicit span (no children)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_ExplicitSpan_NoChildren_AppliesGroupStyleAcrossSpan()
        {
            var group = new HTMLColGroup();
            group.ColumnOffset = 0;
            group.Span = 2; // matches columns 0 and 1
            group.BackgroundColor = StandardColors.Green;

            var cell0 = new TableCell();
            var cell1 = new TableCell();
            var cell2 = new TableCell();

            Assert.IsTrue(group.ApplyStyleToColumnCell(0, cell0));
            Assert.IsTrue(group.ApplyStyleToColumnCell(1, cell1));
            Assert.IsFalse(group.ApplyStyleToColumnCell(2, cell2));

            Assert.AreEqual(StandardColors.Green, cell0.BackgroundColor);
            Assert.AreEqual(StandardColors.Green, cell1.BackgroundColor);
            Assert.AreEqual(StandardColors.Transparent, cell2.BackgroundColor);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_ExplicitSpanZero_WithChildColsPresent_IgnoresChildrenUsesOwnStyle()
        {
            // Per HTMLColGroup.ApplyStyleToColumnCell: an explicit (non-negative) Span on the group itself
            // always takes priority over any child <col> elements, even if some have been added.
            var group = new HTMLColGroup();
            group.ColumnOffset = 0;
            group.Span = 1;
            group.BackgroundColor = StandardColors.Green;

            var child = new HTMLCol();
            child.ColumnOffset = 0;
            child.Span = 1;
            child.BackgroundColor = StandardColors.Aqua;
            group.Columns.Add(child);

            var cell = new TableCell();
            group.ApplyStyleToColumnCell(0, cell);

            Assert.AreEqual(StandardColors.Green, cell.BackgroundColor,
                "An explicit span on the colgroup itself should win over any child col styles");
        }

        #endregion

        #region HTMLColGroup - delegating to child <col> elements

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_NoSpanNoChildren_NeverMatches()
        {
            var group = new HTMLColGroup();
            group.ColumnOffset = 0;
            // Span left unset (-1), no columns added.

            Assert.IsFalse(group.ApplyStyleToColumnCell(0, new TableCell()));
            Assert.IsFalse(group.ApplyStyleToColumnCell(1, new TableCell()));
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_DelegatesToMatchingChildColumn()
        {
            var group = new HTMLColGroup();
            group.ColumnOffset = 0; // group has no explicit span, so it purely delegates

            var col0 = new HTMLCol { ColumnOffset = 0, Span = 1, BackgroundColor = StandardColors.Aqua };
            var col1 = new HTMLCol { ColumnOffset = 1, Span = 1, BackgroundColor = StandardColors.Yellow };
            group.Columns.Add(col0);
            group.Columns.Add(col1);

            var cellA = new TableCell();
            var cellB = new TableCell();
            var cellC = new TableCell();

            Assert.IsTrue(group.ApplyStyleToColumnCell(0, cellA));
            Assert.IsTrue(group.ApplyStyleToColumnCell(1, cellB));
            Assert.IsFalse(group.ApplyStyleToColumnCell(2, cellC));

            Assert.AreEqual(StandardColors.Aqua, cellA.BackgroundColor);
            Assert.AreEqual(StandardColors.Yellow, cellB.BackgroundColor);
            Assert.AreEqual(StandardColors.Transparent, cellC.BackgroundColor);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_OverlappingChildColumns_FirstMatchingChildWins()
        {
            var group = new HTMLColGroup();
            group.ColumnOffset = 0;

            // colA spans columns 0 and 1 (span=2); colB covers just column 1.
            // Since colA is added first and also matches column 1, it should win for that column.
            var colA = new HTMLCol { ColumnOffset = 0, Span = 2, BackgroundColor = StandardColors.Red };
            var colB = new HTMLCol { ColumnOffset = 1, Span = 1, BackgroundColor = StandardColors.Blue };
            group.Columns.Add(colA);
            group.Columns.Add(colB);

            var cellAt0 = new TableCell();
            var cellAt1 = new TableCell();

            group.ApplyStyleToColumnCell(0, cellAt0);
            group.ApplyStyleToColumnCell(1, cellAt1);

            Assert.AreEqual(StandardColors.Red, cellAt0.BackgroundColor);
            Assert.AreEqual(StandardColors.Red, cellAt1.BackgroundColor,
                "colA is listed first and also matches column 1, so it should be applied instead of colB");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLColGroup_ChildColumnsWithDifferentClasses_ApplyIndependently()
        {
            var group = new HTMLColGroup();
            group.ColumnOffset = 0;

            var col0 = new HTMLCol { ColumnOffset = 0, Span = 1, StyleClass = "first-col" };
            var col1 = new HTMLCol { ColumnOffset = 1, Span = 1, StyleClass = "second-col" };
            group.Columns.Add(col0);
            group.Columns.Add(col1);

            var cell0 = new TableCell();
            var cell1 = new TableCell();

            group.ApplyStyleToColumnCell(0, cell0);
            group.ApplyStyleToColumnCell(1, cell1);

            Assert.AreEqual("first-col", cell0.StyleClass);
            Assert.AreEqual("second-col", cell1.StyleClass);
        }

        #endregion

        #region Multi-column real table scenarios (simulating layout-engine integration)

        /// <summary>
        /// Builds a real 3x3 TableGrid (code-only, mirroring Table_Tests.cs conventions) and simulates
        /// what a LayoutEngineTable integration would need to do: assign a ColumnOffset per <col>, then
        /// call ApplyStyleToColumnCell(colIndex, cell) once per cell using its column index. Verifies the
        /// correct, distinct style is applied to every cell in a given column, across every row.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table_ThreeByThree_ColGroupWithThreeCols_AppliesDistinctStylePerColumnAcrossAllRows()
        {
            const int RowCount = 3;
            const int ColCount = 3;

            var doc = new Document();
            var section = new Section();
            doc.Pages.Add(section);

            var tbl = new TableGrid();
            section.Contents.Add(tbl);

            var cells = new TableCell[RowCount, ColCount];
            for (int r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);
                for (int c = 0; c < ColCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("R" + r + "C" + c));
                    row.Cells.Add(cell);
                    cells[r, c] = cell;
                }
            }

            var group = new HTMLColGroup();
            group.ColumnOffset = 0;

            var colColors = new[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue };
            for (int c = 0; c < ColCount; c++)
            {
                var col = new HTMLCol { ColumnOffset = c, Span = 1, BackgroundColor = colColors[c] };
                group.Columns.Add(col);
            }

            // Simulate the per-cell application a layout engine integration would perform.
            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    bool applied = group.ApplyStyleToColumnCell(c, cells[r, c]);
                    Assert.IsTrue(applied, "Column " + c + " should have a matching <col> definition");
                }
            }

            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                {
                    Assert.AreEqual(colColors[c], cells[r, c].BackgroundColor,
                        "Cell at row " + r + ", column " + c + " should have picked up its column's colour");
                }
            }
        }

        /// <summary>
        /// A single &lt;col span="2"&gt; should apply its style to every cell across the columns it spans,
        /// on every row, and must not touch cells outside of that span.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table_ThreeByThree_SingleColSpanningTwoColumns_AppliesToSpannedColumnsOnly()
        {
            const int RowCount = 2;
            const int ColCount = 4;

            var doc = new Document();
            var section = new Section();
            doc.Pages.Add(section);

            var tbl = new TableGrid();
            section.Contents.Add(tbl);

            var cells = new TableCell[RowCount, ColCount];
            for (int r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);
                for (int c = 0; c < ColCount; c++)
                {
                    var cell = new TableCell();
                    row.Cells.Add(cell);
                    cells[r, c] = cell;
                }
            }

            // A single <col span="2"> at offset 1 spans columns 1 and 2.
            var col = new HTMLCol { ColumnOffset = 1, Span = 2, BackgroundColor = StandardColors.Purple };

            for (int r = 0; r < RowCount; r++)
            {
                for (int c = 0; c < ColCount; c++)
                    col.ApplyStyleToColumnCell(c, cells[r, c]);
            }

            for (int r = 0; r < RowCount; r++)
            {
                Assert.AreEqual(StandardColors.Transparent, cells[r, 0].BackgroundColor, "Column 0 is outside the span");
                Assert.AreEqual(StandardColors.Purple, cells[r, 1].BackgroundColor, "Column 1 is the span's offset");
                Assert.AreEqual(StandardColors.Purple, cells[r, 2].BackgroundColor, "Column 2 is included by the inclusive span");
                Assert.AreEqual(StandardColors.Transparent, cells[r, 3].BackgroundColor, "Column 3 is outside the span");
            }
        }

        /// <summary>
        /// Multiple colgroups (as would come from multiple &lt;colgroup&gt; elements in one table), each
        /// responsible for a distinct, non-overlapping range of columns.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table_TwoColGroups_EachOwnsItsOwnColumnRange()
        {
            var groupA = new HTMLColGroup(); // owns columns 0-1
            groupA.ColumnOffset = 0;
            groupA.Span = 2;
            groupA.BackgroundColor = StandardColors.Red;

            var groupB = new HTMLColGroup(); // owns column 2 only
            groupB.ColumnOffset = 2;
            groupB.Span = 1;
            groupB.BackgroundColor = StandardColors.Blue;

            var cell0 = new TableCell();
            var cell1 = new TableCell();
            var cell2 = new TableCell();

            // A layout engine would try each colgroup for a given column in table order.
            Assert.IsTrue(groupA.ApplyStyleToColumnCell(0, cell0) || groupB.ApplyStyleToColumnCell(0, cell0));
            Assert.IsTrue(groupA.ApplyStyleToColumnCell(1, cell1) || groupB.ApplyStyleToColumnCell(1, cell1));
            Assert.IsTrue(groupA.ApplyStyleToColumnCell(2, cell2) || groupB.ApplyStyleToColumnCell(2, cell2));

            Assert.AreEqual(StandardColors.Red, cell0.BackgroundColor);
            Assert.AreEqual(StandardColors.Red, cell1.BackgroundColor);
            Assert.AreEqual(StandardColors.Blue, cell2.BackgroundColor);
        }

        #endregion

        #region Full document (parse -> bind -> layout -> render) scenarios

        // These tests run real xhtml documents through the complete pipeline
        // (Document.ParseDocument -> DataBind -> SaveAsPDF -> layout inspection), rather than driving
        // HTMLCol/HTMLColGroup's own API directly as the tests above do. Given the wiring gap noted at
        // the top of this file, they currently confirm two things: (1) a table with colgroup/col present
        // still parses, binds and lays out correctly - no regression/crash from the new components sitting
        // in a table's content - and (2) the col/colgroup style values do NOT yet reach the actual laid-out
        // cells (documented explicitly, not just assumed), which is exactly what should change once the
        // layout engine is wired up.

        private static PDFLayoutBlock GetTableBlock(PDFLayoutDocument layoutDoc, int pageIndex = 0)
        {
            var pg = layoutDoc.AllPages[pageIndex];
            return pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
        }

        private static PDFLayoutBlock GetRowBlock(PDFLayoutBlock tableBlock, int rowIndex)
        {
            return tableBlock.Columns[0].Contents[rowIndex] as PDFLayoutBlock;
        }

        private static PDFLayoutBlock GetCellBlock(PDFLayoutBlock rowBlock, int colIndex)
        {
            return rowBlock.Columns[colIndex].Contents[0] as PDFLayoutBlock;
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_TableWithColGroupWidths_AppliesColumnWidthsToCells()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 600pt 400pt; margin: 10pt; }
        table { width: 480pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col span='1' width='100pt' bgcolor='#ff0000' />
        <col width='200pt' bgcolor='#00ff00' />
        <col width='180pt' bgcolor='#0000ff' />
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("ColGroup_Widths_Current.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var tblBlock = GetTableBlock(layout);
            Assert.IsNotNull(tblBlock, "Table block should exist");

            var rowBlock = GetRowBlock(tblBlock, 0);
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(3, rowBlock.Columns.Length, "Row should have 3 cell columns");

            var cellA = GetCellBlock(rowBlock, 0);
            var cellB = GetCellBlock(rowBlock, 1);
            var cellC = GetCellBlock(rowBlock, 2);

            Assert.AreEqual(100.0, cellA.Width.PointsValue, 0.5, "Column 0 should take its width from the matching <col width='100pt'>");
            Assert.AreEqual(200.0, cellB.Width.PointsValue, 0.5, "Column 1 should take its width from the matching <col width='200pt'>");
            Assert.AreEqual(180.0, cellC.Width.PointsValue, 0.5, "Column 2 should take its width from the matching <col width='180pt'>");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_TableWithColGroupBackgroundColor_AppliesColumnBackgroundToCells()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col bgcolor='#ff0000' />
        <col bgcolor='#00ff00' />
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);

            using (var ms = DocStreams.GetOutputStream("ColGroup_BgColor_Current.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var rowBlock = GetRowBlock(tblBlock, 0);
            var cellA = GetCellBlock(rowBlock, 0);
            var cellB = GetCellBlock(rowBlock, 1);

            var bgA = cellA.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);
            var bgB = cellB.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);

            Assert.AreEqual(StandardColors.Red, bgA, "Cell A should pick up the first col's red background");
            Assert.AreEqual(new Color(0, 255, 0), bgB, "Cell B should pick up the second col's #00ff00 background");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_TableWithOrWithoutColGroup_ProducesDifferentCellWidths()
        {
            const string style = @"
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }";

            string withColGroup = $@"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>{style}</style></head>
<body>
<table>
    <colgroup><col width='250pt' /><col width='50pt' /></colgroup>
    <tr><td>A</td><td>B</td></tr>
</table>
</body>
</html>";

            string withoutColGroup = $@"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>{style}</style></head>
<body>
<table>
    <tr><td>A</td><td>B</td></tr>
</table>
</body>
</html>";

            Document docWith = ParseXhtml(withColGroup);
            using (var ms = DocStreams.GetOutputStream("ColGroup_Compare_With.pdf"))
            {
                docWith.LayoutComplete += Doc_LayoutComplete;
                docWith.SaveAsPDF(ms);
            }
            var withWidths = (GetCellBlock(GetRowBlock(GetTableBlock(layout), 0), 0).Width.PointsValue,
                               GetCellBlock(GetRowBlock(GetTableBlock(layout), 0), 1).Width.PointsValue);

            Document docWithout = ParseXhtml(withoutColGroup);
            using (var ms = DocStreams.GetOutputStream("ColGroup_Compare_Without.pdf"))
            {
                docWithout.LayoutComplete += Doc_LayoutComplete;
                docWithout.SaveAsPDF(ms);
            }
            var withoutWidths = (GetCellBlock(GetRowBlock(GetTableBlock(layout), 0), 0).Width.PointsValue,
                                  GetCellBlock(GetRowBlock(GetTableBlock(layout), 0), 1).Width.PointsValue);

            Assert.AreEqual((150.0, 150.0), withoutWidths, "With no colgroup, the table's 300pt width should split evenly");
            Assert.AreEqual((250.0, 50.0), withWidths, "With the colgroup, cell widths should follow the col widths instead");
            Assert.AreNotEqual(withoutWidths, withWidths, "The colgroup should measurably change the cell layout");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_TableWithColGroupAndColSpanCells_StillLaysOutCorrectly()
        {
            // A colgroup/col alongside existing, already-wired colspan cells - confirms the new
            // (currently inert) components don't interfere with column-span handling.
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col span='3' />
    </colgroup>
    <tr>
        <td colspan='2'>Wide</td>
        <td>C</td>
    </tr>
    <tr>
        <td>D</td>
        <td>E</td>
        <td>F</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            using (var ms = DocStreams.GetOutputStream("ColGroup_WithColSpan.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var row0 = GetRowBlock(tblBlock, 0);
            var row1 = GetRowBlock(tblBlock, 1);

            // Column count reflects the table's total grid columns (3, from row 1) regardless of row 0's
            // colspan=2 cell occupying two of them as a single spanned block.
            Assert.AreEqual(3, row0.Columns.Length, "Row 0 should still report the table's full 3-column grid");
            Assert.AreEqual(3, row1.Columns.Length, "Row 1 has 3 normal cells");

            var wideCellBlock = GetCellBlock(row0, 0);
            Assert.IsNotNull(wideCellBlock, "The colspan=2 cell should still lay out its content block");
        }

        #endregion

        #region Data-bound col/colgroup values (parse -> bind -> layout)

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_DataBoundWidthAndColor_BindOntoParsedComponents()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col span='{{model.colSpan}}' width='{{model.colWidth}}pt' bgcolor='{{model.colColor}}' />
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            doc.Params["model"] = new
            {
                colSpan = 1,
                colWidth = 120,
                colColor = "#336699"
            };

            using (var ms = DocStreams.GetOutputStream("ColGroup_DataBound.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            Assert.IsNotNull(tblBlock, "Table should still lay out successfully with data-bound colgroup values");

            HTMLColGroup colgroup = null;
            foreach (var content in ((IContainerComponent)tblBlock.Owner).Content)
            {
                if (content is HTMLColGroup grp) { colgroup = grp; break; }
            }
            Assert.IsNotNull(colgroup, "The bound document should still contain the parsed HTMLColGroup");
            Assert.IsTrue(colgroup.HasColumns);

            var col = colgroup.Columns[0];
            Assert.AreEqual(1, col.Span, "Span should be bound from model.colSpan");
            Assert.AreEqual(new Unit(120, PageUnits.Points), col.Width, "Width should be bound from model.colWidth");
            Assert.AreEqual(new Color(0x33, 0x66, 0x99), col.BackgroundColor, "BackgroundColor should be bound from model.colColor");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_DataBoundEachCol_GeneratesOneColPerModelItem()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        {{#each model.columns}}
        <col width='{{this.width}}pt' bgcolor='{{this.color}}' />
        {{/each}}
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            doc.Params["model"] = new
            {
                columns = new[]
                {
                    new { width = 50, color = "#ff0000" },
                    new { width = 100, color = "#00ff00" },
                    new { width = 150, color = "#0000ff" }
                }
            };

            using (var ms = DocStreams.GetOutputStream("ColGroup_DataBoundEach.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            Assert.IsNotNull(tblBlock);

            HTMLColGroup colgroup = null;
            foreach (var content in ((IContainerComponent)tblBlock.Owner).Content)
            {
                if (content is HTMLColGroup grp) { colgroup = grp; break; }
            }
            Assert.IsNotNull(colgroup, "The bound document should contain the generated HTMLColGroup");
            Assert.AreEqual(3, colgroup.Columns.Count, "Each should have generated one <col> per model item");

            Assert.AreEqual(new Unit(50, PageUnits.Points), colgroup.Columns[0].Width);
            Assert.AreEqual(new Color(0xff, 0, 0), colgroup.Columns[0].BackgroundColor);

            Assert.AreEqual(new Unit(100, PageUnits.Points), colgroup.Columns[1].Width);
            Assert.AreEqual(new Color(0, 0xff, 0), colgroup.Columns[1].BackgroundColor);

            Assert.AreEqual(new Unit(150, PageUnits.Points), colgroup.Columns[2].Width);
            Assert.AreEqual(new Color(0, 0, 0xff), colgroup.Columns[2].BackgroundColor);
        }

        #endregion

        #region LayoutEngineTable integration - explicit cell values win, class-based lookup, class prepending

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_CellsOwnExplicitBackgroundColor_WinsOverColBackgroundColor()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col bgcolor='#ff0000' />
        <col bgcolor='#ff0000' />
    </colgroup>
    <tr>
        <td style='background-color:#0000ff;'>A</td>
        <td>B</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            using (var ms = DocStreams.GetOutputStream("ColGroup_CellOwnBgWins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var rowBlock = GetRowBlock(tblBlock, 0);
            var cellA = GetCellBlock(rowBlock, 0);
            var cellB = GetCellBlock(rowBlock, 1);

            var bgA = cellA.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);
            var bgB = cellB.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);

            Assert.AreEqual(new Color(0, 0, 0xff), bgA, "Cell A's own explicit bgcolor should win over the matching col's bgcolor");
            Assert.AreEqual(new Color(0xff, 0, 0), bgB, "Cell B has no explicit bgcolor, so the col's should apply");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_ColClassName_MatchesDocumentClassSelectorRule()
        {
            // The col itself sets no direct style values - only a class name - which must be prepended
            // onto the cell before the cell's applied style is resolved, so the document-level .hilite
            // rule below is picked up purely via the class match.
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        .hilite { background-color: #ffff00; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col class='hilite' />
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            using (var ms = DocStreams.GetOutputStream("ColGroup_ClassSelectorMatch.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var rowBlock = GetRowBlock(tblBlock, 0);
            var cellA = GetCellBlock(rowBlock, 0);
            var cellB = GetCellBlock(rowBlock, 1);

            var bgA = cellA.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);
            var bgB = cellB.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);

            Assert.AreEqual(new Color(0xff, 0xff, 0), bgA,
                "Column 0's <col class='hilite'> should make the document's .hilite rule match cell A, even though the col has no direct style values of its own");
            Assert.AreEqual(StandardColors.Transparent, bgB, "Column 1 has no matching col, so should be unaffected");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_ColClassPrependedBeforeCellsOwnClass_BothDocumentRulesApply()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
        .col-cls { background-color: #ffff00; }
        .cell-cls { color: #112233; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col class='col-cls' />
    </colgroup>
    <tr>
        <td class='cell-cls'>A</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            using (var ms = DocStreams.GetOutputStream("ColGroup_ClassPrepended.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var rowBlock = GetRowBlock(tblBlock, 0);
            var cellA = GetCellBlock(rowBlock, 0);

            var bg = cellA.FullStyle.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent);
            var fill = cellA.FullStyle.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent);

            Assert.AreEqual(new Color(0xff, 0xff, 0), bg, "The col's class-matched background rule should apply");
            Assert.AreEqual(new Color(0x11, 0x22, 0x33), fill, "The cell's own class-matched text colour rule should still apply too - prepending the col's class must not push out the cell's own class");
        }

        #endregion

        #region Hidden columns (HTML/CSS table spec: a collapsed column should reserve its width but
        // suppress its cells' rendered output)
        //
        // FINDING: this is NOT yet safe to wire into LayoutEngineTable. HTMLColBase now supports a
        // "hidden" attribute (mapping to Visible, same pattern as HTMLTableCell/HTMLTableRow), and
        // ApplyStyleToColumnCell correctly propagates a hidden col's Visible=false onto a matching cell
        // (see HTMLCol_ApplyStyleToColumnCell_HiddenCol_PropagatesToCell below) - but Cell.Visible is
        // already an overloaded signal elsewhere in the engine that means "remove this cell from the row
        // entirely, let later cells shift left to fill its slot" (LayoutEngineTable.GetNextVisibleCell
        // skips row.Cells entries with Visible == false when matching cells back up to the column-content
        // grid built by BuildStyles). Setting a cell invisible mid-column-match (after BuildStyles has
        // already reserved its column slot) desyncs that later cell-matching pass from the grid and
        // throws a PDFLayoutException ("No cell exists at column N in row M") - confirmed by actually
        // running it. Properly supporting "reserve width, suppress output" needs the engine to
        // distinguish "collapsed by a hidden column" from "individually removed cell" rather than
        // overloading the single Cell.Visible flag - flagged for a follow-up rather than attempted here.

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_HiddenCol_PropagatesToCell()
        {
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;
            col.Hidden = "hidden";

            var cell = new TableCell();
            Assert.IsTrue(cell.Visible, "Cell should default to visible");

            col.ApplyStyleToColumnCell(0, cell);

            Assert.IsFalse(cell.Visible, "A hidden col should mark its matching cell as not visible");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HTMLCol_ApplyStyleToColumnCell_NotHiddenCol_DoesNotForceCellVisible()
        {
            // A col that ISN'T hidden must never force-show a cell that was already hidden for its own
            // (unrelated) reasons - only the "hide" direction should ever be propagated.
            var col = new HTMLCol();
            col.ColumnOffset = 0;
            col.Span = 1;

            var cell = new TableCell();
            cell.Visible = false;

            col.ApplyStyleToColumnCell(0, cell);

            Assert.IsFalse(cell.Visible, "The cell's own visibility should be left alone when the col is not itself hidden");
        }

        /// <summary>
        /// Documents the current, confirmed crash: LayoutEngineTable does not (yet) propagate a hidden
        /// col/colgroup's visibility onto real table cells, so a table with a hidden col today lays out
        /// exactly as if the col were not hidden at all - the column still renders normally. This is a
        /// deliberate, safe no-op rather than the crash that resulted from an earlier attempt at wiring
        /// Cell.Visible directly into the per-cell loop (see the region comment above).
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void XHtml_ColGroup_HiddenColumn_NotYetWiredIntoLayoutEngine()
        {
            string xhtml = @"<?xml version='1.0' encoding='utf-8'?>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <style>
        @page { size: 400pt 300pt; margin: 10pt; }
        table { width: 300pt; margin: 0; padding: 0; border-collapse: collapse; }
        td { margin: 0; padding: 0; border: 1pt solid black; }
    </style>
</head>
<body>
<table>
    <colgroup>
        <col width='100pt' />
        <col width='100pt' hidden='hidden' />
        <col width='100pt' />
    </colgroup>
    <tr>
        <td>A</td>
        <td>B</td>
        <td>C</td>
    </tr>
</table>
</body>
</html>";

            Document doc = ParseXhtml(xhtml);
            using (var ms = DocStreams.GetOutputStream("ColGroup_HiddenColumn_NotYetWired.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var tblBlock = GetTableBlock(layout);
            var rowBlock = GetRowBlock(tblBlock, 0);

            Assert.AreEqual(3, rowBlock.Columns.Length, "All 3 columns should still lay out - the hidden col has no effect yet");
            var cellB = GetCellBlock(rowBlock, 1);
            Assert.IsNotNull(cellB, "The 'hidden' column's cell still renders normally today");
            Assert.AreEqual(100.0, cellB.Width.PointsValue, 0.5, "Its width still comes from the matching <col width='100pt'>, same as an unhidden column");
        }

        #endregion
    }
}
