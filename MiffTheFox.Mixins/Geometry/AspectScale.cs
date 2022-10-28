using System;
using System.Collections.Generic;
using System.Text;

namespace MiffTheFox.Geometry
{
    internal static class AspectScale
    {
        internal static void ScaleToFitSquare(int targetSize, int srcWidth, int srcHeight, out int scaledWidth, out int scaledHeight)
        {
            if (targetSize <= 0 || srcWidth <= 0 || srcHeight <= 0) throw new ArgumentException("Sizes must be positive.");

            double scale;
            if (srcWidth > srcHeight)
            {
                scale = (double)targetSize / srcWidth;
            }
            else
            {
                scale = (double)targetSize / srcHeight;
            }

            scaledWidth = (int)Math.Round(scale * srcWidth);
            scaledHeight = (int)Math.Round(scale * srcHeight);
        }
    }
}
