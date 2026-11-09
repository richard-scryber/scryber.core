using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using System.Runtime.CompilerServices;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class ImageWebp_Tests
    {
        const string TestCategoryName = "Layout";

        const string ImagePath = "Content/Images/Webp/";
        

        PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this._layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutComponentRun GetBlockImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = _layout.AllPages[pg];
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
        public void WebP_1_YellowRoseAlphaLossy()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "1_webp_a.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_1_alpha_lossy.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

        [TestMethod]
        public void WepP_1_YellowRoseAlphaLossless()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "1_webp_ll.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_1_alpha_lossless.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        [TestMethod]
        public void WebP_2_PenguinTuxAlphaLossy()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "2_webp_a.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_2_alpha_lossy.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

        [TestMethod]
        public void WebP_2_PenguinTuxAlphaLossless()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "2_webp_ll.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_2_alpha_lossless.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        [TestMethod]
        public void WebP_3_TransparencyAlphaLossy()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "3_webp_a.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_3_alpha_lossy.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

        [TestMethod]
        public void WebP_3_TransparencyAlphaLossless()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "3_webp_ll.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_3_alpha_lossless.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        [TestMethod]
        public void WebP_4_MendelAlphaLossy()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "4_webp_a.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Silver;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_4_alpha_lossy.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

        [TestMethod]
        public void WebP_4_MendelAlphaLossless()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "4_webp_ll.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Silver;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_4_alpha_lossless.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        [TestMethod]
        public void WebP_5_CompassAlphaLossy()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "5_webp_a.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Silver;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(300);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_5_alpha_lossy.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(300, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(300, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

        [TestMethod]
        public void WebP_5_CompassAlphaLossless()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "5_webp_ll.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Silver;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(300);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_5_alpha_lossless.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(300, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(300, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        [TestMethod]
        public void LaptopRGB_50k()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "file_example_WEBP_50kb.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;

            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_LaptopRGB_50k.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(200, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(200, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }
        
        
        
        [TestMethod]
        public void LaptopRGB_500k()
        {
            var imagePath = DocStreams.AssertGetTemplatePath(ImagePath + "file_example_WEBP_500kb.webp");

            
            var doc = new Document();
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;

            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = imagePath;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Green;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(300);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("ImageWebP_LaptopRGB_500k.pdf"))
            {
                //doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout, "The layout was not saved from the event");

            var lpg = _layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, lpg.ContentBlock.Columns[0].Contents.Count);

            var ldiv = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ldiv);
            
            Assert.AreEqual(1, ldiv.Columns.Length);
            Assert.AreEqual(3, ldiv.Columns[0].Contents.Count);

            var line = ldiv.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            var chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("Before the block image", chars.Characters);

            var runnningHeight = line.Height; //height of the first line.
            
            line = ldiv.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var compRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun);
            
            var border = compRun.BorderRect;
            Assert.AreEqual(0, border.Y);
            Assert.AreEqual(100, border.X); //img margin
            Assert.AreEqual(300, border.Width); //explicit width
            
            Assert.AreEqual(img, compRun.Owner);

            //now check the actual render bounds - offset 20, 20 for margins and padding
            
            var arrange = img.GetFirstArrangement();
            border = arrange.RenderBounds;
            
            Assert.AreEqual(20 + runnningHeight, border.Y);
            Assert.AreEqual(20 + 100, border.X);
            Assert.AreEqual(300, border.Width);

            runnningHeight += border.Height;
            
            //last line
            
            line = ldiv.Columns[0].Contents[2] as PDFLayoutLine;
            
            Assert.IsNotNull(line);
            Assert.AreEqual(runnningHeight, line.OffsetY);

            Assert.AreEqual(3, line.Runs.Count);

            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.AreEqual("After the block image", chars.Characters);
        }

    }
}
