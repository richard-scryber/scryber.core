using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;

using Scryber.Layout;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class HtmlParsing_Test
    {
        

        [TestMethod()]
        public void DocumentParsing()
        {
            var src = @"<html xmlns='http://www.w3.org/1999/xhtml' >
                            <head>
                                <title>This is the title</title>
                            </head>
                            <body class='strong' style='margin:20px' >
                                <p>This is a paragraph of content</p>
                            </body>
                        </html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = PDFDocument.ParseDocument(sr, ParseSourceType.DynamicContent);
                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                doc.ProcessDocument("C:\\Temp\\Html.pdf", System.IO.FileMode.Create);
            }
        }

        

        
    }
}
