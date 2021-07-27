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
using System.Diagnostics;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class CssParsing_Test
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

        [TestMethod]
        public void CSSStringEnumerator()
        {
            var chars = "0123456789";
            int index = 0;
            var str = new StringEnumerator(chars);

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);

            while (str.MoveNext())
            {
                Assert.IsFalse(str.EOS);
                Assert.AreEqual(index, str.Offset);
                Assert.AreEqual(chars[index], str.Current);
                index++;
            }
            Assert.AreEqual(10, index);
            Assert.AreEqual(10, str.Offset);
            Assert.AreEqual(true, str.EOS);
        }

        



        private void SimpleDocumentParsing_Layout(object sender, PDFLayoutEventArgs args)
        {
            _layoutcontext = args.Context;
        }

        string commentedCSS = @"
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

        [TestMethod]
        public void ParseCSSWithComments()
        {
            var css = commentedCSS;

            var cssparser = new Scryber.Styles.Parsing.CSSStyleParser(css, null);

            StyleCollection col = new StyleCollection();

            foreach (var style in cssparser)
            {
                col.Add(style);
            }

            Assert.AreEqual(3, col.Count);

            //First one
            var one = col[0] as StyleDefn;

            Assert.AreEqual("body.grey", one.Match.ToString());
            Assert.AreEqual(2, one.ValueCount);
            Assert.AreEqual((PDFColor)"#808080", one.GetValue(StyleKeys.BgColorKey, PDFColors.Transparent));
            Assert.AreEqual((PDFColor)"#222", one.GetValue(StyleKeys.FillColorKey, PDFColors.Transparent));

            var two = col[1] as StyleDefn;

            Assert.AreEqual("body.grey div", two.Match.ToString());
            Assert.AreEqual(10, two.ValueCount); //All, Top, Left, Bottom and Right are all set for Margins and Padding
            // 96 pixels per inch, 72 points per inch
            Assert.AreEqual(7.5, two.GetValue(StyleKeys.PaddingAllKey, PDFUnit.Zero).PointsValue); 
            Assert.AreEqual(11.25, two.GetValue(StyleKeys.MarginsAllKey, PDFUnit.Zero).PointsValue);

            var three = col[2] as StyleDefn;

            Assert.AreEqual("body.grey div.reverse", three.Match.ToString());
            Assert.AreEqual(2, one.ValueCount); 
            Assert.AreEqual((PDFColor)"#222", three.GetValue(StyleKeys.BgColorKey, PDFColors.Transparent));
            Assert.AreEqual((PDFColor)"#808080", three.GetValue(StyleKeys.FillColorKey, PDFColors.Transparent));



        }
        [TestMethod()]
        [TestCategory("Performance")]
        public void ParseCSSWithComments_Performance()
        {
            var css = commentedCSS;

            int repeatCount = 1000;

            Stopwatch counter = Stopwatch.StartNew();
            for (int i = 0; i < repeatCount; i++)
            {

                var cssparser = new Scryber.Styles.Parsing.CSSStyleParser(css, null);

                StyleCollection col = new StyleCollection();

                foreach (var style in cssparser)
                {
                    col.Add(style);
                }
            }
            counter.Stop();

            var elapsed = counter.Elapsed.TotalMilliseconds / repeatCount;
            Assert.IsTrue(elapsed < 0.15, "Took too long to parse. Expected < 0.15ms per string, Actual : " + elapsed + "ms");

        }

        [TestMethod]
        public void ParseCSSWithMedia()
        {
            var css = @"
@media only screen and (min-width : 1224px) {
    body.grey{
        font-family:'Times New Roman', Times, serif
    }
    body.grey div{
        font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif
    }
}
/* This is the grey body */
body.grey
{
    background-color:#808080; /* body background */
    color: #222;
}

@media print and (orientation: landscape)
{
    body.grey{
        font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif
    }
}
body.grey div{
    padding: 10px;
    /*color: #AAA;*/
    margin:10px;
}


body.grey div.reverse{
    
    /* Reverse the colors */
    background-color: #222;
    color:#808080;
}

@media print {
    /* Nested media selector*/
    @media (orientation: portrait) {
        body.grey {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            font-weight: bold;
        }
    }
}
";

            var cssparser = new Scryber.Styles.Parsing.CSSStyleParser(css, null);

            StyleCollection col = new StyleCollection();

            foreach (var style in cssparser)
            {
                col.Add(style);
            }

            Assert.AreEqual(6, col.Count);

            //Top one should be a media query
            Assert.IsInstanceOfType(col[0], typeof(StyleMediaGroup));
            
            var media = (StyleMediaGroup)col[0];
            Assert.AreEqual("screen", media.Media.Type);
            Assert.AreEqual(2, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());

            //Second one normal style
            Assert.IsInstanceOfType(col[1], typeof(StyleDefn));
            Assert.AreEqual(2, col[1].ValueCount);

            //Third is a media for print
            Assert.IsInstanceOfType(col[2], typeof(StyleMediaGroup));

            media = (StyleMediaGroup)col[2];
            Assert.AreEqual("print", media.Media.Type);
            Assert.AreEqual(1, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());

            //Fourth and Fifth are normal
            Assert.IsInstanceOfType(col[3], typeof(StyleDefn));
            Assert.AreEqual(10, col[3].ValueCount); //All, Top, Left, Bottom and Right are all set for Margins and Padding
            Assert.AreEqual("body.grey div", (col[3] as StyleDefn).Match.ToString());

            Assert.IsInstanceOfType(col[4], typeof(StyleDefn));
            Assert.AreEqual(2, col[4].ValueCount); //Include the background type
            Assert.AreEqual("body.grey div.reverse", (col[4] as StyleDefn).Match.ToString());

            //Sixth is nested
            Assert.IsInstanceOfType(col[5], typeof(StyleMediaGroup));

            media = (StyleMediaGroup)col[5];
            Assert.AreEqual("print", media.Media.Type);
            Assert.AreEqual(1, media.Styles.Count);
            Assert.IsInstanceOfType(media.Styles[0], typeof(StyleMediaGroup));
            //inner item
            media = media.Styles[0] as StyleMediaGroup;
            Assert.IsTrue(string.IsNullOrEmpty(media.Media.Type));
            Assert.AreEqual("(orientation: portrait)", media.Media.Features);
            //one inner style
            Assert.AreEqual(1, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());
        }

        [TestMethod]
        public void ParseMinifiedCssFile()
        {
            //This is a minimised version of the styles above
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/CSS/include.min.css");
            path = System.IO.Path.GetFullPath(path);
            var css = System.IO.File.ReadAllText(path);

            var cssparser = new Scryber.Styles.Parsing.CSSStyleParser(css, null);

            StyleCollection col = new StyleCollection();

            foreach (var style in cssparser)
            {
                col.Add(style);
            }

            //Sames tests, just with a minimised file

            Assert.AreEqual(6, col.Count);

            //Top one should be a media query
            Assert.IsInstanceOfType(col[0], typeof(StyleMediaGroup));

            var media = (StyleMediaGroup)col[0];
            Assert.AreEqual("screen", media.Media.Type);
            Assert.AreEqual(2, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());

            //Second one normal style
            Assert.IsInstanceOfType(col[1], typeof(StyleDefn));
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());
            Assert.AreEqual(2, col[1].ValueCount);

            //Third is a media for print
            Assert.IsInstanceOfType(col[2], typeof(StyleMediaGroup));

            media = (StyleMediaGroup)col[2];
            Assert.AreEqual("print", media.Media.Type);
            Assert.AreEqual(1, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());

            //Fourth and Fifth are normal
            Assert.IsInstanceOfType(col[3], typeof(StyleDefn));
            Assert.AreEqual(10, col[3].ValueCount); //All, Top, Left, Bottom and Right are all set for Margins and Padding
            Assert.AreEqual("body.grey div", (col[3] as StyleDefn).Match.ToString());

            Assert.IsInstanceOfType(col[4], typeof(StyleDefn));
            Assert.AreEqual(2, col[4].ValueCount); //Include the background type
            Assert.AreEqual("body.grey div.reverse", (col[4] as StyleDefn).Match.ToString());

            //Sixth is nested
            Assert.IsInstanceOfType(col[5], typeof(StyleMediaGroup));

            media = (StyleMediaGroup)col[5];
            Assert.AreEqual("print", media.Media.Type);
            Assert.AreEqual(1, media.Styles.Count);
            Assert.IsInstanceOfType(media.Styles[0], typeof(StyleMediaGroup));
            //inner item
            media = media.Styles[0] as StyleMediaGroup;
            Assert.IsTrue(string.IsNullOrEmpty(media.Media.Type));
            Assert.AreEqual("(orientation:portrait)", media.Media.Features);
            //one inner style
            Assert.AreEqual(1, media.Styles.Count);
            Assert.AreEqual("body.grey", (media.Styles[0] as StyleDefn).Match.ToString());
        }

        [TestMethod()]
        public void RemoteCssFileLoading()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.Core.UnitTest/Content/HTML/CSS/Include.css";
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                            </body>

                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HtmlRemoteCSS.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.SaveAsPDF(stream);
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;
                
                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((PDFColor)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                

            }
        }

        [TestMethod]
        public void ParsePDFFontSource()
        {
            string sample = "url(https://somewebsite.com/path/to/font.woff)";

            PDFFontSource parsed;
            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("https://somewebsite.com/path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(path/to/font.woff)";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(path/to/font.woff) format(\"woff\")";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.WOFF, parsed.Format);

            sample = "url('path/to/font.woff')";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(\"path/to/svgfont.svg#example\")";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);


            sample = "url(\"path/to/svgfont.svg#example\") format(\"svg\")";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.SVG, parsed.Format);

            //Some locals

            sample = "local(font)";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "local(some font)";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);


            sample = "local('some font') format(truetype)";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.TrueType, parsed.Format);


            sample = "local(\"some other font\") format(\"opentype\")";

            Assert.IsTrue(PDFFontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some other font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.OpenType, parsed.Format);

            //empty is false
            Assert.IsFalse(PDFFontSource.TryParseOneValue("", out parsed));

            //unbalanced quotes is false
            Assert.IsFalse(PDFFontSource.TryParseOneValue("local(\"some other font) format(\"opentype\")", out parsed));

            //Unknown source type is false
            Assert.IsFalse(PDFFontSource.TryParseOneValue("remote(\"path/to/svgfont.svg#example\") format(\"svg\")", out parsed));

            //Other marker e.g. other is ignored so true
            Assert.IsTrue(PDFFontSource.TryParseOneValue("url(\"path/to/svgfont.svg#example\") other(\"svg\")", out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            //Parse Multiple

            var full = @"local(font), url(path/to/font.svg) format('svg'),
                url(path/to/font.woff) format('woff'),
                url(path/to/font.ttf) format(truetype),
                url('path/to/font.otf') format(embedded-opentype)";

            Assert.IsTrue(PDFFontSource.TryParse(full, out parsed));

            Assert.AreEqual("font", parsed.Source);
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            parsed = parsed.Next;
            Assert.IsNotNull(parsed);

            Assert.AreEqual("path/to/font.svg", parsed.Source);
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual(FontSourceFormat.SVG, parsed.Format);

            parsed = parsed.Next;
            Assert.IsNotNull(parsed);

            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual(FontSourceFormat.WOFF, parsed.Format);

            parsed = parsed.Next;
            Assert.IsNotNull(parsed);

            Assert.AreEqual("path/to/font.ttf", parsed.Source);
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual(FontSourceFormat.TrueType, parsed.Format);

            parsed = parsed.Next;
            Assert.IsNotNull(parsed);

            Assert.AreEqual("path/to/font.otf", parsed.Source);
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual(FontSourceFormat.EmbeddedOpenType, parsed.Format);
        }
    }
}
