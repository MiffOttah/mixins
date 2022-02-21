using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Text
{
    internal static class StringBuilderAppendJoin
    {
        public static StringBuilder AppendJoin(this StringBuilder builder, IEnumerable<string> strings, char separator)
        {
            if (strings != null)
            {
                bool first = true;
                foreach (string s in strings)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(separator);
                    }

                    builder.Append(s);
                }
            }
            return builder;
        }

        public static StringBuilder AppendJoin(this StringBuilder builder, IEnumerable<string> strings, string separator)
        {

            if (strings != null)
            {
                bool first = true;
                foreach (string s in strings)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(separator);
                    }

                    builder.Append(s);
                }
            }
            return builder;
        }

        public static StringBuilder AppendJoin<T>(this StringBuilder builder, IEnumerable<T> strings, char separator)
        {
            if (strings != null)
            {
                bool first = true;
                foreach (T s in strings)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(separator);
                    }

                    if (s != null)
                    {
                        builder.Append(s.ToString());
                    }
                }
            }
            return builder;
        }

        public static StringBuilder AppendJoin<T>(this StringBuilder builder, IEnumerable<T> strings, string separator)
        {
            if (strings != null)
            {
                bool first = true;
                foreach (T s in strings)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(separator);
                    }

                    if (s != null)
                    {
                        builder.Append(s.ToString());
                    }
                }
            }
            return builder;
        }
    }
}
