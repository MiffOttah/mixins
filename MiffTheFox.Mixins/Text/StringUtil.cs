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
                return new StringPartitionResult(str, string.Empty);
            }
            else
            {
                int tailStart = index + partitionLength;
                return new StringPartitionResult(
                    str.Remove(index),
                    (tailStart < str.Length) ? str.Substring(index + partitionLength) : string.Empty
                );
            }
        }

        // TODO: test
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

        // TODO: test
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

        public readonly struct StringPartitionResult
        {
            public string Head { get; }
            public string Tail { get; }

            public StringPartitionResult(string head, string tail)
            {
                Head = head;
                Tail = tail;
            }

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
                return (obj is StringPartitionResult that) ? (Head == that.Head && Tail == that.Tail) : base.Equals(obj);
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

        public static bool SplitContains(this string str, char partition, string needle)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (string.IsNullOrEmpty(needle) || needle.IndexOf(partition) != -1)
            {
                throw new ArgumentException($"'{nameof(needle)}' cannot be null or empty, nor can it contain the value of '{nameof(partition)}'.", nameof(needle));
            }

            if (str.Length == 0) return false;

            int matchIndex = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (matchIndex > -1 && str[i] == needle[matchIndex])
                {
                    matchIndex++;

                    // matched the entire needle
                    if (matchIndex >= needle.Length)
                    {
                        // are we actually at the end of the piece
                        if (i >= (str.Length - 1) || str[i + 1] == partition)
                        {
                            return true;
                        }
                        else
                        {
                            matchIndex = -1;
                        }
                    }
                }
                else if (str[i] == partition)
                {
                    // start of a new partition, so start matching
                    matchIndex = 0;
                }
                else
                {
                    // this area is not a match, so just continue on until partition is matched
                    matchIndex = -1;
                }
            }

            // nothing found
            return false;
        }
    }
}
