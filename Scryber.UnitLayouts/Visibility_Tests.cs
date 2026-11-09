using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Hidden content takes up its space in the layout, but is not drawn.
    /// </summary>
    [TestClass]
    public class Visibility_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private string Render(string css, string body, object model = null)
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body style='margin:0; padding:0'>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                doc.RenderOptions.Compression = Scryber.OutputCompressionType.None;
                using (var ms = new System.IO.MemoryStream())
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                    return System.Text.Encoding.Latin1.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Gets the text that is actually painted - the hex strings shown with Tj, when the text render mode is not 3 (invisible)
        /// </summary>
        private static string PaintedText(string pdf)
        {
            var sb = new System.Text.StringBuilder();
            int mode = 0;
            foreach (Match m in Regex.Matches(pdf, @"(?<tr>\d)\s+Tr|<(?<hex>[0-9A-Fa-f]+)>\s*Tj"))
            {
                if (m.Groups["tr"].Success)
                    mode = int.Parse(m.Groups["tr"].Value);
                else if (mode != 3)
                {
                    var hex = m.Groups["hex"].Value;
                    for (int i = 0; i + 1 < hex.Length; i += 2)
                        sb.Append((char)Convert.ToByte(hex.Substring(i, 2), 16));
                }
            }
            return sb.ToString();
        }

        private PDFLayoutBlock Find(string id, PDFLayoutBlock parent = null)
        {
            parent = parent ?? _layout.AllPages[0].ContentBlock;
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

        [TestMethod]
        public void Visible_IsTheDefault()
        {
            var pdf = Render("", "<p>Shown text</p>");
            Assert.AreEqual("Shown text", PaintedText(pdf));
        }

        [TestMethod]
        public void Hidden_Block_IsNotPainted_ButKeepsItsSpace()
        {
            var visible = Render("", "<div id='a' style='height:50pt;background-color:red'>Before</div><div id='b' style='height:30pt'>After</div>");
            var visibleTop = Find("b").TotalBounds.Y.PointsValue;

            var hidden = Render("", "<div id='a' style='height:50pt;background-color:red;visibility:hidden'>Before</div><div id='b' style='height:30pt'>After</div>");
            Assert.AreEqual(visibleTop, Find("b").TotalBounds.Y.PointsValue, 0.01, "The hidden block still takes its space");
            Assert.AreEqual("After", PaintedText(hidden), "Only the visible text is painted");
            Assert.IsFalse(hidden.Contains("1.0000 0.0000 0.0000 rg"), "The red background is not painted");
            Assert.IsTrue(visible.Contains("1.0000 0.0000 0.0000 rg"), "The red background is painted when visible");
        }

        [TestMethod]
        public void Hidden_Block_ChildrenAreAlsoHidden()
        {
            var pdf = Render("", "<div style='visibility:hidden'><p>Inner one</p><div><span>Inner two</span></div></div><p>Outer</p>");
            Assert.AreEqual("Outer", PaintedText(pdf));
        }

        [TestMethod]
        public void Hidden_InlineText_KeepsTheSpace_ButIsNotPainted()
        {
            var pdf = Render("", "<p>Start <span style='visibility:hidden'>hidden words</span> end</p>");
            var painted = PaintedText(pdf);
            Assert.IsTrue(painted.Contains("Start"));
            Assert.IsTrue(painted.Contains("end"));
            Assert.IsFalse(painted.Contains("hidden"), "The hidden words are not painted");
        }

        [TestMethod]
        public void Hidden_InlineText_FollowingTextIsPositionedAfterTheHiddenText()
        {
            var visible = Render("", "<p>Start <span>hidden words</span> end</p>");
            var hidden = Render("", "<p>Start <span style='visibility:hidden'>hidden words</span> end</p>");
            //the same text operations are written, only the render mode differs, so the following text is in the same place
            Assert.AreEqual(Regex.Matches(visible, "Tj").Count, Regex.Matches(hidden, "Tj").Count);
        }

        [TestMethod]
        public void Hidden_Image_IsNotDrawn()
        {
            var png = DocStreams.AssertGetTemplatePath("Content/Images/Toroid32x9.png").Replace("\\", "/");
            var shown = Render("", "<img id='i' src='" + png + "' style='width:100pt;height:50pt' /><p>Text</p>");
            var hidden = Render("", "<img id='i' src='" + png + "' style='width:100pt;height:50pt;visibility:hidden' /><p>Text</p>");
            Assert.IsTrue(shown.Contains(" Do"), "The image is drawn");
            Assert.IsFalse(hidden.Contains("imgx1 Do"), "The hidden image is not drawn");
        }

        [TestMethod]
        public void Hidden_ThenVisibleAgain_OnSiblings()
        {
            var pdf = Render(".h { visibility: hidden; } .v { visibility: visible; }", "<p class='h'>One</p><p class='v'>Two</p><p>Three</p>");
            Assert.AreEqual("TwoThree", PaintedText(pdf));
        }

        [TestMethod]
        public void Hidden_FromExpression()
        {
            var pdf = Render("", "<p style='visibility: {{model.v}}'>Maybe</p><p>Always</p>", new { v = "hidden" });
            Assert.AreEqual("Always", PaintedText(pdf));
            pdf = Render("", "<p style='visibility: {{model.v}}'>Maybe</p><p>Always</p>", new { v = "visible" });
            Assert.AreEqual("MaybeAlways", PaintedText(pdf));
        }

        [TestMethod]
        public void Collapse_IsTreatedAsHidden()
        {
            var pdf = Render("", "<p style='visibility:collapse'>Gone</p><p>Here</p>");
            Assert.AreEqual("Here", PaintedText(pdf));
        }

        [TestMethod]
        public void Hidden_TableCell_KeepsTheTableLayout()
        {
            var visible = Render("td { width: 100pt; }", "<table><tr><td id='a'>One</td><td id='b'>Two</td></tr></table>");
            var visibleWidth = Find("b").TotalBounds.X.PointsValue;
            var hidden = Render("td { width: 100pt; }", "<table><tr><td id='a' style='visibility:hidden'>One</td><td id='b'>Two</td></tr></table>");
            Assert.AreEqual(visibleWidth, Find("b").TotalBounds.X.PointsValue, 0.01);
            Assert.AreEqual("Two", PaintedText(hidden));
        }

        private const string Rows = "<table id='t'><tr id='r1'><td>One</td></tr><tr id='r2' {0}><td>Two</td></tr><tr id='r3'><td id='c3'>Three</td></tr></table>";

        [TestMethod]
        public void Collapse_TableRow_IsRemovedWithItsSpace()
        {
            Render("", string.Format(Rows, ""));
            var fullHeight = Find("t").TotalBounds.Height.PointsValue;
            var thirdTop = Find("r3").TotalBounds.Y.PointsValue;

            var pdf = Render("", string.Format(Rows, "style='visibility:collapse'"));
            Assert.AreEqual("OneThree", PaintedText(pdf), "The collapsed row is not drawn");
            Assert.IsTrue(Find("t").TotalBounds.Height.PointsValue < fullHeight - 5.0, "The table is shorter");
            Assert.IsTrue(Find("r3").TotalBounds.Y.PointsValue < thirdTop - 5.0, "The next row moves up");
            Assert.IsNull(Find("r2"), "The row is not laid out");
        }

        [TestMethod]
        public void Hidden_TableRow_KeepsItsSpace()
        {
            Render("", string.Format(Rows, ""));
            var fullHeight = Find("t").TotalBounds.Height.PointsValue;

            var pdf = Render("", string.Format(Rows, "style='visibility:hidden'"));
            Assert.AreEqual("OneThree", PaintedText(pdf));
            Assert.AreEqual(fullHeight, Find("t").TotalBounds.Height.PointsValue, 0.01);
        }

        [TestMethod]
        public void Collapse_RowGroup_RemovesItsRows()
        {
            var body = "<table id='t'><thead><tr><td>Head</td></tr></thead><tbody style='visibility:collapse'><tr><td>Body one</td></tr><tr><td>Body two</td></tr></tbody><tfoot><tr><td>Foot</td></tr></tfoot></table>";
            var pdf = Render("", body);
            Assert.AreEqual("HeadFoot", PaintedText(pdf));
        }

        [TestMethod]
        public void Collapse_AllRows_DoesNotFail()
        {
            var pdf = Render("", "<table id='t'><tr style='visibility:collapse'><td>One</td></tr></table><p>After</p>");
            Assert.AreEqual("After", PaintedText(pdf));
        }

        [TestMethod]
        public void Collapse_OutsideATable_IsHidden_KeepingTheSpace()
        {
            var pdf = Render("", "<div id='a' style='height:40pt;visibility:collapse'>Gone</div><div id='b' style='height:20pt'>Here</div>");
            Assert.AreEqual("Here", PaintedText(pdf));
            Assert.AreEqual(40.0, Find("b").TotalBounds.Y.PointsValue, 0.01, "The space is kept");
        }
    }
}
