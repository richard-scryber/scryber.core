using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Html.CSSParsing
{
    /// <summary>
    /// A percentage border-radius is of the width of the box (or the container if the box has no width),
    /// and a radius is never larger than the box, so a 50% radius on a square is a circle.
    /// </summary>
    [TestClass]
    public class CSSBorderRadius_Tests
    {
        private static string Render(string style)
        {
            var src = "<html xmlns='http://www.w3.org/1999/xhtml'><head><style>@page{size:400pt 300pt;margin:0} body{margin:0}</style></head>" +
                      "<body><div style='margin:10pt;height:72pt;background-color:#446688;" + style + "'></div></body></html>";
            using (var doc = Document.ParseDocument(new StringReader(src), ParseSourceType.DynamicContent))
            {
                var ms = new MemoryStream();
                doc.SaveAsPDF(ms);
                return PdfContent.Decompressed(ms.ToArray());
            }
        }

        [TestMethod]
        public void Percent_OfTheBoxWidth_WhenItHasAWidth()
        {
            Assert.AreEqual(Render("width:72pt;border-radius:36pt"), Render("width:72pt;border-radius:50%"));
            Assert.AreEqual(Render("width:72pt;border-radius:18pt"), Render("width:72pt;border-radius:25%"));
            Assert.AreNotEqual(Render("width:72pt;border-radius:36pt"), Render("width:72pt;border-radius:25%"));
        }

        [TestMethod]
        public void Percent_OfTheBoxWidth_PerCorner()
        {
            Assert.AreEqual(Render("width:80pt;border-top-left-radius:20pt;border-bottom-right-radius:10pt"),
                            Render("width:80pt;border-top-left-radius:25%;border-bottom-right-radius:12.5%"));
        }

        [TestMethod]
        public void Percent_WithNoWidth_IsLimitedToTheBox()
        {
            //50% of the container width (380pt) is more than the height, so the radius is limited to half the height
            Assert.AreEqual(Render("border-radius:36pt"), Render("border-radius:50%"));
        }

        [TestMethod]
        public void Radius_LargerThanTheBox_IsLimited()
        {
            Assert.AreEqual(Render("width:72pt;border-radius:36pt"), Render("width:72pt;border-radius:500pt"));
            Assert.AreEqual(Render("width:72pt;border-radius:36pt"), Render("width:72pt;border-radius:50%;box-sizing:border-box"));
        }

        [TestMethod]
        public void Radius_Fixed_IsUnchanged()
        {
            Assert.AreNotEqual(Render("width:72pt;border-radius:10pt"), Render("width:72pt;border-radius:20pt"));
            Assert.AreNotEqual(Render("width:72pt"), Render("width:72pt;border-radius:10pt"));
        }

        [TestMethod]
        public void Radius_OnlyReducedOnTheSidesThatNeedIt()
        {
            //20pt each corner of a 72pt wide box needs no reduction
            Assert.AreEqual(Render("width:72pt;border-radius:20pt"), Render("width:72pt;border-top-left-radius:20pt;border-top-right-radius:20pt;border-bottom-left-radius:20pt;border-bottom-right-radius:20pt"));
        }
    }
}
