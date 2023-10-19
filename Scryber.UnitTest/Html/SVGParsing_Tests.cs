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

    }



}
