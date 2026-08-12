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
        
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void InjectLayouts_iFrame_DataContentDocument_DefaultPermissions()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameHTMLMultiDocument.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' data-content='{{boundContent}}' data-content-type='text/html'
        style='border:solid 1pt black; padding: 5pt; background-color: silver;' ></iframe>
    <div>After the frame with <var>{{model.restored}}</var></div>
</body>
</html>";
            
            var text = System.IO.File.ReadAllText(path);

            var doc = Document.ParseDocument(new StringReader(str));

            var layouts = new Dictionary<string, string>();
            
            using (var ms = DocStreams.GetOutputStream("InjectLayouts_iFrame_DataContentDocument_DefaultPermissions.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.Params["boundContent"] = str;
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
    }
}
