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
            rect.FillValue = new SVGFillReferenceValue(null, "#star");
            svg.Contents.Add(rect);
            
            SVGPath svgPath = new SVGPath();
            svgPath.PathData = GraphicsPath.Parse(@"M 200,30
            A 20,20 0 0 1 240,30
            A 20,20 0,0,1 280,30
            Q 280,60 240,90
            Q 200,60 200,30 z");
            svgPath.Fill = new SVGFillReferenceValue(null, "#star");
            svg.Contents.Add(svgPath);
            
            SVGPolygon polyline = new SVGPolygon();
            polyline.Points.Add(new Point(300, 100));
            polyline.Points.Add(new Point(350, 70));
            polyline.Points.Add(new Point(400, 10));
            polyline.Points.Add(new Point(350, 30));
            polyline.Fill = new SVGFillReferenceValue(null, "#star");
            
            svg.Contents.Add(polyline);
            
            
            SVGCircle circle = new SVGCircle();
            circle.Radius = 40;
            circle.CentreX = 450;
            circle.CenterY = 50;
            circle.Fill = new SVGFillReferenceValue(null, "#star");
            svg.Contents.Add(circle);
            
            var pattern = new SVGPattern();
            pattern.ID = "star";
            
            //gradient.Stops.Add(new SVGGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            //gradient.Stops.Add(new SVGGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Maroon});
            svg.Contents.Add(pattern);
            
            using(var stream = DocStreams.GetOutputStream("SVG_PatternFillBrushes.pdf"))
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
            
            
           
        }

        

        private static void AssertPattern(PDFRadialShadingPattern pattern, string forComponent)
        {
            var desc = pattern.Descriptor;
            Assert.IsNotNull(desc);
           // Assert.AreEqual(0, desc.Angle, forComponent + " angle failed");
            Assert.AreEqual(2, desc.Colors.Count, forComponent + " colour count failed");
            Assert.AreEqual(StandardColors.Aqua, desc.Colors[0].Color, forComponent + " color 0 failed");
            Assert.AreEqual(0, desc.Colors[0].Distance, forComponent + " color 0 failed");
            Assert.AreEqual(StandardColors.Maroon, desc.Colors[1].Color, forComponent + " color 1 failed");
            Assert.AreEqual(1, desc.Colors[1].Distance, forComponent + " color 1 failed");
        }
        
        [TestMethod()]
        public void SVGRadialGradientBrushesWithOpacity_Test()
        {
            Assert.Inconclusive("Need to check that opacity is being fed into the brush");
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
            
            gradient.Stops.Add(new SVGGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            gradient.Stops.Add(new SVGGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Maroon});
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
        public void SVGRadialGradientWith2Color_Test()
        {
            

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGRadialGradients2Color.html", TestContext);
            var doc = Document.ParseDocument(path);
            
            using(var stream = DocStreams.GetOutputStream("SVG_RadialGradients2Color.pdf"))
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
            
            var radial = patterns[0] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            var offset = radial.Start;
            Assert.AreEqual(new Point(10, 100), offset); //PDF Position within XObject Canvas
            var size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            var gold = new Color(255, 215, 0);
            var red = new Color(255, 0, 0);
            
            var func2 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(red, func2.ColorOne);

            var bounds = radial.Descriptor.GetCoordsForBounds(new Point(100, 100), new Size(80, -90));
            Assert.AreEqual(0.0, bounds[2]);
            Assert.AreEqual(45.0, Math.Round(bounds[5])); //Default radius is 50%
            
            //
            // 2 Color Offset short
            //
            
            radial = patterns[1] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            offset = radial.Start;
            Assert.AreEqual(new Point(100, 100), offset); //PDF Position within XObject Canvas
            size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            func2 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(red, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            bounds = radial.Descriptor.GetCoordsForBounds(new Point(100, 100), new Size(80, -90));
            Assert.AreEqual(0.0, bounds[2]);
            Assert.AreEqual(63.0, Math.Round(bounds[5]));
            
            
            
            //
            // 2 Color Repeat
            //
            
            radial = patterns[2] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            offset = radial.Start;
            Assert.AreEqual(new Point(190, 100), offset); //PDF Position within XObject Canvas
            size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //3 stops 0, 0.4, 0.6 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3
            
            var func3 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(5, func3.Functions.Length);
            
            Assert.AreEqual(4, func3.Boundaries.Length);
            Assert.AreEqual(0.75/2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(1.0/2.0, func3.Boundaries[1].Bounds);
            Assert.AreEqual(1.0/2.0, func3.Boundaries[2].Bounds);
            Assert.AreEqual(0.5 + (0.75/2.0), func3.Boundaries[3].Bounds);

            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(red, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(red, func2.ColorOne);
            
            func2 = func3.Functions[3] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(red, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[4] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            bounds = radial.Descriptor.GetCoordsForBounds(new Point(100, 100), new Size(80, -90));
            Assert.AreEqual(0.0, bounds[2]);
            Assert.AreEqual(90.0, Math.Round(bounds[5]));

            
            
            
            //
            //2 Color Repeat 4 stops
            //
            
            radial = patterns[3] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            offset = radial.Start;
            Assert.AreEqual(new Point(280, 100), offset); //PDF Position within XObject Canvas
            size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas
            
            //19 stops - 4 repeats of 4 colors with 3 padding between
            //Wrapped in a Function3
            
            func3 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(19, func3.Functions.Length);
            
            Assert.AreEqual(18, func3.Boundaries.Length);
            
            bounds = radial.Descriptor.GetCoordsForBounds(new Point(100, 100), new Size(80, -90));
            Assert.AreEqual(0.0, bounds[2]);
            Assert.AreEqual(90.0, Math.Round(bounds[5]));

            
            
            //
            //2 color reflect twice
            //
            
            
            radial = patterns[4] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            offset = radial.Start;
            Assert.AreEqual(new Point(370, 100), offset); //PDF Position within XObject Canvas
            size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            
            func3 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            //red, gold, gold + gold, gold, red
            
            Assert.AreEqual(6, func3.Functions.Length);
            
            Assert.AreEqual(5, func3.Boundaries.Length);
            Assert.AreEqual(0.75/2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(1.0/2.0, func3.Boundaries[1].Bounds);
            Assert.AreEqual(1.0/2.0, func3.Boundaries[2].Bounds);
            Assert.AreEqual(1 - (0.75/2.0), func3.Boundaries[3].Bounds);
            Assert.AreEqual(1.0, func3.Boundaries[4].Bounds);
            

            func2 = func3.Functions[0] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(red, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[1] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[2] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[3] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(gold, func2.ColorOne);
            
            func2 = func3.Functions[4] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(gold, func2.ColorZero);
            Assert.AreEqual(red, func2.ColorOne);
            
            func2 = func3.Functions[5] as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(red, func2.ColorZero);
            Assert.AreEqual(red, func2.ColorOne);
            
            bounds = radial.Descriptor.GetCoordsForBounds(new Point(100, 100), new Size(80, -90));
            Assert.AreEqual(0.0, bounds[2]);
            Assert.AreEqual(90.0, Math.Round(bounds[5]));

            
            
            
            //
            //2 color repeat padded short
            //
            
            radial = patterns[5] as PDFRadialShadingPattern;
            Assert.IsNotNull(radial);
            
            offset = radial.Start;
            Assert.AreEqual(new Point(460, 100), offset); //PDF Position within XObject Canvas
            size = radial.Size;
            Assert.AreEqual(new Size(80, -90), size); //PDF Size within XObject Canvas

            
            //4 repeats of 4 gradients + 3 transitions of zero size.
            //Wrapped in a Function3
            

            func3 = radial.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            Assert.AreEqual(21, func3.Functions.Length);
            
             
        }

        

    }
}
