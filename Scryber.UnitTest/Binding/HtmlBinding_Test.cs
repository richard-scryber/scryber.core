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

        //Capture document layouts.

        public PDFLayoutDocument DocumentLayout { get; set; }

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.DocumentLayout = args.Context.GetLayout<PDFLayoutDocument>();
        }

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
        <div id='div1' class='wrapper' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>

        <h2>2. Multiple Line, not formatted</h2>
        <div id='div2' class='wrapper' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>3. Single Line with spans, not formatted</h2>
        <div id='div3' class='wrapper' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>

        <h2>4. Multiple Line with spans, not formatted</h2>
        <div id='div4' class='wrapper' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;&lt;span&gt;That flows onto multiple lines.&lt;/span&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

        <h2>5. All on one line, pre-formatted</h2>
        <div id='div5' class='wrapper preformatted' >&lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;</div>

        <h2>6. Single separate line pre-formatted</h2>
        <div id='div6' class='wrapper preformatted' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>

        <h2>7. Multiple Line, pre-formatted</h2>
        <div id='div7' class='wrapper preformatted' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>8. Single Line with spans, pre-formatted</h2>
        <div id='div8' class='wrapper preformatted' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>
        <h2>9. Multiple Line with spans, pre-formatted</h2>
        <div id='div9' class='wrapper preformatted' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

    </body>
</html>";

        /// <summary>
        /// No binding, just an initial test to make sure we are parsing all the content - both preformatted and not preformatted
        /// </summary>
        [TestMethod()]
        [TestCategory("Binding")]
        public void BindContent_NoBindingEscapedLayoutTest()
        {
            

            var src = HTMLBindContentLayoutEscaped;

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                
                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_LayoutEscpaped.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    Assert.IsNotNull(this.DocumentLayout);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    //1. Single line (with whitespace before as part of the text)
                    var wrapper = pg.FindAComponentById("div1") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //Just the inner div
                    var literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div data-content='{{model.items[0].content}}'></div>\n        ", literal.Text);

                    //2. Multile lines (with whitespace before as part of the text)
                    wrapper = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n        ", literal.Text);

                    // 3. Whitespace + Span either side of the encoded string
                    wrapper = pg.FindAComponentById("div3") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    var span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    Assert.AreEqual(" <div data-content='{{model.items[0].content}}'></div> ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                    //4. Multiple lines - whitespace, spans either side long text.
                    wrapper = pg.FindAComponentById("div4") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b><span>That flows onto multiple lines.</span>\n            </div>\n            ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                    //5. Preformatted Single line (without whitespace)
                    wrapper = pg.FindAComponentById("div5") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //Just the inner text
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("<div data-content='{{model.items[0].content}}'></div>", literal.Text);

                    //6 Preformatted with the spacing kept.
                    wrapper = pg.FindAComponentById("div6") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div data-content='{{model.items[0].content}}'></div>\n        ", literal.Text);

                    //7 Preformatted multiple lines with spacing kept.
                    wrapper = pg.FindAComponentById("div7") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n        ", literal.Text);

                    //8 Preformatted with whitespace, spans and single line of text
                    wrapper = pg.FindAComponentById("div8") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    //should still be a space before and after
                    Assert.AreEqual(" <div data-content='{{model.items[0].content}}'></div> ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);


                    //9 Preformatted with whitespace, spans and multiple lines of text
                    wrapper = pg.FindAComponentById("div9") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    //should still be the whitespace before, in the middle and after
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n            ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                }
            }
        }

        

        

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_DefaultValue()
        {
            var contentString = @"<div xmlns='http://www.w3.org/1999/xhtml' >Inner Content</div>";

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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);

                    //One div in the bound content.
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    var content = wrapper.Contents[0] as Div;
                    Assert.IsNotNull(content);
                    Assert.AreEqual(1, content.Contents.Count);

                    //One literal in the bound div with 'Inner Content' text.
                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Inner Content", lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_AppendValue()
        {
            var literal = "";
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' >Inner Content</div>";

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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(4, wrapper.Contents.Count);

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Div));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Div)); //This is the appended one.
                    
                    var content = wrapper.Contents[3] as Div;
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Inner Content", lit.Text);

                }
            }
        }

        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXHtmlContent_PrependValue()
        {
            var id = "Appended";
            var contentString = @"<div id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' >Inner Content</div>";

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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(4, wrapper.Contents.Count);

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Div)); //This is the prepended one.
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(Div));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Whitespace));
                    

                    var content = wrapper.Contents[0] as Div; //pre-pended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Inner Content", lit.Text);

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

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //everything goes after binding

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Div)); //This is the prepended one.
                    
                    var content = wrapper.Contents[0] as Div; //replaced
                    Assert.IsNotNull(content);
                    Assert.AreEqual(id, content.ID);
                    Assert.AreEqual(1, content.Contents.Count);

                    var lit = content.Contents[0] as TextLiteral;
                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Inner Content", lit.Text);

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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(4, wrapper.Contents.Count);

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Div));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Div)); //This is the appended one.

                    var content = wrapper.Contents[3] as Div; //appended
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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));

                    var fragment = wrapper.Contents[1] as HTMLFragmentWrapper; //content is put in an invisible fragment
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
            var unencoded = "Inner-Bound" + (char)160 + "Content";
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
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    var fragment = wrapper.Contents[1] as HTMLFragmentWrapper; //content is put in an invisible fragment
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


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindHtmlContent_PlainTextFragment()
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
        <div id='wrapper' style='border: solid 1px red;' data-content='{{boundContent}}' data-content-type='text/plain'>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.Params["boundContent"] = contentString;
                doc.Params["literal"] = literal;
                doc.Params["--marginsize"] = Unit.Pt(10);

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_PlainTextFragment.pdf"))
                {
                    doc.SaveAsPDF(stream);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);
                    

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(2, wrapper.Contents.Count);

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));

                    var txt = wrapper.Contents[1] as TextLiteral;
                    Assert.IsNotNull(txt);
                    Assert.AreEqual(contentString, txt.Text); //This should not be parsed, just a string.
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
            var contentString = @"<div class='test' style='border: solid 1px blue' id='" + id + @"' xmlns='http://www.w3.org/1999/xhtml' >Before
                <b>" + literal + @"</b>
                After</div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red; white-space:preserve'>
            <div id='content' data-content='{{boundContent}}' ></div>
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
                    //Assert.AreEqual(1, pg.Contents.Count);

                    var wrapper = pg.FindAComponentById("wrapper") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);

                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Div)); //This is the bound one.
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Div)); 
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));




                    var content = wrapper.FindAComponentById("content") as Div; //pre-pended
                    Assert.IsNotNull(content);
                    Assert.AreEqual(1, content.Contents.Count);

                    var div = content.Contents[0] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual("test", div.StyleClass);
                    Assert.AreEqual(3, div.Contents.Count);

                    var b = div.Contents[1] as HTMLBoldSpan;
                    Assert.IsNotNull(b);
                    Assert.AreEqual(1, b.Contents.Count);


                    

                    div = wrapper.Contents[3] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(1, div.Contents.Count);

                }
            }
        }
    }
}
