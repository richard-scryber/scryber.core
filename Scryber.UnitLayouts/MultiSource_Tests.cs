using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Drawing;
using Scryber.Html.Components;
using Scryber.PDF.Resources;
using Scryber.Svg.Components;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class MultiSource_Tests
    {
        const string TestCategoryName = "Layout Multisource";

        PDFLayoutDocument layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            layout = args.Context.GetLayout<PDFLayoutDocument>();
        }


        protected string AssertGetContentFile(string name, string ext = "html")
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/Multisource/" + name + "." + ext);
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }

        public string ReadContentFile(string fileName, string ext = "html")
        {
            var path = AssertGetContentFile(fileName, ext);
            var content = System.IO.File.ReadAllText(path);

            return content;
        }

        public static bool AddStyleSheetToDocument(string sourceStyles, Document doc)
        {
            var html = doc as Scryber.Html.Components.HTMLDocument;

            if (!string.IsNullOrEmpty(sourceStyles))
            {
                if (null == html.Head)
                {
                    html.Head = new HTMLHead();
                }
                
                var css = new HTMLStyle()
                {
                    Contents = sourceStyles,
                    LoadedSource = string.Empty,
                    LoadType = ParserLoadType.None,
                };
                
                html.Head.Contents.Add(css);
            }

            return false;

        }
        
        public static object ParseJsonData(string source)
        {
            System.Text.Json.JsonDocument doc;
            doc = System.Text.Json.JsonDocument.Parse(source);

            return doc.RootElement.Clone();
        }
        

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void FunctionTemplateList_Test()
        {
            var path = AssertGetContentFile("FunctionTemplate");
            
            var fonts = ReadContentFile("Fonts", "css");
            var styles = ReadContentFile("Styles", "css");
            var model = ReadContentFile("ModelData", "json");
            var funcs = ReadContentFile("FuncList", "json");
            
            var doc = Document.ParseDocument(path);
            AddStyleSheetToDocument(fonts, doc);
            AddStyleSheetToDocument(styles, doc);
            doc.Params["model"] = ParseJsonData(model);
            doc.Params["func"] = ParseJsonData(funcs);

            using (var ms = DocStreams.GetOutputStream("Multisource_01_Functions.pdf"))
            {
                doc.Pages[0].Style.OverlayGrid.ShowGrid = true;
                doc.Pages[0].Style.OverlayGrid.GridSpacing = 10;
                doc.Pages[0].Style.OverlayGrid.GridColor = StandardColors.Aqua;
                doc.Pages[0].Style.OverlayGrid.GridMajorCount = 5;
                doc.RenderOptions.Compression = OutputCompressionType.None;
                

                doc.LayoutComplete += Doc_LayoutComplete;
                doc.ConformanceMode = ParserConformanceMode.Lax;
                doc.TraceLog.SetRecordLevel(TraceRecordLevel.Errors);
                doc.AppendTraceLog = true;
                doc.SaveAsPDF(ms);
            }

            
            
            
        }
        
    }
}
