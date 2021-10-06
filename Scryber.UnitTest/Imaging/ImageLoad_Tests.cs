using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using io = System.IO;
using System.IO;
using Scryber.Drawing;

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
            img.BorderColor = PDFColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);
            page.Padding = new PDFThickness(20);

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
            img.BorderColor = PDFColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);

            page.Padding = new PDFThickness(20);
            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;

            using (var stream = DocStreams.GetOutputStream("NewImagingTest_Performance.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);
        }


        [TestMethod()]
        public void WritePreviousPngFromFile()
        {
            var doc = new Document();
            var page = new Page();
            doc.Pages.Add(page);

            
            var path = io.Path.Combine(io.Directory.GetCurrentDirectory(), PathToImages, "Group.png");
            path = io.Path.GetFullPath(path);

            if (!io.File.Exists(path))
                throw new FileNotFoundException(path);

            Image img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);
            page.Padding = new PDFThickness(20);
            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;

            using (var stream = DocStreams.GetOutputStream("PrevImagingTest_Group.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);

            //Second time for performance

            doc = new Document();
            page = new Page();
            doc.Pages.Add(page);
            img = new Image();
            img.Source = path;
            img.BorderColor = PDFColors.Black;
            img.BorderStyle = LineType.Solid;

            page.Contents.Add(img);

            page.Padding = new PDFThickness(20);
            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;

            using (var stream = DocStreams.GetOutputStream("PrevImagingTest_Performance.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            Assert.AreEqual(1, doc.SharedResources.Count);
        }

        
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
            page.Padding = new PDFThickness(20);

            var factory = new Scryber.Imaging.ImageFactoryPng();
            doc.ImageFactories.Add(new Options.PDFImageFactory("PNG", new System.Text.RegularExpressions.Regex(".*\\.png", System.Text.RegularExpressions.RegexOptions.IgnoreCase), factory));

            foreach (var src in allPng)
            {

                Image img = new Image();
                img.Source = src;
                img.BorderColor = PDFColors.Black;
                img.BorderStyle = LineType.Solid;
                img.Width = 100;

                page.Contents.Add(img);

                Span label = new Span();
                label.Contents.Add(new TextLiteral(System.IO.Path.GetFileNameWithoutExtension(src)));
                label.Margins = new PDFThickness(0, 0, 10, 0);
                page.Contents.Add(label);

            }

            doc.RenderOptions.Compression = OutputCompressionType.FlateDecode;
            doc.AppendTraceLog = true;
            doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
            using (var stream = DocStreams.GetOutputStream("PngImageTypes.pdf"))
            {
                doc.SaveAsPDF(stream);
            }
        }

    }
}
