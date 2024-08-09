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
    /// <summary>
    /// Tests the positioning with relative units (e.g 50% or 4em)
    /// </summary>
    [TestClass()]
    public class PositionedRelativeUnit_Tests
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
        
        protected string AssertGetContentFile(string name)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Relative/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_01_BlockPercentToPage()
        {

            var path = AssertGetContentFile("Relative_01_BlockPercentToPage");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_01_BlockPercentToPage.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 50 + 15;
            Unit xOffset = 0;
            Unit width = pg.Width / 2.0;
            Unit height = pg.Height / 2.0;
            
            Assert.AreEqual(width, block.Width);
            Assert.AreEqual(height, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width, block.TotalBounds.Width);
            Assert.AreEqual(height, block.TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height, after.OffsetY);
        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_02_BlockPercentToPageMarginsPadding()
        {

            var path = AssertGetContentFile("Relative_02_BlockPercentToPageMarginsPadding");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_02_BlockPercentToPageMarginsPadding.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 50 + 15;
            Unit xOffset = 0;
            Unit width = (pg.Width - 40 ) / 2.0;
            Unit height = (pg.Height - 40 ) / 2.0; 
            
            Assert.AreEqual(width + 40, block.Width);
            Assert.AreEqual(height + 40, block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 40, block.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 40, block.TotalBounds.Height);
            
            Assert.AreEqual(width - 20, block.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, block.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 40, after.OffsetY);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Relative_03_BlockPercentToPageMarginsPaddingTopLeft()
        {

            var path = AssertGetContentFile("Relative_03_BlockPercentToPageMarginsPaddingTopLeft");

            var doc = Document.ParseDocument(path);
            
            using (var ms = DocStreams.GetOutputStream("Relative_03_BlockPercentToPageMarginsPaddingTopLeft.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.1;
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(4, pg.ContentBlock.Columns[0].Contents.Count); //heading, span, relative, span
            var block = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            
            
            //check the bounds.
            Unit yOffset = 30 + 20 + 15; //heading, head margins, a line
            Unit xOffset = 0;
            Unit width = (pg.Width - 40) / 2.0;
            Unit height = (pg.Height - 40) / 2.0; 
            
            Assert.AreEqual(width + 10, block.Width); //includes the margins
            Assert.AreEqual(height + 10 , block.Height);
            
            Assert.AreEqual(yOffset,  block.TotalBounds.Y);
            Assert.AreEqual(xOffset, block.TotalBounds.X);
            Assert.AreEqual(width + 10 , block.TotalBounds.Width); //The block total bounds includes the margins
            Assert.AreEqual(height + 10 , block.TotalBounds.Height);
            
            Assert.AreEqual(width - 20, block.Columns[0].TotalBounds.Width); //The inner region total bounds includes the padding.
            Assert.AreEqual(height - 20, block.Columns[0].TotalBounds.Height);
            
            //check before and after
            var before = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(before);
            Assert.AreEqual(yOffset - 15, before.OffsetY);

            var after = pg.ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.IsNotNull(after);
            Assert.AreEqual(yOffset + height + 10, after.OffsetY); //relative block margins

            //relative position is applied to the render bounds AFTER layout.
            //because it is just part of the layout as normal, then moved
            
            var arrange = ((Div)block.Owner).GetFirstArrangement();

            yOffset = 20 + 30 + 15 + 20; //top margin, heading, line, explicit top value
            xOffset = 20 + 10; //left page margin, explcit left value
            //Assert.AreEqual(yOffset + 5, arrange.RenderBounds.Y); //position is applied to the render bounds for relative
            Assert.AreEqual(xOffset + 5 , arrange.RenderBounds.X);
            Assert.AreEqual(width, arrange.RenderBounds.Width);
            
            
        }
        
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentFullToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.Percent),
                Width = new Unit(100, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentMinWidthAndHeightToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                MinimumWidth = new Unit(50, PageUnits.ViewPortWidth),
                MinimumHeight = new Unit(30, PageUnits.ViewPortHeight),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            

            relative.Contents.Add(new TextLiteral("min width and height"));
            section.Contents.Add(relative);

            using (var ms = DocStreams.GetOutputStream("RelativePositioned_MinWidthAndHeightToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(300, block.Width);
            Assert.AreEqual(240, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockOffsetMinWidthAndHeightToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                X = new Unit(25, PageUnits.ViewPortWidth),
                Y = new Unit(40, PageUnits.ViewPortHeight),
                MinimumWidth = new Unit(50, PageUnits.ViewPortWidth),
                MinimumHeight = new Unit(20, PageUnits.ViewPortHeight),
                PositionMode = PositionMode.Relative,
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                Contents = { new TextLiteral("min width and height") }
            };


            
            section.Contents.Add(relative);

            using (var ms = DocStreams.GetOutputStream("RelativeOffset_MinWidthAndHeightToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            
            
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
   
            //Check the widths and heights
            Assert.AreEqual(0, block.TotalBounds.X);
            Assert.AreEqual(0, block.TotalBounds.Y);
            Assert.AreEqual(600 * 0.5, block.Width);
            Assert.AreEqual(800 * 0.2, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockOffsetMinWidthAndHeightToPageTransformed()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                X = new Unit(25, PageUnits.ViewPortWidth),
                Y = new Unit(40, PageUnits.ViewPortHeight),
                MinimumWidth = new Unit(50, PageUnits.ViewPortWidth),
                MinimumHeight = new Unit(20, PageUnits.ViewPortHeight),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            //rotate about 28 degrees
            relative.TransformOperation = new Styles.TransformOperation(TransformType.Rotate, 0.5F, 0.0F);

            relative.Contents.Add(new TextLiteral("min width and height"));
            section.Contents.Add(relative);

            using (var ms = DocStreams.GetOutputStream("RelativeOffset_MinWidthAndHeightToPageTransformed.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var line = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(posRun);

            var posRegion = pg.ContentBlock.PositionedRegions[0] as PDFLayoutRegion;
            Assert.AreEqual(posRun.Region, posRegion);

            
            //Check the dimensions for the region as transformed
            Assert.AreEqual(480, posRegion.TotalBounds.Height);
            Assert.AreEqual(450, posRegion.TotalBounds.Width);

            var block = posRegion.Contents[0] as PDFLayoutBlock;
            

            Assert.IsNotNull(block);

            Assert.AreEqual(600 * 0.5, block.Width);
            Assert.AreEqual(800 * 0.2, block.Height);

            double[] expected = new double[6];
            expected[0] = 0.8775825618903728;
            expected[1] = 0.479425538604203;
            expected[2] = -0.479425538604203;
            expected[3] = 0.8775825618903728;
            expected[4] = 300;
            expected[5] = 400;

            Assert.IsTrue(block.HasTransformedOffset);
            var components = block.Position.TransformMatrix.Components;

            Assert.AreEqual(6, components.Length);
            for (var i = 0; i < 6; i++)
            {
                Assert.AreEqual(expected[i], components[i]);
            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentFullToPageMargins()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.Percent),
                Width = new Unit(100, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPageMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600 - 40, block.Width);
            Assert.AreEqual(800 - 40, block.Height);
        }


        /// <summary>
        /// Tests the overflowing of the content for 100vh and 100vw with margins - this is as per browsers
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortFullToPageMargins()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.ViewPortHeight),
                Width = new Unit(100, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% viewport width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullToPageMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(2, layout.AllPages.Count);

            //Should overflow because 100vh > page height with margins
            Assert.AreEqual(0, layout.AllPages[0].ContentBlock.Columns[0].Contents.Count);

            var pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }


        /// <summary>
        /// Tests the the content for 100vh and 100vw with margins to be clipped
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortFullToPageMarginsClipped()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.OverflowAction = OverflowAction.Clip;
            section.Margins = new Thickness(20);

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(100, PageUnits.ViewPortHeight),
                Width = new Unit(100, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("100% viewport width and height"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FullClippedToPageMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Clipped so it should all be on the first page
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Assert.AreEqual(600, block.Width);
            Assert.AreEqual(800, block.Height);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortWidthAndHeightRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortHeight),
                Width = new Unit(25, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page size"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPort_BlockToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Unit expectedWidth = (600) / 4.0;
            Unit expectedHeight = (800) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortMaxRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortMax),
                Width = new Unit(25, PageUnits.ViewPortMax),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page max"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPortMax_BlockToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Unit expectedWidth = (800) / 4.0;
            Unit expectedHeight = (800) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockViewPortMinRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(20, PageUnits.ViewPortMin),
                Width = new Unit(25, PageUnits.ViewPortMin),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("20% width and 25% height from page min"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositionedViewPortMin_BlockToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            Unit expectedWidth = (600) / 4.0;
            Unit expectedHeight = (600) / 5.0;

            Assert.AreEqual(expectedWidth, block.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, block.Height, "Heights did not match");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TextEMHeightRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            doc.Pages.Add(section);

            section.Contents.Add(new TextLiteral("Normal Size Text"));
            Div relative = new Div()
            {
                FontSize = new Unit(0.5, PageUnits.EMHeight),
                BorderWidth = 0.1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("Half em-height text"));
            section.Contents.Add(relative);

            section.Contents.Add(new TextLiteral("Back to normal Size Text"));


            using (var ms = DocStreams.GetOutputStream("RelativeTextSize_HalfEMHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var before = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            var inner = block.Columns[0].Contents[0] as PDFLayoutLine;

            var after = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.IsNotNull(before);
            Assert.IsNotNull(block);
            Assert.IsNotNull(after);

            Unit expectedouterFont = 20;
            Unit expectedInnerFont = 20 / 2.0;


            var start = before.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(start);
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);

            start = inner.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(expectedInnerFont, start.TextRenderOptions.Font.Size);

            start = after.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TextEXHeightRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            doc.Pages.Add(section);

            section.Contents.Add(new TextLiteral("Normal Size Text"));
            Div relative = new Div()
            {
                FontSize = new Unit(1, PageUnits.EXHeight),
                BorderWidth = 0.1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("Ex-height text"));
            section.Contents.Add(relative);

            section.Contents.Add(new TextLiteral("Back to normal Size Text"));


            using (var ms = DocStreams.GetOutputStream("RelativeTextSize_OneEXHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var before = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            var inner = block.Columns[0].Contents[0] as PDFLayoutLine;

            var after = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.IsNotNull(before);
            Assert.IsNotNull(block);
            Assert.IsNotNull(after);

            Unit expectedouterFont = 20;

            var start = before.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(start);
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);

            var metrics = start.TextRenderOptions.Font.FontMetrics;
            
            Unit expectedInnerFont = metrics.ExHeight;
            start = inner.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(expectedInnerFont, start.TextRenderOptions.Font.Size);

            start = after.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TextZeroHeightRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            doc.Pages.Add(section);

            section.Contents.Add(new TextLiteral("Normal Size Text"));
            Div relative = new Div()
            {
                FontSize = new Unit(1, PageUnits.ZeroWidth),
                TextFirstLineIndent = new Unit(1, PageUnits.ZeroWidth),
                BorderWidth = 0.1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("Ex-height text"));
            section.Contents.Add(relative);

            section.Contents.Add(new TextLiteral("Back to normal Size Text"));


            using (var ms = DocStreams.GetOutputStream("RelativeTextSize_ZeroWidthWithInset.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(3, pg.ContentBlock.Columns[0].Contents.Count);

            var before = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var block = pg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            var inner = block.Columns[0].Contents[0] as PDFLayoutLine;

            var after = pg.ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;

            Assert.IsNotNull(before);
            Assert.IsNotNull(block);
            Assert.IsNotNull(after);

            Unit expectedouterFont = 20;

            var start = before.Runs[0] as PDFTextRunBegin;
            Assert.IsNotNull(start);
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);

            var metrics = start.TextRenderOptions.Font.FontMetrics;

            Unit expectedInnerFont = metrics.ZeroWidth;
            Unit firstLineIndent = metrics.ZeroWidth;

            var spacer = inner.Runs[0] as PDFTextRunSpacer;
            start = inner.Runs[1] as PDFTextRunBegin;

            Assert.AreEqual(firstLineIndent, spacer.Width);
            Assert.AreEqual(expectedInnerFont, start.TextRenderOptions.Font.Size);
            

            //Back up to outer font
            start = after.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(expectedouterFont, start.TextRenderOptions.Font.Size);
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToPageMargins()
        {

            Document doc = new Document();
            Page section = new Page();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 900;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(Unit.Percent(10), Unit.Percent(10), Unit.Percent(10), Unit.Percent(10)); // 10% all vertical
            section.BorderWidth = 1;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 30;
            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("full width and 50% height in margins"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToPageWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);

            var pgContent = pg.Size;

            pgContent.Width -= (pgContent.Width * 0.1) * 2; //2 page width margins
            pgContent.Height -= (pgContent.Height * 0.1) * 2; //2 page height margins

            Assert.AreEqual(pgContent.Width, pg.ContentBlock.Width);
            Assert.AreEqual(pgContent.Height, pg.ContentBlock.Height);
            
            Unit expectedWidth = pgContent.Width;
            Unit expectedHeight = pgContent.Height / 2.0; //50% of (page height - 10% margins)

            

            Assert.AreEqual(expectedWidth.PointsValue, Math.Floor(block.Width.PointsValue), "Widths did not match");
            Assert.AreEqual(expectedHeight.PointsValue, Math.Round(block.Height.PointsValue,0), "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToColumnMargins()
        {

            Document doc = new Document();
            Page section = new Page();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.ColumnCount = 2;
            section.AlleyWidth = 60;
            section.Margins = new Thickness(new Unit(10, PageUnits.Percent));
            section.BorderWidth = 1;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 60;
            section.Style.OverlayGrid.HighlightColumns = true;

            doc.Pages.Add(section);

            Div relative = new Div()
            {
                Height = new Unit(50, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent), //50% of half size column
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };

            relative.Contents.Add(new TextLiteral("50% column width and 50% height inside margins and alley"));
            section.Contents.Add(relative);



            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToColumnWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var block = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);


            Unit contentWidth = 600 - 60; //page width - alley
            contentWidth = contentWidth - (600.0 / 10) * 2; //remainder - 10% margins
            var columnWidth = contentWidth / 2; // 2 columns;
            var expectedWidth = columnWidth / 2;  //50% column width

            Unit expectedHeight = ((800.0) - ((800.0 / 10.0) * 2.0)) / 2.0; //50% of (page height - 10% margins)

            Assert.AreEqual(expectedWidth.PointsValue, block.Width.PointsValue, "Widths did not match");
            Assert.AreEqual(expectedHeight.PointsValue, block.Height.PointsValue, "Heights did not match");
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Padding = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                Margins = 10,
                Padding = 10,
                BorderWidth = 1,
                BorderColor = StandardColors.Blue
            };

            section.Contents.Add(wrapper);
            Div relative = new Div()
            {
                Height = new Unit(25, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red
            };
            wrapper.Contents.Add(relative);

            relative.Contents.Add(new TextLiteral("50% width and 25% height with margins"));
            

            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(580, wrapperBlock.Width); //page - margins

            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(1, wrapperBlock.Columns[0].Contents.Count);

            var relativeBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(relativeBlock);

            Unit expectedWidth = (580 - 40) / 2.0;
            Unit expectedHeight = (800 - 60) / 4.0;
            Assert.AreEqual(expectedWidth, relativeBlock.Width, "Widths did not match");
            Assert.AreEqual(expectedHeight, relativeBlock.Height, "Heights did not match");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeFloatToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 30;
            section.Padding = 10;
            section.BackgroundColor = StandardColors.Aqua;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 10;

            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                //Margins = 10,
                Padding = 10,
                BorderWidth = 1,
                BorderColor = StandardColors.Blue
            };

            section.Contents.Add(wrapper);

            wrapper.Contents.Add("This is a long text run that should be before the floating div.");

            Div floating = new Div()
            {
                Height = Unit.Percent(8),
                Width = Unit.Percent(50),
                BorderWidth = 1,
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left,
                FontSize = Unit.Em(0.8),
                TextLeading = Unit.Pt(20),
                Padding = 5,
                Margins = 10
            };
            wrapper.Contents.Add(floating);

            floating.Contents.Add(new TextLiteral("40% width and 10% height floating in the margins"));

            wrapper.Contents.Add("This is a long text run that should flow nicely around the 50% width, and 8% height floating div on the page");


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FloatWithContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            
            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(580, wrapperBlock.Width); //page - margins
            
            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(5, wrapperBlock.Columns[0].Contents.Count); //5 lines of text
            var region = wrapperBlock.Columns[0];

            Assert.AreEqual(1, region.Floats.Count);
            var floatAdd = region.Floats;

            Assert.AreEqual(0, floatAdd.FloatInset);
            Assert.AreEqual(30, floatAdd.YOffset);

            Unit width = 560 * 0.5; //padding and margins in the page and wrapper removed, 50% of that
            Assert.AreEqual(width + 20, floatAdd.FloatWidth); //add the margins here
            Unit height = 760 * 0.08; //padding and margins in the page and wrapper removed, 10% of that
            Assert.AreEqual(height + 20, floatAdd.FloatHeight); //add the margins here

            Assert.AreEqual(1, wrapperBlock.PositionedRegions.Count);
            region = wrapperBlock.PositionedRegions[0];

            Assert.AreEqual(width, region.Width); //without the margins
            Assert.AreEqual(height, region.Height); //without the margins

            Assert.AreEqual(30, region.TotalBounds.Y); //After the first line
            Assert.AreEqual(0, region.TotalBounds.X);
            Assert.AreEqual(1, region.Contents.Count);

            var floatBlock = region.Contents[0];
            Assert.IsNotNull(floatBlock);

            //TODO: Check this against a normal positioned and standard region - does the width normally extend beyond the width of the region
            Assert.AreEqual(width + 20, floatBlock.Width); //Actual width of the block includes the margins???
            Assert.AreEqual(height + 20, floatBlock.Height); //Actual Height includes the margins - OK

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BlockPercentRelativeToSizedContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 30;
            //section.Padding = 10;
            section.BackgroundColor = StandardColors.Silver;
            section.VerticalAlignment = VerticalAlignment.Middle;
            section.HorizontalAlignment = HorizontalAlignment.Center;

            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                ID = "wrapper",
                Width = new Unit(50, PageUnits.Percent),
                Height = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = StandardColors.Blue,
                VerticalAlignment = VerticalAlignment.Middle,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            section.Contents.Add(wrapper);
            Div relative = new Div()
            {
                ID = "inner_relative",
                Height = new Unit(50, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                VerticalAlignment = VerticalAlignment.Middle,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            wrapper.Contents.Add(relative);

            relative.Contents.Add(new TextLiteral("50% width and 25% height with margins"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_BlockToSizedContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Unit parentBoundsWidth = 600;
            Unit parentBoundsHeight = 800;

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
            var wrapperBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            

            Assert.AreEqual(1, wrapperBlock.Columns.Length);
            Assert.AreEqual(1, wrapperBlock.Columns[0].Contents.Count);

            Unit expectedWidth = 600 / 2.0;
            Unit expectedHeight = 800 / 2.0;
            Unit expectedX = (parentBoundsWidth - expectedWidth) / 2;
            Unit expectedY = (parentBoundsHeight - expectedHeight) / 2;

            Assert.AreEqual(expectedWidth, wrapperBlock.Width, "Widths did not match on wrapper");
            Assert.AreEqual(expectedHeight, wrapperBlock.Height, "Heights did not match on wrapper");
            Assert.AreEqual(expectedX, wrapperBlock.TotalBounds.X, "X Value did not match on wrapper");
            Assert.AreEqual(expectedY, wrapperBlock.TotalBounds.Y, "Y Value did not match on wrapper");

            var relativeBlock = wrapperBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(relativeBlock);

            parentBoundsWidth = expectedWidth;
            parentBoundsHeight = expectedHeight;

            expectedWidth = expectedWidth / 2.0;
            expectedHeight = expectedHeight / 2.0;

            expectedX = (parentBoundsWidth - expectedWidth) / 2;
            expectedY = (parentBoundsHeight - expectedHeight) / 2;

            Assert.AreEqual(expectedWidth, relativeBlock.Width, "Widths did not match on inner");
            Assert.AreEqual(expectedHeight, relativeBlock.Height, "Heights did not match on inner");
            Assert.AreEqual(expectedX, relativeBlock.TotalBounds.X, "X Value did not match on inner");
            Assert.AreEqual(expectedY, relativeBlock.TotalBounds.Y, "Y Value did not match on inner");
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentPercentToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;
            Unit[] CellWidths = new Unit[] {
                new Unit(20, PageUnits.Percent),
                new Unit(30, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            section.Contents.Add(grid);

            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }
            

            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_TableToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count);

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(580, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(580, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = 580 * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);


                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);
                }

            }
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentFixedWidthToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;

            Unit tableWidth = 500;

            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = tableWidth;
            grid.FontSize = new Unit(70, PageUnits.Percent);
            section.Contents.Add(grid);

            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_FixedWidthTable.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count);

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);
                }

            }
        }



        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentCellRelativeWidthToPageCellHeight()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Aqua;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;

            

            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = new Unit(60, PageUnits.Percent);
            grid.FontSize = new Unit(80, PageUnits.Percent);
            section.Contents.Add(grid);

            Unit tableWidth = (600 - 20) * 0.6;


            for (var r = 0; r < RowCount; r++)
            {
                TableRow row = new TableRow();
                

                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    cell.Width = CellWidths[c];

                    //all cell heights should add up to the page content block height
                    cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_RelativeWidthTableCellPercentHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //After the table should be on a new page
            Assert.AreEqual(2, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(2, pg.ContentBlock.Columns[0].Contents.Count); //last line is zero height with the overflow

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //page - margins
            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);

            //Check the 80% font size on the full style
            Assert.AreEqual(20 * 0.8, tableBlock.FullStyle.Font.FontSize);

            for (var r = 0; r < RowCount; r++)
            {
                var rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }

            var emptyBlock = pg.ContentBlock.Columns[0].Contents[1];
            Assert.AreEqual(0, emptyBlock.Height);

            pg = layout.AllPages[1];
            Assert.AreEqual(1, pg.ContentBlock.Columns.Length);

            //Quick check we have the literal on the new page.
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var line = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void TablePercentCellRelativeWidthToColumnCellHeight()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Style.PageStyle.Width = 600;
            section.Style.PageStyle.Height = 800;
            section.FontSize = 20;
            //section.TextLeading = 25;
            section.Margins = 10;
            section.ColumnCount = 2;
            section.AlleyWidth = 10;
            section.BackgroundColor = StandardColors.Silver;

            doc.Pages.Add(section);

            const int RowCount = 10;
            const int CellCount = 3;



            Unit[] CellWidths = new Unit[] {
                new Unit(25, PageUnits.Percent),
                new Unit(50, PageUnits.Percent),
                new Unit(25, PageUnits.Percent),
            };

            TableGrid grid = new TableGrid();
            grid.Width = new Unit(40, PageUnits.ViewPortMin); //40% of page width without margins
            grid.FontSize = new Unit(80, PageUnits.Percent);
            section.Contents.Add(grid);

            Unit tableWidth = 600 * 0.4;

            TableRow row  = new TableHeaderRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                var content = "Header " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }

            for (var r = 0; r < RowCount; r++)
            {
                row = new TableRow();
                grid.Rows.Add(row);

                for (var c = 0; c < CellCount; c++)
                {

                    TableCell cell = new TableCell();
                    //cell.Width = CellWidths[c]; - implied width from the header

                    //all cell heights should add up to the page content block height
                    cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                    var content = (r + 1).ToString() + "." + (c + 1).ToString() + " at " + CellWidths[c].ToString();

                    cell.Contents.Add(content);
                    row.Cells.Add(cell);
                }

            }

            //Add a footer row to the table
            row = new TableFooterRow();
            grid.Rows.Add(row);

            for (var c = 0; c < CellCount; c++)
            {

                TableCell cell = new TableCell();
                cell.Width = CellWidths[c];

                //all cell heights should add up to the page content block height
                cell.Height = new Unit(100 / RowCount, PageUnits.Percent);

                var content = "Footer " + (RowCount).ToString() + " at " + CellWidths[c].ToString();

                cell.Contents.Add(content);
                row.Cells.Add(cell);
            }


            section.Contents.Add(new TextLiteral("After the Table"));


            using (var ms = DocStreams.GetOutputStream("RelativePositioned_RelativeWidthTableToColumnCellPercentHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //The table should force a new column
            Assert.AreEqual(1, layout.AllPages.Count);

            var pg = layout.AllPages[0];
            Assert.AreEqual(2, pg.ContentBlock.Columns.Length);
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count); //just the table

            var tableBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, tableBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(RowCount, tableBlock.Columns[0].Contents.Count);
            Assert.AreEqual(800 - 20, tableBlock.Height);

            //Check the 80% font size on the full style
            Assert.AreEqual(20 * 0.8, tableBlock.FullStyle.Font.FontSize);


            //Header Row
            var rowBlock = tableBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.AreEqual(tableWidth, rowBlock.Width); //40vmin of page (inc. margins)

            Assert.AreEqual(CellCount, rowBlock.Columns.Length);

            //Header cells

            for (var c = 0; c < CellCount; c++)
            {
                var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                Assert.IsNotNull(rowColumn);
                Assert.AreEqual(1, rowColumn.Contents.Count);

                var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                Assert.AreEqual(cellW, rowColumn.Width);

                var cellBlock = rowColumn.Contents[0];
                Assert.AreEqual(cellW, cellBlock.Width);

                Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
            }

            for (var r = 1; r < tableBlock.Columns[0].Contents.Count; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }

            var col1Total = tableBlock.Columns[0].Contents.Count;
            var col2Total = RowCount + 2 + 1 - col1Total; //total rows + the 2 headers and a footer - the count on column1

            //Quick check we have the rest of the table and literal on the new page.
            Assert.AreEqual(2, pg.ContentBlock.Columns[1].Contents.Count);

            tableBlock = pg.ContentBlock.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(tableBlock);

            Assert.AreEqual(col2Total, tableBlock.Columns[0].Contents.Count);

            for (var r = 0; r < col2Total; r++)
            {
                rowBlock = tableBlock.Columns[0].Contents[r] as PDFLayoutBlock;
                Assert.AreEqual(tableWidth, rowBlock.Width); //page - margins
                Assert.AreEqual(CellCount, rowBlock.Columns.Length);

                for (var c = 0; c < CellCount; c++)
                {
                    var rowColumn = rowBlock.Columns[c] as PDFLayoutRegion;
                    Assert.IsNotNull(rowColumn);
                    Assert.AreEqual(1, rowColumn.Contents.Count);

                    var cellW = tableWidth * (CellWidths[c].Value / 100.0); //calculate the percent
                    Assert.AreEqual(cellW, rowColumn.Width);

                    var cellBlock = rowColumn.Contents[0];
                    Assert.AreEqual(cellW, cellBlock.Width);

                    Assert.AreEqual((800 - 20) / RowCount, cellBlock.Height); //(page height - margins) split into rows
                }

            }


            var line = pg.ContentBlock.Columns[1].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
        }


        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void ListAlphaLowerRelativeInsetLeft()
        {
            const int PageWidth = 600;
            const int PageHeight = 800;
            const int ItemCount = 5;
            string[] ItemValues = new[] { "1a-", "1b-", "1c-", "1d-", "1e-" };
            const double DefaultNumberWidth = 600 * 0.1;
            const double DefaultGutterWidth = 10.0;

            Document doc = new Document();
            Section section = new Section();
            section.FontSize = 10;
            section.Width = PageWidth;
            section.Height = PageHeight;
            section.Margins = 10;
            section.BackgroundColor = StandardColors.Silver;
            doc.Pages.Add(section);

            var ol = new ListOrdered();
            ol.NumberingStyle = ListNumberingGroupStyle.LowercaseLetters;
            ol.NumberPostfix = "-";
            ol.NumberPrefix = "1";
            ol.NumberAlignment = HorizontalAlignment.Left;
            ol.NumberInset = new Unit(10, PageUnits.ViewPortWidth);
            ol.Width = new Unit(50, PageUnits.ViewPortWidth);
            ol.BorderColor = StandardColors.Aqua;

            section.Contents.Add(ol);



            for (var i = 0; i < ItemCount; i++)
            {
                var li = new ListItem();
                li.Contents.Add(new TextLiteral("Item " + i));
                li.BorderColor = StandardColors.Green;
                li.Width = new Unit(50, PageUnits.Percent);
                ol.Items.Add(li);

            }


            using (var ms = DocStreams.GetOutputStream("RelativeList_AlphaLowerRelativeInsetLeft.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            //Assert.Inconclusive();

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);

            var olBlock = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(olBlock);

            Assert.AreEqual(1, olBlock.Columns.Length);
            Assert.AreEqual(ItemCount, olBlock.Columns[0].Contents.Count);

            

            Assert.AreEqual(600 * 0.5, olBlock.Width);

            for (var i = 0; i < ItemCount; i++)
            {

                var itemBlock = olBlock.Columns[0].Contents[i] as PDFLayoutBlock;
                //var column = rowBlock.Columns[i];
                Assert.IsNotNull(itemBlock);

                Assert.AreEqual(1, itemBlock.Columns.Length);
                Assert.AreEqual(1, itemBlock.PositionedRegions.Count);

                Assert.AreEqual(1, itemBlock.Columns[0].Contents.Count);
                Assert.AreEqual(1, itemBlock.PositionedRegions[0].Contents.Count);

                //the block has a width of the 50% of the ol + the margins for the label and gutter
                //and the label is 10% of the page width
                Assert.AreEqual((600 * 0.1) + 10, itemBlock.Position.Margins.Left);
                Assert.AreEqual(olBlock.Width * 0.5 + itemBlock.Position.Margins.Left, itemBlock.Width);
                

                var numBlock = itemBlock.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
                var itemLine = itemBlock.Columns[0].Contents[0] as PDFLayoutLine;

                Assert.IsNotNull(numBlock);
                Assert.IsNotNull(itemLine);

                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);
                Assert.AreEqual(4, itemLine.Runs.Count);



                var start = itemLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                var chars = itemLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                var end = itemLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                var posRun = itemLine.Runs[3] as PDFLayoutPositionedRegionRun;
                Assert.IsNotNull(posRun);
                Assert.AreEqual(DefaultNumberWidth, posRun.Region.Width);

                //cursor starts at margins + number + gutter
                Assert.AreEqual(DefaultNumberWidth + DefaultGutterWidth + 10, start.StartTextCursor.Width);
                Assert.AreEqual("Item " + i, chars.Characters);


                Assert.AreEqual(1, numBlock.Columns.Length);
                Assert.AreEqual(1, numBlock.Columns[0].Contents.Count);
                Assert.AreEqual(DefaultNumberWidth, numBlock.Width);

                var numLine = numBlock.Columns[0].Contents[0] as PDFLayoutLine;
                Assert.AreEqual(3, numLine.Runs.Count);

                start = numLine.Runs[0] as PDFTextRunBegin;
                Assert.IsNotNull(start);

                chars = numLine.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(chars);

                end = numLine.Runs[2] as PDFTextRunEnd;
                Assert.IsNotNull(end);

                Assert.AreEqual(ItemValues[i], chars.Characters);

                //Make sure we are left aligned from margins
                Assert.AreEqual(10.0, start.StartTextCursor.Width);
            }

        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void HtmlParsingTest()
        {

            var src = @"<?scryber append-log='true' log-level='Verbose' parser-log='true' ?>
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title>Relative Unit Test</title>

    <style >
        body {
            background-color:#CCC;
            padding:20pt;
            font-size:1.2rem; /* 120 percent of the root size  */
            margin:5vw; /* 5 percent of the view width */
            font-family: 'Segoe UI', sans-serif;
        }

        h1{
            font-size:2em;  /* double body size */
            font-weight:normal;
            height: 20vmax;  /* 20% of the max view dimension */
            background-color: gray;
        }

    </style>
</head>
<body>
    <div id='wrapper' style='height: 40vh; background-color: silver; padding: 1em;' >Above the heading
        <h1 id='heading' >This is my first heading</h1>
        <div id='inner' >And this is the content below the heading that should flow across multiple lines within the page and flow nicely along those lines.</div>
    </div>
</body>
</html>";

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.RenderOptions.Compression = OutputCompressionType.None;

                Assert.IsInstanceOfType(doc, typeof(HTMLDocument));

                using (var stream = DocStreams.GetOutputStream("RelativeHTML_ParsingAndOutputTest.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }

                Assert.IsNotNull(this.layout);
                Assert.AreEqual(1, this.layout.AllPages.Count);

                var pg = this.layout.AllPages[0];
                Assert.IsNotNull(pg);

                var margin = pg.Size.Width * 0.05; // 5vw
                Assert.AreEqual(margin, pg.PositionOptions.Margins.Left);
                Assert.AreEqual(margin, pg.PositionOptions.Margins.Top);
                Assert.AreEqual(margin, pg.PositionOptions.Margins.Bottom);
                Assert.AreEqual(margin, pg.PositionOptions.Margins.Right);


                var rootEm = Font.DefaultFontSize; //16
                Unit fbody = rootEm * 1.2; // 1.2em
                var content = pg.ContentBlock;
                Assert.AreEqual(fbody, content.FullStyle.Font.FontSize);

                Assert.AreEqual(1, pg.ContentBlock.Columns.Length);
                Assert.AreEqual(1, pg.ContentBlock.Columns[0].Contents.Count);
                

                var wrapper = pg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;

                var h = pg.Size.Height * 0.4; //40vh
                Assert.AreEqual(h, wrapper.Height);

                var pad = fbody; //padding = 1em

                Assert.AreEqual(pad, wrapper.Position.Padding.Left);
                Assert.AreEqual(pad, wrapper.Position.Padding.Top);
                Assert.AreEqual(pad, wrapper.Position.Padding.Bottom);
                Assert.AreEqual(pad, wrapper.Position.Padding.Right);

                Assert.AreEqual(1, wrapper.Columns.Length);
                Assert.AreEqual(3, wrapper.Columns[0].Contents.Count);

                var literalLine = wrapper.Columns[0].Contents[0] as PDFLayoutLine;
                var heading = wrapper.Columns[0].Contents[1] as PDFLayoutBlock; //After the text
                var inner = wrapper.Columns[0].Contents[2] as PDFLayoutBlock;


                var start = literalLine.Runs[0] as PDFTextRunBegin;
                Assert.AreEqual(fbody, start.TextRenderOptions.GetSize());

                var fhead = fbody * 2; //2em of the 1.2em body size
                var headHeight = pg.Size.Height * 0.2; //20vmax;

                Assert.AreEqual(headHeight, heading.Height);
                literalLine = heading.Columns[0].Contents[0] as PDFLayoutLine;
                start = literalLine.Runs[0] as PDFTextRunBegin;

                Assert.AreEqual(fhead, start.TextRenderOptions.GetSize());

                //Should revert back to normal within the inner div.
                literalLine = inner.Columns[0].Contents[0] as PDFLayoutLine;
                start = literalLine.Runs[0] as PDFTextRunBegin;

                Assert.AreEqual(fbody, start.TextRenderOptions.GetSize());


            }
        }

        

    }
}
