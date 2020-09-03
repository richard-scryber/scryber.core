using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Data;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass()]
    public class TemplateParam_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }

        [TestMethod()]
        [TestCategory("Components")]
        public void PlaceholderTemplate()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd' >

  <Params>
    <!-- template parameter with -->
    <doc:Template-Param id='datatable' >
      <doc:Div styles:class='bordered'>No data is available for the table</doc:Div>
    </doc:Template-Param>
  </Params>
  
  <Styles>
    <styles:Style applied-class='bordered' >
      <styles:Border color='#777' width='1pt' style='Solid'/>
      <styles:Background color='#EEE'/>
      <styles:Padding all='4pt'/>
    </styles:Style>
  </Styles>
  <Pages>
   
    <doc:Page styles:margins='20pt' styles:font-size='18pt'>
      <Content>
        <doc:Div styles:class='bordered' >
          The content of this div is all as a block (by default)
        </doc:Div>
        
        <!-- bind to a template parameter with a default value -->
        <doc:PlaceHolder id='DynamicContent' template='{@:datatable}' />
        
        <doc:Div styles:class='bordered' styles:width='300pt' >
          This is after the placeholder.
        </doc:Div>
       
      </Content>
    </doc:Page>
  </Pages>

</doc:Document>";

            var template = GetTemplateData();

            using (System.IO.StringReader sr = new System.IO.StringReader(src))
            {
                PDFDocument doc = PDFDocument.ParseDocument(sr, ParseSourceType.DynamicContent);
                doc.Params["datatable"] = template;


                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    doc.ProcessDocument(ms);
                    var placeholder = doc.FindAComponentById("DynamicContent") as PDFPlaceHolder;

                    //placeholder should contain a template instance, that contains the table.

                    Assert.IsTrue(placeholder.Contents.Count > 0);
                    Assert.IsInstanceOfType(placeholder.Contents[0], typeof(PDFTemplateInstance));

                    var instance = placeholder.Contents[0] as PDFTemplateInstance;
                    Assert.IsTrue(instance.HasContent);
                    Assert.IsInstanceOfType(instance.Content[0], typeof(PDFTableGrid));
                }
            }
        }


        private string GetTemplateData()
        {
            //This content could be from any source, and could use XElements, XMLNodes or any other source.
            //And really we should be using a string builder anyway.

            var str = "<doc:Table styles:full-width='true' >";
            for (var i = 0; i < 5; i++)
            {
                str += "<doc:Row><doc:Cell >" + i + "</doc:Cell><doc:Cell >" + (i + 5) + "</doc:Cell></doc:Row>";
            }
            str += "</doc:Table>";

            return str;

        }

    }
}
