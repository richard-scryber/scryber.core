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

using Scryber.PDF.Layout;
using Scryber.PDF;

using System.Diagnostics;
using Scryber.Text;
using Scryber.PDF.Resources;
using System.Runtime.ExceptionServices;
using System.IO;
using System.Collections;

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

        [TestMethod()]
        public void CSSErrors()
        {
            Assert.Inconclusive("Add tests for invalid css content. Make sure that the other styles are actually parsed");
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
            Assert.AreEqual(chars, str.ToString());

            Assert.IsTrue(str.MoveNext());
            Assert.IsTrue(str.Matches("0123"));
            Assert.IsFalse(str.Matches("34567"));
            str.Reset();

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);
            Assert.AreEqual(chars, str.ToString());

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

            //Move Back one

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(9, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('9', str.Current);
            Assert.AreEqual('7', str.Peek(-2));

            //Move Back another

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(8, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('8', str.Current);
            Assert.AreEqual('9', str.Peek(1));

            Assert.IsTrue(str.Matches("89"));

            //Substring
            Assert.AreEqual(chars.Substring(5), str.Substring(5));
            Assert.AreEqual(chars.Substring(5, 5), str.Substring(5, 5));
            Assert.AreEqual(chars.Substring(2, 2), str.Substring(2, 2));

        }


        [TestMethod]
        public void CSSStringEnumerator_Partial()
        {
            var chars = "___0123456789________";
            var subchars = "0123456789";
            int index = 3;
            int length = 10;
            var str = new StringEnumerator(chars, index, length);

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);
            Assert.AreEqual(subchars, str.ToString());

            Assert.IsTrue(str.MoveNext());
            Assert.IsTrue(str.Matches("0123"));
            Assert.IsFalse(str.Matches("34567"));

            str.Reset();

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);
            Assert.AreEqual(subchars, str.ToString());

            while (str.MoveNext())
            {
                Assert.IsFalse(str.EOS);
                Assert.AreEqual(index - 3, str.Offset);
                Assert.AreEqual(chars[index], str.Current);
                index++;
            }
            Assert.AreEqual(13, index);
            Assert.AreEqual(10, str.Offset);
            Assert.AreEqual(true, str.EOS);

            //Move Back one

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(9, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('9', str.Current);
            Assert.AreEqual('7', str.Peek(-2));

            //Move Back another

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(8, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('8', str.Current);
            Assert.AreEqual('9', str.Peek(1));

            Assert.IsTrue(str.Matches("89"));

            //Substring
            Assert.AreEqual(subchars.Substring(5), str.Substring(5));
            Assert.AreEqual(subchars.Substring(5, 5), str.Substring(5, 5));
            Assert.AreEqual(subchars.Substring(2, 2), str.Substring(2, 2));
        }

        [TestMethod]
        public void CSSStringEnumerator_SubEnumerator()
        {
            var chars = "___0123456789________";
            var subchars = "0123456789";
            int index = 3;
            int length = 10;

            var str_long = new StringEnumerator(chars);
            Assert.AreEqual(chars.Length, str_long.Length);
            Assert.AreEqual(-1, str_long.Offset);
            Assert.IsFalse(str_long.EOS);
            Assert.AreEqual(chars, str_long.ToString());

            //Create a new enumerator from the base, with new index and length
            var str = new StringEnumerator(str_long, index, length);

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);
            Assert.AreEqual(subchars, str.ToString());

            Assert.IsTrue(str.MoveNext());
            Assert.IsTrue(str.Matches("0123"));
            Assert.IsFalse(str.Matches("34567"));

            str.Reset();

            Assert.AreEqual(10, str.Length);
            Assert.AreEqual(-1, str.Offset);
            Assert.IsFalse(str.EOS);
            Assert.AreEqual(subchars, str.ToString());

            while (str.MoveNext())
            {
                Assert.IsFalse(str.EOS);
                Assert.AreEqual(index - 3, str.Offset);
                Assert.AreEqual(chars[index], str.Current);
                index++;
            }
            Assert.AreEqual(13, index);
            Assert.AreEqual(10, str.Offset);
            Assert.AreEqual(true, str.EOS);

            //Move Back one

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(9, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('9', str.Current);
            Assert.AreEqual('7', str.Peek(-2));

            //Move Back another

            Assert.IsTrue(str.MovePrev());
            Assert.AreEqual(8, str.Offset);
            Assert.AreEqual(false, str.EOS);
            Assert.AreEqual('8', str.Current);
            Assert.AreEqual('9', str.Peek(1));

            Assert.IsTrue(str.Matches("89"));

            //Substring
            Assert.AreEqual(subchars.Substring(5), str.Substring(5));
            Assert.AreEqual(subchars.Substring(5, 5), str.Substring(5, 5));
            Assert.AreEqual(subchars.Substring(2, 2), str.Substring(2, 2));
        }


        private void SimpleDocumentParsing_Layout(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);
            _layoutcontext = context;
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
                margin: 20pt 10pt;
                padding: 10pt 5pt 15pt 1pt;
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
            Assert.AreEqual((Color)"#808080", one.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent));
            Assert.AreEqual((Color)"#222", one.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent));

            var two = col[1] as StyleDefn;

            Assert.AreEqual("body.grey div", two.Match.ToString());
            Assert.AreEqual(10, two.ValueCount); //All, Top, Left, Bottom and Right are all set for Margins and Padding
            // 96 pixels per inch, 72 points per inch
            Assert.AreEqual(7.5, two.GetValue(StyleKeys.PaddingAllKey, Unit.Zero).PointsValue);
            Assert.AreEqual(11.25, two.GetValue(StyleKeys.MarginsAllKey, Unit.Zero).PointsValue);

            var three = col[2] as StyleDefn;

            Assert.AreEqual("body.grey div.reverse", three.Match.ToString());
            Assert.AreEqual(2 + 4 + 4, three.ValueCount); //2 colors and 4 each for margins and padding

            Assert.AreEqual((Color)"#222", three.GetValue(StyleKeys.BgColorKey, StandardColors.Transparent));
            Assert.AreEqual((Color)"#808080", three.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent));

            Assert.AreEqual((Unit)20, three.GetValue(StyleKeys.MarginsTopKey, 0.0));
            Assert.AreEqual((Unit)20, three.GetValue(StyleKeys.MarginsBottomKey, 0.0));
            Assert.AreEqual((Unit)10, three.GetValue(StyleKeys.MarginsLeftKey, 0.0));
            Assert.AreEqual((Unit)10, three.GetValue(StyleKeys.MarginsRightKey, 0.0));

            Assert.AreEqual((Unit)10, three.GetValue(StyleKeys.PaddingTopKey, 0.0));
            Assert.AreEqual((Unit)5, three.GetValue(StyleKeys.PaddingRightKey, 0.0));
            Assert.AreEqual((Unit)15, three.GetValue(StyleKeys.PaddingBottomKey, 0.0));
            Assert.AreEqual((Unit)1, three.GetValue(StyleKeys.PaddingLeftKey, 0.0));
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
            Assert.IsTrue(elapsed < 0.20, "Took too long to parse. Expected < 0.20ms per string, Actual : " + elapsed + "ms");

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
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/CSS/Include.css";
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
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");


            }
        }

        [TestMethod]
        public void ParsePDFFontSource()
        {
            string sample = "url(https://somewebsite.com/path/to/font.woff)";

            FontSource parsed;
            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("https://somewebsite.com/path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(path/to/font.woff)";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(path/to/font.woff) format(\"woff\")";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.WOFF, parsed.Format);

            sample = "url('path/to/font.woff')";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/font.woff", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "url(\"path/to/svgfont.svg#example\")";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);


            sample = "url(\"path/to/svgfont.svg#example\") format(\"svg\")";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.SVG, parsed.Format);

            //Some locals

            sample = "local(font)";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            sample = "local(some font)";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);


            sample = "local('some font') format(truetype)";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.TrueType, parsed.Format);


            sample = "local(\"some other font\") format(\"opentype\")";

            Assert.IsTrue(FontSource.TryParseOneValue(sample, out parsed));
            Assert.AreEqual(FontSourceType.Local, parsed.Type);
            Assert.AreEqual("some other font", parsed.Source);
            Assert.AreEqual(FontSourceFormat.OpenType, parsed.Format);

            //empty is false
            Assert.IsFalse(FontSource.TryParseOneValue("", out parsed));

            //unbalanced quotes is false
            Assert.IsFalse(FontSource.TryParseOneValue("local(\"some other font) format(\"opentype\")", out parsed));

            //Unknown source type is false
            Assert.IsFalse(FontSource.TryParseOneValue("remote(\"path/to/svgfont.svg#example\") format(\"svg\")", out parsed));

            //Other marker e.g. other is ignored so true
            Assert.IsTrue(FontSource.TryParseOneValue("url(\"path/to/svgfont.svg#example\") other(\"svg\")", out parsed));
            Assert.AreEqual(FontSourceType.Url, parsed.Type);
            Assert.AreEqual("path/to/svgfont.svg#example", parsed.Source);
            Assert.AreEqual(FontSourceFormat.Default, parsed.Format);

            //Parse Multiple

            var full = @"local(font), url(path/to/font.svg) format('svg'),
                url(path/to/font.woff) format('woff'),
                url(path/to/font.ttf) format(truetype),
                url('path/to/font.otf') format(embedded-opentype)";

            Assert.IsTrue(FontSource.TryParse(full, out parsed));

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

        [TestMethod()]
        public void ParseFontFace()
        {

            var src = @"@font-face {
                          font-family: 'Roboto Condensed';
                          font-style: normal;
                          font-weight: 400;
                          src: url(https://fonts.gstatic.com/s/robotocondensed/v19/ieVl2ZhZI2eCN5jzbjEETS9weq8-59U.ttf) format('truetype');
                        }";

            var parser = new CSSStyleParser(src, null);
            StyleFontFace first = null;

            foreach (var item in parser)
            {
                if (null != first)
                    throw new InvalidOperationException("There has been more than one parsed style");

                if (!(item is StyleFontFace))
                    throw new InvalidCastException("The item is not a font face");

                first = item as StyleFontFace;
            }

            Assert.IsNotNull(first, "No font face was parsed");

            var fsrc = first.GetValue(StyleKeys.FontFaceSrcKey, null);

            Assert.AreEqual("https://fonts.gstatic.com/s/robotocondensed/v19/ieVl2ZhZI2eCN5jzbjEETS9weq8-59U.ttf", fsrc.Source, "Source does not match");
            Assert.AreEqual(FontSourceFormat.TrueType, fsrc.Format, "Format is invalid");


        }

        [TestMethod()]
        public void ParseCounters()
        {

            var src = @"ol {
                          counter-reset: group;
                        }
                        li::before{
                          counter-increment: group 10 other;
                          content: counter(group) '-';
                        }";

            var parser = new CSSStyleParser(src, null);
            StyleDefn ol = null;
            StyleDefn li = null;

            foreach (var item in parser)
            {
                if (item is StyleDefn defn)
                {
                    if (defn.Match.Selector.AppliedElement == "ol")
                        ol = defn;
                    else if (defn.Match.Selector.AppliedElement == "li" && defn.Match.Selector.AppliedState == ComponentState.Before)
                        li = defn;
                }

                
            }

            Assert.IsNotNull(ol, "No ordered list was parsed");
            Assert.IsNotNull(li, "No ordered list was parsed");

            ContentDescriptor content;

            

            Assert.AreEqual(1, ol.ValueCount);
            Assert.IsNotNull(ol.GetValue(StyleKeys.CounterResetKey, null));
            Assert.IsNull(ol.GetValue(StyleKeys.CounterIncrementKey, null));
            Assert.AreEqual("group", ol.GetValue(StyleKeys.CounterResetKey, null).Name);

            Assert.AreEqual(2, li.ValueCount);
            Assert.IsNotNull(li.GetValue(StyleKeys.CounterIncrementKey, null));
            Assert.IsNull(li.GetValue(StyleKeys.CounterResetKey, null));

            //li has 2 increment items - group with 10, and then other (with default 1)

            Assert.AreEqual("group", li.GetValue(StyleKeys.CounterIncrementKey, null).Name);
            Assert.AreEqual(10, li.GetValue(StyleKeys.CounterIncrementKey, null).Value);
            Assert.IsNotNull(li.GetValue(StyleKeys.CounterIncrementKey, null).Next);

            Assert.AreEqual("other", li.GetValue(StyleKeys.CounterIncrementKey, null).Next.Name);
            Assert.AreEqual(1, li.GetValue(StyleKeys.CounterIncrementKey, null).Next.Value);
            Assert.IsNull(li.GetValue(StyleKeys.CounterIncrementKey, null).Next.Next);

            content = li.GetValue(StyleKeys.ContentTextKey, null);
            Assert.IsNotNull(content);

            Assert.AreEqual(ContentDescriptorType.Counter, content.Type);
            Assert.IsInstanceOfType(content, typeof(ContentCounterDescriptor));
            Assert.AreEqual("group", (content as ContentCounterDescriptor).CounterName);
            Assert.IsNotNull(content.Next);

            content = content.Next;

            Assert.AreEqual(ContentDescriptorType.Text, content.Type);
            Assert.IsInstanceOfType(content, typeof(ContentTextDescriptor));
            Assert.AreEqual("-", (content as ContentTextDescriptor).Text);
            Assert.IsNull(content.Next);

        }

        [TestMethod]
        public void ParseBase64FontFace()
        {
            //The string containing the font data is declared in a separate static class.

            var base64 = Scryber.UnitTests.Base64FontData.OswaldBold;

            var src = @"@font-face {
                          font-family: Oswald;
                          font-style: normal;
                          font-weight: 700;
                          src: url('data:font/opentype; base64, " + base64 + @"') format('truetype');
                        }";

            var parser = new CSSStyleParser(src, null);
            StyleFontFace first = null;

            foreach (var item in parser)
            {
                if (null != first)
                    throw new InvalidOperationException("There has been more than one parsed style");

                if (!(item is StyleFontFace))
                    throw new InvalidCastException("The item is not a font face");

                first = item as StyleFontFace;

            }

            Assert.IsNotNull(first, "No font face was parsed");

            var fsrc = first.GetValue(StyleKeys.FontFaceSrcKey, null);

            Assert.AreEqual(FontSourceType.Base64, fsrc.Type, "Type is invalid");
            Assert.AreEqual(FontSourceFormat.TrueType, fsrc.Format, "Format is invalid");
            Assert.IsTrue(fsrc.Source.StartsWith("data:font/opentype; base64, "), "Source incorrectly starts with " + fsrc.Source.Substring(0, 20));
            Assert.IsTrue(fsrc.Source.EndsWith("=="), "Source incorrectly ends with " + fsrc.Source.Substring(fsrc.Source.Length - 10));
        }



        [TestMethod()]
        public void ParseCSSItemContentStyle()
        {
            var path = "Content/HTML/Images/group.png";

            var src = @".added{
                            content: 'replacement text'
                        }

                        .img-src{
                            content: url('" + path + @"');
                        }

                        .quote{
                            content: open-quote;
                        }

                        .counter{
                            content: counter(counterName);
                        }

                        .multiple{
                            color: red;
                            content: 'some text' url(""" + path + @""") linear-gradient(#000 #AAA);
                        }";

            var parser = new CSSStyleParser(src, null);
            List<Style> all = new List<Style>();

            foreach (var item in parser)
            {
                all.Add(item as Style);
            }

            Assert.AreEqual(5, all.Count);

            var added = all[0];
            var source = all[1];
            var quote = all[2];
            var counter = all[3];
            var multiple = all[4];

            //'replacement text'

            StyleValue<ContentDescriptor> parsed;
            Assert.AreEqual(1, added.ValueCount);
            Assert.IsTrue(added.TryGetValue(StyleKeys.ContentTextKey, out parsed));
            var value = parsed.Value(added);

            Assert.IsNotNull(value);
            Assert.AreEqual(ContentDescriptorType.Text, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentTextDescriptor));
            Assert.AreEqual("replacement text", (value as ContentTextDescriptor).Text);
            
            Assert.IsNull(value.Next);

            //'url(...)'

            Assert.AreEqual(1, source.ValueCount);
            Assert.IsTrue(source.TryGetValue(StyleKeys.ContentTextKey, out parsed));
            value = parsed.Value(source);

            Assert.IsNotNull(value);
            Assert.AreEqual(ContentDescriptorType.Image, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentImageDescriptor));
            Assert.AreEqual("url('Content/HTML/Images/group.png')", (value as ContentImageDescriptor).Text);
            
            Assert.IsNull(value.Next);

            //quote
            Assert.AreEqual(1, quote.ValueCount);
            Assert.IsTrue(quote.TryGetValue(StyleKeys.ContentTextKey, out parsed));
            value = parsed.Value(quote);

            Assert.IsNotNull(value);
            Assert.AreEqual(ContentDescriptorType.Quote, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentQuoteDescriptor));
            Assert.AreEqual("open-quote", (value as ContentQuoteDescriptor).Text);
            Assert.AreEqual("“", (value as ContentQuoteDescriptor).Chars); //Change to the open-quote char
            Assert.IsNull(value.Next);

            //counter
            Assert.AreEqual(1, counter.ValueCount);
            Assert.IsTrue(counter.TryGetValue(StyleKeys.ContentTextKey, out parsed));
            value = parsed.Value(counter);

            Assert.IsNotNull(value);
            Assert.AreEqual(ContentDescriptorType.Counter, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentCounterDescriptor));
            Assert.AreEqual("counterName", (value as ContentCounterDescriptor).CounterName);
            Assert.IsNull(value.Next);

            // multiple
            Assert.AreEqual(2, multiple.ValueCount);
            Assert.IsTrue(multiple.TryGetValue(StyleKeys.ContentTextKey, out parsed));
            value = parsed.Value(multiple);

            Assert.IsNotNull(value);
            Assert.AreEqual(ContentDescriptorType.Text, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentTextDescriptor));
            Assert.AreEqual("some text", (value as ContentTextDescriptor).Text);
            Assert.IsNotNull(value.Next);

            value = value.Next;
            Assert.AreEqual(ContentDescriptorType.Image, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentImageDescriptor));
            Assert.AreEqual("url(\"Content/HTML/Images/group.png\")", (value as ContentImageDescriptor).Text);
            Assert.AreEqual("Content/HTML/Images/group.png", (value as ContentImageDescriptor).Source);
            Assert.IsNotNull(value.Next);

            value = value.Next;
            Assert.AreEqual(ContentDescriptorType.Gradient, value.Type);
            Assert.IsInstanceOfType(value, typeof(ContentGradientDescriptor));
            Assert.AreEqual("linear-gradient(#000 #AAA)", (value as ContentGradientDescriptor).Text);
            Assert.IsNotNull((value as ContentGradientDescriptor).Gradient);
            Assert.IsNull(value.Next);

        }


        [TestMethod]
        public void ParseCSSPseudoClasses()
        {
            var src = @".added::before{
                            content: 'replacement text'
                        }

                        
                        .quote::before{
                            content: open-quote;
                        }
                        /* Should be backwards compatible and support single colon */
                        .quote:after{
                            content: close-quote;
                        }

                        h1:hover{
                            color: red;
                        }

                        /* Focus is not supported */

                        h1:focus{
                            color: red;
                        }";

            var parser = new CSSStyleParser(src, null);
            List<Style> all = new List<Style>();

            foreach (var item in parser)
            {
                all.Add(item as Style);
            }

            Assert.AreEqual(5, all.Count);

            var added = all[0] as StyleDefn;
            var quoteBefore = all[1] as StyleDefn;
            var quoteAfter = all[2] as StyleDefn;
            var hover = all[3] as StyleDefn;
            var notsupported = all[4] as StyleDefn;

            Assert.IsNotNull(added);
            Assert.IsNotNull(quoteBefore);
            Assert.IsNotNull(quoteAfter);
            Assert.IsNotNull(hover);
            Assert.IsNotNull(notsupported);

            Assert.AreEqual(ComponentState.Before, added.Match.Selector.AppliedState);
            Assert.AreEqual("added", added.Match.Selector.AppliedClass.ClassName);
            Assert.IsNull(added.Match.Selector.AppliedElement);
            Assert.IsNull(added.Match.Selector.AppliedID);

            Assert.AreEqual(ComponentState.Before, quoteBefore.Match.Selector.AppliedState);
            Assert.AreEqual("quote", quoteBefore.Match.Selector.AppliedClass.ClassName);
            Assert.IsNull(quoteBefore.Match.Selector.AppliedElement);
            Assert.IsNull(quoteBefore.Match.Selector.AppliedID);

            Assert.AreEqual(ComponentState.After, quoteAfter.Match.Selector.AppliedState);
            Assert.AreEqual("quote", quoteAfter.Match.Selector.AppliedClass.ClassName);
            Assert.IsNull(quoteAfter.Match.Selector.AppliedElement);
            Assert.IsNull(quoteAfter.Match.Selector.AppliedID);

            Assert.AreEqual(ComponentState.Over, hover.Match.Selector.AppliedState);
            Assert.IsNull(hover.Match.Selector.AppliedClass);
            Assert.AreEqual("h1", hover.Match.Selector.AppliedElement);
            Assert.IsNull(hover.Match.Selector.AppliedID);

            //If the pseudo is not known then it should be used as the full name
            Assert.AreEqual(ComponentState.Normal, notsupported.Match.Selector.AppliedState);
            Assert.IsNull(notsupported.Match.Selector.AppliedClass);
            Assert.AreEqual("h1:focus", notsupported.Match.Selector.AppliedElement);
            Assert.IsNull(notsupported.Match.Selector.AppliedID);
        }

        [TestMethod]
        public void ParseCSSComplexPseudoClasses()
        {
            var src = @".added::before, .added::after{
                            content: '!'
                        }

                        
                        .cite .quote::before{
                            content: open-quote;
                        }

                        .cite .quote::after{
                            content: close-quote;
                        }

                        a:hover > i::before{
                            color: red;
                            content: '>>';
                        }

                        /* Focus is not supported */

                        article > h1:focus{
                            color: red;
                        }";

            var parser = new CSSStyleParser(src, null);
            List<Style> all = new List<Style>();

            foreach (var item in parser)
            {
                all.Add(item as Style);
            }

            Assert.AreEqual(5, all.Count);

            var added = all[0] as StyleDefn;
            var quoteBefore = all[1] as StyleDefn;
            var quoteAfter = all[2] as StyleDefn;
            var hover = all[3] as StyleDefn;
            var notsupported = all[4] as StyleDefn;

            Assert.IsNotNull(added);
            Assert.IsNotNull(quoteBefore);
            Assert.IsNotNull(quoteAfter);
            Assert.IsNotNull(hover);
            Assert.IsNotNull(notsupported);

            //.added::before, .added::after
            //These are parsed in reverse order for commas
            var match = added.Match;

            Assert.AreEqual(ComponentState.After, match.Selector.AppliedState);
            Assert.AreEqual("added", match.Selector.AppliedClass.ClassName);
            Assert.IsNull(match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);

            Assert.IsInstanceOfType(match, typeof(Scryber.Styles.Selectors.StyleMultipleMatcher));
            match = ((Scryber.Styles.Selectors.StyleMultipleMatcher)added.Match).Next;
            Assert.IsNotNull(match);
            Assert.AreEqual(ComponentState.Before, match.Selector.AppliedState);
            Assert.AreEqual("added", match.Selector.AppliedClass.ClassName);
            Assert.IsNull(match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);


            //.cite .quote::before
            match = quoteBefore.Match;
            Assert.AreEqual(ComponentState.Before, match.Selector.AppliedState);
            Assert.AreEqual("quote", match.Selector.AppliedClass.ClassName);
            Assert.IsNull(match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);
            Assert.IsTrue(match.Selector.HasAncestor);

            var ancestor = match.Selector.Ancestor;
            Assert.IsNotNull(ancestor);
            Assert.AreEqual(Scryber.Styles.Selectors.StylePlacement.Any, ancestor.Placement);
            Assert.AreEqual(ComponentState.Normal, ancestor.AppliedState);
            Assert.AreEqual("cite", ancestor.AppliedClass.ClassName);
            Assert.IsNull(ancestor.AppliedElement);
            Assert.IsNull(ancestor.AppliedID);

            Assert.IsNotInstanceOfType(match, typeof(Scryber.Styles.Selectors.StyleMultipleMatcher));

            //.cite .quote::after
            match = quoteAfter.Match;
            Assert.AreEqual(ComponentState.After, match.Selector.AppliedState);
            Assert.AreEqual("quote", match.Selector.AppliedClass.ClassName);
            Assert.IsNull(match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);
            Assert.IsTrue(match.Selector.HasAncestor);

            ancestor = match.Selector.Ancestor;
            Assert.IsNotNull(ancestor);
            Assert.AreEqual(Scryber.Styles.Selectors.StylePlacement.Any, ancestor.Placement);
            Assert.AreEqual(ComponentState.Normal, ancestor.AppliedState);
            Assert.AreEqual("cite", ancestor.AppliedClass.ClassName);
            Assert.IsNull(ancestor.AppliedElement);
            Assert.IsNull(ancestor.AppliedID);

            Assert.IsNotInstanceOfType(match, typeof(Scryber.Styles.Selectors.StyleMultipleMatcher));

            //a:hover > i::before
            match = hover.Match;
            Assert.AreEqual(ComponentState.Before, match.Selector.AppliedState);
            Assert.IsNull(match.Selector.AppliedClass);
            Assert.AreEqual("i", match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);
            Assert.IsTrue(match.Selector.HasAncestor);

            ancestor = match.Selector.Ancestor;
            Assert.IsNotNull(ancestor);
            Assert.AreEqual(Scryber.Styles.Selectors.StylePlacement.DirectParent, ancestor.Placement);
            Assert.AreEqual(ComponentState.Over, ancestor.AppliedState);
            Assert.IsNull(ancestor.AppliedClass);
            Assert.AreEqual("a", ancestor.AppliedElement);
            Assert.IsNull(ancestor.AppliedID);

            Assert.IsNotInstanceOfType(match, typeof(Scryber.Styles.Selectors.StyleMultipleMatcher));


            //article > h1:focus
            //If the pseudo is not known then it should be used as the full name
            match = notsupported.Match;
            Assert.AreEqual(ComponentState.Normal, match.Selector.AppliedState);
            Assert.IsNull(match.Selector.AppliedClass);
            Assert.AreEqual("h1:focus", match.Selector.AppliedElement);
            Assert.IsNull(match.Selector.AppliedID);
            Assert.IsTrue(match.Selector.HasAncestor);

            ancestor = match.Selector.Ancestor;
            Assert.IsNotNull(ancestor);
            Assert.AreEqual(Scryber.Styles.Selectors.StylePlacement.DirectParent, ancestor.Placement);
            Assert.AreEqual(ComponentState.Normal, ancestor.AppliedState);
            Assert.IsNull(ancestor.AppliedClass);
            Assert.AreEqual("article", ancestor.AppliedElement);
            Assert.IsNull(ancestor.AppliedID);
        }


        PDF.Layout.PDFLayoutDocument _docLayout;

        [TestMethod]
        public void ParseCSSWithContentApplied()
        {
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";

            var src = @"<?scryber append-log=true ?>
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>Content Tests</title>
                        <style type='text/css' >

                            div{
                                border: solid 1px silver;
                                margin: 20px;
                            }

                            .txt{ content: 'This will not be used'; height: 40pt; /* This should be used */ }

                            .img{content: url('" + imgPath + @"'); height: 50pt;}

                        </style>
                    </head>
                    <body>
                        <div id='default' class='txt' > This will not be replaced by the css.</div>
                        <div id='explicit' class='' ><img class='img' />
                            <!-- the source is set from the css -->
                        </div>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);


                using (var stream = DocStreams.GetOutputStream("ParseCSSWithContentApplied.pdf"))
                {

                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }

            var pg = _docLayout.AllPages[0];
            var content = pg.ContentBlock.Columns[0];
            Assert.AreEqual(2, content.Contents.Count);

            // .txt{ content: 'This will not be used'; height: 40pt; /* This should be used */ }
            // <div id='default' class='txt' > This will not be replaced by the css.</div>

            var div = content.Contents[0] as PDFLayoutBlock;
            var line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

            //'          content    '        
            //TextBegin, Text,      TextEnd

            Assert.AreEqual(3, line.Runs.Count);
            var text = line.Runs[1] as PDF.Layout.PDFTextRunCharacter;
            Assert.IsNotNull(text);
            Assert.AreEqual("This will not be replaced by the css.", text.Characters);
            //Check the height was not ignored
            Assert.AreEqual(40, div.Position.Height.Value);

            //.img{content: url('[imgPath]'); height: 50pt;}
            //<div id='explicit' class='' ><img class='img' /></div>

            div = content.Contents[1] as PDFLayoutBlock;
            line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;


            var imgPointer = line.Runs[0] as PDF.Layout.PDFLayoutComponentRun;
            Assert.IsInstanceOfType(imgPointer.Owner, typeof(Image));
            Assert.AreEqual(50.0, imgPointer.Height.PointsValue);
            Assert.AreEqual(imgPath, (imgPointer.Owner as Image).Source);

            var rsrc = _docLayout.DocumentComponent.SharedResources.GetResource(PDF.Resources.PDFResource.XObjectResourceType, imgPath);
            Assert.IsNotNull(rsrc);
            Assert.AreEqual(rsrc, (imgPointer.Owner as Image).XObject);

        }

        [TestMethod]
        public void ParseCSSWithContentBefore()
        {
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var src = @"<?scryber append-log='false' ?>
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>Content Tests</title>
                        <style type='text/css' >

                            div{ border: solid 1px silver; margin: 20px; padding-bottom: 5pt; }

                            .txt{ color: red; }

                            div > i.txt::before{ content: '>>'; color:blue; }

                            .empty::before { content: url('" + imgPath + @"'); width:20pt; padding-top: 5pt; }

                            .quote::before{ content: open-quote; width:20pt; color: green; padding-top: 5pt; }

                            .multiple::before{ content: url('" + imgPath + @"') '\40' open-quote; color: green; font-style: italic; }

                            .multiple img { padding-top: 5px; display: inline; width: 20pt; }
                           
                        </style>
                    </head>
                    <body>
                        <div id='default' >
                            <i id='txt1' class='txt'>
                                This will have text before, in the italic span.
                            </i>
                        </div>
                        <div id='anImage' class='empty' ></div>
                        <div id='aquote' class='quote' >A quote will be in front in green.</div>
                        <div id='aquote' class='multiple' > An image, a character, and a quote will be infront with the image inlined.</div>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                
                using (var stream = DocStreams.GetOutputStream("ParseCSSWithContentBefore.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }


                var pg = _docLayout.AllPages[0];
                var content = pg.ContentBlock.Columns[0];
                Assert.AreEqual(4, content.Contents.Count);

                // .txt::before { content: '>>'; color: blue; }
                //<i id='inner1' class='txt'> This will have some content before inside the italic span.</i>

                var div = content.Contents[0] as PDF.Layout.PDFLayoutBlock;
                var line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

                //<i           <before      '          >>    '        span>      '          content  '         span> 
                //InlineBegin, InlineBegin, TextBegin, Text, TextEnd, InlineEnd, TextBegin, Text    , TextEnd, InlineEnd

                Assert.AreEqual(10, line.Runs.Count);
                var text = line.Runs[3] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual(">>", text.Characters);


                
                text = line.Runs[7] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual(" This will have text before, in the italic span. ", text.Characters);


                // .empty::before { content: url('imgPath'); width: 20pt; padding-top: 5pt; }
                //  <div id='anImage' class='empty' ></div>

                div = content.Contents[1] as PDF.Layout.PDFLayoutBlock;
                line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

                //Images should be on their own if there is no other content in the PseudoClass
                //<img   
                //ComponentRun

                Assert.AreEqual(1, line.Runs.Count);

                var imgPointer = line.Runs[0] as PDF.Layout.PDFLayoutComponentRun;
                Assert.IsInstanceOfType(imgPointer.Owner, typeof(Image));
                Assert.AreEqual(20.0, imgPointer.Width.PointsValue);
                Assert.AreEqual(imgPath, (imgPointer.Owner as Image).Source);
                var rsrc = _docLayout.DocumentComponent.SharedResources.GetResource(PDF.Resources.PDFResource.XObjectResourceType, imgPath);
                Assert.IsNotNull(rsrc);
                Assert.AreEqual(rsrc, (imgPointer.Owner as Image).XObject);

                


                // .quote::before{ content: open-quote; width:20pt; color: green; padding-top: 5pt; }
                // <div id='aquote' class='quote' >A quote will be in front in green.</div>

                div = content.Contents[2] as PDF.Layout.PDFLayoutBlock;
                line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

                //(before span) '           “     '        (before end)    '          content   '        line    
                //InlineBegin,   TextBegin, Text, TextEnd, InlineEnd    ,  TextBegin, Text,     TextEnd, InlineEnd

                Assert.AreEqual(8, line.Runs.Count);

                text = line.Runs[2] as PDF.Layout.PDFTextRunCharacter;

                Assert.IsNotNull(text);
                Assert.AreEqual("“", text.Characters);
                Assert.AreEqual(StandardColors.Green, (line.Runs[0] as PDFLayoutInlineBegin).FullStyle.Fill.Color);

                text = line.Runs[6] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual("A quote will be in front in green.", text.Characters);


                //<div id='aquote' class='multiple' > An image ... image inlined.</div>
                //.multiple::before{ content: url('" + imgPath + @"') '\40' open-quote; color: green; font-style: italic; }
                //.multiple img { padding-top: 5px; display: inline; width: 20pt; }

                div = content.Contents[3] as PDF.Layout.PDFLayoutBlock;
                line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

                //(before span) img   '           @     '        '         “     '         (before end)    '          part content  \r\n    
                //InlineBegin,  Image  TextBegin, Text, TextEnd, TextBegin Text   TextEnd, InlineEnd    ,  TextBegin, Text,         TextNewLine

                Assert.AreEqual(12, line.Runs.Count);
                Assert.AreEqual(StandardColors.Green, (line.Runs[0] as PDFLayoutInlineBegin).FullStyle.Fill.Color);

                imgPointer = line.Runs[1] as PDF.Layout.PDFLayoutComponentRun;
                Assert.IsNotNull(imgPointer);
                Assert.IsInstanceOfType(imgPointer.Owner, typeof(Image));
                Assert.AreEqual(20.0, imgPointer.Width.PointsValue);
                Assert.AreEqual(imgPath, (imgPointer.Owner as Image).Source);

                text = line.Runs[3] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual("@", text.Characters);


                text = line.Runs[6] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual("“", text.Characters);
                
                text = line.Runs[10] as PDF.Layout.PDFTextRunCharacter;
                Assert.IsNotNull(text);
                Assert.AreEqual(" An image, a character, and a quote will be infront with the image inlined.", text.Characters);
            }
        }



        [TestMethod]
        public void ParseCSSWithContentAfter()
        {
            var imgPath = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>Content Tests</title>
                        <style type='text/css' >

                            div{
                                border: solid 1px silver;
                                margin: 20px;
                                padding-bottom: 5pt;
                            }

                            .txt{
                                color: red;
                            }

                            .txt::after{
                                content: '&lt;&lt;';
                                color:blue;
                            }


                            .empty::after {
                                content: url('" + imgPath + @"') 'and some text';
                            }

                            .empty img { display: inline; height:25pt; }

                            .quote::after{
                                content: close-quote;
                                color: green;
                            }

                        </style>
                    </head>
                    <body>
                        <div id='default' class='' >
                            <span id='inner1' class='txt'>This will be have content after.</span>
                        </div>
                        <div id='explicit' class='empty' ></div>
                        <div id='asAClass' class='quote' >
                            An italic quote will follow.
                        </div>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);


                using (var stream = DocStreams.GetOutputStream("ParseCSSWithContentAfter.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }

            var pg = _docLayout.AllPages[0];
            var content = pg.ContentBlock.Columns[0];
            Assert.AreEqual(3, content.Contents.Count);

            // .txt::after{ content: '&lt;&lt;'; color: blue; }
            //<span id='inner1' class='txt'> This will be have content after.</span>

            var div = content.Contents[0] as PDF.Layout.PDFLayoutBlock;
            var line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

            //<span       '           content      ' <span       '           <<           ' span>      span>
            //InlineBegin, TextBegin, Text, TextEnd, InlineBegin, TextBegin, Text, TextEnd, InlineEnd, InlineEnd

            Assert.AreEqual(10, line.Runs.Count);
            var text = line.Runs[2] as PDF.Layout.PDFTextRunCharacter;
            Assert.IsNotNull(text);
            Assert.AreEqual("This will be have content after.", text.Characters);

            text = line.Runs[6] as PDF.Layout.PDFTextRunCharacter;
            Assert.IsNotNull(text);
            Assert.AreEqual("<<", text.Characters);

            // .empty::after { content: url('...') 'and some text'; } 
            // .empty img { display: inline; height:25pt; }
            // <div id='explicit' class='empty' ></div>

            div = content.Contents[1] as PDF.Layout.PDFLayoutBlock;
            line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

            //<span        img    '         content      '  span>    
            //InlineBegin, Image, TextBegin, Text, TextEnd, InlineEnd

            Assert.AreEqual(6, line.Runs.Count);

            var imgPointer = line.Runs[1] as PDF.Layout.PDFLayoutComponentRun;
            Assert.IsInstanceOfType(imgPointer.Owner, typeof(Image));
            Assert.AreEqual(25.0, imgPointer.Height.PointsValue);
            Assert.AreEqual(imgPath, (imgPointer.Owner as Image).Source);

            // .quote::after{ content: close-quote; color: green;}
            // <div id='asAClass' class='quote' >An italic quote will follow.</div>

            div = content.Contents[2] as PDF.Layout.PDFLayoutBlock;
            line = div.Columns[0].Contents[0] as PDF.Layout.PDFLayoutLine;

            //'          content      ' <span       '           "           '  span>    
            //TextBegin, Text, TextEnd, InlineBegin, TextBegin, Text, TextEnd, InlineEnd

            Assert.AreEqual(8, line.Runs.Count);

            text = line.Runs[1] as PDF.Layout.PDFTextRunCharacter;
            Assert.IsNotNull(text);
            Assert.AreEqual("An italic quote will follow. ", text.Characters);

            text = line.Runs[5] as PDF.Layout.PDFTextRunCharacter;
            Assert.IsNotNull(text);
            Assert.AreEqual("”", text.Characters);

            
        }



        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this._docLayout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
        }

    

        [TestMethod()]
        public void ParseGoogleFontLink()
        {
            var link = "<link href=\"https://fonts.googleapis.com/css2?family=Roboto+Condensed:wght@400&amp;display=swap\" rel=\"stylesheet\"/>";
            var family = "\"Roboto Condensed\"";
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>{{concat('Hello ', model.user.firstname)}}</title>
                        " + link + @"
                    </head>
                    <body>
                        <div id='lime' style='font-family: " + family + @"' >
                            <span id='inner1' class=''>
                                Should be in Roboto Condensed
                            </span>
                        </div>
                    </body>
                </html>";

            Document doc = null;

            using (var reader = new System.IO.StringReader(src))
            {
                doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);


                using (var stream = DocStreams.GetOutputStream("FontsRobotoCondensed.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                Assert.AreEqual(1, doc.SharedResources.Count, "Remote font not loaded");
                var fntRsrc = doc.SharedResources[0] as PDFFontResource;
                Assert.IsNotNull(fntRsrc, "The font was not loaded");
                var name = fntRsrc.FontName;
                Assert.AreEqual("Roboto Condensed", name, "The font name does not match");


            }

        }

        [TestMethod()]
        public void ParseTransformations()
        {

            var src = @"
                        .rotate {
                          transform : rotate(10deg);
                        }

                        .skew {
                          transform:skew(25deg, 15deg);
                        }

                        .translate {
                          transform:   translate(10pt, 15pt);
                        }

                        .scale {
                          transform: scale(1, 5);
                        }

                        .multiple {
                          transform: rotate(20deg) scale(2, 4) translate(5pt, 20pt);
                        }
";

            var parser = new CSSStyleParser(src, null);
            List<StyleDefn> found = new List<StyleDefn>();

            foreach (var item in parser)
            {
                found.Add((StyleDefn)item);
            }

            Assert.AreEqual(5,found.Count);

            var t1 = found[0].GetValue(StyleKeys.TransformOperationKey, null);
            var t2 = found[1].GetValue(StyleKeys.TransformOperationKey, null);
            var t3 = found[2].GetValue(StyleKeys.TransformOperationKey, null);
            var t4 = found[3].GetValue(StyleKeys.TransformOperationKey, null);
            var multiple = found[4].GetValue(StyleKeys.TransformOperationKey, null);

            Assert.IsNotNull(t1);
            Assert.AreEqual(TransformType.Rotate, t1.Type);
            Assert.AreEqual(-Math.Round((Math.PI / 180) * 10, 4), Math.Round(t1.Value1, 4));
            Assert.IsFalse(TransformOperation.IsSet(t1.Value2));

            Assert.IsNotNull(t2);
            Assert.AreEqual(TransformType.Skew, t2.Type);
            Assert.AreEqual(-Math.Round((Math.PI / 180) * 25, 4), Math.Round(t2.Value1, 4));
            Assert.AreEqual(-Math.Round((Math.PI / 180) * 15, 4), Math.Round(t2.Value2, 4));

            Assert.IsNotNull(t3);
            Assert.AreEqual(TransformType.Translate, t3.Type);
            Assert.AreEqual(Math.Round(10.0, 4), Math.Round(t3.Value1, 4));
            Assert.AreEqual(-Math.Round(15.0, 4), Math.Round(t3.Value2, 4));

            Assert.IsNotNull(t4);
            Assert.AreEqual(TransformType.Scale, t4.Type);
            Assert.AreEqual(Math.Round(1.0, 4), Math.Round(t4.Value1, 4));
            Assert.AreEqual(Math.Round(5.0, 4), Math.Round(t4.Value2, 4));

            Assert.IsNotNull(multiple);
            Assert.AreEqual(TransformType.Rotate, multiple.Type);
            Assert.AreEqual(-Math.Round((Math.PI / 180) * 20, 4), Math.Round(multiple.Value1, 4));
            Assert.IsFalse(TransformOperation.IsSet(multiple.Value2));
            Assert.IsNotNull(multiple.Next);

            multiple = multiple.Next;
            Assert.AreEqual(TransformType.Scale, multiple.Type);
            Assert.AreEqual(Math.Round(2.0, 4), Math.Round(multiple.Value1, 4));
            Assert.AreEqual(Math.Round(4.0, 4), Math.Round(multiple.Value2, 4));
            Assert.IsNotNull(multiple.Next);

            multiple = multiple.Next;
            Assert.AreEqual(TransformType.Translate, multiple.Type);
            Assert.AreEqual(Math.Round(5.0, 4), Math.Round(multiple.Value1, 4));
            Assert.AreEqual(-Math.Round(20.0, 4), Math.Round(multiple.Value2, 4));
        }

        [TestMethod()]
        public void ParseCSSWithRoot()
        {
            var css = @"
                :root{
                    color: #00FF00;
                }

                .other{
                    color: #0000FF
                }";
            using (var doc = BuildDocumentWithStyles(css))
            {
                var applied = doc.GetAppliedStyle();
                Assert.AreEqual("rgb(0,255,0)", applied.Fill.Color.ToString());
            }

            using (var doc = BuildDocumentWithStyles(css))
            {

                //This should override the root declaration
                doc.StyleClass = "other";

                var applied = doc.GetAppliedStyle();
                Assert.AreEqual("rgb(0,0,255)", applied.Fill.Color.ToString());
            }
        }

        


        [TestMethod()]
        public void ParseCSSWithVariables()
        {
            //1. Initial to make sure it is parsed, but should not be used

            string cssWithVariable = @"

                :root{
                    color: #00FF00;
                    --main-color: #FF0000;
                }

                .other{
                    color: var(--main-color);
                }";

            using (var doc = BuildDocumentWithStyles(cssWithVariable))
            {
                //Check that the variable is there.
                Assert.AreEqual(2, doc.Styles.Count);
                StyleDefn defn = doc.Styles[0] as StyleDefn;

                Assert.IsTrue(defn.HasVariables);
                Assert.AreEqual(1, defn.Variables.Count);
                Assert.AreEqual("--main-color", defn.Variables["--main-color"].CssName);
                Assert.AreEqual("main-color", defn.Variables["--main-color"].NormalizedName);
                Assert.AreEqual("#FF0000", defn.Variables["--main-color"].Value);

                //Should not be applied
                var applied = doc.GetAppliedStyle();
                Assert.AreEqual("rgb(0,255,0)", applied.Fill.Color.ToString());
            }
        }

        [TestMethod]
        public void ParseCSSWithVariablesApplied()
        {

            //2. Second check that will use the variable
            string cssWithVariable = @"

                :root{
                    color: #00FF00;
                    --main-color: #FF0000;
                }

                .other{
                    color: var(--main-color);
                }";

            using (var doc = BuildDocumentWithStyles(cssWithVariable))
            {
                //This should override the root declaration
                doc.StyleClass = "other";

                var applied = doc.GetAppliedStyle();
                Assert.AreEqual("rgb(255,0,0)", applied.Fill.Color.ToString(), "Variable '--main-color' was not applied to the document");
            }
        }

        [TestMethod]
        public void ParseCSSWithVariablesOverriden()
        {
            //3. Third check that will use the items collection rather than the declared value
            string cssWithVariable = @"

                :root{
                    color: #00FF00;
                    --main-color: #FF0000;
                }

                .other{
                    color: var(--main-color);
                }";

            using (var doc = BuildDocumentWithStyles(cssWithVariable))
            {
                
                doc.StyleClass = "other";

                //And now we apply the color to the params collection
                doc.Params["--main-color"] = StandardColors.Aqua;

                var applied = doc.GetAppliedStyle();
                Assert.AreEqual(StandardColors.Aqua.ToString(), applied.Fill.Color.ToString(), "Parameter '--main-color' was not overriden in the document based on the parameters");

            }
        }

        [TestMethod]
        public void ParseCSSWithInnerVariableApplied()
        {
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>{{concat('Hello ', model.user.firstname)}}</title>
                        <style type='text/css' >

                            :root{
                                color: #00FF00;
                                --var-color: #FF0000;
                            }

                            body{
                                padding:20px;
                            }
                            .asclass{
                                color: var(--var-color);
                            }

                            .change{
                                --var-color: #0000FF;
                                font-style: italic;
                            }
                        </style>
                    </head>
                    <body>
                        <div id='default' class='' >
                            <span id='inner1' class=''>
                                Default root color of lime (#00FF00)
                            </span>
                        </div>
                        <div id='explicit' class='' >
                            <span id='inner2' style='color: var(--var-color);' >
                                Explicit color of the variable value = red (#FF0000)
                            </span>
                        </div>
                        <div id='asAClass' class='' >
                            <span id='inner1' class='asclass'>
                                Assigned as a class value = red (#FF0000)
                            </span>
                        </div>
                        <div id='overridden' class='change' >
                            <span id='inner' class='asclass'>
                                Overwritten by the outer change class to blue (#0000FF)
                            </span>
                        </div>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                

                using (var stream = DocStreams.GetOutputStream("CSSVariableApplicationTest.pdf"))
                {
                    doc.LayoutComplete += ParseCSSWithInnerVariableApplied_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }
        }

        private void ParseCSSWithInnerVariableApplied_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Make sure the variables are correctly assigned to the inline begin spans.
            var context = (PDFLayoutContext)(args.Context);
            var layout = context.DocumentLayout;
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;
            var def = content.Columns[0].Contents[0] as PDFLayoutBlock; //Default block
            var exp = content.Columns[0].Contents[1] as PDFLayoutBlock; //Explicit block
            var asClass = content.Columns[0].Contents[2] as PDFLayoutBlock; //As a class block
            var over = content.Columns[0].Contents[3] as PDFLayoutBlock; //Overridden block

            Assert.AreEqual(StandardColors.Lime, CheckInnerVariableSpanColor(def), "The default colour did not match lime");
            Assert.AreEqual(StandardColors.Red, CheckInnerVariableSpanColor(exp), "The explicit colour did not match red");
            Assert.AreEqual(StandardColors.Red, CheckInnerVariableSpanColor(asClass), "The assigned class colour did not match red");
            Assert.AreEqual(StandardColors.Blue, CheckInnerVariableSpanColor(over), "The overriden colour did not match blue");
        }


        private Color CheckInnerVariableSpanColor(PDFLayoutBlock block)
        {
            var inline = block.Columns[0].Contents[0] as PDFLayoutLine;
            var span = inline.Runs[0] as PDFLayoutInlineBegin;
            return span.FullStyle.Fill.Color;
        }


        /// <summary>
        /// Checks the calc value on style attributes. Appling both a light and a dark theme with properties
        /// </summary>
        [TestMethod]
        public void ParseCSSWithInlineCalcApplied()
        {
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>{{concat('Hello ', model.user.firstname)}}</title>
                    </head>
                    <body>
                        <div id='lime' style='color: calc(theme.lime);' >
                            <span id='inner1' class=''>
                                Default root color of lime (#00FF00)
                            </span>
                        </div>
                        <div id='red' style='color: calc(theme.red);' >
                            <span id='inner2' >
                                Explicit color of the variable value = red (#FF0000)
                            </span>
                        </div>
                        <div id='darkorlight' style='background-color: calc(if(theme.dark, #000, #FFF)); color: calc(if(theme.dark, theme.lime, #000));' >
                            <span id='inner1' class='asclass'>
                                Based on dark
                            </span>
                        </div>
                        
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["theme"] = new
                {
                    lime = StandardColors.Lime,
                    red = StandardColors.Red,
                    dark = false
                };

                using (var stream = DocStreams.GetOutputStream("CSSInlineCalcLightTest.pdf"))
                {
                    doc.LayoutComplete += ParseCSSWithInlineCalcLight_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }

            //Change to the dark theme
            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["theme"] = new
                {
                    lime = StandardColors.Lime,
                    red = StandardColors.Red,
                    dark = true
                };

                using (var stream = DocStreams.GetOutputStream("CSSInlineCalcDarkTest.pdf"))
                {
                    doc.LayoutComplete += ParseCSSWithInlineCalcDark_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }
        }

        private void ParseCSSWithInlineCalcLight_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Make sure the variables are correctly assigned to the inline begin spans.
            var context = (PDFLayoutContext)(args.Context);
            var layout = context.DocumentLayout;
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;
            var lime = content.Columns[0].Contents[0] as PDFLayoutBlock; //Default block
            var red = content.Columns[0].Contents[1] as PDFLayoutBlock; //Explicit block
            var dark = content.Columns[0].Contents[2] as PDFLayoutBlock; //As a class block

            Assert.AreEqual(StandardColors.Lime, lime.FullStyle.Fill.Color, "The default colour did not match lime");
            Assert.AreEqual(StandardColors.Red, red.FullStyle.Fill.Color, "The explicit colour did not match red");
            Assert.AreEqual(StandardColors.Black, dark.FullStyle.Fill.Color, "The calc colour did not match black");
            Assert.AreEqual(StandardColors.White, dark.FullStyle.Background.Color, "The calc background colour did not match white");
        }

        private void ParseCSSWithInlineCalcDark_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Make sure the variables are correctly assigned to the inline begin spans.
            var context = (PDFLayoutContext)(args.Context);
            var layout = context.DocumentLayout;
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;
            var lime = content.Columns[0].Contents[0] as PDFLayoutBlock; //Default block
            var red = content.Columns[0].Contents[1] as PDFLayoutBlock; //Explicit block
            var dark = content.Columns[0].Contents[2] as PDFLayoutBlock; //As a class block

            Assert.AreEqual(StandardColors.Lime, lime.FullStyle.Fill.Color, "The default colour did not match lime");
            Assert.AreEqual(StandardColors.Red, red.FullStyle.Fill.Color, "The explicit colour did not match red");
            Assert.AreEqual(StandardColors.Lime, dark.FullStyle.Fill.Color, "The calc colour did not match lime");
            Assert.AreEqual(StandardColors.Black, dark.FullStyle.Background.Color, "The calc background colour did not match black");
        }

        [TestMethod]
        public void ParseCSSWithInlineCalcRepeatingApplied()
        {
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>Repeating calc in template</title>
                    </head>
                    <body>
                        <template data-bind='{{model.items}}' >
                            <div style='background-color: calc(if(index() % 2 == 1, model.red, #00FF00));' >
                                <span >Item {{index()}}. {{model.items[index()]}}</span>
                            </div>
                        </template>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["model"] = new
                {
                    items = new[]
                    {
                        "First",
                        "Second",
                        "Third"
                    },
                    red = StandardColors.Red,
                    dark = false
                };

                using (var stream = DocStreams.GetOutputStream("CSSInlineCalcRepeatingTest.pdf"))
                {
                    doc.LayoutComplete += ParseCSSWithInlineCalcRepeater_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }

        }

        private void ParseCSSWithInlineCalcRepeater_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Make sure the variables are correctly assigned to the inline begin spans.
            var context = (PDFLayoutContext)(args.Context);
            var layout = context.DocumentLayout;
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;
            var first = content.Columns[0].Contents[0] as PDFLayoutBlock; //Default block
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock; //Explicit block
            var third = content.Columns[0].Contents[2] as PDFLayoutBlock; //As a class block

            Assert.AreEqual(StandardColors.Lime, first.FullStyle.Background.Color, "The default colour did not match lime");
            Assert.AreEqual(StandardColors.Red, second.FullStyle.Background.Color, "The explicit colour did not match red");
            Assert.AreEqual(StandardColors.Lime, third.FullStyle.Background.Color, "The calc colour did not match lime");
        }


        [TestMethod]
        public void ParseCSSWithCalcExpression()
        {
            var cssWithCalc = @"

            .other{
               background-color: calc(concat('#', 'FF', '00', '00'));
               color: var(--text-color, #00FFFF);
            }";


            using (var doc = BuildDocumentWithStyles(cssWithCalc))
            {
                doc.StyleClass = "other";
                var applied = doc.GetAppliedStyle();

                Assert.AreEqual("rgb(255,0,0)", applied.Background.Color.ToString(), "Expression was not applied to the document");
                Assert.AreEqual("rgb(0,255,255)", applied.Fill.Color.ToString(), "The fallback for the variable --text-color was not used");
            }
        }

        [TestMethod]
        public void ParseCSSWithCalcExpressionAndVariable()
        {
            var cssWithCalc = @"

            :root{
               --text-color: #000000;
            }
            .other{
               background-color: calc(concat('#', 'FF', '00', '00'));
               color: var(--text-color, #00FFFF);
            }";


            using (var doc = BuildDocumentWithStyles(cssWithCalc))
            {
                doc.StyleClass = "other";
                doc.Params["--text-color"] = StandardColors.Lime;
                var applied = doc.GetAppliedStyle();

                Assert.AreEqual("rgb(255,0,0)", applied.Background.Color.ToString(), "Expression was not applied to the document");
                Assert.AreEqual(StandardColors.Lime.ToString(), applied.Fill.Color.ToString(), "The parameter value for the variable --text-color was not used");
            }
        }


        [TestMethod]
        public void ParseCssAllVariableProperties()
        {
            var cssAll = @"

            :root{
               --color: #0000FF;
               --color-2: #FF00FF;

               --unit: 12pt;
               --unit-big: 5in;

               --number: 3;

               --img: url('paper.gif');
               --repeat: repeat-x;
               --font: Arial, sans-serif;
               --border-style: dotted;
               --breaks: always;
               --no-breaks: avoid;
               --widths: 0.5 * 20%;
               --orientation: landscape;
               --pgsize: A3;
               --float: right;
               --pos: absolute;
               --dashes: 1 0 2 1;
               --linecap: round;
               --linejoin: mitre;
               --opacity: 0.5;
               --halign: justify;
               --valign: bottom;
               --decoration: line-through underline;
               --white-space: pre;
            }

            .other{
               background-color: var(--color-2);
               color: var(--color, #00FFFF);
               margin: var(--unit);
               height: var(--unit-big);
               padding: var(--unit) 10pt calc(--unit * 2) 5pt;
               background-image: var(--img);
               background-repeat: var(--repeat);
               background-position: 10px var(--unit);
               background-size: var(--unit) 10px;

               border-color: var(--color-2);
               border-radius: var(--unit);
               border-style: var(--border-style);
               border-width: calc(--unit / 10);

               break-before: var(--breaks);
               break-after: var(--no-breaks);
               page-break-before: var(--breaks);
               page-break-after: var(--no-breaks);

               column-count: var(--number);
               column-gap: var(--unit);
               column-width: var(--widths);

               size: var(--pgsize) var(--orientation);
               float: var(--float);
               position: var(--pos);

               stroke: var(--color-2);
               stroke-dasharray: var(--dashes);
               stroke-linecap: var(--linecap);
               stroke-linejoin: var(--linejoin);
               stroke-opacity: var(--opacity);
               stroke-width: calc(var(--unit) / 10);

               text-align: var(--halign);
               vertical-align: var(--valign);

               left: var(--unit-big);
               top: var(--unit);
               width: var(--unit-big);
               height: calc(--unit-big / 2);

               letter-spacing: calc(--unit / 5);
               word-spacing: var(--unit);
               text-decoration: var(--decoration);
               white-space: var(--white-space);
            }";


            var doc = BuildDocumentWithStyles(cssAll);
            doc.StyleClass = "other";
            var applied = doc.GetAppliedStyle();

            Assert.AreEqual("rgb(0,0,255)", applied.Fill.Color.ToString(), "Color was not set");

            Assert.AreEqual("12pt", applied.Margins.All.ToString(), "Margins was not set");
            Assert.AreEqual("12pt", applied.Margins.Left.ToString(), "Margins Left was not set");
            Assert.AreEqual("12pt", applied.Margins.Right.ToString(), "Margins Right was not set");
            Assert.AreEqual("12pt", applied.Margins.Top.ToString(), "Margins Top was not set");
            Assert.AreEqual("12pt", applied.Margins.Bottom.ToString(), "Margins Bottom was not set");

            Assert.AreEqual("12pt", applied.Padding.Top.ToString(), "Padding Top was not set");
            Assert.AreEqual("10pt", applied.Padding.Right.ToString(), "Padding Right was not set");
            Assert.AreEqual("24pt", applied.Padding.Bottom.ToString(), "Padding Top was not set");
            Assert.AreEqual("5pt", applied.Padding.Left.ToString(), "Padding Right was not set");

            Assert.AreEqual("rgb(255,0,255)", applied.Background.Color.ToString(), "Background Color was not set");
            Assert.AreEqual("paper.gif", applied.Background.ImageSource, "Image Source was not set");

            Assert.AreEqual(PatternRepeat.RepeatX, applied.Background.PatternRepeat, "Pattern Repeat was not set");

            Assert.AreEqual("7.5pt", applied.Background.PatternXPosition.ToString(), "Pattern X Position not set");
            Assert.AreEqual("12pt", applied.Background.PatternYPosition.ToString(), "Pattern Y Position not set");

            Assert.AreEqual("12pt", applied.Background.PatternXSize.ToString(), "Pattern X Size not set");
            Assert.AreEqual("7.5pt", applied.Background.PatternYSize.ToString(), "Pattern Y Size not set");

            Assert.AreEqual("rgb(255,0,255)", applied.Border.Color.ToString(), "Border color not set");
            Assert.AreEqual(LineType.Dash, applied.Border.LineStyle, "Border line style not set");
            Assert.AreEqual("[2] 0", applied.Border.Dash.ToString(), "Border dash not set");
            Assert.AreEqual("1.2pt", applied.Border.Width.ToString(), "Border width not set");

            Assert.IsTrue(applied.Columns.BreakBefore, "Column break before not set");
            Assert.IsTrue(applied.IsValueDefined(StyleKeys.ColumnBreakAfterKey), "Column break after not set");
            Assert.IsFalse(applied.Columns.BreakAfter, "Column break after not set correctly");

            Assert.IsTrue(applied.PageStyle.BreakBefore, "Column break before not set");
            Assert.IsTrue(applied.IsValueDefined(StyleKeys.PageBreakAfterKey), "Page break after not set");
            Assert.IsFalse(applied.PageStyle.BreakAfter, "Page break after not set correctly");

            Assert.AreEqual(3, applied.Columns.ColumnCount, "Column count not set correctly");
            Assert.AreEqual("12pt", applied.Columns.AlleyWidth.ToString(), "Alley width not set correctly");

            Assert.IsNotNull(applied.Columns.ColumnWidths, "Column widths is not set");
            Assert.IsFalse(applied.Columns.ColumnWidths.IsEmpty, "Column widths is empty");
            Assert.IsTrue(applied.Columns.ColumnWidths.Explicit.IsZero, "Column widths explicit values is not empty");
            Assert.AreEqual(3, applied.Columns.ColumnWidths.Widths.Length, "Column widths is not the right length");
            Assert.AreEqual("[0.5 0 0.2]", applied.Columns.ColumnWidths.ToString(), "Column widths are not correct");

            Assert.AreEqual(PaperSize.A3, applied.PageStyle.PaperSize, "Paper Size was not set");
            Assert.AreEqual(PaperOrientation.Landscape, applied.PageStyle.PaperOrientation, "Paper Orientation was not set");

            Assert.AreEqual(FloatMode.Right, applied.Position.Float, "Float was not set");
            Assert.AreEqual(PositionMode.Absolute, applied.Position.PositionMode, "Position mode was not set");

            Assert.AreEqual("rgb(255,0,255)", applied.Stroke.Color.ToString(), "Stroke color was not set");
            Assert.AreEqual("[1 0 2 1] 0", applied.Stroke.Dash.ToString(), "Stroke dashes were not set");
            Assert.AreEqual(LineCaps.Round, applied.Stroke.LineCap, "Stroke line caps were not set");
            Assert.AreEqual(LineJoin.Mitre, applied.Stroke.LineJoin, "Stroke line join was not set");
            Assert.AreEqual(0.5, applied.Stroke.Opacity, "Stroke opacity was not set");
            Assert.AreEqual(1.2, applied.Stroke.Width.PointsValue, "Stroke width was not set");

            Assert.AreEqual(HorizontalAlignment.Justified, applied.Position.HAlign, "Horizontal alignment was not set");
            Assert.AreEqual(VerticalAlignment.Bottom, applied.Position.VAlign, "Vertical alignment was not set");

            Assert.AreEqual(360, applied.Position.X.PointsValue, "Left (X) was not applied");
            Assert.AreEqual(12, applied.Position.Y.PointsValue, "Top (Y) was not applied");
            Assert.AreEqual(360, applied.Size.Width.PointsValue, "Width was not applied");
            Assert.AreEqual(180, applied.Size.Height.PointsValue, "Height was not applied");

            Assert.AreEqual(TextDecoration.StrikeThrough | TextDecoration.Underline, applied.Text.Decoration, "Text decoration was not applied");
            Assert.AreEqual(2.4, applied.Text.CharacterSpacing.PointsValue, "Letter spacing was not set");
            Assert.AreEqual(12, applied.Text.WordSpacing.PointsValue, "Word spacing was not set");
            Assert.AreEqual(WordWrap.NoWrap, applied.Text.WrapText, "Word wrapping was not set");
            Assert.AreEqual(true, applied.Text.PreserveWhitespace, "White space preservation was not set");
            
        }

        [TestMethod]
        public void ParseCSSWithMultipleSelectors()
        {
            var src = @"<!DOCTYPE HTML >
                <html lang='en' xmlns='http://www.w3.org/1999/xhtml' >
                    <head>
                        <title>Repeating calc in template</title>
                        <style>

                            div,p
                            {
                               padding: 10px;
                               background-color: blue;
                            }

                            p.green div, div.red > div{font-style: italic;}

                            p.green div,.other{
                                color: green;
                            }

                            #nanid  ,  div.red > div{
                                color: red;
                            }

                        </style>
                    </head>
                    <body>
                            <div id='redTextDiv' class='red'>
                                <div >Red Text on blue background</div>
                            </div>
                            <p id='greenTextPara' class='green'>
                                <div >Green text on blue background</div>
                            </p>
                    </body>
                </html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("CSSWithMultipleSelectorsTest.pdf"))
                {
                    doc.LayoutComplete += ParseCSSWithMultipleSelectors_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

            }
        }

        [TestMethod()]
        public void RemoteCssWithRelativeFileLoading()
        {
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/inlineblock/Scryber.UnitTest/Content/HTML/CSS/IncludeRelative.css";
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>Html document title</title>
                                <link href='" + path + @"' rel='stylesheet' />
                            </head>

                            <body class='grey' style='margin:20px;' >
                                <p id='myPara' >This is a paragraph of content</p>
                                <div class='relative-background' style='height:250px; background-size: 60px 50px; font-size:3rem;' >With a background image</div>
                            </body>
                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HtmlRemoteCSSRelative.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                var html = (HTMLDocument)_layoutcontext.Document;

                //The font and the image
                Assert.AreEqual(2, html.SharedResources.Count);

                //could be first or second
                var imgXObject = html.SharedResources[0] as PDFImageXObject;
                if (null == imgXObject)
                    imgXObject = html.SharedResources[1] as PDFImageXObject;

                Assert.IsNotNull(imgXObject);
                Assert.AreEqual("https://raw.githubusercontent.com/richard-scryber/scryber.core/inlineblock/Scryber.UnitTest/Content/HTML/Images/group.png", imgXObject.ResourceKey);

                var w = imgXObject.ImageData.PixelWidth;
                var h = imgXObject.ImageData.PixelHeight;

                Assert.AreEqual(396, w);
                Assert.AreEqual(342, h);

            }
        }

        [TestMethod()]
        public async Task RemoteRelativeCssWithRelativeFileLoading()
        {
            //Full path to source, contains a relative link to a css file, that contains anothor relative link to a background image, that should be resolved.
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/inlineblock/Scryber.UnitTest/Content/HTML/LinkRelative.html";


            
            

            using (var src = await new System.Net.Http.HttpClient().GetStreamAsync(path))
            {
                var doc = Document.ParseDocument(src, path, ParseSourceType.RemoteFile);

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("HtmlRemoteFileRelativeCSSAndRelativeImage.pdf"))
                {
                    doc.LayoutComplete += SimpleDocumentParsing_Layout;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }


                var body = _layoutcontext.DocumentLayout.AllPages[0].ContentBlock;

                Assert.AreEqual("Html document title", doc.Info.Title, "Title is not correct");

                //This has been loaded from the remote file
                Assert.AreEqual((Color)"#808080", body.FullStyle.Background.Color, "Fill colors do not match");
                var html = (HTMLDocument)_layoutcontext.Document;

                //The font and the image
                Assert.AreEqual(2, html.SharedResources.Count);

                //could be first or second
                var imgXObject = html.SharedResources[0] as PDFImageXObject;
                if (null == imgXObject)
                    imgXObject = html.SharedResources[1] as PDFImageXObject;

                Assert.IsNotNull(imgXObject);
                Assert.AreEqual("https://raw.githubusercontent.com/richard-scryber/scryber.core/inlineblock/Scryber.UnitTest/Content/HTML/Images/group.png", imgXObject.ResourceKey);

                var w = imgXObject.ImageData.PixelWidth;
                var h = imgXObject.ImageData.PixelHeight;

                Assert.AreEqual(396, w);
                Assert.AreEqual(342, h);

            }
        }


        private void ParseCSSWithMultipleSelectors_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Make sure the variables are correctly assigned to the inline begin spans.
            var context = (PDFLayoutContext)(args.Context);
            var layout = context.DocumentLayout;
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;
            var div = content.Columns[0].Contents[0] as PDFLayoutBlock; //Default block

            Assert.AreEqual(StandardColors.Blue, div.FullStyle.Background.Color, "The background colour for '" + div.Owner.ID + "' did not match blue");

            var inner = div.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(StandardColors.Red, inner.FullStyle.Fill.Color, "The fill colour did not match red");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Italic, inner.FullStyle.Font.FontFaceStyle, "The font style was no italic for '" + div.Owner.ID);
            div = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(StandardColors.Blue, div.FullStyle.Background.Color, "The background colour for '" + div.Owner.ID + "' did not match blue");

            inner = div.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(StandardColors.Green, inner.FullStyle.Fill.Color, "The fill colour did not match Green");
            Assert.AreEqual(Scryber.Drawing.FontStyle.Italic, inner.FullStyle.Font.FontFaceStyle, "The font style was no italic for '" + div.Owner.ID);


        }


        /// <summary>
        /// Returns a style that would be applied to the document, based on the passed css and any class
        /// </summary>
        /// <param name="css">The css styles to use</param>
        /// <param name="docClass">The css class to set on the document if any</param>
        /// <returns>The applied style</returns>
        private Document BuildDocumentWithStyles(string css)
        {
            var doc = new Document();
            var context = new LoadContext(doc.Params, doc.TraceLog, doc.PerformanceMonitor, doc, OutputFormat.PDF);
            var cssparser = new CSSStyleParser(css, context);

            //Add the parsed styles
            foreach (var style in cssparser)
            {
                doc.Styles.Add(style);
            }

            //do the load and bind
            doc.InitializeAndLoad(OutputFormat.PDF);
            doc.DataBind(OutputFormat.PDF);

            return doc;
        }



       

    }
}
