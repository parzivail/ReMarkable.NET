using System;
using System.Net;
using System.Net.Http.Headers;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Graphite.Util
{
    public static class RectangleExtensions
    {
        public static RectangleF GetSmallestContaining(this RectangleF a, RectangleF b)
        {
            var x = Math.Min(a.Left, b.Left);
            var y = Math.Min(a.Top, b.Top);

            var width = Math.Max(a.Right, b.Right) - x;
            var height = Math.Max(a.Bottom, b.Bottom) - y;

            return new RectangleF(x, y, width, height);
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

        public static void Align(ref this RectangleF src, RectangleF dest, RectAlign align)
        {
            var x = src.Left;
            var y = src.Top;

            // Handle horizontal positioning
            if (align.HasFlag(RectAlign.Left))
            {
                x = dest.Left;
            }
            else if (align.HasFlag(RectAlign.Center))
            {
                x = dest.Left + (dest.Width - src.Width) / 2;
            }
            else if (align.HasFlag(RectAlign.Right))
            {
                x = dest.Right - src.Width;
            }

            // Handle vertical positioning
            if (align.HasFlag(RectAlign.Top))
            {
                y = dest.Top;
            }
            else if (align.HasFlag(RectAlign.Middle))
            {
                y = dest.Top + (dest.Height - src.Height) / 2;
            }
            else if (align.HasFlag(RectAlign.Bottom))
            {
                y = dest.Bottom - src.Height;
            }

            src.Location = new PointF(x, y);
        }
    }
}
