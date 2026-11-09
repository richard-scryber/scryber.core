using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;

namespace Scryber.Core.UnitTests.Imaging
{
    /// <summary>
    /// Covers the EXIF metadata capture/extraction pipeline end to end: the ImageEXIFMap/Value
    /// classes themselves, ImageEXIFExtractor's generic tag sweep and GPS DMS-to-decimal
    /// combination, and the meta() expression function reaching all the way through to a real
    /// document. Uses synthetic EXIF data built directly via SixLabors (not a real photo file),
    /// so these are fully portable/CI-safe.
    /// </summary>
    [TestClass()]
    public class ImageExifMetadata_Tests
    {
        // -----------------------------------------------------------------------
        // ImageEXIFMap / ImageEXIFValue
        // -----------------------------------------------------------------------

        [TestMethod()]
        public void ImageEXIFMap_SetGet_RoundTrips()
        {
            var map = new ImageEXIFMap();
            map.Set("Make", new ImageEXIFValueString("Acme"));
            map.Set("ISO", new ImageEXIFValueNumber(ImageEXIFValueType.Int, 400));

            Assert.AreEqual("Acme", map.Get("Make").ToString());
            Assert.AreEqual("400", map.Get("ISO").ToString());
            Assert.IsNull(map.Get("NoSuchKey"));
            Assert.AreEqual(2, map.Count);
            CollectionAssert.AreEquivalent(new[] { "Make", "ISO" }, new System.Collections.Generic.List<string>(map.Keys));
        }

        [TestMethod()]
        public void ImageEXIFValueNumber_ToString_FormatsByType()
        {
            Assert.AreEqual("400", new ImageEXIFValueNumber(ImageEXIFValueType.Int, 400.7).ToString());
            Assert.AreEqual("51.507400", new ImageEXIFValueNumber(ImageEXIFValueType.Coord, 51.5074).ToString());
            Assert.AreEqual("1.78", new ImageEXIFValueNumber(ImageEXIFValueType.Real, 1.78).ToString());
        }

        [TestMethod()]
        public void ImageEXIFValue_CompareTo_OrdersByTypeThenValue()
        {
            var a = new ImageEXIFValueNumber(ImageEXIFValueType.Real, 1.0);
            var b = new ImageEXIFValueNumber(ImageEXIFValueType.Real, 2.0);
            Assert.IsTrue(a.CompareTo(b) < 0);
            Assert.IsTrue(a.Equals(new ImageEXIFValueNumber(ImageEXIFValueType.Real, 1.0)));
            Assert.IsFalse(a.Equals(b));
        }

        // -----------------------------------------------------------------------
        // ImageEXIFExtractor - synthetic EXIF profile, including GPS
        // -----------------------------------------------------------------------

        private static ExifProfile BuildSyntheticProfile()
        {
            var profile = new ExifProfile();
            profile.SetValue(ExifTag.Make, "Acme");
            profile.SetValue(ExifTag.Model, "TestCam 1000");
            profile.SetValue(ExifTag.Orientation, (ushort)1);
            profile.SetValue(ExifTag.FNumber, new Rational(1.8));

            // 51 30' 26.64" N, 0 7' 39.6" W - roughly central London.
            profile.SetValue(ExifTag.GPSLatitude, new[] { new Rational(51), new Rational(30), new Rational(26.64) });
            profile.SetValue(ExifTag.GPSLatitudeRef, "N");
            profile.SetValue(ExifTag.GPSLongitude, new[] { new Rational(0), new Rational(7), new Rational(39.6) });
            profile.SetValue(ExifTag.GPSLongitudeRef, "W");

            return profile;
        }

        [TestMethod()]
        public void ImageEXIFExtractor_GenericSweep_MapsCommonFields()
        {
            var map = ImageEXIFExtractor.Extract(BuildSyntheticProfile());

            Assert.IsNotNull(map);
            Assert.AreEqual("Acme", map.Get("Make").ToString());
            Assert.AreEqual("TestCam 1000", map.Get("Model").ToString());
            Assert.AreEqual(ImageEXIFValueType.String, map.Get("Make").Type);
            Assert.AreEqual(ImageEXIFValueType.Int, map.Get("Orientation").Type);
            Assert.AreEqual(ImageEXIFValueType.Real, map.Get("FNumber").Type);
        }

        [TestMethod()]
        public void ImageEXIFExtractor_GPS_CombinesDmsAndRefIntoSignedDecimalDegrees()
        {
            var map = ImageEXIFExtractor.Extract(BuildSyntheticProfile());

            var lat = map.Get("GPSLatitude");
            var lon = map.Get("GPSLongitude");

            Assert.IsNotNull(lat);
            Assert.IsNotNull(lon);
            Assert.AreEqual(ImageEXIFValueType.Coord, lat.Type);
            Assert.AreEqual(ImageEXIFValueType.Coord, lon.Type);

            // 51 + 30/60 + 26.64/3600 = 51.5074, positive (N)
            Assert.AreEqual(51.5074, ((ImageEXIFValueNumber)lat).Value, 0.0001);
            // 0 + 7/60 + 39.6/3600 = 0.1277, negated (W)
            Assert.AreEqual(-0.1277, ((ImageEXIFValueNumber)lon).Value, 0.0001);
        }

        [TestMethod()]
        public void ImageEXIFExtractor_NoProfile_ReturnsNull()
        {
            Assert.IsNull(ImageEXIFExtractor.Extract((ExifProfile)null));
            Assert.IsNull(ImageEXIFExtractor.Extract((SixLabors.ImageSharp.Image)null));
        }

        // -----------------------------------------------------------------------
        // End to end: real image bytes carrying EXIF, through ImageFactoryJpeg
        // -----------------------------------------------------------------------

        private static byte[] BuildJpegWithExif()
        {
            using var image = new Image<Rgba32>(4, 4);
            image.Metadata.ExifProfile = BuildSyntheticProfile();

            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms);
            return ms.ToArray();
        }

        [TestMethod()]
        public void ImageFactoryJpeg_LoadRawData_PopulatesExifMetadata()
        {
            var doc = new Document();
            var page = new Page();
            var factory = new ImageFactoryJpeg();
            var raw = BuildJpegWithExif();

            var data = factory.LoadImageData(doc, page, raw, MimeType.JpegImage);

            Assert.IsNotNull(data);
            Assert.IsNotNull(data.ExifMetadata, "A JPEG saved with an attached ExifProfile should come back with ExifMetadata populated");
            Assert.AreEqual("Acme", data.ExifMetadata.Get("Make").ToString());
            Assert.AreEqual(51.5074, ((ImageEXIFValueNumber)data.ExifMetadata.Get("GPSLatitude")).Value, 0.0001);
        }

        // -----------------------------------------------------------------------
        // meta() expression function - full document pipeline via a data: URL
        // -----------------------------------------------------------------------

        private static string BuildJpegDataUrl()
        {
            var raw = BuildJpegWithExif();
            return "data:image/jpeg;base64," + Convert.ToBase64String(raw);
        }

        [TestMethod()]
        public void MetaFunction_TwoArg_ReturnsSingleValue()
        {
            var dataUrl = BuildJpegDataUrl();
            var html = "<html><body>" +
                        "<p id='make'>{{meta('" + dataUrl + "', 'Make')}}</p>" +
                        "<p id='lat'>{{meta('" + dataUrl + "', 'GPSLatitude')}}</p>" +
                        "<p id='missing'>{{meta('" + dataUrl + "', 'NoSuchKey')}}</p>" +
                        "</body></html>";

            var doc = Document.ParseHtmlDocument(new StringReader(html));
            using var ms = DocStreams.GetOutputStream("MetaFunction_TwoArg.pdf");
            doc.SaveAsPDF(ms);

            Assert.AreEqual("Acme", GetText(doc.FindAComponentById("make")));
            Assert.AreEqual("51.507400", GetText(doc.FindAComponentById("lat")));
            Assert.AreEqual(string.Empty, GetText(doc.FindAComponentById("missing")));
        }

        [TestMethod()]
        public void MetaFunction_OneArg_ReturnsKeyListUsableWithCount()
        {
            var dataUrl = BuildJpegDataUrl();
            var html = "<html><body>" +
                        "<p id='keycount'>{{count(meta('" + dataUrl + "'))}}</p>" +
                        "</body></html>";

            var doc = Document.ParseHtmlDocument(new StringReader(html));
            using var ms = DocStreams.GetOutputStream("MetaFunction_OneArg.pdf");
            doc.SaveAsPDF(ms);

            //Make, Model, Orientation, FNumber, GPSLatitude(Ref), GPSLongitude(Ref) - at least
            //the fields explicitly set, not an exact count (SixLabors may add its own).
            var count = int.Parse(GetText(doc.FindAComponentById("keycount")));
            Assert.IsTrue(count >= 6, "Expected at least the 6 explicitly-set EXIF fields (got " + count + ")");
        }

        [TestMethod()]
        public void MetaFunction_UnknownImage_ReturnsEmptyNotError()
        {
            var html = "<html><body>" +
                        "<p id='val'>{{meta('does-not-exist.jpg', 'Make')}}</p>" +
                        "<p id='keys'>{{count(meta('does-not-exist.jpg'))}}</p>" +
                        "</body></html>";

            var doc = Document.ParseHtmlDocument(new StringReader(html));
            doc.ConformanceMode = ParserConformanceMode.Lax;
            using var ms = DocStreams.GetOutputStream("MetaFunction_Unknown.pdf");
            doc.SaveAsPDF(ms);

            Assert.AreEqual(string.Empty, GetText(doc.FindAComponentById("val")));
            Assert.AreEqual("0", GetText(doc.FindAComponentById("keys")));
        }

        private static string GetText(IComponent comp)
        {
            if (comp is IContainerComponent cc && cc.HasContent)
            {
                foreach (var c in cc.Content)
                {
                    if (c is ITextLiteral tl)
                        return tl.Text ?? string.Empty;
                }
            }
            return string.Empty;
        }
    }
}
