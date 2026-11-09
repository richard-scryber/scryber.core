using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.PDF.Graphics;

namespace Scryber.Core.UnitTests.Svg
{
    /// <summary>
    /// Layer 1 tests — pure math on ViewPortAspectRatio.
    /// All tests should pass without any implementation changes.
    /// </summary>
    [TestClass]
    public class SVGViewportMath_Tests
    {
        private const double Delta = 0.01;

        #region Helpers

        private static double ScaleX(PDFTransformationMatrix m) => m.Components[0];
        private static double ScaleY(PDFTransformationMatrix m) => m.Components[3];
        private static double TranslateX(PDFTransformationMatrix m) => m.Components[4];
        private static double TranslateY(PDFTransformationMatrix m) => m.Components[5];

        #endregion

        // ---------------------------------------------------------------
        // 1.1  Parse()
        // ---------------------------------------------------------------

        [TestMethod]
        public void Parse_Default_XMidYMid_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMidYMid meet");
            Assert.AreEqual(AspectRatioAlign.xMidYMid, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMinYMin_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMinYMin meet");
            Assert.AreEqual(AspectRatioAlign.xMinYMin, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMaxYMax_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMaxYMax meet");
            Assert.AreEqual(AspectRatioAlign.xMaxYMax, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMidYMid_Slice()
        {
            var par = ViewPortAspectRatio.Parse("xMidYMid slice");
            Assert.AreEqual(AspectRatioAlign.xMidYMid, par.Align);
            Assert.AreEqual(AspectRatioMeet.Slice, par.Meet);
        }

        [TestMethod]
        public void Parse_None()
        {
            var par = ViewPortAspectRatio.Parse("none");
            Assert.AreEqual(AspectRatioAlign.None, par.Align);
        }

        [TestMethod]
        public void Parse_XMinYMid_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMinYMid meet");
            Assert.AreEqual(AspectRatioAlign.xMinYMid, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMidYMin_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMidYMin meet");
            Assert.AreEqual(AspectRatioAlign.xMidYMin, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMaxYMin_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMaxYMin meet");
            Assert.AreEqual(AspectRatioAlign.xMaxYMin, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMinYMax_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMinYMax meet");
            Assert.AreEqual(AspectRatioAlign.xMinYMax, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMidYMax_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMidYMax meet");
            Assert.AreEqual(AspectRatioAlign.xMidYMax, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMaxYMid_Meet()
        {
            var par = ViewPortAspectRatio.Parse("xMaxYMid meet");
            Assert.AreEqual(AspectRatioAlign.xMaxYMid, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        [TestMethod]
        public void Parse_XMinYMin_Slice()
        {
            var par = ViewPortAspectRatio.Parse("xMinYMin slice");
            Assert.AreEqual(AspectRatioAlign.xMinYMin, par.Align);
            Assert.AreEqual(AspectRatioMeet.Slice, par.Meet);
        }

        [TestMethod]
        public void Parse_XMaxYMax_Slice()
        {
            var par = ViewPortAspectRatio.Parse("xMaxYMax slice");
            Assert.AreEqual(AspectRatioAlign.xMaxYMax, par.Align);
            Assert.AreEqual(AspectRatioMeet.Slice, par.Meet);
        }

        [TestMethod]
        public void Parse_EmptyString_ReturnsDefault()
        {
            // Empty string → Parse returns None align with Meet.None
            // (See implementation: empty → new ViewPortAspectRatio(AspectRatioAlign.None))
            var par = ViewPortAspectRatio.Parse("");
            Assert.AreEqual(AspectRatioAlign.None, par.Align);
        }

        [TestMethod]
        public void Parse_Invalid_ReturnsDefault()
        {
            var par = ViewPortAspectRatio.Parse("garbage input");
            Assert.AreEqual(AspectRatioAlign.xMidYMid, par.Align);
            Assert.AreEqual(AspectRatioMeet.Meet, par.Meet);
        }

        // ---------------------------------------------------------------
        // 1.2  ApplyMaxNonUniformScaling (preserveAspectRatio="none")
        // ---------------------------------------------------------------

        [TestMethod]
        public void None_SquareDestSquareVB()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 300);
            var vb = new Rect(0, 0, 300, 300);
            ViewPortAspectRatio.ApplyMaxNonUniformScaling(m, dest, vb);

            Assert.AreEqual(1.0, ScaleX(m), Delta, "ScaleX should be 1.0");
            Assert.AreEqual(1.0, ScaleY(m), Delta, "ScaleY should be 1.0");
        }

        [TestMethod]
        public void None_DoubleWidthDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(600, 300);
            var vb = new Rect(0, 0, 300, 300);
            ViewPortAspectRatio.ApplyMaxNonUniformScaling(m, dest, vb);

            Assert.AreEqual(2.0, ScaleX(m), Delta, "ScaleX should be dest.W/vb.W = 2.0");
            Assert.AreEqual(1.0, ScaleY(m), Delta, "ScaleY should be dest.H/vb.H = 1.0");
        }

        [TestMethod]
        public void None_HalfHeightDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 150);
            var vb = new Rect(0, 0, 300, 300);
            ViewPortAspectRatio.ApplyMaxNonUniformScaling(m, dest, vb);

            Assert.AreEqual(1.0, ScaleX(m), Delta, "ScaleX should be dest.W/vb.W = 1.0");
            Assert.AreEqual(0.5, ScaleY(m), Delta, "ScaleY should be dest.H/vb.H = 0.5");
        }

        [TestMethod]
        public void None_WiderVBInTallDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(400, 600);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyMaxNonUniformScaling(m, dest, vb);

            Assert.AreEqual(2.0, ScaleX(m), Delta, "ScaleX should be 400/200 = 2.0");
            Assert.AreEqual(4.0, ScaleY(m), Delta, "ScaleY should be 600/150 = 4.0");
        }

        // ---------------------------------------------------------------
        // 1.3  ApplyUniformScaling (preserveAspectRatio="<align> meet")
        //
        // NOTE: Y offsets are in PDF bottom-left coordinate space.
        //   "top" visually = offy = dest.H - scaledH  (distance from bottom)
        //   "bottom" visually = offy = 0               (at the bottom)
        //   "middle" visually = offy = (dest.H - scaledH) / 2
        // ---------------------------------------------------------------

        // Landscape dest: 300×200, viewBox: 0 0 200 150
        // scalex=300/200=1.5, scaley=200/150≈1.333 → min=1.333
        // scaledW=200*1.333=266.7, scaledH=150*1.333=200
        // spareX=300-266.7=33.3, spareY=200-200=0

        [TestMethod]
        public void Meet_xMidYMid_LandscapeDest_ConstrainedByHeight()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMid);

            double expectedScale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * expectedScale;
            double spareX = 300 - scaledW;
            // spareY = 0 (height exactly fits)

            Assert.AreEqual(expectedScale, ScaleX(m), Delta, "Uniform scale");
            Assert.AreEqual(expectedScale, ScaleY(m), Delta, "Uniform scale Y");
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Y at bottom (no spare)");
        }

        // Portrait dest: 200×300, viewBox: 0 0 200 150
        // scalex=200/200=1.0, scaley=300/150=2.0 → min=1.0
        // scaledW=200, scaledH=150  spareX=0, spareY=300-150=150

        [TestMethod]
        public void Meet_xMidYMid_PortraitDest_ConstrainedByWidth()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMid);

            double expectedScale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * expectedScale;
            double spareY = 300 - scaledH; // 150

            Assert.AreEqual(expectedScale, ScaleX(m), Delta, "Uniform scale");
            Assert.AreEqual(expectedScale, ScaleY(m), Delta, "Uniform scale Y");
            Assert.AreEqual(0.0, TranslateX(m), Delta, "X at left (no spare)");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        public void Meet_xMinYMin_LandscapeDest()
        {
            // "top-left" visually: offx=0, offy=dest.H-scaledH (from bottom = spare, if no Y spare then 0)
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMin);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledH = 150 * scale;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=dest.H-scaledH");
        }

        [TestMethod]
        public void Meet_xMinYMin_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMin);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * scale;
            double spareY = 300 - scaledH; // 150

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space");
        }

        [TestMethod]
        public void Meet_xMaxYMax_LandscapeDest()
        {
            // "bottom-right" visually: offx=spareX, offy=0 (from bottom)
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMax);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double spareX = 300 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMaxYMax_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMax);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledW = 200 * scale;
            double spareX = 200 - scaledW; // 0

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMinYMid_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMid);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledH = 150 * scale;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        public void Meet_xMidYMin_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMin);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Meet_xMaxYMin_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMin);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Meet_xMinYMax_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMax);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMidYMax_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMax);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double spareX = 300 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMaxYMid_LandscapeDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMid);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        public void Meet_xMinYMid_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMid);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * scale;
            double spareY = 300 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        public void Meet_xMidYMin_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMin);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * scale;
            double spareY = 300 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "X centred (no spare)");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Meet_xMaxYMin_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMin);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * scale;
            double spareY = 300 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Right-aligned (no spare): X=0");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Meet_xMinYMax_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMinYMax);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMidYMax_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMidYMax);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledW = 200 * scale;
            double spareX = 200 - scaledW; // 0

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred (no spare)");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Meet_xMaxYMid_PortraitDest()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(200, 300);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformScaling(m, dest, vb, AspectRatioAlign.xMaxYMid);

            double scale = Math.Min(200.0 / 200.0, 300.0 / 150.0); // 1.0
            double scaledH = 150 * scale;
            double spareY = 300 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Right-aligned (no spare): X=0");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        // ---------------------------------------------------------------
        // 1.4  ApplyUniformStretching (preserveAspectRatio="<align> slice")
        //
        // Landscape dest: 300×200, viewBox: 0 0 200 150
        // scalex=1.5, scaley=1.333 → max=1.5
        // scaledW=300, scaledH=225  spareX=0, spareY=200-225=-25
        // ---------------------------------------------------------------

        [TestMethod]
        public void Slice_xMidYMid()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMidYMid);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0); // 1.5
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW; // 0
            double spareY = 200 - scaledH; // -25

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred (clips equally top and bottom)");
        }

        [TestMethod]
        public void Slice_xMinYMin()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMinYMin);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledH = 150 * scale;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY (negative = clip bottom)");
        }

        [TestMethod]
        public void Slice_xMaxYMax()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMaxYMax);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double spareX = 300 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX (0 or negative)");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Slice_xMinYMid()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMinYMid);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledH = 150 * scale;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        public void Slice_xMidYMin()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMidYMin);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Slice_xMaxYMin()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMaxYMin);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top-aligned in PDF space: Y=spareY");
        }

        [TestMethod]
        public void Slice_xMinYMax()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMinYMax);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left-aligned: X=0");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Slice_xMidYMax()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMidYMax);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double spareX = 300 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom-aligned in PDF space: Y=0");
        }

        [TestMethod]
        public void Slice_xMaxYMid()
        {
            var m = new PDFTransformationMatrix();
            var dest = new Size(300, 200);
            var vb = new Rect(0, 0, 200, 150);
            ViewPortAspectRatio.ApplyUniformStretching(m, dest, vb, AspectRatioAlign.xMaxYMid);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), Delta);
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right-aligned: X=spareX");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }
    }
}
