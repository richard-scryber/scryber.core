using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class PageNumberSamples : SampleBase
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
        public void CurrentPageNumber()
        {
            var path = GetTemplatePath("PageNumbers", "PageNumbersCurrent.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("PageNumbers", "PageNumbersCurrent.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


    }
}
