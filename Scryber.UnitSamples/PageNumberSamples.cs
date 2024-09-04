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

        [TestMethod()]
        public void TotalPageNumbers()
        {
            var path = GetTemplatePath("PageNumbers", "PageNumberTotal.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("PageNumbers", "PageNumberTotal.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void ForComponentPageNumbers()
        {
            var path = GetTemplatePath("PageNumbers", "PageNumbersFor.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = GetOutputStream("PageNumbers", "PageNumbersFor.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


        [TestMethod()]
        public void CodedPageNumbers()
        {
            using (var doc = new Document())
            {
                for(var i = 0; i < 5; i++)
                {
                    var pg = new Page();
                    var head = new Head1() { ID = "Item" + i };
                    var lit = new TextLiteral() { Text = "This is the heading index " + i + " on page " };
                    var num = new PageNumberLabel() { DisplayFormat = "{0} of {1}" };
                    pg.Style.Margins.All = 20;

                    doc.Pages.Add(pg);
                    pg.Contents.Add(head);
                    head.Contents.Add(lit);
                    head.Contents.Add(num);

                    if(i == 0) //First page add links to components on the nex
                    {
                        var div = new Div();
                        
                        div.Style.Margins.All = 20;
                        div.Style.Border.Color = StandardColors.Black;
                        pg.Contents.Add(div);

                        for (int j = 0; j < 5; j++)
                        {
                            var span = new Span() { DisplayMode = DisplayMode.Block, Padding = new Thickness(4) };
                            span.Contents.Add(new TextLiteral("The page number of index " + j + " is "));
                            span.Contents.Add(new PageOfLabel() { ComponentName = "#Item" + j });
                            div.Contents.Add(span);
                        }
                    }

                }

                
                using (var stream = GetOutputStream("PageNumbers", "PageNumbersCoded.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }


    }
}
