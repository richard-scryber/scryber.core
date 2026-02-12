using System;
using System.IO;
using Scryber.Drawing;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF;
using Scryber.PDF.Layout;
using System.Linq;
using Scryber.PDF.Graphics;


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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.ClosestSide, parsed.Size);
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.IsNull(parsed.StartCenter);
            Assert.IsNull(parsed.EndCenter);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
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
            
            
            //
            // just one postion
            //

            gradient = "radial-gradient(circle at right, #333, #333 50%, #eee 75%, #333 75%)";
            centre = new Point(Unit.Percent(100), Unit.Percent(50)); //with just one position, then the other should be 50%;
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);

            gradient = "repeating-radial-gradient(circle at top, #333, #333 50%, #eee 75%, #333 75%)";
            centre = new Point(Unit.Percent(50), Unit.Percent(0)); //with just one position, then the other should be 50%;
            
            parsed = GradientDescriptor.Parse(gradient) as GradientRadialDescriptor;
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            Assert.IsTrue(parsed.StartCenter.HasValue);
            Assert.AreEqual(centre, parsed.StartCenter);

        }


        private static void AssertCoordinates(double[] coords, double fx, double fy, double fr, double cx, double cy,
            double cr, int testIndex, string expected)
        {
            Assert.IsNotNull(coords);
            Assert.AreEqual(6, coords.Length);
            
            Assert.AreEqual(fx, coords[0], "First X coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
            Assert.AreEqual(fy, coords[1], "First Y coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
            Assert.AreEqual(fr, coords[2], "First radius coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
            Assert.AreEqual(cx, coords[3], "End X coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
            Assert.AreEqual(cy, coords[4], "End Y coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
            Assert.AreEqual(cr, coords[5], "End Radius coordinate failed for the Radial Gradient " + testIndex + " expected : " + expected);
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
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            
            //Standard rectangle at 25, 50 with a width of 200pts and height of 100pts - starting point in the centre
            Point location = new Point(Unit.Pt(25), Unit.Pt(50));;
            Size size = new Size(Unit.Pt(200), Unit.Pt(100));
            
            var coords = parsed.GetCoordsForBounds(location, size);
            var dia = Math.Sqrt(100d * 100d + 50d * 50d);
            AssertCoordinates(coords, 125d, 100d, 0d, 125d, 100d, dia, 1, "Centre to Centre with 100pt radius to the farthest side");

            grad = "radial-gradient(at left top, red, green)";
            parsed = GradientDescriptor.Parse(grad) as GradientRadialDescriptor;
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            
            //Standard rectangle at 25, 50 with a width of 200pts and height of 100pts - starting point left top
            location = new Point(Unit.Pt(25), Unit.Pt(50));;
            size = new Size(Unit.Pt(200), Unit.Pt(100));
            
            dia = Math.Sqrt(200d * 200d + 100d * 100d);
            coords = parsed.GetCoordsForBounds(location, size);
            AssertCoordinates(coords, 25d, 50d, 0d, 25d, 50d, dia, 2, "Left Top with 200pt radius to the farthest corner");
            
            grad = "radial-gradient(at bottom right, red, green)";
            parsed = GradientDescriptor.Parse(grad) as GradientRadialDescriptor;
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            
            //Standard rectangle at 25, 50 with a width of 200pts and height of 100pts - starting bottom right
            location = new Point(Unit.Pt(25), Unit.Pt(50));;
            size = new Size(Unit.Pt(200), Unit.Pt(100));
            
            dia = Math.Sqrt(200d * 200d + 100d * 100d);
            coords = parsed.GetCoordsForBounds(location, size);
            AssertCoordinates(coords, 225d, 150d, 0d, 225d, 150d, dia, 3, "Bottom Right with 200pt radius to the farthest corner");
            
            grad = "radial-gradient(at 25pt 35pt, red, green)";
            parsed = GradientDescriptor.Parse(grad) as GradientRadialDescriptor;
            
            Assert.IsNotNull(parsed);
            Assert.AreEqual(RadialShape.Circle, parsed.Shape);
            Assert.AreEqual(RadialSize.FarthestCorner, parsed.Size);
            
            //Standard rectangle at 25, 50 with a width of 200pts and height of 100pts - starting at 25, 35
            location = new Point(Unit.Pt(25), Unit.Pt(50));;
            size = new Size(Unit.Pt(200), Unit.Pt(100));
            
            dia = Math.Sqrt(175d * 175d + 65d * 65d);
            coords = parsed.GetCoordsForBounds(location, size);
            AssertCoordinates(coords, 50d, 15d, 0d, 50d, 15d, dia, 4,
                "50pt, 15pt with 175pt radius to the farthest corner"); //Y is reduced as PDF down is -ve;
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
        
        private static void ValidateRadialGradient(PDFRadialShadingPattern one, Size size, Color[] cols, double[] coords, string id, int repeatCount = -1)
        {
            Assert.IsTrue(one.PatternType == PatternType.ShadingPattern, "Pattern type failed for " + id);
            Assert.IsTrue(one.Registered, "Registered flag failed for " + id);
            Assert.IsNotNull(one.Descriptor, "Null descriptor for " + id);
            Assert.AreEqual(cols.Length, one.Descriptor.Colors.Count, "Descriptor color count failed for " + id);
            
            var descCoords = one.Descriptor.GetCoordsForBounds(Point.Empty, size);
            Assert.AreEqual(coords.Length, descCoords.Length, "Coords length failed for " + id);
            for (var i = 0; i < 6; i++)
            {
                Assert.AreEqual(coords[i], descCoords[i], "Coords value at '" + i + "' failed for " + id);
            }
            
            Assert.AreEqual(GradientType.Radial, one.Descriptor.GradientType, "Descriptor pattern type failed for " + id);

            for (int i = 0; i < cols.Length; i++)
            {
                        
                Assert.AreEqual(cols[i], one.Descriptor.Colors[i].Color,
                    "Color '" + i + "' does not match '" + cols[i].ToString() + "' for " + id);
            }
            
            var func = one.Descriptor.GetGradientFunction(Point.Empty, size);
            
            if (repeatCount > 1)
            {
                
                Assert.IsTrue(one.Descriptor.Repeating, "Repeating flag does not match for " + id);
                var func3 = func as PDFGradientFunction3;
                Assert.IsNotNull(func3, "Function type failed for " + id);
                
                Assert.AreEqual(repeatCount, func3.Functions.Length);
                Assert.AreEqual(repeatCount-1, func3.Boundaries.Length);
            }
            else
            {
                if (repeatCount == 1)
                    Assert.IsTrue(one.Descriptor.Repeating, "Repeating flag does not match for " + id); //Special case - repeating but does not actually repeat 
                else
                    Assert.IsFalse(one.Descriptor.Repeating, "Repeating flag does not match for " + id);
                
                if (cols.Length > 2)
                {
                    var func3 = func as PDFGradientFunction3;
                    Assert.IsNotNull(func3, "Function type failed for " + id);
                    Assert.AreEqual(func3.Functions.Length, cols.Length - 1,"Function type 3 length failed for id " + id);
                    Assert.AreEqual(cols.Length - 2, func3.Boundaries.Length, "Function type 3 boundaries failed for id " + id);
                }
                else
                {
                    var func2 = func as PDFGradientFunction2;
                    Assert.IsNotNull(func2, "Function type failed for " + id);
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/RadialGradients.html");
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("RadialGradients.pdf"))
                    {
                        doc.Params["value1"] = 2.056023E-11d;
                        doc.LayoutComplete += Gradient_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                    var rg = new Color[] { StandardColors.Red, StandardColors.Green };
                    // var rgby = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Blue, StandardColors.Yellow };
                    var ry = new Color[] { StandardColors.Red, StandardColors.Yellow};
                    var rgy = new Color[] { StandardColors.Red, StandardColors.Green, StandardColors.Yellow };

                    Assert.IsNotNull(_layout);
                    var pg = _layout.AllPages[0];

                    var resources = pg.Resources;
                    Assert.AreEqual(2, resources.Types.Count);

                    var patterns = resources.Types["Pattern"];
                    Assert.IsNotNull(patterns);
                    Assert.AreEqual(21, patterns.Count);

                    var stdSz = new Size(500, 200);
                    var diag = Math.Sqrt((500*500) + (200*200));
                    var halfDiag = diag / 2;
                    
                    // Red to Green default from centre.
                    var coords = new double[] { 250, 100, 0, 250, 100, halfDiag };
                    ValidateRadialGradient(patterns[0] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara1" );
                    
                    //Red to Green Circle from centre.
                    coords = [250, 100, 0, 250, 100, halfDiag];
                    ValidateRadialGradient(patterns[1] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara2" );
                    
                    //Red to Green closest side from centre.
                    coords = [250, 100, 0, 250, 100, 100];
                    ValidateRadialGradient(patterns[2] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara3" );
                    
                    // Farthest side
                    coords = [250, 100, 0, 250, 100, 250];
                    ValidateRadialGradient(patterns[3] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara4" );
                    
                    // closest corner
                    coords = [250, 100, 0, 250, 100, halfDiag];
                    ValidateRadialGradient(patterns[4] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara5" );
                    
                    // farthest corner
                    coords = [250, 100, 0, 250, 100, halfDiag];
                    ValidateRadialGradient(patterns[5] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara6" );
                    
                    diag = Math.Sqrt((400*400) + (150*150));
                    halfDiag = diag / 2;
                    // farthest corner from 400, 150 = the diagonal
                    coords = [400, -150, 0, 400, -150, diag];
                    ValidateRadialGradient(patterns[6] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara7" );
                    
                    // closest side from 400, 150 = 50 to bottom edge
                    coords = [400, -150, 0, 400, -150, 50];
                    ValidateRadialGradient(patterns[7] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara8" );
                    
                    diag = Math.Sqrt((400*400) + (200*200));
                    halfDiag = diag / 2;
                    // farthest corner from 400 top =  to left bottom corner
                    coords = [400, 0, 0, 400, 0, diag];
                    ValidateRadialGradient(patterns[8] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara9" );
                    
                    // closest side from left top =  0, but invalid so we set to 0.01
                    coords = [0, 0, 0, 0, 0, 0.01];
                    ValidateRadialGradient(patterns[9] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara10" );
                    
                    diag = Math.Sqrt((500*500) + (200*200));
                    // farthest corner from top left is full diagonal
                    coords = [0, 0, 0, 0, 0, diag];
                    ValidateRadialGradient(patterns[10] as PDFRadialShadingPattern, stdSz, rg, coords, "myPara11" );
                    
                    // closest side from 400, 50 = 50 up (3 color)
                    coords = [400, -50, 0, 400, -50, 50];
                    ValidateRadialGradient(patterns[11] as PDFRadialShadingPattern, stdSz, rgy, coords, "myPara12" );
                    
                    // farthest corner from 400, top (3 color)
                    diag = Math.Sqrt((400*400) + (200*200));
                    
                    coords = [400, 0, 0, 400, 0, diag];
                    ValidateRadialGradient(patterns[12] as PDFRadialShadingPattern, stdSz, rgy, coords, "myPara13" );
                    
                    // farthest corner from bottom right
                    diag = Math.Sqrt((500*500) + (200*200));
                    
                    coords = [500, 200, 0, 500, 200, diag];
                    ValidateRadialGradient(patterns[13] as PDFRadialShadingPattern, stdSz, rgy, coords, "myPara14" );
                    
                    // farthest corner from 400 top
                    diag = Math.Sqrt((400*400) + (200*200));
                    
                    //repeating - 0% to 50%, therefore 2 repeats.
                    coords = [400, 0, 0, 400, 0, diag];
                    ValidateRadialGradient(patterns[14] as PDFRadialShadingPattern, stdSz, ry, coords, "myPara15", 2 );
                    
                    //no explicit start so - 0% set on start, 100% on end and 1 repeat
                    coords = [400, 0, 0, 400, 0, diag];
                    ValidateRadialGradient(patterns[15] as PDFRadialShadingPattern, stdSz, ry, coords, "myPara16", 1 );
                    
                    //no explicit end to the gradient so - 100% on the last color and 1 repeat
                    coords = [400, 0, 0, 400, 0, diag];
                    ValidateRadialGradient(patterns[16] as PDFRadialShadingPattern, stdSz, ry, coords, "myPara17", 1 );
                    
                    //25% width to closest side from 400, 50 = 4 repeats to 100% and 9 repeats of 4 to farthest corner = 36
                    diag = Math.Sqrt((400*400) + (150*150));
                    
                    coords = [400, -50, 0, 400, -50, diag];
                    ValidateRadialGradient(patterns[17] as PDFRadialShadingPattern, stdSz, ry, coords, "myPara18", 36 );
                    
                    //50% width to closest side from 50,50 (top) = 2 repeats * 10 to farthest corner (bottom right) = 20
                    diag = Math.Sqrt((450*450) + (150*150));
                    
                    coords = [50, -50, 0, 50, -50, diag];
                    ValidateRadialGradient(patterns[18] as PDFRadialShadingPattern, stdSz, rgy, coords, "myPara19", 20 );
                    
                }
            }

            
        }
        


    }
}