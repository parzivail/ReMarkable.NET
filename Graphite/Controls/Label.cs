using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class Label : Control
    {
        public Label(string text, Font font)
        {
            Text = text;
            Font = font;
            var r = GetMinimumRedrawRect();
            Size = new SizeF(r.Width, r.Height);
        }

        public Label()
        {

        }

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
