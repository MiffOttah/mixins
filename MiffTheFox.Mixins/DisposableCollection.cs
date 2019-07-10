using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Collections
{
    internal class DisposableCollection : ICollection<IDisposable>, IDisposable
    {
        private List<IDisposable> _Items = new List<IDisposable>();
        private bool _Disposed = false; // To detect redundant calls

        public int Count => _Items.Count;
        public bool IsReadOnly => ((ICollection<IDisposable>)_Items).IsReadOnly;

        public void Add(IDisposable item)
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_Index(item) != -1) throw new ArgumentException("This item is already in the collection.", nameof(item));

            _Items.Add(item);
        }

        public void Clear() => Clear(true);

        public void Clear(bool dispose)
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            if (dispose)
            {
                foreach (var i in _Items) i.Dispose();
            }
            _Items.Clear();
        }

        private int _Index(IDisposable item)
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            for (int i = 0; i < _Items.Count; i++)
            {
                if (ReferenceEquals(_Items[i], item)) return i;
            }
            return -1;
        }

        public bool Contains(IDisposable item) => _Index(item) != -1;

        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            _Items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<IDisposable> GetEnumerator()
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            return ((ICollection<IDisposable>)_Items).GetEnumerator();
        }

        public bool Remove(IDisposable item) => Remove(item, true);

        public bool Remove(IDisposable item, bool dispose)
        {
            int i = _Index(item);
            if (i != -1)
            {
                if (dispose)
                {
                    _Items[i].Dispose();
                }
                _Items.RemoveAt(i);
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_Disposed) throw new ObjectDisposedException(null);
            return ((ICollection<IDisposable>)_Items).GetEnumerator();
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    Clear();
                }

                _Items = null;
                _Disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
