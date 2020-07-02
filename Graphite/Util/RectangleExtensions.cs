using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace Graphite.Util
{
    internal static class RectangleExtensions
    {
        public static void CenterIn(ref this Rectangle self, Rectangle other)
        {
            self.Location = other.Location + (other.Size - self.Size) / 2;
        }

        public static Rectangle ToRectangle(this FontRectangle fr)
        {
            return new Rectangle(new Point((int)fr.Left, (int)fr.Top), new Size((int)fr.Width, (int)fr.Height));
        }
    }
}
