using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Drawing;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
                var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
                doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

                using (var stream = GetOutputStream("Pages", "PagesFlowing.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void PagesBreaks()
        {
            var path = GetTemplatePath("Pages", "PagesBreaks.html");

            using (var doc = Document.ParseDocument(path))
            {
                var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
                doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

                using (var stream = GetOutputStream("Pages", "PagesBreaks.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod]
        public void PagesSizes()
        {
            var path = GetTemplatePath("Pages", "PageSizes.html");

            using (var doc = Document.ParseDocument(path))
            {
                var txtPath = GetTemplatePath("Pages", "LongTextFile.txt");
                doc.Params["content"] = System.IO.File.ReadAllText(txtPath);

                using (var stream = GetOutputStream("Pages", "PageSizes.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod]
        public void PagesCoded()
        {
            using(var doc = new Document())
            {
                //Define the title style that matches onto the '.title' style class.
                var titleStyle = new StyleDefn(".title");
                
                titleStyle.Background.ImageSource = "../../../Images/Landscape.jpg";
                titleStyle.Background.PatternRepeat = PatternRepeat.Fill;
                titleStyle.Position.VAlign = VerticalAlignment.Middle;
                titleStyle.Position.HAlign = HorizontalAlignment.Center;
                titleStyle.Size.Height = 300;
                titleStyle.Font.FontSize = 30;
                titleStyle.Fill.Color = PDFColors.White;
                titleStyle.Font.FontFamily = new FontSelector("serif");


                //Define the body style that matches onto the '.body' style class
                var bodyStyle = new StyleDefn(".body");
                bodyStyle.Font.FontSize = 12;
                bodyStyle.Padding.All = 10;
                bodyStyle.Border.Color = (Color)"#AAA";
                bodyStyle.Columns.ColumnCount = 2;

                var textStyle = new StyleDefn(".preserve");
                textStyle.Text.PreserveWhitespace = true;

                //Add the styles to the document
                doc.Styles.Add(bodyStyle);
                doc.Styles.Add(titleStyle);
                doc.Styles.Add(textStyle);

                //Create a page with a size
                var pg = new Page()
                {
                    PaperSize = PaperSize.A4,
                    PaperOrientation = PaperOrientation.Landscape
                };

                //add it to the document Pages collection
                doc.Pages.Add(pg);

                //Create new instances of the header and footer classes that implement
                //The IPDFTemplate interface and set to the header and footer.
                pg.Header = new CodedHeader();
                pg.Footer = new CodedFooter();

                //Create the title div and add it to the first page
                var div = new Div();
                div.StyleClass = "title";
                pg.Contents.Add(div);

                //With some text in it.
                var txt = new TextLiteral("This is the title page");
                div.Contents.Add(txt);

                //Now add a section to the document
                var sect = new Section()
                {
                    PaperOrientation = PaperOrientation.Portrait
                };
                doc.Pages.Add(sect);

                //Set the header and footer (to the same as the page)
                sect.Header = pg.Header;
                sect.Footer = pg.Footer;

                //Add a header
                var contentTitle = new Head1() { Text = "This is the loaded content", Margins = new PDFThickness(20) };
                sect.Contents.Add(contentTitle);

                //And add the body content to the section.
                var body = new Div();
                //Add the body class, and preserve so extra returns are retained
                //Will still wrap text.
                body.StyleClass = "body preserve";
                sect.Contents.Add(body);

                //Read some long plain text from a file into a text literal
                var path = GetTemplatePath("Pages", "LongTextFile.txt");
                var content = new TextLiteral();
                content.Text = System.IO.File.ReadAllText(path);

                //We set the style to preserve, so that the white space in the content is retained
                content.StyleClass = "preserve";
                
                //Add it to the body.
                body.Contents.Add(content);

                //And process in the same way
                using (var stream = GetOutputStream("Pages", "PagesCoded.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }


        }

        /// <summary>
        /// IPDFTemplate for the header
        /// </summary>
        private class CodedHeader : ITemplate
        {
            public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
            {
                return new IComponent[]
                {
                    new Head4(){
                        Text = "This is the header",
                        Padding = new PDFThickness(10, 20, 10, 20),
                        Margins = PDFThickness.Empty(),
                        BackgroundColor = PDFColors.Silver,
                        HorizontalAlignment = HorizontalAlignment.Right
                    }
                };
            }
        }

        /// <summary>
        /// IPDFTemplate for the footer
        /// </summary>
        private class CodedFooter : ITemplate
        {
            public IEnumerable<IComponent> Instantiate(int index, IComponent owner)
            {
                var div = new Div() {
                    BackgroundColor = PDFColors.Silver,
                    FillColor = PDFColors.White,
                    FontSize = 12,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new PDFThickness(10)
                };
                div.Contents.Add(new PageNumberLabel() { DisplayFormat = "{0} of {1}" });

                return new IComponent[] { div };
            }
        }
    }
}
