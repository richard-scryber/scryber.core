using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.OpenType;

namespace Scryber.Core.UnitTests.Drawing
{
    /// <summary>
    /// Embedding a subset of a TrueType font - only the glyphs used - with the SubsetEmbeddedFonts option (GitHub issue #202).
    /// The font data is subset by Scryber.Core.OpenType FontSubsetter, which is tested there; these tests check the rendered document.
    /// </summary>
    [TestClass()]
    public class FontSubsetting_Tests
    {
        private static string FontPath(string name)
        {
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(DocStreams.GetTestProjectDirectory(), name));
        }

        private static string SegoePath => FontPath("Mocks/Fonts/segoeui.ttf");
        private static string SegoeBoldPath => FontPath("Mocks/Fonts/segoeuib.ttf");
        private static string ArchivePath => FontPath("Resources/Archive.otf");

        private static string Html(string extraCss = "", string body = null)
        {
            return @"<html xmlns='http://www.w3.org/1999/xhtml'><head><style>
@font-face { font-family: 'Segoe'; src: url('" + SegoePath + @"'); font-weight: normal; }
@font-face { font-family: 'Segoe'; src: url('" + SegoeBoldPath + @"'); font-weight: bold; }
@page { size: 400pt 300pt; margin: 20pt; }
body { font-family: 'Segoe'; font-size: 12pt; }
" + extraCss + @"
</style></head><body>" + (body ?? @"
<p>A few paragraphs of text in Segoe UI, regular and <b>bold</b>, with accents: café, naïve.</p>
<p>The quick brown fox jumps over the lazy dog. 0123456789</p>") + @"
</body></html>";
        }

        private static byte[] Render(string html, bool? subset, out Document document, out Scryber.Logging.CollectorTraceLog log, string name)
        {
            document = Document.ParseDocument(new StringReader(html), ParseSourceType.DynamicContent);
            log = new Scryber.Logging.CollectorTraceLog(TraceRecordLevel.Warnings, "test", true);
            ((IParsedDocument)document).SetTraceLog(log);

            document.RenderOptions.Compression = OutputCompressionType.None;
            if (subset.HasValue)
                document.RenderOptions.SubsetEmbeddedFonts = subset.Value;

            using (var ms = new MemoryStream())
            {
                document.SaveAsPDF(ms);
                var data = ms.ToArray();
                using (var output = DocStreams.GetOutputStream(name))
                    output.Write(data, 0, data.Length);
                return data;
            }
        }

        /// <summary>
        /// Returns the embedded font programs (FontFile2 streams) in an uncompressed document
        /// </summary>
        private static byte[][] GetFontFiles(byte[] pdf)
        {
            var text = Encoding.Latin1.GetString(pdf);
            var files = Regex.Matches(text, @"<<\s*/Length\s+(?<len>\d+)\s*/Length1\s+(?<len1>\d+)\s*>>\s*stream\r?\n");
            return files.Select(m =>
            {
                var length = int.Parse(m.Groups["len"].Value);
                var start = m.Index + m.Length;
                var data = new byte[length];
                Buffer.BlockCopy(pdf, start, data, 0, length);
                Assert.AreEqual(int.Parse(m.Groups["len1"].Value), length, "Uncompressed, the length is the length of the font program");
                return data;
            }).ToArray();
        }

        private static string[] GetBaseFonts(byte[] pdf)
        {
            var text = Encoding.Latin1.GetString(pdf);
            return Regex.Matches(text, @"/Subtype\s*/Type0\s*/BaseFont\s*/(?<name>[^\s/>]+)").Select(m => m.Groups["name"].Value).ToArray();
        }

        /// <summary>
        /// The text drawing operations of the page content (between BT and ET), which are the same with or without subsetting
        /// </summary>
        private static string GetTextContent(byte[] pdf)
        {
            var text = Encoding.Latin1.GetString(pdf);
            var all = Regex.Matches(text, @"BT\b[\x20-\x7E\r\n]*?\bET").Select(m => m.Value);
            return Regex.Replace(string.Join("\n", all), @"/[A-Za-z]+_frsc\d+", "/font");
        }

        private static bool HasWarning(Scryber.Logging.CollectorTraceLog log, string contains)
        {
            foreach (var entry in log)
            {
                if (entry.Level == TraceLevel.Warning && null != entry.Message && entry.Message.Contains(contains))
                    return true;
            }
            return false;
        }

        [TestMethod()]
        public void FontSubsetting_01_OffByDefault()
        {
            var options = new Scryber.Options.FontOptions();
            Assert.IsFalse(options.SubsetEmbeddedFonts, "The configuration default is false");

            var doc = new Document();
            Assert.IsFalse(doc.RenderOptions.SubsetEmbeddedFonts, "The document default is false");

            var pdf = Render(Html(), null, out _, out _, "FontSubsetting_01_Default.pdf");
            var files = GetFontFiles(pdf);
            Assert.AreEqual(2, files.Length, "Segoe UI regular and bold are embedded");

            var regular = File.ReadAllBytes(SegoePath);
            var bold = File.ReadAllBytes(SegoeBoldPath);
            Assert.IsTrue(files.Any(f => f.Length == regular.Length) && files.Any(f => f.Length == bold.Length), "The whole font files are embedded");
        }

        [TestMethod()]
        public void FontSubsetting_02_SubsetIsMuchSmaller()
        {
            var full = Render(Html(), false, out _, out _, "FontSubsetting_02_Full.pdf");
            var subset = Render(Html(), true, out _, out var log, "FontSubsetting_02_Subset.pdf");

            Assert.IsTrue(subset.Length < full.Length / 10, "The subset document (" + subset.Length + ") is much smaller than the full (" + full.Length + ")");

            var files = GetFontFiles(subset);
            Assert.AreEqual(2, files.Length);
            foreach (var file in files)
                Assert.IsTrue(file.Length < 100000, "The subset font (" + file.Length + ") is much smaller than the full font (~950KB)");

            //the fonts have a tag (6 capital letters and a +)
            var names = GetBaseFonts(subset);
            Assert.AreEqual(2, names.Length);
            foreach (var name in names)
                Assert.IsTrue(Regex.IsMatch(name, "^[A-Z]{6}\\+SegoeUI"), "The subset font name " + name + " has a subset tag");

            Assert.IsFalse(HasWarning(log, "subset"), "No warnings about subsetting");
        }

        [TestMethod()]
        public void FontSubsetting_03_TextIsUnchangedAndTheFontIsValid()
        {
            var full = Render(Html(), false, out _, out _, "FontSubsetting_03_Full.pdf");
            var subset = Render(Html(), true, out _, out _, "FontSubsetting_03_Subset.pdf");

            //the glyph numbers in the text are the same, so the page content is the same
            Assert.AreEqual(GetTextContent(full), GetTextContent(subset), "The text drawn is the same with a subset font");

            //the embedded subset reads as a valid Segoe UI font
            foreach (var file in GetFontFiles(subset))
            {
                using (var reader = new TypefaceReader())
                using (var ms = new MemoryStream(file))
                {
                    var font = reader.GetFonts(ms, "subset").FirstOrDefault();
                    Assert.IsNotNull(font, "The subset font data reads as a font");
                    Assert.IsTrue(font.IsValid);
                    Assert.AreEqual("Segoe UI", font.FamilyName);
                }
            }

            //and the ToUnicode maps are still written, so the text can be extracted
            Assert.AreEqual(2, Regex.Matches(Encoding.Latin1.GetString(subset), "/ToUnicode").Count);
        }

        [TestMethod()]
        public void FontSubsetting_04_GlyphsUsedOnLaterPagesAreIncluded()
        {
            //a page number, the only use of the digit, on the last page
            var body = "<footer><div>Page <page-number /></div></footer><p>One</p>" +
                       string.Concat(Enumerable.Range(0, 8).Select(i => "<p style='page-break-before: always'>More</p>"));
            var subset = Render(Html("", body), true, out var doc, out _, "FontSubsetting_04_PageNumbers.pdf");
            Assert.AreEqual(9, Regex.Matches(Encoding.Latin1.GetString(subset), @"/Type\s*/Page\b").Count, "Nine pages");

            var files = GetFontFiles(subset);
            Assert.AreEqual(1, files.Length, "Only the regular font is used");

            //the subset has outlines for 1 to 9 (they are only used in the page numbers on each page)
            var regular = File.ReadAllBytes(SegoePath);
            using (var reader = new TypefaceReader())
            using (var ms = new MemoryStream(regular))
            {
                var font = reader.GetFonts(ms, "full").First() as IOpenTypeFont;
                Assert.IsTrue(font.TryGetTable<Scryber.OpenType.SubTables.CMAPTable>("cmap", out var cmap));
                var map = cmap.GetOffsetTable(Scryber.OpenType.SubTables.CMapEncoding.Windows(Scryber.OpenType.SubTables.WindowsCharacterEncodings.Unicode));
                foreach (var c in "123456789")
                {
                    var gid = map.GetCharacterGlyphOffset(c);
                    Assert.IsTrue(GlyphLength(files[0], gid) > 0, "The digit " + c + " (glyph " + gid + ") has an outline in the subset");
                }
                Assert.AreEqual(0, GlyphLength(files[0], map.GetCharacterGlyphOffset('Z')), "Z is not used, so has no outline");
            }
        }

        /// <summary>
        /// The length of the outline of a glyph in TrueType data (from the loca table)
        /// </summary>
        private static int GlyphLength(byte[] font, int gid)
        {
            int count = (font[4] << 8) | font[5];
            int head = -1, loca = -1;
            for (var i = 0; i < count; i++)
            {
                var pos = 12 + i * 16;
                var tag = Encoding.ASCII.GetString(font, pos, 4);
                var offset = (font[pos + 8] << 24) | (font[pos + 9] << 16) | (font[pos + 10] << 8) | font[pos + 11];
                if (tag == "head") head = offset;
                else if (tag == "loca") loca = offset;
            }
            bool longFormat = ((font[head + 50] << 8) | font[head + 51]) == 1;
            int At(int i) => longFormat
                ? (font[loca + i * 4] << 24) | (font[loca + i * 4 + 1] << 16) | (font[loca + i * 4 + 2] << 8) | font[loca + i * 4 + 3]
                : ((font[loca + i * 2] << 8) | font[loca + i * 2 + 1]) * 2;
            return At(gid + 1) - At(gid);
        }

        [TestMethod()]
        public void FontSubsetting_05_ACFFFontIsEmbeddedInFullWithAWarning()
        {
            var css = "@font-face { font-family: 'Archive'; src: url('" + ArchivePath + "'); } .a { font-family: 'Archive'; }";
            var body = "<p class='a'>Archive text</p>";
            var pdf = Render(Html(css, body), true, out _, out var log, "FontSubsetting_05_CFF.pdf");

            Assert.IsTrue(HasWarning(log, "could not be subset"), "A warning that the font could not be subset");
            Assert.IsTrue(HasWarning(log, "CFF"), "With the reason");

            //the font is embedded in full
            var archive = File.ReadAllBytes(ArchivePath);
            Assert.IsTrue(GetFontFiles(pdf).Any(f => f.Length == archive.Length), "The whole Archive font is embedded");
        }
    }
}
