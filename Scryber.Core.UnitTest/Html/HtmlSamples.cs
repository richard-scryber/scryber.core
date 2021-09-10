using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlSamples
    {

        #region public TestContext TestContext

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod()]
        public void Table1_SimpleTable()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Samples/TableSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("Samples_TableSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void Table2_SimpleTableSpanned()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Samples/TableSpanned.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("Samples_TableSpanned.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void Table3_SimpleTableInCode()
        {
            var doc = new Document();

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Padding = new PDFThickness(20);

            var tbl = new TableGrid();
            pg.Contents.Add(tbl);
            tbl.FullWidth = true;

            for (int i = 0; i < 3; i++)
            {
                var row = new TableRow();
                tbl.Rows.Add(row);

                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 2)
                    {
                        //We make the previous cell 2 clolumns wide rather than add a new one.
                        row.Cells[1].ColumnCount = 2;
                        continue;
                    }
                    else
                    {
                        var cell = new TableCell() { BorderColor = PDFColors.Aqua, FontItalic = true };
                        row.Cells.Add(cell);

                        var txt = new TextLiteral("Cell " + (i + 1) + "." + (j + 1));
                        cell.Contents.Add(txt);
                    }
                }
            }

            using (var stream = DocStreams.GetOutputStream("Samples_TableInCode.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

        }
    }
}
