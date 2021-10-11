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
using Scryber.PDF;

namespace Scryber
{
    /// <summary>
    /// This is the base class for all ITypedObjects to inherit from so they set their type from the ObjectType.
    /// </summary>
    /// <remarks>
    /// Using a Type rather than the built in System.Type, all objects so multiple classes can share the same type (Tables, Panels, Text etc.)
    /// </remarks>
    public abstract class TypedObject : ITypedObject
    {
        #region ObjectType Type {get;}

        private ObjectType _type;
        /// <summary>
        /// Gets the type identifier for this object
        /// </summary>
        public ObjectType Type
        {
            get { return this._type; }
        }

        #endregion

        /// <summary>
        /// Protected constructor for the PDFObject
        /// </summary>
        /// <param name="type">The identifying type name for this instance</param>
        protected TypedObject(ObjectType type)
        {
            this._type = type;
        }

        
    }

    
}
