using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Layouts that match a browser: images in a flex continuation header, long words that overflow, and inline-flex.
    /// </summary>
    [TestClass]
    public class BrowserParity_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private void Layout(string body, string css = "", string page = "size:400pt 300pt;margin:0")
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{" + page + "} body{margin:0;font-size:10pt} " + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
        }

        private static PDFLayoutBlock Find(string id, PDFLayoutBlock parent)
        {
            if (null == parent)
                return null;

            foreach (var column in parent.Columns)
                foreach (var item in column.Contents)
                    if (item is PDFLayoutBlock block)
                    {
                        if (block.Owner != null && block.Owner.ID == id)
                            return block;
                        var inner = Find(id, block);
                        if (null != inner)
                            return inner;
                    }
            return null;
        }

        private static IEnumerable<PDFLayoutRun> AllRuns(PDFLayoutBlock parent)
        {
            foreach (var column in parent.Columns)
                foreach (var item in column.Contents)
                {
                    if (item is PDFLayoutBlock block)
                    {
                        foreach (var run in AllRuns(block))
                            yield return run;
                    }
                    else if (item is PDFLayoutLine line)
                    {
                        foreach (var run in line.Runs)
                            yield return run;
                    }
                }
        }

        private static string ImagePath()
        {
            var path = System.IO.Path.GetFullPath(System.IO.Path.Combine(System.Environment.CurrentDirectory, "../../../Content/Images/Toroid32.png"));
            Assert.IsTrue(System.IO.File.Exists(path), "Test image not found at " + path);
            return path;
        }

        // images in a flex continuation header

        [TestMethod]
        public void BrowserParity_01_ImageInAFlexContinuationHeader_IsOnTheFirstPageItIsUsed()
        {
            var css = "continuation-header{display:flex;align-items:center;height:40pt} .num{width:100pt} .site{flex:1} .logo{height:20pt;margin-right:30pt;display:block}";
            var body = "<header></header><continuation-header><div class='num'><page></page></div><div class='site'>Site</div><img class='logo' src='" + ImagePath() + "' /></continuation-header>" +
                       "<main><div>Cover</div><div style='page-break-before:always'>Two</div><div style='page-break-before:always'>Three</div></main>";
            Layout(body, css);

            Assert.AreEqual(3, _layout.AllPages.Count);

            for (int i = 1; i < 3; i++)
            {
                var header = _layout.AllPages[i].HeaderBlock;
                Assert.IsNotNull(header, "Page " + (i + 1) + " has a header");

                var image = AllRuns(header).OfType<PDFLayoutComponentRun>().FirstOrDefault(r => r.Owner is Scryber.Html.Components.HTMLImage);
                Assert.IsNotNull(image, "Page " + (i + 1) + " has the image");
                Assert.IsTrue(image.Width.PointsValue > 10 && !double.IsNaN(image.Width.PointsValue), "Page " + (i + 1) + " image has a width: " + image.Width);

                var column = image.Line.Region;
                Assert.IsTrue(column.TotalBounds.X.PointsValue + column.TotalBounds.Width.PointsValue <= 400.01,
                              "Page " + (i + 1) + " image is on the page: " + column.TotalBounds);
            }
        }

        // long words

        [TestMethod]
        public void BrowserParity_02_LongWord_IsNotBroken_AndOverflows()
        {
            Layout("<div id='num' style='width:60pt;font-size:60pt'>0234</div>");
            var block = Find("num", _layout.AllPages[0].ContentBlock);

            var lines = block.Columns[0].Contents.OfType<PDFLayoutLine>().ToList();
            Assert.AreEqual(1, lines.Count, "The word is on one line");
            Assert.IsTrue(lines[0].Width.PointsValue > 60, "The line overflows the block: " + lines[0].Width);
        }

        [TestMethod]
        public void BrowserParity_03_LongWord_RightAligned_OverflowsToTheLeft()
        {
            Layout("<div style='margin-left:150pt'><div id='num' style='width:60pt;font-size:60pt;text-align:right'>0234</div></div>");
            var block = Find("num", _layout.AllPages[0].ContentBlock);
            var line = block.Columns[0].Contents.OfType<PDFLayoutLine>().Single();

            var chars = line.Runs.OfType<PDFTextRunCharacter>().Single();
            Assert.AreEqual("0234", chars.Characters);
            Assert.IsTrue(line.Width.PointsValue > 60);
        }

        [TestMethod]
        public void BrowserParity_04_LongWord_BetweenOtherWords_IsOnItsOwnLine()
        {
            Layout("<div id='box' style='width:60pt;font-size:12pt'>a verylongwordindeed b</div>");
            var block = Find("box", _layout.AllPages[0].ContentBlock);

            var text = block.Columns[0].Contents.OfType<PDFLayoutLine>()
                .Select(l => string.Concat(l.Runs.OfType<PDFTextRunCharacter>().Select(r => r.Characters)).Trim())
                .ToList();

            CollectionAssert.AreEqual(new[] { "a", "verylongwordindeed", "b" }, text);
        }

        // inline-flex

        private const string MarkerCss = ".row{display:flex;align-items:center} .marker{display:inline-flex;align-items:center;justify-content:center;width:30pt;height:30pt;background-color:#E2C46F;font-size:15pt}";

        [TestMethod]
        public void BrowserParity_05_InlineFlex_InAFlexRow_CentresItsContent()
        {
            Layout("<div class='row'><span>E1.1</span><span id='marker' class='marker'>H</span></div>", MarkerCss);
            var marker = Find("marker", _layout.AllPages[0].ContentBlock);
            Assert.IsNotNull(marker);
            Assert.AreEqual(30.0, marker.Height.PointsValue, 0.01);

            //the content of the flex item, offset by the column it is in
            var column = marker.Columns.First(c => c.Contents.OfType<PDFLayoutBlock>().Any());
            var content = column.Contents.OfType<PDFLayoutBlock>().First();
            var x = column.TotalBounds.X.PointsValue + content.TotalBounds.X.PointsValue;
            var w = content.TotalBounds.Width.PointsValue;
            Assert.IsTrue(x > 5, "The H is moved right from the start: " + x);
            Assert.AreEqual(15.0, x + w / 2, 1.0, "The H is in the middle");
        }

        [TestMethod]
        public void BrowserParity_06_InlineFlex_InText_StaysOnTheLine()
        {
            Layout("<p id='para' style='margin:0'>Text with <span class='marker'>H</span> inline marker.</p>", MarkerCss);
            var para = Find("para", _layout.AllPages[0].ContentBlock);

            var lines = para.Columns[0].Contents.OfType<PDFLayoutLine>().ToList();
            Assert.AreEqual(1, lines.Count, "The marker is on the same line as the text");
            Assert.IsTrue(lines[0].Height.PointsValue >= 30, "The line is as tall as the marker");
        }
    }
}
