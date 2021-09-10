using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using System.IO;

namespace Scryber.Core.UnitTests.Html.Samples
{
    [TestClass]
    public class TableSamples
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


        /// <summary>
        /// Returns a file stream in '/My Documents/Scryber Test Output' folder.
        /// If the document exists it will be overwritten
        /// </summary>
        /// <param name="docName">The name of the file with extension</param>
        /// <returns>A new file stream</returns>
        public Stream GetOutputStream(string docName)
        {
            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.Combine(path, "Scryber Test Output");

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            var output = System.IO.Path.Combine(path, docName);

            return new System.IO.FileStream(output, System.IO.FileMode.Create);
        }

        /// <summary>
        /// Returns the path to a template in the samples folder.
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public string GetSampleTemplatePath(string templateName)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Samples/", templateName);

            return path;
        }


        [TestMethod()]
        public void Table1_SimpleTable()
        {
            var path = this.GetSampleTemplatePath("TableSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Samples_TableSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod()]
        public void Table2_SimpleTableSpanned()
        {
            var path = this.GetSampleTemplatePath("TableSpanned.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Samples_TableSpanned.pdf"))
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
