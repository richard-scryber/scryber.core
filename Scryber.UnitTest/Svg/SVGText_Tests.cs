using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;
using System.IO;
using System.Linq;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.Svg;
using Path = Scryber.Components.Path;
using TransformMatrixOperation = Scryber.Styles.TransformMatrixOperation;
using TransformOperation = Scryber.Drawing.TransformOperation;

namespace Scryber.Core.UnitTests.Svg
{
    [TestClass()]
    public class SVGText_Tests
    {

        #region public TestContext TestContext

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        
        [TestMethod]
        public void SVGTextPosition()
        {
            var svgString = @"
            <svg width=""500"" height=""300"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 500 300"" style=""background-color: silver;"">
<text id=""positionedText"" dominant-baseline=""auto"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""50"" y=""50"" fill=""#6E7079"">At 150 + 150</text>
</svg>";
            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 50;
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 50;

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextPositionAndTranslate.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Making changes to the layout - need to come back and adjust the test");
            
            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            var lblock = lpg.ContentBlock;
            Assert.IsNotNull(lblock);
            Assert.AreEqual(1, lblock.Columns.Length);
            var lreg = lblock.Columns[0];
            Assert.IsNotNull(lreg);
            Assert.AreEqual(1, lreg.Contents.Count);

            //Canvas inpage block
            var litem = lreg.Contents[0];
            Assert.IsNotNull(litem);
            Assert.IsInstanceOfType(litem, typeof(PDFLayoutBlock));
            var lcanv = litem as PDFLayoutBlock;
            Assert.IsNotNull(lcanv);
            Assert.AreSame(lcanv.Owner, svg);
            Assert.AreEqual(2, lcanv.PositionedRegions.Count);

            //Canvas positioned region
            var lcanvReg = lcanv.PositionedRegions[0];
            Assert.IsNotNull(lcanvReg);
            Assert.AreEqual(498.0, lcanv.Size.Width.PointsValue);
            Assert.AreEqual(305.0, lcanv.Size.Height.PointsValue);

            //Positioned run on the first line in the canvas for the text
            Assert.AreEqual(1, lcanvReg.Contents.Count);
            var lline = lcanvReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(lline);
            Assert.AreEqual(1, lline.Runs.Count);
            var lposRun = lline.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(lposRun);

            //Positioned region in the canvas for the text block
            Assert.AreSame(lposRun.Region, lcanv.PositionedRegions[1]);
            var ltxtPosReg = lcanv.PositionedRegions[1];

            //Positioned region contains a block with 1 region with 1 line with 3 runs - text begin, chars and end
            var ltxtBlock = ltxtPosReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ltxtBlock);
            var ltxtReg = ltxtBlock.Columns[0];
            Assert.IsNotNull(ltxtReg);
            Assert.AreEqual(1, ltxtReg.Contents.Count);
            var ltxtLine = ltxtReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(ltxtLine);
            Assert.AreEqual(3, ltxtLine.Runs.Count);
            Assert.IsInstanceOfType(ltxtLine.Runs[0], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(ltxtLine.Runs[1], typeof(PDFTextRunCharacter));
            Assert.IsInstanceOfType(ltxtLine.Runs[2], typeof(PDFTextRunEnd));

            //Check transform of the layout text block
            var lineH = 10.8; //baseline offset
            var offsetX = 100.0; //translate x
            var offsetY = -100.0 + lineH; //negative (translate y - the baseline offset)

            Assert.AreEqual(offsetX, Math.Round(ltxtBlock.TransformedOffset.X.PointsValue, 1), "X offsets for the translate do not match");
            Assert.AreEqual(offsetY, Math.Round(ltxtBlock.TransformedOffset.Y.PointsValue, 1), "Y offsets for the translate do not match");

            //Check the position of the layout text block
            var boundsX = 50.0; // explicit x
            var boundsY = 52.1; // explicit y + descender 

            Assert.AreEqual(boundsX, Math.Round(ltxtBlock.TotalBounds.X.PointsValue, 1));
            Assert.AreEqual(boundsY, Math.Round(ltxtBlock.TotalBounds.Y.PointsValue, 1));


        }
        
        [TestMethod]
        public void SVGTextPositionAndTranslate()
        {
            var svgString = @"
            <svg width=""500"" height=""300"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 500 300"" style=""background-color: silver;"">
<text id=""positionedText"" dominant-baseline=""auto"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""50"" y=""50"" transform=""translate(100 100)"" fill=""#6E7079"">At 150 + 150</text>
</svg>";
            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 50;
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 50;

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextPositionAndTranslate.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Making changes to the layout - need to come back and adjust the test");
            
            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            var lblock = lpg.ContentBlock;
            Assert.IsNotNull(lblock);
            Assert.AreEqual(1, lblock.Columns.Length);
            var lreg = lblock.Columns[0];
            Assert.IsNotNull(lreg);
            Assert.AreEqual(1, lreg.Contents.Count);

            //Canvas inpage block
            var litem = lreg.Contents[0];
            Assert.IsNotNull(litem);
            Assert.IsInstanceOfType(litem, typeof(PDFLayoutBlock));
            var lcanv = litem as PDFLayoutBlock;
            Assert.IsNotNull(lcanv);
            Assert.AreSame(lcanv.Owner, svg);
            Assert.AreEqual(2, lcanv.PositionedRegions.Count);

            //Canvas positioned region
            var lcanvReg = lcanv.PositionedRegions[0];
            Assert.IsNotNull(lcanvReg);
            Assert.AreEqual(498.0, lcanv.Size.Width.PointsValue);
            Assert.AreEqual(305.0, lcanv.Size.Height.PointsValue);

            //Positioned run on the first line in the canvas for the text
            Assert.AreEqual(1, lcanvReg.Contents.Count);
            var lline = lcanvReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(lline);
            Assert.AreEqual(1, lline.Runs.Count);
            var lposRun = lline.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(lposRun);

            //Positioned region in the canvas for the text block
            Assert.AreSame(lposRun.Region, lcanv.PositionedRegions[1]);
            var ltxtPosReg = lcanv.PositionedRegions[1];

            //Positioned region contains a block with 1 region with 1 line with 3 runs - text begin, chars and end
            var ltxtBlock = ltxtPosReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ltxtBlock);
            var ltxtReg = ltxtBlock.Columns[0];
            Assert.IsNotNull(ltxtReg);
            Assert.AreEqual(1, ltxtReg.Contents.Count);
            var ltxtLine = ltxtReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(ltxtLine);
            Assert.AreEqual(3, ltxtLine.Runs.Count);
            Assert.IsInstanceOfType(ltxtLine.Runs[0], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(ltxtLine.Runs[1], typeof(PDFTextRunCharacter));
            Assert.IsInstanceOfType(ltxtLine.Runs[2], typeof(PDFTextRunEnd));

            //Check transform of the layout text block
            var lineH = 10.8; //baseline offset
            var offsetX = 100.0; //translate x
            var offsetY = -100.0 + lineH; //negative (translate y - the baseline offset)

            Assert.AreEqual(offsetX, Math.Round(ltxtBlock.TransformedOffset.X.PointsValue, 1), "X offsets for the translate do not match");
            Assert.AreEqual(offsetY, Math.Round(ltxtBlock.TransformedOffset.Y.PointsValue, 1), "Y offsets for the translate do not match");

            //Check the position of the layout text block
            var boundsX = 50.0; // explicit x
            var boundsY = 52.1; // explicit y + descender 

            Assert.AreEqual(boundsX, Math.Round(ltxtBlock.TotalBounds.X.PointsValue, 1));
            Assert.AreEqual(boundsY, Math.Round(ltxtBlock.TotalBounds.Y.PointsValue, 1));


        }

        [TestMethod]
        public void SVGTextAnchorOptions()
        {
            var svgString = @"
            <svg width=""500"" height=""400""
                xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full""
                style=""position : absolute; left: 30; top: 60; user-select:none; border: solid 1px green;"">
                <path d=""M 100 50 L400 50 M 100 80 L 400 80 M 100 110 L 400 110 M 100 140 L 400 140"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <path d=""M 250 0 L250 200"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <rect x=""10"" y=""10"" width=""10"" height=""10"" />
                <g>
                  <text id=""textStart"" x=""250"" y=""50"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Start</text>
                  <text id=""textMiddle"" x=""250"" y=""80"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Middle</text>
                  <text id=""textEnd"" x=""250"" y=""110"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">End</text>
                  <text id=""textCSSMiddle"" x=""250"" y=""140"" style=""font-size:12px;font-family:sans-serif;text-anchor:middle"" fill=""black"">CSS Middle</text>
                </g>
            </svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);

            // Assert.AreEqual(7, svg.Contents.Count);
            // var group = svg.Contents[5] as SVGGroup;
            // Assert.IsNotNull(group);
            //
            // Assert.AreEqual(4 * 2 + 1, group.Contents.Count);
            //
            // var txt = group.Contents[1] as SVGText;
            // Assert.IsNotNull(txt);
            // Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            //
            // txt = group.Contents[3] as SVGText;
            // Assert.IsNotNull(txt);
            // Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);
            //
            // txt = group.Contents[5] as SVGText;
            // Assert.IsNotNull(txt);
            // Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            //
            // //Set via CSS
            // txt = group.Contents[7] as SVGText;
            // Assert.IsNotNull(txt);
            // Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

            svg.OverflowAction = OverflowAction.Clip;
            

            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridMajorCount = 5;
            pg.Style.OverlayGrid.GridColor = StandardColors.Aqua;
            
            doc.Pages.Add(pg);
            pg.Padding = new Thickness(0, 30, 30, 0);
            pg.Contents.Add("Before the SVG");
            pg.Contents.Add(svg);
            pg.Contents.Add(" After the SVG");
            
            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextAnchor.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }


        [TestMethod]
        public void SVGTextDominantBaselineOptions()
        {
            var svgString = @"<svg
                              width=""400""
                              height=""500""
                              viewBox=""0 0 400 500""
                              xmlns=""http://www.w3.org/2000/svg"">
<rect x=""50"" y=""50"" width=""300"" height=""400"" fill=""lime"" fill-opacity=""0.5"" stroke='black' stroke-width='2pt' />
<rect x=""100"" y=""100"" width=""200"" height=""300"" fill=""navy"" fill-opacity=""0.5"" stroke='white' stroke-width='2pt' />
                              <!-- Materialization of anchors -->
                              
                              <path
                                d=""M60,20 L60,470
                                       M30,20 L400,20
                                       M30,70 L400,70
                                       M30,120 L400,120
                                       M30,170 L400,170
                                       M30,220 L400,220
                                       M30,270 L400,270
	                                   M30,320 L400,320
                                       M30,370 L400,370
                                       M30,420 L400,420
                                       M30,470 L400,470
	                               ""
                                stroke=""grey"" />

                              <!-- Anchors in action -->
                              <text dominant-baseline=""auto"" x=""70"" y=""20"">auto</text>
                              <text dominant-baseline=""middle"" x=""70"" y=""70"">middle</text>
                              <text dominant-baseline=""central"" x=""70"" y=""120"">central</text>
                              <text dominant-baseline=""hanging"" x=""70"" y=""170"">hanging</text>
                              <text dominant-baseline=""mathematical"" x=""70"" y=""220"">mathematical</text>
                              <text dominant-baseline=""text-top"" x=""70"" y=""270"" >text-top</text>
                              <text dominant-baseline=""ideographic"" x=""70"" y=""320"">ideographic</text>
                              <text dominant-baseline=""alphabetic"" x=""70"" y=""370"">alphabetic</text>
                              <text dominant-baseline=""text-after-edge"" x=""70"" y=""420"">text-after-edge</text>
                              <text dominant-baseline=""text-before-edge"" x=""70"" y=""470"" >text-before-edge</text> 

                              <!-- Materialization of anchors -->
                              <circle cx=""60"" cy=""20"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""70"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""120"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""170"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""220"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""270"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""320"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""370"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""420"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""470"" r=""3"" fill=""red"" /> 
                              <style>
                                <![CDATA[
                                  body{ padding: 20px}
                                  text {
                                    font: bold 30px Helvetica, Arial, sans-serif;
                                  }
                                  ]]>
                              </style>

                              
                            </svg>";

            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            Assert.IsNotNull(svg);
            
            Assert.AreEqual(52, svg.Contents.Count);

            var txt = svg.Contents[9] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Auto, txt.DominantBaseline);

            txt = svg.Contents[11] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Middle, txt.DominantBaseline);

            txt = svg.Contents[13] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[15] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Hanging, txt.DominantBaseline);

            txt = svg.Contents[17] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Mathematical, txt.DominantBaseline);

            txt = svg.Contents[19] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Top, txt.DominantBaseline);

            txt = svg.Contents[21] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Ideographic, txt.DominantBaseline);

            txt = svg.Contents[23] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Alphabetic, txt.DominantBaseline);

            txt = svg.Contents[25] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_After_Edge, txt.DominantBaseline);

            txt = svg.Contents[27] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Before_Edge, txt.DominantBaseline);


            using var doc = new Document();
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            pg.PaperSize = PaperSize.Custom;
            pg.Width = 500;
            pg.Height = 600;
            doc.Pages.Add(pg);
            //pg.Contents.Add(new TextLiteral("Above the SVG"));
            
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridOpacity = 0.5;
            pg.Style.OverlayGrid.GridSpacing = 50;
            pg.Style.OverlayGrid.GridColor = StandardColors.Gray;
            svg.BorderColor = StandardColors.Aqua;
            pg.FontFamily = new FontSelector("serif");
            pg.Margins = new Thickness(20, 0, 0, 50);
            pg.FontSize = 10;
            pg.FontWeight = 700;
            
            pg.Contents.Add(svg);
            
            //pg.Contents.Add(new TextLiteral("Below the SVG"));
            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextDominantBaseline.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }



        [TestMethod]
        public void SVGTextAnchorChart()
        {
            var svgString = @"
            <svg width=""951"" height=""752"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 951 752"">
<rect width=""951"" height=""752"" x=""0"" y=""0"" id=""0"" fill=""none""></rect>
<polyline points=""214.9 112.4 227.9 104.9 242.9 104.9"" fill=""none"" stroke=""#5470c6""></polyline>
<polyline points=""173.3 221.3 177.9 235.6 192.9 235.6"" fill=""none"" stroke=""#91cc75""></polyline>
<polyline points=""87 190.7 74.4 198.8 59.4 198.8"" fill=""none"" stroke=""#fac858""></polyline>
<polyline points=""83.8 114.8 70.5 107.8 55.5 107.8"" fill=""none"" stroke=""#ee6666""></polyline>
<polyline points=""127.9 78.3 123.4 64 108.4 64"" fill=""none"" stroke=""#73c0de""></polyline>
<path d=""M150 75A75 75 0 0 1 215.0266 187.3702L150 150Z"" fill=""rgb(84,112,198)""></path>
<path d=""M215.0266 187.3702A75 75 0 0 1 119.5358 218.5342L150 150Z"" fill=""#91cc75""></path>
<path d=""M119.5358 218.5342A75 75 0 0 1 75.0011 149.5882L150 150Z"" fill=""rgb(250,200,88)""></path>
<path d=""M75.0011 149.5882A75 75 0 0 1 107.7157 88.0562L150 150Z"" fill=""#ee6666""></path>
<path d=""M107.7157 88.0562A75 75 0 0 1 150 75L150 150Z"" fill=""#73c0de""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(247.8973 104.9222)"" fill=""black"">Sear...</text>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(197.9226 235.5589)"" fill=""black"">Direct</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(54.4 198.8328)"" fill=""black"">Email</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(50.5333 107.7502)"" fill=""black"">Unio...</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" transform=""translate(103.4475 64.006)"" fill=""black"">Video Ads</text>
</svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual((16 * 2) + 1, svg.Contents.Count); //whitespace is significant

            var txt = svg.Contents[22 + 1] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 3] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 5] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 7] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 9] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);


            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextAnchorChart.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }


        
        
       

    }

}



