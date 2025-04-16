using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;
using Scryber.PDF.Graphics;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for GraientDecriptors and is intended
    ///to contain all GradientDecriptor and GradientFunction Unit Tests
    ///</summary>
    [TestClass()]
    public class GradientDescriptor_Test
    {

        #region public TestContext TestContext {get;set;}
        
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

        #region public void LinearGradientDescriptorCtor_Test()

        /// <summary>
        ///A test for GradientLinearDescriptor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Gradients")]
        public void LinearGradientDescriptorCtor_Test()
        {
            GradientLinearDescriptor target = new GradientLinearDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Linear);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);
            Assert.AreEqual(target.Angle, 0.0);
            
        }

        #endregion

        #region public void RadientGradientDescriptorCtor_Test()

        /// <summary>
        ///A test for RadientGradientDescriptor Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Gradients")]
        public void RadientGradientDescriptorCtor_Test()
        {
            GradientRadialDescriptor target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);
            Assert.AreEqual(target.Shape, RadialShape.Circle);
            Assert.AreEqual(target.Size, RadialSize.FarthestSide);
        }

        #endregion

        #region public void RadientGradientDescriptorColor_Test()

        /// <summary>
        ///A test for PDFRadientGradientDescriptor Colors
        ///</summary>
        [TestMethod()]
        [TestCategory("Gradients")]
        public void GradientDescriptorColor_Test()
        {
            GradientDescriptor target = new GradientLinearDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Linear);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green)
            });

            Assert.AreEqual(2, target.Colors.Count);
            Assert.AreEqual(StandardColors.Red, target.Colors[0].Color);
            Assert.IsFalse(target.Colors[0].Distance.HasValue);
            Assert.IsFalse(target.Colors[0].Opacity.HasValue);

            Assert.AreEqual(StandardColors.Green, target.Colors[1].Color);
            Assert.IsFalse(target.Colors[1].Distance.HasValue);
            Assert.IsFalse(target.Colors[1].Opacity.HasValue);

            //Same test with radial

            target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green)
            });

            Assert.AreEqual(2, target.Colors.Count);
            Assert.AreEqual(StandardColors.Red, target.Colors[0].Color);
            Assert.IsFalse(target.Colors[0].Distance.HasValue);
            Assert.IsFalse(target.Colors[0].Opacity.HasValue);

            Assert.AreEqual(StandardColors.Green, target.Colors[1].Color);
            Assert.IsFalse(target.Colors[1].Distance.HasValue);
            Assert.IsFalse(target.Colors[1].Opacity.HasValue);
        }

        #endregion

        #region public void GradientDescriptorColorFunction2_Test()

        /// <summary>
        ///A test for GradientDescriptorColorFunction2 with appropriate stops
        ///</summary>
        [TestMethod()]
        [TestCategory("Gradients")]
        public void GradientDescriptorColorFunction2_Test()
        {
            GradientDescriptor target = new GradientLinearDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Linear);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green)
            });

            var fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));
            Assert.IsInstanceOfType(fn, typeof(PDFGradientFunction2));

            var fn2 = fn as PDFGradientFunction2;
            Assert.AreEqual(fn2.ColorZero, StandardColors.Red);
            Assert.AreEqual(fn2.ColorOne, StandardColors.Green);
            Assert.AreEqual(0.0, fn2.DomainStart);
            Assert.AreEqual(1.0, fn2.DomainEnd);
            Assert.AreEqual(1.0, fn2.Exponent);

            //Radial gradient at 0 and 50% test

            target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green, 50, null)
            });

            fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));
            Assert.IsInstanceOfType(fn, typeof(PDFGradientFunction2));

            fn2 = fn as PDFGradientFunction2;
            Assert.AreEqual(fn2.ColorZero, StandardColors.Red);
            Assert.AreEqual(fn2.ColorOne, StandardColors.Green);
            Assert.AreEqual(0.0, fn2.DomainStart);
            Assert.AreEqual(1.0, fn2.DomainEnd);
            Assert.AreEqual(1.0, fn2.Exponent);

            //Radial gradient at 50% and 100% test

            target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, 50, null),
                new GradientColor(StandardColors.Green, 100, null)
            });

            fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));

            //This should not be a function 2 type as we add the Red colour at 0% too.
            Assert.IsNotInstanceOfType(fn, typeof(PDFGradientFunction2));
            

        }

        #endregion


        #region public void GradientDescriptorColorFunction3_Test()

        /// <summary>
        ///A test for GradientDescriptorColorFunction3 with appropriate stops
        ///</summary>
        [TestMethod()]
        [TestCategory("Gradients")]
        public void GradientDescriptorColorFunction3_Test()
        {
            GradientDescriptor target = new GradientLinearDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Linear);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green)
            });

            var fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));
            Assert.IsInstanceOfType(fn, typeof(PDFGradientFunction2));

            var fn2 = fn as PDFGradientFunction2;
            Assert.AreEqual(fn2.ColorZero, StandardColors.Red);
            Assert.AreEqual(fn2.ColorOne, StandardColors.Green);
            Assert.AreEqual(0.0, fn2.DomainStart);
            Assert.AreEqual(1.0, fn2.DomainEnd);
            Assert.AreEqual(1.0, fn2.Exponent);

            //Radial gradient at 0 and 50% test

            target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red),
                new GradientColor(StandardColors.Green, 50, null)
            });

            fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));
            Assert.IsInstanceOfType(fn, typeof(PDFGradientFunction2));

            fn2 = fn as PDFGradientFunction2;
            Assert.AreEqual(fn2.ColorZero, StandardColors.Red);
            Assert.AreEqual(fn2.ColorOne, StandardColors.Green);
            Assert.AreEqual(0.0, fn2.DomainStart);
            Assert.AreEqual(1.0, fn2.DomainEnd);
            Assert.AreEqual(1.0, fn2.Exponent);

            //Radial gradient at 50% and 100% test

            target = new GradientRadialDescriptor();
            Assert.IsNotNull(target);
            Assert.IsTrue(target.GradientType == GradientType.Radial);
            Assert.IsFalse(target.Repeating);
            Assert.IsNotNull(target.Colors);
            Assert.AreEqual(0, target.Colors.Count);

            target.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, 50, null),
                new GradientColor(StandardColors.Green, 100, null)
            });

            fn = target.GetGradientFunction(Point.Empty, new Size(100, 100));

            //This should not be a function 3 type with 2 inner function 2's
            
            Assert.IsInstanceOfType(fn, typeof(PDFGradientFunction3));
            var fn3 = fn as PDFGradientFunction3;
            
            Assert.AreEqual(1, fn3.Boundaries.Length);
            Assert.AreEqual(2, fn3.Functions.Length);
            Assert.AreEqual(2, fn3.Encodes.Length);
            
            Assert.AreEqual(0.0, fn3.DomainStart);
            Assert.AreEqual(1.0, fn3.DomainEnd);
            
            var bounds = fn3.Boundaries[0];
            Assert.AreEqual(50, bounds.Bounds);
            
            var enc1 = fn3.Encodes[0];
            var enc2 = fn3.Encodes[1];
            Assert.AreEqual(0, enc1.Start);
            Assert.AreEqual(1, enc1.End);
            
            Assert.AreEqual(0, enc2.Start);
            Assert.AreEqual(1, enc2.End);
            
            var fn2_0 = fn3.Functions[0] as PDFGradientFunction2;
            var fn2_1 = fn3.Functions[1] as PDFGradientFunction2;
            
            Assert.IsNotNull(fn2_0);
            Assert.IsNotNull(fn2_1);
            
            //First is padded red to 50%
            Assert.AreEqual(0, fn2_0.DomainStart);
            Assert.AreEqual(1, fn2_0.DomainEnd);
            Assert.AreEqual(StandardColors.Red, fn2_0.ColorZero);
            Assert.AreEqual(StandardColors.Red, fn2_0.ColorOne);
            
            //Second is actual gradient to green
            Assert.AreEqual(0, fn2_0.DomainStart);
            Assert.AreEqual(1, fn2_0.DomainEnd);
            Assert.AreEqual(StandardColors.Red, fn2_1.ColorZero);
            Assert.AreEqual(StandardColors.Green, fn2_1.ColorOne);
        }

        #endregion


    }
}
