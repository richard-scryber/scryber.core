using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber.Drawing;
using Scryber.Native;
using Scryber.Styles.Selectors;
using Scryber.Styles;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass()]
    public class PDFStyleDefnParsingTests
    {

        #region public TestContext TestContext

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

        #endregion 

        [TestMethod]
        public void SimpleIsMatch_Test()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                      xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                      xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
                          <Params>
                            <doc:Object-Param id='Model' ></doc:Object-Param>
                          </Params>

                          <Styles>
                            <!-- Bind the head and body styles to the Theme -->
                            <styles:Style match='.head'>
                              <styles:Padding all='20pt'/>
                              <styles:Background color='#323232' />
                              <styles:Fill color='#00a8a1'/>
                              <styles:Font family='Segoe UI Light' bold='false' italic='false' />
                            </styles:Style>

                            <styles:Style match='.body'>
                              <styles:Font family='Segoe UI' size='12pt' />
                              <styles:Fill color='#323232'/>
                              <styles:Padding all='20pt'/>
                            </styles:Style>

                            <styles:Style match='.body doc:Para'>
                              <styles:Font family='Segoe UI' size='12pt' />
                              <styles:Fill color='#323232'/>
                              <styles:Padding all='20pt'/>
                            </styles:Style>

                          </Styles>

                          <Pages>

                            <doc:Section>
                              <Content>
                                <!-- Specify the class names on the components to use the styles -->
                                <doc:H1 styles:class='head' text='This is the title' ></doc:H1>
                                <doc:Div styles:class='body' >
                                  Content
                                  <doc:Para>Another paragraph</doc:Para>
                                  <doc:Para>This is the content in a para</doc:Para>
                                </doc:Div>
                              </Content>
                            </doc:Section>

                          </Pages>
                        </doc:Document>";

            bool parsed = false;

            using (var sr = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
                parsed = true;


                Assert.IsTrue(parsed);
                Assert.AreEqual(3, doc.Styles.Count);
                Assert.AreEqual(".head", (doc.Styles[0] as StyleDefn).Match.ToString());
                Assert.AreEqual(".body", (doc.Styles[1] as StyleDefn).Match.ToString());
                Assert.AreEqual(".body doc:Para", (doc.Styles[2] as StyleDefn).Match.ToString());
            }

           
        }
    }
}
