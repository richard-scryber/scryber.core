using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Layout;
using LD = Scryber.Layout.PDFLayoutDocument;

namespace Scryber.Core.UnitTests.Layout
{

    [TestClass()]
    public class PDFPage_Numbers_Test
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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


        private LD layout;

        private void Doc_LayoutCompleted(object sender, PDFLayoutEventArgs args)
        {
            layout = args.Context.DocumentLayout;
        }


        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void Default_Numbered_Test()
        {
            PDFDocument doc = new PDFDocument();
            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                PDFPage pg = new PDFPage();
                doc.Pages.Add(pg);
                PDFPageNumberLabel lbl = new PDFPageNumberLabel();
                pg.Contents.Add(lbl);
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.ProcessDocument(ms);
            }

            for (int i = 0; i < totalcount; i++)
            {
                int pgNum = i + 1;
                PDFLayoutPage lpg = layout.AllPages[i];

                PDFPageNumberData data = lpg.GetPageNumber();

                //Check the number
                Assert.AreEqual(pgNum, data.PageNumber);
                Assert.AreEqual(totalcount, data.LastPageNumber);
                Assert.AreEqual(pgNum, data.GroupNumber);
                Assert.AreEqual(totalcount, data.GroupLastNumber);
                Assert.AreEqual(pgNum.ToString(), data.ToString());
            }
        }

        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void DocumentPrefixUpperAlpha_Numbered_Test()
        {
            PDFDocument doc = new PDFDocument();

            //Add a catch all number style definition for upper letter with Prefix
            PDFStyleDefn pgNumStyle = new PDFStyleDefn();
            pgNumStyle.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
            doc.Styles.Add(pgNumStyle);


            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                PDFPage pg = new PDFPage();
                doc.Pages.Add(pg);
                PDFPageNumberLabel lbl = new PDFPageNumberLabel();
                pg.Contents.Add(lbl);
            }

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.ProcessDocument(ms);
            }

            //Check the numbers
            for (int i = 0; i < totalcount; i++)
            {
                int pgNum = i + 1;
                PDFLayoutPage lpg = layout.AllPages[i];

                PDFPageNumberData data = lpg.GetPageNumber();

                //Check the number
                Assert.AreEqual(pgNum, data.PageNumber, "Page Number failed");
                Assert.AreEqual(totalcount, data.LastPageNumber, "Last Page number failed");
                Assert.AreEqual(pgNum, data.GroupNumber, "Group number failed");
                Assert.AreEqual(totalcount, data.GroupLastNumber, "Last Group number failed");

                //Should be upper letter with hash prefix
                string output = ((char)((int)'A' + i)).ToString();
                Assert.AreEqual(output, data.ToString(), "String result failed");
            }
        }

        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void DefaultWithSection_Numbered_Test()
        {
            PDFDocument doc = new PDFDocument();
            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                if (i == 5)
                {
                    PDFSection section = new PDFSection();
                    section.PageNumberStyle = PageNumberStyle.UppercaseLetters;
                    
                    doc.Pages.Add(section);

                    for (int j = 0; j < 4; j++) //4 page breaks = 5 pages
                    {
                        PDFPageBreak br = new PDFPageBreak();
                        section.Contents.Add(br);
                    }
                }
                else
                {
                    PDFPage pg = new PDFPage();
                    doc.Pages.Add(pg);
                    PDFPageNumberLabel lbl = new PDFPageNumberLabel();
                    pg.Contents.Add(lbl);
                }
            }

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.ProcessDocument(ms);
            }
            string[] allpages = new string[] { "1", "2", "3", "4", "5", "A", "B", "C", "D", "E", "6", "7", "8", "9" };
            string[] grptotal = new string[] { "5", "5", "5", "5", "5", "E", "E", "E", "E", "E", "9", "9", "9", "9" };

            for (int i = 0; i < allpages.Length; i++)
            {
                string expected = string.Format("Page {0} of {1} ({2} of {3})", allpages[i], grptotal[i], i + 1, allpages.Length);

                PDFLayoutPage lpg = layout.AllPages[i];
                PDFPageNumberData data = lpg.GetPageNumber();

                string actual = data.ToString("Page {0} of {1} ({2} of {3})");
                Assert.AreEqual(expected, actual);
                TestContext.WriteLine("Expected '{0}', Actual '{1}'", expected, actual);

            }
        }


        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void SinglePage_Numbered_Test()
        {
            PDFDocument doc = new PDFDocument();
            PDFPage pg = new PDFPage();
            doc.Pages.Add(pg);
            //First page with 
            pg.Style.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;

            PDFPageNumberLabel lbl = new PDFPageNumberLabel();
            pg.Contents.Add(lbl);

            // second page - no style so back to default.
            pg = new PDFPage();
            doc.Pages.Add(pg);
            lbl = new PDFPageNumberLabel();
            pg.Contents.Add(lbl);

            // second page - continues default.
            pg = new PDFPage();
            doc.Pages.Add(pg);
            lbl = new PDFPageNumberLabel();
            pg.Contents.Add(lbl);

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.ProcessDocument(ms);
            }

            //check page 1
            PDFLayoutLine line = layout.AllPages[0].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine; //first line of the first region, of the first group of the first page
            PDFTextRunProxy chars = (PDFTextRunProxy)line.Runs[1]; //Start text, Chars, End Text
            Assert.AreEqual("A", chars.Proxy.Text);

            //check page 2
            line = layout.AllPages[1].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine; //first line of the first region, of the first group of the second page
            chars = (PDFTextRunProxy)line.Runs[1]; //Start text, Chars, End Text
            Assert.AreEqual("1", chars.Proxy.Text);

            //check page 3
            line = layout.AllPages[2].ContentBlock.Columns[0].Contents[0] as PDFLayoutLine; //first line of the first region, of the first group of the second page
            chars = (PDFTextRunProxy)line.Runs[1]; //Start text, Chars, End Text
            Assert.AreEqual("2", chars.Proxy.Text);
        }

        

        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void DocumentStyle_WithTitle_PageGroup_AndSection_Test()
        {
            // Blank title page, followed by 2 pages with lower roman, 3 pages in the page group, 
            // then inner section of lower alpha and back to a page group single page.

            string[] pgNums = new string[] { "", "i", "ii", "1", "2", "3", "e", "f", "g", "4" };

            //set up the styles

            PDFDocument doc = new PDFDocument();

            //catch all style will be applied to the document
            PDFStyleDefn catchall = new PDFStyleDefn();
            catchall.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            doc.Styles.Add(catchall);

            //style for the page group
            PDFStyleDefn pgGrpStyle = new PDFStyleDefn();
            pgGrpStyle.AppliedType = typeof(PDFPageGroup);
            pgGrpStyle.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            doc.Styles.Add(pgGrpStyle);

            //style for the section
            PDFStyleDefn sectStyle = new PDFStyleDefn();
            sectStyle.AppliedType = typeof(PDFSection);
            sectStyle.PageStyle.NumberStyle = PageNumberStyle.LowercaseLetters;
            sectStyle.PageStyle.NumberStartIndex = 5;
            doc.Styles.Add(sectStyle);

            //build the document pages to match

            //empty title page

            PDFPage title = new PDFPage();
            title.Style.PageStyle.NumberStyle = PageNumberStyle.None;
            doc.Pages.Add(title);

            // 2 lower roman from the document style

            PDFPage pi = new PDFPage();
            doc.Pages.Add(pi);

            PDFPage pii = new PDFPage();
            doc.Pages.Add(pii);

            //page group with 3 pages of decimals

            PDFPageGroup grp = new PDFPageGroup();
            doc.Pages.Add(grp);

            PDFPage pg1 = new PDFPage();
            grp.Pages.Add(pg1);

            PDFPage pg2 = new PDFPage();
            grp.Pages.Add(pg2);

            PDFPage pg3 = new PDFPage();
            grp.Pages.Add(pg3);

            // section of lower alpha with 3 pages of content

            PDFSection sect = new PDFSection();
            grp.Pages.Add(sect);
            sect.Contents.Add(new PDFPageBreak());
            sect.Contents.Add(new PDFPageBreak());

            // final page in the group
            PDFPage pg4 = new PDFPage();
            grp.Pages.Add(pg4);


            

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.ProcessDocument(ms);
            }

            Assert.AreEqual(layout.AllPages.Count, pgNums.Length);

            for (int i = 0; i < pgNums.Length; i++)
            {
                PDFLayoutPage lpg = layout.AllPages[i];

                string expected = pgNums[i];
                string actual = lpg.GetPageNumber().ToString();

                Assert.AreEqual(expected, actual);
                TestContext.WriteLine("Expected '{0}', Actual '{1}'", expected, actual);
            }
        }

        [TestMethod()]
        public void TestingNumberPrefixLayou()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>
<pdf:Document xmlns:pdf='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
  <Render-Options compression-type='None' string-output='Text' />
  <Styles>
    <styles:Style applied-class='pg-num1' >
      <styles:Padding all='20pt'/>
      <styles:Font size='100pt'/>
    </styles:Style>

    <styles:Style applied-type='pdf:Section' >
      <styles:Page display-format='Page {0}' number-start-index='1' />
    </styles:Style>

    <styles:Style applied-class='appendix' >
      <styles:Page display-format='Appendix {0}' number-style='UppercaseLetters' number-start-index='1' />
    </styles:Style>
  </Styles>
  
  <Pages>
    <pdf:Page styles:class='pg-num1'>
      <Content>
        <!-- Page 1 -->
        This is the Page content of <pdf:PageNumber />
      </Content>
    </pdf:Page>

    <pdf:Section styles:class='pg-num1'>
      <Content>
        <!-- Page 2 -->
        This is the content of <pdf:PageNumber />
        <pdf:PageBreak/>
        <!-- Page 3 -->
        This is the content of <pdf:PageNumber />
        <pdf:PageBreak />
        <!-- Page 4 -->
        This is the content of <pdf:PageNumber />
      </Content>
    </pdf:Section>

    <pdf:Section styles:class='pg-num1 appendix'>
      <Content>
        <!-- Page 5 -->
        This is the Appendix content of <pdf:PageNumber />
        <pdf:PageBreak />
        <!-- Page 6 -->
        This is the Appendix content of <pdf:PageNumber />
      </Content>
    </pdf:Section>
  </Pages>
  
</pdf:Document>";

            var ms = new System.IO.StringReader(src);
            var doc = PDFDocument.ParseDocument(ms, ParseSourceType.DynamicContent);

            var path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.Combine(path, "PageNumberTest.pdf");

            doc.ProcessDocument(path, System.IO.FileMode.OpenOrCreate);

            Assert.IsTrue(System.IO.File.Exists(path));
        }
    }
}
