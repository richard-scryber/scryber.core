using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using Scryber.Html.Components;
using System.CodeDom;
using System.Xml.Linq;
using Scryber.OpenType.Woff2.Numerics;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;
using Size = Scryber.Drawing.Size;

namespace Scryber.Core.UnitTests.Svg
{
    
    
    /// <summary>
    ///This is a test class for PDFColor_Test and is intended
    ///to contain all PDFColor_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class SVGPatternFill_Test
    {


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

        [TestMethod()]
        public void SVGPatternFillBrushes_Test()
        {
            
            var doc = new Document();
            var page = new Page();
            page.Style.Font.FontFamily = FontSelector.Parse("Serif");
            page.Padding = 8;
            doc.Pages.Add(page);
            
            
            var svg = new SVGCanvas() { Width = 510, Height = 110 };
            svg.BackgroundColor = Color.Parse("#AAA");
            page.Contents.Add(svg);
            
            var text = new SVGText();
            text.Fill = new SVGFillReferenceValue(null, "#star");
            text.FontFamily = FontSelector.Parse("sans-serif");
            text.FontSize = 24;
            text.FontWeight = FontWeights.Bold;
            text.X = 10;
            text.Y = 10;
            text.DominantBaseline = DominantBaseline.Hanging;
            text.Content.Add(new TextLiteral("Hello World"));
            text.StrokeColor = StandardColors.Black;
            text.StrokeWidth = 1;
            text.TextLength = 80;
            svg.Contents.Add(text);
            
            SVGRect rect = new SVGRect();
            rect.X = 100;
            rect.Y = 10;
            rect.Width = 70;
            rect.Height = 70;
            rect.FillValue = new SVGFillReferenceValue(null, "#star");
            rect.StrokeColor = StandardColors.Black;
            rect.StrokeWidth = 1;
            
            
            svg.Contents.Add(rect);
            
            SVGPath svgPath = new SVGPath();
            svgPath.PathData = GraphicsPath.Parse(@"M 200,30
            A 20,20 0 0 1 240,30
            A 20,20 0,0,1 280,30
            Q 280,60 240,90
            Q 200,60 200,30 z");
            svgPath.Fill = new SVGFillReferenceValue(null, "#star");
            svgPath.StrokeColor = StandardColors.Black;
            svgPath.StrokeWidth = 1;
            svg.Contents.Add(svgPath);
            
            SVGPolygon polyline = new SVGPolygon();
            polyline.Points.Add(new Point(300, 100));
            polyline.Points.Add(new Point(350, 70));
            polyline.Points.Add(new Point(400, 10));
            polyline.Points.Add(new Point(350, 30));
            polyline.Fill = new SVGFillReferenceValue(null, "#star");
            polyline.StrokeColor = StandardColors.Black;
            polyline.StrokeWidth = 1;
            
            svg.Contents.Add(polyline);
            
            
            SVGCircle circle = new SVGCircle();
            circle.Radius = 40;
            circle.CentreX = 450;
            circle.CenterY = 50;
            circle.Fill = new SVGFillReferenceValue(null, "#star");
            circle.StrokeColor = StandardColors.Black;
            circle.StrokeWidth = 1;
            svg.Contents.Add(circle);
            
            var pattern = new SVGPattern();
            pattern.ID = "star";
            pattern.PatternWidth = Unit.Percent(15);
            pattern.PatternHeight = Unit.Percent(30);
            //pattern.ViewBox = new Rect(0, 0, 10, 10);
            pattern.OffsetX = 5;
            pattern.OffsetY = 5;
            pattern.Contents.Add(new SVGRect()
                { X = 2.5, Y = 2.5, Width = 5, Height = 5, FillValue = new SVGFillColorValue(StandardColors.Red, "Red") });
            
            svg.Contents.Add(pattern);
            
            using(var stream = DocStreams.GetOutputStream("SVG_PatternFillBrushes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            var layoutKey = PDFPatternLayoutResource.GetLayoutResourceKey(pattern.UniqueID);
            Assert.AreEqual(pattern.UniqueID + "_layout", layoutKey);
            
            var descriptorKey = Scryber.Drawing.GraphicTilingPatternDescriptor.GetResourceKey(pattern.UniqueID);
            Assert.AreEqual(pattern.UniqueID + "_descriptor", descriptorKey);
            
            Assert.AreEqual(5, doc.SharedResources.Count);
            
            
            
            var layout = doc.SharedResources[2] as PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            Assert.AreEqual(PDFResource.XObjectResourceType, layout.ResourceType);
            Assert.AreEqual(layoutKey, layout.ResourceKey);
            Assert.AreEqual(pattern, layout.Pattern);
            Assert.IsNotNull(pattern.InnerCanvas);
            
            var descriptor = doc.SharedResources[3] as GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descriptor);
            Assert.AreEqual(PDFResource.PatternResourceType, descriptor.ResourceType);
            Assert.AreEqual(descriptorKey, descriptor.ResourceKey);
            Assert.AreEqual(15, descriptor.PatternSize.Width.Value);
            Assert.AreEqual(30, descriptor.PatternSize.Height.Value);
            
            Assert.AreEqual(layout.Descriptor, descriptor);
            
            //Just check the patterns to make sure they have been set based on the brushes (from the SVGFillReferenceValue) for each component.

            var arrange = text.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.IsNotNull(arrange.FullStyle);
            var brush = arrange.FullStyle.CreateFillBrush();
            Assert.IsNotNull(brush);
            Assert.IsInstanceOfType<Scryber.PDF.Graphics.PDFGraphicPatternBrush>(brush);
            
            arrange = rect.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.IsNotNull(arrange.FullStyle);
            brush = arrange.FullStyle.CreateFillBrush();
            Assert.IsNotNull(brush);
            Assert.IsInstanceOfType<Scryber.PDF.Graphics.PDFGraphicPatternBrush>(brush);

            arrange = svgPath.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.IsNotNull(arrange.FullStyle);
            brush = arrange.FullStyle.CreateFillBrush();
            Assert.IsNotNull(brush);
            Assert.IsInstanceOfType<Scryber.PDF.Graphics.PDFGraphicPatternBrush>(brush);
            
            arrange = polyline.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.IsNotNull(arrange.FullStyle);
            brush = arrange.FullStyle.CreateFillBrush();
            Assert.IsNotNull(brush);
            Assert.IsInstanceOfType<Scryber.PDF.Graphics.PDFGraphicPatternBrush>(brush);
            
            arrange = circle.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            Assert.IsNotNull(arrange.FullStyle);
            brush = arrange.FullStyle.CreateFillBrush();
            Assert.IsNotNull(brush);
            Assert.IsInstanceOfType<Scryber.PDF.Graphics.PDFGraphicPatternBrush>(brush);

            //The id of the brush pattern should be the same as the pattern resource
            var patternBrush = brush as Scryber.PDF.Graphics.PDFGraphicPatternBrush;
            Assert.IsNotNull(patternBrush);
            
            Assert.AreEqual(descriptorKey, patternBrush.DescriptorKey);
            Assert.AreEqual(layoutKey, patternBrush.LayoutKey);
            
        }


        /// <summary>
        ///A test to make sure the SVG is rendered correctly with tiling patterns
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGSimplePatterns_Test()
        {


            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsSimple.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsSimple.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(11, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)

            //First is the square layout and descriptor
            
            var layout = doc.SharedResources[2] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            var descrptor = doc.SharedResources[3] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);

            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(10), Unit.Percent(10)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(Point.Empty, descrptor.PatternOffset);

            
            var tilingBounds = new Rect(0,0, 80, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            var actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, 11.25), actualSize); //viewbox in proportion to 80 and 90
            var actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            var actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            var comps = actualMatrix.Components;
            Assert.AreEqual(0.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.8, comps[3]);
            Assert.AreEqual(0.0, comps[4]);
            Assert.AreEqual(105 - 8.5, comps[5]);

            
            
            
            //Then the star - same size and pattern spacing
            layout = doc.SharedResources[4] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[5] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(10), Unit.Percent(10)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(Point.Empty, descrptor.PatternOffset);

            
            tilingBounds = new Rect(0,0, 80, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, 11.25), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.8, comps[3]);
            Assert.AreEqual(0.0, comps[4]);
            Assert.AreEqual(105 - 8.5, comps[5]);
            
            
            
            
            //Then the square with gradient - thinner, horizontal spacing
            layout = doc.SharedResources[6] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[7] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            tilingBounds = new Rect(0,0, 80, 30);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size((8.0/3.0)*10.0, 10), actualSize); //viewbox in proportion to 80 and 30
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.6, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.6, comps[3]);
            Assert.AreEqual(5, comps[4]);
            Assert.AreEqual(99, comps[5]);
            
            //Then the star (again) - with hign and narrow
            layout = doc.SharedResources[4] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[5] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(10), Unit.Percent(10)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(Point.Empty, descrptor.PatternOffset);


            tilingBounds = new Rect(0, 0, 50, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, (9.0/5)*10.0), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.5, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.5, comps[3]);
            Assert.AreEqual(0, comps[4]);
            Assert.AreEqual(105 - 7, comps[5]);
            

            //Then the circleswithstar - 20% width and height in a wide box
            layout = doc.SharedResources[8] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[9] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(20), Unit.Percent(20)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(Point.Empty, descrptor.PatternOffset);


            tilingBounds = new Rect(0, 0, 150, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size((15.0/9.0) * 10.0, 10), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(1.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(1.8, comps[3]);
            Assert.AreEqual(6, comps[4]); //added left space to balance for default preserve aspect ratio
            Assert.AreEqual(105 - 18, comps[5]);
        }


        
        /// <summary>
        ///A test to make sure the SVG is rendered correctly with offset tiling patterns
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGOffsetPatterns_Test()
        {


            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsOffset.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsOffset.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(11, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)

            //First is the square layout and descriptor - 5% x and y offset.
            
            var layout = doc.SharedResources[2] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            var descrptor = doc.SharedResources[3] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);

            Assert.AreEqual(new Size(Unit.Percent(10), Unit.Percent(10)), descrptor.PatternSize);
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(new Point(Unit.Percent(5), Unit.Percent(5)), descrptor.PatternOffset);

            
            var tilingBounds = new Rect(0,0, 80, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            var actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, 11.25), actualSize); //viewbox in proportion to 80 and 90
            var actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            var offset = new Point(80 * 0.05, 90 * 0.05);
            
            var actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            var comps = actualMatrix.Components;
            Assert.AreEqual(0.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.8, comps[3]);
            Assert.AreEqual(offset.X, comps[4]);
            Assert.AreEqual(105 - 8.5 + offset.Y, comps[5]);

            
            
            
            //Then the star - same size and pattern spacing 5% x offset
            layout = doc.SharedResources[4] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[5] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(10), Unit.Percent(10)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(new Point(Unit.Percent(5), 0), descrptor.PatternOffset);

            
            tilingBounds = new Rect(0,0, 80, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, 11.25), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            offset = new Point(80 * 0.05, 0);
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.8, comps[3]);
            Assert.AreEqual(offset.X, comps[4]);
            Assert.AreEqual(105 - 8.5 + offset.Y, comps[5]);
            
            
            
            
            //Then the square with gradient - thinner, horizontal spacing -6.7% X offset
            layout = doc.SharedResources[6] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[7] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            Assert.AreEqual(new Point(Unit.Percent(-6.7d), 0), descrptor.PatternOffset);
            
            tilingBounds = new Rect(0,0, 80, 30);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size((8.0/3.0)*10.0, 10), actualSize); //viewbox in proportion to 80 and 30
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            offset = new Point(80 * -0.067, 0);
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.6, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.6, comps[3]);
            Assert.AreEqual(Math.Round(5 + offset.X.PointsValue, 6), Math.Round(comps[4], 6));
            Assert.AreEqual(99 + offset.Y, comps[5]);
            
            //Then the star (again) - with hign and narrow at 5% x
            layout = doc.SharedResources[4] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[5] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(10), Unit.Percent(10)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(new Point(Unit.Percent(5), 0), descrptor.PatternOffset);


            tilingBounds = new Rect(0, 0, 50, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size(10, (9.0/5)*10.0), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            offset = new Point(50 * 0.05, 0); //50 width on the rect
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(0.5, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(0.5, comps[3]);
            Assert.AreEqual(offset.X.PointsValue, comps[4]);
            Assert.AreEqual(105 - 7 + offset.Y.PointsValue, comps[5]);
            

            //Then the circleswithstar - 20% width and height in a wide box -10% y
            layout = doc.SharedResources[8] as Scryber.PDF.Resources.PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            descrptor = doc.SharedResources[9] as Scryber.Drawing.GraphicTilingPatternDescriptor;
            Assert.IsNotNull(descrptor);
            
            Assert.AreEqual(descrptor.PatternSize, new Size(Unit.Percent(20), Unit.Percent(20)));
            Assert.AreEqual(new Rect(0,0,10,10), descrptor.PatternViewBox);
            Assert.AreEqual(new Point(0, Unit.Percent(-10)), descrptor.PatternOffset);


            tilingBounds = new Rect(0, 0, 150, 90);
            descrptor.CurrentContainerSize = new Size(550, 105);

            actualSize = descrptor.CalculatePatternStepForShape(tilingBounds, null);
            Assert.AreEqual(new Size((15.0/9.0) * 10.0, 10), actualSize); //viewbox in proportion to 80 and 90
            actualBBox = descrptor.CalculatePatternBoundsForShape(tilingBounds, null);
            Assert.AreEqual(new Rect(0,0,10,10), actualBBox);
            
            offset = new Point(0, 90 * -0.10); //-10% of 90 high on the rect
            actualMatrix = descrptor.CalculatePatternTransformMatrixForShape(tilingBounds, null);
            comps = actualMatrix.Components;
            Assert.AreEqual(1.8, comps[0]);
            Assert.AreEqual(0.0, comps[1]);
            Assert.AreEqual(0.0, comps[2]);
            Assert.AreEqual(1.8, comps[3]);
            Assert.AreEqual(6 + offset.X, comps[4]); //added left space to balance for default preserve aspect ratio
            Assert.AreEqual(105 - 18 + offset.Y, comps[5]);
        }
        
        
        private void AssertLayoutPattern(PDFPatternLayoutResource layout, dynamic data, int index)
        {
            Assert.AreEqual(data.key, layout.ResourceKey, "Layout resource key failed at index " + index);
            Assert.IsNotNull(layout.Descriptor, "Layout for index " + index + " is null");
            Assert.IsNotNull(layout.Pattern, "Layout for index " + index + " is null");
            Assert.IsInstanceOfType(layout.Pattern, typeof(SVGPattern));
            var pattern = (SVGPattern)layout.Pattern;
            Assert.AreEqual(3, pattern.Contents.Count, "Layout pattern content count for index " + index + " is invalid");
            var rect = pattern.Contents[1] as SVGRect;
            Assert.IsNotNull(rect, "Layout rect for index " + index + " is null");

        }

        private void AssertPatternDescriptor(GraphicTilingPatternDescriptor descriptor, dynamic data, int index)
        {
            Assert.AreEqual(data.key, descriptor.ResourceKey, "Descriptor resource key failed at index " + index );
            Assert.AreEqual(data.type, descriptor.ResourceType, "Descriptor resource type failed at index " + index);
            Assert.AreEqual(data.size, descriptor.PatternSize, "Descriptor size failed at index " + index);
            Assert.AreEqual(data.viewBox, descriptor.PatternViewBox, "Descriptor viewBox failed at index " + index);
            Assert.AreEqual(data.offset, descriptor.PatternOffset, "Descriptor offset failed at index " + index);
            Assert.AreEqual(data.aspect, descriptor.PatternAspectRatio.Align, "Descriptor AspectRatio alignment failed at index " + index);
            Assert.AreEqual(data.meet, descriptor.PatternAspectRatio.Meet , "Descriptor AspectRatio meet failed at index " + index);
        }

        private void AssertPatternCalculations(GraphicTilingPatternDescriptor descriptor, dynamic data, int index)
        {
            Rect actBounds = (Rect)data.bounds;
            
            var step = descriptor.CalculatePatternStepForShape(actBounds, null);
            var bbox = descriptor.CalculatePatternBoundsForShape(actBounds, null);
            var matrix = descriptor.CalculatePatternTransformMatrixForShape(actBounds, null).Components;
            
            Assert.AreEqual(data.xStep, Math.Round(step.Width.PointsValue, 1), "Descriptor step width failed at index " + index);
            Assert.AreEqual(data.yStep, Math.Round(step.Height.PointsValue, 1), "Descriptor step height failed at index " + index);
            
            Assert.AreEqual(data.bbox, bbox, "Descriptor bbox failed at index " + index);
            Assert.AreEqual(6, matrix.Length, "Descriptor matrix failed at index " + index);
            
            for (var i = 0; i < 6; i++)
            {
                Assert.AreEqual(data.matrix[i], Math.Round(matrix[i], 1),
                    "Descriptor pattern matrix value " + i + " failed at index " + index);
            }
            
            
        }
        
        
        [TestMethod]
        public void SVGPreserveAspectRatioWidePatterns_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsAspectRatioWide.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsAspectRatioWide.pdf"))
            {
                //doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            var patterns = 11;
            var fonts = 2;
            var canvas = 1;
            var gradients = 1;
            
            Assert.AreEqual((patterns * 2) + fonts + canvas + gradients, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)
            //Top Label
            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);

            var layouts = new[]
            {
                new { key = "hBdy1_Patterns_noAp_layout" },
                new { key = "hBdy1_Patterns_xminymin_layout" },
                new { key = "hBdy1_Patterns_xminymid_layout" },
                new { key = "hBdy1_Patterns_xminymax_layout" },
                new { key = "hBdy1_Patterns_xMidYMin_layout" },
                new { key = "hBdy1_Patterns_xMidYMid_layout" },
                new { key = "hBdy1_Patterns_xMidYMax_layout" },
                new { key = "hBdy1_Patterns_xMaxYMin_layout" },
                new { key = "hBdy1_Patterns_xMaxYMid_layout" },
                new { key = "hBdy1_Patterns_xMaxYMax_layout" },
                new { key = "hBdy1_Patterns_apNone_layout" }
            };

            var size = new Size(Unit.Percent(20), Unit.Percent(10));
            var viewBox = new Rect(0, 0, 10, 10);
            var offset = new Point(0, 0);

            var descriptors = new[]
            {
                new { key = "hBdy1_Patterns_noAp_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymax_descriptor" , type = PDFResource.PatternResourceType , size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMax, meet = AspectRatioMeet.Meet},
                new { key = "hBdy1_Patterns_xMidYMin_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMidYMid_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMidYMax_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMax, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMax_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMax, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_apNone_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.None , meet = AspectRatioMeet.Meet},
            };
            
            var calcs = new []
            {
                new { bounds = new Rect(10, 10, 80, 90), xStep = 17.8d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 13.5d, 331.0d }},
                
                new { bounds = new Rect(100, 10, 80, 90), xStep = 17.8d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 100.0d, 331.0d }},
                new { bounds = new Rect(65, 120, 100, 90), xStep = 22.2d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 65.0d, 221.0d }},
                new { bounds = new Rect(10, 230, 140, 90), xStep = 31.1d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 10.0d, 111.0d }},
                
                new { bounds = new Rect(190, 10, 80, 90), xStep = 17.8d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 193.5d, 331.0d }},
                new { bounds = new Rect(180, 120, 100, 90), xStep = 22.2d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 185.5d, 221.0d }},
                new { bounds = new Rect(160, 230, 140, 90), xStep = 31.1d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 169.5d, 111.0d }},
                
                new { bounds = new Rect(280, 10, 80, 90), xStep = 17.8d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 287.8d, 331.0d }},
                new { bounds = new Rect(295, 120, 100, 90), xStep = 22.2d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 307.2d, 221.0d }},
                new { bounds = new Rect(310, 230, 140, 90), xStep = 31.1d, yStep = 10.0d, bbox = new Rect(0,0,10,10), matrix = new [] {0.9d, 0.0d, 0.0d, 0.9d, 331.1d, 111.0d }}
            };
            
            
            
            for (var i = 0; i < patterns; i++)
            {
                var index = i * 2;
                index += 2; //for the font and canvas layout run
                var layout = doc.SharedResources[index] as PDFPatternLayoutResource;
                Assert.IsNotNull(layout, "Layout for index " + index + " is null");
                var desc = doc.SharedResources[index + 1] as GraphicTilingPatternDescriptor;
                Assert.IsNotNull(desc, "Descriptor for index " + index + " is null");

                AssertLayoutPattern(layout, layouts[i], i);
                AssertPatternDescriptor(desc, descriptors[i], i);
                
                if (calcs.Length > i)
                    AssertPatternCalculations(desc, calcs[i], i);
            }
        }
        
        
        [TestMethod]
        public void SVGPreserveAspectRatioHighPatterns_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsAspectRatioHigh.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsAspectRatioHigh.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            var patterns = 11;
            var fonts = 2;
            var canvas = 1;
            var gradients = 1;
            
            Assert.AreEqual((patterns * 2) + fonts + canvas + gradients, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)
            //Top Label
            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);

            var layouts = new[]
            {
                new { key = "hBdy1_Patterns_noAp_layout" },
                new { key = "hBdy1_Patterns_xminymin_layout" },
                new { key = "hBdy1_Patterns_xminymid_layout" },
                new { key = "hBdy1_Patterns_xminymax_layout" },
                new { key = "hBdy1_Patterns_xMidYMin_layout" },
                new { key = "hBdy1_Patterns_xMidYMid_layout" },
                new { key = "hBdy1_Patterns_xMidYMax_layout" },
                new { key = "hBdy1_Patterns_xMaxYMin_layout" },
                new { key = "hBdy1_Patterns_xMaxYMid_layout" },
                new { key = "hBdy1_Patterns_xMaxYMax_layout" },
                new { key = "hBdy1_Patterns_apNone_layout" }
            };

            var size = new Size(Unit.Percent(10), Unit.Percent(20));
            var viewBox = new Rect(0, 0, 10, 10);
            var offset = new Point(0, 0);

            var descriptors = new[]
            {
                new { key = "hBdy1_Patterns_noAp_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymax_descriptor" , type = PDFResource.PatternResourceType , size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMax, meet = AspectRatioMeet.Meet},
                new { key = "hBdy1_Patterns_xMidYMin_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMidYMid_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMidYMax_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMax, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMin, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xMaxYMax_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMax, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_apNone_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.None , meet = AspectRatioMeet.Meet},
            };
            
            var calcs = new []
            {
                new { bounds = new Rect(10, 10, 80, 90), xStep = 10.0d, yStep = 22.5d, bbox = new Rect(0,0,10,10), matrix = new [] {0.8d, 0.0d, 0.0d, 0.8d, 10.0d, 327.0d }},
                
                new { bounds = new Rect(100, 10, 80, 90), xStep = 10.0d, yStep = 22.5d, bbox = new Rect(0,0,10,10), matrix = new [] {0.8d, 0.0d, 0.0d, 0.8d, 100.0d, 332.0d }},
                new { bounds = new Rect(65, 120, 100, 90), xStep = 10.0d, yStep = 18.0d, bbox = new Rect(0,0,10,10), matrix = new [] {1.0d, 0.0d, 0.0d, 1.0d, 65.0d, 216.0d }},
                new { bounds = new Rect(10, 230, 140, 90), xStep = 10.0d, yStep = 12.9d, bbox = new Rect(0,0,10,10), matrix = new [] {1.4d, 0.0d, 0.0d, 1.4d, 10.0d, 102.0d }},
                
                new { bounds = new Rect(190, 10, 80, 90), xStep = 10.0d, yStep = 22.5d, bbox = new Rect(0,0,10,10), matrix = new [] {0.8d, 0.0d, 0.0d, 0.8d, 190.0d, 332.0d }},
                new { bounds = new Rect(180, 120, 100, 90), xStep = 10.0d, yStep = 18.0d, bbox = new Rect(0,0,10,10), matrix = new [] {1.0d, 0.0d, 0.0d, 1.0d, 180.0d, 216.0d }},
                new { bounds = new Rect(160, 230, 140, 90), xStep = 10.0d, yStep = 12.9d, bbox = new Rect(0,0,10,10), matrix = new [] {1.4d, 0.0d, 0.0d, 1.4d, 160.0d, 102.0d }},
                
                new { bounds = new Rect(280, 10, 80, 90), xStep = 10.0d, yStep = 22.5d, bbox = new Rect(0,0,10,10), matrix = new [] {0.8d, 0.0d, 0.0d, 0.8d, 280.0d, 332.0d }},
                new { bounds = new Rect(295, 120, 100, 90), xStep = 10.0d, yStep = 18.0d, bbox = new Rect(0,0,10,10), matrix = new [] {1.0d, 0.0d, 0.0d, 1.0d, 295.0d, 216.0d }},
                new { bounds = new Rect(310, 230, 140, 90), xStep = 10.0d, yStep = 12.9d, bbox = new Rect(0,0,10,10), matrix = new [] {1.4d, 0.0d, 0.0d, 1.4d, 310.0d, 102.0d }}
            };
            
            for (var i = 0; i < patterns; i++)
            {
                var index = i * 2;
                index += 2; //for the font and canvas layout run
                var layout = doc.SharedResources[index] as PDFPatternLayoutResource;
                Assert.IsNotNull(layout, "Layout for index " + index + " is null");
                var desc = doc.SharedResources[index + 1] as GraphicTilingPatternDescriptor;
                Assert.IsNotNull(desc, "Descriptor for index " + index + " is null");

                AssertLayoutPattern(layout, layouts[i], i);
                AssertPatternDescriptor(desc, descriptors[i], i);
                
                if (calcs.Length > i)
                    AssertPatternCalculations(desc, calcs[i], i);
            }
        }
        
        [TestMethod]
        public void SVGPreserveAspectRatioHighSlicePatterns_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsAspectRatioHighSlice.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsAspectRatioHighSlice.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            var patterns = 11;
            var fonts = 2;
            var canvas = 1;
            var gradients = 1;
            
            Assert.AreEqual((patterns * 2) + fonts + canvas + gradients, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)
            //Top Label
            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);

            var layouts = new[]
            {
                new { key = "hBdy1_Patterns_noAp_layout" },
                new { key = "hBdy1_Patterns_xminymin_layout" },
                new { key = "hBdy1_Patterns_xminymid_layout" },
                new { key = "hBdy1_Patterns_xminymax_layout" },
                new { key = "hBdy1_Patterns_xMidYMin_layout" },
                new { key = "hBdy1_Patterns_xMidYMid_layout" },
                new { key = "hBdy1_Patterns_xMidYMax_layout" },
                new { key = "hBdy1_Patterns_xMaxYMin_layout" },
                new { key = "hBdy1_Patterns_xMaxYMid_layout" },
                new { key = "hBdy1_Patterns_xMaxYMax_layout" },
                new { key = "hBdy1_Patterns_apNone_layout" }
            };

            var size = new Size(Unit.Percent(10), Unit.Percent(20));
            var viewBox = new Rect(0, 0, 10, 10);
            var offset = new Point(0, 0);

            var descriptors = new[]
            {
                new { key = "hBdy1_Patterns_noAp_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xminymid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xminymax_descriptor" , type = PDFResource.PatternResourceType , size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMax, meet = AspectRatioMeet.Slice},
                new { key = "hBdy1_Patterns_xMidYMin_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMidYMid_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMidYMax_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMax, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMax_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMax, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_apNone_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.None , meet = AspectRatioMeet.Meet},
            };
            
            for (var i = 0; i < patterns; i++)
            {
                var index = i * 2;
                index += 2; //for the font and canvas layout run
                var layout = doc.SharedResources[index] as PDFPatternLayoutResource;
                Assert.IsNotNull(layout, "Layout for index " + index + " is null");
                var desc = doc.SharedResources[index + 1] as GraphicTilingPatternDescriptor;
                Assert.IsNotNull(desc, "Descriptor for index " + index + " is null");

                AssertLayoutPattern(layout, layouts[i], i);
                AssertPatternDescriptor(desc, descriptors[i], i);
            }
            
            
        }
        
        [TestMethod]
        public void SVGPreserveAspectRatioWideSlicePatterns_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsAspectRatioWideSlice.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsAspectRatioWideSlice.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            var patterns = 11;
            var fonts = 2;
            var canvas = 1;
            var gradients = 1;
            
            Assert.AreEqual((patterns * 2) + fonts + canvas + gradients, doc.SharedResources.Count); //1 font, 4 layouts and 4 descriptors, 2 xObj (main and pattern)
            //Top Label
            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);

            var layouts = new[]
            {
                new { key = "hBdy1_Patterns_noAp_layout" },
                new { key = "hBdy1_Patterns_xminymin_layout" },
                new { key = "hBdy1_Patterns_xminymid_layout" },
                new { key = "hBdy1_Patterns_xminymax_layout" },
                new { key = "hBdy1_Patterns_xMidYMin_layout" },
                new { key = "hBdy1_Patterns_xMidYMid_layout" },
                new { key = "hBdy1_Patterns_xMidYMax_layout" },
                new { key = "hBdy1_Patterns_xMaxYMin_layout" },
                new { key = "hBdy1_Patterns_xMaxYMid_layout" },
                new { key = "hBdy1_Patterns_xMaxYMax_layout" },
                new { key = "hBdy1_Patterns_apNone_layout" }
            };

            var size = new Size(Unit.Percent(20), Unit.Percent(10));
            var viewBox = new Rect(0, 0, 10, 10);
            var offset = new Point(0, 0);

            var descriptors = new[]
            {
                new { key = "hBdy1_Patterns_noAp_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Meet },
                new { key = "hBdy1_Patterns_xminymin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xminymid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xminymax_descriptor" , type = PDFResource.PatternResourceType , size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMinYMax, meet = AspectRatioMeet.Slice},
                new { key = "hBdy1_Patterns_xMidYMin_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMidYMid_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMidYMax_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMidYMax, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMin_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMin, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMid_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMid, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_xMaxYMax_descriptor" , type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.xMaxYMax, meet = AspectRatioMeet.Slice },
                new { key = "hBdy1_Patterns_apNone_descriptor", type = PDFResource.PatternResourceType, size = size, viewBox = viewBox, offset = offset, aspect = AspectRatioAlign.None , meet = AspectRatioMeet.Meet},
            };
            
            for (var i = 0; i < patterns; i++)
            {
                var index = i * 2;
                index += 2; //for the font and canvas layout run
                var layout = doc.SharedResources[index] as PDFPatternLayoutResource;
                Assert.IsNotNull(layout, "Layout for index " + index + " is null");
                var desc = doc.SharedResources[index + 1] as GraphicTilingPatternDescriptor;
                Assert.IsNotNull(desc, "Descriptor for index " + index + " is null");

                AssertLayoutPattern(layout, layouts[i], i);
                AssertPatternDescriptor(desc, descriptors[i], i);
            }
        }

        
        [TestMethod]
        public void SVGTransformedPatterns_Test()
        {
            //TODO: Implement the inclusion of a second matrix 
            Assert.Inconclusive();
        }

        [TestMethod]
        public void SVGMixedContentPatterns_Test()
        {
            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGPatternsMixedContent.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_PatternsMixedContent.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
            
            Assert.AreEqual(9, doc.SharedResources.Count);
            var layout = doc.SharedResources[3] as PDFPatternLayoutResource;
            Assert.IsNotNull(layout, "Layout Resource at index 3 was not found");

            var pattern = layout.Pattern as SVGPattern;
            Assert.IsNotNull(pattern, "Pattern for Layout Resource at index 3 was not found");
            
            Assert.AreEqual(11, pattern.Contents.Count);
            Assert.IsInstanceOfType(pattern.Contents[1], typeof(SVGEllipse));
            Assert.IsInstanceOfType(pattern.Contents[3], typeof(SVGRect));
            Assert.IsInstanceOfType(pattern.Contents[5], typeof(SVGRect));
            Assert.IsInstanceOfType(pattern.Contents[7], typeof(SVGRect));
            Assert.IsInstanceOfType(pattern.Contents[9], typeof(SVGText));

            //If we have an arrangement then we have been rendered
            var arrange = pattern.Contents[1].GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for Ellipse was null");
            Assert.AreEqual(new Rect(0, 0, 80, 10), arrange.RenderBounds);
            
            arrange = pattern.Contents[3].GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for Rect 1 was null");
            Assert.AreEqual(new Rect(0, 0, 30, 6), arrange.RenderBounds);
            
            arrange = pattern.Contents[5].GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for Rect 2 was null");
            Assert.AreEqual(new Rect(25, 2, 35, 6), arrange.RenderBounds);
            
            arrange = pattern.Contents[7].GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for Rect 3 was null");
            Assert.AreEqual(new Rect(55, 4, 20, 6), arrange.RenderBounds);
            
            arrange = pattern.Contents[9].GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for Text was null");
            Assert.AreEqual(new Rect(8, -0.7001953125, 64, 12), arrange.RenderBounds);
            
            layout = doc.SharedResources[6] as PDFPatternLayoutResource;
            Assert.IsNotNull(layout, "Layout Resource at index 6 was not found");
            
            pattern = layout.Pattern as SVGPattern;
            Assert.IsNotNull(pattern, "Pattern for Layout Resource at index 6 was not found");
            Assert.AreEqual(5, pattern.Contents.Count);

            var img = pattern.Contents[1] as SVGImage;
            Assert.IsNotNull(img, "Image for Layout Resource at index 6 was not found");
            arrange = img.GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for image was null");
            
            var rect = pattern.Contents[3] as SVGRect;
            Assert.IsNotNull(rect, "Rect for Layout Resource at index 6 was not found");
            arrange = rect.GetFirstArrangement();
            Assert.IsNotNull(arrange, "Arrangement for rect was null");
            
        }
        
        

        
        
        
        

    }
}
