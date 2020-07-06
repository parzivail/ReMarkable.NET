using System;
using System.IO;
using System.Threading.Tasks;
using ReMarkable.NET.Unix.Driver.Display.Framebuffer;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace ReMarkable.NET.Graphics
{
    public class Rgb565FramebufferDecoder : IImageDecoder
    {
        private readonly HardwareFramebuffer _framebuffer;
        private readonly Rectangle _area;

        public Rgb565FramebufferDecoder(HardwareFramebuffer framebuffer, Rectangle area)
        {
            _framebuffer = framebuffer;
            _area = area;
        }

        /// <inheritdoc />
        public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            var image = new Image<TPixel>(configuration, _area.Width, _area.Height);
            return DecodeIntoImage(stream, image);
        }

        /// <inheritdoc />
        public Image Decode(Configuration configuration, Stream stream)
        {
            var image = new Image<Rgb24>(configuration, _area.Width, _area.Height);
            return DecodeIntoImage(stream, image);
        }

        public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            throw new NotImplementedException();
        }

        public Task<Image> DecodeAsync(Configuration configuration, Stream stream)
        {
            throw new NotImplementedException();
        }

        private Image<TPixel> DecodeIntoImage<TPixel>(Stream stream, Image<TPixel> image) where TPixel : unmanaged, IPixel<TPixel>
        {
            var buf = new byte[_area.Width * sizeof(short)];
            var rgb565Buf = new ushort[_area.Width];

            for (var y = 0; y < _area.Height; y++)
            {
                stream.Seek(_framebuffer.PointToOffset(_area.X, _area.Y + y), SeekOrigin.Begin);
                stream.Read(buf, 0, buf.Length);
                Buffer.BlockCopy(buf, 0, rgb565Buf, 0, buf.Length);

                var span = image.GetPixelRowSpan(y);

                for (var x = 0; x < _area.Width; x++)
                {
                    var (r, g, b) = Rgb565.Unpack(rgb565Buf[x]);
                    span[x].FromRgb24(new Rgb24(r, g, b));
                }
            }

            return image;
        }
    }
}