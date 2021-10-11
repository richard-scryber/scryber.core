using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using System.Collections.Generic;
using io = System.IO;
using System.IO;
using Scryber.Drawing;
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


        [TestMethod()]
        public void LoadPngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new Scryber.Imaging.ImageFactoryPng();
            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages , "Group.png");
            path = io.Path.GetFullPath(path);

            if (!io.File.Exists(path))
                throw new FileNotFoundException(path);

            var data = factory.LoadImageData(doc, page, path);

            Assert.IsNotNull(data);
        }


        [TestMethod()]
        public void WritePngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            var factory = new Scryber.Imaging.ImageFactoryPng();
            doc.ImageFactories.Add(new Options.PDFImageFactory("PNG", new System.Text.RegularExpressions.Regex(".*\\.png", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages, "Group.png");
            path = io.Path.GetFullPath(path);

            if (!io.File.Exists(path))
                throw new FileNotFoundException(path);

            Image img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);
            page.Padding = new Thickness(20);

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;

            using (var stream = DocStreams.GetOutputStream("NewImagingTest_Group.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);

            //Second time for performance

            doc = new Document();
            doc.ImageFactories.Add(new Options.PDFImageFactory("PNG", new System.Text.RegularExpressions.Regex(".*\\.png", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

            page = new Page();
            doc.Pages.Add(page);

            img = new Image();
            img.Source = path;
            img.BorderColor = StandardColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);

            page.Padding = new Thickness(20);
            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;

            using (var stream = DocStreams.GetOutputStream("NewImagingTest_Performance.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);
        }


        //
        // Testing many of the different PNG formats
        //


        string[] allPng = new string[]
        {
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/basn3p01.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/basn3p02.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/basn3p04.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/basn3p08.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/bpp1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/chunklength1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/chunklength2.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/colors-saturation-lightness.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-1-trns.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-16-tRNS-interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-16.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-2-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-4-tRNS.png",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-8-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-alpha-16.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray-alpha-8.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/gray_4bpp.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/icon.png",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/indexed.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/rgb-16-alpha.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/rgb-48bpp-interlaced.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/rgb-48bpp.png",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/rgb-8-tRNS.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/vim16x16_1.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/vim16x16_2.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/palette-8bpp.png",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Png/filter4.png",

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
            doc.ImageFactories.Add(new Options.PDFImageFactory("PNG", new System.Text.RegularExpressions.Regex(".*\\.png", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

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
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/rgb_deflate.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-minisblack-16_lsb.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/grayscale_deflate_multistrip.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/rgb_palette.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-palette-02.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-palette-04.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-02.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-08.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-10.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-12.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-14.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-16.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-16_lsb_zip_predictor.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-16_msb_zip_predictor.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-24.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-24_lsb.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-32.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-rgb-contig-32_lsb.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h1v1.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h2v1.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h2v2.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-ycbcr-contig-08_h4v4.tiff",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/flower-ycbcr-planar-08_h1v1.tiff",

            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Tiff/little_endian.tiff",

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
            doc.ImageFactories.Add(new Options.PDFImageFactory("TIFF", new System.Text.RegularExpressions.Regex(".*\\.tiff", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

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
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/GlobalQuantizationTest.gif",
            "https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/base_1x4.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/base_4x1.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/cheers.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/giphy.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/image-zero-height.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/image-zero-size.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/image-zero-width.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/kumin.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/large_comment.gif",
            //"https://media.githubusercontent.com/media/SixLabors/ImageSharp/master/tests/Images/Input/Gif/leo.gif",

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
            doc.ImageFactories.Add(new Options.PDFImageFactory("GIF", new System.Text.RegularExpressions.Regex(".*\\.gif", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

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
