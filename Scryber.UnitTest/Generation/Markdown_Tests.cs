using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass]
    public class Markdown_Tests
    {
        public Markdown_Tests()
        {
        }

        private string src = "";

        [TestMethod]
        public void MarkdownToHtmlTest()
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/Markdown/Markdown.md");
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
            path = System.IO.Path.Combine(path, "../../../Content/Markdown/Markdown.md");
            path = System.IO.Path.GetFullPath(path);
            var content = System.IO.File.ReadAllText(path);

            var doc = new Document();
            var pg = new Section();
            pg.FontSize = 12;
            pg.Margins = new Scryber.Drawing.Thickness(20);
            var frag = new HtmlFragment();

            doc.Pages.Add(pg);
            pg.Contents.Add(frag);
            frag.ContentsAsString = content;
            frag.Format = HtmlFormatType.Markdown;

            using (var output = DocStreams.GetOutputStream("MarkdownTest.pdf"))
            {
                doc.LayoutComplete += Doc_LayoutComplete;
                doc.SaveAsPDF(output);
            }


        }

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            
        }
    }
}
