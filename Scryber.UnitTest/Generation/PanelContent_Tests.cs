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

    [PDFParsableComponent("DashboardCard")]
    public class DashboardCardComponent : Panel
    {
        [PDFAttribute("title")]
        public string Title { get; set; }

        [PDFElement("Content")]
        public Panel ContentPanel { get; set; }

        public DashboardCardComponent() : base(ObjectTypes.Panel) { }
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
                i = end + 9;
            }
            return sb.ToString();
        }

        [TestMethod]
        public void PanelDerivedComponent_ParsesChildContent_AndPanelElement()
        {
            var src = "<?scryber parser-mode='Strict' ?><html xmlns='http://www.w3.org/1999/xhtml' xmlns:c='Scryber.Core.UnitTests.Generation, Scryber.UnitTests'><body><c:DashboardCard id='dc' title='x'><div>Q1 Sales</div><div>Q2 Sales</div></c:DashboardCard><c:DashboardCard id='dc2'><c:Content id='inner'><p>Inside</p></c:Content></c:DashboardCard></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var dc = doc.FindAComponentById("dc") as DashboardCardComponent;
                Assert.IsNotNull(dc);
                Assert.AreEqual(2, dc.Contents.Count);
                var text = Text(ms.ToArray());
                Assert.IsTrue(text.Contains("Q1 Sales") && text.Contains("Q2 Sales"), "Child content was not rendered: " + text);

                var dc2 = doc.FindAComponentById("dc2") as DashboardCardComponent;
                Assert.IsNotNull(dc2.ContentPanel);
                Assert.AreEqual(1, dc2.ContentPanel.Contents.Count, "Panel element content was not parsed");
            }
        }

        [TestMethod]
        public void MovedSectionsRenderInColumns()
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml' xmlns:c='Scryber.Core.UnitTests.Generation, Scryber.UnitTests'><body><c:SectionedPanel id='cp' style='column-count:2; width:400pt'><c:Sections><div><h3>Alpha</h3><p>One</p></div><div><h3>Beta</h3><p>Two</p></div><div><h3>Gamma</h3><p>Three</p></div></c:Sections></c:SectionedPanel></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                var text = Text(ms.ToArray());
                Assert.IsTrue(text.Contains("Alpha") && text.Contains("Beta") && text.Contains("Gamma"), "Not all sections were rendered: " + text);
            }
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
