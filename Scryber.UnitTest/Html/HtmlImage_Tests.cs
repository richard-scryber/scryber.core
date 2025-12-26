using System;
using System.IO;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.Drawing;

namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlImage_Tests
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

        public string GetLocalProjectPath()
        {
            var dir = new DirectoryInfo(TestContext.TestRunDirectory);
            while (dir.Name.Equals("Scryber.Core",  StringComparison.InvariantCultureIgnoreCase) == false)
            {
                dir = dir.Parent;
                Assert.IsNotNull(dir);
            }

            dir = new DirectoryInfo(System.IO.Path.Combine(dir.FullName, "Scryber.UnitTest"));
            Assert.IsTrue(dir.Exists, "The Unit Test project folder could not be found");
            return dir.FullName + System.IO.Path.DirectorySeparatorChar;
        }
        
        public string GetLocalImagePath(string nameWithExtension)
        {
            var dir = GetLocalProjectPath();
            var path = System.IO.Path.Combine(dir, "Content", "HTML", "Images", nameWithExtension);
            path = System.IO.Path.GetFullPath(path);
            
            //Check to make sure we are looking in the local path
            Assert.IsTrue(System.IO.File.Exists(path), "Could not find the path to the image file");

            return path;
        }
        
        /// <summary>
        /// Checks the red dot image resource for validity
        /// </summary>
        public void AssertGroupImage(PDFImageXObject img)
        {
            Assert.IsNotNull(img.ImageData, "The resource had no data");
            var data = img.ImageData as ImageRasterData;
            
            Assert.IsNotNull(data, "The resource had no data, or is not a raster image");
            Assert.AreEqual(396, data.PixelWidth, "Expected a 5 pixel wide image");
            Assert.AreEqual(342, data.PixelHeight, "Expected a 5 pixel high image");
            Assert.AreEqual(8, data.BitsPerColor);
        }

        /// <summary>
        /// Check to make sure the image data is as expected for a Png image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="alpha"></param>
        /// <param name="compressed"></param>
        public void AssertPngImage(PDFImageXObject img, bool alpha, bool compressed)
        {
            if (alpha)
                Assert.IsTrue(img.ImageData.HasAlpha,"Image should have an alpha channel");
            else
                Assert.IsFalse(img.ImageData.HasAlpha,"Image should not have an alpha channel");
            
            Assert.IsFalse(img.ImageData.IsPrecompressedData);
            
            if (compressed)
            {
                Assert.AreEqual(1, img.Filters.Length, "Expected a single filter");
                Assert.AreEqual(PDFDeflateStreamFilter.DefaultFilterName, img.Filters[0].FilterName, "Expected the filter name to be for FlateDecode");
            }
            else
            {
                Assert.IsTrue(null == img.Filters || img.Filters.Length == 0, "Should not be any filters on the image");
            }

        }
        
        /// <summary>
        /// Check to make sure the image data is as expected for a Jepg image
        /// </summary>
        /// <param name="img"></param>
        public void AssertJpgImage(PDFImageXObject img)
        {
            var data = img.ImageData as ImageRasterData;
            Assert.IsNotNull(data, "The resource had no data, or is not a raster image");
            Assert.AreEqual(8, data.BitsPerColor, "Expected a 24 bit RGB image");
            Assert.IsFalse(img.ImageData.HasAlpha, "Jpeg images should not have an alpha");
            Assert.IsTrue(img.ImageData.IsPrecompressedData,"Jpeg images should be pre-compressed");
            Assert.AreEqual(1, img.ImageData.Filters.Length, "Expected a single filter"); 
            Assert.AreEqual("DCTDecode", img.ImageData.Filters[0].FilterName, "Expected the filter name to be DCTDecode");
     
        }
        
        
        /// <summary>
        /// An image in the content with a full file path
        /// </summary>
        [TestMethod]
        public void ImagePngLocalAbsoluteSource_Test()
        {
            var path = GetLocalImagePath("group.png");
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalAbsolutePngGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual(img.Source.ToLower(), path.ToLower(), "Image source was expected to be " + path);
                        AssertPngImage(img, true, true);
                    }

                }
            }
        }
        
        /// <summary>
        /// An image in the content with a full file path
        /// </summary>
        [TestMethod]
        public void ImageJpegLocalAbsoluteSource_Test()
        {
            var path = GetLocalImagePath("Group.jpg");
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalAbsoluteJpegGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual(img.Source, path, "Image source was expected to be " + path);
                        AssertJpgImage(img);
                    }

                }
            }
        }
        
        /// <summary>
        /// An image in the content with a full file path
        /// </summary>
        [TestMethod]
        public void ImageTiffLocalAbsoluteSource_Test()
        {
            var path = GetLocalImagePath("groupBasic.tiff");
            
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalAbsoluteTiffGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        var img = (PDFImageXObject) one;
                        AssertGroupImage(img);
                        Assert.AreEqual(img.Source, path, "Image source was expected to be " + path);
                        AssertPngImage(img, false, true);
                    }

                }
            }
        }
        
        
        
        
        /// <summary>
        /// An image in the content with a relative path and path provided to the parser
        /// </summary>
        [TestMethod]
        public void ImagePngLocalRelativeSource_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/group.png";
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, project, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalRelativePngGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual( (project + path).ToLower(), img.Source.ToLower(), "Image source was expected to be " + path);
                        AssertPngImage(img, true, true);
                    }

                }
            }
        }
        
        /// <summary>
        /// An image in the content with a relative path and path provided to the parser
        /// </summary>
        [TestMethod]
        public void ImageJpegLocalRelativeSource_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/Group.jpg";
            
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, project, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalRelativeJpegGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertJpgImage(img);
                    }

                }
            }
        }
        
        /// <summary>
        /// An image in the content with a relative path and path provided to the parser
        /// </summary>
        [TestMethod]
        public void ImageTiffLocalRelativeSource_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/groupBasic.tiff";
            
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, project, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalRelativeTiffGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        var img = (PDFImageXObject) one;
                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertPngImage(img, false, true);
                    }

                }
            }
        }
        
        
        /// <summary>
        /// An image in the content with a relative path and base path set in the template
        /// </summary>
        [TestMethod]
        public void ImagePngLocalBaseSource_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/group.png";
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalRelativePngGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual( (project + path).ToLower(), img.Source.ToLower(), "Image source was expected to be " + path);
                        AssertPngImage(img, true, true);
                    }

                }
            }
        }
        
        /// <summary>
        /// An image in the content with a relative path and a base path set in the template
        /// (the path from the parser should be ignored)
        /// </summary>
        [TestMethod]
        public void ImageJpegLocalBaseSource_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/Group.jpg";
            
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, "http://ignored/path", ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalRelativeJpegGroup.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject) one;
                        
                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertJpgImage(img);
                    }

                }
            }
        }


        /// <summary>
        /// Loads a test image from creative suspects based on an issue raised with the
        /// 6.0 beta 8 version of the library.
        /// </summary>
        [TestMethod]
        public void CreativeSuspectsImageTest()
        {

            var path = "https://cf.creativesuspects.io/img/sad-face.png";
            var project = "Creative Suspects test";
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <img src='" + path + @"' alt='Group' />
</body>
</html>";


            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("CreativeSuspectsTest.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject)one;

                        
                        Assert.AreEqual(path, img.Source, "Image source was expected to be " + path);
                        AssertPngImage(img, true, true);
                    }

                }
            }
        }

        //
        // Image sizing
        //

        /// <summary>
        /// An image in the content with a relative path and a base path set in the template
        /// (the path from the parser should be ignored)
        /// </summary>
        [TestMethod]
        public void ImageSizeJpegNatural_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/Group.jpg";

            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <img id='naturalImage' src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, "http://ignored/path", ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalNaturalSize.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject)one;

                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertJpgImage(img);

                        var page = _layout.AllPages[0];
                        var line = page.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
                        Assert.AreEqual(1, line.Runs.Count);

                        var imgLayout = line.Runs[0] as PDFLayoutComponentRun;
                        
                        var size = img.ImageData.GetSize();
                        Assert.AreEqual(size.Width, imgLayout.Width);
                    }

                }
            }
        }

        /// <summary>
        /// An image in the content with a relative path and a base path set in the template
        /// (the path from the parser should be ignored)
        /// </summary>
        [TestMethod]
        public void ImageSizeJpegFixedWidth_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/Group.jpg";
            var width = new Unit(40, PageUnits.Points);
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <img id='fixedImage' style='width:" + width.ToString() + @"' src='" + path + @"' alt='Group' />
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, "http://ignored/path", ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalFixedWidth.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject)one;

                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertJpgImage(img);

                        var page = _layout.AllPages[0];
                        var line = page.ContentBlock.Columns[0].Contents[0] as PDFLayoutLine;
                        Assert.AreEqual(1, line.Runs.Count);

                        var imgLayout = line.Runs[0] as PDFLayoutComponentRun;
                        Assert.AreEqual(width, imgLayout.Width);

                    }

                }
            }
        }

        /// <summary>
        /// An image in the content with a relative path and a base path set in the template
        /// (the path from the parser should be ignored)
        /// </summary>
        [TestMethod]
        public void ImageSizeJpegFixedContainerWidth_Test()
        {
            var project = GetLocalProjectPath();
            var path = "Content/HTML/Images/Group.jpg";
            var width = new Unit(40, PageUnits.Points);
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
</head>
<body style='padding:20pt;' >
    <div style='width:" + width.ToString() + @"; border: solid 1px black;' >
        <img id='containedImage'  src='" + path + @"' alt='Group' />
    </div>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, "http://ignored/path", ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalFixedContainerWidth.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject)one;

                        AssertGroupImage(img);
                        Assert.AreEqual(project + path, img.Source, "Image source was expected to be " + path);
                        AssertJpgImage(img);

                        var page = _layout.AllPages[0];
                        var div = page.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                        var line = div.Columns[0].Contents[0] as PDFLayoutLine;

                        Assert.AreEqual(1, line.Runs.Count);

                        var imgLayout = line.Runs[0] as PDFLayoutComponentRun;
                        
                        //As per chrome, the height will use the available space and the content will overflow the available width
                        var imgWidth = new Unit(2.75, PageUnits.Inches);
                        Assert.AreEqual(imgWidth, imgLayout.Width);

                    }

                }
            }
        }

        /// <summary>
        /// An image in the content with a relative path and a base path set in the template
        /// (the path from the parser should be ignored)
        /// </summary>
        [TestMethod]
        public void ImageSizeJpegFixedWidthWithContent_Test()
        {
            var project = GetLocalProjectPath();
            var path = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/docs/images/ScyberLogo2_alpha_small.png";
            var width = new Unit(40, PageUnits.Points);
            var html = @"<?scryber append-log='true' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
<head>
    <base href='" + project + @"' />
    <style>
        .content{
            content:url('" + path + @"');
        }
    </style>
</head>
<body style='padding:20pt;' >
    <div style='width:" + width.ToString() + @"; border: solid 1px black;' >
        <img class='content' />
    </div>
</body>
</html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, "http://ignored/path", ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("ImageLocalFixedWidthWithContent.pdf"))
                    {
                        doc.LayoutComplete += Doc_LayoutComplete;
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        var img = (PDFImageXObject)one;


                        //AssertGroupImage(img);
                        Assert.AreEqual(path, img.Source, "Image source was expected to be " + path);
                        //AssertJpgImage(img);

                        var page = _layout.AllPages[0];
                        var div = page.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                        var line = div.Columns[0].Contents[0] as PDFLayoutLine;

                        Assert.AreEqual(1, line.Runs.Count);

                        var imgLayout = line.Runs[0] as PDFLayoutComponentRun;
                        var imgWidth = new Unit(320, PageUnits.Points);
                        Assert.AreEqual(Math.Round(imgWidth.PointsValue, 3), Math.Round(imgLayout.Width.PointsValue, 3));

                    }

                }
            }
        }


        private PDF.Layout.PDFLayoutDocument _layout;


        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDF.Layout.PDFLayoutDocument>();
        }
    }
}