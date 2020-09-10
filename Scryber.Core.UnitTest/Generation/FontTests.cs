using System;
using System.Linq;
using System.Text;
using Scryber.Generation;
using Scryber.Components;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass()]
    public class FontTests
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

        public FontTests()
        {
        }

        [TestMethod]
        public void DefaultFont_Test()
        {
            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' ?>
                                <pdf:Document xmlns:pdf='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' >
                                  <Pages>

                                    <pdf:Page id='titlepage' >
                                      <Content>
                                        <pdf:Span id='mylabel' >This is text in the default font</pdf:Span>
                                      </Content>
                                    </pdf:Page>

                                  </Pages>
                                </pdf:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            parsed.LayoutComplete += DefaultFont_LayoutComplete;
            using (var ms = new System.IO.MemoryStream())
                parsed.ProcessDocument(ms);

        }

        private void DefaultFont_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            //Default font is Sans-Serif
            var doc = args.Context.DocumentLayout.DocumentComponent;
            var rsrc = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Sans-Serif");

            Assert.IsNotNull(rsrc);
            
        }

        [TestMethod]
        public void StandardFonts_Test()
        {
            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' ?>
                                <pdf:Document xmlns:pdf='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              xmlns:styles='Scryber.Styles, Scryber.Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' >
                                  <Pages>

                                    <pdf:Page id='titlepage' >
                                      <Content>
                                        <pdf:Span id='mylabel' styles:font-family='Helvetica' >This is text in the Helvetica font</pdf:Span>
                                        <pdf:Span id='mylabel' styles:font-family='Times' >This is text in the Times font</pdf:Span>
                                        <pdf:Span id='mylabel' styles:font-family='Courier' >This is text in the Courier font</pdf:Span>
                                        <pdf:Span id='mylabel' styles:font-family='Zapf Dingbats' >This is text in the Dingbats font</pdf:Span>
                                        <pdf:Span id='mylabel' styles:font-family='Symbol' >This is text in the Symbol font</pdf:Span>
                                      </Content>
                                    </pdf:Page>

                                  </Pages>
                                </pdf:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            parsed.LayoutComplete += StandardFont_LayoutComplete;
            using (var ms = new System.IO.MemoryStream())
                parsed.ProcessDocument(ms);

        }

        private void StandardFont_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            //Default font is Sans-Serif
            var doc = args.Context.DocumentLayout.DocumentComponent;
            var hel = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Helvetica") as Scryber.Resources.PDFFontResource;
            var times = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Times") as Scryber.Resources.PDFFontResource;
            var cour = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Courier") as Scryber.Resources.PDFFontResource;
            var zapf = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Zapf Dingbats") as Scryber.Resources.PDFFontResource;
            var sym = doc.SharedResources.GetResource(Scryber.Resources.PDFResource.FontDefnResourceType, "Symbol") as Scryber.Resources.PDFFontResource;

            Assert.IsNotNull(hel, "Helvetica is null");
            Assert.IsNotNull(times, "Times is null");
            Assert.IsNotNull(cour, "Courier is null");
            Assert.IsNotNull(zapf, "Zapf is null");
            Assert.IsNotNull(sym, "Symbol is null");

          

        }

    }
}
