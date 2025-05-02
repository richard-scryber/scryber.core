using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;

namespace Scryber.UnitLayouts
{
    [TestClass()]
    public class HTMLHeaderFooter_Tests
    {
        const string TestCategoryName = "Layout";

        PDFLayoutDocument _layout;

        /// <summary>
        /// Event handler that sets the layout instance variable, after the layout has completed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            _layout = args.Context.GetLayout<PDFLayoutDocument>();
        }

        protected string AssertGetContentFile(string name)
        {
            var path = System.Environment.CurrentDirectory;
            path = System.IO.Path.Combine(path, "../../../Content/HTML/HeadersAndFooters/" + name + ".html");
            path = System.IO.Path.GetFullPath(path);

            if (!System.IO.File.Exists(path))
                Assert.Fail("The path the file " + name + " was not found at " + path);

            return path;
        }
        

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyNoHeaderOrFooters()
        {
            var path = AssertGetContentFile("HTMLBodyNoHeadersOrFooters");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyNoHeadersOrFooters.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }
                
                var ldoc = _layout;
                Assert.IsNotNull(ldoc);
                Assert.AreEqual(1, ldoc.AllPages.Count);
                
                var lpg = ldoc.AllPages[0];
                Assert.IsNotNull(lpg);
                Assert.IsNull(lpg.HeaderBlock);
                Assert.IsNull(lpg.FooterBlock);
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                
                
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSectionNoHeaderOrFooters()
        {
            var path = AssertGetContentFile("HTMLBodyAndSectionNoHeadersOrFooters");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSectionNoHeadersOrFooters.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }
                
                var ldoc = _layout;
                Assert.IsNotNull(ldoc);
                
                //By default all sections start on a new page.
                Assert.AreEqual(2, ldoc.AllPages.Count);
                
                var lpg = ldoc.AllPages[0];
                Assert.IsNotNull(lpg);
                Assert.IsNull(lpg.HeaderBlock);
                Assert.IsNull(lpg.FooterBlock);
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                
                lpg = ldoc.AllPages[1];
                Assert.IsNotNull(lpg);
                Assert.IsNull(lpg.HeaderBlock);
                Assert.IsNull(lpg.FooterBlock);
                Assert.IsNotNull(lpg.ContentBlock);
                
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSectionNoHeaderOrFootersSamePage()
        {
            var path = AssertGetContentFile("HTMLBodyAndSectionNoHeadersOrFootersSamePage");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSectionNoHeadersOrFootersSamePage.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.SaveAsPDF(stream);
                }
                
                var ldoc = _layout;
                Assert.IsNotNull(ldoc);
                
                //By default all sections start on a new page.
                Assert.AreEqual(1, ldoc.AllPages.Count);
                
                var lpg = ldoc.AllPages[0];
                Assert.IsNotNull(lpg);
                Assert.IsNull(lpg.HeaderBlock);
                Assert.IsNull(lpg.FooterBlock);
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(2, lcol.Contents.Count);
                
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                
                
            }


        }

    }
}
