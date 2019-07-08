using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiffTheFox.Collections
{
    /// <summary>
    /// Repersents a dictionary where each value is a key that can be used to reterieve the original key as its value.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    internal class DoubleDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly List<Tuple<TKey, TValue>> _Entries = new List<Tuple<TKey, TValue>>();

        public TValue this[TKey key]
        {
            get
            {
                foreach (var e in _Entries)
                {
                    if (Equals(e.Item1, key)) return e.Item2;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                for (int i = 0; i < _Entries.Count; i++)
                {
                    var e = _Entries[i];
                    if (Equals(e.Item1, key))
                    {
                        if (Equals(e.Item2, value)) return;
                        if (ContainsValue(value)) throw new InvalidOperationException("Dictionary already contains this value.");

                        _Entries[i] = new Tuple<TKey, TValue>(e.Item1, value);
                        return;
                    }
                }
                
                if (ContainsValue(value)) throw new InvalidOperationException("Dictionary already contains this value.");
                _Entries.Add(new Tuple<TKey, TValue>(key, value));
            }
        }

        public ICollection<TKey> Keys => _Entries.Select(e => e.Item1).ToList().AsReadOnly();

        public ICollection<TValue> Values => _Entries.Select(e => e.Item2).ToList().AsReadOnly();

        public int Count => _Entries.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key)) throw new InvalidOperationException("Dictionary already contains this key.");
            if (ContainsValue(value)) throw new InvalidOperationException("Dictionary already contains this value.");
            _Entries.Add(new Tuple<TKey, TValue>(key, value));
        }

        public void Clear()
        {
            _Entries.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var e in _Entries)
            {
                if (Equals(e.Item1, key)) return true;
            }
            return false;
        }

        public bool ContainsValue(TValue value)
        {
            foreach (var e in _Entries)
            {
                if (Equals(e.Item2, value)) return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var e in _Entries)
            {
                yield return new KeyValuePair<TKey, TValue>(e.Item1, e.Item2);
            }
        }

        public bool Remove(TKey key)
        {
            for (int i = 0; i < _Entries.Count; i++)
            {
                if (Equals(_Entries[i].Item1, key))
                {
                    _Entries.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveValue(TValue value)
        {
            for (int i = 0; i < _Entries.Count; i++)
            {
                if (Equals(_Entries[i].Item2, value))
                {
                    _Entries.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            foreach (var e in _Entries)
            {
                if (Equals(e.Item1, key))
                {
                    value = e.Item2;
                    return true;
                }
            }
            value = default;
            return false;
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            foreach (var e in _Entries)
            {
                if (Equals(e.Item2, value))
                {
                    key = e.Item1;
                    return true;
                }
            }
            key = default;
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            foreach (var e in _Entries)
            {
                if (Equals(e.Item1, item.Key) && Equals(e.Item2, item.Value))
                {
                    return true;
                }
            }
            return false;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < _Entries.Count; i++)
            {
                array[arrayIndex + i] = new KeyValuePair<TKey, TValue>(_Entries[i].Item1, _Entries[i].Item2);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            for (int i = 0; i < _Entries.Count; i++)
            {
                if (Equals(_Entries[i].Item1, item.Key) && Equals(_Entries[i].Item2, item.Value))
                {
                    _Entries.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
    }
}
