using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class PanelLayout_Tests
    {
        private const string LayoutTestCategory = "Layout";
        private const int PageWidth = 300;
        private const int PageHeight = 500;

        private PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        #region public void PageWithAPanel()

        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithAPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            Panel panel = new Panel();
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            Label lbl = new Label();
            lbl.Contents.Add(new TextLiteral("Just Some Content"));
            panel.Contents.Add(lbl);

            using (var ms = DocStreams.GetOutputStream("Panels_NoSizing.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            var width = panelBlock.Width;
            var height = panelBlock.Height;

            //Panel width and height
            Assert.IsTrue( width > 0, "Panel block should be not be 0 wide");
            Assert.IsTrue(height > 0, "Panel block should be not be 0 high");

            //Total bounds
            Assert.AreEqual(width, panelBlock.TotalBounds.Width, "Panel block total width should be " + width);
            Assert.AreEqual(height, panelBlock.TotalBounds.Height, "Panel block total height should be " + height);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            //Available bounds
            Assert.AreEqual(PageWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be the page width of " + PageWidth);
            Assert.AreEqual(PageHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be the page height of " + PageHeight);
            Assert.AreEqual(0, panelBlock.AvailableBounds.X, "Panel block available X should be 0");
            Assert.AreEqual(0, panelBlock.AvailableBounds.Y, "Panel block available Y should be 0");

            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(width, region.TotalBounds.Width, "Panel region total width should be " + width);
            Assert.AreEqual(height, region.TotalBounds.Height, "Panel region total height should be " + height);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            

            //region Used Size
            Assert.AreEqual(width, region.UsedSize.Width, "Panel region used width should be " + width);
            Assert.AreEqual(height, region.UsedSize.Height, "Panel region used height should be " + height);

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(0, region.AvailableHeight, "Panel region available height should be zero");
        }

        #endregion

        #region public void PageWithAPanelWithPaddingAndMargins()

        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithAPanelWithPaddingAndMargins()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Background.Color = Drawing.StandardColors.Gray;
            doc.Pages.Add(pg);

            Panel panel = new Panel();
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            panel.Margins = new Drawing.Thickness(10);
            panel.Padding = new Drawing.Thickness(20);
            panel.BackgroundColor = Drawing.StandardColors.White;
            pg.Contents.Add(panel);

            Label lbl = new Label();
            lbl.Contents.Add(new TextLiteral("Just Some Content"));
            panel.Contents.Add(lbl);

            using (var ms = DocStreams.GetOutputStream("Panels_NoSizingMarginsAndPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            

            var lblLine = panelBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var lblWidth = lblLine.Width; //203.7
            var lblHeight = lblLine.Height; //28.8

            //Panel width and height
            var width = panelBlock.Width;
            var height = panelBlock.Height;

            //The horizontal and vertical spacing around the panel
            var hSpace = panel.Margins.Left + panel.Margins.Right + panel.Padding.Left + panel.Padding.Right;
            var vSpace = panel.Margins.Top + panel.Margins.Bottom + panel.Padding.Top + panel.Padding.Bottom;

            Assert.AreEqual(width, lblWidth + hSpace, "Panel block should be " + width + " not " + (lblWidth + hSpace)   + " wide");
            Assert.AreEqual(height, lblHeight + vSpace, "Panel block should be not be " + height + " not " + (lblHeight + vSpace)   + " high");

            //Total bounds should match width and height (in this instance)
            Assert.AreEqual(width, panelBlock.TotalBounds.Width, "Panel block total width should be " + width);
            Assert.AreEqual(height, panelBlock.TotalBounds.Height, "Panel block total height should be " + height);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            //Available bounds (as we have no explicit size) - is the page size that we can use, minus the space for margins and padding
            Assert.AreEqual(PageWidth - hSpace, panelBlock.AvailableBounds.Width, "Panel block available width should be the page width of " + (PageWidth - hSpace));
            Assert.AreEqual(PageHeight - vSpace, panelBlock.AvailableBounds.Height, "Panel block available height should be the page height of " + (PageHeight - vSpace));
            //And the inner position is offset by the left and top padding and margins
            Assert.AreEqual(panel.Margins.Left + panel.Padding.Left, panelBlock.AvailableBounds.X, "Panel block available X should be " + (panel.Margins.Left + panel.Padding.Left));
            Assert.AreEqual(panel.Margins.Top + panel.Padding.Top, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + (panel.Margins.Top + panel.Padding.Top));

            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(width - hSpace, region.TotalBounds.Width, "Panel region total width should be " + width);
            Assert.AreEqual(height - vSpace, region.TotalBounds.Height, "Panel region total height should be " + height);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");


            //region Used Size is the width and height, again minus the spacing
            Assert.AreEqual(width - hSpace, region.UsedSize.Width, "Panel region used width should be " + width);
            Assert.AreEqual(height - vSpace, region.UsedSize.Height, "Panel region used height should be " + height);

            //Region Offset and available height are back to zero, as we are now outside of the padding and margin calculations
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(0, region.AvailableHeight, "Panel region available height should be zero");
        }

        #endregion

        #region public void PageWithAFixedSizePanelWithPaddingAndMargins()

        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithAFixedSizePanelWithPaddingAndMargins()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Background.Color = Drawing.StandardColors.Gray;
            pg.FontSize = 16;
            doc.Pages.Add(pg);

            Panel panel = new Panel();
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            panel.Margins = new Drawing.Thickness(10);
            panel.Padding = new Drawing.Thickness(20);
            panel.Width = 200;
            panel.Height = 100;
            panel.BackgroundColor = Drawing.StandardColors.White;
            pg.Contents.Add(panel);

            Label lbl = new Label();
            lbl.Contents.Add(new TextLiteral("Just Some Content"));
            panel.Contents.Add(lbl);

            using (var ms = DocStreams.GetOutputStream("Panels_ExplicitSizeMarginsAndPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");



            var lblLine = panelBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var lblWidth = lblLine.Width; //203.7
            var lblHeight = lblLine.Height; //28.8

            //Panel width and height
            var width = panelBlock.Width;
            var height = panelBlock.Height;

            //The horizontal and vertical spacing around the panel
            var hSpace = panel.Margins.Left + panel.Margins.Right + panel.Padding.Left + panel.Padding.Right;
            var vSpace = panel.Margins.Top + panel.Margins.Bottom + panel.Padding.Top + panel.Padding.Bottom;

            //The layout width should include the margins
            Assert.AreEqual(panel.Width + panel.Margins.Left + panel.Margins.Right, width , "Panel block should be " + (panel.Width + panel.Margins.Left + panel.Margins.Right) + " not " + (width) + " wide");
            Assert.AreEqual(panel.Height + panel.Margins.Top + panel.Margins.Bottom, height, "Panel block should be " + (panel.Height + panel.Margins.Top + panel.Margins.Bottom) + " not " + (height) + " high");

            //Total bounds should match width and height (in this instance)
            Assert.AreEqual(width, panelBlock.TotalBounds.Width, "Panel block total width should be " + width);
            Assert.AreEqual(height, panelBlock.TotalBounds.Height, "Panel block total height should be " + height);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            //Available bounds (as we have no explicit size) - is the page size that we can use, minus the space for margins and padding
            Assert.AreEqual(panel.Width - (panel.Padding.Left + panel.Padding.Right) , panelBlock.AvailableBounds.Width, "Panel block available width should be the panel width minus the padding = " + (panel.Width - (panel.Padding.Left + panel.Padding.Right)));
            Assert.AreEqual(panel.Height - (panel.Padding.Top + panel.Padding.Bottom), panelBlock.AvailableBounds.Height, "Panel block available height should be the panel height minus the padding = " + (panel.Height - (panel.Padding.Top + panel.Padding.Bottom)));
            //And the inner position is offset by the left and top padding and margins
            Assert.AreEqual(panel.Margins.Left + panel.Padding.Left, panelBlock.AvailableBounds.X, "Panel block available X should be " + (panel.Margins.Left + panel.Padding.Left));
            Assert.AreEqual(panel.Margins.Top + panel.Padding.Top, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + (panel.Margins.Top + panel.Padding.Top));

            //Panel inner region

            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(width - hSpace, region.TotalBounds.Width, "Panel region total width should be " + width);
            Assert.AreEqual(height - vSpace, region.TotalBounds.Height, "Panel region total height should be " + height);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");


            //region Used Size is the width and height, again minus the spacing
            Assert.AreEqual(lblWidth, region.UsedSize.Width, "Panel region used width should be " + width);
            Assert.AreEqual(lblHeight, region.UsedSize.Height, "Panel region used height should be " + height);

            //Region Offset back to zero, as we are now outside of the padding and margin calculations
            //because we have a fixed height, that is not used the actual available height is still there.
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(height - (vSpace + lblHeight), region.AvailableHeight, "Panel region available height should be zero");
        }

        #endregion

        #region public void PageWithExplicitSizedPanel()

        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithExplicitSizedPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            int expectedWidth = 100;
            int expectedHeight = 50;

            Panel panel = new Panel();
            panel.Width = expectedWidth;
            panel.Height = expectedHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            
            using (var ms = DocStreams.GetOutputStream("Panels_ExplicitSizing.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1,"There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");
            
            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width,"Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            //Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(0, panelBlock.AvailableBounds.X, "Panel block available X should be 0");
            Assert.AreEqual(0, panelBlock.AvailableBounds.Y, "Panel block available Y should be 0");

            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        #region public void PageWithFullWidthPanel()

        /// <summary>
        /// Tests a single full width panel in a page with an explicit height.
        /// The page has no margins or padding so panel is the same width as the page.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithFullWidthPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            int expectedWidth = PageWidth;
            int expectedHeight = 50;


            Panel panel = new Panel();
            
            panel.FullWidth = true;
            panel.Height = expectedHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);


            using (var ms = DocStreams.GetOutputStream("Panels_FullWidthSizing.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
                
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width, "Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Panel Block Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            //Panel Block Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(0, panelBlock.AvailableBounds.X, "Panel block available X should be 0");
            Assert.AreEqual(0, panelBlock.AvailableBounds.Y, "Panel block available Y should be 0");

            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        #region public void PageWithPaddingAndFullWidthPanel()

        /// <summary>
        /// Tests a single full width panel in a page with an explicit height.
        /// THe page has margins and padding that reduce the available width.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithPaddingAndFullWidthPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            
            int space = 20;

            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = space;

            pg.Style.Padding.All = space;
            //pg.Style.Margins.All = space;

            doc.Pages.Add(pg);

            //Width of fullwidth panel is reduced by padding
            int expectedWidth = PageWidth - (2 * space); // + (2 * space)) ;
            int expectedHeight = 50;

            //Position of panel is zero - margins and padding are accounted for when rendering
            int expectedX = 0;
            int expectedY = 0;

            Panel panel = new Panel();

            panel.FullWidth = true;
            panel.Height = expectedHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);


            
            using (var ms = DocStreams.GetOutputStream("Panels_FullWidthWithPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
                
            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width, "Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Panel Block Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.TotalBounds.X, "Panel block total X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.TotalBounds.Y, "Panel block total Y should be " + expectedY);

            //Panel Block Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.AvailableBounds.X, "Panel block available X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + expectedY);


            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        #region public void PageWithMarginsAndFullWidthPanel()

        /// <summary>
        /// Tests a single full width panel in a page with an explicit height.
        /// THe page has margins and padding that reduce the available width.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithMarginsAndFullWidthPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            int space = 20;

            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = space;

            //pg.Style.Padding.All = space;
            pg.Style.Margins.All = space;

            doc.Pages.Add(pg);

            //Width of fullwidth panel is reduced by margins
            int expectedWidth = PageWidth - (2 * space); // + (2 * space)) ;
            int expectedHeight = 50;

            //Position of panel is zero -  margins is accounted for when rendering
            int expectedX = 0;
            int expectedY = 0;

            Panel panel = new Panel();

            panel.FullWidth = true;
            panel.Height = expectedHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);



            using (var ms = DocStreams.GetOutputStream("Panels_FullWidthWithMargins.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);

            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width, "Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Panel Block Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.TotalBounds.X, "Panel block total X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.TotalBounds.Y, "Panel block total Y should be " + expectedY);

            //Panel Block Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.AvailableBounds.X, "Panel block available X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + expectedY);


            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        #region public void PageWithMarginsPaddingAndFullWidthPanel()

        /// <summary>
        /// Tests a single full width panel in a page with an explicit height.
        /// THe page has margins and padding that reduce the available width.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithMarginsPaddingAndFullWidthPanel()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            int space = 20;

            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = space;

            pg.Style.Padding.All = space;
            pg.Style.Margins.All = space;

            doc.Pages.Add(pg);

            //Width of fullwidth panel is reduced by margins and padding
            int expectedWidth = PageWidth - ((2 * space) + (2 * space)) ;
            int expectedHeight = 50;

            //Position of panel is zero - margins and padding are accounted for when rendering
            int expectedX = 0;
            int expectedY = 0;

            Panel panel = new Panel();

            panel.FullWidth = true;
            panel.Height = expectedHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);



            using (var ms = DocStreams.GetOutputStream("Panels_FullWidthWithBothMarginsAndPadding.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);

            }

            Assert.AreEqual(1, layout.AllPages.Count, "There should be only 1 page");
            PDFLayoutPage layoutpg = layout.AllPages[0];

            //Page content block
            PDFLayoutBlock content = layoutpg.ContentBlock;
            Assert.IsTrue(content.Columns.Length == 1, "There should be only 1 column");

            //Page content region
            PDFLayoutRegion region = content.Columns[0];
            Assert.IsTrue(region.Contents.Count == 1, "There should be only one item in the region contents");

            //Panel in content
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width, "Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Panel Block Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.TotalBounds.X, "Panel block total X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.TotalBounds.Y, "Panel block total Y should be " + expectedY);

            //Panel Block Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.AvailableBounds.X, "Panel block available X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + expectedY);


            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        #region public void PageWithMarginsPaddingAndFullWidthPanelWithMarginsAndPadding()

        /// <summary>
        /// Tests a single full width panel in a page with an explicit height and margins and padding.
        /// THe page has margins and padding that reduce the available width.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PageWithMarginsPaddingAndFullWidthPanelWithMarginsAndPadding()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            int margin = 20;
            int padding = 10;

            pg.Style.Padding.All = padding;
            pg.Style.Margins.All = margin;
            pg.Style.Background.Color = (Scryber.Drawing.Color)"#AAA";

            doc.Pages.Add(pg);

            int explicitHeight = 50;

            

            Panel panel = new Panel();

            panel.FullWidth = true;
            panel.Height = explicitHeight;
            panel.Style.Margins.All = margin;
            panel.Style.Padding.All = padding;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            panel.Style.Background.Color = (Scryber.Drawing.Color)"#888";
            pg.Contents.Add(panel);


            
            using (var ms = DocStreams.GetOutputStream("Panels_FullWidthWithSpacingBoth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
                
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock content = layoutpg.ContentBlock;
            PDFLayoutRegion region = content.Columns[0];
            
            //Panel in page content region
            PDFLayoutBlock panelBlock = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //Expected measurements of the panel block

            //Width of fullwidth panel is reduced by margins and padding
            int expectedWidth = PageWidth - ((2 * margin) + (2 * padding));

            //Actual height of the panel is explicit height + the margins
            int expectedHeight = explicitHeight + (2 * margin);

            //Position of panel is zero - margins and padding are accounted for when rendering
            int expectedX = 0;
            int expectedY = 0;

            //Panel width and height
            Assert.AreEqual(expectedWidth, panelBlock.Width, "Panel block should be " + expectedWidth + " wide");
            Assert.AreEqual(expectedHeight, panelBlock.Height, "Panel block should be " + expectedHeight + " high");

            //Panel Block Total bounds
            Assert.AreEqual(expectedWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.TotalBounds.X, "Panel block total X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.TotalBounds.Y, "Panel block total Y should be " + expectedY);

            //As we have both padding and margins on the panel width and height are reduced for the available space.
            expectedWidth = (int)panelBlock.TotalBounds.Width.PointsValue - ((2 * margin) + (2 * padding));
            expectedHeight = (int)panelBlock.TotalBounds.Height.PointsValue - ((2 * margin) + (2 * padding));

            //X and Y of the available bounds (and also the inner region content) are offset by the padding.
            expectedX = padding + margin;
            expectedY = padding + margin;

            //Panel Block Available bounds
            Assert.AreEqual(expectedWidth, panelBlock.AvailableBounds.Width, "Panel block available width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, panelBlock.AvailableBounds.Height, "Panel block available height should be " + expectedHeight);
            Assert.AreEqual(expectedX, panelBlock.AvailableBounds.X, "Panel block available X should be " + expectedX);
            Assert.AreEqual(expectedY, panelBlock.AvailableBounds.Y, "Panel block available Y should be " + expectedY);


            //Panel inner region
            Assert.IsTrue(panelBlock.Columns.Length == 1, "There should be one region in the panel block");
            region = panelBlock.Columns[0];

            //region Total bounds
            Assert.AreEqual(expectedWidth, region.TotalBounds.Width, "Panel region total width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.TotalBounds.Height, "Panel region total height should be " + expectedHeight);
            Assert.AreEqual(0, region.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, region.TotalBounds.Y, "Panel region total Y should be 0");

            //region Unused bounds
            Assert.AreEqual(expectedWidth, region.UnusedBounds.Width, "Panel region unused width should be " + expectedWidth);
            Assert.AreEqual(expectedHeight, region.UnusedBounds.Height, "Panel region unused height should be " + expectedHeight);
            Assert.AreEqual(0, region.UnusedBounds.X, "Panel region unused X should be 0");
            Assert.AreEqual(0, region.UnusedBounds.Y, "Panel region unused Y should be 0");

            //region Used Size
            Assert.AreEqual(0, region.UsedSize.Height, "Panel region used height should be 0");
            Assert.AreEqual(0, region.UsedSize.Width, "Panel region used width should be 0");

            //Region Offset and available height
            Assert.AreEqual(0, region.OffsetX, "Panel region offsetX should be 0");
            Assert.AreEqual(expectedHeight, region.AvailableHeight, "Panel region available height should be " + expectedHeight);
        }

        #endregion

        //
        // minimum sizes
        //

        #region public void PanelWithMinWidthAndHeight()

        /// <summary>
        /// Single panel with min-width and min-height that has a label in it, but it not long enougth to 
        /// extend beyond the min-width or min-height of the panel. So panel stays the same minimun size
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelWithMinWidthAndHeight()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            int expectedMinWidth = 200;
            int expectedMinHeight = 50;

            Panel panel = new Panel();
            panel.MinimumWidth = expectedMinWidth;
            panel.MinimumHeight = expectedMinHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);


            Label lbl = new Label() { Text = "Not wide enough", FontSize = 16 };
            panel.Contents.Add(lbl); //Will not push the panel beyond its minimumn width

            
            using (var ms = DocStreams.GetOutputStream("Panels_MinWidthAndHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be the minimums
            Assert.AreEqual(expectedMinWidth, panelBlock.Width, "Panel block should be " + expectedMinWidth + " wide");
            Assert.AreEqual(expectedMinHeight, panelBlock.Height, "Panel block should be " + expectedMinHeight + " high");

            //Total bounds
            Assert.AreEqual(expectedMinWidth, panelBlock.TotalBounds.Width, "Panel block total width should be " + expectedMinWidth);
            Assert.AreEqual(expectedMinHeight, panelBlock.TotalBounds.Height, "Panel block total height should be " + expectedMinHeight);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            Assert.IsTrue(expectedMinWidth > panelregion.TotalBounds.Width, "Panel region total width should be less than " + expectedMinWidth);
            Assert.IsTrue(expectedMinHeight > panelregion.TotalBounds.Height, "Panel region total height should be less than " + expectedMinHeight);
            Assert.AreEqual(0, panelregion.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, panelregion.TotalBounds.Y, "Panel region total Y should be 0");

        }

        #endregion

        #region public void PanelBeyondMinWidth()

        /// <summary>
        /// Single panel with min-width and min-height that has a label in it, but label text is long enougth to 
        /// extend beyond the min-width of the panel. So panel width goes beyond the minimum, but not past the page.
        /// And the height stays the same min-height.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelBeyondMinWidth()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Font.FontSize = 12;

            doc.Pages.Add(pg);

            int expectedMinWidth = 200;
            int expectedMinHeight = 100;

            Panel panel = new Panel();
            panel.MinimumWidth = expectedMinWidth;
            panel.MinimumHeight = expectedMinHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            Label lbl = new Label() { Text = "This label is wide enough to go beyond the 200pt minimum width of the panel " + 
                "and also the width of the page, so should flow onto the next line. (but not beyond the minimum height)" };
            panel.Contents.Add(lbl); //WILL push the panel beyond its minimumn width

            
            using (var ms = DocStreams.GetOutputStream("Panels_BeyondMinWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be greater than the minimum width, but not beyond the height.
            Assert.IsTrue(expectedMinWidth < panelBlock.Width, "Panel block with width '" + panelBlock.Width + " should be greater than " + expectedMinWidth + " wide");
            Assert.IsTrue(PageWidth > panelBlock.Width, "Panel block should not go beyond the page width");
            Assert.AreEqual(expectedMinHeight, panelBlock.Height, "Panel block with height '" + panelBlock.Height + " should be " + expectedMinHeight + " high");


        }

        #endregion

        #region public void PanelBeyondMinWidthAndHeight()

        /// <summary>
        /// Single panel with min-width and min-height that has a label in it, but label text is long enougth to 
        /// extend beyond the min-width of the panel. So panel width goes beyond the minimum, but not past the page.
        /// And the height stays the same min-height.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelBeyondMinWidthAndHeight()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Font.FontSize = 18;

            doc.Pages.Add(pg);

            int expectedMinWidth = 200;
            int expectedMinHeight = 100;

            Panel panel = new Panel();
            panel.MinimumWidth = expectedMinWidth;
            panel.MinimumHeight = expectedMinHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            Label lbl = new Label()
            {
                Text = "This label is wide enough to go beyond the 200pt minimum width of the panel " +
                    "and also the width of the page, so should flow onto the next line and keep going beyond the minimum height of the " +
                    "panel. Therefore extending beyond the bounds of both min- values."
            };
            panel.Contents.Add(lbl); //WILL push the panel beyond its minimumn width

            
            using (var ms = DocStreams.GetOutputStream("Panels_BeyondMinWidthAndHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
                
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be greater than the minimum width, but not beyond the height.
            Assert.IsTrue(expectedMinWidth < panelBlock.Width, "Panel block should be greater than " + expectedMinWidth + " wide");
            Assert.IsTrue(PageWidth > panelBlock.Width, "Panel block should not go beyond the page width");
            Assert.IsTrue(expectedMinHeight < panelBlock.Height, "Panel block should be greater than " + panelBlock.Height + " high");


        }

        #endregion


        //
        // Maximum sizes
        //

        #region public void PanelWithMaxWidthAndHeight()

        /// <summary>
        /// Single panel with max-width and max-height that has a label in it, but it not long enougth to 
        /// extend beyond the max-width or max-height of the panel. So panel should shrink to the used size
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelWithMaxWidthAndHeight()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;

            doc.Pages.Add(pg);

            int expectedMaxWidth = 200;
            int expectedMaxHeight = 50;

            Panel panel = new Panel();
            panel.MaximumWidth = expectedMaxWidth;
            panel.MaximumHeight = expectedMaxHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);


            Label lbl = new Label() { Text = "Not wide enough", FontSize = 16 };
            panel.Contents.Add(lbl); //Will not push the panel beyond its minimumn width


            using (var ms = DocStreams.GetOutputStream("Panels_MaxWidthAndHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be the minimums
            Assert.IsTrue(expectedMaxWidth > panelBlock.Width, "Panel block should be less than " + expectedMaxWidth + " wide");
            Assert.IsTrue(expectedMaxHeight > panelBlock.Height, "Panel block should be less than " + expectedMaxHeight + " high");

            //Total bounds
            Assert.IsTrue(expectedMaxWidth > panelBlock.TotalBounds.Width, "Panel block total width should be less than " + expectedMaxWidth);
            Assert.IsTrue(expectedMaxHeight > panelBlock.TotalBounds.Height, "Panel block total height should be less than " + expectedMaxHeight);
            Assert.AreEqual(0, panelBlock.TotalBounds.X, "Panel block total X should be 0");
            Assert.AreEqual(0, panelBlock.TotalBounds.Y, "Panel block total Y should be 0");

            Assert.IsTrue(expectedMaxWidth > panelregion.TotalBounds.Width, "Panel region total width should be less than " + expectedMaxWidth);
            Assert.IsTrue(expectedMaxHeight > panelregion.TotalBounds.Height, "Panel region total height should be less than " + expectedMaxHeight);
            Assert.AreEqual(0, panelregion.TotalBounds.X, "Panel region total X should be 0");
            Assert.AreEqual(0, panelregion.TotalBounds.Y, "Panel region total Y should be 0");

        }

        #endregion

        #region public void PanelBeyondMaxWidth()

        /// <summary>
        /// Single panel with max-width and max-height that has a label in it, but label text is long enougth to 
        /// extend beyond the max-width of the panel. So panel width stays at the max and flows onto the next line, but not past the page.
        /// And the height does not extend beyond the max-height.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelBeyondMaxWidth()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Font.FontSize = 12;

            doc.Pages.Add(pg);

            int expectedMaxWidth = 200;
            int expectedMaxHeight = 100;

            Panel panel = new Panel();
            panel.MaximumWidth = expectedMaxWidth;
            panel.MaximumHeight = expectedMaxHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            Label lbl = new Label()
            {
                Text = "This label is wide enough to go beyond the 200pt minimum width of the panel" +
                ", so should flow onto the next line. (but not beyond the maximum height)"
            };
            panel.Contents.Add(lbl); //WILL push the panel beyond its maximumn width


            using (var ms = DocStreams.GetOutputStream("Panels_BeyondMaxWidth.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be greater than the minimum width, but not beyond the height.
            Assert.IsTrue(expectedMaxWidth > panelBlock.Width, "Panel block with width '" + panelBlock.Width + " should NOT be greater than " + expectedMaxWidth + " wide");
            Assert.IsTrue(PageWidth > panelBlock.Width, "Panel block should not go beyond the page width");
            Assert.IsTrue(expectedMaxHeight > panelBlock.Height, "Panel block with height '" + panelBlock.Height + " should NOT be greater than " + expectedMaxHeight + " high");


        }

        #endregion

        #region public void PanelBeyondMaxWidthAndHeight()

        /// <summary>
        /// Single panel with max-width and max-height that has a label in it, but label text is long enougth to 
        /// extend beyond the max-width of the panel. So panel width stays at the max and flows onto the next line, but not past the page.
        /// And the text is too long to fit in the height so gets truncated and does NOT extend beyond the max-height.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelBeyondMaxWidthAndHeight()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Font.FontSize = 14;

            doc.Pages.Add(pg);

            int expectedMaxWidth = 200;
            int expectedMaxHeight = 100;

            Panel panel = new Panel();
            panel.MaximumWidth = expectedMaxWidth;
            panel.MaximumHeight = expectedMaxHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            pg.Contents.Add(panel);

            Label lbl = new Label()
            {
                Text = "This label is wide enough to go beyond the 200pt minimum width of the panel" +
                ", so should flow onto the next line, and keep going beyond the maximum height " +
                " allowed on the panel, so will get truncated."
            };
            panel.Contents.Add(lbl); //WILL push the panel beyond its maximumn width


            using (var ms = DocStreams.GetOutputStream("Panels_BeyondMaxWidthAndHeight.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be greater than the minimum width, but not beyond the height.
            Assert.IsTrue(expectedMaxWidth > panelBlock.Width, "Panel block with width '" + panelBlock.Width + " should NOT be greater than " + expectedMaxWidth + " wide");
            Assert.IsTrue(PageWidth > panelBlock.Width, "Panel block should not go beyond the page width");
            Assert.IsTrue(expectedMaxHeight > panelBlock.Height, "Panel block with height '" + panelBlock.Height + " should NOT be greater than " + expectedMaxHeight + " high");


        }

        #endregion

        #region public void PanelBeyondMaxWidthAndHeightClipped()

        /// <summary>
        /// Single panel with max-width and max-height that has a label in it, but label text is long enougth to 
        /// extend beyond the max-width of the panel. So panel width stays at the max and flows onto the next line, but not past the page.
        /// And the text is too long to fit in the height. But overflow is clipped so continues on but is not visible outside the height.
        /// </summary>
        [TestCategory(LayoutTestCategory)]
        [TestMethod()]
        public void PanelBeyondMaxWidthAndHeightClipped()
        {
            Document doc = new Document();
            Page pg = new Page();
            pg.Style.PageStyle.Width = PageWidth;
            pg.Style.PageStyle.Height = PageHeight;
            pg.Style.Font.FontSize = 14;

            doc.Pages.Add(pg);

            int expectedMaxWidth = 200;
            int expectedMaxHeight = 100;

            Panel panel = new Panel();
            panel.MaximumWidth = expectedMaxWidth;
            panel.MaximumHeight = expectedMaxHeight;
            panel.BorderColor = Scryber.Drawing.StandardColors.Black;
            panel.OverflowAction = Drawing.OverflowAction.Clip;
            pg.Contents.Add(panel);

            Label lbl = new Label()
            {
                Text = "This label is wide enough to go beyond the 200pt minimum width of the panel" +
                ", so should flow onto the next line, and keep going beyond the max height " +
                " allowed on the panel, so will get clipped."
            };
            panel.Contents.Add(lbl); //WILL push the panel beyond its maximumn width


            using (var ms = DocStreams.GetOutputStream("Panels_BeyondMaxWidthAndHeightClipped.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            PDFLayoutPage layoutpg = layout.AllPages[0];
            PDFLayoutBlock pgcontent = layoutpg.ContentBlock;
            PDFLayoutRegion pgregion = pgcontent.Columns[0];
            PDFLayoutBlock panelBlock = pgregion.Contents[0] as PDFLayoutBlock;
            PDFLayoutRegion panelregion = panelBlock.Columns[0];
            Assert.IsNotNull(panelBlock, "The layout block in the page column should not be null");

            //block width and height should be greater than the minimum width, but not beyond the height.
            Assert.IsTrue(expectedMaxWidth > panelBlock.Width, "Panel block with width '" + panelBlock.Width + " should NOT be greater than " + expectedMaxWidth + " wide");
            Assert.IsTrue(PageWidth > panelBlock.Width, "Panel block should not go beyond the page width");

            //As we overflow but are clipped the height of the panel is EXACTLY max-height
            Assert.AreEqual(expectedMaxHeight, panelBlock.Height.PointsValue, "Panel block with height '" + panelBlock.Height + " should be equal to " + expectedMaxHeight + " high");


        }

        #endregion
    }
}
