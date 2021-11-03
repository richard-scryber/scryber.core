using System;
using System.Linq;
using System.Text;
using Scryber.Generation;
using Scryber.Components;
using Scryber.PDF.Resources;
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
                                <doc:Document xmlns:doc='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' >
                                  <Pages>

                                    <doc:Page id='titlepage' >
                                      <Content>
                                        <doc:Span id='mylabel' >This is text in the default font</doc:Span>
                                      </Content>
                                    </doc:Page>

                                  </Pages>
                                </doc:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            parsed.LayoutComplete += DefaultFont_LayoutComplete;
            using (var ms = DocStreams.GetOutputStream("DefaultFont.pdf"))
                parsed.SaveAsPDF(ms);

        }

        private void DefaultFont_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Default font is Sans-Serif
            var context = (PDF.PDFLayoutContext)(args.Context);
            var doc = context.DocumentLayout.DocumentComponent;
            var rsrc = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Sans-Serif");

            Assert.IsNotNull(rsrc);
            
        }

        [TestMethod]
        public void StandardFonts_Test()
        {
            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' ?>
                                <doc:Document xmlns:doc='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              xmlns:styles='Scryber.Styles, Scryber.Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' >
                                  <Pages>

                                    <doc:Page id='titlepage' styles:padding='10' >
                                      <Content>
                                        <doc:Span id='mylabel' styles:font-family='Helvetica' >This is text in the Helvetica font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:font-family='Times' >This is text in the Times font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:font-family='Courier' >This is text in the Courier font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:font-family='Zapf Dingbats' >This is text in the Dingbats font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:font-family='Symbol' >This is text in the Symbol font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:font-family='Times' styles:font-bold='true' >This is text in the Times font that is bold</doc:Span><doc:Br/>
                                      </Content>
                                    </doc:Page>

                                  </Pages>
                                </doc:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            using (var ms = DocStreams.GetOutputStream("StandardFont.pdf"))
                parsed.SaveAsPDF(ms);

            var hel = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Helvetica") as PDFFontResource;
            var times = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Times") as PDFFontResource;
            var cour = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Courier") as PDFFontResource;
            var zapf = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Zapf Dingbats") as PDFFontResource;
            var sym = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Symbol") as PDFFontResource;
            var timesB = parsed.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Times,Bold") as PDFFontResource;

            Assert.IsNotNull(hel, "Helvetica is null");
            Assert.IsNotNull(times, "Times is null");
            Assert.IsNotNull(cour, "Courier is null");
            Assert.IsNotNull(zapf, "Zapf is null");
            Assert.IsNotNull(sym, "Symbol is null");
            Assert.IsNotNull(timesB, "Times Bold is null");
        }

        


        [TestMethod()]
        public void ValidateSelectorUse()
        {
            string sansFontFamily = "\"Does not exist\", Futura, Arial, sans-serif";
            string serifFontFamily = "\"ITC Clearface\", Romana, serif ";

            string documentxml = @"<?xml version='1.0' encoding='utf-8' ?>
                                <?scryber parser-mode='Strict' parser-log='false' append-log='false' log-level='Warnings' ?>
                                <doc:Document xmlns:doc='Scryber.Components, Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              xmlns:styles='Scryber.Styles, Scryber.Styles, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                                              id='outerdoc' >
                                  <Styles>
                                    <styles:Style match='doc:Span.sans' >
                                        <styles:Font family='"+ sansFontFamily + @"'/>
                                    </styles:Style>
                                    <styles:Style match='doc:Span.serif' >
                                        <styles:Font family='" + serifFontFamily + @"'/>
                                    </styles:Style>
                                  </Styles>
                                  <Pages>

                                    <doc:Page id='titlepage' >
                                      <Content>
                                        <doc:Span id='mylabel' styles:class='sans' >This is text in the Sans family font
that will flow across multiple lines and show the expected default leading for the font</doc:Span><doc:Br/>
                                        <doc:Span id='mylabel' styles:class='serif' >This is text in the Times font
that will flow across multiple lines and show the expected default leading for the font</doc:Span><doc:Br/>
                                      </Content>
                                    </doc:Page>

                                  </Pages>
                                </doc:Document>";

            Document parsed;
            using (System.IO.StringReader sr = new System.IO.StringReader(documentxml))
            {
                parsed = Document.ParseDocument(sr, ParseSourceType.DynamicContent);
            }

            parsed.LayoutComplete += SelectorFont_LayoutComplete;
            using (var ms = DocStreams.GetOutputStream("FontSelectorChoice.pdf"))
                parsed.SaveAsPDF(ms);

        }

        private void SelectorFont_LayoutComplete(object sender, LayoutEventArgs args)
        {
            //Default font is Sans-Serif
            var context = (PDF.PDFLayoutContext)(args.Context);
            var doc = context.DocumentLayout.DocumentComponent;
            
            var sans = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, "Arial") as PDFFontResource;
            
            if (null == sans) //Arial might not be present and if not then should fall back to the sans-serif
                sans = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, "sans-serif") as PDFFontResource;

            var serif = doc.SharedResources.GetResource(PDFResource.FontDefnResourceType, "serif") as PDFFontResource;

            Assert.IsNotNull(sans, "Sans-Serif is null");
            Assert.IsNotNull(serif, "Serif is null");
            

            


        }
    }
}
