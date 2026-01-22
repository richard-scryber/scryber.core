using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.Html.Components;
using Scryber.PDF;
using Scryber.Drawing;

namespace Scryber.UnitLayouts
{
    /// <summary>
    /// Tests the layout of SVG Images based on image sizes and inner svg dimesions
    /// </summary>
    [TestClass()]
    public class SVGImageLayout_Tests
    {
        

        //Samle.svg - 200 x 150 natural size
        

        PDFLayoutDocument layout;

        
        private string GetResourcePath(string category, string filename, bool assertExists = true)
        {
            var dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            dir = System.IO.Path.Combine(dir, "../../../Content");
            
            if(!string.IsNullOrEmpty(category))
                dir = System.IO.Path.Combine(dir, category);
            
            dir = System.IO.Path.Combine(dir, filename);
            
            if(assertExists)
                Assert.IsTrue(System.IO.File.Exists(dir), "Could not find the file " + dir);
            
            return dir;
        }

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            this.layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        private PDFLayoutComponentRun GetBlockImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
        }

        private PDFLayoutComponentRun GetInlineImageRunForPage(int pg, int column = 0, int contentIndex = 0, int runIndex = 0)
        {
            var lpg = layout.AllPages[pg];
            var l1 = lpg.ContentBlock.Columns[column].Contents[contentIndex] as PDFLayoutLine;
            var lrun = l1.Runs[runIndex] as Scryber.PDF.Layout.PDFLayoutComponentRun;
            return lrun;
        }

        private void AssertAreApproxEqual(double one, double two, string message = null)
        {
            int precision = 5;
            one = Math.Round(one, precision);
            two = Math.Round(two, precision);
            Assert.AreEqual(one, two, message);
        }

        
        
        
        //no image sizes - no inner

        [TestMethod()]
        public void SVGImageContainer_NoImgSize_VariousSVGDimensions()
        {
            var path = GetResourcePath("SVGImages", "SVGImageContainer_NoImgSizes.html");

            using (var doc = Document.ParseDocument(path))
            {
                using (var stream = DocStreams.GetOutputStream("SVGImageContainer_NoImgSizes.pdf"))
                {
                    doc.AppendTraceLog = true;
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }
            }
        }
        




        //no image sizes - inner viewbox
        
        //no image sizes - inner width
        
        //no image sizes - inner height
        
        //no image sizes - inner width and height
        
        //no image sizes - inner relative width
        
        //no image sizes - inner relative height
        
        //no image sizes - inner relative width and height
        
        //no image sizes - inner viewbox, width and height
        
        //no image sizes - inner viewbox and width
        
        //no image sizes - inner viewbox and height
        
        //no image sizes - inner INVALID sizes
        
        //no image sizes - inner REVERSED sizes
        
        
        //image width and height - no inner
        
        //image width and height - inner viewbox
        
        //image width and height - inner width
        
        //image width and height - inner height
        
        //image width and height - inner width and height
        
        //image width and height - inner relative width
        
        //image width and height - inner relative height
        
        //image width and height - inner relative width and height
        
        //image width and height - inner viewbox, width and height
        
        //image width and height - inner viewbox and width
        
        //image width and height - inner viewbox and height
        
        //image width and height - inner INVALID sizes
        
        //image width and height - inner REVERSED sizes


        //image width only - no inner
        
        //image width only - inner viewbox
        
        //image width only - inner width
        
        //image width only - inner height
        
        //image width only - inner width and height
        
        //image width only - inner relative width
        
        //image width only - inner relative height
        
        //image width only - inner relative width and height
        
        //image width only - inner viewbox, width and height
        
        //image width only - inner viewbox and width
        
        //image width only - inner viewbox and height
        
        //image width only - inner INVALID sizes
        
        //image width only - inner REVERSED sizes
        
        
        //image height only - no inner
        
        //image height only - inner viewbox
        
        //image height only - inner width
        
        //image height only - inner height
        
        //image height only - inner width and height
        
        //image height only - inner relative width
        
        //image height only - inner relative height
        
        //image height only - inner relative width and height
        
        //image height only - inner viewbox, width and height
        
        //image height only - inner viewbox and width
        
        //image height only - inner viewbox and height
        
        //image height only - inner INVALID sizes
        
        //image height only - inner REVERSED sizes
    }
}
