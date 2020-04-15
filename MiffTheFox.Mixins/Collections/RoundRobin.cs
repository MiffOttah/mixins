using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiffTheFox.Collections
{
    internal class RoundRobin<T> : ICollection<T>, ICollection
    {
        private readonly T[] _Items;
        private int _ItemIndex;

        public int MaximumItems => _Items.Length;
        public int Count { get; private set; }

        public bool IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;

        public RoundRobin(int maximumItems)
        {
            if (maximumItems <= 0) throw new ArgumentOutOfRangeException(nameof(maximumItems), "Maximum items must be greater than zero.");
            _Items = new T[maximumItems];
            _ItemIndex = 0;
            Count = 0;
        }

        protected IEnumerable<T> Enumerate()
        {
            for (int i = 0; i < MaximumItems; i++)
            {
                // add i to _ItemIndex and wrap around
                // so that the enumeration starts with the
                // oldest item
                int i2 = (i + _ItemIndex) % MaximumItems;

                // terminate early if the array isn't full
                if (i2 >= Count) continue;
                
                // yield this item
                yield return _Items[i2];
            }
        }

        public void Add(T item)
        {
            _Items[_ItemIndex] = item;
            if (Count < MaximumItems) Count++;

            _ItemIndex++;
            if (_ItemIndex >= MaximumItems) _ItemIndex = 0;
        }

        public void Clear()
        {
            _ItemIndex = 0;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return Enumerate().Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (arrayIndex + Count > array.Length) throw new ArgumentException("The number of elements in the source ICollection<T> is greater than the available space from arrayIndex to the end of the destination array.");

            int i = 0;
            foreach (var item in Enumerate())
            {
                array[i + arrayIndex] = item;
                i++;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1) throw new ArgumentException("Array is multidimensional.", nameof(array));
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            if (index + Count > array.Length) throw new ArgumentException("The number of elements in the source ICollection<T> is greater than the available space from arrayIndex to the end of the destination array.");
            if (!array.GetType().GetElementType().IsAssignableFrom(typeof(T))) throw new ArgumentException("The type of the source ICollection cannot be cast automatically to the type of the destination array.", nameof(array));

            var values = ToArray();
            Array.Copy(values, 0, array, index, values.Length);
        }

        public T[] ToArray()
        {
            var result = new T[Count];
            int i = 0;
            foreach (var item in Enumerate())
            {
                result[i++] = item;
            }
            return result;
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator() => Enumerate().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Enumerate().GetEnumerator();
    }
}
