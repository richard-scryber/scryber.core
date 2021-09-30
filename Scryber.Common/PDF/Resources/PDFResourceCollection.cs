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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Scryber.PDF.Resources
{
    public class PDFResourceCollection : IEnumerable<PDFResource>
    {
        

        private IComponent _owner;

        public IComponent Owner
        {
            get { return _owner; }
        }

        private List<PDFResource> _items;

        protected List<PDFResource> Items
        {
            get { return _items; }
        }

        public int Count
        {
            get { return this._items.Count; }
        }

        public PDFResource this[int index]
        {
            get { return this.Items[index]; }
        }


        public PDFResourceCollection(IComponent owner) 
        {
            this._owner = owner;
            this._items = new List<PDFResource>();
        }

        public PDFResource GetResource(string type, string name)
        {
            foreach (PDFResource resx in this.Items)
            {
                if (resx.Equals(type, name))
                {
                    return resx;
                }

            }
            return null;
        }

        
        public PDFResource Add(PDFResource resource)
        {
            foreach (PDFResource resx in this.Items)
            {
                if (resx.Equals(resource))
                {
                    return resx;
                }

            }
            this.Items.Add(resource);
            return resource;
        }

        public IEnumerator<PDFResource> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
