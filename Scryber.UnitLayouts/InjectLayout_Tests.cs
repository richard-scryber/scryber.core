using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
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
        public void InjectLayouts_iFrame_SourceXHTMLComplexFragment()
        {
            var path = DocStreams.AssertGetTemplatePath("Content/HTML/InnerContent/FrameXHTMLComplexFragment.html");
            
            var str = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body>
    <h3>Content below is injected into an iFrame</h3>
    <iframe id='srcFrame' src='" + path + @"' allow='inner-styles'
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
    }
}
