using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Components;
using Scryber.Styles;
using Scryber.Layout;
using LD = Scryber.Layout.PDFLayoutDocument;

namespace Scryber.Core.UnitTests.Layout
{
    /// <summary>
    /// A set of tests for the PDFPageNumberingCollection, PDFPageNumberGroup and PDFPageStyle
    /// Specifically around the registation of pages and their page numbering styles so that
    /// we know the format of page numbers and labels is correct.
    /// </summary>
    [TestClass()]
    public class PDFPageNumbering_Test
    {


        //
        // page numbering tests
        //

        [TestMethod]
        [TestCategory("Page Numbering")]
        public void PageRegistrations()
        {

            PDFPageNumberGroup grp = new PDFPageNumberGroup(null, "", PageNumberStyle.Decimals, 1, "");
            PDFPageNumberRegistration reg = new PDFPageNumberRegistration(0, 10, grp);

            for (int i = 0; i < 10; i++)
            {
                string pg = (i + 1).ToString();
                Assert.AreEqual(pg, reg.GetPageLabel(i), "Block 1: Page index " + i + " does not match " + pg);
            }


            grp = new PDFPageNumberGroup(null, "", PageNumberStyle.LowercaseLetters, 1, "");
            reg = new PDFPageNumberRegistration(0, 10, grp);

            for (int i = 0; i < 10; i++)
            {
                char value = 'a';
                value =  (char)((int)value + i);
                string pg = value.ToString();

                Assert.AreEqual(pg, reg.GetPageLabel(i), "Block 2: Page index " + i + " does not match " + pg);
            }

            grp = new PDFPageNumberGroup(null, "", PageNumberStyle.Decimals, 1, "");
            reg = new PDFPageNumberRegistration(0, 10, grp);
            reg.PreviousLinkedRegistrationPageCount = 5;

            for (int i = 0; i < 10; i++)
            {
                string pg = (i + 1 + 5).ToString();
                Assert.AreEqual(pg, reg.GetPageLabel(i));
            }

            grp = new PDFPageNumberGroup(null, "", PageNumberStyle.LowercaseLetters, 1, "");
            reg = new PDFPageNumberRegistration(0, 10, grp);
            reg.PreviousLinkedRegistrationPageCount = 6;

            for (int i = 0; i < 10; i++)
            {
                char value = 'a';
                value = (char)((int)value + i + 6);
                string pg = value.ToString();

                Assert.AreEqual(pg, reg.GetPageLabel(i), "Block 3: Page index " + i + " does not match " + pg);
            }

            // A registration of UpperCase letter from pg 15 to 25
            // with a previous count of 6 pages
            grp = new PDFPageNumberGroup(null, "", PageNumberStyle.UppercaseLetters, 1, "");
            reg = new PDFPageNumberRegistration(15, 25, grp);
            reg.PreviousLinkedRegistrationPageCount = 6;

            for (int i = 0; i < 10; i++)
            {
                char value = 'A';
                value = (char)((int)value + i + 6);
                string pg = value.ToString();

                Assert.AreEqual(pg, reg.GetPageLabel(i + 15), "Block 4: Page index " + i + " does not match " + pg);
            }
        }


        #region public void PageEmptyCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageEmptyCollection()
        {
            int repeatcount = 20;
            //index                0    1    2    3    4    5    6    7    8    9     10    11    12    13    14    15    16    17    18    19
            string[] expected = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                
                Assert.AreEqual(expected[i], actual);
            }
            
        }

        #endregion

        #region public void PageNullStyleCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageNullStyleCollection()
        {
            int repeatcount = 20;
            //index                0    1    2    3    4    5    6    7    8    9     10    11    12    13    14    15    16    17    18    19
            string[] expected = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            col.Register(0);

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(expected[i], actual);
            }

        }

        #endregion

        #region public void PageDecimalsCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageDecimalsCollection()
        {
            int repeatcount = 20;
            //index                0    1    2    3    4    5    6    7    8    9     10    11    12    13    14    15    16    17    18    19
            string[] expected = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

            PDFPageNumbers col = new PDFPageNumbers();

            PDFPageNumberOptions options = new PDFPageNumberOptions() { NumberStyle = PageNumberStyle.Decimals };

            col.StartNumbering(options);

            col.Register(0);
            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(expected[i], actual);
            }

        }

        #endregion

        #region public void PageRomanUpperCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperCollection()
        {
            int repeatcount = 20;
            //index                0    1      2      3    4    5      6      7      8     9    10    11      12     13     14    15      16      17      18     19
            string[] expected = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            PDFStyle full = new PDFStyle();
            full.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
            PDFPageNumberOptions opts = full.CreatePageNumberOptions();
            col.PushPageNumber(opts);

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(expected[i], actual);
            }

        }

        #endregion

        #region public void PageRomanLowerCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanLowerCollection()
        {
            int repeatcount = 20;
            //index                0    1     2       3    4    5      6      7      8     9    10    11      12     13     14    15      16      17      18     19
            string[] expected = { "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix", "x", "xi", "xii", "xiii", "xiv", "xv", "xvi", "xvii", "xviii", "xix", "xx" };

            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            PDFStyle full = new PDFStyle();
            full.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            PDFPageNumberOptions opts = full.CreatePageNumberOptions();
            col.PushPageNumber(opts);

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
               
                Assert.AreEqual(expected[i], actual);
            }

        }

        #endregion

        #region public void PageLetterLowerCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageLetterLowerCollection()
        {
            int repeatcount = 29;
            //index                0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25    26    27    28
            string[] expected = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "bb", "cc" };

            PDFPageNumbers col = new PDFPageNumbers();

            PDFStyle full = new PDFStyle();

            full.PageStyle.NumberStyle = PageNumberStyle.LowercaseLetters;
            col.StartNumbering(full.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(28);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(expected[i], actual);
            }
        }

        #endregion

        #region public void PageLetterUpperCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageLetterUpperCollection()
        {
            int repeatcount = 29;
            //index                0    1    2    3    4    5    6    7    8    9   10    11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26    27    28
            string[] expected = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "BB", "CC" };

            PDFPageNumbers col = new PDFPageNumbers();

            PDFStyle full = new PDFStyle();
            full.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
            col.StartNumbering(full.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(28);
            col.EndNumbering();
            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                
                Assert.AreEqual(expected[i], actual);
            }


        }

        #endregion

        #region public void PageNoneCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageNoneCollection()
        {
            int repeatcount = 20;

            PDFPageNumbers col = new PDFPageNumbers();
            PDFStyle style = new PDFStyle();
            style.PageStyle.NumberStyle = PageNumberStyle.None;
            col.StartNumbering(style.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                string expected = string.Empty;
                Assert.AreEqual(expected, actual);
            }

        }

        #endregion

        #region public void PagePrefixOnlyCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PagePrefixOnlyCollection()
        {
            int repeatcount = 20;
            string prefix = "Page Label ";
            PDFPageNumbers col = new PDFPageNumbers();
            PDFStyle style = new PDFStyle();
            style.PageStyle.NumberStyle = PageNumberStyle.None;
            style.PageStyle.NumberPrefix = prefix;
            col.StartNumbering(style.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                string expected = prefix; //No page number, so just the prefix
                Assert.AreEqual(expected, actual);
            }
        }

        #endregion

        #region public void PageRomanUpperWithPrefixCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperWithPrefixCollection()
        {
            int repeatcount = 20;
            //index                0    1      2      3    4    5      6       7     8     9    10     11    12      13     14    15      16      17       18    19
            string[] expected = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX"};
            string prefix = "Page ";

            PDFPageNumbers col = new PDFPageNumbers();
            
            PDFStyle style = new PDFStyle();
            style.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
            style.PageStyle.NumberPrefix = prefix;
            col.StartNumbering(style.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(prefix + expected[i], actual);
            }

        }

        #endregion

        #region public void PageRomanUpperWithStartIndexCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperWithStartIndexCollection()
        {
            int startindex = 4;
            int repeatcount = 20;
            //index                0     1    2      3      4      5     6    7      8      9      10     11    12     13       14      15    16     17      18      19
            string[] expected = { "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII" };

            PDFPageNumbers col = new PDFPageNumbers();
            PDFStyle style = new PDFStyle();
            style.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
            style.PageStyle.NumberStartIndex = startindex;
            col.StartNumbering(style.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                Assert.AreEqual(expected[i], actual);
            }

        }

        #endregion

        #region public void PageRomanUpperWithStartIndexAndPrefixCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperWithStartIndexAndPrefixCollection()
        {
            int startindex = 4;
            int repeatcount = 20;
            //index                0     1    2      3      4      5     6    7      8      9      10     11    12     13       14      15    16     17      18      19
            string[] expected = { "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII" };
            string prefix = "Page ";

            PDFPageNumbers col = new PDFPageNumbers();
            PDFStyle style = new PDFStyle();
            style.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
            style.PageStyle.NumberStartIndex = startindex;
            style.PageStyle.NumberPrefix = prefix;
            col.StartNumbering(style.CreatePageNumberOptions());

            col.Register(0);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < repeatcount; i++)
            {
                string actual = col.GetPageLabel(i);
                
                Assert.AreEqual(prefix + expected[i], actual);
            }

        }

        #endregion


        #region public void PageCollection_DefaultAndBack()

        /// <summary>
        /// Starts with standard numbering, then 2 new styles and then continues the 
        /// first numbering from the last page index.
        /// </summary>
        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageCollection_DefaultAndBack()
        {
            // page indices                          0,    1,   2,   3,   4,   5,   6,     7,    8,    9,   10,    11 ,   12,    13,    14,   15,  16,  17,  18,   19
            string[] expectedlabels = new string[] { "1", "2", "3", "4", "5", "i", "ii", "iii", "iv", "v", "P B", "P C", "P D", "P E", "P F", "6", "7", "8", "9", "10" };

            PDFPageNumbers col = new PDFPageNumbers();
            PDFStyle def = new PDFStyle();
            
            def.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            def.PageStyle.NumberStartIndex = 1;
            col.StartNumbering(def.CreatePageNumberOptions());

            PDFStyle lowRoman = new PDFStyle();
            lowRoman.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            lowRoman.PageStyle.NumberStartIndex = 1;
            

            PDFStyle alpha = new PDFStyle();
            alpha.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
            alpha.PageStyle.NumberStartIndex = 2;
            alpha.PageStyle.NumberPrefix = "P ";

            col.Register(0);
            col.Register(1);
            col.Register(2);
            col.Register(3);
            col.Register(4);

            PDFPageNumberGroup grp = col.PushPageNumber(lowRoman.CreatePageNumberOptions());
            col.Register(5);
            col.Register(6);
            col.Register(7);
            col.Register(8);
            col.Register(9);
            col.UnRegister(9);
            col.PopNumberStyle(grp);

            grp = col.PushPageNumber(alpha.CreatePageNumberOptions());
            col.Register(10);
            col.Register(11);
            col.Register(12);
            col.Register(13);
            col.Register(14);
            col.UnRegister(14);
            col.PopNumberStyle(grp);

            col.Register(15);
            col.Register(16);
            col.Register(17);
            col.Register(18);
            col.Register(19);
            col.UnRegister(19);
            col.EndNumbering();

            for (int i = 0; i < 20; i++)
            {
                string lbl = col.GetPageLabel(i);
                Assert.AreEqual(expectedlabels[i], lbl);
            }
        }

        #endregion

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageCollection_Nested()
        {
            // structure                  |  None      | Start                 | Reset Numbers                                                | Back To Start sequence        | Back to None
            // output                     | 0   1   2  | 3    4      5     6   | 7      8        9       10     11       12      13      14   | 15       16      17      18   | 19  20  21
            string[] expectedLabels =     { "", "", "", "i", "ii", "iii", "iv", "i",   "ii",    "iii",  "iv",   "v",    "vi",   "vii",  "viii", "v",    "vi",   "vii",  "viii", "", "", "" };
            string[] expectedLastLabels = { "", "", "", "iv", "iv", "iv", "iv", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "viii", "", "", "" };

            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            PDFStyle none = new PDFStyle();
            none.PageStyle.NumberStyle = PageNumberStyle.None;

            PDFPageNumberGroup noneGrp = col.PushPageNumber(none.CreatePageNumberOptions());
            int noneReg = 0;
            col.Register(noneReg);
            //keep none at the top - no unregister

            PDFStyle intro = new PDFStyle();
            intro.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            PDFPageNumberGroup introGrp = col.PushPageNumber(intro.CreatePageNumberOptions());

            int introReg = 3;
            col.Register(introReg);
            //col.UnRegister(introUnreg);
            //col.PopNumberStyle(grp);

            //no style, just start index
            PDFStyle resetStartIndex = new PDFStyle();
            resetStartIndex.PageStyle.NumberStartIndex = 1;
            PDFPageNumberGroup resetNumGrp = col.PushPageNumber(resetStartIndex.CreatePageNumberOptions());

            int pgReg = 7;
            int pgUnreg = 14;

            col.Register(pgReg);
            col.UnRegister(pgUnreg);
            col.PopNumberStyle(resetNumGrp);

            int introUnreg = 18;
            col.UnRegister(introUnreg);
            col.PopNumberStyle(introGrp);

            int noneUnReg = 21;
            col.UnRegister(noneUnReg);
            col.PopNumberStyle(noneGrp);

            col.EndNumbering();

            for (int i = 0; i < 21; i++)
            {
                PDFPageNumberData number = col.GetPageData(i);
                Assert.AreEqual(expectedLabels[i], number.Label, "Page labels did not match for index " + i);
                Assert.AreEqual(expectedLastLabels[i], number.LastLabel, "Page Last labels did not match for index " + i);
            }
        }

        #region public void PageCollection_BookNumbers()

        /// <summary>
        /// Use of the PDFPageNumbers and registering into the PDFPageNumberingCollection
        /// to validate that all the info returned is correct in the instance.
        /// </summary>
        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageCollection_BookNumbers()
        {
            // structure                  |  None      | Intro                 | Pages                                 | Appendices                                            | References
            // output                     | 0   1   2  | 3    4      5     6   | 7    8     9   10  11   12   13   14  | 15            16            17            18          | 19       20       21
            string[] expectedLabels =     { "", "", "", "i", "ii", "iii", "iv", "1", "2", "3", "4", "5", "6", "7", "8", "Appendix A", "Appendix A", "Appendix B", "Appendix C", "Ref 1", "Ref 2", "Ref 3" };
            string[] expectedLastLabels = { "", "", "", "iv", "iv", "iv", "iv", "8", "8", "8", "8", "8", "8", "8", "8", "Appendix A", "Appendix A", "Appendix B", "Appendix C", "Ref 3", "Ref 3", "Ref 3" };
            int[] expectedPageIndex = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };
            int expectedTotalPageIndex = 22;
            int[] expectedGroupNumber = { 1, 2, 3, 1, 2, 3, 4, 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 1, 1, 1, 2, 3 };
            int[] expectedGroupMax =    { 3, 3, 3, 4, 4, 4, 4, 8, 8, 8, 8, 8, 8, 8, 8, 2, 2, 1, 1, 3, 3, 3 };
            PDFPageNumbers col = new PDFPageNumbers();
            col.StartNumbering(null);

            PDFStyle none = new PDFStyle();
            none.PageStyle.NumberStyle = PageNumberStyle.None;
            int noneReg = 0;
            int noneUnReg = 2;
            PDFPageNumberGroup grp = col.PushPageNumber(none.CreatePageNumberOptions());
            col.Register(noneReg);
            col.UnRegister(noneUnReg);
            col.PopNumberStyle(grp);

            PDFStyle intro = new PDFStyle();
            intro.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            intro.PageStyle.NumberStartIndex = 1;
            grp = col.PushPageNumber(intro.CreatePageNumberOptions());

            int introReg = 3;
            int introUnreg = 6;
            col.Register(introReg);
            col.UnRegister(introUnreg);
            col.PopNumberStyle(grp);

            PDFStyle pages = new PDFStyle();
            pages.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            pages.PageStyle.NumberStartIndex = 1;
            grp = col.PushPageNumber(pages.CreatePageNumberOptions());
            int pagesReg = 7;
            int pagesUnReg = 14;
            col.Register(pagesReg);
            col.UnRegister(pagesUnReg);
            col.PopNumberStyle(grp);

            PDFStyle appendixA = new PDFStyle();
            appendixA.PageStyle.NumberStyle = PageNumberStyle.None;
            appendixA.PageStyle.NumberPrefix = "Appendix A";
            grp = col.PushPageNumber(appendixA.CreatePageNumberOptions());

            int appendAReg = 15;
            int appendAUnReg = 16;
            col.Register(appendAReg);
            col.UnRegister(appendAUnReg);
            col.PopNumberStyle(grp);

            PDFStyle appendixB = new PDFStyle();
            appendixB.PageStyle.NumberPrefix = "Appendix B";
            appendixB.PageStyle.NumberStyle = PageNumberStyle.None;
            grp = col.PushPageNumber(appendixB.CreatePageNumberOptions());
            int appendBReg = 17;
            int appendBUnReg = 17;
            col.Register(appendBReg);
            col.UnRegister(appendBUnReg);
            col.PopNumberStyle(grp);

            PDFStyle appendixC = new PDFStyle();
            appendixC.PageStyle.NumberPrefix = "Appendix C";
            appendixC.PageStyle.NumberStyle = PageNumberStyle.None;
            grp = col.PushPageNumber(appendixC.CreatePageNumberOptions());

            int appendCReg = 18;
            int appendCUnReg = 18;
            col.Register(appendCReg);
            col.UnRegister(appendCUnReg);
            col.PopNumberStyle(grp);

            PDFStyle refs = new PDFStyle();
            refs.PageStyle.NumberPrefix = "Ref ";
            refs.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            refs.PageStyle.NumberStartIndex = 1;
            grp = col.PushPageNumber(refs.CreatePageNumberOptions());

            int refsReg = 19;
            int refsUnReg = 21;
            col.Register(refsReg);
            col.UnRegister(refsUnReg);
            col.PopNumberStyle(grp);

            col.EndNumbering();
            
            for (int i = 0; i < expectedLabels.Length; i++)
            {
                PDFPageNumberData number = col.GetPageData(i);
                Assert.AreEqual(expectedLabels[i], number.Label, "Page labels did not match for index " + i);
                Assert.AreEqual(expectedLastLabels[i], number.LastLabel, "Page last labels did not match for index " + i);
                Assert.AreEqual(expectedGroupNumber[i], number.GroupNumber, "Page group number did not match for index " + i);
                Assert.AreEqual(expectedGroupMax[i], number.GroupLastNumber, "Page group last number did not match for index " + i);
                Assert.AreEqual(expectedPageIndex[i], number.PageNumber, "Page number did not match for index " + i);
                Assert.AreEqual(expectedTotalPageIndex, number.LastPageNumber, "Page last page number did not match for index " + i);
            }

        }

        #endregion

    }
}
