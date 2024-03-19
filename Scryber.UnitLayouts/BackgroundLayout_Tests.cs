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
    public class BackgroundLayout_Tests
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

        private PDFLayoutComponentRun GetBlockImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
        }

        private PDFLayoutComponentRun GetInlineImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
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
            pg.Contents.Add(div);

            var span = new Span();
            span.ID = "InnerContent";
            span.Contents.Add("This is a long string that will flow across two lines, and has a background");
            span.BackgroundColor = StandardColors.Blue;
            span.FillColor = StandardColors.White;
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
        public void RepeatingImage()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;


            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageNaturalWidth, pattern.Step.Width);
            Assert.AreEqual(ImageNaturalHeight, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            

        }

        [TestMethod]
        public void RepeatingImage_ExplicitSize()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.Style.Background.PatternXSize = ImageNaturalWidth / 5.0;
            div.Style.Background.PatternYSize = ImageNaturalHeight / 4.0;

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageExplicitSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.Step.Width);
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);



        }

        [TestMethod]
        public void RepeatingImage_ExplicitSizeXOnly()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.Style.Background.PatternXSize = ImageNaturalWidth / 5.0;
            div.Style.Background.PatternYSize = ImageNaturalHeight / 4.0;
            div.Style.Background.PatternRepeat = PatternRepeat.RepeatX;

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageExplicitSizeXOnly.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.Step.Width);
            Assert.AreEqual(int.MaxValue, pattern.Step.Height); //only repeats in the X direction

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);



        }

        [TestMethod]
        public void RepeatingImage_ExplicitSizeYOnly()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.Style.Background.PatternXSize = ImageNaturalWidth / 5.0;
            div.Style.Background.PatternYSize = ImageNaturalHeight / 4.0;
            div.Style.Background.PatternRepeat = PatternRepeat.RepeatY;

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageExplicitSizeYOnly.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(int.MaxValue, pattern.Step.Width); //only repeats in the Y direction
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.Step.Height);

        }

        [TestMethod]
        public void RepeatingImage_ExplicitSizeStepAndOffset()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.Style.Background.PatternXSize = ImageNaturalWidth / 5.0;
            div.Style.Background.PatternYSize = ImageNaturalHeight / 4.0;
            div.Style.Background.PatternXStep = ImageNaturalWidth / 2.5;
            div.Style.Background.PatternYStep = ImageNaturalHeight / 2.0;
            div.Style.Background.PatternXPosition = 20;
            div.Style.Background.PatternYPosition = 30;

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageExplicitSizeStepAndOffset.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0 + 20.0, pattern.Start.X.PointsValue); //margins + offsetX
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - (10.0 + 30.0), pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height + offsetY
            Assert.AreEqual(ImageNaturalWidth / 2.5, pattern.Step.Width);
            Assert.AreEqual(ImageNaturalHeight / 2.0, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);



        }

        [TestMethod]
        public void RepeatingImage_Fill()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Margins = 10;
            pg.BackgroundColor = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            var div = new Div();
            //div.Width = 400;
            div.Height = 700;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.BackgroundRepeat = PatternRepeat.Fill;

            //height = 700
            //width will be expanded so the image retains proportions
            //and will fill all the availalbe space.

            var factor = ImageNaturalWidth.PointsValue / ImageNaturalHeight.PointsValue;
            var height = div.Height.PointsValue;
            var width = height * factor;
            
            

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageFill.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(width, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(height, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            var offset = (width - divBlock.Width.PointsValue) / 2.0;
            AssertAreApproxEqual(10 - offset, pattern.Start.X.PointsValue); //image scaled width, centered based on margins
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(width, pattern.Step.Width);
            Assert.AreEqual(height, pattern.Step.Height);

        }

        [TestMethod]
        public void SimpleBackgroundGradient()
        {

            Assert.Inconclusive("Background gradients are tested on the HTML Parser tests");

            

        }


    }
}
