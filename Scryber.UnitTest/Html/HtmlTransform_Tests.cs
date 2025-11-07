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
                matrix = new double[] { 0.984807753012208,-0.17364817766693033,0.17364817766693033,0.984807753012208,-124.29138466766238,23.122500025989666 },
                offset = new double[] { 70, 120 }
            };
            var transform3 = new
            {
                matrix = new double[] { 0.9396926207859084,-0.3420201433256687,0.3420201433256687,0.9396926207859084,-242.6793239283823,67.47668976784826 },
                offset = new double[] { 70, 120 }
            };
            var transform4 = new
            {
                matrix = new double[] { 0.8660254037844387,-0.49999999999999994,0.49999999999999994,0.8660254037844387,-351.5666601549106,131.71488961457237 },
                offset = new double[] { 70, 120 }
            };
            var transform5 = new
            {
                matrix = new double[] { 0.766044443118978,-0.6427876096865393,0.6427876096865393,0.766044443118978,-447.6449067356551,213.8852543699171 },
                offset = new double[] { 70, 120 }
            };
            var transform6 = new
            {
                matrix = new double[] { 0.6427876096865394,-0.766044443118978,0.766044443118978,0.6427876096865394,-527.9947747661984,311.4910790810017 },
                offset = new double[] { 70, 120 }
            };
            var transform7 = new
            {
                matrix = new double[] { 0.5000000000000001,-0.8660254037844386,0.8660254037844386,0.5000000000000001,-590.1748741654275,421.5666601549106 },
                offset = new double[] { 70, 120 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.3420201433256688,-0.9396926207859083,0.9396926207859083,0.3420201433256688,-632.2958940777453,540.7674067616447 },
                offset = new double[] { 70, 120 }
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
                matrix = new double[] { 0.984807753012208,-0.17364817766693033,0.17364817766693033,0.984807753012208,-109.18415069528453,35.798974480320794 },
                offset = new double[] { 150, 200 }
            };
            var transform3 = new
            {
                matrix = new double[] { 0.9396926207859084,-0.3420201433256687,0.3420201433256687,0.9396926207859084,-207.4777531644969,107.11471806305792 },
                offset = new double[] { 200, 200 }
            };
            var transform4 = new
            {
                matrix = new double[] { 0.8660254037844387,-0.49999999999999994,0.49999999999999994,0.8660254037844387,-287.4512328361096,210.99692191732754 },
                offset = new double[] {250, 200 }
            };
            var transform5 = new
            {
                matrix = new double[] { 0.766044443118978,-0.6427876096865393,0.6427876096865393,0.766044443118978,-342.41211987809686,343.0099600473393 },
                offset = new double[] { 300, 200 }
            };
            var transform6 = new
            {
                matrix = new double[] { 0.6427876096865394,-0.766044443118978,0.766044443118978,0.6427876096865394,-366.6917500289112,497.4065319292387 },
                offset = new double[] { 350, 200 }
            };
            var transform7 = new
            {
                matrix = new double[] { 0.5000000000000001,-0.8660254037844386,0.8660254037844386,0.5000000000000001,-355.8928418626724,667.3550434037754 },
                offset = new double[] { 400, 200 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.3420201433256688,-0.9396926207859083,0.9396926207859083,0.3420201433256688,-307.08813887862675,845.2122141263434 },
                offset = new double[] { 450, 200 }
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
                matrix = new double[] { 1, 0, 0, 1, 10, -20 },
                offset = new double[] { 70, 120 } //middle of shape from page top
            };
            var transform3 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 20, -40 },
                offset = new double[] { 70.0, 120 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 30, -60 },
                offset = new double[] { 70, 120 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 40, -80 },
                offset = new double[] { 70, 120 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 50, -100 },
                offset = new double[] { 70, 120 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 60, -120 },
                offset = new double[] { 70, 120 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 70, -140 },
                offset = new double[] { 70, 120 }
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
                matrix = new double[] { 1, 0, 0, 1, 10, -20 },
                offset = new double[] { 125, 200 } //middle of shape from page top
            };
            var transform3 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 20, -40 },
                offset = new double[] { 150, 200 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 30, -60 },
                offset = new double[] { 175, 200 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 40, -80 },
                offset = new double[] { 200, 200 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 50, -100 },
                offset = new double[] { 225, 200 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 60, -120 },
                offset = new double[] { 250, 200 }
            };
            var transform8 = new
            {
                matrix = new double[] { 1, 0, 0, 1, 70, -140 },
                offset = new double[] { 275, 200 }
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
                matrix = new double[] { 1, -0.0875, -0.1763, 1, 127.2886, 6.1242 },
                offset = new double[] { 70, 120 }
            };
            
            var transform3 = new
            {
                matrix = new double[] { 1, -0.1763, -0.3640, 1, 262.7464, 12.3429 },
                offset = new double[] { 70.0, 120 }
            };
            
            var transform4 = new
            {
                matrix = new double[] { 1, -0.2679, -0.5774, 1, 416.7832, 18.7564 },
                offset = new double[] { 70.0, 120 }
            };
                
            var transform5 = new
            {
                matrix = new double[] { 1, -0.364, -0.8391, 1, 605.7374, 25.4779 },
                offset = new double[] { 70.0, 120 }
            };
            
            var transform6 = new
            {
                matrix = new double[] { 1, -0.4663, -1.1918, 1, 860.3147, 32.6415 },
                offset = new double[] { 70.0, 120 }
            };
            
            var transform7 = new
            {
                matrix = new double[] { 1, -0.5774, -1.7321, 1, 1250.3497, 40.4145 },
                offset = new double[] { 70.0, 120 }
            };
            
            var transform8 = new
            {
                matrix = new double[] { 1, -0.7002, -2.7475, 1, 1983.3758, 49.01453 },
                offset = new double[] { 70.0, 120 }
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
                matrix = new double[] { 1,-0.087488663525924,-0.17632698070846498,1,108.77430947728557,13.123299528888651 },
                offset = new double[] { 150, 225 }
            };
            
            var transform3 = new
            {
                matrix = new double[] { 1,-0.17632698070846498,-0.36397023426620234,1,215.43025598277376,35.26539614169303 },
                offset = new double[] { 200, 250 }
            };
            
            var transform4 = new
            {
                matrix = new double[] { 1,-0.2679491924311227,-0.5773502691896256,1,327.2939577192262,66.9872981077807 },
                offset = new double[] { 250, 275 }
            };
                
            var transform5 = new
            {
                matrix = new double[] { 1,-0.36397023426620234,-0.8390996311772799,1,454.6995009265413,109.1910702798607 },
                offset = new double[] { 300, 300 }
            };
            
            var transform6 = new
            {
                matrix = new double[] { 1,-0.46630765815499864,-1.1917535925942098,1,616.0052329599874,163.20768035424953 },
                offset = new double[] { 350, 325 }
            };
            
            var transform7 = new
            {
                matrix = new double[] { 1,-0.5773502691896256,-1.7320508075688767,1,851.9780625900129,230.94010767585024 },
                offset = new double[] { 400, 350 }
            };
            
            var transform8 = new
            {
                matrix = new double[] { 1,-0.7002075382097097,-2.747477419454621,1,1282.769083360052,315.09339219436936 },
                offset = new double[] { 450, 375 }
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
                matrix = new double[] { 2,0,0,4,-70,-2165.6692913399997 },
                offset = new double[] { 70,120 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1.8,0,0,3.6,-56,-1876.9133858279997 },
                offset = new double[] { 70.0, 120 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1.6,0,0,3.2,-42,-1588.1574803159997 },
                offset = new double[] { 70.0, 120 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1.4,0,0,2.8,-28,-1299.4015748039997 },
                offset = new double[] { 70.0, 120 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1.2,0,0,2.4,-14,-1010.645669292 },
                offset = new double[] { 70.0, 120 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1,0,0,2,0,-721.88976378 },
                offset = new double[] { 70.0, 120 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.8,0,0,1.6,14,-433.1338582679999 },
                offset = new double[] { 70.0, 120 }
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

            
            var transform1 = new
            {
                matrix = new double[] { },
                offset = new double[] { }
            };

            var transform2 = new
            {
                matrix = new double[] { 2,0,0,4,-125,-2000.6692913399997 },
                offset = new double[] { 125,175 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1.8,0,0,3.6,-120,-1668.9133858279997 },
                offset = new double[] { 150, 200 }
            };
            var transform4 = new
            {
                matrix = new double[] { 1.6,0,0,3.2,-120,-1302.1574803160001 },
                offset = new double[] { 200, 250 }
            };
            var transform5 = new
            {
                matrix = new double[] { 1.4,0,0,2.8,-90,-1020.4015748039998 },
                offset = new double[] { 225.0, 275.0 }
            };
            var transform6 = new
            {
                matrix = new double[] { 1.2,0,0,2.4,-50,-758.645669292 },
                offset = new double[] { 250, 300 }
            };
            var transform7 = new
            {
                matrix = new double[] { 1,0,0,2,0,-516.88976378 },
                offset = new double[] { 275.0, 325 }
            };
            var transform8 = new
            {
                matrix = new double[] { 0.8,0,0,1.6,60,-295.13385826800004 },
                offset = new double[] { 300, 350 }
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
                matrix = new double[] { 1.8793852415718169,-0.6840402866513374,1.3680805733026749,3.7587704831436337,-966.0954401864426,-1702.4225050831692 },
                offset = new double[] { 100, 200 }
            };
            var transform3 = new
            {
                matrix = new double[] { 1,-0.8390996311772799,-0.36397023426620234,1,333.62876769608386,23.90996311772801 },
                offset = new double[] { 100, 200 }
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

            Assert.IsNotNull(_docLayout, "The document layout is null, and we cannot validate the blocks");

            var page = _docLayout.AllPages[0];
            var regions = page.ContentBlock.PositionedRegions;
            
            if(regions.Count != transforms.Length){
                throw new ArgumentException("The number of positioned regions does not match the number of transforms.");
            }
            
            for(var i = 0; i < transforms.Length; i++)
            {
                var region = regions[i];
                var transform = transforms[i];
                var block = region.Contents[0] as PDF.Layout.PDFLayoutBlock;
                
                Assert.IsNotNull(block, "The block is null, and we cannot validate the transform");
                
#if DEBUG

                var blockOrigin = block.RenderOrigin;
                var blockMatrix = block.RenderMatrix;

                if (transform.offset.Length == 0)
                {
                    if (null != blockOrigin)
                    {
                        Assert.IsTrue(blockOrigin.HorizontalOrigin == Unit.Zero,
                            "The horizontal origin (" + blockOrigin.HorizontalOrigin + ") of block " + i + " was not zero");
                        Assert.IsTrue(blockOrigin.VerticalOrigin == Unit.Zero,
                            "The vertical origin (" + blockOrigin.VerticalOrigin + ") of block " + i + " was not zero");
                    }
                }
                else
                {
                    Assert.AreEqual(Math.Round(transform.offset[0], 4), 
                                    Math.Round(blockOrigin.HorizontalOrigin.PointsValue, 4),
                                    "The horizontal origin (" + blockOrigin.HorizontalOrigin + ") of block " + (i+1) + " did not match the expected output of " + transform.offset[0].ToString());
                   
                    Assert.AreEqual(Math.Round(transform.offset[1], 4), 
                                    Math.Round(blockOrigin.VerticalOrigin.PointsValue, 4),
                                    "The vertical origin (" + blockOrigin.VerticalOrigin + ") of block " + (i+1) + " did not match the expected output of " + transform.offset[1].ToString());
                }

                if(transform.matrix.Length == 0)
                {
                    Assert.IsNull(blockMatrix, "The matrix for block " + i + " was not null ");
                }
                else
                {
                    var values = blockMatrix.Components;
                    for(var m = 0; m < values.Length; m++)
                    {
                        Assert.AreEqual(Math.Round(transform.matrix[m], 4),
                                        Math.Round(values[m], 4),
                                        "The matrix [" + string.Join(",", values) + "] for block " + (i+1) + " did not match " + string.Join(",", (double[])transform.matrix));
                    }
                }
                
#else
                
                Assert.Inconclusive("Not testing is release mode");

#endif

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