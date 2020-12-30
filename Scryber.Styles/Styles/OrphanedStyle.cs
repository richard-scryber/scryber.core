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
using System.Threading.Tasks;

namespace Scryber.Styles
{
    /// <summary>
    /// The PDFOrphanedStyle acts as a holding container for orpaned style items to 
    /// hold values until their owner is set.
    /// Use the static Pool property for creation and release.
    /// </summary>
    internal class OrphanedStyle : StyleBase
    {
        private OrphanedStylePool _orphanPool;

        internal OrphanedStyle(OrphanedStylePool pool)
            : base((PDFObjectType)"_ors")
        {
            _orphanPool = pool;
        }


        public void ReleaseBackToPool()
        {
            this._orphanPool.Release(this);
        }


        /// <summary>
        /// Thread safe version of the style pool that can be used
        /// </summary>
        private static PDFOrphanedStyleSafePool _globalPool = new PDFOrphanedStyleSafePool();

        internal static OrphanedStylePool Pool
        {
            get { return _globalPool; }
        }

        protected override void DoDataBind(PDFDataContext context, bool includechildren)
        {
            base.DoDataBind(context, includechildren);
        }

        
    }
}
