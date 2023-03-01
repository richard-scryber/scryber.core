using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests the float left and right positioning
    /// </summary>
    [TestClass()]
    public class PositionedFloating_Tests
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
        public void FloatLeftRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div floating = new Div() {
                Height = new Unit(20, PageUnits.Percent),
                Width = new Unit(20, PageUnits.Percent),
                BorderWidth = 1, BorderColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left
            };

            floating.Contents.Add(new TextLiteral("50% width and height"));
            section.Contents.Add(floating);

            

            using (var ms = DocStreams.GetOutputStream("PositionedFloat_LeftToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive();
        }


    }
}
