using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Graphics
{
    public class Rgb565FramebufferEncoder : IImageEncoder
    {
        private readonly Rgb565Framebuffer _framebuffer;
        private readonly Rectangle _srcArea;
        private readonly Point _destPoint;

        public Rgb565FramebufferEncoder(Rgb565Framebuffer framebuffer, Rectangle srcArea, Point destPoint)
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
    }
}