using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Layout;

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass()]
    public class PageLayout_Test
    {

        private const string TestCategory = "Layout";
        private const int PageWidth = 300;
        private const int PageHeight = 500;

        /// <summary>
        /// Holds the last document layout
        /// </summary>
        private PDFLayoutDocument layout;

        /// <summary>
        /// Should be used to set the last layout of the document on rendering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            layout = args.Context.DocumentLayout;
        }

        #region public void SinglePage()

        /// <summary>
        /// Just a single page document - no padding or margins
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void SinglePage()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);

            //Check page size
            PDFLayoutPage one = layout.AllPages[0];
            Assert.AreEqual(PageWidth, one.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, one.Height, "Layout page height was not the expected value");
            Assert.AreEqual(PageWidth, one.Size.Width, "Layout Page size was not the expected value");
            Assert.AreEqual(PageHeight, one.Size.Height, "Layout Page Size was not the expected value");

            //Check page content block size
            PDFLayoutBlock content = one.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");
            
            //Check page content block TotalBounds
            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");
        
            //Check page content block size
            Assert.AreEqual(PageWidth, content.Size.Width, "Layout page content block size width was not the expected value");
            Assert.AreEqual(PageHeight, content.Size.Height, "Layout page content block size width was not the expected value");
            
            //Check page content bloc available size
            Assert.AreEqual(PageWidth, content.AvailableBounds.Width, "Layout page content block available width was not the expected value");
            Assert.AreEqual(PageHeight, content.AvailableBounds.Height, "Layout page content block available height was not the expected value");
            Assert.AreEqual(0, content.AvailableBounds.X, "Layout page content block available X was not the expected value");
            Assert.AreEqual(0, content.AvailableBounds.Y, "Layout page content block available Y was not the expected value");
        }

        #endregion

        #region public void SinglePageWithMargins()

        /// <summary>
        /// Single page document with some margins - to ensure the measurements are retained.
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void SinglePageWithMargins()
        {
            int MarginLeft = 10;
            int MarginRight = 15;
            int MarginTop = 20;
            int MarginBottom = 25;

            Document doc = new Document();

            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Margins.Left = MarginLeft;
            pg.Style.Margins.Right = MarginRight;
            pg.Style.Margins.Top = MarginTop;
            pg.Style.Margins.Bottom = MarginBottom;

            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
                
            }

            Assert.AreEqual(1, layout.AllPages.Count);

            PDFLayoutPage one = layout.AllPages[0];

            Assert.AreEqual(PageWidth, one.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, one.Height, "Layout page height was not the expected value");

            PDFLayoutBlock content = one.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");
            
            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");
            
            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");
       
            int expectedWidth = PageWidth - (MarginLeft + MarginRight);
            int expectedHeight = PageHeight - (MarginTop + MarginBottom);
            int availX = MarginLeft;
            int availY = MarginTop;


            Assert.AreEqual(availX, content.AvailableBounds.X, "Layout page content block Available Bounds X sould be Zero");
            Assert.AreEqual(availY, content.AvailableBounds.Y, "Layout page content block Available Bound Y should be Zero");
            Assert.AreEqual(expectedWidth, content.AvailableBounds.Width, "Layout page content block Available Bounds Width should be expectedWidth");
            Assert.AreEqual(expectedHeight, content.AvailableBounds.Height, "Layout page content block Available Bounds Height should be expectedHeigt");

            PDFLayoutRegion innerContent = content.Columns[0];

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Inner content region does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Inner content region does not reflect the margins");
            Assert.AreEqual(0, innerContent.TotalBounds.X, "Inner content offset X should be Zero");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Inner content offset Y should be Zeor");
        }

        #endregion

        #region public void SinglePageWithMarginsAndPadding()

        /// <summary>
        /// Single page document with both margins and padding - to check the content sizes.
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void SinglePageWithMarginsAndPadding()
        {
            int MarginLeft = 10;
            int MarginRight = 15;
            int MarginTop = 20;
            int MarginBottom = 25;

            int PaddingLeft = 10;
            int PaddingRight = 15;
            int PaddingTop = 20;
            int PaddingBottom = 25;



            Document doc = new Document();

            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            pg.Style.Margins.Left = MarginLeft;
            pg.Style.Margins.Right = MarginRight;
            pg.Style.Margins.Top = MarginTop;
            pg.Style.Margins.Bottom = MarginBottom;

            pg.Style.Padding.Left = PaddingLeft;
            pg.Style.Padding.Right = PaddingRight;
            pg.Style.Padding.Top = PaddingTop;
            pg.Style.Padding.Bottom = PaddingBottom;


            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);

            PDFLayoutPage one = layout.AllPages[0];

            Assert.AreEqual(PageWidth, one.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, one.Height, "Layout page height was not the expected value");

            PDFLayoutBlock content = one.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");

            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");

            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");

            int expectedWidth = PageWidth - (MarginLeft + MarginRight + PaddingLeft + PaddingRight);
            int expectedHeight = PageHeight - (MarginTop + MarginBottom + PaddingTop + PaddingBottom);
            int availX = PaddingLeft + MarginLeft;
            int availY = PaddingTop + MarginTop;

            Assert.AreEqual(availX, content.AvailableBounds.X, "Layout page content block Available Bounds X sould be PaddingLeft");
            Assert.AreEqual(availY, content.AvailableBounds.Y, "Layout page content block Available Bound Y should be PaddingTop");
            Assert.AreEqual(expectedWidth, content.AvailableBounds.Width, "Layout page content block Available Bounds Width should be expectedWidth");
            Assert.AreEqual(expectedHeight, content.AvailableBounds.Height, "Layout page content block Available Bounds Height should be expectedHeigt");

            PDFLayoutRegion innerContent = content.Columns[0];

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Inner content region does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Inner content region does not reflect the margins");
            Assert.AreEqual(0, innerContent.TotalBounds.X, "Inner content offset X should be Zero");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Inner content offset Y should be Zeor");
        }

        #endregion

        #region public void DoublePageWithMarginsAndPadding()

        /// <summary>
        /// 2 page document that ensures all the sizing information flows down onto the second page aswell
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void DoublePageWithMarginsAndPadding()
        {
            int MarginLeft = 10;
            int MarginRight = 15;
            int MarginTop = 20;
            int MarginBottom = 25;

            int PaddingLeft = 10;
            int PaddingRight = 15;
            int PaddingTop = 20;
            int PaddingBottom = 25;



            Document doc = new Document();

            Section pg = new Section();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            pg.Style.Margins.Left = MarginLeft;
            pg.Style.Margins.Right = MarginRight;
            pg.Style.Margins.Top = MarginTop;
            pg.Style.Margins.Bottom = MarginBottom;

            pg.Style.Padding.Left = PaddingLeft;
            pg.Style.Padding.Right = PaddingRight;
            pg.Style.Padding.Top = PaddingTop;
            pg.Style.Padding.Bottom = PaddingBottom;


            doc.Pages.Add(pg);

            PageBreak pgBreak = new PageBreak();
            pg.Contents.Add(pgBreak);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            PDFLayoutPage two = layout.AllPages[1];

            Assert.AreEqual(PageWidth, two.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, two.Height, "Layout page height was not the expected value");

            PDFLayoutBlock content = two.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");

            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");

            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");

            int expectedWidth = PageWidth - (MarginLeft + MarginRight + PaddingLeft + PaddingRight);
            int expectedHeight = PageHeight - (MarginTop + MarginBottom + PaddingTop + PaddingBottom);
            int availX = PaddingLeft + MarginLeft;
            int availY = PaddingTop + MarginTop;

            Assert.AreEqual(availX, content.AvailableBounds.X, "Layout page content block Available Bounds X sould be PaddingLeft");
            Assert.AreEqual(availY, content.AvailableBounds.Y, "Layout page content block Available Bound Y should be PaddingTop");
            Assert.AreEqual(expectedWidth, content.AvailableBounds.Width, "Layout page content block Available Bounds Width should be expectedWidth");
            Assert.AreEqual(expectedHeight, content.AvailableBounds.Height, "Layout page content block Available Bounds Height should be expectedHeigt");

            PDFLayoutRegion innerContent = content.Columns[0];

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Inner content region does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Inner content region does not reflect the margins");
            Assert.AreEqual(0, innerContent.TotalBounds.X, "Inner content offset X should be Zero");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Inner content offset Y should be Zeor");
        }

        

        #endregion

        #region public void TwoColumnPageWithMarginsAndPadding()

        /// <summary>
        /// Single page document with both margins and padding - to check the content sizes.
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void TwoColumnPageWithMarginsAndPadding()
        {
            int MarginLeft = 10;
            int MarginRight = 15;
            int MarginTop = 20;
            int MarginBottom = 25;

            int PaddingLeft = 10;
            int PaddingRight = 15;
            int PaddingTop = 20;
            int PaddingBottom = 25;

            int Alley = 20;
            int ColumnCount = 2;

            Document doc = new Document();

            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            pg.Style.Margins.Left = MarginLeft;
            pg.Style.Margins.Right = MarginRight;
            pg.Style.Margins.Top = MarginTop;
            pg.Style.Margins.Bottom = MarginBottom;

            pg.Style.Padding.Left = PaddingLeft;
            pg.Style.Padding.Right = PaddingRight;
            pg.Style.Padding.Top = PaddingTop;
            pg.Style.Padding.Bottom = PaddingBottom;

            pg.Style.Columns.ColumnCount = 2;
            pg.Style.Columns.AlleyWidth = Alley;

            doc.Pages.Add(pg);

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);

            PDFLayoutPage one = layout.AllPages[0];

            Assert.AreEqual(PageWidth, one.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, one.Height, "Layout page height was not the expected value");

            PDFLayoutBlock content = one.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");

            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");

            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");

            int expectedWidth = PageWidth - (MarginLeft + MarginRight + PaddingLeft + PaddingRight);
            int expectedHeight = PageHeight - (MarginTop + MarginBottom + PaddingTop + PaddingBottom);
            int availX = PaddingLeft + MarginLeft;
            int availY = PaddingTop + MarginTop;

            Assert.AreEqual(availX, content.AvailableBounds.X, "Layout page content block Available Bounds X sould be PaddingLeft");
            Assert.AreEqual(availY, content.AvailableBounds.Y, "Layout page content block Available Bound Y should be PaddingTop");
            Assert.AreEqual(expectedWidth, content.AvailableBounds.Width, "Layout page content block Available Bounds Width should be expectedWidth");
            Assert.AreEqual(expectedHeight, content.AvailableBounds.Height, "Layout page content block Available Bounds Height should be expectedHeigt");

            Assert.AreEqual(ColumnCount, content.Columns.Length);

            //First column
            PDFLayoutRegion innerContent = content.Columns[0];
            expectedWidth = expectedWidth - Alley;
            expectedWidth = expectedWidth / ColumnCount;

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Region 1 Inner content region  does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Region 1 Inner content region does not reflect the margins");
            Assert.AreEqual(0, innerContent.TotalBounds.X, "Region 1 Inner content offset X should be Zero");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Region 1 Inner content offset Y should be Zeor");

            innerContent = content.Columns[1];

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Region 2 Inner content region  does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Region 2 Inner content region does not reflect the margins");
            Assert.AreEqual(expectedWidth + Alley, innerContent.TotalBounds.X, "Region 2 Inner content offset X should be last region width + Alley");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Region 2 Inner content offset Y should be Zeor");
        }

        #endregion

        #region public void TwoColumn_TwoPageWithMarginsAndPadding()

        /// <summary>
        /// Two page document with both margins and padding that has 2 columns - to check 
        /// the content sizes and columns flow onto the next page layout.
        /// </summary>
        [TestCategory(TestCategory)]
        [TestMethod()]
        public void TwoColumn_TwoPageWithMarginsAndPadding()
        {
            int MarginLeft = 10;
            int MarginRight = 15;
            int MarginTop = 20;
            int MarginBottom = 25;

            int PaddingLeft = 10;
            int PaddingRight = 15;
            int PaddingTop = 20;
            int PaddingBottom = 25;

            int Alley = 20;
            int ColumnCount = 2;

            Document doc = new Document();

            Section pg = new Section();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            pg.Style.Margins.Left = MarginLeft;
            pg.Style.Margins.Right = MarginRight;
            pg.Style.Margins.Top = MarginTop;
            pg.Style.Margins.Bottom = MarginBottom;

            pg.Style.Padding.Left = PaddingLeft;
            pg.Style.Padding.Right = PaddingRight;
            pg.Style.Padding.Top = PaddingTop;
            pg.Style.Padding.Bottom = PaddingBottom;

            pg.Style.Columns.ColumnCount = 2;
            pg.Style.Columns.AlleyWidth = Alley;

            doc.Pages.Add(pg);

            pg.Contents.Add(new PageBreak());

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            PDFLayoutPage two = layout.AllPages[1];

            Assert.AreEqual(PageWidth, two.Width, "Layout page width was not the expected value");
            Assert.AreEqual(PageHeight, two.Height, "Layout page height was not the expected value");

            PDFLayoutBlock content = two.ContentBlock;
            Assert.AreEqual(PageWidth, content.Width, "Layout page content block width was not the expected value");
            Assert.AreEqual(PageHeight, content.Height, "Layout page content block height was not the expected value");

            Assert.AreEqual(PageWidth, content.TotalBounds.Width, "Layout page content block bounds width was not the expected value");
            Assert.AreEqual(PageHeight, content.TotalBounds.Height, "Layout page content block bounds width was not the expected value");

            Assert.AreEqual(0, content.TotalBounds.X, "Layout page content block offset X was not the expected value");
            Assert.AreEqual(0, content.TotalBounds.Y, "Layout page content block offset Y was not the expected value");

            int expectedWidth = PageWidth - (MarginLeft + MarginRight + PaddingLeft + PaddingRight);
            int expectedHeight = PageHeight - (MarginTop + MarginBottom + PaddingTop + PaddingBottom);
            int expectedX = PaddingLeft + MarginLeft;
            int expectedY = PaddingTop + MarginTop;

            Assert.AreEqual(expectedX, content.AvailableBounds.X, "Layout page content block Available Bounds X sould be PaddingLeft");
            Assert.AreEqual(expectedY, content.AvailableBounds.Y, "Layout page content block Available Bound Y should be PaddingTop");
            Assert.AreEqual(expectedWidth, content.AvailableBounds.Width, "Layout page content block Available Bounds Width should be expectedWidth");
            Assert.AreEqual(expectedHeight, content.AvailableBounds.Height, "Layout page content block Available Bounds Height should be expectedHeigt");

            Assert.AreEqual(ColumnCount, content.Columns.Length);

            //First column
            PDFLayoutRegion innerContent = content.Columns[0];
            expectedWidth = expectedWidth - Alley;
            expectedWidth = expectedWidth / ColumnCount;
            expectedX = PaddingLeft + MarginLeft;
            expectedY = PaddingTop + MarginTop;

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Region 1 Inner content region  does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Region 1 Inner content region does not reflect the margins");
            Assert.AreEqual(0, innerContent.TotalBounds.X, "Region 1 Inner content offset X should be Zero");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Region 1 Inner content offset Y should be Zero");

            innerContent = content.Columns[1];

            Assert.AreEqual(expectedWidth, innerContent.TotalBounds.Width, "Region 2 Inner content region  does not reflect the margins");
            Assert.AreEqual(expectedHeight, innerContent.TotalBounds.Height, "Region 2 Inner content region does not reflect the margins");
            Assert.AreEqual(expectedWidth + Alley, innerContent.TotalBounds.X, "Region 2 Inner content offset X should be last region width + Alley");
            Assert.AreEqual(0, innerContent.TotalBounds.Y, "Region 2 Inner content offset Y should be Zero");
        }

        #endregion
    }
}
