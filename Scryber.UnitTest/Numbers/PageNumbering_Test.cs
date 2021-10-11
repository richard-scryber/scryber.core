using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scryber.Styles;


namespace Scryber.Core.UnitTests.Numbers
{
    /// <summary>
    /// A set of tests for the PageNumberingCollection, PageNumberGroup and PageStyle
    /// Specifically around the registation of pages and their page numbering styles so that
    /// we know the format of page numbers and labels is correct.
    /// </summary>
    [TestClass()]
    public class PageNumbering_Test
    {


        //
        // page numbering tests
        //

        [TestMethod]
        [TestCategory("Page Numbering")]
        public void PageRegistrations()
        {

            PageNumberGroup grp = new PageNumberGroup(null, "", PageNumberStyle.Decimals, 1);
            PageNumberRegistration reg = new PageNumberRegistration(0, 10, grp);

            for (int i = 0; i < 10; i++)
            {
                string pg = (i + 1).ToString();
                Assert.AreEqual(pg, reg.GetPageLabel(i), "Block 1: Page index " + i + " does not match " + pg);
            }


            grp = new PageNumberGroup(null, "", PageNumberStyle.LowercaseLetters, 1);
            reg = new PageNumberRegistration(0, 10, grp);

            for (int i = 0; i < 10; i++)
            {
                char value = 'a';
                value =  (char)((int)value + i);
                string pg = value.ToString();

                Assert.AreEqual(pg, reg.GetPageLabel(i), "Block 2: Page index " + i + " does not match " + pg);
            }

            grp = new PageNumberGroup(null, "", PageNumberStyle.Decimals, 1);
            reg = new PageNumberRegistration(0, 10, grp);
            reg.PreviousLinkedRegistrationPageCount = 5;

            for (int i = 0; i < 10; i++)
            {
                string pg = (i + 1 + 5).ToString();
                Assert.AreEqual(pg, reg.GetPageLabel(i));
            }

            grp = new PageNumberGroup(null, "", PageNumberStyle.LowercaseLetters, 1);
            reg = new PageNumberRegistration(0, 10, grp);
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
            grp = new PageNumberGroup(null, "", PageNumberStyle.UppercaseLetters, 1);
            reg = new PageNumberRegistration(15, 25, grp);
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

            PageNumbers col = new PageNumbers();
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

            PageNumbers col = new PageNumbers();
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

            PageNumbers col = new PageNumbers();

            PageNumberOptions options = new PageNumberOptions() { NumberStyle = PageNumberStyle.Decimals };

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

            PageNumbers col = new PageNumbers();
            col.StartNumbering(null);

            Style full = new Style();
            full.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
            PageNumberOptions opts = full.CreatePageNumberOptions();
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

            PageNumbers col = new PageNumbers();
            col.StartNumbering(null);

            Style full = new Style();
            full.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            PageNumberOptions opts = full.CreatePageNumberOptions();
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

            PageNumbers col = new PageNumbers();

            Style full = new Style();

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

            PageNumbers col = new PageNumbers();

            Style full = new Style();
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

            PageNumbers col = new PageNumbers();
            Style style = new Style();
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

        #region public void PageRomanUpperWithPrefixCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperWithPrefixCollection()
        {
            int repeatcount = 20;
            //index                0    1      2      3    4    5      6       7     8     9    10     11    12      13     14    15      16      17       18    19
            string[] expected = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX"};
            
            PageNumbers col = new PageNumbers();
            
            Style style = new Style();
            style.PageStyle.NumberStyle = PageNumberStyle.UppercaseRoman;
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

        #region public void PageRomanUpperWithStartIndexCollection()

        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageRomanUpperWithStartIndexCollection()
        {
            int startindex = 4;
            int repeatcount = 20;
            //index                0     1    2      3      4      5     6    7      8      9      10     11    12     13       14      15    16     17      18      19
            string[] expected = { "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII" };

            PageNumbers col = new PageNumbers();
            Style style = new Style();
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
            
            PageNumbers col = new PageNumbers();
            Style style = new Style();
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


        #region public void PageCollection_DefaultAndBack()

        /// <summary>
        /// Starts with standard numbering, then 2 new styles and then continues the 
        /// first numbering from the last page index.
        /// </summary>
        [TestMethod()]
        [TestCategory("Page Numbering")]
        public void PageCollection_DefaultAndBack()
        {
            // page indices                          0,    1,   2,   3,   4,   5,   6,     7,    8,    9,  10,  11 , 12,  13,  14,  15,  16,  17,  18,   19
            string[] expectedlabels = new string[] { "1", "2", "3", "4", "5", "i", "ii", "iii", "iv", "v", "B", "C", "D", "E", "F", "6", "7", "8", "9", "10" };

            PageNumbers col = new PageNumbers();
            Style def = new Style();
            
            def.PageStyle.NumberStyle = PageNumberStyle.Decimals;
            def.PageStyle.NumberStartIndex = 1;
            col.StartNumbering(def.CreatePageNumberOptions());

            Style lowRoman = new Style();
            lowRoman.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            lowRoman.PageStyle.NumberStartIndex = 1;
            

            Style alpha = new Style();
            alpha.PageStyle.NumberStyle = PageNumberStyle.UppercaseLetters;
            alpha.PageStyle.NumberStartIndex = 2;

            col.Register(0);
            col.Register(1);
            col.Register(2);
            col.Register(3);
            col.Register(4);

            PageNumberGroup grp = col.PushPageNumber(lowRoman.CreatePageNumberOptions());
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

            PageNumbers col = new PageNumbers();
            col.StartNumbering(null);

            Style none = new Style();
            none.PageStyle.NumberStyle = PageNumberStyle.None;

            PageNumberGroup noneGrp = col.PushPageNumber(none.CreatePageNumberOptions());
            int noneReg = 0;
            col.Register(noneReg);
            //keep none at the top - no unregister

            Style intro = new Style();
            intro.PageStyle.NumberStyle = PageNumberStyle.LowercaseRoman;
            PageNumberGroup introGrp = col.PushPageNumber(intro.CreatePageNumberOptions());

            int introReg = 3;
            col.Register(introReg);
            //col.UnRegister(introUnreg);
            //col.PopNumberStyle(grp);

            //no style, just start index
            Style resetStartIndex = new Style();
            resetStartIndex.PageStyle.NumberStartIndex = 1;
            PageNumberGroup resetNumGrp = col.PushPageNumber(resetStartIndex.CreatePageNumberOptions());

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
                PageNumberData number = col.GetPageData(i);
                Assert.AreEqual(expectedLabels[i], number.Label, "Page labels did not match for index " + i);
                Assert.AreEqual(expectedLastLabels[i], number.LastLabel, "Page Last labels did not match for index " + i);
            }
        }

    }
}
