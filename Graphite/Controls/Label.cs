using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class Label : Control
    {
        public override void Draw(Image<Rgb24> buffer)
        {
            DrawString(buffer, Text, Bounds, TextAlign);
        }

        protected override RectangleF GetMinimumRedrawRect()
        {
            return MeasureString(Text, Bounds, TextAlign);
        }
    }
}
