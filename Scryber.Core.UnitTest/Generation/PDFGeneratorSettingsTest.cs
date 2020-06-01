using Scryber.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;

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
            Type literaltype = typeof(Scryber.Components.PDFTextLiteral);
            Type templategenerator = typeof(Scryber.Data.PDFParsableTemplateGenerator);
            Type templateinstance = typeof(Scryber.Data.PDFTemplateInstance);
            PDFReferenceResolver resolver = new PDFReferenceResolver(this.ShimResolver);
            ParserConformanceMode conformance = ParserConformanceMode.Lax;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            PDFTraceLog log = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            PDFPerformanceMonitor mon = new PDFPerformanceMonitor(true);
            PDFGeneratorSettings target = new PDFGeneratorSettings(literaltype, templategenerator, templateinstance, resolver, conformance, loadtype, log, mon);

            Assert.IsNotNull(target);
            Assert.AreSame(literaltype, target.TextLiteralType);
            Assert.AreSame(templategenerator, target.TempateGeneratorType);
            Assert.AreSame(resolver, target.Resolver);
            Assert.AreEqual(conformance, target.ConformanceMode);
            Assert.AreEqual(loadtype, target.LoadType);
            Assert.AreSame(log, target.TraceLog);
            Assert.AreSame(mon, target.PerformanceMonitor);
        }

        IPDFComponent ShimResolver(string filename, string xpath, PDFGeneratorSettings settings)
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
            Type literaltype = typeof(Scryber.Components.PDFTextLiteral);
            Type templategenerator = typeof(Scryber.Data.PDFParsableTemplateGenerator);
            Type templateinstance = typeof(Scryber.Data.PDFTemplateInstance);
            PDFReferenceResolver resolver = new PDFReferenceResolver(this.ShimResolver);
            ParserConformanceMode conformance = ParserConformanceMode.Lax;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            PDFTraceLog log = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            PDFPerformanceMonitor mon = new PDFPerformanceMonitor(true);
            PDFGeneratorSettings target = new PDFGeneratorSettings(literaltype, templategenerator, templateinstance, resolver, conformance, loadtype, log, mon);

            Assert.AreEqual(conformance, target.ConformanceMode);

            ParserConformanceMode expected = ParserConformanceMode.Strict;
            ParserConformanceMode actual;
            target.ConformanceMode = expected;
            actual = target.ConformanceMode;
            Assert.AreEqual(expected, actual);
            

        }

        
    }
}
