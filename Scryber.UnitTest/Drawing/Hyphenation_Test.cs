using Scryber.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Scryber;
using System.Collections.Generic;
using Scryber.PDF.Graphics;

namespace Scryber.Core.UnitTests.Drawing
{
    
    
    /// <summary>
    ///This is a test class for GraientDecriptors and is intended
    ///to contain all GradientDecriptor and GradientFunction Unit Tests
    ///</summary>
    [TestClass()]
    public class HyphenationStrategy_Test
    {

        #region public TestContext TestContext {get;set;}
        
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

        #endregion

        #region public void HypenateCanTrimLeft_Test()

        /// <summary>
        /// Checks the hypenation LEFT side checks against minimum length
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateCanTrimLeft_Test()
        {

            //1. 3 characters min length - we have 3

            var chars = "The quick brown fox";
            var len = chars.IndexOf("o") + 1;
            var minCharsLeft = 3;

            Assert.AreEqual("The quick bro", chars.Substring(0, len));

            var actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            var expected = len; //OK - 3 characters before the proposed split

            Assert.AreEqual(expected, actual, "'The quick bro' split failed");
            Assert.AreEqual("The quick bro", chars.Substring(0, actual), "'The quick bro' split failed");

            //2. 3 characters min length - we have 4

            len = chars.IndexOf("w") + 1;
            Assert.AreEqual("The quick brow", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = len; //OK - 4 characters before the proposed split

            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick brow", chars.Substring(0, actual), "'The quick brow' split failed for " + minCharsLeft);


            //3. 3 characters min length - we have 2

            len = chars.IndexOf("r") + 1;
            Assert.AreEqual("The quick br", chars.Substring(0, len));

            //Should return the 'The quick'
            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = 9; // Changed 
            
            Assert.AreEqual(expected, actual, "'The quick br' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(0, actual), "'The quick br' split failed for " + minCharsLeft);

            //4. 3 characters min length - we have 1

            len = chars.IndexOf("b");
            Assert.AreEqual("The quick ", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = len - 1; //OK - breaks on a white space, but trimmed
            

            Assert.AreEqual(expected, actual, "'The quick' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(0, actual), "'The quick ' split failed for " + minCharsLeft);


            //5. 3 characters min length - index is 3

            len = chars.IndexOf("e") + 1;
            Assert.AreEqual("The", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = 3; //OK - breaks on a white space, but trimmed


            Assert.AreEqual(expected, actual, "'The' split failed for " + minCharsLeft);
            Assert.AreEqual("The", chars.Substring(0, actual), "'The' split failed for " + minCharsLeft);

            //6. 3 characters min length - index is 2

            len = chars.IndexOf("e");
            Assert.AreEqual("Th", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = 0; //Cannot split at all. Should be zero


            Assert.AreEqual(expected, actual, "'Th' split failed for " + minCharsLeft);
            Assert.AreEqual("", chars.Substring(0, actual), "'Th' split failed for " + minCharsLeft);


            //7. 6 characters min length - we have 4
            minCharsLeft = 6;

            len = chars.IndexOf("w") + 1;
            Assert.AreEqual("The quick brow", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = 9; //Changed

            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(0, actual), "'The quick brow' split failed for " + minCharsLeft);



            //8. 6 characters min length - a really long word

            chars = "ALongWordWithoutSpaces";
            len = chars.IndexOf("S") + 1;
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, 0, len, minCharsLeft);
            expected = len; //Changed

            Assert.AreEqual(expected, actual, "'ALongWordWithoutSpaces' split failed for " + minCharsLeft);
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(0, actual), "'ALongWordWithoutSpaces' split failed for " + minCharsLeft);

        }

        #endregion

        #region public void HypenateCanTrimLeft_Test()

        /// <summary>
        /// Checks the hypenation LEFT side checks against minimum length
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateCanTrimLeftMidString_Test()
        {

            //1. 3 characters min length - we have 3

            var chars = "THE QUICK BROWN FOX. The quick brown fox";
            var start = chars.IndexOf(".") + 2;

            var len = chars.IndexOf("o") + 1 - start;
            var minCharsLeft = 3;

            Assert.AreEqual("The quick bro", chars.Substring(start, len));

            var actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            var expected = len; //OK - 3 characters before the proposed split

            Assert.AreEqual(expected, actual, "'The quick bro' split failed");
            Assert.AreEqual("The quick bro", chars.Substring(start, actual), "'The quick bro' split failed");

            //2. 3 characters min length - we have 4

            len = chars.IndexOf("w") + 1 - start;
            Assert.AreEqual("The quick brow", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = len; //OK - 4 characters before the proposed split

            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick brow", chars.Substring(start, actual), "'The quick brow' split failed for " + minCharsLeft);


            //3. 3 characters min length - we have 2

            len = chars.IndexOf("r") + 1 - start;
            Assert.AreEqual("The quick br", chars.Substring(start, len));

            //Should return the 'The quick'
            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = 9; // Changed 

            Assert.AreEqual(expected, actual, "'The quick br' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(start, actual), "'The quick br' split failed for " + minCharsLeft);

            //4. 3 characters min length - we have 1

            len = chars.IndexOf("b") - start;
            Assert.AreEqual("The quick ", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = len - 1; //OK - breaks on a white space, but trimmed


            Assert.AreEqual(expected, actual, "'The quick' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(start, actual), "'The quick ' split failed for " + minCharsLeft);


            //5. 3 characters min length - index is 3

            len = chars.IndexOf("e") + 1 - start;
            Assert.AreEqual("The", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = 3; //OK - breaks on a white space, but trimmed


            Assert.AreEqual(expected, actual, "'The' split failed for " + minCharsLeft);
            Assert.AreEqual("The", chars.Substring(start, actual), "'The' split failed for " + minCharsLeft);

            //6. 3 characters min length - index is 2

            len = chars.IndexOf("e") - start;
            Assert.AreEqual("Th", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = 0; //Cannot split at all. Should be zero


            Assert.AreEqual(expected, actual, "'Th' split failed for " + minCharsLeft);
            Assert.AreEqual("", chars.Substring(start, actual), "'Th' split failed for " + minCharsLeft);


            //7. 6 characters min length - we have 4
            minCharsLeft = 6;

            len = chars.IndexOf("w") + 1 - start;
            Assert.AreEqual("The quick brow", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = 9; //Changed

            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsLeft);
            Assert.AreEqual("The quick", chars.Substring(start, actual), "'The quick brow' split failed for " + minCharsLeft);



            //8. 6 characters min length - a really long word

            chars = "THE QUICK BROWN FOX. ALongWordWithoutSpaces";
            len = chars.IndexOf("S") + 1 - start;
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForLeft(chars, start, len, minCharsLeft);
            expected = len; //Changed

            Assert.AreEqual(expected, actual, "'ALongWordWithoutSpaces' split failed for " + minCharsLeft);
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(start, actual), "'ALongWordWithoutSpaces' split failed for " + minCharsLeft);

        }

        #endregion


        #region public void HypenateCanTrimRight_Test()

        /// <summary>
        /// Checks the hypenation RIGHT side checks against minimum length.
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateCanTrimRight_Test()
        {
            //1. 3 characters min length - we have 3

            var chars = "The quick brown fox";
            var len = chars.IndexOf("o");
            var minCharsRight = 3;

            Assert.AreEqual("The quick br", chars.Substring(0, len));

            var actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            var expected = len; //OK - 3 characters after the proposed split

            Assert.AreEqual(expected, actual, "'The quick br' split failed");
            Assert.AreEqual("The quick br", chars.Substring(0, actual), "'The quick br' split failed");


            //2. 3 characters min length - we have 4

            len = chars.IndexOf("r");
            Assert.AreEqual("The quick b", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = len; //OK - 4 characters after the proposed split

            Assert.AreEqual(expected, actual, "'The quick b' split failed for " + minCharsRight);
            Assert.AreEqual("The quick b", chars.Substring(0, actual), "'The quick b' split failed for " + minCharsRight);



            //3. 3 characters min length - we have 2

            len = chars.IndexOf("w");
            Assert.AreEqual("The quick bro", chars.Substring(0, len));

            //Should return the 'The quick'
            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = len + 2; // Changed to the end of the word

            Assert.AreEqual(expected, actual, "'The quick bro' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(0, actual), "'The quick bro' split failed for " + minCharsRight);


            
            //4. 3 characters min length - we have 1

            len = chars.IndexOf("n");
            Assert.AreEqual("The quick brow", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = len + 1; //Changed to the end of the word


            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(0, actual), "'The quick brow' split failed for " + minCharsRight);


            //5. End of the word

            len = chars.IndexOf("n") + 1;
            Assert.AreEqual("The quick brown", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = len; //It is the end of the word


            Assert.AreEqual(expected, actual, "'The quick brown' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(0, actual), "'The quick brown' split failed for " + minCharsRight);


            //6. On a space

            len = chars.IndexOf("n") + 2;
            Assert.AreEqual("The quick brown ", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = len - 1; //OK - breaks after a white space, but trimmed so -1


            Assert.AreEqual(expected, actual, "'The quick brown ' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(0, actual), "'The quick brown ' split failed for " + minCharsRight);


            //7. 3 characters min length - penultimate character - return the full string length

            len = chars.IndexOf("x");
            Assert.AreEqual("The quick brown fo", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = chars.Length; //Cannot split at all. Should be lenght of the string


            Assert.AreEqual(expected, actual, "'The quick brown fo' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown fox", chars.Substring(0, actual), "'The quick brown fo' split failed for " + minCharsRight);

            //8. Very end of the string
            

            len = chars.IndexOf("x") + 1;
            Assert.AreEqual("The quick brown fox", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = chars.Length; //Changed

            Assert.AreEqual(expected, actual, "'The quick brown fox' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown fox", chars.Substring(0, actual), "'The quick brown fox' split failed for " + minCharsRight);

            

            //8. 6 characters min length - a really long word
            minCharsRight = 6;
            chars = "ALongWordWithoutSpaces";
            len = chars.IndexOf("S") + 1;
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(0, len));

            actual = PDFHyphenationRule.SplitForRight(chars, 0, len, minCharsRight);
            expected = chars.Length; //Changed to the end of the string

            Assert.AreEqual(expected, actual, "'ALongWordWithoutSpaces' split failed for " + minCharsRight);
            Assert.AreEqual("ALongWordWithoutSpaces", chars.Substring(0, actual), "'ALongWordWithoutSpaces' split failed for " + minCharsRight);

        }

        #endregion


        #region public void HypenateCanTrimRight_Test()

        /// <summary>
        /// Checks the hypenation RIGHT side checks against minimum length.
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateCanTrimRightMidString_Test()
        {
            //1. 3 characters min length - we have 3

            var chars = "THE QUICK BROWN FOX. The quick brown fox";
            var start = chars.IndexOf(".") + 2;

            var len = chars.IndexOf("o") - start;
            var minCharsRight = 3;

            Assert.AreEqual("The quick br", chars.Substring(start, len));

            var actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            var expected = len; //OK - 3 characters after the proposed split

            Assert.AreEqual(expected, actual, "'The quick br' split failed");
            Assert.AreEqual("The quick br", chars.Substring(start, actual), "'The quick br' split failed");


            //2. 3 characters min length - we have 4

            len = chars.IndexOf("r") - start;
            Assert.AreEqual("The quick b", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = len; //OK - 4 characters after the proposed split

            Assert.AreEqual(expected, actual, "'The quick b' split failed for " + minCharsRight);
            Assert.AreEqual("The quick b", chars.Substring(start, actual), "'The quick b' split failed for " + minCharsRight);



            //3. 3 characters min length - we have 2

            len = chars.IndexOf("w") - start;
            Assert.AreEqual("The quick bro", chars.Substring(start, len));

            //Should return the 'The quick'
            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = len + 2; // Changed to the end of the word

            Assert.AreEqual(expected, actual, "'The quick bro' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(start, actual), "'The quick bro' split failed for " + minCharsRight);



            //4. 3 characters min length - we have 1

            len = chars.IndexOf("n") - start;
            Assert.AreEqual("The quick brow", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = len + 1; //Changed to the end of the word


            Assert.AreEqual(expected, actual, "'The quick brow' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(start, actual), "'The quick brow' split failed for " + minCharsRight);


            //5. End of the word

            len = chars.IndexOf("n") + 1 - start;
            Assert.AreEqual("The quick brown", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = len; //It is the end of the word


            Assert.AreEqual(expected, actual, "'The quick brown' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(start, actual), "'The quick brown' split failed for " + minCharsRight);


            //6. On a space

            len = chars.IndexOf("n") + 2 - start;
            Assert.AreEqual("The quick brown ", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = len - 1; //OK - breaks after a white space, but trimmed so -1


            Assert.AreEqual(expected, actual, "'The quick brown ' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown", chars.Substring(start, actual), "'The quick brown ' split failed for " + minCharsRight);


            //7. 3 characters min length - penultimate character - return the full string length

            len = chars.IndexOf("x") - start;
            Assert.AreEqual("The quick brown fo", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = chars.Length - start; //Cannot split at all. Should be lenght of the string


            Assert.AreEqual(expected, actual, "'The quick brown fo' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown fox", chars.Substring(start, actual), "'The quick brown fo' split failed for " + minCharsRight);

            //8. Very end of the string


            len = chars.IndexOf("x") + 1 - start;
            Assert.AreEqual("The quick brown fox", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = chars.Length - start; //Changed

            Assert.AreEqual(expected, actual, "'The quick brown fox' split failed for " + minCharsRight);
            Assert.AreEqual("The quick brown fox", chars.Substring(start, actual), "'The quick brown fox' split failed for " + minCharsRight);



            //8. 6 characters min length - a really long word
            minCharsRight = 6;
            chars = "THE QUICK BROWN FOX. ALongWordWithoutSpaces";
            len = chars.IndexOf("S") + 1 - start;
            Assert.AreEqual("ALongWordWithoutS", chars.Substring(start, len));

            actual = PDFHyphenationRule.SplitForRight(chars, start, len, minCharsRight);
            expected = chars.Length - start; //Changed to the end of the string

            Assert.AreEqual(expected, actual, "'ALongWordWithoutSpaces' split failed for " + minCharsRight);
            Assert.AreEqual("ALongWordWithoutSpaces", chars.Substring(start, actual), "'ALongWordWithoutSpaces' split failed for " + minCharsRight);

        }

        #endregion


        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateGetFirstWordBeforeEnd()
        {
            var chars = "The quick brown fox";
            var expected = chars.IndexOf("n") + 1;

            //check from the end of the string
            var offset = chars.Length - 1;

            var actual = PDFHyphenationRule.GetFirstWordBoundaryBefore(chars, 0, offset, null, null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual("The quick brown", chars.Substring(0, actual));

            //check it will go all the way back
            chars = "The quickbrownfox";
            offset = chars.Length - 1;

            expected = chars.IndexOf("e") + 1;
            actual = PDFHyphenationRule.GetFirstWordBoundaryBefore(chars, 0, offset, null, null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual("The", chars.Substring(0, actual));

            //Check using the from start offset
            chars = "THE QUICK BROWN FOX. The quickbrownfox";
            int start = "THE QUICK BROWN FOX. ".Length;
            offset = chars.Length - 1;

            expected = chars.IndexOf("e") + 1 - start;
            actual = PDFHyphenationRule.GetFirstWordBoundaryBefore(chars, start, offset, null, null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual("The", chars.Substring(start, actual));


            //Check using the offset as mid string
            chars = "The quick brown fox. THE QUICK BROWN FOX";
            start = 0;
            offset = chars.Length - 1 - "THE QUICK BROWN FOX".Length;

            expected = chars.IndexOf(".") + 1 - start;
            actual = PDFHyphenationRule.GetFirstWordBoundaryBefore(chars, start, offset, null, null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual("The quick brown fox.", chars.Substring(start, actual));

            chars = "The quickbrownfox. THE QUICK BROWN FOX";
            start = 0;
            offset = chars.Length - 2 - "THE QUICK BROWN FOX".Length;

            expected = chars.IndexOf("e") + 1 - start;
            actual = PDFHyphenationRule.GetFirstWordBoundaryBefore(chars, start, offset, null, null);

            Assert.AreEqual(expected, actual);
            Assert.AreEqual("The", chars.Substring(start, actual));
        }

        #region public void HypenateWord_AllowTest()

        /// <summary>
        ///A successfull test for hypenation of brown with min 2 chars before and after
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_AllowTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("o") + 1;
            Assert.AreEqual("The quick bro", chars.Substring(0, len));

            int minBefore = 2;
            int minAfter = 2;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);
            Assert.IsTrue(opportunity.IsHyphenation);

            var expected = "The quick bro-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsTrue(opportunity.AppendHyphenCharacter.HasValue);
            Assert.AreEqual('-', opportunity.AppendHyphenCharacter.Value);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);
            actual += opportunity.AppendHyphenCharacter.Value;
            
            Assert.AreEqual(expected, actual);
                        
        }

        #endregion


        #region public void HypenateWord_NotEnoughRightTest()

        /// <summary>
        ///A successfull test for splitting of brown with min 2 chars before and after.
        ///But the poposed position is too far right, 
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotEnoughRightMoveLeftTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("w") + 1;
            Assert.AreEqual("The quick brow", chars.Substring(0, len));

            //2 before and after are allowed, so can hyphenate
            //but our original position is too far right.
            //so should be pushed a little to the left.

            int minBefore = 2;
            int minAfter = 2;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation);

            //Shifted one
            var expected = "The quick bro-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsTrue(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NotEnoughRightWordJustLongEnoughTest()

        /// <summary>
        ///A successfull test for splitting of brown with 3 chars before and 2 after,
        ///even though desired position doesn't quite work and is shifted left one.
        ///Same as above, but the right has to be 3 chars
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotEnoughRightWordJustLongEnoughTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("w") + 1;
            Assert.AreEqual("The quick brow", chars.Substring(0, len));

            //3 before and 2 after are allowed, so can hyphenate
            // just fits
            int minBefore = 3;
            int minAfter = 2;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation);

            var expected = "The quick bro-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsTrue(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NotLongEnoughTest()

        /// <summary>
        ///A test for splitting of brown with min 3 chars before and after.
        ///It doesn't fit so should split on the word before - quick.
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotLongEnoughTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("o") + 1;
            Assert.AreEqual("The quick bro", chars.Substring(0, len));

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);
            Assert.IsFalse(opportunity.IsHyphenation);

            var expected = "The quick";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NotEnoughLeftTest()

        /// <summary>
        ///A test for splitting of brown with min 3 chars before and after.
        ///Works for right, but not enough left, so should split on the word before
        ///As we do not make the string longer.
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotEnoughLeftTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("r") + 1;
            Assert.AreEqual("The quick br", chars.Substring(0, len));

            

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            var expected = "The quick";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);

            

            Assert.AreEqual(expected, actual);

        }

        #endregion


        #region public void HypenateWord_NotEnoughForAnyTest()

        /// <summary>
        ///A test for splitting of brown with min 3 chars before and after.
        ///Works for right, but not enough left, so should split on the word before
        ///As we do not make the string longer.
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotEnoughForAnyTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("h") + 1;
            Assert.AreEqual("Th", chars.Substring(0, len));

            //2 before and after are allowed, so can hyphenate
            //but our original position is too far right.
            //so should be pushed a little to the left.

            int minBefore = 2;
            int minAfter = 2;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            //Nothing
            var expected = "";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NotEnoughForAnyTest()

        /// <summary>
        ///A test for splitting of brown and fox with min 3 chars before and after.
        /// As we are on a space that should be the actual split
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_OnASpaceTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("n") + 1;
            Assert.AreEqual("The quick brown", chars.Substring(0, len));

            //we are on a space

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            //Nothing
            var expected = "The quick brown";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NotEnoughForAnyTest()

        /// <summary>
        ///A test for splitting of brown and fox with min 3 chars before and after.
        /// As we are on a space that should be the actual split
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_AfterASpaceTest()
        {
            var chars = "The quick brown fox";
            var len = chars.IndexOf("n") + 2;
            Assert.AreEqual("The quick brown ", chars.Substring(0, len));

            //we are on a space

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            var expected = "The quick brown";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NoSpacesTest()

        /// <summary>
        /// A test for splitting of brown but there are no spaces at all.
        /// But we can hypenate
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NoSpacesTest()
        {
            var chars = "Thequickbrownfox";
            var len = chars.IndexOf("w") + 1; //there are enough characters
            Assert.AreEqual("Thequickbrow", chars.Substring(0, len));

            //we have not spaces

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation);

            //This is allowed to split

            var expected = "Thequickbrow-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsTrue(opportunity.AppendHyphenCharacter.HasValue);
            Assert.AreEqual('-', opportunity.AppendHyphenCharacter.Value);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_NoSpacesTest()

        /// <summary>
        /// A test for splitting but there are no spaces at all.
        /// Not enough chars to the right, so we can only hyphenate
        /// before the proposed length. So we return length - min left chars with a hypen
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NoSpacesRightTest()
        {
            var chars = "Thequickbrownfox";
            var len = chars.IndexOf("f") + 1; //there are NOT enough characters
            Assert.AreEqual("Thequickbrownf", chars.Substring(0, len));

            //we have not spaces

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation);

            //This is allowed to split back at the hyphen 3 chars

            var expected = "Thequickbrown-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsTrue(opportunity.AppendHyphenCharacter.HasValue);
            Assert.AreEqual('-', opportunity.AppendHyphenCharacter.Value);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_HyphenToTheRightTest()

        /// <summary>
        /// A test for splitting of quick-brown.
        /// We do not have enough characters at the end of the word so go back and look for a space or a hyphen
        /// We find a hyphen first so split there. No appending
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_HyphenToTheRightTest()
        {
            var chars = "The quick-brown fox";
            var len = chars.IndexOf("w") + 1; //there are NOT enough characters
            Assert.AreEqual("The quick-brow", chars.Substring(0, len));

            //we have not spaces

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation); //

            //This is allowed to split back at the hyphen

            var expected = "The quick-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            //Assert.AreEqual('-', opportunity.AppendHyphenCharacter.Value);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            //actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region public void HypenateWord_HyphenToTheRightTest()

        /// <summary>
        /// A test for splitting of quick-brown.
        /// We do not have enough characters at the end of the word so go back and look for a space or a hyphen
        /// We find a hyphen first so split there. No appending of the strategy hyphen
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_HyphenWithEnoughToTheRightTest()
        {
            var chars = "The qui-ckbrown fox";
            var len = chars.IndexOf("r") + 1; //there ARE enough characters on BOTH sides
            Assert.AreEqual("The qui-ckbr", chars.Substring(0, len));

            

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsTrue(opportunity.IsHyphenation); //

            //This is forced to split back at the hyphen

            var expected = "The qui-";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            //Assert.AreEqual('-', opportunity.AppendHyphenCharacter.Value);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.RemoveSplitWhiteSpace);

            //actual += opportunity.AppendHyphenCharacter.Value;

            Assert.AreEqual(expected, actual);

        }

        #endregion


        #region public void HypenateWord_NotOnANumberTest()

        /// <summary>
        ///A test for never splitting up a number
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotOnANumberTest()
        {
            var chars = "The quick -1.234 fox";
            var len = chars.IndexOf("2") + 1;
            Assert.AreEqual("The quick -1.2", chars.Substring(0, len));

            //we are on a space

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            var expected = "The quick";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion


        #region public void HypenateWord_NotOnANumberTest()

        /// <summary>
        ///A test for never splitting up a word with symbols in it 
        ///</summary>
        [TestMethod()]
        [TestCategory("Hyphenation")]
        public void HypenateWord_NotOnSymbolTest()
        {
            var chars = "The quick brow$n fox";
            var len = chars.IndexOf("o") + 1;
            Assert.AreEqual("The quick bro", chars.Substring(0, len));

            //we are on a space

            int minBefore = 3;
            int minAfter = 3;
            PDFHyphenationStrategy strategy = new PDFHyphenationStrategy('-', null, minBefore, minAfter);

            var opportunity = Scryber.PDF.Graphics.PDFHyphenationRule.HyphenateLine(strategy, chars, 0, len);

            Assert.IsFalse(opportunity.IsHyphenation);

            var expected = "The quick";
            var actual = chars.Substring(0, opportunity.NewLength);

            Assert.IsFalse(opportunity.AppendHyphenCharacter.HasValue);
            Assert.IsFalse(opportunity.PrependHyphenCharacter.HasValue);
            Assert.IsTrue(opportunity.RemoveSplitWhiteSpace);



            Assert.AreEqual(expected, actual);

        }

        #endregion

        //TODO: Add tests for mid strings and 

    }
}
