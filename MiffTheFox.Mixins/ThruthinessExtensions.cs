using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox
{
    internal static class ThruthinessExtensions
    {
        public static void If<T>(this T obj, Action then, Action @else = null)
        {
            if (Truthiness.IsTruthy(obj))
            {
                then?.Invoke();
            }
            else
            {
                @else?.Invoke();
            }
        }

        public static void If<T>(this T obj, Action<T> then, Action<T> @else = null)
        {
            if (Truthiness.IsTruthy(obj))
            {
                then?.Invoke(obj);
            }
            else
            {
                @else?.Invoke(obj);
            }
        }

        public static TResult If<T, TResult>(this T obj, Func<T, TResult> then, Func<T, TResult> @else = null)
        {
            if (Truthiness.IsTruthy(obj))
            {
                return then is null ? default : then(obj);
            }
            else
            {
                return @else is null ? default : @else(obj);
            }
        }

        public static T Or<T>(this T obj, T @else)
        {
            return Truthiness.IsTruthy(obj) ? obj : @else;
        }
    }
}
