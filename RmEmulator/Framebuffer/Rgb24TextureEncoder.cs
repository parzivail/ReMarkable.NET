using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace RmEmulator.Framebuffer
{
    internal class Rgb24TextureEncoder : IImageEncoder
    {
        private readonly int _width;
        private readonly int _height;

        public Rgb24TextureEncoder(int width, int height)
        {
            _width = width;
            _height = height;
        }

        /// <inheritdoc />
        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            var rgba32 = new Rgba32();

            for (var y = 0; y < _height; y++)
            {
                var span = image.GetPixelRowSpan(y);

                for (var x = 0; x < _width; x++)
                {
                    span[x].ToRgba32(ref rgba32);
                    stream.WriteByte(rgba32.R);
                    stream.WriteByte(rgba32.G);
                    stream.WriteByte(rgba32.B);
                }
            }
        }
    }
}