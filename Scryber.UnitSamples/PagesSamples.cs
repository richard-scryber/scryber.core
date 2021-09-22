using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class PagesSamples : SampleBase
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
        public void PagesSimple()
        {
            var path = GetTemplatePath("Pages", "PagesSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Pages", "PagesSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void PagesFlowing()
        {
            var path = GetTemplatePath("Pages", "PagesFlowing.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("Pages", "PagesFlowing.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

    }
}
