using System;
namespace Scryber.PDF.Graphics
{
	public class PDFHyphenationStrategy
	{
        /// <summary>
        /// Gets the optional character that should appear at the end of a hypenated line.
        /// </summary>
        public char? HyphenAppend { get; private set; }

        /// <summary>
        /// Gets the optional character that should appear at the beginning of a new hypenated line.
        /// </summary>
        public char? HyphenPrepend { get; private set; }

        /// <summary>
        /// Gets the minimum length of the a word that can be hypenated
        /// </summary>
        ///public int MinWordLength { get; private set; }

        /// <summary>
        /// Gets the minimun length of characters that form the start of a word before it can be hypenated
        /// </summary>
        public int MinCharsBeforeHyphen { get; private set; }

        /// <summary>
        /// Gets the minumum length of characters that form the End of a word before it can be hypenated
        /// </summary>
        public int MinCharsAfterHyphen { get; private set; }

        /// <summary>
        /// Returns true if the char.IsWhitespace method should be used, otherwise the check against the
        /// WhiteSpaceChars values should be performs to evaluate if a character is actually white space
        /// </summary>
        public bool UseIsWhitespace{ get { return true; } }

        
        public PDFHyphenationStrategy()
            : this('-', null, 3, 2)
        {
        }

        public PDFHyphenationStrategy(char? append, char? prepend, int minCharsbefore, int minCharsAfter)
        {
            HyphenAppend = append;
            HyphenPrepend = prepend;
            //MinWordLength = minlength;
            MinCharsBeforeHyphen = minCharsbefore;
            MinCharsAfterHyphen = minCharsAfter;
            //Match = null;
            //WhiteSpaceChars = null;
        }


        //
        // Default
        //

        public static readonly PDFHyphenationStrategy Default = new PDFHyphenationStrategy();

    }
}

