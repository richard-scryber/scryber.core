using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Html.Components;

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
        public void BindContent_IgnoreTags()
        {
            var literal = "Inner Content";
            var id = "Appended";
            var contentString = @"<div class='test' id='" + id + "' xmlns='http://www.w3.org/1999/xhtml' ><b>" + literal + "</b></div>";

            var src = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
    </head>
    <body style='padding: 10pt' >
        <div id='wrapper' style='border: solid 1px red; white-space:preserve'>
            {{boundContent}}
            <div>After Content</div>
        </div>
    </body>
</html>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                doc.Params["boundContent"] = contentString;

                using (var stream = DocStreams.GetOutputStream("HtmlBoundContent_IgnoreTags.pdf"))
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
