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

namespace Scryber
{
    internal class TempIDFactory
    {
        private static int _tempid = 0;
        private static int MaxTempID = 10000000; //ten million item limit

        public static string GetTempID(PDFObjectType type)
        {
            _tempid++;
            if (_tempid > MaxTempID)
                _tempid = 1;
            return "_temp" + _tempid;
        }

        public static bool IsTempID(string id)
        {
            return !string.IsNullOrEmpty(id) && id.StartsWith("_temp");
        }

        
    }

}
