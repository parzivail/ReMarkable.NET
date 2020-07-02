using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Graphite.Controls
{
    public class Button : Control
    {
        /// <inheritdoc />
        public override void Draw(Image<Rgb24> buffer)
        {
            buffer.Mutate(DrawBounds);
            DrawString(buffer, Text, Bounds);
        }

        private void DrawBounds(IImageProcessingContext g)
        {
            g.Draw(ForegroundColor, 4, Bounds);
        }
    }
}
