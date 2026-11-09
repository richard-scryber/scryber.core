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

namespace Scryber.Text
{
    /// <summary>
    /// Parses a block of text with XML elements for styles and classes
    /// </summary>
    [Obsolete("No longer used for parsing",true)]
    public class PDFXMLFragmentParser
    {

        //
        // const and static values 
        //

        public static char[] WhiteSpace = new char[] { ' ', '\r', '\n', '\t', '\x00a0', '\x0085' };
        private static string[] Replacements = new string[] { "&lt;", "&gt;", "&amp;","&quot;", "&apos;" };
        private static string[] Replacewith = new string[] { "<", ">", "&", "\"", "'" };
        private const string SpanElementName = "span";
        private const string BreakElementName = "br";
        private static string[] FontElements = new string[] { "b", "i", "sup", "sub", "u" };
        private const string ClassNameAttribute = "class";

        //
        // ivars
        //


        private Stack<string> _classstack = new Stack<string>();


        //
        // ctor
        //

        public PDFXMLFragmentParser()
        {
        }


        //
        // public methods
        //


        /// <summary>
        /// Takes the provided text and splits it into a list of 
        /// tokens that can be read in sequence to generate the text
        /// </summary>
        /// <param name="text">The full text to be read</param>
        /// <returns></returns>
        public virtual List<PDFTextOp> Parse(string text, bool preserveWhitespace)
        {
            List<PDFTextOp> all = new List<PDFTextOp>();
            int lastindex = 0;
            string normalized;
            string value;
            int index = text.IndexOf('<');
            bool first = true;
            while (index >= 0)
            {

                if (lastindex < index)
                {
                    //We have some text between the last point and the position of the <, so we need to normalise and add
                    value = text.Substring(lastindex, index - lastindex);
                    if (preserveWhitespace)
                    {
                        string[] lines = WhitespaceText(first, value);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string txt = lines[i];
                            if (i > 0)
                                all.Add(new PDFTextNewLineOp());
                            if (string.IsNullOrEmpty(txt))
                                all.Add(new PDFTextDrawOp(" "));
                            else
                                all.Add(new PDFTextDrawOp(txt));
                        }
                    }
                    else
                    {
                        normalized = NormalizeText(first, value);

                        if (!string.IsNullOrEmpty(normalized))
                            all.Add(new PDFTextDrawOp(normalized));
                    }

                }

                //check the validity of the element
                int endindex = text.IndexOf('>', index);

                if (endindex < 0) //Make sure it is closed
                    throw new PDFXmlFormatException("No closing brace found after opening brace - character #" + index.ToString());

                int nextindex = text.IndexOf('<', index + 1); //and make sure a new element is not opened before
                if (nextindex > 0 && endindex > nextindex)
                    throw new PDFXmlFormatException("No closing brace found after opening brace - character #" + index.ToString());

                value = text.Substring(index + 1, (endindex - index) - 1);

                //We have element content - parse and if valid add
                PDFTextOp ele = ParseElement(value);
                if (ele is PDFTextNewLineOp)
                    first = true;
                else
                    first = false;

                if (null != ele)
                    all.Add(ele);
                lastindex = endindex + 1;

                //check to see if there are some more elements
                if (lastindex < text.Length)
                    index = text.IndexOf('<', lastindex);
                else
                    index = -1;

            }

            //No more elements, so just make sure we cature any trailing text
            if (lastindex < text.Length)
            {
                value = text.Substring(lastindex);
                if (preserveWhitespace)
                {
                    string[] lines = WhitespaceText(first, value);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i > 0)
                            all.Add(new PDFTextNewLineOp());
                        string txt = lines[i];
                        if (string.IsNullOrEmpty(txt))
                            all.Add(new PDFTextDrawOp(" "));
                        else
                            all.Add(new PDFTextDrawOp(txt));
                    }
                }
                else
                {
                    normalized = NormalizeText(first, value);
                    if (!string.IsNullOrEmpty(normalized))
                    {
                        PDFTextDrawOp op = new PDFTextDrawOp(normalized);
                        all.Add(op);
                    }
                }
            }

            return all;
        }



        //
        // private implementation
        //        

        private string[] WhitespaceText(bool first, string text)
        {
            string[] split = text.Split('\n');
            for (int i = 0; i < split.Length; i++)
            {
                string value = split[i];
                if (value.IndexOf('&') > -1)
                {
                    for (int j = 0; j < Replacements.Length; j++)
                    {
                        value = value.Replace(Replacements[j], Replacewith[j]);
                    }
                    split[i] = value;
                }
                
            }
            return split;
        }

        /// <summary>
        /// Converts the string given into normalized text 
        /// (line breaks removed, white space trimmed, and encoded characters replaced).
        /// </summary>
        /// <param name="first">If true then any white space from the beginning is trimmed.</param>
        /// <param name="value">The text to normalise</param>
        /// <returns>The normalized string or null if there is no valuable text within the string</returns>
        private string NormalizeText(bool first, string value)
        {
            bool endsinspace = false;
            bool startinspace = true;

#if AlwaysRemoveFirstWhiteSpace
            if (first)
                value = value.TrimStart(WhiteSpace);
#endif

            if (value.Length == 0)
                return null;

            startinspace = (char.IsWhiteSpace(value, 0));
            endsinspace = (char.IsWhiteSpace(value, value.Length - 1));

            string[] all = value.Split(WhiteSpace, StringSplitOptions.RemoveEmptyEntries);
            value = string.Join(" ", all);

            if (string.IsNullOrEmpty(value))
                value = " ";
            else
            {
                if (endsinspace)
                    value = value + " ";
                if (startinspace)
                    value = " " + value;

                if (value.IndexOf('&') > -1)
                {
                    for (int i = 0; i < Replacements.Length; i++)
                    {
                        value = value.Replace(Replacements[i], Replacewith[i]);
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Converts the element between &lt; ... &gt; into a PDFTextOpof the correct type
        /// </summary>
        /// <param name="contents"></param>
        /// <returns>The PDFTextOp, or null if the element could not be parsed</returns>
        private PDFTextOp ParseElement(string contents)
        {

            if (contents.StartsWith("/"))
            {
                contents = contents.Substring(1).Trim();
                if (string.Equals(SpanElementName, contents, StringComparison.OrdinalIgnoreCase))
                {
                    string name = "";
                    if (_classstack.Count > 0)
                        name = _classstack.Pop();
                    return new PDFTextClassOp(name, false);
                }
                else if (Array.IndexOf<string>(FontElements, contents.ToLower()) > -1)
                {
                    return new PDFTextFontOp(contents.ToLower(), false);
                }
                else
                    return null;
            }
            else if (contents.EndsWith("/"))
            {
                contents = contents.Substring(0, contents.Length - 1).Trim();
                if (string.Equals(contents, BreakElementName, StringComparison.OrdinalIgnoreCase))
                {
                    return new PDFTextNewLineOp();
                }
                else
                    return null;
            }
            else if (contents.StartsWith(SpanElementName, StringComparison.OrdinalIgnoreCase))
            {
                string name = "";
                int index = contents.IndexOf(ClassNameAttribute);

                if (index > 0)
                {
                    char terminator = contents[ClassNameAttribute.Length + 1 + index];

                    index += ClassNameAttribute.Length + 2; //="
                    contents = contents.Substring(index);
                    index = contents.IndexOf(terminator);
                    if (index > 0)
                    {
                        name = contents.Substring(0, index);
                    }
                }
                _classstack.Push(name);
                return new PDFTextClassOp(name, true);
            }
            else if (Array.IndexOf<string>(FontElements, contents.ToLower()) > -1)
            {
                return new PDFTextFontOp(contents.ToLower(), true);
            }

            else
                return null;
        }
    }
}
