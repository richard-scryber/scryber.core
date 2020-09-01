using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Layout;

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
    }
}
