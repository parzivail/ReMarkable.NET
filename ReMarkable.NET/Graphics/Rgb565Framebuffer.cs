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
        public int VirtualWidth { get; }
        public int VirtualHeight { get; }
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }

        public Rgb565Framebuffer(string devicePath, int visibleWidth, int visibleHeight, int virtualWidth, int virtualHeight)
        {
            DevicePath = devicePath;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            
            _deviceStream = File.Open(DevicePath, FileMode.Open);
        }

        public Image<Rgb24> Read(Rectangle area)
        {
            return Image.Load<Rgb24>(_deviceStream, new Rgb565FramebufferDecoder(this, area));
        }

        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>
        {
            image.Save(_deviceStream, new Rgb565FramebufferEncoder(this, srcArea, destPoint));
        }

        internal int PointToOffset(int x, int y)
        {
            return (VirtualWidth * y + x) * sizeof(short);
        }

        internal Point OffsetToPoint(long offset)
        {
            offset /= sizeof(short);
            var x = (int)(offset % VirtualWidth);
            var y = (int)(offset / VirtualWidth);

            return new Point(x, y);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _deviceStream?.Dispose();
        }
    }
}