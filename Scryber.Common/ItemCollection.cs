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
    /// A collection of objects that is unique to a Document, 
    /// but is accessible in any context of the document lifecycle (Init, Load, Databind, Layout and Render)
    /// </summary>
    [Serializable()]
    public class ItemCollection : System.Collections.Specialized.NameObjectCollectionBase, ICloneable
    {
        private IComponent _ownercomp;

        public IComponent OwnerComponent
        {
            get { return this._ownercomp; }
        }

        /// <summary>
        /// Creates a new empty instance of the ItemCollection
        /// </summary>
        public ItemCollection(IComponent owner)
        {
            this._ownercomp = owner;
        }

        /// <summary>
        /// Creates a new instance of the ItemCollection and adds the specified items to the collection
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="doc">The document that owns this item collection</param>
        public ItemCollection(IDictionary<string, object> contents, IDocument doc)
            : this(doc)
        {
            if (null != contents)
            {
                foreach (string str in contents.Keys)
                {
                    this.BaseAdd(str, contents[str]);
                }
            }
        }

        /// <summary>
        /// Gets the object value in this item collection at the specified index 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                string key = this.BaseGetKey(index);
                object value = this.BaseGet(key);
                if (value is IKeyValueProvider)
                    value = ((IKeyValueProvider)value).GetNativeValue(key, this._ownercomp);
                return value;
            }
        }


        /// <summary>
        /// Gets the object in this collection associated with the specified key.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                object value = this.BaseGet(name);
                if (value is IKeyValueProvider)
                    value = ((IKeyValueProvider)value).GetNativeValue(name, this._ownercomp);
                
                return value;
            }
            set {
                object curr = this.BaseGet(name);

                if (null != curr && curr is IKeyValueProvider)
                    ((IKeyValueProvider)curr).SetNativeValue(name, value, this._ownercomp);
                else
                    this.BaseSet(name, value);
            }
        }

        /// <summary>
        /// Adds a new entry in this collection with the key value of the provider, and the provider as the value.
        /// </summary>
        /// <param name="item"></param>
        /// <remarks>When retrieveing the value the provider will be invoked to return the actual required value</remarks>
        public void Add(IKeyValueProvider item)
        {
            string name = item.ID;
            this[name] = item;
        }

        public int Add(string name, object value)
        {
            int index = this.Count;
            this.BaseAdd(name, value);
            return index;
        }

        public void Init(InitContext context)
        {
            foreach (string key in this.Keys)
            {
                object value = this.BaseGet(key);
                if (value is IKeyValueProvider)
                    ((IKeyValueProvider)value).Init();
            }
        }


        /// <summary>
        /// Updates or adds new entries in this collection for each of 
        /// the entries in the passed collection
        /// </summary>
        /// <param name="all"></param>
        public void Merge(ItemCollection all)
        {
            foreach (string key in all.Keys)
            {
                this.BaseSet(key, all[key]);
            }
        }

        public void Remove(string name)
        {
            this.BaseRemove(name);
        }

        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        public void Clear()
        {
            this.BaseClear();
        }

        public void SetStringValue(string key, string value)
        {
            object curr = this.BaseGet(key);
            if(null == curr)
            {
                this[key] = value;
            }
            else if(curr is IKeyValueProvider)
            {
                var kvp = (IKeyValueProvider)curr;
                kvp.SetValue(key, value, this.OwnerComponent);
            }
        }

        public bool TryGetValue(string key, out object value)
        {
            value = this.BaseGet(key);
            if (null == value)
                return false;

            if (value is IKeyValueProvider provider)
                value = provider.GetNativeValue(key, OwnerComponent);

            return null != value;

        }

        public ItemCollection Clone()
        {
            ItemCollection instance = this.MemberwiseClone() as ItemCollection;
            instance.Clear();
            foreach (string key in this.BaseGetAllKeys())
            {
                instance.Add(key, this[key]);
            }
            return instance;
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }


}
