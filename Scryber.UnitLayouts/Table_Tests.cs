using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber;
using Scryber.Drawing;
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
                cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
        public void SimpleTable3Row3CellsFullWidthTextRightBottom()
        {
            const int PageWidth = 400;
            const int PageHeight = 500;
            //const int CellWidth = 50;
            const int CellHeight = 50;
            const int CellCount = 3;
            const int RowCount = 3;
            const int CellPadding = 4;

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
                    cell.Padding = CellPadding;
                    cell.Margins = (Thickness)0;
                    cell.HorizontalAlignment = HorizontalAlignment.Right;
                    cell.VerticalAlignment = VerticalAlignment.Bottom;
                    row.Cells.Add(cell);

                }

            }

            using (var ms = DocStreams.GetOutputStream("Table_3Row3CellsFullWidthTextRightBottom.pdf"))
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

                var rowXOffset = Unit.Zero;
                
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
                    var chars = (line.Runs[1] as PDFTextRunCharacter);
                    Assert.IsNotNull(chars);
                    Assert.AreEqual("Cell " + r + "." + c, chars.Characters);
                    
                    
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                    var begin = line.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(begin);
                    
                    //check bottom alignment
                    Unit yOffset = CellHeight - CellPadding - line.Height;
                    yOffset += (CellHeight * r); //add preceeding rows
                    yOffset += line.BaseLineOffset; //move down to the baseline for the actual offset
                    
                    Assert.AreEqual(yOffset, begin.StartTextCursor.Height);
                    
                    
                    //check right alignment
                    Unit xOffset = cellBlock.Width - CellPadding - chars.Width;
                    xOffset += rowXOffset;
                    Assert.AreEqual(xOffset, begin.StartTextCursor.Width);

                    //update the running total for the next check on X
                    rowXOffset += cellBlock.Width;
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
                    cell.Margins = (Thickness)0;
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

                    cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;

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
                cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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
                    cell.Margins = (Thickness)0;
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
                cell.Margins = (Thickness)0;
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

        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentPercentToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;
            Unit[] CellWidths = new Unit[] {
                new Unit(20, PageUnits.Percent),
                new Unit(30, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            section.Contents.Add(grid);

            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];
                    cell.Margins = (Thickness)0;
                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }
            

            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_TableToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count);

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(580, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(580, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = 580 * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);


                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);
                }

            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentFixedWidthToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;

            Unit tableWidth = 500;

            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = tableWidth;
            grid.FontSize = new Unit(70, PageUnits.Percent);
            section.Contents.Add(grid);

            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];
                    cell.Margins = (Thickness)0;
                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FixedWidthTable.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count);

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);
                }

            }
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentCellRelativeWidthToPageCellHeight()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;

            

            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = new Unit(60, PageUnits.Percent);
            grid.FontSize = new Unit(80, PageUnits.Percent);
            section.Contents.Add(grid);

            Unit tableWidth = (600 - 20) * 0.6;


            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                

                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];
                    cell.Margins = (Thickness)0;
                    //all cell heights should add up to the page content block height
                    cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_RelativeWidthTableCellPercentHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //After the table should be on a new page
            Assert.AreEqual(2, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //last line is zero height with the overflow

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            //Check the 80% font size on the full style
            Assert.AreEqual(20 * 0.8, tableBlock.FullStyle.Font.FontSize);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }

            var emptyBlock = pg.ContentBlock.Columns[0].Contents[1];
            Assert.AreEqual(0, emptyBlock.Height);

            pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);

            //Quick check we have the literal on the new page.
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var line = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentCellRelativeWidthToColumnCellHeight()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 10;
            section.BackgroundColor = StandardColors.Silver;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;



            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = new Unit(40, PageUnits.ViewPortMin); //40% of page width without margins
            grid.FontSize = new Unit(80, PageUnits.Percent);
            section.Contents.Add(grid);

            Unit tableWidth = 600 * 0.4;

            TableRow row  = new TableHeaderRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);
                cell.Margins = (Thickness)0;
                
                var content = "Header " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }

            for (var r = 0; r < RowCount; r++)
            {
                row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    //cell.Width = CellWidths[c]; - implied width from the header

                    //all cell heights should add up to the page content block height
                    cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();
                    cell.Margins = (Thickness)0;
                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }

            //Add a footer row to the table
            row = new TableFooterRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);
                cell.Margins = (Thickness)0;
                var content = "Footer " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_RelativeWidthTableToColumnCellPercentHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //The table should force a new column
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //just the table

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);
            Assert.AreEqual(800 - 20, tableBlock.Height);

            //Check the 80% font size on the full style
            Assert.AreEqual(20 * 0.8, tableBlock.FullStyle.Font.FontSize);


            //Header Row
            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, rowBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            //Header cells

            for (var c = 0; c < CellCount; c++)
            {
                var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                Assert.IsNotNull(rowColumn);
                Assert.AreEqual(1, rowColumn.Contents.Count);

                var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                Assert.AreEqual(cellW, rowColumn.Width);

                var cellBlock = rowColumn.Contents[0];
                Assert.AreEqual(cellW, cellBlock.Width);

                Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
            }

            for (var r = 1; r < tableBlock.Columns[0].Contents.Count; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }

            var col1Total = tableBlock.Columns[0].Contents.Count;
            var col2Total = RowCount + 2 + 1 - col1Total; //total rows + the 2 headers and a footer - the count on column1

            //Quick check we have the rest of the table and literal on the new page.
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);

            tableBlock = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tableBlock);

            Assert.AreEqual(col2Total, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < col2Total; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }


            var line = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentCellRelativeWidthToColumnCellHeightRightAlign()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 10;
            section.BackgroundColor = StandardColors.Silver;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;



            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = new Unit(40, PageUnits.ViewPortMin); //40% of page width without margins
            grid.FontSize = new Unit(80, PageUnits.Percent);
            section.Contents.Add(grid);
            grid.Style.Position.HAlign = HorizontalAlignment.Right;

            Unit tableWidth = 600 * 0.4;

            TableRow row  = new TableHeaderRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);
                cell.Margins = (Thickness)0;
                var content = "Header " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }

            for (var r = 0; r < RowCount; r++)
            {
                row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    //cell.Width = CellWidths[c]; - implied width from the header

                    //all cell heights should add up to the page content block height
                    cell.Height = new Unit(100 / RowCount, PageUnits.Percent);
                    cell.Margins = (Thickness)0;
                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }

            //Add a footer row to the table
            row = new TableFooterRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);
                cell.Margins = (Thickness)0;
                var content = "Footer " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_RelativeWidthTableToColumnCellPercentHeightRightAlign.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //The table should force a new column
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //just the table

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);
            Assert.AreEqual(800 - 20, tableBlock.Height);

            //Check the 80% font size on the full style
            Assert.AreEqual(20 * 0.8, tableBlock.FullStyle.Font.FontSize);


            //Header Row
            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, rowBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            //Header cells

            for (var c = 0; c < CellCount; c++)
            {
                var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                Assert.IsNotNull(rowColumn);
                Assert.AreEqual(1, rowColumn.Contents.Count);

                var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                Assert.AreEqual(cellW, rowColumn.Width);

                var cellBlock = rowColumn.Contents[0];
                Assert.AreEqual(cellW, cellBlock.Width);

                Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
            }

            for (var r = 1; r < tableBlock.Columns[0].Contents.Count; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }

            var col1Total = tableBlock.Columns[0].Contents.Count;
            var col2Total = RowCount + 2 + 1 - col1Total; //total rows + the 2 headers and a footer - the count on column1

            //Quick check we have the rest of the table and literal on the new page.
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);

            tableBlock = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tableBlock);

            Assert.AreEqual(col2Total, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < col2Total; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }


            var line = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
            
            Assert.Inconclusive("Need to test for the right alignment of text");
        }
        
         [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentFixedWidthCenteredToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;

            Unit tableWidth = 300;
            

            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Margins = new Thickness(Unit.Auto);
            grid.Width = tableWidth;
            grid.FontSize = new Unit(70, PageUnits.Percent);
            section.Contents.Add(grid);

            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];
                    cell.Margins = (Thickness)0;
                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FixedWidthTableCentred.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count);

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width);
            
            //The margins of auto should be 150 left and right
            Assert.IsTrue(tableBlock.Position.AutoMarginLeft);
            Assert.IsTrue(tableBlock.Position.AutoMarginRight);
            Assert.AreEqual(150, tableBlock.PagePosition.X);
            Assert.AreEqual(300, tableBlock.Width);
            
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);
                }

            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowCell()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowRow()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowAll()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowHead()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowFoot()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowWrapped()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowWrappedCell()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowWrappedRow()
        {
            Assert.Inconclusive("Need to add this");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowWrappedConainer()
        {
            Assert.Inconclusive("Need to add this - container is keep together and should simply move to the next page");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowCellToBigText()
        {
            Assert.Inconclusive("Need to add this - cell is too large for the page in the remainder and should simply move to the next page");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowCellToBigTextWhole()
        {
            Assert.Inconclusive("Need to add this - cell is too large for the entire page and should simply move to the next page");
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TableOverflowCellToBigTextWholeWrapped()
        {
            Assert.Inconclusive("Need to add this - cell is too large for the entire page in a wrapped div and should simply move to the next page");
        }
        
        
        public void TableFromCode()
        {

            var doc = new Document();
            var pg = new Page();

            var data = GetData();

            var tbl = GenerateTableGridWithAddRange(data);
        }

        private TableGrid GenerateTableGridWithAddRange(string[][] data)
        {

            var grid = new TableGrid() { FullWidth = true };
            grid.Rows.AddRange(data.Select(rowData => {

                    var row = new TableRow();

                    row.Cells.AddRange(rowData.Select(cellData => {

                        var cell = new TableCell();
                        cell.Contents.Add(cellData);

                        return cell;
                    }));

                    return row;
                }));
            return grid;
        }


        private string[][] GetData()
        {
            var list = new List<string[]>();

            for(var r = 0; r < 5; r++)
            {
                string[] inner = new string[5];
                for(var c = 0; c < 5; c++)
                {
                    inner[c] = "Cell " + r + "." + c;
                }

                list.Add(inner);
            }

            return list.ToArray();
        }

    }
}
