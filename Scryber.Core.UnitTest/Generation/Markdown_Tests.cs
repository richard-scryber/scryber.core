using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass]
    public class Markdown_Tests
    {
        public Markdown_Tests()
        {
        }

        [TestMethod]
        public void MarkdownToHtmlTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/Markdown.md");
            path = System.IO.Path.GetFullPath(path);
            var content = System.IO.File.ReadAllText(path);
            
            var md = new Scryber.Html.Parsing.Markdown();
            var html = md.Transform(content);

            System.Diagnostics.Debug.WriteLine(html);
        }


        [TestMethod]
        public void MarkdownToPDFTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/Markdown.md");
            path = System.IO.Path.GetFullPath(path);
            var content = System.IO.File.ReadAllText(path);

            var doc = new PDFDocument();
            var pg = new PDFSection();
            pg.FontSize = 12;
            var frag = new PDFHtmlFragment();

            doc.Pages.Add(pg);
            pg.Contents.Add(frag);
            frag.ContentsAsString = content;
            frag.Format = Html.HtmlFormatType.Markdown;

            doc.LayoutComplete += Doc_LayoutComplete;
            using (var ms = new System.IO.FileStream("/Users/Richard/Test.pdf", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                doc.ProcessDocument(ms);


        }

        private void Doc_LayoutComplete(object sender, PDFLayoutEventArgs args)
        {
            
        }
    }
}
