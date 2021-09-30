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
    public struct PDFFileRange
    {
        public long StartOffset { get; private set; }

        public long EndOffset { get; private set; }

        public string MatchedString { get; private set; }

        public long Length { get { return EndOffset - StartOffset; } }

        public bool Found { get; private set; }

        public PDFFileRange(string match, long start, long end)
            : this()
        {
            this.MatchedString = match;
            this.StartOffset = start;
            this.EndOffset = end;
            this.Found = start > -1;
        }

        public override string ToString()
        {
            if (this.Found)
                return "Found : " + this.StartOffset + " -> " + this.EndOffset + " = '" + ((MatchedString.Length > 20) ? this.MatchedString.Substring(0, 20) + "..." : this.MatchedString) + "'";
            else
                return "Not Found";
        }

        public static PDFFileRange NotFound
        {
            get { return new PDFFileRange("", -1, -1); }
        }


    }
}
