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
    /// <summary>
    /// Tests the float left and right positioning
    /// </summary>
    /// <remarks>
    /// Floating blocks are treated exactly the same as relatively positioned regions , but a FloatAddition instance
    /// is added to the region containing the float. This is then used to reduce the width of the lines (on the left or the right)
    /// whilst they run with the float area.
    /// Once past the float(s) then the line is restored back to full width in the layout.
    /// </remarks>
    [TestClass()]
    public class PositionedFloating_Tests
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
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Positioning/Floating/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        /// <summary>
        /// Checks a simple unsized float left div with text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftToPage()
        {

            var path = AssertGetContentFile("Float_01_Left");

            var doc = Document.ParseDocument(path);



            using (var ms = DocStreams.GetOutputStream("Float_LeftToPage.pdf"))
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
            var content = pg.ContentBlock;


            Assert.AreEqual(1, content.PositionedRegions.Count);
            var pos = content.PositionedRegions[0];

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //pos run, start, chars, end

            var floatAddition = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float should be chars width and line height
            Unit w = chars.Width;
            Unit h = 15;

            Assert.AreEqual(w, innerLine.Width);
            Assert.AreEqual(h, innerLine.Height);

            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            //outer line content
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var startRun = line.Runs[1] as PDFTextRunBegin;

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(startRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            //The start run should account for the float with the text cursors position (size)
            Assert.AreEqual(w, startRun.StartTextCursor.Width );

        }

        /// <summary>
        /// Checks a simple float left div with explicit size and text after.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftSizedToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 10;
            section.TextLeading = 15;
            section.Margins = 15;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 15;
            doc.Pages.Add(section);


            Div floating = new Div()
            {
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left,
                Width = 120,
                Height = 150
            };

            floating.Contents.Add(new TextLiteral("Floating Div"));
            section.Contents.Add(floating);

            section.Contents.Add("After the float");


            using (var ms = DocStreams.GetOutputStream("Float_LeftSizedToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(1, content.PositionedRegions.Count);
            var pos = content.PositionedRegions[0];

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //pos run, start, chars, end

            var floatAddition = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float are explicit
            Unit w = 120;
            Unit h = 150;

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(section.TextLeading, innerLine.Height);

            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            //outer line content
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var startRun = line.Runs[1] as PDFTextRunBegin;

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(startRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            //The start run should account for the float with the text cursors position (size)
            Assert.AreEqual(w + section.Margins.Left, startRun.StartTextCursor.Width);

        }

        /// <summary>
        /// Checks a float left div with explicit size with text before and after reaching beyond the size of the div.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftInMiddleSizedToPageFlowingContent()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 10;
            section.TextLeading = 15;
            section.Margins = 15;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 15;
            doc.Pages.Add(section);

            section.Contents.Add("A bit of content before the div. ");

            Div floating = new Div()
            {
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left,
                Width = 120,
                Height = 45
            };

            floating.Contents.Add(new TextLiteral("Floating Div"));
            section.Contents.Add(floating);

            section.Contents.Add("After the float is some text that should continue to flow around the div. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc congue, nisl nec sollicitudin facilisis, nulla magna facilisis tortor, vitae facilisis ipsum tortor eu urna. Morbi laoreet velit ex. Donec lectus nibh, tincidunt a arcu eleifend, fringilla dignissim eros. Maecenas tristique tortor et condimentum vestibulum. Nullam quis turpis at neque ultricies feugiat sit amet sed odio. Sed accumsan porttitor risus, ut convallis enim dignissim ac. Etiam id nibh id sem gravida dictum.");


            using (var ms = DocStreams.GetOutputStream("Float_LeftSizedToPageFlowingContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(1, content.PositionedRegions.Count);
            var pos = content.PositionedRegions[0];

            Assert.AreEqual(1, content.Columns.Length);

            //we are now over 6 lines
            Assert.AreEqual(6, content.Columns[0].Contents.Count);

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos run, start, chars, end

            var floatAddition = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float are explicit
            Unit w = 120;
            Unit h = 45;

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(section.TextLeading, innerLine.Height);

            Assert.AreEqual(w, innerBlock.Width);
            Assert.AreEqual(h, innerBlock.Height);

            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            //outer line content #1
            var startRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;

            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(chars);
            Assert.IsNotNull(startRun);
            Assert.AreEqual(0 + section.Margins.Left, startRun.StartTextCursor.Width); //first line at zero + margins
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the size and position
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(0, pos.TotalBounds.X);
            Assert.AreEqual(section.TextLeading, pos.TotalBounds.Y);
            Assert.AreEqual(w, pos.TotalBounds.Width);
            Assert.AreEqual(h, pos.TotalBounds.Height);

            //The following 3 lines should be running inset the width of the float 
            PDFTextRunSpacer spacer;
            var available = 600;
            Unit used;
            for (var i = 1; i < 4; i++)
            {
                line = content.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);
                spacer = line.Runs[0] as PDFTextRunSpacer;
                chars = line.Runs[1] as PDFTextRunCharacter;

                Assert.IsNotNull(spacer);
                Assert.IsNotNull(chars);
                Assert.AreEqual(w, spacer.Width);
                used = spacer.Width + chars.Width;
                Assert.IsTrue(available >= used);
            }

            //Fifth line is full width again
            line = content.Columns[0].Contents[4] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            spacer = line.Runs[0] as PDFTextRunSpacer;
            chars = line.Runs[1] as PDFTextRunCharacter;

            Assert.IsNotNull(spacer);
            Assert.IsNotNull(chars);
            Assert.AreEqual(0, spacer.Width);
            used = chars.Width;
            Assert.IsTrue(used > available - w);

            //6th line ends the literal
            line = content.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, spacer.Width);
            Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
        }

        /// <summary>
        /// Checks a float left div with explicit size with text before and after reaching beyond the size of the div.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftInMiddleSizedToPageWithMarginsFlowingContent()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 10;
            section.TextLeading = 15;
            section.Margins = 15;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 15;
            doc.Pages.Add(section);

            section.Contents.Add("A bit of content before the div. ");

            Span floating = new Span()
            {
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left,
                PositionMode = PositionMode.Block,
                Width = 120,
                Height = 45,
                Margins = 15
            };

            floating.Contents.Add(new TextLiteral("Floating Div"));
            section.Contents.Add(floating);

            section.Contents.Add("After the float is some text that should continue to flow around the div. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc congue, nisl nec sollicitudin facilisis, nulla magna facilisis tortor, vitae facilisis ipsum tortor eu urna. Morbi laoreet velit ex. Donec lectus nibh, tincidunt a arcu eleifend, fringilla dignissim eros. Maecenas tristique tortor et condimentum vestibulum. Nullam quis turpis at neque ultricies feugiat sit amet sed odio. Sed accumsan porttitor risus, ut convallis enim dignissim ac. Etiam id nibh id sem gravida dictum.");


            using (var ms = DocStreams.GetOutputStream("Float_LeftSizedToPageWithMarginsFlowingContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(1, content.PositionedRegions.Count);
            var pos = content.PositionedRegions[0];

            Assert.AreEqual(1, content.Columns.Length);

            //we are now over 6 lines
            Assert.AreEqual(6, content.Columns[0].Contents.Count);

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count); //start, chars, end, pos run, start, chars, end

            var floatAddition = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            var innerBlock = pos.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            //width and hight of the content and float are explicit with margins
            Unit w = 120;
            Unit h = 45;

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(section.TextLeading, innerLine.Height);

            Assert.AreEqual(w + (2 * 15), innerBlock.Width);
            Assert.AreEqual(h + (2 * 15), innerBlock.Height);

            Assert.AreEqual(w + (2 * 15), floatAddition.FloatWidth);
            Assert.AreEqual(h + (2 * 15), floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            //outer line content #1
            var startRun = line.Runs[0] as PDFTextRunBegin;
            chars = line.Runs[1] as PDFTextRunCharacter;

            var posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;


            Assert.IsNotNull(posRun);
            Assert.IsNotNull(chars);
            Assert.IsNotNull(startRun);
            Assert.AreEqual(0 + section.Margins.Left, startRun.StartTextCursor.Width); //first line at zero + margins
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //And the positioned region should also have the size and position
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            Assert.AreEqual(0, pos.TotalBounds.X);
            Assert.AreEqual(section.TextLeading, pos.TotalBounds.Y);
            Assert.AreEqual(w, pos.TotalBounds.Width);
            Assert.AreEqual(h + (2 * 15), pos.TotalBounds.Height); //TODO: TotalBounds.Height is including margins. Noted elsewhere and need to fix

            //The following 3 lines should be running inset the width of the float 
            PDFTextRunSpacer spacer;
            var available = 600;
            Unit used;
            for (var i = 1; i < 5; i++)
            {
                line = content.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);
                spacer = line.Runs[0] as PDFTextRunSpacer;
                chars = line.Runs[1] as PDFTextRunCharacter;

                Assert.IsNotNull(spacer);
                Assert.IsNotNull(chars);
                Assert.AreEqual(w + (2 * 15), spacer.Width); //float width + margins
                used = spacer.Width + chars.Width;
                Assert.IsTrue(available >= used);
            }

            //6th line ends the literal but still in the float are
            line = content.Columns[0].Contents[5] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);
            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(w + (2 * 15), spacer.Width);
            Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
        }

        /// <summary>
        /// Checks a float left div with explicit size AND a second floating div with text flowing around both divs.
        /// </summary>
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftTwiceSizedToPageWithMarginsFlowingContent()
        {
            Unit space = 15;

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 10;
            section.TextLeading = space;
            section.Margins = space;
            section.BorderColor = StandardColors.Lime;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = space;
            doc.Pages.Add(section);

            // First float at the front

            Div floating = new Div()
            {
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left,
                PositionMode = PositionMode.Block,
                Width = 120,
                Height = 45,
                Margins = 15
            };

            floating.Contents.Add(new TextLiteral("Floating Div"));
            section.Contents.Add(floating);

            // Some content

            section.Contents.Add("After the float is some text that should continue to flow around the div. Lorem ipsum dolor sit amet, consectetur adipiscing elit. ");

            // And a second float

            Div floating2 = new Div()
            {
                BackgroundColor = Drawing.StandardColors.Aqua,
                FloatMode = FloatMode.Left,
                PositionMode = PositionMode.Block,
                Width = 60,
                Height = 30,
                Margins = 15,
                FontSize = 7.5
            };

            floating2.Contents.Add(new TextLiteral("Second Floating Div"));
            section.Contents.Add(floating2);

            // And the final content

            section.Contents.Add("Nunc congue, nisl nec sollicitudin facilisis, nulla magna facilisis tortor, vitae facilisis ipsum tortor eu urna. Morbi laoreet velit ex. Donec lectus nibh, tincidunt a arcu eleifend, fringilla dignissim eros. Maecenas tristique tortor et condimentum vestibulum. Nullam quis turpis at neque ultricies feugiat sit amet sed odio. Sed accumsan porttitor risus, ut convallis enim dignissim ac. Etiam id nibh id sem gravida dictum.");


            using (var ms = DocStreams.GetOutputStream("Float_LeftTwiceSizedToPageWithMarginsFlowingContent.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Unit w1 = 120;
            Unit h1 = 45;

            Unit w2 = 60;
            Unit h2 = 30;

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;

            #region check the float additions

            //Float additions are added as a queue, LILO arrancement, so the second is at the top.

            var floatAddition2 = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition2);
            Assert.AreEqual(2, floatAddition2.Count);
            Assert.IsNotNull(floatAddition2.Prev);

            //And the first below

            var floatAddition1 = floatAddition2.Prev;
            Assert.AreEqual(1, floatAddition1.Count);

            Assert.AreEqual(0, floatAddition1.FloatInset);
            Assert.AreEqual(120 + (2 * 15), floatAddition1.FloatWidth);
            Assert.AreEqual(45 + (2 * 15), floatAddition1.FloatHeight);
            Assert.AreEqual(0, floatAddition1.YOffset);

            Assert.AreEqual(120 + (2 * 15), floatAddition2.FloatInset); //second should be inset by the width of the first
            Assert.AreEqual(30, floatAddition2.YOffset); //on the 3rd line
            Assert.AreEqual(60 + (2 * 15), floatAddition2.FloatWidth);
            Assert.AreEqual(30 + (2 * 15), floatAddition2.FloatHeight);


            #endregion

            #region check the positioned regions

            Assert.AreEqual(2, content.PositionedRegions.Count);

            var pos1 = content.PositionedRegions[0];
            var pos2 = content.PositionedRegions[1];

            //positioned region 1

            Assert.AreEqual(w1, pos1.Width); //TODO: Positioned regions with inner blocks that have margins - Layout still works
            Assert.AreEqual(h1, pos1.Height); //TODO: Positioned regions with inner blocks that have margins - Layout still works

            var innerBlock = pos1.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(innerBlock);
            Assert.AreEqual(1, innerBlock.Columns.Length);
            Assert.AreEqual(1, innerBlock.Columns[0].Contents.Count);
            var innerLine = innerBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(innerLine);

            Assert.AreEqual(3, innerLine.Runs.Count);
            var chars = innerLine.Runs[1] as PDFTextRunCharacter;
            Assert.IsNotNull(chars);

            Assert.AreEqual(chars.Width, innerLine.Width);
            Assert.AreEqual(section.TextLeading, innerLine.Height);

            Assert.AreEqual(w1 + (space * 2), innerBlock.Width); //Width and margins
            Assert.AreEqual(h1 + (space * 2), innerBlock.Height); //Width and margins

            //postioned region 2

            Assert.AreEqual(w2, pos2.Width); //TODO: Positioned regions with inner blocks that have margins - Layout still works
            Assert.AreEqual(h2, pos2.Height); //TODO: Positioned regions with inner blocks that have margins - Layout still works
            Assert.AreEqual(120 + (space * 2), pos2.TotalBounds.X);
            Assert.AreEqual(30, pos2.TotalBounds.Y); //Down on the 3rd line


            #endregion

            #region check the lines

            

            Assert.AreEqual(1, content.Columns.Length);

            //we are now over 7 lines
            Assert.AreEqual(7, content.Columns[0].Contents.Count);

            // line 1 - pos run, start, chars, end

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count);

            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var startRun = line.Runs[1] as PDFTextRunBegin;
            chars = line.Runs[2] as PDFTextRunCharacter;

            Assert.AreEqual(w1 + (space * 2) + space, startRun.StartTextCursor.Width); //margins of the float + page margins
            Assert.AreEqual(posRun.Region, content.PositionedRegions[0]);

            // line 2 - spacer, chars, end, pos2, begin, chars, end

            line = content.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(7, line.Runs.Count);

            var spacer = line.Runs[0] as PDFTextRunSpacer;
            chars = line.Runs[1] as PDFTextRunCharacter;
            var end = line.Runs[2] as PDFTextRunEnd;

            Assert.AreEqual(w1 + (space * 2), spacer.Width);

            posRun = line.Runs[3] as PDFLayoutPositionedRegionRun;
            Assert.AreEqual(posRun.Region, content.PositionedRegions[1]);

            startRun = line.Runs[4] as PDFTextRunBegin;
            Assert.AreEqual(w1 + (space * 2) + chars.Width + space, startRun.StartTextCursor.Width); //float1 + its margins + character + page margins

            // line 3, 4, 5 and 6 - offset is both floats, spacer, chars, newline

            for (var i = 2; i < 6; i++)
            {
                line = content.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);

                spacer = line.Runs[0] as PDFTextRunSpacer;
                chars = line.Runs[1] as PDFTextRunCharacter;

                Assert.AreEqual(w1 + (space * 2) + w2 + (space * 2), spacer.Width);

                if (i == 5)
                {
                    //check that the new line goes back the full offset to the page left
                    var newLine = line.Runs[2] as PDFTextRunNewLine;
                    var back = newLine.NewLineOffset.Width;
                    //like a carriage return the normal space to return to would be the start of the last line,
                    //but we need to move left both floats with margins
                    Assert.AreEqual(w1 + (space * 2) + w2 + (space * 2), back);
                }

            }


            line = content.Columns[0].Contents[6] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(3, line.Runs.Count);

            //zero width spacer

            spacer = line.Runs[0] as PDFTextRunSpacer;
            Assert.AreEqual(0, spacer.Width);

            #endregion


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 10;
            section.TextLeading = 15;
            section.Margins = 15;
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 15;
            doc.Pages.Add(section);


            Div wrapper = new Div()
            {
                BorderColor = StandardColors.Aqua,
                Padding = 15
            };
            section.Contents.Add(wrapper);

            Div floating = new Div()
            {
                BackgroundColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left
            };

            floating.Contents.Add(new TextLiteral("Floating Div"));
            wrapper.Contents.Add(floating);

            wrapper.Contents.Add("After the float");


            using (var ms = DocStreams.GetOutputStream("Float_LeftToContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }


            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;

            Unit pageW = 600;
            Unit pageH = 800;

            Assert.AreEqual(pageW, content.Width);
            Assert.AreEqual(pageH, content.Height);

            //Check the 10vw margins on the content block
            Unit margin = 15;
            Assert.AreEqual(margin, content.Position.Margins.Left);
            Assert.AreEqual(margin, content.Position.Margins.Right);
            Assert.AreEqual(margin, content.Position.Margins.Top);
            Assert.AreEqual(margin, content.Position.Margins.Bottom);
            Assert.AreEqual(600 - (margin * 2), content.AvailableBounds.Width);

            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var wrapperBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;
            var wrapW = pageW - (margin * 2);


            Assert.AreEqual(wrapW, wrapperBlock.Width); //wrapper is full width exluding margins

            Assert.AreEqual(1, wrapperBlock.PositionedRegions.Count);
            var pos = wrapperBlock.PositionedRegions[0];

            var floatW = pos.Width;
            var h = pos.Height;

            Assert.AreEqual(15, h); //line height

            var line = wrapperBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFLayoutPositionedRegionRun));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunCharacter));
            Assert.IsInstanceOfType(line.Runs[3], typeof(PDFTextRunEnd));

            var begin = line.Runs[1] as PDFTextRunBegin;
            var offset = floatW + margin + wrapper.Padding.Left;
            Assert.AreEqual(offset, begin.StartTextCursor.Width);
      


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftRelativeToPage()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 800;
            section.FontSize = 20;
            section.TextLeading = 25;
            doc.Pages.Add(section);

            Div floating = new Div() {
                Height = new Unit(20, PageUnits.Percent),
                Width = new Unit(50, PageUnits.Percent),
                BorderWidth = 1, BorderColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left
            };

            
            floating.Contents.Add(new TextLiteral("50% width and height"));
            section.Contents.Add(floating);

            section.Contents.Add("After the float");
            

            using (var ms = DocStreams.GetOutputStream("Float_LeftRelativeSizeToPage.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;


            Assert.AreEqual(1, content.PositionedRegions.Count);
            var pos = content.PositionedRegions[0];

            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);

            var line = content.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(4, line.Runs.Count); //float run, start, chars, end

            var floatAddition = content.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            Unit w = 600 / 2.0; // 50% width
            Unit h = 800 / 5.0; // 20% height

            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var startRun = line.Runs[1] as PDFTextRunBegin;

            Assert.IsNotNull(posRun);
            Assert.IsNotNull(startRun);
            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //the actual float should contain the relative width
            Assert.AreEqual(w, floatAddition.FloatWidth);
            Assert.AreEqual(h, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(w, pos.Width);
            Assert.AreEqual(h, pos.Height);

            //The start run should account for the float with the text cursors position (size)
            Assert.AreEqual(w, startRun.StartTextCursor.Width);

            
        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FloatLeftRelativeToContainer()
        {

            Document doc = new Document();
            Section section = new Section();
            section.Width = 600;
            section.Height = 900;
            section.FontSize = 20;
            section.TextLeading = 25;
            section.Margins = new Thickness(Unit.Vw(10));
            section.Style.OverlayGrid.ShowGrid = true;
            section.Style.OverlayGrid.GridSpacing = 30;
            doc.Pages.Add(section);

            Div wrapper = new Div()
            {
                Padding = new Thickness(Unit.Vw(5)),
                BorderColor = StandardColors.Blue
            };
            Div floating = new Div()
            {
                Height = new Unit(20, PageUnits.Percent),
                Width = new Unit(50, PageUnits.ViewPortWidth),
                BorderWidth = 1,
                BorderColor = Drawing.StandardColors.Red,
                FloatMode = FloatMode.Left
            };

            floating.Contents.Add(new TextLiteral("50% width and height"));
            section.Contents.Add(wrapper);
            wrapper.Contents.Add(floating);

            wrapper.Contents.Add("After the float");
            wrapper.Contents.Add(new Div() { Width = 60, Height = 20, BorderColor = StandardColors.Aqua });
            section.Contents.Add("After the wrapper");

            using (var ms = DocStreams.GetOutputStream("Float_LeftRelativeSizeToContainer.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(ms);
            }

            Assert.AreEqual(1, layout.AllPages.Count);
            var pg = layout.AllPages[0];
            var content = pg.ContentBlock;

            Unit pageW = 600;
            Unit pageH = 900;

            Assert.AreEqual(pageW, content.Width);
            Assert.AreEqual(pageH, content.Height);

            //Check the 10vw margins on the content block
            Unit margin = (pageW / 10);
            Assert.AreEqual(margin, content.Position.Margins.Left);
            Assert.AreEqual(margin, content.Position.Margins.Right);
            Assert.AreEqual(margin, content.Position.Margins.Top);
            Assert.AreEqual(margin, content.Position.Margins.Bottom);
            Assert.AreEqual(600 - (margin * 2), content.AvailableBounds.Width);
            
            Assert.AreEqual(0, content.PositionedRegions.Count);
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(2, content.Columns[0].Contents.Count);

            var wrapperBlock = content.Columns[0].Contents[0] as PDFLayoutBlock;
            var wrapW = pageW - (margin * 2);
            

            Assert.AreEqual(wrapW, wrapperBlock.Width); //wrapper is full width exluding margins

            Assert.AreEqual(1, wrapperBlock.PositionedRegions.Count);
            var pos = wrapperBlock.PositionedRegions[0];

            var posW = pageW / 2.0; //50vw
            var padding = pageW * 0.05; //5vw
            var posH = (pageH - (margin * 2) - (padding * 2)) * 0.2; //20 percent of available height
                                                                  //(which is the page height - margins and wrapper padding)

            Assert.AreEqual(posW, pos.Width);
            Assert.AreEqual(posH, pos.Height);

            var wrapH = posH + (padding * 2); //20% height of inner + the padding
            Assert.AreEqual(wrapH, wrapperBlock.Height);


            var floatAddition = wrapperBlock.Columns[0].Floats;
            Assert.IsNotNull(floatAddition);
            Assert.AreEqual(1, floatAddition.Count);
            Assert.IsNull(floatAddition.Prev);

            //the actual float should contain the relative width
            Assert.AreEqual(posW, floatAddition.FloatWidth);
            Assert.AreEqual(posH, floatAddition.FloatHeight);
            Assert.AreEqual(0, floatAddition.FloatInset);

            var line = wrapperBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var posRun = line.Runs[0] as PDFLayoutPositionedRegionRun;
            var startRun = line.Runs[1] as PDFTextRunBegin;


            //And the positioned region should also have the relative size
            Assert.AreEqual(pos, posRun.Region);
            Assert.AreEqual(posW, pos.Width);
            Assert.AreEqual(posH, pos.Height);

            Assert.AreEqual(0, posRun.Width); //The positioned run width is zero, as it does not affect the line height etc.
            Assert.AreEqual(0, posRun.Height); //The positioned run height is zero, as it does not affect the line height etc.

            //The start run should account for the float with the text cursors position (size) along with the padding and margins
            Assert.AreEqual(posW + margin + padding, startRun.StartTextCursor.Width);

        }


    }
}
