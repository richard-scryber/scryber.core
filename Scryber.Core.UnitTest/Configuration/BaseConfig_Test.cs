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
            Assert.IsNull(parsing.Bindings,"Binding prefixes are not null");
            Assert.IsNull(parsing.Namespaces, "Namespace Mappings are not null");
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
