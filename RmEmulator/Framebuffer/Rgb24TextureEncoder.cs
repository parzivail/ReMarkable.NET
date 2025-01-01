using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace RmEmulator.Framebuffer
{
    internal class Rgb24TextureEncoder : IImageEncoder
    {
        public Rgb24TextureEncoder()
        {
        }

        /// <inheritdoc />
        public void Encode<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            var rgba32 = new Rgba32();

            for (var y = 0; y < image.Height; y++)
            {
                //var span = image.GetPixelRowSpan(y);
                var span = image.DangerousGetPixelRowMemory(y).Span;

                for (var x = 0; x < image.Width; x++)
                {
                    span[x].ToRgba32(ref rgba32);
                    stream.WriteByte(rgba32.R);
                    stream.WriteByte(rgba32.G);
                    stream.WriteByte(rgba32.B);
                }
            }
        }

        public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream) where TPixel : unmanaged, IPixel<TPixel>
        {
            throw new System.NotImplementedException();
        }

        public Task EncodeAsync<TPixel>(Image<TPixel> image, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
        {
            throw new System.NotImplementedException();
        }
    }
}