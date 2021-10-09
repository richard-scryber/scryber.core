using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for PDFFontMetrics_Test and is intended
    ///to contain all PDFFontMetrics_Test Unit Tests
    ///</summary>
    [TestClass()]
    public class FontMetrics_Test
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



        /// <summary>
        ///A test for PDFFontMetrics Constructor
        ///</summary>
        [TestMethod()]
        [TestCategory("Fonts")]
        public void FontMetricsConstructor_Test()
        {
            double emheight = 12.7;
            double ascent = 120.34;
            double descent = 12.9;
            double lineheight = 13.6;
            FontMetrics target = new FontMetrics(emheight, ascent, descent, lineheight);
            

            Assert.IsNotNull(target);
            Assert.AreEqual(emheight, target.EmHeight);
            Assert.AreEqual(ascent, target.Ascent);
            Assert.AreEqual(descent, target.Descent);
            Assert.AreEqual(lineheight, target.LineHeight);
            Assert.AreEqual(lineheight - (ascent + descent), target.LineSpacing);
        }

    }
}
