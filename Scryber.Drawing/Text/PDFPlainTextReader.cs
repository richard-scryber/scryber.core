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
using Scryber.Drawing;

namespace Scryber.Text
{
    public class PDFPlainTextReader : PDFTextReader
    {
        private string[] _lines;
        private int _index = -1;
        

        public override int Length
        {
            get
            {
                int count = 0;
                foreach (string s in _lines)
                {
                    if (s != "\n")
                        count += s.Length;
                }
                return count;
            }
        }


        //TODO: Read the markers and not split the text;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="preserveWhiteSpace"></param>
        public PDFPlainTextReader(string text, bool preserveWhiteSpace)
            : base()
        {
            StringSplitOptions opts = preserveWhiteSpace ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries;
            
            this._lines = string.IsNullOrEmpty(text) ? new string[] { } : text.Split(new char[] { '\n' }, opts);
            
            if (_lines.Length > 0)
            {
                List<string> act = new List<string>(this._lines.Length * 2);
                for (int i = 0; i < this._lines.Length; i++)
                {
                    bool lastline = (i == _lines.Length - 1);
                    string s = this._lines[i];
                    //If we are not  preserving whitespace then remove any trailing spaces
                    if (preserveWhiteSpace == false && char.IsWhiteSpace(s, s.Length - 1))
                    {
                        s = s.TrimEnd();
                        //if we are the last line then we trim all trailing spaces and then add one back on
                        if (lastline)
                            s += " ";
                    }
                    act.Add(s);

                    if(!lastline)
                        act.Add("\n");
                }
                
                _lines = act.ToArray();
            }

            _index = -1;
        }

        public override bool Read()
        {
            _index++;
            _curr = null;
            return _index < _lines.Length;
        }

        public override bool EOF
        {
            get { return _index >= _lines.Length; }
        }

        private PDFTextOp _curr = null;

        public override PDFTextOp Value
        {
            get
            {
                if (_curr == null)
                {
                    if (_index < 0)
                        throw new ArgumentOutOfRangeException(Errors.CannotAccessStreamWithoutRead);
                    if (_index >= _lines.Length)
                        throw new ArgumentOutOfRangeException(Errors.CannotReadPastTheEOF);
                    string s = this._lines[_index];
                    if (s == "\n")
                        _curr = new PDFTextNewLineOp();
                    else
                        _curr = new PDFTextDrawOp(s);
                }
                return _curr;
            }
        }

        protected override void ResetTextMarkers()
        {
            this._index = -1;
            this._curr = null;
        }
    }
}
