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
    [TestClass()]
    public class Positioned_Tests
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
        public void BlockRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div() {
                Height = 150, Width = 200, X = 50, Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1, BorderColor = Drawing.StandardColors.Red };
            relative.Contents.Add(new TextLiteral("Sits relative on the first page"));
            section.Contents.Add(relative);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteToPage()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits absolute on the first page"));
            section.Contents.Add(relative);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockExplicitSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 40;
            section.Padding = 10;
            doc.Pages.Add(section);


            section.Contents.Add(new TextLiteral("Before the inline "));


            Div inline = new Div()
            {
                Height = 60,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                TextLeading = 24,
                BorderColor = Drawing.StandardColors.Red,
                OverflowAction = OverflowAction.Clip
                //Margins = new Thickness(5)
            };

            //inline.Contents.Add(new TextLiteral("In the block"));
            section.Contents.Add(inline);

            section.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));


            Div block = new Div()
            {
                BorderColor = Drawing.StandardColors.Blue,
                Padding = 10,

            };
            section.Contents.Add(block);

            block.Contents.Add(new TextLiteral("Before the inline "));


            inline = new Div()
            {
                Height = 60,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                TextLeading = 24,
                BorderColor = Drawing.StandardColors.Red,
                OverflowAction = OverflowAction.Clip
                //Margins = new Thickness(5)
            };

            //inline.Contents.Add(new TextLiteral("In the block"));
            block.Contents.Add(inline);

            block.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));


            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockExplicitSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockBlockSmallExplicitSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 40;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = 10,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                //Margins = new Thickness(5)
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            span.FontBold = true;
            section.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockExplicitSizeSmall.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = 50,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                //Margins = new Thickness(5)
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);
            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            span.FontBold = true;
            section.Contents.Add(span);

            inline = new Div()
            {
                Height = 10,
                Width = 50,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                //Margins = new Thickness(10)
            };

            section.Contents.Add(inline);
            section.Contents.Clear();

            span = new Span();
            //span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line"));
            span.Contents.Add(new TextLiteral("Inline and flowing onto a new line "));
            span.FontSize = 24;
            //span.TextLeading = 30;
            section.Contents.Add(span);


            inline = new Div()
            {
                Height = 60,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                //Margins = new Thickness(10)
            };
            //section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line"));
            section.Contents.Add(span);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockExplicitSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(1, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(0, first.Height);
            Assert.AreEqual(1, first.Runs.Count);
            var posRun = first.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            Assert.ReferenceEquals(posRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);

            Assert.AreEqual(150, posReg.TotalBounds.Height);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(1, posReg.Contents.Count);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(150, posBlock.Height);
            Assert.AreEqual(200, posBlock.Width);
            Assert.AreEqual(1, posBlock.Columns.Length);
            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count);

            //Check the block after to make sure it is ignoring the positioned region.
            var second = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.AreEqual(0, second.OffsetY);
            Assert.AreEqual(25, second.Height);
            Assert.AreEqual(1, second.Columns[0].Contents.Count); //just a line

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((second.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeToParent()
        {
            

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));
            

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);
            

            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);
            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Positioned region should be mapped to the same dimensions as the inner block.
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height);

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(250, first.Height); //Includes the relative block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteToParent()
        {


            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(100, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height);

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The absolutely positioned block is positioned beyond the size of the page, so should not be shown
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteExplicitOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteExplicitOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Nothing in the region - has been reset
            Assert.AreEqual(0, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);
            Assert.AreEqual(0, posReg.TotalBounds.Width); 
            Assert.AreEqual(0, posReg.TotalBounds.Height); 

            Assert.AreEqual(0, posReg.Contents.Count);
            //var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //Just the line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The absolutely positioned block is positioned beyond the size of the page, but page is clipped, so should all be there
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteExplicitClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteExplicitClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Nothing in the region - has been reset
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(150, posReg.TotalBounds.Height);

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height); //we explicitly fit

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //Just the line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// No explicit height on the absolute div, so layout should stop when the second line overflows the page
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteFlowOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            //section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                //Height = 150, - no explicit height
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteFlowOverToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(25, posReg.TotalBounds.Height); //we only have 1 line that will fit

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(25, posBlock.TotalBounds.Height); //we only have 1 line that will fit

            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //should have both lines

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The section has an overflow action of clipped so the content that is absolutely positioned beyond the outside of the page
        /// will continue to be rendered.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockAbsoluteFlowClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip; //Set to clipped so the content is there
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Absolute,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockAbsoluteFlowClippedToParent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(50, posReg.TotalBounds.Height); //both lines fit

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(50, posBlock.TotalBounds.Height); //both lines fit

            Assert.AreEqual(2, posBlock.Columns[0].Contents.Count); //should have both lines

            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(25, first.Height); //DOES NOT Include the absolute block

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page,
        /// but as it is relative it increases the size of the parent block.
        /// Unusual Scenario, but the page is overflown and rendered there.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeExplicitOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeExplicitOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(1, firstLine.Runs.Count);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;

            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(0, posReg.TotalBounds.Width); //Nothing in
            Assert.AreEqual(0, posReg.TotalBounds.Height); //Nothing in

            Assert.AreEqual(0, posReg.Contents.Count);

            //second page

            content = layout.AllPages[1].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var second = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, second.PositionedRegions.Count);
            Assert.AreEqual(1, second.Columns.Length);

            Assert.AreEqual(content.Width, second.Width); //full width
            Assert.AreEqual(800 + 150, second.Height); //Pushed over

            var secondReg = second.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, secondReg.Contents.Count);


            var secondLine = secondReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, secondLine.Runs.Count);

            posRun = secondLine.Runs[0] as PDFLayoutPositionedRegionRun;

            Assert.IsNotNull(posRun);
            Assert.AreEqual(second.PositionedRegions[0], posRun.Region);

            posReg = second.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width); //Nothing in
            Assert.AreEqual(150, posReg.TotalBounds.Height); //Nothing in

            Assert.AreEqual(1, posReg.Contents.Count);

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (secondLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page,
        /// As it is relative it increases the size of the parent block. The page is set to clip, so everything should still be shown
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeExplicitClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeExplicitClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width); 
            Assert.AreEqual(150, posReg.TotalBounds.Height); 

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(150, posBlock.TotalBounds.Height); //both lines fit


            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(950, first.Height); //The offset and the height of the relative block are used

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the parent is not overflowing so the overflowing second line of the relative block should be hidden, and height adjusted appropraitely
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeFlowOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                //Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeFlowOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);

            
            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(25, posReg.TotalBounds.Height); // just one line

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(25, posBlock.TotalBounds.Height); //just one line fits



            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(825, first.Height); //Just the offset + line

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the parent is not clipped so the second line of the relative block should be there, and height adjusted appropriately
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeFlowClippedOverPageSize()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            doc.Pages.Add(section);

            Div absolute = new Div()
            {
                //Height = 150,
                Width = 200,
                X = 50,
                Y = 800,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            absolute.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            //Add the relative block to the inflow div.
            inflow.Contents.Add(absolute);

            inflow.Contents.Add(new TextLiteral("In normal content flow"));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeFlowClippedOverPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var first = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(1, first.PositionedRegions.Count);
            Assert.AreEqual(1, first.Columns.Length);

            var firstReg = first.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(1, firstReg.Contents.Count);

            var firstLine = firstReg.Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, firstLine.Runs.Count);
            Assert.AreEqual(25, firstLine.Height);

            var posRun = firstLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);
            Assert.AreEqual(first.PositionedRegions[0], posRun.Region);

            var posReg = first.PositionedRegions[0] as PDFLayoutPositionedRegion;
            Assert.IsNotNull(posReg);


            //Matches the size of the relative block
            Assert.AreEqual(50, posReg.TotalBounds.X);
            Assert.AreEqual(800, posReg.TotalBounds.Y);
            Assert.AreEqual(200, posReg.TotalBounds.Width);
            Assert.AreEqual(50, posReg.TotalBounds.Height); // both lines

            Assert.AreEqual(1, posReg.Contents.Count);
            var posBlock = posReg.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(0, posBlock.TotalBounds.X); //offsets on the region
            Assert.AreEqual(0, posBlock.TotalBounds.Y); //offsets on the region
            Assert.AreEqual(200, posBlock.TotalBounds.Width);
            Assert.AreEqual(50, posBlock.TotalBounds.Height); //both lines fit



            Assert.AreEqual(content.Width, first.Width); //full width
            Assert.AreEqual(850, first.Height); //The offset and both lines

            //posrun, textbegin, chars, textend
            Assert.AreEqual("In normal content flow", (firstLine.Runs[2] as PDFTextRunCharacter).Characters);
        }


        /// <summary>
        /// The relative positioned block is positioned beyond the size of the page, without an explicit height
        /// As it is relative it increases the size of the parent block.
        /// However the page is not clipped so the the relative block should be on a new page with the parent block there, and height adjusted appropriately
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockRelativeOverflowingToNewPage()
        {
            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(section);

            Div top = new Div() { Height = 800, BorderColor = Drawing.StandardColors.Aqua, ID = "top" };
            section.Contents.Add(top);
            top.Contents.Add("Pushes the relative content down over to the next page");

            Div relative = new Div()
            {
                //Height = 150,
                Width = 200,
                X = 50,
                Y = 100,
                PositionMode = Drawing.PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                ID = "relative"
            };
            relative.Contents.Add(new TextLiteral("Sits relative to the parent div"));


            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue, ID = "inflow" };
            //Add the relative block to the inflow div.
            //inflow.Contents.Add(new TextLiteral("Before the relative div."));

            inflow.Contents.Add(relative);

            inflow.Contents.Add(new TextLiteral("After the relative div."));

            //add the div to the section
            section.Contents.Add(inflow);


            using (var ms = DocStreams.GetOutputStream("Positioned_BlockRelativeOverflowingNewPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(2, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            //top block and inflow index 0 - that is empty and excluded
            Assert.AreEqual(2, content.Columns[0].Contents.Count);
            Assert.AreEqual(0, content.PositionedRegions.Count);

            var topBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(800, topBlock.Height);

            //There is a block where inflow was satarted but it has no content
            //so excluded from output
            var excluded = content.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(excluded);
            Assert.AreEqual(inflow, excluded.Owner);
            Assert.IsTrue(excluded.ExcludeFromOutput);

            //move to the second page
            content = layout.AllPages[1].ContentBlock;
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var inflowBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;

            Assert.IsNotNull(inflowBlock);
            Assert.AreEqual(1, inflowBlock.Columns.Length);
            Assert.AreEqual(1, inflowBlock.Columns[0].Contents.Count);

            var inflowLine = inflowBlock.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.IsNotNull(inflowLine);
            Assert.AreEqual(4, inflowLine.Runs.Count);

            
            var relativeRun = inflowLine.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(relativeRun);

            Assert.AreEqual(1, inflowBlock.PositionedRegions.Count);

            var relativeRegion = inflowBlock.PositionedRegions[0];
            Assert.AreEqual(50.0, relativeRegion.TotalBounds.X);
            Assert.AreEqual(100.0, relativeRegion.TotalBounds.Y);
            Assert.AreEqual(200.0, relativeRegion.Width);

            //Make sure we are pointing to the correct region
            Assert.AreEqual(relativeRun.Region, relativeRegion);

            Assert.AreEqual(1, relativeRegion.Contents.Count);

            var relativeBlock = relativeRegion.Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(0, relativeBlock.OffsetX);
            Assert.AreEqual(0, relativeBlock.OffsetY);

            Assert.AreEqual(200, relativeBlock.Width);
            Assert.AreEqual(50, relativeBlock.Height); //two lines of 25 leading text

            //Check the rest of the inflow line
            Assert.AreEqual("After the relative div.", (inflowLine.Runs[2] as PDFTextRunCharacter).Characters);
        }

    }
}
