using System;
using Graphite.Typography;
using Graphite.Util;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite
{
    public abstract class Control
    {
        public Control Parent { get; protected internal set; }
        public Window Window { get; protected internal set; }

        public Rectangle Bounds { get; set; }

        public int Layer { get; set; }

        public Color ForegroundColor { get; set; } = Color.Black;
        public Color BackgroundColor { get; set; } = Color.White;

        public Font Font { get; set; } = Fonts.SegoeUi.CreateFont(36);

        public string Text { get; set; }

        public abstract void Draw(Image<Rgb24> buffer);

        protected void DrawString(Image<Rgb24> buffer, string s, Rectangle layoutRectangle)
        {
            var size = TextMeasurer.Measure(s, new RendererOptions(Font)).ToRectangle();
            size.CenterIn(layoutRectangle);

            buffer.Mutate(g => g.DrawText(s, Font, ForegroundColor, size.Location));
        }
    }
}
