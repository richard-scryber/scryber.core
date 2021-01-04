using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass]
    public class ReadMe_Tests
    {
        public ReadMe_Tests()
        {
        }

        [TestMethod]
        public void ReadMe_PDFX_Test()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/PDFX/ReadMeSample.xml");
            path = System.IO.Path.GetFullPath(path);
            var content = System.IO.File.ReadAllText(path);

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["model"] = new
                {
                    Title = "This is the title",
                    TitleStyle = "color:red;"
                };

                using (var output = DocStreams.GetOutputStream("ReadMePDFX.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(output);
                }
            }
        }


        [TestMethod]
        public void ReadMe_Html_Test()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/ReadMeSample.html");
            path = System.IO.Path.GetFullPath(path);
            var content = System.IO.File.ReadAllText(path);

            using (var doc = Document.ParseDocument(path))
            {
                doc.Params["model"] = new
                {
                    Title = "This is the title",
                    TitleStyle = "color:red;"
                };

                using (var output = DocStreams.GetOutputStream("ReadMeHtml.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(output);
                }
            }

        }

        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            
        }
    }
}
