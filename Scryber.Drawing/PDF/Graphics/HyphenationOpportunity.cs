using System;
namespace Scryber.PDF.Graphics
{

    public enum HyphenationType
    {
        AllowedHyphenation = 1, //The strategy works for the proposal and the hyphenation is allowed at the suggested location.
        NoHyphenation = 2 //The strategy will not hyphenate any words.
    }


    /// <summary>
    /// Defines the result of a proposed hyphenation as calculated by the PDFHyphenationRule checker.
    /// </summary>
    public struct HyphenationOpportunity
    {
        /// <summary>
        /// If true then hyphenation is allowed, if false then it should be a word break
        /// </summary>
        public bool IsHyphenation { get; private set; }

        /// <summary>
        /// Set to the new length where the hyphen or the word break should occur
        /// </summary>
        public int NewLength { get; private set; }

        /// <summary>
        /// The character, if any, that should be appended to the split for hyphenation.
        /// </summary>
        public char? AppendHyphenCharacter { get; private set; }

        /// <summary>
        /// The character, if any, that should be pre-pended onto the new line for a hyphenation split
        /// </summary>
        public char? PrependHyphenCharacter { get; private set; }

        /// <summary>
        /// If true then a trim of the strings should be done, to remove white space. If this is no longer hyphenated,
        /// then a new line will have an extra space, so it should be removed.
        /// </summary>
        public bool RemoveSplitWhiteSpace { get; private set; }


        public HyphenationOpportunity(bool hyphenate, int length, char? append, char? prepend, bool trim)
        {
            this.IsHyphenation = hyphenate;
            this.NewLength = length;
            this.AppendHyphenCharacter = append;
            this.PrependHyphenCharacter = prepend;
            this.RemoveSplitWhiteSpace = trim;
        }

        
    }
}

