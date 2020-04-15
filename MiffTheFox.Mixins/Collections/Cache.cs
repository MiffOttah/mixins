using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiffTheFox.Collections
{
    internal class Cache<TKey, TValue> : IEnumerable<TValue>, IDisposable
    {
        private readonly Dictionary<TKey, TValue> _Values;
        private readonly Dictionary<TKey, DateTime> _Ages;

        public TimeSpan LifeSpan { get; set; } = TimeSpan.FromMinutes(10);
        public int? Capacity { get; set; } = null;
        public int Count
        {
            get
            {
                Clean();
                return _Values.Count;
            }
        }

        public Cache()
        {
            _Values = new Dictionary<TKey, TValue>();
            _Ages = new Dictionary<TKey, DateTime>();
        }

        public Cache(TimeSpan lifespan) : this()
        {
            LifeSpan = lifespan;
        }

        public Cache(int capacity)
        {
            _Values = new Dictionary<TKey, TValue>();
            _Ages = new Dictionary<TKey, DateTime>();
            Capacity = capacity;
        }

        public Cache(TimeSpan lifespan, int capacity) : this(capacity)
        {
            LifeSpan = lifespan;
        }

        public void Add(TKey key, TValue value)
        {
            _Values[key] = value;
            _Ages[key] = DateTime.UtcNow;
            Clean();
        }

        public TValue GetItem(TKey key, Func<TValue> generate)
        {
            if (GetItem(key, out TValue value))
            {
                return value;
            }
            else
            {
                TValue newValue = generate();
                Add(key, newValue);
                return newValue;
            }
        }

        public bool GetItem(TKey key, out TValue value)
        {
            DateTime oldTime = DateTime.UtcNow - LifeSpan;
            if (_Values.ContainsKey(key) && _Ages[key] >= oldTime)
            {
                value = _Values[key];
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public void Clean()
        {
            DateTime oldTime = DateTime.UtcNow - LifeSpan;
            var removeByAge = _Ages.Where(d => d.Value < oldTime).Select(d => d.Key).ToArray();
            _RemoveKeys(removeByAge);

            if (Capacity.HasValue)
            {
                int rem = _Values.Count - Capacity.Value;
                if (rem >= 0)
                {
                    var toRemove = _Ages.OrderBy(d => d.Value).Select(d => d.Key).Take(rem).ToArray();
                    _RemoveKeys(toRemove);
                }
            }
        }

        private void _RemoveKeys(TKey[] toRemove)
        {
            foreach (var toRemoveItem in toRemove)
            {
                (_Values[toRemoveItem] as IDisposable)?.Dispose();
                _Values.Remove(toRemoveItem);
                _Ages.Remove(toRemoveItem);
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            DateTime oldTime = DateTime.UtcNow - LifeSpan;
            return _Ages.Where(d => d.Value >= oldTime).Select(d => _Values[d.Key]).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public void Dispose()
        {
            Clear();
        }

        private void Clear()
        {
            foreach (TValue v in _Values.Values)
            {
                (v as IDisposable)?.Dispose();
            }
            _Values.Clear();
            _Ages.Clear();
        }
    }
}
