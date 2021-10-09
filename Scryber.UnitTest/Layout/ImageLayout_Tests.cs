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

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass()]
    public class ImageLayoutTests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            this.layout = args.Context.DocumentLayout;
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

        private void AssertAreApproxEqual(double one, double two, string message)
        {
            int precision = 5;
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }

        [TestMethod()]
        public void JustATextRun()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over multiple lines in the page with a default line height"));
        }


        [TestMethod]
        public void FixedSizes()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72);
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0);

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
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
            img.Width = new PDFUnit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = new PDFUnit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt High"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = new PDFUnit(100);
            img.Width = new PDFUnit(100);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("100pt Both"));

            using (var stream = DocStreams.GetOutputStream("Images_FixedSizes.pdf"))
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

            //100pt wide
            width = 100;
            height = naturalHeight.PointsValue * (100.0 / naturalWidth.PointsValue);
            lrun = GetBlockImageRunForPage(1);

            AssertAreApproxEqual(width, lrun.Width.PointsValue, "Width does not match for 100pt wide");
            AssertAreApproxEqual(height, lrun.Height.PointsValue, "Height does not match for 100pt wide");

            //100pt high
            width =  naturalWidth.PointsValue * (100.0 / naturalHeight.PointsValue);
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72);
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0);

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);


            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumWidth = new PDFUnit(600); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumWidth = new PDFUnit(200); //smaller so should reduce the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 200pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new PDFUnit(200); //smaller so should reduce height and width proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 200pt High"));


            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new PDFUnit(200); //smaller so should reduce height and width proportionally
            img.MaximumWidth = new PDFUnit(400);
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Max 400pt wide and 200pt High"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MaximumHeight = new PDFUnit(400); //smaller so should reduce height and width proportionally
            img.MaximumWidth = new PDFUnit(200);
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);


            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new PDFUnit(400); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size - min width 400pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumHeight = new PDFUnit(300); // bigger than the image so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Natural Size - min height 300"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new PDFUnit(550); //larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 550pt Wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumHeight = new PDFUnit(350); //larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 350pt High"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new PDFUnit(550);
            img.MinimumHeight = new PDFUnit(350); //width larger so should increase the width and height proportionally
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Min 550 wide and 350pt High - 550 wide"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.MinimumWidth = new PDFUnit(500);
            img.MinimumHeight = new PDFUnit(350); //height larger so should increase the width and height proportionally
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 200;
            img.MinimumHeight = new PDFUnit(100); // smaller so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and min height 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 200;
            img.MinimumHeight = new PDFUnit(300); // larger so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and min height 300pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MinimumWidth = new PDFUnit(100); // smaller so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Heigth 200pt and min width 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MinimumWidth = new PDFUnit(500); // larger so will change
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 300;
            img.MaximumHeight = new PDFUnit(300); //1. larger so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and max height 300pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Width = 300;
            img.MaximumHeight = new PDFUnit(100); //2. smaller so will change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Width 200pt and max height 100pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MaximumWidth = new PDFUnit(500); //3. larger so no change
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("Height 250pt and max width 500pt"));

            pg.Contents.Add(new PageBreak());

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.Height = 250;
            img.MaximumWidth = new PDFUnit(200); //4. smaller so will change
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new PDFThickness(10);
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

            pg.Margins = new PDFThickness(10);
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

            pg.Margins = new PDFThickness(10);
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
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 100;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            var img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            //1. No Explicit sizes so natural
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("1. No Explicit sizes - shrink"));



            pg = new Page();
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            div = new Div();
            div.Height = Papers.GetSizeInDeviceIndependentUnits(PaperSize.A4).Height - 50;
            div.BorderColor = StandardColors.Aqua;
            pg.Contents.Add(div);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral("6. No Size, but cannot go below 20% shrink"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
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
            pg.Margins = new PDFThickness(10);
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
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            //bottom of first page
            lrun = GetBlockImageRunForPage(0, 0, 1);

            //1. Natural size in container
            var width = naturalWidth.PointsValue * (80.0 / naturalHeight.PointsValue);
            var height = 80.0; //available space (100 - margins)

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
        public void InlineMultipleImageLineHeight()
        {
            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();

            var pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral("5. Inline "));

            var img = new Image();
            img.ID = "Image1";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 40;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" explicit height "));

            img = new Image();
            img.ID = "Image2";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" within the content of the text on following line that will be text points high"));

            using (var stream = DocStreams.GetOutputStream("Images_InlineLineHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;

            lrun = GetInlineImageRunForPage(0, 0, 0, 3); //BT Tx ET Img

            //5. Explicit height @60pt within the content
            var width = naturalWidth.PointsValue * (60.0 / naturalHeight.PointsValue);
            var height = 60.0;

            Assert.Inconclusive("We know this is not yet working");

            //AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for explicit h within the content on image " + lrun.Owner.ID);
            //AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for explicit h within the content on image " + lrun.Owner.ID);
            //Assert.AreEqual(height, lrun.Line.BaseLineOffset, "The base line offset of the line was not the height of the image");
        }

        [TestMethod]
        public void InlineSizes()
        {

            //Toroid32.png - 682 × 452 pixels natural size @96 ppi
            var naturalWidth = new PDFUnit((682.0 / 96.0) * 72); //551.5pt
            var naturalHeight = new PDFUnit((452.0 / 96.0) * 72.0); //339pt

            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Images/Toroid32.png");
            path = System.IO.Path.GetFullPath(path);

            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the base path to the image to use for the tests");

            var doc = new Document();
            var pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            Image img;

            img = new Image();
            img.Source = path;
            img.ID = "Image1";
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 1. Inline at text size"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = 80;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.ID = "Image2";
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 2. Inline at text size for 80pt"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.ID = "Image3";
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 3. Inline at explicit 60pt height"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.ID = "Image4";
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Width = 100;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 4. Inline with explicit width"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral(" 5. Inline with explicit height "));

            img = new Image();
            img.ID = "Image5";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" within the content of the text on following line that will be text points high"));


            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral(" 6. Inline with explicit height "));

            img = new Image();
            img.ID = "Image6";
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" then a second image on another following line that will be 80 points high"));

            //img = new Image();
            //img.ID = "Image6";
            //img.Source = path;
            //img.BorderColor = PDFColors.Black;
            //img.PositionMode = PositionMode.Inline;
            //img.Height = 80;
            //pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" after the second image"));

            /* pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new PDFColor(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;


            pg.Contents.Add(new TextLiteral(" 6. Inline at "));
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 60pt height within the content"));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new PDFColor(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg); 

            img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 60;

            pg.Contents.Add(new TextLiteral(" 7. Inline at "));
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" 60pt height and then a second image "));

            img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.VerticalAlignment = VerticalAlignment.Bottom;
            pg.Contents.Add(img);

            pg.Contents.Add(new TextLiteral(" at general height in the content."));

            pg = new Page();
            pg.Margins = new PDFThickness(10);
            pg.BackgroundColor = new PDFColor(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.TextLeading = 40;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral(" 8. Inline within the content of the paragraph with explicit line leading "));

            img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.PositionMode = PositionMode.Inline;
            img.Height = 100;
            pg.Contents.Add(img);
            pg.Contents.Add(new TextLiteral(" that push out the leading space in the paragraph")); */

            using (var stream = DocStreams.GetOutputStream("Images_Inlining.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout, "The layout was not saved from the event");

            PDFLayoutComponentRun lrun;


            //As we are inline we use the content rects to make sure we are the right size
            
            lrun = GetInlineImageRunForPage(0, 0, 0);

            //1. Natural size in container
            var width = naturalWidth.PointsValue;
            var height = naturalHeight.PointsValue;

            AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for natural in the space on image " + lrun.Owner.ID);
            AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for natural in the space on image " + lrun.Owner.ID);


            lrun = GetInlineImageRunForPage(1, 0, 0);

            //2. 80pt font size in container
            width = naturalWidth.PointsValue;
            height = naturalHeight.PointsValue; //Default size

            AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for natural on image " + lrun.Owner.ID);
            AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for natural in the space on image " + lrun.Owner.ID);

            lrun = GetInlineImageRunForPage(2, 0, 0);

            //3. Explicit height
            width = naturalWidth.PointsValue * (60.0 / naturalHeight.PointsValue);
            height = 60.0;

            AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for explicit h in the space on image " + lrun.Owner.ID);
            AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for explicit h in the space on image " + lrun.Owner.ID);


            lrun = GetInlineImageRunForPage(3, 0, 0);

            //4. Explicit width
            width = 100.0;
            height = naturalHeight.PointsValue * (100.0 / naturalWidth.PointsValue);

            AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for explicit w in the space on image " + lrun.Owner.ID);
            AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for explicit w in the space on image " + lrun.Owner.ID);


            lrun = GetInlineImageRunForPage(4, 0, 0, 3); //BT Tx ET Img

            //5. Explicit height @60pt within the content
            width = naturalWidth.PointsValue * (60.0 / naturalHeight.PointsValue);
            height = 60.0;

            AssertAreApproxEqual(width, lrun.ContentRect.Width.PointsValue, "Width does not match for explicit h within the content on image " + lrun.Owner.ID);
            AssertAreApproxEqual(height, lrun.ContentRect.Height.PointsValue, "Height does not match for explicit h within the content on image " + lrun.Owner.ID);
            Assert.AreEqual(height, lrun.Line.BaseLineOffset, "The base line offset of the line was not the height of the image");
        }

    }
}
