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
        public void SVGFSimple()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/SVG/SVGSimple.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGSimple.pdf"))
                {
                    doc.SaveAsPDF(stream);
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
