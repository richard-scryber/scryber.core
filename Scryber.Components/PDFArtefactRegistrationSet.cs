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
using Scryber.Components;
using Scryber.Styles;
using Scryber.Layout;

namespace Scryber
{
    /// <summary>
    /// A set of registered artefacts
    /// </summary>
    public class PDFArtefactRegistrationSet
    {
        #region private class LinkedNameValue

        /// <summary>
        /// A linked list of NameValues
        /// </summary>
        private class LinkedNameValue
        {
            public string Name;
            public object Value;
            public LinkedNameValue Next;
        }

        #endregion

        private const int COMPARE_EQUAL = 0;

        private PDFLayoutDocument _doc;
        private Component _comp;
        private LinkedNameValue _artefacts;
        private IComparer<string> _comparer;

        //
        // properties
        //

        #region public PDFLayoutDocument Document {get;}

        /// <summary>
        /// Gets the layout document associated with this artefact registration set
        /// </summary>
        public PDFLayoutDocument Document
        {
            get { return _doc; }
        }

        #endregion

        #region public PDFComponent Component {get;}

        /// <summary>
        /// Gets the component the artefacts are registered on
        /// </summary>
        public Component Component
        {
            get { return _comp; }
        }

        #endregion

        //
        // .ctor
        //

        #region public PDFRegistrationArtefactSet(PDFLayoutDocument doc, PDFComponent comp)

        /// <summary>
        /// Creates a new instance of a PDFRegistrationArtefactSet
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="comp"></param>
        public PDFArtefactRegistrationSet(PDFLayoutDocument doc, Component comp)
            : this(doc, comp, StringComparer.OrdinalIgnoreCase)
        {
        }

        #endregion

        #region protected PDFRegistrationArtefactSet(PDFLayoutDocument doc, PDFComponent comp, IComparer<string> comparer)

        /// <summary>
        /// Protected constructor that includes the comparer used to get and set values based on keys
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="comp"></param>
        /// <param name="comparer"></param>
        protected PDFArtefactRegistrationSet(PDFLayoutDocument doc, Component comp, IComparer<string> comparer)
        {
            if (null == doc)
                throw new ArgumentNullException("doc");
            if (null == comp)
                throw new ArgumentNullException("comp");

            _doc = doc;
            _comp = comp;
            _comparer = comparer;
        }

        #endregion

        //
        // methods
        //

        #region public bool TryGetArtefact(string name, out object value)

        /// <summary>
        /// Attempts to retrieve an artefact with the specified name.
        /// Returns true if one was found otherwise false.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetArtefact(string name, out object value)
        {
            LinkedNameValue current = _artefacts;
            while (null != current)
            {
                if (_comparer.Compare(current.Name, name) == COMPARE_EQUAL)
                {
                    value = current.Value;
                    return true;
                }
                current = current.Next;
            }
            value = null;
            return false;
        }

        #endregion

        #region public void SetArtefact(string name, object value)

        /// <summary>
        /// Sets the value of the artefact with the specified name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void SetArtefact(string name, object value)
        {
            LinkedNameValue current = _artefacts;
            while (null != current)
            {
                //If we alread have one with the same name, just set the value
                if (_comparer.Compare(current.Name, name) == COMPARE_EQUAL)
                {
                    current.Value = value;
                    return;
                }
                current = current.Next;
            }

            //Not found so - create a new one and put it at the front of the list
            current = new LinkedNameValue()
            {
                Name = name,
                Value = value,
                Next = _artefacts
            };

            _artefacts = current;

        }

        #endregion

        #region public void ClearArtefacts()

        /// <summary>
        /// Clears all registered artefacts from the set
        /// </summary>
        public void ClearArtefacts()
        {
            _artefacts = null;
        }

        #endregion



    }
}
