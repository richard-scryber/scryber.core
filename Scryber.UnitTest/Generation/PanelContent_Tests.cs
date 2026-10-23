using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Generation
{
    [PDFParsableComponent("SectionedPanel")]
    public class SectionedPanelComponent : Div
    {
        public SectionedPanelComponent() { this.Sections = new ComponentList(this, ObjectTypes.Panel); }

        [PDFElement("Sections")]
        [PDFArray(typeof(Panel))]
        public ComponentList Sections { get; }

        protected override void OnInitialized(InitContext context)
        {
            base.OnInitialized(context);
            foreach (var section in this.Sections.ToArray())
            {
                this.Sections.Remove(section);
                this.Contents.Add(section);
            }
        }
    }

    [TestClass]
    public class PanelContent_Tests
    {
        private static string Text(byte[] pdf)
        {
            //text is hex encoded in the content stream (after inflating)
            var sb = new System.Text.StringBuilder();
            var all = System.Text.Encoding.Latin1.GetString(pdf);
            int i = 0;
            while ((i = all.IndexOf("stream", i, StringComparison.Ordinal)) >= 0)
            {
                i += 6; if (i < all.Length && all[i] == '\r') i++; if (i < all.Length && all[i] == '\n') i++;
                var end = all.IndexOf("endstream", i, StringComparison.Ordinal);
                if (end < 0) break;
                try
                {
                    using var inflate = new System.IO.Compression.ZLibStream(new MemoryStream(pdf, i, end - i), System.IO.Compression.CompressionMode.Decompress);
                    using var sr = new StreamReader(inflate, System.Text.Encoding.Latin1);
                    var content = sr.ReadToEnd();
                    foreach (System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(content, "<([0-9A-F]+)> Tj"))
                        for (int h = 0; h + 1 < m.Groups[1].Value.Length; h += 2)
                            sb.Append((char)Convert.ToByte(m.Groups[1].Value.Substring(h, 2), 16));
                    sb.Append(' ');
                }
                catch { }
                i = end;
            }
            return sb.ToString();
        }

        [TestMethod]
        public void CollectionOfPanelsParses_AndMovedChildrenRender()
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml' xmlns:c='Scryber.Core.UnitTests.Generation, Scryber.UnitTests'><body><c:SectionedPanel id='cp'><c:Sections><div>First section</div><div>Second section</div></c:Sections></c:SectionedPanel></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var ms2 = ms;
                var cp = doc.FindAComponentById("cp") as SectionedPanelComponent;
                Assert.IsNotNull(cp);
                Assert.AreEqual(2, cp.Contents.Count);
                Assert.AreEqual(0, cp.Sections.Count);
                Assert.IsTrue(Text(ms2.ToArray()).Contains("First section"), "First section was not rendered");
                Assert.IsTrue(Text(ms2.ToArray()).Contains("Second section"), "Second section was not rendered");
            }
        }
    }
}
