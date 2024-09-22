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

            Assert.Inconclusive("Need to support the transformation");
            
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

            floating.Contents.Add(new TextLiteral("40% width and 8% height floating in the margins"));

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

            Assert.AreEqual(width + 20, region.Width); //with the margins
            Assert.AreEqual(height + 20, region.Height); //with the margins

            Assert.AreEqual(50, region.TotalBounds.Y); //After the first line + margins
            Assert.AreEqual(20, region.TotalBounds.X); //Margin and padding inset
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
            section.Margins = 0;
            //section.Padding = 10;
            section.BackgroundColor = StandardColors.Silver;
            
            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                ID = "wrapper", 
                Width = new Unit(50, PageUnits.Percent),
                Height = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = StandardColors.Blue,
                Margins = Unit.Auto
            };

            section.Contents.Add(wrapper);
            Div relative = new Div()
            {
                ID = "inner_relative",
                Height = new Unit(50, PageUnits.Percent), 
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                Margins = Unit.Auto
            };
            
            wrapper.Contents.Add(relative);

            relative.Contents.Add(new TextLiteral("25% width and 25% height with auto margins"));


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
            Unit expectedY = 0; //auto margins vertical = 0

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
            expectedY = 0; //auto margins verical = 0

            Assert.AreEqual(expectedWidth, relativeBlock.Width, "Widths did not match on inner");
            Assert.AreEqual(expectedHeight, relativeBlock.Height, "Heights did not match on inner");
            Assert.AreEqual(expectedX, relativeBlock.TotalBounds.X, "X Value did not match on inner");
            Assert.AreEqual(expectedY, relativeBlock.TotalBounds.Y, "Y Value did not match on inner");
        }





    }
}
