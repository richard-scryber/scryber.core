using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

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
    }
}
