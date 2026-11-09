using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    [TestClass]
    public class CSSTextIndent_Tests
    {
        /// <summary>
        /// Renders and returns the x position of the first text line (the first Td in the content stream)
        /// </summary>
        private static double FirstLineX(string body, string css = "")
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>" + css + "</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var text = PdfContent.Decompressed(ms.ToArray());
                var m = Regex.Match(text, @"([\-0-9\.]+)\s+([\-0-9\.]+)\s+Td");
                Assert.IsTrue(m.Success, "No text found in: " + text);
                return double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        [TestMethod]
        public void TextIndent_Points_IndentsFirstLine()
        {
            Assert.AreEqual(0.0, FirstLineX("<p>No indent for this text</p>"), 0.01);
            Assert.AreEqual(40.0, FirstLineX("<p style='text-indent: 40pt'>Indented text</p>"), 0.01);
        }

        [TestMethod]
        public void TextIndent_OnlyFirstLine()
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body><div style='width:100pt'><p style='text-indent: 30pt'>one two three four five six seven eight nine ten eleven twelve</p></div></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var text = PdfContent.Decompressed(ms.ToArray());
                var xs = new System.Collections.Generic.List<double>();
                foreach (Match m in Regex.Matches(text, @"([\-0-9\.]+)\s+([\-0-9\.]+)\s+Td"))
                    xs.Add(double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture));
                Assert.IsTrue(xs.Count > 1, "The text should wrap onto more than one line");
                Assert.AreEqual(30.0, xs[0], 0.01, "The first line is indented");
                //later lines are relative offsets (Td moves from the previous line start), so they take the indent back
                Assert.AreEqual(-30.0, xs[1], 0.01, "The second line returns to the left edge");
            }
        }

        [TestMethod]
        public void TextIndent_Inherited_FromParent()
        {
            Assert.AreEqual(25.0, FirstLineX("<div class='i'><p>Inherited indent</p></div>", ".i { text-indent: 25pt; }"), 0.01);
        }

        [TestMethod]
        public void TextIndent_Percentage_OfContainerWidth()
        {
            Assert.AreEqual(50.0, FirstLineX("<div style='width:200pt'><p style='text-indent: 25%'>Percent indent</p></div>"), 0.01);
        }

        [TestMethod]
        public void TextIndent_Expression()
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p style='text-indent: {{model.indent}}'>Bound indent</p></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                doc.Params["model"] = new { indent = "18pt" };
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var m = Regex.Match(PdfContent.Decompressed(ms.ToArray()), @"([\-0-9\.]+)\s+([\-0-9\.]+)\s+Td");
                Assert.AreEqual(18.0, double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture), 0.01);
            }
        }

        private static System.Collections.Generic.List<double> LineXs(string body)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:300pt 300pt;margin:60pt} body{font-size:12pt}</style></head><body>" + body + "</body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var xs = new System.Collections.Generic.List<double>();
                foreach (Match m in Regex.Matches(PdfContent.Decompressed(ms.ToArray()), @"([\-0-9\.]+)\s+([\-0-9\.]+)\s+Td"))
                    xs.Add(double.Parse(m.Groups[1].Value, System.Globalization.CultureInfo.InvariantCulture));
                return xs;
            }
        }

        [TestMethod]
        public void TextIndent_Negative_OnlyFirstLine_NextLineOffsetBack()
        {
            var xs = LineXs("<p style='text-indent: -30pt'>First line of a long paragraph that wraps over several lines of the box here.</p>");
            Assert.IsTrue(xs.Count > 2, "The text should wrap");
            Assert.AreEqual(30.0, xs[0], 0.01, "The first line hangs 30pt out of the 60pt page margin");
            Assert.AreEqual(30.0, xs[1], 0.01, "The next line (only) is offset back by the inset");
            Assert.AreEqual(0.0, xs[2], 0.01, "Later lines follow with no further offset");
        }

        [TestMethod]
        public void TextIndent_Negative_LineAfterHardReturn_AtTheEdge()
        {
            var xs = LineXs("<p style='text-indent: -30pt'>Hard one<br/>Second after return<br/>Third</p>");
            Assert.AreEqual(3, xs.Count);
            Assert.AreEqual(30.0, xs[0], 0.01);
            Assert.AreEqual(60.0, xs[1], 0.01);
            Assert.AreEqual(60.0, xs[2], 0.01);
        }

        [TestMethod]
        public void TextIndent_Positive_WrappedLinesReturnToTheEdge()
        {
            var xs = LineXs("<p style='text-indent: 30pt'>First line of a long paragraph that wraps over several lines of the box here.</p>");
            Assert.AreEqual(90.0, xs[0], 0.01);
            Assert.AreEqual(-30.0, xs[1], 0.01);
            Assert.AreEqual(0.0, xs[2], 0.01);
        }

        [TestMethod]
        public void TextIndent_None_Zero()
        {
            Assert.AreEqual(0.0, FirstLineX("<p style='text-indent: 0'>Zero</p>"), 0.01);
        }
    }

    /// <summary>
    /// Helper to read the text content streams out of a generated pdf
    /// </summary>
    internal static class PdfContent
    {
        public static string Decompressed(byte[] pdf)
        {
            var sb = new System.Text.StringBuilder();
            var all = System.Text.Encoding.Latin1.GetString(pdf);
            int i = 0;
            while ((i = all.IndexOf("stream", i, StringComparison.Ordinal)) >= 0)
            {
                i += 6;
                if (i < all.Length && all[i] == '\r') i++;
                if (i < all.Length && all[i] == '\n') i++;
                var end = all.IndexOf("endstream", i, StringComparison.Ordinal);
                if (end < 0) break;
                try
                {
                    using (var inflate = new System.IO.Compression.ZLibStream(new MemoryStream(pdf, i, end - i), System.IO.Compression.CompressionMode.Decompress))
                    using (var sr = new StreamReader(inflate, System.Text.Encoding.Latin1))
                        sb.Append(sr.ReadToEnd()).Append('\n');
                }
                catch { }
                i = end + 9;
            }
            return sb.ToString();
        }
    }
}
