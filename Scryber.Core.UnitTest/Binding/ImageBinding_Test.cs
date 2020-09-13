using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

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
        public void ImageParamerterBinding()
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

                var path = this.TestContext.TestDir;

#if MAC_OS
                // back up from obj/Debug/TestDirectoryName
                path = System.IO.Path.Combine(path, "../../../Content/Toroid24.jpg");
#else
                path = System.IO.Path.Combine(path, "../../Scryber.Core.UnitTest/Content/Toroid24.jpg"); ;
#endif

                path = System.IO.Path.GetFullPath(path);

                var data = Scryber.Drawing.PDFImageData.LoadImageFromLocalFile(doc, doc, path);
                doc.Params["MyImage"] = data;

                var img = doc.FindAComponentById("LoadedImage") as Image;
                Assert.IsNull(img.Data);

                doc.InitializeAndLoad();
                doc.DataBind();

                
                Assert.IsNotNull(img.Data);
                Assert.AreEqual(data, img.Data);

            }
        }
    }
}
