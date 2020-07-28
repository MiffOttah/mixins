using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Collections
{
    /// <summary>
    /// Repersents a dictionary that can return a read-only version of itself, in a fasion similar to 
    /// List&lt;T&gt;.AsReadOnly.
    /// </summary>
    internal class LockingDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// A reference to this dictionary that cannot be modified. Reference types can still
        /// be mutated.
        /// </summary>
        public IDictionary<TKey, TValue> Locked { get; }

        public LockingDictionary()
        {
            Locked = new LockingDictionaryLocked(this);
        }

        private class LockingDictionaryLocked : IDictionary<TKey, TValue>
        {
            private readonly LockingDictionary<TKey, TValue> _Parent;
            
            public LockingDictionaryLocked(LockingDictionary<TKey, TValue> parent)
            {
                _Parent = parent;
            }

            public TValue this[TKey key] { get => _Parent[key]; set => throw new NotSupportedException(); }
            public ICollection<TKey> Keys => _Parent.Keys;
            public ICollection<TValue> Values => _Parent.Values;
            public int Count => _Parent.Count;
            
            bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

            public void Add(TKey key, TValue value) => throw new NotSupportedException();
            public void Clear() => throw new NotSupportedException();
            public bool Remove(TKey key) => throw new NotSupportedException();

            public bool ContainsKey(TKey key) => _Parent.ContainsKey(key);
            public bool TryGetValue(TKey key, out TValue value) => _Parent.TryGetValue(key, out value);

            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _Parent.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Parent).GetEnumerator();
            void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_Parent).CopyTo(array, arrayIndex);
            void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();
            bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_Parent).Remove(item);
            bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_Parent).Contains(item);
        }
    }
}
