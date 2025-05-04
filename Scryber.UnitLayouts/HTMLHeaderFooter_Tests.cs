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
                AssertLayoutDocument(ldoc, doc, 1);

                var lpg = ldoc.AllPages[0];

                var contentRegion = AssertPage(lpg, 0, false, false, 1);
                
                Assert.IsNotNull(contentRegion);
                
                Assert.AreEqual(1, contentRegion.Contents.Count);
                
                var lblock = contentRegion.Contents[0];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));


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
                
                AssertLayoutDocument(ldoc, doc, 2);
                
                var lpg = ldoc.AllPages[0];
                AssertPage(lpg, 0,false, false, 1);
                AssertBeforeSectionDivContent(lpg, 0);
                
                //page2
                
                lpg = ldoc.AllPages[1];
                AssertPage(lpg, 1,false, false, 2); //section and div
                
                //Section forces a new page
                var lsection = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                AssertSection(lsection, 0, false, false, 1);

                AssertAfterSectionDivContent(lpg, 1);

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
                AssertLayoutDocument(ldoc, doc, 1);

                var lpg = ldoc.AllPages[0];
                AssertPage(lpg, 0,false, false, 3);
                AssertBeforeSectionDivContent(lpg, 0);
                
                var lsection = lpg.ContentBlock.Columns[0].Contents[1] as PDFLayoutBlock;
                AssertSection(lsection, 1, false, false, 1);

                AssertAfterSectionDivContent(lpg, 2);
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
                AssertLayoutDocument(ldoc, doc, 2);

                var lpg = ldoc.AllPages[0];
                AssertPage(lpg, 0,true, false, 1);
                AssertPageHeaderContent(lpg, false);
                AssertBeforeSectionDivContent(lpg, 0);
                
                lpg = ldoc.AllPages[1];
                AssertPage(lpg, 1,true, false, 2);
                AssertPageHeaderContent(lpg, false);
                
                var lsection = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                AssertSection(lsection, 0, false, false, 1);

                AssertAfterSectionDivContent(lpg, 1);
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
                AssertLayoutDocument(ldoc, doc, 2);

                var lpg = ldoc.AllPages[0];
                AssertPage(lpg, 0,true, true, 1);
                AssertPageHeaderContent(lpg, false);
                AssertPageFooterContent(lpg, false);
                AssertBeforeSectionDivContent(lpg, 0);
                
                lpg = ldoc.AllPages[1];
                AssertPage(lpg, 1,true, true, 2);
                AssertPageHeaderContent(lpg, false);
                AssertPageFooterContent(lpg, false);
                
                var lsection = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                AssertSection(lsection, 0, false, false, 1);

                AssertAfterSectionDivContent(lpg, 1);
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
                using (var stream =
                       DocStreams.GetOutputStream("HTMLBodyAndSection_06_BodyHeaderAndFooterWithContinuation.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }

                var ldoc = _layout;
                AssertLayoutDocument(ldoc, doc, 2);

                var lpg = ldoc.AllPages[0];
                AssertPage(lpg, 0,true, true, 1);
                AssertPageHeaderContent(lpg, false);
                AssertPageFooterContent(lpg, false);
                AssertBeforeSectionDivContent(lpg, 0);
                
                lpg = ldoc.AllPages[1];
                AssertPage(lpg, 1,true, true, 2);
                AssertPageHeaderContent(lpg, true);
                AssertPageFooterContent(lpg, true);
                
                var lsection = lpg.ContentBlock.Columns[0].Contents[0] as PDFLayoutBlock;
                AssertSection(lsection, 0, false, false, 1);

                AssertAfterSectionDivContent(lpg, 1);
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
                Assert.AreEqual(15 + 10 + 15,
                    lblock.Height); //One line, and a header with 5pt padding and a single row table
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
                Assert.AreEqual(10 + 15, lsectHead.Height);

                var lsectBody = lsect.Columns[0].Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(lsectBody);
                Assert.AreEqual(15, lsectBody.Height);

                //After the section

                lblock = lcol.Contents[1];
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);

            }


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_08_SectionHeaderSamePage()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_08_SectionHeaderSamePage");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_08_SectionHeaderSamePage.pdf"))
                {
                    doc.LayoutComplete += Doc_LayoutComplete;
                    doc.AppendTraceLog = true;
                    doc.SaveAsPDF(stream);
                }

                var ldoc = _layout;
                Assert.IsNotNull(ldoc);

                //By default all sections start on a new page.
                Assert.AreEqual(1, ldoc.AllPages.Count);

                var lpg = ldoc.AllPages[0];
                Assert.IsNotNull(lpg);

                //Header footer null
                Assert.IsNull(lpg.HeaderBlock);
                Assert.IsNull(lpg.FooterBlock);

                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(3, lcol.Contents.Count);

                //Div Before

                var lblock = lcol.Contents[0] as PDFLayoutBlock;


                //Section on same page with header

                lblock = lcol.Contents[1] as PDFLayoutBlock; //section block
                Assert.IsNotNull(lblock);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                Assert.AreEqual(40, lblock.Height); //header table + content
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.AreEqual(1, lblock.Columns.Length);
                var lcol1 = lblock.Columns[0];
                Assert.IsNotNull(lcol1);
                Assert.AreEqual(2, lcol1.Contents.Count); //header and content



                //Inside the section
                var lsectHead = lcol1.Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(lsectHead);
                Assert.IsInstanceOfType(lsectHead.Owner, typeof(ComponentHeader));
                Assert.AreEqual(1, lsectHead.Columns.Length);
                Assert.AreEqual(1, lsectHead.Columns[0].Contents.Count);


                var lsectBody = lcol1.Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(lsectBody);
                Assert.AreEqual(15, lsectBody.Height);

                //After the section

                lblock = lcol.Contents[2] as PDFLayoutBlock;
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));

            }


        }

        [TestCategory(TestCategoryName)]
        [TestMethod()]
        public void BodyAndSection_09_SectionAndBodyHeader()
        {
            var path = AssertGetContentFile("HTMLBodyAndSection_09_SectionAndBodyHeader");
            using (var doc = Document.ParseDocument(path))
            {
                doc.RenderOptions.Compression = OutputCompressionType.None;
                using (var stream = DocStreams.GetOutputStream("HTMLBodyAndSection_09_SectionAndBodyHeader.pdf"))
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

                //Footer is null
                Assert.IsNull(lpg.FooterBlock);


                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                var lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(1, lcol.Contents.Count);

                //Div Before

                var lblock = lcol.Contents[0] as PDFLayoutBlock;
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);

                //Section on next page with header
                lpg = ldoc.AllPages[1];

                //Check the page header
                Assert.IsNotNull(lpg.HeaderBlock);
                Assert.AreEqual(15, lpg.HeaderBlock.Height);
                Assert.AreEqual(lpg.Width - 20, lpg.HeaderBlock.Width);
                Assert.IsInstanceOfType(lpg.HeaderBlock.Owner, typeof(PDFPageHeader));

                //Footer is null
                Assert.IsNull(lpg.FooterBlock);

                Assert.IsNotNull(lpg.ContentBlock);
                Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
                lcol = lpg.ContentBlock.Columns[0];
                Assert.IsNotNull(lcol);
                Assert.AreEqual(2, lcol.Contents.Count); //section and div


                lblock = lcol.Contents[1] as PDFLayoutBlock; //section block
                Assert.IsNotNull(lblock);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLSection));
                Assert.AreEqual(40, lblock.Height); //header table + content
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.AreEqual(1, lblock.Columns.Length);
                var lcol1 = lblock.Columns[0];
                Assert.IsNotNull(lcol1);
                Assert.AreEqual(2, lcol1.Contents.Count); //header and content



                //Inside the section
                var lsectHead = lcol1.Contents[0] as PDFLayoutBlock;
                Assert.IsNotNull(lsectHead);
                Assert.IsInstanceOfType(lsectHead.Owner, typeof(ComponentHeader));
                Assert.AreEqual(1, lsectHead.Columns.Length);
                Assert.AreEqual(1, lsectHead.Columns[0].Contents.Count);


                var lsectBody = lcol1.Contents[1] as PDFLayoutBlock;
                Assert.IsNotNull(lsectBody);
                Assert.AreEqual(15, lsectBody.Height);

                //After the section

                lblock = lcol.Contents[2] as PDFLayoutBlock;
                Assert.IsNotNull(lblock);
                Assert.AreEqual(15, lblock.Height);
                Assert.AreEqual(lpg.ContentBlock.Width, lblock.Width);
                Assert.IsInstanceOfType(lblock.Owner, typeof(HTMLDiv));

            }


        }

        private static void AssertLayoutDocument(PDFLayoutDocument docLayout,  Document owner, int pageCount)
        {
            Assert.IsNotNull(docLayout);
            Assert.AreEqual(docLayout.Owner, owner);
            Assert.AreEqual(pageCount, docLayout.AllPages.Count, "The expected number of pages did not match the actual number of pages.");
        }
        
        /// <summary>
        /// Checks the page and if flagged and page headers and footers. Use the assertPageHeaders to check the actual content of them.
        /// </summary>
        /// <param name="lpg"></param>
        /// <param name="index"></param>
        /// <param name="hasHeader"></param>
        /// <param name="hasFooter"></param>
        /// <param name="contentCount"></param>
        /// <returns></returns>
        private static PDFLayoutRegion AssertPage(PDFLayoutPage lpg, int index, bool hasHeader, bool hasFooter, int contentCount)
        {
            Assert.IsNotNull(lpg);
            Assert.AreEqual(index, lpg.PageIndex);

            if (hasHeader)
                Assert.IsNotNull(lpg.HeaderBlock);
            else
            {
                Assert.IsNull(lpg.HeaderBlock);
            }

            if (hasFooter)
                Assert.IsNotNull(lpg.FooterBlock);
            else
                Assert.IsNull(lpg.FooterBlock);
            
            Assert.IsNotNull(lpg.ContentBlock);
            Assert.AreEqual(1, lpg.ContentBlock.Columns.Length);
            var region = lpg.ContentBlock.Columns[0];
            
            Assert.AreEqual(contentCount, region.Contents.Count);
            
            return region;
        }

        private static void AssertSectionTableHeaderContent(PDFLayoutBlock sectionBlock, bool isContinuation)
        {
            throw new NotImplementedException();
        }

        private static void AssertSectionTableFooterContent(PDFLayoutBlock sectionBlock, bool isContinuation)
        {
            throw new NotImplementedException();

        }

        private void AssertSection(PDFLayoutBlock section, int index, bool hasHeader, bool hasFooter, int contentCount)
        {
            Assert.IsNotNull(section);
            Assert.IsInstanceOfType(section.Owner, typeof(HTMLSection));
            int totalCount = contentCount;
            
            if (hasHeader)
            {
                totalCount += 1;
            }

            if (hasFooter)
            {
                totalCount += 1;
            }
            
            Assert.AreEqual(totalCount, section.Columns[0].Contents.Count);

        }

        private static void AssertPageHeaderContent(PDFLayoutPage page, bool isContinuation)
        {
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.HeaderBlock);
            
            if (!isContinuation)
            {
                Assert.AreEqual(15, page.HeaderBlock.Height);
                Assert.AreEqual(page.Width - 20, page.HeaderBlock.Width);
                Assert.AreEqual(1, page.HeaderBlock.Columns.Length);
                Assert.AreEqual(1, page.HeaderBlock.Columns[0].Contents.Count);
            }
            else //continuation header
            {
                Assert.AreEqual(30 + 20, page.HeaderBlock.Height);
                Assert.AreEqual(page.Width - 20, page.HeaderBlock.Width);
                Assert.AreEqual(1, page.HeaderBlock.Columns.Length);
                Assert.AreEqual(1, page.HeaderBlock.Columns[0].Contents.Count);
            }
        }

        private static void AssertPageFooterContent(PDFLayoutPage page, bool isContinuation)
        {
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.FooterBlock);

            if (!isContinuation)
            {

                Assert.AreEqual(30, page.FooterBlock.Height);
                Assert.AreEqual(page.Width - 20, page.HeaderBlock.Width);
                Assert.AreEqual(1, page.HeaderBlock.Columns.Length);
                Assert.AreEqual(1, page.HeaderBlock.Columns[0].Contents.Count);
            }
            else //continuation footer
            {
                Assert.AreEqual(45 + 20, page.FooterBlock.Height);
                Assert.AreEqual(page.Width - 20, page.FooterBlock.Width);
                Assert.AreEqual(1, page.FooterBlock.Columns.Length);
                Assert.AreEqual(1, page.FooterBlock.Columns[0].Contents.Count);
            }
        }


        private static void AssertBeforeSectionDivContent(PDFLayoutPage page, int expectedIndex)
        {
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ContentBlock);
            var before = page.ContentBlock.Columns[0].Contents[expectedIndex] as PDFLayoutBlock;
            Assert.IsNotNull(before);
            Assert.AreEqual(15, before.Height);
            Assert.AreEqual(page.ContentBlock.Width, before.Width);
            Assert.AreEqual(1, before.Columns.Length);
            Assert.AreEqual(1, before.Columns[0].Contents.Count);
            Assert.IsInstanceOfType(before.Owner, typeof(HTMLDiv));
        }
        
        private static void AssertAfterSectionDivContent(PDFLayoutPage page, int expectedIndex)
        {
            Assert.IsNotNull(page);
            Assert.IsNotNull(page.ContentBlock);
            var before = page.ContentBlock.Columns[0].Contents[expectedIndex] as PDFLayoutBlock;
            Assert.IsNotNull(before);
            Assert.AreEqual(15, before.Height);
            Assert.AreEqual(page.ContentBlock.Width, before.Width);
            Assert.AreEqual(1, before.Columns.Length);
            Assert.AreEqual(1, before.Columns[0].Contents.Count);
            Assert.IsInstanceOfType(before.Owner, typeof(HTMLDiv));
        }


}
}
