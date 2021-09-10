using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class TablesTest : TestBase
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

        [TestMethod]
        public void SimpleTable()
        {
            var path = GetTemplatePath("Tables", "TableSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using(var stream = GetOutputStream("Tables", "TableSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void SpannedTable()
        {
            var path = GetTemplatePath("Tables", "TableSpanned.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Tables", "TableSpanned.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void CodedTable()
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
                        //We make the previous cell 2 columns wide rather than add a new one.
                        row.Cells[1].CellColumnSpan = 2;
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

            
            using (var stream = GetOutputStream("Tables","CodedTable.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }


        [TestMethod]
        public void ModifyTable()
        {
            //Use the simple table sample
            var path = GetTemplatePath("Tables", "TableSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                //Make full width and add a footer to the table
                if(doc.TryFindAComponentByID("FirstTable", out TableGrid tbl))
                {
                    tbl.FullWidth = true;

                    var row = new TableRow();
                    tbl.Rows.Add(row);

                    var span = tbl.Rows[0].Cells.Count;

                    var cell = new TableCell();
                    cell.Contents.Add(new TextLiteral("Adding a bottom row to the table with a span of " + span));
                    cell.CellColumnSpan = span;
                    row.Cells.Add(cell);
                }

                using (var stream = GetOutputStream("Tables", "TableWithNewRow.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod]
        public void TableHeaderAndFooter()
        {
            var path = GetTemplatePath("Tables", "TableHeaders.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Tables", "TableHeaders.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void TableMixedNestedContent()
        {
            var path = GetTemplatePath("Tables", "TableNested.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Tables", "TableNested.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void TableBoundContent()
        {
            var path = GetTemplatePath("Tables", "TableDatabound.html");

            using (var doc = Document.ParseDocument(path))
            {
                List<dynamic> all = new List<dynamic>();
                for(int i = 0; i < 1000; i++)
                {
                    all.Add(new { Key = "Item " + (i + 1).ToString(), Value = i * 50.0 });
                }

                doc.Params["model"] = all;
                using (var stream = GetOutputStream("Tables", "TableDatabound.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }
    }
}
