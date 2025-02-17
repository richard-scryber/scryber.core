using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Binding
{
    [TestClass()]
    public class ImageBinding_Test
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


        public ImageBinding_Test()
        {
        }

        [TestMethod]
        public void ImagePathParamerterBinding()
        {

            var pdfx = @"<?xml version='1.0' encoding='utf-8' ?>
<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
  <Params>
    <doc:Object-Param id='MyImage' />
  </Params>
  <Pages>

    <doc:Page styles:margins='20pt'>
      <Content>
        <doc:Image id='LoadedImage' img-data='{@:MyImage}' />
        
      </Content>
    </doc:Page>
  </Pages>

</doc:Document>";

            using (var reader = new System.IO.StringReader(pdfx))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                var path = this.TestContext.TestRunDirectory;


                path = System.IO.Path.Combine(path, "../../Scryber.UnitTest/Content/HTML/Images/Toroid24.jpg"); ;

                path = System.IO.Path.GetFullPath(path);

                if (!System.IO.File.Exists(path))
                {
                    path = System.IO.Path.Combine(this.TestContext.TestDir, "../../Content/HTML/Images/Toroid24.jpg");
                    path = System.IO.Path.GetFullPath(path);
                    
                    if (!System.IO.File.Exists(path))
                        throw new System.IO.FileNotFoundException("Could not load the test image at path " + path);
                }

                var imgReader = Scryber.Imaging.ImageReader.Create();
                ImageData data;
                
                using (var fs = new System.IO.FileStream(path, FileMode.Open))
                {
                    data = imgReader.ReadStream(path, fs, false);
                }
                
                
                doc.Params["MyImage"] = data;

                var img = doc.FindAComponentById("LoadedImage") as Image;
                Assert.IsNotNull(img);
                Assert.IsNull(img.Data);

                doc.InitializeAndLoad(OutputFormat.PDF);
                doc.DataBind(OutputFormat.PDF);
                
                //Makes sure the image data is bound to the element
                Assert.IsNotNull(img.Data);
                Assert.AreEqual(data, img.Data);

            }
        }

        [TestMethod()]
        public void ImageDataParameterBinding()
        {
            var path = this.TestContext.TestRunDirectory;

            path = System.IO.Path.Combine(path, "../../Scryber.UnitTest/Content/HTML/Images"); ;

            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.Directory.Exists(path))
            {
                path = System.IO.Path.Combine(this.TestContext.TestDir, "../../Content/HTML/Images");
                path = System.IO.Path.GetFullPath(path);

                if (!System.IO.Directory.Exists(path))
                    throw new System.IO.FileNotFoundException("Could not load the test image directory at path " + path);
            }


            var imgReader = Scryber.Imaging.ImageReader.Create();
            ImageData data1, data2;

            if (!path.EndsWith(System.IO.Path.DirectorySeparatorChar))
                path += System.IO.Path.DirectorySeparatorChar;

            using (var fs = new System.IO.FileStream(path + "Toroid24.jpg", FileMode.Open))
            {
                data1 = imgReader.ReadStream(path + "Toroid24.jpg", fs, false);
            }
            
            using (var fs = new System.IO.FileStream(path + "group.png", FileMode.Open))
            {
                data2 = imgReader.ReadStream(path + "group.png", fs, false);
            }
            
            var model = new
            {
                img1 = data1,
                img2 = data2
            };


            var content = @"<?xml version='1.0' encoding='utf-8' ?>
<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
  <Params>
    <doc:Object-Param id='model' />
  </Params>
  <Pages>

    <doc:Section styles:margins='20pt'>
      <Content>
        
        <doc:Image id='LoadedImage1' img-data='{{model.img1}}' />
        <doc:PageBreak/>
        <doc:Image id='LoadedImage2' img-data='{{model.img2}}' />
      </Content>
    </doc:Section>

  </Pages>

</doc:Document>";

            Document doc;
            using (var reader = new System.IO.StringReader(content))
            {
                doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            }

            doc.Params["model"] = model;
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var stream = DocStreams.GetOutputStream("DataImageOutput.pdf"))
            {
                doc.SaveAsPDF(stream);
            }

            var found1 = doc.FindAComponentById("LoadedImage1") as Image;
            var found2 = doc.FindAComponentById("LoadedImage2") as Image;
            Assert.IsNotNull(found1);
            Assert.IsNotNull(found1.Data);
            Assert.AreEqual(data1, found1.Data);

            Assert.IsNotNull(found2);
            Assert.IsNotNull(found2.Data);
            Assert.AreEqual(data2, found2.Data);
            
            Assert.AreEqual(2, doc.SharedResources.Count, "Shared resource count was not 2, as expected");
            var rsrc1 = doc.SharedResources[0] as PDFImageXObject;
            var rsrc2 = doc.SharedResources[1] as PDFImageXObject;

            Assert.IsNotNull(rsrc1, "First resource image was null");
            Assert.IsTrue(rsrc1.Registered, "First image was not registered");
            Assert.AreEqual(data1.SourcePath, rsrc1.ImageData.SourcePath, "First Image, Paths did not match");
            Assert.IsTrue(rsrc1.ImageData.SourcePath.EndsWith("Toroid24.jpg"), "First image source path did not end with Toroid24.jpg");
            Assert.AreEqual(data1.PixelWidth, rsrc1.ImageData.PixelWidth, "First Image, Sizes did not match");
            
            Assert.IsNotNull(rsrc2, "Second resource image was null");
            Assert.IsTrue(rsrc2.Registered, "Second image was not registered");
            Assert.AreEqual(data2.SourcePath, rsrc2.ImageData.SourcePath, "Second Image, Paths did not match");
            Assert.IsTrue(rsrc2.ImageData.SourcePath.EndsWith("group.png"), "Second image source path did not end with group.png");
            Assert.AreEqual(data2.PixelWidth, rsrc2.ImageData.PixelWidth, "Second Image, Sizes did not match");
        }

        /// <summary>
        /// Checks the layout to ensure it has a Component layout and the images are registered with the layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);

            var layoutPg = context.DocumentLayout.AllPages[0];
            var layoutLine1 = layoutPg.ContentBlock.Columns[0].Contents[0] as Scryber.PDF.Layout.PDFLayoutLine;

            Assert.IsNotNull(layoutLine1);
            Assert.AreEqual(1, layoutLine1.Runs.Count);

            var compRun1 = layoutLine1.Runs[0] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);

            Assert.IsNotNull(compRun1.Owner);
            Assert.AreEqual("LoadedImage1", compRun1.Owner.ID);

            layoutPg = context.DocumentLayout.AllPages[1];
            var layoutLine2 = layoutPg.ContentBlock.Columns[0].Contents[0] as Scryber.PDF.Layout.PDFLayoutLine;

            Assert.IsNotNull(layoutLine2);
            Assert.AreEqual(1, layoutLine2.Runs.Count);

            var compRun2= layoutLine2.Runs[0] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            Assert.IsNotNull(compRun2);

            Assert.IsNotNull(compRun2.Owner);
            Assert.AreEqual("LoadedImage2", compRun2.Owner.ID);
            

        }
    }
}
