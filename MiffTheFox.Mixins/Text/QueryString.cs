using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MiffTheFox.Text
{
    internal class QueryString : IDictionary<string, string>, ICloneable
    {
        private readonly Dictionary<string, string> _Values;

        public ICollection<string> Keys => _Values.Keys;
        public ICollection<string> Values => _Values.Values;
        public int Count => _Values.Count;
        public bool IsReadOnly => false;
        public bool IsEmpty => _Values.Count == 0;

        public static QueryString Empty => new QueryString();

        public QueryString()
        {
            _Values = new Dictionary<string, string>();
        }

        public QueryString(string queryString)
        {
            _Values = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(queryString))
            {
                int i = 0;
                int ampersand;
                do
                {
                    ampersand = queryString.IndexOf('&', i);
                    string part = (ampersand == -1) ? queryString.Substring(i) : queryString.Substring(i, ampersand - i);
                    int equals = part.IndexOf('=');
                    if (equals == -1)
                    {
                        Add(part, null);
                    }
                    else
                    {
                        try
                        {
                            var value = BinString.FromUrlString(part.Substring(equals + 1));
                            Add(part.Substring(0, equals), value.ToString(Encoding.UTF8));
                        }
                        catch
                        {
                            // ignore any invalid parameters
                        }
                    }

                    i = ampersand + 1;
                } while (ampersand != -1);
            }
        }

        public QueryString(IDictionary<string, string> source)
        {
            _Values = new Dictionary<string, string>();
            if (source != null)
            {
                foreach (string k in source.Keys)
                {
                    _Values[k] = source[k];
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var kvp in _Values)
            {
                if (sb.Length > 0) sb.Append('&');
                sb.Append(kvp.Key);

                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    sb.Append('=');
                    sb.Append(BinString.FromTextString(kvp.Value, Encoding.UTF8).ToUrlString(false, CultureInfo.InvariantCulture));
                }
            }
            return sb.ToString();
        }

        public void Overlay(QueryString other)
        {
            if (other != null)
            {
                foreach (var kvp in other)
                {
                    _Values[kvp.Key] = kvp.Value;
                }
            }
        }

        public string this[string key]
        {
            get => GetString(key);
            set => _Values[key] = value;
        }


        public string GetString(string key, string defaultValue = null)
        {
            return _Values.ContainsKey(key) ? _Values[key] : defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return _Values.ContainsKey(key) && int.TryParse(_Values[key], NumberStyles.Integer, CultureInfo.InvariantCulture, out int n) ? n : defaultValue;
        }

        public bool GetBoolean(string key, bool defaultValue = false)
        {
            string v = GetString(key, defaultValue ? "1" : null);
            if (string.IsNullOrEmpty(v)) return false;
            switch (v.Trim().ToLowerInvariant())
            {
                case "":
                case "0":
                case "false":
                case "no":
                    return false;

                default:
                    return true;
            }
        }

        public QueryString Clone() => new QueryString(this);
        object ICloneable.Clone() => new QueryString(this);

        public void Add(string key, string value) => _Values.Add(key, value);
        public bool ContainsKey(string key) => _Values.ContainsKey(key);
        public bool Remove(string key) => _Values.Remove(key);
        public bool TryGetValue(string key, out string value) => _Values.TryGetValue(key, out value);
        public void Clear() => _Values.Clear();

        void ICollection<KeyValuePair<string, string>>.Add(KeyValuePair<string, string> item)
        {
            ((IDictionary<string, string>)_Values).Add(item);
        }
        bool ICollection<KeyValuePair<string, string>>.Contains(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_Values).Contains(item);
        }
        void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            ((IDictionary<string, string>)_Values).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
        {
            return ((IDictionary<string, string>)_Values).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_Values).GetEnumerator();

        public static QueryString operator +(QueryString x, QueryString y)
        {
            if (x is null)
            {
                return new QueryString(y);
            }
            else
            {
                var r = x.Clone();
                r.Overlay(y);
                return r;
            }
        }
    }
}
