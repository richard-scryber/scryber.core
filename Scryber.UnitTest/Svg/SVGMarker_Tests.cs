using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Svg.Components;
using System.IO;
using System.Linq;
using Scryber.PDF.Graphics;
using Scryber.PDF.Layout;
using Scryber.Svg;
using Path = Scryber.Components.Path;
using TransformMatrixOperation = Scryber.Styles.TransformMatrixOperation;
using TransformOperation = Scryber.Drawing.TransformOperation;

namespace Scryber.Core.UnitTests.Svg
{
    [TestClass()]
    public class SVGMarker_Tests
    {

        #region public TestContext TestContext

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
        
        
        [TestMethod]
        public void SVGMarkers_Start()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_Start.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_Start.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Start, false, null);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 10), vertex[0].Location);
                    
                    //2Pi = 360deg
                    
                    var rad = Math.PI / 2.0; //90 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location);
                    
                    
                    rad = Math.PI / 4.0; //45 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 3 top right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 10), vertex[0].Location);
                    
                    
                    rad = Math.PI - (Math.PI / 4.0); //135 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerStart);
                    Assert.AreEqual("#arrow", wavy.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(110, 10), vertex[0].Location);
                    rad = Math.PI / 4.0; //45 deg; (as starts with a half curve) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerStart);
                    Assert.AreEqual("#arrow", heart.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    rad = Math.PI + (Math.PI / 2.0); //270 deg; (straight up) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerStart);
                    Assert.AreEqual("#arrow", arrows.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location); //has a translation applied after
                    rad = 0.0; //horizontal
                    Assert.AreEqual(rad, vertex[0].Angle);
                }
            }
        }
        
        [TestMethod]
        public void SVGMarkers_StartFixedAngle()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_StartFixedAngle.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                var angle = (Math.PI / 2.0);
                
                using (var stream = DocStreams.GetOutputStream("SVGMarkers_StartFixedAngle.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Start, false, angle);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 10), vertex[0].Location);
                    
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location);
                    
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    //line 3 top right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 10), vertex[0].Location);
                    
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerStart);
                    Assert.AreEqual("#arrow", wavy.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(110, 10), vertex[0].Location);
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerStart);
                    Assert.AreEqual("#arrow", heart.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerStart);
                    Assert.AreEqual("#arrow", arrows.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location); //has a translation applied after
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                }
            }
        }
        
        
        [TestMethod]
        public void SVGMarkers_StartReversed()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_StartReversed.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_StartReversed.pdf"))
                {
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);

                     var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Start, true, null);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 10), vertex[0].Location);
                    
                    //2Pi = 360deg
                    var full = Math.PI * 2;
                    var half = Math.PI;
                    
                    var rad = half + Math.PI / 2.0; //90 deg reversed = 270; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location);


                    rad = half + Math.PI / 4.0; //135 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerStart);
                    Assert.AreEqual("#arrow", line.MarkerStart.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 10), vertex[0].Location);
                    
                    
                    rad = full - (Math.PI / 4.0); //315 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerStart);
                    Assert.AreEqual("#arrow", wavy.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(110, 10), vertex[0].Location);
                    rad = half + Math.PI / 4.0; //225 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerStart);
                    Assert.AreEqual("#arrow", heart.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    rad = half + (Math.PI + (Math.PI / 2.0)); //270 deg + 180 deg; (straight down) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerStart);
                    Assert.AreEqual("#arrow", arrows.MarkerStart.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location); //has a translation applied after
                    rad = half; //horizontal back
                    Assert.AreEqual(rad, vertex[0].Angle);


                }
            }
        }
        
        [TestMethod]
        public void SVGMarkers_StartReversedAndEnd()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_StartReversedAndEnd.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_StartReversedAndEnd.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);
                    
                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);
                    

                    var builder = new VertexBuilder(AdornmentPlacements.End | AdornmentPlacements.Start, true, null);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(50, 10), vertex[0].Location);
                    var rad = Math.PI + Math.PI / 2.0; // 270 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(50, 90), vertex[1].Location);
                    rad = Math.PI / 2.0; // 90 deg
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location);
                    rad = Math.PI + Math.PI / 4.0; //225 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(90,90), vertex[1].Location);
                    Assert.AreEqual(Math.PI / 4.0, vertex[1].Angle); //45 deg
                    
                    
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(90, 10), vertex[0].Location);
                    rad = Math.PI * 2 - (Math.PI / 4.0); //315 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(10, 90), vertex[1].Location);
                    rad = Math.PI / 2.0 + (Math.PI / 4.0);
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerEnd);
                    Assert.AreEqual("#arrow", wavy.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    
                    Assert.AreEqual(new Point(110, 10), vertex[0].Location);
                    rad = Math.PI + Math.PI / 4.0; //-45 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(200, 10), vertex[1].Location);
                    rad = - Math.PI / 4.0; //-45 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerEnd);
                    Assert.AreEqual("#arrow", heart.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    Assert.AreEqual(new Point(10, 30), vertex[1].Location); //same as start
                    
                    rad = Math.PI * 2 + (Math.PI / 2.0); //360 + 90 deg straight down - should sort to just +90
                    Assert.AreEqual(rad, vertex[0].Angle); 
                    
                    rad = - (Math.PI / 2.0); //- 90 deg; (straight up) 
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerEnd);
                    Assert.AreEqual("#arrow", above.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(10, 25), vertex[0].Location); //has a translation applied after
                    rad = Math.PI * 2 + (Math.PI / 2.0); //90 deg; (straight down)  - should sort to just +90
                    Assert.AreEqual(rad, vertex[0].Angle);
                    Assert.AreEqual(new Point(90, 25), vertex[1].Location); //has a translation applied after
                    rad = (Math.PI / 2.0); //90 deg; (straight down) 
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerEnd);
                    Assert.AreEqual("#arrow", arrows.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    Assert.AreEqual(new Point(10, 10), vertex[0].Location); //has a translation applied after
                    rad = Math.PI ; //180 deg - horizontal back
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(30, 80), vertex[1].Location); //has a translation applied after
                    rad = 0.0; //horizontal foreward
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                }
            }
        }
        
        [TestMethod]
        public void SVGMarkers_End()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_End.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_End.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.End, false, null);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 90), vertex[0].Location);
                    
                    //2Pi = 360deg
                    var full = Math.PI * 2;
                    var half = Math.PI;
                    
                    var rad = Math.PI / 2.0; //90 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 90), vertex[0].Location);


                    rad = Math.PI / 4.0; //45 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 90), vertex[0].Location);
                    
                    
                    rad = Math.PI/2.0 + (Math.PI / 4.0); //135 deg; 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerEnd);
                    Assert.AreEqual("#arrow", wavy.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(200, 10), vertex[0].Location);
                    rad = - Math.PI / 4.0; //-45 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerEnd);
                    Assert.AreEqual("#arrow", heart.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    rad = - (Math.PI / 2.0); //- 90 deg; (straight up) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerEnd);
                    Assert.AreEqual("#arrow", above.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 25), vertex[0].Location); //has a translation applied after
                    rad = (Math.PI / 2.0); //90 deg; (straight down) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerEnd);
                    Assert.AreEqual("#arrow", arrows.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(30, 80), vertex[0].Location); //has a translation applied after
                    rad = 0.0; //horizontal back
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                }
            }
        }

         [TestMethod]
        public void SVGMarkers_EndFixedAngle()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_EndFixedAngle.html",
                this.TestContext);
            
            var angle = (Math.PI / 2.0);
            
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_EndFixedAngle.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.End, false, angle);
                    
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 90), vertex[0].Location);

                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 90), vertex[0].Location);


                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerEnd);
                    Assert.AreEqual("#arrow", line.MarkerEnd.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 90), vertex[0].Location);
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerEnd);
                    Assert.AreEqual("#arrow", wavy.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(200, 10), vertex[0].Location);
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerEnd);
                    Assert.AreEqual("#arrow", heart.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(10, 30), vertex[0].Location); //has a translation applied after
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerEnd);
                    Assert.AreEqual("#arrow", above.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(90, 25), vertex[0].Location); //has a translation applied after
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerEnd);
                    Assert.AreEqual("#arrow", arrows.MarkerEnd.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(30, 80), vertex[0].Location); //has a translation applied after
                    
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                }
            }
        }
        
        [TestMethod]
        public void SVGMarkers_Mid()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_Mid.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_Mid.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Middle, false, null);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    //2Pi = 360deg
                    var full = Math.PI * 2;
                    var half = Math.PI;
                    
                    var rad = Math.PI / 2.0; //90 deg; 
                    //Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);


                    //rad = Math.PI / 4.0; //45 deg; 
                    //Assert.AreEqual(rad, vertex[0].Angle);
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    //rad = Math.PI/2.0 + (Math.PI / 4.0); //135 deg; 
                    //Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerMiddle);
                    Assert.AreEqual("#arrow", wavy.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    
                    Assert.AreEqual(new Point(140, 10), vertex[0].Location);
                    rad = 0 - Math.PI / 4.0; //-45 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(170, 10), vertex[1].Location);
                    rad = 0 + Math.PI / 4.0; //+45 deg; (as starts with a half curve and reversed) 
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerMiddle);
                    Assert.AreEqual("#arrow", heart.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(4, vertex.Count);
                    
                    Assert.AreEqual(new Point(50, 30), vertex[0].Location); //has a translation applied after
                    rad = 0; // 0 deg (straight across) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(90, 30), vertex[1].Location); //has a translation applied after
                    rad = (Math.PI / 2.0); // 90 deg; (straight down) 
                    Assert.AreEqual(rad, vertex[1].Angle);
                    
                    Assert.AreEqual(new Point(50, 90), vertex[2].Location); //has a translation applied after
                    rad =  (Math.PI); // 180 deg; (straight across back) 
                    Assert.AreEqual(rad, vertex[2].Angle);
                    
                    Assert.AreEqual(new Point(10, 30), vertex[3].Location); //has a translation applied after
                    rad = - (Math.PI / 2.0); //- 90 deg; (straight up) 
                    Assert.AreEqual(rad, vertex[3].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerMiddle);
                    Assert.AreEqual("#arrow", above.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 25), vertex[0].Location); //has a translation applied after
                    rad = 0; //0 deg; (straight across) 
                    Assert.AreEqual(rad, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerMiddle);
                    Assert.AreEqual("#arrow", arrows.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(20, vertex.Count);
                    var locs = new Point[20]
                    {
                        new Point(20, 10),
                        new Point(20, 20), new Point(30, 20), 
                        new Point(30, 30), new Point(40, 30),
                        
                        new Point(40, 20), new Point(50, 20), 
                        new Point(50, 30), new Point(60, 30),
                        new Point(60, 40), new Point(70, 40),
                        new Point(70,50), new Point(80,50),
                        
                        new Point(50,50), new Point(60,50),
                        new Point(40,60), new Point(50,60),
                        new Point(30,70), new Point(40,70),
                        new Point(20,80)
                    };
                    
                    for(var i = 0; i < 20; i++)
                    {
                        Assert.AreEqual(locs[i], vertex[i].Location, "Location " + i + " failed"); //has a translation applied after
                        rad = 0.0; //horizontal across
                        Assert.AreEqual(rad, vertex[i].Angle, "Angle " + i + " failed");
                    }
                    
                    
                    
                }
            }
        }
        
        [TestMethod]
        public void SVGMarkers_MidFixedAngle()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_MidFixedAngle.html",
                this.TestContext);
            var angle = Math.PI;
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_MidFixedAngle.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Middle, false, angle);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerMiddle);
                    Assert.AreEqual("#arrow", wavy.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    
                    Assert.AreEqual(new Point(140, 10), vertex[0].Location);
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(170, 10), vertex[1].Location);
                    Assert.AreEqual(angle, vertex[1].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerMiddle);
                    Assert.AreEqual("#arrow", heart.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(4, vertex.Count);
                    
                    Assert.AreEqual(new Point(50, 30), vertex[0].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(90, 30), vertex[1].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[1].Angle);
                    
                    Assert.AreEqual(new Point(50, 90), vertex[2].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[2].Angle);
                    
                    Assert.AreEqual(new Point(10, 30), vertex[3].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[3].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerMiddle);
                    Assert.AreEqual("#arrow", above.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 25), vertex[0].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerMiddle);
                    Assert.AreEqual("#arrow", arrows.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(20, vertex.Count);
                    var locs = new Point[20]
                    {
                        new Point(20, 10),
                        new Point(20, 20), new Point(30, 20), 
                        new Point(30, 30), new Point(40, 30),
                        
                        new Point(40, 20), new Point(50, 20), 
                        new Point(50, 30), new Point(60, 30),
                        new Point(60, 40), new Point(70, 40),
                        new Point(70,50), new Point(80,50),
                        
                        new Point(50,50), new Point(60,50),
                        new Point(40,60), new Point(50,60),
                        new Point(30,70), new Point(40,70),
                        new Point(20,80)
                    };
                    
                    for(var i = 0; i < 20; i++)
                    {
                        Assert.AreEqual(locs[i], vertex[i].Location, "Location " + i + " failed"); //has a translation applied after
                        Assert.AreEqual(angle, vertex[i].Angle, "Angle " + i + " failed");
                    }
                    
                    
                    
                }
            }
        }
        
        
        
        [TestMethod]
        public void SVGMarkers_VariousTypes()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_VariousTypes.html",
                this.TestContext);
            var angle = Math.PI;
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_VariousTypes.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var canvas = doc.FindAComponentById("svgmarkers") as SVGCanvas;
                    Assert.IsNotNull(canvas);

                    var arrowMarker = canvas.FindAComponentById("arrow") as SVGMarker;
                    Assert.IsNotNull(arrowMarker);

                    var circleMarker = canvas.FindAComponentById("circle") as SVGMarker;
                    Assert.IsNotNull(circleMarker);
                    Assert.AreEqual(5, circleMarker.Contents.Count);
                    Assert.IsInstanceOfType(circleMarker.Contents[1], typeof(SVGCircle));
                    Assert.IsInstanceOfType(circleMarker.Contents[3], typeof(SVGText));
                    
                    var rectMarker = canvas.FindAComponentById("square") as SVGMarker;
                    Assert.IsNotNull(rectMarker);
                    Assert.AreEqual(5, rectMarker.Contents.Count);
                    Assert.IsInstanceOfType(rectMarker.Contents[1], typeof(SVGRect));
                    Assert.IsInstanceOfType(rectMarker.Contents[3], typeof(SVGText));

                    var bounds = new Rect(0, 0, 10, 10);

                    //line 1= vertical down
                    
                    var line = canvas.FindAComponentById("line1") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    var gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    var builder = new VertexBuilder(AdornmentPlacements.Middle, false, angle);
                    var vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    
                    //line 4 top left to bottom right
                    
                    line = canvas.FindAComponentById("line4") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    //line 3 rop right to bottom left
                    
                    line = canvas.FindAComponentById("line3") as SVGLine;
                    Assert.IsNotNull(line);
                    Assert.IsNotNull(line.MarkerMiddle);
                    Assert.AreEqual("#arrow", line.MarkerMiddle.MarkerReference);

                    gpath = ((IGraphicPathComponent)line).CreatePath(bounds.Size, line.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(0, vertex.Count);
                    
                    
                    // wavyline

                    var wavy = canvas.FindAComponentById("wavyline") as SVGPath;
                    Assert.IsNotNull(wavy);
                    Assert.IsNotNull(wavy.MarkerMiddle);
                    Assert.AreEqual("#arrow", wavy.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)wavy).CreatePath(bounds.Size, wavy.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(2, vertex.Count);
                    
                    Assert.AreEqual(new Point(140, 10), vertex[0].Location);
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(170, 10), vertex[1].Location);
                    Assert.AreEqual(angle, vertex[1].Angle);
                    
                    // heart

                    var heart = canvas.FindAComponentById("heart") as SVGPath;
                    Assert.IsNotNull(heart);
                    Assert.IsNotNull(heart.MarkerMiddle);
                    Assert.AreEqual("#arrow", heart.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)heart).CreatePath(bounds.Size, heart.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(4, vertex.Count);
                    
                    Assert.AreEqual(new Point(50, 30), vertex[0].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    Assert.AreEqual(new Point(90, 30), vertex[1].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[1].Angle);
                    
                    Assert.AreEqual(new Point(50, 90), vertex[2].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[2].Angle);
                    
                    Assert.AreEqual(new Point(10, 30), vertex[3].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[3].Angle);
                    
                    // aboveheart for an arc end

                    var above = canvas.FindAComponentById("aboveheart") as SVGPath;
                    Assert.IsNotNull(above);
                    Assert.IsNotNull(above.MarkerMiddle);
                    Assert.AreEqual("#arrow", above.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)above).CreatePath(bounds.Size, above.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(1, vertex.Count);
                    Assert.AreEqual(new Point(50, 25), vertex[0].Location); //has a translation applied after
                    Assert.AreEqual(angle, vertex[0].Angle);
                    
                    // manyarrows

                    var arrows = canvas.FindAComponentById("manyarrows") as SVGPath;
                    Assert.IsNotNull(arrows);
                    Assert.IsNotNull(arrows.MarkerMiddle);
                    Assert.AreEqual("#arrow", arrows.MarkerMiddle.MarkerReference);
                    
                    gpath = ((IGraphicPathComponent)arrows).CreatePath(bounds.Size, arrows.GetAppliedStyle());
                    Assert.IsTrue(gpath.HasAdornments);

                    vertex = builder.CollectVertices(gpath).ToList();
                    Assert.AreEqual(20, vertex.Count);
                    var locs = new Point[20]
                    {
                        new Point(20, 10),
                        new Point(20, 20), new Point(30, 20), 
                        new Point(30, 30), new Point(40, 30),
                        
                        new Point(40, 20), new Point(50, 20), 
                        new Point(50, 30), new Point(60, 30),
                        new Point(60, 40), new Point(70, 40),
                        new Point(70,50), new Point(80,50),
                        
                        new Point(50,50), new Point(60,50),
                        new Point(40,60), new Point(50,60),
                        new Point(30,70), new Point(40,70),
                        new Point(20,80)
                    };
                    
                    for(var i = 0; i < 20; i++)
                    {
                        Assert.AreEqual(locs[i], vertex[i].Location, "Location " + i + " failed"); //has a translation applied after
                        Assert.AreEqual(angle, vertex[i].Angle, "Angle " + i + " failed");
                    }
                    
                    
                    
                }
            }
        }

    }

}



