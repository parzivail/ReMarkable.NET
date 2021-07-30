using Graphite.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace Graphite.Controls
{
    public class PictureBox : Control
    {
        private Image<Rgb24> image;

        public PictureBox()
        {

        }

        public Image<Rgb24> Source
        {
            get => image;
            set {
                image = value;
                RedrawWithChange(() => image = value);
            }
        }

        protected override RectangleF GetMinimumRedrawRect()
        {
            var imageRect = new RectangleF(0, 0, image.Size().Width, image.Size().Height);
            imageRect.Align(Bounds, TextAlign);
            return imageRect;
        }

        public override void Draw(Image<Rgb24> buffer)
        {
            var rect = GetMinimumRedrawRect();
            buffer.Mutate(g => g.DrawImage(image, rect.Location.ToInteger(), 1));
        }
    }
}
