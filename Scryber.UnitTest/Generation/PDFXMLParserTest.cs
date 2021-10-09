using Scryber.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using Scryber;
using System.Xml;
using Scryber.Logging;

namespace Scryber.Core.UnitTests.Generation
{
    
    
    /// <summary>
    ///This is a test class for PDFXMLParserTest and is intended
    ///to contain all PDFXMLParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PDFXMLParserTest
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

        //
        // support methods
        //


        private ParserSettings GetSettings()
        {
            Type literaltype = typeof(Scryber.Components.TextLiteral);
            Type templategenerator = typeof(Scryber.Data.ParsableTemplateGenerator);
            Type templateinstance = typeof(Scryber.Data.TemplateInstance);
            PDFReferenceResolver resolver = new PDFReferenceResolver(this.ShimResolver);
            ParserConformanceMode conformance = ParserConformanceMode.Lax;
            ParserLoadType loadtype = ParserLoadType.ReflectiveParser;
            TraceLog log = new Scryber.Logging.DoNothingTraceLog(TraceRecordLevel.Off);
            PerformanceMonitor perfmon = new PerformanceMonitor(true);
            ParserSettings settings = new ParserSettings(literaltype, templategenerator, templateinstance, resolver, conformance, loadtype, log, perfmon, null);

            return settings;
        }

        
        /// <summary>
        /// returns a string as a stream
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private System.IO.Stream ToStream(string xml)
        {
            byte[] data = null;

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                using (System.IO.StreamWriter sr = new StreamWriter(ms, System.Text.Encoding.UTF8))
                {
                    sr.Write(xml);
                }
                data = ms.ToArray();
            }

            return new System.IO.MemoryStream(data);
        }

        //
        // tests
        //


        /// <summary>
        ///A test for PDFXMLParser Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void PDFXMLParserConstructorTest()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            Assert.IsNotNull(target);
            Assert.AreSame(settings, target.Settings);
        }

        
        //
        // XML Valid 1
        //
        
        #region XML Valid 1

        string xmlValid1 = @"<?xml version='1.0' encoding='utf-8' ?>
<Root1 xmlns='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:other='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                              name='root component' >
       <Complex-Element other:concrete='implemented' index='24' size='2.5' />
</Root1>";


        /// <summary>
        /// Checks the object aginst the xml representation above
        /// </summary>
        /// <param name="obj"></param>
        private void AssertValidXml1(Fakes.ParserRootOne obj)
        {
            Assert.IsNotNull(obj);
            Assert.AreEqual("root component", obj.Name);
            Assert.IsNotNull(obj.Complex);
            Assert.AreEqual("implemented", obj.Complex.Inheritable);
            Assert.AreEqual(24, obj.Complex.Index);
            Assert.AreEqual(2.5, obj.Complex.Size);
            Assert.AreEqual(0, obj.Complex.AnotherIndex); //not set

            Assert.IsNull(obj.DefaultCollection);
            Assert.IsNull(obj.CollectionOne);
        }


        /// <summary>
        ///A test for Parsind xmlValid1
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void Parse_XmlValid1_Test()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            string source = @"C:\Fake\File\Path.xml";

            using (Stream stream = ToStream(xmlValid1))
            {
                IComponent result;
                result = target.Parse(source, stream, ParseSourceType.DynamicContent);

                AssertValidXml1(result as Fakes.ParserRootOne);
            }

            //Check with stream reader

            using (Stream stream = ToStream(xmlValid1))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    IComponent actual;
                    actual = target.Parse(source, reader, ParseSourceType.DynamicContent);

                    AssertValidXml1(actual as Fakes.ParserRootOne);
                }
            }

            //Check with XMLReader

            using (Stream stream = ToStream(xmlValid1))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    IComponent actual;
                    actual = target.Parse(source, reader, ParseSourceType.DynamicContent);

                    AssertValidXml1(actual as Fakes.ParserRootOne);
                }
            }

        }

        #endregion

        //
        // Valid XML 2
        //

        #region XML Valid 2 - more complex

        string xmlValid2 = @"<?xml version='1.0' encoding='utf-8' ?>
<r:Root1 xmlns:r='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:more='Scryber.Core.UnitTests.Generation.Fakes.More, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe' >


       <Complex-Element r:concrete='implemented' index='4' size='1.5' />

       <!-- Inner collection -->

       <Collection-One>
             <r:Inner r:concrete='inner implemented' index='24' ></r:Inner>
             <more:DifferentNS index='30' ></more:DifferentNS>
             <r:Inner index='32' ></r:Inner>
       </Collection-One>

       <!-- Items below are in the default collection -->

       <more:DifferentNS inherited='another base value' index='40' >
           <Simple>12in 10pt 4mm 13</Simple>
       </more:DifferentNS>
       <r:Inner index='42' />

</r:Root1>";


        /// <summary>
        /// Checks the object aginst the xml representation above
        /// </summary>
        /// <param name="obj"></param>
        private void AssertValidXml2(Fakes.ParserRootOne obj)
        {
            Assert.IsNotNull(obj);
            Assert.IsTrue(String.IsNullOrEmpty(obj.Name));

            //Complex-Element
            Assert.IsNotNull(obj.Complex);
            Assert.AreEqual("implemented", obj.Complex.Inheritable);
            Assert.AreEqual(4, obj.Complex.Index);

            //Collection-One
            Assert.IsNotNull(obj.CollectionOne, "No Collection-One data");
            Assert.AreEqual(3, obj.CollectionOne.Count);
            Assert.IsInstanceOfType(obj.CollectionOne[0], typeof(Fakes.ParserInnerComplex));
            Assert.IsInstanceOfType(obj.CollectionOne[1], typeof(Fakes.More.ParserDifferentComplex)); //Different namespace
            Assert.IsInstanceOfType(obj.CollectionOne[2], typeof(Fakes.ParserInnerComplex));

            //Default collection

            Assert.IsNotNull(obj.DefaultCollection);
            Assert.AreEqual(2, obj.DefaultCollection.Count);

            Assert.IsInstanceOfType(obj.DefaultCollection[0], typeof(Fakes.More.ParserDifferentComplex));
            Fakes.More.ParserDifferentComplex dif = (Fakes.More.ParserDifferentComplex)obj.DefaultCollection[0];
            //Parsing order = Top Right Bottom Left.
            Assert.AreEqual((Scryber.Drawing.PDFUnit)13, dif.Thickness.Left);

            Assert.IsInstanceOfType(obj.DefaultCollection[1], typeof(Fakes.ParserInnerComplex));
            Assert.AreEqual(42, obj.DefaultCollection[1].Index);

        }

        /// <summary>
        ///A test for Parse
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void Parse_XmlValid2_Test()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);

            string source = @"C:\Fake\File\Path.xml";

            using (Stream stream = ToStream(xmlValid2))
            {
                IComponent result;
                result = target.Parse(source, stream, ParseSourceType.DynamicContent);

                AssertValidXml2(result as Fakes.ParserRootOne);
            }
        }

        #endregion

        //
        // processing instructions
        //

        #region XML Valid Processing Instructions

        string xmlValidProcess = @"<?xml version='1.0' encoding='utf-8' ?>
                             <?scryber parser-mode='Strict' parser-log='false' ?>
<Root1 xmlns='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:other='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                              name='root component' >
       <Complex-Element other:concrete='implemented' index='24' size='2.5' />
</Root1>";

        /// <summary>
        ///A test for ParseProcessingInstructions
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void ParseProcessingInstructionsTest()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            string source = @"C:\Fake\File\Path.xml";

            using (Stream stream = ToStream(xmlValidProcess))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);
            }
            
        }

        #endregion

        //
        // remote component parsing
        //

        #region XML Valid Remote references

        string xmlValidRemote = @"<?xml version='1.0' encoding='utf-8' ?>
<r:Root1 xmlns:r='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:more='Scryber.Core.UnitTests.Generation.Fakes.More, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe' >


       <Complex-Element r:concrete='implemented' index='4' size='1.5' />

       <!-- Inner collection -->

       <Collection-One>
             <r:Inner r:concrete='inner implemented' index='24' ></r:Inner>
             <more:DifferentNS index='30' ></more:DifferentNS>
             <r:Inner-Ref source='ShimFile.pcfx' ></r:Inner-Ref> <!-- remote reference -->
       </Collection-One>

       <!-- Items below are in the default collection -->

       <more:DifferentNS inherited='another base value' index='40' >
           <Simple>12in 10pt 4mm 13</Simple>
       </more:DifferentNS>
       <r:Inner-Ref source='ShimFile2.pcfx' select='//xpath/node'  /> <!-- another remote reference -->

</r:Root1>";

        int resolverCallCount = 0;

        /// <summary>
        /// Based on the above remote components there should be 2 calls to this resolver each resolving to a different path
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="xpath"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        IComponent ShimResolver(string filename, string xpath, ParserSettings settings)
        {
            resolverCallCount++;
            IComponent resolverReturnValue;
            Assert.IsNotNull(settings);

            if (string.Equals(filename, "ShimFile2.pcfx"))
            {
                Assert.AreEqual("//xpath/node", xpath);
                Fakes.ParserInnerComplex complex = new Fakes.ParserInnerComplex();
                complex.Inheritable = "ShimFile2" + xpath;
                complex.Index = 42;
                resolverReturnValue = complex;
            }
            else if (string.Equals(filename, "ShimFile.pcfx"))
            {
                Assert.IsTrue(String.IsNullOrEmpty(xpath));
                Fakes.ParserInnerComplex complex = new Fakes.ParserInnerComplex();
                complex.Inheritable = "ShimFile";
                resolverReturnValue = complex;
            }
            else
                throw new ArgumentOutOfRangeException("Unknown remote file : " + filename);

            return resolverReturnValue;
        }

        /// <summary>
        /// Checks the object aginst the xml representation above
        /// </summary>
        /// <param name="obj"></param>
        private void AssertValidXmlRemote(Fakes.ParserRootOne obj)
        {
            Assert.IsNotNull(obj);
            Assert.IsTrue(String.IsNullOrEmpty(obj.Name));

            //Complex-Element
            Assert.IsNotNull(obj.Complex);
            Assert.AreEqual("implemented", obj.Complex.Inheritable);
            Assert.AreEqual(4, obj.Complex.Index);

            //Collection-One
            Assert.IsNotNull(obj.CollectionOne, "No Collection-One data");
            Assert.AreEqual(3, obj.CollectionOne.Count);
            Assert.IsInstanceOfType(obj.CollectionOne[0], typeof(Fakes.ParserInnerComplex));
            Assert.IsInstanceOfType(obj.CollectionOne[1], typeof(Fakes.More.ParserDifferentComplex)); //Different namespace
            Assert.IsInstanceOfType(obj.CollectionOne[2], typeof(Fakes.ParserInnerComplex)); //remote reference
            Assert.AreEqual("ShimFile", obj.CollectionOne[2].Inheritable); //Check the inner propeties set in resolver.

            //Default collection

            Assert.IsNotNull(obj.DefaultCollection);
            Assert.AreEqual(2, obj.DefaultCollection.Count);

            Assert.IsInstanceOfType(obj.DefaultCollection[0], typeof(Fakes.More.ParserDifferentComplex));
            Fakes.More.ParserDifferentComplex dif = (Fakes.More.ParserDifferentComplex)obj.DefaultCollection[0];
            Assert.AreEqual((Scryber.Drawing.PDFUnit)13, dif.Thickness.Left);

            Assert.IsInstanceOfType(obj.DefaultCollection[1], typeof(Fakes.ParserInnerComplex));
            Assert.AreEqual(42, obj.DefaultCollection[1].Index);
            Assert.AreEqual("ShimFile2//xpath/node", obj.DefaultCollection[1].Inheritable); //Check the inner propeties set in resolver.
        }

        [TestMethod()]
        [TestCategory("Parser")]
        public void Parse_XmlValidRemote_Test()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            string source = @"C:\Fake\File\Path.xml";

            using (Stream stream = ToStream(xmlValidRemote))
            {
                IComponent actual;
                resolverCallCount = 0;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(2, resolverCallCount);
                AssertValidXmlRemote(actual as Fakes.ParserRootOne);

            }
        }

        #endregion


        //
        // cultural parsing
        //


        #region XML Valid Processing Instructions

        string xmlValidInvariant = @"<?xml version='1.0' encoding='utf-8' ?>
                             <?scryber parser-mode='Strict' parser-log='false' ?>
<Root1 xmlns='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:other='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                              name='root component' >
       <Complex-Element other:concrete='implemented' index='24' size='2.5' date='12/25/2015 1:30:24PM' />
</Root1>";

        /// <summary>
        /// Uses a en-GB explict culture with appropriate date formats
        /// </summary>
        string xmlValidExplicitGB = @"<?xml version='1.0' encoding='utf-8' ?>
                             <?scryber parser-mode='Strict' parser-log='false' parser-culture='en-GB' ?>
<Root1 xmlns='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:other='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                              name='root component' >
       <Complex-Element other:concrete='implemented' index='24' size='2.5' date='25/12/2015 1:30:24PM' />
</Root1>";

        /// <summary>
        /// Uses a fr-FR explict culture with appropriate date and number formats
        /// </summary>
        string xmlValidExplicitFR = @"<?xml version='1.0' encoding='utf-8' ?>
                             <?scryber parser-mode='Strict' parser-log='false' parser-culture='fr-FR' ?>
<Root1 xmlns='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
       xmlns:other='Scryber.Core.UnitTests.Generation.Fakes, Scryber.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe'
                              name='root component' >
       <Complex-Element other:concrete='implemented' index='24' size='2,5' date='25 décembre 2015 13:30:24' />
</Root1>";

        /// <summary>
        ///A test for Parse with cultural values.
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void ParseProcessingCultureTest()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            string source = @"C:\Fake\File\Path.xml";

            //Invariant format for dates and numbers

            using (Stream stream = ToStream(xmlValidInvariant))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "Invariant number failed");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "Invariant date failed");
            }

            //British format for dates and numbers

            settings = GetSettings();
            target = new XMLParser(settings);

            using (Stream stream = ToStream(xmlValidExplicitGB))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "Britsh number failed");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "Britsh date failed");
            }

            //French format for dates and numbers

            settings = GetSettings();
            target = new XMLParser(settings);

            using (Stream stream = ToStream(xmlValidExplicitFR))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "French number failed");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "French date failed");
            }

            //Make sure the current culture is ignored

            System.Globalization.CultureInfo current = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("fr-FR");

            //Invariant format for dates and numbers

            settings = GetSettings();
            target = new XMLParser(settings);

            using (Stream stream = ToStream(xmlValidInvariant))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "Invariant number failed in french culture");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "Invariant date failed in french culture");
            }

            //British format for dates and numbers

            settings = GetSettings();
            target = new XMLParser(settings);

            using (Stream stream = ToStream(xmlValidExplicitGB))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "Britsh number failed");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "Britsh date failed");
            }

            //French format for dates and numbers

            settings = GetSettings();
            target = new XMLParser(settings);

            using (Stream stream = ToStream(xmlValidExplicitFR))
            {
                IComponent actual;
                actual = target.Parse(source, stream, ParseSourceType.DynamicContent);

                Assert.AreEqual(false, target.Settings.LogParserOutput);
                Assert.AreEqual(ParserConformanceMode.Strict, target.Mode);

                Assert.IsInstanceOfType(actual, typeof(Fakes.ParserRootOne));

                Fakes.ParserRootOne r1 = (Fakes.ParserRootOne)actual;
                Assert.AreEqual(r1.Complex.Size, 2.5, "French number failed");
                Assert.AreEqual(r1.Complex.Date, new DateTime(2015, 12, 25, 13, 30, 24), "French date failed");
            }

            System.Threading.Thread.CurrentThread.CurrentCulture = current;

        }

        #endregion


        //
        // PDFXMLParser.RootComponent
        //


        /// <summary>
        ///A test for RootComponent - should stay the same if set, otherwise first parsed should become the actual root component
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void RootComponentTest()
        {
            ParserSettings settings = GetSettings();
            XMLParser target = new XMLParser(settings);
            IComponent expected = new Fakes.ParserRootOne();
            IComponent actual;

            //
            // if we set it, then it should stay as set
            //

            target.RootComponent = expected;
            string source = @"C:\Fake\File\Path.xml";
            using (Stream stream = ToStream(xmlValid1))
            {
                target.Parse(source, stream, ParseSourceType.DynamicContent); //not capturing the output - want to check the RootComponent
            }

            actual = target.RootComponent as IComponent;
            Assert.AreSame(expected, actual); //Should not have changed

            //
            // if we don't set it then it becomes the output
            //

            target = new XMLParser(settings);
            source = @"C:\Fake\File\Path.xml";
            using (Stream stream = ToStream(xmlValid1))
            {
                expected = target.Parse(source, stream, ParseSourceType.DynamicContent);
                AssertValidXml1(expected as Fakes.ParserRootOne);
            }

            actual = target.RootComponent as Fakes.ParserRootOne;  //root component should be the same as the returned value from parse
            Assert.AreSame(expected, actual);
        }


        //
        // PDFXMLParser.Settings
        //

        /// <summary>
        ///A test for Settings
        ///</summary>
        [TestMethod()]
        [TestCategory("Parser")]
        public void SettingsTest()
        {
            ParserSettings settings = this.GetSettings();
            XMLParser target = new XMLParser(settings);
            ParserSettings actual;
            actual = target.Settings;
            Assert.AreSame(settings, actual);
        }
    }
}
