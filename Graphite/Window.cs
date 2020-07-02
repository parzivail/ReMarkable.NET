using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite
{
    public class Window
    {
        private readonly Stack<Page> _pages;

        public int Width { get; }
        public int Height { get; }

        public bool CanGoBack => _pages.Count > 0;

        public event EventHandler<WindowUpdateEventArgs> Update;

        public Image<Rgb24> Buffer { get; }

        public Window(int width, int height)
        {
            Width = width;
            Height = height;

            Buffer = new Image<Rgb24>(Width, Height);
            Buffer.Mutate(g => g.Clear(Color.White));

            _pages = new Stack<Page>();
        }

        public Page CreatePage()
        {
            return new Page(this, Width, Height);
        }

        public void Forward(Page page)
        {
            _pages.Push(page);
            Refresh(new Rectangle(0, 0, Width, Height));
        }

        public void Back()
        {
            _pages.Pop();
            Refresh(new Rectangle(0, 0, Width, Height));
        }

        public void Refresh(Rectangle rectangle)
        {
            _pages.Peek().Draw(Buffer);
            Update?.Invoke(this, new WindowUpdateEventArgs(Buffer, rectangle));
        }
    }
}