using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber
{
    /// <summary>
    /// Represents a generic dictionary of Key values initialiized and populated from the constructor that is read only and cannot be modified.
    /// </summary>
    /// <typeparam name="TKey">The key type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    internal class ReadOnlyDictionary<TKey,TValue> : IDictionary<TKey,TValue>
    {
        private IDictionary<TKey, TValue> _inner;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> inner)
        {
            if (null == inner)
                throw new ArgumentNullException("inner");
            _inner = inner;
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotSupportedException("This dictionary is readonly");
        }

        public bool ContainsKey(TKey key)
        {
            return this._inner.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return this._inner.Keys; }
        }

        public bool Remove(TKey key)
        {
            throw new NotSupportedException("This dictionary is readonly");
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._inner.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return this._inner.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return this._inner[key];
            }
            set
            {
                throw new NotSupportedException("This dictionary is readonly");
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is readonly");
        }

        public void Clear()
        {
            throw new NotSupportedException("This dictionary is readonly");
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this._inner.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this._inner.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._inner.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("This dictionary is readonly");
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._inner.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
