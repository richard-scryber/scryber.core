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
            pattern.PatternWidth = 15;
            pattern.PatternHeight = 10;
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

            var layoutKey = pattern.UniqueID + "_layout";
            Assert.AreEqual(layoutKey, PDFPatternLayoutResource.GetLayoutResourceKey(pattern.UniqueID));
            
            Assert.AreEqual(5, doc.SharedResources.Count);
            
            var tile = doc.SharedResources[1] as PDFGraphicTilingPattern;
            Assert.IsNotNull(tile);
            Assert.AreEqual(PDFResource.PatternResourceType, tile.ResourceType);
            Assert.AreEqual(pattern.UniqueID, tile.ResourceKey);
            Assert.AreEqual(15, tile.Step.Width.PointsValue);
            Assert.AreEqual(10, tile.Step.Height.PointsValue);
            Assert.AreEqual(layoutKey, tile.PatternLayout.ResourceKey);
            Assert.AreEqual(PatternTilingType.NoDistortion, tile.TilingType);
            Assert.AreEqual(PatternPaintType.ColoredTile, tile.PaintType);
            Assert.IsNotNull(tile.GraphicCanvas);
            
            var layout = doc.SharedResources[3] as PDFPatternLayoutResource;
            Assert.IsNotNull(layout);
            Assert.AreEqual(PDFResource.XObjectResourceType, layout.ResourceType);
            Assert.AreEqual(layoutKey, layout.ResourceKey);
            Assert.AreEqual(tile.GraphicCanvas, layout.Container);
            Assert.AreEqual(pattern, layout.Pattern);
            
            
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
            var descriptorKey = Scryber.PDF.Resources.PDFPatternLayoutResource.GetLayoutResourceKey(pattern.UniqueID);
            Assert.AreEqual(descriptorKey, patternBrush.DescriptorKey);
            Assert.AreEqual(layoutKey, patternBrush.LayoutKey);
            
        }


        /// <summary>
        ///A test to make sure the SVG is rendered correctly with horizontal gradients
        ///</summary>
        [TestMethod()]
        [TestCategory("SVG")]
        public void SVGSimplePatterns_Test()
        {


            var path = DocStreams.AssertGetContentPath(
                "../../Scryber.UnitTest/Content/SVG/SVGSimplePatterns.html", TestContext);
            var doc = Document.ParseDocument(path);

            using (var stream = DocStreams.GetOutputStream("SVG_SimplePatterns.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count); //1 font, 1 inline blocks, 1 canvas xObj

            var canvXObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            Assert.IsNotNull(canvXObj);
            Assert.IsNotNull(canvXObj.Renderer);
            Assert.IsInstanceOfType(canvXObj.Renderer.Owner, typeof(SVGCanvas));

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
