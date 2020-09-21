using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Styles.Parsing;

using Scryber.Layout;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class HtmlParsing_Test
    {


        private PDFLayoutContext _layoutcontext;
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

        private void SimpleDocumentParsing_Layout(object sender, PDFLayoutEventArgs args)
        {
            _layoutcontext = args.Context;
        }

        [TestMethod()]
        public void SimpleDocumentParsing()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <style>
                                    .strong { font-weight: bold; color: #880088; }
                                    body.strong p { background-color: #F8F; }
                                </style>
                            </head>

                            <body class='strong' style='margin:20px;' >
                                <p id='myPara' style='border: solid 1px blue; padding: 5px;' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var ms = new System.IO.MemoryStream())
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF("C:\\Temp\\Html.pdf", System.IO.FileMode.Create);
                }

                
                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;
                var p = body.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("#880088"), body.FullStyle.Fill.Color, "Fill colors do not match");
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("#F8F"), p.FullStyle.Background.Color, "Background color has not been applied");
                Assert.AreEqual(Scryber.Drawing.PDFColor.Parse("blue"), p.FullStyle.Border.Color, "Inline Border Color not correct");

            }
        }

        
        [TestMethod()]
        public void LoadHtmlFromSource()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/sample.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.SaveAsPDF("C:\\Temp\\Html.pdf", System.IO.FileMode.Create);
            }

        }

        [TestMethod]
        public void CSSParserWithComments()
        {
            var css = @"
            /* This is the grey body */
            body.grey
            {
                background-color:#808080; /* body background */
                color: #222;
            }

            body.grey div /* Inner divs */{
                padding: 10px;
                /*color: #AAA;*/
                margin:15px;
            }


            body.grey div.reverse{
    
                /* Reverse the colors */
                background-color: #222;
                color:#808080;
            }";

            var cssparser = new Scryber.Styles.Parsing.CSSStyleParser(css);

            StyleCollection col = new StyleCollection();

            foreach (var style in cssparser)
            {
                col.Add(style);
            }

            Assert.AreEqual(3, col.Count);

            //First one
            var one = col[0] as StyleDefn;

            Assert.AreEqual("body.grey", one.Match.ToString());
            Assert.AreEqual(3, one.ValueCount);
            Assert.AreEqual((PDFColor)"#808080", one.GetValue(StyleKeys.BgColorKey, PDFColors.Transparent));
            Assert.AreEqual(FillType.Solid, one.GetValue(StyleKeys.BgStyleKey, FillType.None));
            Assert.AreEqual((PDFColor)"#222", one.GetValue(StyleKeys.FillColorKey, PDFColors.Transparent));

            var two = col[1] as StyleDefn;

            Assert.AreEqual("body.grey div", two.Match.ToString());
            Assert.AreEqual(2, two.ValueCount);
            // 96 pixels per inch, 72 points per inch
            Assert.AreEqual(7.5, two.GetValue(StyleKeys.PaddingAllKey, PDFUnit.Zero).PointsValue); 
            Assert.AreEqual(11.25, two.GetValue(StyleKeys.MarginsAllKey, PDFUnit.Zero).PointsValue);

            var three = col[2] as StyleDefn;

            Assert.AreEqual("body.grey div.reverse", three.Match.ToString());
            Assert.AreEqual(3, one.ValueCount);
            Assert.AreEqual((PDFColor)"#222", three.GetValue(StyleKeys.BgColorKey, PDFColors.Transparent));
            Assert.AreEqual(FillType.Solid, three.GetValue(StyleKeys.BgStyleKey, FillType.None));
            Assert.AreEqual((PDFColor)"#808080", three.GetValue(StyleKeys.FillColorKey, PDFColors.Transparent));



        }

    }
}
