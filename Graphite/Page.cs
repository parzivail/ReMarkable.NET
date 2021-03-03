using Graphite.Controls;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite
{
    public class Page
    {
        public int Width { get; }
        public int Height { get; }
        public Panel Content { get; }

        public Window Window { get; }

        internal Page(Window window, int width, int height, Panel content)
        {
            Window = window;
            Width = width;
            Height = height;

            Content = content;
            Content.Window = window;
            Content.Bounds = new Rectangle(0, 0, width, height);
            Content.LayoutControls();
        }

        public void Draw(Image<Rgb24> buffer)
        {
            buffer.Mutate(g => g.Clear(Color.White));
            Content.Draw(buffer);
        }
    }
}