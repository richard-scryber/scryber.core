﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.PDF.Layout;
using Scryber.PDF;
using Scryber.Styles;

namespace Scryber.Core.UnitTests.Numbers
{
    [TestClass()]
    public class Page_NumberRegistration_Test
    {
        private PDFLayoutDocument _layout;

        private void Doc_LayoutComplete(object sender, LayoutEventArgs args)
        {
            var context = (PDFLayoutContext)(args.Context);
            this._layout = context.DocumentLayout;
        }

        /// <summary>
        /// Tests the PDFPageNumbering returned from the layout document GetPageNumber(int index) method for a simple document without any explicit styling.
        /// </summary>
        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void Numbering_SimpleDocument()
        {


            Document doc = new Document();

            

            for (int i = 0; i < 20; i++)
            {
                Page pg = new Page();
                doc.Pages.Add(pg);
            }


            doc.ViewPreferences.PageDisplay = PageDisplayMode.Thumbnails;
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var ms = new System.IO.MemoryStream())
                doc.SaveAsPDF(ms);
            var ldoc = this._layout;

            for (int i = 0; i < 20; i++)
            {
                PageNumberData num = ldoc.GetNumbering(i);
                Assert.AreEqual(i + 1, num.GroupNumber);
                Assert.AreEqual(i + 1, num.PageNumber);
                Assert.AreEqual(20, num.LastPageNumber);
                Assert.AreEqual(20, num.GroupLastNumber);
                Assert.AreEqual((i + 1).ToString(), num.Label);
                Assert.AreEqual("20", num.LastLabel);
            }

            int last = -1;

            foreach (PDFLayoutPage pg in ldoc.AllPages)
            {
                int index = pg.PageIndex;
                Assert.AreEqual(last + 1, index);
                last = index;
                PageNumberData num = pg.GetPageNumber();

                Assert.AreEqual(index + 1, num.GroupNumber);
                Assert.AreEqual(index + 1, num.PageNumber);
                Assert.AreEqual(20, num.LastPageNumber);
                Assert.AreEqual(20, num.GroupLastNumber);
                Assert.AreEqual((index + 1).ToString(), num.Label);
                Assert.AreEqual("20", num.LastLabel);
            }

        }

        

        /// <summary>
        /// Tests the PDFPageNumbering returned from the layout page GetPageNumber() method for a simple document without any explicit styling.
        /// </summary>
        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageNumbering_SimpleDocument()
        {
            Document doc = new Document();


            for (int i = 0; i < 20; i++)
            {
                Page pg = new Page();
                doc.Pages.Add(pg);
            }

            doc.ViewPreferences.PageDisplay = PageDisplayMode.Thumbnails;
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var ms = DocStreams.GetOutputStream("Numbering_Simple.pdf"))
                doc.SaveAsPDF(ms);
            var ldoc = this._layout;
            int last = -1;

            foreach (PDFLayoutPage pg in ldoc.AllPages)
            {
                int index = pg.PageIndex;
                Assert.AreEqual(last + 1, index);
                last = index;

                //Get the page number details for the page
                PageNumberData num = pg.GetPageNumber();

                Assert.AreEqual(index + 1, num.GroupNumber, "Group number of page was not correct");
                Assert.AreEqual(index + 1, num.PageNumber, "Page number was not correct");
                Assert.AreEqual(20, num.LastPageNumber, "Last page number was not correct");
                Assert.AreEqual(20, num.GroupLastNumber, "Group last number was not correct");
                Assert.AreEqual((index + 1).ToString(), num.Label, "Page label was not correct");
                Assert.AreEqual("20", num.LastLabel, "Last page label was not correct");
            }

        }

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageNumbering_MultiSectionDocument()
        {
            Document doc = new Document();


            // numbering                           | Default                 | Lower roman                | Upper letter with prefix        | Back to default
            // page indices                          0,    1,   2,   3,   4,   5,   6,     7,    8,    9,   10,    11 ,   12,    13,    14,   15,  16,  17,  18,   19
            string[] expectedlabels = new string[] { "1", "2", "3", "4", "5", "i", "ii", "iii", "iv", "v", "B", "C", "D", "E", "F", "6", "7", "8", "9", "10" };

            for (int i = 0; i < 4; i++)
            {
                Section group = new Section();
                if (i == 1)
                {
                    group.Style.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
                    group.Style.PageStyle.NumberStartIndex = 1;
                }
                else if (i == 2)
                {
                    group.Style.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
                    group.Style.PageStyle.NumberStartIndex = 2;
                }
                doc.Pages.Add(group);

                for (int j = 0; j < 4; j++)
                {
                    PageBreak br = new PageBreak();
                    group.Contents.Add(br);
                }
            }


            doc.ViewPreferences.PageDisplay = PageDisplayMode.Thumbnails;
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var ms = DocStreams.GetOutputStream("Numbering_MultiSection.pdf"))
                doc.SaveAsPDF(ms);
            var ldoc = this._layout;
            int index = -1;

            List<string> actuallabels = new List<string>();

            Assert.AreEqual(20, ldoc.AllPages.Count);
            

            int last = -1;
            foreach (PDFLayoutPage pg in ldoc.AllPages)
            {
                index = pg.PageIndex;
                Assert.AreEqual(last + 1, index, "Page numbers are not in synch");
                last = index;

                //Get the page number details for the page
                PageNumberData num = pg.GetPageNumber();
                actuallabels.Add(num.Label);

                if (index < 5)
                {
                    Assert.AreEqual(index + 1, num.GroupNumber, "First block page group number was wrong");
                    Assert.AreEqual(index + 1, num.PageNumber, "First block Global page number was wrong");
                    Assert.AreEqual(20, num.LastPageNumber, "First block last page number was wrong");
                    Assert.AreEqual(5, num.GroupLastNumber, "First block last group number was wrong");
                    Assert.AreEqual(expectedlabels[index], num.Label, "First block page label was wrong");
                    Assert.AreEqual("5", num.LastLabel, "First block last label was wrong");
                }
                else if (index < 10)
                {
                    Assert.AreEqual(index + 1, num.PageNumber, "First block page group number was wrong");
                    Assert.AreEqual(20, num.LastPageNumber, "First block Global page number was wrong");

                    index -= 5;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                    Assert.AreEqual(Scryber.Utilities.NumberHelper.GetRomanLower(index + 1), num.Label);
                    Assert.AreEqual("v", num.LastLabel);
                }
                else if (index < 15)
                {
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);
                    index -= 10;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                    Assert.AreEqual(Scryber.Utilities.NumberHelper.GetLetterUpper(index + 2), num.Label);
                    Assert.AreEqual("F", num.LastLabel);
                }
                else
                {
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);

                    //group is one based from the start of that sequence
                    index -= 15;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);

                    //labels are based from the start of that entire sequence
                    index += 5;
                    Assert.AreEqual((index + 1).ToString(), num.Label);
                    Assert.AreEqual("10", num.LastLabel);
                }
            }
            string fullexpected = string.Join(", ", expectedlabels);
            string fullactual = string.Join(", ", actuallabels);

            Assert.AreEqual(fullexpected, fullactual);

            //Check the page registrations

            // numbering                           | Default                 | Lower roman                | Upper letter with prefix        | Back to default
            // page indices                          0,    1,   2,   3,   4,   5,   6,     7,    8,    9,   10,    11 ,   12,    13,    14,   15,  16,  17,  18,   19

            PageNumbers nums = ldoc.Numbers;

            Assert.AreEqual(4, nums.Registrations.Count);

            //Default 1-4
            PageNumberRegistration reg = nums.Registrations[0];
            Assert.AreEqual(0, reg.FirstPageIndex);
            Assert.AreEqual(4, reg.LastPageIndex);
            Assert.AreEqual(0, reg.PreviousLinkedRegistrationPageCount);
            Assert.AreEqual(true, reg.IsClosed);

            //Lower Roman 5-9
            reg = nums.Registrations[1];
            Assert.AreEqual(5, reg.FirstPageIndex);
            Assert.AreEqual(9, reg.LastPageIndex);
            Assert.AreEqual(0, reg.PreviousLinkedRegistrationPageCount);
            Assert.AreEqual(true, reg.IsClosed);

            //Lower Upper Letter with prefix 10-14
            reg = nums.Registrations[2];
            Assert.AreEqual(10, reg.FirstPageIndex);
            Assert.AreEqual(14, reg.LastPageIndex);
            Assert.AreEqual(0, reg.PreviousLinkedRegistrationPageCount);
            Assert.AreEqual(true, reg.IsClosed);

            //Lower Default 15-19
            reg = nums.Registrations[3];
            Assert.AreEqual(15, reg.FirstPageIndex);
            Assert.AreEqual(19, reg.LastPageIndex);
            Assert.AreEqual(5, reg.PreviousLinkedRegistrationPageCount); //restarting from the last default onto this page
            Assert.AreEqual(true, reg.IsClosed);

        }


        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageNumbering_FullStyledMultiSectionDocument()
        {
            Document doc = new Document();


            for (int i = 0; i < 4; i++)
            {
                Section group = new Section();
                if (i == 0)
                {
                    group.Style.PageStyle.NumberStyle = PageNumberStyle.LowercaseLetters;
                }
                if (i == 1)
                {
                    group.Style.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
                    group.Style.PageStyle.NumberStartIndex = 1;
                }
                else if (i == 2)
                {
                    group.Style.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
                    group.Style.PageStyle.NumberStartIndex = 2;

                }
                doc.Pages.Add(group);

                for (int j = 0; j < 4; j++)
                {
                    PageNumberLabel pgNum = new PageNumberLabel();
                    group.Contents.Add(pgNum);

                    PageBreak br = new PageBreak();
                    group.Contents.Add(br);
                }
            }


            doc.ViewPreferences.PageDisplay = PageDisplayMode.Thumbnails;
            doc.LayoutComplete += Doc_LayoutComplete;

            using (var ms = DocStreams.GetOutputStream("Numbering_StyledMultiSection.pdf"))
                doc.SaveAsPDF(ms);
            var ldoc = this._layout;

            // page indices                          0,    1,   2,   3,   4,   5,   6,    7,    8,    9,   10,  11 , 12,  13,  14,  15,  16,  17,  18,   19
            string[] expectedlabels = new string[] { "a", "b", "c", "d", "e", "i", "ii", "iii", "iv", "v", "B", "C", "D", "E", "F", "1", "2", "3", "4", "5" };

            int index = -1;

            List<string> actuallabels = new List<string>();

            Assert.AreEqual(20, ldoc.AllPages.Count);
            int last = -1;
            foreach (PDFLayoutPage pg in ldoc.AllPages)
            {
                index = pg.PageIndex;
                Assert.AreEqual(last + 1, index, "Page numbers are not in synch");
                last = index;

                //Get the page number details for the page
                PageNumberData num = pg.GetPageNumber();
                actuallabels.Add(num.Label);

                if (index < 5)
                {
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                    Assert.AreEqual(expectedlabels[index], num.Label);
                    Assert.AreEqual("e", num.LastLabel);
                }
                else if (index < 10)
                {
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);

                    index -= 5;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                    Assert.AreEqual(Scryber.Utilities.NumberHelper.GetRomanLower(index + 1), num.Label);
                    Assert.AreEqual("v", num.LastLabel);
                }
                else if (index < 15)
                {
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);
                    index -= 10;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                    Assert.AreEqual(Scryber.Utilities.NumberHelper.GetLetterUpper(index + 2), num.Label);
                    Assert.AreEqual("F", num.LastLabel);
                }
                else
                {
                    Assert.AreEqual(index + 1, num.PageNumber);
                    Assert.AreEqual(20, num.LastPageNumber);

                    //labels are based from the continuation of the sequence
                    Assert.AreEqual(expectedlabels[index], num.Label);
                    Assert.AreEqual("5", num.LastLabel);

                    //group is one based from the start of that sequence
                    index -= 15;
                    Assert.AreEqual(index + 1, num.GroupNumber);
                    Assert.AreEqual(5, num.GroupLastNumber);
                }
            }
            string fullexpected = string.Join(", ", expectedlabels);
            string fullactual = string.Join(", ", actuallabels);

            Assert.AreEqual(fullexpected, fullactual);
        }
        
    }
}
