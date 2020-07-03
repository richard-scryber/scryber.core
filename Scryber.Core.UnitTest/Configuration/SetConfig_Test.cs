using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Scryber.UnitTests.Configuration
{
    /// <summary>
    /// Tests the configuration with any appsettings.json
    /// </summary>
    [TestClass()]
    public class SetConfig_Test
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
            var path = testContext.TestRunDirectory;
            path = Path.GetFullPath(Path.Combine(path, "../../../appsettings.json"));
            if (!File.Exists(path))
            {
                path = Path.Combine(testContext.DeploymentDirectory, "../../../appsettings.json");
                path = Path.GetFullPath(path);

                if (!File.Exists(path))
                    throw new FileNotFoundException("Cannot find the location of the appsettings.json file to run the tests from");
            }

            var builder = new ConfigurationBuilder()
                                .AddJsonFile(path, optional: false, reloadOnChange: false);

            IConfiguration config = builder.Build();

            Assert.IsNotNull(config);

            //Create a dictionary services provider
            System.Collections.Generic.Dictionary<Type, object> configDict = new System.Collections.Generic.Dictionary<Type, object>();
            configDict.Add(typeof(IConfiguration), config);
            ServiceProvider.DictionaryServiceProvider services = new ServiceProvider.DictionaryServiceProvider(configDict);

            ServiceProvider.SetProvider(services);
            ServiceProvider.GetService<IScryberConfigurationService>().Reset();
            testContext.WriteLine("Loaded the appsettings file and added it to the services with a reset");
        }

        [TestMethod()]
        [TestCategory("Config")]
        public void ParsingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var parsing = service.ParsingOptions;
            Assert.IsNotNull(parsing, "The parsing options are null");

            Assert.AreEqual(ParserReferenceMissingAction.LogError, parsing.MissingReferenceAction, "The missing reference action is not set to LogError");

            //Namespace Mappings
            Assert.IsNotNull(parsing.Namespaces, "Namespace mappings is null");

            int expectedLength = 4;
            string expectedNs = "Scryber.Core.UnitTests.Generation.Fakes";
            string expectedAssm = "Scryber.UnitTests";
            string expectedSrc = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Fakes.xsd";

            Assert.AreEqual(expectedLength, parsing.Namespaces.Count, "Namespace mappings length is not 4");

            var xmlNs = parsing.GetXmlNamespaceForAssemblyNamespace(expectedNs, expectedAssm);
            Assert.AreEqual(expectedSrc, xmlNs, "The expected xml source was not matched");

            //Check defaults are there
            expectedNs = "Scryber.Components";
            expectedAssm = "Scryber.Components, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";
            expectedSrc = "http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd";

            xmlNs = parsing.GetXmlNamespaceForAssemblyNamespace(expectedNs, expectedAssm);
            Assert.AreEqual(expectedSrc, xmlNs, "The expected xml source was not matched");


            Assert.IsNotNull(parsing.Bindings, "Binding prefixes are null");

            expectedLength = 5;
            string expectedPrefix = "custom";

            expectedAssm = "Scryber.Generation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=872cbeb81db952fe";
            string expectedType = "Scryber.BindingXPathExpressionFactory";

            Assert.AreEqual(expectedLength, parsing.Bindings.Count, "Binding mappings length is not " + expectedLength);
            Assert.AreEqual(expectedPrefix, parsing.Bindings[expectedLength-1].Prefix);
            Assert.AreEqual(expectedAssm, parsing.Bindings[expectedLength - 1].FactoryAssembly);
            Assert.AreEqual(expectedType, parsing.Bindings[expectedLength - 1].FactoryType);

        }


        [TestMethod()]
        [TestCategory("Config")]
        public void FontOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var font = service.FontOptions;
            Assert.IsNotNull(font, "The font options are null");

            Assert.IsFalse(font.UseSystemFonts, "Use System Foints is not false");
            Assert.IsTrue(font.FontSubstitution, "Use Font Substitution is not true");
            Assert.IsFalse(string.IsNullOrEmpty(font.DefaultDirectory), "The default font directory is not provided");
            Assert.AreEqual("/Users/RichardHewitson/Library/Fonts", font.DefaultDirectory, "The default font directory is not '/Users/RichardHewitson/Library/Fonts'");
            Assert.AreEqual("Arial", font.DefaultFont, "The default font is not 'Arial'");

            //Should be 5 registered fonts - 4 x Gill Sans and a Dingbats Regular
            Assert.IsNotNull(font.Register, "The font register should not be null");
            Assert.AreEqual(5, font.Register.Length, "There are not 5 registered fonts");

            var family = "Gill Sans";
            var style = System.Drawing.FontStyle.Regular;
            var fileStem = "/Users/RichardHewitson/Other/GillSans";
            var fileExt = ".ttf";

            //Gill Sans Regular
            var option = font.Register[0]; 
            Assert.AreEqual(family, option.Family);
            Assert.AreEqual(style, option.Style);
            Assert.AreEqual(fileStem + fileExt, option.File);

            // Gill Sans Bold
            option = font.Register[1]; 
            style = System.Drawing.FontStyle.Bold;

            Assert.AreEqual(family, option.Family);
            Assert.AreEqual(style, option.Style);
            Assert.AreEqual(fileStem + "Bold" + fileExt, option.File);

            // Gill Sans Bold
            option = font.Register[2];
            style = System.Drawing.FontStyle.Italic;

            Assert.AreEqual(family, option.Family);
            Assert.AreEqual(style, option.Style);
            Assert.AreEqual(fileStem + "Italic" + fileExt, option.File);

            // Gill Sans Bold Italif
            option = font.Register[3];
            style = System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic;

            Assert.AreEqual(family, option.Family);
            Assert.AreEqual(style, option.Style);
            Assert.AreEqual(fileStem + "BoldItalic" + fileExt, option.File);

            // Dingbats
            option = font.Register[4];
            family = "Dingbats";
            style = System.Drawing.FontStyle.Regular;
            fileStem = "/Users/RichardHewitson/Other/WebDingbats";

            Assert.AreEqual(family, option.Family);
            Assert.AreEqual(style, option.Style);
            Assert.AreEqual(fileStem + fileExt, option.File);

        }


        [TestMethod()]
        [TestCategory("Config")]
        public void OutputOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var output = service.OutputOptions;
            Assert.IsNotNull(output, "The render options are null");

            Assert.AreEqual(OutputCompressionType.None, output.Compression, "Expected None output compression");
            Assert.AreEqual(OutputCompliance.Other, output.Compliance, "Expected Other output compliance");
            Assert.AreEqual(OutputStringType.Text, output.StringType, "Expected Text output string type");
            Assert.AreEqual(ComponentNameOutput.All, output.NameOutput,  "Expected 'All' string type");
            Assert.AreEqual("1.4", output.PDFVersion, "Expected a PDF Version of 1.4");


        }


        [TestMethod()]
        [TestCategory("Config")]
        public void ImagingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var img = service.ImagingOptions;

            Assert.IsNotNull(img, "The imaging options are null");

            Assert.IsTrue(img.AllowMissingImages, "Missing images is not true");
            Assert.AreEqual(60, img.ImageCacheDuration, "The image cache duration is not 60");
            Assert.IsNotNull(img.Factories, "The image factories are null");

            Assert.AreEqual(1, img.Factories.Length);
            Assert.AreEqual("*.\\.dynamic\\.png", img.Factories[0].Match, "The img factory match path is incorrect");
            Assert.AreEqual("Scryber.UnitTests.Mocks.DynamicImageFactory", img.Factories[0].FactoryType, "The image factory type is not correct");
            Assert.AreEqual("Scryber.UnitTests", img.Factories[0].FactoryAssembly, "The image factory assembly is not correct");
        }

        [TestMethod()]
        [TestCategory("Config")]
        public void TracingOptions_Test()
        {
            var service = Scryber.ServiceProvider.GetService<IScryberConfigurationService>();
            Assert.IsNotNull(service, "The scryber config service is null");

            var trace = service.TracingOptions;
            Assert.IsNotNull(trace, "The tracing options are null");
            Assert.AreEqual(TraceRecordLevel.Diagnostic, trace.TraceLevel, "Trace level is not Debug");

            Assert.IsNotNull(trace.Loggers, "The tracing loggers is null");
            Assert.AreEqual(2, trace.Loggers.Length, "The length of the tracing loggers is not 1");

            Assert.AreEqual("Spoof", trace.Loggers[0].Name, "THe logger name is not Spoof");
            Assert.AreEqual("Scryber.UnitTests.Mocks.SpoofTraceLog", trace.Loggers[0].FactoryType, "The logger type does not match");
            Assert.AreEqual("Scryber.UnitTests", trace.Loggers[0].FactoryAssembly, "The loggers assembly does not match");
            Assert.IsFalse(trace.Loggers[0].Enabled, "The first logger is incorrectly enabled");

            Assert.AreEqual("Spoof2", trace.Loggers[1].Name, "The second logger name is not Spoof2");
            Assert.AreEqual("Scryber.UnitTests.Mocks.SpoofTraceLog2", trace.Loggers[1].FactoryType, "The second logger type does not match");
            Assert.AreEqual("Scryber.UnitTests", trace.Loggers[1].FactoryAssembly, "The second loggers assembly does not match");
            Assert.IsTrue(trace.Loggers[1].Enabled, "The default for a logger should be enabled");
        }

    }
}
