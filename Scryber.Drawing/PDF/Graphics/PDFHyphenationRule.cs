using System;
using System.Globalization;

namespace Scryber.PDF.Graphics
{

	


	public static class PDFHyphenationRule
	{

		/// <summary>
		/// Hypenates a line based on the culture rules and returns the opportunity for hyphenation.
		/// </summary>
		/// <param name="culture"></param>
		/// <param name="chars"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <param name="appendhyphen"></param>
		/// <param name="prependHypenOnNewLine"></param>
		/// <returns></returns>
		public static HyphenationOpportunity HyphenateLine(string chars, int start, int length)
		{

			PDFHyphenationStrategy strategy = PDFHyphenationStrategy.Default;
			return HyphenateLine(strategy, chars, start, length);
		}


		public static HyphenationOpportunity HyphenateLine(PDFHyphenationStrategy strategy, string chars, int start, int length)
		{
			HyphenationOpportunity opportunity;

			//first check is to make sure we can split the word up
			if (CheckForSymbolOrDigits(chars, start, length))
			{
				//return the first space, ignoring any hyphens
				var len = GetFirstWordBoundaryBefore(chars, start, length, null, null);
				return new HyphenationOpportunity(
					hyphenate: false,
					length: len,
					append: null,
					prepend: null,
					trim: true
					);
			}
			
			var minAfter = strategy.MinCharsAfterHyphen;
			if (strategy.HyphenPrepend.HasValue)
				minAfter++; //We have a hypen on the new line as well so add one to the min chars after
							//(e.g. break min is 3, need space for a - and 3 chars)

			var minBefore = strategy.MinCharsBeforeHyphen;

			if (strategy.HyphenAppend.HasValue)
				minBefore++; //we have a hyphen before, so add one to the chars before
							 //(e.g break min is 3, need space for 3 and a -)

			var right = SplitForRight(chars, start, length, minAfter);

			if (right == length)
            {
                //we are OK to split on the current position based on looking right min chars after the break.


                //first check if there is a hyphen on the current word before the needed split
                //if so then we should always use that.

                var hyphen = CheckWordForHyphen(chars, start, length, strategy.HyphenAppend, strategy.HyphenPrepend);
				if (hyphen != 0)
				{
					if (strategy.HyphenAppend.HasValue)
					{
						return new HyphenationOpportunity(
							hyphenate: true,
							length: hyphen + 1, //we include the hyphen to split on
							append: null, //And don't append a character
							prepend: strategy.HyphenPrepend,
							trim: false
							);
					}
					else //it is just the pre-pend
					{
                        return new HyphenationOpportunity(
                            hyphenate: true,
                            length: hyphen, //we DO NOT include the hyphen to split on
                            append: null, //We know this is already null
                            prepend: null, //And don't prepend a character
                            trim: false
                            );
                    }
				}

                
                //need to check the left


                var left = SplitForLeft(chars, start, length, minBefore);

				if (left == 0) //TESTED
				{
					//our characters are too short to split, so return not allowed
					return new HyphenationOpportunity(

						hyphenate: false,
						length: left,
						append: null,
						prepend: null,
						trim: false
					);
				}
				else if (left == length) //TESTED
				{
					//We are OK to split here based on the right as well as the left.
					//but check for white space. If we are a whitespace then not hyphen, otherwise yes
					var c = chars[length];
					if (char.IsWhiteSpace(c)) //TESTED
					{
						return new HyphenationOpportunity(
							hyphenate: false,
							length: length,
							append: null,
							prepend: null,
							trim: true
						);
					}
					else //TESTED
					{
						return new HyphenationOpportunity(
							hyphenate: true,
							append: strategy.HyphenAppend,
							length: length,
							prepend: strategy.HyphenPrepend,
							trim: false
						);
					}
				}
				else if (left < length) //TESTED
				{
					//There is a word boundary before the min chars - so no hyphen, and break on that length.
					return new HyphenationOpportunity(
						hyphenate: false,
						length: left,
						append: null,
						prepend: null,
						trim: true
					);
				}
				else
					throw new ArgumentOutOfRangeException("The strategy failed to find a split. And returned an unknown value");
			}
			else if (right + start == chars.Length)
			{
				//Not enough characters at the end
				//And there are no white spaces after the proposed split so at the end.
				//We don't hyphenate on the last word for a line - so go back to the first word space and break there
				var left = GetFirstWordBoundaryBefore(chars, start, chars.Length - 1 - start, strategy.HyphenAppend, strategy.HyphenPrepend);

				if (left == start)
				{
					//There are no spaces or hyphens in the entire string
					//so if our single word length is greater than the total allowed, break on the right length.

					if (length > (minBefore + minAfter))
					{
						return new HyphenationOpportunity(
							hyphenate: true,
							length: chars.Length - minAfter - start,
							append: strategy.HyphenAppend,
							prepend: strategy.HyphenPrepend,
							trim: false
						);
					}
					else
					{
						//our word is smaller than the total allowed so don't break and force a new line for all of it.
						return new HyphenationOpportunity(
							hyphenate: false,
							length: 0,
							append: null,
							prepend: null,
							trim: false
						);
					}
				}
				else
				{
					//We have a white space character before, so go ahead and move that down to the next line.
					return new HyphenationOpportunity(
						hyphenate: false, //Not hyphenating
						length: left - start, //This is our new break point
						append: null, //So no char at end
						prepend: null, //Or at the front of the new line
						trim: true //And no white space trimming
					);
				}
			}
			else if (right == length - 1) //TESTED
			{
				//We are on a white space
				//so we can split there without a hyphen (trimming after)

				return new HyphenationOpportunity(
					hyphenate: false, //Not hyphenating
					length: right, //This is our new break point
					append: null, //So no char at end
					prepend: null, //Or at the front of the new line
					trim: true //And no white space trimming
				);
			}
			else if (right >= length)
			{
				//we don't have enough characters beyond the proposed length
				//so we need to go back enough to split.
				var left = GetFirstWordBoundaryBefore(chars, start, length, strategy.HyphenAppend, strategy.HyphenPrepend);

				if (left == start) //TESTED
				{
					//No spaces or hyphens found
					return new HyphenationOpportunity(
						hyphenate: false,
						length: 0,
						append: null,
						prepend: null,
						trim: false
					);
				}
				else if (right - left >= (minBefore + minAfter)) //TESTED
				{
					//we pick the best position to split on which will be minAfter
					//as it most approximates the original desired position
					left = (length - minAfter) + 1;

					return new HyphenationOpportunity(
						hyphenate: true,
						length: left,
						append: strategy.HyphenAppend,
						prepend: strategy.HyphenPrepend,
						trim: false //Not a newline on a space
					);
				}
				else if (strategy.HyphenAppend.HasValue && chars[left + start] == strategy.HyphenAppend.Value)
				{
					//we are on a hyphen in the word so split on that
					return new HyphenationOpportunity(
						hyphenate: true,
						length: left + 1, //include the hyphen as part of the string
						append: null, // because there is already one on the string
						prepend: strategy.HyphenPrepend,
						trim: false
						);
				}
				else if (strategy.HyphenPrepend.HasValue && chars[start + left] == strategy.HyphenPrepend.Value)
				{
					throw new NotImplementedException();
				}
				else //TESTED
				{
					//it is just the first word boundary found

					return new HyphenationOpportunity(
						hyphenate: false,
						length: left,
						append: null,
						prepend: null,
						trim: true
					);
				}
			}
			else
				throw new ArgumentOutOfRangeException("The strategy failed to find a split. And returned an unknown value of " + right + " for a length of " + length);

		}

        private static bool CheckForSymbolOrDigits(string chars, int start, int len)
        {
			int pos = start + len;
			while (pos >= start)
			{
				var c = chars[pos];
				if (char.IsSymbol(c) || char.IsNumber(c))
					return true;
				else if (IsWordBreakChar(c))
					break;
				pos--;
			}
			pos = start + len + 1;
			while (pos < chars.Length)
			{
                var c = chars[pos];
                if (char.IsSymbol(c) || char.IsNumber(c))
                    return true;
                else if (IsWordBreakChar(c))
                    break;

				pos++;
            }

			return false;
        }

        /// <summary>
        /// Traverses back up the string from the offset to find an existing hyphen based on the provided characters.
        /// </summary>
        /// <param name="chars">The characters to check</param>
        /// <param name="start">The first character in the string that marks the chosen characters starting position</param>
        /// <param name="offset">The offset in the string to start looking for a hyphen</param>
        /// <param name="hyphenAppend">An optional character to denote the hyphen that would be split on</param>
        /// <param name="hyphenPrepend">An optional character to denote the hyphen that would be split afterwards</param>
        /// <returns>Zero (0) if no hyphen was found. Or the position of the hyphen</returns>
        private static int CheckWordForHyphen(string chars, int start, int len, char? hyphenAppend, char? hyphenPrepend)
        {
			var offset = start + len;
            while (offset > start) //ignore the very first character
            {
                var c = chars[offset];
                if (hyphenAppend.HasValue && c.Equals(hyphenAppend.Value))
                    return (offset - start); //we have a hyphen that matches and should be at the end of the first string.
                else if (hyphenPrepend.HasValue && c.Equals(hyphenPrepend.Value))
                    return (offset - start); //we have a hyphen that matches and should be at the beginning of the second string.
                offset--;
            }
            //no word boundary
            return 0;
        }


        /// <summary>
        /// Traverses back up the string from the offset returning the index (back up from the start position) of
        /// the first occuring whitespace character in the string, or the start offset if there is none
        /// </summary>
        /// <param name="chars">The string to search</param>
        /// <param name="start">The offset in the string that marks the chosen characters starting position</param>
        /// <param name="offset">The offset in the string to start looking for a white space from relative to the start of the string</param>
        /// <param name="hyphenAppend">An optional character to denote the hyphen that would be split on</param>
		/// <param name="hyphenPrepend">An optional character to denote the hyphen that would be split afterwards</param>
		/// <returns>The position in the string (from the start position) of the first space, or the start index itself if none is not found</returns>
        public static int GetFirstWordBoundaryBefore(string chars, int start, int len, char? hyphenAppend, char? hyphenPrepend)
		{
			var offset = start + len;
			while (offset > start) //ignore the very first character
			{
				var c = chars[offset];
				if (IsWordBreakChar(c))
					return offset - start;
				else if (hyphenAppend.HasValue && c.Equals(hyphenAppend.Value))
					return (offset - start); //we have a hyphen that matches and should be at the end of the first string.
				else if (hyphenPrepend.HasValue && c.Equals(hyphenPrepend.Value))
					return (offset - start); //we have a hyphen that matches and should be at the beginning of the second string.
                offset--;
			}
			//no word boundary
			return start;

		}


        /// <summary>
        /// Checks the number of characters before a proposed split to make sure it is ok to hypenate.
        /// </summary>
        /// <param name="chars">The character string to check</param>
        /// <param name="start">The start of the string within the characters to check</param>
        /// <param name="proposedSplitLength">The length after the start where the proposed split would be</param>
        /// <param name="minCharsLeft">The minimum number of characters before the proposed split there must be to allow that split</param>
        /// <returns>Returns 0 is the proposed length is less than the minimum length,
        /// or the length for the first white space if there is one found before the minimum number of characters,
        /// or the proposed split length as it is on or above the minimum number of characters to split on</returns>
        public static int SplitForLeft(string chars, int start, int proposedSplitLength, int minCharsLeft)
		{
			int result = -1;

			//check that we are not a shorter string than the minimum
			if (proposedSplitLength < minCharsLeft)
				return 0;

			var offset = start + proposedSplitLength - 1;
			for (var i = 0; i < minCharsLeft; i++)
			{
				var c = chars[offset - i];
                if (IsWordBreakChar(c))
                {
					//As we are white space - return this length
					result = offset - i - start;
                    return result;
                }
            }

			//we have enough characters before the start of a word
			result = proposedSplitLength;
			return result;
					
		}


        /// <summary>
        /// Checks to see if there are enough characters to the right of the proposed split.
        /// If so then returns the proposed split length. If not then returns the offset of the
        /// last character before a space, AFTER the proposed split.
        /// </summary>
        /// <param name="chars">The characters to check</param>
        /// <param name="start">The starting offset in the characters</param>
        /// <param name="proposedSplitLength">The the length of the string at where is being proposed to split</param>
        /// <param name="minCharsRight">The minimum number of characters allowed to the right in a word where it will be split. </param>
        /// <returns>Either the current proposed split length if its ok, the length -1
        /// if it is currently a whitespace, or the offset before the first space to the right beyond the proposed split,
		/// or the end of the string if there are no spaces</returns>
        public static int SplitForRight(string chars, int start, int proposedSplitLength, int minCharsRight)
        {
            int result = -1;

            var offset = start + proposedSplitLength;

			if (offset >= chars.Length)
				return proposedSplitLength;

			if (offset == start)
				return proposedSplitLength;

			if (IsWordBreakChar(chars[offset - 1]))
				return proposedSplitLength - 1;

            for (var i = 0; i < minCharsRight; i++)
            {
				if (offset + i >= chars.Length)
				{
					//past the end of the string - return the end
					result = chars.Length - start;
					return result;
				}
                var c = chars[offset + i];
                if (IsWordBreakChar(c))
                {
                    //As we are white space - return this length
                    result = offset + i - start;
                    return result;
                }
            }

            //we have enough characters before the start of a word
            result = proposedSplitLength;
            return result;

        }

		private static bool IsWordBreakChar(char c)
		{
			return char.IsWhiteSpace(c);
		}
    }
}

