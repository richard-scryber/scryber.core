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

namespace Scryber.Core.UnitTests.Imaging
{
    [TestClass()]
    public class ImageLoad_Tests
    {
        public const string PathToImages = "../../../Content/HTML/Images/";

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
            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages , "Group.png");
            path = io.Path.GetFullPath(path);

            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path);
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
            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages , "Group.jpg");
            path = io.Path.GetFullPath(path);
            
            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path);
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
        public void WritePngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            doc.ImageFactories.Clear();
            var factory = new Scryber.Imaging.ImageFactoryPng();
            doc.ImageFactories.Add(factory);

            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages, "Group.png");
            path = io.Path.GetFullPath(path);

            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

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
            Assert.IsTrue(xobj.Source.EndsWith("Group.png"), "The source was expected to end with the name of the file");
            Assert.AreEqual(path, xobj.Source, "The source did not match the xObject source");
            var data = xobj.ImageData;

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
            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages , "groupBasic.tiff");
            path = io.Path.GetFullPath(path);
            
            if (!io.File.Exists(path))
                throw new io.FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path);
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
                label.PositionMode = PositionMode.Block;
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

    }
}
