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

namespace Scryber.Caching
{

    /// <summary>
    /// Creates and returns a cacheprovider based on the required technology
    /// </summary>
    public static class PDFCacheProvider
    {

        public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        
        public  static readonly TimeSpan NoCacheDuration = TimeSpan.Zero;

        /// <summary>
        /// The defaul caching duration
        /// </summary>
        public static TimeSpan DefaultCacheDuration = new TimeSpan(0, 30, 0);
        

        /// <summary>
        /// Gets the IPDFCacheProvider for and application / service based infrastructure
        /// </summary>
        /// <returns></returns>
        public static ICacheProvider GetStatic()
        {
            return new PDFStaticCacheProvider();
        }
    }
}
