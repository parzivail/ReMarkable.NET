using System;
using System.IO;
using ReMarkable.NET.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ReMarkable.NET.Unix.Driver.Display.Framebuffer
{
    /// <summary>
    ///     Provides methods for interacting with the device's framebuffer
    /// </summary>
    public sealed class HardwareFramebuffer : IDisposable, IFramebuffer
    {
        /// <summary>
        ///     The framebuffer stream
        /// </summary>
        private readonly FileStream _deviceStream;

        /// <summary>
        ///     The visible bounds of the buffer
        /// </summary>
        private readonly Rectangle _visibleBounds;

        /// <summary>
        ///     The buffer file location
        /// </summary>
        public string DevicePath { get; }

        /// <inheritdoc />
        public int VirtualHeight { get; }

        /// <inheritdoc />
        public int VirtualWidth { get; }

        /// <inheritdoc />
        public int VisibleHeight { get; }

        /// <inheritdoc />
        public int VisibleWidth { get; }

        public HardwareFramebuffer(string devicePath, int visibleWidth, int visibleHeight, int virtualWidth,
            int virtualHeight)
        {
            DevicePath = devicePath;
            VisibleWidth = visibleWidth;
            VisibleHeight = visibleHeight;
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            _visibleBounds = new Rectangle(0, 0, VisibleWidth, VisibleHeight);

            _deviceStream = File.Open(DevicePath, FileMode.Open);
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle srcArea, ref Point destPoint)
        {
            var tempRect = new Rectangle(destPoint, srcArea.Size);
            tempRect.Intersect(_visibleBounds);
            srcArea.Size = tempRect.Size;
            destPoint = tempRect.Location;
        }

        /// <inheritdoc />
        public void ConstrainRectangle(ref Rectangle area)
        {
            area.Intersect(_visibleBounds);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _deviceStream?.Dispose();
        }

        /// <summary>
        ///     Finds the pixel coordinates corresponding to the stream location
        /// </summary>
        /// <param name="offset">The stream location</param>
        /// <returns>The pixel coordinates</returns>
        public Point OffsetToPoint(long offset)
        {
            offset /= sizeof(short);
            var x = (int)(offset % VirtualWidth);
            var y = (int)(offset / VirtualWidth);

            return new Point(x, y);
        }

        /// <summary>
        ///     Finds the stream location corresponding to the pixel coordinates
        /// </summary>
        /// <param name="x">The pixel X coordinate</param>
        /// <param name="y">The pixel Y coordinate</param>
        /// <returns>The stream location</returns>
        public int PointToOffset(int x, int y)
        {
            return (VirtualWidth * y + x) * sizeof(short);
        }

        /// <inheritdoc />
        public Image<Rgb24> Read(Rectangle area)
        {
            ConstrainRectangle(ref area);
            return Image.Load<Rgb24>(_deviceStream, new Rgb565FramebufferDecoder(this, area));
        }

        /// <inheritdoc />
        public void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint)
            where TPixel : unmanaged, IPixel<TPixel>
        {
            var (x, y) = new Point(destPoint.X, destPoint.Y);
            ConstrainRectangle(ref srcArea, ref destPoint);
            srcArea.Location = new Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y);

            image
                .Clone(srcImage => srcImage.Crop(srcArea))
                .Save(_deviceStream, new Rgb565FramebufferEncoder(this, srcArea, destPoint));
        }

        public void SetPixel(int x, int y, Color color)
        {
            _deviceStream.Seek(PointToOffset(x, y), SeekOrigin.Begin);
            var rgb = color.ToPixel<Rgb24>();
            _deviceStream.Write(BitConverter.GetBytes(Rgb565.Pack(rgb.R, rgb.G, rgb.B)), 0, 2);
        }
    }
}