using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using System.IO;
using Scryber.PDF.Resources;
using static System.Net.Mime.MediaTypeNames;

namespace Scryber.UnitLayouts
{
    [TestClass]
    public class TextLayout_Tests
    {
        public TextLayout_Tests()
        {
        }

        PDFLayoutDocument layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private void AssertAreApproxEqual(double one, double two, string message, int precision = 5)
        {
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }


        #region public void ASingleLiteral()


        /// <summary>
        /// Checks the font and line sizing and line height in a default font at 24 points
        /// </summary>
        [TestMethod()]
        public void ASingleLiteral()
        {
            var doc = new Document();
            var pg = new Page();
            var size = 24.0;  //Point size of font
            var lead = size * 1.2; //default leading

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = size;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines " +
                "in the page with a default line height so that we can check the leading of default lines " +
                "as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteral");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];




            var ascent = 18.48;
            var halflead = (lead - size) / 2;
            var offset = halflead + ascent;

            var rsrc = doc.SharedResources[0] as PDFFontResource;

            Assert.IsNotNull(rsrc, "The font resource should be the one and only shared resource");
            Assert.IsNotNull(rsrc.Definition, "The font definition should not be null");

            var line = region.Contents[0] as PDFLayoutLine;
            AssertAreApproxEqual(lead, line.Height.PointsValue, "Line 0 was not the correct height", 2);
            AssertAreApproxEqual(offset, line.BaseLineOffset.PointsValue, "Line zero offset was not half the leading + ascender height", 2);

            //Check the heights of the continuation lines
            var newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
            Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line zero");

            for (var i = 1; i < 4; i++)
            {

                line = region.Contents[i] as PDFLayoutLine;
                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " was not the correct height", 2);

                if (i < 3)
                {
                    newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
                    Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line " + i);
                }
            }

        }

        #endregion

        #region public void ASingleLiteral()


        /// <summary>
        /// Checks the font and line sizing and line height in a default font at 24 points with explicit leading
        /// </summary>
        [TestMethod()]
        public void ASingleLiteralExplicitLineHeight()
        {
            var doc = new Document();
            var pg = new Page();
            var size = 24.0;  //Point size of font
            var lead = 40.0; // Line height

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = size;
            pg.TextLeading = lead;

            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines " +
                "in the page with a default line height so that we can check the leading of default lines " +
                "as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralWithLeading");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];




            var ascent = 18.48;
            var halflead = (lead - size) / 2;
            var offset = halflead + ascent;

            var rsrc = doc.SharedResources[0] as PDFFontResource;

            Assert.IsNotNull(rsrc, "The font resource should be the one and only shared resource");
            Assert.IsNotNull(rsrc.Definition, "The font definition should not be null");

            var line = region.Contents[0] as PDFLayoutLine;
            AssertAreApproxEqual(lead, line.Height.PointsValue, "Line 0 was not the correct height", 2);
            AssertAreApproxEqual(offset, line.BaseLineOffset.PointsValue, "Line zero offset was not half the leading + ascender height", 2);

            //Check the heights of the continuation lines
            var newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
            Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line zero");

            for (var i = 1; i < 4; i++)
            {

                line = region.Contents[i] as PDFLayoutLine;
                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " was not the correct height", 2);

                if (i < 3)
                {
                    newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
                    Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line " + i);
                }
            }

        }

        #endregion

        #region public void ASingleLiteralInTimes()

        /// <summary>
        /// Checks the font and line sizing in an explict system font at 24 point
        /// </summary>
        [TestMethod()]
        public void ASingleLiteralInTimes()
        {
            var fontFamily = "Times New Roman";
            var fontWeight = FontWeights.Regular;
            var fontStyle = FontStyle.Regular;

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            pg.FontFamily = new FontSelector(fontFamily);
            pg.FontWeight = fontWeight;
            pg.FontStyle = fontStyle;

            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));

            var font = Scryber.Drawing.FontFactory.GetFontDefinition(fontFamily, fontStyle, fontWeight);
            Assert.IsNotNull(font, "This test will fail if the  font is not present, or could not be loaded from the System fonts");

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralTimes");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            Assert.AreEqual(1, doc.SharedResources.Count);
            var fontrsrc = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(fontrsrc, "The first and only resource in the document should be a font resource");

            var defn = fontrsrc.Definition;
            Assert.IsNotNull(defn, "The font does not have a definition");
            //Use the fallback default of Times as this will be the loaded font.
            Assert.AreEqual("Times", defn.Family, "The '" + fontFamily + " font was not loaded by the document");

            var region = layout.AllPages[0].ContentBlock.Columns[0];


            var em = 24.0;  //Point size of font
            var metrics = defn.GetFontMetrics(em);


            var line = region.Contents[0] as PDFLayoutLine;
            AssertAreApproxEqual(24 * 1.2, line.Height.PointsValue, "Line 0 was not the correct height");


            //Check the heights of the continuation lines
            Assert.AreEqual(3, region.Contents.Count, "Expected 3 lines");

            for (var i = 1; i < 3; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                AssertAreApproxEqual(24 * 1.2, line.Height.PointsValue, "Line " + i + " was not the correct height");

            }

        }

        #endregion

        #region public void ASingleLiteralWithLeading()

        /// <summary>
        /// Checks the font and line sizing with an explicit line leading at 24pt
        /// </summary>
        [TestMethod()]
        public void ASingleLiteralWithLeading()
        {
            var doc = new Document();
            var pg = new Page();
            Unit leading = 40;

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.TextLeading = leading;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralWithLeading");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];
            Assert.AreEqual(4, region.Contents.Count, "The exected number of flowing lines was 4");

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.AreEqual(leading.PointsValue, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");
            }

        }

        #endregion

        #region public void ASingleLiteral()


        /// <summary>
        /// Checks the font and line sizing and line height in a default font at 24 points
        /// </summary>
        [TestMethod()]
        public void ASingleLiteralWithLineBreaks()
        {
            var doc = new Document();
            var pg = new Page();
            var size = 24.0;  //Point size of font
            var lead = size * 1.2; //default leading
            var lineCount = 6;

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = size;
            doc.Pages.Add(pg);

            pg.Contents.Add(new TextLiteral(
                "This is a text run that should run\r\n" +
                "over multiple lines\r\n" +
                "in the page with a explicit\r\n" +
                "line breaks so that we can check the\r\n" +
                "breaks of lines as they\r\n" +
                "flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralLineBreaks");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];




            var ascent = 18.48;
            var halflead = (lead - size) / 2;
            var offset = halflead + ascent;

            var rsrc = doc.SharedResources[0] as PDFFontResource;

            Assert.IsNotNull(rsrc, "The font resource should be the one and only shared resource");
            Assert.IsNotNull(rsrc.Definition, "The font definition should not be null");

            Assert.AreEqual(lineCount, region.Contents.Count);

            var line = region.Contents[0] as PDFLayoutLine;

            AssertAreApproxEqual(lead, line.Height.PointsValue, "Line 0 was not the correct height", 2);
            AssertAreApproxEqual(offset, line.BaseLineOffset.PointsValue, "Line zero offset was not half the leading + ascender height", 2);


            //Check the heights of the continuation lines
            var newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
            Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line zero");

            for (var i = 1; i < lineCount; i++)
            {

                line = region.Contents[i] as PDFLayoutLine;
                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " was not the correct height", 2);

                if (i < lineCount - 1)
                {
                    newLine = line.Runs[line.Runs.Count - 1] as PDFTextRunNewLine;
                    Assert.AreEqual(lead, newLine.NewLineOffset.Height, "Expected the line offset to be the same as the leading on line " + i);
                }
            }

        }

        #endregion

        #region public void ASingleLiteralInTimes12ptWithLeading()

        /// <summary>
        /// Checks the font and line sizing with an explicit line leading and font at 12pt
        /// </summary>
        [TestMethod()]
        public void ASingleLiteralInTimes12ptWithLeading()
        {
            var doc = new Document();
            var pg = new Page();

            Unit leading = 40;
            Unit size = 12;

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.TextLeading = leading;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 40;
            pg.Style.OverlayGrid.GridXOffset = 10;
            pg.Style.OverlayGrid.GridYOffset = 10;
            pg.FontSize = size;
            pg.FontFamily = new FontSelector("Serif");

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with an explict line height so that we can check the leading of default lines as they flow down the page. " +
                "Repeated text that should flow over more than two lines in the page with an explicit line height so that we can check the leading of lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralWithLeadingAt12ptSerif");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            //Get the font definition for optima
            Assert.AreEqual(1, doc.SharedResources.Count);
            var fontrsrc = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(fontrsrc);

            var defn = fontrsrc.Definition;
            Assert.IsNotNull(defn);
            Assert.AreEqual("Times", defn.Family);

            Assert.AreEqual(3, region.Contents.Count, "Expected 4 lines for the textual content");

            for (var i = 0; i < 3; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.AreEqual(leading.PointsValue, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");
            }


        }

        #endregion

        #region public void LiteralAfterABlock()

        /// <summary>
        /// Checks that the line is offset correctly after a sprevious sibling in the page
        /// </summary>
        [TestMethod()]
        public void LiteralAfterABlock()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var div = new Div();
            div.Contents.Add(new TextLiteral("Inner Content"));
            div.BorderColor = new Color(200, 255, 255);
            div.Padding = new Thickness(10);
            div.Height = 100;
            pg.Contents.Add(div);

            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            this.SaveAsPDF(doc, "Text_LiteralAfterABlock");

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            PDFLayoutRegion region = layout.AllPages[0].ContentBlock.Columns[0];

            Assert.AreEqual(5, region.Contents.Count);
            var block = region.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.AreEqual(100, block.Height);



            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var lead = em * 1.2; // line leading 
            var offsetY = 100.0;

            //Skip the first block
            for (var i = 1; i < 5; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(offsetY, line.OffsetY);
                Assert.AreEqual(24.0 * 1.2, line.Height);

                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");

                //add the leading for the next line
                offsetY += lead;
            }


        }

        #endregion

        /// <summary>
        /// Checks the font and line sizing along with offset Y for an explicit alignment at the bottom
        /// </summary>
        [TestMethod()]
        public void LiteralWithVAlignBottom()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.VerticalAlignment = VerticalAlignment.Bottom;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralVAlignBottom");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];


            var em = 24.0;  //Point size of font
            var lead = 24.0 * 1.2; //full leading


            //default sans-sefif is set up as follows
            var space = em * 0.2; // line leading - point size
            var desc = em * 0.25;  // descender height 6pt
            var asc = em * 0.75;  // ascender height 18pt

            var pgHeight = layout.AllPages[0].Size.Height - pg.Margins.Top - pg.Margins.Bottom;
            var lineCount = 4.0;
            double offsetY = 0; // pgHeight.PointsValue - (lineCount * lead);

            //The textrunbegin holds the starting position for rendering the text from the TotalBounds
            //This is held from the line heights
            var initialOffsetRun = (region.Contents[0] as PDFLayoutLine).Runs[0] as PDFTextRunBegin;
            AssertAreApproxEqual(initialOffsetRun.TotalBounds.Y.PointsValue, offsetY, "The initial text offset was not correct");

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);

                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");

                offsetY += lead;
            }




        }

        /// <summary>
        /// Checks the font and line sizing along with offset Y for an explicit aligmnent in the middle
        /// </summary>
        [TestMethod()]
        public void LiteralWithVAlignMiddle()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.VerticalAlignment = VerticalAlignment.Middle;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralVAlignMiddle");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];


            var em = 24.0;  //Point size of font
            var lead = 24.0 * 1.2; //full leading


            //default sans-sefif is set up as follows
            var space = em * 0.2; // line leading - point size
            var desc = em * 0.25;  // descender height 6pt
            var asc = em * 0.75;  // ascender height 18pt

            var pgHeight = layout.AllPages[0].Size.Height - pg.Margins.Top - pg.Margins.Bottom;
            var lineCount = 4.0;
            var all = pgHeight.PointsValue - (lineCount * lead);

            //divide all the space by 2 and this should be our middle start point;
            var half = all / 2.0;
            double offsetY = 0; //should not affect the text

            //The textrunbegin holds the starting position for rendering the text from the TotalBounds
            //This is held from the line heights
            var initialOffsetRun = (region.Contents[0] as PDFLayoutLine).Runs[0] as PDFTextRunBegin;
            AssertAreApproxEqual(initialOffsetRun.TotalBounds.Y.PointsValue, offsetY, "The initial text offset was not correct");

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);

                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");

                offsetY += lead;
            }

        }


        /// <summary>
        /// Checks the font and line sizing along with offset Y for an explicit aligmnent in the middle
        /// </summary>
        [TestMethod()]
        public void LiteralWithHAlignRight()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Right;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));
            pg.Contents.Add(new LineBreak());
            pg.Contents.Add(new TextLiteral("This is on a new line on it's own"));
            
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralHAlignRight");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(text);
                var twidth = text.Width;

                var offset = pgContentWidth - twidth;

                if (i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);
                    AssertAreApproxEqual(offset.PointsValue, start.LineInset.PointsValue, "First line inset should be " + offset);
                }
                else
                {
                    //check the new line offset from the previous line to this line
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    var prev = (PDFLayoutLine) region.Contents[i - 1];
                    var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                    var prevWidth = pgContentWidth - prev.Width;
                    var newWidth = pgContentWidth - twidth;
                    AssertAreApproxEqual(last.NewLineOffset.Width.PointsValue, (prevWidth - newWidth).PointsValue, "Line " + i + " has invalid offset from previous of " + last.NewLineOffset.Width );
                    //AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
                }


            }

        }
        
        /// <summary>
        /// Checks the font and line sizing along with offset Y for an explicit aligmnent in the middle
        /// </summary>
        [TestMethod()]
        public void LiteralVeryLongWithHAlignRight()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Right;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page\r\n\r\nThis is on a new line on it's own"));
            //pg.Contents.Add(new LineBreak());
            //pg.Contents.Add(new LineBreak());
            //pg.Contents.Add(new TextLiteral("This is on a new line on it's own"));
            
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            //doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralHAlignRight");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(text);
                var twidth = text.Width;

                var offset = pgContentWidth - twidth;

                if (i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);
                    AssertAreApproxEqual(offset.PointsValue, start.LineInset.PointsValue, "First line inset should be " + offset);
                }
                else
                {
                    //check the new line offset from the previous line to this line
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    var prev = (PDFLayoutLine) region.Contents[i - 1];
                    var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                    var prevWidth = pgContentWidth - prev.Width;
                    var newWidth = pgContentWidth - twidth;
                    AssertAreApproxEqual(last.NewLineOffset.Width.PointsValue, (prevWidth - newWidth).PointsValue, "Line " + i + " has invalid offset from previous of " + last.NewLineOffset.Width );
                    //AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
                }


            }

        }

        /// <summary>
        /// Checks the font and line sizing along with offset Y for an explicit aligmnent in the middle
        /// </summary>
        [TestMethod()]
        public void LiteralWithHAlignCentre()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Center;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));
            pg.Contents.Add(new LineBreak());
            pg.Contents.Add(new TextLiteral("This is on a new line on it's own"));
            
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            
            doc.RenderOptions.Compression = OutputCompressionType.None;
            //doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralHAlignCenter");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line);
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                Assert.IsNotNull(text);
                var twidth = text.Width;

                var offset = pgContentWidth - twidth;

                if (i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    Assert.IsNotNull(start);
                    AssertAreApproxEqual(offset.PointsValue / 2, start.LineInset.PointsValue, "First line inset should be " + (offset / 2));
                }
                else
                {
                    //check the new line offset from the previous line to this line
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    var prev = (PDFLayoutLine) region.Contents[i - 1];
                    var last = (PDFTextRunNewLine)prev.Runs[prev.Runs.Count - 1];
                    var prevWidth = pgContentWidth - prev.Width;
                    var newWidth = pgContentWidth - twidth;
                    AssertAreApproxEqual(last.NewLineOffset.Width.PointsValue, ((prevWidth - newWidth) / 2).PointsValue, "Line " + i + " has invalid offset from previous of " + last.NewLineOffset.Width );
                    //AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
                }


            }

        }


        [TestMethod]
        public void LiteralWithHAlignJustified()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Justified;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = false;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralHAlignJustified");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);

            for (var i = 0; i < 4; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                var twidth = text.Width + text.ExtraSpace;

                var offset = pgContentWidth - twidth;
                Assert.IsNotNull(line.LineSpacingOptions);

                if (i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    AssertAreApproxEqual(0, start.TotalBounds.X.PointsValue, "First line inset should be " + offset);
                    AssertAreApproxEqual(pgContentWidth.PointsValue, twidth.PointsValue, "First line width should be about the same as the page content width");
                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.IsTrue(line.LineSpacingOptions.WordSpace > 0.0);

                }
                else if (i == 3) //Last line is not justified
                {
                    Assert.AreEqual(twidth, text.Width, "Last lines should not be justified");
                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.AreEqual(0.0, line.LineSpacingOptions.WordSpace);
                }
                else
                {
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.IsTrue(line.LineSpacingOptions.WordSpace > 0.0);
                }


            }
        }

        [TestMethod]
        public void MultipleSpansWithHAlignJustified()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Justified;
            pg.FontSize = 14;

            var lead = 24.0;  //Full line height

            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = lead;
            pg.Style.OverlayGrid.GridXOffset = pg.Margins.Left;
            pg.Style.OverlayGrid.GridYOffset = pg.Margins.Top;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow "));
            var span = new Span();
            span.FontSize = 20;
            span.FontWeight = 700;
            span.Contents.Add(new TextLiteral("over more than two lines in the page with a default "));
            pg.Contents.Add(span);

            span = new Span();
            span.FontSize = 16;
            span.FontStyle = FontStyle.Italic;
            span.Contents.Add(new TextLiteral("line height so that we can check the leading of "));
            pg.Contents.Add(span);

            pg.Contents.Add(new TextLiteral("default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = false;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_MultipleSpansHAlignJustified");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];



            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);
            Assert.AreEqual(3, region.Contents.Count);

            for (var i = 0; i < 3; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line.LineSpacingOptions);


                if (i == 2) //Last line is not justified
                {

                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.AreEqual(0.0, line.LineSpacingOptions.WordSpace);
                    //The last line only has a max font size of 14pt - so should be reduced in height
                    Assert.AreEqual(14 * 1.2, line.Height.PointsValue);
                }
                else
                {
                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.IsTrue(line.LineSpacingOptions.WordSpace > 0.0);
                    Assert.AreEqual(lead, line.Height);


                    Unit w = Unit.Zero;
                    for (var r = 0; r < line.Runs.Count; r++)
                    {
                        var run = line.Runs[r];
                        w += run.Width;
                        if (run is PDFTextRunCharacter chars)
                            w += chars.ExtraSpace;
                    }

                    //The extra space is applied when justified but not updated on the line or region
                    //TODO: Update widths on the line an region for the extra space when justifying text.
                    //Be we can use the total bounds width.

                    Assert.AreEqual(region.TotalBounds.Width, w);
                }

            }
        }

        [TestMethod]
        public void MultipleSpansWithHAlignJustifiedFixedLineHeight()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.HorizontalAlignment = HorizontalAlignment.Justified;
            pg.FontSize = 14;
            pg.TextLeading = 40;


            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = pg.TextLeading;
            pg.Style.OverlayGrid.GridXOffset = pg.Margins.Left;
            pg.Style.OverlayGrid.GridYOffset = pg.Margins.Top;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a justified run that should flow "));

            var span = new Span();
            span.FontSize = 20;
            span.FontWeight = 700;
            span.Contents.Add(new TextLiteral("over more than two lines in the page with a fixed "));
            pg.Contents.Add(span);

            span = new Span();
            span.FontSize = 16;
            span.FontStyle = FontStyle.Italic;
            span.Contents.Add(new TextLiteral("line height of 40pt that we can check the leading of "));
            pg.Contents.Add(span);

            pg.Contents.Add(new TextLiteral("default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = false;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_MultipleSpansHAlignJustifiedFixedLine");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];

            var em = 24.0;  //Point size of font

            //default sans-sefif is set up as follows
            var pgContentWidth = layout.AllPages[0].Width - (pg.Margins.Left + pg.Margins.Right);
            Assert.AreEqual(3, region.Contents.Count);

            for (var i = 0; i < 3; i++)
            {
                var line = region.Contents[i] as PDFLayoutLine;
                Assert.IsNotNull(line.LineSpacingOptions);

                //All lines have the explicit line height, including the last one
                Assert.AreEqual(pg.TextLeading, line.Height);

                if (i == 2) //Last line is not justified
                {

                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.AreEqual(0.0, line.LineSpacingOptions.WordSpace);
                }
                else
                {
                    Assert.AreEqual(0.0, line.LineSpacingOptions.CharSpace);
                    Assert.IsTrue(line.LineSpacingOptions.WordSpace > 0.0);


                    Unit w = Unit.Zero;
                    for (var r = 0; r < line.Runs.Count; r++)
                    {
                        var run = line.Runs[r];
                        w += run.Width;
                        if (run is PDFTextRunCharacter chars)
                            w += chars.ExtraSpace;
                    }

                    //The extra space is applied when justified but not updated on the line or region
                    //TODO: Update widths on the line an region for the extra space when justifying text.
                    //Be we can use the total bounds width.

                    Assert.AreEqual(region.TotalBounds.Width, w);
                }



            }
        }

        string longText = "Quisque gravida elementum nisl, at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius. Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Curabitur viverra ligula ut tellus feugiat mattis. Curabitur id urna sed nulla gravida ultricies." +
                " Duis molestie mi id tincidunt mattis. Maecenas consectetur quis lectus nec lobortis. " +
                "Donec nec sapien eu mi commodo porta in quis nibh. Sed quam sem, tristique vel lobortis nec, " +
                "pulvinar id libero. Donec aliquet consectetur lorem, id hendrerit lectus feugiat a. " +
                "Mauris fringilla nunc consequat sapien varius, in pretium nibh dignissim. Duis in erat neque. " +
                "Cras dui purus, laoreet vel lacus nec, scelerisque posuere nisl. Nam sed rutrum metus. " +
                "Ut vel vehicula lorem. Morbi rutrum leo quis nunc lobortis, venenatis posuere dolor porta.";



        [TestMethod]
        public void ALongTextBlock()
        {
            var content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " + longText;


            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral(content));
            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteral");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;

            var contentW = layout.AllPages[0].ContentBlock.Width;

            Assert.AreEqual(17, region.Contents.Count);

            for (var i = 0; i < 17; i++)
            {
                var line = layout.AllPages[0].ContentBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);
                Assert.AreEqual(3, line.Runs.Count);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (i == 16)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }
        }


        /// <summary>
        /// Tests a long string literal that flows over 2 columns. Explicit font size, default leading.
        /// </summary>
        [TestMethod]
        public void ALongTextBlockMultiColumn()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " + longText + longText;


            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            pg.AlleyWidth = 10;
            pg.FontSize = 16;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral(text));
            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteralMultiColumn");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            Assert.AreEqual(1, layout.AllPages.Count);

            var lp = layout.AllPages[0];
            var content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(2, content.Columns.Length);

            //First Column

            var region = content.Columns[0] as PDFLayoutRegion;


            var colW = (content.AvailableBounds.Width - 10);
            colW = colW / 2.0;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            var lineH = 16 * 1.2;
            var pgH = lp.Height.PointsValue - 20.0;
            var lineCount = Math.Floor(pgH / lineH);

            Assert.AreEqual(lineCount + 1, region.Contents.Count);//all available plus an empty line at the end.


            PDFLayoutLine line;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));


            //Second Column

            region = content.Columns[1] as PDFLayoutRegion;

            Assert.AreEqual(colW, region.TotalBounds.Width);


            Assert.IsTrue(region.Contents.Count > 0);//all available plus an empty line at the end.
            lineCount = region.Contents.Count;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (i == lineCount - 1)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }
        }

        /// <summary>
        /// Tests a long string literal that flows over 2 pages. Explicit font size, default leading.
        /// </summary>
        [TestMethod]
        public void ALongTextBlockMultiPage()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " + longText + longText + longText + longText + longText;


            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = 16;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral(text));
            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteralMultiPage");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            var lp = layout.AllPages[0];
            var content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(1, content.Columns.Length);

            //First Page


            var region = content.Columns[0] as PDFLayoutRegion;


            var colW = content.AvailableBounds.Width;

            Assert.AreEqual(colW, region.TotalBounds.Width);

            var lineH = 16 * 1.2;
            var pgH = lp.Height.PointsValue - 20.0;
            var lineCount = Math.Floor(pgH / lineH);

            Assert.AreEqual(lineCount + 1, region.Contents.Count);//all available plus an empty line at the end.


            PDFLayoutLine line;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));


            //Second page

            lp = layout.AllPages[1];
            content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(1, content.Columns.Length);
            region = content.Columns[0] as PDFLayoutRegion;

            Assert.AreEqual(colW, region.TotalBounds.Width);


            Assert.IsTrue(region.Contents.Count > 0);//all available plus an empty line at the end.
            lineCount = region.Contents.Count;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (i == lineCount - 1)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }
        }

        /// <summary>
        /// Tests a long string literal that flows over 2 columns and onto a second page with 1 column of content.
        /// Explicit font size, default leading.
        /// </summary>
        [TestMethod]
        public void ALongTextBlockMultiPageMultiColumn()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " + longText + longText +
                longText + longText + longText;


            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            pg.FontSize = 16;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral(text));
            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteralMultiPageMultiColumn");

            Assert.AreEqual(2, layout.AllPages.Count);

            var lp = layout.AllPages[0];
            var content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(2, content.Columns.Length);

            //First Column

            var region = content.Columns[0] as PDFLayoutRegion;


            var colW = (content.AvailableBounds.Width - 10);
            colW = colW / 2.0;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            var lineH = 16 * 1.2;
            var pgH = lp.Height.PointsValue - 20.0;
            var lineCount = Math.Floor(pgH / lineH);

            Assert.AreEqual(lineCount + 1, region.Contents.Count);//all available plus an empty line at the end.


            PDFLayoutLine line;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));


            //Second Column

            region = content.Columns[1] as PDFLayoutRegion;

            Assert.AreEqual(colW, region.TotalBounds.Width);


            Assert.AreEqual(lineCount + 1, region.Contents.Count);//same number of lines as the first column

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));

            //Second page

            lp = layout.AllPages[1];
            content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(2, content.Columns.Length);

            //First Column - Page 2

            region = content.Columns[0] as PDFLayoutRegion;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            lineCount = region.Contents.Count;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (i == lineCount - 1)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            //Second Column - Page 2 = Empty

            region = content.Columns[1] as PDFLayoutRegion;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            lineCount = region.Contents.Count;
            Assert.AreEqual(0, lineCount);
        }

        [TestMethod()]
        public void ASingleLineBreak()
        {
            
            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.FontSize = 16;

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("Before the line break"));
            pg.Contents.Add(new LineBreak());
            pg.Contents.Add(new TextLiteral("After the line break"));

            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLineBreak");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            var lp = layout.AllPages[0];
            var content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(1, content.Columns.Length);

            var region = content.Columns[0];
            Assert.AreEqual(2, region.Contents.Count);
            var lineH = 16 * 1.2;
            Assert.AreEqual(lineH, region.Contents[0].Height);
            Assert.AreEqual(lineH, region.Contents[1].Height);
        }





        /// <summary>
        /// Tests a long string literal that flows over 2 columns on 2 pages. Explicit font size, and explicit leading.
        /// </summary>
        [TestMethod]
        public void ALongTextBlockMultiPageMultiColumnExplicitLeading()
        {
            var text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " + longText + longText +
                longText + longText + longText;


            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            pg.ColumnCount = 2;
            pg.FontSize = 16;
            pg.TextLeading = 25;
            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral(text));
            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteralMultiPageMultiColumnExplicitLeading");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            Assert.AreEqual(2, layout.AllPages.Count);

            var lp = layout.AllPages[0];
            var content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(2, content.Columns.Length);

            //First Column

            var region = content.Columns[0] as PDFLayoutRegion;


            var colW = (content.AvailableBounds.Width - 10);
            colW = colW / 2.0;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            var lineH = 25;
            var pgH = lp.Height.PointsValue - 20.0;
            var lineCount = Math.Floor(pgH / lineH);

            Assert.AreEqual(lineCount + 1, region.Contents.Count);//all available plus an empty line at the end.


            PDFLayoutLine line;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));


            //Second Column

            region = content.Columns[1] as PDFLayoutRegion;

            Assert.AreEqual(colW, region.TotalBounds.Width);


            Assert.AreEqual(lineCount + 1, region.Contents.Count);//same number of lines as the first column

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));

            //Second page

            lp = layout.AllPages[1];
            content = lp.ContentBlock;
            Assert.AreEqual(lp.Width - 20, content.Width); //page width - 10pt margins
            Assert.AreEqual(2, content.Columns.Length);

            //First Column - Page 2 - full

            region = content.Columns[0] as PDFLayoutRegion;

            Assert.AreEqual(colW, region.TotalBounds.Width);


            Assert.AreEqual(lineCount + 1, region.Contents.Count);//same number of lines as the first column

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);
                Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

            line = region.Contents[(int)lineCount] as PDFLayoutLine; //last line is empty
            Assert.AreEqual(2, line.Runs.Count);
            Assert.AreEqual(0.0, line.Height);

            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunEnd));


            //Second Column - Page 2

            region = content.Columns[1] as PDFLayoutRegion;
            Assert.AreEqual(colW, region.TotalBounds.Width);

            lineCount = region.Contents.Count;

            for (var i = 0; i < lineCount; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < colW);
                Assert.AreEqual(3, line.Runs.Count);
                Assert.AreEqual(lineH, line.Height.PointsValue);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (i == lineCount - 1)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine));
            }

        }


        [TestMethod]
        public void ALongTextBlockWithHypens()
        {



            var content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Quisque gravida elementum nisl, at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius. Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Curabitur viverra ligula ut tellus feugiat mattis. Curabitur id urna sed nulla gravida ultricies." +
                " Duis molestie mi id tincidunt mattis. Maecenas consectetur quis lectus nec lobortis. " +
                "Donec nec sapien eu mi commodo porta in quis nibh. Sed quam sem, tristique vel lobortis nec, " +
                "pulvinar id libero. Donec aliquet consectetur lorem, id hendrerit lectus feugiat a. " +
                "Mauris fringilla nunc consequat sapien varius, in pretium nibh dignissim. Duis in erat neque. " +
                "Cras dui purus, laoreet vel lacus nec, scelerisque posuere nisl. Nam sed rutrum metus. " +
                "Ut vel vehicula lorem. Morbi rutrum leo quis nunc lobortis, venenatis posuere dolor porta.";




            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.FontSize = 12;
            pg.ColumnCount = 4; //make small columns to force hypenation
            pg.Style.Text.Hyphenation = Text.WordHyphenation.Auto;
            pg.HorizontalAlignment = HorizontalAlignment.Justified;

            pg.Contents.Add(new TextLiteral(content));
            pg.Contents.Add(new ColumnBreak());

            var div = new Div();
            div.Style.Text.HyphenationCharacterAppended = '?';
            div.Style.Text.HyphenationMinCharsAfter = 5;
            div.Style.Text.HyphenationMinCharsBefore = 5;
            div.Style.Border.Width = 1;

            div.Contents.Add(new TextLiteral(content));
            pg.Contents.Add(div); //second column

            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            doc.RenderOptions.ConformanceMode = ParserConformanceMode.Strict;
            doc.AppendTraceLog = true;

            SaveAsPDF(doc, "Text_LongLiteralWithHyphens");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;

            var contentW = layout.AllPages[0].ContentBlock.Width;

            Assert.AreEqual(36, region.Contents.Count);
            var colHyphens = new int[] { 1, 3, 4, 8, 12, 14, 15, 17, 31 };

            for (var i = 0; i < 36; i++)
            {
                var line = layout.AllPages[0].ContentBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);
                Assert.AreEqual(3, line.Runs.Count);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                var chars = line.Runs[1] as PDFTextRunCharacter;

                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (Array.IndexOf(colHyphens, i) > -1)
                    Assert.IsTrue(chars.Characters.EndsWith("-"), "Line " + i + " was expected to end with a hyphen for characters: " + chars.Characters);
                else
                    Assert.IsFalse(chars.Characters.EndsWith("-"), "Line " + i + " was expected to NOT end with a hyphen for characters: " + chars.Characters);


                if (i == 35)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd), "Failed line " + i);
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine), "Failed line " + i);
            }

            //Second column - min 5 chars both and a ? as the separator

            region = layout.AllPages[0].ContentBlock.Columns[1] as PDFLayoutRegion;
            var divBlock = region.Contents[0] as PDFLayoutBlock;

            Assert.AreEqual(36, divBlock.Columns[0].Contents.Count);
            colHyphens = new int[] { 1, 8 };

            for (var i = 0; i < 36; i++)
            {
                var line = divBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);
                Assert.AreEqual(3, line.Runs.Count);

                if (i == 0)
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
                else
                    Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                var chars = line.Runs[1] as PDFTextRunCharacter;

                Assert.AreEqual(line.Width, line.Runs[1].Width);

                if (Array.IndexOf(colHyphens, i) > -1)
                    Assert.IsTrue(chars.Characters.EndsWith("?"), "Line " + i + " on second column was expected to end with a hyphen for characters: " + chars.Characters);
                else
                    Assert.IsFalse(chars.Characters.EndsWith("?"), "Line " + i + " on second column was expected to NOT end with a hyphen for characters: " + chars.Characters);


                if (i == 35)
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd), "Failed line " + i + " on second column");
                else
                    Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunNewLine), "Failed line " + i + " on second column");
            }
        }

        [TestMethod()]
        public void BoldAndItalicSpans()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.FontFamily = new FontSelector("Sans-Serif");
            pg.FontSize = 20;
            
            var span = new Span();
            span.Contents.Add(new TextLiteral("This is a text run that should flow over more than "));
            pg.Contents.Add(span);

            span = new Span();
            span.FontBold = true;
            span.Contents.Add(new TextLiteral("two lines in the page with a default line height "));
            pg.Contents.Add(span);

            span = new Span();
            span.FontItalic = true;
            span.Contents.Add(new TextLiteral("so that we can check the leading "));
            pg.Contents.Add(span);


            span = new Span();
            span.FontBold = true;
            span.FontItalic = true;
            span.Contents.Add(new TextLiteral("of default lines as they flow down the page"));
            pg.Contents.Add(span);

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;

            SaveAsPDF(doc, "Text_LiteralsInBoldAndItalic");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            // line 1

            PDFLayoutLine first = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(24, first.Height);
            Assert.AreEqual(9, first.Runs.Count);
            Assert.IsInstanceOfType(first.Runs[0], typeof(PDFLayoutInlineBegin));
            Assert.IsInstanceOfType(first.Runs[1], typeof(PDFTextRunBegin));

            Assert.IsInstanceOfType(first.Runs[2], typeof(PDFTextRunCharacter));
            Assert.AreEqual("This is a text run that should flow over more than ", ((first.Runs[2]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[3], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(first.Runs[4], typeof(PDFLayoutInlineEnd));
            Assert.IsInstanceOfType(first.Runs[5], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (first.Runs[5] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.IsInstanceOfType(first.Runs[6], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(first.Runs[7], typeof(PDFTextRunCharacter));
            Assert.AreEqual("two lines in", ((first.Runs[7]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[8], typeof(PDFTextRunNewLine));
            

            //Line 2

            PDFLayoutLine second = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(24, second.Height);
            Assert.AreEqual(8, second.Runs.Count);

            Assert.IsInstanceOfType(second.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (second.Runs[0] as PDFTextRunSpacer).Width);
            Assert.IsInstanceOfType(second.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("the page with a default line height ", ((second.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(second.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(second.Runs[3], typeof(PDFLayoutInlineEnd));


            Assert.IsInstanceOfType(second.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(400, (second.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.IsInstanceOfType(second.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(second.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual("so that we can check the", ((second.Runs[6]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(second.Runs[7], typeof(PDFTextRunNewLine));
            
            //Line 3

            PDFLayoutLine third = layout.AllPages[0].ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(third);
            Assert.AreEqual(24, third.Height);
            Assert.AreEqual(9, third.Runs.Count);

            Assert.IsInstanceOfType(third.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (third.Runs[0] as PDFTextRunSpacer).Width);
            Assert.IsInstanceOfType(third.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("leading ", ((third.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(third.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(third.Runs[3], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(third.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.IsInstanceOfType(third.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(third.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual("of default lines as they flow down the page", ((third.Runs[6]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(third.Runs[7], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(third.Runs[8], typeof(PDFLayoutInlineEnd));
        }

        [TestMethod()]
        public void ItalicThenRegularSpans()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.FontFamily = new FontSelector("Sans-Serif");
            pg.FontSize = 20;


            var ital = new Div();
            ital.FontStyle = FontStyle.Italic;
            ital.Contents.Add(new TextLiteral("This is an italic run with an "));
            pg.Contents.Add(ital);

            var inner = new Span();
            inner.FontItalic = false;
            inner.Contents.Add(new TextLiteral("inner regular font."));
            ital.Contents.Add(inner);

            ital.Contents.Add(new TextLiteral(" And after the regular, back to italic"));

            doc.LayoutComplete += Doc_LayoutComplete;

            SaveAsPDF(doc, "Text_LiteralsInItalicWithInnerRegular");
        }

        [TestMethod()]
        public void MultipleFixedLeadingSpans()
        {
            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.FontFamily = new FontSelector("Sans-Serif");
            pg.FontSize = 15;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 25;
            pg.Style.OverlayGrid.GridYOffset = 10;

            var span = new Span();
            span.Contents.Add(new TextLiteral("50pt leading text run that should flow over more than "));
            span.TextLeading = 50;
            pg.Contents.Add(span);

            span = new Span();
            span.FontBold = true;
            span.Contents.Add(new TextLiteral("two lines pushing the default line height "));
            pg.Contents.Add(span);

            span = new Span();
            span.FontItalic = true;
            
            span.Contents.Add(new TextLiteral("so that we can check the leading "));
            pg.Contents.Add(span);


            span = new Span();
            span.FontBold = true;
            span.FontItalic = true;
            span.TextLeading = 25;
            span.Contents.Add(new TextLiteral("of default lines and fixed leading, down the page and onto new lines"));

            pg.Contents.Add(span);

            span = new Span();
            span.FontItalic = true;
            span.Contents.Add(new TextLiteral(" with more content back at 50pt"));
            span.TextLeading = 50;
            pg.Contents.Add(span);

            doc.RenderOptions.Compression = OutputCompressionType.None;
            //doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;

            SaveAsPDF(doc, "Text_LiteralsInBoldAndItalicWithMultipleLeading");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            // line 1

            PDFLayoutLine first = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(50, first.Height);
            Assert.AreEqual(9, first.Runs.Count);
            Assert.IsInstanceOfType(first.Runs[0], typeof(PDFLayoutInlineBegin));
            Assert.IsInstanceOfType(first.Runs[1], typeof(PDFTextRunBegin));
            var begin = first.Runs[1] as PDFTextRunBegin;
            Assert.AreEqual(first.BaseLineOffset + 10, begin.StartTextCursor.Height); //margin top + offset

            Assert.IsInstanceOfType(first.Runs[2], typeof(PDFTextRunCharacter));
            Assert.AreEqual("50pt leading text run that should flow over more than ", ((first.Runs[2]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[3], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(first.Runs[4], typeof(PDFLayoutInlineEnd));
            Assert.IsInstanceOfType(first.Runs[5], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (first.Runs[5] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.IsInstanceOfType(first.Runs[6], typeof(PDFTextRunBegin));

            begin = first.Runs[6] as PDFTextRunBegin;
            Assert.AreEqual(first.BaseLineOffset + 10, begin.StartTextCursor.Height); //margin top + offset

            Assert.IsInstanceOfType(first.Runs[7], typeof(PDFTextRunCharacter));
            Assert.AreEqual("two lines pushing the default", ((first.Runs[7]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[8], typeof(PDFTextRunNewLine));

            //Line 2

            PDFLayoutLine second = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.AreEqual(25, second.Height);
            
            //Check the offset of the previous line
            Assert.AreEqual(first.BaseLineToBottom + second.BaseLineOffset, (first.Runs[8] as PDFTextRunNewLine).NewLineOffset.Height);

            Assert.AreEqual(13, second.Runs.Count);
            Assert.IsInstanceOfType(second.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (second.Runs[0] as PDFTextRunSpacer).Width);
            Assert.IsInstanceOfType(second.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("line height ", ((second.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(second.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(second.Runs[3], typeof(PDFLayoutInlineEnd));


            Assert.IsInstanceOfType(second.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(400, (second.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);

            Assert.IsInstanceOfType(second.Runs[5], typeof(PDFTextRunBegin));
            begin = second.Runs[5] as PDFTextRunBegin;
            Assert.AreEqual(first.Height + 10 + second.BaseLineOffset, begin.StartTextCursor.Height); //margin top + first height + offset

            Assert.IsInstanceOfType(second.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual("so that we can check the leading ", ((second.Runs[6]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(second.Runs[7], typeof(PDFTextRunEnd));
          

            Assert.IsInstanceOfType(second.Runs[8], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(second.Runs[9], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (second.Runs[9] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (second.Runs[9] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);

            Assert.IsInstanceOfType(second.Runs[10], typeof(PDFTextRunBegin));
            begin = second.Runs[10] as PDFTextRunBegin;
            Assert.AreEqual(first.Height + 10 + second.BaseLineOffset, begin.StartTextCursor.Height); //margin top + first height + offset

            Assert.IsInstanceOfType(second.Runs[11], typeof(PDFTextRunCharacter));
            Assert.AreEqual("of default lines and fixed leading,", ((second.Runs[11]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(second.Runs[12], typeof(PDFTextRunNewLine));

            //Line 3

            PDFLayoutLine third = layout.AllPages[0].ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.IsNotNull(third);
            Assert.AreEqual(50, third.Height);

            //Check the offset of the previous line to the new line
            Assert.AreEqual(second.BaseLineToBottom + third.BaseLineOffset, (second.Runs[12] as PDFTextRunNewLine).NewLineOffset.Height);

            Assert.AreEqual(9, third.Runs.Count);

            Assert.IsInstanceOfType(third.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (third.Runs[0] as PDFTextRunSpacer).Width);

            Assert.IsInstanceOfType(third.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("down the page and onto new lines", ((third.Runs[1]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(third.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(third.Runs[3], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(third.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(400, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);

            Assert.IsInstanceOfType(third.Runs[5], typeof(PDFTextRunBegin));
            begin = third.Runs[5] as PDFTextRunBegin;
            Assert.AreEqual(10 + first.Height + second.Height + third.BaseLineOffset, begin.StartTextCursor.Height); //margin top + first height + second height + offset

            Assert.IsInstanceOfType(third.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual(" with more content back at 50pt", ((third.Runs[6]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(third.Runs[7], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(second.Runs[8], typeof(PDFLayoutInlineEnd));

        }


        [TestMethod()]
        public void MultipleFixedLeadingSpansOverlow()
        {
            Unit overflowBlockHeight = 750;

            var doc = new Document();
            var pg = new Section();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.FontFamily = new FontSelector("Sans-Serif");
            pg.FontSize = 15;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 25;
            pg.Style.OverlayGrid.GridYOffset = 10;

            var div = new Div() { Height = overflowBlockHeight, BorderColor = Drawing.StandardColors.Black };
            pg.Contents.Add(div);

            var span = new Span();
            span.Contents.Add(new TextLiteral("50pt leading text run that should flow over more than "));
            span.TextLeading = 50;
            pg.Contents.Add(span);

            span = new Span();
            span.FontBold = true;
            span.Contents.Add(new TextLiteral("two lines pushing the default line height which fits on the page, "));
            pg.Contents.Add(span);

            
            span = new Span();
            span.FontBold = true;
            span.FontItalic = true;
            span.TextLeading = 25;
            span.Contents.Add(new TextLiteral("and fixed leading that doesn't fit, over the page and onto a new line"));

            pg.Contents.Add(span);

            span = new Span();
            span.FontItalic = true;
            span.Contents.Add(new TextLiteral(" with more content back at 50pt"));
            span.TextLeading = 50;
            pg.Contents.Add(span);

            doc.RenderOptions.Compression = OutputCompressionType.None;
            //doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;

            SaveAsPDF(doc, "Text_LiteralsInBoldAndItalicWithMultipleLeadingOverflowing");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            Assert.AreEqual(2, layout.AllPages.Count);
            Assert.AreEqual(3, layout.AllPages[0].ContentBlock.Columns[0].Contents.Count);

            var block = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(block);
            Assert.AreEqual(overflowBlockHeight, block.Height);
            Assert.AreEqual(layout.AllPages[0].Width - 20, block.Width);


            // line 1

            PDFLayoutLine first = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(first);
            Assert.AreEqual(50, first.Height);
            Assert.AreEqual(9, first.Runs.Count);
            Assert.IsInstanceOfType(first.Runs[0], typeof(PDFLayoutInlineBegin));
            Assert.IsInstanceOfType(first.Runs[1], typeof(PDFTextRunBegin));
            var begin = first.Runs[1] as PDFTextRunBegin;
            Assert.AreEqual(first.BaseLineOffset + 10 + overflowBlockHeight, begin.StartTextCursor.Height); //margin top + offset + block

            Assert.IsInstanceOfType(first.Runs[2], typeof(PDFTextRunCharacter));
            Assert.AreEqual("50pt leading text run that should flow over more than ", ((first.Runs[2]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[3], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(first.Runs[4], typeof(PDFLayoutInlineEnd));
            Assert.IsInstanceOfType(first.Runs[5], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (first.Runs[5] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.IsInstanceOfType(first.Runs[6], typeof(PDFTextRunBegin));

            begin = first.Runs[6] as PDFTextRunBegin;
            Assert.AreEqual(first.BaseLineOffset + 10 + overflowBlockHeight, begin.StartTextCursor.Height); //margin top + offset + block

            Assert.IsInstanceOfType(first.Runs[7], typeof(PDFTextRunCharacter));
            Assert.AreEqual("two lines pushing the default", ((first.Runs[7]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(first.Runs[8], typeof(PDFTextRunNewLine));

            //Line 2

            PDFLayoutLine second = layout.AllPages[0].ContentBlock.Columns[0].Contents[2] as PDFLayoutLine;
            Assert.AreEqual(15 * 1.2, second.Height); //default height

            //Check the offset of the previous line
            Assert.AreEqual(first.BaseLineToBottom + second.BaseLineOffset, (first.Runs[8] as PDFTextRunNewLine).NewLineOffset.Height);

            //We can finish this span

            Assert.AreEqual(7, second.Runs.Count);
            Assert.IsInstanceOfType(second.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (second.Runs[0] as PDFTextRunSpacer).Width);

            Assert.IsInstanceOfType(second.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("line height which fits on the page, ", ((second.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(second.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(second.Runs[3], typeof(PDFLayoutInlineEnd));


            //but the next is too high so it overflows onto the next page. So it has an inline-begin, text-begin and then a text-end.
            Assert.IsInstanceOfType(second.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (second.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (second.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);
            Assert.IsInstanceOfType(second.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(second.Runs[6], typeof(PDFTextRunEnd));


            //And goes onto the second page
            var pg2first = layout.AllPages[1].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(pg2first);
            Assert.AreEqual(50, pg2first.Height); //line height is from the second span on this line (not the first which is 25).
            Assert.AreEqual(8, pg2first.Runs.Count);

            Assert.IsInstanceOfType(pg2first.Runs[0], typeof(PDFTextRunBegin));
            begin = pg2first.Runs[0] as PDFTextRunBegin;
            Assert.AreEqual(10 + pg2first.BaseLineOffset, begin.StartTextCursor.Height); //margin top + first offset


            Assert.IsInstanceOfType(pg2first.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("and fixed leading that doesn't fit, over the page and onto a new line", ((pg2first.Runs[1]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(pg2first.Runs[2], typeof(PDFTextRunEnd));

            Assert.IsInstanceOfType(pg2first.Runs[3], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(pg2first.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(400, (pg2first.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (pg2first.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);

            Assert.IsInstanceOfType(pg2first.Runs[5], typeof(PDFTextRunBegin));
            begin = pg2first.Runs[5] as PDFTextRunBegin;
            Assert.AreEqual(10 + pg2first.BaseLineOffset, begin.StartTextCursor.Height); //margin top + offset

            Assert.IsInstanceOfType(pg2first.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual(" with more", ((pg2first.Runs[6]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(pg2first.Runs[7], typeof(PDFTextRunNewLine));

            //Line 2 on second page

            PDFLayoutLine pg2second = layout.AllPages[1].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(pg2second);
            Assert.AreEqual(50, pg2second.Height);


            //Check the offset of the previous line to the new line
            Assert.AreEqual(pg2first.BaseLineToBottom + pg2second.BaseLineOffset, (pg2first.Runs[7] as PDFTextRunNewLine).NewLineOffset.Height);

            Assert.AreEqual(4, pg2second.Runs.Count);

            Assert.IsInstanceOfType(pg2second.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (pg2second.Runs[0] as PDFTextRunSpacer).Width);

            Assert.IsInstanceOfType(pg2second.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("content back at 50pt", ((pg2second.Runs[1]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(pg2second.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(pg2second.Runs[3], typeof(PDFLayoutInlineEnd));

        }


        [TestMethod]
        public void LiteralWithExplicitWordAndCharSpacing()
        {
            var wide = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Quisque gravida elementum nisl, at ultrices odio suscipit interdum. " +
                "Sed sed diam non sem fringilla varius. Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
                "Curabitur viverra ligula ut tellus feugiat mattis. Curabitur id urna sed nulla gravida ultricies.";
            var evenwider = " Duis molestie mi id tincidunt mattis. Maecenas consectetur quis lectus nec lobortis. " +
                "Donec nec sapien eu mi commodo porta in quis nibh. Sed quam sem, tristique vel lobortis nec, " +
                "pulvinar id libero. Donec aliquet consectetur lorem, id hendrerit lectus feugiat a. " +
                "Mauris fringilla nunc consequat sapien varius, in pretium nibh dignissim. Duis in erat neque. ";
            var restorewide = "Cras dui purus, laoreet vel lacus nec, scelerisque posuere nisl. Nam sed rutrum metus. " +
                "Ut vel vehicula lorem. Morbi rutrum leo quis nunc lobortis, venenatis posuere dolor porta.";

            var normal = "After the explicit spacing that fits all on a single line.";
            //just long enough to go across the line without spacing so we know it has reset.

            var doc = new Document();
            var pg = new Page();

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);

            var span1 = new Span();
            span1.TextCharacterSpacing = 3;
            span1.TextWordSpacing = 10;
            span1.Contents.Add(new TextLiteral(wide));

            var span2 = new Span();
            span2.TextCharacterSpacing = 5;
            span2.TextWordSpacing = 20;
            span2.FontWeight = 700;
            span2.Contents.Add(new TextLiteral(evenwider));
            span1.Contents.Add(span2);

            span1.Contents.Add(new TextLiteral(restorewide));

            //add the mixed span to the page.
            pg.Contents.Add(span1);
            pg.Contents.Add(new LineBreak());
            pg.Contents.Add(new TextLiteral(normal));

            //pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LiteralWithExplicitWordAndCharSpacing");

            

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;

            var contentW = layout.AllPages[0].ContentBlock.Width;

            Assert.AreEqual(27, region.Contents.Count);

            //Line 0

            var line = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(4, line.Runs.Count);
            var start = line.Runs[0] as PDFLayoutInlineBegin;
            var begin = line.Runs[1] as PDFTextRunBegin;
            Assert.IsNotNull(begin);

            
            Assert.IsNotNull(begin.TextRenderOptions);
            Assert.IsTrue(begin.TextRenderOptions.CharacterSpacing.HasValue);
            Assert.AreEqual(3.0, begin.TextRenderOptions.CharacterSpacing.Value.PointsValue);

            Assert.IsTrue(begin.TextRenderOptions.WordSpacing.HasValue);
            Assert.AreEqual(10.0, begin.TextRenderOptions.WordSpacing.Value.PointsValue);

            //Lines 1 to 7 inc. are normal flowing lines

            for (var i = 1; i < 8; i++)
            {
                line = layout.AllPages[0].ContentBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);

                Assert.AreEqual(3, line.Runs.Count);
                Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

            }

            //Line 8 is a split

            line = layout.AllPages[0].ContentBlock.Columns[0].Contents[8] as PDFLayoutLine;
            Assert.AreEqual(7, line.Runs.Count);
            //Spacer, Chars, End, InlineBegin, TextBegin, Chars, NewLine

            begin = line.Runs[4] as PDFTextRunBegin;
            Assert.IsNotNull(begin.TextRenderOptions);
            Assert.IsTrue(begin.TextRenderOptions.CharacterSpacing.HasValue);
            Assert.AreEqual(5.0, begin.TextRenderOptions.CharacterSpacing.Value.PointsValue);

            Assert.IsTrue(begin.TextRenderOptions.WordSpacing.HasValue);
            Assert.AreEqual(20.0, begin.TextRenderOptions.WordSpacing.Value.PointsValue);

            //Lines 9 to 20 inc. are normal flowing lines
            for (var i = 9; i < 21; i++)
            {
                line = layout.AllPages[0].ContentBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);

                Assert.AreEqual(3, line.Runs.Count);
                Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));

                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

            }

            //Line 21 is a split, and reverts back to original spacing

            line = layout.AllPages[0].ContentBlock.Columns[0].Contents[21] as PDFLayoutLine;
            Assert.AreEqual(6, line.Runs.Count);
            //Spacer, End, InlineEnd, TextBegin, Chars, NewLine

            begin = line.Runs[3] as PDFTextRunBegin;
            Assert.IsNotNull(begin.TextRenderOptions);
            Assert.IsTrue(begin.TextRenderOptions.CharacterSpacing.HasValue);
            Assert.AreEqual(3.0, begin.TextRenderOptions.CharacterSpacing.Value.PointsValue);

            Assert.IsTrue(begin.TextRenderOptions.WordSpacing.HasValue);
            Assert.AreEqual(10.0, begin.TextRenderOptions.WordSpacing.Value.PointsValue);


            //Lines 9 to 20 inc. are normal flowing lines
            for (var i = 22; i < 25; i++)
            {
                line = layout.AllPages[0].ContentBlock.Columns[0].Contents[i] as PDFLayoutLine;
                Assert.IsTrue(line.Width < contentW);

                Assert.AreEqual(3, line.Runs.Count);
                Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
                Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
                Assert.AreEqual(line.Width, line.Runs[1].Width);

            }

            //Last explicit spaced line

            line = layout.AllPages[0].ContentBlock.Columns[0].Contents[25] as PDFLayoutLine;
            Assert.IsTrue(line.Width < contentW);

            Assert.AreEqual(4, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunSpacer));
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual(line.Width, line.Runs[1].Width);
            Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(line.Runs[3], typeof(PDFLayoutInlineEnd));


            //Back to normal spacing
            line = layout.AllPages[0].ContentBlock.Columns[0].Contents[26] as PDFLayoutLine;
            Assert.IsTrue(line.Width < contentW);

            //BeginInline, BeginText, Chars, EndText, EndInline

            Assert.AreEqual(3, line.Runs.Count);
            Assert.IsInstanceOfType(line.Runs[0], typeof(PDFTextRunBegin));
            begin = line.Runs[0] as PDFTextRunBegin;
            Assert.IsFalse(begin.TextRenderOptions.CharacterSpacing.HasValue);
            Assert.IsFalse(begin.TextRenderOptions.WordSpacing.HasValue);
            Assert.IsInstanceOfType(line.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual(line.Width, line.Runs[1].Width);
            Assert.IsInstanceOfType(line.Runs[2], typeof(PDFTextRunEnd));

        }



        
        


        /// <summary>
        /// A test document with escaped text in inline, and pre-formatted styles to check the layout.
        /// </summary>
        private string HTMLTextContentEscapedSrc = @"
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <title>Bound Document</title>
        <style>
            h2 { font-size: 16pt; font-weight: 400; font-style: italic;}
            .wrapper { border: solid 1px red; margin: 10px; font-size: 12pt;}
            .preformatted{ white-space: pre; }
        </style>
    </head>
    <body style='padding: 10pt' >
        <h2>1. Single Line,not formatted</h2>
        <div id='div1' class='wrapper' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>
        <div class='wrapper' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>
        <h2>2. Multiple Line, not formatted</h2>
        <div id='div2' class='wrapper' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>3. Single Line with spans, not formatted</h2>
        <div id='div3' class='wrapper' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>

        <h2>4. Multiple Line with spans, not formatted</h2>
        <div id='div4' class='wrapper' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;&lt;span&gt;That flows onto multiple lines.&lt;/span&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

        <h2>5. All on one line, pre-formatted</h2>
        <div id='div5' class='wrapper preformatted' >&lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;</div>

        <h2>6. Single separate line pre-formatted</h2>
        <div id='div6' class='wrapper preformatted' >
            &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt;
        </div>

        <h2>7. Multiple Line, pre-formatted</h2>
        <div id='div7' class='wrapper preformatted' >
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
        </div>

        <h2>8. Single Line with spans, pre-formatted</h2>
        <div id='div8' class='wrapper preformatted' >
            <span>Before Content</span> &lt;div data-content='\{{model.items[0].content}}\'&gt;&lt;/div&gt; <span>After Content</span>
        </div>
        <h2>9. Multiple Line with spans, pre-formatted</h2>
        <div id='div9' class='wrapper preformatted' >
            <span>Before Content</span>
            &lt;div class='test' id='Appended' xmlns='' &gt;
                &lt;b&gt;Inner &amp; Content&lt;/b&gt;
            &lt;/div&gt;
            <span>After Content</span>
        </div>

    </body>
</html>";

        /// <summary>
        /// No binding, just an initial test to make sure we are parsing all the content - both preformatted and not preformatted
        /// </summary>
        [TestMethod()]
        public void MultipleSpansWithPreformattedAndEscapedTest()
        {


            var src = HTMLTextContentEscapedSrc;

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("Text_PreFormattedAndEscpaped.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);

                    Assert.IsNotNull(this.layout);

                    var pg = doc.Pages[0] as Page;
                    Assert.IsNotNull(pg);

                    //1. Single line (with whitespace before as part of the text)
                    var wrapper = pg.FindAComponentById("div1") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //Just the inner div
                    var literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div data-content='{{model.items[0].content}}'></div>\n        ", literal.Text);

                    //2. Multile lines (with whitespace before as part of the text)
                    wrapper = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n        ", literal.Text);

                    // 3. Whitespace + Span either side of the encoded string
                    wrapper = pg.FindAComponentById("div3") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    var span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    Assert.AreEqual(" <div data-content='{{model.items[0].content}}'></div> ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                    //4. Multiple lines - whitespace, spans either side long text.
                    wrapper = pg.FindAComponentById("div4") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b><span>That flows onto multiple lines.</span>\n            </div>\n            ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                    //5. Preformatted Single line (without whitespace)
                    wrapper = pg.FindAComponentById("div5") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count); //Just the inner text
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("<div data-content='{{model.items[0].content}}'></div>", literal.Text);

                    //6 Preformatted with the spacing kept.
                    wrapper = pg.FindAComponentById("div6") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div data-content='{{model.items[0].content}}'></div>\n        ", literal.Text);

                    //7 Preformatted multiple lines with spacing kept.
                    wrapper = pg.FindAComponentById("div7") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(1, wrapper.Contents.Count);
                    literal = wrapper.Contents[0] as TextLiteral;
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n        ", literal.Text);

                    //8 Preformatted with whitespace, spans and single line of text
                    wrapper = pg.FindAComponentById("div8") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    //should still be a space before and after
                    Assert.AreEqual(" <div data-content='{{model.items[0].content}}'></div> ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);


                    //9 Preformatted with whitespace, spans and multiple lines of text
                    wrapper = pg.FindAComponentById("div9") as Div;
                    Assert.IsNotNull(wrapper);
                    Assert.AreEqual(5, wrapper.Contents.Count);
                    Assert.IsInstanceOfType(wrapper.Contents[0], typeof(Whitespace));
                    Assert.IsInstanceOfType(wrapper.Contents[1], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[2], typeof(TextLiteral));
                    Assert.IsInstanceOfType(wrapper.Contents[3], typeof(Span));
                    Assert.IsInstanceOfType(wrapper.Contents[4], typeof(Whitespace));

                    Assert.AreEqual("\n            ", ((Whitespace)wrapper.Contents[0]).Text);
                    span = wrapper.Contents[1] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("Before Content", ((TextLiteral)span.Contents[0]).Text);
                    literal = wrapper.Contents[2] as TextLiteral;
                    //should still be the whitespace before, in the middle and after
                    Assert.AreEqual("\n            <div class='test' id='Appended' xmlns='' >\n                <b>Inner & Content</b>\n            </div>\n            ", literal.Text);
                    span = wrapper.Contents[3] as Span;
                    Assert.AreEqual(1, span.Contents.Count);
                    Assert.IsInstanceOfType(span.Contents[0], typeof(TextLiteral));
                    Assert.AreEqual("After Content", ((TextLiteral)span.Contents[0]).Text);
                    Assert.AreEqual("\n        ", ((Whitespace)wrapper.Contents[4]).Text);

                }
            }
        }



        private void SaveAsPDF(Document doc, string fileName)
        {
            using(var stream = DocStreams.GetOutputStream(fileName + ".pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }

        private void SaveAsPDFAndText(Document doc, string fileName)
        {
            using(var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);

                using (var stream = DocStreams.GetOutputStream(fileName + ".pdf"))
                {
                    ms.Position = 0;
                    ms.CopyTo(stream);
                }

                using (var stream = DocStreams.GetOutputStream(fileName + ".text"))
                {
                    ms.Position = 0;
                    ms.CopyTo(stream);
                }
            }
        }
    }
}
