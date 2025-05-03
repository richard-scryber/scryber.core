using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Html.Components;
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
                Assert.Inconclusive("The path the file " + name + " was not found at " + path);

            return path;
        }
        

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void Body_01_NoHeaderOrFooters()
        {
            var path = AssertGetContentFile("HTMLBody_01_NoHeadersOrFooters");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBody_01_NoHeadersOrFooters.pdf"))
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
        public void BodyAndSection_02_NoHeaderOrFooters()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_02_NoHeadersOrFooters");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_02_NoHeadersOrFooters.pdf"))
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
                
                //Section forces a new page
                
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
                Assert.AreEqual(2, lcol.Contents.Count);
                
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);

                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);

            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_03_NoHeaderOrFootersSamePage()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_03_NoHeadersOrFootersSamePage");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_03_NoHeadersOrFootersSamePage.pdf"))
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
                Assert.AreEqual(3, lcol.Contents.Count);
                
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                
                lblock = lcol.Contents[2];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                
                
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_04_BodyHeader()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_04_BodyHeader");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_04_BodyHeader.pdf"))
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
                Assert.IsNull(lpg.FooterBlock);
                
                
                //Check the page header
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(lpg.Height - 15 - 20, lpg.ContentBlock.Height); //- margins and header
                
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                //Before Div
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                
                //Section forces a new page
                
                lpg = ldoc.AllPages[1];
                Assert.IsNotNull(lpg);
                Assert.IsNull(lpg.FooterBlock);
                
                //Check the page header again
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(2, lcol.Contents.Count);
                Assert.AreEqual(lpg.Height - 15 - 20, lpg.ContentBlock.Height); //- margins and header
                
                
                //Section
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                
                //After div
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_05_BodyHeaderAndFooter()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_05_BodyHeaderAndFooter");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_05_BodyHeaderAndFooter.pdf"))
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
                
                //Check the page header
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the page footer
                Assert.IsNotNull(lpg.FooterBlock);
                Assert.AreEqual(15, lpg.FooterBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.FooterBlock.Width);
                Assert.IsInstanceOfType(lpg.FooterBlock.Owner, typeof(PDFPageFooter));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(lpg.Height - 30 - 20, lpg.ContentBlock.Height); //- margins and header and footer
                
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                //Before Div
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                
                //Section forces a new page
                
                lpg = ldoc.AllPages[1];
                Assert.IsNotNull(lpg);
                
                
                //Check the page header again
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the page footer again
                Assert.IsNotNull(lpg.FooterBlock);
                Assert.AreEqual(15, lpg.FooterBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.FooterBlock.Width);
                Assert.IsInstanceOfType(lpg.FooterBlock.Owner, typeof(PDFPageFooter));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(2, lcol.Contents.Count);
                Assert.AreEqual(lpg.Height - 30 - 20, lpg.ContentBlock.Height); //- margins and header and footer
                
                
                //Section
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                
                //After div
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_06_BodyHeaderAndFooterWithContinuation()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_06_BodyHeaderAndFooterWithContinuation");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_06_BodyHeaderAndFooterWithContinuation.pdf"))
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
                
                //Check the page header
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the page footer
                Assert.IsNotNull(lpg.FooterBlock);
                Assert.AreEqual(15, lpg.FooterBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.FooterBlock.Width);
                Assert.IsInstanceOfType(lpg.FooterBlock.Owner, typeof(PDFPageFooter));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(lpg.Height - 30 - 20, lpg.ContentBlock.Height); //- margins and header and footer
                
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);
                
                //Before Div
                var lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                
                //Section forces a new page
                
                lpg = ldoc.AllPages[1];
                Assert.IsNotNull(lpg);
                
                
                //Check the page header again
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(30 + 20, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));
                
                //Check the page footer again
                Assert.IsNotNull(lpg.FooterBlock);
                Assert.AreEqual(45 + 20, lpg.FooterBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.FooterBlock.Width);
                Assert.IsInstanceOfType(lpg.FooterBlock.Owner, typeof(PDFPageFooter));
                
                //Check the content block
                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(2, lcol.Contents.Count);
                Assert.AreEqual(lpg.Height - 75 - 40 - 20, lpg.ContentBlock.Height); //- margins and header and footer
                
                
                //Section
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                
                //After div
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
            }


        }
        
        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_07_SectionHeader()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_07_SectionHeader");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_07_SectionHeader.pdf"))
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
                
                //Section forces a new page
                
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
                Assert.AreEqual(2, lcol.Contents.Count);
                
                lblock = lcol.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(30 + 10, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                
                
                //Inside the section
                var lsect = lblock as PDFLayoutBlock;
                Assert.IsNotNull(lsect);
                Assert.AreEqual(1, lsect.Columns.Length);
                Assert.AreEqual(2, lsect.Columns[0].Contents.Count);
                
                //Section Head
                var lsectHead = lsect.Columns[0].Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(lsectHead);
                Assert.AreEqual(25, lsectHead.Height);
                
                var lsectBody = lsect.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(lsectBody);
                Assert.AreEqual(15, lsectBody.Height);
                
                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);

            }


        }

    }
}
