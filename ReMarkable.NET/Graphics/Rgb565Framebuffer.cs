using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Graphics
{
    public sealed class Rgb565Framebuffer : IDisposable
    {
        private readonly FileStream _deviceStream;

        public string DevicePath { get; }
        public int Width { get; }
        public int Height { get; }

        public Rgb565Framebuffer(string devicePath, int width, int height)
        {
            DevicePath = devicePath;
            Width = width;
            Height = height;
            
            _deviceStream = File.Open(DevicePath, FileMode.Open);
        }

        public Image<Rgb24> Read(Rectangle area)
        {
            return Image.Load<Rgb24>(_deviceStream, new FramebufferDecoder(this, area));
        }

        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>
        {
            image.Save(_deviceStream, new FramebufferEncoder(this, srcArea, destPoint));
        }

        internal int PointToOffset(int x, int y)
        {
            return (Width * y + x) * sizeof(short);
        }

        internal Point OffsetToPoint(long offset)
        {
            offset /= sizeof(short);
            var x = (int)(offset % Width);
            var y = (int)(offset / Width);

            return new Point(x, y);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _deviceStream?.Dispose();
        }
    }
}