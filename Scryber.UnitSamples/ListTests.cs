using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class ListTests : TestBase
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
        public void SimpleList()
        {
            var path = GetTemplatePath("Lists", "ListsSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void OverflowingList()
        {
            var path = GetTemplatePath("Lists", "ListsOverflow.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Lists", "ListsOverflow.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


    }
}
