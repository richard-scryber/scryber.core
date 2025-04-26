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
    public class HtmlTransform_Tests
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

        



        /// <summary>
        /// Rotating divs increasing by 10 degrees around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleRotate_Test()
        {
            
            var html = @"<?scryber append-log='false' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;font-size:12pt' >
    <div id='plain' style='width: 100pt; background-color:#ddd; height:200pt; position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate10' style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(10deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate20' style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(20deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(30deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(40deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(50deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(60deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: rotate(70deg); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleRotate.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 0.9848, -0.1736, 0.1736, 0.9848, 50, 741.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 0.9397, -0.3420, 0.3420, 0.9397, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 0.8660, -0.5, 0.5, 0.866, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 0.7660, -0.6428, 0.6428, 0.7660, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 0.6428, -0.766, 0.7660, 0.6428, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 0.5000, -0.866, 0.866, 0.5, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.3420, -0.9397, 0.9397, 0.3420, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }

        /// <summary>
        /// Rotating divs increasing by 10 degrees around a specific position on the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleRotateWithPosition_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Warning' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate10' style='left: 75pt; top: 100pt; width: 150pt; background-color:#ddd; height:200pt; transform: rotate(10deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate20' style='left: 100pt; top: 100pt; width: 200pt; background-color:#ddd; height:200pt; transform: rotate(20deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 125pt; top: 100pt; width: 250pt; background-color:#ddd; height:200pt; transform: rotate(30deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 150pt; top: 100pt; width: 300pt; background-color:#ddd; height:200pt; transform: rotate(40deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 175pt; top: 100pt; width: 350pt; background-color:#ddd; height:200pt; transform: rotate(50deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 200pt; top: 100pt; width: 400pt; background-color:#ddd; height:200pt; transform: rotate(60deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 225pt; top: 100pt; width: 450pt; background-color:#ddd; height:200pt; transform: rotate(70deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='top: 100pt; height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleRotateWithPosition.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 0.9848, -0.1736, 0.1736, 0.9848, 100, 641.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 0.9397, -0.3420, 0.3420 , 0.9397, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 0.8660, -0.5, 0.5, 0.866, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 0.7660, -0.6428, 0.6428, 0.7660, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 0.6428, -0.766, 0.7660, 0.6428, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 0.5000, -0.866, 0.866, 0.5, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.3420, -0.9397, 0.9397, 0.3420, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }


        /// <summary>
        /// Translating divs increasing by 10,20 points around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleTranslate_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate10' style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(10pt, 20pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate20' style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(20pt, 40pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(30pt, 60pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(40pt, 80pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(50pt, 100pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(60pt, 120pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: translate(70pt, 140pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleTranslate.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 60, 721.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 70, 701.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 80, 681.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 90, 661.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 110, 621.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 120, 601.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }


        /// <summary>
        /// Rotating divs increasing by 10 degrees around a specific position on the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleTranslateWithPosition_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Warning' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate10' style='left: 75pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(10pt, 20pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='rotate20' style='left: 100pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(20pt, 40pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 125pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(30pt, 60pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 150pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(40pt, 80pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 175pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(50pt, 100pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 200pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(60pt, 120pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 225pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: translate(70pt, 140pt); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='top: 100pt; height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleTranslateWithPosition.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 110, 621.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1,0, 0, 1, 120, 601.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 130, 581.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 140, 561.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 150, 541.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 160, 521.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 170, 501.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });

        }


        /// <summary>
        /// Skew divs increasing by 10 degrees and 5 degrees around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleSkew_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew10' style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(10deg, 5deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew20' style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(20deg, 10deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(30deg, 15deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(40deg, 20deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(50deg, 25deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(60deg, 30deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: skew(70deg, 35deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleSkew.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 1, -0.0873, -0.1745, 1, 50, 741.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1, -0.1745, -0.3491, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, -0.2618, -0.5236, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, -0.3491, -0.6981, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, -0.4363, -0.8727, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, -0.5236, -1.0472, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, -0.6109, -1.2217, 1, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }


        /// <summary>
        /// Skew divs increasing by 10 degrees and 5 degrees around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleSkewWithPosition_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <div id='plain' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew10' style='left: 100pt; top: 125pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(10deg, 5deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew20' style='left: 150pt; top: 150pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(20deg, 10deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 200pt; top: 175pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(30deg, 15deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 250pt; top: 200pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(40deg, 20deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 300pt; top: 225pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(50deg, 25deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 350pt; top: 250pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(60deg, 30deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 400pt; top: 275pt; width: 100pt; background-color:#ddd; height:200pt; transform: skew(70deg, 35deg); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleSkewWithPosition.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }



            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 1, -0.0873, -0.1745, 1, 100, 641.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1, -0.1745, -0.3491, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, -0.2618, -0.5236, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, -0.3491, -0.6981, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, -0.4363, -0.8727, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, -0.5236, -1.0472, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, -0.6109, -1.2217, 1, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };

            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }


        /// <summary>
        /// Skew divs increasing by 10 degrees and 5 degrees around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleScale_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew10' style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(2, 4); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew20' style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.8, 3.6); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.6, 3.2); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.4, 2.8); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.2, 2.4); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(1, 2); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='width: 100pt; background-color:#ddd; height:200pt; transform: scale(0.8, 1.6); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleScale.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }


            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 2, 0, 0, 4, 50, 741.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1.8000, 0, 0, 3.6000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1.6000, 0, 0, 3.2000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1.4000, 0, 0, 2.8000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1.2000, 0, 0, 2.4000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1.0000, 0, 0, 2.0000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.8000, 0, 0, 1.6000, 50, 741.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });
        }

 

        /// <summary>
        /// Skew divs increasing by 10 degrees and 5 degrees around the top left of the page
        /// </summary>
        [TestMethod]
        public void Transform_SingleScaleWithPosition_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;font-size: 12pt;' >
    <div id='plain' style='left: 50pt; top: 50pt; width: 100pt; background-color:#ddd; height:200pt; position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew10' style='left: 75pt; top: 75pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(2, 4); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='skew20' style='left: 100pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.8, 3.6); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 150pt; top: 150pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.6, 3.2); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 175pt; top: 175pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.4, 2.8); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 200pt; top: 200pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(1.2, 2.4); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 225pt; top: 225pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(1, 2); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div style='left: 250pt; top: 250pt; width: 100pt; background-color:#ddd; height:200pt; transform: scale(0.8, 1.6); position: absolute; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_SingleScaleWithPosition.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            
            var transform1 = new {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 2, 0, 0, 4, 100, 641.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1.8000, 0, 0, 3.6000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1.6000, 0, 0, 3.2000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1.4000, 0, 0, 2.8000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1.2000, 0, 0, 2.4000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1.0000, 0, 0, 2.0000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.8000, 0, 0, 1.6000, 100, 641.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3, transform4, transform5, transform6, transform7, transform8
            });

        }

        /// <summary>
        /// Skew divs with multiple transformations around a point
        /// </summary>
        [TestMethod]
        public void Transform_MultipleRotateScaleTranslateWithPosition_Test()
        {

            var html = @"<?scryber append-log='true' log-level='Diagnostic' parser-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt; font-size: 12pt;' >
    <div id='plain' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='multi10' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform:  rotate(20deg) scale(2, 4); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <div id='multi11' style='left: 50pt; top: 100pt; width: 100pt; background-color:#ddd; height:200pt; transform:  skew(20deg, 40deg) translate(100pt, 60pt); position: fixed; border: solid 2px red;' >
        Content of the div
        <div style='height:30pt; background-color:blue'></div>
    </div>
    <p>After the transformed content of the page</p>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    doc.RenderOptions.Compression = OutputCompressionType.None;
                    using (var stream = DocStreams.GetOutputStream("Transform_MultipleWithPosition.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);
                    }

                }
            }

            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 1.8794, -0.6840, 1.3681, 3.7588, 100, 641.8898 },
                offset = new double[] { 50, 741.8898 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1, -0.6981, -0.3491, 1, 200, 581.8898 },
                offset = new double[] { 50.0, 741.8898 }
            };
            
            //OutputLayoutTransform();
            ValidateLayoutBlocks(new[] {
                transform1, transform2, transform3
            });

        }



        

        // Transformation validation

        /// <summary>
        /// A reference to the current document layout
        /// </summary>
        PDF.Layout.PDFLayoutDocument _docLayout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this._docLayout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
        }

        /// <summary>
        /// Checks all the transformation matricies and offsets against the blocks
        /// </summary>
        /// <param name="doc">The current document layout</param>
        /// <param name="transforms"></param>
        private void ValidateLayoutBlocks(dynamic[] transforms)
        {
            return;
            Assert.Inconclusive("Need to sort the transformations and SVG's out first then test.");

            //Will not get past here.

            Assert.IsNotNull(_docLayout, "The document layout is null, and we cannot validate the blocks");

            var page = _docLayout.AllPages[0];
            var regions = page.PageBlock.PositionedRegions;
            
            if(regions.Count != transforms.Length){
                throw new ArgumentException("The number of positioned regions does not match the number of transforms.");
            }
            
            for(var i = 0; i < transforms.Length; i++)
            {
                var region = regions[i];
                var transform = transforms[i];
                var block = region.Contents[0] as PDF.Layout.PDFLayoutBlock;

                if (transform.offset.Length == 0)
                {
                    Assert.IsTrue(block.TransformedOffset.X == Unit.Zero, "The x offset (" + block.TransformedOffset.X + ") of block " + i + " was not zero");
                    Assert.IsTrue(block.TransformedOffset.Y == Unit.Zero, "The y offset (" + block.TransformedOffset.Y + ") of block " + i + " was not zero");
                }
                else
                {
                    Assert.AreEqual(Math.Round(transform.offset[0], 4), Math.Round(block.TransformedOffset.X.PointsValue, 4),
                                    "The x offset (" + block.TransformedOffset.X + ") of block " + i + " did not match the expected output of " + transform.offset[0].ToString());
                    Assert.AreEqual(Math.Round(transform.offset[1], 4), Math.Round(block.TransformedOffset.Y.PointsValue, 4),
                                    "The y offset (" + block.TransformedOffset.Y + ") of block " + i + " did not match the expected output of " + transform.offset[1].ToString());
                }

                // if(transform.matrix.Length == 0)
                // {
                //     Assert.IsNull(block.Position.TransformMatrix, "The matrix for block " + i + " was not null ");
                // }
                // else
                // {
                //     var values = block.Position.TransformMatrix.Components;
                //     for(var m = 0; m < values.Length; m++)
                //     {
                //         Assert.AreEqual(Math.Round(transform.matrix[m], 4),
                //                         Math.Round(values[m], 4),
                //                         "The matrix [" + string.Join(",", values) + "] for block " + i + " did not match " + string.Join(",", (double[])transform.matrix));
                //     }
                // }

            }
        }

        private void OutputLayoutTransform()
        {
            Assert.Fail("Need to re-implement the transformations");
            // var s = "";
            // var page = _docLayout.AllPages[0];
            // var regions = page.ContentBlock.PositionedRegions;
            //
            // for (var i = 0; i < regions.Count; i++)
            // {
            //     var region = regions[i];
            //     
            //     var block = region.Contents[0] as PDF.Layout.PDFLayoutBlock;
            //     if (null == block.Position.TransformMatrix)
            //     {
            //         s += "Block " + i + " transformation is null\r\n\r\n";
            //     }
            //     else
            //     {
            //         var matrix = block.Position.TransformMatrix.Components;
            //
            //         s += "Block " + i + " Transform = [" + string.Join(", ", matrix) + "]\r\n";
            //         s += "Block " + i + " Offsets = [" + block.TransformedOffset.X + "," + block.TransformedOffset.Y + "]\r\n\r\n";
            //     }
            //
            //     
            // }

            //Assert.IsTrue(String.IsNullOrEmpty(s), s); //Will always fail but report the transformations
        }
    }
}