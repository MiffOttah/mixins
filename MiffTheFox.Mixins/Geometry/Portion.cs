using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Geometry
{
    /// <summary>
    /// Represents a portion of a whole, that is, a floating point value between 0 and 1 inclusively.
    /// </summary>
    internal readonly struct Portion : IEquatable<Portion>, IComparable<Portion>, IFormattable
    {
        /// <summary>
        /// The value represented by this portion
        /// </summary>
        public readonly double Value;

        /// <summary>
        /// A portion representing none of the whole with a value of 0.
        /// </summary>
        public static Portion Zero { get; } = new Portion(0);

        /// <summary>
        /// A portion representing all of the whole with a value of 1.
        /// </summary>
        public static Portion One { get; } = new Portion(1);

        /// <summary>
        /// The portion which, when added to this portion, has a total value of 1.
        /// </summary>
        public Portion Complement => new Portion(1.0 - Value);

        public Portion(double amount) { Value = Clamp(amount); }

        public override bool Equals(object obj) => obj is Portion portion && Equals(portion);
        public bool Equals(Portion other) => Value == other.Value;
        public override int GetHashCode() => -1179731741 + Value.GetHashCode();

        public int CompareTo(Portion other)
        {
            return Value.CompareTo(other.Value);
        }

        public static bool operator ==(Portion x, Portion y) => x.Value == y.Value;
        public static bool operator !=(Portion x, Portion y) => x.Value != y.Value;
        public static bool operator >(Portion x, Portion y) => x.Value > y.Value;
        public static bool operator >=(Portion x, Portion y) => x.Value >= y.Value;
        public static bool operator <(Portion x, Portion y) => x.Value < y.Value;
        public static bool operator <=(Portion x, Portion y) => x.Value <= y.Value;

        public static Portion operator +(Portion x, Portion y) => new Portion(x.Value + y.Value);
        public static Portion operator -(Portion x, Portion y) => new Portion(x.Value - y.Value);
        public static Portion operator *(Portion x, Portion y) => new Portion(x.Value * y.Value);
        public static Portion operator *(Portion portion, double multiple) => new Portion(portion.Value * multiple);
        public static Portion operator *(double multiple, Portion portion) => new Portion(portion.Value * multiple);
        public static Portion operator /(Portion portion, double multiple) => new Portion(portion.Value / multiple);

        public static Portion operator !(Portion x) => x.Complement;

        public override string ToString() => Value.ToString("G", null);
        public string ToString(string format) => Value.ToString(format, null);
        public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);

        /// <summary>
        /// Calculates a value between 0 and <paramref name="full"/> represented by this portion.
        /// </summary>
        /// <param name="full">The maximum value to return</param>
        public double Lerp(double full) => Value * full;

        /// <summary>
        /// Calcuates a value between <paramref name="min"/> and <paramref name="max"/>, inclusive represented by this portion.
        /// </summary>
        /// <param name="min">The minimum value returned if the portion is 0.</param>
        /// <param name="max">The maximum value retured if the portion is 1.</param>
        public double Lerp(double min, double max) => (Value * (max - min)) + min;

        /// <summary>
        /// Calculates a value between 0 and <paramref name="full"/> represented by this portion.
        /// </summary>
        /// <param name="full">The maximum value to return</param>
        public int Lerp(int full) => Convert.ToInt32(Math.Round(Lerp(Convert.ToDouble(full))));

        /// <summary>
        /// Calcuates a value between <paramref name="min"/> and <paramref name="max"/>, inclusive represented by this portion.
        /// </summary>
        /// <param name="min">The minimum value returned if the portion is 0.</param>
        /// <param name="max">The maximum value retured if the portion is 1.</param>
        public int Lerp(int min, int max) => Convert.ToInt32(Math.Round(Lerp(Convert.ToDouble(min), Convert.ToDouble(max))));

        /// <summary>
        /// Creates a portion based on a fraction equal to <paramref name="numerator"/> over <paramref name="denominator"/>.
        /// </summary>
        /// <param name="numerator">The numerator of the fraction.</param>
        /// <param name="denominator">The demoninator of the fraction.</param>
        public static Portion Fraction(int numerator, int denominator) => new Portion(Convert.ToDouble(numerator) / Convert.ToDouble(denominator));

        /// <summary>
        /// Creates a portion based on a fraction equal to 1 over <paramref name="denominator"/>.
        /// </summary>
        /// <param name="denominator">The demoninator of the fraction.</param>
        public static Portion Fraction(int denominator) => new Portion(1.0 / Convert.ToDouble(denominator));

        /// <summary>
        /// Clamps a value within the range of 0.0 to 1.0, inclusive.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <returns>0 if the value is less than zero or NaN, 1 if the value is greater than 1, the value itself otherwise.</returns>
        public static double Clamp(double value)
        {
            if (double.IsNaN(value)) return 0.0;
            else if (value > 1.0) return 1.0;
            else if (value < 0.0) return 0.0;
            else return value;
        }

        public static explicit operator double(Portion v) => v.Value;
        public static explicit operator float(Portion v) => Convert.ToSingle(v.Value);
        public static explicit operator decimal(Portion v) => Convert.ToDecimal(v.Value);
        public static explicit operator byte(Portion v) => Convert.ToByte(v.Lerp(255));

        public static explicit operator Portion(double v) => new Portion(v);
        public static explicit operator Portion(float v) => new Portion(v);
        public static explicit operator Portion(decimal v) => new Portion(Convert.ToDouble(v));
        public static explicit operator Portion(byte v) => new Portion(Convert.ToDouble(v) / 255.0);
    }
}
