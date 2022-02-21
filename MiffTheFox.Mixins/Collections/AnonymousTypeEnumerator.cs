using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Collections
{
    internal static class AnonymousTypeEnumerator
    {
        internal static IEnumerable<KeyValuePair<string, object>> Enumerate(object obj)
        {
            if (obj != null)
            {
                var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                
                for (int i = 0; i < props.Length; i++)
                {
                    yield return new KeyValuePair<string, object>(props[i].Name, props[i].GetValue(obj));
                }
            }
        }
    }
}
