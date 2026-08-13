using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Styles;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class InjectLayout_Tests
    {
        private const string TestCategory = "Inject-Layouts";

        // Page and container sizes chosen to give clean integer column widths.
        private const double PageW = 600;
        private const double PageH = 800;

        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        // -----------------------------------------------------------------------
        // Dynamic Injection — basic layout with injected content
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimple()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner =
                @"<h2 xmlns='http://www.w3.org/1999/xhtml' style='padding: 10pt; background-color: lime; border: solid 1pt green;' >
This is the inner content
</h2>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimple.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");
            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimpleHTML()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' data-content-type='text/html' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner = @"<h2 style='padding: 10pt; background-color: lime; border: solid 1pt green;' >
This is the inner content
</h2>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimpleHTML.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");
            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimpleMarkdown()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
<style>
    h2 { padding: 10pt; background-color: lime; border: solid 1pt green; }
</style>
</head>
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' data-content-type='text/markdown' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner = @"## This is the inner content";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimpleMarkdown.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");
            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        // -----------------------------------------------------------------------
        // Dynamic Injection —  layout with binding statements
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimpleWithBinding()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner =
                @"<h2 xmlns='http://www.w3.org/1999/xhtml' style='padding: 10pt; background-color: lime; border: solid 1pt green;' >
This is the {{model.content}}
</h2>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new { content = "inner content" };

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimpleWithBinding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");
            Assert.AreEqual(9, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the ", chars.Characters);

            chars = innerLine.Runs[4] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimpleHTMLWithBinding()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' data-content-type='text/html' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner = @"<h2 style='padding: 10pt; background-color: lime; border: solid 1pt green;' >
This is the {{model.content}}
</h2>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new { content = "inner content" };
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimpleHTMLWithBinding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");

            Assert.AreEqual(9, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the ", chars.Characters);

            chars = innerLine.Runs[4] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_SingleSimpleMarkdownWithBinding()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
<style>
    h2 { padding: 10pt; background-color: lime; border: solid 1pt green; }
</style>
</head>
<body>
    <h1>Content below is injected into the document</h1>
    <div data-content='{{$layouts[""innerContent""]}}' data-content-type='text/markdown' ></div>
    <div>After the content</div>
</body>
</html>";

            var inner = @"## This is the {{model.content}}";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new { content = "inner content" };

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_SingleSimpleMarkdownWithBinding.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(1, injected.Columns[0].Contents.Count);

            var innerBlock = injected.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock, "inner layout block should not be null");
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine, "inner layout line should not be null");

            Assert.AreEqual(6, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("This is the ", chars.Characters);

            chars = innerLine.Runs[4] as PDFTextRunCharacter;
            Assert.IsNotNull(chars, "chars should not be null");
            Assert.AreEqual("inner content", chars.Characters);

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        // -----------------------------------------------------------------------
        // Dynamic Injection —  layout within loops with binding statements
        // -----------------------------------------------------------------------

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_LoopedWithBinding()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <ul>
    {{#each model.items }}
    <li data-content='{{$layouts.innerContent}}' ></li>
    {{log {{concat(""inside the loop at index "", @index)}} }}
    {{/each}}
    </ul>
    <div>After the content</div>
</body>
</html>";

            var inner =
                @"<div xmlns='http://www.w3.org/1999/xhtml' style='padding: 2pt; background-color: lime; border: solid 1pt green;' >
This is the {{.name}} item at index {{.index}}</div>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            var items = new[]
            {
                new { index = 1, name = "first" },
                new { index = 2, name = "second" },
                new { index = 3, name = "third" },
                new { index = 4, name = "fourth" }
            };

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new
            {
                content = "inner content",
                items = items
            };

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_LoopedWithBinding.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(4, injected.Columns[0].Contents.Count);

            for (int i = 0; i < items.Length; i++)
            {
                var innerBlock = injected.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(innerBlock, "inner layout block should not be null");
                Assert.AreEqual(1, innerBlock.Columns.Length);
                Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);

                var divContent = innerBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(divContent, "divContent should not be null");
                Assert.AreEqual(1, divContent.Columns.Length);
                Assert.AreEqual(1, divContent.Columns[0].Contents.Count);

                var innerLine = divContent.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.IsNotNull(innerLine, "inner layout line should not be null");
                Assert.AreEqual(12, innerLine.Runs.Count);
                var chars = innerLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual("This is the ", chars.Characters);

                chars = innerLine.Runs[4] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].name, chars.Characters);

                chars = innerLine.Runs[7] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(" item at index ", chars.Characters);

                chars = innerLine.Runs[10] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].index.ToString(), chars.Characters);
            }

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_LoopedHTMLWithBinding()
        {
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h1>Content below is injected into the document</h1>
    <ul>
    {{#each model.items }}
    <li data-content='{{$layouts.innerContent}}' data-content-type='text/html' ></li>
    {{log {{concat(""inside the loop at index "", @index)}} }}
    {{/each}}
    </ul>
    <div>After the content</div>
</body>
</html>";

            var inner = @"<div style='padding: 2pt; background-color: lime; border: solid 1pt green;' >
This is the {{.name}} item at index {{.index}}</div>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            var items = new[]
            {
                new { index = 1, name = "first" },
                new { index = 2, name = "second" },
                new { index = 3, name = "third" },
                new { index = 4, name = "fourth" }
            };

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new
            {
                content = "inner content",
                items = items
            };

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_LoopedHTMLWithBinding.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(4, injected.Columns[0].Contents.Count);

            for (int i = 0; i < items.Length; i++)
            {
                var innerBlock = injected.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(innerBlock, "inner layout block should not be null");
                Assert.AreEqual(1, innerBlock.Columns.Length);
                Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);

                var divContent = innerBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(divContent, "divContent should not be null");
                Assert.AreEqual(1, divContent.Columns.Length);
                Assert.AreEqual(1, divContent.Columns[0].Contents.Count);

                var innerLine = divContent.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.IsNotNull(innerLine, "inner layout line should not be null");
                Assert.AreEqual(12, innerLine.Runs.Count);
                var chars = innerLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual("This is the ", chars.Characters);

                chars = innerLine.Runs[4] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].name, chars.Characters);

                chars = innerLine.Runs[7] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(" item at index ", chars.Characters);

                chars = innerLine.Runs[10] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].index.ToString(), chars.Characters);
            }

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_LoopedMarkdownWithBinding()
        {

            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
<style>
    h4 { padding: 2pt; background-color: lime; border: solid 1pt green; }
</style>
</head>
<body>
    <h1>Content below is injected into the document</h1>
    <ul>
    {{#each model.items }}
    <li data-content='{{$layouts.innerContent}}' data-content-type='text/markdown' ></li>
    {{log {{concat(""inside the loop at index "", @index)}} }}
    {{/each}}
    </ul>
    <div>After the content</div>
</body>
</html>";

            var inner = @"#### This is the {{.name}} item at index {{.index}}";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            layouts.Add("innerContent", inner);

            var items = new[]
            {
                new { index = 1, name = "first" },
                new { index = 2, name = "second" },
                new { index = 3, name = "third" },
                new { index = 4, name = "fourth" }
            };

            doc.Params["$layouts"] = layouts;
            doc.Params["model"] = new
            {
                content = "inner content",
                items = items
            };

            using (var ms = DocStreams.GetOutputStream("InjectLayouts_LoopedMarkdownWithBinding.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);

            var lpg = _layout.AllPages[0];
            var pageRegion = lpg.ContentBlock.Columns[0];
            var headingBlock = pageRegion.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(headingBlock, "heading layout block should not be null");

            var injected = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(injected, "injected layout block should not be null");
            Assert.AreEqual(1, injected.Columns.Length);
            Assert.AreEqual(4, injected.Columns[0].Contents.Count);

            for (int i = 0; i < items.Length; i++)
            {
                var innerBlock = injected.Columns[0].Contents[i] as PDFLayoutBlock;
                Assert.IsNotNull(innerBlock, "inner layout block should not be null");
                Assert.AreEqual(1, innerBlock.Columns.Length);
                Assert.AreEqual(2, innerBlock.Columns[0].Contents.Count);

                var divContent = innerBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(divContent, "divContent should not be null");
                Assert.AreEqual(1, divContent.Columns.Length);
                Assert.AreEqual(1, divContent.Columns[0].Contents.Count);

                var innerLine = divContent.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.IsNotNull(innerLine, "inner layout line should not be null");
                Assert.AreEqual(12, innerLine.Runs.Count);
                var chars = innerLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual("This is the ", chars.Characters);

                chars = innerLine.Runs[4] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].name, chars.Characters);

                chars = innerLine.Runs[7] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(" item at index ", chars.Characters);

                chars = innerLine.Runs[10] as PDFTextRunCharacter;
                Assert.IsNotNull(chars, "chars should not be null");
                Assert.AreEqual(items[i].index.ToString(), chars.Characters);
            }

            var after = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(after, "after layout block should not be null");

        }


        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceXHTMLFragment()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameXHTMLFragment.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' 
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame</div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceXHTMLFragment.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            Assert.AreEqual(1, frame.Columns[0].Contents.Count);

        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceHTMLMultiFragment()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameHTMLMultiFragment.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' data-content-type='text/html' 
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame</div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceHTMLMultiFragment.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            Assert.AreEqual(2, frame.Columns[0].Contents.Count);

        }
        
        /// <summary>
        /// Testing 2 components and a style.
        /// Contains a style definition that should not be allowed in the frame.
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceXHTMLComplexFragment()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameXHTMLComplexFragment.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"'
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div class='captured'>After the frame</div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceXHTMLComplexFragment.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            Assert.AreEqual(1, frame.Columns[0].Contents.Count);
            
            var divBlock = frame.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock, "div should not be null");
            Assert.AreEqual(2, divBlock.Columns[0].Contents.Count);
            
            //This block has the .captured class, but it should not be applied.
            var notBgBlue =  divBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(notBgBlue, "notBgBlue should not be null");
            Assert.AreEqual(Color.Transparent, notBgBlue.FullStyle.Background.Color, "The background should not be set, no permission");
            
            //Check the style in the frame is NOT registered with the document based on permissions
            Assert.AreEqual(0, doc.Styles.Count);

        }
        
        /// <summary>
        /// Testing 2 components and a style.
        /// Contains a style definition that SHOULDt be allowed in the frame (and only the frame).
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceXHTMLComplexFragmentAllowStyles()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameXHTMLComplexFragment.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' allow='inner-style; inline-styles'
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div class='captured'>After the frame</div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceXHTMLComplexFragmentAllowStyles.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            Assert.AreEqual(1, frame.Columns[0].Contents.Count);
            
            var divBlock = frame.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock, "div should not be null");
            Assert.AreEqual(2, divBlock.Columns[0].Contents.Count);
            
            //This block has the .captured class, but it should not be applied.
            var bgBlue =  divBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(bgBlue, "notBgBlue should not be null");
            Assert.AreEqual(StandardColors.Blue, bgBlue.FullStyle.Background.Color, "The background should be blue - permission");
            
            //Check the style in the frame is registered with the document but IS NOT enabled
            Assert.AreEqual(1, doc.Styles.Count);
            var style = doc.Styles[0] as StyleGroup;
            Assert.IsNotNull(style, "style should not be null");
            Assert.IsFalse(style.Enabled,  "style should not be enabled (even though it was allowed during layout of frame content.");
            
            //Check the div after the frame - should be transparent
            var outerDiv = pageRegion.Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(outerDiv, "outerDiv should not be null");
            Assert.AreEqual(StandardColors.Transparent, outerDiv.FullStyle.Background.Color, "The background should be transparent, no longer in scope.");

        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceHTMLMultiFragmentWithBinding()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameHTMLMultiFragmentBinding.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' data-content-type='text/html' 
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame with <var>{{model.restored}}</var></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceHTMLMultiFragmentWithBinding.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.Params["model"] = new { fallout = "This should not appear", restored = "This value should be accessible after the frame" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            Assert.AreEqual(2, frame.Columns[0].Contents.Count);

            var vars = doc.FindMatches("var");
            Assert.IsNotNull(vars, "vars should not be null");
            Assert.AreEqual(3, vars.Count);
            var var1 = vars[0] as HTMLVar;
            var var2 = vars[1] as HTMLVar;
            var varAfter = vars[2] as HTMLVar;
            
            Assert.IsNotNull(var1, "var1 should not be null");
            Assert.IsNotNull(var2, "var2 should not be null");
            Assert.IsNotNull(varAfter, "varAfter should not be null");
            
            Assert.AreEqual(1, var1.Contents.Count);
            var literal = var1.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.IsTrue(string.IsNullOrEmpty(literal.Text), "Frame literal should not have text");
            
            Assert.AreEqual(1, var2.Contents.Count);
            literal = var2.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.IsTrue(string.IsNullOrEmpty(literal.Text), "Frame literal not should have text");
            
            //after the frame is still bound correctly
            Assert.AreEqual(1, varAfter.Contents.Count);
            literal = varAfter.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.AreEqual("This value should be accessible after the frame", literal.Text, "literal should have text '" + literal.Text + "'");

        }
        
        //HTMLDocumentHelperMethods

        
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceHTMLMultiFragmentWithBindingAllowed()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameHTMLMultiFragmentBinding.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' data-content-type='text/html' allow='data-passthrough'
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame with <var>{{model.restored}}</var></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceHTMLMultiFragmentWithBindingAllowed.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.Params["model"] = new { fallout = "This should appear", restored = "This value should be accessible after the frame" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(_layout, "Layout should not be null");
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            
            Assert.AreEqual(2, frame.Columns[0].Contents.Count);

            var vars = doc.FindMatches("var");
            Assert.IsNotNull(vars, "vars should not be null");
            Assert.AreEqual(3, vars.Count);
            var var1 = vars[0] as HTMLVar;
            var var2 = vars[1] as HTMLVar;
            var varAfter = vars[2] as HTMLVar;
            
            Assert.IsNotNull(var1, "var1 should not be null");
            Assert.IsNotNull(var2, "var2 should not be null");
            Assert.IsNotNull(varAfter, "varAfter should not be null");
            
            Assert.AreEqual(1, var1.Contents.Count);
            var literal = var1.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.AreEqual("This should appear", literal.Text, "literal should have text '" + literal.Text + "'");

            Assert.AreEqual(1, var2.Contents.Count);
            literal = var2.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.IsTrue(literal.Text.Length > 0, "Frame literal should have text");
            
            //after the frame is still bound correctly
            Assert.AreEqual(1, varAfter.Contents.Count);
            literal = varAfter.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.AreEqual("This value should be accessible after the frame", literal.Text, "literal should have text '" + literal.Text + "'");

        }
        
        
        protected PDFLayoutBlock AssertGetFrameBlock(PDFLayoutDocument layout)
        {
            Assert.IsNotNull(layout, "Layout should not be null");
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var pageRegion = pg.ContentBlock.Columns[0];
            Assert.IsNotNull(pageRegion, "pageRegion should not be null");
            Assert.AreEqual(3, pageRegion.Contents.Count);
            
            var frame = pageRegion.Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(frame, "frame should not be null");
            Assert.AreEqual(1, frame.Columns.Length);
            return frame;
        }
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_SourceHTMLDocument_DefaultPermissions()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameHTMLMultiDocument.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' data-content-type='text/html'
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame with <var>{{model.restored}}</var></div>
</body>
</html>";

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_SourceHTMLMultiDocument_DefaultPermissions.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.Params["model"] = new { fallout = "This should appear", restored = "This value should be accessible after the frame" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var frame = AssertGetFrameBlock(_layout);
            Assert.AreEqual(1, frame.Columns[0].Contents.Count);

            var article = frame.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(article, "article should not be null");
            Assert.IsInstanceOfType(article.Owner, typeof(HTMLArticle), "article should be HTMLArticle");
            

            var vars = doc.FindMatches("var");
            Assert.IsNotNull(vars, "vars should not be null");
            Assert.AreEqual(2, vars.Count);
            var var1 = vars[0] as HTMLVar;
            var varAfter = vars[1] as HTMLVar;
            
            Assert.IsNotNull(var1, "var1 should not be null");
            Assert.IsNotNull(varAfter, "varAfter should not be null");
            
            Assert.AreEqual(1, var1.Contents.Count);
            var literal = var1.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.IsNull(literal.Text, "literal should not have text");
            
            //after the frame is still bound correctly
            Assert.AreEqual(1, varAfter.Contents.Count);
            literal = varAfter.Contents[0] as TextLiteral;
            Assert.IsNotNull(literal, "literal should not be null");
            Assert.AreEqual("This value should be accessible after the frame", literal.Text, "literal should have text '" + literal.Text + "'");

        }
        
        // Inner HTML used by data-content permission tests — contains one of each element type.
        // Note: <form> is not a registered HTML component, so input is placed directly in body.
        private static readonly string IFramePermissionInnerHtml =
            "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            "<head><style>.head-style{ color:red; }</style></head>" +
            "<body>" +
            "<div>Value: <var>{{model.value}}</var></div>" +
            "<div class='inner-inline' style='color:blue;'>Inline Styled</div>" +
            "<img id='inner-img' src='missing.png' alt='test' />" +
            "<a id='inner-link' href='https://example.com'>Link</a>" +
            "<style>.body-style{ font-size:12pt; }</style>" +
            "<iframe id='inner-frame' src='nested.html'>Nested frame</iframe>" +
            "<input id='inner-input' type='text' name='field' />" +
            "</body></html>";

        // Outer template used by data-content permission tests
        private static string BuildPermissionOuterTemplate(string allowPolicy = "inline-styles any; inner-images any; inner-navigation any") =>
            "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            "<body>" +
            "<h3>Content below</h3>" +
            "<iframe id='srcFrame' data-content='{{boundContent}}' allow='" + allowPolicy + "' style='border:solid 1pt black;'></iframe>" +
            "<div>After: <var>{{model.restored}}</var></div>" +
            "</body></html>";

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContentDocument_DefaultPermissions()
        {
            // Default: inline-styles any; inner-images any; inner-navigation any
            // Denied by default: inner-style, inner-link, outer-html, inner-frames, inner-forms, data-passthrough, style-passthrough
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate()));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContentDocument_DefaultPermissions.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "Model Value", restored = "Restored After" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // Frame wraps content in HTMLArticle (outer-html denied by default)
            var frame = AssertGetFrameBlock(_layout);
            Assert.IsTrue(frame.Columns[0].Contents.Count > 0, "Frame should have content");
            var articleBlock = frame.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(articleBlock, "Article block should exist");
            Assert.IsInstanceOfType(articleBlock.Owner, typeof(HTMLArticle), "Content should be wrapped in HTMLArticle (outer-html denied)");

            // inner-style denied: style blocks stripped (outer template has none, so total = 0)
            var styles = doc.FindMatches("style");
            Assert.AreEqual(0, styles.Count, "Style blocks should be removed (inner-style denied)");

            // inner-images allowed: img kept
            var images = doc.FindMatches("img");
            Assert.AreEqual(1, images.Count, "Image should be kept (inner-images allowed)");

            // inner-navigation allowed: anchor kept
            var links = doc.FindMatches("a");
            Assert.AreEqual(1, links.Count, "Link should be kept (inner-navigation allowed)");

            // inner-frames denied: nested iframe removed; only outer srcFrame remains
            var frames = doc.FindMatches("iframe");
            Assert.AreEqual(1, frames.Count, "Only outer iframe should remain (inner-frames denied)");

            // inner-forms denied: input element stripped (form tag is not a registered component)
            var inputs = doc.FindMatches("input");
            Assert.AreEqual(0, inputs.Count, "Input should be removed (inner-forms denied)");

            // data-passthrough denied: outer model.restored is still accessible after the frame
            var vars = doc.FindMatches("var");
            Assert.AreEqual(2, vars.Count, "Two var elements: one inside frame, one after");
            var varAfter = vars[1] as HTMLVar;
            Assert.IsNotNull(varAfter, "varAfter should not be null");
            Assert.AreEqual(1, varAfter.Contents.Count);
            var literal = varAfter.Contents[0] as TextLiteral;
            Assert.AreEqual("Restored After", literal.Text, "Outer var should be bound to model.restored");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_DataPassthrough_Allow()
        {
            // data-passthrough any: model IS accessible inside the frame
            var policy = "data-passthrough any; inline-styles any; inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_DataPassthrough_Allow.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "Bound Inside", restored = "Restored After" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var vars = doc.FindMatches("var");
            Assert.AreEqual(2, vars.Count, "Two var elements: one inside frame, one after");

            // Inner var should be bound to model.value
            var varInner = vars[0] as HTMLVar;
            Assert.IsNotNull(varInner, "varInner should not be null");
            Assert.AreEqual(1, varInner.Contents.Count);
            var innerLiteral = varInner.Contents[0] as TextLiteral;
            Assert.IsNotNull(innerLiteral, "inner literal should not be null");
            Assert.AreEqual("Bound Inside", innerLiteral.Text, "Inner var should be bound to model.value");

            // Outer var still accessible
            var varAfter = vars[1] as HTMLVar;
            Assert.IsNotNull(varAfter, "varAfter should not be null");
            var literal = varAfter.Contents[0] as TextLiteral;
            Assert.AreEqual("Restored After", literal.Text, "Outer var should be bound to model.restored");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerStyle_Allow()
        {
            // inner-style any: style blocks are kept in the frame
            var policy = "inner-style any; inline-styles any; inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerStyle_Allow.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // style block in body should be kept (inner-style allowed); head style excluded by article wrap
            var styles = doc.FindMatches("style");
            Assert.AreEqual(1, styles.Count, "Body style block should be kept when inner-style is allowed");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InlineStyles_Deny()
        {
            // No inline-styles in policy: style attributes should be stripped
            var policy = "inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InlineStyles_Deny.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // The inline-styled div should have its Style cleared
            var inlineDivs = doc.FindMatches(".inner-inline");
            Assert.AreEqual(1, inlineDivs.Count, "Inline-styled div should exist");
            var inlineDiv = inlineDivs[0] as IStyledComponent;
            Assert.IsNotNull(inlineDiv, "Inline div should be an IStyledComponent");
            Assert.IsFalse(inlineDiv.Style.IsValueDefined(StyleKeys.FillColorKey),
                "Inline color style should be cleared when inline-styles is denied");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerImages_Deny()
        {
            // No inner-images in policy: img elements should be removed
            var policy = "inline-styles any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerImages_Deny.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var images = doc.FindMatches("img");
            Assert.AreEqual(0, images.Count, "Images should be removed when inner-images is denied");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerNavigation_Deny()
        {
            // No inner-navigation in policy: anchor href should be cleared (element kept but link removed)
            var policy = "inline-styles any; inner-images any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerNavigation_Deny.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // Anchor element still exists but href is cleared
            var links = doc.FindMatches("a");
            Assert.AreEqual(1, links.Count, "Anchor element should still exist");
            var anchor = links[0] as HTMLAnchor;
            Assert.IsNotNull(anchor, "Link should be an HTMLAnchor");
            Assert.IsTrue(string.IsNullOrEmpty(anchor.File),
                "Anchor href should be cleared when inner-navigation is denied");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_OuterHtml_Allow()
        {
            // outer-html any: content is wrapped in spoof document div, not HTMLArticle
            var policy = "outer-html any; inline-styles any; inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_OuterHtml_Allow.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var frame = AssertGetFrameBlock(_layout);
            Assert.IsTrue(frame.Columns[0].Contents.Count > 0, "Frame should have content");
            var outerBlock = frame.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(outerBlock, "Outer block should exist");

            // With outer-html allowed, content is wrapped in a Div (spoof document), not HTMLArticle
            Assert.IsNotInstanceOfType(outerBlock.Owner, typeof(HTMLArticle),
                "Content should NOT be wrapped in HTMLArticle when outer-html is allowed");
            Assert.IsInstanceOfType(outerBlock.Owner, typeof(Div),
                "Content should be wrapped in a Div (spoof document) when outer-html is allowed");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerFrames_Allow()
        {
            // inner-frames any: nested iframe is kept
            var policy = "inner-frames any; inline-styles any; inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerFrames_Allow.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // Both outer srcFrame and inner-frame should exist
            var frames = doc.FindMatches("iframe");
            Assert.AreEqual(2, frames.Count, "Both outer and inner iframes should exist when inner-frames is allowed");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerForms_Deny()
        {
            // inner-forms denied (default): input elements removed (form tag is not a registered component)
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate()));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerForms_Deny.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            var inputs = doc.FindMatches("input");
            Assert.AreEqual(0, inputs.Count, "Input should be removed when inner-forms is denied");
        }

        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContent_InnerForms_Allow()
        {
            // inner-forms any: with allow policy, no crash occurs and document renders correctly.
            // Form elements (input, select, button) are not currently registered components so
            // they produce no components in the tree; this test verifies the policy is accepted.
            var policy = "inner-forms any; inline-styles any; inner-images any; inner-navigation any";
            var doc = Document.ParseDocument(new StringReader(BuildPermissionOuterTemplate(policy)));
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContent_InnerForms_Allow.pdf"))
            {
                doc.Params["boundContent"] = IFramePermissionInnerHtml;
                doc.Params["model"] = new { value = "V", restored = "R" };
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            // Frame should still have content despite allow policy
            var frame = AssertGetFrameBlock(_layout);
            Assert.IsTrue(frame.Columns[0].Contents.Count > 0, "Frame should have content with inner-forms allowed");

            // Outer model still accessible after the frame
            var vars = doc.FindMatches("var");
            Assert.AreEqual(2, vars.Count);
            var varAfter = vars[1] as HTMLVar;
            var literal = varAfter.Contents[0] as TextLiteral;
            Assert.AreEqual("R", literal.Text, "Outer var should still be accessible");
        }
    }
}
