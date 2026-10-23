using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Drawing;
using Scryber.Options;

namespace Scryber.Core.UnitTests.Drawing
{
    [TestClass]
    public class FontRegistration_Tests
    {
        private static string LightFont
        {
            get { return System.IO.Path.GetFullPath(System.IO.Path.Combine(DocStreams.GetTestProjectDirectory(), "Resources", "OpenSans-Light.ttf")); }
        }

        private static object LoadRegistered(params FontRegistrationOption[] options)
        {
            var load = typeof(FontFactory).GetMethod("UnsafeLoadCustomFonts", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(load);
            var opts = new FontOptions();
            opts.Register = new System.Collections.Generic.List<FontRegistrationOption>(options);
            return load.Invoke(null, new object[] { opts });
        }

        private static FontFactory.FontReference Lookup(object bag, string family, FontStyle style, int weight)
        {
            var indexer = bag.GetType().GetProperty("Item", new[] { typeof(string), typeof(FontStyle), typeof(int) });
            Assert.IsNotNull(indexer);
            return (FontFactory.FontReference)indexer.GetValue(bag, new object[] { family, style, weight });
        }

        [TestMethod]
        public void RegisteredFont_UsesDeclaredWeightAndStyle()
        {
            var bag = LoadRegistered(new FontRegistrationOption()
            {
                Family = "RegisteredBrand", File = LightFont, Weight = 500, Style = "italic"
            });

            Assert.IsNull(Lookup(bag, "RegisteredBrand", FontStyle.Regular, 500), "Registered as italic, not regular");
            Assert.IsNull(Lookup(bag, "RegisteredBrand", FontStyle.Italic, 300), "Registered as 500, not the file weight of 300");
            var found = Lookup(bag, "RegisteredBrand", FontStyle.Italic, 500);
            Assert.IsNotNull(found);
            Assert.AreEqual(500, found.Weight);
            Assert.AreEqual(FontStyle.Italic, found.Style);
        }

        [TestMethod]
        public void RegisteredFont_WithoutWeightOrStyle_UsesFile()
        {
            var bag = LoadRegistered(new FontRegistrationOption() { Family = "RegisteredFile", File = LightFont });
            var found = Lookup(bag, "RegisteredFile", FontStyle.Regular, 300);
            Assert.IsNotNull(found, "The file weight should be used when none is declared");
        }
    }
}
