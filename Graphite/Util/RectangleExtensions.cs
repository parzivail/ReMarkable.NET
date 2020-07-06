using System;
using System.Net.Http.Headers;
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

        public static void CenterInVertically(ref this RectangleF self, RectangleF other)
        {
            var loc = other.Location + (other.Size - self.Size) / 2;
            self.Location = new PointF(self.Left, loc.Y);
        }

        public static void CenterInHorizontally(ref this RectangleF self, RectangleF other)
        {
            var loc = other.Location + (other.Size - self.Size) / 2;
            self.Location = new PointF(loc.X, self.Top);
        }

        public static RectangleF ToRectangle(this FontRectangle fr)
        {
            return new RectangleF(new PointF(fr.Left, fr.Top), new SizeF(fr.Width, fr.Height));
        }

        public static Rectangle GetContainingIntRect(this RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)Math.Round(rect.Width), (int)Math.Round(rect.Height));
        }

        public static RectangleF Inflated(this RectangleF rect, float width, float height)
        {
            var rect2 = new RectangleF(rect.Location, rect.Size);
            rect2.Inflate(width, height);
            return rect2;
        }
    }
}
