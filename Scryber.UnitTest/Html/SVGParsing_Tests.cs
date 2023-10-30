using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;
using System.IO;
using Scryber.PDF.Layout;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class SVGParsing_Tests
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
        public void SVGSimple()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGSimple.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.SaveAsPDF(stream);

                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    Assert.AreEqual(3, section.Contents.Count);
                    Assert.IsInstanceOfType(section.Contents[0], typeof(HTMLParagraph));
                    Assert.IsInstanceOfType(section.Contents[2], typeof(HTMLParagraph));
 
                    var canvas = section.Contents[1] as Canvas;
                    Assert.IsNotNull(canvas);
                    Assert.AreEqual(300, canvas.Style.Size.Width.PointsValue, "The width of the canvas was not set");
                    Assert.AreEqual(200, canvas.Style.Size.Height.PointsValue, "The height of the canvas was not set");
                    Assert.AreEqual("canvas", canvas.StyleClass, "The style class of the canvas was not set");

                    Assert.AreEqual(1, canvas.Contents.Count);
                    var rect = canvas.Contents[0] as Svg.Components.SVGRect;
                    Assert.IsNotNull(rect, "The inner rectangle was not found");
                    Assert.AreEqual("box", rect.StyleClass, "The rect style class was not correct");
                    Assert.AreEqual(100, rect.Style.Position.X.PointsValue, "The X position of the rect was not correct");
                    Assert.AreEqual(10, rect.Style.Position.Y.PointsValue, "The Y position of the rect was not correct");
                    Assert.AreEqual(60, rect.Style.Size.Width.PointsValue, "The width of the rect was not correct");
                    Assert.AreEqual(70, rect.Style.Size.Height.PointsValue, "The height of the rect was not correct");
                    Assert.AreEqual(StandardColors.Green, rect.Style.Stroke.Color, "The stroke color of the rect was not set");
                    Assert.AreEqual(2, rect.Style.Stroke.Width.PointsValue, "The width of the stroke was not correct");
                    Assert.AreEqual(StandardColors.Yellow, rect.Style.Fill.Color, "The fill color of the rect was not correct");
                    Assert.AreEqual(20, rect.CornerRadiusX, "The x corner radius was not correct");
                    Assert.AreEqual(10, rect.CornerRadiusY, "The Y corner radius was not correct");
                }
            }
        }


        [TestMethod]
        public void SVGComponents()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGComponents.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGComponents.pdf"))
                {
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var clock = section.Contents[1] as Canvas;
                    
                    Assert.IsNotNull(clock);
                    Assert.AreEqual("ClockIcon", clock.ID);
                    Assert.AreEqual(1, clock.Contents.Count);
                    Assert.AreEqual(20, clock.Width);
                    Assert.AreEqual(20, clock.Height);

                    var clockPath = clock.Contents[0] as SVGPath;
                    Assert.IsNotNull(clockPath);
                    Assert.AreEqual(StandardColors.Blue, clockPath.FillColor);
                }
            }
        }

        [TestMethod]
        public void SVGTransform()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGTransform.html");

            using (var doc = Document.ParseDocument(path))
            {
                Div div;
                if(doc.TryFindAComponentById("mydiv", out div))
                {
                    var angle = 90.0;
                    angle = (Math.PI / 180.0) * angle;
                    div.Style.SetValue(StyleKeys.TransformOperationKey, new TransformOperation(TransformType.Rotate, (float)angle, TransformOperation.NotSetValue()));
                }

                using (var stream = DocStreams.GetOutputStream("Transform.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        [TestMethod]
        public void SVGInsert()
        {
            var doc = new Document();
            var pg1 = new Page();
            doc.Pages.Add(pg1);

            var svgString = @"
            <svg xmlns=""http://www.w3.org/2000/svg"" style=""border:solid 1px black"" >
                <rect x=""0pt"" y=""0pt"" width=""100pt"" height=""80pt"" fill=""lime"" ></rect>
                <g id=""eye"" stroke=""black"" stroke-width=""2pt"" >
                    <ellipse cx=""50pt"" cy=""40pt"" rx=""40pt"" ry=""20pt"" fill=""white""></ellipse>
                    <circle cx=""50pt"" cy=""40pt"" r=""20pt"" fill=""#66F""></circle>
                    <circle cx=""50pt"" cy=""40pt"" r=""10pt"" fill=""black""></circle>
                    <line x1=""10"" x2=""90"" y1=""40"" y2=""40"" />
                    <line x1=""50"" x2=""50"" y1=""20"" y2=""60"" />
                </g>
            </svg>";



            //1. Explicit parsing of the content and adding to the page.
            //Allows handling of any parsing errors independently of the document flow.
            pg1.Contents.Add("The parsed component will be explicitly added after this.");
            var icomp = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            pg1.Contents.Add(icomp as Component);
            pg1.Contents.Add("This is after the parsed component");

            //2. Appending the data content to the page
            //Simply setting the DataContent to a string and the action as append.
            var pg2 = new Page();
            doc.Pages.Add(pg2);

            pg2.Contents.Add("The SVG content will be appended to the page.");
            pg2.DataContent = svgString;
            pg2.DataContentAction = DataContentAction.Append;
            pg2.Contents.Add(" Will still appear before the data content.");

            
            var pg3 = new Page();
            doc.Pages.Add(pg3);

            //3. Adding the content to an inner div (with some margins)
            //Gives flexibility in positioning and document flow.
            var div = new Div();
            div.DataContent = svgString;

            div.Margins = new Thickness(10);
            pg3.Contents.Add("Before the div with the inner svg");
            pg3.Contents.Add(div);
            pg3.Contents.Add("After the div with the inner svg");

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGInserted.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }


            if (null == layout)
                throw new InvalidOperationException("Layout was not set");

            Assert.AreEqual(3, pg1.Contents.Count);
            var svg = pg1.Contents[1] as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(3, pg2.Contents.Count);
            svg = pg2.Contents[2] as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(3, pg3.Contents.Count);
            div = pg3.Contents[1] as Div;
            Assert.IsNotNull(div);
            Assert.AreEqual(1, div.Contents.Count);
            svg = div.Contents[0] as SVGCanvas;
            Assert.IsNotNull(svg);
        }
        
        /// <summary>
        /// Test case for Issue #107
        /// </summary>
        [TestMethod]
        public void SVGToComponentWithInvariantUnits()
        {
            var svgString = @"
            <svg width=""1000"" height=""400"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""position:absolute;left:0;top:0;user-select:none"">
              <rect width=""1000"" height=""400"" x=""0"" y=""0"" id=""0"" fill=""red"" fill-opacity=""1""></rect>
              <g>
                <path d=""M503.1463 120.0619L503.7363 105.0735L745 105.0735"" fill=""none"" stroke=""#5470c6""></path>
                <path d=""M504.3059 279.884L505.1132 294.8623L649 294.8623"" fill=""none"" stroke=""#91cc75""></path>
                <path d=""M488.3618 120.8511L417.7603 131.2324L291 131.2324"" fill=""none"" stroke=""#fac858""></path>
                <path d=""M495.7775 120.1115L494.9857 105.1324L315 105.1324"" fill=""none"" stroke=""#ee6666""></path>
                <path d=""M500 120A80 80 0 0 1 506.2878 120.2475L500 200Z"" fill=""#5470c6"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M506.2878 120.2475A80 80 0 1 1 485.1759 121.3855L500 200Z"" fill=""#91cc75"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M485.1759 121.3855A80 80 0 0 1 491.5667 120.4457L500 200Z"" fill=""#fac858"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M491.5667 120.4457A80 80 0 0 1 500 120L500 200Z"" fill=""#ee6666"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(750 294.8623)"" fill=""black"">Airplane</text>
                <text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(250 131.2324)"" fill=""black"">Car</text>
                <text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(250 105.1324)"" fill=""black"">Train</text>
              </g>
            </svg>";


            SVGCanvas svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            var grp = svg.Contents[1] as SVGGroup;
            Assert.IsNotNull(grp);

            var path = grp.Contents[0] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            Assert.AreEqual(1, path.PathData.Paths.Count);

            var opPath = path.PathData.Paths[0];
            Assert.AreEqual(3, opPath.Operations.Count);

            var opMove = opPath.Operations[0] as PathMoveData;
            Assert.IsNotNull(opMove);
            Assert.AreEqual(503.1463, opMove.MoveTo.X.PointsValue);
            Assert.AreEqual(120.0619, opMove.MoveTo.Y.PointsValue);

            var opLineTo = opPath.Operations[1] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(503.7363, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            opLineTo = opPath.Operations[2] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(745, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            var origCult = System.Threading.Thread.CurrentThread.CurrentCulture;
            var origUICult = System.Threading.Thread.CurrentThread.CurrentUICulture;

            //
            //Switch to the german culture
            //

            var german = System.Globalization.CultureInfo.CreateSpecificCulture("de-DE");
            System.Threading.Thread.CurrentThread.CurrentCulture = german;
            System.Threading.Thread.CurrentThread.CurrentUICulture = german;


            svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            grp = svg.Contents[1] as SVGGroup;
            Assert.IsNotNull(grp);

            path = grp.Contents[0] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            Assert.AreEqual(1, path.PathData.Paths.Count);

            opPath = path.PathData.Paths[0];
            Assert.AreEqual(3, opPath.Operations.Count);

            opMove = opPath.Operations[0] as PathMoveData;
            Assert.IsNotNull(opMove);
            Assert.AreEqual(503.1463, opMove.MoveTo.X.PointsValue);
            Assert.AreEqual(120.0619, opMove.MoveTo.Y.PointsValue);

            opLineTo = opPath.Operations[1] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(503.7363, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            opLineTo = opPath.Operations[2] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(745, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);


            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.PaperOrientation = PaperOrientation.Landscape;
            pg.Contents.Add(svg);

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGInvariant.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var first = layout.AllPages[0];
            Assert.AreEqual(1, first.ContentBlock.Columns.Length);
            var reg = first.ContentBlock.Columns[0];
            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var run = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(run);

            var posReg = run.Region;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(1, posReg.Contents.Count);
            Assert.AreEqual(svg, posReg.Owner);

            System.Threading.Thread.CurrentThread.CurrentUICulture = origUICult;
            System.Threading.Thread.CurrentThread.CurrentCulture = origCult;
        }


        [TestMethod]
        public void SVGWithOverflowClipping()
        {
            var svgString = "";

            try
            {
                var path = System.Environment.CurrentDirectory;
                path = System.IO.Path.Combine(path, "../../../Content/SVG/Chart.svg");
                svgString = System.IO.File.ReadAllText(path);

                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                var svg = (SVGCanvas)component;

                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));

                using var doc = new Document()
                {
                    AppendTraceLog = false
                };
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Diagnostic);
                doc.RenderOptions.Compression = OutputCompressionType.None;

                var pg = new Page();
                doc.Pages.Add(pg);
                pg.Contents.Add(svg);
                svg.OverflowAction = OverflowAction.Clip;

                foreach (VisualComponent item in svg.Contents)
                {
                    item.Style.Overflow.Action = OverflowAction.Clip;
                }
                var gStyle = new StyleDefn("g");
                gStyle.Overflow.Action = OverflowAction.Clip;
                
                doc.Styles.Add(gStyle);
                

                PDF.Layout.PDFLayoutDocument layout = null;
                //Output the document (including databinding the data content)
                using (var stream = DocStreams.GetOutputStream("SVGViewboxOverflow.pdf"))
                {
                    doc.LayoutComplete += (sender, args) =>
                    {
                        layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                    };
                    doc.SaveAsPDF(stream);
                }
            }
            catch(Exception ex)
            {
                Assert.Fail("Svg image has not been parsed : " + ex);
            }
        }

        [TestMethod]
        public void SVGTextAnchorOptions()
        {
            var svgString = @"
            <svg width=""500"" height=""400""
                xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full""
                style=""position:absolute;left:10;top:10;user-select:none; border: solid 1px navy;"">
                <path d=""M 100 50 L400 50 M 100 80 L 400 80 M 100 110 L 400 110 M 100 140 L 400 140"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <path d=""M 250 0 L250 200"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <g>
              
                  <text x=""250"" y=""50"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Start</text>
                  <text x=""250"" y=""80"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Middle</text>
                  <text x=""250"" y=""110"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">End</text>
                  <text x=""250"" y=""140"" style=""font-size:12px;font-family:sans-serif;text-anchor:middle"" fill=""black"">CSS Middle</text>
                </g>
            </svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(3, svg.Contents.Count);
            var group = svg.Contents[2] as SVGGroup;
            Assert.IsNotNull(group);

            Assert.AreEqual(4, group.Contents.Count);

            var txt = group.Contents[0] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);

            txt = group.Contents[1] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

            txt = group.Contents[2] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);

            //Set via CSS
            txt = group.Contents[3] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

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
                              height=""550""
                              viewBox=""0 0 400 550""
                              xmlns=""http://www.w3.org/2000/svg"">
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
                                    font: bold 30px Verdana, Helvetica, Arial, sans-serif;
                                  }
                                  ]]>
                              </style>
                            </svg>";

            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual(21, svg.Contents.Count);

            var txt = svg.Contents[1] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Auto, txt.DominantBaseline);

            txt = svg.Contents[2] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Middle, txt.DominantBaseline);

            txt = svg.Contents[3] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[4] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Hanging, txt.DominantBaseline);

            txt = svg.Contents[5] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Mathematical, txt.DominantBaseline);

            txt = svg.Contents[6] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Top, txt.DominantBaseline);

            txt = svg.Contents[7] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Ideographic, txt.DominantBaseline);

            txt = svg.Contents[8] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Alphabetic, txt.DominantBaseline);

            txt = svg.Contents[9] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_After_Edge, txt.DominantBaseline);

            txt = svg.Contents[10] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Before_Edge, txt.DominantBaseline);


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);
            //pg.FontFamily = new FontSelector("serif");
            pg.Margins = new Thickness(20, 0, 0, 0);
            pg.FontSize = 24;
            pg.FontWeight = 700;

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
            Assert.AreEqual(16, svg.Contents.Count);

            var txt = svg.Contents[11] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[12] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[13] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[14] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[15] as SVGText;
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
            pg.Margins = new Thickness(20, 0, 0, 0);
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



