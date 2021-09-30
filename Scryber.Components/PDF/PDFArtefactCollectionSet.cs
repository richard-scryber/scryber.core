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

namespace Scryber.PDF
{
    /// <summary>
    /// A keyed set of artefact collections
    /// </summary>
    public class PDFArtefactCollectionSet : IEnumerable<IArtefactCollection>
    {
        private Dictionary<string, IArtefactCollection> _artefacts;

        public int Count
        {
            get { return _artefacts == null ? 0 : _artefacts.Count; }
        }

        public bool TryGetCollection(string name, out IArtefactCollection collection)
        {
            if (null == _artefacts)
            {
                collection = null;
                return false;
            }
            else
                return _artefacts.TryGetValue(name, out collection);
        }

        public void Add(IArtefactCollection collection)
        {
            if (null == _artefacts)
                _artefacts = new Dictionary<string, IArtefactCollection>();

            _artefacts.Add(collection.CollectionName, collection);
        }

        public bool Contains(string name)
        {
            if (null == name)
                throw RecordAndRaise.ArgumentNull("name");

            return this._artefacts.ContainsKey(name);
        }

        public bool Remove(string name)
        {
            if (null == name)
                throw RecordAndRaise.ArgumentNull("name");

            return this._artefacts.Remove(name);
        }

        public void Clear()
        {
            if (null != _artefacts)
                this._artefacts.Clear();
        }

        #region IEnumerable<IArtefactCollection> Members

        public IEnumerator<IArtefactCollection> GetEnumerator()
        {
            if (null == _artefacts)
                _artefacts = new Dictionary<string, IArtefactCollection>();

            return _artefacts.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
