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
    internal class TextConst
    {

        /// <summary>
        /// All the characters that should be escaped
        /// </summary>
        public static readonly char[] EscapeChars = new char[] { '\\', '(', ')' };

        /// <summary>
        /// String representations of the escape characters
        /// </summary>
        public static readonly string[] EscapeStrings = new string[] { "\\", "(", ")" };

        /// <summary>
        /// String representations of the escaped versions of the characters to replace
        /// Matches both count and order of the EscapeStrings
        /// </summary>
        public static readonly string[] ReplaceStrings = new string[] { "\\\\", "\\(", "\\)" };
    }
}
