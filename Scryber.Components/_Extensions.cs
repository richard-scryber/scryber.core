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
using Scryber.PDF.Native;
using Scryber.PDF;

namespace Scryber
{

    /// <summary>
    /// Adds Extension methods to the PDFArray class
    /// </summary>
    public static class PDFArrayExtensions
    {
        public static T[] ContentAs<T>(this PDFArray ary) where T : IPDFFileObject
        {
            if (null == ary)
                return null;

            T[] all = new T[ary.Count];
            for (int i = 0; i < ary.Count; i++)
            {
                all[i] = (T)ary[i];
            }
            return all;
        }
    }
}
