using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox
{
    public static class CompareExtensions
    {
        public static IEnumerable<T> Ordered<T>(this IEnumerable<T> sequence) where T : IComparable<T>
        {
            var l = new List<T>(sequence);
            l.Sort(new ComparableComparer<T>());
            return l.AsReadOnly();
        }

        public static IEnumerable<T> Ordered<T>(this IEnumerable<T> sequence, IComparer<T> comparer)
        {
            var l = new List<T>(sequence);
            l.Sort(comparer);
            return l.AsReadOnly();
        }

        public static IComparer<T> Reversed<T>(this IComparer<T> comparer)
        {
            return Comparer<T>.Create((x, y) => -comparer.Compare(x, y));
        }
    }

    public sealed class ComparableComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return x.CompareTo(y);
        }
    }

    public sealed class MapComparer<T, TMapResult> : IComparer<T>
    {
        private readonly Func<T, TMapResult> _MapFunction;
        public IComparer<TMapResult> ResultComparer { get; set; }

        public MapComparer(Func<T, TMapResult> mapFunction) : this(mapFunction, Comparer<TMapResult>.Default)
        {
        }

        public MapComparer(Func<T, TMapResult> mapFunction, IComparer<TMapResult> resultComparer)
        {
            _MapFunction = mapFunction;
            ResultComparer = resultComparer;
        }

        public int Compare(T x, T y)
        {
            return ResultComparer.Compare(_MapFunction(x), _MapFunction(y));
        }
    }
}
