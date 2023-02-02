using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class HeaderFooter_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionHeader()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is ok the remaining space on the page
            Div second = new Div() { Height = 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Still on the first page as it fits"));
            section.Contents.Add(second);

            using (var ms = DocStreams.GetOutputStream("Section_BlockNoOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            Assert.Inconclusive();

        }


    }
}
