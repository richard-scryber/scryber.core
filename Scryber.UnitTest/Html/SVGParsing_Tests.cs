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

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGSimple.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGSimple.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.SaveAsPDF(stream);

                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    Assert.AreEqual(7, section.Contents.Count);
                    Assert.IsInstanceOfType(section.Contents[1], typeof(HTMLParagraph));
                    Assert.IsInstanceOfType(section.Contents[3], typeof(Canvas));
                    Assert.IsInstanceOfType(section.Contents[5], typeof(HTMLParagraph));
 
                    var canvas = section.Contents[3] as Canvas;
                    Assert.IsNotNull(canvas);
                    Assert.AreEqual(300, canvas.Style.Size.Width.PointsValue, "The width of the canvas was not set");
                    Assert.AreEqual(200, canvas.Style.Size.Height.PointsValue, "The height of the canvas was not set");
                    Assert.AreEqual("canvas", canvas.StyleClass, "The style class of the canvas was not set");

                    Assert.AreEqual(3, canvas.Contents.Count);
                    var rect = canvas.Contents[1] as SVGRect;
                    Assert.IsNotNull(rect, "The inner rectangle was not found");
                    Assert.AreEqual("box", rect.StyleClass, "The rect style class was not correct");
                    Assert.AreEqual(100, rect.Style.GetValue(StyleKeys.SVGGeometryXKey, 0), "The X position of the rect was not correct");
                    Assert.AreEqual(10, rect.Style.GetValue(StyleKeys.SVGGeometryYKey, 0), "The Y position of the rect was not correct");
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
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGComponents.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGComponents.pdf"))
                {
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var clock = section.Contents[3] as Canvas;
                    
                    Assert.IsNotNull(clock);
                    Assert.AreEqual("ClockIcon", clock.ID);
                    Assert.AreEqual(3, clock.Contents.Count);
                    Assert.AreEqual(20, clock.Width);
                    Assert.AreEqual(20, clock.Height);

                    Assert.IsInstanceOfType(clock.Contents[0], typeof(Whitespace));
                    var clockPath = clock.Contents[1] as SVGPath;
                    Assert.IsInstanceOfType(clock.Contents[2], typeof(Whitespace));
                    Assert.IsNotNull(clockPath);
                    Assert.AreEqual(StandardColors.Blue, clockPath.FillColor);
                }
            }
        }

        [TestMethod]
        public void SVGTransform()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform.html",
                this.TestContext);
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
        
        [TestMethod]
        public void SVGFillColor()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <rect id='rect1' width=""100"" height=""80"" x=""10"" y=""10"" fill=""red"" fill-opacity=""1""></rect>
              <rect id='rect2' width=""100"" height=""80"" x=""10"" y=""80"" fill=""#ABABAB"" fill-opacity=""0.5""></rect>
            </svg>
";
            
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

            Assert.AreEqual(5, svg.Contents.Count);
            //0 = whitespace
            var rect1 = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var colVal = rect1.FillValue as SVGFillColorValue;
            Assert.IsNotNull(colVal);
            Assert.AreEqual(StandardColors.Red, colVal.FillColor);
            Assert.AreEqual("red", colVal.Value);
            
            //2 = whitespace

            var rect2 = svg.Contents[3] as SVGRect;
            Assert.IsNotNull(rect2);
            Assert.AreEqual("rect2", rect2.ID);
            Assert.IsNotNull(rect2.FillValue);
            
            colVal = rect2.FillValue as SVGFillColorValue;
            Assert.IsNotNull(colVal);
            Assert.AreEqual("rgb(171,171,171)", colVal.FillColor.ToString());
            Assert.AreEqual("#ABABAB", colVal.Value);
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillColor.pdf"))
            {
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            Assert.IsNotNull(brush);
            Assert.AreEqual(StandardColors.Red, brush.Color);
            Assert.AreEqual(1.0, brush.Opacity);

            line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun2);
            Assert.AreEqual(rect2, compRun2.Owner);
            arrange = rect2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            brush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            Assert.IsNotNull(brush);
            Assert.AreEqual("rgb(171,171,171)", brush.Color.ToString());
            Assert.AreEqual(0.5, brush.Opacity);
        }
        
        [TestMethod]
        public void SVGFillLinearGradient()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <defs>
                <linearGradient id=""grad1"" x1=""0"" x2=""0.5"" y1=""0"" y2=""1"">
                    <stop offset=""0.3"" stop-color=""red"" />
                    <stop offset=""0.5"" stop-color=""white""  />
                    <stop offset=""0.7"" stop-color=""blue"" />
                </linearGradient>
               </defs>
              <rect id='rect1' width=""100"" height=""80"" x=""10"" y=""10"" fill=""url(#grad1)"" fill-opacity=""1""></rect>
            </svg>
";
            
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

            Assert.AreEqual(4, svg.Contents.Count);
            
            //0 = whitespace
            var defs = svg.Definitions;
            Assert.IsNotNull(defs);
            Assert.AreEqual(3, defs.Count);
            var grad = defs[1] as SVGLinearGradient;
            Assert.AreEqual("grad1", grad.ID);
            
            var rect1 = svg.Contents[2] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var gradVal = rect1.FillValue as SVGFillReferenceValue;
            Assert.IsNotNull(gradVal);
            Assert.IsNull(gradVal.Adapter); //should be null at this stage
            Assert.AreEqual("#grad1", gradVal.Value);
            
            //3 = whitespace

            
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillLinearGradient.pdf"))
            {
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFBrush;
            var gradbrush = brush as PDFGradientLinearBrush;
            Assert.IsNotNull(gradbrush);
            
            Assert.AreEqual(5, gradbrush.Colors.Length);
            
            //auto added for padding
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[0].Color);
            Assert.AreEqual(0.0, gradbrush.Colors[0].Distance.Value);
            
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[1].Color);
            Assert.AreEqual(0.3, gradbrush.Colors[1].Distance.Value);
            
            Assert.AreEqual(StandardColors.White, gradbrush.Colors[2].Color);
            Assert.AreEqual(0.5, gradbrush.Colors[2].Distance.Value);
            
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[3].Color);
            Assert.AreEqual(0.7, gradbrush.Colors[3].Distance.Value);
            
            //Auto added for padding
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[4].Color);
            Assert.AreEqual(1.0, gradbrush.Colors[4].Distance.Value);
            
            //Assert.AreEqual(StandardColors.Red, brush.Color);
            //Assert.AreEqual(1.0, brush.Opacity);

            //line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            //Assert.IsNotNull(line);
            //var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            //Assert.IsNotNull(compRun2);
            
            //Assert.AreEqual(rect2, compRun2.Owner);
            //arrange = rect2.GetFirstArrangement();
            //Assert.IsNotNull(arrange);
            //var solidbrush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            //Assert.IsNotNull(brush);
            //Assert.AreEqual("rgb(171,171,171)", solidbrush.Color.ToString());
            //Assert.AreEqual(0.5, solidbrush.Opacity);
        }
        
        [TestMethod]
        public void SVGFillRepeatingLinearGradient()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <defs>
                <linearGradient id=""grad1"" x1=""0"" x2=""1"" y1=""0.5"" y2=""0.3"" spreadMethod=""pad""  >
                    <stop offset=""0.1"" stop-color=""red"" />
                    <stop offset=""0.5"" stop-color=""white""  />
                    <stop offset=""1"" stop-color=""blue"" />
                </linearGradient>
               </defs>
              <rect id='rect1' width=""100"" height=""100"" x=""10"" y=""10"" fill=""url(#grad1)"" fill-opacity=""1""></rect>
            </svg>
";
            
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

            Assert.AreEqual(4, svg.Contents.Count);
            
            //0 = whitespace
            var defs = svg.Definitions;
            Assert.IsNotNull(defs);
            Assert.AreEqual(3, defs.Count);
            var grad = defs[1] as SVGLinearGradient;
            Assert.AreEqual("grad1", grad.ID);
            
            var rect1 = svg.Contents[2] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var gradVal = rect1.FillValue as SVGFillReferenceValue;
            Assert.IsNotNull(gradVal);
            Assert.IsNull(gradVal.Adapter); //should be null at this stage
            Assert.AreEqual("#grad1", gradVal.Value);
            
            //3 = whitespace

            
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 10;

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillRepeatingLinearGradient.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFBrush;
            var gradbrush = brush as PDFGradientLinearBrush;
            Assert.IsNotNull(gradbrush);
            
            Assert.AreEqual(5, gradbrush.Colors.Length);
            
            //auto added for padding
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[0].Color);
            Assert.AreEqual(0.0, gradbrush.Colors[0].Distance.Value);
            
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[1].Color);
            Assert.AreEqual(0.3, gradbrush.Colors[1].Distance.Value);
            
            Assert.AreEqual(StandardColors.White, gradbrush.Colors[2].Color);
            Assert.AreEqual(0.5, gradbrush.Colors[2].Distance.Value);
            
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[3].Color);
            Assert.AreEqual(0.7, gradbrush.Colors[3].Distance.Value);
            
            //Auto added for padding
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[4].Color);
            Assert.AreEqual(1.0, gradbrush.Colors[4].Distance.Value);
            
            //Assert.AreEqual(StandardColors.Red, brush.Color);
            //Assert.AreEqual(1.0, brush.Opacity);

            //line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            //Assert.IsNotNull(line);
            //var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            //Assert.IsNotNull(compRun2);
            
            //Assert.AreEqual(rect2, compRun2.Owner);
            //arrange = rect2.GetFirstArrangement();
            //Assert.IsNotNull(arrange);
            //var solidbrush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            //Assert.IsNotNull(brush);
            //Assert.AreEqual("rgb(171,171,171)", solidbrush.Color.ToString());
            //Assert.AreEqual(0.5, solidbrush.Opacity);
        }

        private PDF.Layout.PDFLayoutDocument _layoutDocument;
        
        private void DocOnFillColorLayoutComplete(object sender, LayoutEventArgs args)
        {
            _layoutDocument = args.Context.GetLayout<PDFLayoutDocument>();
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

            var grp = svg.Contents[3] as SVGGroup;
            Assert.IsNotNull(grp);

            var path = grp.Contents[1] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            var paths = path.PathData.SubPaths.ToArray();
            Assert.AreEqual(1, paths.Length);

            var opPath = paths[0];
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

            grp = svg.Contents[3] as SVGGroup;
            Assert.IsNotNull(grp);

            path = grp.Contents[1] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            paths = path.PathData.SubPaths.ToArray();
            Assert.AreEqual(1, paths.Length);

            opPath = paths[0];
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

                var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/Chart.svg",
                    this.TestContext);
                svgString = System.IO.File.ReadAllText(path);

                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                var svg = (SVGCanvas)component;

                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));

                using var doc = new Document()
                {
                    AppendTraceLog = false
                };
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
                doc.RenderOptions.Compression = OutputCompressionType.None;

                var pg = new Page();
                doc.Pages.Add(pg);
                pg.Contents.Add(svg);
                svg.OverflowAction = OverflowAction.Clip;

                foreach (Component item in svg.Contents)
                {
                    if (item is VisualComponent vis)
                        vis.Style.Overflow.Action = OverflowAction.Clip;
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

            Assert.AreEqual(7, svg.Contents.Count);
            var group = svg.Contents[5] as SVGGroup;
            Assert.IsNotNull(group);

            Assert.AreEqual(4 * 2 + 1, group.Contents.Count);

            var txt = group.Contents[1] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);

            txt = group.Contents[3] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

            txt = group.Contents[5] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);

            //Set via CSS
            txt = group.Contents[7] as SVGText;
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
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridOpacity = 0.5;
            pg.Style.OverlayGrid.GridSpacing = 50;
            pg.Style.OverlayGrid.GridColor = StandardColors.Gray;
            svg.BorderColor = StandardColors.Aqua;
            pg.FontFamily = new FontSelector("serif");
            pg.Margins = new Thickness(50, 0, 0, 50);
            pg.FontSize = 10;
            pg.FontWeight = 700;
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

        [TestMethod]
        public void SVGTextPositionAndTranslate()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">
<text dominant-baseline=""auto"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""50"" y=""50"" transform=""translate(100 100)"" fill=""#6E7079"">At 50 + 100</text>
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
        public void SVGCirclePositionAndTranslate()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">
<circle cx=""50"" cy=""50"" style=""font-size:12px;font-family:sans-serif;"" r=""10"" transform=""translate(100 100)"" fill=""#6E7079""></circle>
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
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 50;

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGCirclePositionAndTranslate.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

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
            
            Assert.Inconclusive("Making changes to the layout - need to comeback and update");

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

            //Positioned run on the first line in the canvas for the circle
            Assert.AreEqual(1, lcanvReg.Contents.Count);
            var lline = lcanvReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(lline);
            Assert.AreEqual(1, lline.Runs.Count);
            var lposRun = lline.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(lposRun);

            //Positioned region in the canvas for the text block
            Assert.AreSame(lposRun.Region, lcanv.PositionedRegions[1]);
            var lcirclePosReg = lcanv.PositionedRegions[1];

            Assert.IsNotNull(lcirclePosReg);
            Assert.AreEqual(1, lcirclePosReg.Contents.Count);
            var lcircleLine = lcirclePosReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(lcircleLine);

            Assert.AreEqual(1, lcircleLine.Runs.Count);
            var lcircleRun = lcircleLine.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(lcircleRun);

            const double cWidth = 20.0; //bounds horizontal
            const double cHeight = 20.0; // bounds vertical
            const double cOffsetX = 50.0 - (cWidth / 2.0); // circle centre - half width
            const double cOffsetY = 50.0 - (cHeight / 2.0); // circle centre - half height
            const double ctranslateX = 100.0;
            const double ctranslateY = 100.0;

            //The offsets are on the positioned region bounds
            Assert.AreEqual(cOffsetX, lcirclePosReg.TotalBounds.X);
            Assert.AreEqual(cOffsetY, lcirclePosReg.TotalBounds.Y);

            var circle = lcircleRun.Owner as SVGShape;

            var path = (circle as IGraphicPathComponent).Path;
            Assert.IsNotNull(path);
            Assert.AreEqual(cWidth, path.Bounds.Width);
            Assert.AreEqual(cHeight, path.Bounds.Height);

            var transform = circle.TransformOperation;
            var matrix = transform.GetMatrix(MatrixOrder.Append);
            Assert.AreEqual(ctranslateX, matrix.Components[4]);
            Assert.AreEqual(-ctranslateY, matrix.Components[5]);
        }
        

        [TestMethod]
        public void SVGLineChart()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">

<rect width=""498"" height=""305"" x=""0"" y=""0"" id=""0"" fill=""none""></rect>
<!-- Axes -->
<path d=""M52.9693 276.5L478.08 276.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 240.5L478.08 240.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 204.5L478.08 204.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 168.5L478.08 168.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 132.5L478.08 132.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 96.5L478.08 96.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 60.5L478.08 60.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 276.5L478.08 276.5"" fill=""none"" stroke=""#6E7079"" stroke-linecap=""round""></path>
<path d=""M53.5 275.85L53.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M124.5 275.85L124.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M194.5 275.85L194.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M265.5 275.85L265.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M336.5 275.85L336.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M407.5 275.85L407.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M478.5 275.85L478.5 280.85"" fill=""none"" stroke=""#6E7079""></path> 
<!-- Labels -->
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 275.85)"" fill=""#6E7079"">0</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 239.875)"" fill=""#6E7079"">500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 203.9)"" fill=""#6E7079"">1,000</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 167.925)"" fill=""#6E7079"">1,500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 131.95)"" fill=""#6E7079"">2,000</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 95.975)"" fill=""#6E7079"">2,500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 60)"" fill=""#6E7079"">3,000</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(52.9693 283.85)"" fill=""#6E7079"">Mon</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(123.8211 283.85)"" fill=""#6E7079"">Tue</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(194.6729 283.85)"" fill=""#6E7079"">Wed</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(265.5246 283.85)"" fill=""#6E7079"">Thu</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(336.3764 283.85)"" fill=""#6E7079"">Fri</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(407.2282 283.85)"" fill=""#6E7079"">Sat</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(478.08 283.85)"" fill=""#6E7079"">Sun</text>
<!-- Data lines -->
<g clip-path=""url(#zr0-c0)"">
<path d=""M52.9693 267.216L123.8211 266.3526L194.6729 268.583L265.5247 266.2087L336.3764 269.3745L407.2282 259.3015L478.08 260.7405"" fill=""none"" stroke=""rgb(84,112,198)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c1)"">
<path d=""M52.9693 251.387L123.8211 253.2577L194.6729 254.8406L265.5247 249.3724L336.3764 248.509L407.2282 235.558L478.08 238.436"" fill=""none"" stroke=""rgb(145,204,117)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c2)"">
<path d=""M52.9693 240.5945L123.8211 236.5653L194.6729 240.3786L265.5247 238.2921L336.3764 234.8385L407.2282 211.8145L478.08 208.9365"" fill=""none"" stroke=""rgb(250,200,88)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c3)"">
<path d=""M52.9693 217.5705L123.8211 212.6779L194.6729 218.7217L265.5247 214.2608L336.3764 206.778L407.2282 188.071L478.08 185.9125"" fill=""none"" stroke=""rgb(238,102,102)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c4)"">
<path d=""M52.9693 158.5715L123.8211 145.6205L194.6729 153.8947L265.5247 147.0595L336.3764 113.9625L407.2282 92.3775L478.08 90.9385"" fill=""none"" stroke=""rgb(115,192,222)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g> 
<!-- Data circles -->
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 52.9693 267.216)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 123.8211 266.3526)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 194.6729 268.583)"" fill=""rgb(255,255,255)"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 265.5247,266.2087)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,269.3745)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,259.3015)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,260.7405)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,251.387)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,253.2577)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,254.8406)"" fill=""rgb(255,255,255)"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,249.3724)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,248.509)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,235.558)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,238.436)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,240.5945)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,236.5653)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,240.3786)"" fill=""rgb(255,255,255)"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,238.2921)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,234.8385)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,211.8145)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,208.9365)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,217.5705)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,212.6779)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,218.7217)"" fill=""rgb(255,255,255)"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,214.2608)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,206.778)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,188.071)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,185.9125)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,158.5715)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,145.6205)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,153.8947)"" fill=""rgb(255,255,255)"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,147.0595)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,113.9625)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,92.3775)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,90.9385)"" fill=""#fff"" stroke=""#73c0de""></path> 
<!-- Legend  -->
<path d=""M-5 -5l454.9199 0l0 23.2l-454.9199 0Z"" transform=""translate(26.54 5)"" fill=""rgb(0,0,0)"" fill-opacity=""0"" stroke=""#ccc"" stroke-width=""0""></path>
<path d=""M0 7L25 7"" transform=""translate(27.54 4.6)"" fill=""#000"" stroke=""#5470c6"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(27.54 4.6)"" fill=""#fff"" stroke=""#5470c6"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""30"" y=""7"" transform=""translate(27.54 4.6)"" fill=""#333"">Email</text>
<path d=""M0 7L25 7"" transform=""translate(98.5459 4.6)"" fill=""#000"" stroke=""#91cc75"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(98.5459 4.6)"" fill=""#fff"" stroke=""#91cc75"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(98.5459 4.6)"" fill=""#333"">Union Ads</text>
<path d=""M0 7L25 7"" transform=""translate(194.9111 4.6)"" fill=""#000"" stroke=""#fac858"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(194.9111 4.6)"" fill=""#fff"" stroke=""#fac858"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(194.9111 4.6)"" fill=""#333"">Video Ads</text>
<path d=""M0 7L25 7"" transform=""translate(290.4033 4.6)"" fill=""#000"" stroke=""#ee6666"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(290.4033 4.6)"" fill=""#fff"" stroke=""#ee6666"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""30"" y=""7"" transform=""translate(290.4033 4.6)"" fill=""#333"">Direct</text>
<path d=""M0 7L25 7"" transform=""translate(362.7393 4.6)"" fill=""#000"" stroke=""#73c0de"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(362.7393 4.6)"" fill=""#fff"" stroke=""#73c0de"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(362.7393 4.6)"" fill=""#333"">Search Engine</text> 
<path d=""M-5 -5l10 0l0 10l-10 0Z"" transform=""translate(5 5)"" fill=""rgb(0,0,0)"" fill-opacity=""0"" stroke=""#ccc"" stroke-width=""0""></path>
<defs >
<clipPath id=""zr0-c0"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c1"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c2"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c3"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c4"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
</defs>
</svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual(181, svg.Contents.Count);

            

            svg.OverflowAction = OverflowAction.Clip;
            svg.BorderWidth = 1;

            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);
            pg.Margins = new Thickness(25);
            pg.BorderWidth = 1;
            pg.Padding = new Thickness(25);

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGLineChart.pdf"))
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



