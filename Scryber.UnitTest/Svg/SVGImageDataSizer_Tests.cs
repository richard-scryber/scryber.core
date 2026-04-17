using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.PDF.Graphics;
using Scryber.Styles;
using Scryber.Svg.Components;
using Scryber.Svg.Imaging;

namespace Scryber.Core.UnitTests.Svg
{
    /// <summary>
    /// Layer 2 tests — SVGImageDataSizer contract.
    /// Tests for GetLayoutSize() and GetImageToCanvasBBox() should pass once
    /// DoGetLayoutSize() is fixed to respect viewBox. Tests for GetCanvasToImageMatrix()
    /// will fail until that method is implemented (currently returns identity).
    /// </summary>
    [TestClass]
    public class SVGImageDataSizer_Tests
    {
        private const double Delta = 0.5; // half a point tolerance

        #region Helpers

        private static SVGCanvas MakeCanvas(Unit? width = null, Unit? height = null,
            Rect? viewBox = null, ViewPortAspectRatio? par = null)
        {
            var canvas = new SVGCanvas { IsDiscreetSVG = true };
            if (width.HasValue)
                canvas.Style.SetValue(StyleKeys.SizeWidthKey, width.Value);
            if (height.HasValue)
                canvas.Style.SetValue(StyleKeys.SizeHeightKey, height.Value);
            if (viewBox.HasValue)
                canvas.Style.SetValue(StyleKeys.PositionViewPort, viewBox.Value);
            if (par.HasValue)
                canvas.Style.SetValue(StyleKeys.ViewPortAspectRatioStyleKey, par.Value);
            return canvas;
        }

        private static SVGImageDataSizer MakeSizer(SVGCanvas canvas, Style appliedStyle = null, Size? available = null)
        {
            var style = appliedStyle ?? canvas.Style;
            var space = available ?? new Size(new Unit(800), new Unit(600));
            return SVGImageDataSizer.CreateSizingStrategy(canvas, style, null);
        }

        private static double ScaleX(PDFTransformationMatrix m) => m.Components[0];
        private static double ScaleY(PDFTransformationMatrix m) => m.Components[3];
        private static double TranslateX(PDFTransformationMatrix m) => m.Components[4];
        private static double TranslateY(PDFTransformationMatrix m) => m.Components[5];

        #endregion

        // ---------------------------------------------------------------
        // 2.1  GetLayoutSize()
        // ---------------------------------------------------------------

        [TestMethod]
        public void Size_NoVB_ExplicitWidthAndHeight()
        {
            var canvas = MakeCanvas(width: 150, height: 100);
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(150.0, size.Width.PointsValue, Delta, "Width from explicit SVG width");
            Assert.AreEqual(100.0, size.Height.PointsValue, Delta, "Height from explicit SVG height");
        }

        [TestMethod]
        public void Size_NoVB_WidthOnly_HeightDefaulted()
        {
            var canvas = MakeCanvas(width: 150);
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(150.0, size.Width.PointsValue, Delta, "Width from explicit SVG width");
            Assert.AreEqual(SVGCanvas.DefaultHeight.PointsValue, size.Height.PointsValue, Delta, "Height falls back to SVG default");
        }

        [TestMethod]
        public void Size_NoVB_HeightOnly_WidthDefaulted()
        {
            var canvas = MakeCanvas(height: 100);
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(SVGCanvas.DefaultWidth.PointsValue, size.Width.PointsValue, Delta, "Width falls back to SVG default");
            Assert.AreEqual(100.0, size.Height.PointsValue, Delta, "Height from explicit SVG height");
        }

        [TestMethod]
        public void Size_NoVB_NoDims_BothDefaulted()
        {
            var canvas = MakeCanvas();
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(SVGCanvas.DefaultWidth.PointsValue, size.Width.PointsValue, Delta, "Width defaults to 300");
            Assert.AreEqual(SVGCanvas.DefaultHeight.PointsValue, size.Height.PointsValue, Delta, "Height defaults to 150");
        }

        [TestMethod]
        [Description("viewBox present with no explicit width/height — layout size should come from viewBox dimensions")]
        public void Size_HasVB_NoDims_UsesViewBoxDimensions()
        {
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(200.0, size.Width.PointsValue, Delta, "Width from viewBox width");
            Assert.AreEqual(150.0, size.Height.PointsValue, Delta, "Height from viewBox height");
        }

        [TestMethod]
        [Description("viewBox present with explicit width/height — explicit dims should win (viewBox scales content, not container)")]
        public void Size_HasVB_ExplicitDims_ExplicitWins()
        {
            var canvas = MakeCanvas(width: 200, height: 150, viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(200.0, size.Width.PointsValue, Delta, "Explicit width wins");
            Assert.AreEqual(150.0, size.Height.PointsValue, Delta, "Explicit height wins");
        }

        [TestMethod]
        [Description("img element overrides width — height should scale proportionally from the SVG's intrinsic aspect ratio")]
        public void Size_ImgOverride_WidthOnly_HeightProportional()
        {
            // SVG is 150×100 (intrinsic aspect ratio 3:2)
            // img width=400 → height should be 400 * (100/150) ≈ 266.7
            var canvas = MakeCanvas(width: 150, height: 100);
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(400));
            // no height set → proportional
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(400.0, size.Width.PointsValue, Delta, "img width override");
            Assert.AreEqual(400.0 * (100.0 / 150.0), size.Height.PointsValue, Delta, "Height proportional to width override");
        }

        [TestMethod]
        [Description("img element overrides height — width should scale proportionally")]
        public void Size_ImgOverride_HeightOnly_WidthProportional()
        {
            // SVG is 150×100 (intrinsic aspect ratio 3:2)
            // img height=200 → width should be 200 * (150/100) = 300
            var canvas = MakeCanvas(width: 150, height: 100);
            var style = new Style();
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(200));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(200.0 * (150.0 / 100.0), size.Width.PointsValue, Delta, "Width proportional to height override");
            Assert.AreEqual(200.0, size.Height.PointsValue, Delta, "img height override");
        }

        [TestMethod]
        [Description("img element overrides both width and height — both should be respected exactly")]
        public void Size_ImgOverride_BothDims_BothExact()
        {
            var canvas = MakeCanvas(width: 150, height: 100, viewBox: new Rect(0, 0, 200, 150));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(100));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(80));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(100.0, size.Width.PointsValue, Delta, "img width override");
            Assert.AreEqual(80.0, size.Height.PointsValue, Delta, "img height override");
        }

        // ---------------------------------------------------------------
        // 2.2  GetCanvasToImageMatrix()
        // These tests define the required behaviour — all currently FAIL
        // because GetCanvasToImageMatrix() returns identity.
        // ---------------------------------------------------------------

        [TestMethod]
        [Description("No viewBox → identity matrix (no transform needed)")]
        public void Matrix_NoVB_ExplicitWH_IsIdentity()
        {
            var canvas = MakeCanvas(width: 150, height: 100);
            var sizer = MakeSizer(canvas);
            var m = sizer.GetCanvasToImageMatrix(new Size(150, 200), Point.Empty, null);

            Assert.IsTrue(m.IsIdentity, "No viewBox should produce identity matrix");
        }

        [TestMethod]
        [Description("viewBox exactly matches layout size → identity matrix")]
        public void Matrix_HasVB_ExactMatch_IsIdentity()
        {
            var canvas = MakeCanvas(width: 200, height: 150, viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var m = sizer.GetCanvasToImageMatrix(new Size(200, 150), Point.Empty, null);

            Assert.IsTrue(m.IsIdentity, "viewBox matching dims should produce identity matrix");
        }

        [TestMethod]
        [Description("Wider dest than viewBox with xMidYMid meet — content centred horizontally")]
        public void Matrix_HasVB_WiderDest_xMidYMid_Meet()
        {
            // viewBox: 0 0 400 150, dest: 600×150
            // scalex=600/400=1.5, scaley=150/150=1.0 → min=1.0 (height constrains)
            // scaledW=400, spareX=200 → offx=100
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 400, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(600));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(150));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(600, 150), Point.Empty, null);

            double scale = Math.Min(600.0 / 400.0, 150.0 / 150.0); // 1.0
            double scaledW = 400 * scale;
            double spareX = 600 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), 0.01, "Uniform scale");
            Assert.AreEqual(scale, ScaleY(m), 0.01, "Uniform scale Y");
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "No Y spare");
        }

        [TestMethod]
        [Description("Taller dest than viewBox with xMidYMid meet — content centred vertically")]
        public void Matrix_HasVB_TallerDest_xMidYMid_Meet()
        {
            // viewBox: 0 0 150 400, dest: 150×600
            // scalex=150/150=1.0, scaley=600/400=1.5 → min=1.0 (width constrains)
            // scaledH=400, spareY=200 → offy=100
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 150, 400),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(150));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(600));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(150, 600), Point.Empty, null);

            double scale = Math.Min(150.0 / 150.0, 600.0 / 400.0); // 1.0
            double scaledH = 400 * scale;
            double spareY = 600 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), 0.01, "Uniform scale");
            Assert.AreEqual(0.0, TranslateX(m), Delta, "No X spare");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred");
        }

        [TestMethod]
        [Description("preserveAspectRatio=none — non-uniform scaling, no translation")]
        public void Matrix_PAR_None_NonUniformScale()
        {
            // viewBox: 0 0 200 150, dest: 400×300 → scaleX=2, scaleY=2
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150),
                par: ViewPortAspectRatio.None);
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(400));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(300));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(400, 300), Point.Empty, null);

            Assert.AreEqual(2.0, ScaleX(m), 0.01, "ScaleX = dest.W / vb.W");
            Assert.AreEqual(2.0, ScaleY(m), 0.01, "ScaleY = dest.H / vb.H");
            Assert.AreEqual(0.0, TranslateX(m), Delta, "No X offset for none mode");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "No Y offset for none mode");
        }

        [TestMethod]
        [Description("xMinYMin meet — content at top-left (no offset in X, top in PDF space)")]
        public void Matrix_PAR_xMinYMin_Meet()
        {
            // viewBox: 0 0 200 150, dest: 300×200
            // min scale = min(1.5, 1.333) = 1.333, scaledH=200 spareY=0
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMinYMin, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(300));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(200));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(300, 200), Point.Empty, null);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledH = 150 * scale;
            double spareY = 200 - scaledH;

            Assert.AreEqual(scale, ScaleX(m), 0.01, "Uniform scale");
            Assert.AreEqual(0.0, TranslateX(m), Delta, "Left: X=0");
            Assert.AreEqual(spareY, TranslateY(m), Delta, "Top in PDF space: Y=spareY");
        }

        [TestMethod]
        [Description("xMaxYMax meet — content at bottom-right")]
        public void Matrix_PAR_xMaxYMax_Meet()
        {
            // viewBox: 0 0 200 150, dest: 300×200
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMaxYMax, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(300));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(200));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(300, 200), Point.Empty, null);

            double scale = Math.Min(300.0 / 200.0, 200.0 / 150.0);
            double scaledW = 200 * scale;
            double spareX = 300 - scaledW;

            Assert.AreEqual(scale, ScaleX(m), 0.01, "Uniform scale");
            Assert.AreEqual(spareX, TranslateX(m), Delta, "Right: X=spareX");
            Assert.AreEqual(0.0, TranslateY(m), Delta, "Bottom in PDF space: Y=0");
        }

        [TestMethod]
        [Description("xMidYMid slice — scale fills dest, content clips equally")]
        public void Matrix_PAR_xMidYMid_Slice()
        {
            // viewBox: 0 0 200 150, dest: 300×200
            // max scale = max(1.5, 1.333) = 1.5
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Slice));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(300));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(200));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(300, 200), Point.Empty, null);

            double scale = Math.Max(300.0 / 200.0, 200.0 / 150.0); // 1.5
            double scaledW = 200 * scale;
            double scaledH = 150 * scale;
            double spareX = 300 - scaledW; // 0
            double spareY = 200 - scaledH; // -25

            Assert.AreEqual(scale, ScaleX(m), 0.01, "Uniform max scale");
            Assert.AreEqual(spareX / 2, TranslateX(m), Delta, "X centred (0)");
            Assert.AreEqual(spareY / 2, TranslateY(m), Delta, "Y centred (clips)");
        }

        [TestMethod]
        [Description("Non-zero viewBox origin — matrix must include origin translation to shift content")]
        public void Matrix_NonZeroViewBoxOrigin_ShiftsContent()
        {
            // viewBox: 50 50 200 150 — content starts at (50,50) in SVG space
            // dest: 200×150, scale=1.0, origin offset TX=-50, TY=-50 (in addition to alignment)
            var canvas = MakeCanvas(viewBox: new Rect(50, 50, 200, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(200));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(150));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var m = sizer.GetCanvasToImageMatrix(new Size(200, 200), Point.Empty, null);

            // scale=1.0, spareX=0, spareY=0, plus origin shift: TX=-50, TY=-50
            Assert.AreEqual(1.0, ScaleX(m), 0.01, "Scale should be 1.0");
            Assert.AreEqual(-50.0, TranslateX(m), Delta, "Origin X shift");
            Assert.AreEqual(-50.0, TranslateY(m), Delta, "Origin Y shift");
        }

        // ---------------------------------------------------------------
        // 2.3  GetImageToCanvasBBox()
        // ---------------------------------------------------------------

        [TestMethod]
        public void BBox_NoVB_ExplicitWH_IsFullRect()
        {
            var canvas = MakeCanvas(width: 150, height: 100);
            var sizer = MakeSizer(canvas);
            var bbox = sizer.GetImageToCanvasBBox(null);

            Assert.AreEqual(0.0, bbox.X.PointsValue, Delta);
            Assert.AreEqual(0.0, bbox.Y.PointsValue, Delta);
            Assert.AreEqual(150.0, bbox.Width.PointsValue, Delta);
            Assert.AreEqual(100.0, bbox.Height.PointsValue, Delta);
        }

        [TestMethod]
        public void BBox_HasVB_Match_IsFullRect()
        {
            var canvas = MakeCanvas(width: 200, height: 150, viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var bbox = sizer.GetImageToCanvasBBox(null);

            Assert.AreEqual(0.0, bbox.X.PointsValue, Delta);
            Assert.AreEqual(0.0, bbox.Y.PointsValue, Delta);
            Assert.AreEqual(200.0, bbox.Width.PointsValue, Delta);
            Assert.AreEqual(150.0, bbox.Height.PointsValue, Delta);
        }

        [TestMethod]
        [Description("Wider dest with meet — BBox is the full dest rectangle; the matrix handles centring")]
        public void BBox_HasVB_WiderDest_Meet_IsFullDest()
        {
            // viewBox: 0 0 400 150, dest: 600×150
            // The matrix centres the content, but the BBox of the XObject is always the full dest.
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 400, 150),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(600));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(150));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var bbox = sizer.GetImageToCanvasBBox(null);

            Assert.AreEqual(0.0, bbox.X.PointsValue, Delta);
            Assert.AreEqual(0.0, bbox.Y.PointsValue, Delta);
            Assert.AreEqual(600.0, bbox.Width.PointsValue, Delta, "BBox width = full dest width");
            Assert.AreEqual(150.0, bbox.Height.PointsValue, Delta, "BBox height = full dest height");
        }

        [TestMethod]
        [Description("Taller dest with meet — BBox is the full dest rectangle; the matrix handles centring")]
        public void BBox_HasVB_TallerDest_Meet_IsFullDest()
        {
            // viewBox: 0 0 150 400, dest: 150×600
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 150, 400),
                par: new ViewPortAspectRatio(AspectRatioAlign.xMidYMid, AspectRatioMeet.Meet));
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(150));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(600));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var bbox = sizer.GetImageToCanvasBBox(null);

            Assert.AreEqual(0.0, bbox.X.PointsValue, Delta);
            Assert.AreEqual(0.0, bbox.Y.PointsValue, Delta);
            Assert.AreEqual(150.0, bbox.Width.PointsValue, Delta, "BBox width = full dest width");
            Assert.AreEqual(600.0, bbox.Height.PointsValue, Delta, "BBox height = full dest height");
        }

        [TestMethod]
        [Description("none mode — non-uniform scale, content fills exactly, no offset")]
        public void BBox_PAR_None_FillsDest()
        {
            var canvas = MakeCanvas(viewBox: new Rect(0, 0, 200, 150),
                par: ViewPortAspectRatio.None);
            var style = new Style();
            style.SetValue(StyleKeys.SizeWidthKey, new Unit(400));
            style.SetValue(StyleKeys.SizeHeightKey, new Unit(300));
            var sizer = MakeSizer(canvas, appliedStyle: style);
            var bbox = sizer.GetImageToCanvasBBox(null);

            Assert.AreEqual(0.0, bbox.X.PointsValue, Delta);
            Assert.AreEqual(0.0, bbox.Y.PointsValue, Delta);
            Assert.AreEqual(400.0, bbox.Width.PointsValue, Delta, "Content fills full dest width");
            Assert.AreEqual(300.0, bbox.Height.PointsValue, Delta, "Content fills full dest height");
        }

        // ---------------------------------------------------------------
        // 2.4  Percentage width / height on the SVG element itself
        // ---------------------------------------------------------------

        [TestMethod]
        [Description("width='50%' with viewBox 200x150 → intrinsic width=100, height proportional from viewBox AR")]
        public void Size_PercentWidth_WithViewBox_ResolvesAndProportional()
        {
            // viewBox: 0 0 200 150, width=50% → width=100pt, height=100*(150/200)=75pt
            var canvas = MakeCanvas(width: Unit.Percent(50), viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(100.0, size.Width.PointsValue, Delta, "50% of viewBox width 200");
            Assert.AreEqual(75.0,  size.Height.PointsValue, Delta, "Height proportional: 100*(150/200)");
        }

        [TestMethod]
        [Description("height='50%' with viewBox 200x150 → intrinsic height=75, width proportional from viewBox AR")]
        public void Size_PercentHeight_WithViewBox_ResolvesAndProportional()
        {
            // viewBox: 0 0 200 150, height=50% → height=75pt, width=75*(200/150)=100pt
            var canvas = MakeCanvas(height: Unit.Percent(50), viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(100.0, size.Width.PointsValue, Delta, "Width proportional: 75*(200/150)");
            Assert.AreEqual(75.0,  size.Height.PointsValue, Delta, "50% of viewBox height 150");
        }

        [TestMethod]
        [Description("width='50%' + height='50%' with viewBox 200x150 → intrinsic 100x75")]
        public void Size_PercentBothDims_WithViewBox_BothResolved()
        {
            var canvas = MakeCanvas(width: Unit.Percent(50), height: Unit.Percent(50),
                viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(100.0, size.Width.PointsValue, Delta, "50% of viewBox width 200");
            Assert.AreEqual(75.0,  size.Height.PointsValue, Delta, "50% of viewBox height 150");
        }

        [TestMethod]
        [Description("width='75%' + height='100%' with viewBox 200x150 → non-square result")]
        public void Size_PercentDims_NonSquare_WithViewBox()
        {
            // width=75% of 200=150, height=100% of 150=150
            var canvas = MakeCanvas(width: Unit.Percent(75), height: Unit.Percent(100),
                viewBox: new Rect(0, 0, 200, 150));
            var sizer = MakeSizer(canvas);
            var size = sizer.GetLayoutSize();

            Assert.AreEqual(150.0, size.Width.PointsValue, Delta, "75% of viewBox width 200");
            Assert.AreEqual(150.0, size.Height.PointsValue, Delta, "100% of viewBox height 150");
        }

        [TestMethod]
        [Description("width='50%' without viewBox → throws InvalidOperationException")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Size_PercentWidth_NoViewBox_Throws()
        {
            var canvas = MakeCanvas(width: Unit.Percent(50));
            var sizer = MakeSizer(canvas);
            _ = sizer.GetLayoutSize(); // must throw
        }

        [TestMethod]
        [Description("height='50%' without viewBox → throws InvalidOperationException")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Size_PercentHeight_NoViewBox_Throws()
        {
            var canvas = MakeCanvas(height: Unit.Percent(50));
            var sizer = MakeSizer(canvas);
            _ = sizer.GetLayoutSize(); // must throw
        }
    }
}
