using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Geometry
{
    internal struct Portion : IEquatable<Portion>, IComparable<Portion>, IFormattable
    {
        public readonly double Value;

        public static Portion Zero { get; } = new Portion(0);
        public static Portion One { get; } = new Portion(1);

        public Portion Complement => new Portion(1.0 - Value);

        public Portion(double amount) { Value = Clamp(amount); }

        public override bool Equals(object obj) => obj is Portion && Equals((Portion)obj);
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

        public double Lerp(double full) => Value * full;
        public double Lerp(double min, double max) => (Value * (max - min)) + min;

        public int Lerp(int full) => Convert.ToInt32(Math.Round(Lerp(Convert.ToDouble(full))));
        public int Lerp(int min, int max) => Convert.ToInt32(Math.Round(Lerp(Convert.ToDouble(min), Convert.ToDouble(max))));

        public static Portion Fraction(int numerator, int denominator) => new Portion(Convert.ToDouble(numerator) / Convert.ToDouble(denominator));
        public static Portion Fraction(int denominator) => new Portion(1.0 / Convert.ToDouble(denominator));

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
