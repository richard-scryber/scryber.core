using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Drawing;
using System.IO;
using Scryber.PDF.Resources;

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

        private void AssertAreApproxEqual(double one, double two, string message)
        {
            int precision = 5;
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

            pg.Margins = new Thickness(10);
            pg.BackgroundColor = new Color(240, 240, 240);
            pg.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines " +
                "in the page with a default line height so that we can check the leading of default lines " +
                "as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteral");
            

            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0];


            var em = 24.0;  //Point size of font
            var lead = 24.0 * 1.2; //default leading

            var rsrc = doc.SharedResources[0] as PDFFontResource;

            Assert.IsNotNull(rsrc, "The font resource should be the one and only shared resource");
            Assert.IsNotNull(rsrc.Definition, "The font definition should not be null");

            var metrics = rsrc.Definition.GetFontMetrics(em);

            var line = region.Contents[0] as PDFLayoutLine;
            AssertAreApproxEqual(lead, line.Height.PointsValue, "Line 0 was not the correct height");
            

            //Check the heights of the continuation lines

            for (var i = 1; i < 4; i++)
            {
                line = region.Contents[i] as PDFLayoutLine;
                AssertAreApproxEqual(lead, line.Height.PointsValue, "Line " + i + " was not the correct height");
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
            Assert.IsNotNull(font, "This test will fail as the  font is not present, or could not be loaded from the System fonts");
               
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
                AssertAreApproxEqual(leading.PointsValue, line.Height.PointsValue, "Line " + i + " did not use the explicit leading");
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
            pg.FontSize = size;
            pg.FontFamily = new FontSelector("Serif");

            doc.Pages.Add(pg);
            pg.Contents.Add(new TextLiteral("This is a text run that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page. " +
                "Repeated text that should flow over more than two lines in the page with a default line height so that we can check the leading of default lines as they flow down the page"));


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_SingleLiteralWithLeadingAt12ptOptima");


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
            var offsetY = pgHeight.PointsValue - (lineCount * lead);

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
            var offsetY = half;

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
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                var twidth = text.Width;

                var offset = pgContentWidth - twidth;

                if(i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    AssertAreApproxEqual(offset.PointsValue, start.TotalBounds.X.PointsValue, "First line inset should be " + offset);
                }
                else
                {
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
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


            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
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
                Assert.AreEqual(3, line.Runs.Count);
                var text = line.Runs[1] as PDFTextRunCharacter;
                var twidth = text.Width;

                var offset = (pgContentWidth - twidth) / 2.0;

                if (i == 0)
                {
                    var start = line.Runs[0] as PDFTextRunBegin;
                    AssertAreApproxEqual(offset.PointsValue, start.TotalBounds.X.PointsValue, "First line inset should be " + offset);
                }
                else
                {
                    var space = line.Runs[0] as PDFTextRunSpacer;
                    AssertAreApproxEqual(offset.PointsValue, space.Width.PointsValue, "Line " + i + " spacer should be " + offset);
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
        public void ALongTextBlock()
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
            pg.Contents.Add(new TextLiteral(content));
            pg.TextDecoration = Text.TextDecoration.Underline;

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.LayoutComplete += Doc_LayoutComplete;
            SaveAsPDF(doc, "Text_LongLiteral");


            Assert.IsNotNull(layout, "The layout was not saved from the event");
            var region = layout.AllPages[0].ContentBlock.Columns[0] as PDFLayoutRegion;
            var first = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            var second = layout.AllPages[0].ContentBlock.Columns[0].Contents[1] as PDFLayoutLine;

            Assert.Inconclusive("Not tested");
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
        public void FixedLeadingSpans()
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
            span.TextLeading = 50;
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
            span.TextLeading = 30;
            span.Contents.Add(new TextLiteral("of default lines as they flow down the page and onto new lines"));

            pg.Contents.Add(span);

            span = new Span();
            span.FontItalic = true;
            span.Contents.Add(new TextLiteral(" with more content"));

            pg.Contents.Add(span);

            doc.RenderOptions.Compression = OutputCompressionType.None;
            doc.AppendTraceLog = true;
            doc.LayoutComplete += Doc_LayoutComplete;

            SaveAsPDF(doc, "Text_LiteralsInBoldAndItalicWithLeading");


            Assert.IsNotNull(layout, "The layout was not saved from the event");

            // line 1

            PDFLayoutLine first = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.AreEqual(50, first.Height);
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
            Assert.AreEqual(30, third.Height);
            Assert.AreEqual(8, third.Runs.Count);

            Assert.IsInstanceOfType(third.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (third.Runs[0] as PDFTextRunSpacer).Width);
            Assert.IsInstanceOfType(third.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("leading ", ((third.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(third.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(third.Runs[3], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(third.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(700, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);
            Assert.IsInstanceOfType(third.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(third.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual("of default lines as they flow down the page and onto", ((third.Runs[6]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(third.Runs[7], typeof(PDFTextRunNewLine));
            //Still has the explicit line height from the span, onto the next line
            Assert.AreEqual(30, (third.Runs[7] as PDFTextRunNewLine).NewLineOffset.Height);


            //Fourth line

            PDFLayoutLine fourth = layout.AllPages[0].ContentBlock.Columns[0].Contents[3] as PDFLayoutLine;
            Assert.AreEqual(9, fourth.Runs.Count);

            Assert.IsInstanceOfType(fourth.Runs[0], typeof(PDFTextRunSpacer));
            Assert.AreEqual(0, (fourth.Runs[0] as PDFTextRunSpacer).Width);
            Assert.IsInstanceOfType(fourth.Runs[1], typeof(PDFTextRunCharacter));
            Assert.AreEqual("new lines", ((fourth.Runs[1]) as PDFTextRunCharacter).Characters);
            Assert.IsInstanceOfType(fourth.Runs[2], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(fourth.Runs[3], typeof(PDFLayoutInlineEnd));

            Assert.IsInstanceOfType(fourth.Runs[4], typeof(PDFLayoutInlineBegin));
            Assert.AreEqual(400, (fourth.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontWeight);
            Assert.AreEqual(FontStyle.Italic, (third.Runs[4] as PDFLayoutInlineBegin).FullStyle.Font.FontFaceStyle);
            Assert.IsInstanceOfType(fourth.Runs[5], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(fourth.Runs[6], typeof(PDFTextRunCharacter));
            Assert.AreEqual(" with more content", ((fourth.Runs[6]) as PDFTextRunCharacter).Characters);

            Assert.IsInstanceOfType(fourth.Runs[7], typeof(PDFTextRunEnd));
            Assert.IsInstanceOfType(fourth.Runs[8], typeof(PDFLayoutInlineEnd));

        }


        [TestMethod]
        public void LiteralWithExplicitWordAndCharSpacing()
        {
            Assert.Inconclusive("No Test");
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
