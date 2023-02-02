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
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
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



        /// <summary>
        /// A pair of nested divs just fit onto a single layout page for the section
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockNoOverflow()
        {
            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div wrapper = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);


            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 2, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is ok the remaining space on the page
            Div second = new Div() { Height = 100, BorderWidth = 3, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Still on the first page as it fits"));
            wrapper.Contents.Add(second);

            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockNoOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            Assert.AreEqual(1, firstpage.ContentBlock.Columns.Length);
            Assert.AreEqual(1, firstpage.ContentBlock.Columns[0].Contents.Count);

            var wrapperBlock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(2, wrapperBlock.Columns[0].Contents.Count);

            var firstblock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);



            //Check that the block has not overflowed
            PDFLayoutBlock secondblock = wrapperBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, secondblock.TotalBounds.Y);
            Assert.AreEqual(100, secondblock.Height);
            Assert.AreEqual(second, secondblock.Owner);
        }


        /// <summary>
        /// A nested div will force a new page layout within the section.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockOverflow()
        {


            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            Div wrapper = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page due to height"));
            wrapper.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            Assert.AreEqual(1, firstpage.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock1 = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, wrapperBlock1.Height);

            //First wrapper block with the top as content
            Assert.AreEqual(1, wrapperBlock1.Columns[0].Contents.Count);
            Assert.AreEqual(wrapper, wrapperBlock1.Owner);
            Assert.AreEqual(0, wrapperBlock1.BlockRepeatIndex);

            var topblock = wrapperBlock1.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, topblock.Owner);
            Assert.AreEqual(PageHeight - 100, topblock.Height);


            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];
            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);


            //Second wrapper block with the tooverflow as content
            Assert.AreEqual(1, lastpage.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock2 = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(150, wrapperBlock2.Height);
            Assert.AreEqual(wrapper, wrapperBlock2.Owner);
            Assert.AreEqual(0.0, wrapperBlock2.OffsetY);
            Assert.AreEqual(1, wrapperBlock2.BlockRepeatIndex);
            Assert.AreEqual(1, wrapperBlock2.Columns[0].Contents.Count);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = wrapperBlock2.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tooverflow, overflowedblock.Owner);
            Assert.AreEqual(0.0, overflowedblock.OffsetY);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockBannedOverflow()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            //Block any overflow
            section.OverflowAction = Drawing.OverflowAction.None;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);

            var wrapper = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 2, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is too big for the remaining space on the page
            Div tooverflow = new Div() { Height = 150, BorderWidth = 3, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Cannot overflow, so not shown"));
            wrapper.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockBannedOverflow.pdf"))
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
            var wrapperBlock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

            //wrapper should only have 1 child block as other could not be fitted
            Assert.AreEqual(1, wrapperBlock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 100, wrapperBlock.Height);
            Assert.AreEqual(wrapper, wrapperBlock.Owner);
            Assert.AreEqual(0.0, wrapperBlock.OffsetY);

            //Check the topblock owner and dimensions
            PDFLayoutBlock topBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, topBlock.Owner);
            Assert.AreEqual(0, topBlock.OffsetY);
            Assert.AreEqual(PageHeight - 100, topBlock.Height);


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockClippedOverflow()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);

            Section section = new Section();
            section.FontSize = 20;
            section.OverflowAction = Drawing.OverflowAction.Clip;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            doc.Pages.Add(section);


            Div wrapper = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is too big for the remaining space on the page - but should be clipped
            Div tooverflow = new Div() { Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            tooverflow.Contents.Add(new TextLiteral("Clipped overflow, so shown beyond the page"));
            wrapper.Contents.Add(tooverflow);

            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockClippedOverflow.pdf"))
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
            PDFLayoutBlock wrapperBlock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(wrapper, wrapperBlock.Owner);
            Assert.AreEqual(0, wrapperBlock.BlockRepeatIndex);
            Assert.AreEqual(2, wrapperBlock.Columns[0].Contents.Count);
            Assert.AreEqual(0, wrapperBlock.OffsetY);
            Assert.AreEqual(PageWidth, wrapperBlock.Width);
            Assert.AreEqual(PageHeight + 50, wrapperBlock.Height);

            var firstBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, firstBlock.Owner);
            Assert.AreEqual(0, firstBlock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstBlock.Height);


            //Check that the block is clipped an still on the page
            PDFLayoutBlock overflowedblock = wrapperBlock.Columns[0].Contents[1] as PDFLayoutBlock;

            Assert.AreEqual(PageHeight - 100, overflowedblock.TotalBounds.Y);
            Assert.AreEqual(150, overflowedblock.Height);
        }


        /// <summary>
        /// Margins affect the outer total size
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockOverflowWithMargins()
        {


            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 12;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;


            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Green,
                Margins = new Drawing.Thickness(5)
            };
            section.Contents.Add(wrapper);

            Div top = new Div()
            {
                Height = PageHeight - 100,
                Margins = new Drawing.Thickness(5),
                BorderWidth = 2,
                BorderColor = Drawing.StandardColors.Red
            };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div()
            {
                Height = 100,
                Margins = new Drawing.Thickness(5),
                BorderWidth = 3,
                BorderColor = Drawing.StandardColors.Blue
            };

            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page due to margins"));
            wrapper.Contents.Add(tooverflow);



            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockOverflowWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);



            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(1, firstpage.ContentBlock.Columns[0].Contents.Count);

            PDFLayoutBlock wrapperBlock1 = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(wrapper, wrapperBlock1.Owner);
            Assert.AreEqual(0, wrapperBlock1.BlockRepeatIndex);
            Assert.AreEqual(1, wrapperBlock1.Columns[0].Contents.Count);
            Assert.AreEqual(0, wrapperBlock1.OffsetY);
            Assert.AreEqual(PageWidth, wrapperBlock1.Width);
            Assert.AreEqual(PageHeight - 80, wrapperBlock1.Height); //margins on wrapper + margins on top block = 20



            var firstBlock = wrapperBlock1.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, firstBlock.Owner);
            Assert.AreEqual(0, firstBlock.TotalBounds.Y);
            Assert.AreEqual(PageWidth - 10, firstBlock.Width); //margins on wrapper
            Assert.AreEqual(PageHeight - 90, firstBlock.Height); //margins on top block = 10



            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);

            PDFLayoutBlock wrapperBlock2 = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(wrapper, wrapperBlock2.Owner);
            Assert.AreEqual(1, wrapperBlock2.BlockRepeatIndex);
            Assert.AreEqual(1, wrapperBlock2.Columns[0].Contents.Count);
            Assert.AreEqual(0, wrapperBlock2.OffsetY);
            Assert.AreEqual(PageWidth, wrapperBlock2.Width);
            Assert.AreEqual(120, wrapperBlock2.Height); //margins on wrapper + margins on overflowed block = 20

            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = wrapperBlock2.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.OffsetY);
            Assert.AreEqual(110, overflowedblock.Height); //margins on the overflowed block
            Assert.AreEqual(PageWidth - 10, overflowedblock.Width); //margins on wrapper

        }


        /// <summary>
        /// Padding affects the inner available space
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedBlockOverflowWithPadding()
        {


            const int PageWidth = 200;
            const int PageHeight = 300;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 12;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;


            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Green,
                Padding = new Drawing.Thickness(5)
            };
            section.Contents.Add(wrapper);

            Div top = new Div()
            {
                Height = PageHeight - 100,
                Padding = new Drawing.Thickness(5),
                BorderWidth = 2,
                BorderColor = Drawing.StandardColors.Red
            };
            top.Contents.Add(new TextLiteral("Sits on the first page"));
            wrapper.Contents.Add(top);

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div()
            {
                Height = 100,
                Padding = new Drawing.Thickness(5),
                BorderWidth = 3,
                BorderColor = Drawing.StandardColors.Blue
            };

            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page due to padding"));
            wrapper.Contents.Add(tooverflow);



            using (var ms = DocStreams.GetOutputStream("Section_NestedBlockOverflowWithPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);



            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(1, firstpage.ContentBlock.Columns[0].Contents.Count);

            PDFLayoutBlock wrapperBlock1 = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(wrapper, wrapperBlock1.Owner);
            Assert.AreEqual(0, wrapperBlock1.BlockRepeatIndex);
            Assert.AreEqual(1, wrapperBlock1.Columns[0].Contents.Count);
            Assert.AreEqual(0, wrapperBlock1.OffsetY);
            Assert.AreEqual(PageWidth, wrapperBlock1.Width);
            Assert.AreEqual(PageHeight - 90, wrapperBlock1.Height); //padding on wrapper + top block = 210



            var firstBlock = wrapperBlock1.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(top, firstBlock.Owner);
            Assert.AreEqual(0, firstBlock.TotalBounds.Y);
            Assert.AreEqual(PageWidth - 10, firstBlock.Width); //padding on wrapper
            Assert.AreEqual(PageHeight - 100, firstBlock.Height); //explicit height (padding inside)



            //Check that the overflowed page has the same dimensions.
            PDFLayoutPage lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);

            PDFLayoutBlock wrapperBlock2 = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(wrapper, wrapperBlock2.Owner);
            Assert.AreEqual(1, wrapperBlock2.BlockRepeatIndex);
            Assert.AreEqual(1, wrapperBlock2.Columns[0].Contents.Count);
            Assert.AreEqual(0, wrapperBlock2.OffsetY);
            Assert.AreEqual(PageWidth, wrapperBlock2.Width);
            Assert.AreEqual(110, wrapperBlock2.Height); //padding on wrapper + explicit content height

            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock overflowedblock = wrapperBlock2.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, overflowedblock.OffsetY);
            Assert.AreEqual(100, overflowedblock.Height); //explicit height
            Assert.AreEqual(PageWidth - 10, overflowedblock.Width); //padding on wrapper

        }

        //
        // Deep nested overflow, single column
        //


        /// <summary>
        /// Padding affects the inner available space
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionNestedDeepOverflowWithText()
        {


            const int PageWidth = 200;
            const int PageHeight = 300;
            const int LineHeight = 15;
            const int Padding = 5;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 12;
            section.TextLeading = LineHeight; //fixed size line height
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;


            doc.Pages.Add(section);

            Div wrapper1 = new Div()
            {
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Green,
                Padding = new Drawing.Thickness(Padding)
            };

            section.Contents.Add(new TextLiteral("Top level Line"));
            section.Contents.Add(wrapper1);


            Div wrapper2 = new Div()
            {
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Blue,
                Padding = new Drawing.Thickness(Padding)
            };
            wrapper1.Contents.Add(new TextLiteral("Inside wrapper 1"));
            wrapper1.Contents.Add(wrapper2);


            Div wrapper3 = new Div()
            {
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                Padding = new Drawing.Thickness(Padding)
            };
            wrapper2.Contents.Add(new TextLiteral("Inside wrapper 2"));
            wrapper2.Contents.Add(wrapper3);

            wrapper3.Contents.Add(new TextLiteral("Inside wrapper 3"));
            Div top = new Div()
            {
                Height = PageHeight - 150,
                Padding = new Drawing.Thickness(Padding),
                BorderWidth = 2,
                BorderColor = Drawing.StandardColors.Fuchsia
            };
            top.Contents.Add(new TextLiteral("Inside top div and 2 wrappers"));
            wrapper3.Contents.Add(top);

            //div is too ok for the remaining space on the page - but the margins push it over
            Div tooverflow = new Div()
            {
                Height = 100,
                Padding = new Drawing.Thickness(Padding),
                BorderWidth = 3,
                BorderColor = Drawing.StandardColors.Blue
            };

            tooverflow.Contents.Add(new TextLiteral("Forced onto on the second page"));
            wrapper3.Contents.Add(tooverflow);

            wrapper3.Contents.Add(new TextLiteral("After the overflow"));

            wrapper2.Contents.Add(new TextLiteral("After wrapper 3"));

            wrapper1.Contents.Add(new TextLiteral("After wrapper 2"));

            section.Contents.Add(new TextLiteral("After wrapper 1"));

            using (var ms = DocStreams.GetOutputStream("Section_NestedDeepOverflowWithText.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(2, layout.AllPages.Count);



            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            Assert.AreEqual(2, firstpage.ContentBlock.Columns[0].Contents.Count);

            //Section level

            var text = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var wrap1Block = firstpage.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;

            var contentWidth = PageWidth;

            Assert.AreEqual(contentWidth, text.AvailableWidth + text.Width);
            Assert.AreEqual("Top level Line", (text.Runs[1] as PDFTextRunCharacter).Characters);
            Assert.AreEqual(LineHeight, text.Height);
            Assert.AreEqual(contentWidth, wrap1Block.Width);
            Assert.AreEqual(LineHeight, wrap1Block.TotalBounds.Y);
            Assert.AreEqual(0, wrap1Block.TotalBounds.X);
            Assert.AreEqual(contentWidth, wrap1Block.TotalBounds.Width);

            Assert.AreEqual(2, wrap1Block.Columns[0].Contents.Count);
            Assert.AreEqual((PageHeight - 150) + (6 * Padding) + (3 * LineHeight), wrap1Block.Height);

            //Wrapper 1 level

            text = wrap1Block.Columns[0].Contents[0] as PDFLayoutLine;
            var wrap2Block = wrap1Block.Columns[0].Contents[1] as PDFLayoutBlock;
            contentWidth -= Padding * 2;

            Assert.AreEqual(contentWidth, text.AvailableWidth + text.Width);
            Assert.AreEqual("Inside wrapper 1", (text.Runs[1] as PDFTextRunCharacter).Characters);
            Assert.AreEqual(LineHeight, text.Height);
            Assert.AreEqual(contentWidth, wrap2Block.Width);
            Assert.AreEqual(LineHeight, wrap2Block.TotalBounds.Y);
            Assert.AreEqual(0, wrap2Block.TotalBounds.X);
            Assert.AreEqual(contentWidth, wrap2Block.TotalBounds.Width);

            Assert.AreEqual(2, wrap2Block.Columns[0].Contents.Count);
            Assert.AreEqual((PageHeight - 150) + (4 * Padding) + (2 * LineHeight), wrap2Block.Height);

            //Wrapper 2 level

            text = wrap2Block.Columns[0].Contents[0] as PDFLayoutLine;
            var wrap3Block = wrap2Block.Columns[0].Contents[1] as PDFLayoutBlock;
            contentWidth -= Padding * 2;

            Assert.AreEqual(contentWidth, text.AvailableWidth + text.Width);
            Assert.AreEqual("Inside wrapper 2", (text.Runs[1] as PDFTextRunCharacter).Characters);
            Assert.AreEqual(LineHeight, text.Height);
            Assert.AreEqual(contentWidth, wrap3Block.Width);
            Assert.AreEqual(LineHeight, wrap3Block.TotalBounds.Y);
            Assert.AreEqual(0, wrap3Block.TotalBounds.X);
            Assert.AreEqual(contentWidth, wrap3Block.TotalBounds.Width);

            Assert.AreEqual(2, wrap3Block.Columns[0].Contents.Count);
            Assert.AreEqual((PageHeight - 150) + (2 * Padding) + (1 * LineHeight), wrap3Block.Height);

            //Wrapper 3 level

            text = wrap3Block.Columns[0].Contents[0] as PDFLayoutLine;
            var topBlock = wrap3Block.Columns[0].Contents[1] as PDFLayoutBlock;
            contentWidth -= Padding * 2;

            Assert.AreEqual(contentWidth, text.AvailableWidth + text.Width);
            Assert.AreEqual("Inside wrapper 3", (text.Runs[1] as PDFTextRunCharacter).Characters);
            Assert.AreEqual(LineHeight, text.Height);
            Assert.AreEqual(contentWidth, topBlock.Width);
            Assert.AreEqual(LineHeight, topBlock.TotalBounds.Y);
            Assert.AreEqual(0, topBlock.TotalBounds.X);
            Assert.AreEqual(contentWidth, topBlock.TotalBounds.Width);

            Assert.AreEqual(1, topBlock.Columns[0].Contents.Count); //Just the text
            Assert.AreEqual(PageHeight - 150, topBlock.Height); //Explicit height



            // page 2 with the overflow nested within the content.

            var lastpage = layout.AllPages[1];

            Assert.AreEqual(PageWidth, lastpage.Width);
            Assert.AreEqual(PageHeight, lastpage.Height);

            Assert.AreEqual(2, lastpage.ContentBlock.Columns[0].Contents.Count);

            wrap1Block = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            text = lastpage.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(100 + (6 * Padding) + (3 * LineHeight), wrap1Block.Height);
            Assert.AreEqual(PageWidth, wrap1Block.Width);
            Assert.AreEqual(wrapper1, wrap1Block.Owner);
            Assert.AreEqual(1, wrap1Block.BlockRepeatIndex);
            Assert.AreEqual(2, wrap1Block.Columns[0].Contents.Count);
            Assert.AreEqual("After wrapper 1", (text.Runs[1] as PDFTextRunCharacter).Characters);

            wrap2Block = wrap1Block.Columns[0].Contents[0] as PDFLayoutBlock;
            text = wrap1Block.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(100 + (4 * Padding) + (2 * LineHeight), wrap2Block.Height);
            Assert.AreEqual(PageWidth - (2 * Padding), wrap2Block.Width);
            Assert.AreEqual(wrapper2, wrap2Block.Owner);
            Assert.AreEqual(1, wrap2Block.BlockRepeatIndex);
            Assert.AreEqual(2, wrap2Block.Columns[0].Contents.Count);
            Assert.AreEqual("After wrapper 2", (text.Runs[1] as PDFTextRunCharacter).Characters);

            wrap3Block = wrap2Block.Columns[0].Contents[0] as PDFLayoutBlock;
            text = wrap2Block.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(100 + (2 * Padding) + (1 * LineHeight), wrap3Block.Height);
            Assert.AreEqual(PageWidth - (4 * Padding), wrap3Block.Width);
            Assert.AreEqual(wrapper3, wrap3Block.Owner);
            Assert.AreEqual(1, wrap3Block.BlockRepeatIndex);
            Assert.AreEqual(2, wrap3Block.Columns[0].Contents.Count);
            Assert.AreEqual("After wrapper 3", (text.Runs[1] as PDFTextRunCharacter).Characters);


            var overflowBlock = wrap3Block.Columns[0].Contents[0] as PDFLayoutBlock;
            text = wrap3Block.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(100 + (0 * Padding) + (0 * LineHeight), overflowBlock.Height);
            Assert.AreEqual(PageWidth - (6 * Padding), overflowBlock.Width);
            Assert.AreEqual(tooverflow, overflowBlock.Owner);
            Assert.AreEqual(0, overflowBlock.BlockRepeatIndex);
            Assert.AreEqual(2, overflowBlock.Columns[0].Contents.Count); //2 lines of content
            Assert.AreEqual("After the overflow", (text.Runs[1] as PDFTextRunCharacter).Characters);


        }


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

            Div wrapper = new Div() { BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top = new Div() { Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top);

            Div second = new Div() { Height = 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
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
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual((PageWidth / 2.0), firstblock.Width);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, secondblock.TotalBounds.Y);
            Assert.AreEqual(100, secondblock.Height);
            Assert.AreEqual((PageWidth / 2.0), secondblock.Width);
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

            Div wrapper = new Div() { ID = "Wrapper", BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top = new Div() { ID = "top", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Moved to the second column as it does not fit"));
            wrapper.Contents.Add(second);



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
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(150, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual(150, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
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
            const int Margins = 10;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                ID = "Wrapper",
                BorderColor = Drawing.StandardColors.Green,
                Margins = new Drawing.Thickness(Margins)
            };
            section.Contents.Add(wrapper);

            Div top = new Div()
            {
                ID = "top",
                Height = PageHeight - 100,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                Margins = new Drawing.Thickness(Margins)
            };
            top.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top);

            //div = 100 + margins, just overflow
            Div second = new Div()
            {
                ID = "second",
                Height = 100, //would fit without margins
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Blue,
                Margins = new Drawing.Thickness(Margins)
            };

            second.Contents.Add(new TextLiteral("Moved to the second column as it does not fit"));
            wrapper.Contents.Add(second);



            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockWithOverflowAndMargins.pdf"))
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
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual((PageHeight - 100) + (4 * Margins), wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual((PageHeight - 100) + (2 * Margins), firstblock.Height);
            Assert.AreEqual(top, firstblock.Owner);
            Assert.AreEqual((PageWidth / 2.0) - (2 * Margins), firstblock.Width); // remove left and right margin for the wrapper

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(100 + (4 * Margins), wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual(100 + (2 * Margins), secondblock.Height);
            Assert.AreEqual((PageWidth / 2.0) - (2 * Margins), secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
            Assert.AreEqual(second, secondblock.Owner);
        }

        /// <summary>
        /// Overflows onto a second column due to available height
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockBannedOverflow()
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
            section.OverflowAction = Drawing.OverflowAction.None; //Block all overflow (onto a new page, not column)

            doc.Pages.Add(section);

            Div wrapper = new Div() { ID = "Wrapper", BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top1 = new Div() { ID = "top1", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top1.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top1);

            Div top2 = new Div() { ID = "top2", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Sits on the second column"));
            wrapper.Contents.Add(top2);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Does not fit, so should not output"));
            wrapper.Contents.Add(second);



            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockBannedOverflow.pdf"))
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
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top1, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count); //make sure we only have 1 child
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
            Assert.AreEqual(top2, secondblock.Owner);
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
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.OverflowAction = Drawing.OverflowAction.Clip; //Clips, so keeps going

            doc.Pages.Add(section);

            Div wrapper = new Div() { ID = "Wrapper", BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top1 = new Div() { ID = "top1", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top1.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top1);

            Div top2 = new Div() { ID = "top2", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Sits on the second column"));
            wrapper.Contents.Add(top2);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Does not fit, but should be clipped and output within the wrapper"));
            wrapper.Contents.Add(second);



            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockClippedOverflow.pdf"))
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
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top1, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(2, wrapperblock.Columns[0].Contents.Count); //make sure we only have 1 child
            Assert.AreEqual((PageHeight - 100) + 150, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
            Assert.AreEqual(top2, secondblock.Owner);

            PDFLayoutBlock overflowBlock = wrapperblock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(PageHeight - 100, overflowBlock.TotalBounds.Y);
            Assert.AreEqual(150, overflowBlock.Height);
            Assert.AreEqual(PageWidth / 2.0, overflowBlock.Width);
            Assert.AreEqual(0, overflowBlock.BlockRepeatIndex);
            Assert.AreEqual(second, overflowBlock.Owner);
        }





        /// <summary>
        /// The section allows overflow, and the last column cannot fit the contents, so should continue onto a new page, preseving padding and margins.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockOverflowNewPage()
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
            section.OverflowAction = Drawing.OverflowAction.NewPage; //New page with 2 columns, so keeps going

            doc.Pages.Add(section);

            Div wrapper = new Div() { ID = "Wrapper", BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top1 = new Div() { ID = "top1", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top1.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top1);

            Div top2 = new Div() { ID = "top2", Height = PageHeight - 100, BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Sits on the second column"));
            wrapper.Contents.Add(top2);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 150, BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Does not fit, so should be pushed to the next column on the next page"));
            wrapper.Contents.Add(second);



            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockNewPageOverflow.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            PDFLayoutBlock wrapperblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, firstblock.Height);
            Assert.AreEqual(top1, firstblock.Owner);
            Assert.AreEqual(PageWidth / 2.0, firstblock.Width);

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count); //make sure we only have 1 child
            Assert.AreEqual(PageHeight - 100, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual(PageHeight - 100, secondblock.Height);
            Assert.AreEqual(PageWidth / 2.0, secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
            Assert.AreEqual(top2, secondblock.Owner);


            PDFLayoutPage lastpage = layout.AllPages[1];
            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            Assert.AreEqual(2, lastpage.ContentBlock.Columns.Length);

            wrapperblock = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(150, wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(2, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);

            //Check that the block has overflowed to 0 y offset on the new page within the wrapper
            PDFLayoutBlock thirdblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, thirdblock.TotalBounds.Y);
            Assert.AreEqual(150, thirdblock.Height);
            Assert.AreEqual(PageWidth / 2.0, thirdblock.Width);
            Assert.AreEqual(0, thirdblock.BlockRepeatIndex);
            Assert.AreEqual(second, thirdblock.Owner);

        }


        /// <summary>
        /// The section allows overflow, and the last column cannot fit the contents, so should continue onto a new page, preseving padding and margins.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnNestedBlockOverflowNewPageWithMargins()
        {

            const int PageWidth = 200;
            const int PageHeight = 300;
            const int Margins = 10;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Style.PageStyle.Width = PageWidth;
            section.Style.PageStyle.Height = PageHeight;
            section.ColumnCount = 2;
            section.AlleyWidth = 0;
            section.OverflowAction = Drawing.OverflowAction.NewPage; //New page with 2 columns, so keeps going

            doc.Pages.Add(section);

            Div wrapper = new Div() { ID = "Wrapper", Margins = new Drawing.Thickness(Margins), BorderColor = Drawing.StandardColors.Green };
            section.Contents.Add(wrapper);

            Div top1 = new Div() { ID = "top1", Height = PageHeight - 100, Margins = new Drawing.Thickness(Margins), BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top1.Contents.Add(new TextLiteral("Sits on the first column"));
            wrapper.Contents.Add(top1);

            Div top2 = new Div() { ID = "top2", Height = PageHeight - 100, Margins = new Drawing.Thickness(Margins), BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            top2.Contents.Add(new TextLiteral("Sits on the second column"));
            wrapper.Contents.Add(top2);

            //div = 100, just overflow
            Div second = new Div() { ID = "second", Height = 150, Margins = new Drawing.Thickness(Margins), BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            second.Contents.Add(new TextLiteral("Does not fit, so should be pushed to the next column on the next page"));
            wrapper.Contents.Add(second);



            using (var ms = DocStreams.GetOutputStream("Section_NestedColumnBlockNewPageOverflowWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);


            //Check that the first page has the same dimensions.
            PDFLayoutPage firstpage = layout.AllPages[0];

            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);

            PDFLayoutBlock wrapperblock = firstpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual((PageHeight - 100) + (4 * Margins), wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(0, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            PDFLayoutBlock firstblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, firstblock.TotalBounds.Y);
            Assert.AreEqual((PageHeight - 100) + (2 * Margins), firstblock.Height);
            Assert.AreEqual(top1, firstblock.Owner);
            Assert.AreEqual((PageWidth / 2.0) - (2 * Margins), firstblock.Width);

            //second column

            wrapperblock = firstpage.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count); //make sure we only have 1 child
            Assert.AreEqual((PageHeight - 100) + (4 * Margins), wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(1, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);


            //Check that the block has overflowed to 0 y offset
            PDFLayoutBlock secondblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, secondblock.TotalBounds.Y);
            Assert.AreEqual((PageHeight - 100) + (2 * Margins), secondblock.Height);
            Assert.AreEqual((PageWidth / 2.0) - (2 * Margins), secondblock.Width);
            Assert.AreEqual(0, secondblock.BlockRepeatIndex);
            Assert.AreEqual(top2, secondblock.Owner);

            //Second page

            PDFLayoutPage lastpage = layout.AllPages[1];
            Assert.AreEqual(PageWidth, firstpage.Width);
            Assert.AreEqual(PageHeight, firstpage.Height);
            Assert.AreEqual(2, lastpage.ContentBlock.Columns.Length);

            wrapperblock = lastpage.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(1, wrapperblock.Columns[0].Contents.Count);
            Assert.AreEqual(150 + (4 * Margins), wrapperblock.Height);
            Assert.AreEqual(PageWidth / 2.0, wrapperblock.Width);
            Assert.AreEqual(2, wrapperblock.BlockRepeatIndex);
            Assert.AreEqual(wrapper, wrapperblock.Owner);

            //Check that the block has overflowed to 0 y offset on the new page within the wrapper
            PDFLayoutBlock thirdblock = wrapperblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0.0, thirdblock.TotalBounds.Y);
            Assert.AreEqual(150 + (2 * Margins), thirdblock.Height);
            Assert.AreEqual((PageWidth / 2.0) - (2 * Margins), thirdblock.Width);
            Assert.AreEqual(0, thirdblock.BlockRepeatIndex);
            Assert.AreEqual(second, thirdblock.Owner);

        }


        /// <summary>
        /// The section allows flows over 2 columns but a parent div has a overflow split of any, so the inner contents flow onto the next column
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockOverflowAnySplit()
        {
            var action = Scryber.Drawing.OverflowSplit.Any;

            Assert.Inconclusive();
        }

        /// <summary>
        /// The section allows flows over 2 columns but a parent div has a overflow split of Never, so moves as a block, rather than the inner contents
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void SectionColumnBlockOverflowKeepTogether()
        {
            var action = Scryber.Drawing.OverflowSplit.Never;

            Assert.Inconclusive();
        }

    }
}
