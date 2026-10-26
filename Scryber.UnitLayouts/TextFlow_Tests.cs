using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests word-break, overflow-wrap, word-wrap and text-overflow.
    /// </summary>
    [TestClass]
    public class TextFlow_Tests
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        /// <summary>
        /// Lays out a div (id 'box') with the style and html content, 80pt wide, in a 12pt font.
        /// </summary>
        private PDFLayoutBlock Layout(string style, string content, object model = null, string css = "")
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body style='margin:0;padding:0'><div id='box' style='width:80pt;font-size:12pt;" + style + "'>" + content + "</div></body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            {
                if (model != null)
                    doc.Params["model"] = model;
                using (var ms = new System.IO.MemoryStream())
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(ms);
                }
            }
            var box = Find("box", _layout.AllPages[0].ContentBlock);
            Assert.IsNotNull(box);
            return box;
        }

        private static PDFLayoutBlock Find(string id, PDFLayoutBlock parent)
        {
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

        /// <summary>
        /// The text of each line of the block, and the width of each.
        /// </summary>
        private static List<string> Lines(PDFLayoutBlock box, List<double> widths = null)
        {
            var lines = new List<string>();
            foreach (var column in box.Columns)
                foreach (var item in column.Contents)
                    if (item is PDFLayoutLine line)
                    {
                        var text = new System.Text.StringBuilder();
                        double width = 0;
                        foreach (var run in line.Runs)
                            if (run is PDFTextRunCharacter chars)
                            {
                                text.Append(chars.Characters);
                                width += chars.Width.PointsValue;
                            }
                        lines.Add(text.ToString());
                        widths?.Add(width);
                    }
            return lines;
        }

        private const string Long = "short supercalifragilisticexpialidocious end of line words";

        // word-break

        [TestMethod]
        public void WordBreak_Default_BreaksOnlyTheWordsTooLongForALine()
        {
            var lines = Lines(Layout("", Long));
            Assert.AreEqual("short", lines[0].Trim());
            Assert.IsTrue(lines[1].StartsWith("supercalifragili"), "The long word starts its own line: " + lines[1]);
        }

        [TestMethod]
        public void WordBreak_BreakAll_FillsEachLineBreakingAnywhere()
        {
            var lines = Lines(Layout("word-break: break-all", Long));
            //the first line has the start of the long word, rather than waiting for a new line
            Assert.IsTrue(lines[0].StartsWith("short s") && lines[0].Trim().Length > "short s".Length - 1, "The first line is filled: " + lines[0]);
            Assert.IsTrue(lines[0].Trim().Length > 6);
            Assert.IsFalse(lines.Any(l => l.Contains("-")), "No hyphens are added");
        }

        [TestMethod]
        public void WordBreak_BreakAll_NoHyphens_EvenWithHyphensAuto()
        {
            var lines = Lines(Layout("word-break: break-all; hyphens: auto", Long));
            Assert.IsFalse(lines.Any(l => l.Contains("-")), "No hyphens: " + string.Join("|", lines));
        }

        [TestMethod]
        public void WordBreak_NormalKeepAllBreakWord_AreTheDefault()
        {
            var expected = string.Join("|", Lines(Layout("", Long)));
            foreach (var value in new[] { "normal", "keep-all", "break-word" })
                Assert.AreEqual(expected, string.Join("|", Lines(Layout("word-break: " + value, Long))), value);
        }

        [TestMethod]
        public void WordBreak_IsInherited()
        {
            var lines = Lines(Layout("word-break: break-all", "<span>" + Long + "</span>"));
            Assert.IsTrue(lines[0].Trim().Length > 6, "The span breaks too: " + lines[0]);
        }

        [TestMethod]
        public void WordBreak_BreakAll_WithNoWrap_DoesNotWrap()
        {
            var lines = Lines(Layout("word-break: break-all; white-space: nowrap", Long));
            Assert.AreEqual(1, lines.Count);
        }

        [TestMethod]
        public void WordBreak_Invalid_IsIgnored()
        {
            var expected = string.Join("|", Lines(Layout("", Long)));
            Assert.AreEqual(expected, string.Join("|", Lines(Layout("word-break: sideways", Long))));
        }

        [TestMethod]
        public void WordBreak_FromExpression()
        {
            var lines = Lines(Layout("word-break: {{model.wb}}", Long, new { wb = "break-all" }));
            Assert.IsTrue(lines[0].Trim().Length > 6);
        }

        // overflow-wrap, word-wrap

        [TestMethod]
        public void OverflowWrap_AndWordWrap_AreAccepted_AndLongWordsStillBreak()
        {
            var expected = string.Join("|", Lines(Layout("", Long)));
            foreach (var style in new[] { "overflow-wrap: break-word", "overflow-wrap: anywhere", "overflow-wrap: normal", "word-wrap: break-word", "word-wrap: normal" })
                Assert.AreEqual(expected, string.Join("|", Lines(Layout(style, Long))), style);
        }

        // text-overflow

        private const string Clipped = "overflow: hidden; white-space: nowrap; ";
        private const string LongText = "This text is much too long for the box";

        [TestMethod]
        public void Ellipsis_ReplacesTheEnd_WhenTheTextIsClipped()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped + "text-overflow: ellipsis", LongText), widths);
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(lines[0].EndsWith("…"), lines[0]);
            Assert.IsTrue(lines[0].Length < LongText.Length, "The text is shortened");
            Assert.IsTrue(LongText.StartsWith(lines[0].Substring(0, lines[0].Length - 1)), "It is the start of the text");
            Assert.IsTrue(widths[0] <= 80.0 + 0.01, "It fits in the box: " + widths[0]);
            Assert.IsTrue(widths[0] > 60.0, "It fills the box: " + widths[0]);
        }

        [TestMethod]
        public void Ellipsis_ShortText_IsNotChanged()
        {
            var lines = Lines(Layout(Clipped + "text-overflow: ellipsis", "Short"));
            Assert.AreEqual("Short", lines[0]);
        }

        [TestMethod]
        public void Hidden_NoWrap_CutsBackToTheLastWordThatFits()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped, LongText), widths);
            Assert.AreEqual(1, lines.Count);
            Assert.IsFalse(lines[0].Contains("\u2026"));
            Assert.IsTrue(lines[0].Length < LongText.Length, "The text is cut: " + lines[0]);
            Assert.AreEqual(lines[0].Trim(), lines[0], "No trailing space");
            Assert.IsTrue(LongText.StartsWith(lines[0]), "It is the start of the text");
            Assert.AreEqual(' ', LongText[lines[0].Length], "It is cut at the end of a word: " + lines[0]);
            Assert.IsTrue(widths[0] <= 80.01, "It fits in the box: " + widths[0]);
        }

        [TestMethod]
        public void TextOverflowClip_WithHidden_IsTheSameAsHidden()
        {
            Assert.AreEqual(string.Join("|", Lines(Layout(Clipped, LongText))), string.Join("|", Lines(Layout(Clipped + "text-overflow: clip", LongText))));
        }

        [TestMethod]
        public void Hidden_NoWrap_ShortText_IsNotChanged()
        {
            Assert.AreEqual("Short", Lines(Layout(Clipped, "Short"))[0]);
        }

        [TestMethod]
        public void Hidden_NoWrap_FirstWordTooLong_IsCutAtTheCharacter()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped, "supercalifragilisticexpialidocious and more"), widths);
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(lines[0].Length > 5 && lines[0].Length < "supercalifragilisticexpialidocious".Length, lines[0]);
            Assert.IsTrue("supercalifragilisticexpialidocious".StartsWith(lines[0]));
            Assert.IsFalse(lines[0].Contains("-"), "No hyphen");
            Assert.IsTrue(widths[0] <= 80.01 && widths[0] > 60, "Width: " + widths[0]);
        }

        [TestMethod]
        public void Hidden_NoWrap_InSpans_StopsAtTheEdge()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped, "Start <b>and a long bold part</b> then more text after"), widths);
            var text = string.Join("", lines);
            Assert.IsTrue(text.StartsWith("Start "), text);
            Assert.IsFalse(text.Contains("after"), text);
            Assert.IsTrue(widths.Sum() <= 80.01, "Total width: " + widths.Sum());
        }

        [TestMethod]
        public void Clip_NoWrap_IsNotCutInTheLayout()
        {
            Assert.AreEqual(LongText, Lines(Layout("overflow: clip; white-space: nowrap", LongText))[0], "overflow: clip is a clipping rectangle");
        }

        [TestMethod]
        public void Ellipsis_NeedsOverflowToBeClipped()
        {
            var lines = Lines(Layout("white-space: nowrap; text-overflow: ellipsis", LongText));
            Assert.AreEqual(LongText, lines[0], "The overflow is visible, so there is nothing clipped to replace");
            lines = Lines(Layout("overflow: visible; white-space: nowrap", LongText));
            Assert.AreEqual(LongText, lines[0]);
        }

        [TestMethod]
        public void Ellipsis_NeedsNoWrap_OtherwiseTheTextWraps()
        {
            var lines = Lines(Layout("overflow: hidden; text-overflow: ellipsis", LongText));
            Assert.IsTrue(lines.Count > 1, "The text wraps");
            Assert.IsFalse(lines.Any(l => l.Contains("…")));
        }

        [TestMethod]
        public void CustomString_IsUsed()
        {
            var lines = Lines(Layout(Clipped + "text-overflow: \"[more]\"", LongText));
            Assert.IsTrue(lines[0].EndsWith("[more]"), lines[0]);
            lines = Lines(Layout(Clipped + "text-overflow: &apos;~&apos;", LongText));
            Assert.IsTrue(lines[0].EndsWith("~"), lines[0]);
        }

        [TestMethod]
        public void Ellipsis_InsideASpan_IsInherited()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped + "text-overflow: ellipsis", "<span>" + LongText + "</span>"), widths);
            Assert.IsTrue(lines[0].EndsWith("…"), lines[0]);
            Assert.IsTrue(widths[0] <= 80.01);
        }

        [TestMethod]
        public void Ellipsis_AcrossSeveralRuns_IsOnlyAddedOnce()
        {
            var widths = new List<double>();
            var lines = Lines(Layout(Clipped + "text-overflow: ellipsis", "Start <b>and a long bold part</b> then more text after"), widths);
            var text = string.Join("", lines);
            Assert.AreEqual(1, Regex(text, "…"), text);
            Assert.IsTrue(widths[0] <= 80.01, "Total width: " + widths[0]);
        }

        [TestMethod]
        public void Ellipsis_FromExpression()
        {
            var lines = Lines(Layout(Clipped + "text-overflow: {{model.o}}", LongText, new { o = "ellipsis" }));
            Assert.IsTrue(lines[0].EndsWith("…"), lines[0]);
        }

        [TestMethod]
        public void Ellipsis_Invalid_IsIgnored()
        {
            var plain = string.Join("|", Lines(Layout(Clipped, LongText)));
            Assert.AreEqual(plain, string.Join("|", Lines(Layout(Clipped + "text-overflow: dots", LongText))));
            Assert.AreEqual(plain, string.Join("|", Lines(Layout(Clipped + "text-overflow: ellipsis ellipsis", LongText))), "Two values are not supported");
        }

        [TestMethod]
        public void Ellipsis_VeryNarrowBox_DoesNotFail()
        {
            var html = "<html xmlns='http://www.w3.org/1999/xhtml'><body style='margin:0'><div id='box' style='width:4pt;overflow:hidden;white-space:nowrap;text-overflow:ellipsis'>Narrow</div></body></html>";
            using (var doc = Document.ParseDocument(new System.IO.StringReader(html), ParseSourceType.DynamicContent))
            using (var ms = new System.IO.MemoryStream())
                doc.SaveAsPDF(ms);
        }

        private static int Regex(string text, string pattern)
        {
            return System.Text.RegularExpressions.Regex.Matches(text, pattern).Count;
        }
    }
}
