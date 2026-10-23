using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Generation;

namespace Scryber.Core.UnitTests.Html
{
    /// <summary>
    /// Both the scryber and paperwork processing instructions are accepted, in both the XHTML and the HTML5 parsers.
    /// </summary>
    [TestClass()]
    public class ProcessingInstructions_Tests
    {
        private const string Html5Body = "<!DOCTYPE html><html><body><p>Text</p></body></html>";
        private const string XhtmlBody = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p>Text</p></body></html>";

        private static Document ParseHtml5(string content)
        {
            using (var sr = new StringReader(content))
                return Document.ParseHtmlDocument(sr, ParseSourceType.DynamicContent);
        }

        private static Document ParseXhtml(string content)
        {
            using (var sr = new StringReader(content))
                return Document.ParseDocument(sr, ParseSourceType.DynamicContent);
        }

        [TestMethod()]
        public void Html5_ScryberInstruction_IsApplied()
        {
            using (var doc = ParseHtml5("<?scryber append-log='true' parser-mode='strict' ?>" + Html5Body))
            {
                Assert.IsTrue(doc.AppendTraceLog, "append-log should be applied");
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void Html5_PaperworkInstruction_IsApplied()
        {
            using (var doc = ParseHtml5("<?paperwork append-log='true' parser-mode='strict' ?>" + Html5Body))
            {
                Assert.IsTrue(doc.AppendTraceLog, "append-log should be applied");
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void Html5_NoInstruction_UsesDefaults()
        {
            using (var doc = ParseHtml5(Html5Body))
            {
                Assert.IsFalse(doc.AppendTraceLog);
                Assert.AreEqual(ParserConformanceMode.Lax, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void Xhtml_ScryberAndPaperworkInstructions_AreApplied()
        {
            using (var doc = ParseXhtml("<?scryber append-log='true' parser-mode='strict' ?>" + XhtmlBody))
            {
                Assert.IsTrue(doc.AppendTraceLog);
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }

            using (var doc = ParseXhtml("<?paperwork append-log='true' parser-mode='strict' ?>" + XhtmlBody))
            {
                Assert.IsTrue(doc.AppendTraceLog);
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }
    }

    /// <summary>
    /// ParseDocument overloads that take explicit ParserSettings.
    /// </summary>
    [TestClass()]
    public class ParseDocumentWithSettings_Tests
    {
        private const string Html5 = "<!DOCTYPE html><html><body><p>Text</p></body></html>";
        private const string Xhtml = "<html xmlns='http://www.w3.org/1999/xhtml'><body><p>Text</p></body></html>";

        private static ParserSettings Settings(ParserConformanceMode mode)
        {
            return Document.CreateParserSettings("", mode);
        }

        [TestMethod()]
        public void Xhtml_TextReader_UsesTheConformanceFromTheSettings()
        {
            foreach (var mode in new[] { ParserConformanceMode.Strict, ParserConformanceMode.Lax })
            {
                using (var sr = new StringReader(Xhtml))
                using (var doc = Document.ParseDocument(sr, "", ParseSourceType.DynamicContent, Settings(mode)))
                {
                    Assert.AreEqual(mode, doc.ConformanceMode);
                }
            }
        }

        [TestMethod()]
        public void Xhtml_Stream_UsesTheConformanceFromTheSettings()
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(Xhtml)))
            using (var doc = Document.ParseDocument(ms, "", ParseSourceType.DynamicContent, Settings(ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void Html5_TextReaderAndStream_UseTheConformanceFromTheSettings()
        {
            using (var sr = new StringReader(Html5))
            using (var doc = Document.ParseHtmlDocument(sr, "", ParseSourceType.DynamicContent, Settings(ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(Html5)))
            using (var doc = Document.ParseHtmlDocument(ms, "", ParseSourceType.DynamicContent, Settings(ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public async Task Async_Xhtml_And_Html5_UseTheConformanceFromTheSettings()
        {
            using (var sr = new StringReader(Xhtml))
            using (var doc = await Document.ParseDocumentAsync(sr, "", ParseSourceType.DynamicContent, Document.CreateParserSettingsAsync("", ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }

            using (var sr = new StringReader(Html5))
            using (var doc = await Document.ParseHtmlDocumentAsync(sr, "", ParseSourceType.DynamicContent, Document.CreateParserSettingsAsync("", ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Strict, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void ProcessingInstruction_StillOverridesTheSettings()
        {
            using (var sr = new StringReader("<?paperwork parser-mode='lax' ?>" + Xhtml))
            using (var doc = Document.ParseDocument(sr, "", ParseSourceType.DynamicContent, Settings(ParserConformanceMode.Strict)))
            {
                Assert.AreEqual(ParserConformanceMode.Lax, doc.ConformanceMode);
            }
        }

        [TestMethod()]
        public void NullSettings_Throws()
        {
            using (var sr = new StringReader(Xhtml))
                Assert.ThrowsException<ArgumentNullException>(() => Document.ParseDocument(sr, "", ParseSourceType.DynamicContent, (ParserSettings)null));
        }
    }
}
