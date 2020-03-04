using System;
using System.Collections;

// WARNING - not ready for produciton useh

namespace MiffTheFox
{
    internal static class Truthiness
    {
        public static bool IsTruthy<T>(this T obj)
        {
            // note: nullables will pattern-match as null if
            // lacking a value, or will pattern-match to the underlying
            // type if possessing a value. thus, we can treat a nullable
            // with a value with the same mechanisms as for the underlying
            // type

            // null values are always falsey
            if (obj == null) return false;

            switch (obj)
            {
                case DBNull _:
                    // null values are always falsey
                    return false;

                case string stringValue:
                    // the null case is handled above
                    // so only check if the string is empty
                    return stringValue.Length > 0;

                case bool b:
                    // bool values are truthy if true
                    // seems pretty straighforward :)
                    return b;

                // integer values are truthy if nonzero
                case sbyte n: return n != 0;
                case byte n: return n != 0;
                case short n: return n != 0;
                case ushort n: return n != 0;
                case int n: return n != 0;
                case uint n: return n != 0U;
                case long n: return n != 0L;
                case ulong n: return n != 0UL;

                // floating point values are truthy if nonzero and not nan
                case float f: return !float.IsNaN(f) && f != 0f;
                case double f: return !double.IsNaN(f) && f != 0.0;

                case decimal dec:
                    // decimal values are truthy if nonzero
                    return dec != 0M;

                case char ch:
                    // chars are truthy if not NUL
                    return ch != '\0';

                case Enum e:
                    // use Activator to create the default enum
                    // value, then compare it to the provided value
                    return !Activator.CreateInstance(e.GetType()).Equals(e);

                case ICollection collection:
                    // ICollection handles lists, dictionarys, arrays
                    // and other collection types
                    return collection.Count > 0;

                case IEnumerable enumerable:
                    // if the length can't be read, but the value can
                    // be enumerated, attempt to enumerate it
                    IEnumerator enumerator = null;
                    try
                    {
                        enumerator = enumerable.GetEnumerator();
                        return enumerator.MoveNext();
                    }
                    finally
                    {
                        // IEnumerator<T> is required to be dispoable
                        // idk if i've ever seen an enumerator that
                        // would require disposing, but we
                        // might as well since IEnumerator<T>s are far
                        // more common than plan IEnumerators
                        (enumerator as IDisposable)?.Dispose();
                    }

                default:
                    // if the type is a struct, compare to the default
                    // if the struct doesn't implement Equals, then it will
                    // return false and the type is always truthy

                    if (obj.GetType().IsValueType)
                    {
                        return !Activator.CreateInstance(obj.GetType()).Equals(obj);
                    }

                    // if we don't know what to do with the type,
                    // then assume it's true
                    return true;
            }
        }
    }
}
