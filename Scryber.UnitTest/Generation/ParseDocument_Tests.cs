using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{

    /// <summary>
    /// Tests each of the static ParseDocument, ParseHtml and Parse methods on the Scryber.Components.Document class
    /// </summary>
    [TestClass()]
    public class ParseDocument_Tests
    {

        [TestMethod()]
        [TestCategory("Document")]
        public void ParseLocalXHTMLFile()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/HelloWorld.xhtml");

            using (Document doc = Document.ParseDocument(path))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_LocalXhtml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(2, pg.Contents.Count);

                //first div has a bound literal
                var div = pg.Contents[0] as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.Contents[1] as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("&lt;Inside&gt;", lit.Text);
                
            }


        }

        
    }
}
