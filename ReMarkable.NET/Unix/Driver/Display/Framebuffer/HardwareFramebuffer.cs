using System;
using System.IO;
using ReMarkable.NET.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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

        private readonly Rectangle _visualBounds;

        public HardwareFramebuffer(string devicePath, int visibleWidth, int visibleHeight, int virtualWidth, int virtualHeight)
        {
            DevicePath = devicePath;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            _visualBounds = new Rectangle(0, 0, VisibleWidth, VisibleHeight);
            
            _deviceStream = File.Open(DevicePath, FileMode.Open);
        }

        public Image<Rgb24> Read(Rectangle area)
        {
            ConstrainRectangle(ref area);
            return Image.Load<Rgb24>(_deviceStream, new Rgb565FramebufferDecoder(this, area));
        }

        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint) where TPixel : unmanaged, IPixel<TPixel>
        {
            var (x, y) = new Point(destPoint.X, destPoint.Y);
            ConstrainRectangle(ref srcArea, ref destPoint);
            var dPoint = new Point(destPoint.X - x, destPoint.Y - y);
            srcArea.Location = dPoint;

            image.Mutate(srcImage => srcImage.Crop(srcArea));

            image.Save(_deviceStream, new Rgb565FramebufferEncoder(this, srcArea, destPoint));
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle srcArea, ref Point destPoint)
        {
            var tempRect = new Rectangle(destPoint, srcArea.Size);
            tempRect.Intersect(_visualBounds);
            srcArea.Size = tempRect.Size;
            destPoint = tempRect.Location;
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle area)
        {
            area.Intersect(_visualBounds);
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