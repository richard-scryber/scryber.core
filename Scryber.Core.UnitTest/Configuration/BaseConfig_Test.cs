using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.UnitTests.Configuration
{
    /// <summary>
    /// Tests the configuration without any appsettings.json
    /// </summary>
    [TestClass()]
    public class BaseConfig_Test
    {
        private TestContext testContextInstance;

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

        [ClassInitialize()]
        public static void ConfigClassInitialize(TestContext testContext)
        {
            ServiceProvider.GetService<IScryberConfigurationService>().Reset();
            testContext.WriteLine("Loaded and reset the configuration service");
        }



        [TestMethod()]
        [TestCategory("Config")]
        public void ParsingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var parsing = service.ParsingOptions;
            Assert.IsNotNull(parsing, "The parsing options are null");

            Assert.AreEqual(parsing.MissingReferenceAction, ParserReferenceMissingAction.RaiseException, "The missing reference action is not to throw an error");
            Assert.IsNotNull(parsing.Bindings,"Binding prefixes are null");

            //We should have 3 namespaces by default for Scryber.Components, Scryber.Data and Scryber.Styles
            Assert.IsNotNull(parsing.Namespaces, "Namespace Mappings are null");

            Assert.AreEqual(3, parsing.Namespaces.Count, "There are not 3 namespaces");

            var compType = typeof(Scryber.Components.PDFDocument);
            var dataType = typeof(Scryber.Data.PDFXMLDataSource);
            var styleType = typeof(Scryber.Styles.PDFStyle);

            var full = compType.Namespace + ", " + compType.Assembly.FullName;
            
            var xml = parsing.GetXmlNamespaceForAssemblyNamespace(full);
            var expected = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd";
            Assert.AreEqual(expected, xml, "The xml namepaces for the components do not match for " + full);


            full = dataType.Namespace + ", " + dataType.Assembly.FullName;

            xml = parsing.GetXmlNamespaceForAssemblyNamespace(full);
            expected = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd";
            Assert.AreEqual(expected, xml, "The xml namepaces for the components do not match for " + full);


            full = styleType.Namespace + ", " + styleType.Assembly.FullName;

            xml = parsing.GetXmlNamespaceForAssemblyNamespace(full);
            expected = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd";
            Assert.AreEqual(expected, xml, "The xml namepaces for the components do not match for " + full);


            // Bindings check

            Assert.AreEqual(3, parsing.Namespaces.Count, "There are not 3 namespaces");

            var itemType = typeof(Scryber.Binding.BindingItemExpressionFactory);
            var atType = typeof(Scryber.Binding.BindingItemExpressionFactory);
            var xpathType = typeof(Scryber.Binding.BindingXPathExpressionFactory);

            var bind = parsing.GetGetBindingFactoryForPrefix("item");
            Assert.AreEqual(itemType, bind.GetType(), "The binding factory for the prefix 'item' does not match");


            bind = parsing.GetGetBindingFactoryForPrefix("@");
            Assert.AreEqual(atType, bind.GetType(), "The binding factory for the prefix '@' does not match");

            bind = parsing.GetGetBindingFactoryForPrefix("xpath");
            Assert.AreEqual(xpathType, bind.GetType(), "The binding factory for the prefix 'xpath' does not match");


        }


        [TestMethod()]
        [TestCategory("Config")]
        public void FontOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var font = service.FontOptions;
            Assert.IsNotNull(font, "The font options are null");

            Assert.IsTrue(font.UseSystemFonts, "Use System Foints is not true");
            Assert.IsFalse(font.FontSubstitution, "Use Font Substitution is not false");
            Assert.IsTrue(string.IsNullOrEmpty(font.DefaultDirectory), "The default font directory is not null");
            Assert.AreEqual(font.DefaultFont, "Sans-Serif", "The default font is not 'Sans-Serif'");

        }

        [TestMethod()]
        [TestCategory("Config")]
        public void ImagingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var img = service.ImagingOptions;

            Assert.IsNotNull(img, "The imaging options are null");
            Assert.IsFalse(img.AllowMissingImages, "Missing images is not false");
            Assert.AreEqual(-1, img.ImageCacheDuration, "The image cache duration is not -1");
            Assert.IsNull(img.Factories, "The image factories are not null");
        }



        [TestMethod()]
        [TestCategory("Config")]
        public void OutputOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var output = service.OutputOptions;
            Assert.IsNotNull(output, "The output options are null");

        }

        [TestMethod()]
        [TestCategory("Config")]
        public void TracingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var trace = service.TracingOptions;
            Assert.IsNotNull(trace, "The tracing options are null");

        }

    }
}
