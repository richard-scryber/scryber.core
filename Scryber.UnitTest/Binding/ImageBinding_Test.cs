using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Drawing;
using Scryber.PDF;
using Scryber.PDF.Layout;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.Binding
{
    [TestClass()]
    public class ImageBinding_Test
    {

        private TestContext testContextInstance;
        private PDFLayoutDocument _layout;
        
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

                var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/Toroid24.jpg", this.TestContext);

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
            
            var path = DocStreams.AssertGetDirectoryPath("../../Scryber.UnitTest/Content/HTML/Images/", this.TestContext);



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
            
            //Check the actual layout
            Assert.IsNotNull(_layout, "The layout was not captured");
            
            var layoutPg = _layout.AllPages[0];
            var layoutLine1 = layoutPg.ContentBlock.Columns[0].Contents[0] as Scryber.PDF.Layout.PDFLayoutLine;

            Assert.IsNotNull(layoutLine1);
            Assert.AreEqual(1, layoutLine1.Runs.Count);

            var compRun1 = layoutLine1.Runs[0] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            Assert.IsNotNull(compRun1);

            Assert.IsNotNull(compRun1.Owner);
            Assert.AreEqual("LoadedImage1", compRun1.Owner.ID);

            layoutPg = _layout.AllPages[1];
            var layoutLine2 = layoutPg.ContentBlock.Columns[0].Contents[0] as Scryber.PDF.Layout.PDFLayoutLine;

            Assert.IsNotNull(layoutLine2);
            Assert.AreEqual(1, layoutLine2.Runs.Count);

            var compRun2= layoutLine2.Runs[0] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            Assert.IsNotNull(compRun2);

            Assert.IsNotNull(compRun2.Owner);
            Assert.AreEqual("LoadedImage2", compRun2.Owner.ID);
        }

        /// <summary>
        /// Checks the layout to ensure it has a Component layout and the images are registered with the layout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);
            this._layout = context.DocumentLayout;

        }

        [TestMethod]
        public void LargeImageBoundWithFixedContainer()
        {
            var imgReader = Scryber.Imaging.ImageReader.Create();


            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/HTML/Images/LongRedBar.png", this.TestContext); ;

            path = System.IO.Path.GetFullPath(path);

            
            ImageData data;

            using (var fs = new System.IO.FileStream(path, FileMode.Open))
            {
                data = imgReader.ReadStream(path, fs, false);
            }

            var model = new
            {
                ClientHasLogo = true,
                ClientLogo = data
            };

            var html = @"<?xml version='1.0' encoding='utf-8' ?>
<html xmlns='http://www.w3.org/1999/xhtml' >
    <head>
        <style>
            .logo-container {
               background: #dddddd;
               width: 35mm;
               height: 21mm;
               vertical-align: middle;
               text-align: center;
               overflow: hidden;
               padding:0px;
            }

        </style>
    </head>
    <body style='padding: 20pt'>
        <table>
           <tr>
               <td class=""logo-container"">
                   <if data-test=""{@:Model.ClientHasLogo}"">
                       <img img-data=""{@:Model.ClientLogo}"" />
                   </if>
               </td>
               <td></td>
           </tr>
        </table>
    </body>
</html>";

            Document doc;

            using (var reader = new System.IO.StringReader(html))
            {
                doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
            }

            doc.Params["Model"] = model;
            

            using (var stream = DocStreams.GetOutputStream("OverflowImageOutput.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(stream);
            }

            Assert.IsNotNull(_layout);
            Assert.AreEqual(1, _layout.AllPages.Count);
            var pg = _layout.AllPages[0];

            var content = pg.ContentBlock;
            Assert.AreEqual(1, content.Columns.Length);
            Assert.AreEqual(1, content.Columns[0].Contents.Count);
            
            var tbl = content.Columns[0].Contents[0] as PDFLayoutBlock; //
            
            Assert.IsNotNull(tbl);
            Assert.AreEqual(1, tbl.Columns.Length);
            Assert.AreEqual(1, tbl.Columns[0].Contents.Count);
            
            var row = tbl.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(row);

            Assert.AreEqual(2, row.Columns.Length);
            Assert.AreEqual(1, row.Columns[0].Contents.Count);
            Assert.AreEqual(1, row.Columns[1].Contents.Count);

            //first cell has the image if and image - but should maintain the explicit height.
            var cell = row.Columns[0].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(cell);
            Assert.AreEqual(new Unit(35, PageUnits.Millimeters), cell.Width);
            Assert.AreEqual(new Unit(21, PageUnits.Millimeters), cell.Height);
            Assert.AreEqual(1, cell.Columns.Length);
            Assert.AreEqual(1, cell.Columns[0].Contents.Count);
            
            //second empty cell - still the same height
            cell = row.Columns[1].Contents[0] as PDFLayoutBlock;
            Assert.IsNotNull(cell);
            Assert.AreEqual(new Unit(21, PageUnits.Millimeters), cell.Height);
        }


    }
}
