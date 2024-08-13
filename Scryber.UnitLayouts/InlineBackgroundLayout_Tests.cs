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
    public class InlineBackgroundLayout_Tests
    {
        const string TestCategoryName = "Layout";

        const string ImagePath = "../../../Content/Images/Toroid32.png";
        const double ImageWidth = 682.0;
        const double ImageHeight = 452.0;

        //Toroid32.png - 682 × 452 pixels natural size @96 ppi
        Unit ImageNaturalWidth = new Unit((ImageWidth / 96.0) * 72);
        Unit ImageNaturalHeight = new Unit((ImageHeight / 96.0) * 72.0);

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        private void AssertAreApproxEqual(double one, double two, string message = null)
        {
            int precision = 5;
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }

        [TestMethod]
        public void InlineBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);
            doc.Pages.Add(pg);
            
            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            div.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            Assert.AreEqual(0, linner.OffsetX, "Line offset should be 0");
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var lchars = linner.Runs[2] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            var h = linner.Height;

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);
        }

        [TestMethod]
        public void InlineRightBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            div.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var lchars = linner.Runs[2] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            var x = lcontent.Width - w; //as we are right aligned, then x is offset the with of the chars.
            var h = linner.Height;

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);

        }

        [TestMethod]
        public void InlineBottomRightBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            div.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBottomBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var lchars = linner.Runs[2] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            var x = lcontent.Width - w; //as we are right aligned, then x is offset the with of the chars.
            var h = linner.Height;
            var y = lcontent.Height - linner.Height; //as we are bottom aligned, then the y offset is the height of the div - line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);

        }

        [TestMethod]
        public void InlineMiddleCentreBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            div.HorizontalAlignment = HorizontalAlignment.Center;
            div.VerticalAlignment = VerticalAlignment.Middle;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            div.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineMiddleCenterBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var lchars = linner.Runs[2] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            var x = (lcontent.Width - w)/2; //as we are center aligned, then x is offset half the with of the chars.
            var h = linner.Height;
            var y = (lcontent.Height - linner.Height)/2; //as we are middle aligned, then the y offset is half the height of the div - line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);

        }

        [TestMethod]
        public void InlineBackgroundColorWithPadding()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);
            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 10;
            div.Contents.Add(span);

            var lit = new TextLiteral(" And this is after\r\nOn a new line");
            div.Contents.Add(lit);

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineBGColorWithPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            Assert.AreEqual(0, linner.OffsetX, "Line offset should be 0");
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var ltextSpaceLeft = linner.Runs[2] as PDFTextRunSpacer; //For the padding left
            var lchars = linner.Runs[3] as PDFTextRunCharacter;
            var ltextSpaceRight = linner.Runs[4] as PDFTextRunSpacer; //For the padding right

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpaceLeft);
            Assert.IsNotNull(lchars);
            Assert.IsNotNull(ltextSpaceRight);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Top);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Left);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Bottom);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Right);

            var w = lchars.Width + ltextSpaceLeft.Width + ltextSpaceRight.Width; 
            var h = linner.Height;

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);
        }


        [TestMethod]
        public void InlineBackgroundColorWithPaddingAndRadius()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(30);
            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 10;
            span.BorderCornerRadius = 5;
            div.Contents.Add(span);

            var lit = new TextLiteral(" And this is after\r\nOn a new line");
            div.Contents.Add(lit);

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineBGColorWithPaddingAndRadius.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            Assert.AreEqual(0, linner.OffsetX, "Line offset should be 0");
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var ltextSpaceLeft = linner.Runs[2] as PDFTextRunSpacer; //For the padding left
            var lchars = linner.Runs[3] as PDFTextRunCharacter;
            var ltextSpaceRight = linner.Runs[4] as PDFTextRunSpacer; //For the padding right

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpaceLeft);
            Assert.IsNotNull(lchars);
            Assert.IsNotNull(ltextSpaceRight);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Top);
            Assert.AreEqual(5, ltextStart.TextRenderOptions.BorderRadius.PointsValue);

            var w = lchars.Width + ltextSpaceLeft.Width + ltextSpaceRight.Width;
            var h = linner.Height;

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(0, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);
        }


        [TestMethod]
        public void InlineBottomRightBackgroundColorWithPaddingAndRadius()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(30);
            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 10;
            span.BorderCornerRadius = 5;
            span.BorderWidth = 1;
            span.BorderColor = StandardColors.White;
            
            div.Contents.Add(span);

            var lit = new TextLiteral(" And this is after\r\nOn a new line");
            div.Contents.Add(lit);

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineBottomRightBGColorWithPaddingAndRadius.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            Assert.AreEqual(0, linner.OffsetX, "Line offset should be 0");
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var ltextSpaceLeft = linner.Runs[2] as PDFTextRunSpacer; //For the padding left
            var lchars = linner.Runs[3] as PDFTextRunCharacter;
            var ltextSpaceRight = linner.Runs[4] as PDFTextRunSpacer; //For the padding right
            var lchars2 = linner.Runs[8] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpaceLeft);
            Assert.IsNotNull(lchars);
            Assert.IsNotNull(ltextSpaceRight);
            Assert.IsNotNull(lchars2);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);
            Assert.AreEqual(10, ltextStart.TextRenderOptions.Padding.Value.Top);
            Assert.AreEqual(5, ltextStart.TextRenderOptions.BorderRadius.PointsValue);

            var w = lchars.Width + ltextSpaceLeft.Width + ltextSpaceRight.Width;
            var h = linner.Height;
            var x = lcontent.Width - (w + lchars2.Width); //right align with the extra characters
            var y = lcontent.Height - (linner.Height * 2.0); //bottom align with the 2 line heights

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.IsTrue(ltextStart.CalculatedBounds[2].IsEmpty);
        }


        [TestMethod]
        public void InlineBottomRightLongTextBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 100;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            div.TextLeading = 40;
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a long string that will flow across two lines, and has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            div.Contents.Add(span);


            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBottomLongBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var ltextSpacer1 = linner.Runs[2] as PDFTextRunSpacer;
            var lchars = linner.Runs[3] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpacer1);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            Unit space = 4;
            var x = lcontent.Width - (w + space); //as we are right aligned, then x is offset the with of the chars + the padding.
            var h = linner.Height;
            var y = lcontent.Height - (linner.Height * 2); //as we are bottom aligned, then the y offset is the height of the div - 2 x line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w + space, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle empty
            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);

            //last line.
            linner = lcontent.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            x = lcontent.Width - (w + space); //right aligned still with end padding space.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w + space, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);



        }

        [TestMethod]
        public void InlineBottomRightVeryLongTextBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 300;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            div.TextLeading = 40;
            div.FontSize = 24;
            pg.Contents.Add(div);

            div.Contents.Add("This is before the span. ");

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a very long string that will flow across more than two lines in the page, and will show the background across all the lines ending with the padding.");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            div.Contents.Add(span);

            div.Contents.Add(" After the span");

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBottomVeryLongBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;

            //This is before the span. 
            var llitStart = linner.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(llitStart);
            var llitChars = linner.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual("This is before the span. ", llitChars.Characters);
            var llitEnd = linner.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(llitEnd);

            var lspanStart = linner.Runs[3] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[4] as PDFTextRunBegin;
            var ltextPadd = linner.Runs[5] as PDFTextRunSpacer;
            var lchars = linner.Runs[6] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextPadd);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);


            var w = lchars.Width;
            Unit padd = 4;
            var x = lcontent.Width - (w + padd); //as we are right aligned, then x is offset the with of the chars.
            var h = linner.Height;
            var y = lcontent.Height - (linner.Height * 4); //as we are bottom aligned, then the y offset is the height of the div - 4 x line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[0].Width); //first line width is chars width + padding
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle has 2 lines
            Assert.IsFalse(ltextStart.CalculatedBounds[1].IsEmpty);
            Assert.AreEqual(linner.Height * 2, ltextStart.CalculatedBounds[1].Height);
            y += linner.Height * 2;

            //last line.
            linner = lcontent.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(lchars);

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            //x = lcontent.Width - w; //As we have a span after - check the width of the span afterwards.

            //Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);


            //right align ending with After the Span literal.
            llitChars = linner.Runs[6] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            w = llitChars.Width;
            x = linner.Width + linner.RightInset - (lchars.Width + padd + llitChars.Width);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);


        }


        [TestMethod]
        public void InlineBottomRightVeryLongTextNestedSpanBackgroundColor()
        {

            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;

            div.Height = 300;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            div.TextLeading = 40;
            div.FontSize = 24;
            pg.Contents.Add(div);

            div.Contents.Add("This is before the span. ");

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a very long string that will flow across more than two lines in the page, ");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            span.BorderColor = StandardColors.White;

            var spanNest = new Span();
            spanNest.Contents.Add("and will show the background across all the");
            spanNest.FontSize = 40;
            spanNest.BackgroundColor = StandardColors.Blue;
            span.Contents.Add(spanNest);

            span.Contents.Add(" lines ending with the padding.");

            div.Contents.Add(span);

            span = new Span();
            span.BackgroundColor = StandardColors.Red;
            span.Contents.Add(" After the span that will push across multiple lines");
            div.Contents.Add(span);
            //div.Contents.Add(" After the span");

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBottomVeryLongNestedBGColor.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            Assert.Inconclusive("The inner spans should flow on from one another - needs unit testing checks");

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;
            var lspanStart = linner.Runs[0] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[1] as PDFTextRunBegin;
            var lchars = linner.Runs[2] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            var w = lchars.Width;
            var x = lcontent.Width - w; //as we are right aligned, then x is offset the with of the chars.
            var h = linner.Height;
            var y = lcontent.Height - (linner.Height * 2); //as we are bottom aligned, then the y offset is the height of the div - 2 x line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);

            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle empty
            Assert.IsTrue(ltextStart.CalculatedBounds[1].IsEmpty);

            //last line.
            linner = lcontent.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            x = lcontent.Width - w; //right aligned still.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);



        }

        [TestMethod]
        public void InlineBottomRightVeryLongTextWithReturnsBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 300;
            div.HorizontalAlignment = HorizontalAlignment.Right;
            div.VerticalAlignment = VerticalAlignment.Bottom;
            div.TextDecoration = Text.TextDecoration.Underline;
            div.TextLeading = 40;
            div.FontSize = 24;
            pg.Contents.Add(div);

            div.Contents.Add("This is before the span. ");

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a very long string that will flow across more than two lines\r\n in the page,\r\n\r\nand will show the background across all the lines ending with the padding.");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            div.Contents.Add(span);
            span = new Span();
            span.Contents.Add(" After the span");
            div.Contents.Add(span);
            //div.Contents.Add("After");

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineRightBottomVeryLongReturnsBGColor.pdf"))
            {
                pg.Style.OverlayGrid.ShowGrid = true;
                pg.Style.OverlayGrid.GridSpacing = 10;
                pg.Style.OverlayGrid.GridColor = StandardColors.Silver;
                pg.Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;

            //This is before the span. 
            var llitStart = linner.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(llitStart);
            var llitChars = linner.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual("This is before the span. ", llitChars.Characters);
            var llitEnd = linner.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(llitEnd);


            var lspanStart = linner.Runs[3] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[4] as PDFTextRunBegin;
            var ltextSpace = linner.Runs[5] as PDFTextRunSpacer;
            var lchars = linner.Runs[6] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpace);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            


            var w = lchars.Width;
            Unit padd = 4;
            var x = lcontent.Width - (w + padd); //as we are right aligned, then x is offset the with of the chars.
            var h = linner.Height;
            var y = lcontent.Height - (linner.Height * 6); //as we are bottom aligned, then the y offset is the height of the div - 2 x line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);


            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle empty
            Assert.IsFalse(ltextStart.CalculatedBounds[1].IsEmpty);


            //TODO: Test the inbetween lines as needed
            //We at least test that the 4 lines have been created here.
            Assert.AreEqual(linner.Height * 4, ltextStart.CalculatedBounds[1].Height);
            y += linner.Height * 4;

            //last line.
            linner = lcontent.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            //x = lcontent.Width - w; //check x after the span

            //Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);

            //right align ending with After the Span literal.
            llitChars = linner.Runs[7] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual(" After the span", llitChars.Characters);

            w = llitChars.Width;
            x = linner.Width + linner.RightInset - (lchars.Width + padd + llitChars.Width);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);


        }

        
        [TestMethod]
        public void InlineMiddleCentreVeryLongTextWithReturnsBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 300;
            div.HorizontalAlignment = HorizontalAlignment.Center;
            div.VerticalAlignment = VerticalAlignment.Middle;
            div.TextDecoration = Text.TextDecoration.Underline;
            div.TextLeading = 40;
            div.FontSize = 24;
            pg.Contents.Add(div);

            div.Contents.Add("This is before the span. ");

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a very long string that will flow across more than two lines\r\n in the page,\r\n\r\nand will show the background across all the lines ending with the padding.");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            div.Contents.Add(span);
            span = new Span();
            span.Contents.Add(" After the span");
            div.Contents.Add(span);
            //div.Contents.Add("After");

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineMiddleCentreVeryLongReturnsBGColor.pdf"))
            {
                pg.Style.OverlayGrid.ShowGrid = true;
                pg.Style.OverlayGrid.GridSpacing = 10;
                pg.Style.OverlayGrid.GridColor = StandardColors.Silver;
                pg.Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;

            //This is before the span. 
            var llitStart = linner.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(llitStart);
            var llitChars = linner.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual("This is before the span. ", llitChars.Characters);
            var llitEnd = linner.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(llitEnd);


            var lspanStart = linner.Runs[3] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[4] as PDFTextRunBegin;
            var ltextSpace = linner.Runs[5] as PDFTextRunSpacer;
            var lchars = linner.Runs[6] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpace);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            


            var w = lchars.Width;
            Unit padd = 4;
            var space = lcontent.Width - (w + llitChars.Width + padd); //total space in the line.
            
            var x = llitChars.Width + (space/2); //as we are centre aligned, then x is the width of the first span + half the remaining space.
            var h = linner.Height;
            var y = (lcontent.Height - (linner.Height * 6)) / 2; //as we are centre aligned, then the y offset is half the height of the div - 6 x line height.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);


            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle empty
            Assert.IsFalse(ltextStart.CalculatedBounds[1].IsEmpty);


            //TODO: Test the inbetween lines as needed
            //We at least test that the 4 lines have been created here.
            Assert.AreEqual(linner.Height * 4, ltextStart.CalculatedBounds[1].Height);
            y += linner.Height * 4;

            //last line.
            linner = lcontent.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            //x = lcontent.Width - w; //check x after the span

            //Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);

            //right align ending with After the Span literal.
            llitChars = linner.Runs[7] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual(" After the span", llitChars.Characters);

            w = llitChars.Width;
            x = linner.Width + linner.RightInset - (lchars.Width + padd + llitChars.Width);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);


        }
        
        
        [TestMethod]
        public void InlineJustifiedCentreVeryLongTextWithReturnsBackgroundColor()
        {
            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(20);

            doc.Pages.Add(pg);

            var div = new Div();
            div.ID = "WithContent";
            div.BackgroundColor = StandardColors.Aqua;
            div.Height = 300;
            div.HorizontalAlignment = HorizontalAlignment.Justified;
            div.VerticalAlignment = VerticalAlignment.Middle;
            div.TextDecoration = Text.TextDecoration.Underline;
            div.TextLeading = 40;
            div.FontSize = 24;
            pg.Contents.Add(div);

            div.Contents.Add("This is before the span. ");

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a very long string that will flow across a lot more than two lines in the page,\r\n\r\nand will show the background across all the lines ending with the padding.");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
            span.Padding = 4;
            span.BorderCornerRadius = 4;
            div.Contents.Add(span);
            span = new Span();
            span.Contents.Add(" After the span");
            div.Contents.Add(span);
            //div.Contents.Add("After");

            using (var ms = DocStreams.GetOutputStream("Backgrounds_InlineJustifiedCentreVeryLongReturnsBGColor.pdf"))
            {
                pg.Style.OverlayGrid.ShowGrid = true;
                pg.Style.OverlayGrid.GridSpacing = 10;
                pg.Style.OverlayGrid.GridColor = StandardColors.Silver;
                pg.Style.OverlayGrid.GridMajorCount = 5;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            var lblock = lpg.ContentBlock;
            var lcontent = lblock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(lcontent);
            Assert.AreEqual("WithContent", lcontent.Owner.ID);

            var linner = lcontent.Columns[0].Contents[0] as PDFLayoutLine;

            //This is before the span. 
            var llitStart = linner.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(llitStart);
            var llitChars = linner.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual("This is before the span. ", llitChars.Characters);
            var llitEnd = linner.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(llitEnd);


            var lspanStart = linner.Runs[3] as PDFLayoutInlineBegin;
            var ltextStart = linner.Runs[4] as PDFTextRunBegin;
            var ltextSpace = linner.Runs[5] as PDFTextRunSpacer;
            var lchars = linner.Runs[6] as PDFTextRunCharacter;

            Assert.IsNotNull(lspanStart);
            Assert.IsNotNull(ltextStart);
            Assert.IsNotNull(ltextSpace);
            Assert.IsNotNull(lchars);

            Assert.AreEqual("InnerContent", lspanStart.Owner.ID);
            Assert.AreEqual(0, lspanStart.OffsetX);

            


            var w = lchars.Width + linner.ExtraSpace; //Extra space is for the word spacing added by justification
            Unit padd = 4;
            var x = lcontent.Width - (w + padd); //as we are right aligned, then x is offset the width of the chars.
            var h = linner.Height;
            var y = (lcontent.Height - (linner.Height * 6)) / 2; //as we are centre aligned, then the y offset is the height of the div - 6 x line height, then halved for the centre.

            Assert.AreEqual(3, ltextStart.CalculatedBounds.Length);


            //rect for the top line,
            //empty rect for the middle content
            //rect for the bottom line.

            Assert.AreEqual(x, ltextStart.CalculatedBounds[0].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[0].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[0].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[0].Height);

            Assert.IsNotNull(ltextStart.TextRenderOptions.Background);

            //middle empty
            Assert.IsFalse(ltextStart.CalculatedBounds[1].IsEmpty);


            //TODO: Test the inbetween lines as needed
            //We at least test that the 4 lines have been created here.
            Assert.AreEqual(linner.Height * 4, ltextStart.CalculatedBounds[1].Height);
            y += linner.Height * 4;

            //last line.
            linner = lcontent.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(linner);
            lchars = linner.Runs[1] as PDFTextRunCharacter;

            w = lchars.Width; //last line width
            y += linner.Height; //add another line height
            h = linner.Height; //the height
            //x = lcontent.Width - w; //check x after the span

            //Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);
            Assert.AreEqual(w + padd, ltextStart.CalculatedBounds[2].Width);
            Assert.AreEqual(y, ltextStart.CalculatedBounds[2].Y);
            Assert.AreEqual(h, ltextStart.CalculatedBounds[2].Height);

            //centre align ending with After the Span literal.
            llitChars = linner.Runs[7] as PDFTextRunCharacter;
            Assert.IsNotNull(llitChars);
            Assert.AreEqual(" After the span", llitChars.Characters);

            w = llitChars.Width;
            x = linner.Width + linner.RightInset - (lchars.Width + padd + llitChars.Width);

            Assert.AreEqual(x, ltextStart.CalculatedBounds[2].X);


        }

    }
}
