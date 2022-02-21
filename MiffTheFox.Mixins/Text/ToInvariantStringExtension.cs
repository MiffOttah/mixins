using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MiffTheFox.Text
{
    internal static class ToInvariantStringExtension
    {
        public static string ToInvariantString(this object o)
        {
            switch (o)
            {
                case null:
                case "":
                    return string.Empty;

                case IFormattable f:
                    return f.ToString(null, CultureInfo.InvariantCulture);

                default:
                    return o.ToString();
            }
        }
    }
}
