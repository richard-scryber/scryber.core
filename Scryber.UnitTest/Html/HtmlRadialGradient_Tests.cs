using System;
using System.IO;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF;
using Scryber.PDF.Layout;
using System.Linq;


namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlRadialGradient_Tests
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
        
        
        [TestMethod]
        public void RadialGradientParsingTest()
        {
            var gradient = "radial-gradient(red)";
            var pcntCentre = new Point(Unit.Percent(50), Unit.Percent(50));
            var pcntRadius = new Unit(50, PageUnits.Percent);
            
            var parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(1, parsed.Colors.Count);
            var color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(circle, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(Closest-Side, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(new Point(Unit.Percent(50), Unit.Percent(50)), parsed.StartCenter);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.ClosestSide, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(closest-corner, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.ClosestCorner, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(FartHest-Side, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(Farthest-Corner, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(Circle Farthest-Corner, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.AreEqual(pcntCentre, parsed.StartCenter);
            Assert.AreEqual(pcntCentre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(at 400pt 150pt, red, green)";
            var centre = new Point(400, 150);
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(Circle at 400pt 150pt, red, green)";
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(2, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(circle closest-corner at 400pt top, red, green, yellow)";
            centre = new Point(400, Unit.Percent(0));
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.ClosestCorner, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(3, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[2];
            Assert.AreEqual(StandardColors.Yellow, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            
            gradient = "radial-gradient(at top left, red, green, yellow)";
            centre = new Point(Unit.Percent(0), Unit.Percent(0));
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(3, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[2];
            Assert.AreEqual(StandardColors.Yellow, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(at right bottom, red, green, yellow)";
            centre = new Point(Unit.Percent(100), Unit.Percent(100));
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(3, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[2];
            Assert.AreEqual(StandardColors.Yellow, color.Color);
            Assert.IsFalse(color.Distance.HasValue);
            Assert.IsFalse(color.Opacity.HasValue);
            
            gradient = "radial-gradient(at right bottom, red 10%, green, yellow 90%)";
            centre = new Point(Unit.Percent(100), Unit.Percent(100));
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);
            Assert.IsTrue(parsed.EndCenter.HasValue);
            Assert.AreEqual(centre, parsed.EndCenter);
            Assert.IsFalse(parsed.StartRadius.HasValue);
            Assert.IsFalse(parsed.EndRadius.HasValue);
            
            Assert.AreEqual(5, parsed.Colors.Count);
            
            color = parsed.Colors[0];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsTrue(color.Distance.HasValue);
            Assert.AreEqual(color.Distance, 0.0);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            
            color = parsed.Colors[1];
            Assert.AreEqual(StandardColors.Red, color.Color);
            Assert.IsTrue(color.Distance.HasValue);
            Assert.AreEqual(color.Distance, 0.1);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[2];
            Assert.AreEqual(StandardColors.Green, color.Color);
            Assert.IsTrue(color.Distance.HasValue);
            Assert.AreEqual(color.Distance, 0.5);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[3];
            Assert.AreEqual(StandardColors.Yellow, color.Color);
            Assert.IsTrue(color.Distance.HasValue);
            Assert.AreEqual(color.Distance, 0.9);
            Assert.IsFalse(color.Opacity.HasValue);
            
            color = parsed.Colors[4];
            Assert.AreEqual(StandardColors.Yellow, color.Color);
            Assert.IsTrue(color.Distance.HasValue);
            Assert.AreEqual(color.Distance, 1.0);
            Assert.IsFalse(color.Opacity.HasValue);
            
            
        }


        private static void AssertCoordinates(double[] coords, double fx, double fy, double fr, double cx, double cy,
            double cr, int testIndex, string expected)
        {
            Assert.IsNotNull(coords);
            Assert.AreEqual(6, coords.Length);
            
            Assert.AreEqual(fx, coords[0], "First X coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
        }

        /// <summary>
        /// Checks that the Co-ordinates returned for a parsed radial gradient are correct.
        /// </summary>
        /// <remarks>The coordinates are returned as an array of 6 double precision numbers,
        /// fx, fx, fr for the starting centre and raidus or the gradient,
        /// and then cx, cy, cr for the outer centre and radius for the ending circle of the gradient</remarks>
        [TestMethod]
        public void RadialGradientCoordinatesTest()
        {
            var grad = "radial-gradient(red, green)";
            var parsed = GradientDescriptor.Parse(grad) as GradientRadialDescriptor;
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestSide, parsed.Size);
            
            //Standard rectangle at 25, 50 with a width of 200pts and height of 100pts
            Point location = new Point(Unit.Pt(25), Unit.Pt(50));;
            Size size = new Size(Unit.Pt(200), Unit.Pt(100));
            
            var coords = parsed.GetCoordsForBounds(location, size);
            AssertCoordinates(coords, 125d, 100d, 0d, 125d, 100d, 100d, 1, "Centre to Centre with 100pt radius to the farthest side");

        }
        
        
        private PDFLayoutDocument _layout;

        private static void ValidateLinearGradient(PDFLinearShadingPattern one, Color[] cols, double angle, string id, bool repeating = false)
        {
            Assert.IsTrue(one.PatternType == PatternType.ShadingPattern, "Pattern type failed for " + id);
            Assert.IsTrue(one.Registered, "Registered flag failed for " + id);
            Assert.IsNotNull(one.Descriptor, "Null descriptor for " + id);
            Assert.AreEqual(cols.Length, one.Descriptor.Colors.Count, "Descriptor color count failed for " + id);
            Assert.AreEqual(angle, Math.Round(one.Descriptor.Angle), "Descriptor angle failed for " + id);
            Assert.AreEqual(repeating, one.Descriptor.Repeating, "Repeating flag does not match for " + id);
            Assert.AreEqual(GradientType.Linear, one.Descriptor.GradientType, "Descriptor pattern type failed for " + id);
            for (int i = 0; i < cols.Length; i++)
            {
                Assert.AreEqual(cols[i], one.Descriptor.Colors[i].Color, "Color '" + i + "' does not match '" + cols[i].ToString() + "' for " + id);
            }
        }

        private static void ValidateDistances(PDFLinearShadingPattern pattern, string id, params double[] distances)
        {
            
            Assert.AreEqual(pattern.Descriptor.Colors.Count, distances.Length, "Distances count failed for " + id);
            for (int i = 0; i < distances.Length; i++)
            {
                var color = pattern.Descriptor.Colors[i];
                var distance = distances[i];
                Assert.IsTrue(color.Distance.HasValue, "Distances has value failed for " + id);
                var rounded = color.Distance.Value;
                rounded = Math.Round(rounded, 3);
                Assert.AreEqual(rounded, distance, "Distances value failed for " + id);
            }
        }
        
        private static void ValidateRepeatingLinearGradient(PDFLinearShadingPattern one, int repeatCount, Color[] cols, decimal[] distances, double angle, string id)
        {
            Assert.IsTrue(one.PatternType == PatternType.ShadingPattern, "Pattern type failed for " + id);
            Assert.IsTrue(one.Registered, "Registered flag failed for " + id);
            Assert.IsNotNull(one.Descriptor, "Null descriptor for " + id);
            Assert.AreEqual(cols.Length * repeatCount, one.Descriptor.Colors.Count, "Descriptor color count failed for " + id);
            Assert.AreEqual(angle, Math.Round(one.Descriptor.Angle), "Descriptor angle failed for " + id);
            //Assert.AreEqual(repeating, one.Descriptor.Repeating, "Repeating flag does not match for " + id);
            
            Assert.AreEqual(GradientType.Linear, one.Descriptor.GradientType, "Descriptor pattern type failed for " + id);
            for (int repeat = 0; repeat < repeatCount; repeat++)
            {
                decimal offset = (1.0m / repeatCount) * repeat;

                for (int i = 0; i < cols.Length; i++)
                {
                    var index = i + (repeat * cols.Length);
                    var color = one.Descriptor.Colors[index];
                    Assert.AreEqual(cols[i], color.Color,
                        "Color '" + index + "' does not match '" + cols[i] + "' for " + id);
                    if (null != distances)
                    {
                        Assert.IsTrue(color.Distance.HasValue,
                            "Distances has value at '" + index + "' failed for " + id);
                        Assert.AreEqual(distances[i] + offset, Math.Round((decimal)color.Distance.Value, 5),
                            "Distances value at '" + index + "' failed for " + id);
                    }
                }
            }
        }

        private void Gradient_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);
            _layout = context.DocumentLayout;
        }
        
        
        

        [TestMethod]
        public void RadialGradientTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/RadialGradients.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("RadialGradients.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    
                    ValidateLinearGradient(patterns[0] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom, "myPara1" );
                    ValidateLinearGradient(patterns[1] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom_Left, "myPara2");
                    ValidateLinearGradient(patterns[2] as PDFLinearShadingPattern, rg, (double)GradientAngle.Left, "myPara3");
                    ValidateLinearGradient(patterns[3] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Left, "myPara4");
                    ValidateLinearGradient(patterns[4] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top, "myPara5");
                    ValidateLinearGradient(patterns[5] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Right, "myPara6");
                    ValidateLinearGradient(patterns[6] as PDFLinearShadingPattern, rg, (double)GradientAngle.Right, "myPara7");
                    ValidateLinearGradient(patterns[7] as PDFLinearShadingPattern, ryg, (double)GradientAngle.Bottom_Right, "myPara8");
                    ValidateLinearGradient(patterns[8] as PDFLinearShadingPattern, rgby, (double)GradientAngle.Bottom, "myPara9");
                }
            }

            
        }
        
        [TestMethod]
        public void LinearGradientTurnsTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LinearGradientsTurns.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradientTurns.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    
                    ValidateLinearGradient(patterns[0] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom, "myPara1" );
                    ValidateLinearGradient(patterns[1] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom_Left, "myPara2");
                    ValidateLinearGradient(patterns[2] as PDFLinearShadingPattern, rg, (double)GradientAngle.Left, "myPara3");
                    ValidateLinearGradient(patterns[3] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Left, "myPara4");
                    ValidateLinearGradient(patterns[4] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top, "myPara5");
                    ValidateLinearGradient(patterns[5] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Right, "myPara6");
                    ValidateLinearGradient(patterns[6] as PDFLinearShadingPattern, rg, (double)GradientAngle.Right, "myPara7");
                    ValidateLinearGradient(patterns[7] as PDFLinearShadingPattern, ryg, (double)GradientAngle.Bottom_Right, "myPara8");
                    ValidateLinearGradient(patterns[8] as PDFLinearShadingPattern, rgby, (double)GradientAngle.Bottom, "myPara9");
                }
            }

            
        }
        
        [TestMethod]
        public void LinearGradientDegreesTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LinearGradientsDegree.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradientDegrees.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    
                    ValidateLinearGradient(patterns[0] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom, "myPara1" );
                    ValidateLinearGradient(patterns[1] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom_Left, "myPara2");
                    ValidateLinearGradient(patterns[2] as PDFLinearShadingPattern, rg, (double)GradientAngle.Left, "myPara3");
                    ValidateLinearGradient(patterns[3] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Left, "myPara4");
                    ValidateLinearGradient(patterns[4] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top, "myPara5");
                    ValidateLinearGradient(patterns[5] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Right, "myPara6");
                    ValidateLinearGradient(patterns[6] as PDFLinearShadingPattern, rg, (double)GradientAngle.Right, "myPara7");
                    ValidateLinearGradient(patterns[7] as PDFLinearShadingPattern, ryg, (double)GradientAngle.Bottom_Right, "myPara8");
                    ValidateLinearGradient(patterns[8] as PDFLinearShadingPattern, rgby, (double)GradientAngle.Bottom, "myPara9");
                }
            }

            
        }
        
        [TestMethod]
        public void LinearGradientRadiansTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LinearGradientsRadians.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradientRadians.pdf"))
                    {
                        doc.RenderOptions.Compression = OutputCompressionType.None;
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    
                    ValidateLinearGradient(patterns[0] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom, "myPara1" );
                    ValidateLinearGradient(patterns[1] as PDFLinearShadingPattern, rg, (double)GradientAngle.Bottom_Left, "myPara2");
                    ValidateLinearGradient(patterns[2] as PDFLinearShadingPattern, rg, (double)GradientAngle.Left, "myPara3");
                    ValidateLinearGradient(patterns[3] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Left, "myPara4");
                    ValidateLinearGradient(patterns[4] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top, "myPara5");
                    ValidateLinearGradient(patterns[5] as PDFLinearShadingPattern, rg, (double)GradientAngle.Top_Right, "myPara6");
                    ValidateLinearGradient(patterns[6] as PDFLinearShadingPattern, rg, 360.0, "myPara7");
                    ValidateLinearGradient(patterns[7] as PDFLinearShadingPattern, ryg, (double)GradientAngle.Bottom_Right, "myPara8");
                    ValidateLinearGradient(patterns[8] as PDFLinearShadingPattern, rgby, (double)GradientAngle.Bottom, "myPara9");
                }
            }

            
        }
        
         [TestMethod]
        public void LinearGradientDistanceTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LinearGradientDistances.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradientDistances.pdf"))
                    {
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(9, patterns.Count);

                    var grad = patterns[0] as PDFLinearShadingPattern;
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Green
                    ], (double)GradientAngle.Bottom, "myPara1" );
                    
                    ValidateDistances(grad, "myPara1", 0.0, 0.1, 0.3, 1.0);
                    
                    grad = patterns[1] as PDFLinearShadingPattern;
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Red,
                        StandardColors.Green
                    ], (double)GradientAngle.Bottom, "myPara2");
                    ValidateDistances(grad, "myPara2", 0.0, 0.9, 1.0);
                    
                    grad = patterns[2] as PDFLinearShadingPattern;
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Red,
                        StandardColors.Red
                    ], (double)GradientAngle.Left, "myPara3");
                    ValidateDistances(grad, "myPara3", 0.0, 0.25, 0.5, 1.0);
                    
                    grad = patterns[3] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Yellow
                    ], (double)GradientAngle.Top, "myPara4");
                    ValidateDistances(grad, "myPara4", 0.0, 0.1, 0.55, 1.0);
                    
                    grad = patterns[4] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Yellow
                    ], (double)GradientAngle.Bottom, "myPara5");
                    ValidateDistances(grad, "myPara5", 0.0, 0.1, 0.2, 0.3, 1.0);
                    
                    grad = patterns[5] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Yellow
                    ], (double)GradientAngle.Bottom, "myPara6");
                    ValidateDistances(grad, "myPara6", 0.0, 0.2, 0.25, 0.3, 0.35, 1.0);
                    
                    
                    grad = patterns[5] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Yellow
                    ], (double)GradientAngle.Bottom, "myPara6");
                    ValidateDistances(grad, "myPara6", 0.0, 0.2, 0.25, 0.3, 0.35, 1.0);
                    
                    grad = patterns[6] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Yellow,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Blue,
                    ], (double)GradientAngle.Bottom, "myPara7");
                    ValidateDistances(grad, "myPara7", 0.0, 0.1, 0.2, 0.3, 0.7, 0.75, 0.8, 0.85, 1.0);
                    
                    grad = patterns[7] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Yellow,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                    ], (double)GradientAngle.Bottom, "myPara8");
                    ValidateDistances(grad, "myPara8", 0.0, 0.1, 0.2, 0.3, 0.7, 0.8, 0.9, 1.0);
                    
                    grad = patterns[8] as PDFLinearShadingPattern;
                    
                    ValidateLinearGradient(grad, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                    ], 30.0 + 270.0, "myPara9");
                    
                    ValidateDistances(grad, "myPara9", 0.0, 0.083, 0.167, 0.25, 0.5, 0.75, 1.0);
                }
            }

            
        }
        
        
         [TestMethod]
        public void LinearGradientRepeatingTest()
        {
            
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/LinearGradientRepeating.html",
                this.TestContext);
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradientRepeating.pdf"))
                    {
                        doc.RenderOptions.Compression = OutputCompressionType.None;
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ryg = new Color[] { StandardColors.Red, StandardColors.Yellow, StandardColors.Green };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(5, patterns.Count);

                    var grad = patterns[0] as PDFLinearShadingPattern;
                    ValidateRepeatingLinearGradient(grad, 4, [
                        StandardColors.Red,
                        StandardColors.Blue
                    ], [0.0m, 0.25m], (double)GradientAngle.Top, "myPara1" );
                    
                    grad = patterns[1] as PDFLinearShadingPattern;
                    ValidateRepeatingLinearGradient(grad, 5, [
                        StandardColors.Red,
                        StandardColors.Green
                    ], [0.0m, 0.20m], (double)GradientAngle.Right, "myPara2" );

                    grad = patterns[2] as PDFLinearShadingPattern;
                    ValidateRepeatingLinearGradient(grad, 4, [
                        StandardColors.Red,
                        StandardColors.Green
                    ], [0.0m, 0.25m], 36.0d, "myPara3" );
                    
                    
                    grad = patterns[3] as PDFLinearShadingPattern;
                    ValidateRepeatingLinearGradient(grad, 20, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Yellow,
                    ], [0.0m, 0.025m, 0.05m], 299.0d, "myPara4" );
                    
                    grad = patterns[4] as PDFLinearShadingPattern;
                    ValidateRepeatingLinearGradient(grad, 1, [
                        StandardColors.Red,
                        StandardColors.Green,
                        StandardColors.Blue,
                        StandardColors.Yellow,
                    ], null, 90d, "myPara5" ); //No Distances specifed

                    
                }
            }

            
        }

        


    }
}