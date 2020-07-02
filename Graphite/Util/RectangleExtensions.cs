using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Graphite.Util
{
    internal static class RectangleExtensions
    {
        public static void CenterIn(ref this RectangleF self, RectangleF other)
        {
            self.Location = other.Location + (other.Size - self.Size) / 2;
        }

        public static RectangleF ToRectangle(this FontRectangle fr)
        {
            return new RectangleF(new PointF(fr.Left, fr.Top), new SizeF(fr.Width, fr.Height));
        }
    }
}
