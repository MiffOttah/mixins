using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Collections
{
    /// <summary>
    /// Repersents a list of <typeparamref name="T"/> that will never contain any items.
    /// </summary>
    internal class EmptyList<T> : IList<T>
    {
        // The static shared list is initilized the first time the type is
        // referenced for a particular T.
        public static EmptyList<T> Shared { get; } = new EmptyList<T>();
        private static readonly EmptyEnumerator _Enumerator = new EmptyEnumerator();

        public int Count => 0;
        public bool IsReadOnly => true;

        public T this[int index]
        {
            // always throw the IndexOutOfRangeException since there are no
            // possible valid indices.
            get => throw new IndexOutOfRangeException();
            set => throw new IndexOutOfRangeException();
        }

        public EmptyList()
        {
        }

        // the list will never contain anything
        public bool Contains(T item) => false;
        public int IndexOf(T item) => -1;
        public bool Remove(T item) => false;

        // being read-only, the list will always throw NotSupportedException
        // in response to attempts to modify it.
        public void Add(T item) => throw new NotSupportedException();
        public void Insert(int index, T item) => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();

        // as with the accessor, there are no valid indices into the list
        public void RemoveAt(int index) => throw new ArgumentOutOfRangeException();

        // technically this is copying nothing into the array, so this is a no-op
        public void CopyTo(T[] array, int arrayIndex) { }
        
        public IEnumerator<T> GetEnumerator() => _Enumerator;
        IEnumerator IEnumerable.GetEnumerator() => _Enumerator;

        private class EmptyEnumerator : IEnumerator<T>
        {
            public T Current => default;
            object IEnumerator.Current => default(T);
            public void Dispose() { }
            public bool MoveNext() => false;
            public void Reset() { }
        }
    }
}
