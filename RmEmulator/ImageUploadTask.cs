using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace RmEmulator
{
    internal class ImageUploadTask
    {
        public Image<Rgb24> Image { get; }
        public Point DestPoint { get; }

        public ImageUploadTask(Image<Rgb24> image, Point destPoint)
        {
            Image = image;
            DestPoint = destPoint;
        }
    }
}