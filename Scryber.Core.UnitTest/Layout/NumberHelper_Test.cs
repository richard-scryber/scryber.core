using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scryber.Core.UnitTests.Layout
{
    [TestClass()]
    public class NumberHelper_Test
    {
        //
        // Utilities.NumberHelper tests
        //

        #region public void RomanUpperString()

        [TestMethod()]
        [TestCategory("Utilities")]
        public void RomanUpperString()
        {
            //index                     0   1    2      3      4    5    6      7      8       9    10   11    12      13     14     15    16      17      18      19     20
            string[] expectedArray = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX" };

            string actual;
            string expected;
            for (int i = 0; i < expectedArray.Length; i++)
            {
                actual = Utilities.NumberHelper.GetRomanUpper(i);
                Assert.AreEqual(expectedArray[i], actual);
            }


            int value = 50;
            expected = "L";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 49;
            expected = "XLIX";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 58;
            expected = "LVIII";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 59;
            expected = "LIX";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 100;
            expected = "C";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 99;
            expected = "XCIX";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 501;
            expected = "DI";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 552;
            expected = "DLII";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);

            value = 1552;
            expected = "MDLII";
            actual = Utilities.NumberHelper.GetRomanUpper(value);
            Assert.AreEqual(expected, actual);


        }

        #endregion

        #region public void RomanLowerString()

        [TestMethod()]
        [TestCategory("Utilities")]
        public void RomanLowerString()
        {
            //index                     0   1    2      3      4    5    6      7      8       9    10   11    12      13     14     15    16      17      18      19     20
            string[] expectedArray = { "", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix", "x", "xi", "xii", "xiii", "xiv", "xv", "xvi", "xvii", "xviii", "xix", "xx" };

            string actual;
            string expected;
            for (int i = 0; i < expectedArray.Length; i++)
            {
                actual = Utilities.NumberHelper.GetRomanLower(i);
                Assert.AreEqual(expectedArray[i], actual);
            }


            int value = 50;
            expected = "l";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 49;
            expected = "xlix";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 58;
            expected = "lviii";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 59;
            expected = "lix";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 100;
            expected = "c";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 99;
            expected = "xcix";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 501;
            expected = "di";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 552;
            expected = "dlii";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);

            value = 1552;
            expected = "mdlii";
            actual = Utilities.NumberHelper.GetRomanLower(value);
            Assert.AreEqual(expected, actual);


        }

        #endregion

        #region public void LetterLowerString()

        [TestMethod()]
        [TestCategory("Utilities")]
        public void LetterLowerString()
        {
            int repeatcount = 30;
            //index                     0   1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25    26   27    28    29
            string[] expectedArray = { "", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "bb", "cc" };

            for (int i = 0; i < repeatcount; i++)
            {
                string expected = expectedArray[i];
                string actual = Utilities.NumberHelper.GetLetterLower(i);
                Assert.AreEqual(expected, actual);
            }
        }

        #endregion

        #region public void LetterUpperString()

        [TestMethod()]
        [TestCategory("Utilities")]
        public void LetterUpperString()
        {
            int repeatcount = 30;
            //index                     0   1    2    3    4    5    6    7    8    9   10    11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27    28    29
            string[] expectedArray = { "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "BB", "CC" };

            
            for (int i = 0; i < repeatcount; i++)
            {
                string expected = expectedArray[i];
                string actual = Utilities.NumberHelper.GetLetterUpper(i);

                Assert.AreEqual(expected, actual);
            }

        }

        #endregion
    }
}
