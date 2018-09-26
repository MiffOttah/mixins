using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiffTheFox.Geometry
{
    public struct Portion : IEquatable<Portion>, IComparable<Portion>, IFormattable
    {
        private double _Value;

        public double Value
        {
            get => _Value;
            set => _Value = Clamp(value);
        }

        public static Portion Zero => new Portion { _Value = 0 };
        public static Portion One => new Portion { _Value = 1 };

        public Portion Complement => new Portion { _Value = 1.0 - _Value };

        public Portion(double amount) { _Value = Clamp(amount); }

        public override bool Equals(object obj) => obj is Portion && Equals((Portion)obj);
        public bool Equals(Portion other) => _Value == other._Value;
        public override int GetHashCode() => -1179731741 + _Value.GetHashCode();

        public int CompareTo(Portion other)
        {
            return _Value.CompareTo(other._Value);
        }

        public static bool operator ==(Portion x, Portion y) => x._Value == y._Value;
        public static bool operator !=(Portion x, Portion y) => x._Value != y._Value;
        public static bool operator >(Portion x, Portion y) => x._Value > y._Value;
        public static bool operator >=(Portion x, Portion y) => x._Value >= y._Value;
        public static bool operator <(Portion x, Portion y) => x._Value < y._Value;
        public static bool operator <=(Portion x, Portion y) => x._Value <= y._Value;

        public static Portion operator +(Portion x, Portion y) => new Portion(x._Value + y._Value);
        public static Portion operator -(Portion x, Portion y) => new Portion(x._Value - y._Value);
        public static Portion operator *(Portion x, Portion y) => new Portion(x._Value * y._Value);
        public static Portion operator *(Portion portion, double multiple) => new Portion(portion._Value * multiple);
        public static Portion operator *(double multiple, Portion portion) => new Portion(portion._Value * multiple);
        public static Portion operator /(Portion portion, double multiple) => new Portion(portion._Value / multiple);

        public static Portion operator !(Portion x) => x.Complement;

        public override string ToString() => _Value.ToString("G", null);
        public string ToString(string format) => _Value.ToString(format, null);
        public string ToString(string format, IFormatProvider formatProvider) => _Value.ToString(format, formatProvider);

        public double Lerp(double full) => _Value * full;
        public double Lerp(double min, double max) => (_Value * (max - min)) + min;

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
