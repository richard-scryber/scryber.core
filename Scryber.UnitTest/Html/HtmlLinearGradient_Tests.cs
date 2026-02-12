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
    public class HtmlLinearGradient_Tests
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
        public void LinearGradientParsingTest()
        {
            var gradient = "linear-gradient(red)";
           
            var parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, parsed.Angle); //To Bottom is default
            Assert.AreEqual(1, parsed.Colors.Count);
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);

            
            gradient = "linear-gradient(red, blue)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, parsed.Angle); //To Bottom is default
            Assert.AreEqual(2, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            
            gradient = "linear-gradient(red, blue, green, yellow)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, parsed.Angle); //To Bottom is default
            Assert.AreEqual(4, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Green, parsed.Colors[2].Color);
            Assert.IsFalse(parsed.Colors[2].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[2].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Yellow, parsed.Colors[3].Color);
            Assert.IsFalse(parsed.Colors[3].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[3].Distance.HasValue);
            
        }
        
        [TestMethod]
        public void LinearGradientParsingWithKnownAngleTest()
        {
            var gradient = "linear-gradient(to bottom, red)";
           
            var parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, parsed.Angle); //To Bottom is default
            Assert.AreEqual(1, parsed.Colors.Count);
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);

            
            gradient = "linear-gradient(to top, red, blue)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(270, parsed.Angle); //To Bottom is default
            Assert.AreEqual(2, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            
            gradient = "linear-gradient(to bottom right, red, blue, green, yellow)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(45, parsed.Angle); //To Bottom is default
            Assert.AreEqual(4, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Green, parsed.Colors[2].Color);
            Assert.IsFalse(parsed.Colors[2].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[2].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Yellow, parsed.Colors[3].Color);
            Assert.IsFalse(parsed.Colors[3].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[3].Distance.HasValue);
            
        }
        
        [TestMethod]
        public void LinearGradientParsingWithExplicitAngleTest()
        {
            var gradient = "linear-gradient(0, red)";
           
            var parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(270, parsed.Angle); //To Top is 270 in PDF
            Assert.AreEqual(1, parsed.Colors.Count);
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);

            
            gradient = "linear-gradient(225deg, red, blue)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(135, parsed.Angle); //To bottom left
            Assert.AreEqual(2, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            
            gradient = "linear-gradient(45deg, red, blue, green, yellow)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(315, parsed.Angle); //To Top Right
            Assert.AreEqual(4, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Green, parsed.Colors[2].Color);
            Assert.IsFalse(parsed.Colors[2].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[2].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Yellow, parsed.Colors[3].Color);
            Assert.IsFalse(parsed.Colors[3].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[3].Distance.HasValue);
            
        }
        
         [TestMethod]
        public void LinearGradientParsingWithExplicitTurnTest()
        {
            var gradient = "linear-gradient(0.5turn, red)";
           
            var parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, parsed.Angle); //To Bottom is default
            Assert.AreEqual(1, parsed.Colors.Count);
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);

            
            gradient = "linear-gradient(0.25turn, red, blue)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(0, parsed.Angle); //To Bottom is default
            Assert.AreEqual(2, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            
            gradient = "linear-gradient(0.0turn, red, blue, green, yellow)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(270, parsed.Angle); //To Top is 0 turns - PDF 270
            Assert.AreEqual(4, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Green, parsed.Colors[2].Color);
            Assert.IsFalse(parsed.Colors[2].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[2].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Yellow, parsed.Colors[3].Color);
            Assert.IsFalse(parsed.Colors[3].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[3].Distance.HasValue);
            
        }
        
         [TestMethod]
        public void LinearGradientParsingWithExplicitRadianTest()
        {
            var gradient = "linear-gradient(0rad, red)";
           
            var parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(270, parsed.Angle); //To Bottom is default
            Assert.AreEqual(1, parsed.Colors.Count);
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);

            
            gradient = "linear-gradient(3.142rad, red, blue)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(90, Math.Round(parsed.Angle)); //To Bottom is default
            Assert.AreEqual(2, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            
            gradient = "linear-gradient(1.5707rad, red, blue, green, yellow)";
            parsed = GradientDescriptor.Parse(gradient) as GradientLinearDescriptor;
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(360, Math.Round(parsed.Angle)); //To Bottom is default
            Assert.AreEqual(4, parsed.Colors.Count);
            
            Assert.AreEqual(StandardColors.Red, parsed.Colors[0].Color);
            Assert.IsFalse(parsed.Colors[0].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[0].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Blue, parsed.Colors[1].Color);
            Assert.IsFalse(parsed.Colors[1].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[1].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Green, parsed.Colors[2].Color);
            Assert.IsFalse(parsed.Colors[2].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[2].Distance.HasValue);
            
            Assert.AreEqual(StandardColors.Yellow, parsed.Colors[3].Color);
            Assert.IsFalse(parsed.Colors[3].Opacity.HasValue);
            Assert.IsFalse(parsed.Colors[3].Distance.HasValue);
            
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
        public void LinearGradientTest()
        {
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradients.html");
            using (var sr = new System.IO.StreamReader(path))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("LinearGradient.pdf"))
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradientsTurns.html");
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradientsDegree.html");
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradientsRadians.html");
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradientDistances.html");
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
            
            var path = DocStreams.AssertGetTemplatePath("HTML/LinearGradientRepeating.html");
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