using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF.Layout;

namespace Scryber.Core.UnitTests.Binding
{

    [TestClass()]
    public class HtmlBinding_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }


        public HtmlBinding_Test()
        {
        }

        public PDFLayoutDocument DocumentLayout { get; set; }

        /// <summary>
        /// A test document with escaped text in inline, and pre-formatted styles to check the layout.
        /// </summary>
        private string HTMLBindContentLayoutEscaped = @"<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
        <style>
            h2 { font-size: 16pt; font-weight: 400; font-style: italic;}
            .wrapper { border: solid 1px red; margin: 10px; font-size: 12pt;}
            .preformatted{ white-space: pre; }
        </style>
    </head>
    <body style='padding: 10pt' >

        <h2>1. Single Line,not formatted</h2>
        <div class='wrapper' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt; &lt;/div&gt;
        </div>
        <div class='wrapper' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>

        <h2>2. Multiple Line, not formatted</h2>
        <div class='wrapper' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>3. Single Line with spans, not formatted</h2>
        <div class='wrapper' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>

        <h2>4. Multiple Line with spans, not formatted</h2>
        <div class='wrapper' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

        <h2>5. All on one line, pre-formatted</h2>
        <div class='wrapper preformatted' >&lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;</div>

        <h2>6. Single separate line pre-formatted</h2>
        <div class='wrapper preformatted' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>

        <h2>7. Multiple Line, pre-formatted</h2>
        <div class='wrapper preformatted' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>8. Single Line with spans, pre-formatted</h2>
        <div class='wrapper preformatted' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>
        <h2>9. Multiple Line with spans, pre-formatted</h2>
        <div class='wrapper preformatted' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

    </body>
</html>";

        /// <summary>
        /// No binding, just an initial test to make sure we are parsing
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_1NoBindingEscapedLayoutTest()
        {
            

            var src = HTMLBindContentLayoutEscaped;

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_LayoutEscpaped.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //Just the inner div


                    var div = wrapper.Contents[0] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    var content = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("After Content", content.Text);
                }
            }
        }

        /// <summary>
        /// No binding, just an initial test to make sure we are parsing
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_2InlineEscapedTest()
        {
            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;'>
            &lt;div&gt;Inner Content&lt;/div&gt;
            <div>After Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_InlineEscapedTags.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count); //Escaped content and another div

                    var lit = wrapper.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    

                    var div = wrapper.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    var content = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("After Content", content.Text);

                    Assert.IsNotNull(this.DocumentLayout);
                    var lpg = this.DocumentLayout.AllPages[0];
                    var cb = lpg.ContentBlock;
                    var lwrap = cb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);
                    Assert.AreEqual(2, lwrap.Columns[0].Contents.Count);
                    var l1 = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, l1.Runs.Count); //begin, chars, end.
                    var txt = l1.Runs[1] as PDFTextRunCharacter;
                    //includes a space at the end
                    Assert.AreEqual("<div>Inner Content</div> ", txt.Characters);

                }
            }
        }

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.DocumentLayout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        /// <summary>
        /// No binding, The escaped text should have a single space after it, and then the span should start.
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_3InlineEscapedAndAppendedTest()
        {
            
            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;'>
            &lt;div&gt;Inner Content&lt;/div&gt; <span style='font-weight: 700'>After Escaped</span>
            <div>After Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_InlineEscapedAndAppended.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(3, wrapper.Contents.Count); //Escaped content, span and another div

                    var lit = wrapper.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);

                    var span = wrapper.Contents[1] as Span;
                    Assert.IsNotNull(span);
                    Assert.AreEqual(1, span.Contents.Count);

                    var div = wrapper.Contents[2] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    var content = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("After Content", content.Text);

                    Assert.IsNotNull(this.DocumentLayout);
                    var lpg = this.DocumentLayout.AllPages[0];
                    var cb = lpg.ContentBlock;
                    var lwrap = cb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);
                    Assert.AreEqual(2, lwrap.Columns[0].Contents.Count);
                    var l1 = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(8, l1.Runs.Count); //begin, chars, end. spanbegin, begin, chars, end, spanend
                    var txt = l1.Runs[1] as PDFTextRunCharacter;
                    Assert.AreEqual("<div>Inner Content</div> ", txt.Characters); //content at the end.

                }
            }
        }

        /// <summary>
        /// With escaped text the multiple lines should be relaced with a space (as we are not preserving whitespace)
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_4EscapedTextMultipleLineTest()
        {

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;'>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <div>After Content</div>
        </div>
    </body>
</html>";

            var readContent = @"<div class='test' id='Appended' xmlns='' > <b>Inner & Content</b> </div> ";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_EscapedTextTags.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as TextLiteral; //pre-pended
                    Assert.IsNotNull(content);
                    
                    var div = wrapper.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    //Check the output of the text within the layout to confirm it's good
                    Assert.IsNotNull(this.DocumentLayout);
                    var lpg = this.DocumentLayout.AllPages[0];
                    var cb = lpg.ContentBlock;
                    var lwrap = cb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);
                    Assert.AreEqual(2, lwrap.Columns[0].Contents.Count);
                    var l1 = lwrap.Columns[0].Contents[0] as PDFLayoutLine;
                    Assert.AreEqual(3, l1.Runs.Count); //begin, chars, end
                    var txt = l1.Runs[1] as PDFTextRunCharacter;
                    Assert.AreEqual(readContent, txt.Characters); //content at the end.

                }
            }
        }
        /*     <pre>
    	&lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
    </pre> */

        /// <summary>
        /// With escaped text the multiple lines should be relaced with a space (as we are not preserving whitespace)
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_5EscapedTextMultipleLinePreservedTest()
        {

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red; white-space: pre'>
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>
    </body>
</html>";

            var readContent = new string[] { @"",
"            <div class='test' id='Appended' xmlns='' >",
"                <b>Inner & Content</b>",
"            </div> " };

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_EscapedPreservedTextTags.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as TextLiteral; //pre-pended
                    Assert.IsNotNull(content);

                    var div = wrapper.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    //Check the output of the text within the layout to confirm it's good
                    Assert.IsNotNull(this.DocumentLayout);
                    var lpg = this.DocumentLayout.AllPages[0];
                    var cb = lpg.ContentBlock;
                    var lwrap = cb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);
                    Assert.Inconclusive("Not tested");
                    Assert.AreEqual(1 + readContent.Length, lwrap.Columns[0].Contents.Count);
                    for (var i = 0; i < readContent.Length; i++)
                    {
                        var l1 = lwrap.Columns[0].Contents[i] as PDFLayoutLine;
                        Assert.AreEqual(3, l1.Runs.Count); //begin, chars, end
                        var txt = l1.Runs[1] as PDFTextRunCharacter;
                        Assert.AreEqual(readContent[i], txt.Characters); //content at the end.
                    }

                }
            }
        }

        /// <summary>
        /// With escaped text the multiple lines should be relaced with a space (as we are not preserving whitespace)
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_6EscapedTextMultipleLineWithBindingTest()
        {

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red; white-space: pre'>
            <span>Before Content</span>
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
            <span>After Content</span>
        </div>
    </body>
</html>";

            var readContent = new string[] { @"",
"            <div class='test' id='Appended' xmlns='' >",
"                <b>Inner & Content</b>",
"            </div> " };

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_EscapedPreservedWithBindingTags.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as TextLiteral; //pre-pended
                    Assert.IsNotNull(content);

                    var div = wrapper.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                    //Check the output of the text within the layout to confirm it's good
                    Assert.IsNotNull(this.DocumentLayout);
                    var lpg = this.DocumentLayout.AllPages[0];
                    var cb = lpg.ContentBlock;
                    var lwrap = cb.Columns[0].Contents[0] as PDFLayoutBlock;
                    Assert.IsNotNull(lwrap);

                    Assert.AreEqual(1 + readContent.Length, lwrap.Columns[0].Contents.Count);
                    for (var i = 0; i < readContent.Length; i++)
                    {
                        var l1 = lwrap.Columns[0].Contents[i] as PDFLayoutLine;
                        Assert.AreEqual(3, l1.Runs.Count); //begin, chars, end
                        var txt = l1.Runs[1] as PDFTextRunCharacter;
                        Assert.AreEqual(readContent[i], txt.Characters); //content at the end.
                    }

                }
            }
        }



        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_DefaultValue()
        {
            var literal = "Inner Content";
            var contentString = @"<div xmlns='http://www.w3.org/1999/xhtml' >" + literal + "</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}'></div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_Default.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as Div;
                    Assert.IsNotNull(content);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_AppendValue()
        {
            var literal = "Inner Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' >" + literal + "</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-action='Append'>
            <div>Before Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_Append.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[1] as Div; //appended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_PrependValue()
        {
            var literal = "Inner Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' >" + literal + "</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-action='Prepend'>
            <div>After Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_Prepend.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as Div; //pre-pended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_ReplaceValue()
        {
            var literal = "Inner Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' >" + literal + "</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-action='Replace'>
            <div>Removed Content</div>
            <span>Also removed</span>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_Replace.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //replaced

                    var content = wrapper.Contents[0] as Div;
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_AppendWithInnerBoundValue()
        {
            var literal = "Inner Bound Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' style='border: solid 1px blue; margin: var(--marginsize);' >{{literal}}</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-action='append'>
            <div>Before Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["boundContent"] = contentString;
                doc.Params["literal"] = literal;
                doc.Params["--marginsize"] = Unit.Pt(10);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_InnerBind.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[1] as Div; //appended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);
                    Assert.AreEqual(10, content.Margins.Top);
                    //Check that the bound content is there
                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindHtmlContent_HtmlFragment()
        {

            var literal = "Inner Bound Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' style='border: solid 1px blue; margin: var(--marginsize) ' >" + literal + "</div>" +
                "<div id='" + id + "2' style='border: solid 1px blue;  margin: calc(--marginsize + 5pt)' >" + literal + "2</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-type='text/html'>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["boundContent"] = contentString;
                doc.Params["literal"] = literal;
                doc.Params["--marginsize"] = Unit.Pt(10);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_HtmlFragment.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);

                    var fragment = wrapper.Contents[0] as HTMLFragmentWrapper; //content is put in an invisible fragment
                    Assert.IsNotNull(fragment);
                    Assert.AreEqual(2, fragment.Content.Count);

                    var content = fragment.Content[0] as Div;

                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);
                    Assert.AreEqual(10, content.Margins.Top);

                    //Check that the bound content is there
                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal, lit.Text);


                    content = fragment.Content[1] as Div;

                    Assert.AreEqual(id + "2", content.ID);
                    Assert.AreEqual(1, content.Contents.Count);
                    Assert.AreEqual(10 + 5, content.Margins.Top);
                    //Check that the bound content is there
                    lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(literal + "2", lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindHtmlContent_HtmlFragmentWithEntitiesValue()
        {

            var literal = "Inner&ndash;Bound&nbsp;Content";
            var unencoded = "Inner-Bound Content";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' style='border: solid 1px blue; margin: var(--marginsize) ' >" + literal + "</div>" +
                "<div id='" + id + "2' style='border: solid 1px blue;  margin: calc(--marginsize + 5pt)' >" + literal + "2</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-type='text/html'>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["boundContent"] = contentString;
                doc.Params["literal"] = literal;
                doc.Params["--marginsize"] = Unit.Pt(10);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_EntitiesBind.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);

                    var fragment = wrapper.Contents[0] as HTMLFragmentWrapper; //content is put in an invisible fragment
                    Assert.IsNotNull(fragment);
                    Assert.AreEqual(2, fragment.Content.Count);

                    var content = fragment.Content[0] as Div;

                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);
                    Assert.AreEqual(10, content.Margins.Top);

                    //Check that the bound content is there
                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(unencoded, lit.Text);


                    content = fragment.Content[1] as Div;

                    Assert.AreEqual(id + "2", content.ID);
                    Assert.AreEqual(1, content.Contents.Count);
                    Assert.AreEqual(10 + 5, content.Margins.Top);
                    //Check that the bound content is there
                    lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual(unencoded + "2", lit.Text);

                }
            }
        }

        

        

        /// <summary>
        /// The reverse of the above the content should be just treated as text.
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_IgnoreWhitespaceTags()
        {
            var literal = "Inner Content";
            var id = "Appended";
            //The returns and tabs are not signficant. So the div should all be on one line.
            var contentString = @"<div class='test' style='border: solid 1px red' id='" + id + @"' xmlns='http://www.w3.org/1999/xhtml' >Before
                <b>" + literal + @"</b>
                After</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red; white-space:preserve'>
            <div data-content='{{boundContent}}' ></div>
            <div>After Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_IgnoreWhitespace.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.Contents[0] as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var content = wrapper.Contents[0] as TextLiteral; //pre-pended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(contentString, content.Text);


                    var div = wrapper.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                }
            }
        }
    }
}
