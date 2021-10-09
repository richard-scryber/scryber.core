using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.PDF.Layout;
using Scryber.PDF;
using LD = Scryber.PDF.Layout.PDFLayoutDocument;
using System.Runtime.InteropServices;
using System.IO;

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
            Document doc = new Document();
            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                Page pg = new Page();
                doc.Pages.Add(pg);
                PageNumberLabel lbl = new PageNumberLabel();
                pg.Contents.Add(lbl);
            }

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.SaveAsPDF(ms);
            }

            for (int i = 0; i < totalcount; i++)
            {
                int pgNum = i + 1;
                PDFLayoutPage lpg = layout.AllPages[i];

                PageNumberData data = lpg.GetPageNumber();

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
            Document doc = new Document();

            //Add a catch all number style definition for upper letter with Prefix
            StyleDefn pgNumStyle = new StyleDefn();
            pgNumStyle.AppliedType = typeof(Document);

            pgNumStyle.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
            doc.Styles.Add(pgNumStyle);


            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                Page pg = new Page();
                doc.Pages.Add(pg);
                PageNumberLabel lbl = new PageNumberLabel();
                pg.Contents.Add(lbl);
            }

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.SaveAsPDF(ms);
            }

            //Check the numbers
            for (int i = 0; i < totalcount; i++)
            {
                int pgNum = i + 1;
                PDFLayoutPage lpg = layout.AllPages[i];

                PageNumberData data = lpg.GetPageNumber();

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
            Document doc = new Document();
            int totalcount = 10;

            //Add ten pages with default numbering
            for (int i = 0; i < totalcount; i++)
            {
                if (i == 5)
                {
                    Section section = new Section();
                    section.PageNumberStyle = PageNumberStyle.UppercaseLetters;
                    
                    doc.Pages.Add(section);

                    for (int j = 0; j < 4; j++) //4 page breaks = 5 pages
                    {
                        PageBreak br = new PageBreak();
                        section.Contents.Add(br);
                    }
                }
                else
                {
                    Page pg = new Page();
                    doc.Pages.Add(pg);
                    PageNumberLabel lbl = new PageNumberLabel();
                    pg.Contents.Add(lbl);
                }
            }

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.SaveAsPDF(ms);
            }
            string[] allpages = new string[] { "1", "2", "3", "4", "5", "A", "B", "C", "D", "E", "6", "7", "8", "9" };
            string[] grptotal = new string[] { "5", "5", "5", "5", "5", "E", "E", "E", "E", "E", "9", "9", "9", "9" };

            for (int i = 0; i < allpages.Length; i++)
            {
                string expected = string.Format("Page {0} of {1} ({2} of {3})", allpages[i], grptotal[i], i + 1, allpages.Length);

                PDFLayoutPage lpg = layout.AllPages[i];
                PageNumberData data = lpg.GetPageNumber();

                string actual = data.ToString("Page {0} of {1} ({2} of {3})");
                Assert.AreEqual(expected, actual);
                TestContext.WriteLine("Expected '{0}', Actual '{1}'", expected, actual);

            }
        }


        [TestMethod()]
        [TestCategory("Page Numbers")]
        public void SinglePage_Numbered_Test()
        {
            Document doc = new Document();
            Page pg = new Page();
            doc.Pages.Add(pg);
            //First page with 
            pg.Style.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;

            PageNumberLabel lbl = new PageNumberLabel();
            pg.Contents.Add(lbl);

            // second page - no style so back to default.
            pg = new Page();
            doc.Pages.Add(pg);
            lbl = new PageNumberLabel();
            pg.Contents.Add(lbl);

            // second page - continues default.
            pg = new Page();
            doc.Pages.Add(pg);
            lbl = new PageNumberLabel();
            pg.Contents.Add(lbl);

            
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.SaveAsPDF(ms);
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

            Document doc = new Document();

            //catch all style will be applied to the document
            StyleDefn catchall = new StyleDefn();
            catchall.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            catchall.AppliedType = typeof(Document);
            doc.Styles.Add(catchall);

            //style for the page group
            StyleDefn pgGrpStyle = new StyleDefn();
            pgGrpStyle.AppliedType = typeof(PageGroup);
            pgGrpStyle.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            doc.Styles.Add(pgGrpStyle);

            //style for the section
            StyleDefn sectStyle = new StyleDefn();
            sectStyle.AppliedType = typeof(Section);
            sectStyle.PageStyle.NumberStyle = PageNumberStyle.LowercaseLetters;
            sectStyle.PageStyle.NumberStartIndex = 5;
            doc.Styles.Add(sectStyle);

            //build the document pages to match

            //empty title page

            Page title = new Page();
            title.Style.PageStyle.NumberStyle = PageNumberStyle.None;
            doc.Pages.Add(title);

            // 2 lower roman from the document style

            Page pi = new Page();
            doc.Pages.Add(pi);

            Page pii = new Page();
            doc.Pages.Add(pii);

            //page group with 3 pages of decimals

            PageGroup grp = new PageGroup();
            doc.Pages.Add(grp);

            Page pg1 = new Page();
            grp.Pages.Add(pg1);

            Page pg2 = new Page();
            grp.Pages.Add(pg2);

            Page pg3 = new Page();
            grp.Pages.Add(pg3);

            // section of lower alpha with 3 pages of content

            Section sect = new Section();
            grp.Pages.Add(sect);
            sect.Contents.Add(new PageBreak());
            sect.Contents.Add(new PageBreak());

            // final page in the group
            Page pg4 = new Page();
            grp.Pages.Add(pg4);


            

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                doc.LayoutComplete += Doc_LayoutCompleted;
                doc.SaveAsPDF(ms);
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
        public void TestingNumberOutputFormat()
        {
            var src = @"<?xml version='1.0' encoding='utf-8' ?>

<doc:Document xmlns:doc='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Components.xsd'
              xmlns:styles='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Styles.xsd'
              xmlns:data='http://www.scryber.co.uk/schemas/core/release/v1/Scryber.Data.xsd' >
  <Render-Options compression-type='None' string-output='Text' />
  <Styles>
    <styles:Style applied-class='pg-num' >
      <styles:Padding all='20pt'/>
      <styles:Font size='60pt' family='Helvetica'/>
      <styles:Page display-format='Page {0} of {1}'/>
    </styles:Style>

    <styles:Style applied-class='intro' >
      <styles:Page number-style='LowercaseRoman'/>
    </styles:Style>

    <styles:Style applied-class='appendix' >
      <styles:Page display-format='Appendix {0}'  number-style='UppercaseLetters' number-start-index='1' />
    </styles:Style>
  </Styles>
  
  <Pages>

    <doc:Section styles:class='pg-num intro'>
      <Content>
        <doc:Div>Introductions with lowercase roman</doc:Div>
        <!-- Page 1 -->
        <doc:PageNumber id='IntroNumber1'/>
        <doc:PageBreak/>
        <!-- Page 2 -->
        <doc:PageNumber />
        <doc:PageBreak />
        <!-- Page 3 -->
        <doc:PageNumber id='IntroNumber3'/>
      </Content>
    </doc:Section>

    <doc:Section styles:class='pg-num' styles:page-number-start-index='1' >
      <Content>
        <doc:Div>These are the page numbers shown on each of the pages</doc:Div>
        <!-- Page 1 -->
        <doc:PageNumber id='StandardNumber' />
        <doc:PageBreak/>
        <!-- Page 2 -->
        <doc:PageNumber />
        <doc:PageBreak />
        <!-- Page 3 -->
        <doc:Div>With a different format</doc:Div>
        <doc:PageNumber id='ExplicitPageNum' styles:display-format='Page {0} of {1} (Total {2} of {3})' />
      </Content>
    </doc:Section>

    <doc:Section styles:class='pg-num appendix'>
      <Content>
        <doc:Div>The appendix style has upper case letters with a formatted value to show the current appendix letter.</doc:Div>
        <!-- Page 4 -->
        <doc:PageNumber id='AppendixNumber1' />
        <doc:PageBreak />
        <!-- Page 5 -->
        <doc:PageNumber id='AppendixNumber2' />
      </Content>
    </doc:Section>

  </Pages>
  
</doc:Document>";

            var sr = new System.IO.StringReader(src);
            var doc = Document.ParseDocument(sr, ParseSourceType.DynamicContent);

            using (var ms = new MemoryStream())
            {
                doc.SaveAsPDF(ms);

                //First intro
                var lbl = doc.FindAComponentById("IntroNumber1") as PageNumberLabel;
                Assert.AreEqual("Page i of iii", lbl.Proxy.Text, "Intro1 failed");

                //3rd intro
                lbl = doc.FindAComponentById("IntroNumber3") as PageNumberLabel;
                Assert.AreEqual("Page iii of iii", lbl.Proxy.Text, "Intro 3 failed");

                //Normal pages
                lbl = doc.FindAComponentById("StandardNumber") as PageNumberLabel;
                Assert.AreEqual("Page 1 of 3", lbl.Proxy.Text, "Normal Number failed");

                //Explicit page number format
                lbl = doc.FindAComponentById("ExplicitPageNum") as PageNumberLabel;
                Assert.AreEqual("Page 3 of 3 (Total 6 of 8)", lbl.Proxy.Text, "Explicit number failed");
            }
        }
    }
}
