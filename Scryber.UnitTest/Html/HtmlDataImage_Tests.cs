using System;
using System.IO;
using Scryber.Components;
using Scryber.PDF.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF;


namespace Scryber.Core.UnitTests.Html
{
    [TestClass]
    public class HtmlDataImage_Tests
    {

        /// <summary>
        /// Checks the red dot image resource for validity
        /// </summary>
        public void AssertRedDotImage(PDFImageXObject img)
        {
            Assert.IsNotNull(img.ImageData, "The resource had no data");
            var data = img.ImageData;
            
            Assert.AreEqual(5, data.PixelWidth, "Expected a 5 pixel wide image");
            Assert.AreEqual(5, data.PixelHeight, "Expected a 5 pixel high image");
            Assert.AreEqual(8, data.BitsPerColor);
        }

        /// <summary>
        /// Check to make sure the image data is as expected for a Png image
        /// </summary>
        /// <param name="img"></param>
        /// <param name="compressed"></param>
        public void AssertPngImage(PDFImageXObject img, bool compressed)
        {
            Assert.IsTrue(img.ImageData.HasAlpha);
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
            Assert.AreEqual(8, img.ImageData.BitsPerColor, "Expected a 24 bit RGB image");
            Assert.IsFalse(img.ImageData.HasAlpha, "Jpeg images should not have an alpha");
            Assert.IsTrue(img.ImageData.IsPrecompressedData,"Jpeg images should be pre-compressed");
            Assert.AreEqual(1, img.ImageData.Filters.Length, "Expected a single filter"); 
            Assert.AreEqual("DCTDecode", img.ImageData.Filters[0].FilterName, "Expected the filter name to be DCTDecode");
     
        }
        
        
        /// <summary>
        /// An image in the content with a data url
        /// </summary>
        [TestMethod]
        public void DataImageFactory_Test()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' ><body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
                    </body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImage.pdf"))
                    {
                        doc.SaveAsPDF(stream);


                        Assert.AreEqual(1, doc.SharedResources.Count);
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        
                        AssertRedDotImage(one as PDFImageXObject);
                        AssertPngImage(one as PDFImageXObject, true);
                    }

                }
            }
        }
        
        [TestMethod]
        public void DataImageAsBackgroundTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    .bgimg{
        background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==');
        margin: 20pt;
    }
</style></head>
<body style='padding:20pt;' >
    <p class='bgimg' style='width:140pt; height:100pt'>Content</p>
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageAsBackground.pdf"))
                    {
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(2, doc.SharedResources.Count);

                        PDFImageXObject img = null;
                        var one = doc.SharedResources[0];
                        var two = doc.SharedResources[1];
                        if (one is PDFImageXObject)
                            img = (PDFImageXObject) one;
                        else if (two is PDFImageXObject)
                            img = (PDFImageXObject) two;
                        
                        Assert.IsNotNull(img, "The image resource was not found");
                        //TODO: Add the red dot assertions.
                        this.AssertRedDotImage(img);
                        this.AssertPngImage(img, true);

                    }

                }
            }
        }
        
        [TestMethod]
        public void DataImageWithWhiteSpaceTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    
</style></head>
<body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAF
                                    CAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHx
                                    gljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
    
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageWithWhitespace.pdf"))
                    {
                        doc.AppendTraceLog = true;
                        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);
                        
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(1, doc.SharedResources.Count);

                        
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        AssertRedDotImage(one as PDFImageXObject);
                        AssertPngImage(one as PDFImageXObject, true);
                    }

                }
            }
        }
        
        [TestMethod]
        public void DataImageWithWhiteSpaceNoCompressionTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    
</style></head>
<body style='padding:20pt;' >
                    <img src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAF
                                    CAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHx
                                    gljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==' alt='Red dot' />
    
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageWithWhitespace.pdf"))
                    {
                        doc.AppendTraceLog = true;
                        doc.RenderOptions.Compression = OutputCompressionType.None;
                        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Messages);
                        
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(1, doc.SharedResources.Count);

                        
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));
                        AssertRedDotImage(one as PDFImageXObject);
                        AssertPngImage(one as PDFImageXObject, false);
                    }

                }
            }
        }
        
        [TestMethod]
        public void DataImageBackgroundWithWhiteSpaceTest()
        {
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head><style>
    .bgimg{
        background-image: url('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P
                                                     4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==');
        margin: 20pt;
    }
</style></head>
<body style='padding:20pt;' >
         
    <p class='bgimg' style='width:140pt; height:100pt'></p>
</body></html>";

            using (var sr = new System.IO.StringReader(html))
            {
                using (var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent))
                {
                    using (var stream = DocStreams.GetOutputStream("DataImageWithWhitespace.pdf"))
                    {
                        doc.AppendTraceLog = true;
                        doc.TraceLog.SetRecordLevel(TraceRecordLevel.Verbose);
                        
                        doc.SaveAsPDF(stream);

                        Assert.AreEqual(1, doc.SharedResources.Count); 

                        
                        var one = doc.SharedResources[0];
                        Assert.IsInstanceOfType(one, typeof(PDFImageXObject));

                        AssertRedDotImage(one as PDFImageXObject);
                        AssertPngImage(one as PDFImageXObject, true);
                    }

                }
            }
        }
        
        /// <summary>
        /// A large jpeg data image
        /// </summary>
        [TestMethod]
        public void DataImageWithLargeJpeg()
        {
            
            var img =
                @"data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAkACQAAD/4QB0RXhpZgAATU0AKgAAAAgABAESAAMAAAABAAEAAAEaAAUAAAABAAAAPgEb
AAUAAAABAAAARodpAAQAAAABAAAATgAAAAAAAACQAAAAAQAAAJAAAAABAAKgAgAEAAAAAQAAAYygAwAEAAAAAQAAAVYAAAAA/+IMWElDQ19QUk9GSUxFAAEBAAAMSExpb
m8CEAAAbW50clJHQiBYWVogB84AAgAJAAYAMQAAYWNzcE1TRlQAAAAASUVDIHNSR0IAAAAAAAAAAAAAAAAAAPbWAAEAAAAA0y1IUCAgAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAARY3BydAAAAVAAAAAzZGVzYwAAAYQAAABsd3RwdAAAAfAAAAAUYmtwdAAAAgQAAAAUclhZWgAAAhgAAAAUZ1hZWgAAAiw
AAAAUYlhZWgAAAkAAAAAUZG1uZAAAAlQAAABwZG1kZAAAAsQAAACIdnVlZAAAA0wAAACGdmlldwAAA9QAAAAkbHVtaQAAA/gAAAAUbWVhcwAABAwAAAAkdGVjaAAABDAAA
AAMclRSQwAABDwAAAgMZ1RSQwAABDwAAAgMYlRSQwAABDwAAAgMdGV4dAAAAABDb3B5cmlnaHQgKGMpIDE5OTggSGV3bGV0dC1QYWNrYXJkIENvbXBhbnkAAGRlc2MAAA
AAAAAAEnNSR0IgSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAASc1JHQiBJRUM2MTk2Ni0yLjEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAFhZWiAAAAAAAADzUQABAAAAARbMWFlaIAAAAAAAAAAAAAAAAAAAAABYWVogAAAAAAAAb6IAADj1AAADkFhZWiAAAAAAAABimQAAt4UAABjaWFlaIAAAAAAAACSgA
AAPhAAAts9kZXNjAAAAAAAAABZJRUMgaHR0cDovL3d3dy5pZWMuY2gAAAAAAAAAAAAAABZJRUMgaHR0cDovL3d3dy5pZWMuY2gAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZGVzYwAAAAAAAAAuSUVDIDYxOTY2LTIuMSBEZWZhdWx0IFJHQiBjb2xvdXIgc3BhY2UgLSBzUkdCAAAAAAAAAAAAAAAuSUVDIDYx
OTY2LTIuMSBEZWZhdWx0IFJHQiBjb2xvdXIgc3BhY2UgLSBzUkdCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGRlc2MAAAAAAAAALFJlZmVyZW5jZSBWaWV3aW5nIENvbmRpdG
lvbiBpbiBJRUM2MTk2Ni0yLjEAAAAAAAAAAAAAACxSZWZlcmVuY2UgVmlld2luZyBDb25kaXRpb24gaW4gSUVDNjE5NjYtMi4xAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
AAB2aWV3AAAAAAATpP4AFF8uABDPFAAD7cwABBMLAANcngAAAAFYWVogAAAAAABMCVYAUAAAAFcf521lYXMAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAAKPAAAAAnNpZy
AAAAAAQ1JUIGN1cnYAAAAAAAAEAAAAAAUACgAPABQAGQAeACMAKAAtADIANwA7AEAARQBKAE8AVABZAF4AYwBoAG0AcgB3AHwAgQCGAIsAkACVAJoAnwCkAKkArgCyALcA
vADBAMYAywDQANUA2wDgAOUA6wDwAPYA+wEBAQcBDQETARkBHwElASsBMgE4AT4BRQFMAVIBWQFgAWcBbgF1AXwBgwGLAZIBmgGhAakBsQG5AcEByQHRAdkB4QHpAfIB+g
IDAgwCFAIdAiYCLwI4AkECSwJUAl0CZwJxAnoChAKOApgCogKsArYCwQLLAtUC4ALrAvUDAAMLAxYDIQMtAzgDQwNPA1oDZgNyA34DigOWA6IDrgO6A8cD0wPgA+wD+QQG
BBMEIAQtBDsESARVBGMEcQR+BIwEmgSoBLYExATTBOEE8AT+BQ0FHAUrBToFSQVYBWcFdwWGBZYFpgW1BcUF1QXlBfYGBgYWBicGNwZIBlkGagZ7BowGnQavBsAG0QbjBv
UHBwcZBysHPQdPB2EHdAeGB5kHrAe/B9IH5Qf4CAsIHwgyCEYIWghuCIIIlgiqCL4I0gjnCPsJEAklCToJTwlkCXkJjwmkCboJzwnlCfsKEQonCj0KVApqCoEKmAquCsUK
3ArzCwsLIgs5C1ELaQuAC5gLsAvIC+EL+QwSDCoMQwxcDHUMjgynDMAM2QzzDQ0NJg1ADVoNdA2ODakNww3eDfgOEw4uDkkOZA5/DpsOtg7SDu4PCQ8lD0EPXg96D5YPsw
/PD+wQCRAmEEMQYRB+EJsQuRDXEPURExExEU8RbRGMEaoRyRHoEgcSJhJFEmQShBKjEsMS4xMDEyMTQxNjE4MTpBPFE+UUBhQnFEkUahSLFK0UzhTwFRIVNBVWFXgVmxW9
FeAWAxYmFkkWbBaPFrIW1hb6Fx0XQRdlF4kXrhfSF/cYGxhAGGUYihivGNUY+hkgGUUZaxmRGbcZ3RoEGioaURp3Gp4axRrsGxQbOxtjG4obshvaHAIcKhxSHHscoxzMHP
UdHh1HHXAdmR3DHeweFh5AHmoelB6+HukfEx8+H2kflB+/H+ogFSBBIGwgmCDEIPAhHCFIIXUhoSHOIfsiJyJVIoIiryLdIwojOCNmI5QjwiPwJB8kTSR8JKsk2iUJJTgl
aCWXJccl9yYnJlcmhya3JugnGCdJJ3onqyfcKA0oPyhxKKIo1CkGKTgpaymdKdAqAio1KmgqmyrPKwIrNitpK50r0SwFLDksbiyiLNctDC1BLXYtqy3hLhYuTC6CLrcu7i
8kL1ovkS/HL/4wNTBsMKQw2zESMUoxgjG6MfIyKjJjMpsy1DMNM0YzfzO4M/E0KzRlNJ402DUTNU01hzXCNf02NzZyNq426TckN2A3nDfXOBQ4UDiMOMg5BTlCOX85vDn5
OjY6dDqyOu87LTtrO6o76DwnPGU8pDzjPSI9YT2hPeA+ID5gPqA+4D8hP2E/oj/iQCNAZECmQOdBKUFqQaxB7kIwQnJCtUL3QzpDfUPARANER0SKRM5FEkVVRZpF3kYiRm
dGq0bwRzVHe0fASAVIS0iRSNdJHUljSalJ8Eo3Sn1KxEsMS1NLmkviTCpMcky6TQJNSk2TTdxOJU5uTrdPAE9JT5NP3VAnUHFQu1EGUVBRm1HmUjFSfFLHUxNTX1OqU/ZU
QlSPVNtVKFV1VcJWD1ZcVqlW91dEV5JX4FgvWH1Yy1kaWWlZuFoHWlZaplr1W0VblVvlXDVchlzWXSddeF3JXhpebF69Xw9fYV+zYAVgV2CqYPxhT2GiYfViSWKcYvBjQ2
OXY+tkQGSUZOllPWWSZedmPWaSZuhnPWeTZ+loP2iWaOxpQ2maafFqSGqfavdrT2una/9sV2yvbQhtYG25bhJua27Ebx5veG/RcCtwhnDgcTpxlXHwcktypnMBc11zuHQU
dHB0zHUodYV14XY+dpt2+HdWd7N4EXhueMx5KnmJeed6RnqlewR7Y3vCfCF8gXzhfUF9oX4BfmJ+wn8jf4R/5YBHgKiBCoFrgc2CMIKSgvSDV4O6hB2EgITjhUeFq4YOhn
KG14c7h5+IBIhpiM6JM4mZif6KZIrKizCLlov8jGOMyo0xjZiN/45mjs6PNo+ekAaQbpDWkT+RqJIRknqS45NNk7aUIJSKlPSVX5XJljSWn5cKl3WX4JhMmLiZJJmQmfya
aJrVm0Kbr5wcnImc951kndKeQJ6unx2fi5/6oGmg2KFHobaiJqKWowajdqPmpFakx6U4pammGqaLpv2nbqfgqFKoxKk3qamqHKqPqwKrdavprFys0K1ErbiuLa6hrxavi7
AAsHWw6rFgsdayS7LCszizrrQltJy1E7WKtgG2ebbwt2i34LhZuNG5SrnCuju6tbsuu6e8IbybvRW9j74KvoS+/796v/XAcMDswWfB48JfwtvDWMPUxFHEzsVLxcjGRsbD
x0HHv8g9yLzJOsm5yjjKt8s2y7bMNcy1zTXNtc42zrbPN8+40DnQutE80b7SP9LB00TTxtRJ1MvVTtXR1lXW2Ndc1+DYZNjo2WzZ8dp22vvbgNwF3IrdEN2W3hzeot8p36
/gNuC94UThzOJT4tvjY+Pr5HPk/OWE5g3mlucf56noMui86Ubp0Opb6uXrcOv77IbtEe2c7ijutO9A78zwWPDl8XLx//KM8xnzp/Q09ML1UPXe9m32+/eK+Bn4qPk4+cf6
V/rn+3f8B/yY/Sn9uv5L/tz/bf///+EJIGh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8APD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6Tl
RjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iWE1QIENvcmUgNi4wLjAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6
Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIi8+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC
AgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
PD94cGFja2V0IGVuZD0idyI/Pv/+ACVSZXNpemVkIG9uIGh0dHBzOi8vZXpnaWYuY29tL3Jlc2l6Zf/bAEMAAwICAgICAwICAgMDAwMEBgQEBAQECAYGBQYJCAoKCQgJCQ
oMDwwKCw4LCQkNEQ0ODxAQERAKDBITEhATDxAQEP/bAEMBAwMDBAMECAQECBALCQsQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQ
EP/AABEIACMAKAMBIgACEQEDEQH/xAAdAAACAQQDAAAAAAAAAAAAAAAABwgBAgMGBAUJ/8QALhAAAQQCAgEDAgQHAQAAAAAAAgEDBAUGBwAREggTIRQiMVFTkhUXGEFicX
fU/8QAFwEAAwEAAAAAAAAAAAAAAAAAAAECA//EABkRAAMBAQEAAAAAAAAAAAAAAAABAhEhEv/aAAwDAQACEQMRAD8A9K+UVUT5XnFt7esoKmdfXc5mFXVkZ2ZMkvF4tsMN
ApuOEv8AZBEVVf8AXIv7e3/jG0sbx/VbdJmuKtbGv6OvRy9qjrFuMfkTWgmHFcQ1XowJtsxLwdEJKL4ohdpUy6fCapSOqm9Qmh8iycMLodyYZYXrrqx2oEa5ZN111Pxbb6
LxcP4X7RVV+Pw4weadmeq9eZfrqdre4xWqYxxyCcdmKxEbZbgIIL7bkfxRPZNpUEgMOlFQRUX44ldLer7G7LVeG2OZwcsnG1XVVdk2WsU5OU0G0ebbFBkyfJFEyVxonCECB
snh8yH56fn0tkXry8ok3w4EiiSiSdKKqip+S8OQWKn1E5Lj6YVM1PNosgyG42PV2VNX01Ay2c11pWPB+R5umDLLTSOtqTrpoKKQD8qSJyLuwMhybPsywjWO3cKt7/IMdwp+
NNi6xJq1n0GSvGz4zJSkYtxlbbiRnGkU+vccc8uwFF5KLfep6HNaKXnPhkoZTitBajSO0N3MrnzI2kd9hfpjFXUN1hn7V77VE4udGbr09q7UuMYYFDsKNNj1sd64L+X14bk
uzcbE5ch5z6VfddN5XFI1VVX8+uubw8nZWsxta8Z2ufSvUjkXproYVric1rL78hrs4Zx4Ix2UOrcR4ZL0Fs30Z+qNtGU8UcNG1ecUEJQFOIfGrPJMywnaeg9W62fqaidlE
Sa1h1zJYrMqj0DntrZpHiPmLZD5xmhYMnOupB+S9gPcnf6rdTddfw7YfX/O7z/y8VXqN2ZrrcWIVUDEqDZDmYVN7WSKOXGw68rJUcDltNTG25vsArAORTfE/vFFTpV+UTj
h0uORVj6mSP13sjH9pULuRUMexhFGmv11hXWcVY02tmtKnuxpDSqviY+Qr8KokJCQqqEi8OY9dasxHU8G0qcRW1dC1s3LSbJtLWRYyZMkgBpXDefIjX7GmxRO/wABThzB5
vDZbnTbOX+8/wDrufvXhw4hh7z/AOu5+9eUV55U6V5xUX/NeHDgBbw4cOAH/9k=";
            
            var html = @"<html xmlns='http://www.w3.org/1999/xhtml' >
<head></head>
<body style='padding:20pt;' >
                    <img src='" + img + @"' alt='Group.jpeg' style='width:50pt' />
</body></html>";

            using var reader = new StringReader(html);
            using var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            using var output = DocStreams.GetOutputStream("DataImageWithLargeJpeg.pdf");
            doc.ConformanceMode = ParserConformanceMode.Strict;
            doc.RenderOptions.AllowMissingImages = false;
            doc.SaveAsPDF(output);
            
            Assert.AreEqual(1, doc.SharedResources.Count,"Expected only 1 resource");
            Assert.IsInstanceOfType(doc.SharedResources[0], typeof(PDFImageXObject),"Resource should be an Image XObject");
            AssertJpgImage(doc.SharedResources[0] as PDFImageXObject);
            
        }
    }
}