using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Svg;
using Scryber.Svg.Components;
using Scryber.Html.Components;
using System.CodeDom;
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
            
            
            var svg = new SVGCanvas() { Width = 110, Height = 110 };
            svg.BackgroundColor = Color.Parse("#AAA");
            page.Contents.Add(svg);
            
            var text = new SVGText();
            text.Fill = new SVGFillReferenceValue(null, "#2Color");
            text.FontFamily = FontSelector.Parse("sans-serif");
            text.FontSize = 10;
            text.FontWeight = FontWeights.Bold;
            text.X = 10;
            text.Y = 10;
            text.Content.Add(new TextLiteral("Hello World"));
            svg.Contents.Add(text);
            
            SVGRect rect = new SVGRect();
            rect.X = 20;
            rect.Y = 20;
            rect.Width = 70;
            rect.Height = 70;
            rect.FillValue = new SVGFillReferenceValue(null, "#2Color");
            svg.Contents.Add(rect);
            
            var gradient = new SVGLinearGradient();
            gradient.ID = "2Color";
            
            gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Blue});
            svg.Contents.Add(gradient);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradientBrushes.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }
        }

        /// <summary>
        ///A test to make sure the SVG is rendered as an XObject in the PDF
        ///</summary>
        [TestMethod()]
        [TestCategory("Common")]
        public void SVGLinearGradient2Color_Test()
        {
            // var doc = new Document();
            // var page = new Page();
            // page.Style.Font.FontFamily = FontSelector.Parse("Serif");
            // page.Padding = 8;
            // doc.Pages.Add(page);
            //
            // var p = new HTMLParagraph();
            // p.Contents.Add("2 Colour Linear Gradients");
            // page.Contents.Add(p);
            //
            // var svg = new SVGCanvas() { Width = 410, Height = 110 };
            // svg.BackgroundColor = Color.Parse("#AAA");
            //
            // page.Contents.Add(svg);
            //
            // var rect = new SVGRect() { X = 10, Y = 10, Width = 90, Height = 90, FillValue = new SVGFillReferenceValue(null, "#2Color") };
            // svg.Contents.Add(rect);
            //
            // var gradient = new SVGLinearGradient();
            // gradient.ID = "2Color";
            //
            // gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(0), StopColor = StandardColors.Aqua});
            // gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(100), StopColor = StandardColors.Blue});
            // svg.Contents.Add(gradient);
            //
            // rect = new SVGRect()
            // {
            //     X = 110, Y = 10, Width = 90, Height = 90, FillValue = new SVGFillReferenceValue(null, "#2ColorPadded")
            // };
            // svg.Contents.Add(rect);
            //
            // gradient = new SVGLinearGradient();
            // gradient.ID = "2ColorPadded";
            // gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(20), StopColor = StandardColors.Aqua});
            // gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(80), StopColor = StandardColors.Blue});
            // svg.Contents.Add(gradient);
            //
            //
            // rect = new SVGRect()
            // {
            //     X = 210, Y = 10, Width = 90, Height = 90, FillValue = new SVGFillReferenceValue(null, "#2ColorRepeat")
            // };
            // svg.Contents.Add(rect);
            //
            // gradient = new SVGLinearGradient();
            // gradient.ID = "2ColorRepeat";
            //
            // gradient.X1 = 0;
            // gradient.X2 = 0.5;
            // gradient.Y1 = 0;
            // gradient.Y2 = 0;
            // gradient.SpreadMode = GradientSpreadMode.Repeat;
            // gradient.Stops.Add(new SVGLinearGradientStop() { Offset = Unit.Percent(20), StopColor = StandardColors.Aqua});
            // gradient.Stops.Add(new SVGLinearGradientStop() {Offset = Unit.Percent(80), StopColor = StandardColors.Blue});
            // svg.Contents.Add(gradient);

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGLinearGradients.html", TestContext);
            var doc = Document.ParseDocument(path);
            
            using(var stream = DocStreams.GetOutputStream("SVG_LinearGradient2ColorOutput.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(3, doc.SharedResources.Count);
            var canvXObj = doc.SharedResources[2] as PDFLayoutXObjectResource;
            
            Assert.IsNotNull(canvXObj);
            //Assert.AreEqual(svg, canvXObj.Container);
            
            Assert.AreEqual(1, canvXObj.Renderer.Resources.Types.Count);
            Assert.AreEqual(PDFResource.PatternResourceType, canvXObj.Renderer.Resources.Types[0].Type);
            
            var patterns = canvXObj.Renderer.Resources.Types[0];
            Assert.AreEqual(3, patterns.Count);
            
            //2Color
            var linear = patterns[0] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            var offset = linear.Start;
            var size = linear.Size;

            var func2 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction2;
            Assert.IsNotNull(func2);
            Assert.AreEqual(0.0, func2.DomainStart);
            Assert.AreEqual(1.0, func2.DomainEnd);
            Assert.AreEqual(StandardColors.Aqua, func2.ColorZero);
            Assert.AreEqual(StandardColors.Blue, func2.ColorOne);
            
            //2 ColorPadded
            linear = patterns[1] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            size = linear.Size;
            
            //4 stops 0, 0.2, 0.8 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3

            var func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            Assert.IsNotNull(func3);
            Assert.AreEqual(0.0, func3.DomainStart);
            Assert.AreEqual(1.0, func3.DomainEnd);
            
            Assert.AreEqual(3, func3.Functions.Length);
            
            Assert.AreEqual(2, func3.Boundaries.Length);
            Assert.AreEqual(0.2, func3.Boundaries[0].Bounds);
            Assert.AreEqual(0.8, func3.Boundaries[1].Bounds);
            
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
            
            //2 color repeat
            
            linear = patterns[2] as PDFLinearShadingPattern;
            Assert.IsNotNull(linear);
            
            offset = linear.Start;
            size = linear.Size;
            
            //4 stops 0, 0.2, 0.8 and 1.0
            //Aqua -> Aqua -> Blue -> Blue
            //Wrapped in a Function3

            func3 = linear.Descriptor.GetGradientFunction(offset, size) as PDFGradientFunction3;
            
            Assert.Inconclusive("Not tested - need to check the layout, render bounds, and XObject reference in the document");

        }


    }
}
