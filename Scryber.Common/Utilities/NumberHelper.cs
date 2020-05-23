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

namespace Scryber.Utilities
{
    public static class NumberHelper
    {
        public static string GetRomanUpper(int value)
        {
            StringBuilder sb = new StringBuilder();
            GenerateNumber(sb, 1000, 'M', ref value);
            GenerateNumber(sb, 500, 'D', ref value);
            GenerateNumber(sb, 100, 'C', ref value);
            GenerateNumber(sb, 50, 'L', ref value);
            GenerateNumber(sb, 10, 'X', ref value);
            GenerateNumber(sb, 5, 'V', ref value);
            GenerateNumber(sb, 1, 'I', ref value);
            ReplaceRomanShortcuts(sb, true);
            return sb.ToString();
        }

        public static string GetRomanLower(int value)
        {
            StringBuilder sb = new StringBuilder();
            GenerateNumber(sb, 1000, 'm', ref value);
            GenerateNumber(sb, 500, 'd', ref value);
            GenerateNumber(sb, 100, 'c', ref value);
            GenerateNumber(sb, 50, 'l', ref value);
            GenerateNumber(sb, 10, 'x', ref value);
            GenerateNumber(sb, 5, 'v', ref value);
            GenerateNumber(sb, 1, 'i', ref value);
            ReplaceRomanShortcuts(sb, false);
            return sb.ToString();
        }

        private static void ReplaceRomanShortcuts(StringBuilder sb, bool upper)
        {
            if (upper)
                sb.Replace("IIII", "IV").Replace("VIV", "IX").Replace("XXXX", "XL").Replace("LXL", "XC").Replace("CCCC", "CD").Replace("DCD", "CM");
            else
                sb.Replace("iiii", "iv").Replace("viv", "ix").Replace("xxxx", "xl").Replace("lxl", "xc").Replace("cccc", "cd").Replace("dcd", "cm");
        }

        private static void GenerateNumber(StringBuilder sb, int magnitude, char letter, ref int page)
        {
            while (page >= magnitude)
            {
                page -= magnitude;
                sb.Append(letter);
            }
        }


        private const char UpperCharStart = (char)(((int)'A') - 1);
        private const char LowerCharStart = (char)(((int)'a') - 1);

        public static string GetLetterUpper(int value)
        {
            StringBuilder sb = new StringBuilder();
            GenerateLetters(sb, value, UpperCharStart);
            return sb.ToString();
        }

        public static string GetLetterLower(int value)
        {
            StringBuilder sb = new StringBuilder();
            GenerateLetters(sb, value, LowerCharStart);
            return sb.ToString();
        }

        private static void GenerateLetters(StringBuilder sb, int index, char first)
        {
            if (index == 0)
                return;
            int pad = 1;
            while (index > 26)
            {
                index -= 26;
                pad++;
            }
            char charval = (char)(((int)first) + index);
            sb.Insert(0, charval.ToString(), pad);
        }

        public static string GetPointValue(int depth)
        {
            string val = "•";
            return val;

            //Not used
            //int mod = depth % 3;
            //switch (mod)
            //{
            //    case 2:
            //        val = ((char)0x00E0).ToString();//"▪";
            //        break;
            //    case 1:
            //        val = ((char)0x006F).ToString();//"◦";
            //        break;
            //    default:
            //        val = ((char)149).ToString();//"•";
            //        break;
            //}
            //return val;
        }
    }
}
