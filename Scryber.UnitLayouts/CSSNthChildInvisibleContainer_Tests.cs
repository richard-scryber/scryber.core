using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Verifies the :nth-child / :nth-of-type family of structural pseudo-classes when the real
    /// siblings involved are separated by an IInvisibleContainer - most commonly the per-iteration
    /// wrapper a {{#each}} binding produces, but also {{#if}}/{{#with}} and any nesting of them.
    /// An IInvisibleContainer doesn't create a real level in the structural hierarchy - its own
    /// content should count as if spliced directly into the real enclosing container, matching
    /// the same flattening ComponentWrappingList&lt;T&gt;.BuildAllItems already does for typed
    /// child collections (e.g. HTMLSelect's Choices). See Component.PopulateSiblingPosition /
    /// CountSiblingContent.
    /// </summary>
    [TestClass()]
    public class CSSNthChildInvisibleContainer_Tests
    {
        private const string TestCategory = "Inject-Layouts";

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutBlock[] GetWrapItems(string html, object model)
        {
            var doc = Document.ParseHtmlDocument(new StringReader(html));
            if (null != model)
                doc.Params["model"] = model;

            using (var ms = DocStreams.GetOutputStream("CSSNthChildInvisibleContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var wrapBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(wrapBlock, "wrap div block not found");

            var wrapRegion = wrapBlock.Columns[0];
            var items = new PDFLayoutBlock[wrapRegion.Contents.Count];
            for (int i = 0; i < items.Length; i++)
                items[i] = wrapRegion.Contents[i] as PDFLayoutBlock;
            return items;
        }

        private static string BorderColor(PDFLayoutBlock block)
        {
            Assert.IsNotNull(block, "Expected a layout block");
            return block.FullStyle.Border.Color.ToString();
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Each_NthOfType_AlternatesAcrossGeneratedItems()
        {
            var html =
                "<html><head><style>" +
                ".wrap p:nth-of-type(odd) { border: solid 2pt red; } " +
                ".wrap p:nth-of-type(even) { border: solid 2pt green; }" +
                "</style></head><body>" +
                "<div class='wrap'>{{#each model.items}}<p>{{this.name}}</p>{{/each}}</div>" +
                "</body></html>";

            var model = new { items = new object[] { new { name = "One" }, new { name = "Two" }, new { name = "Three" } } };
            var items = GetWrapItems(html, model);

            Assert.AreEqual(3, items.Length, "whitespace text nodes must not be counted as content items");
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[0]), "1st <p> should match :nth-of-type(odd)");
            Assert.AreEqual("rgb(0,128,0)", BorderColor(items[1]), "2nd <p> should match :nth-of-type(even)");
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[2]), "3rd <p> should match :nth-of-type(odd)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Each_NthChildVsNthOfType_DifferByMixedTagSibling()
        {
            // A leading <div> that is NOT part of the each-loop shifts the plain :nth-child
            // count but must not shift the p-only :nth-of-type count for the first <p>.
            var html =
                "<html><head><style>" +
                ".wrap div:nth-child(1) { border: solid 2pt red; } " +
                ".wrap p:nth-of-type(1) { border: solid 2pt green; }" +
                "</style></head><body>" +
                "<div class='wrap'><div>Lead</div>{{#each model.items}}<p>{{this.name}}</p>{{/each}}</div>" +
                "</body></html>";

            var model = new { items = new object[] { new { name = "One" }, new { name = "Two" } } };
            var items = GetWrapItems(html, model);

            Assert.AreEqual(3, items.Length, "leading div + 2 generated <p> elements");
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[0]), "leading <div> is div:nth-child(1)");
            Assert.AreEqual("rgb(0,128,0)", BorderColor(items[1]), "1st generated <p> is p:nth-of-type(1), not :nth-child(1)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Each_MixedWithRegularSiblings_PreservesDocumentOrder()
        {
            // Regular siblings before and after the {{#each}} block must count in true document
            // order alongside the generated items, not be pushed out by the invisible wrapper.
            // Checked via the component's own resolved style (not layout block position) - a
            // <div> immediately (no whitespace) after {{/each}} isn't attached to the component
            // tree correctly at all (a separate, pre-existing HTML/each-boundary parsing bug,
            // unrelated to sibling counting - worth its own follow-up), so a space separates
            // {{/each}} from the trailing <div> here, which is realistic anyway (real templates
            // are normally whitespace-formatted, not minified onto one line).
            var html =
                "<html><head><style>" +
                ".wrap div:nth-child(5) { border: solid 2pt red; }" +
                "</style></head><body>" +
                "<div class='wrap'><div id='lead'>A</div>{{#each model.items}}<p>{{this.name}}</p>{{/each}} <div id='trail'>B</div></div>" +
                "</body></html>";

            var model = new { items = new object[] { new { name = "One" }, new { name = "Two" }, new { name = "Three" } } };

            var doc = Document.ParseHtmlDocument(new StringReader(html));
            doc.Params["model"] = model;
            using (var ms = DocStreams.GetOutputStream("CSSNthChildInvisibleContainer.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            var lead = doc.FindAComponentById("lead");
            var trail = doc.FindAComponentById("trail");
            Assert.IsNotNull(lead);
            Assert.IsNotNull(trail);

            // div(1) p(2) p(3) p(4) div(5)
            Assert.AreNotEqual("rgb(255,0,0)", lead.GetAppliedStyle().Border.Color.ToString(), "leading <div> is the 1st child, not the 5th");
            Assert.AreEqual("rgb(255,0,0)", trail.GetAppliedStyle().Border.Color.ToString(), "trailing <div> is the 5th child overall and should match div:nth-child(5)");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Each_NestedInsideIf_HierarchyFlattensCorrectly()
        {
            // Nested invisible containers (an {{#if}} inside an {{#each}}) must still flatten
            // fully, not just one level - confirms the recursive case, not just a single unwrap.
            var html =
                "<html><head><style>" +
                ".wrap p:nth-of-type(odd) { border: solid 2pt red; } " +
                ".wrap p:nth-of-type(even) { border: solid 2pt green; }" +
                "</style></head><body>" +
                "<div class='wrap'>{{#each model.items}}{{#if this.show}}<p>{{this.name}}</p>{{/if}}{{/each}}</div>" +
                "</body></html>";

            var model = new
            {
                items = new object[]
                {
                    new { name = "One", show = true },
                    new { name = "Two", show = true },
                    new { name = "Three", show = true }
                }
            };
            var items = GetWrapItems(html, model);

            Assert.AreEqual(3, items.Length, "each+if nested wrapping must still flatten to 3 real <p> items");
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[0]));
            Assert.AreEqual("rgb(0,128,0)", BorderColor(items[1]));
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[2]));
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void Each_FirstLastChild_WithGeneratedItems()
        {
            var html =
                "<html><head><style>" +
                ".wrap p:first-child { border: solid 2pt red; } " +
                ".wrap p:last-child { border: solid 2pt green; }" +
                "</style></head><body>" +
                "<div class='wrap'>{{#each model.items}}<p>{{this.name}}</p>{{/each}}</div>" +
                "</body></html>";

            var model = new { items = new object[] { new { name = "One" }, new { name = "Two" }, new { name = "Three" } } };
            var items = GetWrapItems(html, model);

            Assert.AreEqual(3, items.Length);
            Assert.AreEqual("rgb(255,0,0)", BorderColor(items[0]), "1st generated <p> should match :first-child");
            Assert.AreEqual("rgb(0,128,0)", BorderColor(items[2]), "last generated <p> should match :last-child");
        }
    }
}
