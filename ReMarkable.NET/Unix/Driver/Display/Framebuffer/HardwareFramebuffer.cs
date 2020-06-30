using System;
using System.IO;
using ReMarkable.NET.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    public sealed class HardwareFramebuffer : IDisposable, IFramebuffer
    {
        private readonly FileStream _deviceStream;

        public string DevicePath { get; }
        public int VirtualWidth { get; }
        public int VirtualHeight { get; }
        public int VisibleWidth { get; }
        public int VisibleHeight { get; }

        public HardwareFramebuffer(string devicePath, int visibleWidth, int visibleHeight, int virtualWidth, int virtualHeight)
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

        public int PointToOffset(int x, int y)
        {
            return (VirtualWidth * y + x) * sizeof(short);
        }

        public Point OffsetToPoint(long offset)
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