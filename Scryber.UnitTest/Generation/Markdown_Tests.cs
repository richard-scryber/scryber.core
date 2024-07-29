using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
using Scryber.Html;
using Scryber.PDF.Layout;

namespace Scryber.Core.UnitTests.Generation
{
    [TestClass]
    public class Markdown_Tests
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
        
        public Markdown_Tests()
        {
        }

        private string src = "";

        [TestMethod]
        public void MarkdownToHtmlTest()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/Markdown/Markdown.md", this.TestContext);

            
            var content = System.IO.File.ReadAllText(path);
            
            var md = new Scryber.Html.Parsing.Markdown();
            var html = md.Transform(content);

            System.Diagnostics.Debug.WriteLine(html);

            var expected =
                "<h1>scryber.core</h1>\n\n<p>The dotnet core scryber pdf creation library</p>\n\n<h2>Scryber PDF Library</h2>\n\n<p>The scryber library is an advanced, complete, pdf creation library for dotnet core. \nIt supports the easy definition of documents, pages, content, shapes and images either by xml templates or simple code. </p>\n\n<p>With a styles based layout it is easy to create good looking and flowing documents. \nBinding in xml allows you to quickly load data from many sources and output to PDF. </p>\n\n<h2>Getting Started</h2>\n\n<p>The easiest way to begin is to use the Nuget Packages here</p>\n\n<p><a href=\"https://www.nuget.org/packages/scryber.core/\">scryber.core package</a>\n(Base libraries for GUI or console applications)</p>\n\n<p>OR for asp.net mvc</p>\n\n<p><a href=\"https://www.nuget.org/packages/scryber.core.mvc/\">scryber.core.mvc package</a>\n(Which includes the scryber.core package).</p>\n\n<p>The full documentation is available here</p>\n\n<p><a href=\"https://scrybercore.readthedocs.io/en/latest/\">scryber.core documentation</a></p>\n\n<h2>Hello World Plus</h2>\n\n<p>Just a bit more than a hello world example.</p>\n\n<h3>Create your template pdfx (xml) file.</h3>\n\n<p>```xml</p>\n\n<pre><code>  &lt;?xml version='1.0' encoding='utf-8' ?&gt;\n  &lt;doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'\n                xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'\n                xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' &gt;\n    &lt;Params&gt;\n      &lt;doc:String-Param id='Title' value='Hello World'  /&gt;\n    &lt;/Params&gt;\n    &lt;Styles&gt;\n      &lt;styles:Style applied-type='doc:Page'&gt;\n        &lt;styles:Font family='Arial' size='14pt' /&gt;\n      &lt;/styles:Style&gt;\n\n      &lt;styles:Style applied-class='heading' &gt;\n        &lt;styles:Fill color='#FF7777'/&gt;\n        &lt;styles:Text decoration='Underline'/&gt;\n      &lt;/styles:Style&gt;\n\n    &lt;/Styles&gt;\n    &lt;Pages&gt;\n\n      &lt;doc:Page styles:margins='20pt'&gt;\n        &lt;Content&gt;\n          &lt;doc:H1 styles:class='heading' text='{@:Title}' /&gt;\n          &lt;doc:Div&gt;We hope you like scryber.&lt;/doc:Div&gt;\n\n        &lt;/Content&gt;\n      &lt;/doc:Page&gt;\n    &lt;/Pages&gt;\n\n  &lt;/doc:Document&gt;\n</code></pre>\n\n<p>```</p>\n\n<h3>From your application code.</h3>\n\n<p>```cs</p>\n\n<pre><code>  //using Scryber.Components\n\n  static void Main(string[] args)\n  {\n      using(var doc = PDFDocument.ParseDocument(\"[input template].pdfx\"))\n      {\n          doc.Params[\"Title\"] = \"Hello World from Scryber\";\n          var page = doc.Pages[0] as PDFPage;\n          page.Contents.Add(new PDFLabel(){ Text = \"My Content\" });\n          doc.ProcessDocument(\"[output file].pdf\");\n      }\n  }\n</code></pre>\n\n<p>```</p>\n\n<h3>Or from an MVC web application</h3>\n\n<p>```cs</p>\n\n<pre><code>  //using Scryber.Components\n  //using Scryber.Components.Mvc\n\n  public IActionResult HelloWorld(string title = \"Hello World from Scryber\")\n  {\n    using(var doc = PDFDocument.ParseDocument(\"[input template].pdfx\"))\n      {\n          doc.Params[\"Title\"] = title;\n          var page = doc.Pages[0] as PDFPage;\n          page.Contents.Add(new PDFLabel(){ Text = \"My Content\" });\n\n          return this.PDF(doc); // inline:false, outputFileName:\"HelloWorld.pdf\"\n      }\n  }\n</code></pre>\n\n<p>```</p>\n\n<h3>And the output</h3>\n\n<p>Check out Read the Docs for more information on how to use the library.</p>\n\n<p><a href=\"https://scrybercore.readthedocs.io/en/latest/\">scryber.core documentation</a></p>\n\n<h2>Getting Involved</h2>\n\n<p>We would love to hear your feedback. Feel free to get in touch.\nIssues, ideas, includes are all welcome.</p>\n\n<p>If you would like to help with building, extending then happy to get contributions</p>\n";
            Assert.AreEqual(expected, html);
        }


        [TestMethod]
        public void MarkdownToPDFTest()
        {
            var path = DocStreams.AssertGetContentPath("../../Scryber.UnitTest/Content/Markdown/Markdown.md", this.TestContext);

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
            var layout = args.Context.GetLayout<PDFLayoutDocument>();
            Assert.IsNotNull(layout);
            Assert.AreEqual(2, layout.AllPages.Count);
        }
    }
}
