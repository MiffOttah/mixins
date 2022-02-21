using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Text
{
    /// <summary>
    /// Represents an item with a custom appearance when ToString is called
    /// </summary>
    internal class ToStringer<T>
    {
        public T Value { get; set; }
        public string StringValue { get; set; }

        public ToStringer(T value, string stringValue)
        {
            Value = value;
            StringValue = stringValue;
        }

        public ToStringer(T value) : this(value, value?.ToString()) { }
        public ToStringer(string stringValue) : this(default, stringValue) { }
        public ToStringer() : this(default, null) { }

        public override int GetHashCode() => Value?.GetHashCode() ?? 0;
        public override bool Equals(object obj) => obj is ToStringer<T> stringer && EqualityComparer<T>.Default.Equals(Value, stringer.Value);
        public override string ToString() => StringValue ?? Value?.ToString() ?? string.Empty;
    }
}
