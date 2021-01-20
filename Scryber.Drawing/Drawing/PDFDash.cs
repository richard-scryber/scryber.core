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
using System.Text;
using Scryber.Native;
using System.CodeDom;
using System.ComponentModel;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PDFDash
    {
        private PDFNumber[] _pattern;

        public PDFNumber[] Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

        private PDFNumber _len;

        public PDFNumber Phase
        {
            get { return _len; }
            set { _len = value; }
        }

        public PDFDash()
            : this(new PDFNumber[] { }, PDFNumber.Zero) //solid
        {
        }

        public PDFDash(int[] pattern, int phase)
            : this(ConvertToNumberArray(pattern), ConvertToNumber(phase))
        {
        }

        private static PDFNumber ConvertToNumber(int len)
        {
            return (PDFNumber)len;
        }

        private static PDFNumber[] ConvertToNumberArray(int[] pattern)
        {
            if (null == pattern || pattern.Length == 0)
                return new PDFNumber[] { };
            else
            {
                PDFNumber[] all = new PDFNumber[pattern.Length];
                for (int i = 0; i < pattern.Length; i++)
                {
                    all[i] = ConvertToNumber(pattern[i]);
                }
                return all;
            }
        }

        
        public PDFDash(PDFNumber[] pattern, PDFNumber len)
        {
            this._pattern = pattern;
            this._len = len;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append("[");

            if (this.Pattern != null)
            {
                for (int i = 0; i < this.Pattern.Length; i++)
                {
                    if (i > 0)
                        sb.Append(" ");
                    sb.Append(this.Pattern[i]);
                }
            }
            sb.Append("] ");
            sb.Append(this.Phase);
            
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals((PDFDash)obj);
        }

        public bool Equals(PDFDash dash)
        {
            if (this._pattern == null && dash._pattern == null)
                return this._len == dash._len;
            else if (this._pattern.Length == dash._pattern.Length)
            {
                for (int i = 0; i < this._pattern.Length; i++)
                {
                    if (this._pattern[i] != dash._pattern[i])
                        return false;
                }
                return this._len == dash._len;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            if (null == this._pattern)
                return this._len.GetHashCode();
            else
            {
                int count = 0;
                foreach (PDFNumber num in this._pattern)
                {
                    count = count ^ num.GetHashCode();
                }
                return count ^ this._len.GetHashCode();
            }
        }

        public static PDFDash None
        {
            get { return new PDFDash(); }
        }

        public static PDFDash Parse(string dashpattern)
        {
            
            PDFDash dash = null;
            if(string.IsNullOrEmpty(dashpattern))
            {
                return dash;
            }
            dashpattern = dashpattern.Trim();

            if (dashpattern.StartsWith("[") && dashpattern.IndexOf("]") > 0)
            {
                dash = ParseExplicit(dashpattern);
            }

            else if (PDFDashes.TryGetNamedDash(dashpattern, out dash) == false)
            {
                if(!TryParseDashNumberList(dashpattern, out dash))
                    throw new ArgumentException("The dash pattern '" + dashpattern + "' could not be parsed. The format of a custom dash phase should be '[n n ...] n' or '[n,n,n] n', or one of hte known named patterns.");
            }
            return dash;
        }

        private static PDFDash ParseExplicit(string dashpattern)
        {
            int end = dashpattern.IndexOf(']');

            string pattern = dashpattern.Substring(1, end - 1).Trim();
            string phase = dashpattern.Substring(end + 1).Trim();
            
            List<int> val = new List<int>();
               
            if (pattern.Length > 0)
            {
                string[] array = pattern.Split(' ',',');
                foreach (string a in array)
                {
                    int i;
                    if (string.IsNullOrEmpty(a) == false)
                    {
                        if (int.TryParse(a.Trim(), out i) == false)
                            throw new FormatException("The format of a custom dash phase should be '[n n ...] n' or '[n,n,n] n' or 'n,n,n,n'. Could not understand the format '" + dashpattern + "'");
                        val.Add(i);
                    }
                }
            }
            int p;
            if(int.TryParse(phase, out p) == false)
                throw new FormatException("The format of a custom dash phase should be '[n n ...] n' or [n,n,n] n. Could not understand the format '" + dashpattern + "'");
            
            List<PDFNumber> nums = new List<PDFNumber>(val.Count);
            foreach (int i in val)
            {
                nums.Add(new PDFNumber(i));
            }
            return new PDFDash(nums.ToArray(), new PDFNumber(p));


        }

        public static bool TryParseDashNumberList(string value, out PDFDash dash)
        {
            bool result = false;
            dash = null;

            var entries = value.Split(',', ' ');
            int total = 0;
            List<PDFNumber> items = new List<PDFNumber>(entries.Length);
            foreach (var a in entries)
            {
                if(string.IsNullOrEmpty(a) == false)
                {
                    int i;
                    if (int.TryParse(a.Trim(), out i))
                    {
                        items.Add(new PDFNumber(i));
                        total += i;
                    }
                    else
                        throw new FormatException("The format of a custom dash phase should be '[n n ...] n' or '[n,n,n] n' or 'n,n,n,n'. Could not understand the format '" + value + "'");
                }
            }
            result = items.Count > 0;
            dash = new PDFDash(items.ToArray(), new PDFNumber(total));
            return result;
        }


        #region IPDFSimpleCodeDomValue Members
        public System.Linq.Expressions.Expression GetConstructorExpression()
        {
            throw new NotImplementedException();
            //TODO: Linq Expression
        }

        //public CodeExpression GetConstructorExpression()
        //{
        //    //new PDFDash([d,d,d],d);
        //    if (this.Pattern != null && this.Pattern.Length > 0)
        //    {
        //        CodeArrayCreateExpression array = new CodeArrayCreateExpression(typeof(PDFNumber));
        //        foreach (PDFNumber d in this.Pattern)
        //        {
        //            array.Initializers.Add(new CodePrimitiveExpression((double)d));
        //        }
        //        CodePrimitiveExpression phase = new CodePrimitiveExpression((double)this.Phase);
        //        return new CodeObjectCreateExpression(typeof(PDFDash), array, phase);
        //    }
        //    else
        //        return new CodeObjectCreateExpression(typeof(PDFDash));
        //}

        #endregion
    }
}
