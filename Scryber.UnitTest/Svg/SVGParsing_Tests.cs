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
    public class SVG_Tests
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
        public void SVGSimple()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGSimple.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGSimple.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.SaveAsPDF(stream);

                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    Assert.AreEqual(7, section.Contents.Count);
                    Assert.IsInstanceOfType(section.Contents[1], typeof(HTMLParagraph));
                    Assert.IsInstanceOfType(section.Contents[3], typeof(Canvas));
                    Assert.IsInstanceOfType(section.Contents[5], typeof(HTMLParagraph));
 
                    var canvas = section.Contents[3] as Canvas;
                    Assert.IsNotNull(canvas);
                    Assert.AreEqual(300, canvas.Style.Size.Width.PointsValue, "The width of the canvas was not set");
                    Assert.AreEqual(200, canvas.Style.Size.Height.PointsValue, "The height of the canvas was not set");
                    Assert.AreEqual("canvas", canvas.StyleClass, "The style class of the canvas was not set");

                    Assert.AreEqual(3, canvas.Contents.Count);
                    var rect = canvas.Contents[1] as SVGRect;
                    Assert.IsNotNull(rect, "The inner rectangle was not found");
                    Assert.AreEqual("box", rect.StyleClass, "The rect style class was not correct");
                    Assert.AreEqual(100, rect.Style.GetValue(StyleKeys.SVGGeometryXKey, 0), "The X position of the rect was not correct");
                    Assert.AreEqual(10, rect.Style.GetValue(StyleKeys.SVGGeometryYKey, 0), "The Y position of the rect was not correct");
                    Assert.AreEqual(60, rect.Style.Size.Width.PointsValue, "The width of the rect was not correct");
                    Assert.AreEqual(70, rect.Style.Size.Height.PointsValue, "The height of the rect was not correct");
                    Assert.AreEqual(StandardColors.Green, rect.Style.Stroke.Color, "The stroke color of the rect was not set");
                    Assert.AreEqual(2, rect.Style.Stroke.Width.PointsValue, "The width of the stroke was not correct");
                    Assert.AreEqual(StandardColors.Yellow, rect.Style.Fill.Color, "The fill color of the rect was not correct");
                    Assert.AreEqual(20, rect.CornerRadiusX, "The x corner radius was not correct");
                    Assert.AreEqual(10, rect.CornerRadiusY, "The Y corner radius was not correct");
                }
            }
        }


        [TestMethod]
        public void SVGComponents()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGComponents.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGComponents.pdf"))
                {
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var clock = section.Contents[3] as Canvas;
                    
                    Assert.IsNotNull(clock);
                    Assert.AreEqual("ClockIcon", clock.ID);
                    Assert.AreEqual(3, clock.Contents.Count);
                    Assert.AreEqual(20, clock.Width);
                    Assert.AreEqual(20, clock.Height);

                    Assert.IsInstanceOfType(clock.Contents[0], typeof(Whitespace));
                    var clockPath = clock.Contents[1] as SVGPath;
                    Assert.IsInstanceOfType(clock.Contents[2], typeof(Whitespace));
                    Assert.IsNotNull(clockPath);
                    Assert.AreEqual(StandardColors.Blue, clockPath.FillColor);
                }
            }
        }
        
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
        public void SVGMarkers_StartAndEndReversed()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGMarkers_StartReversed.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGMarkers_StartAndEndReversed.pdf"))
                {
                    doc.AutoBind = true;
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var clock = section.Contents[3] as Canvas;
                    
                    
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
        public void SVGTransformParsing()
        {
            //rotate examples
            string toParse = "rotate(3.142)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var root = parsed.Root;
            var rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, rotate.AngleRadians);
            
            toParse = "rotate(-0.5turn) ";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(-3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "rotate(180deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "scale(1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.0, Math.Round(scale.XScaleValue));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue));
            
            toParse = "scale(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.7, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(1.3, 0.4)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.4, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(-0.5,1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            
            toParse = "scaleX(1.3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(-0.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "skew(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(15, 15)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            const double Deg2Rad = Math.PI / 180.0;

            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));

            toParse = "skew(-6, 18)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(18 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            
           
            
            toParse = "skewX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(-6)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);
            
            toParse = "skewX(35)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);

            toParse = "skewX(234)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(234 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(-6)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(-6 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);
            
            toParse = "skewY(35)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);

            toParse = "skewY(234)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round(234 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            
            toParse = "translate(200)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(200), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);

            toParse = "translate(50)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(50), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);
            
            toParse = "translate(100,200)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(200), translate.YOffset);
            
            toParse = "translate(100,50)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(50), translate.YOffset);
            
            toParse = "translate(-30, 210.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(-30), translate.XOffset);
            Assert.AreEqual(Unit.Pt(210.5), translate.YOffset);
            
            toParse = "translate(30%, -50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(30), translate.XOffset);
            Assert.AreEqual(Unit.Percent(-50), translate.YOffset);
            
            toParse = "translateX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(42)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(42), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(-2.1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(-2.1), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(3), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(42)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(42.0), translate.YOffset);
            
            toParse = "translateY(-2.1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(-2.1), translate.YOffset);
            
            toParse = "translateY(3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(3), translate.YOffset);
            
            toParse = "matrix(1, 0, 0, 1, 0, 0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(1.0, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.0, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(0.0, matrix.MatrixValues[4]);
            Assert.AreEqual(0.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.4, 0, 0.5, 1.2, 60, 10)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.4, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.5, matrix.MatrixValues[2]);
            Assert.AreEqual(1.2, matrix.MatrixValues[3]);
            Assert.AreEqual(60.0, matrix.MatrixValues[4]);
            Assert.AreEqual(10.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.1, 1, -0.3, 1, 20, 20.2)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.1, matrix.MatrixValues[0]);
            Assert.AreEqual(1.0, matrix.MatrixValues[1]);
            Assert.AreEqual(-0.3, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(20.0, matrix.MatrixValues[4]);
            Assert.AreEqual(20.2, matrix.MatrixValues[5]);
        }

        [TestMethod]
        public void SVGTransformParsingSpaces()
        {
            //rotate examples
            string toParse = "rotate(3.142)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var root = parsed.Root;
            var rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, rotate.AngleRadians);
            
            toParse = "rotate(-0.5turn) ";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(-3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "rotate(180deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            rotate = root as TransformRotateOperation;
            Assert.IsNotNull(rotate);
            
            Assert.AreEqual(3.142, Math.Round(rotate.AngleRadians, 3));

            toParse = "scale(1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.0, Math.Round(scale.XScaleValue));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue));
            
            toParse = "scale(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.7, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(1.3 0.4)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(0.4, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scale(-0.5 1)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(0.7)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(0.7, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            
            toParse = "scaleX(1.3)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(1.3, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "scaleX(-0.5)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            scale = root as TransformScaleOperation;
            Assert.IsNotNull(scale);
            
            Assert.AreEqual(-0.5, Math.Round(scale.XScaleValue, 5));
            Assert.AreEqual(1.0, Math.Round(scale.YScaleValue, 5));
            
            toParse = "skew(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(15deg 15deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            const double Deg2Rad = Math.PI / 180.0;

            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(15 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));

            toParse = "skew(-0.06turn 18deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(Math.Round((-0.06 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(Math.Round(18 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skew(.312rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.312, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.312, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewX(-0.6turn)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);
            
            toParse = "skewX(35deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0.0, skew.YAngleRadians);

            toParse = "skewX(.234rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.YAngleRadians, 5));
            
            toParse = "skewY(-0.6turn)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round((-0.6 * 360) * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);
            
            toParse = "skewY(35deg)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            
            Assert.AreEqual(Math.Round(35 * Deg2Rad, 5), Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0.0, skew.XAngleRadians);

            toParse = "skewY(.234rad)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            skew = root as TransformSkewOperation;
            Assert.IsNotNull(skew);
            
            Assert.AreEqual(0.234, Math.Round(skew.YAngleRadians, 5));
            Assert.AreEqual(0, Math.Round(skew.XAngleRadians, 5));
            
            toParse = "translate(200px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(200), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);

            toParse = "translate(50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(50), translate.XOffset);
            Assert.AreEqual(Unit.Zero, translate.YOffset);
            
            toParse = "translate(100pt 200pt)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Pt(100), translate.XOffset);
            Assert.AreEqual(Unit.Pt(200), translate.YOffset);
            
            toParse = "translate(100px 50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Px(100), translate.XOffset);
            Assert.AreEqual(Unit.Percent(50), translate.YOffset);
            
            toParse = "translate(-30%  210.5px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(-30), translate.XOffset);
            Assert.AreEqual(Unit.Px(210.5), translate.YOffset);
            
            toParse = "translate(30%    -50%)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            

            Assert.AreEqual(Unit.Percent(30), translate.XOffset);
            Assert.AreEqual(Unit.Percent(-50), translate.YOffset);
            
            toParse = "translateX(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(42px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Px(42.0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(-2.1rem)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.RootEm(-2.1), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateX(3ch)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Ch(3), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Pt(0), translate.YOffset);
            
            toParse = "translateY(42px)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Px(42.0), translate.YOffset);
            
            toParse = "translateY(-2.1rem)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);

            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.RootEm(-2.1), translate.YOffset);
            
            toParse = "translateY(3ch)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            translate = root as TransformTranslateOperation;
            Assert.IsNotNull(translate);
            
            Assert.AreEqual(Unit.Pt(0), translate.XOffset);
            Assert.AreEqual(Unit.Ch(3), translate.YOffset);
            
            toParse = "matrix(1 0 0 1 0 0)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            var matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(1.0, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.0, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(0.0, matrix.MatrixValues[4]);
            Assert.AreEqual(0.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.4  0  0.5  1.2 60 10)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.4, matrix.MatrixValues[0]);
            Assert.AreEqual(0.0, matrix.MatrixValues[1]);
            Assert.AreEqual(0.5, matrix.MatrixValues[2]);
            Assert.AreEqual(1.2, matrix.MatrixValues[3]);
            Assert.AreEqual(60.0, matrix.MatrixValues[4]);
            Assert.AreEqual(10.0, matrix.MatrixValues[5]);
            
            toParse = "matrix(0.1, 1 -0.3, 1 20 20.2)";
            parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            root = parsed.Root;
            matrix = root as Scryber.Drawing.TransformMatrixOperation;
            Assert.IsNotNull(matrix);
            
            Assert.AreEqual(6, matrix.MatrixValues.Length);
            Assert.AreEqual(0.1, matrix.MatrixValues[0]);
            Assert.AreEqual(1.0, matrix.MatrixValues[1]);
            Assert.AreEqual(-0.3, matrix.MatrixValues[2]);
            Assert.AreEqual(1.0, matrix.MatrixValues[3]);
            Assert.AreEqual(20.0, matrix.MatrixValues[4]);
            Assert.AreEqual(20.2, matrix.MatrixValues[5]);
        }
        [TestMethod]
        public void SVGTransformParsingChained()
        {
            string toParse = " translate(100pt, 30pt) rotate(20deg) scale(2) translate(-100pt, 30pt)";
            
            var parsed = SVGTransformOperationSet.Parse(toParse);
            Assert.IsNotNull(parsed);
            Assert.IsNotNull(parsed.Root);
            var curr = parsed.Root;
            
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Translation);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Rotate);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Scaling);
            
            curr = curr.NextOp;
            Assert.IsNotNull(curr);
            Assert.AreEqual(curr.OperationType, MatrixTransformTypes.Translation);
            
            Assert.IsNull(curr.NextOp);
        }

        [TestMethod]
        public void SVGTransformOriginParsing()
        {
            
            
            Unit zeroPcnt = Unit.Percent(0);
            Unit fiftyPcnt = Unit.Percent(50.0);
            Unit hundredPcnt = Unit.Percent(100.0);
            
            string toParse = "2px";
            
            var parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(Unit.Px(2.0), parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "center";

            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(fiftyPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "center top";
            
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(fiftyPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(zeroPcnt, parsed.VerticalOrigin);

            toParse = "left top";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(zeroPcnt, parsed.VerticalOrigin);
            
            toParse = "left center";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);
            
            toParse = "center left";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);

            toParse = "100 left";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(zeroPcnt, parsed.HorizontalOrigin);
            Assert.AreEqual(100, parsed.VerticalOrigin);

            
            toParse = "100 bottom";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(hundredPcnt, parsed.VerticalOrigin);
            
            toParse = "100 center";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(fiftyPcnt, parsed.VerticalOrigin);
            
            toParse = "100 20px";
            parsed = TransformOrigin.Parse(toParse);
            Assert.IsNotNull(parsed);
            
            Assert.AreEqual(100, parsed.HorizontalOrigin);
            Assert.AreEqual(Unit.Px(20), parsed.VerticalOrigin);
        }
        
        

        
        [TestMethod]
        public void SVGTransformFlattenRelative()
        {
            string transformOps = "translate(20%, 10%)";
            var rectStyle = new StyleDefn(".rect");
            rectStyle.SetValue(StyleKeys.TransformOperationKey, SVGTransformOperationSet.Parse(transformOps));

            
            var doc = new HTMLDocument();
            doc.Styles.Add(rectStyle);
            
            var body = new HTMLBody()
            {
                Margins = Thickness.Empty(),
                Padding = Thickness.Empty()
            };
            
            doc.Body = body;

            var svg = new SVGCanvas()
            {
                Width = 300, 
                Height = 400, 
                BorderColor = StandardColors.Red, 
                BorderWidth = 1
            };
            body.Contents.Add(svg);

            SVGPath path = new SVGPath()
            {
                PathData = Scryber.Drawing.GraphicsPath.Parse("M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z"),
                StyleClass = "rect"
            };
            svg.Contents.Add(path);
            
            using (var stream = DocStreams.GetOutputStream("SVGTransformTranslateRelative.pdf"))
            {
                body.Style.OverlayGrid.ShowGrid = true;
                body.Style.OverlayGrid.GridColor = StandardColors.Aqua;
                body.Style.OverlayGrid.GridMajorCount = 5;
                body.Style.OverlayGrid.GridSpacing = 10;
                
                doc.SaveAsPDF(stream);
            }

            var arrange = path.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var full = arrange.FullStyle;
            Assert.IsNotNull(full);
            Assert.IsTrue(full.IsValueDefined(StyleKeys.TransformOperationKey));
            var set = full.GetValue(StyleKeys.TransformOperationKey, null);
            Assert.IsNotNull(set);
            Assert.IsNotNull(set.Root);
            var transform = set.Root as TransformTranslateOperation;
            Assert.IsNotNull(transform);
            var bounds = path.PathData.Bounds;
            Assert.AreEqual(Unit.Pt(60), transform.XOffset); //20% of 300
            Assert.AreEqual(Unit.Pt(40), transform.YOffset); //10% of 300
            Assert.Inconclusive("Need to update the arrangement bounds based on the transfomation");
            
            Assert.AreEqual(bounds.X + 60, arrange.RenderBounds.X); //bounds.x + XOffset
            Assert.AreEqual(bounds.Y + 40, arrange.RenderBounds.Y); //bounds.y + YOffset
            Assert.AreEqual(bounds.Width, arrange.RenderBounds.Width); //unchanged
            Assert.AreEqual(bounds.Height, arrange.RenderBounds.Height);
            
        }
        

        
        [TestMethod]
        public void SVGTransformOperationRotateDegrees()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateDegrees.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateDegrees.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) *15.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 30.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 45.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 60.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 75.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 90.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * -15.0, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }

        [TestMethod]
        public void SVGTransformOperationRotateRadians()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateRadians.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateRadians.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.26, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.52, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(0.78, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.04, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.40, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(1.66, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual(-0.26, rotate.AngleRadians);
                Assert.IsNull(rotate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationTranslate()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_Translate.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationTranslate.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(0, translate.YOffset);
                Assert.IsNull(translate.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(0, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                translate = transform.Root as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(-50, translate.XOffset);
                Assert.AreEqual(0, translate.YOffset);
                Assert.IsNull(translate.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationScaleOne()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_ScaleOne.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationScaleOne.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(2.0, scale.XScaleValue);
                Assert.AreEqual(2.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(3.0, scale.XScaleValue);
                Assert.AreEqual(3.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.5, scale.XScaleValue);
                Assert.AreEqual(0.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationScaleBoth()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_ScaleBoth.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationScaleBoth.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(0.75, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(2.0, scale.XScaleValue);
                Assert.AreEqual(1.0, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(3.0, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                scale = transform.Root as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.85, scale.XScaleValue);
                Assert.AreEqual(0.65, scale.YScaleValue);
                Assert.IsNull(scale.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationSkewOneX()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewOneX.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewOneX.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(0, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        [TestMethod]
        public void SVGTransformOperationSkewOneY()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewOneY.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewOneY.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.YAngleRadians);
                Assert.AreEqual(0, skew.XAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationSkewBoth()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_SkewBoth.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationSkewBoth.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;
                const double Deg2Rad = Math.PI / 180;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(10 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(40 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(20 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(60 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(30 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                skew = transform.Root as TransformSkewOperation;
                Assert.IsNotNull(skew);
                Assert.AreEqual(-20 * Deg2Rad, skew.XAngleRadians);
                Assert.AreEqual(-10 * Deg2Rad, skew.YAngleRadians);
                Assert.IsNull(skew.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
            }
        }
        
        
        [TestMethod]
        public void SVGTransformOperationMatrix()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_Matrix.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationMatrix.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);

                var use1 = doc.FindAComponentById("Poly1") as SVGUse;
                Assert.IsNotNull(use1);
                
                var use2 = doc.FindAComponentById("Poly2") as SVGUse;
                Assert.IsNotNull(use2);
                
                var use3 = doc.FindAComponentById("Poly3") as SVGUse;
                Assert.IsNotNull(use3);
                
                var poly_1 = doc.FindAComponentById("Poly-1") as SVGPolyLine;
                Assert.IsNotNull(poly_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 20, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = use1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use1.Contents.Count);
                var poly1 = use1.Contents[0];
                
                arrange = poly1.GetFirstArrangement();
                
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                var comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.2, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.2, comps[3]);
                Assert.AreEqual(30.0, comps[4]);
                Assert.AreEqual(40.0, comps[5]);
                Assert.IsNull(matrix.NextOp);

                arrange = use2.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use2.Contents.Count);
                var poly2 = use2.Contents[0];
                
                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.4, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.4, comps[3]);
                Assert.AreEqual(40.0, comps[4]);
                Assert.AreEqual(50.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                arrange = use3.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use3.Contents.Count);
                var poly3 = use3.Contents[0];
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(1.6, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(1.6, comps[3]);
                Assert.AreEqual(50.0, comps[4]);
                Assert.AreEqual(60.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                matrix = transform.Root as Scryber.Drawing.TransformMatrixOperation;
                Assert.IsNotNull(matrix);
                comps = matrix.MatrixValues;
                Assert.AreEqual(6, comps.Length);
                Assert.AreEqual(0.8, comps[0]);
                Assert.AreEqual(1, comps[1]);
                Assert.AreEqual(-1, comps[2]);
                Assert.AreEqual(0.8, comps[3]);
                Assert.AreEqual(10.0, comps[4]);
                Assert.AreEqual(20.0, comps[5]);
                Assert.IsNull(matrix.NextOp);
                
                bounds = arrange.RenderBounds;
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }

        
        [TestMethod]
        public void SVGTransformOperationMultipleChained()
        {

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_MultipleChained.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;
                
                
                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationMultipleChained.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }

                var poly = doc.FindAComponentById("Poly");
                Assert.IsNotNull(poly);
                
                var poly2 = doc.FindAComponentById("Poly2");
                Assert.IsNotNull(poly2);
                
                var poly3 = doc.FindAComponentById("Poly3");
                Assert.IsNotNull(poly3);
                
                var poly4 = doc.FindAComponentById("Poly4");
                Assert.IsNotNull(poly4);
                
                var poly5 = doc.FindAComponentById("Poly5");
                Assert.IsNotNull(poly5);
                
                var poly6 = doc.FindAComponentById("Poly6");
                Assert.IsNotNull(poly6);
                
                var poly7 = doc.FindAComponentById("Poly7");
                Assert.IsNotNull(poly7);

                var use_1 = doc.FindAComponentById("Poly-1") as SVGUse;
                Assert.IsNotNull(use_1);

                Unit marginX = 20;
                Unit marginY = 20;
                Rect orig;

                var arrange = poly.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var bounds = arrange.RenderBounds;
                orig = bounds;
                
                Assert.AreEqual(marginX + 50, bounds.X);
                Assert.AreEqual(marginY + 0, bounds.Y);
                Assert.AreEqual(50, bounds.Width);
                Assert.AreEqual(50, bounds.Height);

                arrange = poly2.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                var transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                var rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) *10.0, rotate.AngleRadians);
                var translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(10, translate.XOffset);
                Assert.AreEqual(20, translate.YOffset);
                var scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.1, scale.XScaleValue);
                Assert.AreEqual(1.1, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                //TODO: check the render bounds
                
                arrange = poly3.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 20.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(20, translate.XOffset);
                Assert.AreEqual(30, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.2, scale.XScaleValue);
                Assert.AreEqual(1.2, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly4.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 30.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(30, translate.XOffset);
                Assert.AreEqual(40, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.3, scale.XScaleValue);
                Assert.AreEqual(1.3, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly5.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 40.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(40, translate.XOffset);
                Assert.AreEqual(50, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.4, scale.XScaleValue);
                Assert.AreEqual(1.4, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly6.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 50.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(50, translate.XOffset);
                Assert.AreEqual(60, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.5, scale.XScaleValue);
                Assert.AreEqual(1.5, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                arrange = poly7.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * 60.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(60, translate.XOffset);
                Assert.AreEqual(70, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(1.6, scale.XScaleValue);
                Assert.AreEqual(1.6, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;

                arrange = use_1.GetFirstArrangement();
                Assert.IsNull(arrange);
                Assert.AreEqual(1, use_1.Contents.Count);
                var poly_1 = use_1.Contents[0];
                
                arrange = poly_1.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                transform = arrange.FullStyle.GetValue(StyleKeys.TransformOperationKey, null);
                Assert.IsNotNull(transform);
                rotate = transform.Root as TransformRotateOperation;
                Assert.IsNotNull(rotate);
                Assert.AreEqual((Math.PI / 180 ) * -10.0, rotate.AngleRadians);
                Assert.IsNotNull(rotate.NextOp);
                translate = rotate.NextOp as TransformTranslateOperation;
                Assert.IsNotNull(translate);
                Assert.AreEqual(-10, translate.XOffset);
                Assert.AreEqual(-20, translate.YOffset);
                scale = translate.NextOp as TransformScaleOperation;
                Assert.IsNotNull(scale);
                Assert.AreEqual(0.9, scale.XScaleValue);
                Assert.AreEqual(0.9, scale.YScaleValue);
                
                bounds = arrange.RenderBounds;
                
                
                //TODO: Calculate the outer resultant render matrix for each of the palylines.
                
                
            }
        }


        private ComponentArrangement AssertGetArrangement(Document doc, string compId, Type compType)
        {
            var comp = doc.FindAComponentById(compId);
            Assert.IsNotNull(comp);
            Assert.IsInstanceOfType(comp, compType);
            var arrange = comp.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            return arrange;
        }

        private TransformOperationSet AssertGetTransform(ComponentArrangement arrangement)
        {
            var style = arrangement.FullStyle;
            Assert.IsNotNull(style);
            var set = style.GetValue(StyleKeys.TransformOperationKey, null);
            Assert.IsNotNull(set);
            return set;
        }
        
        private void AssertTransformTranslate(TransformOperation op, Unit x, Unit y, bool hasNext)
        {
            var tranlate = op as TransformTranslateOperation;
            Assert.IsNotNull(tranlate);
            Assert.AreEqual(x, tranlate.XOffset);
            Assert.AreEqual(y, tranlate.YOffset);
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        private void AssertTransformSkew(TransformOperation op, double xdeg, double ydeg, bool hasNext)
        {
            var x = (Math.PI / 180.0) * xdeg;
            var y = (Math.PI / 180.0) * ydeg;
            
            var skew = op as TransformSkewOperation;
            Assert.IsNotNull(skew);
            Assert.AreEqual(x, skew.XAngleRadians);
            Assert.AreEqual(y, skew.YAngleRadians);
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        private void AssertTransformScale(TransformOperation op, double xscale, double yscale, bool hasNext)
        {
            
            var scale = op as TransformScaleOperation;
            Assert.IsNotNull(scale);
            Assert.AreEqual(xscale, scale.XScaleValue);
            Assert.AreEqual(yscale, scale.YScaleValue);
            
            if(hasNext)
                Assert.IsNotNull(op.NextOp);
            else
                Assert.IsNull(op.NextOp);
        }
        
        

        [TestMethod]
        public void SVGTransformOperationComponents()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_TransformComponents.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;


                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationTransformComponents.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.AppendTraceLog = false;
                    doc.SaveAsPDF(stream);
                }
                
                //Anchor link

                var arrange = AssertGetArrangement(doc, "link", typeof(SVGAnchor));
                var set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 0, 70, true);
                AssertTransformSkew(set.Root.NextOp, 30, 0, false);
                //Circle
                
                arrange = AssertGetArrangement(doc, "circ", typeof(SVGCircle));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 75, 90, true);
                AssertTransformSkew(set.Root.NextOp, 30, 0, false);

                //Ellipse
                
                arrange = AssertGetArrangement(doc, "elli", typeof(SVGEllipse));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 120, 90, true);
                AssertTransformSkew(set.Root.NextOp, 0, 30, false);
                
                //Group
                
                arrange = AssertGetArrangement(doc, "grp", typeof(SVGGroup));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 130, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);

                //Rect
                
                arrange = AssertGetArrangement(doc, "rect", typeof(SVGRect));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 10, 25, true);
                AssertTransformScale(set.Root.NextOp, 4, 1, false);
                
                //Line
                
                arrange = AssertGetArrangement(doc, "ln", typeof(SVGLine));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 180, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);
                
                //Polyline
                
                arrange = AssertGetArrangement(doc, "poly", typeof(SVGPolyLine));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 195, 60, true);
                AssertTransformSkew(set.Root.NextOp, 40, 0, false);

                //Polygon
                
                arrange = AssertGetArrangement(doc, "polyG", typeof(SVGPolygon));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 20, 60, true);
                AssertTransformSkew(set.Root.NextOp, -50, 0, false);
                
                //Path
                
                arrange = AssertGetArrangement(doc, "pth", typeof(SVGPath));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 225, 60, true);
                AssertTransformScale(set.Root.NextOp, 0.2, 0.2, true);
                AssertTransformSkew(set.Root.NextOp.NextOp, 30, 0, false);
                
                //Image
                
                arrange = AssertGetArrangement(doc, "img", typeof(SVGImage));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 15, 125, true);
                AssertTransformScale(set.Root.NextOp, 1.3, 0.1, true);
                AssertTransformSkew(set.Root.NextOp.NextOp, -10, 0, false);
                
                
                //Use - arrangement is null put style is passed down to the referenced component
                var use = doc.FindAComponentById("use") as SVGUse;
                Assert.IsNotNull(use);
                arrange = use.GetFirstArrangement();
                Assert.IsNull(arrange);
                var rect = use.Contents[0] as SVGRect;
                Assert.IsNotNull(rect);
                arrange = rect.GetFirstArrangement();
                Assert.IsNotNull(arrange);
                
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 120, 150, true);
                AssertTransformScale(set.Root.NextOp, 2, 0.2, false);
                
                //text
                
                arrange = AssertGetArrangement(doc, "txt", typeof(SVGText));
                set = AssertGetTransform(arrange);
                AssertTransformTranslate(set.Root, 10, 150, true);
                AssertTransformSkew(set.Root.NextOp, 25, 0, true);
                AssertTransformScale(set.Root.NextOp.NextOp, 1, -1, false);


            }
            
            
        }

        private void AssertOriginAreEqual(SVGPolyLine poly, Unit h, Unit v)
        {
            Assert.IsNotNull(poly);
            var arrange = poly.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var style = arrange.FullStyle;
            Assert.IsNotNull(style);
            var origin = style.GetValue(StyleKeys.TransformOriginKey, null);
            Assert.IsNotNull(origin);
            Assert.AreEqual(h, origin.HorizontalOrigin);
            Assert.AreEqual(v, origin.VerticalOrigin);
            
        }

        [TestMethod]
        public void SVGTransformRotateOrigins()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/SVGTransform_RotateOrigins.html",
                this.TestContext);
            using (var doc = Document.ParseDocument(path))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridOpacity = 0.5;


                using (var stream = DocStreams.GetOutputStream("SVGTransformOperationRotateOrigins.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
                
                //Left Top

                var svg = doc.FindAComponentById("Canvas11");
                var poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                var poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                var poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 0);
                AssertOriginAreEqual(poly2, 0, 0);
                AssertOriginAreEqual(poly3, 0, 0);

                // Center Top
                
                svg = doc.FindAComponentById("Canvas12");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 0);
                AssertOriginAreEqual(poly2, 150, 0);
                AssertOriginAreEqual(poly3, 150, 0);
                
                // Right Top
                
                svg = doc.FindAComponentById("Canvas13");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 0);
                AssertOriginAreEqual(poly2, 300, 0);
                AssertOriginAreEqual(poly3, 300, 0);
                
                //Left Center

                svg = doc.FindAComponentById("Canvas21");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 150);
                AssertOriginAreEqual(poly2, 0, 150);
                AssertOriginAreEqual(poly3, 0, 150);

                // Center Center
                
                svg = doc.FindAComponentById("Canvas22");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 150);
                AssertOriginAreEqual(poly2, 150, 150);
                AssertOriginAreEqual(poly3, 150, 150);
                
                // Right Center
                
                svg = doc.FindAComponentById("Canvas23");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 150);
                AssertOriginAreEqual(poly2, 300, 150);
                AssertOriginAreEqual(poly3, 300, 150);
                
                //Left Bottom

                svg = doc.FindAComponentById("Canvas31");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 0, 300);
                AssertOriginAreEqual(poly2, 0, 300);
                AssertOriginAreEqual(poly3, 0, 300);

                // Center Bottom
                
                svg = doc.FindAComponentById("Canvas32");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 150, 300);
                AssertOriginAreEqual(poly2, 150, 300);
                AssertOriginAreEqual(poly3, 150, 300);
                
                // Right Bottom
                
                svg = doc.FindAComponentById("Canvas33");
                poly1 = svg.FindAComponentById("Poly1") as SVGPolyLine;
                poly2 = svg.FindAComponentById("Poly2") as SVGPolyLine;
                poly3 = svg.FindAComponentById("Poly3") as SVGPolyLine;

                AssertOriginAreEqual(poly1, 300, 300);
                AssertOriginAreEqual(poly2, 300, 300);
                AssertOriginAreEqual(poly3, 300, 300);
            }
            
            
        }
        
        [TestMethod]
        public void SVGInsert()
        {
            var doc = new Document();
            var pg1 = new Page();
            doc.Pages.Add(pg1);

            var svgString = @"
            <svg xmlns=""http://www.w3.org/2000/svg"" style=""border:solid 1px black"" >
                <rect x=""0pt"" y=""0pt"" width=""100pt"" height=""80pt"" fill=""lime"" ></rect>
                <g id=""eye"" stroke=""black"" stroke-width=""2pt"" >
                    <ellipse cx=""50pt"" cy=""40pt"" rx=""40pt"" ry=""20pt"" fill=""white""></ellipse>
                    <circle cx=""50pt"" cy=""40pt"" r=""20pt"" fill=""#66F""></circle>
                    <circle cx=""50pt"" cy=""40pt"" r=""10pt"" fill=""black""></circle>
                    <line x1=""10"" x2=""90"" y1=""40"" y2=""40"" />
                    <line x1=""50"" x2=""50"" y1=""20"" y2=""60"" />
                </g>
            </svg>";



            //1. Explicit parsing of the content and adding to the page.
            //Allows handling of any parsing errors independently of the document flow.
            pg1.Contents.Add("The parsed component will be explicitly added after this.");
            var icomp = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            pg1.Contents.Add(icomp as Component);
            pg1.Contents.Add("This is after the parsed component");

            //2. Appending the data content to the page
            //Simply setting the DataContent to a string and the action as append.
            var pg2 = new Page();
            doc.Pages.Add(pg2);

            pg2.Contents.Add("The SVG content will be appended to the page.");
            pg2.DataContent = svgString;
            pg2.DataContentAction = DataContentAction.Append;
            pg2.Contents.Add(" Will still appear before the data content.");

            
            var pg3 = new Page();
            doc.Pages.Add(pg3);

            //3. Adding the content to an inner div (with some margins)
            //Gives flexibility in positioning and document flow.
            var div = new Div();
            div.DataContent = svgString;

            div.Margins = new Thickness(10);
            pg3.Contents.Add("Before the div with the inner svg");
            pg3.Contents.Add(div);
            pg3.Contents.Add("After the div with the inner svg");

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGInserted.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }


            if (null == layout)
                throw new InvalidOperationException("Layout was not set");

            Assert.AreEqual(3, pg1.Contents.Count);
            var svg = pg1.Contents[1] as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(3, pg2.Contents.Count);
            svg = pg2.Contents[2] as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(3, pg3.Contents.Count);
            div = pg3.Contents[1] as Div;
            Assert.IsNotNull(div);
            Assert.AreEqual(1, div.Contents.Count);
            svg = div.Contents[0] as SVGCanvas;
            Assert.IsNotNull(svg);
        }
        
        [TestMethod]
        public void SVGFillColor()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <rect id='rect1' width=""100"" height=""80"" x=""10"" y=""10"" fill=""red"" fill-opacity=""1""></rect>
              <rect id='rect2' width=""100"" height=""80"" x=""10"" y=""80"" fill=""#ABABAB"" fill-opacity=""0.5""></rect>
            </svg>
";
            
            SVGCanvas svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            Assert.AreEqual(5, svg.Contents.Count);
            //0 = whitespace
            var rect1 = svg.Contents[1] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var colVal = rect1.FillValue as SVGFillColorValue;
            Assert.IsNotNull(colVal);
            Assert.AreEqual(StandardColors.Red, colVal.FillColor);
            Assert.AreEqual("red", colVal.Value);
            
            //2 = whitespace

            var rect2 = svg.Contents[3] as SVGRect;
            Assert.IsNotNull(rect2);
            Assert.AreEqual("rect2", rect2.ID);
            Assert.IsNotNull(rect2.FillValue);
            
            colVal = rect2.FillValue as SVGFillColorValue;
            Assert.IsNotNull(colVal);
            Assert.AreEqual("rgb(171,171,171)", colVal.FillColor.ToString());
            Assert.AreEqual("#ABABAB", colVal.Value);
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillColor.pdf"))
            {
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            Assert.IsNotNull(brush);
            Assert.AreEqual(StandardColors.Red, brush.Color);
            Assert.AreEqual(1.0, brush.Opacity);

            line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun2);
            Assert.AreEqual(rect2, compRun2.Owner);
            arrange = rect2.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            brush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            Assert.IsNotNull(brush);
            Assert.AreEqual("rgb(171,171,171)", brush.Color.ToString());
            Assert.AreEqual(0.5, brush.Opacity);
        }
        
        [TestMethod]
        public void SVGFillLinearGradient()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <defs>
                <linearGradient id=""grad1"" x1=""0"" x2=""0.5"" y1=""0"" y2=""1"">
                    <stop offset=""0.3"" stop-color=""red"" />
                    <stop offset=""0.5"" stop-color=""white""  />
                    <stop offset=""0.7"" stop-color=""blue"" />
                </linearGradient>
               </defs>
              <rect id='rect1' width=""100"" height=""80"" x=""10"" y=""10"" fill=""url(#grad1)"" fill-opacity=""1""></rect>
            </svg>
";
            
            SVGCanvas svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            Assert.AreEqual(4, svg.Contents.Count);
            
            //0 = whitespace
            var defs = svg.Definitions;
            Assert.IsNotNull(defs);
            Assert.AreEqual(3, defs.Count);
            var grad = defs[1] as SVGLinearGradient;
            Assert.AreEqual("grad1", grad.ID);
            
            var rect1 = svg.Contents[2] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var gradVal = rect1.FillValue as SVGFillReferenceValue;
            Assert.IsNotNull(gradVal);
            Assert.IsNull(gradVal.Adapter); //should be null at this stage
            Assert.AreEqual("#grad1", gradVal.Value);
            
            //3 = whitespace

            
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillLinearGradient.pdf"))
            {
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFBrush;
            var gradbrush = brush as PDFGradientLinearBrush;
            Assert.IsNotNull(gradbrush);
            
            Assert.AreEqual(5, gradbrush.Colors.Length);
            
            //auto added for padding
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[0].Color);
            Assert.AreEqual(0.0, gradbrush.Colors[0].Distance.Value);
            
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[1].Color);
            Assert.AreEqual(0.3, gradbrush.Colors[1].Distance.Value);
            
            Assert.AreEqual(StandardColors.White, gradbrush.Colors[2].Color);
            Assert.AreEqual(0.5, gradbrush.Colors[2].Distance.Value);
            
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[3].Color);
            Assert.AreEqual(0.7, gradbrush.Colors[3].Distance.Value);
            
            //Auto added for padding
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[4].Color);
            Assert.AreEqual(1.0, gradbrush.Colors[4].Distance.Value);
            
            //Assert.AreEqual(StandardColors.Red, brush.Color);
            //Assert.AreEqual(1.0, brush.Opacity);

            //line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            //Assert.IsNotNull(line);
            //var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            //Assert.IsNotNull(compRun2);
            
            //Assert.AreEqual(rect2, compRun2.Owner);
            //arrange = rect2.GetFirstArrangement();
            //Assert.IsNotNull(arrange);
            //var solidbrush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            //Assert.IsNotNull(brush);
            //Assert.AreEqual("rgb(171,171,171)", solidbrush.Color.ToString());
            //Assert.AreEqual(0.5, solidbrush.Opacity);
        }
        
        [TestMethod]
        public void SVGFillRepeatingLinearGradient()
        {
            var svgString = @"
<svg xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""border: solid 1px
                lime;"" >
              <defs>
                <linearGradient id=""grad1"" x1=""0"" x2=""1"" y1=""0.5"" y2=""0.3"" spreadMethod=""pad""  >
                    <stop offset=""0.1"" stop-color=""red"" />
                    <stop offset=""0.5"" stop-color=""white""  />
                    <stop offset=""1"" stop-color=""blue"" />
                </linearGradient>
               </defs>
              <rect id='rect1' width=""100"" height=""100"" x=""10"" y=""10"" fill=""url(#grad1)"" fill-opacity=""1""></rect>
            </svg>
";
            
            SVGCanvas svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            Assert.AreEqual(4, svg.Contents.Count);
            
            //0 = whitespace
            var defs = svg.Definitions;
            Assert.IsNotNull(defs);
            Assert.AreEqual(3, defs.Count);
            var grad = defs[1] as SVGLinearGradient;
            Assert.AreEqual("grad1", grad.ID);
            
            var rect1 = svg.Contents[2] as SVGRect;
            Assert.IsNotNull(rect1);
            Assert.AreEqual("rect1", rect1.ID);
            
            Assert.IsNotNull(rect1.FillValue);
            var gradVal = rect1.FillValue as SVGFillReferenceValue;
            Assert.IsNotNull(gradVal);
            Assert.IsNull(gradVal.Adapter); //should be null at this stage
            Assert.AreEqual("#grad1", gradVal.Value);
            
            //3 = whitespace

            
            

            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Margins = 20;

            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 10;
            pg.Style.OverlayGrid.GridMajorCount = 10;

            using (var stream = DocStreams.GetOutputStream("SVGParsing_FillRepeatingLinearGradient.pdf"))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                doc.LayoutComplete += DocOnFillColorLayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layoutDocument);
            Assert.AreEqual(1, _layoutDocument.AllPages.Count);
            var content = _layoutDocument.AllPages[0].ContentBlock;
            Assert.IsNotNull(content);
            Assert.IsTrue(content.HasPositionedRegions);
            var canvas = content.PositionedRegions[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(canvas);
            Assert.AreEqual(svg, canvas.Owner);

            var line = canvas.Columns[0].Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            var compRun1 = line.Runs[0] as PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);
            Assert.AreEqual(rect1, compRun1.Owner);
            var arrange = rect1.GetFirstArrangement();
            Assert.IsNotNull(arrange);
            var brush = arrange.FullStyle.CreateFillBrush() as PDFBrush;
            var gradbrush = brush as PDFGradientLinearBrush;
            Assert.IsNotNull(gradbrush);
            
            Assert.AreEqual(5, gradbrush.Colors.Length);
            
            //auto added for padding
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[0].Color);
            Assert.AreEqual(0.0, gradbrush.Colors[0].Distance.Value);
            
            Assert.AreEqual(StandardColors.Red, gradbrush.Colors[1].Color);
            Assert.AreEqual(0.3, gradbrush.Colors[1].Distance.Value);
            
            Assert.AreEqual(StandardColors.White, gradbrush.Colors[2].Color);
            Assert.AreEqual(0.5, gradbrush.Colors[2].Distance.Value);
            
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[3].Color);
            Assert.AreEqual(0.7, gradbrush.Colors[3].Distance.Value);
            
            //Auto added for padding
            Assert.AreEqual(StandardColors.Blue, gradbrush.Colors[4].Color);
            Assert.AreEqual(1.0, gradbrush.Colors[4].Distance.Value);
            
            //Assert.AreEqual(StandardColors.Red, brush.Color);
            //Assert.AreEqual(1.0, brush.Opacity);

            //line = canvas.Columns[0].Contents[1] as PDFLayoutLine;
            //Assert.IsNotNull(line);
            //var compRun2 = line.Runs[0] as PDFLayoutComponentRun;
            //Assert.IsNotNull(compRun2);
            
            //Assert.AreEqual(rect2, compRun2.Owner);
            //arrange = rect2.GetFirstArrangement();
            //Assert.IsNotNull(arrange);
            //var solidbrush = arrange.FullStyle.CreateFillBrush() as PDFSolidBrush;
            //Assert.IsNotNull(brush);
            //Assert.AreEqual("rgb(171,171,171)", solidbrush.Color.ToString());
            //Assert.AreEqual(0.5, solidbrush.Opacity);
        }

        private PDF.Layout.PDFLayoutDocument _layoutDocument;
        
        private void DocOnFillColorLayoutComplete(object sender, LayoutEventArgs args)
        {
            _layoutDocument = args.Context.GetLayout<PDFLayoutDocument>();
        }

        /// <summary>
        /// Test case for Issue #107
        /// </summary>
        [TestMethod]
        public void SVGToComponentWithInvariantUnits()
        {
            var svgString = @"
            <svg width=""1000"" height=""400"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" style=""position:absolute;left:0;top:0;user-select:none"">
              <rect width=""1000"" height=""400"" x=""0"" y=""0"" id=""0"" fill=""red"" fill-opacity=""1""></rect>
              <g>
                <path d=""M503.1463 120.0619L503.7363 105.0735L745 105.0735"" fill=""none"" stroke=""#5470c6""></path>
                <path d=""M504.3059 279.884L505.1132 294.8623L649 294.8623"" fill=""none"" stroke=""#91cc75""></path>
                <path d=""M488.3618 120.8511L417.7603 131.2324L291 131.2324"" fill=""none"" stroke=""#fac858""></path>
                <path d=""M495.7775 120.1115L494.9857 105.1324L315 105.1324"" fill=""none"" stroke=""#ee6666""></path>
                <path d=""M500 120A80 80 0 0 1 506.2878 120.2475L500 200Z"" fill=""#5470c6"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M506.2878 120.2475A80 80 0 1 1 485.1759 121.3855L500 200Z"" fill=""#91cc75"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M485.1759 121.3855A80 80 0 0 1 491.5667 120.4457L500 200Z"" fill=""#fac858"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <path d=""M491.5667 120.4457A80 80 0 0 1 500 120L500 200Z"" fill=""#ee6666"" stroke=""#fff"" stroke-linejoin=""round""></path>
                <text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(750 294.8623)"" fill=""black"">Airplane</text>
                <text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(250 131.2324)"" fill=""black"">Car</text>
                <text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(250 105.1324)"" fill=""black"">Train</text>
              </g>
            </svg>";


            SVGCanvas svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            var grp = svg.Contents[3] as SVGGroup;
            Assert.IsNotNull(grp);

            var path = grp.Contents[1] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            var paths = path.PathData.SubPaths.ToArray();
            Assert.AreEqual(1, paths.Length);

            var opPath = paths[0];
            Assert.AreEqual(3, opPath.Operations.Count);

            var opMove = opPath.Operations[0] as PathMoveData;
            Assert.IsNotNull(opMove);
            Assert.AreEqual(503.1463, opMove.MoveTo.X.PointsValue);
            Assert.AreEqual(120.0619, opMove.MoveTo.Y.PointsValue);

            var opLineTo = opPath.Operations[1] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(503.7363, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            opLineTo = opPath.Operations[2] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(745, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            var origCult = System.Threading.Thread.CurrentThread.CurrentCulture;
            var origUICult = System.Threading.Thread.CurrentThread.CurrentUICulture;

            //
            //Switch to the german culture
            //

            var german = System.Globalization.CultureInfo.CreateSpecificCulture("de-DE");
            System.Threading.Thread.CurrentThread.CurrentCulture = german;
            System.Threading.Thread.CurrentThread.CurrentUICulture = german;


            svg = null;
            try
            {
                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                svg = (SVGCanvas)component;
                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));
            }
            catch
            {
                Assert.Fail("Svg image has not been parsed");
            }

            grp = svg.Contents[3] as SVGGroup;
            Assert.IsNotNull(grp);

            path = grp.Contents[1] as SVGPath;
            Assert.IsNotNull(path);
            Assert.IsNotNull(path.PathData);
            paths = path.PathData.SubPaths.ToArray();
            Assert.AreEqual(1, paths.Length);

            opPath = paths[0];
            Assert.AreEqual(3, opPath.Operations.Count);

            opMove = opPath.Operations[0] as PathMoveData;
            Assert.IsNotNull(opMove);
            Assert.AreEqual(503.1463, opMove.MoveTo.X.PointsValue);
            Assert.AreEqual(120.0619, opMove.MoveTo.Y.PointsValue);

            opLineTo = opPath.Operations[1] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(503.7363, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);

            opLineTo = opPath.Operations[2] as PathLineData;
            Assert.IsNotNull(opLineTo);
            Assert.AreEqual(745, opLineTo.LineTo.X.PointsValue);
            Assert.AreEqual(105.0735, opLineTo.LineTo.Y.PointsValue);


            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            pg.PaperOrientation = PaperOrientation.Landscape;
            pg.Contents.Add(svg);

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGInvariant.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var first = layout.AllPages[0];
            Assert.AreEqual(1, first.ContentBlock.Columns.Length);
            var reg = first.ContentBlock.Columns[0];
            Assert.AreEqual(1, reg.Contents.Count);
            var line = reg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(line);
            Assert.AreEqual(1, line.Runs.Count);

            var run = line.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(run);

            var posReg = run.Region;
            Assert.IsNotNull(posReg);

            Assert.AreEqual(1, posReg.Contents.Count);
            Assert.AreEqual(svg, posReg.Owner);

            System.Threading.Thread.CurrentThread.CurrentUICulture = origUICult;
            System.Threading.Thread.CurrentThread.CurrentCulture = origCult;
        }


        [TestMethod]
        public void SVGWithOverflowClipping()
        {
            var svgString = "";

            try
            {

                var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/SVG/Chart.svg",
                    this.TestContext);
                svgString = System.IO.File.ReadAllText(path);

                var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
                var svg = (SVGCanvas)component;

                Assert.IsInstanceOfType(svg, typeof(SVGCanvas));

                using var doc = new Document()
                {
                    AppendTraceLog = false
                };
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
                doc.RenderOptions.Compression = OutputCompressionType.None;

                var pg = new Page();
                doc.Pages.Add(pg);
                pg.Contents.Add(svg);
                svg.OverflowAction = OverflowAction.Clip;

                foreach (Component item in svg.Contents)
                {
                    if (item is VisualComponent vis)
                        vis.Style.Overflow.Action = OverflowAction.Clip;
                }

                var gStyle = new StyleDefn("g");
                gStyle.Overflow.Action = OverflowAction.Clip;
                
                doc.Styles.Add(gStyle);
                

                PDF.Layout.PDFLayoutDocument layout = null;
                //Output the document (including databinding the data content)
                using (var stream = DocStreams.GetOutputStream("SVGViewboxOverflow.pdf"))
                {
                    doc.LayoutComplete += (sender, args) =>
                    {
                        layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                    };
                    doc.SaveAsPDF(stream);
                }
            }
            catch(Exception ex)
            {
                Assert.Fail("Svg image has not been parsed : " + ex);
            }
        }

        [TestMethod]
        public void SVGTextAnchorOptions()
        {
            var svgString = @"
            <svg width=""500"" height=""400""
                xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full""
                style=""position:absolute;left:10;top:10;user-select:none; border: solid 1px navy;"">
                <path d=""M 100 50 L400 50 M 100 80 L 400 80 M 100 110 L 400 110 M 100 140 L 400 140"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <path d=""M 250 0 L250 200"" fill=""none"" stroke=""#5470c6"" stroke-width=""0.5""></path>
                <g>
              
                  <text x=""250"" y=""50"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Start</text>
                  <text x=""250"" y=""80"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">Middle</text>
                  <text x=""250"" y=""110"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" fill=""black"">End</text>
                  <text x=""250"" y=""140"" style=""font-size:12px;font-family:sans-serif;text-anchor:middle"" fill=""black"">CSS Middle</text>
                </g>
            </svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);

            Assert.AreEqual(7, svg.Contents.Count);
            var group = svg.Contents[5] as SVGGroup;
            Assert.IsNotNull(group);

            Assert.AreEqual(4 * 2 + 1, group.Contents.Count);

            var txt = group.Contents[1] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);

            txt = group.Contents[3] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

            txt = group.Contents[5] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);

            //Set via CSS
            txt = group.Contents[7] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(TextAnchor.Middle, txt.TextAnchor);

            svg.OverflowAction = OverflowAction.Clip;
            

            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);


            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextAnchor.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }


        [TestMethod]
        public void SVGTextDominantBaselineOptions()
        {
            var svgString = @"<svg
                              width=""400""
                              height=""500""
                              viewBox=""0 0 400 500""
                              xmlns=""http://www.w3.org/2000/svg"">
<rect x=""50"" y=""50"" width=""300"" height=""400"" fill=""lime"" fill-opacity=""0.5"" stroke='black' stroke-width='2pt' />
<rect x=""100"" y=""100"" width=""200"" height=""300"" fill=""navy"" fill-opacity=""0.5"" stroke='white' stroke-width='2pt' />
                              <!-- Materialization of anchors -->
                              
                              <path
                                d=""M60,20 L60,470
                                       M30,20 L400,20
                                       M30,70 L400,70
                                       M30,120 L400,120
                                       M30,170 L400,170
                                       M30,220 L400,220
                                       M30,270 L400,270
	                                   M30,320 L400,320
                                       M30,370 L400,370
                                       M30,420 L400,420
                                       M30,470 L400,470
	                               ""
                                stroke=""grey"" />

                              <!-- Anchors in action -->
                              <text dominant-baseline=""auto"" x=""70"" y=""20"">auto</text>
                              <text dominant-baseline=""middle"" x=""70"" y=""70"">middle</text>
                              <text dominant-baseline=""central"" x=""70"" y=""120"">central</text>
                              <text dominant-baseline=""hanging"" x=""70"" y=""170"">hanging</text>
                              <text dominant-baseline=""mathematical"" x=""70"" y=""220"">mathematical</text>
                              <text dominant-baseline=""text-top"" x=""70"" y=""270"" >text-top</text>
                              <text dominant-baseline=""ideographic"" x=""70"" y=""320"">ideographic</text>
                              <text dominant-baseline=""alphabetic"" x=""70"" y=""370"">alphabetic</text>
                              <text dominant-baseline=""text-after-edge"" x=""70"" y=""420"">text-after-edge</text>
                              <text dominant-baseline=""text-before-edge"" x=""70"" y=""470"" >text-before-edge</text> 

                              <!-- Materialization of anchors -->
                              <circle cx=""60"" cy=""20"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""70"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""120"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""170"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""220"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""270"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""320"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""370"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""420"" r=""3"" fill=""red"" />
                              <circle cx=""60"" cy=""470"" r=""3"" fill=""red"" /> 
                              <style>
                                <![CDATA[
                                  body{ padding: 20px}
                                  text {
                                    font: bold 30px Helvetica, Arial, sans-serif;
                                  }
                                  ]]>
                              </style>

                              
                            </svg>";

            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            Assert.IsNotNull(svg);
            
            Assert.AreEqual(52, svg.Contents.Count);

            var txt = svg.Contents[9] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Auto, txt.DominantBaseline);

            txt = svg.Contents[11] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Middle, txt.DominantBaseline);

            txt = svg.Contents[13] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[15] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Hanging, txt.DominantBaseline);

            txt = svg.Contents[17] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Mathematical, txt.DominantBaseline);

            txt = svg.Contents[19] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Top, txt.DominantBaseline);

            txt = svg.Contents[21] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Ideographic, txt.DominantBaseline);

            txt = svg.Contents[23] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Alphabetic, txt.DominantBaseline);

            txt = svg.Contents[25] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_After_Edge, txt.DominantBaseline);

            txt = svg.Contents[27] as SVGText;
            Assert.IsNotNull(txt);
            Assert.AreEqual(DominantBaseline.Text_Before_Edge, txt.DominantBaseline);


            using var doc = new Document();
            doc.AppendTraceLog = false;
            //doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            pg.PaperSize = PaperSize.Custom;
            pg.Width = 500;
            pg.Height = 600;
            doc.Pages.Add(pg);
            //pg.Contents.Add(new TextLiteral("Above the SVG"));
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridOpacity = 0.5;
            pg.Style.OverlayGrid.GridSpacing = 50;
            pg.Style.OverlayGrid.GridColor = StandardColors.Gray;
            svg.BorderColor = StandardColors.Aqua;
            pg.FontFamily = new FontSelector("serif");
            pg.Margins = new Thickness(50, 0, 0, 50);
            pg.FontSize = 10;
            pg.FontWeight = 700;
            //pg.Contents.Add(new TextLiteral("Below the SVG"));
            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextDominantBaseline.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }



        [TestMethod]
        public void SVGTextAnchorChart()
        {
            var svgString = @"
            <svg width=""951"" height=""752"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 951 752"">
<rect width=""951"" height=""752"" x=""0"" y=""0"" id=""0"" fill=""none""></rect>
<polyline points=""214.9 112.4 227.9 104.9 242.9 104.9"" fill=""none"" stroke=""#5470c6""></polyline>
<polyline points=""173.3 221.3 177.9 235.6 192.9 235.6"" fill=""none"" stroke=""#91cc75""></polyline>
<polyline points=""87 190.7 74.4 198.8 59.4 198.8"" fill=""none"" stroke=""#fac858""></polyline>
<polyline points=""83.8 114.8 70.5 107.8 55.5 107.8"" fill=""none"" stroke=""#ee6666""></polyline>
<polyline points=""127.9 78.3 123.4 64 108.4 64"" fill=""none"" stroke=""#73c0de""></polyline>
<path d=""M150 75A75 75 0 0 1 215.0266 187.3702L150 150Z"" fill=""rgb(84,112,198)""></path>
<path d=""M215.0266 187.3702A75 75 0 0 1 119.5358 218.5342L150 150Z"" fill=""#91cc75""></path>
<path d=""M119.5358 218.5342A75 75 0 0 1 75.0011 149.5882L150 150Z"" fill=""rgb(250,200,88)""></path>
<path d=""M75.0011 149.5882A75 75 0 0 1 107.7157 88.0562L150 150Z"" fill=""#ee6666""></path>
<path d=""M107.7157 88.0562A75 75 0 0 1 150 75L150 150Z"" fill=""#73c0de""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(247.8973 104.9222)"" fill=""black"">Sear...</text>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(197.9226 235.5589)"" fill=""black"">Direct</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(54.4 198.8328)"" fill=""black"">Email</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(50.5333 107.7502)"" fill=""black"">Unio...</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" transform=""translate(103.4475 64.006)"" fill=""black"">Video Ads</text>
</svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual((16 * 2) + 1, svg.Contents.Count); //whitespace is significant

            var txt = svg.Contents[22 + 1] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 3] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.Start, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 5] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 7] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            txt = svg.Contents[22 + 9] as SVGText;
            Assert.IsNotNull(txt);

            Assert.AreEqual(TextAnchor.End, txt.TextAnchor);
            Assert.AreEqual(DominantBaseline.Central, txt.DominantBaseline);

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);


            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextAnchorChart.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }

        [TestMethod]
        public void SVGTextPositionAndTranslate()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">
<text dominant-baseline=""auto"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""50"" y=""50"" transform=""translate(100 100)"" fill=""#6E7079"">At 50 + 100</text>
</svg>";
            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 50;

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGTextPositionAndTranslate.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.Inconclusive("Making changes to the layout - need to come back and adjust the test");
            
            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            var lblock = lpg.ContentBlock;
            Assert.IsNotNull(lblock);
            Assert.AreEqual(1, lblock.Columns.Length);
            var lreg = lblock.Columns[0];
            Assert.IsNotNull(lreg);
            Assert.AreEqual(1, lreg.Contents.Count);

            //Canvas inpage block
            var litem = lreg.Contents[0];
            Assert.IsNotNull(litem);
            Assert.IsInstanceOfType(litem, typeof(PDFLayoutBlock));
            var lcanv = litem as PDFLayoutBlock;
            Assert.IsNotNull(lcanv);
            Assert.AreSame(lcanv.Owner, svg);
            Assert.AreEqual(2, lcanv.PositionedRegions.Count);

            //Canvas positioned region
            var lcanvReg = lcanv.PositionedRegions[0];
            Assert.IsNotNull(lcanvReg);
            Assert.AreEqual(498.0, lcanv.Size.Width.PointsValue);
            Assert.AreEqual(305.0, lcanv.Size.Height.PointsValue);

            //Positioned run on the first line in the canvas for the text
            Assert.AreEqual(1, lcanvReg.Contents.Count);
            var lline = lcanvReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(lline);
            Assert.AreEqual(1, lline.Runs.Count);
            var lposRun = lline.Runs[0] as PDFLayoutPositionedRegionRun;
            Assert.IsNotNull(lposRun);

            //Positioned region in the canvas for the text block
            Assert.AreSame(lposRun.Region, lcanv.PositionedRegions[1]);
            var ltxtPosReg = lcanv.PositionedRegions[1];

            //Positioned region contains a block with 1 region with 1 line with 3 runs - text begin, chars and end
            var ltxtBlock = ltxtPosReg.Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(ltxtBlock);
            var ltxtReg = ltxtBlock.Columns[0];
            Assert.IsNotNull(ltxtReg);
            Assert.AreEqual(1, ltxtReg.Contents.Count);
            var ltxtLine = ltxtReg.Contents[0] as PDFLayoutLine;
            Assert.IsNotNull(ltxtLine);
            Assert.AreEqual(3, ltxtLine.Runs.Count);
            Assert.IsInstanceOfType(ltxtLine.Runs[0], typeof(PDFTextRunBegin));
            Assert.IsInstanceOfType(ltxtLine.Runs[1], typeof(PDFTextRunCharacter));
            Assert.IsInstanceOfType(ltxtLine.Runs[2], typeof(PDFTextRunEnd));

            //Check transform of the layout text block
            var lineH = 10.8; //baseline offset
            var offsetX = 100.0; //translate x
            var offsetY = -100.0 + lineH; //negative (translate y - the baseline offset)

            Assert.AreEqual(offsetX, Math.Round(ltxtBlock.TransformedOffset.X.PointsValue, 1), "X offsets for the translate do not match");
            Assert.AreEqual(offsetY, Math.Round(ltxtBlock.TransformedOffset.Y.PointsValue, 1), "Y offsets for the translate do not match");

            //Check the position of the layout text block
            var boundsX = 50.0; // explicit x
            var boundsY = 52.1; // explicit y + descender 

            Assert.AreEqual(boundsX, Math.Round(ltxtBlock.TotalBounds.X.PointsValue, 1));
            Assert.AreEqual(boundsY, Math.Round(ltxtBlock.TotalBounds.Y.PointsValue, 1));


        }

        [TestMethod]
        public void SVGCirclePositionAndTranslate()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">
<circle cx=""50"" cy=""50"" style=""font-size:12px;font-family:sans-serif;"" r=""10"" transform=""translate(100 100)"" fill=""#6E7079""></circle>
</svg>";
            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;

            svg.OverflowAction = OverflowAction.Clip;


            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);
            pg.Style.OverlayGrid.ShowGrid = true;
            pg.Style.OverlayGrid.GridSpacing = 50;

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGCirclePositionAndTranslate.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(layout);
            Assert.AreEqual(1, layout.AllPages.Count);
            var lpg = layout.AllPages[0];
            Assert.IsNotNull(lpg);
            Assert.IsNotNull(lpg.ContentBlock);
            var lblock = lpg.ContentBlock;
            Assert.IsNotNull(lblock);
            Assert.AreEqual(1, lblock.Columns.Length);
            var lreg = lblock.Columns[0];
            Assert.IsNotNull(lreg);
            Assert.AreEqual(1, lreg.Contents.Count);
            
            Assert.Inconclusive("Making changes to the layout - need to comeback and update");

            
        }
        

        [TestMethod]
        public void SVGLineChart()
        {
            var svgString = @"
            <svg width=""498"" height=""305"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" version=""1.1"" baseProfile=""full"" viewBox=""0 0 498 305"">

<rect width=""498"" height=""305"" x=""0"" y=""0"" id=""0"" fill=""none""></rect>
<!-- Axes -->
<path d=""M52.9693 276.5L478.08 276.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 240.5L478.08 240.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 204.5L478.08 204.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 168.5L478.08 168.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 132.5L478.08 132.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 96.5L478.08 96.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 60.5L478.08 60.5"" fill=""none"" stroke=""#E0E6F1""></path>
<path d=""M52.9693 276.5L478.08 276.5"" fill=""none"" stroke=""#6E7079"" stroke-linecap=""round""></path>
<path d=""M53.5 275.85L53.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M124.5 275.85L124.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M194.5 275.85L194.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M265.5 275.85L265.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M336.5 275.85L336.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M407.5 275.85L407.5 280.85"" fill=""none"" stroke=""#6E7079""></path>
<path d=""M478.5 275.85L478.5 280.85"" fill=""none"" stroke=""#6E7079""></path> 
<!-- Labels -->
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 275.85)"" fill=""#6E7079"">0</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 239.875)"" fill=""#6E7079"">500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 203.9)"" fill=""#6E7079"">1,000</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 167.925)"" fill=""#6E7079"">1,500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 131.95)"" fill=""#6E7079"">2,000</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 95.975)"" fill=""#6E7079"">2,500</text>
<text dominant-baseline=""central"" text-anchor=""end"" style=""font-size:12px;font-family:sans-serif;"" transform=""translate(44.9693 60)"" fill=""#6E7079"">3,000</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(52.9693 283.85)"" fill=""#6E7079"">Mon</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(123.8211 283.85)"" fill=""#6E7079"">Tue</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(194.6729 283.85)"" fill=""#6E7079"">Wed</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(265.5246 283.85)"" fill=""#6E7079"">Thu</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(336.3764 283.85)"" fill=""#6E7079"">Fri</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(407.2282 283.85)"" fill=""#6E7079"">Sat</text>
<text dominant-baseline=""central"" text-anchor=""middle"" style=""font-size:12px;font-family:sans-serif;"" y=""6"" transform=""translate(478.08 283.85)"" fill=""#6E7079"">Sun</text>
<!-- Data lines -->
<g clip-path=""url(#zr0-c0)"">
<path d=""M52.9693 267.216L123.8211 266.3526L194.6729 268.583L265.5247 266.2087L336.3764 269.3745L407.2282 259.3015L478.08 260.7405"" fill=""none"" stroke=""rgb(84,112,198)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c1)"">
<path d=""M52.9693 251.387L123.8211 253.2577L194.6729 254.8406L265.5247 249.3724L336.3764 248.509L407.2282 235.558L478.08 238.436"" fill=""none"" stroke=""rgb(145,204,117)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c2)"">
<path d=""M52.9693 240.5945L123.8211 236.5653L194.6729 240.3786L265.5247 238.2921L336.3764 234.8385L407.2282 211.8145L478.08 208.9365"" fill=""none"" stroke=""rgb(250,200,88)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c3)"">
<path d=""M52.9693 217.5705L123.8211 212.6779L194.6729 218.7217L265.5247 214.2608L336.3764 206.778L407.2282 188.071L478.08 185.9125"" fill=""none"" stroke=""rgb(238,102,102)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g>
<g clip-path=""url(#zr0-c4)"">
<path d=""M52.9693 158.5715L123.8211 145.6205L194.6729 153.8947L265.5247 147.0595L336.3764 113.9625L407.2282 92.3775L478.08 90.9385"" fill=""none"" stroke=""rgb(115,192,222)"" stroke-width=""2"" stroke-linejoin=""bevel""></path>
</g> 
<!-- Data circles -->
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 52.9693 267.216)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 123.8211 266.3526)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 194.6729 268.583)"" fill=""rgb(255,255,255)"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1 0 0 1 265.5247,266.2087)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,269.3745)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,259.3015)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,260.7405)"" fill=""#fff"" stroke=""#5470c6""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,251.387)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,253.2577)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,254.8406)"" fill=""rgb(255,255,255)"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,249.3724)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,248.509)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,235.558)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,238.436)"" fill=""#fff"" stroke=""#91cc75""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,240.5945)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,236.5653)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,240.3786)"" fill=""rgb(255,255,255)"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,238.2921)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,234.8385)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,211.8145)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,208.9365)"" fill=""#fff"" stroke=""#fac858""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,217.5705)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,212.6779)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,218.7217)"" fill=""rgb(255,255,255)"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,214.2608)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,206.778)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,188.071)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,185.9125)"" fill=""#fff"" stroke=""#ee6666""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,52.9693,158.5715)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,123.8211,145.6205)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,194.6729,153.8947)"" fill=""rgb(255,255,255)"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,265.5247,147.0595)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,336.3764,113.9625)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,407.2282,92.3775)"" fill=""#fff"" stroke=""#73c0de""></path>
<path d=""M1 0A2 2 0 1 1 1 -0.0001"" transform=""matrix(1,0,0,1,478.08,90.9385)"" fill=""#fff"" stroke=""#73c0de""></path> 
<!-- Legend  -->
<path d=""M-5 -5l454.9199 0l0 23.2l-454.9199 0Z"" transform=""translate(26.54 5)"" fill=""rgb(0,0,0)"" fill-opacity=""0"" stroke=""#ccc"" stroke-width=""0""></path>
<path d=""M0 7L25 7"" transform=""translate(27.54 4.6)"" fill=""#000"" stroke=""#5470c6"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(27.54 4.6)"" fill=""#fff"" stroke=""#5470c6"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""30"" y=""7"" transform=""translate(27.54 4.6)"" fill=""#333"">Email</text>
<path d=""M0 7L25 7"" transform=""translate(98.5459 4.6)"" fill=""#000"" stroke=""#91cc75"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(98.5459 4.6)"" fill=""#fff"" stroke=""#91cc75"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(98.5459 4.6)"" fill=""#333"">Union Ads</text>
<path d=""M0 7L25 7"" transform=""translate(194.9111 4.6)"" fill=""#000"" stroke=""#fac858"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(194.9111 4.6)"" fill=""#fff"" stroke=""#fac858"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(194.9111 4.6)"" fill=""#333"">Video Ads</text>
<path d=""M0 7L25 7"" transform=""translate(290.4033 4.6)"" fill=""#000"" stroke=""#ee6666"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(290.4033 4.6)"" fill=""#fff"" stroke=""#ee6666"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" x=""30"" y=""7"" transform=""translate(290.4033 4.6)"" fill=""#333"">Direct</text>
<path d=""M0 7L25 7"" transform=""translate(362.7393 4.6)"" fill=""#000"" stroke=""#73c0de"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<path d=""M18.1 7A5.6 5.6 0 1 1 18.1 6.9994"" transform=""translate(362.7393 4.6)"" fill=""#fff"" stroke=""#73c0de"" stroke-width=""2"" stroke-linecap=""butt"" stroke-miterlimit=""10""></path>
<text dominant-baseline=""central"" text-anchor=""start"" style=""font-size:12px;font-family:sans-serif;"" xml:space=""preserve"" x=""30"" y=""7"" transform=""translate(362.7393 4.6)"" fill=""#333"">Search Engine</text> 
<path d=""M-5 -5l10 0l0 10l-10 0Z"" transform=""translate(5 5)"" fill=""rgb(0,0,0)"" fill-opacity=""0"" stroke=""#ccc"" stroke-width=""0""></path>
<defs >
<clipPath id=""zr0-c0"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c1"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c2"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c3"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
<clipPath id=""zr0-c4"">
<path d=""M51 59l427 0l0 217.85l-427 0Z"" fill=""#000""></path>
</clipPath>
</defs>
</svg>";


            var component = Document.Parse(new StringReader(svgString), ParseSourceType.DynamicContent);
            var svg = component as SVGCanvas;
            Assert.IsNotNull(svg);
            Assert.AreEqual(181, svg.Contents.Count);

            

            svg.OverflowAction = OverflowAction.Clip;
            svg.BorderWidth = 1;

            using var doc = new Document();
            doc.AppendTraceLog = false;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);
            doc.RenderOptions.Compression = OutputCompressionType.None;

            var pg = new Page();
            doc.Pages.Add(pg);
            pg.Contents.Add(svg);
            pg.Margins = new Thickness(25);
            pg.BorderWidth = 1;
            pg.Padding = new Thickness(25);

            PDF.Layout.PDFLayoutDocument layout = null;
            //Output the document (including databinding the data content)
            using (var stream = DocStreams.GetOutputStream("SVGLineChart.pdf"))
            {
                doc.LayoutComplete += (sender, args) =>
                {
                    layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
                };
                doc.SaveAsPDF(stream);
            }
        }

    }

}



