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
            
            // var text = new SVGText();
            // text.Fill = new SVGFillReferenceValue(null, "#star");
            // text.FontFamily = FontSelector.Parse("sans-serif");
            // text.FontSize = 14;
            // text.FontWeight = FontWeights.Bold;
            // text.X = 10;
            // text.Y = 10;
            // text.DominantBaseline = DominantBaseline.Hanging;
            // text.Content.Add(new TextLiteral("Hello World"));
            //
            // svg.Contents.Add(text);
            
            SVGRect rect = new SVGRect();
            rect.X = 100;
            rect.Y = 10;
            rect.Width = 70;
            rect.Height = 70;
            rect.FillValue = new SVGFillReferenceValue(null, "#star");
            svg.Contents.Add(rect);
            
            // SVGPath svgPath = new SVGPath();
            // svgPath.PathData = GraphicsPath.Parse(@"M 200,30
            // A 20,20 0 0 1 240,30
            // A 20,20 0,0,1 280,30
            // Q 280,60 240,90
            // Q 200,60 200,30 z");
            // svgPath.Fill = new SVGFillReferenceValue(null, "#star");
            // svg.Contents.Add(svgPath);
            //
            // SVGPolygon polyline = new SVGPolygon();
            // polyline.Points.Add(new Point(300, 100));
            // polyline.Points.Add(new Point(350, 70));
            // polyline.Points.Add(new Point(400, 10));
            // polyline.Points.Add(new Point(350, 30));
            // polyline.Fill = new SVGFillReferenceValue(null, "#star");
            //
            // svg.Contents.Add(polyline);
            //
            //
            // SVGCircle circle = new SVGCircle();
            // circle.Radius = 40;
            // circle.CentreX = 450;
            // circle.CenterY = 50;
            // circle.Fill = new SVGFillReferenceValue(null, "#star");
            // svg.Contents.Add(circle);
            //
            var pattern = new SVGPattern();
            pattern.ID = "star";

            pattern.Contents.Add(new SVGCircle()
                { CentreX = 10, CenterY = 10, Radius = 4, Fill = new SVGFillColorValue(StandardColors.Red, "Red") });
            
            //gradient.Stops.Add(new SVGGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            //gradient.Stops.Add(new SVGGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Maroon});
            svg.Contents.Add(pattern);
            
            using(var stream = DocStreams.GetOutputStream("SVG_PatternFillBrushes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
            
            Assert.AreEqual(2, doc.SharedResources.Count);
            var xObj = doc.SharedResources[1] as PDFLayoutXObjectResource;
            Assert.IsNotNull(xObj);
            
            Assert.AreEqual(0, xObj.Renderer.Resources.Types.Count);
            //Assert.AreEqual(PDFResource.PatternResourceType, xObj.Renderer.Resources.Types[0].Type);
            // Assert.AreEqual(PDFResource.FontDefnResourceType, xObj.Renderer.Resources.Types[1].Type);

            //var patterns = xObj.Renderer.Resources.Types[0];
            //Assert.AreEqual(5, patterns.Count);

            
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
        
        
        

    }
}
