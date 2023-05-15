using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.OpenType;

namespace Scryber.Core.UnitTests.OpenType
{
    [TestClass()]
    [TestCategory("Font")]
    public class FontConversion_Test
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


        private string Woff2Path;
        private string TTFPath;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            var root = System.Environment.CurrentDirectory;
            var path = root + "../../../../Mocks/Fonts/Raleway100.woff2";
            path = System.IO.Path.GetFullPath(path);
            Woff2Path = path;

            
            path = root + "../../../../Mocks/Fonts/Raleway100.ttf";
            path = System.IO.Path.GetFullPath(path);
            TTFPath = path;
        }


        [TestMethod]
        public void ConvertWoff2ToTTF()
        {
            var data = System.IO.File.ReadAllBytes(Woff2Path);
            var result = Scryber.PDF.Resources.PDFWoff2ToTTF.ConvertWoof2ToTTF(data);


            using (var output = DocStreams.GetOutputStream("Raleway100.ttf"))
            {
                var len = result.Length;
                output.Write(result, 0, len);
            }

            Assert.Inconclusive("Woff2 decoding is not done");


        }
        

    }
}
