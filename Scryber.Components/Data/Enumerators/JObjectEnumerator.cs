using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scryber.Data.Enumerators;

public class JObjectEnumerator : DictionaryBindingEnumerator
{

    public JObjectEnumerator(JObject jObject, IDataSource source, bool updateIndex, bool updateContextData,
        DataContext context)
    : base(GetDictionary(jObject), source, updateIndex, updateContextData, context)
    {
        
    }

    private static IDictionary GetDictionary(JObject jobj)
    {
        var dict = (IDictionary<string, JToken>)jobj;
        return new JsonTokenDictionary(dict);

    }
    
    private class JsonTokenDictionary : IDictionary
    {
        private IDictionary<string, JToken> _dictionary;
        private int _count;
        private bool _isSynchronized;
        private object _syncRoot;
        private bool _isFixedSize;
        private bool _isReadOnly;
        private ICollection _keys;
        private ICollection _values;

        public JsonTokenDictionary(IDictionary<string, JToken> dictionary)
        {
            this._dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public void Add(object key, object value)
        {
            this._dictionary.Add((string)key, (JToken)value);
        }

        public void Clear()
        {
            this._dictionary.Clear();
        }

        public bool Contains(object key)
        {
            return this._dictionary.ContainsKey((string)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotSupportedException("Use the IEnumerable interface GetEnumerator");
        }

        public void Remove(object key)
        {
            this._dictionary.Remove((string)key);
        }

        public bool IsFixedSize
        {
            get { return this._dictionary.IsReadOnly;}
        }

        public bool IsReadOnly
        {
            get { return this._dictionary.IsReadOnly; }
        }

        public object this[object key]
        {
            get { return this._dictionary[(string)key];}
            set { this._dictionary[(string)key] = (JToken)value; }
        }

        public ICollection Keys
        {
            get { return new List<string>(this._dictionary.Keys); }
        }

        public ICollection Values
        {
            get { return new List<JToken>(this._dictionary.Values); }
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this._dictionary.Count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }
    }
    
}