using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scryber.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.PDF.Resources;
using System.IO;

namespace Scryber.Core.UnitTests.Generation
{

    /// <summary>
    /// Tests each of the static ParseDocument, ParseHtml and Parse methods on the Scryber.Components.Document class
    /// </summary>
    [TestClass()]
    public class ParseDocument_Tests
    {

        const int GroupImgPixelWidth = 396;
        const int GroupImgPixelHeight = 342;

        //
        // XHTML ParseDocument methods
        //

        #region public void ParseXHTMLLocalFile()

        /// <summary>
        /// Test of ParseDocument with the full path to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLLocalFile()
        {
            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, "../../../Content/HTML/HelloWorld.xhtml");
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));

            using (Document doc = Document.ParseDocument(fullpath))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_LocalXhtml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(fullpath, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(7.5, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(10.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("<‘Inside’>", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.
                //However the lsquo and rsquo should be resolved as they are OK in html and invalid in XML.


                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual("./Images/group.png", img.Source);
                Assert.AreEqual(7.5, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);
                //Not successfull so 1x1 - unsucessfull image data proxy

                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }


        }

        #endregion

        #region public void ParseXHTMLRelativeFile()

        /// <summary>
        /// Test of ParseDocument with the relative path to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLRelativeFile()
        {
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));


            using (Document doc = Document.ParseDocument(relative))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_RelativeXhtml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(relative, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(7.5, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(10.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("<‘Inside’>", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.
                //However the lsquo and rsquo should be resolved as they are OK in html and invalid in XML.


                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual("./Images/group.png", img.Source);
                Assert.AreEqual(7.5, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);
                

                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }
        }

        #endregion

        #region public void ParseXHTMLStream()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLStream()
        {
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (Document doc = Document.ParseDocument(stream))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamXhtml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNull(doc.LoadedSource); //No path provided
                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                    //first div has a bound literal
                    var div = pg.FindAComponentById("div1") as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(7.5, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(10.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("<‘Inside’>", lit.Text);
                    
                    
                    var img = pg.FindAComponentById("img1") as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(7.5, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //just the font

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    //As a relative image, this will then be based on the working directory

                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy
              
                    Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseXHTMLTextReader()

        /// <summary>
        /// Test of ParseDocument with text reader to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLTextReader()
        {
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (Document doc = Document.ParseDocument(reader))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderXhtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNull(doc.LoadedSource); //No path provided
                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(7.5, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(10.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(7.5, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        //As a relative image, this will then be based on the working directory

                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseXHTMLXmlReader()

        /// <summary>
        /// Test of ParseDocument with XMLReader to the xhtml file HellowWorld.xhtml (which is not valid xml)
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLXmlReader()
        {
            
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            
            bool caught = false;

            try
            {
                using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
                {
                    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                    settings.DtdProcessing = System.Xml.DtdProcessing.Ignore;

                    using (var reader = System.Xml.XmlReader.Create(stream, settings))
                    {
                        using (Document doc = Document.ParseDocument(reader))
                        {
                            doc.Params["title"] = "Hello World & everyone in it.";

                            using (var ms = DocStreams.GetOutputStream("HelloWorld_XmlReaderXhtml.pdf"))
                            {
                                doc.SaveAsPDF(ms);
                            }



                        }
                    }
                }
            }
            catch (PDFParserException)
            {
                //Should not parse due to &lsquo; in the content
                caught = true;
            }


            Assert.IsTrue(caught, "A parser exception should have been raised with the helloworld.xhtml file");
        }

        #endregion

        #region public void ParseXHTMLStreamWithRelativePath()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the xhtml file HellowWorld.xhtml, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLStreamWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                //Pass the relative path to the parsing
                using (Document doc = Document.ParseDocument(stream, relative, ParseSourceType.LocalFile))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamRelativePathXhtml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                    Assert.AreEqual(relative, doc.LoadedSource);

                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                    //first div has a bound literal
                    var div = pg.FindAComponentById("div1") as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(7.5, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(10.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("<‘Inside’>", lit.Text);


                    var img = pg.FindAComponentById("img1") as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(7.5, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //image and font

                    //Because we provide the relative path to the source,
                    //the document should be able to find and load the image

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy

                    Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseXHTMLStreamWithRemotePath()

        /// <summary>
        /// Test of ParseDocument with the a remote url stream to the xhtml file HellowWorld.xhtml, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLStreamWithRemotePath()
        {
            var remote = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/HelloWorld.xhtml";

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";

            using (var client = new System.Net.Http.HttpClient())
            {
                using (var stream = client.GetStreamAsync(remote).Result)
                {
                    //Pass the relative path to the parsing
                    using (Document doc = Document.ParseDocument(stream, remote, ParseSourceType.RemoteFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamRemotePathXhtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(remote, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(7.5, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(10.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(7.5, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //image and font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseXHTMLTextReaderWithPath()

        /// <summary>
        /// Test of ParseDocument with text reader to the xhtml file HellowWorld.xhtml, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLTextReaderWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    //Pass the relative path to the parsing
                    using (Document doc = Document.ParseDocument(reader, relative, ParseSourceType.LocalFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderRelativePathXhtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(relative, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(7.5, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(10.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(7.5, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseXHTMLXmlReaderWithPath()

        /// <summary>
        /// Test of ParseDocument with XMLReader to the xhtml file HellowWorld.xhtml (which is not valid xml so fails),
        /// providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXHTMLXmlReaderWithRelativePath()
        {

            var relative = "../../../Content/HTML/HelloWorld.xhtml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);


            bool caught = false;

            try
            {
                using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
                {
                    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                    settings.DtdProcessing = System.Xml.DtdProcessing.Ignore;

                    //Pass the relative path to the parsing
                    using (var reader = System.Xml.XmlReader.Create(stream, settings))
                    {
                        using (Document doc = Document.ParseDocument(reader, relative, ParseSourceType.LocalFile))
                        {
                            doc.Params["title"] = "Hello World & everyone in it.";

                            using (var ms = DocStreams.GetOutputStream("HelloWorld_XmlReaderRelativePathXhtml.pdf"))
                            {
                                doc.SaveAsPDF(ms);
                            }



                        }
                    }
                }
            }
            catch (PDFParserException)
            {
                //Should not parse due to &lsquo; in the content
                caught = true;
            }


            Assert.IsTrue(caught, "A parser exception should have been raised with the helloworld.xhtml file");
        }

        #endregion

        //
        // Parse qualified xml document
        //

        #region public void ParseXMLLocalFile()

        /// <summary>
        /// Test of ParseDocument with the full path to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLLocalFile()
        {
            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, "../../../Content/HTML/HelloWorld.xml");
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));

            using (Document doc = Document.ParseDocument(fullpath))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_LocalXml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(fullpath, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(20.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(20.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("<‘Inside’>", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.
                


                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual(15, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                Assert.AreEqual("./Images/group.png", img.Source);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);
                //Not successfull so 1x1 - unsucessfull image data proxy

                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }


        }

        #endregion

        #region public void ParseXMLRelativeFile()

        /// <summary>
        /// Test of ParseDocument with the relative path to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLRelativeFile()
        {
            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));


            using (Document doc = Document.ParseDocument(relative))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_RelativeXml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(relative, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(7, pg.Contents.Count); //2 divs and an image

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(20.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(20.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.
                //However the lsquo and rsquo should be resolved as they are OK in html and invalid in XML.


                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual("./Images/group.png", img.Source);
                Assert.AreEqual(15, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);


                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }
        }

        #endregion

        #region public void ParseXHTMLStream()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLStream()
        {
            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (Document doc = Document.ParseDocument(stream))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamXml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNull(doc.LoadedSource); //No path provided
                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                    //first div has a bound literal
                    var div = pg.Contents[0] as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(20.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(20.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                    var img = pg.Contents[2] as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(15, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //just the font

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    //As a relative image, this will then be based on the working directory

                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy

                    Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseXMLTextReader()

        /// <summary>
        /// Test of ParseDocument with text reader to the xml file HellowWorld.xml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLTextReader()
        {
            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (Document doc = Document.ParseDocument(reader))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderXml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNull(doc.LoadedSource); //No path provided
                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.Contents[0] as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(20, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.Contents[1] as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(20.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                        var img = pg.Contents[2] as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(15, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        //As a relative image, this will then be based on the working directory

                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseXMLXmlReader()

        /// <summary>
        /// Test of ParseDocument with XMLReader to the xml file HellowWorld.xml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLXmlReader()
        {

            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.DtdProcessing = System.Xml.DtdProcessing.Ignore;

                using (var reader = System.Xml.XmlReader.Create(stream, settings))
                {
                    using (Document doc = Document.ParseDocument(reader))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderXml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNull(doc.LoadedSource); //No path provided
                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.Contents[0] as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(20, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.Contents[1] as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(20.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                        var img = pg.Contents[2] as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(15, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        //As a relative image, this will then be based on the working directory

                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);



                    }
                }
            }

        }

        #endregion

        #region public void ParseXMLStreamWithRelativePath()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the xhtml file HellowWorld.xhtml, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLStreamWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                //Pass the relative path to the parsing
                using (Document doc = Document.ParseDocument(stream, relative, ParseSourceType.LocalFile))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamRelativePathXml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                    Assert.AreEqual(relative, doc.LoadedSource);

                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                    //first div has a bound literal
                    var div = pg.Contents[0] as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(20, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.Contents[1] as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(20.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                    var img = pg.Contents[2] as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(15, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //image and font

                    //Because we provide the relative path to the source,
                    //the document should be able to find and load the image

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy

                    Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseXMLTextReaderWithPath()

        /// <summary>
        /// Test of ParseDocument with text reader to the xhtml file HellowWorld.xhtml, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLTextReaderWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    //Pass the relative path to the parsing
                    using (Document doc = Document.ParseDocument(reader, relative, ParseSourceType.LocalFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderRelativePathXml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(relative, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.Contents[0] as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(20, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.Contents[1] as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(20.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                        var img = pg.Contents[2] as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(15, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseXMLXmlReaderWithPath()

        /// <summary>
        /// Test of ParseDocument with XMLReader to the xhtml file HellowWorld.xhtml (which is not valid xml so fails),
        /// providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseXMLXmlReaderWithRelativePath()
        {

            var relative = "../../../Content/HTML/HelloWorld.xml";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                settings.DtdProcessing = System.Xml.DtdProcessing.Ignore;

                //Pass the relative path to the parsing
                using (var reader = System.Xml.XmlReader.Create(stream, settings))
                {
                    using (Document doc = Document.ParseDocument(reader, relative, ParseSourceType.LocalFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_XmlReaderRelativePathXml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(relative, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(3, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.Contents[0] as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(20, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.Contents[1] as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(20.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("&lt;‘Inside’&gt;", lit.Text);


                        var img = pg.Contents[2] as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(15, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        //
        // Parse unstructured html document
        //



        #region public void ParseHtmlLocalFile()

        /// <summary>
        /// Test of ParseDocument with the full path to the html file HellowWorld.html
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlLocalFile()
        {
            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, "../../../Content/HTML/HelloWorld.html");
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));

            using (Document doc = Document.ParseHtmlDocument(fullpath))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_LocalHtml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(fullpath, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(9, pg.Contents.Count); //2 divs and an image + 2 lines of commments (ignored) and whitespace around each (x6)

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(3.75, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(5, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("<‘Inside’>", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.



                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual(5, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                Assert.AreEqual("./Images/group.png", img.Source);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);
                //Not successfull so 1x1 - unsucessfull image data proxy

                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }


        }

        #endregion

        #region public void ParseHtmlRelativeFile()

        /// <summary>
        /// Test of ParseDocument with the relative path to the html file HellowWorld.html
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlRelativeFile()
        {
            var relative = "../../../Content/HTML/HelloWorld.html";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "../../../Content/HTML/Images/group.png"));


            using (Document doc = Document.ParseHtmlDocument(relative))
            {
                doc.Params["title"] = "Hello World & everyone in it.";

                using (var ms = DocStreams.GetOutputStream("HelloWorld_RelativeHtml.pdf"))
                {
                    doc.SaveAsPDF(ms);
                }

                Assert.AreEqual(relative, doc.LoadedSource);
                Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                Assert.AreEqual(1, doc.Pages.Count);

                var pg = doc.Pages[0] as Page;

                Assert.IsNotNull(pg);
                Assert.AreEqual(9, pg.Contents.Count); //2 divs and an image

                //first div has a bound literal
                var div = pg.FindAComponentById("div1") as Div;

                Assert.IsNotNull(div);
                Assert.AreEqual(3.75, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                var lit = div.Contents[0] as TextLiteral;

                Assert.IsNotNull(lit);
                Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                div = pg.FindAComponentById("div2") as Div;
                Assert.IsNotNull(div);
                Assert.AreEqual(5.0, div.Padding.Left);
                Assert.AreEqual(1, div.Contents.Count);

                lit = div.Contents[0] as TextLiteral;
                Assert.AreEqual("<‘Inside’>", lit.Text);
                //The < and > symbols are not resolved with the XML reader, they are resolved with the inner parser at layout time.
                //However the lsquo and rsquo should be resolved as they are OK in html and invalid in XML.


                var img = pg.FindAComponentById("img1") as Image;
                Assert.AreEqual("./Images/group.png", img.Source);
                Assert.AreEqual(5, img.Padding.Left);
                Assert.AreEqual(100.0, img.Width);

                var shared = doc.SharedResources;
                Assert.AreEqual(2, shared.Count); //image and font

                var imgRsrc = shared[0] as PDFImageXObject;
                Assert.IsNotNull(imgRsrc);
                Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                Assert.IsNotNull(imgRsrc.ImageData);


                Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);
            }
        }

        #endregion

        #region public void ParseHtmlStream()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the html file HellowWorld.html
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlStream()
        {
            var relative = "../../../Content/HTML/HelloWorld.html";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (Document doc = Document.ParseHtmlDocument(stream))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamHtml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNull(doc.LoadedSource); //No path provided
                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(9, pg.Contents.Count);

                    //first div has a bound literal
                    var div = pg.FindAComponentById("div1") as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(3.75, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(5.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("<‘Inside’>", lit.Text);


                    var img = pg.FindAComponentById("img1") as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(5, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //just the font

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    //As a relative image, this will then be based on the working directory

                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy

                    Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseHtmlTextReader()

        /// <summary>
        /// Test of ParseDocument with text reader to the xhtml file HellowWorld.xhtml
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlTextReader()
        {
            var relative = "../../../Content/HTML/HelloWorld.html";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //As no path is defined on parsing, then the imge path will be relative to the working directory
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(path, "./Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    using (Document doc = Document.ParseHtmlDocument(reader))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderHtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNull(doc.LoadedSource); //No path provided
                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(9, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(3.75, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(5.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(5.0, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        //As a relative image, this will then be based on the working directory

                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file (relative to the working directory).
                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(1, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(1, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseHtmlXmlReader()

        /// 
        /// As the html is not valid xml there is no method and therefor no test for parsing an HTML document with an XML Reader
        /// 

        #endregion

        #region public void ParseHtmlStreamWithRelativePath()

        /// <summary>
        /// Test of ParseDocument with the a file stream to the html file HellowWorld.html, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlStreamWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.html";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                //Pass the relative path to the parsing
                using (Document doc = Document.ParseHtmlDocument(stream, relative, ParseSourceType.LocalFile))
                {
                    doc.Params["title"] = "Hello World & everyone in it.";

                    using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamRelativePathHtml.pdf"))
                    {
                        doc.SaveAsPDF(ms);
                    }

                    Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                    Assert.AreEqual(relative, doc.LoadedSource);

                    Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                    Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                    Assert.AreEqual(1, doc.Pages.Count);

                    var pg = doc.Pages[0] as Page;

                    Assert.IsNotNull(pg);
                    Assert.AreEqual(9, pg.Contents.Count);

                    //first div has a bound literal
                    var div = pg.FindAComponentById("div1") as Div;

                    Assert.IsNotNull(div);
                    Assert.AreEqual(3.75, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    var lit = div.Contents[0] as TextLiteral;

                    Assert.IsNotNull(lit);
                    Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                    div = pg.FindAComponentById("div2") as Div;
                    Assert.IsNotNull(div);
                    Assert.AreEqual(5.0, div.Padding.Left);
                    Assert.AreEqual(1, div.Contents.Count);

                    lit = div.Contents[0] as TextLiteral;
                    Assert.AreEqual("<‘Inside’>", lit.Text);


                    var img = pg.FindAComponentById("img1") as Image;
                    Assert.AreEqual("./Images/group.png", img.Source);
                    Assert.AreEqual(5.0, img.Padding.Left);
                    Assert.AreEqual(100.0, img.Width);

                    var shared = doc.SharedResources;
                    Assert.AreEqual(2, shared.Count); //image and font

                    //Because we provide the relative path to the source,
                    //the document should be able to find and load the image

                    var imgRsrc = shared[0] as PDFImageXObject;
                    Assert.IsNotNull(imgRsrc);
                    Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                    Assert.IsNotNull(imgRsrc.ImageData);
                    //Not successfull so 1x1 - unsucessfull image data proxy

                    Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                    Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                }
            }

        }

        #endregion

        #region public void ParseHtmlStreamWithRemotePath()

        /// <summary>
        /// Test of ParseDocument with the a remote url stream to the html file HellowWorld.html, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlStreamWithRemotePath()
        {
            var remote = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/HelloWorld.html";

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = "https://raw.githubusercontent.com/richard-scryber/scryber.core/master/Scryber.UnitTest/Content/HTML/Images/group.png";

            using (var client = new System.Net.Http.HttpClient())
            {
                using (var stream = client.GetStreamAsync(remote).Result)
                {
                    //Pass the relative path to the parsing
                    using (Document doc = Document.ParseHtmlDocument(stream, remote, ParseSourceType.RemoteFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_StreamRemotePathHtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(remote, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(9, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(3.75, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(5.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(5, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //image and font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion

        #region public void ParseHtmlTextReaderWithPath()

        /// <summary>
        /// Test of ParseDocument with text reader to the html file HellowWorld.html, providing the path and parse type
        /// </summary>
        [TestMethod()]
        [TestCategory("Document")]
        public void ParseHtmlTextReaderWithRelativePath()
        {
            var relative = "../../../Content/HTML/HelloWorld.html";

            var path = System.Environment.CurrentDirectory;

            var fullpath = System.IO.Path.Combine(path, relative);

            //With path defined on parsing, then the image path will be relative to that file path
            var imgfullSrc = System.IO.Path.GetFullPath(System.IO.Path.Combine(fullpath, "../Images/group.png"));

            using (var stream = new System.IO.FileStream(fullpath, System.IO.FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    //Pass the relative path to the parsing
                    using (Document doc = Document.ParseHtmlDocument(reader, relative, ParseSourceType.LocalFile))
                    {
                        doc.Params["title"] = "Hello World & everyone in it.";

                        using (var ms = DocStreams.GetOutputStream("HelloWorld_TextReaderRelativePathXhtml.pdf"))
                        {
                            doc.SaveAsPDF(ms);
                        }

                        Assert.IsNotNull(doc.LoadedSource); //Relative path provided
                        Assert.AreEqual(relative, doc.LoadedSource);

                        Assert.AreEqual(ParserLoadType.ReflectiveParser, doc.LoadType);

                        Assert.AreEqual("Hello World & everyone in it.", doc.Info.Title);

                        Assert.AreEqual(1, doc.Pages.Count);

                        var pg = doc.Pages[0] as Page;

                        Assert.IsNotNull(pg);
                        Assert.AreEqual(9, pg.Contents.Count); //2 divs and an image

                        //first div has a bound literal
                        var div = pg.FindAComponentById("div1") as Div;

                        Assert.IsNotNull(div);
                        Assert.AreEqual(3.75, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        var lit = div.Contents[0] as TextLiteral;

                        Assert.IsNotNull(lit);
                        Assert.AreEqual("Hello World & everyone in it.", lit.Text);

                        div = pg.FindAComponentById("div2") as Div;
                        Assert.IsNotNull(div);
                        Assert.AreEqual(5.0, div.Padding.Left);
                        Assert.AreEqual(1, div.Contents.Count);

                        lit = div.Contents[0] as TextLiteral;
                        Assert.AreEqual("<‘Inside’>", lit.Text);


                        var img = pg.FindAComponentById("img1") as Image;
                        Assert.AreEqual("./Images/group.png", img.Source);
                        Assert.AreEqual(5.0, img.Padding.Left);
                        Assert.AreEqual(100.0, img.Width);

                        var shared = doc.SharedResources;
                        Assert.AreEqual(2, shared.Count); //just the font

                        //Because we provide the relative path to the source,
                        //the document should be able to find and load the image

                        var imgRsrc = shared[0] as PDFImageXObject;
                        Assert.IsNotNull(imgRsrc);
                        Assert.AreEqual(imgfullSrc, imgRsrc.Source); //the source should be the full path to the local image file.

                        Assert.IsNotNull(imgRsrc.ImageData);
                        //Not successfull so 1x1 - unsucessfull image data proxy

                        Assert.AreEqual(GroupImgPixelWidth, imgRsrc.ImageData.PixelWidth);
                        Assert.AreEqual(GroupImgPixelHeight, imgRsrc.ImageData.PixelHeight);

                    }
                }
            }

        }

        #endregion


        #region public void ParseHtmlXmlReaderWithPath()

        /// 
        /// As the html is not valid xml there is no method and therefor no test for parsing an HTML document with an XML Reader
        /// 

        #endregion
    }
}
