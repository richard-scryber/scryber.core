using Scryber.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using Scryber.Components;
using Scryber.Logging;

namespace Scryber.Core.UnitTests.Generation
{
    
    
    /// <summary>
    ///This is a test class for PDFGeneratorSettingsTest and is intended
    ///to contain all PDFGeneratorSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFGeneratorSettingsTest
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PDFGeneratorSettings Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void PDFGeneratorSettingsConstructorTest()
        {
            Type literaltype = typeof(Scryber.Components.TextLiteral);
            Type templategenerator = typeof(Scryber.Data.ParsableTemplateGenerator);
            Type templateinstance = typeof(Scryber.Data.TemplateInstance);
            PDFReferenceResolver resolver = new PDFReferenceResolver(this.ShimResolver);
            ParserConformanceMode conformance = ParserConformanceMode.Lax;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            PDFTraceLog log = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            PerformanceMonitor mon = new PerformanceMonitor(true);
            Mocks.MockControllerClass controller = new Mocks.MockControllerClass();

            ParserSettings target = new ParserSettings(literaltype, templategenerator, templateinstance, resolver, conformance, loadtype, log, mon, controller);

            Assert.IsNotNull(target);
            Assert.AreSame(literaltype, target.TextLiteralType);
            Assert.AreSame(templategenerator, target.TempateGeneratorType);
            Assert.AreSame(resolver, target.Resolver);
            Assert.AreEqual(conformance, target.ConformanceMode);
            Assert.AreEqual(loadtype, target.LoadType);
            Assert.AreSame(log, target.TraceLog);
            Assert.AreSame(mon, target.PerformanceMonitor);
            Assert.AreSame(controller, target.Controller);
            Assert.AreEqual(controller.GetType(), target.ControllerType);
        }

        IComponent ShimResolver(string filename, string xpath, ParserSettings settings)
        {
            return null;
        }

        /// <summary>
        ///A test for ConformanceMode - read / write
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void ConformanceModeTest()
        {
            Type literaltype = typeof(Scryber.Components.TextLiteral);
            Type templategenerator = typeof(Scryber.Data.ParsableTemplateGenerator);
            Type templateinstance = typeof(Scryber.Data.TemplateInstance);
            PDFReferenceResolver resolver = new PDFReferenceResolver(this.ShimResolver);
            ParserConformanceMode conformance = ParserConformanceMode.Lax;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            PDFTraceLog log = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            PerformanceMonitor mon = new PerformanceMonitor(true);
            ParserSettings target = new ParserSettings(literaltype, templategenerator, templateinstance, resolver, conformance, loadtype, log, mon, null);

            Assert.AreEqual(conformance, target.ConformanceMode);

            ParserConformanceMode expected = ParserConformanceMode.Strict;
            ParserConformanceMode actual;
            target.ConformanceMode = expected;
            actual = target.ConformanceMode;
            Assert.AreEqual(expected, actual);
            

        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void ProcessingInstructionTest()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
                        <?scryber append-log='true' log-level='Messages' ?>
                        <doc:Document xmlns:doc = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
                                    xmlns:styles = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
                                    xmlns:data = 'http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd'
                                     >

                        <Pages>
    
                        <doc:Section>
                            <Content>
                                <data:ForEach id='Foreach2' value='{@:xml}' select='//node/inner' template='{@:template}' ></data:ForEach>
                            </Content>
                        </doc:Section>

                        </Pages>
                    </doc:Document>";

            using (var reader = new System.IO.StringReader(src))
            {
                var doc = Document.ParseDocument(reader, ParseSourceType.DynamicContent);

                Assert.AreEqual(true, doc.AppendTraceLog, "The append log is not set to true");
                Assert.AreEqual(doc.TraceLog.RecordLevel, TraceRecordLevel.Messages, "The trace log is not set to Messages");
            }
        }

        
    }
}
