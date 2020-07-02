using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Graphite
{
    public abstract class Control
    {
        public Control Parent { get; protected internal set; }
        public Window Window { get; protected internal set; }

        public Rectangle Bounds { get; set; }
        public Color ForegroundColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.Transparent;

        public abstract void Draw(Image<Rgb24> buffer);
    }
}
