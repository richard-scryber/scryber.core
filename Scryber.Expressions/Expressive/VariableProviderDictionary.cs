using System;
using System.Collections;
using System.Collections.Generic;

namespace Scryber.Expressive
{
    /// <summary>
    /// A 'simple' wrapper around <see cref="IVariableProvider"/> to allow for it to be used as a dictionary lookup.
    /// </summary>
    public class VariableProviderDictionary : IDictionary<string, object>
    {
        private readonly IVariableProvider variableProvider;

        public VariableProviderDictionary(IVariableProvider variableProvider)
        {
            this.variableProvider = variableProvider;
        }

        public bool TryGetValue(string key, out object value)
        {
            return this.variableProvider.TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => ThrowNotSupported<IEnumerator<KeyValuePair<string, object>>>();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public void Add(KeyValuePair<string, object> item) => ThrowNotSupported<bool>();

        public void Clear() => ThrowNotSupported<bool>();

        public bool Contains(KeyValuePair<string, object> item) => ThrowNotSupported<bool>();

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => ThrowNotSupported<bool>();

        public bool Remove(KeyValuePair<string, object> item) => ThrowNotSupported<bool>();

        public int Count => ThrowNotSupported<int>();
        public bool IsReadOnly => ThrowNotSupported<bool>();
        public void Add(string key, object value) => ThrowNotSupported<bool>();

        public bool ContainsKey(string key) => ThrowNotSupported<bool>();

        public bool Remove(string key) => ThrowNotSupported<bool>();

        public object this[string key]
        {
            get => ThrowNotSupported<object>();
            // ReSharper disable once ValueParameterNotUsed
            set => ThrowNotSupported<object>();
        }

        public ICollection<string> Keys => ThrowNotSupported<ICollection<string>>();
        public ICollection<object> Values => ThrowNotSupported<ICollection<object>>();

        private static TReturn ThrowNotSupported<TReturn>() => throw new NotSupportedException();
    }
}