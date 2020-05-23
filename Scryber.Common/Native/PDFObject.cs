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
    /// <summary>
    /// This is the base class for all PDF objects to inherit from
    /// </summary>
    public abstract class PDFObject : IPDFObject //, IDisposable
    {
        #region PDFObjectType ItemType {get;}

        private PDFObjectType _type;
        /// <summary>
        /// Gets the type identifier for this object
        /// </summary>
        public PDFObjectType Type
        {
            get { return this._type; }
        }

        #endregion

        /// <summary>
        /// Protected constructor for the PDFObject
        /// </summary>
        /// <param name="type">The identifying type name for this instance</param>
        protected PDFObject(PDFObjectType type)
        {
            this._type = type;
        }

        //#region IDisposable Members

        ///// <summary>
        ///// Disposes this instance and release all resources for this method.
        ///// </summary>
        //public void Dispose()
        //{
        //    this.Dispose(true);
        //}

        ///// <summary>
        ///// Inheritors should override this method to release all unmanaged resources
        ///// </summary>
        ///// <param name="disposing">True if this instance is being disposed of - i.e: this is called from the Dispose() method</param>
        //protected virtual void Dispose(bool disposing)
        //{

        //}

        //~PDFObject()
        //{
        //    this.Dispose(false);
        //}

        //#endregion
    }

    
}
