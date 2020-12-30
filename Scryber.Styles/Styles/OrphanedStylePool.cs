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
    /// A pool of orphaned styles. Get() from the pool and Release() back once finished with
    /// </summary>
    internal class OrphanedStylePool
    {

        private Stack<OrphanedStyle> _available = new Stack<OrphanedStyle>();

        public virtual OrphanedStyle Get()
        {
            if (_available.Count > 0)
                return _available.Pop();
            else
                return new OrphanedStyle(this);

        }

        public virtual void Release(OrphanedStyle style)
        {
            if (null == style)
                throw new ArgumentNullException("style");

            style.Clear();
            _available.Push(style);

        }
    }

    /// <summary>
    /// Implements the orphan style pool in a thread safe manner.
    /// </summary>
    internal class PDFOrphanedStyleSafePool : OrphanedStylePool
    {
        private object _lock = new object();

        public override OrphanedStyle Get()
        {
            lock (_lock)
            {
                return base.Get();
            }
        }

        public override void Release(OrphanedStyle style)
        {
            lock (_lock)
            {
                base.Release(style);
            }
        }
    }
}
