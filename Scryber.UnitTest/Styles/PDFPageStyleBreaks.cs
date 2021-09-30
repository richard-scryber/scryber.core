using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Styles
{
    [TestClass]
    public class PDFPageStyleBreaks
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

        [TestMethod]
        public void TestSimpleStylePageBreak()
        {
            var src = @"<?xml version='1.0' encoding='UTF-8' ?>
            <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                          xmlns:style='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'>
              <Pages>
                <doc:Section style:padding='20pt' style:fill-color='#880000' >
                  <Content>
                    <doc:Div id='pg1' style:fill-color='#008800'>
                      This is some content
                    </doc:Div>
                    <doc:Div id='pg2' style:fill-color='#000088'
                             style:page-break-before='true' 
                             style:page-break-after='true' >
                      This should come on the next page
                    </doc:Div>
                    <doc:Div id='pg3'>
                        This should come on the last page
                    </doc:Div>
                  </Content>
                </doc:Section>
              </Pages>
            </doc:Document>";

            using(var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);
                using (var stream = DocStreams.GetOutputStream("PageBreaksInline.pdf"))
                    doc.SaveAsPDF(stream);

                var arrange = doc.Pages[0].GetFirstArrangement() as ComponentMultiArrangement;
                Assert.IsNotNull(arrange, "The page arrangement should be a multi page arrangement");
                Assert.AreEqual(0, arrange.PageIndex, "First arrangement should be on page 0");
                
                arrange = arrange.NextArrangement;
                Assert.IsNotNull(arrange, "Should be a second arrangement on the page");
                Assert.AreEqual(1, arrange.PageIndex, "Second arrangement should be page 1");

                arrange = arrange.NextArrangement;
                Assert.IsNotNull(arrange, "Should be a third arrangement on the page");
                Assert.AreEqual(2, arrange.PageIndex, "Third arrangement should be page 2");

                var pg1 = doc.FindAComponentById("pg1");
                var pg2 = doc.FindAComponentById("pg2");
                var pg3 = doc.FindAComponentById("pg3");

                var divArrange = pg1.GetFirstArrangement();
                Assert.AreEqual(0, divArrange.PageIndex, "First arrangement should be on page 0");

                divArrange = pg2.GetFirstArrangement();
                Assert.AreEqual(1, divArrange.PageIndex, "Second arrangement should be on page 1");

                divArrange = pg3.GetFirstArrangement();
                Assert.AreEqual(2, divArrange.PageIndex, "Third arrangement should be on page 2");

            }
        }



        [TestMethod]
        public void TestStyleClassPageBreak()
        {
            var src = @"<?xml version='1.0' encoding='UTF-8' ?>
            <doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                          xmlns:style='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'>
              <Styles>
                <style:Style match='.break-before' >
                    <style:Page break-before='true' />
                    <style:Fill color='#000088' />
                </style:Style>
              </Styles>
              <Pages>
                <doc:Section style:padding='20pt' style:fill-color='#880000' >
                  <Content>
                    <doc:Div id='div1' style:fill-color='#008800'>
                      This is some content
                    </doc:Div>
                    <doc:Div id='div2' style:class='break-before' >
                      This should come on the next page
                    </doc:Div>
                    <doc:Div id='div3' style:class='break-before'>
                        This should come on the last page
                    </doc:Div>
                    <doc:Div id='div4' style:class='break-before'
                                      style:page-break-before='false'>
                        This should come on the same last page as overridden
                    </doc:Div>
                  </Content>
                </doc:Section>
              </Pages>
            </doc:Document>";

            using (var ms = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(ms, ParseSourceType.DynamicContent);

                using (var stream = DocStreams.GetOutputStream("PageBreaksCSS.pdf"))
                    doc.SaveAsPDF(stream);

                var arrange = doc.Pages[0].GetFirstArrangement() as ComponentMultiArrangement;
                Assert.IsNotNull(arrange, "The page arrangement should be a multi page arrangement");
                Assert.AreEqual(0, arrange.PageIndex, "First arrangement should be on page 0");

                arrange = arrange.NextArrangement;
                Assert.IsNotNull(arrange, "Should be a second arrangement on the page");
                Assert.AreEqual(1, arrange.PageIndex, "Second arrangement should be page 1");

                arrange = arrange.NextArrangement;
                Assert.IsNotNull(arrange, "Should be a third arrangement on the page");
                Assert.AreEqual(2, arrange.PageIndex, "Third arrangement should be page 2");

                Assert.IsNull(arrange.NextArrangement, "There should not be a fourth arrangement");
                
                var div1 = doc.FindAComponentById("div1");
                var div2 = doc.FindAComponentById("div2");
                var div3 = doc.FindAComponentById("div3"); 
                var div4 = doc.FindAComponentById("div4");

                var divArrange = div1.GetFirstArrangement();
                Assert.AreEqual(0, divArrange.PageIndex, "First arrangement should be on page 0");

                divArrange = div2.GetFirstArrangement();
                Assert.AreEqual(1, divArrange.PageIndex, "Second arrangement should be on page 1");

                divArrange = div3.GetFirstArrangement();
                Assert.AreEqual(2, divArrange.PageIndex, "Third arrangement should be on page 2");

                divArrange = div4.GetFirstArrangement();
                Assert.AreEqual(2, divArrange.PageIndex, "Fourth arrangement should be on page 2, as overridden");

            }
        }
    }
}
