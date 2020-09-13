using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{



    [TestClass()]
    public class ControllerBinding_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }


        public ControllerBinding_Test()
        {
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindController()
        {

            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
<?scryber controller='" + controllerType + @"' ?>

<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
              id='MyDocument'
              on-init='DocumentInitialized' on-loaded='DocumentLoaded'
              on-databinding='DocumentBinding' on-databound='DocumentDataBound' 
              on-prelayout='DocumentPreLayout' on-postlayout='DocumentPostLayout'
              on-prerender='DocumentPreRender' on-postrender='DocumentPostRender' >
  <Pages>

    <doc:Page styles:margins='20pt'>
      <Content>
        
          <!-- This will automatically be set on the controller instance property -->
          <doc:H1 id='Title' on-databinding='HeaderBinding' > </doc:H1>
          
          <doc:Ul>
            <!-- now we call the BindForEach method to set the data value -->
            <data:ForEach on-databinding='ForEachBinding' on-item-databound='ForEachItemBinding' >
              <Template>
                <!-- and finally we use the item data bound to set the
                     content of the list item for each entry -->
                <doc:Li on-databound='ForEachListItemBound'></doc:Li>
              </Template>
            </data:ForEach>
          </doc:Ul>

      </Content>
    </doc:Page>
  </Pages>

</doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var controller = doc.Controller as Mocks.MockControllerClass;
                Assert.IsNotNull(controller);

                var mockresults = controller.Results;
                Assert.IsNotNull(controller.Title);
                

                using (var stream = new System.IO.MemoryStream())
                {
                    doc.SaveAsPDF(stream);
                }

                Assert.IsTrue(mockresults.Count > 0, "No results were recorded");
                Assert.IsNotNull(doc.Controller, "The controller is not set on the document");
                Assert.AreEqual(doc.Controller.GetType(), Type.GetType(controllerType), "The controller types do not match");

                //We have a controler instance, and executed the plan
                //Check that the events recorded in the mock controller are happening (in the right order)

                //Conotrller Init
                Assert.AreEqual("Controller Initialized", mockresults[0], "Controller Init did not happen");

                //Document init and load
                Assert.AreEqual("Controller Document Initialized", mockresults[1], "Document Init did not happen");
                Assert.AreEqual("Controller Document Loaded", mockresults[2], "Document Load did not happen");

                //Document DataBinding
                Assert.AreEqual("Controller Document DataBinding", mockresults[3], "Document DataBinding did not happen");
                //Header and ForEach
                Assert.AreEqual("Controller Header Databound", mockresults[4], "Header DataBound did not happen");
                Assert.AreEqual("Controller ForEach Binding", mockresults[5], "Foreach Databinfing did not happen");
                //All the labels
                Assert.AreEqual("Controller ForEach Label 0 Databound", mockresults[6], "ForEach Label 0 did not happen");
                Assert.AreEqual("Controller ForEach Item Bound 0", mockresults[7], "ForEach Item 0 did not happen");

                Assert.AreEqual("Controller ForEach Label 1 Databound", mockresults[8], "ForEach Label 1 did not happen");
                Assert.AreEqual("Controller ForEach Item Bound 1", mockresults[9], "ForEach Item 1 did not happen");

                Assert.AreEqual("Controller ForEach Label 2 Databound", mockresults[10], "ForEach Label 2 did not happen");
                Assert.AreEqual("Controller ForEach Item Bound 2", mockresults[11], "ForEach Item 2 did not happen");

                //Finally document binding complete
                Assert.AreEqual("Controller Document Databound", mockresults[12], "Document DataBound did not happen");

                //Laying out
                Assert.AreEqual("Controller Document Laying out", mockresults[13], "Document Lay out did not happen");
                Assert.AreEqual("Controller Document Laid out", mockresults[14], "Document Laid out did not happen");

                //Rendering
                Assert.AreEqual("Controller Document Rendering", mockresults[15], "Document rendering did not happen");
                Assert.AreEqual("Controller Document Rendered", mockresults[16], "Document rendered did not happen");

                Assert.AreEqual(17, mockresults.Count);
                
            }


            //TODO: Required success and fail tests.
        }


        /// <summary>
        /// The Mock controller has a reqired PDFDocument reference.
        /// This checks to make sure the required is succssfull when it's there.
        /// </summary>
        [TestMethod()]
        public void ValidateRequiredItemsSuccess()
        {
            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <?scryber controller='" + controllerType + @"' ?>

                        <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                      id='MyDocument' >
                          <Pages>
                          </Pages>

                        </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                

                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                var controller = doc.Controller as Mocks.MockControllerClass;
                Assert.IsNotNull(controller);

                var mockresults = controller.Results;

                
                using (var stream = new System.IO.MemoryStream())
                {
                    doc.SaveAsPDF(stream);
                }
            }
        }

        /// <summary>
        /// The Mock controller has a reqired PDFDocument reference.
        /// This checks to make sure the required fails when it is NOT there.
        /// </summary>
        [TestMethod()]
        public void ValidateRequiredItemsFailed()
        {
            var controllerType = "Scryber.Core.UnitTests.Mocks.MockControllerClass, Scryber.UnitTests";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <?scryber controller='" + controllerType + @"' ?>

                        <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                      id='MyOtherDocument' >
                          <Pages>
                          </Pages>

                        </doc:Document>";

            bool caught = false;
            try
            {
                using (var reader = new System.IO.StringReader(src))
                {
                    var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);
                }
            }
            catch(PDFParserException)
            {
                caught = true;
            }

            Assert.IsTrue(caught, "The exception was not raised for the missing document outlet");

            
        }

    }
}
