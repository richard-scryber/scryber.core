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
using Scryber;

namespace Scryber.Styles
{
    /// <summary>
    /// Abstract base class for all style items. Items act as a grouping for style values.
    /// </summary>
    public abstract class StyleItemBase : Scryber.PDFObject, IPDFBindableComponent
    {

        #region public event PDFDataBindEventHandler DataBinding + OnDataBinding(context)

        public event PDFDataBindEventHandler DataBinding;

        protected virtual void OnDataBinding(PDFDataContext context)
        {
            if (this.DataBinding != null)
                this.DataBinding(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        #region public event PDFDataBindEventHandler DataBound + OnDataBound(context)

        public event PDFDataBindEventHandler DataBound;

        protected virtual void OnDataBound(PDFDataContext context)
        {
            if (this.DataBound != null)
                this.DataBound(this, new PDFDataBindEventArgs(context));
        }

        #endregion

        #region public PDFStyleBase Owner {get;set;}

        private StyleBase _owner;

        /// <summary>
        /// Gets the owner or this style item. The PDFStyle that this item is associated with.
        /// </summary>
        public StyleBase Owner
        {
            get { return (_owner is OrphanedStyle)? null : _owner; }
            internal set
            {
                //We need to check if we are an orphanded style.
                //if so then we copy the current values in our style
                //into the new owner.
                StyleBase orig = this._owner;
                if (value != null && orig is OrphanedStyle)
                {
                    if (orig.ValueCount > 0)
                        orig.MergeInto(value, Style.DirectStylePriority);

                    (orig as OrphanedStyle).ReleaseBackToPool();
                }
                else if (_owner != null && orig != null && _owner != orig)
                    throw new NotSupportedException("Style items cannot be moved between owners. Copy the values across instead.");

                //We are ok - so set the owner
                _owner = value;
            }
        }

        #endregion

        #region public bool IsOrphaned

        /// <summary>
        /// Returns true if this style item has an owner
        /// </summary>
        public bool IsOrphaned
        {
            get { return _owner == null || _owner is OrphanedStyle; }
        }

        #endregion

        #region public StyleKey ItemKey

        private StyleKey _itemkey;

        /// <summary>
        /// Gets the style key associated with this item
        /// </summary>
        public StyleKey ItemKey
        {
            get { return _itemkey; }
        }

        #endregion

        #region public bool IsInherited

        public bool IsInherited
        {
            get { return this._itemkey.Inherited; }
        }

        #endregion

        //
        // .ctor
        //

        #region protected StyleItemBase(StyleKey key)

        /// <summary>
        /// Protected Constructor for the style item base
        /// </summary>
        /// <param name="key"></param>
        protected StyleItemBase(StyleKey key)
            : base(key.StyleItemKey)
        {
            this._itemkey = key;
        }

        #endregion

        //
        // public methods
        //

        #region public bool IsDefined(StyleKey valuekey)

        /// <summary>
        /// Returns true if the style key has a defined value in this items style
        /// </summary>
        /// <param name="valuekey"></param>
        /// <returns></returns>
        public bool IsDefined(StyleKey valuekey)
        {
            return this.AssertOwner().IsValueDefined(valuekey);
        }

        #endregion

        #region public bool RemoveValue(StyleKey key)

        /// <summary>
        /// Removes any associated value in this items style for the provided key;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveValue(StyleKey key)
        {
            return this.AssertOwner().RemoveValue(key);
        }

        #endregion

        #region public bool RemoveAllValues()

        /// <summary>
        /// Removes all the values associated with this item from it's style, and returns true if there were actually some removed.
        /// </summary>
        /// <returns></returns>
        public bool RemoveAllValues()
        {
            return this.AssertOwner().RemoveItemStyleValues(this.ItemKey);
        }

        #endregion

        #region public StyleValueBase[] RemoveAndReturnAllValues()

        /// <summary>
        /// Removes all the values associated with this item from it's style, and returns an array of all the values that were removed
        /// </summary>
        /// <returns></returns>
        public PDFStyleValueBase[] RemoveAndReturnAllValues()
        {
            return this.AssertOwner().RemoveAndReturnItemStyleValues(this);
        }

        #endregion


        public void DataBind(PDFDataContext context)
        {
            this.OnDataBinding(context);
            
            this.OnDataBound(context);
        }

        //
        // internal implementation
        //

        #region internal void AddBaseValue(StyleValueBase valuebase)

        /// <summary>
        /// Internal method to add a specific value (of any type) to this item
        /// </summary>
        /// <param name="valuebase"></param>
        internal void AddBaseValue(PDFStyleValueBase valuebase)
        {
            this.AssertOwner().AddValue(valuebase);
        }

        #endregion

        #region internal PDFStyleBase AssertOwner()

        /// <summary>
        /// Returns the current style item owner.
        /// If this style items owner is null then we need to get an orphan style from the pool to hold values.
        /// These values will be added back into the owner once one is set.
        /// </summary>
        /// <returns></returns>
        public StyleBase AssertOwner()
        {
            if (null == this._owner)
                    this._owner = OrphanedStyle.Pool.Get();
            
            return this._owner;
        }

        #endregion
    }

    /// <summary>
    /// Generic extension methods for a StyleItemBase - allows type inference and zero casting
    /// </summary>
    public static class StyleItemBaseExtensions
    {

        #region public static bool TryGetValue<T>(this StyleItemBase itembase, StyleKey<T> key, out T found)

        /// <summary>
        /// Attempts to retrieve a value of the known type from this StyleItem
        /// </summary>
        /// <typeparam name="T">The type of value to retrieve</typeparam>
        /// <param name="itembase">This style item</param>
        /// <param name="key">The style key of the value to retrieve</param>
        /// <param name="found">Set to the value of any found item, or default value.</param>
        /// <returns>True if the item has a reference to the value required, otherwise false.</returns>
        public static bool TryGetValue<T>(this StyleItemBase itembase, PDFStyleKey<T> key, out T found)
        {
            StyleValue<T> exist;
            if (itembase.AssertOwner().TryGetValue(key, out exist))
            {
                found = exist.Value;
                return true;
            }
            else
            {
                found = default(T);
                return false;
            }
        }

        #endregion

        #region public static void SetValue<T>(this StyleItemBase itembase, StyleKey<T> key, T value)

        /// <summary>
        /// Sets a value of a known type for this style item, based on the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itembase"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue<T>(this StyleItemBase itembase, PDFStyleKey<T> key, T value)
        {
            StyleValue<T> found;
            if (itembase.AssertOwner().TryGetValue(key, out found))
            {
                found.SetValue(value);
            }
            else
            {
                found = new StyleValue<T>(key, value);
                itembase.AddBaseValue(found);
            }
        }

        #endregion

    }
}
