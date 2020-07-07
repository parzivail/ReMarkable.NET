using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Graphite.Controls
{
    public class Label : Control
    {
        public override void Draw(Image<Rgb24> buffer)
        {
            DrawString(buffer, Text, Bounds);
        }

        protected override RectangleF GetMinimumRedrawRect()
        {
            return MeasureString(Text, Bounds);
        }
    }
}
