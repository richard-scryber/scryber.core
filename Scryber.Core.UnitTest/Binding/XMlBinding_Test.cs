using System;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Binding
{

    [TestClass()]
    public class XMlBinding_Test
    {

        public TestContext TextContext
        {
            get;
            set;
        }


        public XMlBinding_Test()
        {
        }


        [TestMethod()]
        [TestCategory("Binding")]
        public void BindXmlValue()
        {
            var xml = @"<DataSources title='Testing Document Datasources'>
        <Entries>
            <Entry Name='First' Id='FirstID' />
            <Entry Name='Second' Id='SecondID' />
        </Entries>
    </DataSources>";

            var src = @"<?xml version='1.0' encoding='utf-8' ?>
<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
  <Data>
    <data:XMLDataSource id='XmlSource' >
      <Data>" + xml + @"</Data>
    </data:XMLDataSource>
  </Data>
  <Pages>

    <!-- Use the models 'DocTitle' property for the outline. -->
    <doc:Page styles:margins='20pt'>
      <Content>
        <data:With datasource-id='XmlSource' select='//DataSources' >

          <!-- And use it as the text on the heading -->
          <doc:H1 id='Heading' styles:class='title' text='{xpath:@title}' > </doc:H1>
          
          <doc:Ul>
            <!-- now we loop through the 'Entries' property -->
            <data:ForEach value='{xpath:Entries/Entry}' >
              <Template>
                <doc:Li>
                  <!-- and create a list item for each entry (. prefix) with the name property. -->
                  <doc:Text id='{xpath:@Id}' value='{xpath:@Name}' />
                </doc:Li>
              </Template>
            </data:ForEach>
          </doc:Ul>
        </data:With>
        
      </Content>
    </doc:Page>
  </Pages>

</doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                doc.InitializeAndLoad();
                doc.DataBind();

                var head = doc.FindAComponentById("Heading") as Head1;
                Assert.AreEqual("Testing Document Datasources", head.Text, "The heading text values do not match");

                var text = doc.FindAComponentById("FirstID") as TextLiteral;
                Assert.IsNotNull(text, "Could not find the text literal that should have been bound");
                Assert.AreEqual("First", text.Text, "The bound value for the text literal does not match");

                text = doc.FindAComponentById("SecondID") as TextLiteral;
                Assert.IsNotNull(text, "Could not find the text literal that should have been bound");
                Assert.AreEqual("Second", text.Text, "The bound value for the text literal does not match");
            }
        }
    }
}
