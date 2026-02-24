using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Scryber.Components;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Styles
{
    /// <summary>
    /// Test class for TableCell CellRowSpan property and HTML rowspan attribute parsing
    /// </summary>
    [TestClass()]
    public class TableRowspanTest
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #region Property Tests (Category A)

        /// <summary>
        /// A test for TableCell CellRowSpan default value
        /// </summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void TableCell_RowSpan_DefaultValue()
        {
            var cell = new TableCell();
            Assert.AreEqual(1, cell.CellRowSpan, "Default rowspan should be 1");
        }

        /// <summary>
        /// A test for TableCell CellRowSpan setter and getter
        /// </summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void TableCell_RowSpan_SetAndGet()
        {
            var cell = new TableCell();
            cell.CellRowSpan = 3;
            Assert.AreEqual(3, cell.CellRowSpan, "Rowspan should be set to 3");
        }

        /// <summary>
        /// A test for TableCell CellRowSpan invalid value (zero)
        /// </summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void TableCell_RowSpan_InvalidValue_Zero()
        {
            var cell = new TableCell();
            cell.CellRowSpan = 0;
            Assert.AreEqual(1, cell.CellRowSpan, "Rowspan of 0 should revert to default 1");
        }

        /// <summary>
        /// A test for TableCell CellRowSpan invalid value (negative)
        /// </summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void TableCell_RowSpan_InvalidValue_Negative()
        {
            var cell = new TableCell();
            cell.CellRowSpan = -5;
            Assert.AreEqual(1, cell.CellRowSpan, "Negative rowspan should revert to default 1");
        }

        /// <summary>
        /// A test for TableCell CellRowSpan large value
        /// </summary>
        [TestMethod()]
        [TestCategory("Style Values")]
        public void TableCell_RowSpan_LargeValue()
        {
            var cell = new TableCell();
            cell.CellRowSpan = 100;
            Assert.AreEqual(100, cell.CellRowSpan, "Large rowspan values should be accepted");
        }

        #endregion

        #region HTML Parser Tests (Category C)

        /// <summary>
        /// A test for parsing HTML table with rowspan attribute and verifying the rowspan is set on parsed cells
        /// </summary>
        [TestMethod()]
        [TestCategory("HTML Parser")]
        public void TableRowspan_Parser_HTMLRowspanAttribute_WithVerification()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <tr>
                                <td rowspan=""3"">Cell A</td>
                                <td>Cell B</td>
                            </tr>
                            <tr>
                                <td>Cell C</td>
                            </tr>
                            <tr>
                                <td>Cell D</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            // Parse the HTML document from string using StringReader
            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                Assert.IsNotNull(doc, "Document should be parsed successfully");
                Assert.IsTrue(doc.Pages.Count > 0, "Document should have at least one page");

                // Navigate to the parsed table
                Section section = doc.Pages[0] as Section;
                Assert.IsNotNull(section, "First page should be a Section");

                // Find the table in section contents
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

                // Verify table structure
                Assert.AreEqual(3, grid.Rows.Count, "Table should have 3 rows");

                // Verify Row 0
                TableRow row0 = grid.Rows[0];
                Assert.AreEqual(2, row0.Cells.Count, "Row 0 should have 2 cells");

                // Cell A (rowspan=3)
                TableCell cellA = row0.Cells[0];
                Assert.AreEqual(3, cellA.CellRowSpan, "Cell A should have rowspan=3");

                // Cell B (rowspan=1, default)
                TableCell cellB = row0.Cells[1];
                Assert.AreEqual(1, cellB.CellRowSpan, "Cell B should have default rowspan=1");

                // Verify Row 1
                TableRow row1 = grid.Rows[1];
                Assert.AreEqual(1, row1.Cells.Count, "Row 1 should have 1 cell (Cell A spans from Row 0)");

                // Cell C (rowspan=1, default)
                TableCell cellC = row1.Cells[0];
                Assert.AreEqual(1, cellC.CellRowSpan, "Cell C should have default rowspan=1");

                // Verify Row 2
                TableRow row2 = grid.Rows[2];
                Assert.AreEqual(1, row2.Cells.Count, "Row 2 should have 1 cell (Cell A spans from Row 0)");

                // Cell D (rowspan=1, default)
                TableCell cellD = row2.Cells[0];
                Assert.AreEqual(1, cellD.CellRowSpan, "Cell D should have default rowspan=1");
            }
        }

        /// <summary>
        /// A test for parsing HTML table with both rowspan and colspan attributes
        /// </summary>
        [TestMethod()]
        [TestCategory("HTML Parser")]
        public void TableRowspan_Parser_HTMLRowspanWithColspan_WithVerification()
        {
            string html = @"
                <html>
                    <body>
                        <table>
                            <tr>
                                <td colspan=""2"" rowspan=""2"">Cell A</td>
                                <td>Cell B</td>
                            </tr>
                            <tr>
                                <td>Cell C</td>
                            </tr>
                        </table>
                    </body>
                </html>";

            // Parse the HTML document from string using StringReader
            using (var reader = new StringReader(html))
            {
                Document doc = Document.ParseHtmlDocument(reader);
                Assert.IsNotNull(doc, "Document should be parsed successfully");
                Assert.IsTrue(doc.Pages.Count > 0, "Document should have at least one page");

                // Navigate to the parsed table
                Section section = doc.Pages[0] as Section;
                Assert.IsNotNull(section, "First page should be a Section");

                // Find the table in section contents
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

                // Verify table structure
                Assert.AreEqual(2, grid.Rows.Count, "Table should have 2 rows");

                // Verify Row 0
                TableRow row0 = grid.Rows[0];
                Assert.AreEqual(2, row0.Cells.Count, "Row 0 should have 2 cells (Cell A with colspan, Cell B)");

                // Cell A with both colspan and rowspan
                TableCell cellA = row0.Cells[0];
                Assert.AreEqual(2, cellA.CellColumnSpan, "Cell A should have colspan=2");
                Assert.AreEqual(2, cellA.CellRowSpan, "Cell A should have rowspan=2");

                // Cell B with default spans
                TableCell cellB = row0.Cells[1];
                Assert.AreEqual(1, cellB.CellColumnSpan, "Cell B should have default colspan=1");
                Assert.AreEqual(1, cellB.CellRowSpan, "Cell B should have default rowspan=1");

                // Verify Row 1
                TableRow row1 = grid.Rows[1];
                Assert.AreEqual(1, row1.Cells.Count, "Row 1 should have 1 cell (Cell C only, Cell A spans columns 0-1)");

                // Cell C with default spans
                TableCell cellC = row1.Cells[0];
                Assert.AreEqual(1, cellC.CellColumnSpan, "Cell C should have default colspan=1");
                Assert.AreEqual(1, cellC.CellRowSpan, "Cell C should have default rowspan=1");
            }
        }

        #endregion
    }
}
