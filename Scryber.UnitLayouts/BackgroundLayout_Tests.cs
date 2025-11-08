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
using Scryber.Html.Components;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class BackgroundLayout_Tests
    {
        const string TestCategoryName = "Layout";

        const string ImagePath = "../../../Content/Images/Toroid32.png";
        const string SVGImagePath = "../../../Content/Images/Chart.svg";
        
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
        
        
        protected string AssertGetContentFile(string name)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
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

           
            Assert.AreEqual(ImageWidth, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageHeight, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageWidth, pattern.Step.Width);
            Assert.AreEqual(ImageHeight, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            

        }
        
        [TestMethod]
        public void RepeatingImageWithTransparency()
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
            div.BackgroundOpacity = 0.5;
            
            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageWithTransparency.pdf"))
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
            
            var w = pattern.ImageSize.Width.PointsValue;
            Assert.AreEqual(ImageWidth,  w);
            
            var h = pattern.ImageSize.Height.PointsValue;
            Assert.AreEqual(ImageHeight, h);
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageWidth, pattern.Step.Width);
            Assert.AreEqual(ImageHeight, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);
            
            var brush = divBlock.FullStyle.CreateBackgroundBrush();
            Assert.IsInstanceOfType(brush, typeof(Scryber.PDF.Graphics.PDFImageBrush));
            var imgBrush = brush as PDF.Graphics.PDFImageBrush;
            Assert.AreEqual(0.5, imgBrush.Opacity);

            

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
            //div.BackgroundOpacity = 0.5;
            
            //height = 700
            //width will be expanded so the image retains proportions
            //and will fill all the availalbe space.

            var factor = ImageNaturalWidth.PointsValue / ImageNaturalHeight.PointsValue;
            var height = div.Height.PointsValue;
            var width = height * factor;
            
            

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageFill.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
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
            //Top of the pattern is the top of the
            offset = layout.AllPages[0].Height.Value - div.GetFirstArrangement().RenderBounds.Y.Value;
            Assert.AreEqual(offset, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(width, pattern.Step.Width);
            Assert.AreEqual(height, pattern.Step.Height);

        }
        
        [TestMethod]
        public void RepeatingImage_FillNarrow()
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
            div.PositionMode = PositionMode.Absolute;
            div.X = 40;
            div.Y = 60;
            div.Width = 400;
            div.Height = 200;
            div.BorderColor = Drawing.StandardColors.Black;
            pg.Contents.Add(div);

            div.BackgroundImage = path;
            div.BackgroundRepeat = PatternRepeat.Fill;
            //div.BackgroundOpacity = 0.5;
            
            //height = 700
            //width will be expanded so the image retains proportions
            //and will fill all the availalbe space.

            var factor = ImageNaturalWidth.PointsValue / ImageNaturalHeight.PointsValue;
            var height = div.Height.PointsValue;
            var width = height * factor;
            
            

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageFillNarrow.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", path);

            var content = layout.AllPages[0].ContentBlock;
            Assert.IsTrue(content.HasPositionedRegions);
            var posReg = content.PositionedRegions[0];
            var divBlock = posReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);

            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);
            
            Assert.AreEqual(400, pattern.ImageSize.Width.ToPoints());
            var scale = 400 / ImageWidth;
            var h = scale * ImageHeight;
            Assert.AreEqual(h, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(path, pattern.Image.ResourceKey);

            AssertAreApproxEqual(40, pattern.Start.X.PointsValue); //image offset for start based on the position of the div
            //Top of the pattern is central
            var offset = 814.4410834280939; 
            Assert.AreEqual(offset, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            
            Assert.AreEqual(400, pattern.Step.Width);
            Assert.AreEqual(265.1026392961877, pattern.Step.Height);

        }


        [TestMethod]
        public void RepeatingMultipleGradientBackgrounds()
        {
            var path = AssertGetContentFile("MultipleBackgrounds");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Backgrounds_MultipleBackgrounds.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);
        }
        
        [TestMethod]
        public void RepeatingImageSVG_ExplicitSize()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, SVGImagePath);
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

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepatingImageSVGExplicitSize.pdf"))
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
        public void RepeatingImageSVGAsBase64_ExplicitSize()
        {

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, SVGImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            //convert to base 64 url
            var bin = System.IO.File.ReadAllBytes(path);
            var dataSvg = System.Convert.ToBase64String(bin);
            dataSvg = "data:" + MimeType.SvgImage + ";base64," + dataSvg;
            
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

            div.BackgroundImage = dataSvg;
            div.BackgroundRepeat = PatternRepeat.RepeatBoth;
            div.Style.Background.PatternXSize = ImageNaturalWidth / 5.0;
            div.Style.Background.PatternYSize = ImageNaturalHeight / 4.0;

            using (var ms = DocStreams.GetOutputStream("Backgrounds_RepeatingImageSVGAsBase64ExplicitSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);
            var rsrc = layout.DocumentComponent.SharedResources.GetResource("XObject", dataSvg);
            Assert.IsNotNull(rsrc);
            Assert.IsInstanceOfType(rsrc, typeof(PDF.Resources.PDFImageXObject));
            var xobj = rsrc as PDF.Resources.PDFImageXObject;
            var pattern = xobj.Container as PDF.Resources.PDFImageTilingPattern;
            Assert.IsNotNull(pattern);

            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.ImageSize.Width.ToPoints());
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.ImageSize.Height.ToPoints());
            Assert.IsTrue(pattern.Registered);
            Assert.IsNotNull(pattern.Image);
            Assert.AreEqual(dataSvg, pattern.Image.ResourceKey);

            Assert.AreEqual(10.0, pattern.Start.X.PointsValue);
            Assert.AreEqual(layout.AllPages[0].Height.PointsValue - 10.0, pattern.Start.Y.PointsValue); //PDF is from the bottom up so take off the margins from the height
            Assert.AreEqual(ImageNaturalWidth / 5.0, pattern.Step.Width);
            Assert.AreEqual(ImageNaturalHeight / 4.0, pattern.Step.Height);

            var divBlock = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(divBlock);



        }
        
        
        [TestMethod]
        public void CoverBackgroundImageSVGMultipleComponents()
        {
            var path = AssertGetContentFile("SVGBackgrounds");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Backgrounds_CoverBackgroundImageSVGMultipleComponents.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
        }
        
        
        [TestMethod]
        public void CoverBackgroundImageSVGWithTransparency()
        {
            var path = AssertGetContentFile("SVGBackgrounds");
            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Backgrounds_CoverBackgroundImageSVGWithTransparency.pdf"))
            {
                var para = doc.FindAComponentById("myPara2") as HTMLParagraph;
                Assert.IsNotNull(para);
                para.Style.Background.Opacity = 0.05;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }
            
        }
        


    }
}
