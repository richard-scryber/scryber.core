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
    public class InlineBlockPositioned_Tests
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
            const int lineHeight = 25;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = section.TextLeading;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
            };

            section.Contents.Add(inline);
            //section.Contents.Clear();

            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            //span.Contents.Add(new TextLiteral(" Inline and flowing onto a new line "));
            section.Contents.Add(span);



            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins.
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignDefault.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);

            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];


            //TODO: Clean up offsetX and Y with TotalBounds.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            Assert.Inconclusive();


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

        [TestMethod()]
        [TestCategory(TestCategoryName)]
        public void InlineBlockMultipleBlockExplicitSizeVAlignBottom()
        {
            const int lineHeight = 30;
            const VerticalAlignment vAlign = VerticalAlignment.Bottom;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            //Inline block twice line height
            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = vAlign
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            //Half height inline block that should be at the top.
            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = vAlign
            };

            section.Contents.Add(inline);


            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            section.Contents.Add(span);

            

            //3 times line height (inc. magins inline block)
            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = vAlign,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //A new full width div - should be set nicely below the rest of the text.
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignBottom.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }



            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign top baseline offset is ascender + half leading.
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (lineHeight - section.FontSize) / 2;

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins and line height for the start text cursor
            Assert.AreEqual(baseline + section.Margins.Top + lineHeight, leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top + lineHeight, rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - offset should be to the line height of the next line (which has a lineHeight * 3 inline block in it)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight * 3, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plus the previous lines height (3) + the inline height (3) - a line, so  = lineHeight * 5
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 5), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);


            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }
        /// <summary>
        /// Vertical align top. All inline blocks should be from the top down,
        /// and the text placed at the top of the line where the height is greater than the set line height.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSizeVAlignTop()
        {
            const int lineHeight = 30;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            //Inline block twice line height
            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = VerticalAlignment.Top
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            //Half height inline block that should be at the top.
            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = VerticalAlignment.Top
            };

            section.Contents.Add(inline);
            

            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            section.Contents.Add(span);


            //3 times line height (inc. magins inline block)
            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = VerticalAlignment.Top,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //A new full width div - should be set nicely below the rest of the text.
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignTop.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.

            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign top baseline offset is ascender + half leading.
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (lineHeight - section.FontSize) / 2;

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins for the start text cursor
            Assert.AreEqual(baseline + section.Margins.Top, leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top, rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);

            
            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);
            
            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight* 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 3), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight * 3, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);

            
            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);
            
            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockMultipleBlockExplicitSizeVAlignMiddle()
        {
            const int lineHeight = 30;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 20;
            section.TextLeading = lineHeight;
            section.Margins = 10;
            section.BackgroundColor = Drawing.StandardColors.Silver;
            //section.HorizontalAlignment = HorizontalAlignment.Justified;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = lineHeight;
            section.Style.OverlayGrid.GridXOffset = 10;
            section.Style.OverlayGrid.GridYOffset = 10;
            doc.Pages.Add(section);

            var span = new Span();
            span.Contents.Add(new TextLiteral("Before the inline "));
            section.Contents.Add(span);

            Div inline = new Div()
            {
                Height = lineHeight * 2,
                Width = 100,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = VerticalAlignment.Middle
            };
            //inline.Contents.Add(new TextLiteral("In the inline block"));
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral(" After the inline and flowing onto a new line with the required offset"));
            section.Contents.Add(span);

            inline = new Div()
            {
                Height = lineHeight / 2,
                Width = 50,
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Aqua,
                VerticalAlignment = VerticalAlignment.Middle
            };

            section.Contents.Add(inline);
            //section.Contents.Clear();

            span = new Span();
            span.Contents.Add(new TextLiteral("After the second inline and flowing onto a new line."));
            //span.Contents.Add(new TextLiteral(" Inline and flowing onto a new line "));
            section.Contents.Add(span);



            inline = new Div()
            {
                Height = (lineHeight * 3) - 10, //take off the margins
                Width = 100 - 10, //take off the margins
                PositionMode = Drawing.PositionMode.InlineBlock,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Lime,
                VerticalAlignment = VerticalAlignment.Middle,
                Margins = 5
            };
            section.Contents.Add(inline);

            span = new Span();
            span.Contents.Add(new TextLiteral("After the third inline and flowing onto a new line that should continue on in the normal height for the page."));
            section.Contents.Add(span);

            //div is too big for the remaining space on the page
            Div inflow = new Div() { BorderWidth = 1, BorderColor = Drawing.StandardColors.Blue };
            inflow.Contents.Add(new TextLiteral("In normal content flow"));
            section.Contents.Add(inflow);

            using (var ms = DocStreams.GetOutputStream("Positioned_InlineBlockMultipleExplicitSizeVAlignMiddle.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.Inconclusive("Need to fix inline block VAlign top");

            Assert.AreEqual(1, layout.AllPages.Count);
            var content = layout.AllPages[0].ContentBlock;

            Assert.AreEqual(6, content.Columns[0].Contents.Count);
            Assert.AreEqual(3, content.PositionedRegions.Count);

            //first line - 1 inline block at 2x line height

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 2, line.Height);
            Assert.AreEqual(10, line.Runs.Count);
            var leftBegin = line.Runs[1] as PDFTextRunBegin;
            var leftChars = line.Runs[2] as PDFTextRunCharacter;
            var inlineRun = line.Runs[5] as PDFLayoutInlineBlockRun;
            var rightBegin = line.Runs[7] as PDFTextRunBegin;
            var rightChars = line.Runs[8] as PDFTextRunCharacter;
            var newline = line.Runs[9] as PDFTextRunNewLine;

            Assert.IsNotNull(leftBegin);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[0]);
            var posReg = content.PositionedRegions[0];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight * 2, posReg.TotalBounds.Height);
            Assert.AreEqual(100, posReg.TotalBounds.Width);

            //valign top baseline offset is ascender + half leading.
            var baseline = leftBegin.TextRenderOptions.GetAscent() + (lineHeight - section.FontSize) / 2;

            Assert.AreEqual(baseline, line.BaseLineOffset);
            //Add the margins for the start text cursor
            Assert.AreEqual(baseline + section.Margins.Top, leftBegin.StartTextCursor.Height);
            Assert.AreEqual(section.Margins.Left, leftBegin.StartTextCursor.Width);

            var posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 2, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text
            Assert.AreEqual(baseline + section.Margins.Top, rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line should push the cursor down and right for the inline block and first chars.
            var offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //second line - 1 small top aligned inline block with text either side

            line = content.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            var leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[1]);
            posReg = content.PositionedRegions[1];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            Assert.AreEqual(lineHeight / 2, posReg.TotalBounds.Height);
            Assert.AreEqual(50, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,50, 100
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight / 2, posBlock.Height);
            Assert.AreEqual(50, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 2), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(leftChars.Width + posReg.Width, offset.Width);

            //third line - 1 top aligned inline block 3 * line height inc margins with text either side

            line = content.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.AreEqual(lineHeight * 3, line.Height);
            Assert.AreEqual(9, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = line.Runs[4] as PDFLayoutInlineBlockRun;
            rightBegin = line.Runs[6] as PDFTextRunBegin;
            rightChars = line.Runs[7] as PDFTextRunCharacter;
            newline = line.Runs[8] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(inlineRun);
            Assert.IsNotNull(rightBegin);
            Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);

            //third positioned region
            Assert.ReferenceEquals(inlineRun.Region, content.PositionedRegions[2]);
            posReg = content.PositionedRegions[2];

            //The positioned region is relative to the origin of the first line.
            Assert.AreEqual(leftChars.Width, posReg.TotalBounds.X);
            Assert.AreEqual(0, posReg.TotalBounds.Y);

            //take off the margins
            Assert.AreEqual((lineHeight * 3) - 10, posReg.TotalBounds.Height);
            Assert.AreEqual(100 - 10, posReg.TotalBounds.Width);


            //Same baseline offset as line 1
            Assert.AreEqual(baseline, line.BaseLineOffset);

            posBlock = posReg.Contents[0] as PDFLayoutBlock;
            //The block within the positioned region should be 0,0,75, 110
            Assert.AreEqual(0, posBlock.OffsetX);
            Assert.AreEqual(0, posBlock.OffsetY);
            Assert.AreEqual(lineHeight * 3, posBlock.Height);
            Assert.AreEqual(100, posBlock.Width);

            //Right begin text - baseline and margins plue the previous line height ( = lineHeight * 2)
            Assert.AreEqual(baseline + section.Margins.Top + (lineHeight * 3), rightBegin.StartTextCursor.Height);
            Assert.AreEqual(leftChars.Width + inlineRun.Width + section.Margins.Left, rightBegin.StartTextCursor.Width);

            //New line - single line height v offset
            offset = newline.NewLineOffset;
            Assert.AreEqual(line.BaseLineOffset + line.BaseLineToBottom, offset.Height);
            Assert.AreEqual(lineHeight * 3, offset.Height);
            //include the margins as well
            Assert.AreEqual(leftChars.Width + posReg.Width + 10, offset.Width);


            //fourth line -  is just a spacer, chars and a new line 

            line = content.Columns[0].Contents[3] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(3, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = line.Runs[2] as PDFTextRunNewLine;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            //Assert.IsNotNull(inlineRun);
            //Assert.IsNotNull(rightBegin);
            //Assert.IsNotNull(rightChars);
            Assert.IsNotNull(newline);


            //Simple line from a newline offset (checked previously) so zero.
            //TODO: zero works as it's ignored for the newline flow. But could be set to make more appropriate.
            Assert.AreEqual(0, line.BaseLineOffset);


            //New line - single line height v offset and 0 width (same start point for fifth line as fourth)
            offset = newline.NewLineOffset;
            Assert.AreEqual(lineHeight, offset.Height);
            Assert.AreEqual(0, offset.Width);


            //fifth line -  is just a spacer, chars, text end and an inline end 

            line = content.Columns[0].Contents[4] as PDFLayoutLine;

            Assert.AreEqual(lineHeight, line.Height);
            Assert.AreEqual(4, line.Runs.Count);

            leftBegin = null;
            leftSpacer = line.Runs[0] as PDFTextRunSpacer;
            leftChars = line.Runs[1] as PDFTextRunCharacter;
            inlineRun = null;
            rightBegin = null;
            rightChars = null;
            newline = null;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.IsNotNull(leftSpacer);
            Assert.IsNotNull(leftChars);
            Assert.IsNotNull(end);

            //Simple line from a newline offset (checked previously) so zero.
            Assert.AreEqual(baseline, line.BaseLineOffset);

            //check the last block is at the correct offset in the page

            var lastBlock = content.Columns[0].Contents[5] as PDFLayoutBlock;
            Assert.IsNotNull(lastBlock);

            //Should be 8 lines down based on the above content.
            Assert.AreEqual(lineHeight * 8, lastBlock.TotalBounds.Y);

            Assert.AreEqual(0, lastBlock.TotalBounds.X);

            //block region line = textbegin, chars, textend
            Assert.AreEqual("In normal content flow", ((lastBlock.Columns[0].Contents[0] as PDFLayoutLine).Runs[1] as PDFTextRunCharacter).Characters);
        }


        //
        // Tests to write
        //


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewLine()
        {
            Assert.Inconclusive();
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewColumn()
        {
            Assert.Inconclusive();
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void InlineBlockOverflowToNewPage()
        {
            Assert.Inconclusive();
        }



    }
}
