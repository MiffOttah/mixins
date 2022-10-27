using System;

namespace MiffTheFox.Geometry
{
    /// <summary>
    /// Repersents a geometric angle in 2D space.
    /// </summary>
    internal struct Angle : IEquatable<Angle>, IComparable<Angle>, IFormattable
    {
        /// <summary>
        /// The zero angle where both sides are the same ray.
        /// </summary>
        public static readonly Angle Zero = new Angle(0, AngleUnit.Turns);

        /// <summary>
        /// The value of the angle in turns.
        /// </summary>
        public readonly double Turns;

        /// <summary>
        /// The value of the angle in degrees.
        /// </summary>
        public readonly double Degrees;

        /// <summary>
        /// The value of hte angle in radians.
        /// </summary>
        public readonly double Radians;

        /// <summary>
        /// Creates an angle of the specified size and angle unit.
        /// </summary>
        /// <param name="value">The size of the angle.</param>
        /// <param name="unit">The unit the size of the angle is measured in.</param>
        public Angle(double value, AngleUnit unit)
        {
            if (double.IsNaN(value)) throw new ArgumentException("Value cannot be NaN.", nameof(value));
            Turns = value / GetConstantForAngleUnit(unit);
            Degrees = unit == AngleUnit.Degrees ? value : Turns * GetConstantForAngleUnit(AngleUnit.Degrees);
            Radians = unit == AngleUnit.Radians ? value : Turns * GetConstantForAngleUnit(AngleUnit.Radians);
        }

        /// <summary>
        /// Gets the angle value that, in the specified unit, repersents a full circle.
        /// </summary>
        /// <param name="unit">The unit to get the constant for.</param>
        public static double GetConstantForAngleUnit(AngleUnit unit)
        {
            switch (unit)
            {
                case AngleUnit.Turns: return 1;
                case AngleUnit.Degrees: return 360;
                case AngleUnit.Radians: return Math.PI * 2;
                case AngleUnit.PiRadians: return 2;
                case AngleUnit.Gradians: return 400;
                case AngleUnit.Percent: return 100;
                default: throw new ArgumentException("Unknown angle unit.", nameof(unit));
            }
        }

        /// <summary>
        /// Returns the value of the angle in the specified unit.
        /// </summary>
        public double ToUnit(AngleUnit unit) => Turns * GetConstantForAngleUnit(unit);

        public override string ToString() => ToString(AngleUnit.Turns, "G", null);
        public string ToString(AngleUnit unit) => ToString(unit, "G", null);
        public string ToString(AngleUnit unit, string decimalFormat, IFormatProvider formatProvider)
        {
            switch (unit)
            {
                case AngleUnit.Turns: return Turns.ToString(decimalFormat, formatProvider) + "τ";
                case AngleUnit.Degrees: return Degrees.ToString(decimalFormat, formatProvider) + "°";
                case AngleUnit.Radians: return Radians.ToString(decimalFormat, formatProvider);
                case AngleUnit.PiRadians: return ToUnit(AngleUnit.PiRadians).ToString(decimalFormat, formatProvider) + "π";
                case AngleUnit.Gradians: return ToUnit(AngleUnit.PiRadians).ToString(decimalFormat, formatProvider) + " gon";
                case AngleUnit.Percent: return ToUnit(AngleUnit.Percent).ToString(decimalFormat, formatProvider) + "%";
                default: throw new ArgumentException("Unknown angle unit.", nameof(unit));
            }
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var unit = AngleUnit.Turns;
            if (!string.IsNullOrEmpty(format))
            {
                switch (format[format.Length - 1])
                {
                    case 'τ': format = format.Remove(format.Length - 1); break;
                    case 'π': unit = AngleUnit.PiRadians; format = format.Remove(format.Length - 1); break;
                    case '°': unit = AngleUnit.Degrees; format = format.Remove(format.Length - 1); break;
                }
            }
            return ToString(unit, format, formatProvider);
        }

        public override bool Equals(object obj) => obj is Angle && Equals((Angle)obj);
        public bool Equals(Angle other) => Turns == other.Turns;
        public bool Equals(Angle other, double delta) => Math.Abs(Turns - other.Turns) < delta;
        public override int GetHashCode() => unchecked(1224962691 + Turns.GetHashCode());
        public int CompareTo(Angle other) => Turns.CompareTo(other.Turns);
        public int CompareTo(Angle other, double delta)
        {
            var rel = Turns - other.Turns;
            if (rel < delta && rel > -delta) return 0;
            return Math.Sign(rel);
        }

        public static bool operator ==(Angle x, Angle y) => x.Turns == y.Turns;
        public static bool operator !=(Angle x, Angle y) => x.Turns != y.Turns;
        public static bool operator >(Angle x, Angle y) => x.Turns > y.Turns;
        public static bool operator >=(Angle x, Angle y) => x.Turns >= y.Turns;
        public static bool operator <(Angle x, Angle y) => x.Turns < y.Turns;
        public static bool operator <=(Angle x, Angle y) => x.Turns <= y.Turns;

        public static Angle operator +(Angle x, Angle y) => new Angle(x.Turns + y.Turns, AngleUnit.Turns);
        public static Angle operator -(Angle x, Angle y) => new Angle(x.Turns - y.Turns, AngleUnit.Turns);
        public static Angle operator *(Angle x, Angle y) => new Angle(x.Turns * y.Turns, AngleUnit.Turns);
        public static Angle operator *(Angle portion, double multiple) => new Angle(portion.Turns * multiple, AngleUnit.Turns);
        public static Angle operator *(double multiple, Angle portion) => new Angle(portion.Turns * multiple, AngleUnit.Turns);
        public static Angle operator /(Angle portion, double multiple) => new Angle(portion.Turns / multiple, AngleUnit.Turns);

        /// <summary>
        /// Returns an angle greater than or equal to 0τ but less than 1τ that is coterminal with this angle.
        /// </summary>
        public Angle Canonical
        {
            get
            {
                double d = Turns;
                while (d < 0) d += 1;
                while (d > 1) d -= 1;
                return new Angle(d, AngleUnit.Turns);
            }
        }

        public static Angle ArcSin(double sine) => new Angle(Math.Asin(sine), AngleUnit.Radians);
        public static Angle ArcCos(double cosine) => new Angle(Math.Acos(cosine), AngleUnit.Radians);
        public static Angle ArcTan(double tangent) => new Angle(Math.Atan(tangent), AngleUnit.Radians);
        public static Angle ArcTan2(double y, double x) => new Angle(Math.Atan2(y, x), AngleUnit.Radians);

        public double Cos() => Math.Cos(Radians);
        public double Sin() => Math.Sin(Radians);
        public double Tan() => Math.Tan(Radians);
        public double Cosh() => Math.Cosh(Radians);
        public double Sinh() => Math.Sinh(Radians);
        public double Tanh() => Math.Tanh(Radians);

        public (double, double) ToPoint()
        {
            return (Math.Cos(Radians), Math.Sin(Radians));
        }

        public (double, double) ToPoint(double radius)
        {
            return (Math.Cos(Radians) * radius, Math.Sin(Radians) * radius);
        }

        public (double, double) ToPoint(double centerX, double centerY, double radius)
        {
            return (Math.Cos(Radians) * radius + centerX, Math.Sin(Radians) * radius + centerY);
        }

        public static explicit operator Portion(Angle angle) => new Portion(angle.Turns);
        public static explicit operator Angle(Portion portion) => new Angle(portion.Value, AngleUnit.Turns);

        internal enum AngleUnit : byte
        {
            /// <summary>Turns, where a complete circle is 1. Also the unit in radians as a multiple of τ (2π).</summary>
            Turns = 0,
            /// <summary>Degrees, where a complete circle is 360°.</summary>
            Degrees = 1,
            /// <summary>Radians, where a complete circle is 2π.</summary>
            Radians = 2,
            /// <summary>Radians as a multiple of π, where a complete circle is 2.</summary>
            PiRadians = 3,
            /// <summary>Gradians, where a complete circle is 400.</summary>
            Gradians = 4,
            /// <summary>Percent, where a complete circle is 100.</summary>
            Percent = 5
        }
    }
}
