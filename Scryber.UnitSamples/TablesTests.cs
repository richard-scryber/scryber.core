using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
