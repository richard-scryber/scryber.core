﻿using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.PDF.Native;
using Scryber;
using System.Collections.Generic;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFGradientBrush_Test and is intended
    ///to contain all PDFGradientBrush_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFGradientBrush_Test
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
        [TestCategory("Graphics")]
        public void PDFGradientDescriptorParserEmpty_Test()
        {
            string value = "";
            GradientDescriptor desc = null;

            var result = GradientDescriptor.TryParse(null, out desc);
            Assert.IsFalse(result, "A null or empty string should return false for PDFGradientDescriptor.TryParse");


            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsFalse(result, "A null or empty string should return false for PDFGradientDescriptor.TryParse");

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFLinearGradientDescriptorParser_Test()
        {
            string value = "linear-gradient(red, green)";
            GradientDescriptor desc = null;

            var result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The linear gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for linear gradient " + value );
            Assert.IsInstanceOfType(desc, typeof(GradientLinearDescriptor), "Returned type for " + value + " was not a linear gradient");

            var linear = desc as GradientLinearDescriptor;
            Assert.AreEqual(180, linear.Angle, "Angle for value " + value + " was not correct");
            Assert.AreEqual(2, linear.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsFalse(linear.Repeating, "The linear gradient was set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, linear.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green
            Assert.AreEqual(StandardColors.Green, linear.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was not null");


            value = "linear-gradient(to top, #FF0000, rgba(0, 128, 0, 0.5))";

            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The linear gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for linear gradient " + value);
            Assert.AreEqual(GradientType.Linear, desc.GradientType, "Returned gradient type was not linear: " + desc.GradientType);
            Assert.IsInstanceOfType(desc, typeof(GradientLinearDescriptor), "Returned type for " + value + " was not a linear gradient");

            linear = desc as GradientLinearDescriptor;
            Assert.AreEqual(0, linear.Angle, "Angle for value " + value + " was not correct");
            Assert.AreEqual(2, linear.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsFalse(linear.Repeating, "The linear gradient was set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, linear.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green 50% opacity
            Assert.AreEqual(StandardColors.Green, linear.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsTrue(linear.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was null");
            Assert.AreEqual(0.5, linear.Colors[1].Opacity.Value, "Second colour opacity for " + value + " was not correct: " + linear.Colors[1].Opacity.Value);


            value = "linear-gradient(25deg, #FF0000, rgba(0, 128, 0, 0.5), yellow)";

            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The linear gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for linear gradient " + value);
            Assert.AreEqual(GradientType.Linear, desc.GradientType, "Returned gradient type was not linear: " + desc.GradientType);
            Assert.IsInstanceOfType(desc, typeof(GradientLinearDescriptor), "Returned type for " + value + " was not a linear gradient");

            linear = desc as GradientLinearDescriptor;
            Assert.AreEqual(25, linear.Angle, "Angle for value " + value + " was not correct");
            Assert.AreEqual(3, linear.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsFalse(linear.Repeating, "The linear gradient was set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, linear.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green 50% opacity
            Assert.AreEqual(StandardColors.Green, linear.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsTrue(linear.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was null");
            Assert.AreEqual(0.5, linear.Colors[1].Opacity.Value, "Second colour opacity for " + value + " was not correct: " + linear.Colors[1].Opacity.Value);

            //Third yellow
            Assert.AreEqual(StandardColors.Yellow, linear.Colors[2].Color, "Third colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[2].Distance.HasValue, "Third colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[2].Opacity.HasValue, "Third colour opacity for " + value + " was not null");


            value = "repeating-linear-gradient(red, green 20%, rgba(200, 200, 200, 0.5) 50%)";

            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The linear gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for linear gradient " + value);
            Assert.AreEqual(GradientType.Linear, desc.GradientType, "Returned gradient type was not linear: " + desc.GradientType);
            Assert.IsInstanceOfType(desc, typeof(GradientLinearDescriptor), "Returned type for " + value + " was not a linear gradient");

            linear = desc as GradientLinearDescriptor;
            Assert.AreEqual(180, linear.Angle, "Angle for value " + value + " was not correct");
            Assert.AreEqual(3, linear.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsTrue(linear.Repeating, "The linear gradient was not set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, linear.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(linear.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green
            Assert.AreEqual(StandardColors.Green, linear.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsTrue(linear.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was null");
            Assert.AreEqual(20, linear.Colors[1].Distance.Value, "Second colour distance for " + value + " was not correct");
            Assert.IsFalse(linear.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was not null");

            //Final color
            Assert.AreEqual(new Scryber.Drawing.Color(200,200,200), linear.Colors[2].Color, "Third colour value for " + value + " was not correct");
            Assert.IsTrue(linear.Colors[2].Distance.HasValue, "Third colour distance for " + value + " was null");
            Assert.AreEqual(50, linear.Colors[2].Distance.Value, "Third colour distance for " + value + " was not correct");
            Assert.IsTrue(linear.Colors[2].Opacity.HasValue, "Third colour opacity for " + value + " was not null");
            Assert.AreEqual(0.5, linear.Colors[2].Opacity.Value, "Third colour distance for " + value + " was not correct");

        }

        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFRadialGradientDescriptorParser_Test()
        {
            string value = "radial-gradient(red, green)";
            GradientDescriptor desc = null;

            var result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The radial gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for radial gradient " + value);
            Assert.IsInstanceOfType(desc, typeof(GradientRadialDescriptor), "Returned type for " + value + " was not a radial gradient");

            var radial = desc as GradientRadialDescriptor;
            Assert.AreEqual(RadialShape.Circle, radial.Shape, "Shape for value " + value + " was not correct");
            Assert.AreEqual(2, radial.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsFalse(radial.Repeating, "The linear gradient was set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, radial.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green
            Assert.AreEqual(StandardColors.Green, radial.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was not null");

            

            value = "radial-gradient(circle, #FF0000, rgba(0, 128, 0, 0.5), yellow)";

            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The radial gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for radial gradient " + value);
            Assert.IsInstanceOfType(desc, typeof(GradientRadialDescriptor), "Returned type for " + value + " was not a radial gradient");

            radial = desc as GradientRadialDescriptor;
            Assert.AreEqual(RadialShape.Circle, radial.Shape, "Angle for value " + value + " was not correct");
            Assert.AreEqual(3, radial.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsFalse(radial.Repeating, "The radial gradient was set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, radial.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green 50% opacity
            Assert.AreEqual(StandardColors.Green, radial.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsTrue(radial.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was null");
            Assert.AreEqual(0.5, radial.Colors[1].Opacity.Value, "Second colour opacity for " + value + " was not correct: " + radial.Colors[1].Opacity.Value);

            //Third yellow
            Assert.AreEqual(StandardColors.Yellow, radial.Colors[2].Color, "Third colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[2].Distance.HasValue, "Third colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[2].Opacity.HasValue, "Third colour opacity for " + value + " was not null");


            value = "repeating-radial-gradient(red, green)";

            result = GradientDescriptor.TryParse(value, out desc);
            Assert.IsTrue(result, "The radial gradient " + value + " failed to be parsed");
            Assert.IsNotNull(desc, "Null returned for radial gradient " + value);
            Assert.IsInstanceOfType(desc, typeof(GradientRadialDescriptor), "Returned type for " + value + " was not a radial gradient");

            radial = desc as GradientRadialDescriptor;
            Assert.AreEqual(RadialShape.Circle, radial.Shape, "Shape for value " + value + " was not correct");
            Assert.AreEqual(2, radial.Colors.Count, "Colour count for value " + value + " was not correct");
            Assert.IsTrue(radial.Repeating, "The radial gradient was not set to repeat for " + value);

            //First red
            Assert.AreEqual(StandardColors.Red, radial.Colors[0].Color, "First colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[0].Distance.HasValue, "First colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[0].Opacity.HasValue, "First colour opacity for " + value + " was not null");

            //Second green
            Assert.AreEqual(StandardColors.Green, radial.Colors[1].Color, "Second colour value for " + value + " was not correct");
            Assert.IsFalse(radial.Colors[1].Distance.HasValue, "Second colour distance for " + value + " was not null");
            Assert.IsFalse(radial.Colors[1].Opacity.HasValue, "Second colour opacity for " + value + " was not null");

            
        }




        /// <summary>
        ///A test for PDFGradientLinearBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFLinearBrushConstructor_Test()
        {
            GradientLinearDescriptor descriptor = new GradientLinearDescriptor();
            descriptor.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, null, null)
            });

            PDF.Graphics.PDFGradientLinearBrush target = new PDF.Graphics.PDFGradientLinearBrush(descriptor);
            Scryber.Drawing.Color color = StandardColors.Red;
            double? opacity = null;
            double? distance = null;

            Assert.IsNotNull(target);
            Assert.AreEqual(GradientType.Linear, target.GradientType);
            Assert.AreEqual(color, target.Colors[0].Color);
            Assert.AreEqual(opacity, target.Colors[0].Opacity);
            Assert.AreEqual(distance, target.Colors[0].Distance);
        }

        /// <summary>
        ///A test for PDFGradientLinearBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFRepeatingAngleLinearBrushConstructor_Test()
        {
            GradientLinearDescriptor descriptor = new GradientLinearDescriptor();
            descriptor.Angle = (double)GradientAngle.Bottom_Left;
            descriptor.Repeating = true;
            descriptor.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, null, null)
            });

            PDF.Graphics.PDFGradientLinearBrush target = new PDF.Graphics.PDFGradientLinearBrush(descriptor);
            Scryber.Drawing.Color color = StandardColors.Red;
            double? opacity = null;
            double? distance = null;

            Assert.IsNotNull(target);
            Assert.AreEqual(GradientType.Linear, target.GradientType);
            Assert.AreEqual(true, target.Repeating);
            Assert.AreEqual((double)GradientAngle.Bottom_Left, target.Angle);
            Assert.AreEqual(color, target.Colors[0].Color);
            Assert.AreEqual(opacity, target.Colors[0].Opacity);
            Assert.AreEqual(distance, target.Colors[0].Distance);
        }

        /// <summary>
        ///A test for PDFGradientRadialBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFRadialBrushConstructor_Test()
        {
            GradientRadialDescriptor descriptor = new GradientRadialDescriptor();
            descriptor.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, null, null)
            });

            PDF.Graphics.PDFGradientRadialBrush target = new PDF.Graphics.PDFGradientRadialBrush(descriptor);
            Scryber.Drawing.Color color = StandardColors.Red;
            double? opacity = null;
            double? distance = null;

            Assert.IsNotNull(target);
            Assert.AreEqual(GradientType.Radial, target.GradientType);
            Assert.AreEqual(false, target.Repeating); //Default
            Assert.AreEqual(RadialShape.Circle, target.Shape); //Default
            Assert.AreEqual(color, target.Colors[0].Color); 
            Assert.AreEqual(opacity, target.Colors[0].Opacity);
            Assert.AreEqual(distance, target.Colors[0].Distance);
        }

        /// <summary>
        ///A test for PDFGradientRadialBrush Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PDFRepeatingCircleRadialBrushConstructor_Test()
        {
            GradientRadialDescriptor descriptor = new GradientRadialDescriptor();
            descriptor.Repeating = true;
            descriptor.Shape = RadialShape.Circle;
            descriptor.Colors = new List<GradientColor>(new GradientColor[]
            {
                new GradientColor(StandardColors.Red, null, null)
            });

            PDF.Graphics.PDFGradientRadialBrush target = new PDF.Graphics.PDFGradientRadialBrush(descriptor);
            Scryber.Drawing.Color color = StandardColors.Red;
            double? opacity = null;
            double? distance = null;

            Assert.IsNotNull(target);
            Assert.AreEqual(GradientType.Radial, target.GradientType);
            Assert.AreEqual(RadialShape.Circle, target.Shape);
            Assert.AreEqual(true, target.Repeating);
            Assert.AreEqual(color, target.Colors[0].Color);
            Assert.AreEqual(opacity, target.Colors[0].Opacity);
            Assert.AreEqual(distance, target.Colors[0].Distance);
        }




    }
}
