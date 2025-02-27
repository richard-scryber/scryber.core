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
using Scryber.PDF.Graphics;
using Scryber.PDF.Native;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Svg
{
    
    
    /// <summary>
    ///This is a test class for PDFColor_Test and is intended
    ///to contain all PDFColor_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class SVGLinearGradient_Test
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
        public void SVGLinearGradientBrushes_Test()
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
            text.Fill = new SVGFillReferenceValue(null, "#2Color");
            text.FontFamily = FontSelector.Parse("sans-serif");
            text.FontSize = 14;
            text.FontWeight = FontWeights.Bold;
            text.X = 10;
            text.Y = 10;
            text.DominantBaseline = DominantBaseline.Hanging;
            text.Content.Add(new TextLiteral("Hello World"));
            
            svg.Contents.Add(text);
            
            SVGRect rect = new SVGRect();
            rect.X = 100;
            rect.Y = 10;
            rect.Width = 70;
            rect.Height = 70;
            rect.FillValue = new SVGFillReferenceValue(null, "#2Color");
            svg.Contents.Add(rect);
            
            SVGPath svgPath = new SVGPath();
            svgPath.PathData = GraphicsPath.Parse(@"M 200,30
            A 20,20 0 0 1 240,30
            A 20,20 0,0,1 280,30
            Q 280,60 240,90
            Q 200,60 200,30 z");
            svgPath.Fill = new SVGFillReferenceValue(null, "#2Color");
            svg.Contents.Add(svgPath);
            
            SVGPolygon polyline = new SVGPolygon();
            polyline.Points.Add(new Point(300, 100));
            polyline.Points.Add(new Point(350, 70));
            polyline.Points.Add(new Point(400, 10));
            polyline.Points.Add(new Point(350, 30));
            polyline.Fill = new SVGFillReferenceValue(null, "#2Color");
            
            svg.Contents.Add(polyline);
            
            
            SVGCircle circle = new SVGCircle();
            circle.Radius = 40;
            circle.CentreX = 450;
            circle.CenterY = 50;
            circle.Fill = new SVGFillReferenceValue(null, "#2Color");
            svg.Contents.Add(circle);
            
            var gradient = new SVGLinearGradient();
            gradient.ID = "2Color";
            
            gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Maroon});
            svg.Contents.Add(gradient);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientBrushes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
            
            Assert.AreEqual(3, doc.SharedResources.Count);
            var xObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            
            Assert.AreEqual(2, xObj.Renderer.Resources.Types.Count);
            Assert.AreEqual(PDFResource.PatternResourceType, xObj.Renderer.Resources.Types[0].Type);
            Assert.AreEqual(PDFResource.FontDefnResourceType, xObj.Renderer.Resources.Types[1].Type);

            var patterns = xObj.Renderer.Resources.Types[0];
            Assert.AreEqual(5, patterns.Count);

            
            //Just check the patterns to make sure they have been set based on the brushes (from the SVGFillReferenceValue) for each component.
            
            var names = new string[] { "text", "rect", "path", "polyline", "circle" };
            for (int i = 0; i < 5; i++)
            {
                var pattern = patterns[i] as PDFLinearShadingPattern;
                Assert.IsNotNull(pattern);
                AssertPattern(pattern, names[i] );
            }
           
        }

        [TestMethod()]
        public void SVGLinearGradientBrushesWithOpacity_Test()
        {
            Assert.Inconclusive("Need to check that opacity is being fed into the brush");
        }

        private static void AssertPattern(PDFLinearShadingPattern pattern, string forComponent)
        {
            var desc = pattern.Descriptor;
            Assert.IsNotNull(desc);
            Assert.AreEqual(0, desc.Angle, forComponent + " angle failed");
            Assert.AreEqual(2, desc.Colors.Count, forComponent + " colour count failed");
            Assert.AreEqual(StandardColors.Aqua, desc.Colors[0].Color, forComponent + " color 0 failed");
            Assert.AreEqual(0, desc.Colors[0].Distance, forComponent + " color 0 failed");
            Assert.AreEqual(StandardColors.Maroon, desc.Colors[1].Color, forComponent + " color 1 failed");
            Assert.AreEqual(1, desc.Colors[1].Distance, forComponent + " color 1 failed");
        }
        
        
        [TestMethod()]
        public void SVGLinearGradientWithTransform_Test()
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
            text.Fill = new SVGFillReferenceValue(null, "#2Color");
            text.FontFamily = FontSelector.Parse("sans-serif");
            text.FontSize = 14;
            text.FontWeight = FontWeights.Bold;
            text.X = 10;
            text.Y = 10;
            text.DominantBaseline = DominantBaseline.Hanging;
            text.Transform = new SVGTransformOperationSet(new TransformTranslateOperation(50, 0));
            text.Content.Add(new TextLiteral("Hello World"));
            
            svg.Contents.Add(text);
            
            SVGRect rect = new SVGRect();
            rect.X = 110;
            rect.Y = 10;
            rect.Width = 70;
            rect.Height = 70;
            rect.FillValue = new SVGFillReferenceValue(null, "#2Color");
            rect.Transform = new SVGTransformOperationSet(new TransformTranslateOperation(50, 0));
            svg.Contents.Add(rect);
            
            var gradient = new SVGLinearGradient();
            gradient.ID = "2Color";
            
            gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Maroon});
            svg.Contents.Add(gradient);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientBrushesWithTransform.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
            
            Assert.Inconclusive("Need to add a test where a transformation is applied to a shape and the gradient is also transformed");
        }

        /// <summary>
        ///A test to make sure the SVG is rendered correctly with horizontal gradients
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGLinearGradientWith2ColorHorizontal_Test()
        {
            

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGLinearGradients2ColorHorizontal.html", TestContext);
            var doc = Document.ParseDocument(path);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientOutput2ColorHorizontal.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count); //1 font, 1 inline blocks, 1 canvas xObj
            
            var canvXObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvXObj);
            Assert.IsNotNull(canvXObj.Renderer);
            Assert.IsInstanceOfType(canvXObj.Renderer.Owner, typeof(SVGCanvas));
            
            //Assert.AreEqual(svg, canvXObj.Container);
            
            Assert.AreEqual(1, canvXObj.Renderer.Resources.Types.Count);
            Assert.AreEqual(PDFResource.PatternResourceType, canvXObj.Renderer.Resources.Types[0].Type);
            
            var patterns = canvXObj.Renderer.Resources.Types[0];
            Assert.AreEqual(6, patterns.Count);
            
            //
            //2Color
            //
            
            var linear = patterns[0] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            var offset = linear.Start;
            Assert.AreEqual(new Point(10, 100), offset); //PDF Position within XObject Canvas
            var size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            var func2 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            // 2 Color Short
            //
            
            linear = patterns[1] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(100, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //3 stops 0, 0.4, 0.6 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3

            var func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(3, func3.Functions.Length);
            
            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[1].Bounds);
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 ColorPadded
            //
            
            linear = patterns[2] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(190, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //4 stops 0, 0.4, 0.6 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3
            
            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(3, func3.Functions.Length);
            
            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.4, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.6, func3.Boundaries[1].Bounds);
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 color repeat twice
            //
            
            linear = patterns[3] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(280, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //3 stops 0, 0.5, 0.5 and 1.0
            //Aqua -> Blue -> Aqua -> Blue
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            //Check bounds and ends

            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.5, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[1].Bounds);
            
            Assert.AreEqual(3, func3.Encodes.Length);
            
            
            Assert.AreEqual(3, func3.Functions.Length);
            Assert.AreEqual(0.0, func3.Encodes[0].Start);
            Assert.AreEqual(1.0, func3.Encodes[0].End);
            Assert.AreEqual(0.0, func3.Encodes[1].Start);
            Assert.AreEqual(1.0, func3.Encodes[1].End);
            Assert.AreEqual(0.0, func3.Encodes[2].Start);
            Assert.AreEqual(1.0, func3.Encodes[2].End);
            
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 color repeat padded
            //
            
            linear = patterns[4] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(370, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            //7 stops 0, 0.2, 0.3, 0.5, 0.5, 0.7, 0.8, 1.0
            //Aqua -> Aqua -> Blue -> Blue -> Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            //Check bounds and ends

            Assert.AreEqual(6, func3.Boundaries.Length);
            Assert.AreEqual(0.2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.3, func3.Boundaries[1].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[2].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[3].Bounds);
            Assert.AreEqual(0.7, func3.Boundaries[4].Bounds);
            Assert.AreEqual(0.8, func3.Boundaries[5].Bounds);
            
            Assert.AreEqual(7, func3.Encodes.Length);
            
            Assert.AreEqual(0.0, func3.Encodes[0].Start);
            Assert.AreEqual(1.0, func3.Encodes[0].End);
            Assert.AreEqual(0.0, func3.Encodes[1].Start);
            Assert.AreEqual(1.0, func3.Encodes[1].End);
            Assert.AreEqual(0.0, func3.Encodes[2].Start);
            Assert.AreEqual(1.0, func3.Encodes[2].End);
            Assert.AreEqual(0.0, func3.Encodes[3].Start);
            Assert.AreEqual(1.0, func3.Encodes[3].End);
            Assert.AreEqual(0.0, func3.Encodes[4].Start);
            Assert.AreEqual(1.0, func3.Encodes[4].End);
            Assert.AreEqual(0.0, func3.Encodes[5].Start);
            Assert.AreEqual(1.0, func3.Encodes[5].End);
            Assert.AreEqual(0.0, func3.Encodes[6].Start);
            Assert.AreEqual(1.0, func3.Encodes[6].End);
            
            Assert.AreEqual(7, func3.Functions.Length);
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[3] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            
            func2 = func3.Functions[4] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[5] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[6] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            
            //
            //2 color repeat padded short
            //
            
            linear = patterns[5] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(460, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            
            //4 repeats of 4 gradients + 3 transitions of zero size.
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            Assert.AreEqual(19, func3.Functions.Length);
            
             
        }

        
        /// <summary>
        ///A test to make sure the SVG gradients are rendered correctly at 45%
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGLinearGradientWith2ColorTurning_Test()
        {
            

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGLinearGradients2ColorTurning.html", TestContext);
            var doc = Document.ParseDocument(path);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientOutput2ColorTurning.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count); //1 font, 1 inline blocks, 1 canvas xObj
            
            var canvXObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvXObj);
            Assert.IsNotNull(canvXObj.Renderer);
            Assert.IsInstanceOfType(canvXObj.Renderer.Owner, typeof(SVGCanvas));
            
            //Assert.AreEqual(svg, canvXObj.Container);
            
            Assert.AreEqual(1, canvXObj.Renderer.Resources.Types.Count);
            Assert.AreEqual(PDFResource.PatternResourceType, canvXObj.Renderer.Resources.Types[0].Type);
            
            var patterns = canvXObj.Renderer.Resources.Types[0];
            Assert.AreEqual(16, patterns.Count);

            var blocks = new[]
            {
                new
                {
                    x = 10, y = 450, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 45.0,
                    coords = new double[] { 10.0, 450.0, 90.0, 370.0 }, name = "2DownRight"
                },
                new
                {
                    x = 190, y = 450, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 90.0,
                    coords = new double[] { 190.0, 450.0, 190.0, 370.0 }, name = "2Down"
                },
                
                new
                {
                    x = 370, y = 450, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 135.0,
                    coords = new double[] { 450.0, 450.0, 370.0, 370.0 }, name = "2DownLeft"
                },

                new
                {
                    x = 370, y = 270, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 180.0,
                    coords = new double[] { 450.0, 190.0, 370.0, 190.0 }, name = "2Left"
                },
                
                new
                {
                    x = 370, y = 90, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 225.0,
                    coords = new double[] { 450.0, 10.0, 370.0, 90.0 }, name = "2UpLeft"
                },
                
                new
                {
                    x = 190, y = 90, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 270.0,
                    coords = new double[] { 190.0, 10.0, 190.0, 90.0 }, name = "2Up"
                },
                
                new
                {
                    x = 10, y = 90, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 315.0,
                    coords = new double[] { 10.0, 10.0, 90.0, 90.0 }, name = "2UpRight"
                },
                
                new
                {
                    x = 10, y = 270, width = 80, height = -80, count = 2, start = 0.0, end = 1.0, angle = 0.0,
                    coords = new double[] { 10.0, 190.0, 90.0, 190.0 }, name = "2Right"
                },
            };
            
            //
            // 2Color Gradients
            //

            for (var i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                var linear = patterns[i] as PDFLinearShadingPattern;
                Assert.IsNotNull(linear);

                var offset = linear.Start;
                Assert.AreEqual(block.x, offset.X.PointsValue, "X failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(block.y, offset.Y.PointsValue, "Y failed for " + block.name + " at index " + i.ToString());
                
                var size = linear.Size;
                Assert.AreEqual(block.width, size.Width.PointsValue, "Width failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(block.height, size.Height.PointsValue, "Height failed for " + block.name + " at index " + i.ToString());
                
                //The coodinates form 2 points that create the path the linear gradient will follow
                var coords = linear.Descriptor.GetCoordsForBounds(offset, size);
                Assert.AreEqual(4, coords.Length);
                Assert.AreEqual(block.coords[0], coords[0],  "Coordinate 0 failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(block.coords[1], coords[1],  "Coordinate 1 failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(block.coords[2], coords[2],  "Coordinate 2 failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(block.coords[3], coords[3],  "Coordinate 3 failed for " + block.name + " at index " + i.ToString());
                
                var desc = linear.Descriptor;
                Assert.AreEqual(block.angle, desc.Angle, "Angle failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(GradientType.Linear, desc.GradientType, "Type failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(false, desc.Repeating, "Repeating failed for " + block.name + " at index " + i.ToString());

                var func = linear.Descriptor.GetGradientFunction(offset, size);
                Assert.IsNotNull(func);
                var func2 = func as PDFGradientFunction2;
                Assert.IsNotNull(func2, "Function failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(0.0, func2.DomainStart, "Domain End failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(1.0, func2.DomainEnd, "Domain Start failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(StandardColors.Aqua, func2.ColorZero, "Color Zero failed for " + block.name + " at index " + i.ToString());
                Assert.AreEqual(StandardColors.Blue, func2.ColorOne, "Color One failed for " + block.name + " at index " + i.ToString());

            }

            //
            // 3 Color Half angle gradients
            //
            
            blocks = [
                new
                {
                    x = 10, y = 360, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 18.3,
                    coords = new double[] { 10.0, 360.0, 106.0, 328.0 }, name = "3HalfDownRight"
                },
                new
                {
                    x = 100, y = 450, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 71.7,
                    coords = new double[] { 100.0, 450.0, 132.0, 354.0 }, name = "3HalfRightDown"
                },
                
                new
                {
                    x = 280, y = 450, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 108.3,
                    coords = new double[] { 360.0, 450.0, 328.0, 354.0 }, name = "3HalfLeftDown"
                },

                new
                {
                    x = 370, y = 360, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 161.7,
                    coords = new double[] { 450.0, 360.0, 354.0, 328.0 }, name = "3HalfDownLeft"
                },
                
                new
                {
                    x = 370, y = 180, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 198.3,
                    coords = new double[] { 450.0, 100.0, 354.0, 132.0 }, name = "3HalfUpLeft"
                },
                
                new
                {
                    x = 280, y = 90, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 251.7,
                    coords = new double[] { 360.0, 10.0, 328.0, 106.0 }, name = "3HalfLeftUp"
                },
                
                new
                {
                    x = 100, y = 90, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 288.3,
                    coords = new double[] { 100.0, 10.0, 132.0, 106.0 }, name = "3HalfRightUp"
                },
                
                new
                {
                    x = 10, y = 180, width = 80, height = -80, count = 3, start = 0.0, end = 1.0, angle = 341.7,
                    coords = new double[] { 10.0, 100.0, 106.0, 132.0 }, name = "3HalfUpRight"
                }
            ];
            
            for (var i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                var patternIndex = i + 8;
                var linear = patterns[patternIndex] as PDFLinearShadingPattern;
                Assert.IsNotNull(linear);

                var offset = linear.Start;
                Assert.AreEqual(block.x, offset.X.PointsValue, "X failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(block.y, offset.Y.PointsValue, "Y failed for " + block.name + " at index " + patternIndex.ToString());
                
                var size = linear.Size;
                Assert.AreEqual(block.width, size.Width.PointsValue, "Width failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(block.height, size.Height.PointsValue, "Height failed for " + block.name + " at index " + patternIndex.ToString());
                
                //The coodinates form 2 points that create the path the linear gradient will follow
                var coords = linear.Descriptor.GetCoordsForBounds(offset, size);
                Assert.AreEqual(4, coords.Length);
                Assert.AreEqual(block.coords[0], Math.Round(coords[0]),  "Coordinate 0 failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(block.coords[1], Math.Round(coords[1]),  "Coordinate 1 failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(block.coords[2], Math.Round(coords[2]),  "Coordinate 2 failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(block.coords[3], Math.Round(coords[3]),  "Coordinate 3 failed for " + block.name + " at index " + patternIndex.ToString());
                
                var desc = linear.Descriptor;
                Assert.AreEqual(block.angle, Math.Round(desc.Angle, 1), "Angle failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(GradientType.Linear, desc.GradientType, "Type failed for " + block.name + " at index " + patternIndex.ToString());
                Assert.AreEqual(false, desc.Repeating, "Repeating failed for " + block.name + " at index " + patternIndex.ToString());

                var func = linear.Descriptor.GetGradientFunction(offset, size);
                Assert.IsNotNull(func);
                var func3 = func as PDFGradientFunction3;
                Assert.IsNotNull(func3);
                Assert.AreEqual(2, func3.Functions.Length);

                
                for (var f = 0; f < func3.Functions.Length; f++)
                {
                    var func2 = func3.Functions[f] as PDFGradientFunction2;
                    Assert.IsNotNull(func2,
                        "Function failed for " + block.name + " at index " + patternIndex.ToString());
                    Assert.AreEqual(0.0, func2.DomainStart,
                        "Domain End failed for " + block.name + " at index " + patternIndex.ToString());
                    Assert.AreEqual(1.0, func2.DomainEnd,
                        "Domain Start failed for " + block.name + " at index " + patternIndex.ToString());
                    Assert.AreEqual(StandardColors.Aqua, func2.ColorZero,
                        "Color Zero failed for " + block.name + " at index " + patternIndex.ToString());
                    if (f == 0)
                        Assert.AreEqual(StandardColors.Aqua, func2.ColorOne,
                            "Color One failed for " + block.name + " at index " + patternIndex.ToString());
                    else
                        Assert.AreEqual(StandardColors.Maroon, func2.ColorOne,
                            "Color One failed for " + block.name + " at index " + patternIndex.ToString());
                }
                
                Assert.AreEqual(1, func3.Boundaries.Length);
                Assert.AreEqual(0.3, Math.Round(func3.Boundaries[0].Bounds, 1));

            }

             
        }
        
        /// <summary>
        ///A test to make sure the SVG gradients are rendered correctly at 45%
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGLinearGradientWith2Color45Degree_Test()
        {
            

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGLinearGradients2Color45Degree.html", TestContext);
            var doc = Document.ParseDocument(path);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientOutput2Color45Degrees.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count); //1 font, 1 inline blocks, 1 canvas xObj
            
            var canvXObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvXObj);
            Assert.IsNotNull(canvXObj.Renderer);
            Assert.IsInstanceOfType(canvXObj.Renderer.Owner, typeof(SVGCanvas));
            
            //Assert.AreEqual(svg, canvXObj.Container);
            
            Assert.AreEqual(1, canvXObj.Renderer.Resources.Types.Count);
            Assert.AreEqual(PDFResource.PatternResourceType, canvXObj.Renderer.Resources.Types[0].Type);
            
            var patterns = canvXObj.Renderer.Resources.Types[0];
            Assert.AreEqual(6, patterns.Count);
            
            //
            //2Color
            //
            
            var linear = patterns[0] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            var offset = linear.Start;
            Assert.AreEqual(new Point(10, 100), offset); //PDF Position within XObject Canvas
            var size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            var func2 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            // 2 Color Short
            //
            
            linear = patterns[1] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(100, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //4 stops 0, 0.4, 0.6 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3

            var func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(4, func3.Functions.Length);
            
            Assert.AreEqual(3, func3.Boundaries.Length);
            Assert.AreEqual(0.28, Math.Round(func3.Boundaries[0].Bounds, 2));
            Assert.AreEqual(0.58, Math.Round(func3.Boundaries[1].Bounds, 2));
            Assert.AreEqual(0.71, Math.Round(func3.Boundaries[2].Bounds, 2));
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 ColorPadded
            //
            
            linear = patterns[2] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(190, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //4 stops 0, 0.4, 0.6 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3
            
            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(3, func3.Functions.Length);
            
            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.4, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.6, func3.Boundaries[1].Bounds);
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 color repeat twice
            //
            
            linear = patterns[3] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(280, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //3 stops 0, 0.5, 0.5 and 1.0
            //Aqua -> Blue -> Aqua -> Blue
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            //Check bounds and ends

            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.5, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[1].Bounds);
            
            Assert.AreEqual(3, func3.Encodes.Length);
            
            
            Assert.AreEqual(3, func3.Functions.Length);
            Assert.AreEqual(0.0, func3.Encodes[0].Start);
            Assert.AreEqual(1.0, func3.Encodes[0].End);
            Assert.AreEqual(0.0, func3.Encodes[1].Start);
            Assert.AreEqual(1.0, func3.Encodes[1].End);
            Assert.AreEqual(0.0, func3.Encodes[2].Start);
            Assert.AreEqual(1.0, func3.Encodes[2].End);
            
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //
            //2 color repeat padded
            //
            
            linear = patterns[4] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(370, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            //7 stops 0, 0.2, 0.3, 0.5, 0.5, 0.7, 0.8, 1.0
            //Aqua -> Aqua -> Blue -> Blue -> Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            //Check bounds and ends

            Assert.AreEqual(6, func3.Boundaries.Length);
            Assert.AreEqual(0.2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.3, func3.Boundaries[1].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[2].Bounds);
            Assert.AreEqual(0.5, func3.Boundaries[3].Bounds);
            Assert.AreEqual(0.7, func3.Boundaries[4].Bounds);
            Assert.AreEqual(0.8, func3.Boundaries[5].Bounds);
            
            Assert.AreEqual(7, func3.Encodes.Length);
            
            Assert.AreEqual(0.0, func3.Encodes[0].Start);
            Assert.AreEqual(1.0, func3.Encodes[0].End);
            Assert.AreEqual(0.0, func3.Encodes[1].Start);
            Assert.AreEqual(1.0, func3.Encodes[1].End);
            Assert.AreEqual(0.0, func3.Encodes[2].Start);
            Assert.AreEqual(1.0, func3.Encodes[2].End);
            Assert.AreEqual(0.0, func3.Encodes[3].Start);
            Assert.AreEqual(1.0, func3.Encodes[3].End);
            Assert.AreEqual(0.0, func3.Encodes[4].Start);
            Assert.AreEqual(1.0, func3.Encodes[4].End);
            Assert.AreEqual(0.0, func3.Encodes[5].Start);
            Assert.AreEqual(1.0, func3.Encodes[5].End);
            Assert.AreEqual(0.0, func3.Encodes[6].Start);
            Assert.AreEqual(1.0, func3.Encodes[6].End);
            
            Assert.AreEqual(7, func3.Functions.Length);
            
            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[3] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            
            func2 = func3.Functions[4] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorOne);
            
            func2 = func3.Functions[5] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            func2 = func3.Functions[6] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Blue, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            
            //
            //2 color repeat padded short
            //
            
            linear = patterns[5] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            Assert.AreEqual(new Point(460, 100), offset); //PDF Position within XObject Canvas
            size = linear.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            
            //4 repeats of 4 gradients + 3 transitions of zero size.
            //Wrapped in a Function3
            

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            Assert.AreEqual(15, func3.Functions.Length);
            
             
        }

    }
}
