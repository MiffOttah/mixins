using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Text
{
    internal static class StringUtil
    {
        public static StringPartitionResult Partition(this string str, char needle)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            int index = str.IndexOf(needle);
            return Partition(str, index, 1);

        }

        public static StringPartitionResult Partition(this string str, string needle)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (needle is null) throw new ArgumentNullException(nameof(needle));
            if (needle.Length == 0) throw new ArgumentException("Needle cannot be blank.", nameof(needle));

            int index = str.IndexOf(needle);
            return Partition(str, index, needle.Length);
        }

        public static StringPartitionResult Partition(this string str, int index, int partitionLength)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (index < -1) throw new ArgumentOutOfRangeException(nameof(index), "Index must be greater than or equal to -1.");
            if (index >= str.Length) throw new ArgumentOutOfRangeException(nameof(index), "Index must be less than the length of the string.");

            if (index == -1)
            {
                return new StringPartitionResult { Head = str, Tail = string.Empty };
            }
            else
            {
                int tailStart = index + partitionLength;
                return new StringPartitionResult
                {
                    Head = str.Remove(index),
                    Tail = (tailStart < str.Length) ? str.Substring(index + partitionLength) : string.Empty
                };
            }
        }

        public static int IndexOf(this string str, Func<char, bool> test)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (test is null) throw new ArgumentNullException(nameof(test));

            for (int i = 0; i < str.Length; i++)
            {
                if (test(str[i])) return i;
            }
            return -1;
        }

        public static int LastIndexOf(this string str, Func<char, bool> test)
        {
            if (str is null) throw new ArgumentNullException(nameof(str));
            if (test is null) throw new ArgumentNullException(nameof(test));

            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (test(str[i])) return i;
            }
            return -1;
        }

        public struct StringPartitionResult
        {
            public string Head;
            public string Tail;

            public void Deconstruct(out string head, out string tail)
            {
                head = Head;
                tail = Tail;
            }

            public override string ToString()
            {
                return $"({Head}, {Tail})";
            }

            public override bool Equals(object obj)
            {
                if (obj is StringPartitionResult that)
                {
                    return this.Head == that.Head && this.Tail == that.Tail;
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + ((Head is null) ? 0 : Head.GetHashCode());
                    hash = hash * 23 + ((Tail is null) ? 0 : Tail.GetHashCode());
                    return hash;
                }
            }
        }
    }
}
