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
