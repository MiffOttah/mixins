using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Collections
{
    internal static class Enumerable
    {
        /// <summary>
        /// Tries to retrieve the first item in the sequence, returning false if the sequence is empty.
        /// </summary>
        internal static bool TryFirst<T>(this IEnumerable<T> enumerable, out T value)
        {
            // This code is based on the code used in .NET's internal
            // System.Linq.Enuemrable.TryGetFirst<TSource> method.
            // Why the heck is this method internal? It's useful!

            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            else if (enumerable is IList<T> list)
            {
                if (list.Count > 0)
                {
                    value = list[0];
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
            else
            {
                using (var e = enumerable.GetEnumerator())
                {
                    if (e.MoveNext())
                    {
                        value = e.Current;
                        return true;
                    }
                    else
                    {
                        value = default;
                        return false;
                    }
                }
            }
        }
    }
}
