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
    public class Overflow_Tests
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
        public void SectionBlockNoOverflow()
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


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            PDFLayoutBlock firstblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight-100, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);



            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = firstpage.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, secondblock.TotalBounds.Y);
            Assert.AreEqual(100, secondblock.Height);
            Assert.AreEqual(second, secondblock.Owner);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionBlockOverflow()
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

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page due to height"));
            section.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_BlockOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2,layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            

            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);
            
            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionBlockBannedOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.OverflowAction = Drawing.OverflowAction.None;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Cannot overflow, so not shown"));
            section.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_BlockBannedOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(1, firstpage.ContentBlock.Columns[0].Contents.Count);
            PDFLayoutBlock firstBlock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, firstBlock.Owner);
            Assert.AreEqual(0, firstBlock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstBlock.Height);

            
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionBlockClippedOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.OverflowAction = Drawing.OverflowAction.Clip;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Clipped overflow, so shown beyond the page"));
            section.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_BlockClippedOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(2, firstpage.ContentBlock.Columns[0].Contents.Count);
            PDFLayoutBlock firstBlock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, firstBlock.Owner);
            Assert.AreEqual(0, firstBlock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstBlock.Height);


            //Check that the block is clipped an still on the page
            PDFLayoutBlock overflowedblock = firstpage.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionBlockOverflowWithPadding()
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

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div() { Height = 100, Margins = new Drawing.Thickness(10), BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page due to margins"));

            section.Contents.Add(tooverflow);
            

            using (var ms = DocStreams.GetOutputStream("Section_BlockOverflowWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);

            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(120, overflowedblock.Height);
        }



        //Multi-column


        /// <summary>
        /// Just about all fits on the first column
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockNoOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(top);

            //div is ok the remaining space on the page
            Div second = new Div() { Height = 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Still on the first column as it fits"));
            section.Contents.Add(second);

            using (var ms = DocStreams.GetOutputStream("Section_ColumnBlockNoOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            PDFLayoutBlock firstblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = firstpage.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, secondblock.TotalBounds.Y);
            Assert.AreEqual(100, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(second, secondblock.Owner);
        }


        /// <summary>
        /// Overflows onto a second column due to available height
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second column due to height"));
            section.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_ColumnBlockOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(2, firstpage.ContentBlock.Columns.Length);

            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);
            Assert.AreEqual(tooverflow.Height, secondColumn.Height);

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(1, secondColumn.Contents.Count);
            PDFLayoutBlock overflowedblock = secondColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }

        /// <summary>
        /// The padding will force the overflow.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockOverflowWithMargins()
        {
            

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { ID = "TopDiv", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div() { ID ="ToOverflow", Height = 100, Margins = new Drawing.Thickness(10), BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second column due to margins"));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_ColumnBlockOverflowWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var firstpage = layout.AllPages[0];

            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);
            Assert.AreEqual(tooverflow.Height + (10 * 2), secondColumn.Height);

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(1, secondColumn.Contents.Count);
            PDFLayoutBlock overflowedblock = secondColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(100 + (10 * 2), overflowedblock.Height);
        }

        /// <summary>
        /// The section does not overflow, and is clipped, but the second div should still flow onto the second column
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockNoOverflowWithClipping()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            //set this a clipped so we keep going.
            section.OverflowAction = Drawing.OverflowAction.Clip;

            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(top);

            

            //div is too big for the remaining space on the column - but, even though we are clipped it continues onto the next column
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Pushed onto the second column."));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_BlockNoOverflowWithClipping.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);

            Assert.AreEqual(tooverflow.Height, secondColumn.Height);

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(1, secondColumn.Contents.Count);
            PDFLayoutBlock overflowedblock = secondColumn.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        /// <summary>
        /// As the section does not overflow, and is clipped the last column (and only the last column) should continue past the end of the page
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockOverflowWithClipping()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            //set this a clipped so we keep going.
            section.OverflowAction = Drawing.OverflowAction.Clip;

            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(top);

            Div top2 = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Pushed to the second column"));
            section.Contents.Add(top2);

            //div is too big for the remaining space on the page - but we are clipped so continues
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Layout below as we cannot overflow but we are clipped."));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_BlockOverflowWithClipping.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);

            //TODO: The height of the column should either be page height or height of both components
            //At the moment the height is just top2.Height. However it works - check on single column and match that.

            //Assert.AreEqual(tooverflow.Height + top2.Height, secondColumn.Height); //clipped so the height is full

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(2, secondColumn.Contents.Count);
            PDFLayoutBlock top2block = secondColumn.Contents[0] as PDFLayoutBlock;
            PDFLayoutBlock overflowedblock = secondColumn.Contents[1] as PDFLayoutBlock;

            Assert.AreEqual(PageHeight - 100, top2block.Height);
            Assert.AreEqual(PageHeight - 100, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }

        //
        // Deep nested overflow, single column
        //


        //
        // Deep nested overflow, Multi column
        //


        /// <summary>
        /// Just about all fits on the first column
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockNoOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            doc.Pages.Add(section);

            Div wrapper = new Div() { BorderColor = Drawing.StandardColors.Green, Padding = new Drawing.Thickness(10) };
            section.Contents.Add(wrapper);

            Div top = new Div() { Height = PageHeight - 80, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top);

            //div = 60, available = 80, wrapper padding = 2 * 10
            Div second = new Div() { Height = 60, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Still on the first column as it fits"));
            wrapper.Contents.Add(second);

            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockNoOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            PDFLayoutBlock wrapperblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(2, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);

            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 80, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual((PageWidth / 2.0) - 20.0, firstblock.Width);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 80, secondblock.TotalBounds.Y);
            Assert.AreEqual(60, secondblock.Height);
            Assert.AreEqual((PageWidth / 2.0) - 20.0, secondblock.Width);
            Assert.AreEqual(second, secondblock.Owner);
        }


        /// <summary>
        /// Overflows onto a second column due to available height
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockWithOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            doc.Pages.Add(section);

            Div wrapper = new Div() { ID = "Wrapper", BorderColor = Drawing.StandardColors.Green, Padding = new Drawing.Thickness(10) };
            section.Contents.Add(wrapper);

            Div top = new Div() { ID = "top", Height = PageHeight - 80, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Moved to the second column as it does not fit"));
            wrapper.Contents.Add(second);

            Assert.Inconclusive("This fails on the layout, by closing the parent block, when it should not");

            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockWithOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            PDFLayoutBlock wrapperblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(2, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 60, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);

            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 80, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 80, secondblock.TotalBounds.Y);
            Assert.AreEqual(60, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(second, secondblock.Owner);
        }

        /// <summary>
        /// The padding will force the overflow.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockOverflowWithMargins()
        {


            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div top = new Div() { ID = "TopDiv", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            section.Contents.Add(top);

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div() { ID = "ToOverflow", Height = 100, Margins = new Drawing.Thickness(10), BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second column due to margins"));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_ColumnBlockOverflowWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var firstpage = layout.AllPages[0];

            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);
            Assert.AreEqual(tooverflow.Height + (10 * 2), secondColumn.Height);

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(1, secondColumn.Contents.Count);
            PDFLayoutBlock overflowedblock = secondColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(100 + (10 * 2), overflowedblock.Height);
        }

        /// <summary>
        /// The section does not overflow, and is clipped, but the second div should still flow onto the second column
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockNoOverflowWithClipping()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            //set this a clipped so we keep going.
            section.OverflowAction = Drawing.OverflowAction.Clip;

            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(top);



            //div is too big for the remaining space on the column - but, even though we are clipped it continues onto the next column
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Pushed onto the second column."));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_BlockNoOverflowWithClipping.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);

            Assert.AreEqual(tooverflow.Height, secondColumn.Height);

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(1, secondColumn.Contents.Count);
            PDFLayoutBlock overflowedblock = secondColumn.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        /// <summary>
        /// As the section does not overflow, and is clipped the last column (and only the last column) should continue past the end of the page
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockOverflowWithClipping()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;

            //set this a clipped so we keep going.
            section.OverflowAction = Drawing.OverflowAction.Clip;

            doc.Pages.Add(section);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            section.Contents.Add(top);

            Div top2 = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Pushed to the second column"));
            section.Contents.Add(top2);

            //div is too big for the remaining space on the page - but we are clipped so continues
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Layout below as we cannot overflow but we are clipped."));

            section.Contents.Add(tooverflow);


            using (var ms = DocStreams.GetOutputStream("Section_BlockOverflowWithClipping.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);


            //Check that the first page has the same dimensions.
            var firstColumn = firstpage.ContentBlock.Columns[0];
            var secondColumn = firstpage.ContentBlock.Columns[1];

            Assert.AreEqual(PageWidth / 2.0, firstColumn.Width);
            Assert.AreEqual(PageHeight - 100, firstColumn.Height); //fit to the height of the contents

            Assert.AreEqual(PageWidth / 2.0, secondColumn.Width);

            //TODO: The height of the column should either be page height or height of both components
            //At the moment the height is just top2.Height. However it works - check on single column and match that.

            //Assert.AreEqual(tooverflow.Height + top2.Height, secondColumn.Height); //clipped so the height is full

            Assert.AreEqual(1, firstColumn.Contents.Count);
            PDFLayoutBlock firstblock = firstColumn.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);

            //Check that the block has overflowed to 0 y offset
            Assert.AreEqual(2, secondColumn.Contents.Count);
            PDFLayoutBlock top2block = secondColumn.Contents[0] as PDFLayoutBlock;
            PDFLayoutBlock overflowedblock = secondColumn.Contents[1] as PDFLayoutBlock;

            Assert.AreEqual(PageHeight - 100, top2block.Height);
            Assert.AreEqual(PageHeight - 100, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        /// <summary>
        /// The section allows overflow, and the last column cannot fit the contents, so should continue onto a new page, preseving padding and margins.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockOverflowNewPage()
        {
        }

    }
}
