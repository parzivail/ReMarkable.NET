using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;

namespace Graphite.Util
{
    internal static class PointExtensions
    {
        public static Point ToInteger(this PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
    }
}
