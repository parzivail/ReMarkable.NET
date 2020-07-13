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
    /// Provides methods for decoding a RGB565 framebuffer stream to an <see cref="Image"/>
    /// </summary>
    public class Rgb565FramebufferDecoder : IImageDecoder
    {
        /// <summary>
        /// The hardware framebuffer to read data from
        /// </summary>
        private readonly HardwareFramebuffer _framebuffer;

        /// <summary>
        /// The rectangular area to read from the framebuffer
        /// </summary>
        private readonly Rectangle _area;

        /// <summary>
        /// Creates a new <see cref="Rgb565FramebufferDecoder"/>
        /// </summary>
        /// <param name="framebuffer">The hardware framebuffer to read data from</param>
        /// <param name="area">The rectangular area to read from the framebuffer</param>
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

        /// <inheritdoc />
        public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<Image> DecodeAsync(Configuration configuration, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Decodes the given RGB565 stream into the provided image
        /// </summary>
        /// <typeparam name="TPixel">The pixel format</typeparam>
        /// <param name="stream">The RGB565 stream to read <see cref="ushort"/>-encoded image data from</param>
        /// <param name="image">The image to read data into</param>
        /// <returns>The image passed as the <paramref name="image"/> argument</returns>
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

                for (var x = 0; x < _area.Width; x++) span[x].FromRgb24(Rgb565.Unpack(rgb565Buf[x]));
            }

            return image;
        }
    }
}