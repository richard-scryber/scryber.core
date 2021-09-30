/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scryber.PDF.Native
{
    internal static class PDFParserHelper
    {
        const string NullString = PDFNull.NullString;

        #region public static PDFNull ParseNull(string value, int offset, out int end)

        /// <summary>
        /// Parses null values within a string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static PDFNull ParseNull(string value, int offset, out int end)
        {
            AssertValidValue(value, offset);

            if (value.Length < offset + NullString.Length)
                throw new PDFNativeParserException(CommonErrors.InvalidNullString);
            else if (value.Substring(offset, NullString.Length) != NullString)
                throw new PDFNativeParserException(CommonErrors.InvalidNullString);
            else
            {
                end = offset + NullString.Length;
                return PDFNull.Value;
            }
        }

        #endregion

        #region public static PDFObjectRef ParseObjectRef(string value, int offset, out int end)

        /// <summary>
        /// Parses an object reference from the provided string starting at the specified offset.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end">Set to the position of the character after the parsed object ref</param>
        /// <returns></returns>
        public static PDFObjectRef ParseObjectRef(string value, int offset, out int end)
        {
            AssertValidValue(value, offset);

            string num = null;
            string gen = null;
            string r = null;

            StringBuilder sb = new StringBuilder(10);

            while (offset < value.Length)
            {
                char cur = value[offset];
                if (char.IsDigit(cur))
                {
                    sb.Append(cur);
                }
                else if (char.IsWhiteSpace(cur))
                {
                    if (string.IsNullOrEmpty(num))
                    {
                        num = sb.ToString();
                        sb.Clear();
                    }
                    else if (string.IsNullOrEmpty(gen))
                    {
                        gen = sb.ToString();
                        sb.Clear();
                    }
                    else throw new PDFNativeParserException(CommonErrors.InvalidObjectReferenceString);
                }
                else if (cur == 'R')
                {
                    r = "R";
                    offset++;
                    break;
                }
                else
                    throw new PDFNativeParserException(CommonErrors.InvalidObjectReferenceString);

                offset++;
            }

            if (string.IsNullOrEmpty(num) || string.IsNullOrEmpty(gen) || string.IsNullOrEmpty(r))
                throw new PDFNativeParserException(CommonErrors.InvalidObjectReferenceString);

            int inum = int.Parse(num);
            int igen = int.Parse(gen);

            end = offset;
            return new PDFObjectRef(inum, igen);
        }

        #endregion

        #region public static IFileObject ParseNumericValue(string value, int offset, out int end)

        /// <summary>
        /// Parses a numeric value from the string starting at the current offset (either a PDFNumber or a PDFReal value is returned) 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end">Set to the character position after the parsed number (or the end of the string)</param>
        /// <returns>The parsed file object that best represents the numeric value</returns>
        public static IFileObject ParseNumericValue(string value, int offset, out int end)
        {
            AssertValidValue(value, offset);

            bool hasdecimal = false;
            StringBuilder full = new StringBuilder();

            if (value[offset] == '-')
            {
                full.Append("-");
                offset++;
            }
            else if (value[offset] == '+')
            {
                offset++;
            }
            else if (value[offset] == '.')
            {
                hasdecimal = true;
                full.Append("0.");
                offset++;
            }
            else if (!char.IsDigit(value[offset]))
                throw new PDFNativeParserException("First character is not a valid number digit");

            bool lastwasdigit = false;

            while (offset < value.Length)
            {
                char c = value[offset];
                if (char.IsDigit(c))
                {
                    full.Append(value[offset]);
                    lastwasdigit = true;
                }
                else if (c == '.')
                {
                    if (hasdecimal)
                        throw new PDFNativeParserException("Too many decimal points in number");
                    hasdecimal = true;
                    lastwasdigit = false;
                    full.Append('.');
                }
                else if (char.IsWhiteSpace(c))
                {
                    break;
                }
                else if (lastwasdigit)
                    break;
                else
                    throw new PDFNativeParserException("Cannot have non numeric values in a number");

                offset++;
            }

            end = offset;

            if (hasdecimal)
                return new PDFReal(double.Parse(full.ToString()));
            else
                return new PDFNumber(long.Parse(full.ToString()));
        }

        #endregion

        #region public static PDFString ParseString(string value, int offset, out int end)

        /// <summary>
        /// Parses the value starting at the specified offset, into a valid PDFString.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static PDFString ParseString(string value, int offset, out int end)
        {
            AssertValidValue(value, offset);

            if (value[offset] == '(')
                return ParseParenthesizedString(value, offset, out end);
            else if (value[offset] == '<')
                return ParseHexadecimalString(value, offset, out end);
            else
                throw new PDFNativeParserException(CommonErrors.PDFStringDoesNotStartWithRequiredCharacter);

        }

        #endregion

        #region public static PDFName ParseName(string value, int startoffset, out int end)

        private const char NameStartCharacter = '/';
        private const char HexStartChar = '#';

        private static char[] _invalidnamechars = new char[] { '[', ']', '%', '\\', '<', '>', '/', '(', ')' };

        /// <summary>
        /// Parses a single PDFName from the string value, starting at the specified offset.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static PDFName ParseName(string value, int startoffset, out int end)
        {
            AssertValidValue(value, startoffset);

            int offset = startoffset;
            if (value[offset] != NameStartCharacter)
                throw new PDFNativeParserException(CommonErrors.PDFNameDoesNotStartWithSlash);
            else
            {
                StringBuilder buffer = new StringBuilder();
                offset++;
                while (offset < value.Length)
                {
                    char c = value[offset];
                    if (char.IsWhiteSpace(c))
                    {
                        break;
                    }
                    else if (c == HexStartChar)
                    {
                        c = GetHexChar(value, offset + 1, out offset);
                    }
                    else if (IsInvalidNameChar(c))
                    {
                        if (buffer.Length > 0)
                            break;
                        else
                            throw new PDFNativeParserException(CommonErrors.InvalidPDFName);
                    }

                    buffer.Append(c);
                    offset++;
                }

                end = offset;

                if (startoffset >= end)
                    throw new PDFNativeParserException(CommonErrors.InvalidPDFName);

                return new PDFName(buffer.ToString());
            }
        }

        #endregion

        #region public static PDFBoolean ParseBoolean(string value, int offset, out int end)

        /// <summary>
        /// Parses a boolean value from the specified string value starting at the specified offset.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static PDFBoolean ParseBoolean(string value, int offset, out int end)
        {
            AssertValidValue(value, offset);

            if (value[offset] == 't')
            {
                AssertCharacter(value, offset + 1, 'r');
                AssertCharacter(value, offset + 2, 'u');
                AssertCharacter(value, offset + 3, 'e');
                end = offset + 4;
                return new PDFBoolean(true);
            }
            else if (value[offset] == 'f')
            {
                AssertCharacter(value, offset + 1, 'a');
                AssertCharacter(value, offset + 2, 'l');
                AssertCharacter(value, offset + 3, 's');
                AssertCharacter(value, offset + 4, 'e');
                end = offset + 5;
                return new PDFBoolean(false);
            }
            else
                throw new PDFNativeParserException(CommonErrors.CouldNotParseBooleanValue);
        }

        #endregion

        #region public static PDFDictionary ParseDictionary(string value, int offset, out int end)

        private const char DictionaryStartChar = '<';
        private const char DictionaryEndChar = '>';

        public static PDFDictionary ParseDictionary(string value, int offset, out int end)
        {
            AssertValidValue(value, offset + 1);
            if (value[offset] != DictionaryStartChar)
                throw new PDFNativeParserException(CommonErrors.DictionaryDoesNotStartWithRequiredChar);
            if (value[offset + 1] != DictionaryStartChar)
                throw new PDFNativeParserException(CommonErrors.DictionaryDoesNotStartWithRequiredChar);
            PDFDictionary dict = new PDFDictionary();

            offset += 2;
            while (offset < value.Length)
            {
                char c = value[offset];
                if (char.IsWhiteSpace(c))
                    offset++;
                else if (c == NameStartCharacter)
                {
                    PDFName name = ParseName(value, offset, out end);
                    offset = end;
                    IFileObject obj = InferAndParseNextObject(value, offset, out end);
                    dict[name] = obj;
                    offset = end;
                }
                else if (c == DictionaryEndChar)
                {
                    if (value.Length > offset + 1 && value[offset + 1] == DictionaryEndChar)
                    {
                        offset += 2;
                        break;
                    }
                    else
                        throw new PDFNativeParserException(CommonErrors.DictionaryDoesNotEndWithRequiredChar);
                }
                else
                    throw new PDFNativeParserException(CommonErrors.AllDictionaryKeysMustBePDFNames);
            }
            end = offset;
            return dict;
        }

        #endregion

        #region public static PDFArray ParseArray(string value, int offset, out int end)

        private const char ArrayStartChar = '[';
        private const char ArrayEndChar = ']';

        public static PDFArray ParseArray(string value, int offset, out int end)
        {
            AssertValidValue(value, offset + 1);
            if (value[offset] != ArrayStartChar)
                throw new PDFNativeParserException(CommonErrors.ArrayDoesNotStartWithRequiredChar);

            PDFArray arry = new PDFArray();
            bool ended = false;

            offset += 1;
            while (offset < value.Length)
            {
                char c = value[offset];

                if (char.IsWhiteSpace(c))
                    offset++;

                else if (c == ArrayEndChar)
                {
                    offset += 1;
                    ended = true;
                    break;
                }
                else
                {
                    IFileObject obj = InferAndParseNextObject(value, offset, out end);
                    arry.Add(obj);
                    offset = end;
                }
            }
            if (!ended)
                throw new PDFNativeParserException(CommonErrors.ArrayDoesNotEndWithRequiredChar);

            end = offset;
            return arry;
        }

        #endregion

        //
        // support methods
        //


        internal static IFileObject InferAndParseNextObject(string value, int offset, out int end)
        {
            while (char.IsWhiteSpace(value, offset))
            {
                offset++;
            }
            char first = value[offset];
            switch (first)
            {
                case '(':
                    return ParseString(value, offset, out end);

                case '<':
                    if (value[offset + 1] == '<')
                        return ParseDictionary(value, offset, out end);
                    else
                        return ParseString(value, offset, out end);

                case '[':
                    return ParseArray(value, offset, out end);

                case '/':
                    return ParseName(value, offset, out end);

                case 't':
                case 'f':
                    return ParseBoolean(value, offset, out end);

                case 'n':
                    return ParseNull(value, offset, out end);
                case '%':
                    //Comment
                    end = offset + 1;
                    while (end < value.Length)
                    {
                        if (value[end] == '\r' || value[end] == '\n')
                            break;
                        end++;
                    }
                    return InferAndParseNextObject(value, end, out end);
                default:

                    //all we are left with are Number, Real, ObjectRef (all start with a number)

                    IFileObject num = ParseNumericValue(value, offset, out end);

                    if (end >= value.Length || num.Type == PDFObjectTypes.Real)
                        return num;

                    if (char.IsWhiteSpace(value, end) && char.IsDigit(value[end + 1]))
                    {
                        int tempoffset = end + 1;
                        int tempend;

                        IFileObject num2 = ParseNumericValue(value, tempoffset, out tempend);
                        if (num2.Type == PDFObjectTypes.Number && char.IsWhiteSpace(value, tempend) && value[tempend + 1] == 'R')
                        {
                            return ParseObjectRef(value, offset, out end);
                        }
                    }
                    return num;
            }
        }


        private static void AssertCharacter(string value, int offset, char required)
        {
            if ((value.Length <= offset) || (value[offset] != required))
                throw new PDFNativeParserException(CommonErrors.CouldNotParseBooleanValue);

        }

        private static void AssertValidValue(string value, int offset)
        {
            if (string.IsNullOrEmpty(value))
                throw new PDFNativeParserException(CommonErrors.CannotParseNullString);
            else if (offset >= value.Length)
                throw new PDFNativeParserException(CommonErrors.CannotParsePastTheEndOfTheString);
            else if (offset < 0)
                throw new PDFNativeParserException(CommonErrors.CannotParsePastTheEndOfTheString);
        }

        private static bool IsInvalidNameChar(char c)
        {

            for (int i = 0; i < _invalidnamechars.Length; i++)
            {
                if (_invalidnamechars[i] == c)
                    return true;
            }

            return false;
        }

        private static char GetHexChar(string value, int start, out int end)
        {
            string sub = value.Substring(start, 2);
            int c = int.Parse(sub, System.Globalization.NumberStyles.HexNumber);
            end = start + 1;
            return (char)c;
        }

        private static PDFString ParseParenthesizedString(string value, int offset, out int end)
        {
            StringBuilder sb = new StringBuilder();
            int depth = 1;
            offset += 1;
            while (offset < value.Length)
            {
                char c = value[offset];
                if (c == '(')
                {
                    depth++;
                    sb.Append('(');
                    offset++;
                }
                else if (c == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        end = offset + 1;
                        return new PDFString(sb.ToString());
                    }
                    sb.Append(')');
                    offset++;
                }
                else if (c == '\\')
                {
                    c = ParseEscapeChar(value, offset + 1, out offset);
                    if (c != char.MinValue)
                        sb.Append(c);
                }
                else
                {
                    sb.Append(c);
                    offset++;
                }

            }
            throw new PDFNativeParserException(CommonErrors.PDFStringHasUnbalancedParenthese);
        }

        private static char ParseEscapeChar(string value, int start, out int end)
        {
            char c = value[start];
            switch (c)
            {
                case ('\r'):
                case ('\n'):
                    //single escape char of '\' ignore following line breaks
                    end = start;
                    while (char.IsWhiteSpace(value, end))
                    {
                        end++;
                    }
                    return char.MinValue;

                case 'n':
                    end = start + 1;
                    return '\n';

                case 'r':
                    end = start + 1;
                    return '\r';

                case 'b':
                    end = start + 1;
                    return '\b';

                case 't':
                    end = start + 1;
                    return '\t';

                case 'f':
                    end = start + 1;
                    return '\f';

                case '(':
                    end = start + 1;
                    return '(';

                case ')':
                    end = start + 1;
                    return ')';

                case '\\':
                    end = start + 1;
                    return '\\';

                default:
                    if (char.IsDigit(c))
                    {
                        int len = 1;
                        if (char.IsDigit(value, start + 1))
                            len = 2;
                        if (char.IsDigit(value, start + 2))
                            len = 3;
                        string octal = "\\" + value.Substring(start, len);
                        int p = int.Parse(octal);
                        end = start + len;
                        return (char)p;
                    }
                    throw new PDFNativeParserException(CommonErrors.InvalidPDFStringEscapeSequence);
            }
        }

        private static PDFString ParseHexadecimalString(string value, int offset, out int end)
        {
            StringBuilder sb = new StringBuilder();
            offset += 1;
            while (offset < value.Length)
            {
                if (value[offset] == '>')
                {
                    end = offset + 1;
                    return new PDFString(sb.ToString());
                }
                string one = value.Substring(offset, 2);
                if (one.EndsWith(">"))
                {
                    one = one.Substring(0, 1) + "0";
                    char c = (char)int.Parse(one, System.Globalization.NumberStyles.HexNumber);
                    sb.Append(one);
                    end = offset;
                    return new PDFString(sb.ToString());
                }
                else
                {
                    char c = (char)int.Parse(one, System.Globalization.NumberStyles.HexNumber);
                    sb.Append(c);
                    offset += 2;
                }
            }
            throw new PDFNativeParserException(CommonErrors.PDFStringHasUnbalancedParenthese);
        }

    }

}
