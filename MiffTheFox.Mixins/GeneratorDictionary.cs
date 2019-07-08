using System;
using System.Collections;
using System.Collections.Generic;

namespace MiffTheFox.Collections
{
    internal class GeneratorDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _Values = new Dictionary<TKey, TValue>();

        public event EventHandler<DictionaryItemGenerationEventArgs<TKey, TValue>> ItemGeneration;

        public GeneratorDictionary()
        {
        }

        public GeneratorDictionary(Func<TKey, TValue> generatorFunction)
        {
            ItemGeneration += (sender, e) => e.Result = generatorFunction(e.Key);
        }

        public TValue this[TKey key]
        {
            get
            {
                if (ContainsKey(key))
                {
                    return _Values[key];
                }
                else
                {
                    var e = new DictionaryItemGenerationEventArgs<TKey, TValue>(key);
                    OnItemGeneration(e);
                    if (!e.ResultAvailable) throw new KeyNotFoundException();
                    _Values.Add(key, e.Result);
                    return e.Result;
                }
            }

            set => _Values[key] = value;
        }

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)_Values).Keys;
        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)_Values).Values;
        public int Count => _Values.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => _Values.Add(key, value);
        public void Clear() => _Values.Clear();
        public bool ContainsKey(TKey key) => _Values.ContainsKey(key);
        public bool Remove(TKey key) => _Values.Remove(key);

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_Values.TryGetValue(key, out value)) return true;

            var e = new DictionaryItemGenerationEventArgs<TKey, TValue>(key);
            OnItemGeneration(e);

            value = e.Result;
            return e.ResultAvailable;
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_Values).Add(item);
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_Values).Contains(item);
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_Values).CopyTo(array, arrayIndex);
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_Values).Remove(item);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => ((IDictionary<TKey, TValue>)_Values).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IDictionary<TKey, TValue>)_Values).GetEnumerator();

        protected virtual void OnItemGeneration(DictionaryItemGenerationEventArgs<TKey, TValue> e)
        {
            ItemGeneration?.Invoke(this, e);
        }
    }

    internal class DictionaryItemGenerationEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; }
        public bool ResultAvailable { get; set; }

        private TValue _Result;
        public TValue Result
        {
            get => _Result;
            set
            {
                _Result = value;
                ResultAvailable = true;
            }
        }

        public DictionaryItemGenerationEventArgs(TKey key)
        {
            Key = key;
            Result = default(TValue);
            ResultAvailable = false;
        }
    }
}
