using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// The indent of html lists, as css - the items are indented by the padding of the list (40px, 30pt by default),
    /// and the marker is in that padding. Checked against Chrome.
    /// </summary>
    [TestClass()]
    public class ListIndentParity_Tests
    {
        const string TestCategoryName = "Layout Lists";

        private const string Html = @"<html xmlns='http://www.w3.org/1999/xhtml'>
<head><style>
@page { size: 400pt 400pt; margin: 0; } html{width:400pt} body { margin: 0; padding: 10pt; font-family: Helvetica, sans-serif; font-size: 12pt; }
ul, ol, dl { margin: 0 0 10pt 0; }
</style></head>
<body>
<ul id='ul'><li id='ul1'>disc item</li><li id='ul2'>second disc item<ul id='ul2n'><li id='ul2n1'>nested item</li></ul></li></ul>
<ol id='ol'><li id='ol1'>first number</li><li>2</li><li>3</li><li>4</li><li>5</li><li>6</li><li>7</li><li>8</li><li>9</li><li id='ol10'>tenth number</li></ol>
<ul id='pad0' style='padding-left: 0'><li id='pad0li'>padding-left 0</li></ul>
<ul id='pad15' style='padding-left: 15pt'><li id='pad15li'>padding-left 15pt</li></ul>
<ul id='none' style='list-style: none'><li id='noneli'>no marker</li></ul>
<dl id='dl'><dt id='dt'>term</dt><dd id='dd'>definition</dd></dl>
</body>
</html>
";

        private static void AssertBounds(Document doc, string id, double x, double width)
        {
            var c = doc.FindAComponentById(id) as Component;
            Assert.IsNotNull(c, "No component " + id);
            var arrange = c.GetFirstArrangement();
            Assert.IsNotNull(arrange, "No arrangement for " + id);
            var bounds = arrange.RenderBounds;
            Assert.AreEqual(x, bounds.X.PointsValue, 0.5, id + " x was " + bounds.X.PointsValue);
            Assert.AreEqual(width, bounds.Width.PointsValue, 0.5, id + " width was " + bounds.Width.PointsValue);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ListIndent_01_AsChrome()
        {
            var doc = Document.ParseDocument(new StringReader(Html), ParseSourceType.DynamicContent);
            using (var ms = DocStreams.GetOutputStream("ListIndent_01_AsChrome.pdf"))
            {
                doc.SaveAsPDF(ms);
            }

            //the list is the full width, and the items are inside its padding
            AssertBounds(doc, "ul", 10, 380);
            AssertBounds(doc, "ul1", 40, 350);
            AssertBounds(doc, "ul2", 40, 350);

            //a nested list is indented by its own padding
            AssertBounds(doc, "ul2n", 40, 350);
            AssertBounds(doc, "ul2n1", 70, 320);

            //numbers (including the wider 10.) do not change the indent
            AssertBounds(doc, "ol", 10, 380);
            AssertBounds(doc, "ol1", 40, 350);
            AssertBounds(doc, "ol10", 40, 350);

            //the padding of the list is the indent
            AssertBounds(doc, "pad0", 10, 380);
            AssertBounds(doc, "pad0li", 10, 380);
            AssertBounds(doc, "pad15", 10, 380);
            AssertBounds(doc, "pad15li", 25, 365);

            //without markers the list keeps its padding
            AssertBounds(doc, "none", 10, 380);
            AssertBounds(doc, "noneli", 40, 350);

            //a definition is indented 40px (30pt)
            AssertBounds(doc, "dl", 10, 380);
            AssertBounds(doc, "dt", 10, 380);
            AssertBounds(doc, "dd", 40, 350);
        }
    }
}
