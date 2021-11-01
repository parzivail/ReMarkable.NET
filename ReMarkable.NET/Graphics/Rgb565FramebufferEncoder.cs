using System;
using System.IO;
using System.Threading.Tasks;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Graphics
{
    /// <summary>
    ///     Provides methods for encoding an <see cref="Image" /> to a RGB565 framebuffer stream
    /// </summary>
    public class Rgb565FramebufferEncoder : IImageEncoder
    {
        /// <summary>
        ///     The location to place the top-leftmost corner of the source area on the destination framebuffer
        /// </summary>
        private readonly Point _destPoint;

        /// <summary>
        ///     The hardware framebuffer to write data to
        /// </summary>
        private readonly HardwareFramebuffer _framebuffer;

        /// <summary>
        ///     The area of the source image to encode
        /// </summary>
        private readonly Rectangle _srcArea;

        /// <summary>
        ///     Creates a new <see cref="Rgb565FramebufferEncoder" />
        /// </summary>
        /// <param name="framebuffer">The hardware framebuffer to write data to</param>
        /// <param name="srcArea">The area of the source image to encode</param>
        /// <param name="destPoint">The location to place the top-leftmost corner of the source area on the destination framebuffer</param>
        public Rgb565FramebufferEncoder(HardwareFramebuffer framebuffer, Rectangle srcArea, Point destPoint)
        {
            _framebuffer = framebuffer;
            _srcArea = srcArea;
            _destPoint = destPoint;
        }

        /// <inheritdoc />
        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            var buf = new byte[_srcArea.Width * sizeof(short)];
            var rgb565Buf = new ushort[_srcArea.Width];
            var rgba32 = new Rgba32();

            for (var y = 0; y < _srcArea.Height; y++)
            {
                var span = image.GetPixelRowSpan(y);

                for (var x = 0; x < _srcArea.Width; x++)
                {
                    span[x].ToRgba32(ref rgba32);
                    rgb565Buf[x] = Rgb565.Pack(rgba32.R, rgba32.G, rgba32.B);
                }

                stream.Seek(_framebuffer.PointToOffset(_destPoint.X, _destPoint.Y + y), SeekOrigin.Begin);
                Buffer.BlockCopy(rgb565Buf, 0, buf, 0, buf.Length);
                stream.Write(buf, 0, buf.Length);
            }
        }

        /// <inheritdoc />
        public async Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            var buf = new byte[_srcArea.Width * sizeof(short)];
            var rgb565Buf = new ushort[_srcArea.Width];
            var rgba32 = new Rgba32();

            for (var y = 0; y < _srcArea.Height; y++)
            {
                var span = image.GetPixelRowSpan(y).ToArray();

                for (var x = 0; x < _srcArea.Width; x++)
                {
                    span[x].ToRgba32(ref rgba32);
                    rgb565Buf[x] = Rgb565.Pack(rgba32.R, rgba32.G, rgba32.B);
                }

                stream.Seek(_framebuffer.PointToOffset(_destPoint.X, _destPoint.Y + y), SeekOrigin.Begin);
                Buffer.BlockCopy(rgb565Buf, 0, buf, 0, buf.Length);
                await stream.WriteAsync(buf, 0, buf.Length);
            }
        }
    }
}