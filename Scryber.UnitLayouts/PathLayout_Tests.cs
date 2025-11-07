using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF.Layout;
using Scryber.PDF;


namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class PathLayout_Tests
    {
        public PathLayout_Tests()
        {
        }

        private PDFLayoutDocument layout;

        /// <summary>
        /// Captures the document layout complete event and stores a reference to the layout in the instance variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        private void AssertAreApproxEqual(double one, double two, string message = null)
        {
            one = Math.Round(one, 1);
            two = Math.Round(two, 1);

            Assert.AreEqual(one, two, message);
        }


        /// <summary>
        /// A single graphic line (on it's own layout line).
        /// </summary>
        [TestMethod()]
        public void LineBlockTest()
        {
            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Line();
            page.Contents.Add(path);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Line();
            path2.Height = 20;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Line();
            path3.Height = 20;
            path3.FullWidth = true;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Line();
            path4.Height = 20;
            path4.Width = 200;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));



            using (var ms = DocStreams.GetOutputStream("Paths_SingleLine.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Default height = 0, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            var line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

           
            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(0.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count);

            var graphicLine = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path);

            Assert.AreEqual(graphicLine.Width, reg.Width);
            Assert.AreEqual(0.0, graphicLine.Height);

            //Second Page - Height 20 (implied width 0)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(0.0, line.Width);
            Assert.AreEqual(20.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count);

            graphicLine = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path2);

            Assert.AreEqual(0.0, graphicLine.Width);
            Assert.AreEqual(20.0, graphicLine.Height);


            //Third Page - Height 20, full width
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(20.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count);

            graphicLine = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path3);

            Assert.AreEqual(graphicLine.Width, reg.Width);
            Assert.AreEqual(20.0, graphicLine.Height);


            //Fourth page - Height 20, Width 200
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(200.0, line.Width.PointsValue);
            Assert.AreEqual(20.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count);

            graphicLine = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path4);

            Assert.AreEqual(200.0, graphicLine.Width.PointsValue);
            Assert.AreEqual(20.0, graphicLine.Height.PointsValue);
        }

        /// <summary>
        /// A single graphic line (on it's own layout line).
        /// </summary>
        [TestMethod()]
        public void LineInlineTest()
        {
            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Line();
            path.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Line();
            path2.Height = 20;
            path2.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Line();
            path3.Height = 20;
            path3.DisplayMode = Drawing.DisplayMode.Inline;
            path3.Width = Unit.Auto;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Line();
            path4.Height = 20;
            path4.Width = 200;
            path4.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));



            using (var ms = DocStreams.GetOutputStream("Paths_SingleInLineLine.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Default height = 0, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            Assert.AreEqual(line.Width, reg.Width);
            AssertAreApproxEqual(28.8, line.Height.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); //3 text + line + begin and newline

            var chars = line.Runs[1] as PDFTextRunCharacter;
            var graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path);

            Assert.AreEqual(reg.Width, graphicLine.Width + chars.Width);
            Assert.AreEqual(0.0, graphicLine.Height);

            

            //Second Page - Height 20 (implied width 0)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 1 line
            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            
            AssertAreApproxEqual(28.8, line.Height.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); //3 text + line + 3 text

            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path2);

            Assert.AreEqual(0.0, graphicLine.Width);
            Assert.AreEqual(20.0, graphicLine.Height);

            

            //Third Page - Height 20, full width, After on new line
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(line.Width, reg.Width);
            AssertAreApproxEqual(28.8, line.Height.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); //3 text, line, begin, newline

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path3);

            AssertAreApproxEqual(reg.Width.PointsValue, graphicLine.Width.PointsValue + chars.Width.PointsValue);
            Assert.AreEqual(20.0, graphicLine.Height);

            

            //Fourth page - Height 20, Width 200
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(28.8, line.Height.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); // 3 text, line, 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);
            Assert.IsNotNull(chars2);

            Assert.AreEqual(graphicLine.Owner, path4);

            Assert.AreEqual(200.0, graphicLine.Width.PointsValue);
            Assert.AreEqual(20.0, graphicLine.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, chars.Width.PointsValue + graphicLine.Width.PointsValue + chars2.Width.PointsValue);
        }


        /// <summary>
        /// A single graphic line (on it's own layout line).
        /// </summary>
        [TestMethod()]
        public void LineInlineWithPaddingTest()
        {
            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Line();
            path.DisplayMode = Drawing.DisplayMode.Inline;
            path.Padding = new Drawing.Thickness(10);
            path.BackgroundColor = Drawing.StandardColors.White;
            page.Contents.Add(path);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Line();
            path2.Height = 20;
            path2.Padding = new Drawing.Thickness(10);
            path2.BackgroundColor = Drawing.StandardColors.White;
            path2.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Line();
            path3.Height = 20;
            path3.Padding = new Drawing.Thickness(10);
            path3.BackgroundColor = Drawing.StandardColors.White;
            path3.DisplayMode = Drawing.DisplayMode.Inline;
            path3.Width = Unit.Auto;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            doc.Pages.Add(page);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            page.Margins = new Drawing.Thickness(20);
            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Line();
            path4.Height = 20;
            path4.Width = 200;
            path4.BackgroundColor = Drawing.StandardColors.White;
            path4.Padding = new Drawing.Thickness(10);
            path4.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));



            using (var ms = DocStreams.GetOutputStream("Paths_SingleInLineLineWithPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Default height = 0, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            Assert.AreEqual(line.Width, reg.Width);
            AssertAreApproxEqual(28.8, line.Height.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); //3 text + line + begin and newline

            var chars = line.Runs[1] as PDFTextRunCharacter;
            var graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path);

            Assert.AreEqual(reg.Width, graphicLine.Width + chars.Width);
            Assert.AreEqual(20.0, graphicLine.Height.PointsValue);// inc padding



            //Second Page - Height 20 (implied width 0)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 1 line
            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(40, line.BaseLineOffset.PointsValue); //20pt line + padding

            Assert.AreEqual(7, line.Runs.Count); //3 text + line + 3 text

            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path2);

            Assert.AreEqual(20.0, graphicLine.Width);  //0 + padding
            Assert.AreEqual(40.0, graphicLine.Height); //20 + padding



            //Third Page - Height 20 + padding, full width, After on new line
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(line.Width, reg.Width);
            AssertAreApproxEqual(40.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); //3 text, line, begin, newline

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);

            Assert.AreEqual(graphicLine.Owner, path3);

            AssertAreApproxEqual(reg.Width.PointsValue, graphicLine.Width.PointsValue + chars.Width.PointsValue);
            Assert.AreEqual(40.0, graphicLine.Height);



            //Fourth page - Height 20, Width 200
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(40.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); // 3 text, line, 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphicLine = line.Runs[3] as PDFLayoutComponentRun;
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphicLine);
            Assert.IsNotNull(chars2);

            Assert.AreEqual(graphicLine.Owner, path4);

            Assert.AreEqual(220.0, graphicLine.Width.PointsValue); //explicit width + padding
            Assert.AreEqual(40.0, graphicLine.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, chars.Width.PointsValue + graphicLine.Width.PointsValue + chars2.Width.PointsValue);
        }


        [TestMethod()]
        public void EllipseBlockTest()
        {

            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Ellipse();
            //No Size should fill the container.
            page.Contents.Add(path);
            //TODO: not visible as we cannot overflow, but causes an exception
            //page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Ellipse();
            path2.Height = 40;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Ellipse();
            path3.Height = 100;
            path3.Width = 100;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Ellipse();
            path4.Height = 100;
            path4.Width = 100;
            path4.Padding = 20;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));


            using (var ms = DocStreams.GetOutputStream("Paths_SingleEllipse.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Second Line, full height, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var h = line.Height;

            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(line.Height, reg.Height - h);

            Assert.AreEqual(1, line.Runs.Count); //ellipse only

            var graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path);

            Assert.AreEqual(reg.Width, graphic.Width );
            Assert.AreEqual(reg.Height - h, graphic.Height.PointsValue);

            

            //Second Page - Height 40 (implied width = region width)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 1 line
            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(40, line.Height.PointsValue); //20pt line + padding

            Assert.AreEqual(1, line.Runs.Count); //ellipse only

            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path2);

            Assert.AreEqual(reg.Width, graphic.Width);
            Assert.AreEqual(40.0, graphic.Height);

            

            //Third Page - Height 100, Width 100
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(100.0, line.Width.PointsValue);
            Assert.AreEqual(100.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count); //graphic

            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path3);

            Assert.AreEqual(100.0, graphic.Width.PointsValue);
            Assert.AreEqual(100.0, graphic.Height.PointsValue);


            

            //Fourth page - Height 20, Width 200
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count); //All on 1 line
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(140.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count); // 3 text, line, 3 text

            
            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);


            Assert.AreEqual(graphic.Owner, path4);

            Assert.AreEqual(140.0, graphic.Width.PointsValue); //explicit width + padding
            Assert.AreEqual(140.0, graphic.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue);
        }


        [TestMethod()]
        public void EllipseInlineTest()
        {

            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Ellipse();
            path.DisplayMode = Drawing.DisplayMode.Inline;
            //No Size should fill the container.
            page.Contents.Add(path);
            //TODO: Sort out the overflow
            //page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Ellipse();
            path2.Height = 40;
            path2.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Ellipse();
            path3.Height = 100;
            path3.Width = 100;
            path3.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Ellipse();
            path4.Height = 100;
            path4.Width = 100;
            path4.DisplayMode = Drawing.DisplayMode.Inline;
            path4.Padding = 20;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path5 = new Ellipse();
            path5.Height = 100;
            path5.Width = 450; //Force a new line for the width
            path5.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path5);

            page.Contents.Add(new TextLiteral("After"));


            using (var ms = DocStreams.GetOutputStream("Paths_SingleInlineEllipse.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Second Line, full height, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(line.Height, reg.Height);

            Assert.AreEqual(4, line.Runs.Count); //3 text + ellipse
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars);

            Assert.AreEqual(graphic.Owner, path);

            Assert.AreEqual(reg.Width, graphic.Width + chars.Width);
            Assert.AreEqual(line.BaseLineOffset, graphic.Height.PointsValue);

            

            //Second Page - Height 40 (implied width = region width - 'Before' width)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 2 lines
            Assert.AreEqual(2, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(40, line.BaseLineOffset.PointsValue); //40pt ellipse

            Assert.AreEqual(6, line.Runs.Count); //3 text + ellipse + text begin and new line

            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path2);

            Assert.AreEqual(reg.Width, graphic.Width + chars.Width);
            Assert.AreEqual(40.0, graphic.Height);

            

            //Third Page - Height 100, Width 100 inline
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); //3  text + graphic + 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);

            Assert.AreEqual(graphic.Owner, path3);

            Assert.AreEqual(100.0, graphic.Width.PointsValue);
            Assert.AreEqual(100.0, graphic.Height.PointsValue);

            Assert.AreEqual(line.Width, chars.Width + graphic.Width + chars2.Width);


            //Fourth page - Height 100, Width 100, Padding 20
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(140.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); // 3 text, ellipse, 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);


            Assert.AreEqual(graphic.Owner, path4);

            Assert.AreEqual(140.0, graphic.Width.PointsValue); //explicit width + padding
            Assert.AreEqual(140.0, graphic.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue + chars2.Width.PointsValue);





            //Fifth page - Height 100, Width 450, force new line with the text
            reg = layout.AllPages[4].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); // 3 text, line, begin, newline

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);


            Assert.AreEqual(graphic.Owner, path5);

            Assert.AreEqual(100.0, graphic.Height.PointsValue); //explicit width + padding
            Assert.AreEqual(450.0, graphic.Width.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue);
        }



        /// Rectangles for completeness

        [TestMethod()]
        public void RectangleBlockTest()
        {

            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Rectangle();
            //No Size should fill the container.
            page.Contents.Add(path);
            //TODO: not visible as we cannot overflow, but causes an exception
            //page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Rectangle();
            path2.Height = 40;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Rectangle();
            path3.Height = 100;
            path3.Width = 100;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Rectangle();
            path4.Height = 100;
            path4.Width = 100;
            path4.Padding = 20;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));


            using (var ms = DocStreams.GetOutputStream("Paths_SingleRectangle.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Second Line, full height, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            var h = line.Height;

            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(line.Height, reg.Height - h);

            Assert.AreEqual(1, line.Runs.Count); //ellipse only

            var graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path);

            Assert.AreEqual(reg.Width, graphic.Width);
            Assert.AreEqual(reg.Height - h, graphic.Height.PointsValue);



            //Second Page - Height 40 (implied width = region width)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 1 line
            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(40, line.Height.PointsValue); //20pt line + padding

            Assert.AreEqual(1, line.Runs.Count); //ellipse only

            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path2);

            Assert.AreEqual(reg.Width, graphic.Width);
            Assert.AreEqual(40.0, graphic.Height);



            //Third Page - Height 100, Width 100
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count);
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(100.0, line.Width.PointsValue);
            Assert.AreEqual(100.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count); //graphic

            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path3);

            Assert.AreEqual(100.0, graphic.Width.PointsValue);
            Assert.AreEqual(100.0, graphic.Height.PointsValue);




            //Fourth page - Height 20, Width 200
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(3, reg.Contents.Count); //All on 1 line
            line = reg.Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(140.0, line.Height.PointsValue);

            Assert.AreEqual(1, line.Runs.Count); // 3 text, line, 3 text


            graphic = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);


            Assert.AreEqual(graphic.Owner, path4);

            Assert.AreEqual(140.0, graphic.Width.PointsValue); //explicit width + padding
            Assert.AreEqual(140.0, graphic.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue);
        }

        [TestMethod()]
        public void RectangleInlineTest()
        {

            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Rectangle();
            path.DisplayMode = Drawing.DisplayMode.Inline;
            //No Size should fill the container.
            page.Contents.Add(path);
            //TODO: Sort out the overflow
            //page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Rectangle();
            path2.Height = 40;
            path2.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Rectangle();
            path3.Height = 100;
            path3.Width = 100;
            path3.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Rectangle();
            path4.Height = 100;
            path4.Width = 100;
            path4.DisplayMode = Drawing.DisplayMode.Inline;
            path4.Padding = 20;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path5 = new Rectangle();
            path5.Height = 100;
            path5.Width = 450; //Force a new line for the width
            path5.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path5);

            page.Contents.Add(new TextLiteral("After"));


            using (var ms = DocStreams.GetOutputStream("Paths_SingleInlineRectangle.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Second Line, full height, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(line.Height, reg.Height);

            Assert.AreEqual(4, line.Runs.Count); //3 text + ellipse
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars);

            Assert.AreEqual(graphic.Owner, path);

            Assert.AreEqual(reg.Width, graphic.Width + chars.Width);
            Assert.AreEqual(line.BaseLineOffset, graphic.Height);



            //Second Page - Height 40 (implied width = region width - 'Before' width)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 2 lines
            Assert.AreEqual(2, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(40, line.BaseLineOffset.PointsValue); //40pt ellipse

            Assert.AreEqual(6, line.Runs.Count); //3 text + ellipse + text begin and new line

            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path2);

            Assert.AreEqual(reg.Width, graphic.Width + chars.Width);
            Assert.AreEqual(40.0, graphic.Height);



            //Third Page - Height 100, Width 100 inline
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); //3  text + graphic + 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);

            Assert.AreEqual(graphic.Owner, path3);

            Assert.AreEqual(100.0, graphic.Width.PointsValue);
            Assert.AreEqual(100.0, graphic.Height.PointsValue);

            Assert.AreEqual(line.Width, chars.Width + graphic.Width + chars2.Width);


            //Fourth page - Height 100, Width 100, Padding 20
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(100.0 + 40.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); // 3 text, ellipse, 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);


            Assert.AreEqual(graphic.Owner, path4);

            Assert.AreEqual(140.0, graphic.Width.PointsValue); //explicit width + padding
            Assert.AreEqual(140.0, graphic.Height.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue + chars2.Width.PointsValue);





            //Fifth page - Height 100, Width 450, force new line with the text
            reg = layout.AllPages[4].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); // 3 text, line, begin, newline

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);


            Assert.AreEqual(graphic.Owner, path5);

            Assert.AreEqual(100.0, graphic.Height.PointsValue); //explicit width + padding
            Assert.AreEqual(450.0, graphic.Width.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue);
        }


        [TestMethod()]
        public void PolygonInlineTest()
        {

            var doc = new Document();
            var page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path = new Polygon() { VertexCount = 5, Closed = true, VertexStep = 2};
            path.DisplayMode = Drawing.DisplayMode.Inline;
            //No Size should fill the container.
            page.Contents.Add(path);
            

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path2 = new Polygon() { VertexCount = 3, Closed = true, VertexStep = 1 };
            path2.Height = 40;
            path2.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path2);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path3 = new Polygon() { VertexCount = 6, Closed = true, VertexStep = 2 };
            path3.Height = 100;
            path3.Width = 100;
            path3.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path3);

            page.Contents.Add(new TextLiteral("After"));

            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path4 = new Polygon() { VertexCount = 7, Closed = true, VertexStep = 1 };
            path4.Height = 100;
            path4.Width = 100;
            path4.DisplayMode = Drawing.DisplayMode.Inline;
            path4.Padding = 20;
            page.Contents.Add(path4);

            page.Contents.Add(new TextLiteral("After"));


            page = new Page();
            page.Margins = new Drawing.Thickness(20);
            page.BackgroundColor = Drawing.StandardColors.Silver;
            doc.Pages.Add(page);

            page.Contents.Add(new TextLiteral("Before"));

            var path5 = new Polygon() { VertexCount = 8, Closed = true, VertexStep = 2 };
            path5.Height = 100;
            path5.Width = 450; //Force a new line for the width
            path5.DisplayMode = Drawing.DisplayMode.Inline;
            page.Contents.Add(path5);

            page.Contents.Add(new TextLiteral("After"));


            using (var ms = DocStreams.GetOutputStream("Paths_SingleInlinePolygon.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.IsNotNull(layout);

            //First page - Second Line, full height, full width

            var reg = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            Assert.AreEqual(line.Width, reg.Width);
            Assert.AreEqual(line.Height, reg.Height);

            Assert.AreEqual(4, line.Runs.Count); //3 text + ellipse
            var chars = line.Runs[1] as PDFTextRunCharacter;
            var graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars);

            Assert.AreEqual(graphic.Owner, path);

            Assert.AreEqual(reg.Width, graphic.Width + chars.Width);
            Assert.AreEqual(line.BaseLineOffset, graphic.Height.PointsValue);



            //Second Page - Height 40 (implied width = region width - 'Before' width)

            reg = layout.AllPages[1].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            //All on 1 line
            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);


            AssertAreApproxEqual(30, line.BaseLineOffset.PointsValue); //Triangle is transcribed by the circle containing it, and will result in a reduced point path height

            Assert.AreEqual(7, line.Runs.Count); //3 text + poly + text begin and new line

            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars = line.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);

            Assert.AreEqual(graphic.Owner, path2);
            
            AssertAreApproxEqual(30.0, graphic.Height.PointsValue);



            //Third Page - Height 100, Width 100 inline
            reg = layout.AllPages[2].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count);
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            //Make sure the line is the full width of the region.
            Assert.AreEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); //3  text + graphic + 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            var chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);

            Assert.AreEqual(graphic.Owner, path3);

            AssertAreApproxEqual(86.6, graphic.Width.PointsValue); //6 sides has a reduced width
            Assert.AreEqual(100.0, graphic.Height.PointsValue);

            Assert.AreEqual(line.Width, chars.Width + graphic.Width + chars2.Width);


            //Fourth page - Height 100, Width 100, Padding 20
            reg = layout.AllPages[3].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(1, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(135.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(7, line.Runs.Count); // 3 text, poly, 3 text

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            chars2 = line.Runs[5] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);
            Assert.IsNotNull(graphic);
            Assert.IsNotNull(chars2);


            Assert.AreEqual(graphic.Owner, path4);

            AssertAreApproxEqual(137.5, graphic.Width.PointsValue); //Reduced size for each points
            AssertAreApproxEqual(135.0, graphic.Height.PointsValue); //Reduced size for the points
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue + chars2.Width.PointsValue);


            //Fifth page - Height 100, Width 450, force new line with the text
            reg = layout.AllPages[4].ContentBlock.Columns[0];
            Assert.IsInstanceOfType(reg, typeof(PDFLayoutRegion));

            Assert.AreEqual(2, reg.Contents.Count); //All on 1 line
            line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);

            AssertAreApproxEqual(100.0, line.BaseLineOffset.PointsValue);

            Assert.AreEqual(6, line.Runs.Count); // 3 text, poly, begin, newline

            chars = line.Runs[1] as PDFTextRunCharacter;
            graphic = line.Runs[3] as PDFLayoutComponentRun;
            Assert.IsNotNull(graphic);


            Assert.AreEqual(graphic.Owner, path5);

            Assert.AreEqual(100.0, graphic.Height.PointsValue); //explicit width + padding
            Assert.AreEqual(450.0, graphic.Width.PointsValue);
            Assert.AreEqual(line.Width.PointsValue, graphic.Width.PointsValue + chars.Width.PointsValue);
        }

    }
}
