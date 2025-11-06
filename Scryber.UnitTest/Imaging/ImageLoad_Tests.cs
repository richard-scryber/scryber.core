using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using System.Collections.Generic;
using io = System.IO;
using Scryber.Drawing;
using Scryber.Imaging;
using Scryber.PDF.Resources;
using System.Threading.Tasks;

namespace Scryber.Core.UnitTests.Imaging
{
    [TestClass()]
    public class ImageLoad_Tests
    {
        public const string PathToImages = "../../../Content/HTML/Images/";

        private TestContext testContextInstance;

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
        
        public ImageLoad_Tests()
        {
        }

        private const int GroupHResolution = 144;
        private const int GroupVResolution = 144;
        private const int GroupWPixel = 396;
        private const int GroupHPixel = 342;
        private const int GroupBitsPerColor = 8;
        private const int GroupColorsPerSample = 3*8;
        private const ColorSpace GroupColorSpace = ColorSpace.RGB;
        private static readonly Unit GroupDisplayWidth = new Unit(2.75, PageUnits.Inches);
        private static readonly Unit GroupDisplayHeight = new Unit(2.375, PageUnits.Inches);
        
        
        [TestMethod()]
        public void LoadPngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryPng();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/group.png",
                this.TestContext);

            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path) as ImageRasterData;
            doc.RemoteRequests.EnsureRequestsFullfilled();

            Assert.IsNotNull(data, "The returned image data was null in the Group.png image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.png image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.png image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.png image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.png image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.png image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.png image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.png image");
            Assert.AreEqual(path, data.SourcePath, "Source path was not matching the load path in the Group.png image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasFilter);
            Assert.IsNull(data.Filters);
            Assert.IsFalse(data.IsPrecompressedData);
            
        }
        
        [TestMethod()]
        public void LoadJpegFromFile()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryJpeg();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Group.jpg",
                this.TestContext);

            var data = factory.LoadImageData(doc, page, path) as ImageRasterData;
            doc.RemoteRequests.EnsureRequestsFullfilled();
            
            Assert.IsNotNull(data, "The returned image data was null in the Group.jpg image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.jpg image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.jpg image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.jpg image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.jpg image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.jpg image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.jpg image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.jpg image");
            Assert.AreEqual(path, data.SourcePath, "Source path was not matching the load path in the Group.jpg image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.jpg image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.jpg image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasAlpha);

            //We should have the JCTDecode filter for jpeg images
            Assert.IsTrue(data.IsPrecompressedData);
            Assert.IsTrue(data.HasFilter);
            Assert.IsNotNull(data.Filters);
            Assert.AreEqual(1, data.Filters.Length);
            Assert.AreEqual("DCTDecode" , data.Filters[0].FilterName);
            

            

        }
        
        [TestMethod()]
        public void LoadPngFromRawData()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryPng();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/group.png",
                this.TestContext);

            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var raw = io.File.ReadAllBytes(path);
            var type = MimeType.PngImage;
            
            var data = factory.LoadImageData(doc, page, raw, type) as ImageRasterData;
            //doc.RemoteRequests.EnsureRequestsFullfilled();

            Assert.IsNotNull(data, "The returned image data was null in the Group.png image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.png image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.png image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.png image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.png image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.png image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.png image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.png image");
            Assert.IsTrue(data.SourcePath.EndsWith(".png"), "Source path was not matching the load path in the Group.tiff image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasFilter);
            Assert.IsNull(data.Filters);
            Assert.IsFalse(data.IsPrecompressedData);
            
        }
        
        [TestMethod()]
        public void LoadJpegFromRawData()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryJpeg();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Group.jpg",
                this.TestContext);

            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);
            

            var raw = io.File.ReadAllBytes(path);
            var type = MimeType.JpegImage;
            
            var data = factory.LoadImageData(doc, page, raw, type) as ImageRasterData;
            
            //doc.RemoteRequests.EnsureRequestsFullfilled();
            
            Assert.IsNotNull(data, "The returned image data was null in the Group.jpg image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.jpg image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.jpg image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.jpg image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.jpg image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.jpg image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.jpg image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.jpg image");
            Assert.IsTrue(data.SourcePath.EndsWith(".jpg"), "Source path was not matching the load path in the Group.tiff image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.jpg image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.jpg image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasAlpha);

            //We should have the JCTDecode filter for jpeg images
            Assert.IsTrue(data.IsPrecompressedData);
            Assert.IsTrue(data.HasFilter);
            Assert.IsNotNull(data.Filters);
            Assert.AreEqual(1, data.Filters.Length);
            Assert.AreEqual("DCTDecode" , data.Filters[0].FilterName);
            

            

        }


        [TestMethod()]
        public void WritePngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            doc.ImageFactories.Clear();
            var factory = new Scryber.Imaging.ImageFactoryPng();
            doc.ImageFactories.Add(factory);

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/group.png",
                this.TestContext);

            Image img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);

            
            
            page.Padding = new Thickness(20);
            

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;

            using (var stream = DocStreams.GetOutputStream("NewImagingTest_Group.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count, "The image is not in the document, or more resources were loaded");

            var xobj = doc.SharedResources[0] as PDFImageXObject;
            Assert.IsNotNull(xobj, "There was no image xObject in the document");
            Assert.IsTrue(xobj.Registered);
            Assert.IsTrue(xobj.Source.EndsWith("group.png"), "The source was expected to end with the name of the file");
            Assert.AreEqual(path, xobj.Source, "The source did not match the xObject source");
            var data = xobj.ImageData as ImageRasterData;

            Assert.IsNotNull(data, "The image data was null");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.png image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.png image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.png image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.png image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.png image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.png image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.png image");
            Assert.AreEqual(path, data.SourcePath, "Source path was not matching the load path in the Group.png image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.png image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
        }
        
        [TestMethod()]
        public void LoadTiffFromFile()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryTiff();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/groupBasic.tiff",
                this.TestContext);
            
     
            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path) as ImageRasterData;
            doc.RemoteRequests.EnsureRequestsFullfilled();

            Assert.IsNotNull(data, "The returned image data was null in the Group.tiff image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.tiff image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.tiff image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.tiff image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.tiff image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.tiff image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.tiff image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.tiff image");
            Assert.AreEqual(path, data.SourcePath, "Source path was not matching the load path in the Group.tiff image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.tiff image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.tiff image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasAlpha, "The image should not have an alpha channel");

            Assert.IsFalse(data.IsPrecompressedData);
            Assert.IsFalse(data.HasFilter);
            
        }
        
        [TestMethod()]
        public void LoadTiffFromRawData()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryTiff();
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/groupBasic.tiff",
                this.TestContext);
            
     
            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);
            
            
            var raw = io.File.ReadAllBytes(path);
            var type = MimeType.TiffImage;
            
            var data = factory.LoadImageData(doc, page, raw, type) as ImageRasterData;
            
            //doc.RemoteRequests.EnsureRequestsFullfilled();

            Assert.IsNotNull(data, "The returned image data was null in the Group.tiff image");
            Assert.AreEqual(GroupDisplayHeight, data.DisplayHeight, "Heights did not match in the Group.tiff image");
            Assert.AreEqual(GroupDisplayWidth, data.DisplayWidth, "Widths did not match in the Group.tiff image");
            Assert.AreEqual(GroupColorSpace, data.ColorSpace, "The color spaces did not match in the Group.tiff image");
            Assert.AreEqual(GroupColorsPerSample, data.ColorsPerSample, "Expected Colours per sample did not match in the Group.tiff image");
            Assert.AreEqual(GroupBitsPerColor,data.BitsPerColor, "Expected Bits per pixel did not match in the Group.tiff image");
            Assert.AreEqual(GroupHPixel, data.PixelHeight, "Expected Pixel Heights did not match in the Group.tiff image");
            Assert.AreEqual(GroupWPixel, data.PixelWidth, "Expected pixel widths did not match in the Group.tiff image");
            Assert.IsTrue(data.SourcePath.EndsWith(".tiff"), "Source path was not matching the load path in the Group.tiff image");
            Assert.AreEqual(GroupHResolution,data.HorizontalResolution, "Expected the horizontal resolutions did not match in the Group.tiff image");
            Assert.AreEqual(GroupVResolution,data.VerticalResolution,"Expected the horizontal resolutions did not match in the Group.tiff image");
            Assert.AreEqual(data.Type, ObjectTypes.ImageData);
            Assert.IsFalse(data.HasAlpha, "The image should not have an alpha channel");

            Assert.IsFalse(data.IsPrecompressedData);
            Assert.IsFalse(data.HasFilter);
            
        }
       

        /// <summary>
        /// This image does not compress with zip, and the algorithm returns null.
        /// Should just write the raw image data
        /// </summary>
        [TestMethod]
        public void ZeroCompressionImage()
        {
            var imgPath =
                "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/rgb-16-alpha.png";

            var doc = new Document();
            doc.AppendTraceLog = true;
            var factory = new Scryber.Imaging.ImageFactoryPng();
            
            //Take everything out and add the PNG factory
            doc.ImageFactories.Clear();
            doc.ImageFactories.Add(factory);

            var pg = new Page() {Padding = 20.0, BorderColor = StandardColors.Black, BorderWidth = 1};
            doc.Pages.Add(pg);

            var img = new Image() {Source = imgPath};
            pg.Contents.Add(img);

            using (var stream = DocStreams.GetOutputStream("ImageCompressionZero.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);
            var resource = doc.SharedResources[0] as PDFImageXObject;
            Assert.IsNotNull(resource);
            var data = resource.ImageData as ImageData;
            Assert.IsNotNull(data);
            
            
        }


        //
        // Testing many of the different PNG formats
        //


        string[] allPng = new string[]
        {
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/basn3p01.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/basn3p02.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/basn3p04.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/basn3p08.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/bpp1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/chunklength1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/chunklength2.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/colors-saturation-lightness.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-1-trns.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-16-tRNS-interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-16.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-2-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-4-tRNS.png",
            
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-8-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-alpha-16.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray-alpha-8.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/gray_4bpp.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/icon.png",
            
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/indexed.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/rgb-16-alpha.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/rgb-48bpp-interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/rgb-48bpp.png",
            
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/rgb-8-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/vim16x16_1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/vim16x16_2.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/palette-8bpp.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/filter4.png",

        };


        [TestMethod()]
        public void WriteAllTestPngsFromImageSharp()
        {
            var doc = new Document();

            var page = new Page();
            page.ColumnCount = 3;
            page.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(page);
            page.Padding = new Thickness(20);
            page.FontSize = 12;

            var factory = new Scryber.Imaging.ImageFactoryPng();
            //Take everything out and add the png factory
            doc.ImageFactories.Clear();
            doc.ImageFactories.Add(factory);

            List<string> ids = new List<string>();

            foreach (var src in allPng)
            {

                Image img = new Image();
                img.Source = src;
                img.ID = System.IO.Path.GetFileNameWithoutExtension(src);
                img.BorderColor = StandardColors.Black;
                img.BorderStyle = LineType.Solid;
                img.MaximumWidth = 100;

                page.Contents.Add(img);

                Span label = new Span();
                label.Contents.Add(new TextLiteral(img.ID));
                label.Margins = new Thickness(0, 0, 10, 0);
                label.DisplayMode = DisplayMode.Block;
                page.Contents.Add(label);

                ids.Add(System.IO.Path.GetFileName(src));

            }

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
            using (var stream = DocStreams.GetOutputStream("ImageTypesPng.pdf"))
            {
                doc.SaveAsPDF(stream);
            }


            Assert.AreEqual(allPng.Length + 1, doc.SharedResources.Count); //images + 1 for the font

            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFImageXObject imgx)
                {
                    string src = System.IO.Path.GetFileName(imgx.Source);
                    ids.Remove(src);
                }
            }

            Assert.AreEqual(0, ids.Count);
        }

        //
        // Testing many of the Tiff formats.
        //

        string[] allTiff = new string[]
        {
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/rgb_deflate.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-minisblack-16_lsb.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/grayscale_deflate_multistrip.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/rgb_palette.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-palette-02.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-palette-04.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-02.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-08.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-10.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-12.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-14.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-16.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-16_lsb_zip_predictor.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-16_msb_zip_predictor.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-24.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-24_lsb.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-32.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-rgb-contig-32_lsb.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h1v1.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h2v1.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h2v2.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h4v4.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/flower-ycbcr-planar-08_h1v1.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Tiff/little_endian.tiff",

        };


        [TestMethod()]
        public void WriteAllTestTiffsFromImageSharp()
        {

            var doc = new Document();

            var page = new Page();
            page.ColumnCount = 3;
            page.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(page);
            page.Padding = new Thickness(20);
            page.FontSize = 12;
            
            var factory = new Scryber.Imaging.ImageFactoryTiff();
            //Take everything out and add the tiff factory
            doc.ImageFactories.Clear();
            doc.ImageFactories.Add(factory);

            List<string> ids = new List<string>();

            foreach (var src in allTiff)
            {
                string id = System.IO.Path.GetFileName(src);
                Image img = new Image();
                img.Source = src;
                img.ID = id;
                img.BorderColor = StandardColors.Black;
                img.BorderStyle = LineType.Solid;
                img.MaximumWidth = 100;
                img.MaximumHeight = 200;

                page.Contents.Add(img);

                Span label = new Span();
                label.Contents.Add(new TextLiteral(id));
                label.Margins = new Thickness(0, 0, 10, 0);
                page.Contents.Add(label);

                ids.Add(id);
            }

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);

            using (var stream = DocStreams.GetOutputStream("ImageTypesTiff.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(allTiff.Length + 1, doc.SharedResources.Count); //images + 1 for the font

            foreach (var rsrc in doc.SharedResources)
            {
                if(rsrc is PDFImageXObject imgx)
                {
                    string src = System.IO.Path.GetFileName(imgx.Source);
                    ids.Remove(src);
                }
            }

            Assert.AreEqual(0, ids.Count);

        }

        //
        // Testing many of the Gif formats.
        //

        string[] allGif = new string[]
        {
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/GlobalQuantizationTest.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/base_1x4.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/base_4x1.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/cheers.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/giphy.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/image-zero-height.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/image-zero-size.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/image-zero-width.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/kumin.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/large_comment.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Gif/leo.gif",
        };


        [TestMethod()]
        public void WriteAllTestGifsFromImageSharp()
        {

            var doc = new Document();

            var page = new Page();
            page.ColumnCount = 3;
            page.OverflowAction = OverflowAction.NewPage;
            doc.Pages.Add(page);
            page.Padding = new Thickness(20);
            page.FontSize = 12;

            var factory = new Scryber.Imaging.ImageFactoryGif();
            //Take everything out and add the png factory
            doc.ImageFactories.Clear();
            doc.ImageFactories.Add(factory);

            List<string> ids = new List<string>();

            foreach (var src in allGif)
            {
                string id = System.IO.Path.GetFileName(src);
                Image img = new Image();
                img.Source = src;
                img.ID = id;
                img.BorderColor = StandardColors.Black;
                img.BorderStyle = LineType.Solid;
                img.MaximumWidth = 100;
                img.MaximumHeight = 200;
                img.BackgroundColor = StandardColors.Gray;
                page.Contents.Add(img);

                Span label = new Span();
                label.Contents.Add(new TextLiteral(id));
                label.Margins = new Thickness(0, 0, 10, 0);
                page.Contents.Add(label);

                ids.Add(id);
            }

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);

            using (var stream = DocStreams.GetOutputStream("ImageTypesGif.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(allGif.Length + 1, doc.SharedResources.Count); //images + 1 for the font

            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFImageXObject imgx)
                {
                    string src = System.IO.Path.GetFileName(imgx.Source);
                    ids.Remove(src);
                }
            }

            Assert.AreEqual(0, ids.Count);

        }

        string urlWithParams = "https://media.githubusercontent.com/media/SixLabors/ImageSharp/main/tests/Images/Input/Png/basn3p01.png?t=" + Random.Shared.Next().ToString() + "&test=true";

        [TestMethod]
        public void TestRemoteImageWithParams()
        {
            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            var p = new Paragraph();
            p.Contents.Add("The image below should be included even with a parameter for url " + urlWithParams);
            pg.Contents.Add(p);
            var img = new Image();
            img.Source = urlWithParams;
            pg.Contents.Add(img);
            doc.ConformanceMode = ParserConformanceMode.Strict;

            using(var stream = DocStreams.GetOutputStream("ImageTypesWithParameters.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            var found = false;
            Assert.AreEqual(2, doc.SharedResources.Count);
            foreach(var rsrc in doc.SharedResources)
            {
                if(rsrc is PDFImageXObject imgx)
                {
                    var src = imgx.Source;
                    Assert.AreEqual(urlWithParams, src);
                    found = true;
                }
            }

            Assert.IsTrue(found);
        }

        [TestMethod]
        public void TestRemoteBackgroundImageWithParams()
        {
            var doc = new Document();
            var pg = new Page();
            doc.Pages.Add(pg);
            var p = new Paragraph();
            p.Contents.Add("The image below should be included as a background with a parameter for url " + urlWithParams);
            pg.Contents.Add(p);
            var div = new Div();
            div.Style.Background.ImageSource = urlWithParams ;
            div.Width = 100;
            div.Height = 100;
            pg.Contents.Add(div);
            doc.ConformanceMode = ParserConformanceMode.Strict;

            using (var stream = DocStreams.GetOutputStream("BackgroundImageWithParameters.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            var found = false;
            Assert.AreEqual(2, doc.SharedResources.Count);
            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFImageXObject imgx)
                {
                    var src = imgx.Source;
                    Assert.AreEqual(urlWithParams, src);
                    found = true;
                }
            }

            Assert.IsTrue(found);
        }


        [TestMethod]
        public async Task TestRemoteCSSBackgroundImageWithParams()
        {
            var doc = new Scryber.Html.Components.HTMLDocument();
            var pg = new Page();
            doc.Pages.Add(pg);
            var p = new Paragraph();
            p.Contents.Add("The image below should be included as a background with a parameter for url " + urlWithParams);
            var style = ".bg-img{" +
                "background-image: url('" + urlWithParams + "');" +
                "}";
            doc.Head = new Scryber.Html.Components.HTMLHead();
            doc.Head.Contents.Add(new Scryber.Html.Components.HTMLStyle() { Contents = style });

            pg.Contents.Add(p);
            var div = new Div();
            div.StyleClass = "bg-img";

            div.Width = 100;
            div.Height = 100;
            pg.Contents.Add(div);
            doc.ConformanceMode = ParserConformanceMode.Strict;
            doc.AppendTraceLog = true;
            
            using (var stream = DocStreams.GetOutputStream("BackgroundCCSImageWithParameters.pdf"))
            {
                await doc.SaveAsPDFAsync(stream);
            }

            var found = false;
            Assert.AreEqual(2, doc.SharedResources.Count);
            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFImageXObject imgx)
                {
                    var src = imgx.Source;
                    Assert.AreEqual(urlWithParams, src);
                    found = true;
                }
            }

            Assert.IsTrue(found);
        }

        [TestMethod]
        public async Task TestRemoteCSSBackgroundImageWithEncodedParams()
        {
            var doc = new Scryber.Html.Components.HTMLDocument();
            var pg = new Page();
            doc.Pages.Add(pg);
            var p = new Paragraph();
            p.Contents.Add("The image below should be included as a background with a parameter for url " + urlWithParams);
            var style = ".bg-img{" +
                "background-image: url('" + urlWithParams.Replace("&", "&amp;") + "');" +
                "}";
            doc.Head = new Scryber.Html.Components.HTMLHead();
            doc.Head.Contents.Add(new Scryber.Html.Components.HTMLStyle() { Contents = style });

            pg.Contents.Add(p);
            var div = new Div();
            div.StyleClass = "bg-img";

            div.Width = 100;
            div.Height = 100;
            pg.Contents.Add(div);
            doc.ConformanceMode = ParserConformanceMode.Strict;
            doc.AppendTraceLog = true;

            using (var stream = DocStreams.GetOutputStream("BackgroundCCSImageWithEncodedParameters.pdf"))
            {
                await doc.SaveAsPDFAsync(stream);
            }

            var found = false;
            Assert.AreEqual(2, doc.SharedResources.Count);
            foreach (var rsrc in doc.SharedResources)
            {
                if (rsrc is PDFImageXObject imgx)
                {
                    var src = imgx.Source;
                    Assert.AreEqual(urlWithParams, src);
                    found = true;
                }
            }

            Assert.IsTrue(found);
        }




        [TestMethod]
        public void TestJPEGHeader_Group()
        {
            //Faster loading of the JPG image data
            var imgSizeX = 396;
            var imgSizeY = 342;

            var resolutionX = 144;
            var resolutionY = 144;
            var bitsPerPixel = 24; //8 x 3

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/group.jpg",
                this.TestContext);

            
            using (var stream = new System.IO.FileStream(path, io.FileMode.Open))
            {
                byte[] marker = new byte[2];
                stream.Read(marker);
                Assert.AreEqual(0xFF, marker[0]);
                Assert.AreEqual(0xD8, marker[1]);

                long pos = 2;

                byte[] appo = new byte[2];
                stream.Read(appo);
                Assert.AreEqual(0xFF, appo[0]);
                Assert.AreEqual(0xE0, appo[1]);


                byte[] len = new byte[2];
                stream.Read(len);
                ushort length = (ushort)((int)len[1] | (len[0] << 8));

                Assert.AreEqual(16, length);

                byte[] ident = new byte[5];
                stream.Read(ident); //JFIF_
                Assert.AreEqual(0x4A, ident[0]); //J
                Assert.AreEqual(0x46, ident[1]); //F
                Assert.AreEqual(0x49, ident[2]); //I
                Assert.AreEqual(0x46, ident[3]); //F
                Assert.AreEqual(0x00, ident[4]); //null

                byte[] vers = new byte[2];
                stream.Read(vers);

                Assert.AreEqual(0x01, vers[0]);
                Assert.AreEqual(0x01, vers[1]);

                byte[] density = new byte[1];
                stream.Read(density);
                Assert.AreEqual(0, density[0]);

                byte[] xdensities = new byte[2];
                byte[] ydensities = new byte[2];

                stream.Read(xdensities);
                stream.Read(ydensities);

                ushort xdensity = (ushort)(xdensities[0] << 8 | xdensities[1]);
                ushort ydensity = (ushort)(ydensities[0] << 8 | ydensities[1]);


                pos += 2 + length; // FF EO + len;

                int w = 0;
                int h = 0;
                int bpp = 0;

                while (pos < stream.Length)
                {
                    stream.Position = pos;
                    stream.Read(marker); //looking for 0xFFC0 - start of frame;
                    stream.Read(len);
                    if (marker[0] != 0xFF)
                        throw new Exception("did not hit the start of a JPEG file block");

                    if (marker[1] == 0xC0) //start of frame marker
                    {
                        //byte - precision
                        byte[] prec = new byte[1];
                        stream.Read(prec);

                        //ushort - no lines aka height
                        var lines = new byte[2];
                        stream.Read(lines);
                        h = (ushort)((int)(lines[0] << 8) | lines[1]);

                        //ushort - sample per line aka width
                        var samples = new byte[2];
                        stream.Read(samples);
                        w = (ushort)((int)(samples[0] << 8) | samples[1]);

                        byte[] components = new byte[1];
                        stream.Read(components);

                        bpp = prec[0] * (int)components[0];
                        break;

                    }
                    else
                    {
                        length = (ushort)((int)len[1] | (len[0] << 8));
                        pos += length + 2;
                    }
                }

                Assert.AreEqual(imgSizeX, w);
                Assert.AreEqual(imgSizeY, h);
                Assert.AreEqual(bitsPerPixel, bpp);
                Assert.AreEqual(resolutionX, xdensity);
                Assert.AreEqual(resolutionY, ydensity);


            }
        }


        [TestMethod]
        public void TestJPEGHeader_Toroid()
        {
            //Faster loading of the JPG image data
            var imgSizeX = 682;
            var imgSizeY = 452;
            var resolutionX = 72;
            var resolutionY = 72;
            var bitsPerPixel = 24; //8 x 3

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Toroid24.jpg",
                this.TestContext);
            
            using (var stream = new System.IO.FileStream(path, io.FileMode.Open))
            {
                byte[] marker = new byte[2];
                stream.Read(marker);
                Assert.AreEqual(0xFF, marker[0]);
                Assert.AreEqual(0xD8, marker[1]);

                long pos = 2;

                byte[] appo = new byte[2];
                stream.Read(appo);
                Assert.AreEqual(0xFF, appo[0]);
                Assert.AreEqual(0xE0, appo[1]);


                byte[] len = new byte[2];
                stream.Read(len);
                ushort length = (ushort)((int)len[1] | (len[0] << 8));

                Assert.AreEqual(16, length);

                byte[] ident = new byte[5];
                stream.Read(ident); //JFIF_
                Assert.AreEqual(0x4A, ident[0]); //J
                Assert.AreEqual(0x46, ident[1]); //F
                Assert.AreEqual(0x49, ident[2]); //I
                Assert.AreEqual(0x46, ident[3]); //F
                Assert.AreEqual(0x00, ident[4]); //null

                byte[] vers = new byte[2];
                stream.Read(vers);

                Assert.AreEqual(0x01, vers[0]);
                Assert.AreEqual(0x01, vers[1]);

                byte[] density = new byte[1];
                stream.Read(density);
                Assert.AreEqual(0, density[0]);

                byte[] xdensities = new byte[2];
                byte[] ydensities = new byte[2];

                stream.Read(xdensities);
                stream.Read(ydensities);

                ushort xdensity = (ushort)(xdensities[0] << 8 | xdensities[1]);
                ushort ydensity = (ushort)(ydensities[0] << 8 | ydensities[1]);
                
                
                pos += 2 + length; // FF EO + len;

                int w = 0;
                int h = 0;
                int bpp = 0;

                while (pos < stream.Length)
                {
                    stream.Position = pos;
                    stream.Read(marker); //looking for 0xFFC0 - start of frame;
                    stream.Read(len);
                    if (marker[0] != 0xFF)
                        throw new Exception("did not hit the start of a JPEG file block");

                    if (marker[1] == 0xC0) //start of frame marker
                    {
                        //byte - precision
                        byte[] prec = new byte[1];
                        stream.Read(prec);

                        //ushort - no lines aka height
                        var lines = new byte[2];
                        stream.Read(lines);
                        h = (ushort)((int)(lines[0] << 8) | lines[1]);

                        //ushort - sample per line aka width
                        var samples = new byte[2];
                        stream.Read(samples);
                        w = (ushort)((int)(samples[0] << 8) | samples[1]);

                        byte[] components = new byte[1];
                        stream.Read(components);

                        bpp = prec[0] * (int)components[0];
                        break;

                    }
                    else
                    {
                        length = (ushort)((int)len[1] | (len[0] << 8));
                        pos += length + 2;
                    }
                }

                Assert.AreEqual(imgSizeX, w);
                Assert.AreEqual(imgSizeY, h);
                Assert.AreEqual(bitsPerPixel, bpp);
                Assert.AreEqual(resolutionX, xdensity);
                Assert.AreEqual(resolutionY, ydensity);
            }


        }

        [TestMethod]
        public void TestJPEGHeaderImageReader()
        {
            //Faster loading of the JPG image data
            var imgSizeX = 396;
            var imgSizeY = 342;
            var resolutionX = 144;
            var resolutionY = 144;
            var bitsPerColor = 8;
            var bitsPerPixel = 24; //8 x 3

           
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/group.jpg",
                            this.TestContext);

            Scryber.Imaging.ImageFactoryJpeg factory = new ImageFactoryJpeg();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var image = factory.LoadImageData(null, null, path) as ImageRasterData;

            stopwatch.Stop();

            //ImageReader reader = ImageReader.Create();
            //var image = reader.ReadStream(path, stream, true);

            Assert.IsNotNull(image);
            Assert.AreEqual(imgSizeX, image.PixelWidth);
            Assert.AreEqual(imgSizeY, image.PixelHeight);
            Assert.AreEqual(bitsPerColor, image.BitsPerColor);
            Assert.AreEqual(bitsPerPixel, image.ColorsPerSample);
            Assert.AreEqual(resolutionX, image.HorizontalResolution);
            Assert.AreEqual(resolutionY, image.VerticalResolution);

            
        }

        [TestMethod]
        public void TestLargeJPEGHeaderImageReader()
        {
            //Faster loading of the JPG image data
            var imgSizeX = 14220; // 682;
            var imgSizeY = 3571; // 452;
            var resolutionX = 72;
            var resolutionY = 72;
            var bitsPerPixel = 24; //8 x 3

            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Superwide.jpeg",
                this.TestContext);
            
            

            Scryber.Imaging.ImageFactoryJpeg factory = new ImageFactoryJpeg();

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            var image = factory.LoadImageData(null, null, path) as ImageRasterData;

            stopwatch.Stop();

            //ImageReader reader = ImageReader.Create();
            //var image = reader.ReadStream(path, stream, true);

            Assert.IsNotNull(image);
            Assert.AreEqual(imgSizeX, image.PixelWidth);
            Assert.AreEqual(imgSizeY, image.PixelHeight);
            Assert.AreEqual(bitsPerPixel, image.BitsPerColor * image.ColorsPerSample);
            Assert.AreEqual(resolutionX, image.HorizontalResolution);
            Assert.AreEqual(resolutionY, image.VerticalResolution);

            
        }

        
    }
}
