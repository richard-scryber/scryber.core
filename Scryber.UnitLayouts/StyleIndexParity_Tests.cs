using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// A style block with more than 30 rules is matched through a style index rather than a loop over the rules.
    /// Each test here runs the same css with 0 extra rules (the loop) and 40 extra rules (the index), and the result must be the same.
    /// </summary>
    [TestClass()]
    public class StyleIndexParity_Tests
    {
        private PDFLayoutDocument layoutDoc;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layoutDoc = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private static string Filler(int count)
        {
            var sb = new System.Text.StringBuilder();
            for (var i = 0; i < count; i++)
                sb.AppendLine(".filler" + i + " { color: #000; }");
            return sb.ToString();
        }

        private Document Layout(string css, string body, string name, int fillers)
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
" + css + @"
" + Filler(fillers) + @"
</style></head>
<body>
" + body + @"
</body></html>";

            var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent);
            doc.LayoutComplete += Doc_LayoutComplete;
            using (var ms = DocStreams.GetOutputStream("StyleIndexParity_" + name + "_" + fillers + ".pdf"))
            {
                doc.SaveAsPDF(ms);
            }
            return doc;
        }

        private static Color ColorOf(Document doc, string id)
        {
            var comp = doc.FindAComponentById(id) as Component;
            Assert.IsNotNull(comp, "No component with id " + id);
            var style = comp.GetAppliedStyle();
            return style.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent);
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_01_PageRule(int fillers)
        {
            Layout("@page { size: A5; margin: 16mm; }",
                "<p>Page 1</p><div style='page-break-before:always'><p>Page 2</p></div>", "Page", fillers);

            Assert.AreEqual(2, layoutDoc.AllPages.Count);
            foreach (var pg in layoutDoc.AllPages)
            {
                Assert.AreEqual(16.0, pg.PositionOptions.Margins.Left.ToMillimeters().Value, 0.01, "Margin with " + fillers + " extra rules");
                Assert.AreEqual(148.0, pg.Size.Width.ToMillimeters().Value, 0.5, "A5 width with " + fillers + " extra rules");
            }
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_02_PagePseudoClassAndName(int fillers)
        {
            Layout(@"@page { margin: 10mm; }
@page :first { margin: 30mm; }
@page wide { size: A4 landscape; }
.wide { page: wide; }",
                "<p>Page 1</p><div style='page-break-before:always'><p>Page 2</p></div><div class='wide' style='page-break-before:always'><p>Page 3</p></div>", "PageNamed", fillers);

            Assert.AreEqual(3, layoutDoc.AllPages.Count);
            Assert.AreEqual(30.0, layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters().Value, 0.01, ":first with " + fillers + " extra rules");
            Assert.AreEqual(10.0, layoutDoc.AllPages[1].PositionOptions.Margins.Left.ToMillimeters().Value, 0.01, "page 2 with " + fillers + " extra rules");
            Assert.IsTrue(layoutDoc.AllPages[2].Size.Width > layoutDoc.AllPages[2].Size.Height, "Named landscape page with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_03_MediaRules(int fillers)
        {
            var doc = Layout(@"@media print { .printed { color: #FF0000; } }
@media screen { .screened { color: #FF0000; } }
.printed, .screened { color: #0000FF; }",
                "<p id='printed' class='printed'>print</p><p id='screened' class='screened'>screen</p>", "Media", fillers);

            Assert.AreEqual(new Color(0, 0, 255), ColorOf(doc, "printed"), "@media print then a later rule, with " + fillers + " extra rules");
            Assert.AreEqual(new Color(0, 0, 255), ColorOf(doc, "screened"), "@media screen is not applied, with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_04_MediaRuleApplies(int fillers)
        {
            var doc = Layout(@".printed, .screened { color: #0000FF; }
@media print { .printed { color: #FF0000; } }
@media screen { .screened { color: #FF0000; } }",
                "<p id='printed' class='printed'>print</p><p id='screened' class='screened'>screen</p>", "MediaApplies", fillers);

            Assert.AreEqual(new Color(255, 0, 0), ColorOf(doc, "printed"), "@media print, with " + fillers + " extra rules");
            Assert.AreEqual(new Color(0, 0, 255), ColorOf(doc, "screened"), "@media screen, with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_05_PageRuleInsideMedia(int fillers)
        {
            Layout("@media print { @page { margin: 22mm; } }", "<p>Page 1</p>", "PageInMedia", fillers);

            Assert.AreEqual(22.0, layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters().Value, 0.01, "@page in @media print with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_06_LaterRuleWinsAtTheSameSpecificity(int fillers)
        {
            //the class attribute lists b before a, but the rule for b is later in the css, so it wins
            var doc = Layout(@".a { color: #FF0000; }
.b { color: #0000FF; }",
                "<p id='both' class='b a'>both</p>", "SourceOrder", fillers);

            Assert.AreEqual(new Color(0, 0, 255), ColorOf(doc, "both"), "Later rule with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_07_RootAndUniversalRules(int fillers)
        {
            var doc = Layout(@":root { --brand: #00FF00; }
* { color: #FF0000; }
p { color: var(--brand); }",
                "<p id='para'>para</p><span id='span'>span</span>", "Root", fillers);

            Assert.AreEqual(new Color(0, 255, 0), ColorOf(doc, "para"), "var from :root with " + fillers + " extra rules");
            Assert.AreEqual(new Color(255, 0, 0), ColorOf(doc, "span"), "universal rule with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_08_GroupInsideALargeCollection(int fillers)
        {
            //a style group in the document styles (not a definition) was added to a list that had not been created,
            //when there were more than 30 styles
            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            var div = new Div() { ID = "grouped", StyleClass = "grouped" };
            div.Contents.Add(new TextLiteral("In a group"));
            pg.Contents.Add(div);

            var group = new StyleGroup();
            var inner = new StyleDefn(".grouped");
            inner.Fill.Color = new Color(255, 0, 0);
            group.Styles.Add(inner);
            doc.Styles.Add(group);

            for (var i = 0; i < fillers; i++)
            {
                var filler = new StyleDefn(".filler" + i);
                filler.Fill.Color = new Color(0, 0, 0);
                doc.Styles.Add(filler);
            }

            using (var ms = DocStreams.GetOutputStream("StyleIndexParity_Group_" + fillers + ".pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(new Color(255, 0, 0), ColorOf(doc, "grouped"), "Grouped style with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(0)]
        [DataRow(40)]
        public void StyleIndexParity_09_SameSelectorLaterInTheCss(int fillers)
        {
            //the second .a is after all the other rules, and only replaces the color
            var css = ".a { color: #FF0000; font-weight: bold; }\n" + Filler(fillers) + ".a { color: #0000FF; }";
            var doc = Layout(css, "<p id='para' class='a'>para</p>", "SameSelector", 0);

            var comp = doc.FindAComponentById("para") as Component;
            var style = comp.GetAppliedStyle();
            Assert.AreEqual(new Color(0, 0, 255), style.GetValue(StyleKeys.FillColorKey, StandardColors.Transparent), "Later .a color with " + fillers + " extra rules");
            Assert.AreEqual(700, style.GetValue(StyleKeys.FontWeightKey, 400), "Earlier .a weight with " + fillers + " extra rules");
        }

        [TestMethod()]
        [DataRow(1)]
        [DataRow(35)]
        public void StyleIndexParity_10_ManyStyleBlocks(int blocks)
        {
            //each style block is a group in the document styles, so more than 30 blocks use the index
            var head = new System.Text.StringBuilder();
            for (var i = 0; i < blocks; i++)
                head.AppendLine("<style>.block" + i + " { color: #000000; } .a { color: #FF0000; }</style>");
            head.AppendLine("<style>.a { color: #0000FF; } @page { margin: 12mm; }</style>");

            var src = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head>" + head + @"</head>
<body><p id='para' class='a'>para</p></body></html>";

            var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent);
            doc.LayoutComplete += Doc_LayoutComplete;
            using (var ms = DocStreams.GetOutputStream("StyleIndexParity_Blocks_" + blocks + ".pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(new Color(0, 0, 255), ColorOf(doc, "para"), "Last block wins with " + blocks + " blocks");
            Assert.AreEqual(12.0, layoutDoc.AllPages[0].PositionOptions.Margins.Left.ToMillimeters().Value, 0.01, "@page in the last block with " + blocks + " blocks");
        }
    }
}
