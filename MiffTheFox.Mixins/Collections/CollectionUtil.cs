using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiffTheFox.Collections
{
    public static class CollectionUtil
    {
        public static T[] Concat<T>(params ICollection<T>[] collections)
        {
            if (collections is null)
            {
                throw new ArgumentNullException(nameof(collections));
            }

            int count = 0;
            for (int i = 0; i < collections.Length; i++)
            {
                if (collections[i] != null)
                {
                    count += collections[i].Count;
                }
            }

            var result = new T[count];
            int position = 0;

            for (int i = 0; i < collections.Length; i++)
            {
                if (collections[i] != null)
                {
                    collections[i].CopyTo(result, position);
                    position += collections[i].Count;
                }
            }

            return result;
        }
    }
}
