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
using Scryber.Drawing;

namespace Scryber.Styles
{
    /// <summary>
    /// Represents an immutable compound key of a Style Item type and a Style Value type designed for fast storage in dictionaries.
    /// </summary>
    /// <remarks>
    /// As styles have key names and their hashes - we need to make sure that
    /// they are unique. A StyleKey with the same names will equal or match
    /// any another instance with the same types based on the hash of the object types,
    /// as they are constructed with the factroy methods that holds a thread safe dictionary of keys.
    /// It's a bit slower than a direct integer keyed dictionary, but 2x as fast as a string keyed dictionary
    /// </remarks>
    public class StyleKey : IComparable<StyleKey>, IEquatable<StyleKey>
    {
        #region ivars

        /// <summary>
        /// we use the standard objects hash for comparison but store it locally.
        /// this means that each key is unique - even if the names are the same.
        /// Allows for very fast comparison - ideal for dictionary keys
        /// </summary>
        private int _fullhash;
        private ObjectType _item;
        private ObjectType _value;
        private bool _isInherited;

        #endregion

        #region public PDFObjectType StyleItemKey {get;}

        /// <summary>
        /// Gets the name of the item that this key is associated with 
        /// </summary>
        public ObjectType StyleItemKey { get { return _item; } }

        #endregion

        #region public PDFObjectType StyleValueKey {get;}

        /// <summary>
        /// Gets the name of the value this key represents
        /// </summary>
        public ObjectType StyleValueKey { get { return _value; } }

        #endregion

        #region public bool Inherited { get;}

        /// <summary>
        /// Returns true if the the style values associated with this key should be inherited.
        /// </summary>
        public bool Inherited { get { return _isInherited; } }

        #endregion

        #region public bool IsItemKey { get;}

        /// <summary>
        /// Returns true if this key is for an item rather than a value
        /// </summary>
        public bool IsItemKey { get { return this.StyleValueKey == StyleItemIdentifier; } }

        #endregion

        /// <summary>
        /// If this StyleKey supports the use of relative dimensions (e.g 5%, 0.8rem) in it's values.
        /// </summary>
        public virtual bool CanBeRelative
        {
            get { return false; }
        }

        /// <summary>
        /// Returns true if the relative dimensions of this style key should use widths as a measure e.g. 20% = 0.2 * container.Width, rather than 0.2 * container.height
        /// </summary>
        public bool UseRelativeWidthAsPriority
        {
            get;
            protected set;
        }

        //
        // .ctor
        //

        #region protected StyleKey(int fullhash, PDFObjectType item, PDFObjectType value, bool inherited, bool isItem)

        /// <summary>
        /// Creates a new instance of an immutable style key with the specified values.
        /// </summary>
        /// <param name="fullhash">The integer value that uniquely identifies this item / value compound key</param>
        /// <param name="item">The style item associated with this key</param>
        /// <param name="value">The style key associated with the value in the item</param>
        /// <param name="inherited">True if values for this key should be inherited in children</param>
        /// <param name="isItem">True if this is an item key (rather than a value item key)</param>
        protected StyleKey(int fullhash, ObjectType item, ObjectType value, bool inherited, bool isItem)
        {

            this._item = item;
            this._value = value;
            this._isInherited = inherited;
            this._fullhash = fullhash;
        }

        #endregion

        //
        // object overrides
        //

        #region public override int GetHashCode()

        /// <summary>
        /// Gets the integer hash code for this style
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _fullhash;
        }

        #endregion

        #region public override bool Equals(object obj)

        /// <summary>
        /// Returns true if this StyleKey is exacly equal to the passed object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is StyleKey)
            {
                StyleKey other = (StyleKey)obj;
                return this._fullhash == other._fullhash;
            }
            return false;
        }

        #endregion

        #region public override string ToString()

        /// <summary>
        /// Overrides the to string method so that it returnsa representation of this style key
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetStyleKeyAsString(this.StyleItemKey, this.StyleValueKey);
        }

        #endregion

        //
        // comparsion methods
        //

        #region public int CompareTo(StyleKey other)

        /// <summary>
        /// Compares this StyleKey to another returning 0 if they exactly match.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(StyleKey other)
        {
            return this._fullhash.CompareTo(other._fullhash);
        }

        #endregion

        #region public bool Equals(StyleKey other)

        /// <summary>
        /// Compares the 2 keys and returns true if they match
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(StyleKey other)
        {
            return this._fullhash == other._fullhash;
        }

        #endregion

        #region public static IEqualityComparer<StyleKey> GetComparer()

        /// <summary>
        /// Public method
        /// </summary>
        /// <returns></returns>
        public static IEqualityComparer<StyleKey> GetComparer()
        {
            return _comparer;
        }

        #endregion

        /// <summary>
        /// Static instance of the StyleHeyHashComparer
        /// </summary>
        private static IEqualityComparer<StyleKey> _comparer = new StyleKeyHashComparer();

        #region private class StyleKeyHashComparer : IEqualityComparer<StyleKey>

        /// <summary>
        /// Private class that compares 2 StyleKeys based on the full hash value.
        /// </summary>
        private class StyleKeyHashComparer : IEqualityComparer<StyleKey>
        {
            public bool Equals(StyleKey x, StyleKey y)
            {
                return x._fullhash == y._fullhash;
            }

            public int GetHashCode(StyleKey obj)
            {
                return obj._fullhash;
            }
        }

        #endregion

        //
        // static factory methods
        //


        /// <summary>
        /// The string that identifies a StyleKey as being for an item rather than a value
        /// </summary>
        public static readonly ObjectType StyleItemIdentifier = (ObjectType)"ITEM";

        /// <summary>
        /// A dictionary of the integer hash keys for specific keys (based on their string representation.
        /// Used as a lookup in factory instaniation, but the integer is stored locally in the Key.
        /// </summary>
        private static Dictionary<string, int> _knownkeys = new Dictionary<string, int>();

        /// <summary>
        /// Thread locking object
        /// </summary>
        private static object _lock = new object();

        /// <summary>
        /// Counter for the hash keys for each style
        /// </summary>
        private static int nextHash = 1;

        #region private static string GetStyleKeyAsString(PDFObjectType item, PDFObjectType value)

        /// <summary>
        /// Returns the PDFStyleKey string representation of the item and value keys
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetStyleKeyAsString(ObjectType item, ObjectType value)
        {
            return value.ToString() + "@" + item.ToString();
        }

        #endregion

        #region private static int GetStyleHash(string fullname)
        
        /// <summary>
        /// Threadsafe method that returns either an existing or new UNIQUE hash number for a particular string
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private static int GetStyleHash(string fullname)
        {
            int value;

            lock (_lock)
            {
                if (_knownkeys.TryGetValue(fullname, out value) == false)
                {
                    value = nextHash++;
                    _knownkeys[fullname] = nextHash;
                }
            }

            return value;
        }

        #endregion

        #region private static int InternalGetStyleHash(string fullname)

        /// <summary>
        /// NOT THREAD SAFE implementation of GetStyleHash
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        private static int InternalGetStyleHash(string fullname)
        {
            int value;
            if (_knownkeys.TryGetValue(fullname, out value) == false)
            {
                value = nextHash++;
                _knownkeys[fullname] = nextHash;
            }
            return value;
        }

        #endregion

        #region public static PDFStyleKey CreateStyleItemKey(PDFObjectType item, bool inherited)

        /// <summary>
        /// Creates a new Item StyleKey.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inherited"></param>
        /// <returns></returns>
        public static StyleKey CreateStyleItemKey(ObjectType item, bool inherited)
        {
            string full = GetStyleKeyAsString(item, StyleItemIdentifier);
            int hash = GetStyleHash(full);

            return new StyleKey(hash, item, StyleItemIdentifier, inherited, true);
        }

        #endregion

        #region public static StyleKey<T> CreateStyleValueKey<T>(PDFObjectType name, StyleKey foritem)

        /// <summary>
        /// Creates a new Value StyleKey based on the item StyleKey and the name of the value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="foritem"></param>
        /// <returns></returns>
        public static StyleKey<T> CreateStyleValueKey<T>(ObjectType name, StyleKey foritem)
        {
            if (foritem.IsItemKey == false)
                throw new InvalidOperationException("forItem is not an item");

            string full = GetStyleKeyAsString(foritem.StyleItemKey, name);
            int hash = GetStyleHash(full);

            return new StyleKey<T>(hash, foritem.StyleItemKey, name, foritem.Inherited, false);
            
        }

        #endregion

        #region internal static StyleKey InternalCreateStyleItemKey(PDFObjectType item, bool inherited)

        /// <summary>
        ///  NOT THREAD SAFE implementation of CreateStyleItemKey that creates a new Item StyleKey.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inherited"></param>
        /// <returns></returns>
        /// <remarks>Used by the PDFStyleKeys static type constructor - as we are assured this is threadsafe</remarks>
        internal static StyleKey InternalCreateStyleItemKey(ObjectType item, bool inherited)
        {
            string full = GetStyleKeyAsString(item, StyleItemIdentifier);
            int hash = InternalGetStyleHash(full);

            return new StyleKey(hash, item, StyleItemIdentifier, inherited, true);
        }

        #endregion

        #region internal static StyleKey<T> InternalCreateStyleValueKey<T>(PDFObjectType name, PDFStyleKey foritem)

        /// <summary>
        /// NOT THREAD SAFE implementation of of CreateStyleValueKey that 
        /// creates a new Value StyleKey based on the item StyleKey and the name of the value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="foritem"></param>
        /// <returns></returns>
        /// <remarks>Used by the PDFStyleKeys style type constructor - as we are assured this is threadsafe</remarks>
        internal static StyleKey<T> InternalCreateStyleValueKey<T>(ObjectType name, StyleKey foritem)
        {
            if (foritem.IsItemKey == false)
                throw new InvalidOperationException("forItem is not an item");

            string full = GetStyleKeyAsString(foritem.StyleItemKey, name);
            int hash = InternalGetStyleHash(full);

            return new StyleKey<T>(hash, foritem.StyleItemKey, name, foritem.Inherited, false);

        }

        #endregion

        #region internal static StyleKey<T> InternalCreateRelativeStyleValueKey<T>(PDFObjectType name, PDFStyleKey foritem)

        /// <summary>
        /// NOT THREAD SAFE implementation of of CreateStyleValueKey that 
        /// creates a new Value StyleKey based on the item StyleKey and the name of the value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="foritem"></param>
        /// <returns></returns>
        /// <remarks>Used by the PDFStyleKeys style type constructor - as we are assured this is threadsafe</remarks>
        internal static StyleKey<T> InternalCreateRelativeStyleValueKey<T>(ObjectType name, StyleKey foritem, bool useWidth, FlattenUnits<T> flatten)
        {
            if (foritem.IsItemKey == false)
                throw new InvalidOperationException("forItem is not an item");

            string full = GetStyleKeyAsString(foritem.StyleItemKey, name);
            int hash = InternalGetStyleHash(full);

            return new RelativeStyleKey<T>(hash, foritem.StyleItemKey, name, foritem.Inherited, useWidth, flatten);

        }

        #endregion

        public virtual void FlattenValue(Style style, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            //Does nothing in the default implementation
        }


    }

    /// <summary>
    /// Strongly Typed key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StyleKey<T> : StyleKey
    {

        public StyleKey(int fullhash, ObjectType item, ObjectType value, bool inherited, bool isItem)
            : base(fullhash, item, value, inherited, isItem)
        {
        }

    }

    public delegate void FlattenUnits<T>(Style style, StyleKey<T> key, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize);

    public class RelativeStyleKey<T> : StyleKey<T>
    {
        public override bool CanBeRelative { get { return true; } }

        

        public FlattenUnits<T> Flatten { get; protected set; }

        public RelativeStyleKey(int fullhash, ObjectType item, ObjectType value, bool inherited, bool useWidth, FlattenUnits<T> flatten)
            : base(fullhash, item, value, inherited, false)
        {
            Flatten = flatten ?? throw new ArgumentNullException(nameof(flatten));
            this.UseRelativeWidthAsPriority = useWidth;
        }

        public override void FlattenValue(Style style, Size pageSize, Size containerSize, Size fontSize, Unit rootFontSize)
        {
            this.Flatten(style, this, pageSize, containerSize, fontSize, rootFontSize);
        }

    }

    /// <summary>
    /// The 
    /// </summary>
    public class StyleValueDictionary : Dictionary<StyleKey, StyleValueBase>
    {
        public StyleValueDictionary()
            : base(StyleKey.GetComparer())
        {
        }

        public StyleValueDictionary(int capacity)
            : base(capacity, StyleKey.GetComparer())
        {
        }

        public void SetPriorityValue(StyleKey key, StyleValueBase value, int priority)
        {
            StyleValueBase exist;
            if (this.TryGetValue(key, out exist))
            {
                if (exist.Priority <= priority)
                {
                    this[key] = value.CloneWithPriority(priority);
                }
            }
            else
            {
                this[key] = value.CloneWithPriority(priority);
            }
        }

    }

    
}
