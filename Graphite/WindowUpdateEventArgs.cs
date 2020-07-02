using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Graphite
{
    public class WindowUpdateEventArgs
    {
        public Image<Rgb24> Buffer { get; }
        public Rectangle UpdatedArea { get; }

        public WindowUpdateEventArgs(Image<Rgb24> buffer, Rectangle updatedArea)
        {
            Buffer = buffer;
            UpdatedArea = updatedArea;
        }
    }
}