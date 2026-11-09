using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.OpenType;
using Scryber.PDF.Resources;

namespace Scryber.Core.UnitTests.OpenType
{
    [TestClass()]
    [TestCategory("Font")]
    public class FontCodeRanges_Test
    {

        #region public TestContext TestContext {get;set;}

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
        public void Font_Without_UnicodeEntities()
        {
            var src = "<!DOCTYPE html>\n" +
                      "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n" +
                      "<head>\n " +
                      " <meta charset=\"utf-8\" />\n" +
                      "  <title>Scryber ToUnicode declared-count repro</title>\n" +
                      "  <style>\n" +
                      "    body { font-family: Arial; font-size: 11pt; }\n" +
                      "    .title { font-size: 14pt; }\n" +
                      "  </style>\n" +
                      "</head>\n" +
                      "<body>\n" +
                      "  <main>\n" +
                      "    <p class=\"title\">ToUnicode declared-count repro (single font on purpose)</p>\n" +
                      "    <p>Copy/paste any text from this PDF in Chrome: it pastes as garbage (raw glyph ids).\n" +
                      "       The ToUnicode CMap of this document declares one more bfrange entry than it\n" +
                      "       contains, and PDFium rejects the whole map when the declared count overruns.</p>\n" +
                      "    <p>The trigger is the no-break space entity at the end of this sentence. This\n" +
                      "       sentence follows it.</p>\n    <p>Digits for the copy test: 0123456789</p>\n" +
                      "    <p>ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz</p>\n" +
                      "  </main>\n" +
                      "</body>\n" +
                      "</html>";
            
            
            using var doc = Document.ParseDocument(new StringReader(src));
            
            using var output = DocStreams.GetOutputStream("FontCodeRanges_Without_UnicodeEntities.pdf");
            
            doc.SaveAsPDF(output);

            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);
            var def = font.Definition as PDFOpenTypeFontDefinition;
            Assert.IsNotNull(def);
            var width = font.Widths as PDFCompositeFontWidths;
            Assert.IsNotNull(width);
            var offsets = width.RegisterdGlyphOffsets;
            Assert.IsNotNull(offsets);
            Assert.AreEqual(70, offsets.Count);

            var chars = width.RegistedCharacters;
            Assert.IsNotNull(chars);
            Assert.AreEqual(70, chars.Count);
        }
        
        [TestMethod]
        public void Font_WITH_UnicodeEntities()
        {
            var src = "<!DOCTYPE html>\n" +
                      "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n" +
                      "<head>\n " +
                      " <meta charset=\"utf-8\" />\n" +
                      "  <title>Scryber ToUnicode declared-count repro</title>\n" +
                      "  <style>\n" +
                      "    body { font-family: Arial; font-size: 11pt; }\n" +
                      "    .title { font-size: 14pt; }\n" +
                      "  </style>\n" +
                      "</head>\n" +
                      "<body>\n" +
                      "  <main>\n" +
                      "    <p class=\"title\">ToUnicode declared-count repro (single font on purpose)</p>\n" +
                      "    <p>Copy/paste any text from this PDF in Chrome: it pastes as garbage (raw glyph ids).\n" +
                      "       The ToUnicode CMap of this document declares one more bfrange entry than it\n" +
                      "       contains, and PDFium rejects the whole map when the declared count overruns.</p>\n" +
                      "    <p>The trigger is the no-break space entity at the end of this sentence.&#160; This\n" +
                      "       sentence follows it.</p>\n    <p>Digits for the copy test: 0123456789</p>\n" +
                      "    <p>ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz</p>\n" +
                      "  </main>\n" +
                      "</body>\n" +
                      "</html>";
            
            using var doc = Document.ParseDocument(new StringReader(src));
            
            using var output = DocStreams.GetOutputStream("FontCodeRanges_WITH_UnicodeEntities.pdf");

            doc.SaveAsPDF(output);

            var font = doc.SharedResources[0] as PDFFontResource;
            Assert.IsNotNull(font);
            var def = font.Definition as PDFOpenTypeFontDefinition;
            Assert.IsNotNull(def);
            var width = font.Widths as PDFCompositeFontWidths;
            Assert.IsNotNull(width);
            
            //The &#160 is a registered character, but does not have a glyph.
            //So the count of glyphs is 70, and the count of chars is 71
            
            var offsets = width.RegisterdGlyphOffsets;
            Assert.IsNotNull(offsets);
            Assert.AreEqual(70, offsets.Count);

            var chars = width.RegistedCharacters;
            Assert.IsNotNull(chars);
            Assert.AreEqual(71, chars.Count);
            

        }
        

    }
}
