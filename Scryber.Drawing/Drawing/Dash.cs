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
using Scryber.PDF.Native;
using Scryber.PDF;
using System.CodeDom;
using System.ComponentModel;

namespace Scryber.Drawing
{
    [PDFParsableValue()]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Dash
    {
        private int[] _pattern;

        public int[] Pattern
        {
            get { return _pattern; }
            set { _pattern = value; }
        }

        private int _len;

        public int Phase
        {
            get { return _len; }
            set { _len = value; }
        }

        public Dash()
            : this(new int[] { }, 0) //solid
        {
        }

        public Dash(int[] pattern, int phase)
        {
            this._pattern = pattern;
            this._len = phase;
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
            return this.Equals((Dash)obj);
        }

        public bool Equals(Dash dash)
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
                foreach (int num in this._pattern)
                {
                    count = count ^ num.GetHashCode();
                }
                return count ^ this._len.GetHashCode();
            }
        }

        public static Dash None
        {
            get { return new Dash(); }
        }

        public static Dash Parse(string dashpattern)
        {
            
            Dash dash = null;
            if(string.IsNullOrEmpty(dashpattern))
            {
                return dash;
            }
            dashpattern = dashpattern.Trim();

            if (dashpattern.StartsWith("[") && dashpattern.IndexOf("]") > 0)
            {
                dash = ParseExplicit(dashpattern);
            }

            else if (Dashes.TryGetNamedDash(dashpattern, out dash) == false)
            {
                if(!TryParseDashNumberList(dashpattern, out dash))
                    throw new ArgumentException("The dash pattern '" + dashpattern + "' could not be parsed. The format of a custom dash phase should be '[n n ...] n' or '[n,n,n] n', or one of hte known named patterns.");
            }
            return dash;
        }

        private static Dash ParseExplicit(string dashpattern)
        {
            int end = dashpattern.IndexOf(']');

            string pattern = dashpattern.Substring(1, end - 1).Trim();
            string phase = dashpattern.Substring(end + 1).Trim();

            int[] val = null;
               
            if (pattern.Length > 0)
            {
                string[] array = pattern.Split(' ',',');
                List<int> all = new List<int>();
                for(var i = 0; i < array.Length; i++)
                {
                    var a = array[i];
                    int parsed;
                    if (string.IsNullOrEmpty(a) == false)
                    {
                        if (int.TryParse(a.Trim(), out parsed) == false)
                            parsed = 0;

                        all.Add(parsed);
                    }
                }
                val = all.ToArray();
            }

            int p = 0;
            if(int.TryParse(phase, out p) == false)
                throw new FormatException("The format of a custom dash phase should be '[n n ...] n' or [n,n,n] n. Could not understand the format '" + dashpattern + "'");

            return new Dash(val, p);

        }

        public static bool TryParseDashNumberList(string value, out Dash dash)
        {
            bool result = false;
            dash = null;

            var entries = value.Split(',', ' ');
            int total = 0;
            List<int> items = new List<int>();

            for(var i = 0; i < entries.Length; i++)
            {
                var a = entries[i];
                if(string.IsNullOrEmpty(a) == false)
                {
                    int parsed;
                    if (int.TryParse(a.Trim(), out parsed))
                    {
                        items.Add(parsed);
                        total += parsed;
                    }
                    else
                        throw new FormatException("The format of a custom dash phase should be '[n n ...] n' or '[n,n,n] n' or 'n,n,n,n'. Could not understand the format '" + value + "'");
                }
            }
            result = items.Count > 0;
            dash = new Dash(items.ToArray(), total);
            return result;
        }


    }
}
