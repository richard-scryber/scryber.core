using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Styles;
using Scryber.Drawing;
using Scryber.Styles.Parsing;

using Scryber.PDF.Layout;
using Scryber.PDF;
using System.Xml.Schema;
using Scryber.Svg.Components;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass()]
    public class SVGParsing_Tests
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGSimple.pdf"))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    doc.SaveAsPDF(stream);

                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    Assert.AreEqual(3, section.Contents.Count);
                    Assert.IsInstanceOfType(section.Contents[0], typeof(HTMLParagraph));
                    Assert.IsInstanceOfType(section.Contents[2], typeof(HTMLParagraph));
 
                    var canvas = section.Contents[1] as Canvas;
                    Assert.IsNotNull(canvas);
                    Assert.AreEqual(300, canvas.Style.Size.Width.PointsValue, "The width of the canvas was not set");
                    Assert.AreEqual(200, canvas.Style.Size.Height.PointsValue, "The height of the canvas was not set");
                    Assert.AreEqual("canvas", canvas.StyleClass, "The style class of the canvas was not set");

                    Assert.AreEqual(1, canvas.Contents.Count);
                    var rect = canvas.Contents[0] as Svg.Components.SVGRect;
                    Assert.IsNotNull(rect, "The inner rectangle was not found");
                    Assert.AreEqual("box", rect.StyleClass, "The rect style class was not correct");
                    Assert.AreEqual(100, rect.Style.Position.X.PointsValue, "The X position of the rect was not correct");
                    Assert.AreEqual(10, rect.Style.Position.Y.PointsValue, "The Y position of the rect was not correct");
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
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGComponents.html");

            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;

                using (var stream = DocStreams.GetOutputStream("SVGComponents.pdf"))
                {
                    doc.SaveAsPDF(stream);
                    
                    var section = doc.Pages[0] as Section;
                    Assert.IsNotNull(section);
                    var clock = section.Contents[1] as Canvas;
                    
                    Assert.IsNotNull(clock);
                    Assert.AreEqual("ClockIcon", clock.ID);
                    Assert.AreEqual(1, clock.Contents.Count);
                    Assert.AreEqual(20, clock.Width);
                    Assert.AreEqual(20, clock.Height);

                    var clockPath = clock.Contents[0] as SVGPath;
                    Assert.IsNotNull(clockPath);
                    Assert.AreEqual(StandardColors.Blue, clockPath.FillColor);
                }
            }
        }

        [TestMethod]
        public void SVGTransform()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGTransform.html");

            using (var doc = Document.ParseDocument(path))
            {
                Div div;
                if(doc.TryFindAComponentById("mydiv", out div))
                {
                    div.Style.SetValue(StyleKeys.TransformRotateKey, 90);
                }

                using (var stream = DocStreams.GetOutputStream("Transform.pdf"))
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

    }
}
