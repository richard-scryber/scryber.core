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
    public class ImageLayout_Tests
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
        public void WidthAsBlock()
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
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);

            var div = new Div();
            div.Padding = 10;
            pg.Contents.Add(div);
            
            

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.BackgroundColor = StandardColors.Silver;
            img.DisplayMode = DisplayMode.Block;
            img.Width = new Unit(200);
            img.Margins = new Thickness(0, 0, 0, 100);
            
            div.Contents.Add(new TextLiteral("Before the block image"));
            div.Contents.Add(img);
            div.Contents.Add(new TextLiteral("After the block image"));
            
            using (var stream = DocStreams.GetOutputStream("Images_WidthAsBlock.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            
            Assert.Inconclusive("Need to check the div");
        }


        [TestMethod]
        public void FixedSizes()
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
            doc.Pages.Add(pg);


            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = new Unit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = new Unit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt High"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = new Unit(100);
            img.Width = new Unit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt Both"));

            using (var stream = DocStreams.GetOutputStream("Images_FixedSizes.pdf"))
            {
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutComponentRun lrun = GetBlockImageRunForPage(0);

            //Natural size check
            var width = ImageNaturalWidth.PointsValue;
            var height = ImageNaturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural sise");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size");

            //100pt wide
            width = 100;
            height = ImageNaturalHeight.PointsValue * (100.0 / ImageNaturalWidth.PointsValue);
            lrun = GetBlockImageRunForPage(1);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 100pt wide");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 100pt wide");

            //100pt high
            width = ImageNaturalWidth.PointsValue * (100.0 / ImageNaturalHeight.PointsValue);
            height = 100;
            lrun = GetBlockImageRunForPage(2);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 100pt high");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 100pt high");

            //100pt wide and high
            width = 100;
            height = 100;
            lrun = GetBlockImageRunForPage(3);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 100pt both");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 100pt both");


        }



        [TestMethod]
        public void MaximumSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72);
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0);

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);


            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumWidth = new Unit(600); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumWidth = new Unit(200); //smaller so should reduce the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 200pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new Unit(200); //smaller so should reduce height and width proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 200pt High"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new Unit(200); //smaller so should reduce height and width proportionally
            img.MaximumWidth = new Unit(400);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 400pt wide and 200pt High"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new Unit(400); //smaller so should reduce height and width proportionally
            img.MaximumWidth = new Unit(200);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 200pt wide and 400pt High"));

            using (var stream = DocStreams.GetOutputStream("Images_MaximumSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutComponentRun lrun = GetBlockImageRunForPage(0);

            //Natural size check
            var width = naturalWidth.PointsValue;
            var height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural sise");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size");

            //Max 200pt wide
            width = 200;
            height = naturalHeight.PointsValue * (200.0 / naturalWidth.PointsValue);
            lrun = GetBlockImageRunForPage(1);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for max 200pt wide");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for max 200pt wide");

            //Max 200pt high
            width = naturalWidth.PointsValue * (200.0 / naturalHeight.PointsValue);
            height = 200;
            lrun = GetBlockImageRunForPage(2);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for max 200pt high");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for max 200pt high");

            //Max 400pt wide and 200pt High
            //As 200 is the limit, should set the width to less than 400
            width = naturalWidth.PointsValue * (200.0/ naturalHeight.PointsValue);
            height = 200;
            lrun = GetBlockImageRunForPage(3);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 400pt wide and 200pt high");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 400pt wide and 200pt high");

            //Max 200pt wide and 400pt High
            //As 200 is the limit, should set the height to less than 400
            width = 200;
            height = naturalHeight.PointsValue * (200.0 / naturalWidth.PointsValue);
            lrun = GetBlockImageRunForPage(4);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 400pt high and 200pt wide");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 400pt high and 200pt wide");


        }

        [TestMethod]
        public void MinimumSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72); //551.5pt
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);


            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new Unit(400); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size - min width 400pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumHeight = new Unit(300); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size - min height 300"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new Unit(550); //larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 550pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumHeight = new Unit(350); //larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 350pt High"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new Unit(550);
            img.MinimumHeight = new Unit(350); //width larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 550 wide and 350pt High - 550 wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new Unit(500);
            img.MinimumHeight = new Unit(350); //height larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 500 wide and 350pt High - 350 high"));


            using (var stream = DocStreams.GetOutputStream("Images_MinumumSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutComponentRun lrun = GetBlockImageRunForPage(0);

            //Min width 400
            var width = naturalWidth.PointsValue;
            var height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min width 400");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min width 400");

            lrun = GetBlockImageRunForPage(1);

            //Min height 300
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min height 300");

            lrun = GetBlockImageRunForPage(2);

            //Min width 550
            width = 550;
            height = naturalHeight.PointsValue * (550.0 / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min width 550");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min width 550");

            lrun = GetBlockImageRunForPage(3);

            //Min height 350
            width = naturalWidth.PointsValue * (350.0 / naturalHeight.PointsValue);
            height = 350;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min height 350");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min height 350");

            lrun = GetBlockImageRunForPage(4);

            //Min width 550 and height 350 - width takes precedence
            width = 550;
            height = naturalHeight.PointsValue * (550.0 / naturalWidth.PointsValue); 

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min width 550 and height 350 ");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min width 550 and height 350 ");

            lrun = GetBlockImageRunForPage(5);

            //Min width 500 and height 350 - height takes precedence
            width = naturalWidth.PointsValue * (350.0 / naturalHeight.PointsValue);
            height = 350;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for min width 500 and height 350");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for min width 500 and height 350");


        }

        [TestMethod]
        public void MixedActualAndMinSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72); //551.5pt
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 200;
            img.MinimumHeight = new Unit(100); // smaller so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and min height 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 200;
            img.MinimumHeight = new Unit(300); // larger so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and min height 300pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MinimumWidth = new Unit(100); // smaller so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Heigth 200pt and min width 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MinimumWidth = new Unit(500); // larger so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Heigth 200pt and min width 500pt"));

            using (var stream = DocStreams.GetOutputStream("Images_MixedActualAndMinSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutComponentRun lrun = GetBlockImageRunForPage(0);

            //Width 200 - min height 100
            var width = 200.0;
            var height = naturalHeight.PointsValue * (200.0 / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for width 200");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for width 200");

            
            lrun = GetBlockImageRunForPage(1);

            //Width 200 - min height 300
            width = 200.0;
            height = 300.0;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for width 200 and min height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for width 200 and min height 300");

            lrun = GetBlockImageRunForPage(2);

            //Height 250 - min width 100
            width = naturalWidth.PointsValue * (250.0 / naturalHeight.PointsValue);
            height = 250.0;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for height 250 and min width 100");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for height 250 and min width 100");

            lrun = GetBlockImageRunForPage(3);

            //Height 250 - min width 400
            width = 500.0;
            height = 250.0;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for height 250 and min width 500");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for height 250 and min width 500");

        }

        [TestMethod]
        public void MixedActualAndMaxSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72); //551.5pt
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 300;
            img.MaximumHeight = new Unit(300); //1. larger so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and max height 300pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 300;
            img.MaximumHeight = new Unit(100); //2. smaller so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and max height 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MaximumWidth = new Unit(500); //3. larger so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Height 250pt and max width 500pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MaximumWidth = new Unit(200); //4. smaller so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Height 250pt and max width 200pt"));

            using (var stream = DocStreams.GetOutputStream("Images_MixedActualAndMaxSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutComponentRun lrun = GetBlockImageRunForPage(0);

            //1. Width 300 - max height 300
            var width = 300.0;
            var height = naturalHeight.PointsValue * (300.0 / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for width 300 and max height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for width 300 and max height 300");


            lrun = GetBlockImageRunForPage(1);

            //2. Width 300 - max height 100
            width = 300.0;
            height = 100;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for width 300 and max height 100");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for width 300 and max height 100");

            lrun = GetBlockImageRunForPage(2);

            //3. Height 250 - max width 500
            width = naturalWidth.PointsValue * (250.0 / naturalHeight.PointsValue);
            height = 250;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for height 250 and max width 500");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for height 250 and max width 500");

            lrun = GetBlockImageRunForPage(3);

            //3. Height 250 - max width 500
            width = 200;
            height = 250;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for height 250 and max width 200");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for height 250 and max width 200");

        }

        [TestMethod]
        public void FitContainerSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72); //551.5pt
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //1. No Explicit sizes so natural
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("1. No Explicit sizes"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //2. Full width
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("2. Full width in page"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with max
            img.MaximumHeight = 200;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("3. Full width in page with 200 max height"));

            //new page with 2 columns
            pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.HighlightColumns = true;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //No Explicit sizes so natural in 2 columns
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("4. No Explicit sizes with 2 columns"));

            //new page with 3 columns
            pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 3;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.HighlightColumns = true;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //No Explicit sizes so natural in 3 columns
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("5. No Explicit sizes with 3 columns"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with actual height
            img.Height = 300;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("6. Full width in 3 columns with height of 300 (stretched)"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with min height
            img.MinimumHeight = 50;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("7. Full width in 3 columns with min height of 50 (natural)"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with min height
            img.MinimumHeight = 300;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("8. Full width in page and min height of 300 (stretched)"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with max height 300
            img.MaximumHeight = 300;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("9. Full width in page and max height of 300 (natural)"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.FullWidth = true; //Full width with max height 50
            img.MaximumHeight = 50;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("10. Full width in page with max height of 50 (squashed)"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //No Width but explicit minimum height
            img.MinimumHeight = 320;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("11. No width but minimum height of 320 (fit width)"));

            using (var stream = DocStreams.GetOutputStream("Images_FitContainerSizes.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");


            PDFLayoutComponentRun lrun;

            lrun = GetBlockImageRunForPage(0);

            //1. Natural size in container
            var width = naturalWidth.PointsValue;
            var height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for full page space");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for full page space");

            //2. Full width
            lrun = GetBlockImageRunForPage(1);
            var pgw = PageSize.A4.Size.Width; //A4
            var pgspace = pgw - 20; //margins
            width = pgspace.PointsValue;
            height = naturalHeight.PointsValue * (pgspace.PointsValue / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for full page space at full width");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for full page space at full width");

            lrun = GetBlockImageRunForPage(2);

            //3. Full width with 200 max height
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            width = pgspace.PointsValue;
            height = 200;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for full page space at full width");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for full page space at full width");

            lrun = GetBlockImageRunForPage(3);

            //4. Natural size in 2 columns
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            var colsize = (pgspace - Scryber.Styles.ColumnsStyle.DefaultAlleyWidth) / 2; //remove alley and divide by 2
            width = colsize.PointsValue;
            height = naturalHeight.PointsValue * (colsize.PointsValue / naturalWidth.PointsValue); 

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 2 column space");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 2 column space");


            lrun = GetBlockImageRunForPage(4);

            //5. Natural size in 3 columns
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = naturalHeight.PointsValue * (colsize.PointsValue / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space");


            lrun = GetBlockImageRunForPage(5);

            //6. Natural size in 3 columns with height of 300
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = 300;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with height 300");


            lrun = GetBlockImageRunForPage(6);

            //7. Natural size in 3 columns with min height of 50
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = naturalHeight.PointsValue * (colsize.PointsValue / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with min height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with min height 300");

            lrun = GetBlockImageRunForPage(7);

            //8. Natural size in 3 columns with min height of 300
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = 300;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with min height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with min height 300");


            lrun = GetBlockImageRunForPage(8);

            //9. Natural size in 3 columns with max height of 300
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = naturalHeight.PointsValue * (colsize.PointsValue / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with max height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with max height 300");

            lrun = GetBlockImageRunForPage(9);

            //10. Natural size in 3 columns with max height of 50
            pgw = PageSize.A4.Size.Width; //A4
            pgspace = pgw - 20; //margins
            pgspace = pgspace - (2.0 * Scryber.Styles.ColumnsStyle.DefaultAlleyWidth.PointsValue); //remove alleys
            colsize = pgspace / 3; //divide by 3 columns
            width = colsize.PointsValue;
            height = 50;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with max height 50");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with max height 50");

            lrun = GetBlockImageRunForPage(10);

            //11. Natural size in 3 columns with min height of 300 - push back into the bounds
            width = colsize.PointsValue;
            height = 320;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 3 column space with min height 300");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 3 column space with max height 300");

        }

        [TestMethod]
        public void ImageOverlowTests()
        {

            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new Unit((ImageWidth / 96.0) * 72); //551.5pt
            var naturalHeight = new Unit((ImageHeight / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 250;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //1. No Explicit sizes so natural
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("1. No Explicit sizes - shrink"));



            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 100;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 200;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("2. Explicit height = 200 - overflowing"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 100;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 200;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("3. Explicit width = 200 - overflowing"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 100;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumHeight = 200;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("4. Minimum Height = 200 - overflowing with natural size"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 100;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = 200;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("5. Minimum Width = 200 - overflowing with natural size"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 50;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.MinimumScaleReduction = 0.2; //set the minimum scale to 20%
            img.BorderColor = StandardColors.Black;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("6. No Size, but cannot go below 20% shrink"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 150;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumScaleReduction = 0.5;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("7. No Size, but explicit cannot go below 50% shrink"));


            //2 column layout

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 150;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 180;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("8. Explicit size so new column at proportionally that size"));

            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 150;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumScaleReduction = 0.5;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("9. No Size, but explicit cannot go below 50% shrink so new column at natural size"));

            using (var stream = DocStreams.GetOutputStream("Images_Overlowing.pdf"))
            {
                doc.ConformanceMode = ParserConformanceMode.Strict;
                doc.AppendTraceLog = true;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            //bottom of first page
            lrun = GetBlockImageRunForPage(0, 0, 1);

            //1. Natural size in container
            var width = naturalWidth.PointsValue * (230.0 / naturalHeight.PointsValue);
            var height = 230.0; //available space (250 - margins)

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for squeezing in the space");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for squeezing in the space");


            lrun = GetBlockImageRunForPage(3, 0, 0);

            //2. Explicit height overflows
            width = naturalWidth.PointsValue * (200.0 / naturalHeight.PointsValue);
            height = 200.0; 

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for explicit size on page 4");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for explicit size on page 4");

            lrun = GetBlockImageRunForPage(5, 0, 0);

            //3. Explicit width overflows
            width = 200.0;  
            height = naturalHeight.PointsValue * (200.0 / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for explicit size on page 6");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for explicit size on page 6");

            lrun = GetBlockImageRunForPage(7, 0, 0);

            //3. minimum height overflows
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural size on page 8 with min height");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size on page 8 with min height");

            lrun = GetBlockImageRunForPage(9, 0, 0);

            //3. minimum height overflows
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural size on page 10 with min width");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size on page 10 with min width");

            lrun = GetBlockImageRunForPage(11, 0, 0);

            //9. below min scale threshold
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural size on page 12 as below min scale threshold");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size on page 12 as below min scale threshold");


            lrun = GetBlockImageRunForPage(13, 0, 0);

            //9. below explicit min scale threshold
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for natural size on page 14 as below explicit scale threshold");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for natural size on page 14 as below explicit scale threshold");



            //Same page, onto new column
            lrun = GetBlockImageRunForPage(14, 1, 0);

            width = naturalWidth.PointsValue * (180.0 / naturalHeight.PointsValue);
            height = 180;

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for columns on page 15 for new column overflow with height");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for columns size on page 15 for new column overflow with height");


            
            //Same page, onto new column
            lrun = GetBlockImageRunForPage(15, 1, 0);

            var pgw = PageSize.A4.Size.Width; //A4
            var pgspace = pgw - 20; //margins
            var colsize = (pgspace - Scryber.Styles.ColumnsStyle.DefaultAlleyWidth) / 2; //remove alley and divide by 2

            width = colsize.PointsValue;
            height = naturalHeight.PointsValue * (colsize.PointsValue / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for columns on page 16 for new column overflow filling width");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for columns size on page 16 for new column overflow filling width");

        }

        

        [TestMethod]
        public void InlineImages1NaturalSizes()
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
            pg.FontSize = 20;
            doc.Pages.Add(pg);

            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 20;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridXOffset = 10;
            pg.Style.OverlayGrid.GridYOffset = 10;

            Image img;

            img = new Image();
            img.Source = path;
            img.ID = "Image1";
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 1. An inline image at natural size, new line at text size that will flow onto multiple lines afterwards"));

            


            using (var stream = DocStreams.GetOutputStream("Images_Inlining_1NaturalSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            

            //1. Natural Size
            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];
            var line = reg.Contents[0] as PDFLayoutLine;

            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            Assert.AreEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, ImageNaturalHeight.PointsValue);

            Assert.AreEqual(4, line.Runs.Count);

            var imgRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(ImageNaturalWidth, imgRun.Width);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            var txtBegin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            Assert.AreEqual(txtBegin.StartTextCursor.Width, ImageNaturalWidth + 10); //text begins after the image width + page margin
            Assert.AreEqual(txtBegin.StartTextCursor.Height, ImageNaturalHeight + 10); //text is baseline with the image height
            
            var txtChars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space 
            Assert.AreEqual(" 1. An", txtChars.Characters);

            var txtBr = line.Runs[3] as PDFTextRunNewLine;
            Assert.AreEqual(24.0, txtBr.NewLineOffset.Height.PointsValue);

            //Second line of 1. Natural size
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("inline image at natural size, new line at text size that will flow", txtChars.Characters);

            txtBr = line.Runs[2] as PDFTextRunNewLine;
            Assert.AreEqual(24.0, txtBr.NewLineOffset.Height.PointsValue);

            //Third line of 1. Natural size
            line = reg.Contents[2] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);

            txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("onto multiple lines afterwards", txtChars.Characters);

            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);
        }


        [TestMethod]
        public void InlineImages2NaturalSizeLargeFont()
        {

            

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();
            pg = new Page();
            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = 60;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.ID = "Image2";
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("2. Inline and text size of 60pt with overflowing content"));



            using (var stream = DocStreams.GetOutputStream("Images_Inlining_NaturalSize60ptText.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            //1. Natural Size
            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];
            var line = reg.Contents[0] as PDFLayoutLine;

            //first layout line
            var halflead = (60 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            Assert.AreEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, ImageNaturalHeight.PointsValue);

            Assert.AreEqual(4, line.Runs.Count);

            var imgRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(ImageNaturalWidth, imgRun.Width);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            var txtBegin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            Assert.AreEqual(txtBegin.StartTextCursor.Width, ImageNaturalWidth + 10); //text begins after the image width + page margin
            Assert.AreEqual(txtBegin.StartTextCursor.Height, ImageNaturalHeight + 10); //text is baseline with the image height
            
            var txtChars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);

            //Only room for the number
            Assert.AreEqual("2.", txtChars.Characters);

            var txtBr = line.Runs[3] as PDFTextRunNewLine;
            Assert.AreEqual(72.0, txtBr.NewLineOffset.Height.PointsValue);

            //Second line
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("Inline and text size of", txtChars.Characters);

            txtBr = line.Runs[2] as PDFTextRunNewLine;
            Assert.AreEqual(72.0, txtBr.NewLineOffset.Height.PointsValue);

            //Third line
            line = reg.Contents[2] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);

            txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("60pt with overflowing", txtChars.Characters);

            txtBr = line.Runs[2] as PDFTextRunNewLine;
            Assert.AreEqual(0, txtBr.Width);
            Assert.AreEqual(72.0, txtBr.NewLineOffset.Height.PointsValue);

            //Fourth line
            line = reg.Contents[3] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);

            txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("content", txtChars.Characters);

            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.IsNotNull(txtEnd);
            Assert.AreEqual(txtBegin, txtEnd.Start);
        }


        [TestMethod]
        public void InlineImages3ExplicitHeight()
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
            pg.FontSize = 20;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.ID = "Image3";
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 3. Inline at explicit 60pt height"));



            using (var stream = DocStreams.GetOutputStream("Images_Inlining_3FixedHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            //1. Natural Size
            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];

            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;

            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            Assert.AreEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, 60);

            Assert.AreEqual(4, line.Runs.Count);

            var imgRun = line.Runs[0] as PDFLayoutComponentRun;
            var calcImgWidth = (60.0 / ImageNaturalHeight.PointsValue) * ImageNaturalWidth.PointsValue;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(60.0, imgRun.Height.PointsValue);
            AssertAreApproxEqual(calcImgWidth, imgRun.Width.PointsValue, "Image not scales proportionately");

            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            var txtBegin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            AssertAreApproxEqual(txtBegin.StartTextCursor.Width.PointsValue, calcImgWidth + 10); //text begins after the image width + page margin
            Assert.AreEqual(txtBegin.StartTextCursor.Height, 60 + 10); //text is baseline with the image height

            var txtChars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space 
            Assert.AreEqual(" 3. Inline at explicit 60pt height", txtChars.Characters);

            var txtEnd = line.Runs[3] as PDFTextRunEnd;
            Assert.IsNotNull(txtEnd);
            Assert.AreEqual(0, txtEnd.Width);
        }


        [TestMethod]
        public void InlineImages4ExplicitWidthFlowing()
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
            pg.FontSize = 20;
            doc.Pages.Add(pg);

            Image img;

            img = new Image();
            img.Source = path;
            img.ID = "Image4";
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Width = 100;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 4. Inline with explicit width with content overflowing onto multiple lines"));




            using (var stream = DocStreams.GetOutputStream("Images_Inlining_4ExplicitWidthFlowing.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            var expectedImgHeight = (100.0 / ImageNaturalWidth.PointsValue) * ImageNaturalHeight.PointsValue;

            
            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];
            
            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;


            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            AssertAreApproxEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, expectedImgHeight);
            

            Assert.AreEqual(4, line.Runs.Count);

            var imgRun = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(100.0, imgRun.Width.PointsValue);
            AssertAreApproxEqual(expectedImgHeight, imgRun.Height.PointsValue);

            
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            var txtBegin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);

            var txtChars = line.Runs[2] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual(" 4. Inline with explicit width with content overflowing", txtChars.Characters);

            var txtBr = line.Runs[3] as PDFTextRunNewLine;
            Assert.AreEqual(24.0, txtBr.NewLineOffset.Height.PointsValue);

            //Second line
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);

            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("onto multiple lines", txtChars.Characters);
            
            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);


        }



        [TestMethod]
        public void InlineImages5ExplicitHeightMidLine()
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
            pg.FontSize = 20;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral(" 5. Inline with explicit height "));

            var img = new Image();
            img.ID = "Image5";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" within the content of the text on following line that will be text points high."));

            var expectedImgWidth = (60.0 / ImageNaturalHeight.PointsValue) * ImageNaturalWidth.PointsValue;


            using (var stream = DocStreams.GetOutputStream("Images_Inlining_5FixedHeightMidLine.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            //1. Natural Size
            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];

            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;

            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            AssertAreApproxEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, 60);

            Assert.AreEqual(7, line.Runs.Count);

            var txtBegin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);

            var txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space at the end 
            Assert.AreEqual("5. Inline with explicit height ", txtChars.Characters);

            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);

            

            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun);
            Assert.AreEqual(60.0, imgRun.Height.PointsValue);
            AssertAreApproxEqual((60.0 / ImageNaturalHeight.PointsValue) * ImageNaturalWidth.PointsValue, imgRun.Width.PointsValue, "Image not scales proportionately");

            
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            txtBegin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            var expectedInset = txtChars.Width.PointsValue + imgRun.Width.PointsValue;
            AssertAreApproxEqual(expectedInset, txtBegin.LineInset.PointsValue);

            txtChars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space 
            Assert.AreEqual(" within the content of the", txtChars.Characters);

            var txtBr = line.Runs[6] as PDFTextRunNewLine;
            Assert.IsNotNull(txtBr);
            Assert.AreEqual(0, txtBr.Width);
            Assert.AreEqual(24.0, txtBr.NewLineOffset.Height.PointsValue);

            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(3, line.Runs.Count);

            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(txtSpace);
            expectedInset = 0.0;
            AssertAreApproxEqual(expectedInset, txtSpace.Width.PointsValue);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space 
            Assert.AreEqual("text on following line that will be text points high.", txtChars.Characters);

            txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);
        }


        [TestMethod]
        public void InlineImages6MultipleImagesFixedSizes()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, ImagePath);
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = 20;
            pg.Contents.Add(new TextLiteral("6. Inline with explicit height "));

            var img = new Image();
            img.ID = "Image6";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" then a second image on a following line that will be 80 points high "));

            img = new Image();
            img.ID = "Image6";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 80;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" after the second image flowing onto a new line."));



            using (var stream = DocStreams.GetOutputStream("Images_Inlining_6MultipleImagesFixedSize.pdf"))
            {
                doc.ConformanceMode = ParserConformanceMode.Strict;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            //TODO: Currently the images sit above the baseline. If so, then they should add the descender height
            //to the height of the image (but this is the descender height of the line above. If they sit at the bottom
            //then they should be aligned to the bottom od the descender.

            Assert.IsNotNull(layout, "The layout was not saved from the event");


            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];
            Assert.AreEqual(3, reg.Contents.Count);

            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            AssertAreApproxEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, 60);

            Assert.AreEqual(7, line.Runs.Count);

            var txtBegin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);

            var txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space at the end 
            Assert.AreEqual("6. Inline with explicit height ", txtChars.Characters);

            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);

            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun, "No Image");
            Assert.AreEqual(60, imgRun.Height);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            txtBegin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            //Check the offset of the begin, beyond the first text and the image
            var expectedInset = txtChars.Width.PointsValue + ((60 / ImageNaturalHeight.PointsValue) * ImageNaturalWidth.PointsValue);
            AssertAreApproxEqual(expectedInset, txtBegin.LineInset.PointsValue);


            txtChars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);

            //Important to keep the space 
            Assert.AreEqual(" then a second image on a", txtChars.Characters);

            var txtBr = line.Runs[6] as PDFTextRunNewLine;
            //Should the max height of the line - which is the 80 point second image
            Assert.AreEqual(80 + txtBegin.TextRenderOptions.GetDescender(), txtBr.NewLineOffset.Height.PointsValue);


            //Second line 
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.AreEqual(7, line.Runs.Count);
            
            var h = txtBegin.TextRenderOptions.GetDescender(); //descender + half the leading (1.2 line height) below the baseline
            h += (txtBegin.TextRenderOptions.GetLineHeight() - txtBegin.TextRenderOptions.GetSize()) / 2;
            h += 80; //+ image height aligned to the baseline
            Assert.AreEqual(h, line.Height);

            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("following line that will be 80 points high ", txtChars.Characters);

            txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);


            imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun, "No Second Image");
            Assert.AreEqual(80, imgRun.Height);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            txtBegin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            //Check the offset of the begin, beyond the first text and the image
            expectedInset = txtChars.Width.PointsValue + imgRun.Width.PointsValue;
            AssertAreApproxEqual(expectedInset, txtBegin.LineInset.PointsValue);


            txtChars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);

            //Important to keep the space 
            Assert.AreEqual(" after the", txtChars.Characters);

            txtBr = line.Runs[6] as PDFTextRunNewLine;
            Assert.AreEqual(0, txtBr.Width);
            //Back to normal line height
            Assert.AreEqual(24, txtBr.NewLineOffset.Height.PointsValue);


            //Third line
            line = reg.Contents[2] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.AreEqual(24, line.Height);

            txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, txtSpace.Width);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            Assert.AreEqual("second image flowing onto a new line.", txtChars.Characters);


            txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);

        }


        [TestMethod]
        public void InlineImages7MultipleSameLineDifferentHeight()
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
            pg.FontSize = 20;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral("7. Inline "));

            var img = new Image();
            img.ID = "Image1";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 40;
            
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" explicit height "));

            img = new Image();
            img.ID = "Image2";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.DisplayMode = DisplayMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" within the content of the text on following line that will be text points high."));

            using (var stream = DocStreams.GetOutputStream("Images_Inline7MultipleImagesDifferentSize.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            //
            // TODO: The inline images need knocking down to the base line if the image alignment is not set on the line.
            //

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            var lpg = layout.AllPages[0];
            var reg = lpg.ContentBlock.Columns[0];
            Assert.AreEqual(2, reg.Contents.Count);

            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var halflead = (20 * 0.2) / 2; //half leading between the last descender and the bottom
            Assert.IsNotNull(line);
            AssertAreApproxEqual(line.Height.PointsValue - line.BaseLineToBottom.PointsValue - halflead, 60);

            Assert.AreEqual(11, line.Runs.Count);

            var txtBegin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);

            var txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space at the end 
            Assert.AreEqual("7. Inline ", txtChars.Characters);


            var txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);


            var imgRun = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun, "No Image");
            Assert.AreEqual(40, imgRun.Height);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            txtBegin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);
            //Check the offset of the begin, beyond the first text and the image
            var expectedInset = txtChars.Width.PointsValue + imgRun.Width.PointsValue;
            AssertAreApproxEqual(expectedInset, txtBegin.LineInset.PointsValue);

            txtChars = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);

            //Important to keep the space 
            Assert.AreEqual(" explicit height ", txtChars.Characters);

            txtEnd = line.Runs[6] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);

            imgRun = line.Runs[7] as PDFLayoutComponentRun;
            Assert.IsNotNull(imgRun, "No Image");
            Assert.AreEqual(60, imgRun.Height);
            Assert.AreEqual(0, imgRun.OffsetX);
            Assert.AreEqual(0, imgRun.OffsetY);

            txtBegin = line.Runs[8] as PDFTextRunBegin;
            Assert.IsNotNull(txtBegin);

            //Check the offset of the begin,first offset + beyond the second text and the image
            expectedInset = expectedInset + txtChars.Width.PointsValue + imgRun.Width.PointsValue;
            AssertAreApproxEqual(expectedInset, txtBegin.LineInset.PointsValue);

            txtChars = line.Runs[9] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);

            //Important to keep the space 
            Assert.AreEqual(" within the content of the", txtChars.Characters);

            var txtBr = line.Runs[10] as PDFTextRunNewLine;
            Assert.AreEqual(0, txtBr.Width);
            Assert.AreEqual(24, txtBr.NewLineOffset.Height);

            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(24, line.Height.PointsValue);

            Assert.AreEqual(3, line.Runs.Count);

            var txtSpace = line.Runs[0] as PDFTextRunSpacer;
            Assert.IsNotNull(txtBegin);

            txtChars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(txtChars);
            //Important to keep the space at the end 
            Assert.AreEqual("text on following line that will be text points high.", txtChars.Characters);


            txtEnd = line.Runs[2] as PDFTextRunEnd;
            Assert.AreEqual(0, txtEnd.Width);
           
        }
    }
}
