using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;

namespace Scryber.UnitSamples
{
    [TestClass]
    public class NumberSamples : SampleBase
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
        public void SimpleNavigationLinks()
        {
            var path = GetTemplatePath("Links", "LinksSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["siteurl"] = "https://www.scryber.co.uk";

                using (var stream = GetOutputStream("Links", "LinksSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void NamedActionLinks()
        {
            var path = GetTemplatePath("Links", "LinksNamedActions.html");

            using (var doc = Document.ParseDocument(path))
            {
                var pages = new[] { new { Id = "first" }, new { Id = "second" }, new { Id = "third" }, new { Id = "fourth" } };
                doc.Params["pages"] = pages;

                using (var stream = GetOutputStream("Links", "LinksNamedActions.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void StyledFooterNavigationLinks()
        {
            var path = GetTemplatePath("Links", "LinksStyledFooter.html");

            using (var doc = Document.ParseDocument(path))
            {
                var pages = new[] { new { Id = "first" }, new { Id = "second" }, new { Id = "third" }, new { Id = "fourth" } };
                doc.Params["pages"] = pages;

                using (var stream = GetOutputStream("Links", "LinksStyledFooter.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void StyledFooterWithTOCLinks()
        {
            var path = GetTemplatePath("Links", "LinksStyledTOC.html");

            using (var doc = Document.ParseDocument(path))
            {
                var pages = new[] { new { Id = "first" }, new { Id = "second" }, new { Id = "third" }, new { Id = "fourth" } };
                doc.Params["pages"] = pages;

                using (var stream = GetOutputStream("Links", "LinksStyledTOC.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void SimpleLinksWithCustomAddition()
        {
            //template from our first example
            var path = GetTemplatePath("Links", "LinksSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["siteurl"] = "https://www.scryber.co.uk";
                doc.ConformanceMode = ParserConformanceMode.Strict;
                //create a new link

                var link = new Link()
                {
                    Action = LinkAction.Uri,
                    File = "https://www.nuget.org/packages/Scryber.Core/",
                    Margins = new PDFThickness(10),
                    Padding = new PDFThickness(5),
                    BackgroundColor = PDFColors.Gray,
                    PositionMode = PositionMode.Block
                };

                //add some inner content

                link.Contents.Add(new TextLiteral("Link to the scryber Nuget package"));

                //add it to the page (at the end)

                var pg = doc.Pages[0] as Page;
                pg.Contents.Add(link);

                using (var stream = GetOutputStream("Links", "LinksCustom.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void BrokenLinks()
        {
            var path = GetTemplatePath("Links", "LinksBroken.html");

            using (var doc = Document.ParseDocument(path))
            {
                var pages = new[] { new { Id = "first" }, new { Id = "second" }, new { Id = "third" }, new { Id = "fourth" } };
                doc.Params["pages"] = pages;

                using (var stream = GetOutputStream("Links", "LinksBrokenWithLog.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

    }
}
