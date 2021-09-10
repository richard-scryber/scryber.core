using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using Scryber.Native;
using Scryber;
using System.Collections.Generic;
using System.Linq;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFImageData_Test and is intended
    ///to contain all PDFImageData_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFImageData_Test
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


        //Image Data for the scryber_logo_flat_350.png file

        static string ImageName = "scryber_logo_flat_350.png";
        static string ImageDirectory = "\\ImageFiles\\";
        static string ProjectDirectory = @"..\..\";
        static string ImageFilePath = ProjectDirectory + ImageDirectory + ImageName;

        static string ImageUriPath = "richard-scryber/scryber.core/master/Scryber.Components/Resources/";
        static string ImageUriHost = "https://raw.githubusercontent.com/";
        static string ImageUrlPath = ImageUriHost + ImageUriPath + "scryber_generatedby_bow.png";


        static int ImagePixelWidth = 370;
        static int ImagePixelHeight = 80;
        static int ImageResolution = 72; //pixels per inch
        static int ImageBitsPerColor = 8;
        static int ImageBytesPerLine = (ImageBitsPerColor * ImageColorsPerSample) * ImagePixelWidth;
        static ColorSpace ImageColorSpace = ColorSpace.RGB;
        static int ImageColorsPerSample = 3;

        [ClassInitialize]
        public static void SetUpResource(TestContext testContext)
        {
            
            var assembly = typeof(Scryber.Components.Block).Assembly;
            var mgr = new System.Resources.ResourceManager("Scryber.Properties.Resources", assembly);
            var bmp = mgr.GetObject("scryber_generatedby_bow") as System.Drawing.Bitmap;

            _cached = bmp;
            ImagePixelHeight = bmp.Height;
            ImagePixelWidth = bmp.Width;
            ImageResolution = (int)bmp.HorizontalResolution;
        }

        static Bitmap _cached = null;

        internal Bitmap CreateImageBitmap()
        {
            if (null == _cached)
            {
                throw new NotImplementedException("The resource image could not be loaded from the resources");
            }
            return _cached;
        }



        /// <summary>
        ///A test for LoadImageFromBitmap
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LoadImageFromBitmap_Test()
        {
            string sourcekey = ImageFilePath;
            Bitmap bitmap = CreateImageBitmap();

            PDFImageData actual = PDFImageData.LoadImageFromBitmap(sourcekey, bitmap, false);
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Data);
        }

        /// <summary>
        ///A test for LoadImageFromLocalFile
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LoadImageFromLocalFile_Test()
        {
            string tmp = System.IO.Path.GetTempFileName();
            Bitmap bmp = CreateImageBitmap();
            bmp.Save(tmp);

            try
            {
                string path = tmp;
                PDFImageData actual;
                actual = PDFImageData.LoadImageFromLocalFile(path);
                Assert.IsNotNull(actual);
                Assert.IsNotNull(actual.Data);
            }
            finally
            {
                System.IO.File.Delete(tmp);
            }
        }

        /// <summary>
        ///A test for LoadImageFromURI
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void LoadImageFromURI_Test()
        {
            string uri = ImageUrlPath;
            PDFImageData actual;
            actual = PDFImageData.LoadImageFromURI(uri);
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Data);
        }

        /// <summary>
        ///A test for GetSize
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void GetSize_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);

            //GetSize() returns actual size based on pixels and resolution
            double w = ((double)ImagePixelWidth) / ImageResolution;
            double h = ((double)ImagePixelHeight) / ImageResolution;
            PDFSize expected = new PDFSize(new PDFUnit(w, PageUnits.Inches), new PDFUnit(h, PageUnits.Inches));

            int accuracy = 0;
            PDFSize actual;
            actual = target.GetSize();
            Assert.AreEqual(Math.Round(expected.Width.PointsValue, accuracy), Math.Round(actual.Width.PointsValue, accuracy));
            Assert.AreEqual(Math.Round(expected.Height.PointsValue, accuracy), Math.Round(actual.Height.PointsValue, accuracy));
        }

        /// <summary>
        ///A test for SourcePath
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void SourcePath_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);

            string expected = ImageFilePath;
            string actual = target.SourcePath;

            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for VerticalResolution
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void VerticalResolution_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImageResolution;
            int actual = target.VerticalResolution;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for HorizontalResolution
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void HorizontalResolution_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImageResolution;
            int actual = target.HorizontalResolution;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BitsPerColor
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void BitsPerColor_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImageBitsPerColor;
            int actual = (int)target.BitsPerColor;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for BytesPerLine
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void BytesPerLine_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImageBytesPerLine;
            int actual = (int)target.BytesPerLine;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ColorSpace
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void ColorSpace_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            ColorSpace expected = ImageColorSpace;
            ColorSpace actual = target.ColorSpace;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ColorsPerSample
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void ColorsPerSample_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImageColorsPerSample;
            int actual = target.ColorsPerSample;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayHeight
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void DisplayHeight_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            double h = ((double)ImagePixelHeight) / ImageResolution;
            PDFUnit expected = new PDFUnit(h, PageUnits.Inches);

            PDFUnit actual = target.DisplayHeight;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for DisplayWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void DisplayWidth_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            double w = ((double)ImagePixelWidth) / ImageResolution;
            PDFUnit expected = new PDFUnit(w, PageUnits.Inches);
            PDFUnit actual = target.DisplayWidth;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PixelHeight
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PixelHeight_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImagePixelHeight;
            int actual = target.PixelHeight;

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for PixelWidth
        ///</summary>
        [TestMethod()]
        [TestCategory("Graphics")]
        public void PixelWidth_Test()
        {
            Bitmap bmp = CreateImageBitmap();
            PDFImageData target = PDFImageData.LoadImageFromBitmap(ImageFilePath, bmp, false);
            int expected = ImagePixelWidth;
            int actual = target.PixelWidth;

            Assert.AreEqual(expected, actual);
        }



    }
}
