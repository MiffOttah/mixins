using System;
using System.Collections.Generic;
using System.Text;

// WARNING - not ready for produciton use

namespace MiffTheFox
{
    internal static class Truthiness
    {
        public static bool IsTruthy(this object obj)
        {
            // note: nullables will pattern-match as null if
            // lacking a value, or will pattern-match to the underlying
            // type if possessing a value. thus, we can treat a nullable
            // with a value with the same mechanisms as for the underlying
            // type

            switch (obj)
            {
                case null:
                case System.DBNull _:
                    // null values are always falsey
                    return false;

                case string stringValue:
                    // the null case is handled above
                    // so only check if the string is empty
                    return stringValue.Length > 0;

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
                    // todo: default enum value
                    throw new NotImplementedException();

                case System.Collections.ICollection collection:
                    // todo: collection length
                    throw new NotImplementedException();

                // xxx: are there other types to check?

                default:
                    return true;
            }
        }
    }
}
