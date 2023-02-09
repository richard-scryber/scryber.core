using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class Table_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable1Row3Cells()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var tbl = new TableGrid();
            section.Contents.Add(tbl);

            var row = new TableRow();
            tbl.Rows.Add(row);

            for (var i = 0; i < CellCount; i++)
            {
                var cell = new TableCell();
                cell.Contents.Add(new TextLiteral("Cell " + i));
                cell.Width = CellWidth;
                cell.Height = CellHeight;
                row.Cells.Add(cell);

            }


            using (var ms = DocStreams.GetOutputStream("Table_1Row3Cells.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(CellWidth * CellCount, tblBlock.Width);
            Assert.AreEqual(CellHeight, tblBlock.Height);

            Assert.AreEqual(1, tblBlock.Columns[0].Contents.Count);

            var rowBlock = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(rowBlock);
            Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
            Assert.AreEqual(CellHeight, rowBlock.Height);

            Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            for (var i = 0; i < CellCount; i++)
            {
                var column = rowBlock.Columns[i];
                Assert.IsNotNull(column);

                Assert.AreEqual(1, column.Contents.Count);
                var cellBlock = column.Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(cellBlock);

                Assert.AreEqual(CellHeight, cellBlock.Height);
                Assert.AreEqual(CellWidth, cellBlock.Width);

                Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.AreEqual(3, line.Runs.Count);
                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual("Cell " + i, (line.Runs[1] as PDFTextRunCharacter).Characters);

            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable3Row3Cells()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;
            const int RowCount = 3;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var tbl = new TableGrid();
            section.Contents.Add(tbl);

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Width = CellWidth;
                    cell.Height = CellHeight;
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3Cells.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(CellWidth * CellCount, tblBlock.Width);
            Assert.AreEqual(CellHeight * RowCount, tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(CellWidth * CellCount, rowBlock.Width);
                Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    Assert.AreEqual(1, column.Contents.Count);
                    var cellBlock = column.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cellBlock);

                    Assert.AreEqual(CellHeight, cellBlock.Height);
                    Assert.AreEqual(CellWidth, cellBlock.Width);

                    Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                    var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.AreEqual(3, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);

                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable3Row3CellsFullWidth()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            //const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;
            const int RowCount = 3;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var tbl = new TableGrid();
            tbl.FullWidth = true;
            section.Contents.Add(tbl);

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    //cell.Width = CellWidth;
                    cell.Height = CellHeight;
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsFullWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(PageWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * RowCount, tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(PageWidth, rowBlock.Width);
                Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    Assert.AreEqual(1, column.Contents.Count);
                    var cellBlock = column.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cellBlock);

                    Assert.AreEqual(CellHeight, cellBlock.Height);
                    Assert.AreEqual((double)PageWidth / (double)CellCount, cellBlock.Width);

                    Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                    var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.AreEqual(3, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);

                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable3Row3CellsFixedTableWidth()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            //const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;
            const int RowCount = 3;
            const int TableWidth = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Height = CellHeight;
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsFixedTableWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * RowCount, tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(TableWidth, rowBlock.Width);
                Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    Assert.AreEqual(1, column.Contents.Count);
                    var cellBlock = column.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cellBlock);

                    Assert.AreEqual(CellHeight, cellBlock.Height);
                    Assert.AreEqual((double)TableWidth / (double)CellCount, cellBlock.Width);

                    Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                    var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.AreEqual(3, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);

                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable3Row3CellsFixedTableWidth1FixedCell()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            //const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;
            const int RowCount = 3;
            const int TableWidth = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));

                    //middle cell is half the width of the table
                    if (c == 1 && r == 1)
                    {
                        cell.Width = TableWidth / 2.0;
                        cell.Height = CellHeight * 2.0;
                    }
                    else
                        cell.Height = CellHeight;

                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsFixedTableWidth1FixedCell.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * (RowCount + 1), tblBlock.Height); // 2 25's and 1 fifty

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(TableWidth, rowBlock.Width);
                if (r == 1)
                    Assert.AreEqual(CellHeight * 2.0, rowBlock.Height);
                else
                    Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    Assert.AreEqual(1, column.Contents.Count);
                    var cellBlock = column.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cellBlock);

                    if (r == 1)
                        Assert.AreEqual(CellHeight * 2.0, cellBlock.Height);
                    else
                        Assert.AreEqual(CellHeight, cellBlock.Height);

                    if (c == 1)
                        Assert.AreEqual((double)TableWidth / 2.0, cellBlock.Width);
                    else
                        Assert.AreEqual((double)TableWidth / 4.0, cellBlock.Width); //full width with half taken up by column 1 so 0 and 2 are a quarter

                    Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                    var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.AreEqual(3, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);

                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SimpleTable3Row3CellsFullTableWidth1FixedCell()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            //const int CellWidth = 50;
            const int CellHeight = 25;
            const int CellCount = 3;
            const int RowCount = 3;
            //const int TableWidth = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.FullWidth = true;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));

                    //middle cell is half the width of the table
                    if (c == 1 && r == 1)
                    {
                        cell.Width = PageWidth / 2.0;
                        cell.Height = CellHeight * 2.0;
                    }
                    else
                        cell.Height = CellHeight;

                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsFullTableWidth1FixedCell.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(PageWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * (RowCount + 1), tblBlock.Height); // 2 25's and 1 fifty

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(PageWidth, rowBlock.Width);
                if (r == 1)
                    Assert.AreEqual(CellHeight * 2.0, rowBlock.Height);
                else
                    Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    Assert.AreEqual(1, column.Contents.Count);
                    var cellBlock = column.Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(cellBlock);

                    if (r == 1)
                        Assert.AreEqual(CellHeight * 2.0, cellBlock.Height);
                    else
                        Assert.AreEqual(CellHeight, cellBlock.Height);

                    if (c == 1)
                        Assert.AreEqual((double)PageWidth / 2.0, cellBlock.Width);
                    else
                        Assert.AreEqual((double)PageWidth / 4.0, cellBlock.Width); //full width with half taken up by column 1 so 0 and 2 are a quarter

                    Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                    var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                    Assert.AreEqual(3, line.Runs.Count);
                    Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                    Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);

                }
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row4CellsWithColumnSpan()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellHeight = 25;
            const int CellCount = 4;
            const int RowCount = 3;
            const int TableWidth = 350;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    if (r == 1 && c == 2)
                        continue; //skip the third column middle row

                    var cell = new TableCell();

                    if (r == 1 && c == 1)
                    {
                        cell.CellColumnSpan = 2;
                        cell.Height = CellHeight * 2;
                    }
                    else
                        cell.Height = CellHeight;

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));

                    
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row4CellsWithSpan.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * (RowCount + 1), tblBlock.Height); //double height middle row

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(TableWidth, rowBlock.Width);

                if (r == 1)
                    Assert.AreEqual(CellHeight * 2, rowBlock.Height);
                else
                    Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    

                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    if (r == 1 && c == 2)
                    {
                        Assert.AreEqual(0, column.Contents.Count);
                    }
                    else
                    {
                        Assert.AreEqual(1, column.Contents.Count);
                        var cellBlock = column.Contents[0] as PDFLayoutBlock;
                        Assert.IsNotNull(cellBlock);

                        if(r == 1)
                            Assert.AreEqual(CellHeight * 2, cellBlock.Height); // all cells in the row should be double height
                        else
                            Assert.AreEqual(CellHeight, cellBlock.Height);

                        if (r == 1 && c == 1)
                        {
                            // double size for the column
                            Assert.AreEqual(((double)TableWidth / (double)CellCount) * 2, cellBlock.Width);
                        }
                        else
                        {
                            Assert.AreEqual((double)TableWidth / (double)CellCount, cellBlock.Width);
                        }
                        Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                        var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                        Assert.AreEqual(3, line.Runs.Count);
                        Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                        Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);
                    }

                }
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row4CellsWithFullColumnSpan()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellHeight = 25;
            const int CellCount = 4;
            const int RowCount = 3;
            const int TableWidth = 350;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    if (r == 1 && c > 0)
                        continue; //skip the middle row after the first cell

                    var cell = new TableCell();

                    if (r == 1 && c == 0)
                    {
                        //make it the width of the table
                        cell.CellColumnSpan = CellCount;
                        cell.Height = CellHeight * 3;
                    }
                    else
                        cell.Height = CellHeight;

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row4CellsWithFullSpan.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * (RowCount + 2), tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tblBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.IsNotNull(rowBlock);
                Assert.AreEqual(TableWidth, rowBlock.Width);

                if (r == 1)
                    Assert.AreEqual(CellHeight * 3, rowBlock.Height);
                else
                    Assert.AreEqual(CellHeight, rowBlock.Height);

                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var column = rowBlock.Columns[c];
                    Assert.IsNotNull(column);

                    if (r == 1 && c > 0)
                    {
                        Assert.AreEqual(0, column.Contents.Count);
                    }
                    else
                    {
                        Assert.AreEqual(1, column.Contents.Count);
                        var cellBlock = column.Contents[0] as PDFLayoutBlock;
                        Assert.IsNotNull(cellBlock);


                        if (r == 1 && c == 0)
                        {
                            // double size for the column
                            Assert.AreEqual((double)TableWidth, cellBlock.Width);
                            Assert.AreEqual(CellHeight * 3, cellBlock.Height);
                        }
                        else
                        {
                            Assert.AreEqual((double)TableWidth / (double)CellCount, cellBlock.Width);
                            Assert.AreEqual(CellHeight, cellBlock.Height);
                        }

                        Assert.AreEqual(1, cellBlock.Columns[0].Contents.Count);

                        var line = cellBlock.Columns[0].Contents[0] as PDFLayoutLine;

                        Assert.AreEqual(3, line.Runs.Count);
                        Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                        Assert.AreEqual("Cell " + r + "." + c, (line.Runs[1] as PDFTextRunCharacter).Characters);
                    }

                }
            }

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row3CellsNoWidthComplexContent()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 3;
            //const int TableWidth = 350;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            //tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();
                    

                    if ((r == 0 && c == 0) || (r == 2 && c == 2))
                    {
                        cell.TextLeading = 15;
                        Span span = new Span();
                        span.Contents.Add("Top Content");
                        Div div = new Div();
                        
                        div.Width = 150;
                        div.Height = 200;
                        div.BackgroundColor = Drawing.StandardColors.Aqua;
                        div.VerticalAlignment = VerticalAlignment.Middle;
                        div.HorizontalAlignment = HorizontalAlignment.Center;
                        div.Contents.Add("Block Content");
                        Span span2 = new Span();
                        span2.Contents.Add("Bottom Content");
                        cell.Contents.AddRange(span, div, span2);
                        cell.Padding = Drawing.Thickness.Empty();
                    }
                    else
                    {
                        cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                        if (r == 1)
                            cell.Height = 25;
                    }
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsNoWidthComplexContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            //Assert.AreEqual(TableWidth, tblBlock.Width);
            //Assert.AreEqual(CellHeight * RowCount, tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            var firstRow = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(firstRow.Owner, typeof(TableRow));

            //Assert.AreEqual(350, firstRow.Width);
            Assert.AreEqual(200 + (2 * 15), firstRow.Height);

            var firstCell = firstRow.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(firstCell.Owner, typeof(TableCell));

            Assert.AreEqual(150, firstCell.Width);
            Assert.AreEqual(200 + (2 * 15), firstCell.Height);

            
            var lastRow = tblBlock.Columns[0].Contents[RowCount - 1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(lastRow.Owner, typeof(TableRow));

            //Assert.AreEqual(350, firstRow.Width);
            Assert.AreEqual(200 + (2 * 15), lastRow.Height);


            var lastCell = (tblBlock.Columns[0].Contents[0] as PDFLayoutBlock).Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(lastCell.Owner, typeof(TableCell));

            Assert.AreEqual(150, lastCell.Width);
            Assert.AreEqual(200 + (2 * 15), lastCell.Height);

            //Total table height should be all 3 rows
            Assert.AreEqual(lastCell.Height + firstCell.Height + 25, tblBlock.Height);

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row3CellsExplicitTableWidthComplexContent()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 3;
            const int TableWidth = 350;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();


                    if ((r == 0 && c == 0) || (r == 2 && c == 2))
                    {
                        cell.TextLeading = 15;
                        Span span = new Span();
                        span.Contents.Add("Top Content");
                        Div div = new Div();

                        div.Width = 150;
                        div.Height = 200;
                        div.BackgroundColor = Drawing.StandardColors.Aqua;
                        div.VerticalAlignment = VerticalAlignment.Middle;
                        div.HorizontalAlignment = HorizontalAlignment.Center;
                        div.Contents.Add("Block Content");
                        Span span2 = new Span();
                        span2.Contents.Add("Bottom Content");
                        cell.Contents.AddRange(span, div, span2);
                        cell.Padding = Drawing.Thickness.Empty();
                    }
                    else
                    {
                        cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                        if (r == 1)
                            cell.Height = 25;
                    }
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsExplicitTableWidthComplexContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("The columns for the tables are being calculated equally, rather than based on the content that they contain");

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            //Assert.AreEqual(TableWidth, tblBlock.Width);
            //Assert.AreEqual(CellHeight * RowCount, tblBlock.Height);

            Assert.AreEqual(RowCount, tblBlock.Columns[0].Contents.Count);

            var firstRow = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(firstRow.Owner, typeof(TableRow));

            //Assert.AreEqual(350, firstRow.Width);
            Assert.AreEqual(200 + (2 * 15), firstRow.Height);

            var firstCell = firstRow.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(firstCell.Owner, typeof(TableCell));

            Assert.AreEqual(150, firstCell.Width);
            Assert.AreEqual(200 + (2 * 15), firstCell.Height);


            var lastRow = tblBlock.Columns[0].Contents[RowCount - 1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(lastRow.Owner, typeof(TableRow));

            //Assert.AreEqual(350, firstRow.Width);
            Assert.AreEqual(200 + (2 * 15), lastRow.Height);


            var lastCell = (tblBlock.Columns[0].Contents[0] as PDFLayoutBlock).Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(lastCell.Owner, typeof(TableCell));

            Assert.AreEqual(150, lastCell.Width);
            Assert.AreEqual(200 + (2 * 15), lastCell.Height);

            //Total table height should be all 3 rows
            Assert.AreEqual(lastCell.Height + firstCell.Height + 25, tblBlock.Height);

        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row3CellsWithHeader()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 3;
            const int TableWidth = 180;
            const int CellHeight = 25;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            var header = new TableHeaderRow();
            tbl.Rows.Add(header);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableHeaderCell();

                cell.Contents.Add(new TextLiteral("Header " + c));
                cell.Height = CellHeight * 2.0;

                header.Cells.Add(cell);

            }

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Height = CellHeight;
                    
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsWithHeader.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * RowCount + (CellHeight * 2), tblBlock.Height);

            Assert.AreEqual(RowCount + 1, tblBlock.Columns[0].Contents.Count);

            var headRow = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(headRow.Owner, typeof(TableHeaderRow));

            Assert.AreEqual(TableWidth, headRow.Width);
            Assert.AreEqual((2 * CellHeight), headRow.Height);

            for (var c = 0; c < CellCount; c++)
            {
                var headCell = headRow.Columns[c].Contents[0] as PDFLayoutBlock;
                Assert.IsInstanceOfType(headCell.Owner, typeof(TableHeaderCell));

                Assert.AreEqual(TableWidth / 3.0, headCell.Width);
                Assert.AreEqual((2 * CellHeight), headCell.Height);
            }


        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Table3Row3CellsWithHeaderAndFooter()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 3;
            const int TableWidth = 180;
            const int CellHeight = 25;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            var header = new TableHeaderRow();
            tbl.Rows.Add(header);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableHeaderCell();

                cell.Contents.Add(new TextLiteral("Header " + c));
                cell.Height = CellHeight * 2.0;

                header.Cells.Add(cell);

            }

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Height = CellHeight;

                    row.Cells.Add(cell);

                }

            }

            var footer = new TableFooterRow();
            tbl.Rows.Add(footer);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableFooterCell();

                cell.Contents.Add(new TextLiteral("Footer " + c));
                cell.Height = CellHeight * 2.0;

                footer.Cells.Add(cell);

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsWithHeaderAndFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock);
            Assert.AreEqual(TableWidth, tblBlock.Width);
            Assert.AreEqual(CellHeight * RowCount + (CellHeight * 4), tblBlock.Height);

            Assert.AreEqual(RowCount + 2, tblBlock.Columns[0].Contents.Count);

            var headRow = tblBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(headRow.Owner, typeof(TableHeaderRow));

            Assert.AreEqual(TableWidth, headRow.Width);
            Assert.AreEqual((2 * CellHeight), headRow.Height);

            for (var c = 0; c < CellCount; c++)
            {
                var headCell = headRow.Columns[c].Contents[0] as PDFLayoutBlock;
                Assert.IsInstanceOfType(headCell.Owner, typeof(TableHeaderCell));

                Assert.AreEqual(TableWidth / 3.0, headCell.Width);
                Assert.AreEqual((2 * CellHeight), headCell.Height);
            }


            var footRow = tblBlock.Columns[0].Contents[RowCount + 1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(footRow.Owner, typeof(TableFooterRow));

            Assert.AreEqual(TableWidth, footRow.Width);
            Assert.AreEqual((2 * CellHeight), footRow.Height);

            for (var c = 0; c < CellCount; c++)
            {
                var footCell = footRow.Columns[c].Contents[0] as PDFLayoutBlock;
                Assert.IsInstanceOfType(footCell.Owner, typeof(TableFooterCell));

                Assert.AreEqual(TableWidth / 3.0, footCell.Width);
                Assert.AreEqual((2 * CellHeight), footCell.Height);
            }
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableMultiRow3CellsWithRepeatingHeaderAndFooter()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 25; //A lot of rows to force the new column
            const int TableWidth = 180;
            const int CellHeight = 25;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 40;

            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.Width = TableWidth;

            var header = new TableHeaderRow();
            tbl.Rows.Add(header);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableHeaderCell();

                cell.Contents.Add(new TextLiteral("Header " + c));
                cell.Height = CellHeight * 2.0;

                header.Cells.Add(cell);

            }

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Height = CellHeight;

                    row.Cells.Add(cell);

                }

            }

            var footer = new TableFooterRow();
            tbl.Rows.Add(footer);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableFooterCell();

                cell.Contents.Add(new TextLiteral("Footer " + c));
                cell.Height = CellHeight * 2.0;

                footer.Cells.Add(cell);

            }

            using (var ms = DocStreams.GetOutputStream("Table_MultiRow3CellsWithRepeatingHeaderAndFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);


            //first column
            var tblBlock1 = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock1);
            Assert.AreEqual(TableWidth, tblBlock1.Width);

            double colRowCount1 = Math.Floor((double)(PageHeight - (2 * CellHeight)) / CellHeight);
            Assert.AreEqual(colRowCount1 + 1, tblBlock1.Columns[0].Contents.Count);
            Assert.AreEqual((colRowCount1 * CellHeight) + (2 * CellHeight), tblBlock1.Height);

            var headRow = tblBlock1.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(headRow.Owner, typeof(TableHeaderRow));

            Assert.AreEqual(TableWidth, headRow.Width);
            Assert.AreEqual((2 * CellHeight), headRow.Height);


            //second column
            var tblBlock2 = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock2);
            Assert.AreEqual(TableWidth, tblBlock2.Width);

            double colRowCount2 = RowCount - colRowCount1;

            //With a repeating header, it is 

            Assert.AreEqual(colRowCount2 + 2, tblBlock2.Columns[0].Contents.Count); //remainder + a header and footer
            Assert.AreEqual((colRowCount2 * CellHeight) + (4 * CellHeight), tblBlock2.Height);//height is also remainder + header and footer at twice height


            var  footRow = tblBlock2.Columns[0].Contents[(int)colRowCount2 + 1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(footRow.Owner, typeof(TableFooterRow));

            Assert.AreEqual(TableWidth, footRow.Width);
            Assert.AreEqual((2 * CellHeight), footRow.Height);

            
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableMultiPage3CellsWithRepeatingHeaderAndFooter()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            const int CellCount = 3;
            const int RowCount = 25; //A lot of rows to force the new column
            //const int TableWidth = 180;
            const int CellHeight = 25;
            const int Padding = 10;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Padding = new Drawing.Thickness(Padding);
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            //section.ColumnCount = 2;
            //section.AlleyWidth = 40;

            doc.Pages.Add(section);


            var tbl = new TableGrid();

            section.Contents.Add(tbl);
            tbl.FullWidth = true;

            var header = new TableHeaderRow();
            tbl.Rows.Add(header);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableHeaderCell();

                cell.Contents.Add(new TextLiteral("Header " + c));
                cell.Height = CellHeight * 2.0;

                header.Cells.Add(cell);

            }

            for (var r = 0; r < RowCount; r++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {
                    var cell = new TableCell();

                    cell.Contents.Add(new TextLiteral("Cell " + r + "." + c));
                    cell.Height = CellHeight;

                    row.Cells.Add(cell);

                }

            }

            var footer = new TableFooterRow();
            tbl.Rows.Add(footer);

            for (var c = 0; c < CellCount; c++)
            {
                var cell = new TableFooterCell();

                cell.Contents.Add(new TextLiteral("Footer " + c));
                cell.Height = CellHeight * 2.0;

                footer.Cells.Add(cell);

            }

            using (var ms = DocStreams.GetOutputStream("Table_MultiPage3CellsWithRepeatingHeaderAndFooter.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);


            //first page
            var tblBlock1 = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock1);
            Assert.AreEqual(PageWidth - (Padding * 2), tblBlock1.Width);

            double colRowCount1 = Math.Floor((double)(PageHeight - (2 * CellHeight + 2 * Padding)) / CellHeight);
            Assert.AreEqual(colRowCount1 + 1, tblBlock1.Columns[0].Contents.Count);
            Assert.AreEqual((colRowCount1 * CellHeight) + (2 * CellHeight), tblBlock1.Height);

            var headRow = tblBlock1.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsInstanceOfType(headRow.Owner, typeof(TableHeaderRow));

            Assert.AreEqual((PageWidth - (Padding * 2)), headRow.Width);
            Assert.AreEqual((2 * CellHeight), headRow.Height);


            //second page
            var pg2 = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var tblBlock2 = pg2.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(tblBlock2);
            Assert.AreEqual(PageWidth - (2 * Padding), tblBlock2.Width);

            double colRowCount2 = RowCount - colRowCount1;

            //With a repeating header, it is 

            Assert.AreEqual(colRowCount2 + 2, tblBlock2.Columns[0].Contents.Count); //remainder + a header and footer
            Assert.AreEqual((colRowCount2 * CellHeight) + (4 * CellHeight), tblBlock2.Height);//height is also remainder + header and footer at twice height


            var footRow = tblBlock2.Columns[0].Contents[(int)colRowCount2 + 1] as PDFLayoutBlock;
            Assert.IsInstanceOfType(footRow.Owner, typeof(TableFooterRow));

            Assert.AreEqual((PageWidth - (Padding * 2)), footRow.Width);
            Assert.AreEqual((2 * CellHeight), footRow.Height);


        }

    }
}
