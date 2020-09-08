using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scryber
{
    public class ReadOnlyList<T> : IList<T>
    {
        private IList<T> _inner;

        public ReadOnlyList(IList<T> inner)
        {
            this._inner = inner;
            if (null == _inner)
                throw new ArgumentNullException("inner");
        }

        public int IndexOf(T item)
        {
            return _inner.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("This list is readonly");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("This list is readonly");
        }

        public T this[int index]
        {
            get
            {
                return this._inner[index];
            }
            set
            {
                throw new NotSupportedException("This list is readonly");
            }
        }

        public void Add(T item)
        {
            throw new NotSupportedException("This list is readonly");
        }

        public void Clear()
        {
            throw new NotSupportedException("This list is readonly");
        }

        public bool Contains(T item)
        {
            return this._inner.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
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

        public bool Remove(T item)
        {
            throw new NotSupportedException("This list is readonly");
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this._inner.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
